using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.IO.Compression;
using System.Text;

public class ApplicationScripts : IHttpHandler
{
    private readonly static TimeSpan CACHE_DURATION = TimeSpan.FromDays(1);
    private HttpContext context;
    private enum CombineType { JavaScript, CSS, NONE };
    private CombineType combineType = CombineType.NONE;

    public void ProcessRequest(HttpContext context)
    {
        this.context = context;
        HttpRequest request = context.Request;

        string screenId = request["screenid"] ?? "0";
        
        // Decide if browser supports compressed response
        bool isCompressed = this.CanGZip(context.Request);

        // If the set has already been cached, write the response directly from
        // cache. Otherwise generate the response and cache it
        if (!this.WriteFromCache(screenId, isCompressed))
        {
            using (MemoryStream memoryStream = new MemoryStream(8092))
            {
                // Decide regular stream or gzip stream based on whether the response can be compressed or not
                using (Stream writer = isCompressed ? (Stream)(new ICSharpCode.SharpZipLib.GZip.GZipOutputStream(memoryStream)) : memoryStream)
                {
                    // Read the files into one big string
                    StringBuilder allScripts = new StringBuilder();
                    foreach (string fileName in GetScriptFileNames(screenId))
                    {
                        if (File.Exists(context.Server.MapPath(fileName)))
                            allScripts.AppendLine(File.ReadAllText(context.Server.MapPath(fileName)));
                    }

                    // Send minfied string to output stream
                    byte[] bts = Encoding.UTF8.GetBytes(allScripts.ToString());
                    writer.Write(bts, 0, bts.Length);
                }

                // Cache the combined response so that it can be directly written
                // in subsequent calls 
                byte[] responseBytes = memoryStream.ToArray();
                //context.Cache.Insert(GetCacheKey(screenId, isCompressed),
                //    responseBytes, null, System.Web.Caching.Cache.NoAbsoluteExpiration,
                //    CACHE_DURATION);

                // Generate the response
                this.WriteBytes(responseBytes, isCompressed);
            }
        }
    }
    private bool WriteFromCache(string screenId,  bool isCompressed)
    {
        byte[] responseBytes = context.Cache[GetCacheKey(screenId, isCompressed)] as byte[];

        if (responseBytes == null || responseBytes.Length == 0)
            return false;

        this.WriteBytes(responseBytes, isCompressed);
        return true;
    }

    private void WriteBytes(byte[] bytes, bool isCompressed)
    {
        HttpResponse response = context.Response;

        response.AppendHeader("Content-Length", bytes.Length.ToString());
        response.ContentType = "application/x-javascript";
        
        if (isCompressed)
            response.AppendHeader("Content-Encoding", "gzip");
        else
            response.AppendHeader("Content-Encoding", "utf-8");

        context.Response.Cache.SetCacheability(HttpCacheability.Public);
        context.Response.Cache.SetExpires(DateTime.Now.Add(CACHE_DURATION));
        context.Response.Cache.SetMaxAge(CACHE_DURATION);

        response.ContentEncoding = Encoding.Unicode;
        response.OutputStream.Write(bytes, 0, bytes.Length);
        response.Flush();
    }

    private bool CanGZip(HttpRequest request)
    {
        string acceptEncoding = request.Headers["Accept-Encoding"];
        if (!string.IsNullOrEmpty(acceptEncoding) &&
             (acceptEncoding.Contains("gzip") || acceptEncoding.Contains("deflate")))
            return true;
        return false;
    }

    private string GetCacheKey(string screenId, bool isCompressed)
    {
        return "ApplicationScripts.ScreenId:" + screenId + "." + isCompressed;
    }

    public bool IsReusable
    {
        get { return true; }
    }

    // private helper method that return an array of file names inside the text file stored in App_Data folder
    private static string[] GetScriptFileNames(string screenId)
    {
        var scripts = new System.Collections.Generic.List<string>();
        scripts.Add("~/Scripts/PageScripts/Page" + screenId.ToString() + ".js");
        return scripts.ToArray();

    }
}

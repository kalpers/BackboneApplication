using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Funq;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Providers;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.WebHost.Endpoints;
using BackboneApplication.ServiceInterface;
using BackboneApplication;

namespace BackboneApplication.AppHost
{
    public class AppHost : AppHostBase
    {
        public AppHost() : base("BackboneApplication", typeof(AppHost).Assembly)
    {
    }

        public override void Configure(Funq.Container container)
        {
            // register storage for user sessions 
            container.Register<ICacheClient>(new MemoryCacheClient());

            // Register AuthFeature with custom user session and custom auth provider
            Plugins.Add(new AuthFeature(() => new CustomUserSession(),
                new[] { new CustomCredentialsAuthProvider() }
            ) { HtmlRedirect = null });   
        }
    }

    public class CustomUserSession : AuthUserSession
    {
        public string UserInfo { get; set; }
    }

    public class CustomCredentialsAuthProvider : CredentialsAuthProvider
    {
        public override bool TryAuthenticate(IServiceBase authService, string userName, string password)
        {
            if (!CheckInDB(userName, password)) return false;
            return true;
        }

        public override void OnAuthenticated(IServiceBase authService, IAuthSession session, IOAuthTokens tokens, Dictionary<string, string> authInfo)
        {
            //Fill the IAuthSession with data which you want to retrieve in the app eg:
            session.FirstName = "Kneale";
            session.LastName = "Alpers";
            
            //Important: You need to save the session!
            authService.SaveSession(session, SessionExpiry);
        }

        private bool CheckInDB(string userName, string password)
        {
            if (userName != "admin" && userName != "user") return false;
            return password == "123";
        }
    }
}
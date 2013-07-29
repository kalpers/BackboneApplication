using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace BackboneApplication.ServiceInterface
{
    [Route("/ApplicationScreens/{ScreenId}")]
    public class ApplicationScreens
    {
        public Int32 ScreenId { get; set; }
    }

    public class ApplicationScreensResponse
    {
        public HtmlString HtmlOutput { get; set; }
    }

    public class ApplicationScreensService : Service
    {

        public object Any(ApplicationScreens screen)
        {
            return new Screens().getScreen(screen.ScreenId);
        }

        public class Screens : System.Web.UI.Page
        {
            private const string HtmlBodyTabTemplate = @"
            {{DisplayAs}}
        ";
            private const string HtmlBodyTabsTemplate = @"
            <div class=""span12"" style=""margin-left: 10px;"">
                    <div id=""maintabsectiontabgroup"" class=""tab-group""></div>
                </div>            
        ";

            private const string HTMLBodyTitleBarTemplate = @"
            <div>{{OrgName}}</div><div>{{UserName}}</div>     
        ";

            private const string HTMLBodyLeftNavItemTemplate = @"
            <a>{{DisplayName}}</a>
        ";

            private const string HTMLBodyPageContent = @"
            <div>hello world from ScreenId {0}</div>            
        ";

            private const string ListPage2 = @"
                                <td width=""120px"">{{ClientId}}</td>
                                <td width=""300px"">{{LastName}}, {{FirstName}}</td>
                                <td width=""200px"">{{HomePhone}}</td>
                                <td width=""100px"">{{AxisV}}</td>
                                <td width=""100px"">{{LastDOS}}</td>
                                <td width=""127px"">{{LastSeen}}</td>
                                <td width=""73px"">{{Primary}}</td>
                ";

            public string processScreen(string ScreenURL)
            {
                var sb = new StringBuilder();
                var sw = new StringWriter(sb);
                var htw = new HtmlTextWriter(sw);
                var uc = (CustomUserControl)this.LoadControl(ScreenURL);
                uc.PageLoad();
                uc.RenderControl(htw);
                return sb.ToString();
            }

            public string getScreen(Int32 screenId)
            {
                switch (screenId)
                {
                    case 0: // page content
                        return HTMLBodyPageContent.Fmt(screenId.ToString());
                        break;
                    case 1: // Login Screen
                        return processScreen(@"/ActivityPages/Login.ascx");
                        break;
                    case 2: // Tab 
                        return HtmlBodyTabTemplate;
                        break;
                    case 3: // Tabs body
                        return HtmlBodyTabsTemplate;
                        break;
                    case 4: // title bar
                        return HTMLBodyTitleBarTemplate;
                        break;
                    case 5: // leftnav item
                        return HTMLBodyLeftNavItemTemplate;
                        break;
                    case 6: // page content
                        return HTMLBodyPageContent.Fmt(screenId.ToString());
                        break;
                    case 7:
                        return processScreen(@"/ActivityPages/Office/ListPages/MyCaseLoad.ascx");
                        break;
                    case 8:
                        return ListPage2;
                        break;
                    case 9:
                        return @"<a class=""small button"" pageid=""{{PageId}}"">{{DisplayAs}}</a>";
                        break;
                    default:
                        return "";
                }
            }
        }
    }
}
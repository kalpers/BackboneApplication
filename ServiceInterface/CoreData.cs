using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BackboneApplication.ServiceInterface
{
    [Route("/CoreData/{CoreDataType}/{CoreDataId}")]
    public class CoreData
    {
        public string CoreDataType { get; set; }
        public Int32 CoreDataId { get; set; }
    }

    public class CoreDataResponse
    {
        public List<object> CoreDataList { get; set; }
    }

    [Authenticate]
    public class CoreDataService : Service
    {
        public Object Any(CoreData cd)
        {
            List<object> filterlist = new List<object>();
            int pageNumber = 0;
            string toSortby = "";
            string filters = base.Request.QueryString["filters"];
            if (filters != null)
            {
                var filterarray = filters.Split('^');
                foreach (var s in filterarray)
                {
                    var x = s.Split('=');
                    if (x[0] == "ListPageNumber")
                    {
                        pageNumber = int.Parse(x[1]);
                    }
                    else if (x[0] == "ListPageSort")
                    {
                        toSortby = x[1].ToString();
                    }
                    filterlist.Add(new
                        {
                            id = x[0],
                            value = x[1]
                        });
                }
            }
            switch (cd.CoreDataType.ToLower())
            {
                case "leftnav":
                    List<object> leftnavitems = null;
                    switch (cd.CoreDataId)
                    {
                        case 1:
                            leftnavitems = new List<object>
                        {
                            new LeftNavItem
                                {
                                    DisplayName = "Dashboard",
                                    ScreenId = 6,
                                    TabId = 1,
                                    HasSubMenu = false,
                                    IsSubMenu = false
                                },
                                new LeftNavItem
                                {
                                    DisplayName = "Reception",
                                    ScreenId = 6,
                                    TabId = 1,
                                    HasSubMenu = false,
                                    IsSubMenu = false
                                },
                                new LeftNavItem
                                {
                                    DisplayName = "My Caseload",
                                    ScreenId = 7,
                                    TabId = 1,
                                    HasSubMenu = false,
                                    IsSubMenu = false
                                },
                                new LeftNavItem
                                {
                                    DisplayName = "Services",
                                    ScreenId = 6,
                                    TabId = 1,
                                    HasSubMenu = false,
                                    IsSubMenu = false
                                },
                                new LeftNavItem
                                {
                                    DisplayName = "My Documents",
                                    ScreenId = 6,
                                    TabId = 1,
                                    HasSubMenu = false,
                                    IsSubMenu = false
                                },
                                new LeftNavItem
                                {
                                    DisplayName = "My Servies",
                                    ScreenId = 6,
                                    TabId = 1,
                                    HasSubMenu = false,
                                    IsSubMenu = false
                                },
                                new LeftNavItem
                                {
                                    DisplayName = "My Calendar",
                                    ScreenId = 6,
                                    TabId = 1,
                                    HasSubMenu = false,
                                    IsSubMenu = false
                                },
                                new LeftNavItem
                                {
                                    DisplayName = "Messages",
                                    ScreenId = 6,
                                    TabId = 1,
                                    HasSubMenu = false,
                                    IsSubMenu = false
                                },
                                new LeftNavItem
                                {
                                    DisplayName = "Clients Tab",
                                    ScreenId = 6,
                                    TabId = 2,
                                    HasSubMenu = false,
                                    IsSubMenu = false
                                }

                        };
                            break;
                        case 2:
                            leftnavitems = new List<object>
                        {
                            new LeftNavItem
                                {
                                    DisplayName = "Client Summary",
                                    ScreenId = 6,
                                    TabId = 2,
                                    HasSubMenu = false,
                                    IsSubMenu = false
                                },
                                new LeftNavItem
                                {
                                    DisplayName = "Client Information",
                                    ScreenId = 6,
                                    TabId = 2,
                                    HasSubMenu = false,
                                    IsSubMenu = false
                                },
                                new LeftNavItem
                                {
                                    DisplayName = "PM Client Information",
                                    ScreenId = 6,
                                    TabId = 2,
                                    HasSubMenu = false,
                                    IsSubMenu = false
                                },
                                new LeftNavItem
                                {
                                    DisplayName = "Services",
                                    ScreenId = 6,
                                    TabId = 2,
                                    HasSubMenu = false,
                                    IsSubMenu = false
                                },
                                new LeftNavItem
                                {
                                    DisplayName = "Client Notes",
                                    ScreenId = 6,
                                    TabId = 2,
                                    HasSubMenu = false,
                                    IsSubMenu = false
                                },
                                new LeftNavItem
                                {
                                    DisplayName = "Diagnosis",
                                    ScreenId = 6,
                                    TabId = 2,
                                    HasSubMenu = false,
                                    IsSubMenu = false
                                },
                                new LeftNavItem
                                {
                                    DisplayName = "Treatment Plan",
                                    ScreenId = 6,
                                    TabId = 2,
                                    HasSubMenu = false,
                                    IsSubMenu = false
                                },
                                new LeftNavItem
                                {
                                    DisplayName = "Medications",
                                    ScreenId = 6,
                                    TabId = 2,
                                    HasSubMenu = false,
                                    IsSubMenu = false
                                }
                        };
                            break;
                        case 3:
                            leftnavitems = new List<object>
                        {
                            new LeftNavItem
                                {
                                    DisplayName = "Program Assignments",
                                    ScreenId = 6,
                                    TabId = 3,
                                    HasSubMenu = false,
                                    IsSubMenu = false
                                }
                        };
                            break;
                        case 4:
                            leftnavitems = new List<object>
                        {
                            new LeftNavItem
                                {
                                    DisplayName = "Merge Clients",
                                    ScreenId = 6,
                                    TabId = 4,
                                    HasSubMenu = false,
                                    IsSubMenu = false
                                },
                                new LeftNavItem
                                {
                                    DisplayName = "Procedure/Rates",
                                    ScreenId = 6,
                                    TabId = 4,
                                    HasSubMenu = false,
                                    IsSubMenu = false
                                },
                                new LeftNavItem
                                {
                                    DisplayName = "Staff/Users",
                                    ScreenId = 6,
                                    TabId = 4,
                                    HasSubMenu = false,
                                    IsSubMenu = false
                                },
                                new LeftNavItem
                                {
                                    DisplayName = "System Configuration",
                                    ScreenId = 6,
                                    TabId = 4,
                                    HasSubMenu = false,
                                    IsSubMenu = false
                                },
                                new LeftNavItem
                                {
                                    DisplayName = "Batch Eligibility",
                                    ScreenId = 6,
                                    TabId = 4,
                                    HasSubMenu = false,
                                    IsSubMenu = false
                                },
                                new LeftNavItem
                                {
                                    DisplayName = "Plans",
                                    ScreenId = 6,
                                    TabId = 4,
                                    HasSubMenu = false,
                                    IsSubMenu = false
                                },
                                new LeftNavItem
                                {
                                    DisplayName = "Reports",
                                    ScreenId = 6,
                                    TabId = 4,
                                    HasSubMenu = false,
                                    IsSubMenu = false
                                },
                                new LeftNavItem
                                {
                                    DisplayName = "Locations",
                                    ScreenId = 6,
                                    TabId = 4,
                                    HasSubMenu = false,
                                    IsSubMenu = false
                                }
                        };
                            break;
                    }
                    return new CoreDataResponse { CoreDataList = leftnavitems };
                    break;
                case "maintabbar":
                    List<object> tabItems = new List<object>
                        {
                            new TabItem
                                {
                                    DisplayAs = "My Office",
                                    TabId = 1,
                                    IsActive = cd.CoreDataId == 1 ? true : false,
                                    IsVisible = true,
                                    Sequence = 1
                                },
                            new TabItem
                                {
                                    DisplayAs = "Client",
                                    TabId = 2,
                                    IsActive = cd.CoreDataId == 2 ? true : false,
                                    IsVisible = cd.CoreDataId == 2 ? true : false,
                                    Sequence = 2
                                },
                            new TabItem
                                {
                                    DisplayAs = "Program",
                                    TabId = 3,
                                    IsActive = cd.CoreDataId == 3 ? true : false,
                                    IsVisible = true,
                                    Sequence = 3
                                },
                            new TabItem
                                {
                                    DisplayAs = "Administrator",
                                    TabId = 4,
                                    IsActive = cd.CoreDataId == 4 ? true : false,
                                    IsVisible = true,
                                    Sequence = 4
                                }
                        };
                    return new CoreDataResponse { CoreDataList = tabItems };
                    break;
                case "userinfo":
                    return new CoreDataResponse
                        {
                            CoreDataList = new List<object> {new UserInfo {UserName = "Kneale Alpers"}}
                        };
                    break;
                default:
                    return new CoreDataResponse();
            }
        }
    }

    public class TabItem
    {
        public int TabId { get; set; }
        public String DisplayAs { get; set; }
        public bool IsActive { get; set; }
        public bool IsVisible { get; set; }
        public int Sequence { get; set; }
    }

    public class LeftNavItem
    {
        public string DisplayName { get; set; }
        public Int32 ScreenId { get; set; }
        public int TabId { get; set; }
        public bool HasSubMenu { get; set; }
        public bool IsSubMenu { get; set; }
    }

    public class UserInfo
    {
        public string UserName { get; set; }
    }
}

using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using BackboneApplicationBaseLayer;

namespace BackboneApplication.ServiceInterface
{
    [Route("/CoreDataList/{ScreenId}")]
    public class CoreDataList
    {
        public Int32 ScreenId { get; set; }
    }

    public class CoreDataListResponse
    {
        public List<object> DataList { get; set; }
        public Int32 TotalRows { get; set; }
        public List<PagerItem> PagerData { get; set; }
    }

    public class CoreDataListService : Service
    {
        public Object Any(CoreDataList cdl)
        {
            Filters filters = new Filters(Request.QueryString["filters"]);
            var pageNumber = 0;
            var RowsPerPage = 20;
            int.TryParse(filters.GetFilterValue("ListPageNumber"), out pageNumber);
            var pageSort = filters.GetFilterValue("ListPageSort");
            List<object> items = new List<object>();
            CoreDataListResponse cdlr = new CoreDataListResponse();
            switch (cdl.ScreenId)
            {
                case 8:
                    if (Session["Screen8Items"] != null)
                    {
                        items = (List<object>) Session["Screen8Items"];
                    }
                    else
                    {
                        string[] firstNames =
                        {
                                "Jacob", "Sophia", "Mason", "Emma", "Ethan", "Isabella", "Noah", "Olivia",
                                "William", "Ava", "Liam", "Emily", "Jayden", "Abigail", "Michael", "Mia", "Alexander",
                                "Madison", "Aiden", "Elizabeth"
                       };
                        string[] lastNames =
                            {
                                "Johnathan", "Smith", "Taylor", "Abbey", "Brown", "Candle", "Doughtfire", "France",
                                "French", "Frank", "Gattos", "Himlock", "Jasper",
                                "Kilroy", "Masters", "Nanto", "Prague", "Randnap", "Sero", "Zabra"
                            };

                        Random random = new Random();
                        int randomNumber;
                        int randomNumber2;
                        DateTime today = DateTime.Now;
                        for (int loop = 1; loop <= 200; loop++)
                        {
                            randomNumber = random.Next(1, 21);
                            randomNumber2 = random.Next(1, 21);
                            items.Add(new ListPage8
                            {
                                ClientId = randomNumber * randomNumber2,
                                LastName = lastNames[randomNumber-1],
                                FirstName = firstNames[randomNumber2-1],
                                HomePhone = "269-" + (randomNumber * 123).ToString().Substring(0, 3) + "-" + (randomNumber2 * 1055).ToString().Substring(0, 4),
                                AxisV = Convert.ToString((randomNumber > randomNumber2 ? randomNumber / randomNumber2 : randomNumber2 / randomNumber)*10),
                                LastDOS = today.AddMonths(- randomNumber).AddDays(- randomNumber2).ToShortDateString(),
                                LastSeen = randomNumber > randomNumber2 ? today.AddMonths(- randomNumber).AddDays(- randomNumber2).ToShortDateString() : "",
                                Primary = "Yes"
                            });
                        }
                           
                        Session["Screen8Items"] = items;
                    }
                    cdlr.DataList = items.Skip(RowsPerPage * (pageNumber - 1)).Take(RowsPerPage).ToList();
                    cdlr.TotalRows = items.Count();
                    cdlr.PagerData = Pager.GeneratePager(pageNumber, (items.Count() / RowsPerPage) + ((items.Count() % RowsPerPage) > 0 ? 1 : 0), 7);
                    break;
                    
            }
            return cdlr;
        }
    }

    public class ListPage8
    {
        public Int32 ClientId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string HomePhone { get; set; }
        public string AxisV { get; set; }
        public string LastDOS { get; set; }
        public string LastSeen { get; set; }
        public string Primary { get; set; }
    }
}
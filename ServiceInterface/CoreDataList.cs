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

    public class FilterItem
    {
        public string ID { get; set; }
        public string Value { get; set; }
    }

    public class PagerItem
    {
        public string PageId { get; set; }
        public string DisplayAs { get; set; }
        public bool IsCurrent { get; set; }
    }

    public class CoreDataListService : Service
    {
        private List<FilterItem> filterList = null;

        private string GetFilterValue(string filterName)
        {
            if (filterList == null) return "";
            var item = filterList.Where(x => x.ID.ToLower().Equals(filterName.ToLower()))
                          .Select(x => x.Value)
                          .Take(1)
                          .ToList();
            if (item.Count > 0) return item[0].ToString();
            else return "";
        }

        private void SetFilter(string queryFilter)
        {
            filterList = new List<FilterItem>();
            if (!string.IsNullOrWhiteSpace(queryFilter))
            {
                foreach (var s in queryFilter.Split('^'))
                {
                    var filterItem = s.Split('=');
                    filterList.Add(new FilterItem
                    {
                        ID = filterItem.Length > 0 ? filterItem[0] : "",
                        Value = filterItem.Length > 1 ? filterItem[1] : ""
                    });
                }
            }
        }

        private List<PagerItem> GeneratePager(int currentPage, int totalPages, int clickCount)
        {
            List<PagerItem> items = new List<PagerItem>();
            bool FirstPageTag = false;
            bool PrevPageTag = false;
            bool NextPageTag = false;
            bool LastPageTag = false;
            int startNumber = 0;
            int startOffset = 0;
            int endNumber = 0;
            int endOffset = 0;
            int midPageNumber = (int)(clickCount * .5 + .5);
            if ((totalPages > clickCount && totalPages >= (currentPage + midPageNumber)) || (totalPages >= (currentPage + midPageNumber)))
            {
                NextPageTag = true;
                LastPageTag = true;
                endOffset = 3;
            }
            if (currentPage > clickCount)
            {
                FirstPageTag = true;
                startOffset = 2;
                if (currentPage > midPageNumber)
                {
                    PrevPageTag = true;
                    startOffset = 3;
                }
            }
            else if ((currentPage - midPageNumber) > 0)
            {
                FirstPageTag = true;
                PrevPageTag = true;
                startOffset = 3;
            }
            if (FirstPageTag)
            {
                items.Add(new PagerItem { DisplayAs = "<<", PageId = "1" });
            }
            if (PrevPageTag)
            {
                items.Add(new PagerItem { DisplayAs = "<", PageId = (currentPage - midPageNumber + (startOffset - 1)).ToString() });
            }
            if ((currentPage + midPageNumber) > totalPages)
            {
                startNumber = totalPages - midPageNumber;
            }
            else
            {
                startNumber = currentPage - midPageNumber + startOffset;
            }
            if (startNumber < 1) startNumber = 1;

            endNumber = currentPage + midPageNumber - endOffset;
            if (endNumber > totalPages)
            {
                endNumber = totalPages;
            }
            else
            {
                if (endNumber <= (clickCount - endOffset)) endNumber = clickCount - endOffset + 1;
            }

            for (var index = startNumber; index <= endNumber; index++)
            {
                items.Add(new PagerItem
                {
                    DisplayAs = index.ToString(),
                    PageId = index.ToString(),
                    IsCurrent = (index == currentPage)
                });
            }
            if (NextPageTag)
            {
                items.Add(new PagerItem { DisplayAs = ">", PageId = (endNumber + 1).ToString() });
            }
            if (LastPageTag)
            {
                items.Add(new PagerItem { DisplayAs = ">>", PageId = (totalPages).ToString() });
            }

            return items;
        }

        public Object Any(CoreDataList cdl)
        {
            SetFilter(Request.QueryString["filters"]);
            var pageNumber = 0;
            var RowsPerPage = 20;
            int.TryParse(GetFilterValue("ListPageNumber"), out pageNumber);
            var pageSort = GetFilterValue("ListPageSort");
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
                    cdlr.PagerData = GeneratePager(pageNumber, (items.Count() / RowsPerPage) + ((items.Count() % RowsPerPage) > 0 ? 1 : 0), 7);
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
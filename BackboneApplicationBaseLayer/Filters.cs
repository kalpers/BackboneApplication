using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackboneApplicationBaseLayer
{
    public class Filters
    {
        private List<FilterItem> filterList = null;

        public Filters(string queryFilter)
        {
            SetFilter(queryFilter);
        }

        public string GetFilterValue(string filterName)
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
    }

    public class FilterItem
    {
        public string ID { get; set; }
        public string Value { get; set; }
    }
}

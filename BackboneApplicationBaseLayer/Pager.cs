using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackboneApplicationBaseLayer
{
    public static class Pager
    {
        public static List<PagerItem> GeneratePager(int currentPage, int totalPages, int clickCount)
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
    }

    public class PagerItem
    {
        public string PageId { get; set; }
        public string DisplayAs { get; set; }
        public bool IsCurrent { get; set; }
    }
}

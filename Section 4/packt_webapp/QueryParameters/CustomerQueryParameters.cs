using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace packt_webapp.QueryParameters
{
    public class CustomerQueryParameters
    {
        private const int maxPageCount = 100;
        public int Page { get; set; } = 1;

        private int _pageCount = 100;
        public int PageCount
        {
            get { return _pageCount; }
            set { _pageCount = (value > maxPageCount) ? maxPageCount : value; }
        }

        public bool HasQuery { get { return !String.IsNullOrEmpty(Query); } }
        public string Query { get; set; }

        public string OrderBy { get; set; } = "Firstname";
        public bool Descending
        {
            get
            {
                if (!String.IsNullOrEmpty(OrderBy))
                {
                    return OrderBy.Split(' ').Last().ToLowerInvariant().StartsWith("desc");
                }
                return false;
            }
        }

    }
}

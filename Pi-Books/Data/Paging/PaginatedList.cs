using System;
using System.Collections.Generic;
using System.Linq;

namespace Pi_Books.Data.Paging
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(List<T> items, int itemCount, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(itemCount / (double)pageSize);

            this.AddRange(items);
        }

        public bool HasPreviousPage { get { return PageIndex > 1; } }
        public bool HasNextPage { get { return PageIndex < TotalPages; } }

        public static PaginatedList<T> Create(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var itemsCount = source.Count();
            var items = source.Skip((pageIndex-1)*pageSize).Take(pageSize).ToList();

            return new PaginatedList<T>(items, itemsCount, pageIndex, pageSize);
        }
    }
}
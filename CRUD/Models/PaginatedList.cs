using System;
using System.Collections.Generic;

namespace CRUD.Models
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; }
        public int TotalRecords { get; }
        public int PageIndex { get; }
        public int TotalPages { get; }
        public int PageSize { get; }

        public PaginatedList(List<T> items, int totalRecords, int pageIndex, int pageSize)
        {
            Items = items;
            TotalRecords = totalRecords;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }
}

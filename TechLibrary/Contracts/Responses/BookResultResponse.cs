using System.Collections.Generic;

namespace TechLibrary.Models
{
    public class BookResultResponse
    {
        public int TotalRecords { get; set; }
        public List<BookResponse> Items { get; set; }
    }
}

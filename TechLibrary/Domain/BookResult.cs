using System.Collections.Generic;

namespace TechLibrary.Domain
{
    public class BookResult
    {
        public int TotalRecords { get; set; }
        public List<Book> Items { get; set; }
    }
}

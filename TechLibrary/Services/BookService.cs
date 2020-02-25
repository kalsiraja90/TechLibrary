using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechLibrary.Data;
using TechLibrary.Domain;
using TechLibrary.Models;

namespace TechLibrary.Services
{
    public interface IBookService
    {
        Task<BookResult> GetBooksAsync(string searchText, int pagenumber, int batchSize);
        Task<Book> GetBookByIdAsync(int bookId);
        Task<Book> AddOrUpdateBookAsync(Book bookRequest);
    }

    public class BookService : IBookService
    {
        private readonly DataContext _dataContext;

        public BookService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        public async Task<BookResult> GetBooksAsync(string searchText, int pagenumber, int batchSize)
        {
            var queryable = _dataContext.Books.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                queryable = queryable.Where(row => EF.Functions.Like(row.Title, $"%{searchText}%") || EF.Functions.Like(row.ShortDescr, $"%{searchText}%"));
            }
            var start = (pagenumber - 1) * batchSize;
            var count = await queryable.CountAsync();
            var result = await queryable.Skip(start).Take(batchSize).ToListAsync();
            return new BookResult
            {
                Items = result,
                TotalRecords = count
            };
        }

        public async Task<Book> GetBookByIdAsync(int bookid)
        {
            return await _dataContext.Books.SingleOrDefaultAsync(x => x.BookId == bookid);
        }

        public async Task<Book> AddOrUpdateBookAsync(Book bookRequest)
        {
            if (bookRequest.BookId > 0)
            {
                var book = await _dataContext.Books.FirstOrDefaultAsync(x => x.BookId == bookRequest.BookId);
                if (book != null)
                {
                    book.Title = bookRequest.Title;
                    book.ISBN = bookRequest.ISBN;
                    book.PublishedDate = bookRequest.PublishedDate;
                    book.ThumbnailUrl = bookRequest.ThumbnailUrl;
                    book.ShortDescr = bookRequest.ShortDescr;
                }
            }
            else
            {
                _dataContext.Books.Add(bookRequest);
            }
            await _dataContext.SaveChangesAsync();
            
            return bookRequest;
        }
    }
}

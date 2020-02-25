using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TechLibrary.Domain;
using TechLibrary.Models;
using TechLibrary.Services;

namespace TechLibrary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : Controller
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;

        public BooksController(ILogger<BooksController> logger, IBookService bookService, IMapper mapper)
        {
            _logger = logger;
            _bookService = bookService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string searchText, int pageNumber, int batchSize)
        {
            _logger.LogInformation("Get all books by page number and batch size");

            var books = await _bookService.GetBooksAsync(searchText, pageNumber, batchSize);

            var bookResponse = _mapper.Map<BookResultResponse>(books);

            return Ok(bookResponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation($"Get book by id {id}");

            var book = await _bookService.GetBookByIdAsync(id);

            var bookResponse = _mapper.Map<BookResponse>(book);

            return Ok(bookResponse);
        }

        [HttpPost]
        public async Task<IActionResult> SaveBook(BookRequest book)
        {
            _logger.LogInformation($"Get book by id {book.BookId}");
            var bookRequest = _mapper.Map<Book>(book);
            bookRequest = await _bookService.AddOrUpdateBookAsync(bookRequest);
            var bookResponse = _mapper.Map<BookResponse>(bookRequest);
            return Ok(bookResponse);
        }
    }
}

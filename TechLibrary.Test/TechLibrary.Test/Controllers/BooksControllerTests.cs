using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using TechLibrary.Domain;
using TechLibrary.Services;

namespace TechLibrary.Controllers.Tests
{
    [TestFixture()]
    [Category("ControllerTests")]
    public class BooksControllerTests
    {

        private  Mock<ILogger<BooksController>> _mockLogger;
        private  Mock<IBookService> _mockBookService;
        private  Mock<IMapper> _mockMapper;
        private NullReferenceException _expectedException;

        [OneTimeSetUp]
        public void TestSetup()
        {
            _expectedException = new NullReferenceException("Test Failed...");
            _mockLogger = new Mock<ILogger<BooksController>>();
            _mockBookService = new Mock<IBookService>();
            _mockMapper = new Mock<IMapper>();
        }

        [Test()]
        public async Task GetAllTest()
        {
            //  Arrange
            _mockBookService.Setup(b => b.GetBooksAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(It.IsAny<Domain.BookResult>()));
            var sut = new BooksController(_mockLogger.Object, _mockBookService.Object, _mockMapper.Object);

            //  Act
            var result = await sut.GetAll("", 1, 10);

            //  Assert
            _mockBookService.Verify(s => s.GetBooksAsync("", 1, 10), Times.Once, "Expected GetBooksAsync to have been called once");
        }



        [Test()]
        public async Task GetByIdTest()
        {
            //  Arrange
            _mockBookService.Setup(b => b.GetBookByIdAsync(It.Is<int>(x => x == 1))).Returns(Task.FromResult(new Book() { BookId = 1 }));
            var sut = new BooksController(_mockLogger.Object, _mockBookService.Object, _mockMapper.Object);

            //  Act
            var result = await sut.GetById(1);

            //  Assert
            _mockBookService.Verify(s => s.GetBookByIdAsync(1), Times.Once, "Expected GetBookByIdAsync to have been called once");
        }


        [Test()]
        public async Task SaveBookTest()
        {
            //  Arrange
            _mockBookService.Setup(b => b.AddOrUpdateBookAsync(It.IsAny<Book>())).Returns(Task.FromResult(new Book() { BookId = 1 }));
            var sut = new BooksController(_mockLogger.Object, _mockBookService.Object, _mockMapper.Object);

            //  Act
            var result = await sut.SaveBook(new Models.BookResponse());

            //  Assert
            _mockBookService.Verify(s => s.AddOrUpdateBookAsync(It.IsAny<Book>()), Times.Once, "Expected AddOrUpdateBookAsync to have been called once");
        }
    }
}
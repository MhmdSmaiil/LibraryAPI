using Library.Data.Interfaces.Repositories;
using Library.Data.Interfaces.Services;
using Library.Data.Models.Requests;
using Library.Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repo;
        public BookService(IBookRepository bookRepository) 
        {
            _repo = bookRepository;
        }

        public RequestResult GetAllBooks()
        {
            var Books = _repo.GetAllBooks();
            return new RequestResult()
            {
                Success = true,
                Result = Books
            };
        }

        public async Task<RequestResult> GetBookByIdAsync(int id)
        {
            var Book = await _repo.GetBookByIdAsync(id);
            return new RequestResult()
            {
                Success = true,
                Result = Book
            };
        }

        public async Task<RequestResult> CreateBookAsync(BookModel Book)
        {
            var BookEntity = new Entities.Book()
            {
                Title = Book.Title,
                Author = Book.Author,
                ISBN = Book.ISBN,
            };
            var result = await _repo.CreateBookAsync(BookEntity);
            return new RequestResult()
            {
                Success = true,
                Result = result
            };
        }

        public async Task<RequestResult> UpdateBookAsync(int id, BookModel Book)
        {
            var BookEntity = new Entities.Book()
            {
                Title = Book.Title,
                Author = Book.Author,
                ISBN = Book.ISBN,
            };
            var result = await _repo.UpdateBookAsync(id, BookEntity);
            return new RequestResult()
            {
                Success = true,
                Result = result
            };
        }

        public async Task<RequestResult> DeleteBookAsync(int id)
        {
            var deletedBook = await _repo.DeleteBookAsync(id);
            return new RequestResult()
            {
                Success = true,
                Result = deletedBook
            };
        }
    }
}

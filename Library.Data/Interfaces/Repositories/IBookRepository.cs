using Library.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Interfaces.Repositories
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetAllBooks();
        Task<Book> GetBookByIdAsync(int id);
        Task<Book> CreateBookAsync(Book Book);
        Task<Book> UpdateBookAsync(int id, Book Book);
        Task<Book> DeleteBookAsync(int id);
    }
}

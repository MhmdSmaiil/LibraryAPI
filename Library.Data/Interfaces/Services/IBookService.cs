using Library.Data.Models.Requests;
using Library.Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Interfaces.Services
{
    public interface IBookService
    {
        RequestResult GetAllBooks();
        Task<RequestResult> GetBookByIdAsync(int id);
        Task<RequestResult> CreateBookAsync(BookModel Book);
        Task<RequestResult> UpdateBookAsync(int id, BookModel Book);
        Task<RequestResult> DeleteBookAsync(int id);
    }
}

using Library.Data.Entities;
using Library.Data.Interfaces.Repositories;
using Library.Resources.Extensions.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Repositories
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        private readonly AppDbContext _appDbContext;
        public BookRepository(AppDbContext appDbContext) : base(appDbContext) 
        { 
            _appDbContext = appDbContext;
        }
        public IEnumerable<Book> GetAllBooks()
        {
            return [.. FindByCondition(c => !c.IsDeleted).OrderBy(c => c.Title)];
        }
        public async Task<Book> GetBookByIdAsync(int id)
        {
            var Book = await FindByCondition(c => c.Id == id && !c.IsDeleted).FirstOrDefaultAsync() ?? throw new NotFoundException($"Book with id {id} not found.");
            return Book;
        }
        public async Task<Book> CreateBookAsync(Book Book)
        {
            await CreateAsync(Book);
            await SaveAsync();
            return Book;
        }
        public async Task<Book> UpdateBookAsync(int id, Book Book)
        {
            var existingBook = await GetBookByIdAsync(id) ?? throw new NotFoundException($"Book with id {id} not found.");

            // Update Book
            existingBook.Title = Book.Title;
            Book.UpdatedDate = DateTime.Now;

            Update(existingBook);
            await SaveAsync();

            return existingBook;
        }
        public async Task<Book> DeleteBookAsync(int id)
        {
            var existingBook = await GetBookByIdAsync(id) ?? throw new NotFoundException($"Book with id {id} not found.");

            // Delete Book
            existingBook.DeletedDate = DateTime.Now;
            existingBook.IsDeleted = true;

            Update(existingBook);
            await SaveAsync();

            return existingBook;
        }
    }
}


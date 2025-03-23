using Core.Common;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IBookRepository
    {
        Task<Result<Book>> AddBookAsync(Book book);
        Task<Result<Book>> DeleteBookAsync(int id);
        Task<Result<Book>> GetBookByIdAsync(int id);
        Task<Result<Book>> GetBookByISBNAsync(string isbn);
        Task<Result<IEnumerable<Book>>> SearchBooksAsync(string query);
        Task<Result<Book>> UpdateBookAsync(int id, Book book);
    }
}

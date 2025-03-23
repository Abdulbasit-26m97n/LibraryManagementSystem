using Core.Common;
using Core.DTOs;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IBookService
    {
        Task<Result<Book>> AddBookAsync(AddBookDto bookDto);
        Task<Result<BookDto>> DeleteBookAsync(int id);
        Task<Result<BookDto>> GetBookByISBNAsync(string isbn);
        Task<Result<IEnumerable<BookDto>>> SearchBookAsync(string query);
        Task<Result<Book>> UpdateBookAsync(int id, Book book);
    }
}

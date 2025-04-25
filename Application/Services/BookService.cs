using Core.Common;
using Core.DTOs;
using Core.Entities;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace Application.Services
{
    public class BookService(IBookRepository bookRepository) : IBookService
    {
        public async Task<Result<BookDto>> GetBookByISBNAsync(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
            {
                return Result<BookDto>.Failure(ResultErrorType.BadRequest,"ISBN cannot be empty.");
            }

            var bookResult = await bookRepository.GetBookByISBNAsync(isbn);

            if (!bookResult.IsSuccess || bookResult.Data == null)
            {
                return Result<BookDto>.Failure(bookResult.ErrorType, bookResult.Message);
            }

            var book = new BookDto(
                bookResult.Data.Id,
                bookResult.Data.Title,
                bookResult.Data.Author,
                bookResult.Data.ISBN,
                bookResult.Data.PublicationYear,
                bookResult.Data.IsAvailable
                );

            return Result<BookDto>.Success(book, bookResult.Message);
        }

        public async Task<Result<IEnumerable<BookDto>>> SearchBookAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Result<IEnumerable<BookDto>>.Failure(ResultErrorType.BadRequest, "Search query cannot be empty.");
            }

            var bookResult = await bookRepository.SearchBooksAsync(query);

            if (!bookResult.IsSuccess || bookResult.Data == null)
            {
                return Result<IEnumerable<BookDto>>.Failure(bookResult.ErrorType, bookResult.Message);
            }

            var books = new List<BookDto>();

            foreach (var book in bookResult.Data)
            {
                var bookNew = new BookDto(
                    book.Id,
                    book.Title,
                    book.Author,
                    book.ISBN,
                    book.PublicationYear,
                    book.IsAvailable
                    );
                books.Add(bookNew);
            }

            return Result<IEnumerable<BookDto>>.Success(books, bookResult.Message);
        }

        public async Task<Result<Book>> AddBookAsync(AddBookDto bookDto)
        {
            if (bookDto == null)
            {
                return Result<Book>.Failure(ResultErrorType.BadRequest, "Invalid book data. The book object cannot be null.");
            }
            var book = new Book
            {
                Title = bookDto.Title,
                Author = bookDto.Author,
                ISBN = bookDto.ISBN,
                PublicationYear = bookDto.PublicationYear,
                IsAvailable = bookDto.IsAvailable,
                AddedDate = bookDto.AddedDate,
                LastIssuedDate = bookDto.LastIssuedDate,
                InternalNote = bookDto.InternalNote,
                TotalCopies = bookDto.TotalCopies,
                IssuedCopies = bookDto.IssuedCopies,
            };
            var bookNew = await bookRepository.AddBookAsync(book);
            return bookNew;
        }

        public async Task<Result<Book>> UpdateBookAsync(int id, Book book)
        {
            if (book == null)
            {
                return Result<Book>.Failure(ResultErrorType.BadRequest, "Invalid book data. The book object cannot be null.");
            }

            if (id <= 0)
            {
                return Result<Book>.Failure(ResultErrorType.BadRequest, "Invalid book ID. ID must be greater than zero.");
            }

            // Save changes
            var updateResult = await bookRepository.UpdateBookAsync(id, book);

            return updateResult;
        }

        public async Task<Result<BookDto>> DeleteBookAsync(int id)
        {
            if (id <= 0)
            {
                return Result<BookDto>.Failure(ResultErrorType.BadRequest, "Invalid book ID. ID must be greater than zero.");
            }

            // Save changes
            var deleteResult = await bookRepository.DeleteBookAsync(id);

            if (!deleteResult.IsSuccess || deleteResult.Data == null)
            {
                return Result<BookDto>.Failure(deleteResult.ErrorType, deleteResult.Message);
            }

            var book = new BookDto(
               deleteResult.Data.Id,
               deleteResult.Data.Title,
               deleteResult.Data.Author,
               deleteResult.Data.ISBN,
               deleteResult.Data.PublicationYear,
            deleteResult.Data.IsAvailable
            );

            return Result<BookDto>.Success(book, deleteResult.Message);
        }

    }
}

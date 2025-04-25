using Core.Common;
using Core.Entities;
using Core.Enums;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BookRepository(LMSDbContext context) : IBookRepository
    {
        public async Task<Result<Book>> AddBookAsync(Book book)
        {
            if (book == null)
            {
                return Result<Book>.Failure(ResultErrorType.BadRequest, "Invalid book data. The book object cannot be null.");
            }

            try
            {
                context.Books.Add(book);
                await context.SaveChangesAsync();
                return Result<Book>.Success(book, "Book added successfully.");
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2627)
            {
                // SQL Server error 2627 = Unique constraint violation
                return Result<Book>.Failure(ResultErrorType.AlreadyExists, "A book with the same ISBN already exists.");
            }
            catch (DbUpdateException ex)
            {
                return Result<Book>.Failure(ResultErrorType.DatabaseError, $"Database error: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return Result<Book>.Failure(ResultErrorType.UnknownError, $"An unexpected error occurred: {ex.Message}");
            }
        }



        public async Task<Result<Book>> UpdateBookAsync(int id, Book book)
        {
            if (book == null)
            {
                return Result<Book>.Failure(ResultErrorType.BadRequest, "Invalid book data. The book object cannot be null.");
            }

            try
            {
                var bookExisting = await context.Books.FirstOrDefaultAsync(b => b.Id == id);
                if (bookExisting == null)
                {
                    return Result<Book>.Failure(ResultErrorType.NotFound, "Book not found.");
                }

                // Update book properties
                bookExisting.Title = book.Title;
                bookExisting.Author = book.Author;
                bookExisting.ISBN = book.ISBN;
                bookExisting.PublicationYear = book.PublicationYear;
                bookExisting.IsAvailable = book.IsAvailable;

                await context.SaveChangesAsync();
                return Result<Book>.Success(bookExisting, "Book updated successfully.");
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result<Book>.Failure(ResultErrorType.ConcurrencyConflict, "The book was modified or deleted by another process.");
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2627)
            {
                return Result<Book>.Failure(ResultErrorType.AlreadyExists, "ISBN must be unique. Another book already has this ISBN.");
            }
            catch (DbUpdateException ex)
            {
                return Result<Book>.Failure(ResultErrorType.DatabaseError, $"Database error: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return Result<Book>.Failure(ResultErrorType.UnknownError, $"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<Result<Book>> DeleteBookAsync(int id)
        {
            try
            {
                var book = await context.Books.FirstOrDefaultAsync(b => b.Id == id);
                if (book == null)
                {
                    return Result<Book>.Failure(ResultErrorType.NotFound, "Book not found.");
                }

                context.Books.Remove(book);
                await context.SaveChangesAsync();

                return Result<Book>.Success(book, "Book deleted successfully.");
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
            {
                // SQL Server error 547 = Foreign key constraint violation
                return Result<Book>.Failure(ResultErrorType.ForeignKeyViolation, "Cannot delete this book because it is referenced in other records.");
            }
            catch (DbUpdateException ex)
            {
                return Result<Book>.Failure(ResultErrorType.DatabaseError, $"Database error: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return Result<Book>.Failure(ResultErrorType.UnknownError, $"An unexpected error occurred: {ex.Message}");
            }
        }
        public async Task<Result<Book>> GetBookByISBNAsync(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
            {
                return Result<Book>.Failure(ResultErrorType.BadRequest, "ISBN cannot be empty.");
            }

            try
            {
                var book = await context.Books.FirstOrDefaultAsync(b => b.ISBN == isbn);

                if (book == null)
                {
                    return Result<Book>.Failure(ResultErrorType.NotFound, "Book not found.");
                }

                return Result<Book>.Success(book);
            }
            catch (Exception ex)
            {
                return Result<Book>.Failure(ResultErrorType.UnknownError, $"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<Result<Book>> GetBookByIdAsync(int id)
        {
            if (id <= 0)
            {
                return Result<Book>.Failure(ResultErrorType.BadRequest, "Invalid book ID. ID must be greater than zero.");
            }

            try
            {
                var book = await context.Books.FirstOrDefaultAsync(b => b.Id == id);

                if (book == null)
                {
                    return Result<Book>.Failure(ResultErrorType.NotFound, "Book not found.");
                }

                return Result<Book>.Success(book);
            }
            catch (Exception ex)
            {
                return Result<Book>.Failure(ResultErrorType.UnknownError, $"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<Book>>> SearchBooksAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Result<IEnumerable<Book>>.Failure(ResultErrorType.BadRequest, "Search query cannot be empty.");
            }

            try
            {
                var books = await context.Books
                    .Where(b =>
                        b.Title.Contains(query) ||
                        b.Author.Contains(query))
                    .ToListAsync();

                if (!books.Any())
                {
                    return Result<IEnumerable<Book>>.Failure(ResultErrorType.NotFound, "No books found matching the search criteria.");
                }

                return Result<IEnumerable<Book>>.Success(books);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Book>>.Failure(ResultErrorType.UnknownError, $"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}

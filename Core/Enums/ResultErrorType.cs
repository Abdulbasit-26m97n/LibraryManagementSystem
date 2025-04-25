using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enums
{
    /// <summary>
    /// Defines error types that can occur in the application, particularly in API responses and EF Core operations.
    /// </summary>
    public enum ResultErrorType
    {
        /// <summary>
        /// No error occurred.
        /// </summary>
        None = 0,

        /// <summary>
        /// The requested entity was not found (HTTP 404).
        /// </summary>
        NotFound = 1,

        /// <summary>
        /// The entity already exists (e.g., unique constraint violation) (HTTP 409).
        /// </summary>
        AlreadyExists = 2,

        /// <summary>
        /// Data validation failed (HTTP 400).
        /// </summary>
        ValidationFailed = 3,

        /// <summary>
        /// A concurrency conflict occurred (e.g., multiple updates at the same time) (HTTP 409).
        /// </summary>
        ConcurrencyConflict = 4,

        /// <summary>
        /// A foreign key constraint was violated (HTTP 400).
        /// </summary>
        ForeignKeyViolation = 5,

        /// <summary>
        /// A general database error occurred (HTTP 500).
        /// </summary>
        DatabaseError = 6,

        /// <summary>
        /// A transaction failed and was rolled back (HTTP 500).
        /// </summary>
        TransactionFailed = 7,

        /// <summary>
        /// The user is unauthorized to perform this action (HTTP 401).
        /// </summary>
        Unauthorized = 8,

        /// <summary>
        /// The user is forbidden from performing this action (HTTP 403).
        /// </summary>
        Forbidden = 9,

        /// <summary>
        /// The request was malformed or invalid (HTTP 400).
        /// </summary>
        BadRequest = 10,

        /// <summary>
        /// An unknown or unhandled error occurred (HTTP 500).
        /// </summary>
        UnknownError = 11
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common
{
    public record Result<T>(bool IsSuccess, ResultErrorType ErrorType, string Message, T? Data)
    {
        public static Result<T> Success(T data, string message = "Operation successful", ResultErrorType errorType = ResultErrorType.None)
        {
            return new Result<T>(true, errorType, message, data);
        }

        public static Result<T> Failure(ResultErrorType errorType ,string message = "Operation failed")
        {
            return new Result<T>(false, errorType, message, default);
        }
    }
}

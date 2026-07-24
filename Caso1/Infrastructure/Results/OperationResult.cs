using System.Collections.Generic;

namespace Caso1.Infrastructure.Results
{
    public class OperationResult
    {
        public bool Success { get; protected set; }
        public string Message { get; protected set; }
        public List<string> Errors { get; protected set; } = new List<string>();

        protected OperationResult(bool success, string message, List<string> errors = null)
        {
            Success = success;
            Message = message;
            if (errors != null)
            {
                Errors = errors;
            }
        }

        public static OperationResult Ok(string message = "Operación exitosa")
        {
            return new OperationResult(true, message);
        }

        public static OperationResult Fail(string message, List<string> errors = null)
        {
            return new OperationResult(false, message, errors);
        }
    }

    public class OperationResult<T> : OperationResult
    {
        public T Data { get; private set; }

        protected OperationResult(bool success, string message, T data, List<string> errors = null)
            : base(success, message, errors)
        {
            Data = data;
        }

        public static OperationResult<T> Ok(T data, string message = "Operación exitosa")
        {
            return new OperationResult<T>(true, message, data);
        }

        public static new OperationResult<T> Fail(string message, List<string> errors = null)
        {
            return new OperationResult<T>(false, message, default(T), errors);
        }
    }
}
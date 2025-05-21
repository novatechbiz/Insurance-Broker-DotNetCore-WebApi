using FluentValidation.Results;

namespace InsuraNova.Helpers
{

    public class ApiResponse<T>
    {
        public string Status { get; set; }
        public string Action { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
    }


    public class ResponseHelper
    {

        public static ApiResponse<T> CreateSuccessResponse<T>(T data, string action)
        {
            return new ApiResponse<T>
            {
                Status = ApiStatus.Success,
                Action = action,
                Data = data
            };
        }

        public static ApiResponse<T> CreateFailureResponse<T>(string action, string message)
        {
            return new ApiResponse<T>
            {
                Status = ApiStatus.Failure,
                Action = action,
                Message = message
            };
        }

        public static ApiResponse<T> CreateNotFoundResponse<T>(string action, string errorMessage = "Resource not found.")
        {
            return new ApiResponse<T>
            {
                Status = ApiStatus.Failure,
                Action = action,
                Data = default,
                Message = errorMessage
            };
        }

        public static ApiResponse<T> CreateValidationErrorResponse<T>(ValidationResult validationResult, string action)
        {
            return new ApiResponse<T>
            {
                Status = ApiStatus.Failure,
                Action = action,
                Data = default,
                Message = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage))
            };
        }

        public static ApiResponse<T> CreateExceptionResponse<T>(Exception ex, string action)
        {
            return new ApiResponse<T>
            {
                Status = ApiStatus.Failure,
                Action = action,
                Data = default,
                Message = ex.Message
            };
        }

        public static ApiResponse<T> CreateCustomErrorResponse<T>(string errorMessage, string action)
        {
            return new ApiResponse<T>
            {
                Status = ApiStatus.Failure,
                Action = action,
                Data = default,
                Message = errorMessage
            };
        }
    }
}

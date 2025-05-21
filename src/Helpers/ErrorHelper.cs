using System;

namespace InsuraNova.Helpers
{
    public static class ErrorHelper
    {
        public static IResult HandleException<T>(ILogger logger, Exception ex, string action)
        {
            logger.LogError(ex, "An error occurred while performing action: {Action}", action);
            int statusCode = StatusCodes.Status500InternalServerError;
            var errorResponse = ResponseHelper.CreateExceptionResponse<T>(ex, action);
            return Results.Json(errorResponse, statusCode: statusCode);
        }

        public static IResult HandleExceptionWithLogging<T>(ILogger logger, Exception exception, HttpContext context, string action)
        {
            var userEmail = JwtHelper.GetEmailFromToken(context);
            logger.LogError(exception,
            "An error occurred while performing action: {Action} for user: {Username} at {DateTime}",
            action,
            userEmail,
            DateTime.UtcNow);
            int statusCode = StatusCodes.Status500InternalServerError;
            var errorResponse = ResponseHelper.CreateExceptionResponse<T>(exception, action);
            return Results.Json(errorResponse, statusCode: statusCode);
        }
    }
}

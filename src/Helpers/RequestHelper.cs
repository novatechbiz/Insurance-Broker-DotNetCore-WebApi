
namespace InsuraNova.Helpers
{
    public static class RequestHelper
    {
        public static async Task<IResult> HandleRequestAsync(Func<Task<IResult>> action)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                var errorResponse = ResponseHelper.CreateExceptionResponse<object>(ex, ApiAction.Read);
                return Results.Problem(errorResponse.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        public static async Task<IResult> HandleRequestWithValidationAsync<T>(IValidator<T> validator, T entity, Func<Task<IResult>> action)
        {
            // Validate the entity
            var validationResult = await validator.ValidateAsync(entity);
            if (!validationResult.IsValid)
            {
                var errorResponse = ResponseHelper.CreateValidationErrorResponse<object>(validationResult, ApiAction.Created);
                return Results.BadRequest(errorResponse);
            }

            return await HandleRequestAsync(action);
        }
    }
}

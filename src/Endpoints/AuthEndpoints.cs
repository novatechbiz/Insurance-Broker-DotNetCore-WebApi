
using InsuraNova.Helpers;

namespace InsuraNova.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            
            app.MapPost("/login", async (IMediator mediator, LoginRequest loginRequest, ILogger<Program> logger, HttpContext context) =>
            {
                try
                {
                    logger.LogInformation("Attempting to log in user with username: {Username}", loginRequest.Username);
                    LoginResponse? result = await mediator.Send(loginRequest);
                    if (result == null)
                    {
                        logger.LogWarning("Login attempt failed for username: {Username}. An error occurred during login.", loginRequest.Username);
                        return Results.BadRequest(new { success = false, message = "An error occurred during login." });
                    }

                    if (!result.Success)
                    {
                        logger.LogWarning("Login attempt failed for username: {Username}. Reason: {Reason}", loginRequest.Username, result.Message);
                        return Results.BadRequest(new { success = false, message = result.Message });
                    }
                    logger.LogInformation("Successfully logged in user with username: {Username}", loginRequest.Username);
                    return Results.Ok(new
                    {
                        success = true,
                        message = result.Message,
                        token = result.Token,
                        user = result.User
                    });
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }

            })
            .WithName("GenerateToken")
            .WithTags("Auth");
        }
    }
}

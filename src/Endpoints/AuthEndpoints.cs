using InsuraNova.Dto;
using InsuraNova.Handlers;
using InsuraNova.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace InsuraNova.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/login", async (LoginRequest loginRequest, IMediator mediator, IValidator<LoginRequest> validator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    logger.LogInformation("Attempting to log in user with username: {Username}", loginRequest.Username);

                    var validationResult = validator.Validate(loginRequest);
                    if (!validationResult.IsValid)
                    {
                        return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(
                            ApiAction.Read,
                            "Validation failed: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))
                        ));
                    }

                    LoginResponse? result = await mediator.Send(new LoginCommand(loginRequest));

                    if (!result.Success)
                    {
                        return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(
                            ApiAction.Read,
                            result.Message ?? "Login failed. Please check your credentials."
                        ));
                    }

                    // Return success response
                    return Results.Ok(ResponseHelper.CreateSuccessResponse(result, ApiAction.Read));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GenerateToken")
            .WithTags("Auth");

            app.MapPost("/logout", [Authorize] async (IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    logger.LogInformation("Attempting to log out user...");
                    LogOutResponse? result = await mediator.Send(new LogoutCommand());
                    return Results.Ok(ResponseHelper.CreateSuccessResponse(result, ApiAction.Read));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("Logout")
            .WithTags("Auth")
            .RequireAuthorization();

            app.MapPost("/refresh-token", [AllowAnonymous] async (IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    logger.LogInformation("Attempting to get new access token from refresh token...");
                    LoginResponse? result = await mediator.Send(new RefreshTokenCommand());

                    if (!result.Success)
                    {
                        return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(
                            ApiAction.Read,
                            result.Message ?? "refresh failed.."
                        ));
                    }

                    return Results.Ok(ResponseHelper.CreateSuccessResponse(result, ApiAction.Read));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("RefreshToken")
            .WithTags("Auth");
        }
    }
}
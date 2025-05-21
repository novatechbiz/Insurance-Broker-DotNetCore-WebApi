using InsuraNova.Handlers;
using InsuraNova.Helpers;

namespace InsuraNova.Endpoints
{
    public static class UserRoleEndpoints
    {
        public static void MapUserRoleEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/user-roles", async (IMediator mediator, ILogger<Program> logger, HttpContext context) =>
            {
                try
                {
                    logger.LogInformation("Attempting to retrieve all user roles");
                    var userRoleDtos = await mediator.Send(new GetAllUserRolesQuery());
                    logger.LogInformation("Successfully retrieved all user roles");
                    return Results.Ok(ResponseHelper.CreateSuccessResponse(userRoleDtos, ApiAction.Read));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GetAllUserRoles")
            .WithTags("UserRoles")
            .RequireAuthorization();
        }
    }
}

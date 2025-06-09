using InsuraNova.Handlers;
using InsuraNova.Helpers;
using InsuraNova.Resources;
using Microsoft.AspNetCore.Authorization;

namespace InsuraNova.Endpoints
{
    public static class MenuItemEndpoints
    {
        public static void MapMenuItemEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/menu-items", [Authorize] async (IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var items = await mediator.Send(new GetMenuItemsQuery());
                    return Results.Ok(ResponseHelper.CreateSuccessResponse(items, ApiAction.Read));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
             .WithName("GetMenuItems")
             .WithTags("Menu")
             .RequireAuthorization();


        }
    }
}

using InsuraNova.Handlers;
using InsuraNova.Helpers;
using InsuraNova.Models;
using InsuraNova.Resources;
using Microsoft.AspNetCore.Authorization;

namespace InsuraNova.Endpoints
{
    public static class RoleEndpoints
    {
        public static void MapRoleEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/roles", async (IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var roles = await mediator.Send(new GetAllRolesQuery());
                    return Results.Ok(ResponseHelper.CreateSuccessResponse(roles, ApiAction.Read));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GetAllRoles")
            .WithTags("Roles")
            .RequireAuthorization();

            app.MapGet("/roles/{id}", async (int id, IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var role = await mediator.Send(new GetRoleByIdQuery(id));
                    if (role != null)
                        return Results.Ok(ResponseHelper.CreateSuccessResponse(role, ApiAction.Read));
                    else
                        return Results.NotFound(ResponseHelper.CreateNotFoundResponse<Role>(ApiAction.Read, ApplicationMessages.NotFoundMessage));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GetRoleById")
            .WithTags("Roles")
            .RequireAuthorization();

            app.MapPost("/roles", [Authorize] async (Role role, IMediator mediator, IValidator<Role> validator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var validation = validator.Validate(role);
                    if (!validation.IsValid)
                    {
                        return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(ApiAction.Created,
                            "Validation failed: " + string.Join(", ", validation.Errors.Select(e => e.ErrorMessage))));
                    }

                    JwtHelper.InitializeEntityMetadata(context, role);
                    var created = await mediator.Send(new AddRoleCommand(role));
                    return Results.Created($"/roles/{created.Id}", ResponseHelper.CreateSuccessResponse(created, ApiAction.Created));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Created);
                }
            })
            .WithName("CreateRole")
            .WithTags("Roles")
            .RequireAuthorization();

            app.MapPut("/roles/{id}", [Authorize] async (int id, Role role, IMediator mediator, IValidator<Role> validator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var validation = validator.Validate(role);
                    if (!validation.IsValid)
                    {
                        return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(ApiAction.Updated,
                            "Validation failed: " + string.Join(", ", validation.Errors.Select(e => e.ErrorMessage))));
                    }

                    if (id != role.Id)
                        return Results.BadRequest(ResponseHelper.CreateCustomErrorResponse<Role>(ApplicationMessages.IdMismatchMessage, ApiAction.Updated));

                    var existing = await mediator.Send(new GetRoleByIdQuery(id));
                    if (existing == null)
                        return Results.NotFound(ResponseHelper.CreateNotFoundResponse<Role>(ApiAction.Updated, ApplicationMessages.NotFoundMessage));

                    existing.Name = role.Name;
                    JwtHelper.InitializeUpdateMetadata(context, existing);

                    var updated = await mediator.Send(new UpdateRoleCommand(existing));
                    return Results.Ok(ResponseHelper.CreateSuccessResponse(updated, ApiAction.Updated));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Updated);
                }
            })
            .WithName("UpdateRole")
            .WithTags("Roles")
            .RequireAuthorization();

            app.MapDelete("/roles/{id}", [Authorize] async (int id, IMediator mediator, ILogger<Program> logger, HttpContext context) =>
            {
                try
                {
                    var success = await mediator.Send(new DeleteRoleCommand(id));
                    if (success)
                        return Results.Ok(ResponseHelper.CreateSuccessResponse(true, ApiAction.Deleted));
                    else
                        return Results.NotFound(ResponseHelper.CreateNotFoundResponse<bool>(ApiAction.Deleted, ApplicationMessages.NotFoundMessage));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Deleted);
                }
            })
            .WithName("DeleteRole")
            .WithTags("Roles")
            .RequireAuthorization();
        }
    }
}

using InsuraNova.Handlers;
using InsuraNova.Helpers;
using InsuraNova.Models;
using InsuraNova.Resources;
using Microsoft.AspNetCore.Authorization;

namespace InsuraNova.Endpoints
{
    public static class UserTypeEndpoints
    {
        public static void MapUserTypeEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/user-types", async (IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var userTypes = await mediator.Send(new GetAllUserTypesQuery());
                    return Results.Ok(ResponseHelper.CreateSuccessResponse(userTypes, ApiAction.Read));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GetAllUserTypes")
            .WithTags("UserTypes")
            .RequireAuthorization();

            app.MapGet("/user-types/{id}", async (int id, IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var userType = await mediator.Send(new GetUserTypeByIdQuery(id));
                    if (userType != null)
                        return Results.Ok(ResponseHelper.CreateSuccessResponse(userType, ApiAction.Read));
                    else
                        return Results.NotFound(ResponseHelper.CreateNotFoundResponse<UserType>(ApiAction.Read, ApplicationMessages.NotFoundMessage));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GetUserTypeById")
            .WithTags("UserTypes")
            .RequireAuthorization();

            app.MapPost("/user-types", [Authorize] async (UserType userType, IMediator mediator, IValidator<UserType> validator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var validation = validator.Validate(userType);
                    if (!validation.IsValid)
                    {
                        return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(ApiAction.Created,
                            "Validation failed: " + string.Join(", ", validation.Errors.Select(e => e.ErrorMessage))));
                    }

                    JwtHelper.InitializeEntityMetadata(context, userType);
                    var created = await mediator.Send(new AddUserTypeCommand(userType));
                    return Results.Created($"/user-types/{created.Id}", ResponseHelper.CreateSuccessResponse(created, ApiAction.Created));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Created);
                }
            })
            .WithName("CreateUserType")
            .WithTags("UserTypes")
            .RequireAuthorization();

            app.MapPut("/user-types/{id}", [Authorize] async (int id, UserType userType, IMediator mediator, IValidator<UserType> validator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var validation = validator.Validate(userType);
                    if (!validation.IsValid)
                    {
                        return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(ApiAction.Updated,
                            "Validation failed: " + string.Join(", ", validation.Errors.Select(e => e.ErrorMessage))));
                    }

                    if (id != userType.Id)
                        return Results.BadRequest(ResponseHelper.CreateCustomErrorResponse<UserType>(ApplicationMessages.IdMismatchMessage, ApiAction.Updated));

                    var existing = await mediator.Send(new GetUserTypeByIdQuery(id));
                    if (existing == null)
                        return Results.NotFound(ResponseHelper.CreateNotFoundResponse<UserType>(ApiAction.Updated, ApplicationMessages.NotFoundMessage));

                    existing.Name = userType.Name;
                    JwtHelper.InitializeUpdateMetadata(context, existing);

                    var updated = await mediator.Send(new UpdateUserTypeCommand(existing));
                    return Results.Ok(ResponseHelper.CreateSuccessResponse(updated, ApiAction.Updated));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Updated);
                }
            })
            .WithName("UpdateUserType")
            .WithTags("UserTypes")
            .RequireAuthorization();

            app.MapDelete("/user-types/{id}", [Authorize] async (int id, IMediator mediator, ILogger<Program> logger, HttpContext context) =>
            {
                try
                {
                    var success = await mediator.Send(new DeleteUserTypeCommand(id));
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
            .WithName("DeleteUserType")
            .WithTags("UserTypes")
            .RequireAuthorization();
        }
    }
}

using InsuraNova.Handlers;
using InsuraNova.Helpers;
using InsuraNova.Models;
using InsuraNova.Resources;
using Microsoft.AspNetCore.Authorization;

namespace InsuraNova.Endpoints
{
    public static class GenderTypeEndpoints
    {
        public static void MapGenderTypeEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/gender-types", async (IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var types = await mediator.Send(new GetAllGenderTypesQuery());
                    return Results.Ok(ResponseHelper.CreateSuccessResponse(types, ApiAction.Read));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GetAllGenderTypes")
            .WithTags("GenderTypes")
            .RequireAuthorization();

            app.MapGet("/gender-types/{id}", async (int id, IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var gender = await mediator.Send(new GetGenderTypeByIdQuery(id));
                    if (gender != null)
                        return Results.Ok(ResponseHelper.CreateSuccessResponse(gender, ApiAction.Read));
                    else
                        return Results.NotFound(ResponseHelper.CreateNotFoundResponse<GenderType>(ApiAction.Read, ApplicationMessages.NotFoundMessage));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GetGenderTypeById")
            .WithTags("GenderTypes")
            .RequireAuthorization();

            app.MapPost("/gender-types", [Authorize] async (GenderType genderType, IMediator mediator, IValidator<GenderType> validator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var validation = validator.Validate(genderType);
                    if (!validation.IsValid)
                    {
                        return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(ApiAction.Created,
                            "Validation failed: " + string.Join(", ", validation.Errors.Select(e => e.ErrorMessage))));
                    }

                    JwtHelper.InitializeEntityMetadata(context, genderType);
                    var created = await mediator.Send(new AddGenderTypeCommand(genderType));
                    return Results.Created($"/gender-types/{created.Id}", ResponseHelper.CreateSuccessResponse(created, ApiAction.Created));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Created);
                }
            })
            .WithName("CreateGenderType")
            .WithTags("GenderTypes")
            .RequireAuthorization();

            app.MapPut("/gender-types/{id}", [Authorize] async (int id, GenderType genderType, IMediator mediator, IValidator<GenderType> validator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var validation = validator.Validate(genderType);
                    if (!validation.IsValid)
                    {
                        return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(ApiAction.Updated,
                            "Validation failed: " + string.Join(", ", validation.Errors.Select(e => e.ErrorMessage))));
                    }

                    if (id != genderType.Id)
                        return Results.BadRequest(ResponseHelper.CreateCustomErrorResponse<GenderType>(ApplicationMessages.IdMismatchMessage, ApiAction.Updated));

                    var existing = await mediator.Send(new GetGenderTypeByIdQuery(id));
                    if (existing == null)
                        return Results.NotFound(ResponseHelper.CreateNotFoundResponse<GenderType>(ApiAction.Updated, ApplicationMessages.NotFoundMessage));

                    existing.Name = genderType.Name;
                    JwtHelper.InitializeUpdateMetadata(context, existing);

                    var updated = await mediator.Send(new UpdateGenderTypeCommand(existing));
                    return Results.Ok(ResponseHelper.CreateSuccessResponse(updated, ApiAction.Updated));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Updated);
                }
            })
            .WithName("UpdateGenderType")
            .WithTags("GenderTypes")
            .RequireAuthorization();

            app.MapDelete("/gender-types/{id}", [Authorize] async (int id, IMediator mediator, ILogger<Program> logger, HttpContext context) =>
            {
                try
                {
                    var success = await mediator.Send(new DeleteGenderTypeCommand(id));
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
            .WithName("DeleteGenderType")
            .WithTags("GenderTypes")
            .RequireAuthorization();
        }
    }
}

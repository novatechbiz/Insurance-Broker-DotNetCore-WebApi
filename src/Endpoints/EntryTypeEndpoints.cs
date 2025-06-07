using InsuraNova.Handlers;
using InsuraNova.Helpers;
using InsuraNova.Resources;
using Microsoft.AspNetCore.Authorization;


namespace InsuraNova.Endpoints
{
    public static class EntryTypeEndpoints
    {
        public static void MapEntryTypeEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/entrytypes", async (IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var result = await mediator.Send(new GetAllEntryTypesQuery());
                    return Results.Ok(ResponseHelper.CreateSuccessResponse(result, ApiAction.Read));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GetAllEntryTypes")
            .WithTags("EntryTypes")
            .RequireAuthorization();

            app.MapGet("/entrytypes/{id}", async (int id, IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var result = await mediator.Send(new GetEntryTypeByIdQuery(id));
                    if (result == null)
                        return Results.NotFound(ResponseHelper.CreateNotFoundResponse<EntryType>(ApiAction.Read, ApplicationMessages.EntityNotFoundMessage));

                    return Results.Ok(ResponseHelper.CreateSuccessResponse(result, ApiAction.Read));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GetEntryTypeById")
            .WithTags("EntryTypes")
            .RequireAuthorization();

            app.MapPost("/entrytypes", [Authorize] async (EntryType entryType, IMediator mediator, IValidator<EntryType> validator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var validationResult = validator.Validate(entryType);
                    if (!validationResult.IsValid)
                    {
                        var errors = string.Join(", ", validationResult.Errors.Select(entryType => entryType.ErrorMessage));
                        return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(ApiAction.Created, $"Validation failed: {errors}"));
                    }

                    JwtHelper.InitializeEntityMetadata(context, entryType);
                    var result = await mediator.Send(new AddEntryTypeCommand(entryType));
                    return Results.Created($"/entrytypes/{result.Id}", ResponseHelper.CreateSuccessResponse(result, ApiAction.Created));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Created);
                }
            })
            .WithName("CreateEntryType")
            .WithTags("EntryTypes")
            .RequireAuthorization();

            app.MapPut("/entrytypes/{id}", [Authorize] async (
                int id,
                EntryType entryType,
                IMediator mediator,
                IValidator<EntryType> validator,
                HttpContext context,
                ILogger<Program> logger) =>
                        {
                            try
                            {
                                var validationResult = validator.Validate(entryType);
                                if (!validationResult.IsValid)
                                {
                                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                                    return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(
                                        ApiAction.Updated, $"Validation failed: {errors}"));
                                }

                                if (id != entryType.Id)
                                    return Results.BadRequest(ResponseHelper.CreateCustomErrorResponse<EntryType>(
                                        ApplicationMessages.IdMismatchMessage, ApiAction.Updated));

                                var existing = await mediator.Send(new GetEntryTypeByIdQuery(id));
                                if (existing == null)
                                    return Results.NotFound(ResponseHelper.CreateNotFoundResponse<EntryType>(
                                        ApiAction.Updated, ApplicationMessages.EntityNotFoundMessage));

                                // Update the single property
                                existing.TypeName = entryType.TypeName;

                                JwtHelper.InitializeUpdateMetadata(context, existing);

                                var updated = await mediator.Send(new UpdateEntryTypeCommand(existing));
                                return Results.Ok(ResponseHelper.CreateSuccessResponse(updated, ApiAction.Updated));
                            }
                            catch (Exception ex)
                            {
                                return ErrorHelper.HandleExceptionWithLogging<object>(
                                    logger, ex, context, ApiAction.Updated);
                            }
                        })
            .WithName("UpdateEntryType")
            .WithTags("EntryTypes")
            .RequireAuthorization();


            app.MapDelete("/entrytypes/{id}", async (int id, IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var deleted = await mediator.Send(new DeleteEntryTypeCommand(id));
                    if (deleted)
                        return Results.Ok(ResponseHelper.CreateSuccessResponse(true, ApiAction.Deleted));

                    return Results.NotFound(ResponseHelper.CreateNotFoundResponse<bool>(ApiAction.Deleted, ApplicationMessages.EntityNotFoundMessage));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Deleted);
                }
            })
            .WithName("DeleteEntryType")
            .WithTags("EntryTypes")
            .RequireAuthorization();
        }
    }
}

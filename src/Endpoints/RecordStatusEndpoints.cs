using InsuraNova.Handlers;
using InsuraNova.Helpers;
using InsuraNova.Resources;
using Microsoft.AspNetCore.Authorization;


namespace InsuraNova.Endpoints
{
    public static class RecordStatusEndpoints
    {
        public static void MapRecordStatusEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/recordStatuses", async (IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var result = await mediator.Send(new GetAllRecordStatusesQuery());
                    return Results.Ok(ResponseHelper.CreateSuccessResponse(result, ApiAction.Read));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GetAllRecordStatuses")
            .WithTags("RecordStatuses")
            .RequireAuthorization();

            app.MapGet("/recordStatuses/{id}", async (int id, IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var result = await mediator.Send(new GetRecordStatusByIdQuery(id));
                    if (result == null)
                        return Results.NotFound(ResponseHelper.CreateNotFoundResponse<RecordStatus>(ApiAction.Read, ApplicationMessages.EntityNotFoundMessage));

                    return Results.Ok(ResponseHelper.CreateSuccessResponse(result, ApiAction.Read));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GetRecordStatusById")
            .WithTags("RecordStatuses")
            .RequireAuthorization();

            app.MapPost("/recordStatuses", [Authorize] async (RecordStatus recordStatus, IMediator mediator, IValidator<RecordStatus> validator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var validationResult = validator.Validate(recordStatus);
                    if (!validationResult.IsValid)
                    {
                        var errors = string.Join(", ", validationResult.Errors.Select(recordStatus => recordStatus.ErrorMessage));
                        return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(ApiAction.Created, $"Validation failed: {errors}"));
                    }

                    JwtHelper.InitializeEntityMetadata(context, recordStatus);
                    var result = await mediator.Send(new AddRecordStatusCommand(recordStatus));
                    return Results.Created($"/recordStatuses/{result.Id}", ResponseHelper.CreateSuccessResponse(result, ApiAction.Created));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Created);
                }
            })
            .WithName("CreateRecordStatus")
            .WithTags("RecordStatuses")
            .RequireAuthorization();

            app.MapPut("/recordStatuses/{id}", [Authorize] async (
                int id,
                RecordStatus recordStatus,
                IMediator mediator,
                IValidator<RecordStatus> validator,
                HttpContext context,
                ILogger<Program> logger) =>
                        {
                            try
                            {
                                if (id != recordStatus.Id)
                                {
                                    return Results.BadRequest(ResponseHelper.CreateCustomErrorResponse<RecordStatus>(
                                        ApplicationMessages.IdMismatchMessage, ApiAction.Updated));
                                }

                                var validationResult = validator.Validate(recordStatus);
                                if (!validationResult.IsValid)
                                {
                                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                                    return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(
                                        ApiAction.Updated, $"Validation failed: {errors}"));
                                }

                                var existing = await mediator.Send(new GetRecordStatusByIdQuery(id));
                                if (existing == null)
                                {
                                    return Results.NotFound(ResponseHelper.CreateNotFoundResponse<RecordStatus>(
                                        ApiAction.Updated, ApplicationMessages.EntityNotFoundMessage));
                                }

                  
                                existing.StatusName = recordStatus.StatusName;
                                existing.StatusValue = recordStatus.StatusValue;

                                JwtHelper.InitializeUpdateMetadata(context, existing);
                                var result = await mediator.Send(new UpdateRecordStatusCommand(existing));

                                return Results.Ok(ResponseHelper.CreateSuccessResponse(result, ApiAction.Updated));
                            }
                            catch (Exception ex)
                            {
                                return ErrorHelper.HandleExceptionWithLogging<object>(
                                    logger, ex, context, ApiAction.Updated);
                            }
                        })
            .WithName("UpdateRecordStatus")
            .WithTags("RecordStatuses")
            .RequireAuthorization();



            app.MapDelete("/recordStatuses/{id}", async (int id, IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var deleted = await mediator.Send(new DeleteRecordStatusCommand(id));
                    if (deleted)
                        return Results.Ok(ResponseHelper.CreateSuccessResponse(true, ApiAction.Deleted));

                    return Results.NotFound(ResponseHelper.CreateNotFoundResponse<bool>(ApiAction.Deleted, ApplicationMessages.EntityNotFoundMessage));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Deleted);
                }
            })
            .WithName("DeleteRecordStatus")
            .WithTags("RecordStatuses")
            .RequireAuthorization();
        }
    }
}

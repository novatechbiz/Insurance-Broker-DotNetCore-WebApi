using InsuraNova.Handlers;
using InsuraNova.Helpers;
using InsuraNova.Resources;
using Microsoft.AspNetCore.Authorization;


namespace InsuraNova.Endpoints
{
    public static class TransactionTypeEndpoints
    {
        public static void MapTransactionTypeEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/transaction-types", async (IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var types = await mediator.Send(new GetAllTransactionTypesQuery());
                    return Results.Ok(ResponseHelper.CreateSuccessResponse(types, ApiAction.Read));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GetAllTransactionTypes")
            .WithTags("TransactionTypes")
            .RequireAuthorization();

            app.MapPost("/transaction-types", [Authorize] async (TransactionType type, IMediator mediator, IValidator<TransactionType> validator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    logger.LogInformation("Creating new transaction type: {@Type}", type);

                    var validationResult = validator.Validate(type);
                    if (!validationResult.IsValid)
                    {
                        return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(ApiAction.Created,
                            "Validation failed: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))));
                    }

                    JwtHelper.InitializeEntityMetadata(context, type);
                    var addedType = await mediator.Send(new AddTransactionTypeCommand(type));

                    return Results.Created($"/transaction-types/{addedType.Id}", ResponseHelper.CreateSuccessResponse(addedType, ApiAction.Created));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Created);
                }
            })
            .WithName("CreateTransactionType")
            .WithTags("TransactionTypes")
            .RequireAuthorization();

            app.MapGet("/transaction-types/{id}", async (int id, IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    logger.LogInformation("Getting transaction type by ID: {Id}", id);
                    var type = await mediator.Send(new GetTransactionTypeByIdQuery(id));

                    if (type == null)
                    {
                        return Results.NotFound(ResponseHelper.CreateNotFoundResponse<TransactionType>(ApiAction.Read, ApplicationMessages.TransactionTypeNotFoundMessage));
                    }

                    return Results.Ok(ResponseHelper.CreateSuccessResponse(type, ApiAction.Read));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GetTransactionTypeById")
            .WithTags("TransactionTypes")
            .RequireAuthorization();

            app.MapPut("/transaction-types/{id}", [Authorize] async (
             int id,
             TransactionType transactionType,
             IMediator mediator,
             IValidator<TransactionType> validator,
             HttpContext context,
             ILogger<Program> logger) =>
                    {
                        try
                        {
                            if (id != transactionType.Id)
                            {
                                return Results.BadRequest(ResponseHelper.CreateCustomErrorResponse<TransactionType>(
                                    ApplicationMessages.IdMismatchMessage, ApiAction.Updated));
                            }

                            var validationResult = validator.Validate(transactionType);
                            if (!validationResult.IsValid)
                            {
                                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                                return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(
                                    ApiAction.Updated, $"Validation failed: {errors}"));
                            }

                            var existing = await mediator.Send(new GetTransactionTypeByIdQuery(id));
                            if (existing == null)
                            {
                                return Results.NotFound(ResponseHelper.CreateNotFoundResponse<TransactionType>(
                                    ApiAction.Updated, ApplicationMessages.TransactionTypeNotFoundMessage));
                            }

                            
                            existing.TypeName = transactionType.TypeName;

                            JwtHelper.InitializeUpdateMetadata(context, existing);
                            var updated = await mediator.Send(new UpdateTransactionTypeCommand(existing));

                            return Results.Ok(ResponseHelper.CreateSuccessResponse(updated, ApiAction.Updated));
                        }
                        catch (Exception ex)
                        {
                            return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Updated);
                        }
                    })
         .WithName("UpdateTransactionType")
         .WithTags("TransactionTypes")
         .RequireAuthorization();


            app.MapDelete("/transaction-types/{id}", async (int id, IMediator mediator, ILogger<Program> logger, HttpContext context) =>
            {
                try
                {
                    var success = await mediator.Send(new DeleteTransactionTypeCommand(id));
                    if (success)
                    {
                        return Results.Ok(ResponseHelper.CreateSuccessResponse(true, ApiAction.Deleted));
                    }
                    else
                    {
                        return Results.NotFound(ResponseHelper.CreateNotFoundResponse<bool>(ApiAction.Deleted, ApplicationMessages.TransactionTypeNotFoundMessage));
                    }
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Deleted);
                }
            })
            .WithName("DeleteTransactionType")
            .WithTags("TransactionTypes")
            .RequireAuthorization();
        }
    }
}

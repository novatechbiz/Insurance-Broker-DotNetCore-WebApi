using InsuraNova.Handlers;
using InsuraNova.Helpers;
using InsuraNova.Resources;
using Microsoft.AspNetCore.Authorization;


namespace InsuraNova.Endpoints
{
    public static class CurrencyEndpoints
    {
        public static void MapCurrencyEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/currencies", async (IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var currencies = await mediator.Send(new GetAllCurrenciesQuery());
                    return Results.Ok(ResponseHelper.CreateSuccessResponse(currencies, ApiAction.Read));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GetAllCurrencies")
            .WithTags("Currencies")
            .RequireAuthorization();

            app.MapGet("/currencies/{id}", async (int id, IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var currency = await mediator.Send(new GetCurrencyByIdQuery(id));
                    if (currency == null)
                        return Results.NotFound(ResponseHelper.CreateNotFoundResponse<Currency>(ApiAction.Read, ApplicationMessages.EntityNotFoundMessage));

                    return Results.Ok(ResponseHelper.CreateSuccessResponse(currency, ApiAction.Read));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GetCurrencyById")
            .WithTags("Currencies")
            .RequireAuthorization();

            app.MapPost("/currencies", [Authorize] async (Currency currency, IMediator mediator, IValidator<Currency> validator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var validationResult = validator.Validate(currency);
                    if (!validationResult.IsValid)
                    {
                        var errors = string.Join(", ", validationResult.Errors.Select(currencies => currencies.ErrorMessage));
                        return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(ApiAction.Created, $"Validation failed: {errors}"));
                    }

                    JwtHelper.InitializeEntityMetadata(context, currency);
                    var addedCurrency = await mediator.Send(new AddCurrencyCommand(currency));
                    return Results.Created($"/currencies/{addedCurrency.Id}", ResponseHelper.CreateSuccessResponse(addedCurrency, ApiAction.Created));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Created);
                }
            })
            .WithName("CreateCurrency")
            .WithTags("Currencies")
            .RequireAuthorization();

            app.MapPut("/currencies/{id}", [Authorize] async (
                 int id,
                 Currency currency,
                 IMediator mediator,
                 IValidator<Currency> validator,
                 HttpContext context,
                 ILogger<Program> logger) =>
                        {
                            try
                            {
                                if (id != currency.Id)
                                {
                                    return Results.BadRequest(ResponseHelper.CreateCustomErrorResponse<Currency>(
                                        ApplicationMessages.IdMismatchMessage, ApiAction.Updated));
                                }

                                var validationResult = validator.Validate(currency);
                                if (!validationResult.IsValid)
                                {
                                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                                    return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(
                                        ApiAction.Updated, $"Validation failed: {errors}"));
                                }

                                var existingCurrency = await mediator.Send(new GetCurrencyByIdQuery(id));
                                if (existingCurrency == null)
                                {
                                    return Results.NotFound(ResponseHelper.CreateNotFoundResponse<Currency>(
                                        ApiAction.Updated, ApplicationMessages.EntityNotFoundMessage));
                                }

                                
                                existingCurrency.CurrencyCode = currency.CurrencyCode;
                                existingCurrency.CurrencyName = currency.CurrencyName;
                    

                                JwtHelper.InitializeUpdateMetadata(context, existingCurrency);

                                var updated = await mediator.Send(new UpdateCurrencyCommand(existingCurrency));
                                return Results.Ok(ResponseHelper.CreateSuccessResponse(updated, ApiAction.Updated));
                            }
                            catch (Exception ex)
                            {
                                return ErrorHelper.HandleExceptionWithLogging<object>(
                                    logger, ex, context, ApiAction.Updated);
                            }
                        })
             .WithName("UpdateCurrency")
             .WithTags("Currencies")
             .RequireAuthorization();


            app.MapDelete("/currencies/{id}", async (int id, IMediator mediator, ILogger<Program> logger, HttpContext context) =>
            {
                try
                {
                    var success = await mediator.Send(new DeleteCurrencyCommand(id));
                    if (success)
                        return Results.Ok(ResponseHelper.CreateSuccessResponse(true, ApiAction.Deleted));

                    return Results.NotFound(ResponseHelper.CreateNotFoundResponse<bool>(ApiAction.Deleted, ApplicationMessages.EntityNotFoundMessage));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Deleted);
                }
            })
            .WithName("DeleteCurrency")
            .WithTags("Currencies")
            .RequireAuthorization();
        }
    }
}

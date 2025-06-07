using InsuraNova.Handlers;
using InsuraNova.Helpers;
using InsuraNova.Models;
using InsuraNova.Resources;
using Microsoft.AspNetCore.Authorization;

namespace InsuraNova.Endpoints
{
    public static class PremiumLineEndpoints
    {
        public static void MapPremiumLineEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/premium-lines", async (IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var result = await mediator.Send(new GetAllPremiumLinesQuery());
                    return Results.Ok(ResponseHelper.CreateSuccessResponse(result, ApiAction.Read));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GetAllPremiumLines")
            .WithTags("PremiumLines")
            .RequireAuthorization();

            app.MapGet("/premium-lines/{id}", async (int id, IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var result = await mediator.Send(new GetPremiumLineByIdQuery(id));
                    if (result != null)
                    {
                        return Results.Ok(ResponseHelper.CreateSuccessResponse(result, ApiAction.Read));
                    }

                    return Results.NotFound(ResponseHelper.CreateNotFoundResponse<PremiumLine>(ApiAction.Read, ApplicationMessages.PremiumLineNotFoundMessage));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GetPremiumLineById")
            .WithTags("PremiumLines")
            .RequireAuthorization();

            app.MapPost("/premium-lines", [Authorize] async (PremiumLine premiumLine, IMediator mediator, IValidator<PremiumLine> validator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var validationResult = validator.Validate(premiumLine);
                    if (!validationResult.IsValid)
                    {
                        return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(
                            ApiAction.Created,
                            "Validation failed: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))
                        ));
                    }

                    JwtHelper.InitializeEntityMetadata(context, premiumLine);
                    var result = await mediator.Send(new AddPremiumLineCommand(premiumLine));
                    return Results.Created($"/premium-lines/{result.Id}", ResponseHelper.CreateSuccessResponse(result, ApiAction.Created));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Created);
                }
            })
            .WithName("CreatePremiumLine")
            .WithTags("PremiumLines")
            .RequireAuthorization();

            app.MapPut("/premium-lines/{id}", [Authorize] async (int id, PremiumLine premiumLine, IMediator mediator, IValidator<PremiumLine> validator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var validation = validator.Validate(premiumLine);
                    if (!validation.IsValid)
                    {
                        return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(ApiAction.Updated,
                            "Validation failed: " + string.Join(", ", validation.Errors.Select(e => e.ErrorMessage))));
                    }

                    if (id != premiumLine.Id)
                        return Results.BadRequest(ResponseHelper.CreateCustomErrorResponse<PremiumLine>(ApplicationMessages.IdMismatchMessage, ApiAction.Updated));

                    var existing = await mediator.Send(new GetPremiumLineByIdQuery(id));
                    if (existing == null)
                        return Results.NotFound(ResponseHelper.CreateNotFoundResponse<PremiumLine>(ApiAction.Updated, ApplicationMessages.PremiumLineNotFoundMessage));

                    // Update properties
                    existing.LineName = premiumLine.LineName;
                    existing.Sign = premiumLine.Sign;
                   

                    JwtHelper.InitializeUpdateMetadata(context, existing);

                    var updated = await mediator.Send(new UpdatePremiumLineCommand(existing));
                    return Results.Ok(ResponseHelper.CreateSuccessResponse(updated, ApiAction.Updated));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Updated);
                }
            })
 .WithName("UpdatePremiumLine")
 .WithTags("PremiumLines")
 .RequireAuthorization();

            app.MapDelete("/premium-lines/{id}", [Authorize] async (int id, IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var success = await mediator.Send(new DeletePremiumLineCommand(id));
                    if (success)
                    {
                        return Results.Ok(ResponseHelper.CreateSuccessResponse(true, ApiAction.Deleted));
                    }

                    return Results.NotFound(ResponseHelper.CreateNotFoundResponse<bool>(ApiAction.Deleted, ApplicationMessages.PremiumLineNotFoundMessage));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Deleted);
                }
            })
            .WithName("DeletePremiumLine")
            .WithTags("PremiumLines")
            .RequireAuthorization();
        }
    }
}

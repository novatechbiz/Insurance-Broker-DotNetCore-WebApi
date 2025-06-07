using InsuraNova.Handlers;
using InsuraNova.Helpers;
using InsuraNova.Models;
using InsuraNova.Resources;
using Microsoft.AspNetCore.Authorization;

namespace InsuraNova.Endpoints
{
    public static class CustomerIdentificationTypeEndpoints
    {
        public static void MapCustomerIdentificationTypeEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/customer-identification-types", async (IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var types = await mediator.Send(new GetAllCustomerIdentificationTypesQuery());
                    return Results.Ok(ResponseHelper.CreateSuccessResponse(types, ApiAction.Read));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GetAllCustomerIdentificationTypes")
            .WithTags("CustomerIdentificationTypes")
            .RequireAuthorization();

            app.MapGet("/customer-identification-types/{id}", async (int id, IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var type = await mediator.Send(new GetCustomerIdentificationTypeByIdQuery(id));
                    if (type != null)
                        return Results.Ok(ResponseHelper.CreateSuccessResponse(type, ApiAction.Read));
                    else
                        return Results.NotFound(ResponseHelper.CreateNotFoundResponse<CustomerIdentificationType>(ApiAction.Read, ApplicationMessages.NotFoundMessage));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GetCustomerIdentificationTypeById")
            .WithTags("CustomerIdentificationTypes")
            .RequireAuthorization();

            app.MapPost("/customer-identification-types", [Authorize] async (CustomerIdentificationType identificationType, IMediator mediator, IValidator<CustomerIdentificationType> validator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var validation = validator.Validate(identificationType);
                    if (!validation.IsValid)
                    {
                        return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(ApiAction.Created,
                            "Validation failed: " + string.Join(", ", validation.Errors.Select(e => e.ErrorMessage))));
                    }

                    JwtHelper.InitializeEntityMetadata(context, identificationType);
                    var created = await mediator.Send(new AddCustomerIdentificationTypeCommand(identificationType));
                    return Results.Created($"/customer-identification-types/{created.Id}", ResponseHelper.CreateSuccessResponse(created, ApiAction.Created));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Created);
                }
            })
            .WithName("CreateCustomerIdentificationType")
            .WithTags("CustomerIdentificationTypes")
            .RequireAuthorization();

            app.MapPut("/customer-identification-types/{id}", [Authorize] async (int id, CustomerIdentificationType identificationType, IMediator mediator, IValidator<CustomerIdentificationType> validator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var validation = validator.Validate(identificationType);
                    if (!validation.IsValid)
                    {
                        return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(ApiAction.Updated,
                            "Validation failed: " + string.Join(", ", validation.Errors.Select(e => e.ErrorMessage))));
                    }

                    if (id != identificationType.Id)
                        return Results.BadRequest(ResponseHelper.CreateCustomErrorResponse<CustomerIdentificationType>(ApplicationMessages.IdMismatchMessage, ApiAction.Updated));

                    var existing = await mediator.Send(new GetCustomerIdentificationTypeByIdQuery(id));
                    if (existing == null)
                        return Results.NotFound(ResponseHelper.CreateNotFoundResponse<CustomerIdentificationType>(ApiAction.Updated, ApplicationMessages.NotFoundMessage));

                    // Update properties
                    existing.IdentificationType = identificationType.IdentificationType;
                    JwtHelper.InitializeUpdateMetadata(context, existing);

                    var updated = await mediator.Send(new UpdateCustomerIdentificationTypeCommand(existing));
                    return Results.Ok(ResponseHelper.CreateSuccessResponse(updated, ApiAction.Updated));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Updated);
                }
            })
            .WithName("UpdateCustomerIdentificationType")
            .WithTags("CustomerIdentificationTypes")
            .RequireAuthorization();

            app.MapDelete("/customer-identification-types/{id}", async (int id, IMediator mediator, ILogger<Program> logger, HttpContext context) =>
            {
                try
                {
                    var success = await mediator.Send(new DeleteCustomerIdentificationTypeCommand(id));
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
            .WithName("DeleteCustomerIdentificationType")
            .WithTags("CustomerIdentificationTypes")
            .RequireAuthorization();
        }
    }
}

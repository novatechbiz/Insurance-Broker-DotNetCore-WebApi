using InsuraNova.Handlers;
using InsuraNova.Helpers;
using InsuraNova.Models;
using InsuraNova.Resources;
using Microsoft.AspNetCore.Authorization;

namespace InsuraNova.Endpoints
{
    public static class CustomerTypeEndpoints
    {
        public static void MapCustomerTypeEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/customer-types", async (IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var types = await mediator.Send(new GetAllCustomerTypesQuery());
                    return Results.Ok(ResponseHelper.CreateSuccessResponse(types, ApiAction.Read));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GetAllCustomerTypes")
            .WithTags("CustomerTypes")
            .RequireAuthorization();

            app.MapGet("/customer-types/{id}", async (int id, IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var type = await mediator.Send(new GetCustomerTypeByIdQuery(id));
                    if (type != null)
                        return Results.Ok(ResponseHelper.CreateSuccessResponse(type, ApiAction.Read));
                    else
                        return Results.NotFound(ResponseHelper.CreateNotFoundResponse<CustomerType>(ApiAction.Read, ApplicationMessages.NotFoundMessage));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GetCustomerTypeById")
            .WithTags("CustomerTypes")
            .RequireAuthorization();

            app.MapPost("/customer-types", [Authorize] async (CustomerType customerType, IMediator mediator, IValidator<CustomerType> validator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var validation = validator.Validate(customerType);
                    if (!validation.IsValid)
                    {
                        return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(ApiAction.Created,
                            "Validation failed: " + string.Join(", ", validation.Errors.Select(e => e.ErrorMessage))));
                    }

                    JwtHelper.InitializeEntityMetadata(context, customerType);
                    var created = await mediator.Send(new AddCustomerTypeCommand(customerType));
                    return Results.Created($"/customer-types/{created.Id}", ResponseHelper.CreateSuccessResponse(created, ApiAction.Created));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Created);
                }
            })
            .WithName("CreateCustomerType")
            .WithTags("CustomerTypes")
            .RequireAuthorization();

            app.MapPut("/customer-types/{id}", [Authorize] async (int id, CustomerType customerType, IMediator mediator, IValidator<CustomerType> validator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var validation = validator.Validate(customerType);
                    if (!validation.IsValid)
                    {
                        return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(ApiAction.Updated,
                            "Validation failed: " + string.Join(", ", validation.Errors.Select(e => e.ErrorMessage))));
                    }

                    if (id != customerType.Id)
                        return Results.BadRequest(ResponseHelper.CreateCustomErrorResponse<CustomerType>(ApplicationMessages.IdMismatchMessage, ApiAction.Updated));

                    var existing = await mediator.Send(new GetCustomerTypeByIdQuery(id));
                    if (existing == null)
                        return Results.NotFound(ResponseHelper.CreateNotFoundResponse<CustomerType>(ApiAction.Updated, ApplicationMessages.NotFoundMessage));

                    // Optionally update fields here
                    existing.TypeName = customerType.TypeName;
                    existing.Alias = customerType.Alias;
                    JwtHelper.InitializeUpdateMetadata(context, existing);

                    var updated = await mediator.Send(new UpdateCustomerTypeCommand(existing));
                    return Results.Ok(ResponseHelper.CreateSuccessResponse(updated, ApiAction.Updated));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Updated);
                }
            })
            .WithName("UpdateCustomerType")
            .WithTags("CustomerTypes")
            .RequireAuthorization();

            app.MapDelete("/customer-types/{id}", async (int id, IMediator mediator, ILogger<Program> logger, HttpContext context) =>
            {
                try
                {
                    var success = await mediator.Send(new DeleteCustomerTypeCommand(id));
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
            .WithName("DeleteCustomerType")
            .WithTags("CustomerTypes")
            .RequireAuthorization();
        }
    }
}

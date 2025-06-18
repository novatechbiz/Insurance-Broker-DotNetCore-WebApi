using InsuraNova.Handlers;
using InsuraNova.Helpers;
using InsuraNova.Models;
using InsuraNova.Resources;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;

namespace InsuraNova.Endpoints
{
    public static class CustomerEndpoints
    {
        public static void MapCustomerEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/customers", async (IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var customers = await mediator.Send(new GetAllCustomersQuery());
                    return Results.Ok(ResponseHelper.CreateSuccessResponse(customers, ApiAction.Read));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GetAllCustomers")
            .WithTags("Customers")
            .RequireAuthorization();

            app.MapGet("/customers/{id}", async (int id, IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var customer = await mediator.Send(new GetCustomerByIdQuery(id));
                    if (customer == null)
                        return Results.NotFound(ResponseHelper.CreateNotFoundResponse<Customer>(ApiAction.Read, ApplicationMessages.EntityNotFoundMessage));

                    return Results.Ok(ResponseHelper.CreateSuccessResponse(customer, ApiAction.Read));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GetCustomerById")
            .WithTags("Customers")
            .RequireAuthorization();

            app.MapPost("/customers", [Authorize] async (Customer customer, IMediator mediator, IValidator<Customer> validator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var validationResult = validator.Validate(customer);
                    if (!validationResult.IsValid)
                    {
                        var errors = string.Join(", ", validationResult.Errors.Select(customer => customer.ErrorMessage));
                        return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(ApiAction.Created, $"Validation failed: {errors}"));
                    }

                    JwtHelper.InitializeEntityMetadata(context, customer);
                    var addedCustomer = await mediator.Send(new AddCustomerCommand(customer));
                    return Results.Created($"/customers/{addedCustomer.Id}", ResponseHelper.CreateSuccessResponse(addedCustomer, ApiAction.Created));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Created);
                }
            })
            .WithName("CreateCustomer")
            .WithTags("Customers")
            .RequireAuthorization();

            app.MapPut("/customers/{id}", [Authorize] async (
                int id,
                Customer customer,
                IMediator mediator,
                IValidator<Customer> validator,
                HttpContext context,
                ILogger<Program> logger) =>
            {
                try
                {
                    if (id != customer.Id)
                    {
                        return Results.BadRequest(ResponseHelper.CreateCustomErrorResponse<Customer>(ApplicationMessages.IdMismatchMessage, ApiAction.Updated));
                    }

                    var validationResult = validator.Validate(customer);
                    if (!validationResult.IsValid)
                    {
                        var errors = string.Join(", ", validationResult.Errors.Select(customer => customer.ErrorMessage));
                        return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(ApiAction.Updated, $"Validation failed: {errors}"));
                    }

                    var existingCustomer = await mediator.Send(new GetCustomerByIdQuery(id));
                    if (existingCustomer == null)
                    {
                        return Results.NotFound(ResponseHelper.CreateNotFoundResponse<Customer>(ApiAction.Updated, ApplicationMessages.EntityNotFoundMessage));
                    }


                    existingCustomer.CompanyId = customer.CompanyId;
                    existingCustomer.CustomerIdentificationTypeId = customer.CustomerIdentificationTypeId;
                    existingCustomer.CustomerTypeId = customer.CustomerTypeId;
                    existingCustomer.CustomerName = customer.CustomerName;
                    existingCustomer.IdentificationNo = customer.IdentificationNo;
                    existingCustomer.FullName = customer.FullName;
                    existingCustomer.ContactNo = customer.ContactNo;
                    existingCustomer.WhatsAppNo = customer.WhatsAppNo;
                    existingCustomer.EmailAddress = customer.EmailAddress;
                    existingCustomer.GenderTypeId = customer.GenderTypeId;
                    existingCustomer.DateOfBirth = customer.DateOfBirth;
                    existingCustomer.RecordStatusId = customer.RecordStatusId;

                    JwtHelper.InitializeUpdateMetadata(context, existingCustomer);

                    var updatedCustomer = await mediator.Send(new UpdateCustomerCommand(existingCustomer));
                    return Results.Ok(ResponseHelper.CreateSuccessResponse(updatedCustomer, ApiAction.Updated));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Updated);
                }
            })
            .WithName("UpdateCustomer")
            .WithTags("Customers")
            .RequireAuthorization();

            app.MapDelete("/customers/{id}", async (int id, IMediator mediator, ILogger<Program> logger, HttpContext context) =>
            {
                try
                {
                    var success = await mediator.Send(new DeleteCustomerCommand(id));
                    if (success)
                        return Results.Ok(ResponseHelper.CreateSuccessResponse(true, "Delete"));


                    return Results.NotFound(ResponseHelper.CreateNotFoundResponse<bool>(ApiAction.Deleted, ApplicationMessages.EntityNotFoundMessage));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Deleted);
                }
            })
.WithName("DeleteCustomer")
.WithTags("Customers")
.RequireAuthorization();
        }
    }
}
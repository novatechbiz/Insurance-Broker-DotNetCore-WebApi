using System.Runtime.CompilerServices;
using InsuraNova.Handlers;
using InsuraNova.Helpers;
using InsuraNova.Models;
using InsuraNova.Resources;
using Microsoft.AspNetCore.Authorization;

namespace InsuraNova.Endpoints
{
    public static class CustomerEndpoints
    {
        public static void MapCustomerEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/customers", [Authorize] async (int? page, int? pageSize, string? filter, string? sortBy, string? sortOrder, IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {

                    var customers = await mediator.Send(new GetAllCustomersQuery());

                    // Filter by name if filter is provided
                    if (!string.IsNullOrEmpty(filter))
                    {
                        customers = customers.Where(customers => customers.CustomerName != null && customers.CustomerName.Contains(filter, StringComparison.OrdinalIgnoreCase)).ToList();
                    }

                    // Sort by Id or Name only (simple version)
                    if (!string.IsNullOrEmpty(sortBy))
                    {
                        if (sortBy.ToLower() == "name")
                        {
                            customers = sortOrder?.ToLower() == "desc"
                                ? customers.OrderByDescending(customers => customers.CustomerName).ToList()
                                : customers.OrderBy(customers => customers.CustomerName).ToList();
                        }

                        else
                        {
                            // Default sorting by Id
                            customers = sortOrder?.ToLower() == "desc"
                                ? customers.OrderByDescending(c => c.Id).ToList()
                                : customers.OrderBy(c => c.Id).ToList();
                        }
                    }
                    else
                    {
                        // Default sorting by Id ascending
                        customers = customers.OrderBy(c => c.Id).ToList();
                    }

                    // Pagination
                    int currentPage = page ?? 1;
                    int currentPageSize = pageSize ?? 10;

                    var pagedCustomers = customers
                        .Skip((currentPage - 1) * currentPageSize)
                        .Take(currentPageSize)
                        .ToList();

                    return Results.Ok(ResponseHelper.CreateSuccessResponse(pagedCustomers, ApiAction.Read));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GetAllCustomers")
            .WithTags("Customers")
            .RequireAuthorization();

            app.MapGet("/customers/{id}", [Authorize] async (int id, IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {

                try
                {
                    var gender = await mediator.Send(new GetCustomerByIdQuery(id));
                    if (gender != null)
                        return Results.Ok(ResponseHelper.CreateSuccessResponse(gender, ApiAction.Read));
                    else
                        return Results.NotFound(ResponseHelper.CreateNotFoundResponse<Customer>(ApiAction.Read, ApplicationMessages.NotFoundMessage));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GetCustomerById")
            .WithTags("Customers")
            .RequireAuthorization();

            app.MapPost("/customers", [Authorize] async (Customer customer, string customerType, IMediator mediator, IValidator<Customer> validator, HttpContext context, ILogger<Program> logger) =>
            {

                try
                {
                    if (!string.IsNullOrEmpty(customerType))
                    {
                        switch (customerType.ToLower())
                        {
                            case "personal":
                                customer.CustomerTypeId = 1;
                                break;
                            case "corporate":
                                customer.CustomerTypeId = 2;
                                break;
                            default:
                                return Results.BadRequest($"Invalid customer type: '{customerType}'. Use 'Personal' or 'Corporate'.");
                        }
                    }
                    else
                    {
                        return Results.BadRequest("CustomerType is required.");
                    }

                    var validation = validator.Validate(customer);
                    if (!validation.IsValid)
                    {
                        return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(ApiAction.Created,
                            "Validation failed: " + string.Join(", ", validation.Errors.Select(e => e.ErrorMessage))));
                    }

                    JwtHelper.InitializeEntityMetadata(context, customer);
                    var created = await mediator.Send(new AddCustomerCommand(customer));
                    return Results.Created($"/customers/{created.Id}", ResponseHelper.CreateSuccessResponse(created, ApiAction.Created));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Created);
                }
            })
            .WithName("CreateCustomer")
            .WithTags("Customers")
            .RequireAuthorization();

            app.MapPut("/customers/{id}", [Authorize] async (int id, Customer customer, IMediator mediator, IValidator<Customer> validator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var validation = validator.Validate(customer);
                    if (!validation.IsValid)
                    {
                        return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(ApiAction.Updated,
                            "Validation failed: " + string.Join(", ", validation.Errors.Select(e => e.ErrorMessage))));
                    }

                    if (id != customer.Id)
                        return Results.BadRequest(ResponseHelper.CreateCustomErrorResponse<Customer>(ApplicationMessages.IdMismatchMessage, ApiAction.Updated));

                    var existing = await mediator.Send(new GetCustomerByIdQuery(id));
                    if (existing == null)
                        return Results.NotFound(ResponseHelper.CreateNotFoundResponse<Customer>(ApiAction.Updated, ApplicationMessages.NotFoundMessage));

                    JwtHelper.InitializeUpdateMetadata(context, customer);
                    var updated = await mediator.Send(new UpdateCustomerCommand(customer));
                    return Results.Ok(ResponseHelper.CreateSuccessResponse(updated, ApiAction.Updated));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Updated);
                }
            })
            .WithName("UpdateCustomer")
            .WithTags("Customers")
            .RequireAuthorization();

            app.MapDelete("/customers/{id}", [Authorize] async (int id, IMediator mediator, ILogger<Program> logger, HttpContext context) =>
            {
                try
                {
                    var success = await mediator.Send(new DeleteCustomerCommand(id));
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
            .WithName("DeleteCustomer")
            .WithTags("Customers")
            .RequireAuthorization();
        }
    }
}

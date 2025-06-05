using InsuraNova.Handlers;
using InsuraNova.Helpers;
using InsuraNova.Models;
using InsuraNova.Resources;
using Microsoft.AspNetCore.Authorization;

namespace InsuraNova.Endpoints
{
    public static class InsuranceCompanyEndpoints
    {
        public static void MapInsuranceCompanyEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/insurance-companies", async (IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var result = await mediator.Send(new GetAllInsuranceCompaniesQuery());
                    return Results.Ok(ResponseHelper.CreateSuccessResponse(result, ApiAction.Read));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GetAllInsuranceCompanies")
            .WithTags("InsuranceCompanies")
            .RequireAuthorization();

            app.MapGet("/insurance-companies/{id}", async (int id, IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var result = await mediator.Send(new GetInsuranceCompanyByIdQuery(id));
                    if (result != null)
                    {
                        return Results.Ok(ResponseHelper.CreateSuccessResponse(result, ApiAction.Read));
                    }
                    return Results.NotFound(ResponseHelper.CreateNotFoundResponse<InsuranceCompany>(ApiAction.Read, ApplicationMessages.InsuranceCompanyNotFoundMessage));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GetInsuranceCompanyById")
            .WithTags("InsuranceCompanies")
            .RequireAuthorization();

            app.MapPost("/insurance-companies", [Authorize] async (InsuranceCompany insuranceCompany, IMediator mediator, IValidator<InsuranceCompany> validator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var validationResult = validator.Validate(insuranceCompany);
                    if (!validationResult.IsValid)
                    {
                        return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(
                            ApiAction.Created,
                            "Validation failed: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))
                        ));
                    }

                    JwtHelper.InitializeEntityMetadata(context, insuranceCompany);
                    var result = await mediator.Send(new AddInsuranceCompanyCommand(insuranceCompany));
                    return Results.Created($"/insurance-companies/{result.Id}", ResponseHelper.CreateSuccessResponse(result, ApiAction.Created));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Created);
                }
            })
            .WithName("CreateInsuranceCompany")
            .WithTags("InsuranceCompanies")
            .RequireAuthorization();

            app.MapPut("/insurance-companies/{id}", [Authorize] async (
            int id,
            InsuranceCompany insuranceCompany,
            IMediator mediator,
            IValidator<InsuranceCompany> validator,
            HttpContext context,
            ILogger<Program> logger) =>
                {
                    try
                    {
                        var validation = validator.Validate(insuranceCompany);
                        if (!validation.IsValid)
                        {
                            return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(
                                ApiAction.Updated,
                                "Validation failed: " + string.Join(", ", validation.Errors.Select(e => e.ErrorMessage))));
                        }

                        if (id != insuranceCompany.Id)
                        {
                            return Results.BadRequest(ResponseHelper.CreateCustomErrorResponse<InsuranceCompany>(
                                ApplicationMessages.IdMismatchMessage,
                                ApiAction.Updated));
                        }

                        var existing = await mediator.Send(new GetInsuranceCompanyByIdQuery(id));
                        if (existing == null)
                        {
                            return Results.NotFound(ResponseHelper.CreateNotFoundResponse<InsuranceCompany>(
                                ApiAction.Updated,
                                ApplicationMessages.InsuranceCompanyNotFoundMessage));
                        }

                        // Update properties
                        existing.CompanyName = insuranceCompany.CompanyName;
                    

                        JwtHelper.InitializeUpdateMetadata(context, existing);

                        var updated = await mediator.Send(new UpdateInsuranceCompanyCommand(existing));
                        return Results.Ok(ResponseHelper.CreateSuccessResponse(updated, ApiAction.Updated));
                    }
                    catch (Exception ex)
                    {
                        return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Updated);
                    }
                })
        .WithName("UpdateInsuranceCompany")
        .WithTags("InsuranceCompanies")
        .RequireAuthorization();


            app.MapDelete("/insurance-companies/{id}", [Authorize] async (int id, IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var success = await mediator.Send(new DeleteInsuranceCompanyCommand(id));
                    if (success)
                    {
                        return Results.Ok(ResponseHelper.CreateSuccessResponse(true, ApiAction.Deleted));
                    }

                    return Results.NotFound(ResponseHelper.CreateNotFoundResponse<bool>(ApiAction.Deleted, ApplicationMessages.InsuranceCompanyNotFoundMessage));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Deleted);
                }
            })
            .WithName("DeleteInsuranceCompany")
            .WithTags("InsuranceCompanies")
            .RequireAuthorization();
        }
    }
}

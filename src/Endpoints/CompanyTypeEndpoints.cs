using InsuraNova.Handlers;
using InsuraNova.Helpers;
using InsuraNova.Models;
using InsuraNova.Resources;
using Microsoft.AspNetCore.Authorization;

namespace InsuraNova.Endpoints
{
    public static class CompanyTypeEndpoints
    {
        public static void MapCompanyTypeEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/company-types", async (IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var result = await mediator.Send(new GetAllCompanyTypesQuery());
                    return Results.Ok(ResponseHelper.CreateSuccessResponse(result, ApiAction.Read));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GetAllCompanyTypes")
            .WithTags("CompanyTypes")
            .AllowAnonymous();
            //.RequireAuthorization();

            app.MapGet("/company-types/{id}", async (int id, IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var result = await mediator.Send(new GetCompanyTypeByIdQuery(id));
                    if (result != null)
                    {
                        return Results.Ok(ResponseHelper.CreateSuccessResponse(result, ApiAction.Read));
                    }
                    return Results.NotFound(ResponseHelper.CreateNotFoundResponse<CompanyType>(ApiAction.Read, ApplicationMessages.CompanyTypeNotFoundMessage));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GetCompanyTypeById")
            .WithTags("CompanyTypes");
            //.RequireAuthorization();

            app.MapPost("/company-types", [Authorize] async (CompanyType companyType, IMediator mediator, IValidator<CompanyType> validator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var validationResult = validator.Validate(companyType);
                    if (!validationResult.IsValid)
                    {
                        return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(
                            ApiAction.Created,
                            "Validation failed: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))
                        ));
                    }

                    JwtHelper.InitializeEntityMetadata(context, companyType);
                    var result = await mediator.Send(new AddCompanyTypeCommand(companyType));
                    return Results.Created($"/company-types/{result.Id}", ResponseHelper.CreateSuccessResponse(result, ApiAction.Created));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Created);
                }
            })
            .WithName("CreateCompanyType")
            .WithTags("CompanyTypes")
            .RequireAuthorization();

            app.MapPut("/company-types/{id}", [Authorize] async (
     int id,
     CompanyType companyType,
     IMediator mediator,
     IValidator<CompanyType> validator,
     HttpContext context,
     ILogger<Program> logger) =>
            {
                try
                {
                    var validation = validator.Validate(companyType);
                    if (!validation.IsValid)
                    {
                        return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(
                            ApiAction.Updated,
                            "Validation failed: " + string.Join(", ", validation.Errors.Select(e => e.ErrorMessage))));
                    }

                    if (id != companyType.Id)
                    {
                        return Results.BadRequest(ResponseHelper.CreateCustomErrorResponse<CompanyType>(
                            ApplicationMessages.IdMismatchMessage,
                            ApiAction.Updated));
                    }

                    var existing = await mediator.Send(new GetCompanyTypeByIdQuery(id));
                    if (existing == null)
                    {
                        return Results.NotFound(ResponseHelper.CreateNotFoundResponse<CompanyType>(
                            ApiAction.Updated,
                            ApplicationMessages.CompanyTypeNotFoundMessage));
                    }

                    // Update properties
                    existing.Name = companyType.Name;

                    JwtHelper.InitializeUpdateMetadata(context, existing);

                    var updated = await mediator.Send(new UpdateCompanyTypeCommand(existing));
                    return Results.Ok(ResponseHelper.CreateSuccessResponse(updated, ApiAction.Updated));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Updated);
                }
            })
 .WithName("UpdateCompanyType")
 .WithTags("CompanyTypes")
 .RequireAuthorization();


            app.MapDelete("/company-types/{id}", [Authorize] async (int id, IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var success = await mediator.Send(new DeleteCompanyTypeCommand(id));
                    if (success)
                    {
                        return Results.Ok(ResponseHelper.CreateSuccessResponse(true, ApiAction.Deleted));
                    }

                    return Results.NotFound(ResponseHelper.CreateNotFoundResponse<bool>(ApiAction.Deleted, ApplicationMessages.CompanyTypeNotFoundMessage));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Deleted);
                }
            })
            .WithName("DeleteCompanyType")
            .WithTags("CompanyTypes");
            //.RequireAuthorization();
        }
    }
}

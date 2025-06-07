using InsuraNova.Handlers;
using InsuraNova.Helpers;
using InsuraNova.Resources;
using Microsoft.AspNetCore.Authorization;


namespace InsuraNova.Endpoints
{
    public static class InsuranceTypeEndpoints
    {
        public static void MapInsuranceTypeEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/insurance-types", async (IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var types = await mediator.Send(new GetAllInsuranceTypesQuery());
                    return Results.Ok(ResponseHelper.CreateSuccessResponse(types, ApiAction.Read));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GetAllInsuranceTypes")
            .WithTags("InsuranceTypes")
            .RequireAuthorization();

            app.MapGet("/insurance-types/{id}", async (int id, IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var type = await mediator.Send(new GetInsuranceTypeByIdQuery(id));
                    if (type != null)
                        return Results.Ok(ResponseHelper.CreateSuccessResponse(type, ApiAction.Read));
                    else
                        return Results.NotFound(ResponseHelper.CreateNotFoundResponse<InsuranceType>(ApiAction.Read, ApplicationMessages.NotFoundMessage));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GetInsuranceTypeById")
            .WithTags("InsuranceTypes")
            .RequireAuthorization();

            app.MapPost("/insurance-types", [Authorize] async (InsuranceType insuranceType, IMediator mediator, IValidator<InsuranceType> validator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var validation = validator.Validate(insuranceType);
                    if (!validation.IsValid)
                    {
                        return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(ApiAction.Created,
                            "Validation failed: " + string.Join(", ", validation.Errors.Select(insuranceType => insuranceType.ErrorMessage))));
                    }

                    JwtHelper.InitializeEntityMetadata(context, insuranceType);
                    var created = await mediator.Send(new AddInsuranceTypeCommand(insuranceType));
                    return Results.Created($"/insurance-types/{created.Id}", ResponseHelper.CreateSuccessResponse(created, ApiAction.Created));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Created);
                }
            })
            .WithName("CreateInsuranceType")
            .WithTags("InsuranceTypes")
            .RequireAuthorization();

            app.MapPut("/insurance-types/{id}", [Authorize] async (
                int id,
                InsuranceType insuranceType,
                IMediator mediator,
                IValidator<InsuranceType> validator,
                HttpContext context,
                ILogger<Program> logger) =>
                        {
                            try
                            {
                                var validationResult = validator.Validate(insuranceType);
                                if (!validationResult.IsValid)
                                {
                                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                                    return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(
                                        ApiAction.Updated, $"Validation failed: {errors}"));
                                }

                                if (id != insuranceType.Id)
                                {
                                    return Results.BadRequest(ResponseHelper.CreateCustomErrorResponse<InsuranceType>(
                                        ApplicationMessages.IdMismatchMessage, ApiAction.Updated));
                                }

                                var existing = await mediator.Send(new GetInsuranceTypeByIdQuery(id));
                                if (existing == null)
                                {
                                    return Results.NotFound(ResponseHelper.CreateNotFoundResponse<InsuranceType>(
                                        ApiAction.Updated, ApplicationMessages.NotFoundMessage));
                                }

                                // Update the property manually
                                existing.TypeName = insuranceType.TypeName;

                                JwtHelper.InitializeUpdateMetadata(context, existing);
                                var updated = await mediator.Send(new UpdateInsuranceTypeCommand(existing));

                                return Results.Ok(ResponseHelper.CreateSuccessResponse(updated, ApiAction.Updated));
                            }
                            catch (Exception ex)
                            {
                                return ErrorHelper.HandleExceptionWithLogging<object>(
                                    logger, ex, context, ApiAction.Updated);
                            }
                        })
            .WithName("UpdateInsuranceType")
            .WithTags("InsuranceTypes")
            .RequireAuthorization();


            app.MapDelete("/insurance-types/{id}", async (int id, IMediator mediator, ILogger<Program> logger, HttpContext context) =>
            {
                try
                {
                    var success = await mediator.Send(new DeleteInsuranceTypeCommand(id));
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
            .WithName("DeleteInsuranceType")
            .WithTags("InsuranceTypes")
            .RequireAuthorization();
        }
    }
}

using InsuraNova.Handlers;
using InsuraNova.Helpers;
using InsuraNova.Models;
using InsuraNova.Resources;
using Microsoft.AspNetCore.Authorization;

namespace InsuraNova.Endpoints
{
    public static class SystemFunctionEndpoints
    {
        public static void MapSystemFunctionEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/system-functions", async (IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var result = await mediator.Send(new GetAllSystemFunctionsQuery());
                    return Results.Ok(ResponseHelper.CreateSuccessResponse(result, ApiAction.Read));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GetAllSystemFunctions")
            .WithTags("SystemFunctions")
            .RequireAuthorization();

            app.MapGet("/system-functions/{id}", async (int id, IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var result = await mediator.Send(new GetSystemFunctionByIdQuery(id));
                    if (result != null)
                    {
                        return Results.Ok(ResponseHelper.CreateSuccessResponse(result, ApiAction.Read));
                    }
                    return Results.NotFound(ResponseHelper.CreateNotFoundResponse<SystemFunction>(ApiAction.Read, ApplicationMessages.SystemFunctionNotFoundMessage));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GetSystemFunctionById")
            .WithTags("SystemFunctions")
            .RequireAuthorization();

            app.MapPost("/system-functions", [Authorize] async (SystemFunction systemFunction, IMediator mediator, IValidator<SystemFunction> validator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var validationResult = validator.Validate(systemFunction);
                    if (!validationResult.IsValid)
                    {
                        return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(
                            ApiAction.Created,
                            "Validation failed: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))
                        ));
                    }

                    JwtHelper.InitializeEntityMetadata(context, systemFunction);
                    var result = await mediator.Send(new AddSystemFunctionCommand(systemFunction));
                    return Results.Created($"/system-functions/{result.Id}", ResponseHelper.CreateSuccessResponse(result, ApiAction.Created));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Created);
                }
            })
            .WithName("CreateSystemFunction")
            .WithTags("SystemFunctions")
            .RequireAuthorization();

            app.MapPut("/system-functions/{id}", [Authorize] async (int id, SystemFunction systemFunction, IMediator mediator, IValidator<SystemFunction> validator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var validation = validator.Validate(systemFunction);
                    if (!validation.IsValid)
                    {
                        return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(
                            ApiAction.Updated,
                            "Validation failed: " + string.Join(", ", validation.Errors.Select(e => e.ErrorMessage))
                        ));
                    }

                    if (id != systemFunction.Id)
                        return Results.BadRequest(ResponseHelper.CreateCustomErrorResponse<SystemFunction>(
                            ApplicationMessages.IdMismatchMessage, ApiAction.Updated));

                    var existing = await mediator.Send(new GetSystemFunctionByIdQuery(id));
                    if (existing == null)
                        return Results.NotFound(ResponseHelper.CreateNotFoundResponse<SystemFunction>(
                            ApiAction.Updated, ApplicationMessages.SystemFunctionNotFoundMessage));

                    // Update properties
                    existing.Name = systemFunction.Name;
                   
                    JwtHelper.InitializeUpdateMetadata(context, existing);

                    var updated = await mediator.Send(new UpdateSystemFunctionCommand(existing));
                    return Results.Ok(ResponseHelper.CreateSuccessResponse(updated, ApiAction.Updated));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Updated);
                }
            })
 .WithName("UpdateSystemFunction")
 .WithTags("SystemFunctions")
 .RequireAuthorization();


            app.MapDelete("/system-functions/{id}", [Authorize] async (int id, IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var success = await mediator.Send(new DeleteSystemFunctionCommand(id));
                    if (success)
                    {
                        return Results.Ok(ResponseHelper.CreateSuccessResponse(true, ApiAction.Deleted));
                    }

                    return Results.NotFound(ResponseHelper.CreateNotFoundResponse<bool>(ApiAction.Deleted, ApplicationMessages.SystemFunctionNotFoundMessage));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Deleted);
                }
            })
            .WithName("DeleteSystemFunction")
            .WithTags("SystemFunctions")
            .RequireAuthorization();
        }
    }
}

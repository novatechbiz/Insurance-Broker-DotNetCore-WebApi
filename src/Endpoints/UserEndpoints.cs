using InsuraNova.Handlers;
using InsuraNova.Helpers;
using InsuraNova.Resources;
using Microsoft.AspNetCore.Authorization;


namespace InsuraNova.Endpoints
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/users", async (IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var userDtos = await mediator.Send(new GetAllUsersQuery());
                    return Results.Ok(ResponseHelper.CreateSuccessResponse(userDtos, ApiAction.Read));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
            .WithName("GetAllUsers")
            .WithTags("Users")
            .RequireAuthorization();

            app.MapPost("/users", [Authorize] async (UserProfile user, IMediator mediator, IValidator<UserProfile> validator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    logger.LogInformation("Attempting to create a user with data: {@UserDto}", user);
                    var validationResult = validator.Validate(user);
                    if (!validationResult.IsValid)
                    {
                        logger.LogWarning("User creation failed validation: {Errors}", validationResult.Errors);
                        return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(ApiAction.Created, "Validation failed: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))));
                    }
                    JwtHelper.InitializeEntityMetadata(context, user);
                    var addedUser = await mediator.Send(new AddUserCommand(user));
                    logger.LogInformation("User created successfully with ID: {UserId}", addedUser.Id);
                    return Results.Created($"/users/{addedUser.Id}", ResponseHelper.CreateSuccessResponse(addedUser, ApiAction.Created));
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Created);
                }
            })
            .WithName("CreateUser")
            .WithTags("Users")
            .RequireAuthorization();

            app.MapPut("/users/{id}", [Authorize] async (int id, UserProfile user, IMediator mediator, IValidator<UserProfile> validator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    logger.LogInformation("Attempting to update user with ID: {UserId}", id);
                    var validationResult = validator.Validate(user);
                    if (!validationResult.IsValid)
                    {
                        logger.LogWarning("User update failed validation for user with ID: {UserId}. Errors: {Errors}", id, validationResult.Errors);
                        var validationErrors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                        return Results.BadRequest(ResponseHelper.CreateFailureResponse<object>(ApiAction.Updated, $"Validation failed: {validationErrors}"));
                    }

                    if (id != user.Id)
                    {
                        logger.LogWarning("ID mismatch: URL ID {Id} does not match DTO ID {UserDtoId}", id, user.Id);
                        return Results.BadRequest(ResponseHelper.CreateCustomErrorResponse<UserProfile>(ApplicationMessages.IdMismatchMessage, ApiAction.Updated));
                    }

                    var existingUser = await mediator.Send(new GetUserByIdQuery(id));
                    if (existingUser == null)
                    {
                        logger.LogWarning("User with ID: {UserId} not found", id);
                        return Results.NotFound(ResponseHelper.CreateCustomErrorResponse<UserProfile>(ApplicationMessages.UserNotFoundMessage, ApiAction.Updated));
                    }

                    //MappingHelper.UpdateEntityProperties(existingUser, user);
                    JwtHelper.InitializeUpdateMetadata(context, existingUser);
                    var updatedUser = await mediator.Send(new UpdateUserCommand(existingUser));

                    logger.LogInformation("User with ID: {UserId} updated successfully", updatedUser.Id);
                    return Results.Ok(ResponseHelper.CreateSuccessResponse(updatedUser, ApiAction.Updated));
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while updating the user with ID: {UserId}", id);
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Updated);
                }
            })
            .WithName("UpdateUser")
            .WithTags("Users")
            .RequireAuthorization();


            app.MapGet("/users/{id}", async (int id, IMediator mediator, HttpContext context, ILogger<Program> logger) =>
            {
                try
                {
                    logger.LogInformation("Attempting to retrieve user with ID: {UserId}", id);
                    var userDto = await mediator.Send(new GetUserByIdQuery(id));
                    if (userDto != null)
                    {
                        logger.LogInformation("Successfully retrieved user with ID: {UserId}", id);
                        return Results.Ok(ResponseHelper.CreateSuccessResponse(userDto, ApiAction.Read));
                    }
                    else
                    {
                        logger.LogWarning("User with ID: {UserId} not found", id);
                        return Results.NotFound(ResponseHelper.CreateNotFoundResponse<UserProfile>(ApiAction.Read, ApplicationMessages.UserNotFoundMessage));
                    }
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Read);
                }
            })
             .WithName("GetUserById")
             .WithTags("Users")
             .RequireAuthorization();


            app.MapDelete("/users/{id}", async (int id, IMediator mediator, ILogger<Program> logger, HttpContext context) =>
            {
                try
                {
                    logger.LogInformation("Attempting to delete user with ID: {UserId}", id);
                    var success = await mediator.Send(new DeleteUserCommand(id));
                    if (success)
                    {
                        logger.LogInformation("Successfully deleted user with ID: {UserId}", id);
                        return Results.Ok(ResponseHelper.CreateSuccessResponse(true, ApiAction.Deleted));
                    }
                    else
                    {
                        logger.LogWarning("User with ID: {UserId} not found", id);
                        return Results.NotFound(ResponseHelper.CreateNotFoundResponse<bool>(ApiAction.Deleted, ApplicationMessages.UserNotFoundMessage));
                    }
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleExceptionWithLogging<object>(logger, ex, context, ApiAction.Deleted);
                }
            })
            .WithName("DeleteUser")
            .WithTags("Users")
            .RequireAuthorization();

        }
    }
}

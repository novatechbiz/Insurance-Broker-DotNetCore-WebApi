using InsuraNova.Data;
using InsuraNova.Repositories;
using InsuraNova.Validation;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using InsuraNova.Endpoints;
using InsuraNova.Middleware;
using InsuraNova.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using InsuraNova.Services;
using Amazon.SimpleEmail;
using InsuraNova.Configurations;
using Serilog;
using InsuraNova.Handlers;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin() 
              .AllowAnyHeader() 
              .AllowAnyMethod();
    });
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




// Configure DbContext with connection string
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));

// Add Amazon SES client
builder.Services.AddAWSService<IAmazonSimpleEmailService>();


builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();


// Add Services

builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IEncryptionHelper, EncryptionHelper>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMenuItemService, MenuItemService>();



// Correctly register MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));


// Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CompanyValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();



builder.Services.AddFluentValidationAutoValidation();

// Add Authentication and Authorization
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = TokenHelper.Issuer,
        ValidAudience = TokenHelper.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenHelper.SecretKey))
    };
});

builder.Services.AddAuthorization();

LoggingConfiguration.ConfigureSerilog();
builder.Host.UseSerilog();

var app = builder.Build();

// Use the CORS policy
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}


// Use authentication middleware
app.UseAuthentication();
app.UseAuthorization();



// Use custom error handling middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

// Enable serving static files
app.UseStaticFiles();

// Redirect root URL to the index.html page
app.MapGet("/", () => Results.Redirect("/index.html"))
   .WithName("HomePage")
   .WithTags("Home");

// Map endpoints
app.MapAuthEndpoints();
app.MapUserEndpoints();
app.MapUserRoleEndpoints();
app.MapMenuItemEndpoints();


app.Run();





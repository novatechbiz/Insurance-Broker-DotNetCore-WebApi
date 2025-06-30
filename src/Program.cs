using Amazon.SimpleEmail;
using AspNetCoreRateLimit;
using AspNetCoreRateLimit.Redis;
using AutoMapper;
using FluentValidation.AspNetCore;
using InsuraNova.Configurations;
using InsuraNova.Data;
using InsuraNova.Endpoints;
using InsuraNova.Helpers;
using InsuraNova.Mappings;
using InsuraNova.Middleware;
using InsuraNova.Repositories;
using InsuraNova.Services;
using InsuraNova.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

// Allowed origins
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins(allowedOrigins!)
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

// Add HttpContextAccessor to access the current HTTP context
builder.Services.AddHttpContextAccessor();

// Configure Redis cache for rate limiting
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RadisConnection");
});

builder.Services.AddRedisRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));


// Configure Radis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = builder.Configuration.GetConnectionString("RadisConnection");
    return ConnectionMultiplexer.Connect(configuration);
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Configure DbContext with connection string
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));

// Add Amazon SES client
builder.Services.AddAWSService<IAmazonSimpleEmailService>();

// Register AutoMapper and create a mapping configuration
var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);


// Add Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();



// Add Services
builder.Services.AddScoped<ICustomerHistoryLogService, CustomerHistoryLogService>();
builder.Services.AddScoped<ITokenBlacklistService, TokenBlacklistService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IRecordStatusService, RecordStatusService>();
builder.Services.AddScoped<IInsuranceTypeService, InsuranceTypeService>();
builder.Services.AddScoped<IEntryTypeService, EntryTypeService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<ITransactionTypeService, TransactionTypeService>();
builder.Services.AddScoped<IEncryptionHelper, EncryptionHelper>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICustomerTypeService, CustomerTypeService>();
builder.Services.AddScoped<ICustomerIdentificationTypeService, CustomerIdentificationTypeService>();
builder.Services.AddScoped<IGenderTypeService, GenderTypeService>();
builder.Services.AddScoped<IUserTypeService, UserTypeService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ICompanyTypeService, CompanyTypeService>();
builder.Services.AddScoped<IInsuranceCompanyService, InsuranceCompanyService>();
builder.Services.AddScoped<ISystemFunctionService, SystemFunctionService>();
builder.Services.AddScoped<IPremiumLineService, PremiumLineService>();


// Correctly register MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));


// Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CompanyValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CustomerValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CustomerTypeValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CustomerIdentificationTypeValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<GenderTypeValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UserTypeValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RoleValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RecordStatusValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<InsuranceTypeValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<TransactionTypeValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<EntryTypeValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CurrencyValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<SystemFunctionValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CompanyTypeValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<PremiumLineValidator>();


builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
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

    // Validate blacklisted tokens
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var tokenBlacklistService = context.HttpContext.RequestServices.GetRequiredService<ITokenBlacklistService>();
            var jti = context.Principal.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            if (!string.IsNullOrEmpty(jti))
            {
                var isBlacklisted = await tokenBlacklistService.IsTokenBlacklistedAsync(jti);
                if (isBlacklisted)
                {
                    context.Fail("Token is blacklisted.");
                }
            }
        }
    };
});


builder.Services.AddAuthorization();

LoggingConfiguration.ConfigureSerilog();
builder.Host.UseSerilog();

// Add Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "User Auth", Version = "v1", Description = "Services to Authenticate user" });


    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Please enter a valid token in the following format: {your token here} do not add the word 'Bearer' before it."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Use the CORS policy
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use Rate Limiting middleware
app.UseIpRateLimiting();

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
app.MapCustomerTypeEndpoints();
app.MapCustomerIdentificationTypeEndpoints();
app.MapGenderTypeEndpoints();
app.MapRoleEndpoints();
app.MapUserTypeEndpoints();
app.MapRecordStatusEndpoints();
app.MapTransactionTypeEndpoints();
app.MapCurrencyEndpoints();
app.MapEntryTypeEndpoints();
app.MapInsuranceTypeEndpoints();
app.MapSystemFunctionEndpoints();
app.MapCompanyTypeEndpoints();
app.MapInsuranceCompanyEndpoints();
app.MapPremiumLineEndpoints();
app.MapCustomerEndpoints();

app.Run();





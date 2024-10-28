using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Pokemon_Review_API;
using Pokemon_Review_API.Authorization;
using Pokemon_Review_API.Data;
using Pokemon_Review_API.Helper;
using Pokemon_Review_API.InterFace;
using Pokemon_Review_API.Repository;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(
    option =>
    {
        option.Filters.Add<PermissionBasedAuthorizationfilter>();
    }
    );
builder.Services.AddTransient<Seed>();
builder.Services.AddScoped<IPokemonRepository, PokemonRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
builder.Services.AddScoped<IReviewerRepository, ReviewerRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddAutoMapper(typeof(Program));
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
   //JWT Bearer    Authentication
var JwtOpation = builder.Configuration.GetSection("Jwt").Get<JwtOption>();
builder.Services.AddSingleton(JwtOpation);
builder.Services.AddAuthentication()
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opation =>
    {
        opation.SaveToken = true;
        opation.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = JwtOpation.Issuer,
            ValidateAudience = true,
            ValidAudience = JwtOpation.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOpation.SigningKey))
        };
    });
builder.Services.AddAuthorization(builder =>
{
    builder.AddPolicy("SuperUsersonly", builder =>
    {
        builder.RequireRole("admin", "SuperUser");
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
            ///  SWAGER
builder.Services.AddSwaggerGen(swagger =>
{
    //This is to generate the Default UI of Swagger Documentation    
   swagger.SwaggerDoc("v1", new OpenApiInfo
   {
       Version = "v1",
       Title = "Pokemon-Review-API",
       Description = " CV Projrcy"
   });
    // To Enable authorization using Swagger (JWT)    
   swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
   {
       Name = "Authorization",
       Type = SecuritySchemeType.ApiKey,
       Scheme = "Bearer",
       BearerFormat = "JWT",
       In = ParameterLocation.Header,
       Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
   });
   swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
               {
                    {
                    new OpenApiSecurityScheme
                    {
                    Reference = new OpenApiReference
                    {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                    }
                    },
                    new string[] {}
                    }
                   });
});
var Conction = builder.Configuration.GetConnectionString("DevfultConnction");
builder.Services.AddDbContext<ApplictionDBCotext>(builder => builder.UseSqlServer(Conction));
var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "seeddata")
    SeedData(app);

void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<Seed>();
        service.SeedDataContext();
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

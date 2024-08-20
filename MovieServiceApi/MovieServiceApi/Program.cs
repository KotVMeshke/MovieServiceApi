using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieServiceApi.DataBase.Context;
using System.Security.Claims;
using System.Text;
using MovieServiceApi.Users.Endpoints;
using MovieServiceApi.Users.Services;
using Microsoft.OpenApi.Models;
using MovieServiceApi.ExceptionHandler;
using MovieServiceApi.Utils.Policies;
using MovieServiceApi.Utils.Roles;
using MovieServiceApi.Movies.Services;
using MovieServiceApi.Movies.Endpoints;
using MovieServiceApi.Persons.Services;
using MovieServiceApi.Persons.Endpoints;
using MovieServiceApi.Favorites.Endpoints;
using MovieServiceApi.Favorites.Service;
using MovieServiceApi.Feedbacks.Endpoints;
using MovieServiceApi.Feedbacks.Service;
using MovieServiceApi.Images.Endpoints;
using MovieServiceApi.Images.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appparams.json");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MovieApi", Version = "v1" });
    //c.OperationFilter<FileUploadOperation>();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Введите токен JWT с префиксом Bearer",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var config = builder.Configuration;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidAudience = config["JwtParameters:Audience"],
            ValidateAudience = true,
            ValidIssuer = config["JwtParameters:Issuer"],
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtParameters:Key"]!))
        };


    });

//builder.Services.AddExceptionHandler<ExceptionHandlerMiddleware>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<MovieService>();
builder.Services.AddScoped<PersonService>();
builder.Services.AddScoped<FavoritesService>();
builder.Services.AddScoped<FeedbackService>();
builder.Services.AddScoped<ImageService>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(PolicyType.AdministratorPolicy, policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, RoleType.Administrator);
    }
    );
    options.AddPolicy(PolicyType.RegularUserPolicy, policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, RoleType.AllRoles);
    }
    );
});
builder.Services.AddDbContext<MovieServiceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServerConnection")));


var app = builder.Build();
//app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGroup("/api/user").MapUser();
app.MapGroup("/api/movie").MapMovies();
app.MapGroup("/api/person").MapCrew();
app.MapGroup("/api/favorites").MapFavorites();
app.MapGroup("/api/feedback").MapFeedback();
app.MapGroup("/api/image").MapImage();
app.Run();


using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Server.BLL.Managers.EventManager;
using Server.BLL.Managers.UserManager;
using Server.BLL.Mapping;
using Server.Common.Classes.Models.Common;
using Server.Common.Classes.Models.EventModels;
using Server.Common.Classes.Models.UserModels;
using Server.Common.Interfaces.Models.IEventModel;
using Server.Common.Interfaces.Models.IUserModel;
using Server.DAL.Models;
using Server.DAL.Repository;
using System.Net.Sockets;
using System.Net;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Authorization - Jwt Tocken
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            AuthenticationType = "Bearer",
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"].ToString(),
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"].ToString(),
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration["Jwt:Key"].ToString())),
            ValidateIssuerSigningKey = true,
        };
    });

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Server API", Version = "v1", Description = "This is API for events stream task." });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
    var filePath = Path.Combine(System.AppContext.BaseDirectory,
        "C:\\Users\\ArciV\\source\\repos\\Server\\Server\\obj\\Debug\\net6.0\\Server.API.xml");
    option.IncludeXmlComments(filePath);
});

//User
builder.Services.AddScoped<IUserRepository<User>, UserRepository>();
builder.Services.AddScoped<IUserManager, UserManager>();

//Event
builder.Services.AddScoped<IEventRepository<Event, EventFilter>, EventRepository>();
builder.Services.AddScoped<IEventManager, EventManager>();

//Automapping
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
};

app.UseWebSockets(webSocketOptions);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

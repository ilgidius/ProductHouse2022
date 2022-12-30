using CallRecording.DAL.Repository;
using CallRecording.DAL.Models;
using CallRecording.Common.IRepository;
using CallRecording.Common.IUser;
using CallRecording.BLL.UserLogic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Authorization - Jwt Tocken
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").ToString(),
            ValidateAudience = true,
            ValidAudience = builder.Configuration.GetSection("Jwt:Audience").ToString(),
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration.GetSection("Jwt:Key").ToString() ?? "n7y2oWXzWIyzPsuMcvGu")),
            ValidateIssuerSigningKey = true,
        };
    });

//User
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IUserRepository<User>, UserRepository>();
builder.Services.AddScoped<IUserValidation<User, UserLogin>, UserValidation>();

//Event
builder.Services.AddScoped<IRepository<Event>, EventRepository>();


var app = builder.Build();


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

using CallRecording.DAL.Repository;
using CallRecording.DAL.Models;
using CallRecording.Common.Repository;
using CallRecording.Common.IUser;
using CallRecording.BLL.UserLogic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IRepository<Event>, EventRepository>();
builder.Services.AddScoped<IUserValidation, UserValidation>();

var app = builder.Build();

//var webSocketOptions = new WebSocketOptions
//{
 //   KeepAliveInterval = TimeSpan.FromMinutes(2)
//};

//app.UseWebSockets(webSocketOptions);

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

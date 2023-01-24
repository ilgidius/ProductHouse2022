using Client.Models;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;
using Client.Managers;
using Client.Managers.UserManager;
using Client.Managers.EventManager;

string URL = "https://localhost:7192/api/";

User user;
Startup startup = new Startup();
UserManager userManager = new UserManager();
EventManager eventManager = new EventManager();

do
{
    Console.WriteLine("Please enter your correct login and password.");
    user = startup.Login(URL + "users/login");
    Console.Clear();
} while (user == null);

Console.WriteLine($"Welcome {user.Login}!");

while (true)
{
    Console.Write("Enter route: ");
    string route = Console.ReadLine() ?? string.Empty;
    if (route.Contains("add"))
    {
        if (route.Contains("user"))
        {
            userManager.AddNewUser(URL + "users", user.Token);
        }
        else if(route.Contains("event"))
        {
            eventManager.AddEvent(URL + "events/", user.Token);
        }
    }
    else if (route.Contains("connect"))
    {
        await startup.WBS(URL + "ws", Console.ReadLine() ?? "Test message");
    }
    else if (route.Contains("delete u"))
    {
        userManager.DeleteUser(URL + "users", user.Token);
    }
    else if(route.Contains("update p"))
    {
        userManager.UpdateUser(URL + "users", user.Token);
    }
    else if (route == "back" || route == "exit")
    {
        break;
    }
    else
    {
        Console.WriteLine("You can use following commands:\nadd user\nupdate user\ndelete user\nconnect\nback\nexit");
    }
    Console.WriteLine("\n*-----------------------------------------------------------------------------*\n\n");
}
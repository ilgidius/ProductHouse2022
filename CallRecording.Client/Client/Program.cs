using Client.Models;
using Client.Managers;
using Client.Common.Models;

string URL = "https://localhost:7192/api/";
string command = string.Empty;
UserManager currentUser = new UserManager();

currentUser.Authorization().Wait();
Console.Clear();
Console.WriteLine($"Welcome {currentUser.Username}!");

while (true)
{
    if (currentUser.Role == "user")
    {
        Console.WriteLine("You can use following commands:\n\treauthorize (change user)\n\tsubscribe\n\texit");
        Console.Write("Enter command: ");

        command = Console.ReadLine();
        if (command.ToLower().Contains("subscribe"))
        {
            Console.WriteLine("Which of the following event types you want to receive?\nINIT | RINGING | START | VOICE | STOP");
            Console.Write("Event type you'd like to sunscribe: ");

            currentUser.WBS(Console.ReadLine().ToUpper());
        }
        else if (command.ToLower().Contains("chunked"))
        {
            await currentUser.HttpManager.ChunkedRequest();
        }
    }
    else if (currentUser.Role == "admin")
    {
        Console.WriteLine("You can use following commands:\n\treauthorize (change user)\n\tadd user\n\tadd event\n\tchange (user) password\n\t" +
            "delete user\n\tget (posted) events \n\tperiodic service\n\texit");
        Console.Write("Enter command: ");

        command = Console.ReadLine();
        if (command.ToLower().Contains("add"))
        {
            if (command.ToLower().Contains("user"))
            {
                User newUser = new User();
                currentUser.HttpManager.PostAsync(URL + "users", "login", newUser.Username, "password", newUser.Password, "role", newUser.Role).Wait();
            }
            else if (command.ToLower().Contains("event"))
            {
                Event newEvent = new Event();
                currentUser.HttpManager.PostAsync(URL + "events", "addedTime", newEvent.AddedTime, "sentTime", DateTime.Now.ToUniversalTime().ToString(),
                    "eventType", newEvent.EventType, "key", newEvent.Key, "value", newEvent.Value, "login", currentUser.Username).Wait();
            }
        }
        else if (command.ToLower().Contains("change"))
        {
            User passChange = new User(string.Empty);
            currentUser.HttpManager.PutAsync(URL + "users", "login", passChange.Username, "password", passChange.Password).Wait();
        }
        else if (command.ToLower().Contains("delete"))
        {
            Console.WriteLine("By id or username?");
            string choice = Console.ReadLine();
            if (choice.ToLower().Contains("id"))
            {
                Console.Write("Enter id: ");
                int.TryParse(Console.ReadLine(), out int id);
                currentUser.HttpManager.DeleteAsync(URL + $"users/id/{id}").Wait();
            }
            Console.Write("Enter username: ");
            currentUser.HttpManager.DeleteAsync(URL + $"users/username/{Console.ReadLine()}").Wait();
        }
        else if (command.ToLower().Contains("get"))
        {
            Console.WriteLine("By id or username?");
            string choice = Console.ReadLine();
            if (choice.ToLower().Contains("id"))
            {
                Console.Write("Enter id: ");
                int.TryParse(Console.ReadLine(), out int id);
                currentUser.HttpManager.GetAsync(URL + $"events/id/{id}").Wait();
            }
            Console.Write("Enter username: ");
            currentUser.HttpManager.GetAsync(URL + $"events/username/{Console.ReadLine()}").Wait();
        }
        else if (command.ToLower().Contains("periodic"))
        {
            string state;
            do
            {
                Console.Write("Enter state of periodic service: ");
                state = Console.ReadLine();
            } while (state.ToLower() != "true" && state.ToLower() != "false");
            currentUser.HttpManager.PatchAsync(URL + $"background?State={state}").Wait();
        }
    }
    if (command.ToLower().Contains("exit"))
    {
        Console.WriteLine($"Bye, {currentUser.Username}!");
        Task.Delay(1500).Wait();
        break;
    }
    else if (command.ToLower().Contains("reauthorize"))
    {
        Console.Clear();
        currentUser.Authorization();
        Console.Clear();
    }
    Console.WriteLine("\n*----------------------------------------------------------------------------*\n\n");
}
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;

namespace Self_Updater;

internal class HttpServer
{
    public static void ListenToWebHook(string linkToRepo)
    {
        Console.WriteLine("Update on Github Repo");
        Console.WriteLine("Running updates on local repo and updating the Docker containers");

        if (OperatingSystem.IsWindows())
        {
            Console.WriteLine("Running on Windows");
        }
        else if (OperatingSystem.IsLinux())
        {
            Console.WriteLine("Running on Linux");

            string gitCommand = $"git pull {linkToRepo}";
            string rebuildContainer = "docker-compose restart";

            Process.Start(gitCommand);
            Process.Start(rebuildContainer);

        }
    }

    public static void CreateServer(string uri)
    {
        var listener = new HttpListener();

        listener.Prefixes.Add(uri);
        Console.WriteLine("Opening HTTP server");
        listener.Start();
        Console.WriteLine("Listening on ... " + uri);

        while (true)
        {
            var context = listener.GetContext();

            var request = context.Request;

            //response
            Console.WriteLine(request.HttpMethod);

            if (request.HttpMethod == "POST")
            {
               ListenToWebHook("https://github.com/GeorgK1/shauntwitter.git");
            }

            var response = context.Response;
        }
    }
}

internal class Updater
{
    private static void Main(string[] args)
    {
        HttpServer.CreateServer("http://localhost:8080/");
    }
}
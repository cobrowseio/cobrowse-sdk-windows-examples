using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Cobrowse.IO.ConsoleApp
{
  class Program
  {
    private static TaskCompletionSource<bool> startedTcs;
    private static TaskCompletionSource<Session> sessionTcs;

    private static void Main(string[] args)
    {
      File.Delete("ConsoleApp.log");
      Trace.Listeners.Add(new ConsoleTraceListener());
      Trace.Listeners.Add(new TextWriterTraceListener("ConsoleApp.log"));
      Trace.AutoFlush = true;

      AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

      Test().GetAwaiter().GetResult();

      Console.WriteLine("All done. Press <Enter>");
      Console.ReadLine();
    }

    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
    {
      Exception e = (Exception)args.ExceptionObject;
      Trace.WriteLine($"--- Unhandled exception: {e.GetType().Name}: {e.Message}");
      Trace.WriteLine(e.StackTrace);
    }

    private static async Task<Session> StartByCode()
    {
      Session session = await CobrowseIO.Instance.CreateSession();
      Console.WriteLine("Code: {0}", session.Code);

      return session;
    }

    private static async Task<Session> StartById(string id)
    {
      return await CobrowseIO.Instance.GetSession(id);
    }

    private static Task<Session> StartByPush()
    {
      sessionTcs = new TaskCompletionSource<Session>(TaskCreationOptions.RunContinuationsAsynchronously);
      Console.WriteLine("Waiting for remote session initiation");
      return sessionTcs.Task;
    }

    private static async Task Test()
    {
      try
      {
        // Set your license key here.
        CobrowseIO.Instance.License = "trial";
        // Production API. Modify if you use another endpoint.
        CobrowseIO.Instance.Api = new Uri("https://cobrowse.io");

        CobrowseIO.Instance.CustomData = new Dictionary<string, object>()
        {
          { CobrowseIO.UserEmailKey, "hello@cobrowse.io" },
          { CobrowseIO.DeviceNameKey, "Console Device" }
        };

        CobrowseIO.Instance.SessionAuthorizing += Session_AuthorizationRequired;
        CobrowseIO.Instance.SessionEnded += Session_Ended;
        CobrowseIO.Instance.SessionUpdated += Session_Updated;
        //CobrowseIO.Instance.SessionControlsUpdated += f => { };

        await CobrowseIO.Instance.Start();
        startedTcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

        Session session =
          //await StartByCode();
          await StartByPush();
          //await StartById("i7G-mikT5K-bKXjAdgty1w");

        if (await startedTcs.Task)
        {
          Console.WriteLine("Press <Enter> to end.");
          Console.ReadLine();
        }

        Console.WriteLine("Ending");
        await session.End();
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }
    }

    private static void Session_Updated(Session session)
    {
      if (sessionTcs != null)
        sessionTcs.SetResult(session);
      sessionTcs = null;

      if (session.State != SessionState.Active)
        return;

      Console.WriteLine("Session started");
      startedTcs.TrySetResult(true);
    }

    private static void Session_Ended(Session session)
    {
      Console.WriteLine("Session ended");
      startedTcs.TrySetResult(false);
    }

    private static void Session_AuthorizationRequired(Session session)
    {
      Console.WriteLine("Authorization required");
      if (Console.ReadLine() == "n")
        Task.Run(session.End);
      else
        Task.Run(session.Activate);
      
    }
  }
}

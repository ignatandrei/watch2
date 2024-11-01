using Spectre.Console;
using System.Diagnostics;
Process? proc = null;
Console.CancelKeyPress += delegate {
    // call methods to clean up
    Kill(proc);
};
bool shouldWait = false;
while (true)
{
    ProcessStartInfo startInfo = new ProcessStartInfo();
    startInfo.FileName = "dotnet";
    startInfo.Arguments = "watch " + string.Join(' ', args);
    //startInfo.LoadUserProfile = true;
    startInfo.WorkingDirectory = Environment.CurrentDirectory;

    startInfo.RedirectStandardOutput = true;
    startInfo.RedirectStandardInput = true;
    startInfo.RedirectStandardError = true;
    startInfo.UseShellExecute = false;
    startInfo.CreateNoWindow = true;

    proc = new Process();
    proc.StartInfo = startInfo;
    proc.OutputDataReceived += (sender, e) =>
    {
        if (shouldWait)
        {
            shouldWait = false;
            Kill(proc);
            Thread.Sleep(15_000);
            return;

        }
        var line = e.Data;
        if (line == null) return;
        if (line.Contains("dotnet watch"))
        {
            if (line.Contains("Started"))
            {
                Console.Clear();
            }
            if (line.Contains("Building"))
            {

            }
        }

        if (line.Contains(": error "))
        {
            AnsiConsole.MarkupLineInterpolated($"->[bold red]:cross_mark: {e.Data}[/]");
            return;
        }
        if (line.Contains("Waiting for a file to change before"))
        {
            Console.WriteLine("wain");
            shouldWait = true;

        }

        Console.WriteLine("->" + e.Data);


    };
    proc.ErrorDataReceived += (sender, e) =>
    {
        if (e.Data != null)
        {
            AnsiConsole.MarkupLineInterpolated($"->[bold red]:cross_mark: {e.Data}[/]");
            return;

        }
    };

    AnsiConsole.MarkupLineInterpolated($"[bold green]Starting...[/]");
    proc.Start();
    Console.Clear();
    proc.BeginOutputReadLine();
    proc.BeginErrorReadLine();
    await proc.WaitForExitAsync();
}

static void Kill(Process? proc)
{
    if (proc == null) return;
    if (proc.HasExited) return;
    proc.Kill(true);
    proc.Close();
    proc = null;
}


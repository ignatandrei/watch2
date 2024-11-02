namespace Watch2;
public class ProcessManager
{
    private IProcessWrapper? _proc = null;
    private bool _shouldWait = false;
    public bool IsKilledByThisSoftware { get; private set; } =false;
    public async Task StartProcessAsync(string[] args, IConsoleWrapper console, IProcessStartInfo startInfo)
    {
        Console.CancelKeyPress += (sender, e) => Kill(_proc);

        do 
        {
            
            _proc = new ProcessWrapper(startInfo);
            _proc.OutputDataReceived += (sender, e) => HandleOutput(new DataReceivedEventArgsWrapper(e).Data, console);
            _proc.ErrorDataReceived += (sender, e) => HandleError(new DataReceivedEventArgsWrapper(e), console);

            console.MarkupLineInterpolated($"[bold green]Starting...[/]");
            _proc.Start();
            console.Clear();
            _proc.BeginOutputReadLine();
            _proc.BeginErrorReadLine();
            await _proc.WaitForExitAsync();
        }while (IsKilledByThisSoftware);
    }

    internal void HandleOutput(string? data, IConsoleWrapper console)
    {
        if (string.IsNullOrWhiteSpace(data)) return;
        if (_shouldWait)
        {
            _shouldWait = false;
            if (_proc != null) { 
                IsKilledByThisSoftware = !(_proc!.HasExited);
            }
            Kill(_proc);
            Thread.Sleep(15_000);
            return;
        }

        var line = data;
        if (line == null) return;

        if (line.Contains("dotnet watch"))
        {
            if (line.Contains("Started"))
            {
                console.Clear();
            }
        }

        if (line.Contains(": error "))
        {
            console.MarkupLineInterpolated($"->[bold red]:cross_mark: {line}[/]");
            return;
        }

        if (line.Contains("Waiting for a file to change before"))
        {
            _shouldWait = true;
        }

        console.WriteLine("->" + line);
    }

    internal void HandleError(IDataReceivedEventArgs e, IConsoleWrapper console)
    {
        if (e.Data != null)
        {
            console.MarkupLineInterpolated($"->[bold red]:cross_mark: {e.Data}[/]");
        }
    }

    private void Kill(IProcessWrapper? proc)
    {
        if (proc == null) return;
        proc.Kill();
    }
}
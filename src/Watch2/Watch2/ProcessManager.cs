namespace Watch2;
public class ProcessManager
{
    public ProcessManager(Func<IProcessStartInfo,IProcessWrapper> ctorProcessWrapper,ILogger<ProcessManager> logger,
                Ioptions_gen_json options
        )
    {
        this.ctorProcessWrapper = ctorProcessWrapper;
        this.logger = logger;
        this.options = options;
    }
    private IProcessWrapper? _proc = null;
    private bool _shouldWait = false;
    private readonly Func<IProcessStartInfo, IProcessWrapper> ctorProcessWrapper;
    private readonly ILogger<ProcessManager> logger;
    private readonly Ioptions_gen_json options;

    public bool IsKilledByThisSoftware { get; private set; } =false;
    public async Task StartProcessAsync(string[] args, 
        IConsoleWrapper console, 
        IProcessStartInfo startInfo
        )
    {
        
        var valid= options.Validate(new(this)).ToArray();
        if(valid.Length > 0)
        {
            foreach (var item in valid)
            {
                console.MarkupLineInterpolated($"[bold red]{item.ErrorMessage}[/]");
            }
            return;
        }
        Console.CancelKeyPress += (sender, e) => Kill(_proc);

        do 
        {
            
            _proc = ctorProcessWrapper(startInfo);
            _proc.OutDataReceived += async (sender, e) => await HandleOutputMultipleLines(e, console);
            _proc.ErrDataReceived += (sender, e) => HandleError(e, console);

            console.MarkupLineInterpolated($"[bold green]Starting...[/]");
            _proc.Start();
            console.Clear();
            _proc.BeginOutputReadLine();
            _proc.BeginErrorReadLine();
            await _proc.WaitForExitAsync();            
        } while (IsKilledByThisSoftware);
    }
    internal async Task HandleOutputMultipleLines(string? data, IConsoleWrapper console)
    {
        if (string.IsNullOrWhiteSpace(data)) return;
        var arrData=data.Split(new string[] { "\r\n","\r","\n" }, StringSplitOptions.RemoveEmptyEntries).ToArray();
        foreach (var line in arrData)
        {
            await HandleOutput(line, console);
        }
    }
    internal async Task HandleOutput(string? data, IConsoleWrapper console)
    {
        if (string.IsNullOrWhiteSpace(data)) return;
        if (_shouldWait)
        {
            _shouldWait = false;
            if (_proc != null) { 
                IsKilledByThisSoftware = !(_proc!.HasExited);
            }
            Kill(_proc);
            var valueTimeOut = options.TimeOut;
            if (valueTimeOut.HasValue && valueTimeOut.Value>0)
            {
                console.MarkupLineInterpolated($"[bold green]Waiting {valueTimeOut.Value}[/]");
                await Task.Delay(valueTimeOut.Value);
            }
            return;
        }

        var line = data;
        if (line == null) return;

        if (line.Contains("dotnet watch"))
        {
            if (line.Contains("Started"))
            {
                if(options.ClearConsole.HasValue && options.ClearConsole.Value)
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

    internal void HandleError(string data, IConsoleWrapper console)
    {
        if (string.IsNullOrWhiteSpace(data)) return;
        if (data != null)
        {
            console.MarkupLineInterpolated($"->[bold red]:cross_mark: {data}[/]");
        }
    }

    private void Kill(IProcessWrapper? proc)
    {
        if (proc == null) return;
        proc.Kill();
    }
}
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
    private string folder { get; set; } = "";
    public bool IsKilledByThisSoftware { get; private set; } =false;
    public async Task StartProcessAsync(string[] args, 
        IConsoleWrapper console, 
        IProcessStartInfo startInfo
        )
    {
        folder = startInfo.WorkingDirectory;
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
            await StartProcessAsync(console, startInfo);
        } while (IsKilledByThisSoftware);
    }

    private async Task StartProcessAsync(IConsoleWrapper console, IProcessStartInfo startInfo)
    {
        if (_proc == null || _proc.HasExited)
        {
            _proc = ctorProcessWrapper(startInfo);
            _proc.OutDataReceived += async (sender, e) => await HandleOutputMultipleLines(e, console);
            _proc.ErrDataReceived += (sender, e) => HandleError(e, console);
        }
        console.Clear();
        console.MarkupLineInterpolated($"[bold green]Starting...[/]");
        var valueTimeOut = options.TimeOut;
        if (valueTimeOut.HasValue && valueTimeOut.Value > 0)
        {
            console.MarkupLineInterpolated($"[bold green]Waiting {valueTimeOut.Value}[/]");
            await Task.Delay(valueTimeOut.Value);
        }
        _proc.Start();

        _proc.BeginOutputReadLine();
        _proc.BeginErrorReadLine();
        await _proc.WaitForExitAsync();
        
    }

    private void StartAfter( IConsoleWrapper console)
    {
        if (!string.IsNullOrWhiteSpace(options.RunAfter))
        {
            try
            {
                var processInfo = new ProcessStartInfo
                {
                    FileName = options.RunAfter,
                    //Arguments = options.RunAfterArgs,
                    WorkingDirectory = folder,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false,
                    UseShellExecute = true,
                    CreateNoWindow = false
                };
                Process.Start(processInfo);
            }
            catch (Exception ex)
            {
                console.WriteLine($"[bold red]Failed to run {options.RunAfter}: {ex.Message}[/]");
            }
        }
        
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
        var line = data;
        if (line == null) return;
        if(!InterpretLine(console, line))
            console.WriteLine("->" + line);
    
        if (_shouldWait)
        {
            _shouldWait = false;
            if (_proc != null)
            {
                IsKilledByThisSoftware = !(_proc!.HasExited);
            }
            Kill(_proc);
            var valueTimeOut = options.TimeOut;
            if (valueTimeOut.HasValue && valueTimeOut.Value > 0)
            {
                console.MarkupLineInterpolated($"[bold green]Waiting {valueTimeOut.Value}[/]");
                await Task.Delay(valueTimeOut.Value);
                
            }
            return;
        }

       
    }

    private bool InterpretLine(IConsoleWrapper console, string line)
    {
        if (line.Contains("dotnet watch"))
        {
            if (line.Contains("Started"))
            {
                if (options.ClearConsole.HasValue && options.ClearConsole.Value)
                    console.Clear();
            }
        }

        if (line.Contains(": error "))
        {
            console.MarkupLineInterpolated($"->[bold red]:cross_mark: {line}[/]");
            return true;
        }

        if (line.Contains("Waiting for a file to change before"))
        {
            console.MarkupLineInterpolated($"->[bold green]: {line}[/]");
            _shouldWait = false;
            StartAfter(console);

        }
        if(line.Contains("Test run summary: Passed!"))
        {
            console.MarkupLineInterpolated($"->[bold green]: {line}[/]");            
            StartAfter(console);
            return true;
        }
        if(line.Contains("Build FAILED"))
        {
            _shouldWait = false;
            console.MarkupLineInterpolated($"->[bold red]:cross_mark: {line}[/]");
            StartAfter(console);
            return true;

        }
        if(line.Contains("failed"))
        {
            console.MarkupLineInterpolated($"->[bold red]: {line}[/]");
            StartAfter(console);
            return true;
        }

        return false;
    }

    internal void HandleError(string data, IConsoleWrapper console)
    {
        if (string.IsNullOrWhiteSpace(data)) return;
        if (data != null)
        {
            InterpretLine(console, data);
            console.MarkupLineInterpolated($"->[bold red]:cross_mark: {data}[/]");
        }
    }

    private void Kill(IProcessWrapper? proc)
    {
        if (proc == null) return;
        proc.Kill();
    }
}
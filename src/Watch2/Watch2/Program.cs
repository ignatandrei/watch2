using Watch2;

IConsoleWrapper console = new ConsoleWrapper();
var processManager = new ProcessManager();
IProcessStartInfo startInfo = new ProcessStartInfoWrapper
{
    FileName = "dotnet",
    Arguments = "watch " + string.Join(' ', args),
    WorkingDirectory = Environment.CurrentDirectory,
    RedirectStandardOutput = true,
    RedirectStandardInput = true,
    RedirectStandardError = true,
    UseShellExecute = false,
    CreateNoWindow = true
};
await processManager.StartProcessAsync(args, console,startInfo);

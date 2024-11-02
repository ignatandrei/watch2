//IConsoleWrapper console = new ConsoleWrapper();
//var processManager = new ProcessManager();
//IProcessStartInfo startInfo = new ProcessStartInfoWrapper
//{
//    FileName = "dotnet",
//    Arguments = "watch " + string.Join(' ', args),
//    WorkingDirectory = Environment.CurrentDirectory,
//    RedirectStandardOutput = true,
//    RedirectStandardInput = true,
//    RedirectStandardError = true,
//    UseShellExecute = false,
//    CreateNoWindow = true
//};
//await processManager.StartProcessAsync(args, console,startInfo);


var serviceCollection = new ServiceCollection();
ConfigureServices(serviceCollection);

var serviceProvider = serviceCollection.BuildServiceProvider();

var console = serviceProvider.GetRequiredService<IConsoleWrapper>();
var processManager = serviceProvider.GetRequiredService<ProcessManager>();
var startInfo = serviceProvider.GetRequiredService<IProcessStartInfo>();

await processManager.StartProcessAsync(args, console, startInfo);

void ConfigureServices(IServiceCollection services)
{
services.AddSingleton<IConsoleWrapper, ConsoleWrapper>();
services.AddSingleton<ProcessManager, ProcessManager>();
services.AddSingleton<IProcessStartInfo>(provider => new ProcessStartInfoWrapper
{
FileName = "dotnet",
Arguments = "watch " + string.Join(' ', args),
WorkingDirectory = Environment.CurrentDirectory,
RedirectStandardOutput = true,
RedirectStandardInput = true,
RedirectStandardError = true,
UseShellExecute = false,
CreateNoWindow = true
});

    services.AddLogging(loggingBuilder =>
    {
        loggingBuilder.ClearProviders();
        loggingBuilder.SetMinimumLevel(LogLevel.Trace);
        loggingBuilder.AddNLog("nlog.config");
    });
}

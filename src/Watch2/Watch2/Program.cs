//test
//args = ["run --no-hot-reload"];
//string folder = @"D:\eu\GitHub\MovingDiagrams\src\MovingDiagrams\MD.DoNotRun";
//string folder = @"D:\eu\GitHub\MovingDiagrams\src\MovingDiagrams\MD.Tests";
//uncomment this line for production
Console.WriteLine($"Version:{Generated.Watch2.TheAssemblyInfo.GeneratedNameNice}");
//return;
string folder = Environment.CurrentDirectory;
if((args?.Length??0) == 0)
{
    args = ["run","--no-hot-reload","--no-restore"];
}
var fileJSON = Path.Combine(folder, "watch2.json");
if (!File.Exists(fileJSON))
{
    File.WriteAllText(fileJSON, MyAdditionalFiles.options_gen_json);
}

var serviceCollection = new ServiceCollection();
ConfigureServices(serviceCollection, folder);

var serviceProvider = serviceCollection.BuildServiceProvider();

var console = serviceProvider.GetRequiredService<IConsoleWrapper>();
var processManager = serviceProvider.GetRequiredService<ProcessManager>();
var startInfo = serviceProvider.GetRequiredService<IProcessStartInfo>();

var fileOptions = serviceProvider.GetRequiredService<IOptionsReader>();

await processManager.StartProcessAsync(args!, console, startInfo);

void ConfigureServices(IServiceCollection services,string folder)
{
    services.AddSingleton<IFileProvider>(new PhysicalFileProvider(folder));
    services.AddSingleton<IOptionsReader, OptionsReader>();
    services.AddSingleton<Ioptions_gen_json>(it =>
    {
        var optionsReader = it.GetRequiredService<IOptionsReader>();
        return optionsReader.GetOptions() ?? options_gen_json.Empty;
    });
    services.AddSingleton<IConsoleWrapper, ConsoleWrapper>();
    services.AddSingleton<ProcessManager, ProcessManager>();
    services.AddSingleton<IProcessStartInfo>(provider => new ProcessStartInfoWrapper
    {
        FileName = "dotnet",
        Arguments = "watch " + string.Join(' ', args!),
        WorkingDirectory = folder,
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
    services.AddSingleton<ILogger<ProcessManager>, Logger<ProcessManager>>();
    services.AddSingleton<Func<IProcessStartInfo, IProcessWrapper>>(it => new ProcessWrapper(it));
}

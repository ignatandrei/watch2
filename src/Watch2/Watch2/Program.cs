//test


using NS_GeneratedJson_options_gen_json;

args = ["run --no-hot-reload"];
string folder = @"D:\gth\RSCG_Examples\v2\Generator";

//comment this line
//string folder = Environment.CurrentDirectory;

var serviceCollection = new ServiceCollection();
ConfigureServices(serviceCollection, folder);

var serviceProvider = serviceCollection.BuildServiceProvider();

var console = serviceProvider.GetRequiredService<IConsoleWrapper>();
var processManager = serviceProvider.GetRequiredService<ProcessManager>();
var startInfo = serviceProvider.GetRequiredService<IProcessStartInfo>();

var fileOptions = serviceProvider.GetRequiredService<IOptionsReader>();
if(!fileOptions.ExistsFile())
{
    var file = Path.Combine(folder, "watch2.json");
    File.WriteAllText(file, MyAdditionalFiles.options_gen_json);    
}

await processManager.StartProcessAsync(args, console, startInfo);



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
    Arguments = "watch " + string.Join(' ', args),
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

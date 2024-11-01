using Watch2;

IConsoleWrapper console = new ConsoleWrapper();
var processManager = new ProcessManager();
await processManager.StartProcessAsync(args, console);

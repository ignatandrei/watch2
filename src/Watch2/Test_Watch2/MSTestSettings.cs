global using Rocks;
global using Watch2_Interfaces;

[assembly: DoNotParallelize()]
[assembly: Rock(typeof(IProcessWrapper), BuildType.Create)]
[assembly: Rock(typeof(IConsoleWrapper), BuildType.Create)]
[assembly: Rock(typeof(IDataReceivedEventArgs), BuildType.Make)]
[assembly: Rock(typeof(Ioptions_gen_json), BuildType.Create)]


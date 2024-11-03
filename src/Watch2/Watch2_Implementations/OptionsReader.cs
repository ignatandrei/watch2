//https://learn.microsoft.com/en-us/aspnet/core/fundamentals/file-providers?view=aspnetcore-8.0
//https://github.com/TestableIO/System.IO.Abstractions

namespace Watch2_Implementations;

public class OptionsReader : IOptionsReader
{
    private readonly IFileProvider fileProvider;
    private string fileName = "watch2.json";

    public OptionsReader(IFileProvider fileProvider)
    {
        this.fileProvider = fileProvider;
    }
    public bool ExistsFile()
    {
        var fileInfo = fileProvider.GetFileInfo(fileName);
        return fileInfo.Exists;
    }
    public Ioptions_gen_json? GetOptions()
    {
        string fileName = "watch2.json";

        var fileInfo = fileProvider.GetFileInfo(fileName);
        if (!fileInfo.Exists)
        { 
            throw new FileNotFoundException($"{fileName} not found");
        }
        using var stream = fileInfo.CreateReadStream();
        using var reader = new StreamReader(stream);
        var text = reader.ReadToEnd();
        return options_gen_json.Deserialize(text);
    }
}

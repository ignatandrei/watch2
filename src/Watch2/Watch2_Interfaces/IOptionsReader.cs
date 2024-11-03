using System.ComponentModel.DataAnnotations;

namespace Watch2_Interfaces;
public interface IOptionsReader
{
    bool ExistsFile();
    Ioptions_gen_json? GetOptions();
}

public interface Ioptions_gen_json:IValidatableObject
{
    public int? Version { get; set; }
    public bool? ClearConsole { get; set; }
    public int? TimeOut { get; set; }

}

namespace Watch2_Implementations;
public class DataReceivedEventArgsWrapper : IDataReceivedEventArgs
{
    private readonly DataReceivedEventArgs _args;
    private string? _data= null;

    public DataReceivedEventArgsWrapper(DataReceivedEventArgs args)
    {
        _args = args;
        
    }

    public string? Data
    {
        get
        {
            if(_data == null)
                return _args.Data;
            return _data;
        }
        set
        {
            _data = value;
        }
    }
    
}


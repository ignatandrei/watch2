
namespace Watch2_Implementations;

public class ProcessStartInfoWrapper : IProcessStartInfo
{
    private readonly ProcessStartInfo _startInfo;

    public ProcessStartInfoWrapper()
    {
        _startInfo = new ProcessStartInfo();
    }

    public string FileName
    {
        get => _startInfo.FileName;
        set => _startInfo.FileName = value;
    }

    public string Arguments
    {
        get => _startInfo.Arguments;
        set => _startInfo.Arguments = value;
    }

    public string WorkingDirectory
    {
        get => _startInfo.WorkingDirectory;
        set => _startInfo.WorkingDirectory = value;
    }

    public bool RedirectStandardOutput
    {
        get => _startInfo.RedirectStandardOutput;
        set => _startInfo.RedirectStandardOutput = value;
    }

    public bool RedirectStandardInput
    {
        get => _startInfo.RedirectStandardInput;
        set => _startInfo.RedirectStandardInput = value;
    }

    public bool RedirectStandardError
    {
        get => _startInfo.RedirectStandardError;
        set => _startInfo.RedirectStandardError = value;
    }

    public bool UseShellExecute
    {
        get => _startInfo.UseShellExecute;
        set => _startInfo.UseShellExecute = value;
    }

    public bool CreateNoWindow
    {
        get => _startInfo.CreateNoWindow;
        set => _startInfo.CreateNoWindow = value;
    }

    public ProcessStartInfo GetProcessStartInfo() => _startInfo;
}


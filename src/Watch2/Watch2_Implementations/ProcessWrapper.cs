
namespace Watch2_Implementations;

public class ProcessWrapper : IProcessWrapper
{
    private readonly Process _process;

    public ProcessWrapper(IProcessStartInfo startInfo)
    {
        _process = new Process { StartInfo = startInfo.GetProcessStartInfo() };
    }

    public event DataReceivedEventHandler OutputDataReceived
    {
        add => _process.OutputDataReceived += value;
        remove => _process.OutputDataReceived -= value;
    }

    public event DataReceivedEventHandler ErrorDataReceived
    {
        add => _process.ErrorDataReceived += value;
        remove => _process.ErrorDataReceived -= value;
    }

    public void Start() => _process.Start();
    public void Kill() => _process.Kill(true);
    public void BeginOutputReadLine() => _process.BeginOutputReadLine();
    public void BeginErrorReadLine() => _process.BeginErrorReadLine();
    public Task WaitForExitAsync() => _process.WaitForExitAsync();

    public bool HasExited => _process.HasExited;
}





namespace Watch2_Implementations;

public class ProcessWrapper : IProcessWrapper
{
    private readonly Process _process;

    public event EventHandler<string>? ErrDataReceived;
    public event EventHandler<string>? OutDataReceived;

    public ProcessWrapper(IProcessStartInfo startInfo)
    {
        _process = new Process { StartInfo = startInfo.GetProcessStartInfo() };
        _process.ErrorDataReceived += (sender, e) => ErrDataReceived?.Invoke(sender, e?.Data??"");
        _process.OutputDataReceived += (sender, e) => OutDataReceived?.Invoke(sender, e?.Data??"");
    }

    

    

    public void Start() => _process.Start();
    public void Kill() => _process.Kill(true);
    public void BeginOutputReadLine() => _process.BeginOutputReadLine();
    public void BeginErrorReadLine() => _process.BeginErrorReadLine();
    public Task WaitForExitAsync() => _process.WaitForExitAsync();

    public bool HasExited => _process?.HasExited??true;
}


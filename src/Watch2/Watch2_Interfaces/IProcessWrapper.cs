namespace Watch2_Interfaces;

public interface IProcessWrapper
{
    event DataReceivedEventHandler OutputDataReceived;
    event DataReceivedEventHandler ErrorDataReceived;
    void Start();
    void Kill();
    void BeginOutputReadLine();
    void BeginErrorReadLine();
    Task WaitForExitAsync();
    bool HasExited { get; }
}



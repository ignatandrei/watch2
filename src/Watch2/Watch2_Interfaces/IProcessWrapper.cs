namespace Watch2_Interfaces;

public interface IProcessWrapper
{
    event EventHandler<string>? OutDataReceived;
    event EventHandler<string>? ErrDataReceived;
    void Start();
    void Kill();
    void BeginOutputReadLine();
    void BeginErrorReadLine();
    Task WaitForExitAsync();
    bool HasExited { get; }
}

namespace Watch2_Interfaces;

public interface IConsoleWrapper
{
    void Clear();
    void WriteLine(string message);
    void MarkupLineInterpolated(FormattableString message);
}



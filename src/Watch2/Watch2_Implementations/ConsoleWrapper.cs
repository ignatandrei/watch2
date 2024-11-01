
namespace Watch2_Implementations;

public class ConsoleWrapper : IConsoleWrapper
{
    public void Clear() => Console.Clear();
    public void WriteLine(string message) => Console.WriteLine(message);
    public void MarkupLineInterpolated(FormattableString message) => AnsiConsole.MarkupLineInterpolated(message);
}


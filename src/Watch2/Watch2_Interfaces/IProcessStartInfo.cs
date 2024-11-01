using System.Diagnostics;

namespace Watch2_Interfaces;

public interface IProcessStartInfo
{
    string FileName { get; set; }
    string Arguments { get; set; }
    string WorkingDirectory { get; set; }
    bool RedirectStandardOutput { get; set; }
    bool RedirectStandardInput { get; set; }
    bool RedirectStandardError { get; set; }
    bool UseShellExecute { get; set; }
    bool CreateNoWindow { get; set; }

    ProcessStartInfo GetProcessStartInfo();
}



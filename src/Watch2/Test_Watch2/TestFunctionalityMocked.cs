using Watch2;

namespace Test_Watch2;

[TestClass]
public sealed class TestFunctionalityMocked
{
    

    
    [TestMethod]
    public void TestRestart()
    {
        // Arrange
        var mockProcess = new IProcessWrapperCreateExpectations();
        var mockConsole = new IConsoleWrapperCreateExpectations();
        var mockDataReceivedEventArgs = new IDataReceivedEventArgsMakeExpectations();
        mockProcess.Methods.Kill().ExpectedCallCount ( 1);
        mockConsole.Methods.WriteLine(Rocks.Arg.Any<string>()).Callback(it=> { });
        

        // Act
        (new ProcessManager()).HandleOutput(("Waiting for a file to change before"), mockConsole.Instance());

        // Assert
        mockProcess.Verify();
    }
    [TestMethod]
    public void TestConsoleClearCalled()
    {
        
        // Arrange
        var mockProcess = new IProcessWrapperCreateExpectations();
        var mockConsole = new IConsoleWrapperCreateExpectations();
        var mockDataReceivedEventArgs = new IDataReceivedEventArgsMakeExpectations();
        mockConsole.Methods.Clear().ExpectedCallCount(1);
        mockConsole.Methods.WriteLine(Arg.Any<string>()).Callback(it => { });
        // Act
        (new ProcessManager()).HandleOutput("dotnet watch Started", mockConsole.Instance());

        // Assert
        mockConsole.Verify();
    }
}

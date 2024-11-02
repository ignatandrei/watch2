using Watch2;

namespace Test_Watch2;

[TestClass]
public sealed class TestFunctionalityMocked
{

    [TestMethod]
    public void TestNoProject()
    {
        // Arrange
        var mockProcess = new IProcessWrapperCreateExpectations();
        var mockConsole = new IConsoleWrapperCreateExpectations();
        var mockDataReceivedEventArgs = new IDataReceivedEventArgsMakeExpectations();
        mockProcess.Methods.Kill().ExpectedCallCount(1);
        mockConsole.Methods.WriteLine(Arg.Any<string>()).Callback(it => { });

        var prc = (new ProcessManager());
        // Act
        prc.HandleOutput(("dotnet watch ❌ Could not find a MSBuild project file in 'D:\\gth\\watch2\\src\\Watch2'. Specify which project to use with the --project option."), mockConsole.Instance());


        // Assert
        mockProcess.Verify();
        Assert.IsFalse(prc.IsKilledByThisSoftware);
    }

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

using System.ComponentModel.DataAnnotations;

namespace Test_Watch2;
[TestClass]

public class TestFromRealData
{
    [TestMethod]
    public async Task TestRSCGExamples()
    {
        var outputString = $@"
D:\gth\RSCG_Examples\v2\GeneratorData\GeneratorData.csproj : warning NU1903: Package 'Microsoft.Extensions.Caching.Memory' 8.0.0 has a known high severity vulnerability, https://github.com/advisories/GHSA-qj66-m88j-hmgj
D:\gth\RSCG_Examples\v2\Generator\Generator.csproj : warning NU1903: Package 'Microsoft.Extensions.Caching.Memory' 8.0.0 has a known high severity vulnerability, https://github.com/advisories/GHSA-qj66-m88j-hmgj
D:\gth\RSCG_Examples\v2\Generator\Generator.csproj : warning NU1903: Package 'Microsoft.Extensions.Caching.Memory' 8.0.0 has a known high severity vulnerability, https://github.com/advisories/GHSA-qj66-m88j-hmgj
D:\gth\RSCG_Examples\v2\GeneratorData\GeneratorData.csproj : warning NU1903: Package 'Microsoft.Extensions.Caching.Memory' 8.0.0 has a known high severity vulnerability, https://github.com/advisories/GHSA-qj66-m88j-hmgj
New generator?(press enter for none in 5 seconds)

generating data
RSCG used by MSFT :10
npm run build
npm run serve
Zip, image, html, pdf : y/n
n
dotnet watch ⌚ Exited
dotnet watch ⏳ Waiting for a file to change before restarting dotnet...
D:\gth\RSCG_Examples\v2\GeneratorData\GeneratorData.csproj : warning NU1903: Package 'Microsoft.Extensions.Caching.Memory' 8.0.0 has a known high severity vulnerability, https://github.com/advisories/GHSA-qj66-m88j-hmgj
D:\gth\RSCG_Examples\v2\Generator\Generator.csproj : warning NU1903: Package 'Microsoft.Extensions.Caching.Memory' 8.0.0 has a known high severity vulnerability, https://github.com/advisories/GHSA-qj66-m88j-hmgj
D:\gth\RSCG_Examples\v2\Generator\Generator.csproj : warning NU1903: Package 'Microsoft.Extensions.Caching.Memory' 8.0.0 has a known high severity vulnerability, https://github.com/advisories/GHSA-qj66-m88j-hmgj
D:\gth\RSCG_Examples\v2\GeneratorData\GeneratorData.csproj : warning NU1903: Package 'Microsoft.Extensions.Caching.Memory' 8.0.0 has a known high severity vulnerability, https://github.com/advisories/GHSA-qj66-m88j-hmgj

";
        var lines = outputString.Split(['\n', '\r'], StringSplitOptions.RemoveEmptyEntries);

        var mockProcess = new IProcessWrapperCreateExpectations();
        var mockProcessStartInfo = new IProcessStartInfoCreateExpectations();
        mockProcess.Methods
            .Kill()
            //TODO: the callback of this should make WaitForExitAsync true
            //.Callback()
            .ExpectedCallCount(1);
        mockProcess.Methods
            .BeginOutputReadLine()
            .ExpectedCallCount(1)
            .RaiseOutDataReceived(outputString);
        mockProcess.Methods
            .BeginErrorReadLine()
            .ExpectedCallCount(1)
            ;
        mockProcess.Methods
            .Start()
            .ExpectedCallCount(1)
            
            ;
        mockProcess.Methods
            .WaitForExitAsync()
            .ExpectedCallCount(1)
            .Callback(async() => {
                await Task.Delay(60_000);
            })
            //.ReturnValue(Task.Delay(5_000))
            ;

        mockProcess.Properties.Getters
            .HasExited()
            .ExpectedCallCount(2)
            .ReturnValue(false);
    

        var mockConsole = new IConsoleWrapperCreateExpectations();
        mockConsole.Methods.WriteLine(Arg.Any<string>()).Callback(it => { });
        mockConsole.Methods.MarkupLineInterpolated(Arg.Any<FormattableString>()).Callback(it => { });
        mockConsole.Methods.Clear().ExpectedCallCount(1);

        var mockOptions = new Ioptions_gen_jsonCreateExpectations();
        mockOptions.Properties.Getters.ClearConsole().ExpectedCallCount(1).ReturnValue(true);
        mockOptions.Properties.Getters.TimeOut().ExpectedCallCount(1).ReturnValue(1_000);
        mockOptions.Methods.Validate(Arg.Any<ValidationContext>())
            .ExpectedCallCount(1)
            .ReturnValue([]);

        
        var mockConsoleInstance = mockConsole.Instance();
        var mockProcessInstance = mockProcess.Instance();
        Func<IProcessStartInfo, IProcessWrapper> f = (it => mockProcessInstance);
        var p = (new ProcessManager(f, NullLogger<ProcessManager>.Instance,mockOptions.Instance()));
        var tStart =p.StartProcessAsync([""],mockConsoleInstance, mockProcessStartInfo.Instance())
                
            ;
        
        var delay = Task.Delay(5_000);
        await Task.WhenAny(tStart, delay);
        // Assert
        mockProcess.Verify();
    }
}

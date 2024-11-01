﻿using Watch2;

namespace Test_Watch2;
[TestClass]

public class TestFromRealData
{
    [TestMethod]
    public void TestRSCGExamples()
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
        var mockConsole = new IConsoleWrapperCreateExpectations();
        
        mockProcess.Methods.Kill().ExpectedCallCount(1);
        mockConsole.Methods.WriteLine(Arg.Any<string>()).Callback(it => { });
        var mockConsoleInstance = mockConsole.Instance();
        var p = (new ProcessManager());
        for (var i = 0; i < lines.Length; i++)
        {
            
            p.HandleOutput(lines[i], mockConsoleInstance);
            
        }
        
        // Assert
        mockProcess.Verify();

    }
}

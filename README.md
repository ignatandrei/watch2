![Build Status](https://github.com/ignatandrei/watch2/actions/workflows/compileDeploy.yml/badge.svg)

[![NuGet Version](https://img.shields.io/nuget/v/Watch2.svg?style=flat)](https://www.nuget.org/packages/Watch2)


# watch2

Dotnet watch on steroids

It can
1. clear the console after each run 
   
2. wait before the watch process to start ( good if you have to run both tests and main program)

## NuGet Tool

You can find the `Watch2` NuGet Tool [here](https://www.nuget.org/packages/Watch2).

Install the package using the following command:

```

dotnet tool install --global Watch2 

```

## Usage

You can use everywhere you use `dotnet watch` . Just replace `dotnet watch` with `dotnet watch2` .

A watch2.json file is created in the current directory. You can modify it to your needs.

```json
{
  "version": 1,
  "ClearConsole": true,
  "TimeOut": 15000,
  "RunAfter":"bin\\Debug\\net10.0\\REP\\LightBDD_Report.html"

}
```

- `ClearConsole` : if true, it will clear the console after each run
- `TimeOut` : the time in milliseconds to wait for the process to start. 
- `version` : the version of the file. Do not change it.
- "RunAfter" : run once this step


## Visual Comparatio Diagrams

Let's say you have  3 dotnet watch for 3 projects ( a fast prototyping console, a Blazor UI and tests project) . If you modify a C# file that is in a common project, the 3 dotnet watch will be running at once, trying to build, and at least 2 will fail.

The options here is put timeout different ( like 1 second =1000 for testproject , 3 seconds for console and 5 seconds for Blazor UI - or viceversa )

You have below the diagram with .NET Watch 

![Concurrent Watch](https://raw.githubusercontent.com/wiki/ignatandrei/watch2/images/ConcurrentWatch.svg "Concurrent Watch")

and  below the diagram with Watch2 

![Wait Watch](https://raw.githubusercontent.com/wiki/ignatandrei/watch2/images/WaitWatch.svg "Wait Watch")



# SyncAndRun

A simple .NET Core console application to synchronize a folder prior to launching an application.

## Installation

Requires .NET Core 3.1 to be installed. Download the package for your OS and extract the contents into the folder you would like to keep in sync. You can install to a separate folder, but this application is designed to live in the target sync path.

## Configuration

Copy the `appsettings-template.json` file and rename to `appsettings.json`, then set the following properties inside `appsettings.json` (remember to use double backslash for Windows paths, e.g. "C:\\\\MyApp"):

* `SourcePath` - The source path to keep in sync.
* `TargetPath` - The target path to keep in sync. If `RunAndSync` is installed in the `TargetPath`, this can be set to `"."`.
* `KeyFile` - A single file to monitor for changes to trigger the sync. If the last modified date doesn't match between the `SourcePath` and the `TargetPath`, the sync will begin.
* `Program` - The application to run after sync is complete.
* `ProgramName` - The name of the application to run after sync, used for logging.
* `IgnoreDirs` - A list of directories to ignore when synchronizing.

## Usage

Run the `SyncAndRun` executable. If configured correctly you should get a terminal with simple logging while the sync process completes. Once finished, the `Program` configured in the `appsettings.json` file will be launched.

## Development

This application is developed using .NET Core 3.1. When testing, use the following command to override the default `appsettings.json` with the `appsettings.Development.json` file:

bash:

```sh
DOTNET_ENVIRONMENT="Development" dotnet run
```

PowerShell:

```pwsh
($env:DOTNET_ENVIRONMENT="Development") | dotnet run
```

## Why Does This Exist?

I have a very customized EmulationStation setup that I keep synchronized between several computers. I needed an easy way to make sure all of my machines keep their settings and themes up-to-date with the master copy on my network share.

## Bug Reports

I'm only using this on Windows, however I will maintain builds for Linux and OS X as supported in .NET Core. Feel free to create an issue or send a pull request for features and bug reports.

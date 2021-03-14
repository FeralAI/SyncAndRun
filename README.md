# SyncAndRun

A simple .NET Core console application to synchronize a folder prior to launching an application.

## Installation

Requires .NET Core 3.1 or later to be installed.

Download the package for your OS and extract the contents into the folder you would like to keep in sync. You can install to a separate folder, but this application is designed to live in the target sync path.

## Configuration

Open the `appsettings.json` and change the following properties:

* `SourcePath` - The source path to keep in sync.
* `TargetPath` - The target path to keep in sync. If `RunAndSync` is installed in the `TargetPath`, this can be set to `"."`.
* `KeyFile` - A single file to monitor for changes to trigger the sync. If the last modified date of that file in the `SourcePath` and the `TargetPath` doesn't match, the sync will begin.
* `Program` - The application to run after sync is complete.
* `ProgramName` - The name of the application to run after sync, used for logging.
* `IgnoreDirs` - A list of directories to ignore when synchronizing.

> NOTE: Remember to escape backslash with an extra backslash for Windows paths, e.g. "C:\\\\MyApp\\\\syncfolder"

## Usage

Run the `SyncAndRun` executable. If configured correctly you should get a terminal with simple logging while the sync process completes. Once finished, the `Program` configured in the `appsettings.json` file will be launched.

## Why Does This Exist?

I'll use my setup as an example use case.

I have a very customized EmulationStation setup that I keep synchronized between several computers. Something like rsync requires a bunch of setup, and I just wanted a way to pull the latest configuration from my home NAS when launching EmulationStation.

Here are the important bits of my folder structure:

``` text
📦EmulationStation
 ┣ 📂.emulationstation
 ┃ ┣ 📂themes
 ┃ ┣ 📜es_settings.cfg
 ┃ ┣ 📜es_systems.cfg
 ┃ ┗ ...
 ┣ 📂plugins
 ┣ 📂resources
 ┣ 📜.changelog.txt
 ┣ 📜appsettings.json
 ┣ 📜emulationstation.exe
 ┣ 📜EmulationStation.lnk
 ┣ 📜SyncAndRun.exe
 ┗ ...
```

A brief description of the important files:

* `.emulationstation/` - Configuration folder. Contains settings, themes, configured systems, etc.
* `.changelog.txt` - There's no one file that I will know will definitely be updated each time I want the folder to sync, so I use this as the `KeyFile` for triggering a sync from the `SourcePath`.
* `EmulationStation.lnk` - A Windows shortcut I created that executes `SyncAndRun.exe`, but has the icon of `emulationstation.exe`.

I keep the whole `EmulationStation` folder synchronized between my machines, since all my systems are running Windows and can use the same binaries. My `appsettings.json` file has the following options:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Warning"
    }
  },
  "AppSettings": {
    "IgnoreDirs": [
      ".git"
    ],
    "KeyFile": ".changelog.txt",
    "Program": "emulationstation.exe",
    "ProgramName": "EmulationStation",
    "SourcePath": "\\\\vault\\Storage\\Emulation\\Frontends\\EmulationStation",
    "TargetPath": "."
  }
}

```

I use the `EmulationStation.lnk` shortcut so it looks like I'm just running EmulationStation, but SyncAndRun will perform the sync prior to execution.

## Development

Requires .NET Core 3.1 SDK. Not much going on here, just a few helper methods for handling files and running a program. The `publish.bat` file will create single file binaries which helps to keep the install path clean.

### Dev Notes

The default build/run environment is `Production` and will use `appsettings.json` for configuration. When testing, use the following command to override the default `appsettings.json` with the `appsettings.Development.json` file:

bash:

```sh
DOTNET_ENVIRONMENT="Development" dotnet run
```

PowerShell:

```pwsh
($env:DOTNET_ENVIRONMENT="Development") | dotnet run
```

## Bug Reports

I'm only using this on Windows, however I will maintain builds for Linux and OS X as supported in .NET Core. Feel free to create an issue or send a pull request for features or bugs.

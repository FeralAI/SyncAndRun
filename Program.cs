using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SyncAndRun
{
	class Program
	{
		static ILogger<Program> Logger;
		static AppSettings Settings = new AppSettings();

		static void Main(string[] args)
		{
			Configure(args);
			Logger.LogInformation($"Starting SyncAndRun for {Settings.ProgramName}");
			SyncFiles();
			RunProgram();
		}

		private static void Configure(string[] args)
		{
			using var host = Host.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration((HostingAbstractionsHostExtensions, configuration) =>
				{
					IConfigurationRoot configurationRoot = configuration.Build();
					configurationRoot.GetSection(nameof(AppSettings)).Bind(Settings);
				})
				.Build();

			Logger = host.Services.GetRequiredService<ILogger<Program>>();
		}

		private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
		{
			// Get the subdirectories for the specified directory.
			var dir = new DirectoryInfo(sourceDirName);

			if (!dir.Exists)
			{
				throw new DirectoryNotFoundException($"Source directory does not exist or could not be found: {sourceDirName}");
			}

			var dirs = dir.GetDirectories();
			
			// If the destination directory doesn't exist, create it.
			Directory.CreateDirectory(destDirName);

			// Get the files in the directory and copy them to the new location.
			var files = dir.GetFiles();
			foreach (var file in files)
			{
				var destFile = new FileInfo(Path.Combine(destDirName, file.Name));
				if (file.LastWriteTimeUtc > destFile.LastWriteTimeUtc)
				{
					var tempPath = Path.Combine(destDirName, file.Name);
					file.CopyTo(tempPath, true);
				}
			}

			// If copying subdirectories, copy them and their contents to new location.
			if (copySubDirs)
			{
				foreach (var subdir in dirs)
				{
					if (Settings.IgnoreDirs.Contains(subdir.Name))
						continue;

					var tempPath = Path.Combine(destDirName, subdir.Name);
					DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
				}
			}
		}

		private static void RunProgram()
		{
			Logger.LogInformation($"Launching {Settings.ProgramName}");
			var es = new Process();
			es.StartInfo.FileName = Path.Combine(Settings.TargetPath, Settings.Program);
			es.StartInfo.WorkingDirectory = Settings.TargetPath;
			es.Start();
		}

		private static void SyncFiles()
		{
			var srcKeyFile = new FileInfo(Path.Combine(Settings.SourcePath, Settings.KeyFile));
			var dstKeyFile = new FileInfo(Path.Combine(Settings.TargetPath, Settings.KeyFile));
			if (srcKeyFile.LastWriteTimeUtc != dstKeyFile.LastWriteTimeUtc)
			{
				Logger.LogInformation($"Directory out of date, begin sync of {Settings.SourcePath} to {Settings.TargetPath}");
				DirectoryCopy(Settings.SourcePath, Settings.TargetPath, true);
				Logger.LogInformation("Sync complete");
			}
			else
			{
				Logger.LogInformation("Directory up to date");
			}
		}
	}
}

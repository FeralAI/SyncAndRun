using System.Collections.Generic;

namespace SyncAndRun
{
	public class AppSettings
	{
		public List<string> IgnoreDirs { get; set; } = new List<string>();
		public string KeyFile { get; set; }
		public string Program { get; set; }
		public string ProgramName { get; set; }
		public string SourcePath { get; set; }
		public string TargetPath { get; set; } = "."; // Use current directory if not set
	}
}
Database:
	Web.config: connectionStrings
GeoServer settings:
	Project settings: GeoServerURL (for user), GeoServerPort, GeoServerUser, GeoServerPassword, GeoServerPath, WorkspaceDir
Error:
	Could not find a part of the path '...\AtlasSolar\AtlasSolar\bin\roslyn\csc.exe'.
Solution:
	run in the Package Manager Console:
	Update-Package Microsoft.CodeDom.Providers.DotNetCompilerPlatform -r
Error:
	Out of memory error.
Solution:
	Visual Studio -> Options -> Projects and Solutions -> Web projects -> Use the 64 bit version of IIS Express for web sites and projects

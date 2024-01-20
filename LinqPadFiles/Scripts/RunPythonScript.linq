<Query Kind="Program">
  <NuGetReference>Microsoft.AspNetCore.App.Ref</NuGetReference>
  <NuGetReference>Python.Included</NuGetReference>
  <Namespace>Python.Runtime</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

void Main()
{
	var pyFilePath = @"D:\temp\test.py";
	var fileInfo = new FileInfo(pyFilePath);
	var runner = new PythonScriptRunner();
	var result = runner.RunScript(fileInfo);
}

/// <summary>
/// Class for running Python scripts using Python.NET.
/// Make sure Python.NET is installed and Python is properly set up in your environment.
/// </summary>
public class PythonScriptRunner
{
	/// <summary>
	/// Executes a Python script file.
	/// </summary>
	/// <param name="fileInfo">FileInfo object of the Python script.</param>
	/// <returns>The result of the Python script execution.</returns>
	/// <exception cref="ArgumentException">Thrown when the file does not exist or is not a Python script.</exception>
	public string RunScript(FileInfo fileInfo)
	{
		if (!fileInfo.Exists)
		{
			throw new ArgumentException("The file does not exist.", nameof(fileInfo));
		}

		if (fileInfo.Extension.ToLower() != ".py")
		{
			throw new ArgumentException("The file is not a Python script.", nameof(fileInfo));
		}
		
		Python.Runtime.Runtime.PythonDLL = "python38.dll";
		
		PythonEngine.DebugGIL = true;
		
		PythonEngine.Initialize();
		string result;

		try
		{
			using (Py.GIL()) // Ensure thread safety with the Global Interpreter Lock
			{
				dynamic py = Py.Import(Path.GetFileNameWithoutExtension(fileInfo.FullName));
				result = py.main(); // Assuming there's a 'main' function in your Python script
			}
		}
		finally
		{
			PythonEngine.Shutdown();
		}

		return result;
	}
}

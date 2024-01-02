<Query Kind="Statements" />

string srcPath = @"D:\repos\Frank.Libraries\src\"; // Change this to your src path
var directories = Directory.GetDirectories(srcPath, "Frank.Libraries.*");

foreach (var directory in directories)
{
	try
	{
		string directoryName = Path.GetFileName(directory);
		string testDirectoryName = directoryName + ".Tests";

		// Create new subdirectory for each library
		string newLibraryPath = Path.Combine(directory, directoryName);
		Directory.CreateDirectory(newLibraryPath);

		// Move existing files from old to new subdirectory
		foreach (var file in Directory.GetFiles(directory))
		{
			string newFilePath = Path.Combine(newLibraryPath, Path.GetFileName(file));
			File.Move(file, newFilePath);
		}

		// Move existing Tests directory if present
		string oldTestPath = Path.Combine(srcPath, testDirectoryName);
		if (Directory.Exists(oldTestPath))
		{
			string newTestPath = Path.Combine(directory, testDirectoryName);
			Directory.Move(oldTestPath, newTestPath);
		}

		// Move existing directories from old to new subdirectory
		foreach (var subDirectory in Directory.GetDirectories(directory))
		{
			if (Path.GetFileName(subDirectory) != directoryName) // Skip the newly created subdirectory
			{
				string newSubDirectoryPath = Path.Combine(newLibraryPath, Path.GetFileName(subDirectory));
				Directory.Move(subDirectory, newSubDirectoryPath);
			}
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine($"An error occurred while processing directory {directory}: {ex.Message}");
	}
}
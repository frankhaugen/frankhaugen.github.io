<Query Kind="Statements" />

string srcPath = @"D:\repos\Frank.Libraries\src\"; // Change this to your src path
var directories = Directory.GetDirectories(srcPath, "Frank.Libraries.*");

foreach (var directory in directories)
{
	try
	{
		string directoryName = Path.GetFileName(directory);
		string testDirectoryName = directoryName + ".Tests";

		// Move files back from new subdirectory to old directory
		string newLibraryPath = Path.Combine(directory, directoryName);
		foreach (var file in Directory.GetFiles(newLibraryPath))
		{
			string oldFilePath = Path.Combine(directory, Path.GetFileName(file));
			File.Move(file, oldFilePath);
		}

		// Move Tests directory back if it was moved
		string newTestPath = Path.Combine(directory, testDirectoryName);
		if (Directory.Exists(newTestPath))
		{
			string oldTestPath = Path.Combine(srcPath, testDirectoryName);
			Directory.Move(newTestPath, oldTestPath);
		}

		// Move directories back from new subdirectory to old directory
		foreach (var subDirectory in Directory.GetDirectories(newLibraryPath))
		{
			string oldSubDirectoryPath = Path.Combine(directory, Path.GetFileName(subDirectory));
			Directory.Move(subDirectory, oldSubDirectoryPath);
		}

		// Delete new subdirectory
		Directory.Delete(newLibraryPath);
	}
	catch (Exception ex)
	{
		Console.WriteLine($"An error occurred while undoing changes in directory {directory}: {ex.Message}");
	}
}
<Query Kind="Statements" />

string directoryPath = @"D:\repos\Frank.Libraries\src\"; // Replace with the directory you want to explore
var maxDepth = 2;
PrintDirectory(directoryPath, maxDepth);

void PrintDirectory(string path, int depth, string indent = "")
{
	DirectoryInfo dirInfo = new DirectoryInfo(path);
	Console.WriteLine($"{indent}{dirInfo.Name}");
	
	var files = dirInfo.EnumerateFiles("*.sln").Concat(dirInfo.EnumerateFiles("*.csproj"));
	
	foreach (var file in files)
	{
		Console.WriteLine($"{indent}-{file.Name}");
	}
	
	if (depth == 0)
	{
		return;
	}

	foreach (DirectoryInfo childDir in dirInfo.GetDirectories().Where(x => x.Name != "bin" && x.Name != "obj" && x.Name != ".idea" && x.Name != ".sarif"))
	{
		PrintDirectory(childDir.FullName, depth-1, indent + "    ");
		//Console.WriteLine($"{indent}{childDir.Name}");
	}
}

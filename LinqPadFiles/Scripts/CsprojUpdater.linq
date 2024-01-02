<Query Kind="Statements" />

var solutionFolder = new DirectoryInfo(@"D:\repos\ubllarsen\UblLarsen");

TransformAllCsprojInDirectory(solutionFolder);

static void TransformAllCsprojInDirectory(DirectoryInfo rootFolder)
{
	if (rootFolder == null || !rootFolder.Exists)
	{
		Console.WriteLine("Invalid directory.");
		return;
	}

	FileInfo[] files = rootFolder.GetFiles("*.csproj", SearchOption.AllDirectories);

	foreach (FileInfo file in files)
	{
		TransformOldCsprojToNew(file);
	}
}

static void TransformOldCsprojToNew(FileInfo oldFileInfo)
{
	if (oldFileInfo == null || !oldFileInfo.Exists)
	{
		Console.WriteLine("Invalid file path.");
		return;
	}

	string newCsprojPath = oldFileInfo.FullName; // Replace with your new .csproj file path

	XmlDocument oldDoc = new XmlDocument();
	oldDoc.Load(oldFileInfo.FullName);

	XmlDocument newDoc = new XmlDocument();

	// Create root Project node
	XmlElement projectNode = newDoc.CreateElement("Project");
	projectNode.SetAttribute("Sdk", "Microsoft.NET.Sdk");

	// Create PropertyGroup node
	XmlElement propertyGroupNode = newDoc.CreateElement("PropertyGroup");
	XmlElement outputType = newDoc.CreateElement("OutputType");
	outputType.InnerText = "Exe"; // Or "Library", based on your project type
	propertyGroupNode.AppendChild(outputType);

	XmlElement targetFramework = newDoc.CreateElement("TargetFramework");
	targetFramework.InnerText = "netcoreapp3.1"; // Replace with your target framework
	propertyGroupNode.AppendChild(targetFramework);

	projectNode.AppendChild(propertyGroupNode);

	// Create ItemGroup for Compile files
	XmlNodeList compileItems = oldDoc.GetElementsByTagName("Compile");
	if (compileItems.Count > 0)
	{
		XmlElement compileItemGroup = newDoc.CreateElement("ItemGroup");
		foreach (XmlNode item in compileItems)
		{
			XmlElement compile = newDoc.CreateElement("Compile");
			compile.SetAttribute("Include", item.Attributes["Include"].Value);
			compileItemGroup.AppendChild(compile);
		}
		projectNode.AppendChild(compileItemGroup);
	}

	newDoc.AppendChild(projectNode);
	newDoc.Save(newCsprojPath);

	Console.WriteLine($"Transformation complete. New .csproj saved as {newCsprojPath}");
}
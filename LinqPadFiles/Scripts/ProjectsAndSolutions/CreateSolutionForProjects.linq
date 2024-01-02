<Query Kind="Statements">
  <NuGetReference>Microsoft.Build</NuGetReference>
  <Namespace>Microsoft.Build.Construction</Namespace>
</Query>

// Replace this with the path to your .sln file.
string solutionPath = @"path\to\your.sln";

var solutionFile = SolutionFile.Parse(solutionPath);

foreach (var projectInSolution in solutionFile.ProjectsByGuid.Values)
{
	// Create a new solution file for the project.
	var newSolution = SolutionFile.Create(projectInSolution.ProjectName + ".sln");
	newSolution.AddProject(projectInSolution.AbsolutePath, projectInSolution.ProjectName, projectInSolution.ProjectType);

	// Save the new solution file next to the original solution.
	newSolution.Save(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(solutionPath), projectInSolution.ProjectName + ".sln"));
}
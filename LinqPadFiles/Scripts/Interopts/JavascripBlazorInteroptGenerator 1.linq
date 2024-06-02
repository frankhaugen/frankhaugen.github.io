<Query Kind="Program">
  <NuGetReference>Esprima</NuGetReference>
  <NuGetReference>Jint</NuGetReference>
  <NuGetReference>Microsoft.AspNetCore.App.Ref</NuGetReference>
  <NuGetReference>Microsoft.CodeAnalysis.CSharp</NuGetReference>
  <NuGetReference>Microsoft.CodeAnalysis.CSharp.Workspaces</NuGetReference>
  <Namespace>Jint</Namespace>
  <Namespace>Jint.Native</Namespace>
  <Namespace>Jint.Native.Function</Namespace>
  <Namespace>Jint.Native.Object</Namespace>
  <Namespace>Microsoft.CodeAnalysis</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp.Syntax</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Esprima</Namespace>
  <Namespace>Esprima.Ast</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

async Task Main()
{
	var path = "https://cdn.jsdelivr.net/npm/mermaid@10.9.0/dist/mermaid.js";
	var javascriptCode = await  Downloader.DowloadStringAsync(path);

	ParseJavaScript(javascriptCode);
}

public static void ParseJavaScript(string code)
{
	// Define the parsing options
	var scriptOptions = new ParserOptions { Tolerant = false };
	var moduleOptions = new ParserOptions { Tolerant = false };

	// Parse as script
	var scriptParser = new JavaScriptParser(scriptOptions);
	var script = scriptParser.ParseScript(code, strict: true);

	// Parse as module
	var moduleParser = new JavaScriptParser(moduleOptions);
	var module = moduleParser.ParseModule(code);

	// Determine type and handle accordingly
	if (script.Body.Count > 0 && script.Body[0] is ExpressionStatement)
	{
		Console.WriteLine("It's an expression.");
	}
	else if (module.Body.Count > 0 && (module.Body[0] is ImportDeclaration || module.Body[0] is ExportDeclaration))
	{
		Console.WriteLine("It's a module.");
	}
	else
	{
		Console.WriteLine("It's a script.");
	}
	
	script.Body[0].ChildNodes.Se.Dump();
	//.Select(x => x.Type).Distinct().ToList().Dump();
}
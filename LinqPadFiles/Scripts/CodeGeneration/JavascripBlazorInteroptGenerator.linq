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
	var generator = new JsInteropCodeFileGenerator();
	//	string javascriptCode = @"
	//    function myFunction(arg1, arg2) {
	//        // Some operation
	//    }
	//
	//    function anotherFunction() {
	//        // Another operation
	//    }";

	//var path = @"D:\repos\Frank.Blazor.Mermaid\Frank.Blazor.Mermaid\wwwroot\mermaid.js";
	//var javascriptCode = File.ReadAllText(path);

	var path = "https://cdn.jsdelivr.net/npm/mermaid@10.9.0/dist/mermaid.min.js";
	var javascriptCode = await  Downloader.DowloadStringAsync(path);

	var functionNames = ParseJavaScriptFunctions(javascriptCode);

	var generatedSyntaxTree = generator.Generate(functionNames);
	var code = generatedSyntaxTree.ToFullString();
	code.Dump();
}

public List<string> ParseJavaScriptFunctions(string scriptContent)
{
	var engine = new JavaScriptParser();

	engine.ParseExpression(scriptContent, true).Dump();
	engine.ParseModule(scriptContent).Dump();
	engine.ParseScript(scriptContent, strict: true).Dump();
	//var parsed = engine.ParseScript(scriptContent).Dump();
	

	//return parsed.ChildNodes.Cast<FunctionDeclaration>().Select(functionNode => functionNode.Id?.Name).ToList();
	return new();
}

public List<string> ParseJavaScriptFunctions1(string scriptContent)
{
	var engine = new Jint.Engine();
	engine.Execute(scriptContent);

	var functionNames = new List<string>();
	foreach (var statement in engine.Global.GetOwnProperties())
	{
		if (statement.Value.Value is Jint.Native.Function.Function)
		{
			functionNames.Add(statement.Key.ToString());
		}
	}

	return functionNames;
}



public class JsInteropCodeFileGenerator
{
	public CompilationUnitSyntax Generate(List<string> functionNames, string className = "GeneratedInterop", string namespaceName = "YourNamespace")
	{
		var classDeclaration = GenerateClass(className);

		foreach (var functionName in functionNames)
		{
			var method = GenerateMethod(functionName);

			classDeclaration = classDeclaration.AddMembers(method);
		}

		var namespaceDeclaration = GenerateNamespace(namespaceName, classDeclaration);

		var syntaxTree = SyntaxFactory.CompilationUnit()
			.AddUsings(
				SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Threading.Tasks")),
				SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("Microsoft.JSInterop")))
			.AddMembers(namespaceDeclaration)
			.NormalizeWhitespace();

		return syntaxTree;
	}

	private ClassDeclarationSyntax GenerateClass(string className)
	{
		return SyntaxFactory.ClassDeclaration(className)
			.AddModifiers(SyntaxFactory.Token(Microsoft.CodeAnalysis.CSharp.SyntaxKind.PublicKeyword));
	}

	private MethodDeclarationSyntax GenerateMethod(string methodName)
	{
		return SyntaxFactory.MethodDeclaration(
			SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
			SyntaxFactory.Identifier(methodName + "Async"))
			.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword), SyntaxFactory.Token(SyntaxKind.AsyncKeyword))
			.WithParameterList(
				SyntaxFactory.ParameterList(
					SyntaxFactory.SingletonSeparatedList(
						SyntaxFactory.Parameter(SyntaxFactory.Identifier("args"))
						.WithType(CreateArrayType())
					)
				)
			)
			.WithBody(CreateMethodBody(methodName));
	}

	private ArrayTypeSyntax CreateArrayType()
	{
		return SyntaxFactory.ArrayType(
			SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.ObjectKeyword)),
			SyntaxFactory.SingletonList(SyntaxFactory.ArrayRankSpecifier())
		);
	}

	private BlockSyntax CreateMethodBody(string methodName)
	{
		return SyntaxFactory.Block(
			SyntaxFactory.SingletonList<StatementSyntax>(
				SyntaxFactory.ExpressionStatement(
					CreateInvocationExpression(methodName)
				)
			)
		);
	}

	private InvocationExpressionSyntax CreateInvocationExpression(string methodName)
	{
		return SyntaxFactory.InvocationExpression(
			SyntaxFactory.MemberAccessExpression(
				SyntaxKind.SimpleMemberAccessExpression,
				SyntaxFactory.IdentifierName("JSRuntime"),
				SyntaxFactory.IdentifierName("InvokeAsync")
			),
			SyntaxFactory.ArgumentList(
				SyntaxFactory.SeparatedList<ArgumentSyntax>(
					new SyntaxNodeOrToken[]
					{
					SyntaxFactory.Argument(
						SyntaxFactory.LiteralExpression(
							SyntaxKind.StringLiteralExpression,
							SyntaxFactory.Literal(methodName)
						)
					),
					SyntaxFactory.Token(SyntaxKind.CommaToken),
					SyntaxFactory.Argument(
						SyntaxFactory.IdentifierName("args")
					)
					}
				)
			)
		);
	}

	private NamespaceDeclarationSyntax GenerateNamespace(string namespaceName, ClassDeclarationSyntax classDeclaration)
	{
		return SyntaxFactory.NamespaceDeclaration(SyntaxFactory.IdentifierName(namespaceName)).AddMembers(classDeclaration);
	}
}
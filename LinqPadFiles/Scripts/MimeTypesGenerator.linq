<Query Kind="Statements">
  <NuGetReference>Microsoft.AspNetCore.App.Ref</NuGetReference>
  <NuGetReference>Microsoft.CodeAnalysis.CSharp</NuGetReference>
  <Namespace>Microsoft.CodeAnalysis</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp.Syntax</Namespace>
  <Namespace>static Microsoft.CodeAnalysis.CSharp.SyntaxFactory</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Text.Json.Serialization</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>


var mimeSource = await Downloader.DowloadJsonAsync<List<MimeTypeSource>>("https://raw.githubusercontent.com/aryanbm/MIME-Types-Table/master/mimetypes-table.json");

mimeSource = mimeSource.OrderBy(x=>x.Extension).ToList();

MimeTypeStructGenerator.Generate().NormalizeWhitespace().ToFullString().Dump();

var fields = mimeSource.Select(x => FieldDeclarationGenerator.CreateField(x.Extension, InvocationExpressionGenerator.Generate(x)));

EnumerableClassGenerator.CreateEnumerableClass("MimeTypes", "MimeType", fields).NormalizeWhitespace().ToFullString().Dump();

public class MimeTypeSource
{
	[JsonPropertyName("Name")]
	public string Name { get; set; }

	[JsonPropertyName("MIME Type / Internet Media Type")]
	public string MediaType { get; set; }

	[JsonPropertyName("File Extension")]
	public string Extension { get; set; }

	[JsonPropertyName("More Details")]
	public string MoreDetails { get; set; }
}
public readonly record struct MimeType(string Name, string MediaType, string Extension, string MoreDetails)
{
};

public static class MimeTypeStructGenerator
{
	public static StructDeclarationSyntax Generate()
	{
		return StructDeclaration("MimeType")
			.WithModifiers(TokenList(
				Token(SyntaxKind.PublicKeyword),
				Token(SyntaxKind.ReadOnlyKeyword),
				Token(SyntaxKind.RecordKeyword)))
			.WithParameterList(ParameterList(SeparatedList(new[]
			{
				Parameter(Identifier("Name")).WithType(PredefinedType(Token(SyntaxKind.StringKeyword))),
				Parameter(Identifier("MediaType")).WithType(PredefinedType(Token(SyntaxKind.StringKeyword))),
				Parameter(Identifier("Extension")).WithType(PredefinedType(Token(SyntaxKind.StringKeyword))),
				Parameter(Identifier("MoreDetails")).WithType(PredefinedType(Token(SyntaxKind.StringKeyword)))
			}, new[] { Token(SyntaxKind.CommaToken), Token(SyntaxKind.CommaToken), Token(SyntaxKind.CommaToken) })))
			.WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
	}
}

public static class InvocationExpressionGenerator
{
	public static ImplicitObjectCreationExpressionSyntax Generate(MimeTypeSource source)
	{
		return ImplicitObjectCreationExpression()
			.WithArgumentList(
				ArgumentList(
					SeparatedList<ArgumentSyntax>(
						new SyntaxNodeOrToken[]
						{
							Argument(StringLiteralSyntaxGenerator.CreateStringLiteralExpression(source.Name)),
							Token(SyntaxKind.CommaToken),
							Argument(StringLiteralSyntaxGenerator.CreateStringLiteralExpression(source.MediaType)),
							Token(SyntaxKind.CommaToken),
							Argument(StringLiteralSyntaxGenerator.CreateStringLiteralExpression(source.Extension)),
                            Token(SyntaxKind.CommaToken),
							Argument(StringLiteralSyntaxGenerator.CreateStringLiteralExpression(source.MoreDetails))
						})));
	}
}

public static class StringLiteralSyntaxGenerator
{
	public static LiteralExpressionSyntax CreateStringLiteralExpression(string value)
	{
		return LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(value));
	}
}

public static class FieldDeclarationGenerator
{
	public static FieldDeclarationSyntax CreateField(string fieldName, ImplicitObjectCreationExpressionSyntax initializer)
	{
		return FieldDeclaration(
			VariableDeclaration(
				IdentifierName("MimeType"))
			.WithVariables(
				SingletonSeparatedList(
					VariableDeclarator(
						Identifier(CleanFieldName(fieldName)))
					.WithInitializer(
						EqualsValueClause(initializer)))))
			.WithModifiers(
				TokenList(
					new[]
					{
						Token(SyntaxKind.PublicKeyword),
						Token(SyntaxKind.StaticKeyword)
					}));
	}
	
	private static string CleanFieldName(string fieldName)
	{
		fieldName = StringHelper.RemoveNonAlphanumericCharacters(fieldName);
		fieldName = StringHelper.CapitalizeFirstLetter(fieldName);
		fieldName = StringHelper.DropAfterSpace(fieldName);
		fieldName = StringHelper.PrependUnderscoreIfNotLetter(fieldName);
		return fieldName;
	}
}

public static class EnumerableClassGenerator
{
	public static ClassDeclarationSyntax CreateEnumerableClass(string className, string typeName, IEnumerable<FieldDeclarationSyntax> fields)
	{
		// Generate yield return statements for each field
		var yieldStatements = new List<StatementSyntax>();
		foreach (var field in fields)
		{
			var fieldName = ((VariableDeclaratorSyntax)field.Declaration.Variables[0]).Identifier.Text;
			fieldName = StringHelper.RemoveNonAlphanumericCharacters(fieldName);
			fieldName = StringHelper.CapitalizeFirstLetter(fieldName);
			fieldName = StringHelper.DropAfterSpace(fieldName);
			fieldName = StringHelper.PrependUnderscoreIfNotLetter(fieldName);

			yieldStatements.Add(YieldStatement(SyntaxKind.YieldReturnStatement, IdentifierName(fieldName)));
		}

		// Create GetEnumerator method
		var getClass = MethodDeclaration(
				GenericName(Identifier("IEnumerator"))
					.AddTypeArgumentListArguments(IdentifierName(typeName)),
				Identifier("GetEnumerator"))
			.WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
			.WithBody(Block(yieldStatements));

		// Create the class
		var classDeclaration = ClassDeclaration(className)
			.AddModifiers(Token(SyntaxKind.PublicKeyword))
			.AddBaseListTypes(SimpleBaseType(
				GenericName(Identifier("IEnumerable"))
					.AddTypeArgumentListArguments(IdentifierName(typeName))))
			.AddMembers(fields.ToArray())
			.AddMembers(getClass);

		return classDeclaration;
	}
}

public static class StringHelper
{
	// 1. Capitalize first character if it's a letter
	public static string CapitalizeFirstLetter(string input)
	{
		if (string.IsNullOrEmpty(input) || !char.IsLetter(input[0]))
		{
			return input;
		}

		return char.ToUpper(input[0]) + input.Substring(1);
	}

	// 2. Prepend an underscore if the first character isn't a letter
	public static string PrependUnderscoreIfNotLetter(string input)
	{
		if (string.IsNullOrEmpty(input) || char.IsLetter(input[0]))
		{
			return input;
		}

		return "_" + input;
	}

	// 3. Remove any non-alphanumeric characters
	public static string RemoveNonAlphanumericCharacters(string input)
	{
		return Regex.Replace(input, "[^a-zA-Z0-9]", "");
	}

	// 4. If a string contains spaces, anything after and including the first space is dropped
	public static string DropAfterSpace(string input)
	{
		var index = input.IndexOf(' ');
		return index == -1 ? input : input.Substring(0, index);
	}

	// 5. Process a list of strings, appending numbers to duplicates
	public static List<string> ProcessDuplicates(List<string> inputs)
	{
		var result = new List<string>();
		var counts = new Dictionary<string, int>();

		foreach (var input in inputs)
		{
			if (counts.ContainsKey(input))
			{
				counts[input]++;
				result.Add(input + counts[input].ToString());
			}
			else
			{
				counts[input] = 0;
				result.Add(input);
			}
		}

		return result;
	}
}
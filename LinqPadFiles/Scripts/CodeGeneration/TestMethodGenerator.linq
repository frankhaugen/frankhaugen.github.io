<Query Kind="Program">
  <NuGetReference>Microsoft.CodeAnalysis.CSharp</NuGetReference>
  <Namespace>Microsoft.CodeAnalysis</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp.Syntax</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Runtime.CompilerServices</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <RemoveNamespace>System.Collections</RemoveNamespace>
  <RemoveNamespace>System.Data</RemoveNamespace>
  <RemoveNamespace>System.Diagnostics</RemoveNamespace>
  <RemoveNamespace>System.IO</RemoveNamespace>
  <RemoveNamespace>System.Linq.Expressions</RemoveNamespace>
  <RemoveNamespace>System.Text.RegularExpressions</RemoveNamespace>
  <RemoveNamespace>System.Threading</RemoveNamespace>
  <RemoveNamespace>System.Transactions</RemoveNamespace>
  <RemoveNamespace>System.Xml</RemoveNamespace>
  <RemoveNamespace>System.Xml.Linq</RemoveNamespace>
  <RemoveNamespace>System.Xml.XPath</RemoveNamespace>
</Query>

void Main()
{
    var generator = new TestMethodGenerator();
    var type = typeof(StringBuilder);
    
    var methods = type.GetMethods();
    
    var nodes = new List<SyntaxNode>();

    foreach (var method in methods)
    {
        var node = generator.GenerateTestMethod(method);
        nodes.Add(node);
    }
    
    nodes.ForEach(x => x.ToFullString().Dump());
}

public class TestMethodGenerator
{
    public MethodDeclarationSyntax GenerateTestMethod(MethodInfo method, string suffix = "")
    {
        var returnType = method.GetReturnStatement();
        
        var statements = new List<StatementSyntax>();

        statements.AddComment("// Arrange");
        statements.AddRange(GetArrangeStatements(method));
        statements.AddLineBreak();

        statements.AddComment("// Act");
        statements.AddRange(GetActStatements(method));
        statements.AddLineBreak();

        statements.AddComment("// Assert");
        statements.AddRange(GetAssertStatements(method));

        var methodBody = SyntaxFactory.Block(statements);
        var methodNode = SyntaxFactory.MethodDeclaration(returnType, $"{method.Name}Test{suffix}")
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
            .AddAttributeLists(SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(
                SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("Fact"))
            )))
            //.WithParameterList(methodParameters)
            .WithBody(methodBody);

        return methodNode.NormalizeWhitespace();
    }

    private IEnumerable<StatementSyntax> GetAssertStatements(MethodInfo method)
    {
        var statements = new List<StatementSyntax>();
        statements.Add(SyntaxFactory.ParseStatement("_outputHelper.WriteLine(result);"));
        statements.Add(SyntaxFactory.ParseStatement("result.ToString().Should().NotBeNullOrEmpty();"));
        return statements;
    }
    
    private IEnumerable<StatementSyntax> GetActStatements(MethodInfo method)
    {
        var statements = new List<StatementSyntax>();
        var methodType = method.GetMethodVariant();
        var methodParameters = method.GetParameters();
        
        statements.Add(SyntaxFactory.ParseStatement("// Act"));

        var awaitPrefix = "";
        if (method.IsAsync())
        {
            awaitPrefix = "await ";
        }

        ExpressionSyntax invocationExpression;
        switch (methodType)
        {
            case MethodVariant.Instance:
                invocationExpression = SyntaxFactory.ParseExpression($"_sut.{method.Name}({string.Join(", ", methodParameters.Where(x => !x.HasDefaultValue).Select(x => x.Name))})");
                break;
            case MethodVariant.Extension:
                invocationExpression = SyntaxFactory.ParseExpression($"{method.Name}({string.Join(", ", methodParameters.Select(x => x.Name))})");
                break;
            case MethodVariant.Static:
                invocationExpression = SyntaxFactory.ParseExpression($"{method.DeclaringType?.GetFriendlyName()}.{method.Name}({string.Join(", ", methodParameters.Select(x => x.Name))})");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        var resultStatement = SyntaxFactory.LocalDeclarationStatement
        (
            SyntaxFactory.VariableDeclaration
            (
                SyntaxFactory.IdentifierName("var")
            )
            .WithTrailingTrivia(SyntaxFactory.ParseTrailingTrivia(" "))
            .WithVariables
            (
                SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>
                (
                    SyntaxFactory.VariableDeclarator
                    (
                        SyntaxFactory.Identifier("result")
                    )
                    .WithTrailingTrivia(SyntaxFactory.ParseTrailingTrivia(" "))
                    .WithInitializer
                    (
                        SyntaxFactory.EqualsValueClause
                        (
                            invocationExpression
                        )
                    )
                )
            )
        );
        statements.Add(resultStatement);

        return statements;
    }
    
    private IEnumerable<StatementSyntax> GetArrangeStatements(MethodInfo method)
    {
        var statements = new List<StatementSyntax>();
        var methodType = method.GetMethodVariant();
        var methodParameters = method.GetParameters();
        statements.Add(SyntaxFactory.ParseStatement("// Arrange"));

        if (methodType == MethodVariant.Instance)
        {
            statements.Add(SyntaxFactory.ParseStatement($"var _sut = new {method.DeclaringType}();"));
        }

        if (methodType == MethodVariant.Extension)
        {
            var thisParameter = methodParameters.First();
            methodParameters.RemoveAt(0);
            statements.Add(SyntaxFactory.ParseStatement($"var {thisParameter.Name.ToCamelcase()} = default({thisParameter.ParameterType.Name});"));
        }

        foreach (var param in methodParameters)
        {
            statements.Add(SyntaxFactory.ParseStatement($"var {param.Name?.ToCamelcase()} = default({param.ParameterType.Name});"));
        }
        
        return statements;
    }
}


public static class EnumerableExtensions
{
    public static void RemoveAt<T>(this IEnumerable<T> source, int index)
    {
    }

    public static void AddComment(this List<StatementSyntax> source, string comment)
    {
        var statement = SyntaxFactory.ParseStatement("").WithLeadingTrivia(SyntaxFactory.Comment(comment));
        source.Add(statement);
    }

    public static void AddLineBreak(this List<StatementSyntax> source, int number = 1)
    {
        for (int i = 0; i < number; i++)
        {
            var comment = SyntaxFactory.Comment(" ");
            var statement = SyntaxFactory.ParseStatement("").WithLeadingTrivia(comment);
            source.Add(statement);
        }
    }
}

public static class StringExtensions
{
    public static string ToCamelcase(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        return value.Substring(0, 1).ToLower() + value.Substring(1);
    }
}

public static class MethodInfoExtensions
{
    public static bool IsPropertyMethod(this MethodInfo methodInfo) => methodInfo.IsSpecialName && (methodInfo.Name.StartsWith("get_") || methodInfo.Name.StartsWith("set_"));
    
    public static bool IsBaseMethod(this MethodInfo methodInfo)
    {
        if (methodInfo.IsPrivate)
        {
            return true;
        }
        
        var type = methodInfo.DeclaringType;
        
        if (type?.BaseType != null && type.BaseType.GetMethods().Select(x => x.Name).Contains(methodInfo.Name))
        {
            return true;
        }

        // Check if the method is declared in the base class
        if (methodInfo.DeclaringType != null && methodInfo.DeclaringType != methodInfo.ReflectedType)
        {
            // Check if the method is not overridden in the derived class
            var baseMethodInfo = methodInfo.DeclaringType.GetMethod(methodInfo.Name, methodInfo.GetParameters().Select(p => p.ParameterType).ToArray());
            if (baseMethodInfo != null && baseMethodInfo.Equals(methodInfo))
            {
                return true;
            }
        }
        return false;
    }
    
    public static bool IsAsync(this MethodInfo methodInfo) => methodInfo.ReturnType == typeof(Task) || methodInfo.ReturnType.IsGenericType && methodInfo.ReturnType.GetGenericTypeDefinition() == typeof(Task<>);

    public static MethodVariant GetMethodVariant(this MethodInfo methodInfo)
    {
        if (methodInfo.IsDefined(typeof(ExtensionAttribute), false))
            return MethodVariant.Extension;
        if (methodInfo.IsStatic)
            return MethodVariant.Static;
        return MethodVariant.Instance;
    }

    public static TypeSyntax GetReturnStatement(this MethodInfo method)
    {
        var isAsync = method.IsAsync();
        var returnType = typeof(object);

        if (isAsync)
        {
            returnType = typeof(Task);
        }
        else
        {
            returnType = typeof(void);
        }

        return SyntaxFactory.ParseTypeName(returnType.Name);
    }
}

public static class TypeExtensions
{
    public static bool HasBaseType(this Type type) => type.BaseType != null;
    
    public static bool IsStatic(this Type type) => type is { IsAbstract: true, IsSealed: true };

    public static TypeVariant GetTypeVariant(this Type type)
    {
        var typeAttributes = type.Attributes;

        if (typeAttributes.HasFlag(TypeAttributes.Abstract & TypeAttributes.Sealed))
            return TypeVariant.Static;
        if (typeAttributes.HasFlag(TypeAttributes.ExplicitLayout))
            return TypeVariant.Static;
        if (typeAttributes.HasFlag(TypeAttributes.Class))
            return TypeVariant.Class;
        return TypeVariant.Virtual;
    }

    public static IEnumerable<string> GetReferencedNamespaces(this Type type)
    {
        var types = new List<Type>();

        types.AddRange(type.GetConstructors().SelectMany(x => x.GetParameters().Select(y => y.ParameterType)));
        types.AddRange(type.GetProperties().Select(x => x.PropertyType));
        types.AddRange(type.GetFields().Select(x => x.FieldType));
        types.AddRange(type.GetMethods().SelectMany(x => x.GetParameters().Select(y => y.ParameterType)));
        types.AddRange(type.GetMethods().Select(x => x.ReturnType));

        return types.Select(x => x.Namespace).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct();
    }

    public static string GetFriendlyName(this Type type, bool fullyQualified = false)
    {
        var typeName = fullyQualified
            ? GetFullNameSansTypeParameters(type).Replace("+", ".")
            : type.Name;

        if (type.IsGenericType)
        {
            var genericArgumentIds = type.GetGenericArguments()
                .Select(t => GetFriendlyName(t, fullyQualified))
                .ToArray();

            return new StringBuilder(typeName)
                .Replace(string.Format("`{0}", genericArgumentIds.Count()), string.Empty)
                .Append(string.Format("<{0}>", string.Join(",", genericArgumentIds).TrimEnd(',')))
                .ToString();
        }

        return typeName;
    }


    private static string GetFullNameSansTypeParameters(Type type)
    {
        var fullName = type.FullName;
        if (string.IsNullOrEmpty(fullName))
            fullName = type.Name;
        var chopIndex = fullName.IndexOf("[[");
        return (chopIndex == -1) ? fullName : fullName.Substring(0, chopIndex);
    }
}

public enum TypeVariant
{
    Static,
    Abstract,
    Virtual,
    Class
}
public enum MethodVariant
{
    Instance,
    Extension,
    Static
}
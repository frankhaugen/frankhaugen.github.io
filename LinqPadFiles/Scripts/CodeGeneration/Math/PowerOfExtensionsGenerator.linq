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
    
}

private static List<string> Types => new()
{
    "int",
    "uint",
    "short",
    "ushort",
    "long",
    "ulong",
    "float",
    "double",
    "decimal"
};

public string Generate(string namespaceName, string className)
{
    var usings = SyntaxFactory.List(new[]
    {
        SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System"))
    });

    var members = new List<MemberDeclarationSyntax>();
    foreach (var type in Types)
    {
        var powerOfMethod = GenerateMethod(type);

        members.Add(powerOfMethod);
        members.Add(sqrtOfMethod);
    }

    var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(namespaceName))
        .WithUsings(usings)
        .WithMembers(SyntaxFactory.SingletonList<MemberDeclarationSyntax>(
            SyntaxFactory.ClassDeclaration(className)
                .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword), SyntaxFactory.Token(SyntaxKind.StaticKeyword)))
                .WithMembers(SyntaxFactory.List(members))
        ));

    var compilationUnit = SyntaxFactory.CompilationUnit()
        .WithMembers(SyntaxFactory.SingletonList<MemberDeclarationSyntax>(@namespace));

    return compilationUnit.NormalizeWhitespace().ToFullString();
}

MethodDeclarationSyntax GenerateMethod(string type)
{
    return SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName(type), "Pow")
            .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword), SyntaxFactory.Token(SyntaxKind.StaticKeyword)))
            .WithParameterList(SyntaxFactory.ParameterList(SyntaxFactory.SingletonSeparatedList(
                SyntaxFactory.Parameter(SyntaxFactory.Identifier("source"))
                    .WithType(SyntaxFactory.ParseTypeName(type))
            )))
            .WithBody(SyntaxFactory.Block(
                SyntaxFactory.LocalDeclarationStatement(
                    SyntaxFactory.VariableDeclaration(SyntaxFactory.ParseTypeName(type))
                        .WithVariables(SyntaxFactory.SingletonSeparatedList(
                            SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier("result"))
                                .WithInitializer(SyntaxFactory.EqualsValueClause(SyntaxFactory.IdentifierName("source")))
                        ))
                ),
                SyntaxFactory.ForStatement(
                    SyntaxFactory.Block(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.AssignmentExpression(
                                SyntaxKind.MultiplyAssignmentExpression,
                                SyntaxFactory.IdentifierName("result"),
                                SyntaxFactory.IdentifierName("source")
                            )
                        )
                    ),
                    SyntaxFactory.BinaryExpression(
                        SyntaxKind.LessThanExpression,
                        SyntaxFactory.IdentifierName("i"),
                        SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal("value"))
                    ),
                    SyntaxFactory.PostfixUnaryExpression(SyntaxKind.PostIncrementExpression, SyntaxFactory.IdentifierName("i"))
                ),
                SyntaxFactory.ReturnStatement(SyntaxFactory.IdentifierName("result"))
            ));

    var sqrtOfMethod = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName(type), "Sqrt")
        .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword), SyntaxFactory.Token(SyntaxKind.StaticKeyword)))
        .WithParameterList(SyntaxFactory.ParameterList(SyntaxFactory.SingletonSeparatedList(
            SyntaxFactory.Parameter(SyntaxFactory.Identifier("source"))
                .WithType(SyntaxFactory.ParseTypeName(type))
        )))
        .WithBody(SyntaxFactory.Block(
            SyntaxFactory.IfStatement(
                SyntaxFactory.BinaryExpression(
                    SyntaxKind.LogicalOrExpression,
                    SyntaxFactory.BinaryExpression(
                        SyntaxKind.EqualsExpression,
                        SyntaxFactory.IdentifierName("source"),
                        SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal("0"))
                    ),
                    SyntaxFactory.BinaryExpression(
                        SyntaxKind.EqualsExpression,
                        SyntaxFactory.IdentifierName("source"),
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName("DecimalConstants"),
                            SyntaxFactory.IdentifierName("SmallestNonZeroDec")
                        )
                    )
                ),
                SyntaxFactory.Block(
                    SyntaxFactory.ReturnStatement(SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal("0")))
                )
            ),
            SyntaxFactory.LocalDeclarationStatement(
                SyntaxFactory.VariableDeclaration(SyntaxFactory.ParseTypeName(type))
                    .WithVariables(SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier("halfS"))
                            .WithInitializer(SyntaxFactory.EqualsValueClause(
                                SyntaxFactory.BinaryExpression(
                                    SyntaxKind.DivideExpression,
                                    SyntaxFactory.IdentifierName("source"),
                                    SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal("2"))
                                )
                            ))
                    ))
            ),
            SyntaxFactory.LocalDeclarationStatement(
                SyntaxFactory.VariableDeclaration(SyntaxFactory.ParseTypeName(type))
                    .WithVariables(SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier("lastX"))
                            .WithInitializer(SyntaxFactory.EqualsValueClause(
                                type.StartsWith("u")
                                    ? SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName(type),
                                        SyntaxFactory.IdentifierName("MinValue")
                                    )
                                    : SyntaxFactory.CastExpression(
                                        SyntaxFactory.ParseTypeName(type),
                                        SyntaxFactory.PrefixUnaryExpression(
                                            SyntaxKind.UnaryMinusExpression,
                                            SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal("1"))
                                        )
                                    )
                            ))
                    ))
            ),
            SyntaxFactory.LocalDeclarationStatement(
                SyntaxFactory.VariableDeclaration(SyntaxFactory.ParseTypeName(type))
                    .WithVariables(SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier("nextX"))
                    ))
            ),
            SyntaxFactory.LocalDeclarationStatement(
                SyntaxFactory.VariableDeclaration(SyntaxFactory.ParseTypeName("double"))
                    .WithVariables(SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier("x"))
                            .WithInitializer(SyntaxFactory.EqualsValueClause(
                                SyntaxFactory.ParseExpression($"Math.Sqrt(Convert.ToDouble(source))")
                            ))
                    ))
            ),
            SyntaxFactory.WhileStatement(
                SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression),
                SyntaxFactory.Block(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName("nextX"),
                            SyntaxFactory.ConditionalExpression(
                                SyntaxFactory.BinaryExpression(
                                    SyntaxKind.LogicalOrExpression,
                                    SyntaxFactory.BinaryExpression(
                                        SyntaxKind.EqualsExpression,
                                        SyntaxFactory.IdentifierName("type"),
                                        SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal("ushort"))
                                    ),
                                    SyntaxFactory.BinaryExpression(
                                        SyntaxKind.EqualsExpression,
                                        SyntaxFactory.IdentifierName("type"),
                                        SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal("short"))
                                    )
                                ),
                                SyntaxFactory.CastExpression(
                                    SyntaxFactory.ParseTypeName(type),
                                    SyntaxFactory.BinaryExpression(
                                        SyntaxKind.AddExpression,
                                        SyntaxFactory.BinaryExpression(
                                            SyntaxKind.DivideExpression,
                                            SyntaxFactory.IdentifierName("x"),
                                            SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal("2"))
                                        ),
                                        SyntaxFactory.BinaryExpression(
                                            SyntaxKind.DivideExpression,
                                            SyntaxFactory.IdentifierName("halfS"),
                                            SyntaxFactory.IdentifierName("x")
                                        )
                                    )
                                ),
                                SyntaxFactory.BinaryExpression(
                                    SyntaxKind.AddExpression,
                                    SyntaxFactory.BinaryExpression(
                                        SyntaxKind.DivideExpression,
                                        SyntaxFactory.IdentifierName("x"),
                                        SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal("2"))
                                    ),
                                    SyntaxFactory.BinaryExpression(
                                        SyntaxKind.DivideExpression,
                                        SyntaxFactory.IdentifierName("halfS"),
                                        SyntaxFactory.IdentifierName("x")
                                    )
                                )
                            )
                        )
                    ),
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.LogicalOrExpression,
                            SyntaxFactory.BinaryExpression(
                                SyntaxKind.EqualsExpression,
                                SyntaxFactory.IdentifierName("nextX"),
                                SyntaxFactory.IdentifierName("x")
                            ),
                            SyntaxFactory.BinaryExpression(
                                SyntaxKind.EqualsExpression,
                                SyntaxFactory.IdentifierName("nextX"),
                                SyntaxFactory.IdentifierName("lastX")
                            )
                        ),
                        SyntaxFactory.Block(
                            SyntaxFactory.BreakStatement()
                        )
                    ),
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName("lastX"),
                            SyntaxFactory.IdentifierName("x")
                        )
                    ),
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName("x"),
                            SyntaxFactory.IdentifierName("nextX")
                        )
                    )
                )
            ),
            SyntaxFactory.ReturnStatement(SyntaxFactory.IdentifierName("nextX"))
        ));
}

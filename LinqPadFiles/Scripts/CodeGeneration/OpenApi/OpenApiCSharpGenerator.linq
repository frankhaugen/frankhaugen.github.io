<Query Kind="Statements">
  <NuGetReference>Microsoft.CodeAnalysis.CSharp</NuGetReference>
  <NuGetReference>Microsoft.OpenApi.Readers</NuGetReference>
  <Namespace>Microsoft.CodeAnalysis</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp.Syntax</Namespace>
  <Namespace>Microsoft.OpenApi.Models</Namespace>
  <Namespace>Microsoft.OpenApi.Readers</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Runtime.CompilerServices</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <RemoveNamespace>System.Data</RemoveNamespace>
  <RemoveNamespace>System.Linq.Expressions</RemoveNamespace>
  <RemoveNamespace>System.Text.RegularExpressions</RemoveNamespace>
  <RemoveNamespace>System.Threading</RemoveNamespace>
  <RemoveNamespace>System.Transactions</RemoveNamespace>
  <RemoveNamespace>System.Xml</RemoveNamespace>
  <RemoveNamespace>System.Xml.Linq</RemoveNamespace>
  <RemoveNamespace>System.Xml.XPath</RemoveNamespace>
</Query>

var schemaWalker = new OpenApiSchemaWalker();

// Load the OpenAPI specification from a file
var openApiDocument = new OpenApiStreamReader().Read(File.OpenRead(@"C:\Users\frank\Downloads\swagger (1).json"), out var diagnostic);


var schemas = openApiDocument.Components.Schemas;

Parallel.ForEach(schemas, schema =>
{
    schemaWalker.Walk(schema.Key, schema.Value);
});

schemaWalker.Classes.Dump();


class OpenApiSchemaWalker
{
    public OpenApiSchemaWalker()
    {
        Classes = new FastLookupDictionary<string, ClassDeclarationSyntax>();
    }

    public FastLookupDictionary<string, ClassDeclarationSyntax> Classes { get; private set; }

    public void Walk(string name, OpenApiSchema schema)
    {
        var isClass = IsClassSchema(schema);

        if (isClass)
        {
            // Create a new class declaration
            var classDeclaration = ToClassSyntax(name, schema);

            // Add the class declaration to the dictionary of classes
            Classes.Add(name, classDeclaration);

            // Recursively walk through each property schema
            foreach (var property in schema.Properties)
            {
                var propertyName = property.Key;
                var propertySchema = property.Value;

                if(IsClassSchema(propertySchema)) Walk(propertyName, propertySchema);
            }
        }
    }

    private ClassDeclarationSyntax ToClassSyntax(string name, OpenApiSchema schema)
    {
        // Create a new class declaration
        var classDeclaration = SyntaxFactory.ClassDeclaration(name)
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

        // Add a property declaration for each schema property
        if (schema.Properties != null)
        {
            foreach (var property in schema.Properties)
            {
                var propertyDeclaration = ToPropertySyntax(property.Key, property.Value);
                classDeclaration = classDeclaration.AddMembers(propertyDeclaration);
            }
        }

        return classDeclaration;
    }

    private PropertyDeclarationSyntax ToPropertySyntax(string name, OpenApiSchema schema)
    {
        var typeName = schema.Type ?? "object";
        
        // Create a new property declaration
        var propertyDeclaration = SyntaxFactory.PropertyDeclaration(
                SyntaxFactory.ParseTypeName(typeName),
                name)
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
            .AddAccessorListAccessors(
                SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));

        return propertyDeclaration;
    }

    private static bool IsClassSchema(OpenApiSchema schema)
    {
        return schema.Properties != null && schema.Properties.Count > 0;
    }
}

public class FastLookupDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>> where TKey : notnull
{
    private readonly Dictionary<TKey, List<TValue>> _dictionary = new Dictionary<TKey, List<TValue>>();

    public void Add(TKey key, TValue value)
    {
        if (!_dictionary.TryGetValue(key, out var list))
        {
            list = new List<TValue>();
            _dictionary.Add(key, list);
        }

        list.Add(value);
        
        key.Dump();
    }

    public bool ContainsKey(TKey key)
    {
        return _dictionary.ContainsKey(key);
    }

    public IEnumerable<TValue> this[TKey key]
    {
        get
        {
            if (!_dictionary.TryGetValue(key, out var list))
            {
                list = new List<TValue>();
                _dictionary.Add(key, list);
            }

            return list;
        }
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        foreach (var pair in _dictionary)
        {
            foreach (var value in pair.Value)
            {
                yield return new KeyValuePair<TKey, TValue>(pair.Key, value);
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
<Query Kind="Statements">
  <NuGetReference>Microsoft.CodeAnalysis.CSharp</NuGetReference>
  <NuGetReference>XmlSchemaClassGenerator-beta</NuGetReference>
  <Namespace>Microsoft.CodeAnalysis.CSharp</Namespace>
  <Namespace>Microsoft.CodeAnalysis.CSharp.Syntax</Namespace>
  <Namespace>Microsoft.CSharp</Namespace>
  <Namespace>System.CodeDom</Namespace>
  <Namespace>System.CodeDom.Compiler</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Xml.Schema</Namespace>
  <Namespace>System.Xml.Serialization</Namespace>
  <Namespace>XmlSchemaClassGenerator</Namespace>
</Query>





var scriptFolder = new FileInfo(Util.CurrentQueryPath).Directory;
var scriptFolderPath = new FileInfo(Util.CurrentQueryPath).Directory!.FullName;

var serializer = new XmlSchemaClassGenerator.Generator();
serializer.Generate(new List<string>() {scriptFolder.GetDirectories().First().GetFiles("*.xsd").First().FullName});



void RunRoslyAndOutput(string code, DirectoryInfo outputDirectory, string @namespace)
{
    var text = code;
    var syntaxTree = CSharpSyntaxTree.ParseText(text);
    var root = syntaxTree.GetRoot() as CompilationUnitSyntax;
    if (root == null) throw new NullReferenceException();
    var enums = root.Members.OfType<EnumDeclarationSyntax>();
    var types = root.Members.OfType<TypeDeclarationSyntax>();

    var usings = new StringBuilder()
    .AppendLine($"namespace {@namespace};")
    .ToString();

    foreach (var element in types) File.WriteAllText(Path.Combine(outputDirectory.FullName, element.Identifier.ToString() + ".cs"), usings + element.ToFullString());
    foreach (var element in enums) File.WriteAllText(Path.Combine(outputDirectory.FullName, element.Identifier.ToString() + ".cs"), usings + element.ToFullString());
}
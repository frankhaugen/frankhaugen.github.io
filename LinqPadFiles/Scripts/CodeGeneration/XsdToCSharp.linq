<Query Kind="Statements">
  <NuGetReference>Microsoft.CodeAnalysis.CSharp</NuGetReference>
  <NuGetReference>XObjectsCodeGen</NuGetReference>
  <NuGetReference>XObjectsCore</NuGetReference>
  <Namespace>Microsoft.CodeAnalysis.CSharp</Namespace>
  <Namespace>Microsoft.CodeAnalysis.Text</Namespace>
  <Namespace>System.Configuration</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Xml.Resolvers</Namespace>
  <Namespace>System.Xml.Schema</Namespace>
  <Namespace>Xml.Schema.Linq</Namespace>
  <Namespace>Xml.Schema.Linq.CodeGen</Namespace>
  <Namespace>Xml.Schema.Linq.Extensions</Namespace>
</Query>














var result = XObjectsCoreGenerator
    .Generate(new DirectoryInfo("C:/repos/ubllarsen/UblLarsen/UBL-2.1/xsd").EnumerateFiles("*.xsd", SearchOption.AllDirectories).Select(x => x.FullName), new LinqToXsdSettings(true)
    {
        EnableServiceReference = true,
        NullableReferences = true
    });

result.Dump();
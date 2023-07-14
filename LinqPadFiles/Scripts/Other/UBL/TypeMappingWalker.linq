<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Xml.Serialization</Namespace>
</Query>

#load ".\XsdReader"

var path = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "xsd");
path.Dump();
var directory = new DirectoryInfo(path);
var mappings = GetTypeMappingsFromDirectory(directory);

foreach (var mapping in mappings)
{
    Walk(mapping);
}



void Walk(XmlTypeMapping mapping)
{
    
}
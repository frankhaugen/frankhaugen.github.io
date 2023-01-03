<Query Kind="Statements">
  <NuGetReference>JsonSchema.Net</NuGetReference>
  <NuGetReference>JsonSchema.Net.Generation</NuGetReference>
  <Namespace>Json.More</Namespace>
  <Namespace>Json.Pointer</Namespace>
  <Namespace>Json.Schema</Namespace>
  <Namespace>Json.Schema.Generation</Namespace>
  <Namespace>Json.Schema.Generation.Generators</Namespace>
  <Namespace>Json.Schema.Generation.Intents</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>


var schemaPath = "C:/Users/frank/Downloads/dnd5e_json_schema-master/schemas/Class.schema.json";
var schema = JsonSchema.FromFile(schemaPath);

schema.ToJsonDocument().Dump();

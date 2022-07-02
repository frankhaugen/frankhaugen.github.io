<Query Kind="Statements">
  <Reference Relative="..\..\..\SemineInvoiceGenerator\SemineInvoiceGenerator\bin\Debug\net6.0-windows\SemineInvoiceGenerator.dll">C:\repos\SemineInvoiceGenerator\SemineInvoiceGenerator\bin\Debug\net6.0-windows\SemineInvoiceGenerator.dll</Reference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <NuGetReference>ObjectDumper.NET</NuGetReference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Bson</Namespace>
  <Namespace>Newtonsoft.Json.Converters</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>Newtonsoft.Json.Schema</Namespace>
  <Namespace>Newtonsoft.Json.Serialization</Namespace>
  <Namespace>ObjectDumping.Internal</Namespace>
  <Namespace>SemineInvoiceGenerator.Models.SemineIntegrationApi</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>

var inputDirectory = new DirectoryInfo(@"C:\repos\SemineConnect.24SevenOffice\SemineConnect.24SevenOffice.Tests\Files\Invoices");

var inputFiles = inputDirectory.EnumerateFiles("*.json");

//inputFiles.Select(x => x.Name).Dump<IEnumerable<string>>();

//var inputJsonFile = new FileInfo(@"C:\repos\SemineConnect.24SevenOffice\SemineConnect.24SevenOffice.Tests\Files\Invoices\TfsoSalmonVoucher50332Failure.json");
//var input = JsonConvert.DeserializeObject<InvoiceDetails>(inputJson);

//var output = ObjectDumper.Dump(input, DumpStyle.CSharp);

//inputJson.Dump<string>();
//input.Dump<InvoiceDetails>();

//Console.WriteLine(output);

foreach (var inputJsonFile in inputFiles)
{
	if(inputJsonFile.Name == "InvoiceFailing_20211124_NullableObjectMustHaveValue.json") continue;
	if(inputJsonFile.Name == "InvoiceMappingFailed_20211113T1300.json") continue;
	
	
	try
	{
		var inputJson = File.ReadAllText(inputJsonFile.FullName);
		var input = System.Text.Json.JsonSerializer.Deserialize<InvoiceDetails>(inputJson);
		
		Console.WriteLine(inputJsonFile.Name);
		Console.WriteLine(input.Dump(DumpStyle.CSharp));
		break;
	}
	catch (Exception e)
	{
		e.Message.Dump<string>();
	}
}

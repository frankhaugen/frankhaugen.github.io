<Query Kind="Program">
  <NuGetReference>DeepCloner</NuGetReference>
  <Namespace>Force.DeepCloner</Namespace>
  <RemoveNamespace>System.Collections</RemoveNamespace>
  <RemoveNamespace>System.Data</RemoveNamespace>
  <RemoveNamespace>System.Diagnostics</RemoveNamespace>
  <RemoveNamespace>System.IO</RemoveNamespace>
  <RemoveNamespace>System.Linq</RemoveNamespace>
  <RemoveNamespace>System.Linq.Expressions</RemoveNamespace>
  <RemoveNamespace>System.Reflection</RemoveNamespace>
  <RemoveNamespace>System.Text</RemoveNamespace>
  <RemoveNamespace>System.Text.RegularExpressions</RemoveNamespace>
  <RemoveNamespace>System.Threading</RemoveNamespace>
  <RemoveNamespace>System.Transactions</RemoveNamespace>
  <RemoveNamespace>System.Xml</RemoveNamespace>
  <RemoveNamespace>System.Xml.Linq</RemoveNamespace>
  <RemoveNamespace>System.Xml.XPath</RemoveNamespace>
</Query>

void Main()
{
    var testObject = TestObject.Create();
    var cloner = new Cloner(new DeepClonerCloningProvider());
    var clone = cloner.Clone(testObject);
}

public interface ICloningProvider
{
    T Clone<T>(T source);
}
public interface ICloner
{
    T Clone<T>(T source);
}

public class DeepClonerCloningProvider : ICloningProvider
{
    public T Clone<T>(T source)
    {
        return source.DeepClone();
    }
}

public class Cloner : ICloner
{
    private readonly ICloningProvider _cloningProvider;

    public Cloner(ICloningProvider cloningProvider)
    {
        _cloningProvider = cloningProvider;
    }

    public T Clone<T>(T source)
    {
        return _cloningProvider.Clone(source);
    }
}


public class TestObject
{
    public string StringField { get; set; }
    public int IntField { get; set; }
    public TestObject ObjectField { get; set; }
    public List<int> ListField { get; set; }
    public Dictionary<string, TestObject> DictionaryField { get; set; }

    private int PrivateIntField { get; set; }

    public static TestObject Create()
    {
        return new TestObject
        {
            StringField = "string value",
            IntField = 123,
            PrivateIntField = 456,
            ObjectField = new TestObject
            {
                StringField = "nested string value"
            },
            ListField = new List<int> { 1, 2, 3 },
            DictionaryField = new Dictionary<string, TestObject>
            {
                { "key1", new TestObject { StringField = "dictionary value 1" } },
                { "key2", new TestObject { StringField = "dictionary value 2" } }
            }
        };
    }

    public int ReadPrivateField() => PrivateIntField;
}
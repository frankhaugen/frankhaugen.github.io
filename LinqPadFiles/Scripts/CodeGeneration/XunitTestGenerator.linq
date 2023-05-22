<Query Kind="Statements">
  <NuGetReference>CodegenCS</NuGetReference>
  <Namespace>System.Globalization</Namespace>
  <Namespace>CodegenCS</Namespace>
  <Namespace>System.Runtime.CompilerServices</Namespace>
</Query>

public class TestGenerator
{
    private readonly Type _type;
    private readonly TypeVariant _typeVariant;
    private readonly string _typeName;
    private readonly CodegenTextWriter _writer = new();
    private readonly List<string> _ignoreMethods = new List<string> { "ToString", "Equals", "GetHashCode", "GetType" };
    private readonly List<string> _namespaces = new List<string> {
            "System",
            "System.Threading.Tasks",
            "Xunit",
            "Xunit.Abstractions",
            "FluentAssertions",
            "NSubstitute",
            "Frank.Libraries.Extensions" };

    public TestGenerator(Type type)
    {
        _type = type;
        _typeVariant = GetTypeVariant();
        _typeName = GetFriendlyName(type);

        if (type.Namespace != null)
        {
            _namespaces.Add(type.Namespace);
        }
    }

    public string Generate(string namespaceName)
    {
        var constructors = _type.GetConstructors();
        var constructorParameters = new List<ParameterInfo>();
        var isNotStatic = !_type.IsAbstract;

        var types = new List<Type>();
        var returnTypes = _type.GetMethods()
                              .Select(x => x.ReturnType);
        var parameterTypes = _type.GetMethods()
                                 .SelectMany(x => x.GetParameters())
                                 .Select(x => x.ParameterType);

        types.AddRange(returnTypes);
        types.AddRange(parameterTypes);

        if (constructors.Any())
        {
            constructors.First()
                        .GetParameters()
                        .DoForEach(x => constructorParameters.Add(x));
        }

        constructorParameters.DoForEach(x => _namespaces.AddUnique(x.GetType().Namespace));

        types.Distinct(x => x.Name)
             .DoForEach(x => _namespaces.AddUnique(x.Namespace));

        _namespaces.Where(x => !string.IsNullOrWhiteSpace(x))
                  .OrderBy(x => x)
                  .ForEach(x => _writer.WriteLine($"using {x};"));

        _writer.Write(_writer.NewLine);
        _writer.WriteLine($"namespace {namespaceName};");
        _writer.WriteLine(" ");

        GenerateTestClass(constructorParameters);

        return _writer.GetContents();
    }

    public static string GetFriendlyName(Type type, bool fullyQualified = false)
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

    private void GenerateTestClass(IEnumerable<ParameterInfo> constructorParameters)
    {
        _writer.WithCurlyBraces($"public class {_typeName}Tests", () =>
        {
            var constructorParametersDictionary = constructorParameters.ToDictionary(x => $"_{x.ParameterType.Name.ToCamelcase()}", x => $"private readonly {x.ParameterType.Name} _{(x.ParameterType.IsInterface ? x.ParameterType.Name.Remove(0, 1).ToCamelcase() : x.ParameterType.Name.ToCamelcase())} = Substitute.For<{x.ParameterType.Name}>();");
            constructorParametersDictionary.DoForEach(x => _writer.WriteLine(x.Value));

            _writer.WriteLine($"private readonly ITestOutputHelper _outputHelper;");
            _writer.Write(_writer.NewLine);

            _writer.WithCurlyBraces($"public {_typeName}Tests(ITestOutputHelper outputHelper)", () =>
            {
                _writer.WriteLine($"_outputHelper = outputHelper;");

                if (_typeVariant != TypeVariant.Static)
                {
                    if (constructorParametersDictionary.Any())
                    {
                        _writer.WriteLine($"_sut{_typeName} = new {_typeName}({string.Join(", ", constructorParametersDictionary.Keys)});");
                    }
                    else
                    {
                        _writer.WriteLine($"_sut{_typeName} = new {_typeName}();");
                    }
                }
            });
            _writer.Write(_writer.NewLine);

            GenerateTestMethods();
        });
    }


    private TypeVariant GetTypeVariant()
    {
        var typeAttributes = _type.Attributes;

        if (typeAttributes.HasFlag(TypeAttributes.Abstract & TypeAttributes.Sealed))
        {
            return TypeVariant.Static;
        }
        else if (typeAttributes.HasFlag(TypeAttributes.ExplicitLayout))
        {
            return TypeVariant.Static;
        }
        else if (typeAttributes.HasFlag(TypeAttributes.Class))
        {
            return TypeVariant.Class;
        }
        else
        {
            return TypeVariant.Virtual;
        }
    }


    public enum TypeVariant
    {
        Static,
        Abstract,
        Virtual,
        Class
    }


    private void GenerateTestMethods()
    {
        switch (_typeVariant)
        {
            case TypeVariant.Class:
            case TypeVariant.Abstract:
                _writer.WriteLine($"private readonly {_typeName} _sut{_typeName};");
                _writer.Write(_writer.NewLine);
                break;
            case TypeVariant.Static:
                break;
        }

        var methodLookup = _type
            .GetMethods()
            .ToLookup(
                x => x.Name,
                info => info
            );


        foreach (var methodGrouping in methodLookup)
        {
            GenerateTestMethods(methodGrouping);
        }
    }

    private void GenerateTestMethods(IGrouping<string, MethodInfo> methodGrouping)
    {
        if (IgnoreMethodGrouping(methodGrouping)) return;

        if (methodGrouping.Count() == 1)
        {
            if (IgnoreMethod(methodGrouping.First()))
            {
                return;
            }

            GenerateTestMethod(methodGrouping.First());
        }
        else
        {
            var i = 0;
            foreach (var methodInfo in methodGrouping)
            {
                i++;
                if (IgnoreMethod(methodInfo))
                {
                    continue;
                }

                GenerateTestMethod(methodInfo, i.ToString());
            }
        }
    }

    private bool IgnoreMethod(MethodInfo methodInfo)
    {
        if (methodInfo.IsPrivate)
        {
            return true;
        }
        if (_type.BaseType != null && _type.BaseType.GetMethods().Select(x => x.Name).Contains(methodInfo.Name))
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
    private bool IgnoreMethodGrouping(IGrouping<string, MethodInfo> methodGrouping)
    {
        if (_ignoreMethods != null && _ignoreMethods.Any(x => x == methodGrouping.Key))
        {
            return true;
        }

        return false;
    }

    private void GenerateTestMethod(MethodInfo method, string suffix = "")
    {
        var methodType = GetMethodVariant(method);
        var returnType = "void";
        if (method.IsAsync())
        {
            returnType = "async Task";
        }

        _writer.Write(_writer.NewLine);
        _writer.WriteLine($"[Fact]");
        _writer.WithCurlyBraces($"public {returnType} {method.Name}Test{suffix}()", () =>
        {
            _writer.WriteLine("// Arrange");

            if (methodType == MethodVariant.Instance)
            {
                _writer.WriteLine($"var _sut = new {_typeName}();");
            }

            var methodParameters = method.GetParameters().ToList();

            if (methodType == MethodVariant.Extension)
            {
                var thisParameter = methodParameters.First();
                methodParameters.RemoveAt(0);
                _writer.WriteLine($"var {thisParameter.Name.ToCamelcase()} = default({thisParameter.ParameterType.Name});");
            }

            foreach (var param in methodParameters)
            {
                _writer.WriteLine($"var {param.Name?.ToCamelcase()} = default({param.ParameterType.Name});");
            }

            _writer.Write(_writer.NewLine);

            _writer.WriteLine("// Act");

            var awaitPrefix = "";
            if (method.IsAsync())
            {
                awaitPrefix = "await ";
            }

            switch (methodType)
            {
                case MethodVariant.Instance:
                    _writer.WriteLine($"var result = {awaitPrefix}_sut.{method.Name}({string.Join(", ", methodParameters.Where(x => !x.HasDefaultValue).Select(x => x.Name))});");
                    break;
                case MethodVariant.Extension:
                    _writer.WriteLine($"var result = {awaitPrefix}{method.Name}({string.Join(", ", methodParameters.Select(x => x.Name))});");
                    break;
                case MethodVariant.Static:
                    _writer.WriteLine($"var result = {awaitPrefix}{_typeName}.{method.Name}({string.Join(", ", methodParameters.Select(x => x.Name))});");
                    break;
            }

            _writer.Write(_writer.NewLine);

            _writer.WriteLine("// Assert");
            _writer.WriteLine($"_outputHelper.WriteLine(result);");
            _writer.WriteLine($"result.ToString().Should().NotBeNullOrEmpty();");
        });
    }

    private static MethodVariant GetMethodVariant(MethodInfo methodInfo)
    {
        if (methodInfo.IsDefined(typeof(ExtensionAttribute), false))
        {
            return MethodVariant.Extension;
        }
        else if (methodInfo.IsStatic)
        {
            return MethodVariant.Static;
        }
        else
        {
            return MethodVariant.Instance;
        }
    }

    public enum MethodVariant
    {
        Instance,
        Extension,
        Static
    }
}

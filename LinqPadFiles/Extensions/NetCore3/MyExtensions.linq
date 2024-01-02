<Query Kind="Program">
  <NuGetReference>CsvHelper</NuGetReference>
  <NuGetReference>Markdig</NuGetReference>
  <NuGetReference>Microsoft.Extensions.Hosting</NuGetReference>
  <NuGetReference>Microsoft.Extensions.Logging.Abstractions</NuGetReference>
  <NuGetReference>Namotion.Reflection</NuGetReference>
  <NuGetReference>OxyPlot.WindowsForms</NuGetReference>
  <NuGetReference>VarDump</NuGetReference>
  <Namespace>CsvHelper</Namespace>
  <Namespace>CsvHelper.Configuration</Namespace>
  <Namespace>Markdig</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Microsoft.Extensions.Hosting</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
  <Namespace>Microsoft.Extensions.Logging.Abstractions</Namespace>
  <Namespace>Namotion.Reflection</Namespace>
  <Namespace>OxyPlot</Namespace>
  <Namespace>OxyPlot.Axes</Namespace>
  <Namespace>OxyPlot.Series</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Numerics</Namespace>
  <Namespace>System.Runtime.InteropServices</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Text.Json.Serialization</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Windows</Namespace>
  <Namespace>System.Windows.Forms</Namespace>
  <Namespace>System.Net.Mail</Namespace>
</Query>

void Main()
{
    // Write code to test your extensions here. Press F5 to compile and run.
}


public static class MyExtensions
{
    // Write custom extension methods here. They will be available to all queries.
    public static async Task DumpToFileAsync(this string value, FileInfo file) => await File.WriteAllTextAsync(file.FullName, value);
    public static async Task DumpAsync(this FileInfo file, string content) => await File.WriteAllTextAsync(file.FullName, content);
    
    public static string ReadAllText(this FileInfo file) => File.ReadAllText(file.FullName);
    public static Guid ToGuid(this string value) => Guid.Parse(value);
    public static string Join(this IEnumerable<string> source, string separator = "\n") => string.Join(separator, source);

    public static float ToRadian(this float angle) => angle * (MathF.PI / 180);
    public static float Round(this float value, int precision = 1) => MathF.Round(value, precision);

    public static Vector2 Copy(this Vector2 source) => new(source.X, source.Y);

    public static T As<T>(this object source) where T : class => source as T;
    
    public static string DumpCSharp<T>(this T obj, VarDump.Visitor.DumpOptions? options = null, System.IO.DirectoryInfo? outputDirectory = null, bool dumpToResults = true)
    {
        if (options == null) options = new VarDump.Visitor.DumpOptions();
        var dumper = new VarDump.CSharpDumper(options);
        var result = dumper.Dump(obj);
        if (dumpToResults) result.Dump(typeof(T).FriendlyName());
        if (outputDirectory != null && outputDirectory.Exists)
        {
            options.IgnoreNullValues = true;
            options.UseNamedArgumentsForReferenceRecordTypes = true;
            var generatorAssemblyName = typeof(VarDump.CSharpDumper).Assembly.GetName();
            var fileResult =
            $$"""
            //----------------------------------------------------------------------------
            // <auto-generated>
            //     This code was generated by a tool.
            //     Tool: {{generatorAssemblyName.Name}}
            //     Version: {{generatorAssemblyName.Version}}
            //     Date: {{DateTime.UtcNow.ToString("s")}}
            // </auto-generated>
            //----------------------------------------------------------------------------
            namespace TODO;
            
            [GeneratedCode("{{generatorAssemblyName.Name}}", "{{generatorAssemblyName.Version}}")]
            public class Generated{{typeof(T).GetFilename()}}
            {
               [GeneratedCode("{{generatorAssemblyName.Name}}", "{{generatorAssemblyName.Version}}")]
               public {{typeof(T).GetFilename()}} Value { get; } {{result.Remove(0, 19)}}
            }
            """;

            System.IO.File.WriteAllText(System.IO.Path.Combine(outputDirectory.FullName, typeof(T).GetFilename() + ".cs"), fileResult);
        }
        return result;
    }

    /// <summary>
    /// Returns a nice name for a class especially for generics by including angle-brackets
    /// </summary>
    /// <param name="type"></param>
    /// <param name="fullyQualified"></param>
    /// <returns></returns>
    public static string GetFilename(this Type type, bool fullyQualified = false)
    {
        if (!type.IsGenericType)
        {
            return type.Name;
        }

        var typeNameBuilder = new StringBuilder();
        AppendNiceGenericName(typeNameBuilder, type, fullyQualified);
        return typeNameBuilder.ToString();
    }

    private static void AppendNiceGenericName(this StringBuilder sb, Type type, bool useFullName)
    {
        if (!type.IsGenericType)
        {
            sb.Append(useFullName ? type.FullName : type.Name);
            return;
        }

        var typeDef = type.GetGenericTypeDefinition();
        var typeName = useFullName ? typeDef.FullName : typeDef.Name;
        sb.Append(typeName);
        sb.Length -= typeName.Length - typeName.LastIndexOf('`');
        sb.Append("Of");

        var arguments = type.GenericTypeArguments;
        var argumentCount = arguments.Count();

        for (int i = 0; i < argumentCount; i++)
        {
            var typeArgument = arguments[i];
            AppendNiceGenericName(sb, typeArgument, useFullName);
            if (i != argumentCount - 2)
            {
                sb.Append("And");
            }
        }

        foreach (var typeArgument in type.GenericTypeArguments)
        {
        }
    }

    /// <summary>
    /// Returns a nice name for a class especially for generics by including angle-brackets
    /// </summary>
    /// <param name="type"></param>
    /// <param name="fullyQualified"></param>
    /// <returns></returns>
    public static string FriendlyName(this Type type, bool fullyQualified = false)
    {
        var typeName = fullyQualified
        ? type.FullNameSansTypeParameters().Replace("+", ".")
        : type.Name;

        if (type.IsGenericType)
        {
            var genericArgumentIds = type.GetGenericArguments()
                .Select(t => FriendlyName(t, fullyQualified))
                .ToArray();

            return new StringBuilder(typeName)
                .Replace(string.Format("`{0}", genericArgumentIds.Count()), string.Empty)
                .Append(string.Format("<{0}>", string.Join(",", genericArgumentIds).TrimEnd(',')))
                .ToString();
        }

        return typeName;
    }

    private static string FullNameSansTypeParameters(this Type type)
    {
        var fullName = type.FullName;
        if (string.IsNullOrEmpty(fullName))
            fullName = type.Name;
        var chopIndex = fullName.IndexOf("[[");
        return (chopIndex == -1) ? fullName : fullName.Substring(0, chopIndex);
    }

    public static string DumpServices(this IServiceCollection services)
    {
        var pipeline = new MarkdownPipelineBuilder().UsePipeTables().Build();

        var table = new StringBuilder();
        table.AppendLine("| Service Type | Implementation Type |");
        table.AppendLine("|--------------|---------------------|");

        foreach (var service in services)
        {
            var serviceType = service.ServiceType.GetDisplayName();
            var implementationType = service.ImplementationType?.GetDisplayName() ?? "<Unknown>";

            table.AppendLine($"| {serviceType} | {implementationType} |");
        }
        
        var html = Markdown.ToHtml(table.ToString(), pipeline);
        Util.RawHtml(html).Dump();

        return html;
    }
}

public static class JsonUtil
{
	public static T Deserialize<T>(string json)
	{
		return JsonSerializer.Deserialize<T>(json, Options);
	}

	public static string Serialize<T>(T obj)
	{
		return JsonSerializer.Serialize(obj, Options);
	}

	public static JsonSerializerOptions Options = new JsonSerializerOptions()
	{
		PropertyNameCaseInsensitive = true,
		Converters = {
			new JsonStringEnumConverter(),
			new JsonDirectoryInfoConverter(),
			new JsonMailAddressConverter()
		},
	    WriteIndented = true,
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
		ReferenceHandler = ReferenceHandler.IgnoreCycles
	};
	
	
}

public class JsonMailAddressConverter : JsonConverter<MailAddress>
{
	public override MailAddress Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		string address = reader.GetString();
		return new MailAddress(address);
	}

	public override void Write(Utf8JsonWriter writer, MailAddress value, JsonSerializerOptions options)
	{
		writer.WriteStringValue(value.ToString());
	}
}

public class JsonDirectoryInfoConverter : JsonConverter<DirectoryInfo>
{
	public override DirectoryInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		string path = reader.GetString();
		return new DirectoryInfo(Path.GetFullPath(path));
	}

	public override void Write(Utf8JsonWriter writer, DirectoryInfo value, JsonSerializerOptions options)
	{
		// Convert to a Unix-like path format (which also works on Windows)
		string normalizedPath = value.FullName.Replace("\\", "/");
		writer.WriteStringValue(normalizedPath);
	}
}

public static class PlotHelper
{
    public static void PlotDictionaryV2(Dictionary<TimeSpan, Vector3> data, string title)
    {
        var model = new PlotModel { Title = title };

        // Create series and axes
        var lineSeries = new LineSeries { MarkerType = MarkerType.Circle };
        var timeAxis = new LinearAxis { Position = AxisPosition.Top, Title = "Time (s)" };
        var distanceAxis = new LinearAxis { Position = AxisPosition.Bottom, Title = "Distance Travelled" };
        var yAxis = new LinearAxis { Position = AxisPosition.Left, Title = "Y Coordinate" };

        foreach (var point in data)
        {
            var timeInSeconds = point.Key.TotalSeconds;
            var xPosition = point.Value.X;
            var yPosition = point.Value.Y;

            var xValue = timeInSeconds * xPosition;  // Assuming that distance traveled is represented by xPosition
            var yValue = yPosition;

            lineSeries.Points.Add(new DataPoint(xValue, yValue));
            //timeAxis.ActualMaximum = Math.Max(timeAxis.ActualMaximum, timeInSeconds);
        }

        // Add series and axes to the model
        model.Series.Add(lineSeries);
        model.Axes.Add(timeAxis);
        model.Axes.Add(distanceAxis);
        model.Axes.Add(yAxis);

        var plotView = new OxyPlot.WindowsForms.PlotView
        {
            Dock = DockStyle.Fill,
            Model = model
        };

        plotView.Dump();  // LINQPad extension method to display the PlotView
    }
    
    public static void PlotDictionaryV1(Dictionary<TimeSpan, Vector3> data, string title)
    {
        var model = new PlotModel { Title = title };
        var lineSeries = new LineSeries { MarkerType = MarkerType.Circle };

        var xAxis = new LinearAxis { Position = AxisPosition.Bottom, Title = "X Coordinate" };
        var yAxis = new LinearAxis { Position = AxisPosition.Left, Title = "Y Coordinate" };

        foreach (var point in data)
        {
            var timeInSeconds = point.Key.TotalSeconds;
            var xPosition = point.Value.X;
            var yPosition = point.Value.Y;

            //var xValue = timeInSeconds * xPosition;  // Assuming that distance traveled is represented by xPosition
            var xValue = xPosition;
            var yValue = yPosition;

            lineSeries.Points.Add(new DataPoint(xValue, yValue));
        }

        model.Series.Add(lineSeries);
        

        var plotView = new OxyPlot.WindowsForms.PlotView
        {
            Dock = DockStyle.Fill,
            Model = model
        };

        plotView.Dump();  // LINQPad extension method to display the PlotView
    }

    public static void PlotDictionary(Dictionary<TimeSpan, Vector3> data, string title)
    {
        var model = new PlotModel { Title = title };

        // Create series and axes
        var lineSeries = new LineSeries { MarkerType = MarkerType.Circle };
        var timeLineSeries = new LineSeries { Color = OxyColors.Red };  // Red color to distinguish the timeline
        var timeAxis = new LinearAxis { Position = AxisPosition.Top, Title = "Time" };
        var distanceAxis = new LinearAxis { Position = AxisPosition.Bottom, Title = "Distance Travelled" };
        var yAxis = new LinearAxis { Position = AxisPosition.Left, Title = "Y Coordinate" };

        foreach (var point in data)
        {
            var time = point.Key;
            var timeInSeconds = time.TotalSeconds;
            var xPosition = point.Value.X;
            var yPosition = point.Value.Y;

            //var xValue = timeInSeconds * xPosition;  // Assuming that distance traveled is represented by xPosition
            var xValue = xPosition;  // Assuming that distance traveled is represented by xPosition
            var yValue = yPosition;

            lineSeries.Points.Add(new DataPoint(xValue, yValue));
            timeLineSeries.Points.Add(new DataPoint(time.TotalMicroseconds, 0));  // Add a point on the timeline at the same X position
        }

        // Add series and axes to the model
        model.Series.Add(lineSeries);
        model.Series.Add(timeLineSeries);
        model.Axes.Add(timeAxis);
        model.Axes.Add(distanceAxis);
        model.Axes.Add(yAxis);

        var plotView = new OxyPlot.WindowsForms.PlotView
        {
            Dock = DockStyle.Fill,
            Model = model
        };

        plotView.Dump();  // LINQPad extension method to display the PlotView
    }
}

public static class CancellationTokenUtil
{
    public static CancellationToken Get(TimeSpan duration) => new CancellationTokenSource(duration).Token;
    public static CancellationTokenSource GetSource(TimeSpan duration) => new CancellationTokenSource(duration);
}
public static class CsvUtil
{
    public static List<TClass> ReadCsvData<TClass, TMap>(string csvData, string delimiter = ";") where TMap : ClassMap<TClass> where TClass : class
    {
        var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = delimiter,
            HasHeaderRecord = true
        };

        using var reader = new StringReader(csvData);
        using var csv = new CsvReader(reader, configuration);


        csv.Context.RegisterClassMap<TMap>();

        var records = csv.GetRecords<TClass>().ToList();
        return records;
    }
}

public static class TextUtil
{
    public static string RemoveWikipediaReferences(string input)
    {
        string pattern = @"\[\d+\]";
        string cleanedText = Regex.Replace(input, pattern, string.Empty);
        return cleanedText;
    }

    public static string Capitalize(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }
        return char.ToUpper(input[0]) + input.Substring(1).ToLowerInvariant();
    }

    public static string Clean(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }
        return input.Replace('"', '`');
    }
}
public class CodeGenerator
{
    public ConstantsCodeGenerator Constants => new();

    public class ConstantsCodeGenerator
    {
        public string Generate(string name, IEnumerable<Tuple<string, string, string>> values)
        {
            var enumValuesAndNamesString = values.Select(x => $"public const {x.Item1} {x.Item2} = {x.Item3};");
            var result = $$"""
                        public static class {{name}}
                        {
                            {{string.Join("\n    ", enumValuesAndNamesString)}}
                        }
                        """;
            return result;
        }
        
        public string Generate(string name, IEnumerable<Tuple<string, IEnumerable<Tuple<string, string, string>>>> groupedValues)
        {
            var codeBuilder = new StringBuilder();
            
            foreach (var group in groupedValues)
            {
                codeBuilder.AppendLine(Generate(group.Item1, group.Item2));
            }
            
            var result = $$"""
                        public static class {{name}}
                        {
                            {{string.Join("\n        ", codeBuilder.ToString())}}
                        }
                        """;
            return result;
        }

        public string Generate(string name, IEnumerable<KeyValuePair<string, string>> values)
        {
            var enumValuesAndNamesString = values.Select(x => $"public const string {x.Key} = {x.Value}");
            var result = $$"""
                        public static class {{name}}
                        {
                            {{string.Join(";\n    ", enumValuesAndNamesString)}}
                        }
                        """;
            return result;
        }
    }



    public EnumCodeGenerator Enums => new EnumCodeGenerator();

    public class EnumCodeGenerator
    {
        public EnumListGenerator List => new EnumListGenerator();

        public string Generate(string name, IEnumerable<string> values)
        {
            var enumValuesAndNamesString = values.Select(x => $"{x}");
            var result = $$"""
                        public enum {{name}}
                        {
                            {{string.Join(",\n    ", enumValuesAndNamesString)}}
                        }
                        """;
            return result;
        }

        public string Generate(string name, IEnumerable<KeyValuePair<int, string>> values)
        {
            var enumValuesAndNamesString = values.Select(x => $"{x.Value} = {x.Key}");
            var result = $$"""
                        public enum {{name}}
                        {
                            {{string.Join(",\n    ", enumValuesAndNamesString)}}
                        }
                        """;
            return result;
        }
    
        
        public class EnumListGenerator
        {
            string Generate<T>() where T : struct, Enum
            {
                var enumValues = Enum.GetValues<T>();
                var enumValuesAndNamesString = enumValues.Select(x => $"{typeof(T).Name}.{x.ToString()}\n");
                var enumValuesAndNamesStringJoined = string.Join(", ", enumValuesAndNamesString);
                var result = $$"""
                            public static class Generated{{typeof(T).FriendlyName()}}
                            {
                                public static IEnumerable<T> Get()
                                {
                                    return new[]
                                    {
                                        {{enumValuesAndNamesStringJoined}}
                                    };
                                }
                            }
                            """;
                return result;
            }
        }
    }
}

public static class AppUtil
{
    /// <summary>Returns a IHostBuilder that has logging configured</summary>
    public static IHostBuilder GetBuiler()
    {
        var builder = Host.CreateDefaultBuilder();
        
        builder.ConfigureLogging(x => x.ClearProviders().AddProvider(new LinqPadLoggerProvider()));
        
        return builder;
    }
    
    public static HostApplicationBuilder GetHostApplicationBuilder()
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Logging.ClearProviders().AddProvider(new LinqPadLoggerProvider());
        return builder;
    }
}

public static class Downloader
{


	public static async Task<string> DowloadStringAsync(string url)
	{
		using (var httpClient = new HttpClient())
		{
			return await httpClient.GetStringAsync(url);
		}
	}
	
	public static async Task<T> DowloadJsonAsync<T>(string url)
	{
		using (var httpClient = new HttpClient())
		{
			var json = await httpClient.GetStringAsync(url);
			return JsonUtil.Deserialize<T>(json);
		}
	}
	public static async Task<byte[]> DownloadAsync(string url) => await new HttpClient().GetByteArrayAsync(url);
    public static Stream Download(string url) => new HttpClient().GetStreamAsync(url).GetAwaiter().GetResult();
}

public class WindowHost<T> : BackgroundService where T : Window
{
	private readonly IServiceProvider _serviceProvider;
	private readonly ILogger<WindowHost<T>> _logger;

	public WindowHost(IServiceProvider serviceProvider, ILogger<WindowHost<T>> logger)
	{
		_serviceProvider = serviceProvider;
		_logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		_logger.LogInformation("Starting");

		using var scope = _serviceProvider.CreateScope();
		var window = scope.ServiceProvider.GetRequiredService<T>();
		var app = scope.ServiceProvider.GetRequiredService<System.Windows.Application>();

		app.ShutdownMode = ShutdownMode.OnMainWindowClose;
		app.Exit += (sender, args) =>
		{
			//FreeConsole();
			app.Shutdown();
			_logger.LogInformation("Stopping");
			Environment.Exit(0);
		};

		app.Run(window);
	}

	//[DllImport("kernel32")]
	//private static extern bool FreeConsole();
}

public class LinqPadLoggerFactory : ILoggerFactory
{
    private ILoggerProvider _provider = new LinqPadLoggerProvider();
    
    public void AddProvider(ILoggerProvider provider)
    {
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _provider.CreateLogger(categoryName);
    }

    public void Dispose()
    {
    }
}

public class LinqPadLoggerProvider : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName)
    {
        return new LinqPadLogger(categoryName);
    }

    public void Dispose()
    {
    }
}

public class LinqPadLogger<T> : ILogger<T>
{
    private readonly string _categoryName;

    public LinqPadLogger()
    {
        _categoryName = typeof(T).FriendlyName();
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return new LinqPadScope();
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var log = new LogThing<TState>(_categoryName, formatter.Invoke(state, exception), logLevel, eventId, state, exception);

        log.Message.Dump();
    }

    private record LogThing<TState>(string CategoryName, string Message, LogLevel logLevel, EventId eventId, TState state, Exception exception);
}

public class LinqPadLogger : ILogger
{
    private readonly string _categoryName;

    public LinqPadLogger(string categoryName)
    {
        _categoryName = categoryName;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return new LinqPadScope();
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        var log = new LogThing<TState>(_categoryName, formatter.Invoke(state, exception), logLevel , eventId, state, exception);
        
        log.Message.Dump();
    }
    
    private record LogThing<TState>(string CategoryName, string Message, LogLevel logLevel, EventId eventId, TState state, Exception exception);
}

public class LinqPadScope : IDisposable
{
    public void Dispose()
    {
        
    }
}

#region Advanced - How to multi-target

// The NETx symbol is active when a query runs under .NET x or later.

#if NET7
// Code that requires .NET 7 or later
#endif

#if NET6
// Code that requires .NET 6 or later
#endif

#if NET5
// Code that requires .NET 5 or later
#endif

#endregion
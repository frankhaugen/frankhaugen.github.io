<Query Kind="Program">
  <NuGetReference>Microsoft.AspNetCore.App.Ref</NuGetReference>
  <Namespace>Microsoft.AspNetCore.Builder</Namespace>
  <Namespace>Microsoft.AspNetCore.Connections</Namespace>
  <Namespace>Microsoft.AspNetCore.Hosting</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Microsoft.Extensions.Hosting</Namespace>
  <Namespace>System.Buffers</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.IO.Pipelines</Namespace>
  <Namespace>System.Reflection.Metadata</Namespace>
  <Namespace>System.Net.Sockets</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

async Task Main()
{
	var builder = WebApplication.CreateEmptyBuilder(new() {});
	builder.UseTcpConnectionHandler<MyCustomProcessor>(6667);
	builder.Logging.AddConsole();
	var app = builder.Build();
	await app.RunAsync(QueryCancelToken);
}

public class MyCustomProcessor : IConnectionProcessor
{
	public async Task<byte[]> ProcessAsync(ReadOnlyMemory<byte> input)
	{
		var stringInput = Encoding.UTF8.GetString(input.ToArray());
		
		stringInput.Dump("Input");
		
		var output = Encoding.UTF8.GetBytes("OK");
		
		return output;
	}
}

public class TcpConnectionHandler : ConnectionHandler
{
	private readonly ILogger<TcpConnectionHandler> _logger;
	private readonly IConnectionProcessor _processor;

	public TcpConnectionHandler(ILogger<TcpConnectionHandler> logger, IConnectionProcessor processor)
	{
		_logger = logger;
		_processor = processor;
	}

	public override async Task OnConnectedAsync(ConnectionContext connection)
	{
		_logger.LogDebug($"Connected: {connection.ConnectionId}");

		while (true)
		{
			var result = await connection.Transport.Input.ReadAsync();
			var buffer = result.Buffer;

			foreach (var segment in buffer)
			{
				//var text = Encoding.UTF8.GetString(segment.Span);

				if (!segment.IsEmpty)
				{
					var responseBytes = await _processor.ProcessAsync(segment);
					await connection.Transport.Output.WriteAsync(responseBytes);
				}
			}

			if (result.IsCompleted)
			{
				break;
			}

			connection.Transport.Input.AdvanceTo(buffer.End);
		}

		_logger.LogDebug($"Disconnected: {connection.ConnectionId}");
	}
}

public interface IConnectionProcessor
{
	Task<byte[]> ProcessAsync(ReadOnlyMemory<byte> input);
}

public static class TcpServerExtensions
{
	public static IWebHostBuilder UseTcpConnectionHandler<TProcessor>(this IWebHostBuilder builder, int port)
		where TProcessor : class, IConnectionProcessor
	{
		builder.ConfigureServices(services =>
		{
			services.AddTransient<IConnectionProcessor, TProcessor>();
		});

		builder.ConfigureKestrel(options =>
		{
			options.ListenAnyIP(port, listenOptions =>
			{
				listenOptions.UseConnectionHandler<TcpConnectionHandler>();
			});
		});

		return builder;
	}
}
public static class TcpServerHostBuilderExtensions
{
	public static IHostBuilder UseTcpConnectionHandler<TProcessor>(this IHostBuilder hostBuilder, int port)
		where TProcessor : class, IConnectionProcessor
	{
		hostBuilder.ConfigureWebHostDefaults(webBuilder =>
		{
			webBuilder.ConfigureServices(services =>
			{
				services.AddTransient<IConnectionProcessor, TProcessor>();
			});

			webBuilder.ConfigureKestrel(serverOptions =>
			{
				serverOptions.ListenAnyIP(port, listenOptions =>
				{
					listenOptions.UseConnectionHandler<TcpConnectionHandler>();
				});
			});
		});

		return hostBuilder;
	}
}

public static class WebApplicationBuilderExtensions
{
	public static WebApplicationBuilder UseTcpConnectionHandler<TProcessor>(this WebApplicationBuilder builder, int port)
		where TProcessor : class, IConnectionProcessor
	{
		// Add services
		builder.Services.AddTransient<IConnectionProcessor, TProcessor>();

		// Configure Kestrel
		builder.WebHost.UseKestrelCore();
		builder.WebHost.ConfigureKestrel(serverOptions =>
		{
			serverOptions.ListenAnyIP(port, listenOptions =>
			{
				listenOptions.UseConnectionHandler<TcpConnectionHandler>();
			});
		});

		return builder;
	}
}


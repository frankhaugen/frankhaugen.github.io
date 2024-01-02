<Query Kind="Statements">
  <NuGetReference Prerelease="true">Bedrock.Framework</NuGetReference>
  <NuGetReference>Microsoft.AspNetCore.App.Ref</NuGetReference>
  <NuGetReference>Microsoft.AspNetCore.Connections.Abstractions</NuGetReference>
  <Namespace>Bedrock.Framework</Namespace>
  <Namespace>Microsoft.Extensions.Hosting</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Microsoft.AspNetCore.Connections</Namespace>
  <Namespace>System.Net.Sockets</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Bedrock.Framework.Protocols</Namespace>
  <Namespace>System.Buffers</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
  <RuntimeVersion>7.0</RuntimeVersion>
</Query>


var builder = Host.CreateDefaultBuilder();

builder.


public class IrcProtocol : IMessageReader<IrcMessage>, IMessageWriter<IrcMessage>
{
	public bool TryParseMessage(in ReadOnlySequence<byte> input, ref SequencePosition consumed, ref SequencePosition examined, out IrcMessage message)
	{
		ReadOnlySpan<byte> buffer = input.FirstSpan;
		var rawMessage = Encoding.UTF8.GetString(buffer.ToArray());
		message = new IrcMessage(rawMessage);
		consumed = input.End;
		return true;
	}

	public void WriteMessage(IrcMessage message, IBufferWriter<byte> output)
	{
		Encoding.UTF8.GetBytes(message.ToString(), output);
	}
}

public class IrcConnectionHandler : ConnectionHandler
{
	private readonly ILogger<IrcConnectionHandler> _logger;

	public IrcConnectionHandler(ILogger<IrcConnectionHandler> logger)
	{
		_logger = logger;
	}

	public override async Task OnConnectedAsync(ConnectionContext connection)
	{
		var protocol = new IrcProtocol();
		var reader = connection.CreateReader();
		var writer = connection.CreateWriter();

		while (true)
		{
			try
			{
				var result = await reader.ReadAsync(protocol);
				var message = result.Message;

				if (result.IsCompleted)
				{
					break;
				}

				if (message.Command.Equals("ping", StringComparison.OrdinalIgnoreCase))
				{
					await writer.WriteAsync(protocol, new IrcMessage("PONG"));
				}


				if (result.IsCompleted)
				{
					break;
				}
			}
			finally
			{
				reader.Advance();
			}
		}
	}
}

public class IrcMessagePrefix
{
	public string? Nick { get; set; }
	public string? User { get; set; }
	public string? Host { get; set; }

	public IrcMessagePrefix(string prefix)
	{
		if (prefix.StartsWith(":"))
		{
			prefix = prefix.Substring(1);
		}

		var parts = prefix.Split('@');
		if (parts.Length == 2)
		{
			Host = parts[1];
			var nickUser = parts[0].Split('!');
			if (nickUser.Length == 2)
			{
				Nick = nickUser[0];
				User = nickUser[1];
			}
			else
			{
				Nick = parts[0];
			}
		}
		else
		{
			Nick = prefix;
		}
	}

	public override string ToString()
	{
		if (Nick != null)
		{
			if (User != null && Host != null)
			{
				return $"{Nick}!{User}@{Host}";
			}
			else
			{
				return $"{Nick}";
			}
		}
		else
		{
			return "";
		}
	}
}

public class IrcMessage
{
	public IrcMessagePrefix? Prefix { get; }
	public string? Command { get; }
	public string? Channel { get; set; }
	public string? Message { get; set; }

	public IrcMessage(string message)
	{
		var parts = message.Split(' ');

		if (parts[0].StartsWith(":"))
		{
			Prefix = new IrcMessagePrefix(parts[0].Substring(1));
			Command = parts[1];
		}
		else
		{
			Command = parts[0];
		}

		for (int i = Prefix != null ? 2 : 1; i < parts.Length; i++)
		{
			var part = parts[i];
			if (part.StartsWith("#"))
			{
				Channel = part;
			}
			else if (part.StartsWith(":"))
			{
				Message = message.Substring(message.IndexOf(part) + 1);
				break;
			}
			else if (i == parts.Length - 1)
			{
				Message = part;
			}
		}
	}

	public override string ToString()
	{
		var parts = new List<string>();
		if (Prefix != null)
		{
			parts.Add($":{Prefix}");
		}

		if (Command != null)
		{
			parts.Add(Command);
		}

		if (Channel != null)
		{
			parts.Add(Channel);
		}

		if (Message != null)
		{
			parts.Add($":{Message}");
		}

		return string.Join(" ", parts);
	}
}


public static class IrcServerExtensions
{
	public static void AddIrcServer(this HostApplicationBuilder applicationBuilder, int port = 6667)
	{
		applicationBuilder.AddServer(serverBuilder =>
		{
			serverBuilder.UseSockets(config => config.ListenAnyIP(port, connection => connection.UseConnectionLogging().UseConnectionHandler<IrcConnectionHandler>()));
		});
	}

	private static void AddServer(this HostApplicationBuilder applicationBuilder, Action<ServerBuilder> configure)
    {
        applicationBuilder.Services.AddHostedService<ServerHostedService>();
        applicationBuilder.Services.AddOptions<ServerHostedServiceOptions>()
                          .Configure<IServiceProvider>((options, sp) =>
                          {
                              options.ServerBuilder = new ServerBuilder(sp);
                              configure(options.ServerBuilder);
                          });
	}
}
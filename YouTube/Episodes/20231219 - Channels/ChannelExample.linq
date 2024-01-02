<Query Kind="Statements">
  <NuGetReference>Microsoft.AspNetCore.App.Ref</NuGetReference>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Channels</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.ComponentModel</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
  <RuntimeVersion>8.0</RuntimeVersion>
</Query>


public class Message
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public string Body { get; set; }
}

public class Messenger
{
	private readonly Channel<Message> _channel = System.Threading.Channels.Channel.CreateUnbounded<Message>();

	public async Task SendAsync(Message message, CancellationToken cancellationToken) => await _channel.Writer.WriteAsync(message, cancellationToken);

	public IAsyncEnumerable<Message> ReadAllAsync(CancellationToken cancellationToken) => _channel.Reader.ReadAllAsync(cancellationToken);
}

public class Listener
{
	private readonly Messenger _messenger;

	public Listener(Messenger messenger)
	{
		_messenger = messenger;
	}

	public async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		await foreach (Message message in _messenger.ReadAllAsync(stoppingToken))
		{
			Console.WriteLine("Received message with ID: {MessageId} and with body: {MessageBody}", message.Id, message.Body);
		}
	}
}
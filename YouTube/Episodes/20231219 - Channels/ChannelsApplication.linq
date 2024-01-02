<Query Kind="Statements">
  <NuGetReference>Microsoft.AspNetCore.App.Ref</NuGetReference>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Channels</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Microsoft.Extensions.Hosting</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
  <Namespace>System.Windows</Namespace>
  <Namespace>System.Windows.Controls</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
  <RuntimeVersion>8.0</RuntimeVersion>
</Query>


var builder = Host.CreateApplicationBuilder();
builder.Logging.AddProvider(new LinqPadLoggerProvider());
builder.Services.AddSingleton<Messenger>();
builder.Services.AddHostedService<Listener>();

var app = builder.Build();
app.StartAsync(QueryCancelToken);

var messenger = app.Services.GetRequiredService<Messenger>();
WindowFactory.CreateWindow(messenger).ShowDialog();

public class Listener : BackgroundService
{
	private readonly Messenger _messenger;
	private readonly ILogger<Listener> _logger;
	
	public Listener(ILogger<Listener> logger, Messenger messenger)
	{
		_logger = logger;
		_messenger = messenger;
	}
	
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		await foreach (Message message in _messenger.ReadAllAsync(stoppingToken))
		{
			_logger.LogInformation("Received message with ID: {MessageId} and with body: {MessageBody}", message.Id, message.Body);
		}
	}
}

public class Message
{
	public Guid Id { get; set; } = Guid.NewGuid();
	
	public string Body { get; set; }
}

public class Messenger
{
	private readonly Channel<Message> _channel = System.Threading.Channels.Channel.CreateUnbounded<Message>();

	public async Task SendAsync(Message message) => await _channel.Writer.WriteAsync(message);

	public IAsyncEnumerable<Message> ReadAllAsync(CancellationToken cancellationToken) => _channel.Reader.ReadAllAsync(cancellationToken);
}

public static class WindowFactory
{
	public static Window CreateWindow(Messenger messenger)
	{

		var window = new Window() {
			WindowStartupLocation = WindowStartupLocation.CenterScreen,
			Width = 256,
			Height = 128,
			FontSize = 31
		};
		var textBox = new TextBox();
		var button = new Button()
		{
			Content = "Send message"
		};

		button.Click += async (sender, e) =>
		{
			var message = new Message
			{
				Body = textBox.Text
			};
			await messenger.SendAsync(message);
		};

		window.Content = new StackPanel()
		{
			Children =
			{
				textBox,
				button
			}
		};
		
		return window;
	}
}

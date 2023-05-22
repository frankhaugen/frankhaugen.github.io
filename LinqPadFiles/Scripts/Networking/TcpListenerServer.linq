<Query Kind="Program">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Net.Sockets</Namespace>
</Query>

async Task Main()
{
    var serviceProvider = new ServiceCollection()
                .AddSingleton<ITcpServer, TcpServer>()
                .BuildServiceProvider();

    var server = serviceProvider.GetRequiredService<ITcpServer>();

    server.DataReceived += (sender, e) =>
    {
        Console.WriteLine($"Data received from client {e.RemoteEndPoint}: {e.Data}");
    };

    await server.StartAsync(IPAddress.Any, 12345);

    Console.WriteLine("Press any key to stop the server...");
    Console.ReadKey();

    await server.StopAsync();
}


public class TcpDataReceivedEventArgs : EventArgs
{
    public string Data { get; }
    public EndPoint RemoteEndPoint { get; }

    public TcpDataReceivedEventArgs(EndPoint remoteEndPoint, string data)
    {
        Data = data;
        RemoteEndPoint = remoteEndPoint;
    }
}

public interface ITcpServer
{
    event EventHandler<TcpDataReceivedEventArgs> DataReceived;
    Task StartAsync(IPAddress ipAddress, int port);
    Task StopAsync();
}

public class TcpServer : ITcpServer
{
    private TcpListener _listener;
    private bool _isRunning;

    public event EventHandler<TcpDataReceivedEventArgs> DataReceived;

    public async Task StartAsync(IPAddress ipAddress, int port)
    {
        _listener = new TcpListener(ipAddress, port);
        _listener.Start();
        _isRunning = true;

        while (_isRunning)
        {
            var client = await _listener.AcceptTcpClientAsync();
            _ = Task.Run(() => HandleClientAsync(client));
        }
    }

    public Task StopAsync()
    {
        _isRunning = false;
        _listener.Stop();
        return Task.CompletedTask;
    }

    private async Task HandleClientAsync(TcpClient client)
    {
        using (client)
        {
            var buffer = new byte[1024];
            var stream = client.GetStream();

            while (_isRunning && client.Connected)
            {
                var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead > 0)
                {
                    var data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    DataReceived?.Invoke(this, new TcpDataReceivedEventArgs(client.Client.RemoteEndPoint, data));
                }
            }
        }
    }
}

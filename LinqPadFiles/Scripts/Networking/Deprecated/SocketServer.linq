<Query Kind="Program">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net.Sockets</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
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
    private Socket _listener;
    private bool _isRunning;

    public event EventHandler<TcpDataReceivedEventArgs> DataReceived;

    public async Task StartAsync(IPAddress ipAddress, int port)
    {
        _listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        _listener.Bind(new IPEndPoint(ipAddress, port));
        _listener.Listen(100);

        _isRunning = true;

        while (_isRunning)
        {
            var client = await _listener.AcceptAsync();
            _ = Task.Run(() => HandleClientAsync(client));
        }
    }

    public Task StopAsync()
    {
        _isRunning = false;
        _listener.Close();
        return Task.CompletedTask;
    }

    private async Task HandleClientAsync(Socket client)
    {
        using (client)
        {
            var buffer = new byte[1024];

            while (_isRunning && client.Connected)
            {
                var bytesRead = await client.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
                if (bytesRead > 0)
                {
                    var data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    DataReceived?.Invoke(this, new TcpDataReceivedEventArgs(client.RemoteEndPoint, data));
                }
            }
        }
    }
}

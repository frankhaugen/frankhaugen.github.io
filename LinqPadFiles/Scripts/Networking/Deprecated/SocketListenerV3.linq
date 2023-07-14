<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
  <Namespace>Microsoft.Extensions.Options</Namespace>
</Query>

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

public interface ISocketListener
{
    void Start();
    void Stop();
}

public interface IOnSocketEventHandler
{
}

public interface IOnSocketReceiveHandler : IOnSocketEventHandler
{
}

public class SocketSettings
{
    public IPAddress IPAddress { get; set; }
    public int Port { get; set; }
    public ProtocolType ProtocolType { get; set; }
    public SocketType SocketType { get; set; }
    public AddressFamily AddressFamily { get; set; }
};

public class SocketListener : ISocketListener
{
    private readonly IOptions<SocketSettings> _options;
    private readonly IOnSocketReceiveHandler _onReceiveHandler;
    
    private readonly Socket _socket;
    private bool _isRunning;

    public SocketListener(IOptions<SocketSettings> options, IOnSocketReceiveHandler onReceiveHandler)
    {
        _options = options;
        _onReceiveHandler = onReceiveHandler;
        
        _socket = new Socket(_options.Value.AddressFamily, _options.Value.SocketType, _options.Value.ProtocolType);
    }

    public void Start()
    {
        if (_isRunning) return;
        _isRunning = true;

        Task.Run(async () =>
        {
            while (_isRunning)
            {
                try
                {
                    var client = await _socket.AcceptAsync();

                    Task.Run(async () =>
                    {
                        var buffer = new byte[1024];
                        while (client.Connected && _isRunning)
                        {
                            var bytesRead = await client.ReceiveAsync(buffer, SocketFlags.None);
                            if (bytesRead > 0)
                            {
                                var data = new byte[bytesRead];
                                Array.Copy(buffer, data, bytesRead);
                                OnReceive?.Invoke(client, data);
                            }
                        }
                    });
                }
                catch (SocketException)
                {
                    // Socket has been closed
                }
            }
        });
    }

    public void Stop()
    {
        _isRunning = false;
        _socket.Close();
    }
}
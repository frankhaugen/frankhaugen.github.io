<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Net.Sockets</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>





using var listenSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
listenSocket.Bind(new IPEndPoint(IPAddress.Loopback, 8080));

Console.WriteLine($"Listening on {listenSocket.LocalEndPoint}");

listenSocket.Listen();

while (true)
{
    // Wait for a new connection to arrive
    var connection = await listenSocket.AcceptAsync();

    // We got a new connection spawn a task to so that we can echo the contents of the connection
    _ = Task.Run(async () =>
    {
        var buffer = new byte[4096];
        try
        {
            while (true)
            {
                int read = await connection.ReceiveAsync(buffer, SocketFlags.None);
                if (read == 0)
                {
                    break;
                }
                await connection.SendAsync(buffer[..read], SocketFlags.None);
            }
        }
        finally
        {
            connection.Dispose();
        }
    });
}
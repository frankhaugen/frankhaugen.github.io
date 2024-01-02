<Query Kind="Program">
  <NuGetReference>Microsoft.AspNetCore.App.Ref</NuGetReference>
  <Namespace>Microsoft.AspNetCore.Builder</Namespace>
  <Namespace>Microsoft.AspNetCore.Connections</Namespace>
  <Namespace>Microsoft.AspNetCore.Hosting</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Microsoft.Extensions.Hosting</Namespace>
  <Namespace>System.Buffers</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Net.Sockets</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

async Task Main()
{
    // Server IP and port
    var serverIP = "127.0.0.1"; // Localhost, adjust if your server is running on a different IP
    var serverPort = 6667;
    var message = "Hello, server!"; // Example message

    var response = await RunClientAsync(IPAddress.Parse(serverIP), serverPort, Encoding.UTF8.GetBytes(message));
	
	var responseMessage = Encoding.UTF8.GetString(response);
	responseMessage.Dump("Response");
}

async Task<byte[]> RunClientAsync(IPAddress serverIP, int serverPort, byte[] data)
{
	var response = Array.Empty<byte>();
    try
    {
        using (var client = new TcpClient())
        {
            await client.ConnectAsync(serverIP, serverPort);
            Console.WriteLine("Connected to the server.");
            using (var networkStream = client.GetStream())
            {
                await networkStream.WriteAsync(data, 0, data.Length);

                if (networkStream.CanRead)
                {
                    var buffer = new byte[4096];
                    var cts = new CancellationTokenSource(5000); // 5 seconds timeout
                    var task = networkStream.ReadAsync(buffer, 0, buffer.Length, cts.Token);
					try
					{
						var bytesRead = await task;
						response = buffer[..bytesRead];
					}
					catch (OperationCanceledException)
					{
						Console.WriteLine("Read timed out.");
					}
				}
			}
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine("Exception: " + ex.Message);
	}
	
	return response;
}

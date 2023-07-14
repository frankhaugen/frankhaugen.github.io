<Query Kind="Program">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
</Query>

#load ".\Connection"

async Task Main()
{
    var services = new ServiceCollection();

    services.Configure<ConnectionSettings>(options =>
    {
        options.Host = IPAddress.Loopback;
        options.Port = 1234;
    });

    services.AddLogging(x => x.AddProvider(new LinqPadLoggerProvider()));

    var serviceProvider = services.BuildServiceProvider();

    var connection = serviceProvider.GetRequiredService<Connection>();

    await connection.ConnectAsync();

    var buffer = new byte[1024];
    var bytesReceived = await connection.ReceiveAsync(buffer);

    Console.WriteLine($"Received {bytesReceived} bytes: {Encoding.UTF8.GetString(buffer, 0, bytesReceived)}");

    connection.Dispose();
}


<Query Kind="Program">
  <Namespace>System.Globalization</Namespace>
  <Namespace>Microsoft.AspNetCore.Hosting</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Microsoft.AspNetCore.Builder</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

static async Task Main(string[] args)
{
    var host = new WebHostBuilder()
        .UseKestrel(options => {
            options.ListenLocalhost(6667);
        })
        .Configure(app =>
        {
            app.Run(async context =>
            {
                // Handle incoming IRC messages
                var message = await ReadMessageAsync(context.Request.Body);
                Console.WriteLine($"Received message: {message}");

                // Send response
                var response = Encoding.UTF8.GetBytes("Hello, world!");
                context.Response.ContentType = "text/plain";
                context.Response.ContentLength = response.Length;
                await context.Response.Body.WriteAsync(response);
            });
        })
        .Build();

    await host.RunAsync(GetCancellationToken());
}

static CancellationToken GetCancellationToken() => new CancellationTokenSource(TimeSpan.FromMinutes(1)).Token;

static async Task<string> ReadMessageAsync(Stream stream)
{
    var buffer = new byte[1024];
    var message = new StringBuilder();
    int bytesRead;

    do
    {
        bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
        message.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
    } while (bytesRead == buffer.Length);

    return message.ToString();
}

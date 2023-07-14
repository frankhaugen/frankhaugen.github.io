using System.Net;
using Irc;
using Microsoft.AspNetCore.Connections;

var serviceProvider = new ServiceCollection().BuildServiceProvider();
var builder = new ConnectionBuilder(serviceProvider);
        .UseSockets(options =>
        {
            options.IOQueueCount = 4;
        })
        .UseConnectionHandler<MyConnectionHandler>();

    var server = builder.Build();
    
    

    Console.WriteLine("Server started. Press any key to stop.");
    Console.ReadKey();


// var builder = Host.CreateApplicationBuilder(args);




// builder.Services.Configure<IrcServerConfiguration>(options =>
// {
//     options.EndPoint = new IPEndPoint(IPAddress.Loopback, 6667);
//     options.Backlog = 10;
// });

// builder.Services.AddSingleton<IrcServer>();

// builder.Services.AddHostedService<Worker>();

// var host = builder.Build();
// host.Run();


csharp
public class IrcMessage
{
public string Prefix { get; set; }
public string Command { get; set; }
public List<string> Parameters { get; set; }

public IrcMessage(string prefix, string command, List<string> parameters)
{
Prefix = prefix;
Command = command;
Parameters = parameters;
}

// A method for converting an IrcMessage to a string
public override string ToString()
{
var sb = new StringBuilder();
if (!string.IsNullOrEmpty(Prefix))
{
sb.Append($":{Prefix} ");
}
sb.Append(Command);
foreach (var parameter in Parameters)
{
if (parameter.Contains(" "))
{
sb.Append($" :{parameter}");
}
else
{
sb.Append($" {parameter}");
}
}
return sb.ToString();
}

// A method for parsing a string to an IrcMessage
public static IrcMessage Parse(string line)
{
var prefix = "";
var command = "";
var parameters = new List<string>();
var index = 0;

// Parse the prefix if it exists
if (line.StartsWith(":"))
{
index = line.IndexOf(" ");
prefix = line.Substring(1, index - 1);
index++;
}

// Parse the command
var nextSpace = line.IndexOf(" ", index);
if (nextSpace == -1)
{
command = line.Substring(index);
return new IrcMessage(prefix, command, parameters);
}
else
{
command = line.Substring(index, nextSpace - index);
index = nextSpace + 1;
}

// Parse the parameters
while (index < line.Length)
{
nextSpace = line.IndexOf(" ", index);

// If the parameter starts with ":", it is the last one and may contain spaces
if (line[index] == ':')
{
parameters.Add(line.Substring(index + 1));
break;
}

// Otherwise, it is a normal parameter
if (nextSpace != -1)
{
parameters.Add(line.Substring(index, nextSpace - index));
index = nextSpace + 1;
}
else
{
parameters.Add(line.Substring(index));
break;
}
}

return new IrcMessage(prefix, command, parameters);
}
}
```

Here is an example of how to implement the IConnectionHandler interface:

```csharp
public class IrcConnectionHandler : IConnectionHandler
{
// A dictionary for storing users by connection id
private readonly ConcurrentDictionary<string, IrcUser> _users;

// A dictionary for storing channels by name
private readonly ConcurrentDictionary<string, IrcChannel> _channels;

public IrcConnectionHandler()
{
_users = new ConcurrentDictionary<string, IrcUser>();
_channels = new ConcurrentDictionary<string, IrcChannel>();
}

public async Task OnConnectedAsync(ConnectionContext connection)
{
// Create a new user for the connection
var user = new IrcUser(connection);

// Add the user to the dictionary
_users.TryAdd(connection.ConnectionId, user);

// Send a welcome message to the user
await user.SendMessageAsync("Welcome to the IRC server!");

// Start reading and writing data from the connection
await Task.WhenAll(ReadAsync(user), WriteAsync(user));
}

public Task OnDisconnectedAsync(ConnectionContext connection)
{
// Remove the user from the dictionary
_users.TryRemove(connection.ConnectionId, out var user);

// Remove the user from all the channels they joined
foreach (var channel in _channels.Values)
{
channel.RemoveUser(user);
}

// Notify other users that the user has quit
BroadcastMessageAsync(null, $"QUIT :{user.Nick} has left");

return Task.CompletedTask;
}

private async Task ReadAsync(IrcUser user)
{
try
{
// Get the reader from the connection
var reader = user.Connection.Transport.Input;

while (true)
{
// Read data from the reader
var result = await reader.ReadAsync();
var buffer = result.Buffer;

try
{
// Process the data if any
if (!buffer.IsEmpty)
{
// Loop through each line in the buffer
while (TryReadLine(ref buffer, out var line))
{
// Parse the line to an IrcMessage
var message = IrcMessage.Parse(line);

// Handle the message according to its command
await HandleMessageAsync(user, message);
}
}

// Check if the reader is completed
if (result.IsCompleted)
{
break;
}
}
finally
{
// Advance the reader to the consumed position
reader.AdvanceTo(buffer.Start, buffer.End);
}
}
}
catch (Exception ex)
{
// Log the exception
Console.WriteLine(ex);
}
finally
{
// Complete the reader
user.Connection.Transport.Input.Complete();
}
}

private async Task WriteAsync(IrcUser user)
{
try
{
// Get the writer from the connection
var writer = user.Connection.Transport.Output;

while (true)
{
// Get a message from the user's queue
var message = await user.OutputQueue.Reader.ReadAsync();

// Encode the message to bytes
var bytes = Encoding.UTF8.GetBytes(message + "\r\n");

// Write the bytes to the writer
await writer.WriteAsync(bytes);
}
}
catch (Exception ex)
{
// Log the exception
Console.WriteLine(ex);
}
finally
{
// Complete the writer
user.Connection.Transport.Output.Complete();
}
}

private bool TryReadLine(ref ReadOnlySequence<byte> buffer, out string line)
{
// Look for a \n in the buffer
var position = buffer.PositionOf((byte)'\n');

if (position == null)
{
line = null;
return false;
}

// Skip the \n and \r characters
var next = buffer.GetPosition(1, position.Value);
var end = next;
if (position.Value.GetInteger() > 0 && buffer.Slice(position.Value.GetInteger() - 1, 1).First.Span[0] == '\r')
{
end = buffer.GetPosition(-1, end);
}

// Convert the data to a string
line = Encoding.UTF8.GetString(buffer.Slice(0, end).ToArray());

// Advance the buffer past \n and \r characters
buffer = buffer.Slice(next);

return true;
}

private async Task HandleMessageAsync(IrcUser user, IrcMessage message)
{
switch (message.Command.ToUpper())
{
case "NICK":
await HandleNickAsync(user, message);
break;
case "USER":
await HandleUserAsync(user, message);
break;
case "JOIN":
await HandleJoinAsync(user, message);
break;
case "PART":
await HandlePartAsync(user, message);
break;
case "PRIVMSG":
await HandlePrivmsgAsync(user, message);
break;
case "PING":
await HandlePingAsync(user, message);
break;
case "PONG":
await HandlePongAsync(user, message);
break;
default:
await HandleUnknownAsync(user, message);
break;
}
}

private async Task HandleNickAsync(IrcUser user, IrcMessage message)
{
// TODO: Implement logic for changing nicknames and checking for conflicts

if (message.Parameters.Count > 0)
{
var newNick = message.Parameters[0];

if (!string.IsNullOrEmpty(newNick))

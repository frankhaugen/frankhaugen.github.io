<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net.Sockets</Namespace>
  <Namespace>System.Net</Namespace>
</Query>

using var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
await socket.ConnectAsync(new IPEndPoint(IPAddress.Loopback, 6667));

var ns = new NetworkStream(socket);

var value = Encoding.UTF8.GetBytes("Hello World is not a long message");

var readTask = Task.Run(() => ns.WriteAsync(value));
var writeTask = ns.CopyToAsync(Console.OpenStandardOutput());

// Quit if any of the tasks-.... complete
await Task.WhenAny(readTask, writeTask);
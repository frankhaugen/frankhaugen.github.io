<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Net.Sockets</Namespace>
  <Namespace>System.Net</Namespace>
</Query>

public class SocketOptions
{
    public IPAddress Address { get; set; }
    public int Port { get; set; }
    public ProtocolType Protocol { get; set; }
    public AddressFamily AddressFamily { get; set; }
    public SocketType SocketType { get; set; }
}
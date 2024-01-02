<Query Kind="Statements">
  <NuGetReference>Microsoft.AspNetCore.Connections.Abstractions</NuGetReference>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>



using Microsoft.AspNetCore.Connections;
using System;
using System.Text;
using System.Threading.Tasks;

public class Sender
{
    private readonly ConnectionContext _connection;

    public Sender(ConnectionContext connection)
    {
        _connection = connection;
    }

    public async Task SendData(string data)
    {
        var payload = Encoding.UTF8.GetBytes(data);
        await _connection.Transport.Output.WriteAsync(payload);
    }
}
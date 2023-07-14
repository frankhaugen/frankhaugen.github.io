using System;
using System.Buffers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;

class MyConnectionHandler : ConnectionHandler
{
    public override async Task OnConnectedAsync(ConnectionContext connection)
    {
        Console.WriteLine($"New connection: {connection.ConnectionId}");

        while (true)
        {
            var message = await connection.Transport.Input.ReadAsync();
            if (message.IsCompleted)
            {
                break;
            }

            // TODO: Process the message according to the protocol

            // connection.Transport.Output.Write(message.Buffer);
            await connection.Transport.Output.FlushAsync();
        }

        Console.WriteLine($"Connection closed: {connection.ConnectionId}");
    }

}
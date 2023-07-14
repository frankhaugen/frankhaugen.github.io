<Query Kind="Program">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Net.Sockets</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>Xunit</Namespace>
</Query>

#load "xunit"

void Main()
{
    RunTests();  // Call RunTests() or press Alt+Shift+T to initiate testing.
}

public class SocketOptionsBuilder : ISocketOptionsBuilder
{
    public IPAddress? Address { get; private set; }
    public int? Port { get; private set; }
    public ProtocolType? Protocol { get; private set; }
    public AddressFamily? AddressFamily { get; private set; }
    public SocketType? SocketType { get; private set; }

    public ISocketOptionsBuilder WithAddress(IPAddress address)
    {
        Address = address;
        return this;
    }

    public ISocketOptionsBuilder WithPort(int port)
    {
        Port = port;
        return this;
    }

    public ISocketOptionsBuilder WithProtocol(ProtocolType protocol)
    {
        Protocol = protocol;
        return this;
    }

    public ISocketOptionsBuilder WithAddressFamily(AddressFamily addressFamily)
    {
        AddressFamily = addressFamily;
        return this;
    }

    public ISocketOptionsBuilder WithSocketType(SocketType socketType)
    {
        SocketType = socketType;
        return this;
    }

    public Socket Build()
    {
        var errors = new List<string>();
        if (Address == null)
            errors.Add("Address is not set");
        if (Port == null)
            errors.Add("Port is not set");
        if (Protocol == null)
            errors.Add("Protocol is not set");
        if (AddressFamily == null)
            errors.Add("AddressFamily is not set");
        if (SocketType == null)
            errors.Add("SocketType is not set");
        if (errors.Any())
            throw new InvalidOperationException(string.Join(", ", errors));

        var socket = new Socket(AddressFamily.Value, SocketType.Value, Protocol.Value);
        socket.Bind(new IPEndPoint(Address, Port.Value));
        return socket;
    }
}

public interface ISocketOptionsBuilder
{
    ISocketOptionsBuilder WithAddress(IPAddress address);
    ISocketOptionsBuilder WithPort(int port);
    ISocketOptionsBuilder WithProtocol(ProtocolType protocol);
    ISocketOptionsBuilder WithAddressFamily(AddressFamily addressFamily);
    ISocketOptionsBuilder WithSocketType(SocketType socketType);
    Socket Build();
}

#region private::Tests

[Fact]
public void Build_ThrowsException_WhenAddressIsNull()
{
    // Arrange
    var builder = new SocketOptionsBuilder()
        .WithPort(1234)
        .WithProtocol(ProtocolType.Tcp)
        .WithAddressFamily(AddressFamily.InterNetwork)
        .WithSocketType(SocketType.Stream);

    // Act & Assert
    Assert.Throws<InvalidOperationException>(() => builder.Build());
}

[Fact]
public void Build_ThrowsException_WhenPortIsNull()
{
    // Arrange
    var builder = new SocketOptionsBuilder()
        .WithAddress(IPAddress.Loopback)
        .WithProtocol(ProtocolType.Tcp)
        .WithAddressFamily(AddressFamily.InterNetwork)
        .WithSocketType(SocketType.Stream);

    // Act & Assert
    Assert.Throws<InvalidOperationException>(() => builder.Build());
}

[Fact]
public void Build_ThrowsException_WhenProtocolIsNull()
{
    // Arrange
    var builder = new SocketOptionsBuilder()
        .WithAddress(IPAddress.Loopback)
        .WithPort(1234)
        .WithAddressFamily(AddressFamily.InterNetwork)
        .WithSocketType(SocketType.Stream);

    // Act & Assert
    Assert.Throws<InvalidOperationException>(() => builder.Build());
}

[Fact]
public void Build_ThrowsException_WhenAddressFamilyIsNull()
{
    // Arrange
    var builder = new SocketOptionsBuilder()
        .WithAddress(IPAddress.Loopback)
        .WithPort(1234)
        .WithProtocol(ProtocolType.Tcp)
        .WithSocketType(SocketType.Stream);

    // Act & Assert
    Assert.Throws<InvalidOperationException>(() => builder.Build());
}

[Fact]
public void Build_ThrowsException_WhenSocketTypeIsNull()
{
    // Arrange
    var builder = new SocketOptionsBuilder()
        .WithAddress(IPAddress.Loopback)
        .WithPort(1234)
        .WithProtocol(ProtocolType.Tcp)
        .WithAddressFamily(AddressFamily.InterNetwork);

    // Act & Assert
    Assert.Throws<InvalidOperationException>(() => builder.Build());
}

[Fact]
public void Build_ReturnsSocket_WhenAllPropertiesAreSet()
{
    // Arrange
    var builder = new SocketOptionsBuilder()
        .WithAddress(IPAddress.Loopback)
        .WithPort(1234)
        .WithProtocol(ProtocolType.Tcp)
        .WithAddressFamily(AddressFamily.InterNetwork)
        .WithSocketType(SocketType.Stream);

    // Act
    var socket = builder.Build();

    // Assert
    Assert.NotNull(socket);
    Assert.Equal(AddressFamily.InterNetwork, socket.AddressFamily);
    Assert.Equal(SocketType.Stream, socket.SocketType);
    Assert.Equal(ProtocolType.Tcp, socket.ProtocolType);
    Assert.Equal(new IPEndPoint(IPAddress.Loopback, 1234), socket.LocalEndPoint);
}

#endregion
<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Net.Sockets</Namespace>
</Query>

var client = new NetworkClient(ProtocolType.Tcp);
client.Open(IPAddress.Loopback, 12345);

client.Send(Encoding.UTF8.GetBytes("Hello world!"));
client.Close();

public abstract class NetworkResource
{
    protected readonly TcpClient _tcpClient;
    protected readonly UdpClient _udpClient;
    protected readonly ProtocolType _protocolType;

    protected NetworkResource(ProtocolType protocolType)
    {
        _protocolType = protocolType;

        switch (_protocolType)
        {
            case ProtocolType.Tcp:
                _tcpClient = new TcpClient();
                break;
            case ProtocolType.Udp:
                _udpClient = new UdpClient();
                break;
            default:
                throw new NotSupportedException($"Unsupported protocol type: {_protocolType}");
        }
    }

    public void Open(IPAddress address, int port)
    {
        switch (_protocolType)
        {
            case ProtocolType.Tcp:
                _tcpClient.Connect(address, port);
                break;
            case ProtocolType.Udp:
                _udpClient.Connect(address, port);
                break;
            default:
                throw new NotSupportedException($"Unsupported protocol type: {_protocolType}");
        }
    }

    public void Close()
    {
        switch (_protocolType)
        {
            case ProtocolType.Tcp:
                _tcpClient.Close();
                break;
            case ProtocolType.Udp:
                _udpClient.Close();
                break;
            default:
                throw new NotSupportedException($"Unsupported protocol type: {_protocolType}");
        }
    }

}

public class NetworkServer : NetworkResource
{
    public NetworkServer(ProtocolType protocolType) : base(protocolType)
    {
    }

    public byte[] Receive()
    {
        byte[] data;

        switch (_protocolType)
        {
            case ProtocolType.Tcp:
                NetworkStream stream = _tcpClient.GetStream();
                data = new byte[1024];
                int bytesRead = stream.Read(data, 0, data.Length);
                Array.Resize(ref data, bytesRead);
                break;
            case ProtocolType.Udp:
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                data = _udpClient.Receive(ref remoteEP);
                break;
            default:
                throw new NotSupportedException($"Unsupported protocol type: {_protocolType}");
        }

        return data;
    }
}

public class NetworkClient : NetworkResource
{
    public NetworkClient(ProtocolType protocolType) : base(protocolType)
    {
    }

    public void Send(byte[] data)
    {
        switch (_protocolType)
        {
            case ProtocolType.Tcp:
                NetworkStream stream = _tcpClient.GetStream();
                stream.Write(data, 0, data.Length);
                break;
            case ProtocolType.Udp:
                _udpClient.Send(data, data.Length);
                break;
            default:
                throw new NotSupportedException($"Unsupported protocol type: {_protocolType}");
        }
    }
}
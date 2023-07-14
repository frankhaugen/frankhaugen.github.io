<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Net.Sockets</Namespace>
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
</Query>

public readonly struct SocketIdentity : IEquatable<SocketIdentity>, IComparable<SocketIdentity>, IParsable<SocketIdentity>
{
    public string Hostname { get; }
    public int Port { get; }
    public ProtocolType Protocol { get; }

    public SocketIdentity(string hostname, int port, ProtocolType protocol)
    {
        Hostname = hostname;
        Port = port;
        Protocol = protocol;
    }

    public override string ToString()
    {
        return $"{Protocol}:{Hostname}:{Port}";
    }

    public bool Equals(SocketIdentity other)
    {
        return Hostname == other.Hostname && Port == other.Port && Protocol == other.Protocol;
    }

    public override bool Equals(object obj)
    {
        if (obj is SocketIdentity other)
        {
            return Equals(other);
        }
        return false;
    }

    public int CompareTo(SocketIdentity other)
    {
        int result = Protocol.CompareTo(other.Protocol);
        if (result == 0)
        {
            result = Hostname.CompareTo(other.Hostname);
            if (result == 0)
            {
                result = Port.CompareTo(other.Port);
            }
        }
        return result;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Hostname, Port, Protocol);
    }

    public static SocketIdentity Parse(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentException("Input string cannot be null or empty.", nameof(input));
        }

        string[] parts = input.Split(':');
        if (parts.Length != 3)
        {
            throw new ArgumentException("Input string must contain three parts separated by colons.", nameof(input));
        }

        if (!int.TryParse(parts[2], out int port))
        {
            throw new ArgumentException("Port number must be a valid integer.", nameof(input));
        }

        if (!Enum.TryParse<ProtocolType>(parts[0], out ProtocolType protocol))
        {
            throw new ArgumentException("Protocol type must be a valid value from the ProtocolType enumeration.", nameof(input));
        }

        return new SocketIdentity(parts[1], port, protocol);
    }
    
    public static bool TryParse(string input, [MaybeNullWhen(false)] out SocketIdentity result)
    {
        return TryParse(input, null, out result);
    }

    public static SocketIdentity Parse(string s, IFormatProvider? provider)
    {
        return Parse(s);
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out SocketIdentity result)
    {
        result = default;
        if (string.IsNullOrEmpty(s))
        {
            return false;
        }

        try
        {
            result = Parse(s);
            return true;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }
}
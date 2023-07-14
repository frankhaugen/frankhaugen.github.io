<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Net.Sockets</Namespace>
</Query>

#load ".\SocketIdentity"
#load ".\SocketBuilder"


public class SocketList : IEnumerable<Socket>
{
    private readonly Dictionary<SocketIdentity, Socket> Sockets = new();
    
    public IEnumerator<Socket> GetEnumerator()
    {
        foreach (var socket in Sockets.Values)
        {
            yield return socket;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    
}
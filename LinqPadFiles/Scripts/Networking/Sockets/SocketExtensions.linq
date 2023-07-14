<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
</Query>

#load ".\SocketIdentity"





public static class SocketExtensions
{
    public static SocketIdentity GetIdentity(this Socket socket)
    {
        return new SocketIdentity(socket)
        ;
    }
}
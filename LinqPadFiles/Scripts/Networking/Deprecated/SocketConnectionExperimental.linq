<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
</Query>

public class IrcMessage : IParsable<IrcMessage>
{
    public string Prefix { get; set; }
    public string Command { get; set; }
    public List<string> Parameters { get; set; }

    public IrcMessage Parse(string message)
    {
        Parameters = new List<string>();

        if (message.StartsWith(":"))
        {
            var prefixEnd = message.IndexOf(' ');
            Prefix = message.Substring(1, prefixEnd - 1);
            message = message.Substring(prefixEnd + 1);
        }

        var commandEnd = message.IndexOf(' ');
        if (commandEnd == -1)
        {
            Command = message;
            return this;
        }

        Command = message.Substring(0, commandEnd);
        message = message.Substring(commandEnd + 1);

        while (!string.IsNullOrEmpty(message))
        {
            if (message.StartsWith(":"))
            {
                Parameters.Add(message.Substring(1));
                break;
            }

            var parameterEnd = message.IndexOf(' ');
            if (parameterEnd == -1)
            {
                Parameters.Add(message);
                break;
            }

            Parameters.Add(message.Substring(0, parameterEnd));
            message = message.Substring(parameterEnd + 1);
        }

        return this;
    }
}
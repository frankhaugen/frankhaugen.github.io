<Query Kind="Program">
  <NuGetReference Prerelease="true">Bedrock.Framework</NuGetReference>
  <Namespace>Bedrock.Framework</Namespace>
  <Namespace>Microsoft.AspNetCore.Connections</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Bedrock.Framework.Protocols</Namespace>
  <Namespace>System.Buffers</Namespace>
  <Namespace>System.IO.Pipelines</Namespace>
  <Namespace>Xunit</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

#load "xunit"

void Main()
{
    RunTests();  // Call RunTests() or press Alt+Shift+T to initiate testing.
}

public class IrcProtocol : IMessageReader<IrcMessage>, IMessageWriter<IrcMessage>
{
    public bool TryParseMessage(in ReadOnlySequence<byte> input, ref SequencePosition consumed, ref SequencePosition examined, out IrcMessage message)
    {
        ReadOnlySpan<byte> buffer = input.FirstSpan;
        var rawMessage = Encoding.UTF8.GetString(buffer.ToArray());
        message = new IrcMessage(rawMessage);
        consumed = input.End;
        return true;
    }

    public void WriteMessage(IrcMessage message, IBufferWriter<byte> output)
    {
        Encoding.UTF8.GetBytes(message.ToString(), output);
    }
}

public class IrcMessagePrefix
{
    public string? Nick { get; set; }
    public string? User { get; set; }
    public string? Host { get; set; }

    public IrcMessagePrefix(string prefix)
    {
        if (prefix.StartsWith(":"))
        {
            prefix = prefix.Substring(1);
        }

        var parts = prefix.Split('@');
        if (parts.Length == 2)
        {
            Host = parts[1];
            var nickUser = parts[0].Split('!');
            if (nickUser.Length == 2)
            {
                Nick = nickUser[0];
                User = nickUser[1];
            }
            else
            {
                Nick = parts[0];
            }
        }
        else
        {
            Nick = prefix;
        }
    }

    public override string ToString()
    {
        if (Nick != null)
        {
            if (User != null && Host != null)
            {
                return $"{Nick}!{User}@{Host}";
            }
            else
            {
                return $"{Nick}";
            }
        }
        else
        {
            return "";
        }
    }
}
public class IrcMessage
{
    public IrcMessagePrefix? Prefix { get; }
    public string? Command { get; }
    public string? Channel { get; set; }
    public string? Message { get; set; }

    public IrcMessage(string message)
    {
        var parts = message.Split(' ');

        if (parts[0].StartsWith(":"))
        {
            Prefix = new IrcMessagePrefix(parts[0].Substring(1));
            Command = parts[1];
        }
        else
        {
            Command = parts[0];
        }
        
        for (int i = Prefix != null ? 2 : 1; i < parts.Length; i++)
        {
            var part = parts[i];
            if (part.StartsWith("#"))
            {
                Channel = part;
            }
            else if (part.StartsWith(":"))
            {
                Message = message.Substring(message.IndexOf(part) + 1);
                break;
            }
            else if (i == parts.Length - 1)
            {
                Message = part;
            }
        }
    }

    public override string ToString()
    {
        var parts = new List<string>();
        if (Prefix != null)
        {
            parts.Add($":{Prefix}");
        }

        if (Command != null)
        {
            parts.Add(Command);
        }

        if (Channel != null)
        {
            parts.Add(Channel);
        }

        if (Message != null)
        {
            parts.Add($":{Message}");
        }

        return string.Join(" ", parts);
    }
}



public enum IrcCodeName
{
    RPL_WELCOME = 001,
    RPL_YOURHOST = 002,
    RPL_CREATED = 003,
    RPL_MYINFO = 004,
    RPL_BOUNCE = 005,
    RPL_TRACELINK = 200,
    RPL_TRACECONNECTING = 201,
    RPL_TRACEHANDSHAKE = 202,
    RPL_TRACEUNKNOWN = 203,
    RPL_TRACEOPERATOR = 204,
    RPL_TRACEUSER = 205,
    RPL_TRACESERVER = 206,
    RPL_TRACESERVICE = 207,
    RPL_TRACENEWTYPE = 208,
    RPL_TRACECLASS = 209,
    RPL_TRACERECONNECT = 210,
    RPL_STATSLINKINFO = 211,
    RPL_STATSCOMMANDS = 212,
    RPL_ENDOFSTATS = 219,
    RPL_UMODEIS = 221,
    RPL_SERVLIST = 234,
    RPL_SERVLISTEND = 235,
    RPL_STATSUPTIME = 242,
    RPL_STATSOLINE = 243,
    RPL_LUSERCLIENT = 251,
    RPL_LUSEROP = 252,
    RPL_LUSERUNKNOWN = 253,
    RPL_LUSERCHANNELS = 254,
    RPL_LUSERME = 255,
    RPL_ADMINME = 256,
    RPL_ADMINLOC1 = 257,
    RPL_ADMINLOC2 = 258,
    RPL_ADMINEMAIL = 259,
    RPL_TRACELOG = 261,
    RPL_NONE = 300,
    RPL_AWAY = 301,
    RPL_USERHOST = 302,
    RPL_ISON = 303,
    RPL_UNAWAY = 305,
    RPL_NOWAWAY = 306,
    RPL_WHOISUSER = 311,
    RPL_WHOISSERVER = 312,
    RPL_WHOISOPERATOR = 313,
    RPL_WHOWASUSER = 314,
    RPL_ENDOFWHO = 315,
    RPL_WHOISIDLE = 317,
    RPL_ENDOFWHOIS = 318,
    RPL_WHOISCHANNELS = 319,
    RPL_LISTSTART = 321,
    RPL_LIST = 322,
    RPL_LISTEND = 323,
    RPL_CHANNELMODEIS = 324,
    RPL_NOTOPIC = 331,
    RPL_TOPIC = 332,
    RPL_INVITING = 341,
    RPL_SUMMONING = 342,
    RPL_VERSION = 351,
    RPL_WHOREPLY = 352,
    RPL_NAMREPLY = 353,
    RPL_LINKS = 364,
    RPL_ENDOFLINKS = 365,
    RPL_ENDOFNAMES = 366,
    RPL_BANLIST = 367,
    RPL_ENDOFBANLIST = 368,
    RPL_ENDOFWHOWAS = 369,
    RPL_INFO = 371,
    RPL_MOTD = 372,
    RPL_ENDOFINFO = 374,
    RPL_MOTDSTART = 375,
    RPL_ENDOFMOTD = 376,
    RPL_YOUREOPER = 381,
    RPL_REHASHING = 382,
    RPL_TIME = 391,
    RPL_USERSSTART = 392,
    RPL_USERS = 393,
    RPL_ENDOFUSERS = 394,
    RPL_NOUSERS = 395,
    ERR_NOSUCHNICK = 401,
    ERR_NOSUCHSERVER = 402,
    ERR_NOSUCHCHANNEL = 403,
    ERR_CANNOTSENDTOCHAN = 404,
    ERR_TOOMANYCHANNELS = 405,
    ERR_WASNOSUCHNICK = 406,
    ERR_TOOMANYTARGETS = 407,
    ERR_NOSUCHSERVICE = 408,
    ERR_NOORIGIN = 409,
    ERR_NORECIPIENT = 411,
    ERR_NOTEXTTOSEND = 412,
    ERR_NOTOPLEVEL = 413,
    ERR_WILDTOPLEVEL = 414,
    ERR_BADMASK = 415,
    ERR_UNKNOWNCOMMAND = 421,
    ERR_NOMOTD = 422,
    ERR_NOADMININFO = 423,
    ERR_FILEERROR = 424,
    ERR_NONICKNAMEGIVEN = 431,
    ERR_ERRONEUSNICKNAME = 432,
    ERR_NICKNAMEINUSE = 433,
    ERR_NICKCOLLISION = 436,
    ERR_UNAVAILRESOURCE = 437,
    ERR_USERNOTINCHANNEL = 441,
    ERR_NOTONCHANNEL = 442,
    ERR_USERONCHANNEL = 443,
    ERR_NOLOGIN = 444,
    ERR_SUMMONDISABLED = 445,
    ERR_USERSDISABLED = 446,
    ERR_NOTREGISTERED = 451,
    ERR_NEEDMOREPARAMS = 461,
    ERR_ALREADYREGISTRED = 462,
    ERR_NOPERMFORHOST = 463,
    ERR_PASSWDMISMATCH = 464,
    ERR_YOUREBANNEDCREEP = 465,
    ERR_KEYSET = 467,
    ERR_CHANNELISFULL = 471,
    ERR_UNKNOWNMODE = 472,
    ERR_INVITEONLYCHAN = 473,
    ERR_BANNEDFROMCHAN = 474,
    ERR_BADCHANNELKEY = 475,
    ERR_BADCHANMASK = 476,
    ERR_NOCHANMODES = 477,
    ERR_BANLISTFULL = 478,
    ERR_NOPRIVILEGES = 481,
    ERR_CHANOPRIVSNEEDED = 482,
    ERR_CANTKILLSERVER = 483,
    ERR_RESTRICTED = 484,
    ERR_UNIQOPPRIVSNEEDED = 485,
    ERR_NOOPERHOST = 491,
    ERR_UMODEUNKNOWNFLAG = 501,
    ERR_USERSDONTMATCH = 502,
    RPL_SERVICEINFO = 231,
    RPL_ENDOFSERVICES = 232,
    RPL_SERVICE = 233,
    RPL_WHOISCHANOP = 316,
    RPL_KILLDONE = 361,
    RPL_CLOSING = 362,
    RPL_CLOSEEND = 363,
    RPL_INFOSTART = 373,
    RPL_MYPORTIS = 384,
    ERR_YOUWILLBEBANNED = 466,
    ERR_BADCHANNAME = 479,
    ERR_KILLDENYED = 488,
    ERR_NOULINE = 491,
    ERR_NOSERVICEHOST = 492,
    ERR_NOSERVICE = 493,
    ERR_NOFEATURE = 494,
    ERR_BADFEATURE = 495,
    ERR_BADLOGTYPE = 496,
    ERR_BADLOGSYS = 497,
    ERR_BADLOGVALUE = 498,
    ERR_ISOPERLCHAN = 499,
    ERR_NUMERICERR = 999,
}

public enum IrcCommandName
{
    Admin,
    Away,
    Connect,
    Die,
    Error,
    Info,
    Invite,
    Ison,
    Join,
    Kick,
    Kill,
    Links,
    List,
    Lusers,
    Mode,
    Motd,
    Names,
    Nick,
    Notice,
    Oper,
    Part,
    Pass,
    Ping,
    Pong,
    Privmsg,
    Quit,
    Rehash,
    Restart,
    Service,
    Servlist,
    Server,
    Squery,
    Squit,
    Stats,
    Summon,
    Time,
    Topic,
    Trace,
    User,
    Userhost,
    Users,
    Version,
    Wallops,
    Who,
    Whois,
    Whowas
}


#region private::Tests

public class IrcMessageTests
{
    [Fact]
    public void TestParsing()
    {
        var message = new IrcMessage(":nick!user@host PRIVMSG #channel :Hello, world!");
        Assert.Equal("nick", message.Prefix.Nick);
        Assert.Equal("user", message.Prefix.User); 
        Assert.Equal("host", message.Prefix.Host);
        Assert.Equal("nick!user@host", message.Prefix.ToString());
        Assert.Equal("PRIVMSG", message.Command);
        Assert.Equal("#channel", message.Channel);
        Assert.Equal("Hello, world!", message.Message);
    }

    [Fact]
    public void TestWriting()
    {
        var message = new IrcMessage(":nick!user@host PRIVMSG #channel :Hello, world!");
        var writer = new ArrayBufferWriter<byte>();
        var protocol = new IrcProtocol();
        protocol.WriteMessage(message, writer);
        var bytes = writer.WrittenSpan.ToArray();
        var expected = Encoding.UTF8.GetBytes(":nick!user@host PRIVMSG #channel :Hello, world!");
        Assert.Equal(expected, bytes);
    }
}

#endregion
<Query Kind="Statements">
  <NuGetReference>CsvHelper</NuGetReference>
  <Namespace>System.Globalization</Namespace>
  <Namespace>CsvHelper.Configuration</Namespace>
  <Namespace>CsvHelper</Namespace>
</Query>

var data = CsvUtil.ReadCsvData<ReplyInfo, ReplyInfoClassMap>(TextUtil.RemoveWikipediaReferences(GetReplyInfoData()), ";");

var result = ProcessIrcReplyInfo(data);

result.Dump();


//data.DumpCSharp();
//data.Dump();

string ProcessIrcReplyInfo(IEnumerable<ReplyInfo> values)
{
    var codeGenerator = new CodeGenerator();
    //var code = codeGenerator.Enums.Generate("ReplyName", values.Select(x => new KeyValuePair<int, string>(x.Number, x.Name)));
    
    var name = "ReplyConstants";
    
    var definitions = new List<Tuple<string, IEnumerable<Tuple<string, string, string>>>>();
    
    foreach (var value in values)
    {
        var innerName = TextUtil.Capitalize(value.Name.Remove(0, 4));
        var type = value.Name.Substring(0, 4);
        var number = value.Number;
        var syntax = value.Format;

        var innerDefinitions = new List<Tuple<string, string, string>>()
        {
            new("string", "DisplayName", $"\"{innerName}\""),
            new("string", "Name", $"\"{value.Name}\""),
            new("ReplyVariant", "Variant", $"ReplyVariant.{type.Replace("_", "")}"),
            new("int", "Number", $"{number}"),
            new("string", "Format", $"\"{syntax}\""),
            //new("string", "Helptext", $"\"{TextUtil.Clean(helpText)}\"")
        };

        definitions.Add(new(innerName.Replace("7", "Seven"), innerDefinitions)); 
    }
    
    
    var code = codeGenerator.Constants.Generate(name, definitions.DistinctBy(x => x.Item1)); 
    
    return code;
}

string ProcessIrcCommandInfo(IEnumerable<CommandInfo> values)
{
    var codeBuilder = new StringBuilder();
    var codeGenerator = new CodeGenerator();

    foreach (var element in values)
    {
        var name = TextUtil.Capitalize(element.Command.ToString());
        var command = element.Command.ToString();
        var syntax = element.Syntax;
        var helpText = element.Helptext;

        var definition = new List<Tuple<string, string, string>>()
        {
            new("string", "DisplayName", $"\"{name}\""),
            new("CommandName", "Command", "CommandName." + command),
            new("string", "Syntax", $"\"{TextUtil.Clean(syntax)}\""),
            new("string", "Helptext", $"\"{TextUtil.Clean(helpText)}\"")
        }
        ;



        var tempCode = codeGenerator.Constants.Generate(name, definition);

        codeBuilder.AppendLine(tempCode);
    }

    return codeBuilder.ToString();
}

string GetReplyInfoData() =>
"""""
Number;Name;Origin;Format;Comments
001;RPL_WELCOME;RFC2812;<client> :Welcome to the Internet Relay Network <nick>!<user>@<host>;The first message sent after client registration. The text used varies widely
002;RPL_YOURHOST;RFC2812;<client> :Your host is <servername>, running version <version>;Part of the post-registration greeting. Text varies widely. Also known as RPL_YOURHOSTIS (InspIRCd v2)
003;RPL_CREATED;RFC2812;<client> :This server was created <date>;Part of the post-registration greeting. Text varies widely and <date> is returned in a human-readable format. Also known as RPL_SERVERCREATED (InspIRCd v2)
004;RPL_MYINFO;RFC2812;<client> <server_name> <version> <usermodes> <chanmodes> [chanmodes_with_a_parameter];Part of the post-registration greeting. Also known as RPL_SERVERVERSION (InspIRCd v2)
005;RPL_BOUNCE;RFC2812;<client> :Try server <server_name>, port <port_number>;Sent by the server to a user to suggest an alternative server, sometimes used when the connection is refused because the server is already full. Also known as RPL_SLINE (AustHex), and RPL_REDIR Also see RPL_BOUNCE (010).
005;RPL_ISUPPORT;;<client> <1-13 tokens> :are supported by this server;Advertises features, limits, and protocol options that clients should be aware of. Also known as RPL_PROTOCTL (Bahamut, Unreal, Ultimate) Also see RPL_REMOTEISUPPORT (105).
006;RPL_MAP;Unreal;;
007;RPL_MAPEND;Unreal;;Also known as RPL_ENDMAP (InspIRCd)
008;RPL_SNOMASK;ircu;;Server notice mask (hex). Also known as RPL_SNOMASKIS (InspIRCd)
009;RPL_STATMEMTOT;ircu;;
010;RPL_BOUNCE;;<client> <hostname> <port> :<info>;Sent to the client to redirect it to another server. Also known as RPL_REDIR
010;RPL_STATMEM;ircu;;
014;RPL_YOURCOOKIE;Hybrid?;;
015;RPL_MAP;ircu;;
016;RPL_MAPMORE;ircu;;
017;RPL_MAPEND;ircu;;Also known as RPL_ENDMAP (InspIRCd)
018;RPL_MAPUSERS;InspIRCd;<client> :<count> servers and <count> users, average <average count> users per server;
020;RPL_HELLO;rusnet-ircd;<client> :<info>;"Used by Rusnet to send the initial ""Please wait while we process your connection"" message, rather than a server-sent NOTICE."
030;RPL_APASSWARN_SET;ircu;;
031;RPL_APASSWARN_SECRET;ircu;;
032;RPL_APASSWARN_CLEAR;ircu;;
042;RPL_YOURID;IRCnet;;Also known as RPL_YOURUUID (InspIRCd)
043;RPL_SAVENICK;IRCnet;<client> <newnick> :<info>;Sent to the client when their nickname was forced to change due to a collision
050;RPL_ATTEMPTINGJUNC;aircd;;
051;RPL_ATTEMPTINGREROUTE;aircd;;
105;RPL_REMOTEISUPPORT;Unreal;;Same format as RPL_ISUPPORT, but returned when the client is requesting information from a remote server instead of the server it is currently connected to Also see RPL_ISUPPORT (005).
200;RPL_TRACELINK;RFC1459;<client> Link <version>[.<debug_level>] <destination> <next_server> [V<protocol_version> <link_uptime_in_seconds> <backstream_sendq> <upstream_sendq>];See RFC
201;RPL_TRACECONNECTING;RFC1459;<client> Try. <class> <server>;See RFC
202;RPL_TRACEHANDSHAKE;RFC1459;<client> H.S. <class> <server>;See RFC
203;RPL_TRACEUNKNOWN;RFC1459;<client> ???? <class> [<connection_address>];See RFC
204;RPL_TRACEOPERATOR;RFC1459;<client> Oper <class> <nick>;See RFC
205;RPL_TRACEUSER;RFC1459;<client> User <class> <nick>;See RFC
206;RPL_TRACESERVER;RFC1459;<client> Serv <class> <int>S <int>C <server> <nick!user|*!*>@<host|server> [V<protocol_version>];See RFC
207;RPL_TRACESERVICE;RFC2812;<client> Service <class> <name> <type> <active_type>;See RFC
208;RPL_TRACENEWTYPE;RFC1459;<client> <newtype> 0 <client_name>;See RFC
209;RPL_TRACECLASS;RFC2812;<client> Class <class> <count>;See RFC
210;RPL_TRACERECONNECT;RFC2812;;
210;RPL_STATS;aircd;;Used instead of having multiple stats numerics
210;RPL_STATSHELP;Unreal;;Used to send lists of stats flags and other help information.
211;RPL_STATSLINKINFO;RFC1459;<client> <linkname> <sendq> <sent_msgs> <sent_bytes> <recvd_msgs> <rcvd_bytes> <time_open>;Reply to STATS (See RFC)
212;RPL_STATSCOMMANDS;RFC1459;<client> <command> <count> [<byte_count> <remote_count>];Reply to STATS (See RFC)
213;RPL_STATSCLINE;RFC1459;<client> C <host> * <name> <port> <class>;Reply to STATS (See RFC)
214;RPL_STATSNLINE;RFC1459;<client> N <host> * <name> <port> <class>;Reply to STATS (See RFC), Also known as RPL_STATSOLDNLINE (ircu, Unreal)
215;RPL_STATSILINE;RFC1459;<client> I <host> * <host> <port> <class>;Reply to STATS (See RFC)
216;RPL_STATSKLINE;RFC1459;<client> K <host> * <username> <port> <class>;Reply to STATS (See RFC)
217;RPL_STATSQLINE;RFC1459;;
217;RPL_STATSPLINE;ircu;;
218;RPL_STATSYLINE;RFC1459;<client> Y <class> <ping_freq> <connect_freq> <max_sendq>;Reply to STATS (See RFC)
219;RPL_ENDOFSTATS;RFC1459;<client> <query> :<info>;End of RPL_STATS* list.
220;RPL_STATSPLINE;Hybrid;;
220;RPL_STATSBLINE;Bahamut, Unreal;;
220;RPL_STATSWLINE;Nefarious;;
221;RPL_UMODEIS;RFC1459;<client> <user_modes> [<user_mode_params>];Information about a user's own modes. Some daemons have extended the mode command and certain modes take parameters (like channel modes).
222;RPL_MODLIST;;;
222;RPL_SQLINE_NICK;Unreal;;
222;RPL_STATSBLINE;Bahamut;;
222;RPL_STATSJLINE;ircu;;
222;RPL_CODEPAGE;rusnet-ircd;;
223;RPL_STATSELINE;Bahamut;;
223;RPL_STATSGLINE;Unreal;;
223;RPL_CHARSET;rusnet-ircd;;
224;RPL_STATSFLINE;Hybrid, Bahamut;;
224;RPL_STATSTLINE;Unreal;;
225;RPL_STATSDLINE;Hybrid;;
225;RPL_STATSCLONE;Bahamut;;
225;RPL_STATSZLINE;Bahamut;;
225;RPL_STATSELINE;Unreal;;
226;RPL_STATSCOUNT;Bahamut;;
226;RPL_STATSALINE;Hybrid;;
226;RPL_STATSNLINE;Unreal;;
227;RPL_STATSGLINE;Bahamut;;
227;RPL_STATSVLINE;Unreal;;
227;RPL_STATSBLINE;Rizon;;Returns details about active DNS blacklists and hits.
228;RPL_STATSQLINE;ircu;;
228;RPL_STATSBANVER;Unreal;;
229;RPL_STATSSPAMF;Unreal;;
230;RPL_STATSEXCEPTTKL;Unreal;;
231;RPL_SERVICEINFO;RFC1459;;
232;RPL_ENDOFSERVICES;RFC1459;;
232;RPL_RULES;Unreal;;
233;RPL_SERVICE;RFC1459;;
234;RPL_SERVLIST;RFC2812;<client> <name> <server> <mask> <type> <hopcount> <info>;A service entry in the service list
235;RPL_SERVLISTEND;RFC2812;<client> <mask> <type> :<info>;Termination of an RPL_SERVLIST list
236;RPL_STATSVERBOSE;ircu;;Verbose server list?
237;RPL_STATSENGINE;ircu;;Engine name?
238;RPL_STATSFLINE;ircu;;Feature lines?
239;RPL_STATSIAUTH;IRCnet;;
240;RPL_STATSVLINE;RFC2812;;
240;RPL_STATSXLINE;AustHex;;
241;RPL_STATSLLINE;RFC1459;<client> L <hostmask> * <servername> <maxdepth>;Reply to STATS (See RFC)
242;RPL_STATSUPTIME;RFC1459;<client> :Server Up <days> days <hours>:<minutes>:<seconds>;Reply to STATS (See RFC)
243;RPL_STATSOLINE;RFC1459;<client> O <hostmask> * <opername> [<privileges>] <class>;"Reply to STATS O (See RFC); The privileges field is an extension in some IRC daemons, which returns either the name of a set of privileges, or a set of privileges. The class extension field returns which connection class the o-line applies to (this is also know to be placeholders like ""0"" and ""-1"" when inapplicable.) ircu doesn't have the privileges field and irc2 uses it to display which port, if any, the oper is restricted to."
244;RPL_STATSHLINE;RFC1459;<client> H <hostmask> * <servername>;Reply to STATS (See RFC)
245;RPL_STATSSLINE;Bahamut, IRCnet, Hybrid;;
245;RPL_STATSTLINE;Hybrid?;;
246;RPL_STATSPING;RFC2812;;
246;RPL_STATSSERVICE;Hybrid;;
246;RPL_STATSTLINE;ircu;;
246;RPL_STATSULINE;Hybrid;;
247;RPL_STATSBLINE;RFC2812;;
247;RPL_STATSXLINE;Hybrid, PTlink, Unreal;;
247;RPL_STATSGLINE;ircu;;
248;RPL_STATSULINE;ircu;;
248;RPL_STATSDEFINE;IRCnet;;
249;RPL_STATSULINE;;;Extension to RFC1459?
249;RPL_STATSDEBUG;Hybrid;;
250;RPL_STATSDLINE;RFC2812;;
250;RPL_STATSCONN;ircu, Unreal;;
251;RPL_LUSERCLIENT;RFC1459;<client> :There are <int> users and <int> invisible on <int> servers;"Reply to LUSERS command, other versions exist (eg. RFC2812); Text may vary."
252;RPL_LUSEROP;RFC1459;<client> <int> :operator(s) online;Reply to LUSERS command - Number of IRC operators online
253;RPL_LUSERUNKNOWN;RFC1459;<client> <int> :unknown connection(s);Reply to LUSERS command - Number of connections in an unknown/unregistered state
254;RPL_LUSERCHANNELS;RFC1459;<client> <int> :channels formed;Reply to LUSERS command - Number of channels formed
255;RPL_LUSERME;RFC1459;<client> :I have <int> clients and <int> servers;"Reply to LUSERS command - Information about local connections; Text may vary."
256;RPL_ADMINME;RFC1459;<client> <server> :Administrative info;Start of an RPL_ADMIN* reply. In practice, the server parameter is often never given, and instead the last parameter contains the text 'Administrative info about <server>'. Newer daemons seem to follow the RFC and output the server's hostname in the last parameter, but also output the server name in the text as per traditional daemons.
257;RPL_ADMINLOC1;RFC1459;<client> :<admin_location>;Reply to ADMIN command (Location, first line)
258;RPL_ADMINLOC2;RFC1459;<client> :<admin_location>;Reply to ADMIN command (Location, second line)
259;RPL_ADMINEMAIL;RFC1459;<client> :<email_address>;Reply to ADMIN command (E-mail address of administrator)
261;RPL_TRACELOG;RFC1459;<client> File <logfile> <debug_level>;See RFC
262;RPL_TRACEPING;;;Extension to RFC1459?
262;RPL_TRACEEND;RFC2812;<client> <server_name> <version>[.<debug_level>] :<info>;Used to terminate a list of RPL_TRACE* replies. Also known as RPL_ENDOFTRACE
263;RPL_TRYAGAIN;RFC2812;<client> <command> :Please wait a while and try again.;When a server drops a command without processing it, it MUST use this reply. The last parameter text changes, and commonly provides the client with more information about why the command could not be processed (such as rate-limiting). Also known as RPL_LOAD_THROTTLED and RPL_LOAD2HI, I'm presuming they do the same thing.
264;RPL_USINGSSL;rusnet-ircd;;
265;RPL_LOCALUSERS;aircd, Hybrid, Bahamut;<client> [<u> <m>] :Current local users <u>, max <m>;Returns the number of clients currently and the maximum number of clients that have been connected directly to this server at one time, respectively. The two optional parameters are not always provided. Also known as RPL_CURRENT_LOCAL
266;RPL_GLOBALUSERS;aircd, Hybrid, Bahamut;<client> [<u> <m>] :Current global users <u>, max <m>;Returns the number of clients currently connected to the network, and the maximum number of clients ever connected to the network at one time, respectively. Also known as RPL_CURRENT_GLOBAL
267;RPL_START_NETSTAT;aircd;;
268;RPL_NETSTAT;aircd;;
269;RPL_END_NETSTAT;aircd;;
270;RPL_PRIVS;ircu;;
270;RPL_MAPUSERS;InspIRCd 2.0;<client> :<count> servers and <count> users, average <average count> users per server;Moved to 018 in InspIRCd 3.0
271;RPL_SILELIST;ircu;;
272;RPL_ENDOFSILELIST;ircu;;
273;RPL_NOTIFY;aircd;;
274;RPL_ENDNOTIFY;aircd;;
274;RPL_STATSDELTA;IRCnet;;
275;RPL_STATSDLINE;ircu, Ultimate;;
275;RPL_USINGSSL;Bahamut;<client> <nick> :is using a secure connection (SSL);
276;RPL_WHOISCERTFP;oftc-hybrid;<client> <nick> :has client certificate fingerprint <fingerprint>;"Shows the SSL/TLS certificate fingerprint used by the client with the given nickname. Only sent when users `""WHOIS""` themselves or when an operator sends the `""WHOIS""`. Also adopted by hybrid 8.1 and charybdis 3.2"
276;RPL_STATSRLINE;ircu;;
276;RPL_VCHANEXIST;Hybrid;;Gone from hybrid 7.1 (2003)
277;RPL_VCHANLIST;Hybrid;;Gone from hybrid 7.1 (2003)
278;RPL_VCHANHELP;Hybrid 7.0?;;Gone from hybrid 7.1 (2003)
280;RPL_GLIST;ircu;;
281;RPL_ENDOFGLIST;ircu;;
281;RPL_ACCEPTLIST;;;
282;RPL_ENDOFACCEPT;;;
282;RPL_JUPELIST;ircu;;
283;RPL_ALIST;;;
283;RPL_ENDOFJUPELIST;ircu;;
284;RPL_ENDOFALIST;;;
284;RPL_FEATURE;ircu;;
285;RPL_GLIST_HASH;;;
285;RPL_CHANINFO_HANDLE;aircd;;
285;RPL_NEWHOSTIS;QuakeNet;;
286;RPL_CHANINFO_USERS;aircd;;
286;RPL_CHKHEAD;QuakeNet;;
287;RPL_CHANINFO_CHOPS;aircd;;
287;RPL_CHANUSER;QuakeNet;;
288;RPL_CHANINFO_VOICES;aircd;;
288;RPL_PATCHHEAD;QuakeNet;;
289;RPL_CHANINFO_AWAY;aircd;;
289;RPL_PATCHCON;QuakeNet;;
290;RPL_CHANINFO_OPERS;aircd;;
290;RPL_HELPHDR;Unreal;;
290;RPL_DATASTR;QuakeNet;;
291;RPL_CHANINFO_BANNED;aircd;;
291;RPL_HELPOP;Unreal;;
291;RPL_ENDOFCHECK;QuakeNet;;
292;RPL_CHANINFO_BANS;aircd;;
292;RPL_HELPTLR;Unreal;;
292;ERR_SEARCHNOMATCH;Nefarious;;
293;RPL_CHANINFO_INVITE;aircd;;
293;RPL_HELPHLP;Unreal;;
294;RPL_CHANINFO_INVITES;aircd;;
294;RPL_HELPFWD;Unreal;;
295;RPL_CHANINFO_KICK;aircd;;
295;RPL_HELPIGN;Unreal;;
296;RPL_CHANINFO_KICKS;aircd;;
299;RPL_END_CHANINFO;aircd;;
300;RPL_NONE;RFC1459;;Dummy reply, supposedly only used for debugging/testing new features, however has appeared in production daemons.
301;RPL_AWAY;RFC1459;<client> <nick> :<message>;Used in reply to a command directed at a user who is marked as away
302;RPL_USERHOST;RFC1459;<client> :*1<reply> *( ' ' <reply> );Reply used by USERHOST (see RFC)
303;RPL_ISON;RFC1459;<client> :*1<nick> *( ' ' <nick> );Reply to the ISON command (see RFC)
304;RPL_TEXT;irc2?;<client> :<text>;Displays text to the user. This seems to have been defined in irc2.7h but never used. Servers generally use specific numerics or server notices instead of this. Unreal uses this numeric, but most others don't use it
305;RPL_UNAWAY;RFC1459;<client> :<info>;Reply from AWAY when no longer marked as away
306;RPL_NOWAWAY;RFC1459;<client> :<info>;Reply from AWAY when marked away
307;RPL_USERIP;;;
307;RPL_WHOISREGNICK;Bahamut, Unreal;;
307;RPL_SUSERHOST;AustHex;;
308;RPL_NOTIFYACTION;aircd;;
308;RPL_WHOISADMIN;Bahamut;;
308;RPL_RULESSTART;Unreal;;Also known as RPL_RULESTART (InspIRCd)
309;RPL_NICKTRACE;aircd;;
309;RPL_WHOISSADMIN;Bahamut;;
309;RPL_ENDOFRULES;Unreal;;Also known as RPL_RULESEND (InspIRCd)
309;RPL_WHOISHELPER;AustHex;;
309;RPL_WHOISSERVICE;oftc-hybrid;;Reply to WHOIS - Client is a Network Service
310;RPL_WHOISSVCMSG;Bahamut;;
310;RPL_WHOISHELPOP;Unreal;;
310;RPL_WHOISSERVICE;AustHex;;
311;RPL_WHOISUSER;RFC1459;<client> <nick> <user> <host> * :<real_name>;Reply to WHOIS - Information about the user
312;RPL_WHOISSERVER;RFC1459;<client> <nick> <server> :<server_info>;Reply to WHOIS - What server they're on
313;RPL_WHOISOPERATOR;RFC1459;<client> <nick> :<privileges>;Reply to WHOIS - User has IRC Operator privileges
314;RPL_WHOWASUSER;RFC1459;<client> <nick> <user> <host> * :<real_name>;Reply to WHOWAS - Information about the user
315;RPL_ENDOFWHO;RFC1459;<client> <name> :<info>;Used to terminate a list of RPL_WHOREPLY replies
316;RPL_WHOISPRIVDEAF;Nefarious;;
316;RPL_WHOISCHANOP;RFC1459;;This numeric was reserved, but never actually used. The source code notes 'redundant and not needed but reserved'
317;RPL_WHOISIDLE;RFC1459;<client> <nick> <seconds> :seconds idle;Reply to WHOIS - Idle information
318;RPL_ENDOFWHOIS;RFC1459;<client> <nick> :<info>;Reply to WHOIS - End of list
319;RPL_WHOISCHANNELS;RFC1459;<client> <nick> :*( ( '@' / '+' ) <channel> ' ' );Reply to WHOIS - Channel list for user (See RFC)
320;RPL_WHOISVIRT;AustHex;;
320;RPL_WHOIS_HIDDEN;Anothernet;;
320;RPL_WHOISSPECIAL;Unreal;;
321;RPL_LISTSTART;RFC1459;<client> Channels :Users Name;Channel list - Header
322;RPL_LIST;RFC1459;<client> <channel> <#_visible> :<topic>;Channel list - A channel
323;RPL_LISTEND;RFC1459;<client> :<info>;Channel list - End of list
324;RPL_CHANNELMODEIS;RFC1459;<client> <channel> <mode> <mode_params>;
325;RPL_UNIQOPIS;RFC2812;<client> <channel> <nickname>;
325;RPL_CHANNELPASSIS;;;
325;RPL_WHOISWEBIRC;Nefarious;;
325;RPL_CHANNELMLOCKIS;sorircd;<client> <nick> <channel> <modeletters> :is the current channel mode-lock;Defined in header file in charybdis, but never used. Also known as RPL_CHANNELMLOCK.
326;RPL_NOCHANPASS;;;
327;RPL_CHPASSUNKNOWN;;;
327;RPL_WHOISHOST;rusnet-ircd;;
328;RPL_CHANNEL_URL;Bahamut, AustHex;;Also known as RPL_CHANNELURL in charybdis
329;RPL_CREATIONTIME;Bahamut;;Also known as RPL_CHANNELCREATED (InspIRCd)
330;RPL_WHOWAS_TIME;;;
330;RPL_WHOISACCOUNT;ircu;<client> <nick> <authname> :<info>;Also known as RPL_WHOISLOGGEDIN (ratbox?, charybdis)
331;RPL_NOTOPIC;RFC1459;<client> <channel> :<info>;Response to TOPIC when no topic is set. Also known as RPL_NOTOPICSET (InspIRCd)
332;RPL_TOPIC;RFC1459;<client> <channel> :<topic>;Response to TOPIC with the set topic. Also known as RPL_TOPICSET (InspIRCd)
333;RPL_TOPICWHOTIME;ircu;;Also known as RPL_TOPICTIME (InspIRCd).
334;RPL_LISTUSAGE;ircu;;
334;RPL_COMMANDSYNTAX;Bahamut;;
334;RPL_LISTSYNTAX;Unreal;;
335;RPL_WHOISBOT;Unreal;;
335;RPL_WHOISTEXT;Hybrid;;Since hybrid 8.2.0
335;RPL_WHOISACCOUNTONLY;Nefarious;;
336;RPL_INVITELIST;Hybrid;<client> :<channel>;"Since hybrid 8.2.0. Not to be confused with the more widely used 346. A ""list of channels a client is invited to"" sent with /INVITE"
336;RPL_WHOISBOT;Nefarious;;
337;RPL_ENDOFINVITELIST;Hybrid;<client> :End of /INVITE list.;Since hybrid 8.2.0. Not to be confused with the more widely used 347.
337;RPL_WHOISTEXT;Hybrid?;;"Before hybrid 8.2.0, for ""User connected using a webirc gateway"". Since charybdis 3.4.0 for ""Underlying IPv4 is %s""."
338;RPL_CHANPASSOK;;;
338;RPL_WHOISACTUALLY;ircu, Bahamut;;
339;RPL_BADCHANPASS;;;
339;RPL_WHOISMARKS;Nefarious;;
340;RPL_USERIP;ircu;;
341;RPL_INVITING;RFC1459;<client> <nick> <channel>;Returned by the server to indicate that the attempted INVITE message was successful and is being passed onto the end client. Note that RFC1459 documents the parameters in the reverse order. The format given here is the format used on production servers, and should be considered the standard reply above that given by RFC1459.
342;RPL_SUMMONING;RFC1459;<client> <user> :<info>;Returned by a server answering a SUMMON message to indicate that it is summoning that user
343;RPL_WHOISKILL;Nefarious;;
344;RPL_WHOISCOUNTRY;InspIRCd 2.0;<client> <nick> :is connected from <country> <country code>;Used by the third-party m_geoipban InspIRCd module.
344;RPL_WHOISCOUNTRY;InspIRCd 3.0;<client> <nick> <country code> :is connecting from <country>;Used by InspIRCd's m_geoban module.
344;RPL_REOPLIST;IRCnet;<client> <channel> <mask>;MODE +R query
345;RPL_INVITED;GameSurge;<client> <channel> <user being invited> <user issuing invite> :<user being invited> has been invited by <user issuing invite>;Sent to users on a channel when an INVITE command has been issued. Also known as RPL_ISSUEDINVITE (ircu)
345;RPL_ENDOFREOPLIST;IRCnet;<client> <channel>: :End of Channel Reop List;MODE +R query
346;RPL_INVITELIST;RFC2812;<client> <channel> <invitemask>;An invite mask for the invite mask list. Also known as RPL_INVEXLIST in hybrid 8.2.0
347;RPL_ENDOFINVITELIST;RFC2812;<client> <channel> :<info>;Termination of an RPL_INVITELIST list. Also known as RPL_ENDOFINVEXLIST in hybrid 8.2.0
348;RPL_EXCEPTLIST;RFC2812;<client> <channel> <exceptionmask> [<who> <set-ts>];An exception mask for the exception mask list. Also known as RPL_EXLIST (Unreal, Ultimate). Bahamut calls this RPL_EXEMPTLIST and adds the last two optional params, <who> being either the nickmask of the client that set the exception or the server name, and <set-ts> being a unix timestamp representing when it was set.
349;RPL_ENDOFEXCEPTLIST;RFC2812;<client> <channel> :<info>;Termination of an RPL_EXCEPTLIST list. Also known as RPL_ENDOFEXLIST (Unreal, Ultimate) or RPL_ENDOFEXEMPTLIST (Bahamut).
350;RPL_WHOISGATEWAY;InspIRCd 3.0;<client> <real host> <real ip> :is connecting via {the <name> WebIRC, an ident} gateway;Used by InspIRCd's m_cgiirc module.
351;RPL_VERSION;RFC1459;<client> <version> <server> :<comments>;Reply by the server showing its version details, however this format is not often adhered to
352;RPL_WHOREPLY;RFC1459;<client> <channel> <user> <host> <server> <nick> <H|G>[*][@|+] :<hopcount> <real_name>;Reply to vanilla WHO (See RFC). This format can be very different if the 'WHOX' version of the command is used (see ircu).
353;RPL_NAMREPLY;RFC1459;<client> ( '=' / '*' / '@' ) <channel> ' ' : [ '@' / '+' ] <nick> *( ' ' [ '@' / '+' ] <nick> );Reply to NAMES (See RFC)
354;RPL_WHOSPCRPL;ircu;;Reply to WHO, however it is a 'special' reply because it is returned using a non-standard (non-RFC1459) format. The format is dictated by the command given by the user, and can vary widely. When this is used, the WHO command was invoked in its 'extended' form, as announced by the 'WHOX' ISUPPORT tag. Also known as RPL_RWHOREPLY (Bahamut).
355;RPL_NAMREPLY_;QuakeNet;<client> ( '=' / '*' / '@' ) <channel> ' ' : [ '@' / '+' ] <nick> *( ' ' [ '@' / '+' ] <nick> );Reply to the \users (when the channel is set +D, QuakeNet relative). The proper define name for this numeric is unknown at this time. Also known as RPL_DELNAMREPLY (ircu) Also see RPL_NAMREPLY (353).
357;RPL_MAP;AustHex;;
358;RPL_MAPMORE;AustHex;;
359;RPL_MAPEND;AustHex;;
360;RPL_WHOWASREAL;Charybdis;;"Defined in header file, but never used. Initially introduced in charybdis 2.1 behind `""#if 0""`, with the other side using RPL_WHOISACTUALLY"
361;RPL_KILLDONE;RFC1459;;
362;RPL_CLOSING;RFC1459;;
363;RPL_CLOSEEND;RFC1459;;
364;RPL_LINKS;RFC1459;<client> <mask> <server> :<hopcount> <server_info>;Reply to the LINKS command
365;RPL_ENDOFLINKS;RFC1459;<client> <mask> :<info>;Termination of an RPL_LINKS list
366;RPL_ENDOFNAMES;RFC1459;<client> <channel> :<info>;Termination of an RPL_NAMREPLY list
367;RPL_BANLIST;RFC1459;<client> <channel> <banid> [[<setter> <time_left>|<time_left> :<reason>]];"A ban-list item (See RFC); <setter>, <time left> and <reason> are additions used by various servers."
368;RPL_ENDOFBANLIST;RFC1459;<client> <channel> :<info>;Termination of an RPL_BANLIST list
369;RPL_ENDOFWHOWAS;RFC1459;<client> <nick> :<info>;Reply to WHOWAS - End of list
371;RPL_INFO;RFC1459;<client> :<string>;Reply to INFO
372;RPL_MOTD;RFC1459;<client> :- <string>;Reply to MOTD
373;RPL_INFOSTART;RFC1459;;
374;RPL_ENDOFINFO;RFC1459;<client> :<info>;Termination of an RPL_INFO list
375;RPL_MOTDSTART;RFC1459;<client> :- <server> Message of the day -;Start of an RPL_MOTD list
376;RPL_ENDOFMOTD;RFC1459;<client> :<info>;Termination of an RPL_MOTD list
377;RPL_KICKEXPIRED;aircd;;
377;RPL_SPAM;AustHex;<client> :<text>;Used during the connection (after MOTD) to announce the network policy on spam and privacy. Supposedly now obsoleted in favor of using NOTICE.
378;RPL_BANEXPIRED;aircd;;
378;RPL_WHOISHOST;Unreal;;
378;RPL_MOTD;AustHex;;Used by AustHex to 'force' the display of the MOTD, however is considered obsolete due to client/script awareness & ability to display the MOTD regardless. Also see RPL_MOTD (372).
379;RPL_KICKLINKED;aircd;;
379;RPL_WHOISMODES;Unreal;;
379;RPL_WHOWASIP;InspIRCd 2.0;<client> <nick> :was connecting from <host>;Moved to 652 in InspIRCd 3.0
380;RPL_BANLINKED;aircd;;
380;RPL_YOURHELPER;AustHex;;
381;RPL_YOUREOPER;RFC1459;<client> :<info>;Successful reply from OPER. Also known as RPL_YOUAREOPER (InspIRCd)
382;RPL_REHASHING;RFC1459;<client> <config_file> :<info>;Successful reply from REHASH
383;RPL_YOURESERVICE;RFC2812;<client> :You are service <service_name>;Sent upon successful registration of a service
384;RPL_MYPORTIS;RFC1459;;
385;RPL_NOTOPERANYMORE;AustHex, Hybrid, Unreal;;
386;RPL_QLIST;Unreal;;
386;RPL_IRCOPS;Ultimate;;
386;RPL_IRCOPSHEADER;Nefarious;;
386;RPL_RSACHALLENGE;Hybrid;:*;Used by Hybrid's old OpenSSL OPER CHALLENGE response. This has been obsoleted in favor of SSL cert fingerprinting in oper blocks
387;RPL_ENDOFQLIST;Unreal;;
387;RPL_ENDOFIRCOPS;Ultimate;;
387;RPL_IRCOPS;Nefarious;;
388;RPL_ALIST;Unreal;;
388;RPL_ENDOFIRCOPS;Nefarious;;
389;RPL_ENDOFALIST;Unreal;;
391;RPL_TIME;RFC1459;<client> <server> :<time string>;Response to the TIME command. The string format may vary greatly.
391;RPL_TIME;ircu;<client> <server> <timestamp> <offset> :<time string>;This extension adds the timestamp and timestamp-offset information for clients.
391;RPL_TIME;bdq-ircd;<client> <server> <timezone name> <microseconds> :<time string>;Timezone name is acronym style (eg. 'EST', 'PST' etc). The microseconds field is the number of microseconds since the UNIX epoch, however it is relative to the local timezone of the server. The timezone field is ambiguous, since it only appears to include American zones.
391;RPL_TIME;;<client> <server> <year> <month> <day> <hour> <minute> <second>;Yet another variation, including the time broken down into its components. Time is supposedly relative to UTC.
392;RPL_USERSSTART;RFC1459;<client> :UserID Terminal Host;Start of an RPL_USERS list
393;RPL_USERS;RFC1459;<client> :<username> <ttyline> <hostname>;Response to the USERS command (See RFC)
394;RPL_ENDOFUSERS;RFC1459;<client> :<info>;Termination of an RPL_USERS list
395;RPL_NOUSERS;RFC1459;<client> :<info>;Reply to USERS when nobody is logged in
396;RPL_VISIBLEHOST;Hybrid;<client> <hostname> :is now your visible host;Also known as RPL_YOURDISPLAYEDHOST (InspIRCd) or RPL_HOSTHIDDEN (ircu, charybdis, Quakenet, Unreal). <hostname> can also be in the form <user@hostname> (Quakenet).
399;RPL_CLONES;InspIRCd 3.0;<client> <local-count> <global-count> <cidr-range>;
400;ERR_UNKNOWNERROR;;<client> <command> [<?>] :<info>;Sent when an error occurred executing a command, but it is not specifically known why the command could not be executed.
401;ERR_NOSUCHNICK;RFC1459;<client> <nick> :<reason>;Used to indicate the nickname parameter supplied to a command is currently unused
402;ERR_NOSUCHSERVER;RFC1459;<client> <server> :<reason>;Used to indicate the server name given currently doesn't exist
403;ERR_NOSUCHCHANNEL;RFC1459;<client> <channel> :<reason>;Used to indicate the given channel name is invalid, or does not exist
404;ERR_CANNOTSENDTOCHAN;RFC1459;<client> <channel> :<reason>;Sent to a user who does not have the rights to send a message to a channel
405;ERR_TOOMANYCHANNELS;RFC1459;<client> <channel> :<reason>;Sent to a user when they have joined the maximum number of allowed channels and they tried to join another channel
406;ERR_WASNOSUCHNICK;RFC1459;<client> <nick> :<reason>;Returned by WHOWAS to indicate there was no history information for a given nickname
407;ERR_TOOMANYTARGETS;RFC1459;<client> <target> :<reason>;The given target(s) for a command are ambiguous in that they relate to too many targets
408;ERR_NOSUCHSERVICE;RFC2812;<client> <service_name> :<reason>;Returned to a client which is attempting to send an SQUERY (or other message) to a service which does not exist
408;ERR_NOCOLORSONCHAN;Bahamut;;
408;ERR_NOCTRLSONCHAN;Hybrid;<client> <channel> :You cannot use control codes on this channel. Not sent: <text>;
409;ERR_NOORIGIN;RFC1459;<client> :<reason>;PING or PONG message missing the originator parameter which is required since these commands must work without valid prefixes
410;ERR_INVALIDCAPCMD;Undernet?;<client> <badcmd> :Invalid CAP subcommand;Returned when a client sends a CAP subcommand which is invalid or otherwise issues an invalid CAP command. Also known as ERR_INVALIDCAPSUBCOMMAND (InspIRCd) or ERR_UNKNOWNCAPCMD (ircu)
411;ERR_NORECIPIENT;RFC1459;<client> :<reason>;Returned when no recipient is given with a command
412;ERR_NOTEXTTOSEND;RFC1459;<client> :<reason>;Returned when NOTICE/PRIVMSG is used with no message given
413;ERR_NOTOPLEVEL;RFC1459;<client> <mask> :<reason>;Used when a message is being sent to a mask without being limited to a top-level domain (i.e. * instead of *.au)
414;ERR_WILDTOPLEVEL;RFC1459;<client> <mask> :<reason>;Used when a message is being sent to a mask with a wild-card for a top level domain (i.e. *.*)
415;ERR_BADMASK;RFC2812;<client> <mask> :<reason>;Used when a message is being sent to a mask with an invalid syntax
416;ERR_TOOMANYMATCHES;IRCnet;<client> <command> [<mask>] :<info>;Returned when too many matches have been found for a command and the output has been truncated. An example would be the WHO command, where by the mask '*' would match everyone on the network! Ouch!
416;ERR_QUERYTOOLONG;ircu;;Same as ERR_TOOMANYMATCHES
417;ERR_INPUTTOOLONG;ircu;;Returned when an input line is longer than the server can process (512 bytes), to let the client know this line was dropped (rather than being truncated)
419;ERR_LENGTHTRUNCATED;aircd;;
420;ERR_AMBIGUOUSCOMMAND;InspIRCd;<client> :Ambiguous abbreviation;Used by InspIRCd's m_abbreviation module
421;ERR_UNKNOWNCOMMAND;RFC1459;<client> <command> :<reason>;Returned when the given command is unknown to the server (or hidden because of lack of access rights)
422;ERR_NOMOTD;RFC1459;<client> :<reason>;Sent when there is no MOTD to send the client
423;ERR_NOADMININFO;RFC1459;<client> <server> :<reason>;Returned by a server in response to an ADMIN request when no information is available. RFC1459 mentions this in the list of numerics. While it's not listed as a valid reply in section 4.3.7 ('Admin command'), it's confirmed to exist in the real world.
424;ERR_FILEERROR;RFC1459;<client> :<reason>;Generic error message used to report a failed file operation during the processing of a command
425;ERR_NOOPERMOTD;Unreal;;
429;ERR_TOOMANYAWAY;Bahamut;;
430;ERR_EVENTNICKCHANGE;AustHex;;Returned by NICK when the user is not allowed to change their nickname due to a channel event (channel mode +E)
431;ERR_NONICKNAMEGIVEN;RFC1459;<client> :<reason>;Returned when a nickname parameter expected for a command isn't found
432;ERR_ERRONEUSNICKNAME;RFC1459;<client> <nick> :<reason>;Returned after receiving a NICK message which contains a nickname which is considered invalid, such as it's reserved ('anonymous') or contains characters considered invalid for nicknames. This numeric is misspelt, but remains with this name for historical reasons :)
433;ERR_NICKNAMEINUSE;RFC1459;<client> <nick> :<reason>;Returned by the NICK command when the given nickname is already in use
434;ERR_SERVICENAMEINUSE;AustHex?;;
434;ERR_NORULES;Unreal, Ultimate;;
435;ERR_SERVICECONFUSED;Unreal;;
435;ERR_BANONCHAN;Bahamut;;Also known as ERR_BANNICKCHANGE (ratbox, charybdis)
436;ERR_NICKCOLLISION;RFC1459;<nick> :<reason>;Returned by a server to a client when it detects a nickname collision
437;ERR_UNAVAILRESOURCE;RFC2812;<client> <nick/channel/service> :<reason>;Return when the target is unable to be reached temporarily, eg. a delay mechanism in play, or a service being offline
437;ERR_BANNICKCHANGE;ircu;;
438;ERR_NICKTOOFAST;ircu;;Also known as ERR_NCHANGETOOFAST (Unreal, Ultimate)
438;ERR_DEAD;IRCnet;;
439;ERR_TARGETTOOFAST;ircu;;Also known as many other things, RPL_INVTOOFAST, RPL_MSGTOOFAST, ERR_TARGETTOFAST (Bahamut), etc
440;ERR_SERVICESDOWN;Bahamut, Unreal;;
441;ERR_USERNOTINCHANNEL;RFC1459;<client> <nick> <channel> :<reason>;Returned by the server to indicate that the target user of the command is not on the given channel
442;ERR_NOTONCHANNEL;RFC1459;<client> <channel> :<reason>;Returned by the server whenever a client tries to perform a channel effecting command for which the client is not a member
443;ERR_USERONCHANNEL;RFC1459;<client> <nick> <channel> [:<reason>];Returned when a client tries to invite a user to a channel they're already on
444;ERR_NOLOGIN;RFC1459;<client> <user> :<reason>;Returned by the SUMMON command if a given user was not logged in and could not be summoned
445;ERR_SUMMONDISABLED;RFC1459;<client> :<reason>;Returned by SUMMON when it has been disabled or not implemented
446;ERR_USERSDISABLED;RFC1459;<client> :<reason>;Returned by USERS when it has been disabled or not implemented
447;ERR_NONICKCHANGE;Unreal;;This numeric is called ERR_CANTCHANGENICK in InspIRCd
448;ERR_FORBIDDENCHANNEL;Unreal;<channel> :Channel is forbidden: <reason>;Returned when this channel name has been explicitly blocked and is not allowed to be used.
449;ERR_NOTIMPLEMENTED;Undernet;Unspecified;Returned when a requested feature is not implemented (and cannot be completed)
451;ERR_NOTREGISTERED;RFC1459;<client> :<reason>;Returned by the server to indicate that the client must be registered before the server will allow it to be parsed in detail
452;ERR_IDCOLLISION;;;
453;ERR_NICKLOST;;;
455;ERR_HOSTILENAME;Unreal;;
456;ERR_ACCEPTFULL;;;
457;ERR_ACCEPTEXIST;;;
458;ERR_ACCEPTNOT;;;
459;ERR_NOHIDING;Unreal;;Not allowed to become an invisible operator?
460;ERR_NOTFORHALFOPS;Unreal;;
461;ERR_NEEDMOREPARAMS;RFC1459;<client> <command> :<reason>;Returned by the server by any command which requires more parameters than the number of parameters given
462;ERR_ALREADYREGISTERED;RFC1459;<client> :<reason>;Returned by the server to any link which attempts to register again Also known as ERR_ALREADYREGISTRED (sic) in ratbox/charybdis.
463;ERR_NOPERMFORHOST;RFC1459;<client> :<reason>;Returned to a client, which attempts to register with a server which has been configured to refuse connections from the client's host
464;ERR_PASSWDMISMATCH;RFC1459;<client> :<reason>;Returned by the PASS command to indicate the given password was required and was either not given or was incorrect
465;ERR_YOUREBANNEDCREEP;RFC1459;<client> :<reason>;Returned to a client after an attempt to register on a server configured to ban connections from that client
466;ERR_YOUWILLBEBANNED;RFC1459;;Sent by a server to a user to inform that access to the server will soon be denied
467;ERR_KEYSET;RFC1459;<client> <channel> :<reason>;Returned when the channel key for a channel has already been set
468;ERR_INVALIDUSERNAME;ircu;;
468;ERR_ONLYSERVERSCANCHANGE;Bahamut, Unreal;;
468;ERR_NOCODEPAGE;rusnet-ircd;;
469;ERR_LINKSET;Unreal;;
470;ERR_LINKCHANNEL;Unreal;;Sent by a server to a user who tried to JOIN a channel, when they are forwarded to a different channel because they could not join the original one. The target channel is usually configured with +f (eg. Charybdis) or +L (eg. Unreal)
470;ERR_KICKEDFROMCHAN;aircd;;
470;ERR_7BIT;rusnet-ircd;;
471;ERR_CHANNELISFULL;RFC1459;<client> <channel> :<reason>;Returned when attempting to join a channel which is set +l and is already full
472;ERR_UNKNOWNMODE;RFC1459;<client> <char> :<reason>;Returned when a given mode is unknown
473;ERR_INVITEONLYCHAN;RFC1459;<client> <channel> :<reason>;Returned when attempting to join a channel, which is invite only without an invitation
474;ERR_BANNEDFROMCHAN;RFC1459;<client> <channel> :<reason>;Returned when attempting to join a channel a user is banned from
475;ERR_BADCHANNELKEY;RFC1459;<client> <channel> :<reason>;Returned when attempting to join a key-locked channel either without a key or with the wrong key
476;ERR_BADCHANMASK;RFC2812;<client> <channel> :<reason>;The given channel mask was invalid
477;ERR_NOCHANMODES;RFC2812;<client> <channel> :<reason>;Returned when attempting to set a mode on a channel, which does not support channel modes, or channel mode changes. Also known as ERR_MODELESS
477;ERR_NEEDREGGEDNICK;Bahamut, ircu, Unreal;;
478;ERR_BANLISTFULL;RFC2812;<client> <channel> [char] :<reason>;Returned when a channel access list (i.e. ban list etc) is full and cannot be added to
479;ERR_BADCHANNAME;Hybrid;<client> <channel> :<reason>;Returned to indicate that a given channel name is not valid. Servers that implement this use it instead of `ERR_NOSUCHCHANNEL` where appropriate. Also see ERR_NOSUCHCHANNEL (403).
479;ERR_LINKFAIL;Unreal;;
479;ERR_NOCOLOR;rusnet-ircd;;
480;ERR_NOULINE;AustHex;;
480;ERR_CANNOTKNOCK;Unreal;;
480;ERR_THROTTLE;Ratbox;<nick> <channel> :Cannot join channel;
480;ERR_SSLONLYCHAN;Hybrid;;Moved to 489 to match other servers. Also see ERR_SECUREONLYCHAN (489).
480;ERR_NOWALLOP;rusnet-ircd;;
481;ERR_NOPRIVILEGES;RFC1459;<client> :<reason>;Returned by any command requiring special privileges (eg. IRC operator) to indicate the operation was unsuccessful
482;ERR_CHANOPRIVSNEEDED;RFC1459;<client> <channel> :<reason>;Returned by any command requiring special channel privileges (eg. channel operator) to indicate the operation was unsuccessful. InspIRCd also uses this numeric 'for other things like trying to kick a uline'
483;ERR_CANTKILLSERVER;RFC1459;< client > :< reason >;Returned by KILL to anyone who tries to kill a server
484;ERR_RESTRICTED;RFC2812;< client > :< reason >;Sent by the server to a user upon connection to indicate the restricted nature of the connection(i.e.usermode + r)
484;ERR_ISCHANSERVICE;Undernet;;
484;ERR_DESYNC;Bahamut, Hybrid, PTlink;;
484;ERR_ATTACKDENY;Unreal;;
485;ERR_UNIQOPRIVSNEEDED;RFC2812;< client > :< reason >;Any mode requiring 'channel creator' privileges returns this error if the client is attempting to use it while not a channel creator on the given channel
485;ERR_KILLDENY;Unreal;;
485;ERR_CANTKICKADMIN;PTlink;;
485;ERR_ISREALSERVICE;QuakeNet;;
485;ERR_CHANBANREASON;Hybrid;< client > < channel > :Cannot join channel(< reason >);
485;ERR_BANNEDNICK;Ratbox;;Defined in header file, but never used.
486;ERR_NONONREG;Unreal ?;;Also known as ERR_ACCOUNTONLY.
486;ERR_RLINED;rusnet - ircd;;
486;ERR_HTMDISABLED;Unreal;;Unreal 3.2 uses 488 as the ERR_HTMDISABLED numeric instead
487;ERR_CHANTOORECENT;IRCnet;;
487;ERR_MSGSERVICES;Bahamut;;
487;ERR_NOTFORUSERS;Unreal ?;;
487;ERR_NONONSSL;ChatIRCd;< target user > :You must be connected using SSL/ TLS to message this user;Used for user mode +t(caller ID for all users not using SSL/ TLS).
488;ERR_TSLESSCHAN;IRCnet;;
488;ERR_HTMDISABLED;Unreal ?;;
488;ERR_NOSSL;Bahamut;<client> <channel> :SSL Only channel (+S), You must connect using SSL to join this channel.;
489;ERR_SECUREONLYCHAN;Unreal;;Also known as ERR_SSLONLYCHAN.
489;ERR_VOICENEEDED;Undernet;;
490;ERR_ALLMUSTSSL;InspIRCd;< client > < channel > :all members of the channel must be connected via SSL;
490;ERR_NOSWEAR;Unreal;< client > :< nick > does not accept private messages containing swearing.;
491;ERR_NOOPERHOST;RFC1459;<client> :<reason>;Returned by OPER to a client who cannot become an IRC operator because the server has been configured to disallow the client's host
492;ERR_NOSERVICEHOST;RFC1459;;
492;ERR_NOCTCP;Hybrid / Unreal?;<client> :You cannot send CTCPs to this channel. Not sent: <message>;Notifies the user that a message they have sent to a channel has been rejected as it contains CTCPs, and they cannot send messages containing CTCPs to this channel. Also known as ERR_NOCTCPALLOWED (InspIRCd).
492;ERR_CANNOTSENDTOUSER;Charybdis?;<client> :Cannot send to user <nick> (<reason>);
493;ERR_NOSHAREDCHAN;Bahamut;<client> :You cannot message that person because you do not share a common channel with them.;
493;ERR_NOFEATURE;ircu;;
494;ERR_BADFEATVALUE;ircu;;
494;ERR_OWNMODE;Bahamut, charybdis?;<client> <nick> :cannot answer you while you are <mode>, your message was not sent;Used for mode +g (CALLERID) in charybdis.
495;ERR_BADLOGTYPE;ircu;;
495;ERR_DELAYREJOIN;InspIRCd 2.0;<channel> :You cannot rejoin this channel yet after being kicked (+J);"This numeric is marked as ""we should use 'resource temporarily unavailable' from ircnet/ratbox or whatever"". Removed in InspIRCd 3.0."
496;ERR_BADLOGSYS;ircu;;
497;ERR_BADLOGVALUE;ircu;;
498;ERR_ISOPERLCHAN;ircu;;
499;ERR_CHANOWNPRIVNEEDED;Unreal;;Works just like ERR_CHANOPRIVSNEEDED except it indicates that owner status (+q) is needed. Also see ERR_CHANOPRIVSNEEDED (482).
500;ERR_TOOMANYJOINS;Unreal?;<client> <string> :Too many join requests. Please wait a while and try again.;
500;ERR_NOREHASHPARAM;rusnet-ircd;;
500;ERR_CANNOTSETMODER;InspIRCd;<client> :Only a server may modify the +r user/channel mode;Returned by the server when a client tries to set MODE +r on a user or channel. This mode is set by services for registered users/channels.
501;ERR_UMODEUNKNOWNFLAG;RFC1459;<client> :<reason>;Returned by the server to indicate that a MODE message was sent with a nickname parameter and that the mode flag sent was not recognised.
501;ERR_UNKNOWNSNOMASK;InspIRCd;<client> <snomask> :is unknown mode char to me;
502;ERR_USERSDONTMATCH;RFC1459;<client> :<reason>;Error sent to any user trying to view or change the user mode for a user other than themselves
503;ERR_GHOSTEDCLIENT;Hybrid;<client> :Message could not be delivered to <target>;
503;ERR_VWORLDWARN;AustHex;<client> :<warning_text>;Warning about Virtual-World being turned off. Obsoleted in favor for RPL_MODECHANGEWARN Also see RPL_MODECHANGEWARN (662).
504;ERR_USERNOTONSERV;;;
511;ERR_SILELISTFULL;ircu;;
512;ERR_TOOMANYWATCH;Bahamut;;Also known as ERR_NOTIFYFULL (aircd), I presume they are the same
512;ERR_NOSUCHGLINE;ircu;;
513;ERR_BADPING;ircu;;Also known as ERR_NEEDPONG (Unreal/Ultimate) for use during registration, however it is not used in Unreal (and might not be used in Ultimate either). Also known as ERR_WRONGPONG (Ratbox/charybdis)
514;ERR_TOOMANYDCC;Bahamut;;
514;ERR_NOSUCHJUPE;irch;;
514;ERR_INVALID_ERROR;ircu;;
515;ERR_BADEXPIRE;ircu;;
516;ERR_DONTCHEAT;ircu;;
517;ERR_DISABLED;ircu;<client> <command> :<info/reason>;
518;ERR_NOINVITE;Unreal;;
518;ERR_LONGMASK;ircu;;
519;ERR_ADMONLY;Unreal;;
519;ERR_TOOMANYUSERS;ircu;;
520;ERR_OPERONLY;Unreal;:Cannot join channel (+O);Also known as ERR_OPERONLYCHAN (Hybrid) and ERR_CANTJOINOPERSONLY (InspIRCd).
520;ERR_MASKTOOWIDE;ircu;;
520;ERR_WHOTRUNC;AustHex;;This is considered obsolete in favor of ERR_TOOMANYMATCHES, and should no longer be used. Also see ERR_TOOMANYMATCHES (416).
521;ERR_LISTSYNTAX;Bahamut;;
521;ERR_NOSUCHGLINE;Nefarious;;
522;ERR_WHOSYNTAX;Bahamut;;
523;ERR_WHOLIMEXCEED;Bahamut;<limit> :<command> search limit exceeded.;
524;ERR_QUARANTINED;ircu;;
524;ERR_OPERSPVERIFY;Unreal;;
524;ERR_HELPNOTFOUND;Hybrid;<term> :Help not found;
525;ERR_INVALIDKEY;ircu;;
525;ERR_REMOTEPFX;CAPAB USERCMDPFX;<nickname> :<reason>;Proposed.
526;ERR_PFXUNROUTABLE;CAPAB USERCMDPFX;<nickname> :<reason>;Proposed.
531;ERR_CANTSENDTOUSER;InspIRCd;<client> <nick> :You are not permitted to send private messages to this user;
550;ERR_BADHOSTMASK;QuakeNet;;
551;ERR_HOSTUNAVAIL;QuakeNet;;
552;ERR_USINGSLINE;QuakeNet;;
553;ERR_STATSSLINE;QuakeNet;;
560;ERR_NOTLOWEROPLEVEL;ircu;;
561;ERR_NOTMANAGER;ircu;;
562;ERR_CHANSECURED;ircu;;
563;ERR_UPASSSET;ircu;;
564;ERR_UPASSNOTSET;ircu;;
566;ERR_NOMANAGER;ircu;;
567;ERR_UPASS_SAME_APASS;ircu;;
568;ERR_LASTERROR;ircu;;
568;RPL_NOOMOTD;Nefarious;;
597;RPL_REAWAY;Unreal;;
598;RPL_GONEAWAY;Unreal;;"Used when adding users to their `""WATCH""` list."
599;RPL_NOTAWAY;Unreal;;"Used when adding users to their `""WATCH""` list."
600;RPL_LOGON;Bahamut, Unreal;;
601;RPL_LOGOFF;Bahamut, Unreal;;
602;RPL_WATCHOFF;Bahamut, Unreal;;
603;RPL_WATCHSTAT;Bahamut, Unreal;;
604;RPL_NOWON;Bahamut, Unreal;;
605;RPL_NOWOFF;Bahamut, Unreal;;
606;RPL_WATCHLIST;Bahamut, Unreal;;
607;RPL_ENDOFWATCHLIST;Bahamut, Unreal;;
608;RPL_WATCHCLEAR;Ultimate;;Also known as RPL_CLEARWATCH in Unreal
609;RPL_NOWISAWAY;Unreal;;"Returned when adding users to their `""WATCH""` list."
610;RPL_MAPMORE;Unreal;;
610;RPL_ISOPER;Ultimate;;
611;RPL_ISLOCOP;Ultimate;;
612;RPL_ISNOTOPER;Ultimate;;
613;RPL_ENDOFISOPER;Ultimate;;
615;RPL_MAPMORE;PTlink;;
615;RPL_WHOISMODES;Ultimate;;
616;RPL_WHOISHOST;Ultimate;;
617;RPL_WHOISSSLFP;Nefarious;<client> <nick> :has client certificate fingerprint <fingerprint>;Also see RPL_WHOISCERTFP (276).
617;RPL_DCCSTATUS;Bahamut;;
617;RPL_WHOISBOT;Ultimate;;
618;RPL_DCCLIST;Bahamut;;
619;RPL_ENDOFDCCLIST;Bahamut;;
619;RPL_WHOWASHOST;Ultimate;;
620;RPL_DCCINFO;Bahamut;;
620;RPL_RULESSTART;Ultimate;;
621;RPL_RULES;Ultimate;;
622;RPL_ENDOFRULES;Ultimate;;
623;RPL_MAPMORE;Ultimate;;
624;RPL_OMOTDSTART;Ultimate;;
625;RPL_OMOTD;Ultimate;;
626;RPL_ENDOFOMOTD;Ultimate;;
630;RPL_SETTINGS;Ultimate;;
631;RPL_ENDOFSETTINGS;Ultimate;;
640;RPL_DUMPING;Unreal;;Never actually used by Unreal - was defined however the feature that would have used this numeric was never created.
641;RPL_DUMPRPL;Unreal;;Never actually used by Unreal - was defined however the feature that would have used this numeric was never created.
642;RPL_EODUMP;Unreal;;Never actually used by Unreal - was defined however the feature that would have used this numeric was never created.
650;RPL_SYNTAX;InspIRCd 3.0;<client> <command> :<syntax>;Sent when the user does not provide enough parameters for a command.
651;RPL_CHANNELSMSG;InspIRCd 3.0;<client> <nick> :is on private/ secret channels:;
652;RPL_WHOWASIP;InspIRCd 3.0;< client > < nick > :was connecting from<host>;
653;RPL_UNINVITED;InspIRCd 3.0;< client > :You were uninvited from<chan> by<nick>;
659;RPL_SPAMCMDFWD;Unreal;<client> <command> :Command processed, but a copy has been sent to ircops for evaluation (anti-spam) purposes. [<reason>];Used to let a client know that a copy of their command has been passed to operators and the reason for it.
670;RPL_STARTTLS;IRCv3;<client> :STARTTLS successful, proceed with TLS handshake;Indicates that the client may begin the TLS handshake
671;RPL_WHOISSECURE;Unreal;<client> <nick> :is using a secure connection;The text in the last parameter may change.Also known as RPL_WHOISSSL (Nefarious).
672;RPL_UNKNOWNMODES;Ithildin;< modes > :< info >;Returns a full list of modes that are unknown when a client issues a MODE command(rather than one numeric per mode)
672;RPL_WHOISREALIP;Rizon;< client > < nick > :is actually from < ip >;Returns the real IP address of a client connected from a CGIIRC host, this has the real IP address of the client. This message is only sent to themselves or to IRC operators who perform a WHOIS on the user.
673;RPL_CANNOTSETMODES;Ithildin;< modes > :< info >;Returns a full list of modes that cannot be set when a client issues a MODE command
674;RPL_WHOISYOURID;ChatIRCd;< client > :EUID is < euid >;Used to display the user's TS6 UID in WHOIS.
690;ERR_REDIRECT;InspIRCd;< client > :Target channel #chan must exist to be set as a redirect;Indicates an error when setting a channel redirect (MODE +L) or using the banredirect module
691;ERR_STARTTLS;IRCv3;< client > :STARTTLS failed(Wrong moon phase);Indicates that a server-side error has occurred
696;ERR_INVALIDMODEPARAM;InspIRCd 3.0;< client > < target chan / user > < mode char> < parameter > :< description >;Indicates that there was a problem with a mode parameter.Replaces various non-standard mode specific numerics.
697;ERR_LISTMODEALREADYSET;InspIRCd 3.0;<client> <target chan> <parameter> <mode char> :<description>;Indicates that the user tried to set a list mode which is already set. Replaces various non-standard mode specific numerics.
698;ERR_LISTMODENOTSET;InspIRCd 3.0;<client> <target chan> <parameter> <mode char> :<description>;Indicates that the user tried to unset a list mode which is not set. Replaces various non-standard mode specific numerics.
700;RPL_COMMANDS;InspIRCd 3.0;<client> :<command> <module name> <minimum parameters> <penalty>;
701;RPL_COMMANDSEND;InspIRCd 3.0;<client> :End of COMMANDS list;
702;RPL_MODLIST;RatBox;<?> 0x<?> <?> <?>;Output from the MODLIST command
702;RPL_COMMANDS;InspIRCd 2.0;<client> :<command> <module name> <minimum parameters>;Moved to 700 in InspIRCd 3.0
703;RPL_ENDOFMODLIST;RatBox;<client> :<text>;Terminates MODLIST output
703;RPL_COMMANDSEND;InspIRCd 2.0;<client> :End of COMMANDS list;Moved to 701 in InspIRCd 3.0
704;RPL_HELPSTART;RatBox;<client> <command> :<text>;Start of HELP command output
705;RPL_HELPTXT;RatBox;<client> <command> :<text>;Output from HELP command
706;RPL_ENDOFHELP;RatBox;<client> <command> :<text>;End of HELP command output
707;ERR_TARGCHANGE;RatBox;<client> <target> :Targets changing too fast, message dropped;See doc/tgchange.txt in the charybdis source.
708;RPL_ETRACEFULL;RatBox;<client> <Oper|User> <class> < nick > < username > < host > < ip > :< capabilities >;Output from 'extended' trace
709;RPL_ETRACE;RatBox;< client > < Oper | User > <?> < nick > < username > < host > :< ip >;Output from 'extended' trace
710;RPL_KNOCK;RatBox;< client > < channel > < nick > !< user >@< host > :< text >;Message delivered using KNOCK command
711;RPL_KNOCKDLVR;RatBox;< client > < channel > :< text >;Message returned from using KNOCK command(KNOCK delivered)
712;ERR_TOOMANYKNOCK;RatBox;< client > < channel > :< text >;Message returned when too many KNOCKs for a channel have been sent by a user
713;ERR_CHANOPEN;RatBox;< client > < channel > :< text >;Message returned from KNOCK when the channel can be freely joined by the user
714;ERR_KNOCKONCHAN;RatBox;< client > < channel > :< text >;Message returned from KNOCK when the user has used KNOCK on a channel they have already joined
715;ERR_KNOCKDISABLED;RatBox;< client > :< text >;Returned from KNOCK when the command has been disabled
715;ERR_TOOMANYINVITE;Hybrid;< client > < channel > :Too many INVITEs(< channel / user >).;"""Sent to indicate an INVITE has been blocked. The last parameter is the literal string """"channel"""" if this is because the channel has had too many INVITEs in a given time"
715;RPL_INVITETHROTTLE;Rizon;"""<client> <nick> <channel> :You are inviting too fast";"invite to <nick> for <channel> not sent."""
716;RPL_TARGUMODEG;RatBox;< nick > :< info >;"""Sent to indicate the given target is set +g (server-side ignore) Mentioned as RPL_TARGUMODEG in the CALLERID spec"
717;RPL_TARGNOTIFY;RatBox;< nick > :< info >;Sent following a PRIVMSG / NOTICE to indicate the target has been notified of an attempt to talk to them while they are set +g
718;RPL_UMODEGMSG;RatBox;< client > < nick > < user >@< host > :< info >;Sent to a user who is +g to inform them that someone has attempted to talk to them (via PRIVMSG/NOTICE), and that they will need to be accepted (via the ACCEPT command) before being able to talk to them
720;RPL_OMOTDSTART;RatBox;< client > :< text >;IRC Operator MOTD header, sent upon OPER command
721;RPL_OMOTD;RatBox;< client > :< text >;IRC Operator MOTD text (repeated, usually)
722;RPL_ENDOFOMOTD;RatBox;< client > :< text >;IRC operator MOTD footer
723;ERR_NOPRIVS;RatBox;<client> <command> :<text>;Returned from an oper command when the IRC operator does not have the relevant operator privileges.
724;RPL_TESTMASK;RatBox;<client> <nick>!<user>@<host> <?> <?> :<text>;Reply from an oper command reporting how many users match a given user @host mask
725;RPL_TESTLINE;RatBox;<client> <?> <?> <?> :<?>;Reply from an oper command reporting relevant I/K lines that will match a given user@host
726;RPL_NOTESTLINE;RatBox;<client> <?> :<text>;Reply from oper command reporting no I/K lines match the given user@host
727;RPL_TESTMASKGECOS;RatBox;<client> <lcount> <gcount> <nick>!<user>@<host> <gecos> :Local/remote clients match;From the m_testmask module, 'Shows the number of matching local and global clients for a user@host mask'
728;RPL_QUIETLIST;Charybdis;<client> <channel> q<banid>[< time_left > :< reason >];Same thing as RPL_BANLIST, but for mode +q (quiet)
729;RPL_ENDOFQUIETLIST;Charybdis;<client> <channel> q :<info>;Same thing as RPL_ENDOFBANLIST, but for mode +q (quiet)
730;RPL_MONONLINE;RatBox;<client> :target[!user@host][,target[!user@host]]*;Used to indicate to a client that either a target has just become online, or that a target they have added to their monitor list is online
731;RPL_MONOFFLINE;RatBox;<client> :target[,target2]*;Used to indicate to a client that either a target has just left the IRC network, or that a target they have added to their monitor list is offline
732;RPL_MONLIST;RatBox;<client> :target[,target2]*;Used to indicate to a client the list of targets they have in their monitor list
733;RPL_ENDOFMONLIST;RatBox;<client> :End of MONITOR list;Used to indicate to a client the end of a monitor list
734;ERR_MONLISTFULL;RatBox;<client> <limit> <targets> :Monitor list is full.;Used to indicate to a client that their monitor list is full, so the MONITOR command failed
740;RPL_RSACHALLENGE2;RatBox;<client> :<chal_line>;From the ratbox m_challenge module, to auth opers.
741;RPL_ENDOFRSACHALLENGE2;RatBox;<client> :End of CHALLENGE;From the ratbox m_challenge module, to auth opers.
742;ERR_MLOCKRESTRICTED;Charybdis;<client> <channel> <modechar> <mlock> :MODE cannot be set due to channel having an active MLOCK restriction policy;InspIRCd 2.0 doesn't send the <client> parameter, while 3.0 does
743;ERR_INVALIDBAN;Charybdis;<channel> <modechar> <mask> :Invalid ban mask;
744;ERR_TOPICLOCK;InspIRCd?;;"Defined in the Charybdis source code with the comment `""/* inspircd */""`"
750;RPL_SCANMATCHED;RatBox;<count> :matches;From the ratbox m_scan module.
751;RPL_SCANUMODES;RatBox;<nick> <username> <host> <sockhost> <servname> <umodes> :<info>;From the ratbox m_scan module.
759;RPL_ETRACEEND;irc2.11;;
760;RPL_WHOISKEYVALUE;IRCv3;<Target> <Key> <Visibility> :<Value>;Reply to WHOIS - Metadata key/value associated with the target
761;RPL_KEYVALUE;IRCv3;<Target> <Key> <Visibility>[ :<Value>];Returned to show a currently set metadata key and its value, or a metadata key that has been cleared if no value is present in the response
762;RPL_METADATAEND;IRCv3;:end of metadata;Indicates the end of a list of metadata keys
764;ERR_METADATALIMIT;IRCv3;<Target> :metadata limit reached;Used to indicate to a client that their metadata store is full, and they cannot add the requested key(s)
765;ERR_TARGETINVALID;IRCv3;<Target> :invalid metadata target;Indicates to a client that the target of a sent METADATA command is invalid
766;ERR_NOMATCHINGKEY;IRCv3;<Key> :no matching key;Indicates to a client that the requested metadata key does not exist
767;ERR_KEYINVALID;IRCv3;<Key> :invalid metadata key;Indicates to a client that the requested metadata key is not valid
768;ERR_KEYNOTSET;IRCv3;<Target> <Key> :key not set;Indicates to a client that the metadata key they requested to clear is not already set
769;ERR_KEYNOPERMISSION;IRCv3;<Target> <Key> :permission denied;Indicates to a client that they do not have permission to set the requested metadata key
771;RPL_XINFO;Ithildin;;Used to send 'eXtended info' to the client, a replacement for the STATS command to send a large variety of data and minimize numeric pollution.
773;RPL_XINFOSTART;Ithildin;;Start of an RPL_XINFO list
774;RPL_XINFOEND;Ithildin;;Termination of an RPL_XINFO list
801;RPL_STATSCOUNTRY;InspIRCd 3.0;<count> <code> :<country>;Used by the m_geoclass module of InspIRCd.
802;RPL_CHECK;InspIRCd 3.0;;Used by the m_check module of InspIRCd.
803;RPL_OTHERUMODEIS;InspIRCd 3.0;<client> <nick> <user modes> <user mode parameters>;Similar to RPL_UMODEIS but used when an oper views the mode of another user.
804;RPL_OTHERSNOMASKIS;InspIRCd 3.0;<client> <nick> <server notice mask> :Server notice mask;Similar to RPL_SNOMASK but used when an oper views the snomasks of another user.
900;RPL_LOGGEDIN;Charybdis/Atheme, IRCv3;<client> <nick>!<ident>@<host> <account> :You are now logged in as <user>;Sent when the user's account name is set (whether by SASL or otherwise)
901;RPL_LOGGEDOUT;Charybdis/Atheme, IRCv3;<client> <nick>!<ident>@<host> :You are now logged out;Sent when the user's account name is unset (whether by SASL or otherwise)
902;ERR_NICKLOCKED;Charybdis/Atheme, IRCv3;<client> :You must use a nick assigned to you.;Sent when the SASL authentication fails because the account is currently locked out, held, or otherwise administratively made unavailable.
903;RPL_SASLSUCCESS;Charybdis/Atheme, IRCv3;<client> :SASL authentication successful;Sent when the SASL authentication finishes successfully Also see RPL_LOGGEDIN (900).
904;ERR_SASLFAIL;Charybdis/Atheme, IRCv3;< client > :SASL authentication failed;Sent when the SASL authentication fails because of invalid credentials or other errors not explicitly mentioned by other numerics
905;ERR_SASLTOOLONG;Charybdis/Atheme, IRCv3;< client > :SASL message too long;Sent when credentials are valid, but the SASL authentication fails because the client-sent AUTHENTICATE command was too long (i.e. the parameter longer than 400 bytes)
906;ERR_SASLABORTED;Charybdis/Atheme, IRCv3;< client > :SASL authentication aborted;Sent when the SASL authentication is aborted because the client sent an AUTHENTICATE command with * as the parameter
907;ERR_SASLALREADY;Charybdis/Atheme, IRCv3;< client > :You have already authenticated using SASL;Sent when the client attempts to initiate SASL authentication after it has already finished successfully for that connection.
908;RPL_SASLMECHS;"""Charybdis/Atheme";"IRCv3""";< client > < mechanisms > :are available SASL mechanisms
910;RPL_ACCESSLIST;InspIRCd;< client > < channel > < status >:< mask > < who > < set - ts >;Used by InspIRCd's m_autoop module.
911;RPL_ENDOFACCESSLIST;InspIRCd;< client > < channel > :End of channel access list;Used by InspIRCd's m_autoop module.
926;ERR_BADCHANNEL;InspIRCd;< client > < channel > :Channel<channel> is forbidden: < reason >;Used by InspIRCd's m_denychans module.
936;ERR_WORDFILTERED;InspIRCd;"""<client> <channel> <message> :Your message contained a censored word";"and was blocked"""
937;ERR_ALREADYCHANFILTERED;InspIRCd 2.0;< client > < chan > :The word<word> is already on the spamfilter list;Used by InspIRCd's m_chanfilter module. Replaced with ERR_LISTMODEALREADYSET in 3.0.
938;ERR_NOSUCHCHANFILTER;InspIRCd 2.0;< client > < chan > :No such spamfilter word is set;Used by InspIRCd's m_chanfilter module. Replaced with ERR_LISTMODENOTSET in 3.0.
939;ERR_CHANFILTERFULL;InspIRCd 2.0;< client > < chan > :Channel spamfilter list is full;Used by InspIRCd's m_chanfilter module. Replaced with ERR_BANLISTFULL in 3.0.
940;RPL_ENDOFSPAMFILTER;InspIRCd;< client > < channel > :End of channel spamfilter list;Used by InspIRCd's m_chanfilter module.
941;RPL_SPAMFILTER;InspIRCd;< client > < channel > < filter > < who > < set - ts >;Used by InspIRCd's m_chanfilter module.
942;ERR_INVALIDWATCHNICK;InspIRCd;< client > < nick > :Invalid nickname;Used by InspIRCd's m_watch module.
944;RPL_IDLETIMESET;InspIRCd;< client > :Idle time set.;Used by InspIRCd's m_setidle module.
945;RPL_NICKLOCKOFF;InspIRCd;< client > < nick > :Nickname now unlocked.;Used by InspIRCd's m_nicklock module.
946;ERR_NICKNOTLOCKED;InspIRCd;< client > < nick > :This user's nickname is not locked.;Used by InspIRCd's m_nicklock module.
947;RPL_NICKLOCKON;InspIRCd;< client > < nick > :Nickname now locked.;Used by InspIRCd's m_nicklock module.
948;ERR_INVALIDIDLETIME;InspIRCd;< client > :Invalid idle time.;Used by InspIRCd's m_setidle module.
950;RPL_UNSILENCED;InspIRCd;< client > < nick > :Removed<mask> < pattern > from silence list;Used by InspIRCd's m_silence module.
951;RPL_SILENCED;InspIRCd;< client > < nick > :Added<mask> < pattern > to silence list;Used by InspIRCd's m_silence module.
952;ERR_SILENCE;InspIRCd;< client > < mask > [< flags >] :< reason >;Used by InspIRCd's m_silence module. The flags field was added in v3.
953;RPL_ENDOFEXEMPTIONLIST;InspIRCd;< client > < channel > :End of channel exemptchanops list;Used by InspIRCd's m_exemptchanop module.
954;RPL_EXEMPTIONLIST;InspIRCd;< client > < channel > < exception > < who > :< set - ts >;Used by InspIRCd's m_exemptchanop module.
960;RPL_ENDOFPROPLIST;InspIRCd;< client > < channel > :End of mode list;Used by InspIRCd's m_namedmodes module.
961;RPL_PROPLIST;InspIRCd;< client > < channel > [+< mode >]...;Used by InspIRCd's m_namedmodes module.
972;ERR_CANNOTDOCOMMAND;Unreal;< client > < command > :< info >;"""Indicates that a command could not be performed for an arbitrary reason. For example"
972;ERR_CANTUNLOADMODULE;InspIRCd;< client > < modulename > :Failed to unload module: < error >;
973;RPL_UNLOADEDMODULE;InspIRCd;< client > < modulename > :Module successfully unloaded.;
974;ERR_CANNOTCHANGECHANMODE;Unreal;< client > < mode > :< info >;"""Indicates that a channel mode could not be changed for an arbitrary reason. For instance"
974;ERR_CANTLOADMODULE;InspIRCd;< client > < modulename > :Failed to load module: < error >;
975;RPL_LOADEDMODULE;InspIRCd;< client > < modulename > :Module successfully loaded.;
975;ERR_LASTERROR;Nefarious;;
988;RPL_SERVLOCKON;InspIRCd;< client > < server > :Closed for new connections;Used by InspIRCd's m_lockserv module.
989;RPL_SERVLOCKOFF;InspIRCd;< client > < server > :Open for new connections;Used by InspIRCd's m_lockserv module.
990;RPL_DCCALLOWSTART;InspIRCd;< client > :Users on your DCCALLOW list:;Used by InspIRCd's m_dccallow module
991;RPL_DCCALLOWLIST;InspIRCd;< client > < nick > :< nick > (< hostmask >);Used by InspIRCd's m_dccallow module
992;RPL_DCCALLOWEND;InspIRCd;< client > :End of DCCALLOW list;Used by InspIRCd's m_dccallow module
993;RPL_DCCALLOWTIMED;InspIRCd;< client > < nick > :Added < nick > to DCCALLOW list for < count > seconds;Used by InspIRCd's m_dccallow module
994;RPL_DCCALLOWPERMANENT;InspIRCd;< client > < nick > :Added < nick > to DCCALLOW list for this session;Used by InspIRCd's m_dccallow module
995;RPL_DCCALLOWREMOVED;InspIRCd;< client > < nick > :Removed < nick > from your DCCALLOW list;Used by InspIRCd's m_dccallow module.
996;ERR_DCCALLOWINVALID;InspIRCd;< client > < nick > :< reason >;Used by InspIRCd's m_dccallow module.
997;RPL_DCCALLOWEXPIRED;InspIRCd;< client > < nick > :DCCALLOW entry for < nick > has expired;Used by InspIRCd's m_dccallow module.
998;ERR_UNKNOWNDCCALLOWCMD;InspIRCd;< client > :DCCALLOW command not understood;Used by InspIRCd's m_dccallow module.
998;RPL_DCCALLOWHELP;InspIRCd;< client > :< help >;Used by InspIRCd's m_dccallow module.
999;RPL_ENDOFDCCALLOWHELP;InspIRCd 2.0;< client > :End of DCCALLOW HELP;Used by InspIRCd's m_dccallow module.
999;ERR_NUMERIC_ERR;Bahamut;;Also known as ERR_NUMERICERR (Unreal)or ERR_LAST_ERR_MSG
""""";








string GetCommandInfoData() =>
"""
Command;Syntax;Function;Defined
ADMIN;ADMIN [<server>];Asks the server for information about the administrator of the server <target>,If target is not used, the current server is used.[1];RFC1459
AWAY;AWAY [<message>];GIves the server a message to send in reply to a PRIVMSG to the user.[2] Removes away status if <message> is not used.;RFC1459
CONNECT;CONNECT <target server> [<port> [<remote server>]] (RFC1459) CONNECT <target server> <port> [<remote server>] ( RFC2812);Tells the server <remote server> (or the current server, if <remote server> is not used) to connect to <target server> on port <port>.[3][4] This command can normally only be used by IRC Operators.;RFC1459
DIE;DIE;Tells the server to shut down.[5];RFC2812
ERROR;ERROR <error message>;Use by servers to report errors to other servers. It is also used before ending client connections.[6];RFC1459
INFO;INFO [<target>];Gives information about the <target> server, or the current server if <target> is not used[7] The Information given includes the server's version, when it was compiled, the patch level, when it was started, and any other information which may be relevant.;RFC1459
INVITE;INVITE <nickname> <channel>;Invites <nickname> to the channel <channel>.[8] <channel> does not have to exist. If it does, only members of the channel can invite other clients. If the channel mode i is set, only channel operators may invite other clients.;RFC1459
ISON;ISON <nicknames>;Asks the server to see if the clients in the list <nicknames> are on the network.[9] The server returns the nicknames that are on the network. If none of the clients are on the network the server returns an empty list.;RFC1459
JOIN;JOIN <channels> [<keys>];Makes the client join the channels in the list <channels> Passwords can be used in the list <keys>.[10] If the channel(s) do not exist, they will be created.;RFC1459
KICK;KICK <channel> <client> [<message>];Removes <client> from <channel>.[11] This command may only be issued by channel operators.;RFC1459
KILL;KILL <client> <comment>;Removes <client> from the network.[12] This command may only be used by IRC operators.;RFC1459
LINKS;LINKS [<remote server> [<server mask>]];Lists all server links matching <server mask> on <remote server>, or the current server if <remote server> is not used.[13];RFC1459
LIST;LIST [<channels> [<server>]];Lists all channels on the server.[14] If the list <channels> is given, it will return the channel topics. If <server> is given, the command will be sent to <server> for evaluation.;RFC1459
LUSERS;LUSERS [<mask> [<server>]];Returns statistics about the size of the network.[15] If used with no arguments, the statistics will be about the entire network. If <mask> is given, it will return only statistics about the masked subset of the network. If <target> is given, the command will be sent to <server> for evaluation.;RFC2812
MODE;"MODE <nickname> <flags> (user)
MODE <channel> <flags> [<args>]";The MODE command is has two uses. It can be used to set both user and channel modes.[16];RFC1459
MOTD;MOTD [<server>];Returns the message of the day on <server> or the current server if it is not stated.[17];RFC2812
NAMES;"NAMES [<channels>] (RFC1459)
NAMES [<channels> [<server>]] ( RFC2812)";"Returns a list of who is on the list of <channels>, by channel name.[18] If <channels> is not used, all users are shown,. They are grouped by channel name with all users who are not on a channel being shown as part of channel ""*"". If <server> is specified, the command is sent to <server> for evaluation.[19]";RFC1459
NICK;NICK <nickname> [<hopcount>] (RFC1459) NICK <nickname> ( RFC2812);Allows a client to change their IRC nickname. Hopcount is for use between servers to say how far away a nickname is from its home server.[20][21];RFC1459
NOTICE;NOTICE <msgtarget> <message>;Similar to PRIVMSG, but automatic replies are never sent in reply to NOTICE messages.[22];RFC1459
OPER;OPER <username> <password>;Identifies a user as an IRC operator on that server/network.[23];RFC1459
PART;PART <channels>;Causes a user to leave the channels in the list <channels>.[24];RFC1459
PASS;PASS <password>;Sets a connection password.[25] This command must be sent before the NICK/USER registration combination.;RFC1459
PING;PING <server1> [<server2>];Tests a connection.[26] A PING message results in a PONG reply. If <server2> is given, the message is sent to it.;RFC1459
PONG;PONG <server2> [<server2>];This command is a reply to the PING command. It works in much the same way.[27];RFC1459
PRIVMSG;PRIVMSG <msgtarget> <message>;Sends <message> to <msgtarget>. The target is usually a user or channel.[28];RFC1459
QUIT;QUIT [<message>];Disconnects the user from the server.[29];RFC1459
REHASH;REHASH;Causes the server to re-read and re-process its configuration file(s).[30] This command can only be sent by IRC Operators.;RFC1459
RESTART;RESTART;Restarts a server.[31] It may only be sent by IRC Operators.;RFC1459
SERVICE;SERVICE <nickname> <reserved> <distribution> <type> <reserved> <info>;Registers a new service on the network.[32];RFC2812
SERVLIST;SERVLIST [<mask> [<type>]];Lists the services currently on the network.[33];RFC2812
SERVER;SERVER <servername> <hopcount> <info>;Used to tell a server that the other end of a new connection is a server.[34] This message is also used to send server data over whole network. <hopcount> says how many hops (server connections) away <servername> is.<info> has information about the server.;RFC1459
SQUERY;SQUERY <servicename> <text>;Same as PRIVMSG except it must be sent to a service.[35];RFC2812
SQUIT;SQUIT <server> <comment>;Causes <server> to quit the network.[36];RFC1459
STATS;STATS <query> [<server>];Returns statistics about the current server, or <server> if it is given.[37];RFC1459
SUMMON;SUMMON <user> [<server>] (RFC1459 SUMMON <user> [<server> [<channel>]] ( RFC2812);Sends a message to users on the same host as <server> asking them to join IRC.[38][39];RFC1459
TIME;TIME [<server>];Gives the local time on the current server, or <server> if specified.[40];RFC1459
TOPIC;TOPIC <channel> [<topic>];Used to get the channel topic on <channel>.[41] If <topic> is given, it sets the channel topic to <topic>. If channel mode +t is set, only a channel operator may set the topic.;RFC1459
TRACE;TRACE [<target>];Trace a path across the IRC network to a specific server or client, in a similar method to traceroute.[42];RFC1459
USER;USER <username> <hostname> <servername> <realname> (RFC1459) USER <user> <mode> <unused> <realname> ( RFC2812);This command is used at the beginning of a connection to specify the username, hostname, real name and initial user modes of the connecting client.[43][44] <realname> may contain spaces, and thus must be prefixed with a colon.;"RFC2812"
USERHOST;USERHOST <nickname> [<nickname> <nickname> ...];Returns a list of information about the nicknames specified.[45];RFC1459
USERS;USERS [<server>];Returns a list of users and information about those users in a format similar to the UNIX commands who, rusers and finger.[46];RFC1459
VERSION;VERSION [<server>];Returns the version of <server>, or the current server if omitted.[47];RFC1459
WALLOPS;WALLOPS <message>;Sends <message> to all operators connected to the server (RFC1459), or all users with user mode 'w' set ( RFC2812).[48][49];RFC1459
WHO;"WHO [<name> [""o""]]";"Returns a list of users who match <name>.[50] If the flag ""o"" is given, the server will only return information about IRC Operators.";RFC1459
WHOIS;WHOIS [<server>] <nicknames>;Returns information about the comma-separated list of nicknames masks <nicknames>.[51] If <server> is given, the command is forwarded to it for processing.;RFC1459
WHOWAS;WHOWAS <nickname> [<count> [<server>]];Returns information about a nickname that is no longer in use (due to client disconnection, or nickname changes).[52] If given, the server will return information from the last <count> times the nickname has been used. If <server> is given, the command is forwarded to it for processing. In RFC2812, <nickname> can be a comma-separated list of nicknames.[53];RFC1459
""";


public class ReplyInfo
{
    public int Number { get; set; }
    public string Name { get; set; }
    public string Origin { get; set; }
    public string Format { get; set; }
    public string Comments { get; set; }
}

public class ReplyInfoClassMap : ClassMap<ReplyInfo>
{
    public ReplyInfoClassMap()
    {
        Map(m => m.Number).Name("Number");
        Map(m => m.Name).Name("Name");
        Map(m => m.Origin).Name("Origin");
        Map(m => m.Format).Name("Format");
        Map(m => m.Comments).Name("Comments");
    }
}

public class CommandInfo
{
    public CommandName Command { get; set; }
    public string Syntax { get; set; }
    public string Helptext { get; set; }
    public Rfc RFC { get; set; }
}

public class CommandInfoClassMap : ClassMap<CommandInfo>
{
    public CommandInfoClassMap()
    {
        Map(m => m.Command).Name("Command");
        Map(m => m.Syntax).Name("Syntax");
        Map(m => m.Helptext).Name("Function");
        Map(m => m.RFC).Name("Defined");
    }
}

[Flags]
public enum Rfc
{
    RFC2812 = 2812,
    RFC1459 = 1459
}

public enum CommandName
{
    ADMIN,
    AWAY,
    CONNECT,
    DIE,
    ERROR,
    INFO,
    INVITE,
    ISON,
    JOIN,
    KICK,
    KILL,
    LINKS,
    LIST,
    LUSERS,
    MODE,
    MOTD,
    NAMES,
    NICK,
    NOTICE,
    OPER,
    PART,
    PASS,
    PING,
    PONG,
    PRIVMSG,
    QUIT,
    REHASH,
    RESTART,
    SERVICE,
    SERVLIST,
    SERVER,
    SQUERY,
    SQUIT,
    STATS,
    SUMMON,
    TIME,
    TOPIC,
    TRACE,
    USER,
    USERHOST,
    USERS,
    VERSION,
    WALLOPS,
    WHO,
    WHOIS,
    WHOWAS
}
# I Just Wanted to Make an IRC Client/Server Library

This is the story of how I wanted to make an IRC client/server library, but ended up making a workflow library, a Pub/Sub 
library, and a DI Channel library, a data persistence library, CronJobs, and a bunch of other things along the way, while still 
not having any idea how to make an IRC client/server library. Adopting a Torrent client along the way was interesting though.

## TL;DR

I wanted to make an IRC client/server library, but ended up making an entire ecosystem of libraries, and still don't have
any IRC client/server library to show for it.

## Motivation

IRC is a protocol that has been around for a long time. It is simple, and it is still used today, and it is still relevant. 
It lends itself to be used in many different ways, and it is a good protocol to learn about networking, and to learn about 
low level stuff and efficient data/string manipulation. It is also a good protocol to learn about security, as it has no 
security. It is also a good protocol to learn about how to make a client/server library.

## The Idea

I wanted to make an IRC client/server library. I wanted to make it in a way that it would be easy to use, and easy to extend.
It should be .net, nuget distributed, and should be able to run on cross-platform. It should be close to out-of-the-box, but 
alse a framework that can be extended. It should be able to run as a server, and as a client, (in different packages of course).

## The Journey

First I looked at the existing libraries. There are a few, many are .net/.net core, but none of them are close to what I 
wanted. I am a big fan of Dependency Injection, and I wanted to make the library in a way that it would be easy to use with 
this pattern. I also wanted to make it in a way that it would be easy to use with modern .net core features like Channel<>, 
IHostedService, and CronJobs.

I started with the server. I wanted to make it in a way that it would be easy to use, and easy to extend. I wanted to make 
it so it would be easy to use with Dependency Injection, and I wanted to make it so it would be easy to use with modern .net,
and also wanted everything to be async. I wanted to make it so it would be easy to use with Channel<>, and I wanted to make 
it with mostly my own code, and not rely on too many third party libraries. Easy peasy, right? WRONG!

### Where it started to go wrong

So I am on the .net stack, and I love it. Microsoft has done a great job with .net core, but its microsoft, and so they 
don't give you A gun to shoot yourself in the foot, they give you a machine gun, or at least a box of guns. So I started 
looking at the System.Net.Sockets namespace, and I was like, "Oh, this is easy, I can do this." And then I started looking 
into life cycle management, and I was like, "Oh, this NOT easy, I can NOT do this.", is this skillissues? Yes, but also: The 
.net team hasn't made it easy to work with sockets. I sank so low I asked a question on Reddit, /dotnet, and I got an answer,
from none other than David Fowler, he pointed me to a tech demo he had noted on his GitHub. This was a good start, and I 
found a library he had made called "Bedrock.Framework". I started looking into it, and I was like, "Oh, this is easy, but oh 
so much stuff I don't need, and oh so much stuff I don't understand." I started looking into the source code, and I did the 
rational thing, I copied it, and started to strip it down, didn't like it, then I took a step back, and started to look at 
what he was doing and saw he was basically demonstrating how to use the new `Microsoft.AspNetCore.Connections.Abstractions`, 
so I pivoted, and now I have a TCP client/server library that is using the new `Microsoft.AspNetCore.Connections.
Abstractions` called "Frank.BedrockSlim". I am happy with it. It is async, it is easy to use, and it is easy to extend. It 
uses a bear minimum of asp.net core low level stuff, and it is easy to use with Dependency Injection. The server does have a 
framework reference to asp.net core, but the client does not. I am happy with it, but I still don't have an IRC 
client/server you might have noticed.

### Servers need data persistence

So I have a server, and I have a client, and I have a protocol, and I have a way to communicate, but servers usually need 
data. Sessions, users, channels, messages, etc. I started to look into how to do this, and I was like, "Oh, this is easy, I 
just use a database and Entity Framework Core". Well, a database, even SQLite, is not always the correct answer. I wanted 
flexibility in my data persistence, because user profile data is nice to blob in a json, logs should be in a log file, and 
messages should perhaps be in a database. Well, I solved it by making a data persistence library called "Frank.DataStorage". 
Now I have a common interface, and I have a few implementations, and I can use it with Dependency Injection. Now I'm just 5 
months into the project, and I still don't have an IRC client/server library. I have something cool though.

### Oooh, shiny!

Channel<>, is maybe the coolest thing added to .net core in the last few years. It is a way to pass data between threads, 
from one to many, from many to one, and it is async. I wanted to use it in my server, and I wanted to use it in my client, 
so it can be more reactive, and many things are workflow-like, maybe I should make a workflow library? Forshadowing? 
Channels seems to be awesome for in-memory Pub/Sub, and I wanted to make a library for that, and I did, and I called it 
`Frank.PulseFlow`. It is a simple Pub/Sub library that uses Channel<>, and it is easy to use with Dependency Injection. Oh, 
there is a need for DI Channel<> other places too, so I made a library for that, and I called it `Frank.Channels.
DependencyInjection`, (It's really cool, you just register your type as a channel and you can inject the entire channel, the 
reader or writer, and looking at resource use its like nothing, AWESOME!). I'm getting a bit off track here, wasn't I making an 
IRC client/server library?

### CronJobs

I have contributed to a few open source projects, one of them is a library called `CronQuery`. It is a library that can very 
easily parse a cron expression from configuation, and start DI CronJobs. I wanted to use it in my server and client, and I 
wanted to use it a bit differently than it was intended, so I made a library called `Frank.CronJobs`. It is a library that 
don't rely on the `Microsoft.Extensions.Configuration` namespace, and it is easy to use with Dependency Injection. I have a 
great deal of respect for open source, and so I copied the license header from the original library, and I have a link to it,
and even rewrote a lot of the more complex parts of the library, so I'm not just a copycat.

### Workflows

I have been working with workflows for a long time, and I have been working with a lot of different workflow engines, I have 
even created a few myself. But I wanted to make a workflow engine that is built on Dependency Injection and modern .net core 
and that uses Channel<>, IHostedService, and CronJobs. I wanted to make it so it would be easy to use, and easy to extend. I 
wanted it to be simple, because I understand that workflows can be complex, but most of the time they are not, they are just 
a linear sequence of steps. So I made a library called `Frank.WorkflowEngine`. It is a simple workflow engine that is built 
with a lot of the previously mentioned libraries.

### Security, what's that?

So, I remembered a tiny bit about IRC, NO SECURITY! Its text over TCP, and it is not encrypted, and it is not authenticated, 
and an IRC server should at least be a little secure, and so I started to look into how to make it secure, and I was like, 
"Oh, lets build a security library", and I did, and I called it `Frank.Security`, (There's a pattern to my naming scheme 
that is REALLY hard to deduce). What kind of security? Well, password hashing, passprashe generation, and a few other things.
How to use it with IRC? Well, he he... Actually securing and encrypting IRC is a bit more complex than I thought, and I can 
say that I'd rather not do it, and I can say that there is no end-to-end encryption, rather than doing it wrong. I have some 
experience with security, but low level networking security and cryptography is not my strong suit, and I don't want to do it badly.

Side note: No security, and openness about it, is usually exponentially better than bad security. A bad implementation of 
encryption is worse than no encryption, as it gives a false sense of security, and opens one up to liability.

### Torrents

How did I end up with a Torrent client? Well, I wanted to dowload some files, and I wanted to do it with a Torrent as it was 
a bunch of data science stuff, and it would take forever to download, so Torrent was the best option. I didn't want to 
install anything, so I looked into Torrent Client libraries, and I found a few, and they were all pretty old and outdated, 
so I forked one, and I started to look into it, and spent a long time to get it into a usable state, and I called it `Frank.
TorrentClient`. It is a simple Torrent client that is built on Dependency Injection and modern .net core features and have a 
couple of UI projects that are built on AvaloniaUI and WPF. It needs a lot of work, but it is usable for now.

### A Game Framework/Engine? Seriously? What's wrong with you?

So, I used to be a game developer, (unity), but I was always fighting the engine. I wanted to buold my own engine, or rather 
my own framework, and I did, and I called it `Frank.GameEngine`. It is a simple game framework that is built on the idea of 
interchangeable parts, so you can use whatever rendering engine you want, and whatever physics engine you want, and it 
should just work. Its pretty jankey, and it needs a lot of work, especially the collision detection.

## Conclusion

So, I have made a lot of libraries, and I have made a lot of things, and I have learned a lot, and I have had a lot of "fun",
yet I still don't have an IRC client/server library. The journey has been interesting, and while I have not reached my 
endgoal, I have made an ecosystem of libraries that are easy to use, and easy to extend, and that are built on Dependency 
Injection and modern .net core features. This has helped me be a better developer, but I still want my IRC client/server 
library to be a reality. I have a lot of ideas, and I have a lot of code, and I have a lot of experience.

## The Future

Will I ever make an IRC client/server library? I don't know, and now I have seen a few other technologies besides TCP that 
might be better suited for making a chat server. There is a lot of cool stuff out there, and I have a lot of ideas, so for 
now I will just keep on making stuff and see where it takes me. I am very interested in code generation, and it would be 
benefical to make a code generator for the IRC client/server library, as there are a lot of commands and events, and LLMs is 
a thing I have been looking into, and that also might be a good fit for the IRC server library, because building in a 
moderation chat bot would be cool.
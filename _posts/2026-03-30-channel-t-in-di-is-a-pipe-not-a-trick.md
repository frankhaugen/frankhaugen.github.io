---
title: "Channel<T> in DI" is a pipe, not a trick
tags:
  - dotnet
  - channels
  - dependency-injection
description: Why Frank.Channels.DependencyInjection exists — backed by System.Threading.Channels docs, DI lifetimes, and when queues lie.
---

People hear "inject a `Channel<T>`" and assume it is cleverness for cleverness' sake. Like we are trying to win a conference talk.

It is not.

## Facts: what `System.Threading.Channels` gives you

Introduced to provide a **producer/consumer** primitive tuned for async code, `System.Threading.Channels` exposes `Channel<T>` with bounded / unbounded modes, explicit completion, and reader/writer halves that cooperate with `async`/`await` without busy-waiting.[^channels-intro]

Useful mental model:

| Piece | Role |
|-------|------|
| `ChannelWriter<T>` | Producers complete / signal failure explicitly |
| `ChannelReader<T>` | Consumers `ReadAllAsync` or try-read patterns |
| Bounded channel | Backpressure when producers outrun consumers[^bounded] |
| Unbounded channel | Memory becomes your implicit throttle (dangerous under load) |

Stephen Toub's detailed posts on channels remain the best narrative explanation of design trade-offs inside .NET's concurrency toolbox.[^toub]

## Facts: why Dependency Injection cares about lifetime

Any shared mutable pipe must align with **service lifetime**:

- **Singleton** channel → every consumer sees one logical bus (great for in-process pub/sub).
- **Scoped** channel → one pipe per request/scope (good when isolation matters).
- **Transient** writers/readers without a stable channel → usually accidental complexity.

Microsoft documents service lifetimes (`Singleton`, `Scoped`, `Transient`) and cautions about capturing scoped services in singletons—the same reasoning applies when the "service" is effectively an async queue.[^di-lifetimes]

## Opinion (labeled honestly): what this library is

A **named pipe with async manners** that the host already understands: backpressure, completion, multiple readers/writers — the stuff you would otherwise reinvent with `ConcurrentQueue` plus vibes.

Registering `Channel<T>` in DI is saying: *this subsystem talks to that subsystem through here, and the lifetime is explicit.*

## What it is not

It is not a replacement for messaging infrastructure. It is not Kafka in your process. It is not "use channels everywhere because channels are cool" — that way lies spaghetti with async characteristics.

## Pitfalls that bite real systems

1. **Unbounded memory** — unbounded channels absorb spikes until `OutOfMemoryException` becomes your profiler.
2. **Forgotten completion** — readers wait forever if writers never call `Complete`.
3. **Blocking sync-over-async on channel edges** — defeats the whole point; keep boundaries async.
4. **Lifetime mismatch** — singleton consumer holding scoped producer references leaks scope or starves work.

## Why I bothered packaging it

Because I kept writing the same three registrations in every project, and copy-paste across repos is how subtle bugs become *tradition*.

## TL;DR

If the API feels boring, it is working. The interesting part is what you push through the channel — not the fact that DI can hand you the ends.

[^channels-intro]: Microsoft Learn — `System.Threading.Channels` namespace overview. https://learn.microsoft.com/dotnet/api/system.threading.channels

[^bounded]: Microsoft Learn — `BoundedChannelOptions` / full / dropped modes. https://learn.microsoft.com/dotnet/api/system.threading.channels.boundedchanneloptions

[^toub]: Stephen Toub — *An Introduction to System.Threading.Channels* (MSDN Magazine / .NET Blog series on channels). https://devblogs.microsoft.com/dotnet/an-introduction-to-system-threading-channels/

[^di-lifetimes]: Microsoft Learn — *Dependency injection in .NET* (service lifetimes). https://learn.microsoft.com/dotnet/core/extensions/dependency-injection#service-lifetimes

## References

- [Microsoft Learn — System.Threading.Channels](https://learn.microsoft.com/dotnet/api/system.threading.channels)
- [Microsoft Learn — Dependency injection in .NET](https://learn.microsoft.com/dotnet/core/extensions/dependency-injection)
- [Stephen Toub — Introduction to System.Threading.Channels](https://devblogs.microsoft.com/dotnet/an-introduction-to-system-threading-channels/)
- [Frank.Channels.DependencyInjection (GitHub)](https://github.com/frankhaugen/Frank.Channels.DependencyInjection)

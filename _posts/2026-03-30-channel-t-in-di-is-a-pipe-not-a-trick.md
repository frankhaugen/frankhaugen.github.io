---
title: "Channel<T> in DI" is a pipe, not a trick
tags:
  - dotnet
  - channels
  - dependency-injection
description: Why Frank.Channels.DependencyInjection exists — and why it is smaller than it sounds on purpose.
---

People hear "inject a `Channel<T>`" and assume it is cleverness for cleverness' sake. Like we are trying to win a conference talk.

It is not.

## What it actually is

A **named pipe with async manners** that the host already understands: backpressure, completion, multiple readers/writers — the stuff you would otherwise reinvent with `ConcurrentQueue` plus vibes.

Registering `Channel<T>` in DI is just saying: *this subsystem talks to that subsystem through here, and the lifetime is explicit.*

## What it is not

It is not a replacement for messaging infrastructure. It is not Kafka in your process. It is not "use channels everywhere because channels are cool" — that way lies spaghetti with async characteristics.

## Why I bothered packaging it

Because I kept writing the same three registrations in every project, and copy-paste across repos is how subtle bugs become *tradition*.

## TL;DR

If the API feels boring, it is working. The interesting part is what you push through the channel — not the fact that DI can hand you the ends.

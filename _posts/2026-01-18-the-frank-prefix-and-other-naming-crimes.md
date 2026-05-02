---
title: The Frank.* prefix and other naming crimes
tags:
  - dotnet
  - open-source
  - opinions
description: Yes, everything is Frank.Something. NuGet rules, discoverability tax, and why vanity namespaces persist.
---

If you browse my GitHub, you will notice a pattern so obvious it is almost a joke: **Frank.** everything. Frank.BedrockSlim. Frank.PulseFlow. Frank.WhateverIThoughtOfLastTuesday.

## Facts: how NuGet expects packages to identify themselves

NuGet package IDs must follow predictable rules—length limits, allowed characters, no reserved prefixes unless you own them. Microsoft documents authoring guidelines and **ID prefix reservation** so consumers can trust who publishes `Microsoft.*`, `System.*`, etc.[^nuget-id]

That matters because names are not cosmetic—they control restore graphs, typosquatting risk, and tooling assumptions.

## Is that good branding?

For *discoverability*, absolutely not. NuGet search is not going to surface "Frank" as a category unless you already know me. For *ownership*, though, it is weirdly practical: I can tell at a glance what I spawned, what I maintain, and what is allowed to be as opinionated as I want without pretending it is a neutral industry utility.

OSS ecosystems repeatedly debate **namespaces vs vanity prefixes**—npm scoped packages (`@org/pkg`) solved part of the collision surface; NuGet leans on prefix reservations + longer IDs instead.

## The real reason

Half of it is ego — I will not pretend otherwise. The other half is namespace hygiene in a world where every second word is `Microsoft.Extensions.SomethingSomething`. I want my packages to read like **mine**, not like a missing chapter of the framework docs.

## When it is embarrassing

When someone asks "which Frank library does channels again?" and I have to mentally grep twelve repos. When I explain my work to a normal human and they think my legal name is a product line.

## When I would still do it again

Because naming is the first API you ship. If `Frank.*` filters out people who cannot tolerate a little vanity, those people were never going to enjoy my commit messages either.

[^nuget-id]: Microsoft Learn — *Package authoring best practices* / identifier rules and prefix reservation. https://learn.microsoft.com/nuget/concepts/package-id-prefix-reservation

## References

- [Microsoft Learn — NuGet package authoring](https://learn.microsoft.com/nuget/concepts/package-authoring-best-practices)
- [Microsoft Learn — Package ID prefix reservation](https://learn.microsoft.com/nuget/concepts/package-id-prefix-reservation)
- [NuGet Gallery — search guidelines](https://learn.microsoft.com/nuget/consume-packages/finding-and-choosing-packages)

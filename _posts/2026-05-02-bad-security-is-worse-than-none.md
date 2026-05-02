---
title: Bad security is worse than admitting you have none
tags:
  - security
  - networking
  - opinions
description: IRC has no security. So do a lot of tutorials with TLS stapled on by someone who watched half a YouTube.
---

This showed up in my longer IRC saga, but it deserves its own line because people still argue about it wrong.

## The setup

IRC is text over TCP. No encryption in the classic story. No authentication that survives a stiff breeze. If you squint, that is horrifying.

## The worse option

Rolling your own crypto-ish layer because you *feel* bad about plaintext — but doing it slightly wrong — is how you get **false confidence**. Users think they are safe. You think you are safe. The attacker thanks you for the predictable mistake.

## The boring correct stance

Either use a boring, audited stack end-to-end, or be loudly honest about what you are not protecting. "No security" with documentation is often more ethical than "security theater" with marketing.

## Where I land

I would rather say "I am not the person to implement your threat model" than ship a half-understood cipher suite because sleep deprivation and hubris formed an alliance.

## TL;DR

Admitting limits is not defeat. Shipping bad crypto because you could not stand the shame of plaintext — that is defeat with extra steps.

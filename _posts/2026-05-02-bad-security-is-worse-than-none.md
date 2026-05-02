---
title: Bad security is worse than admitting you have none
tags:
  - security
  - networking
  - opinions
description: CWE-327, TLS reality, IRC cleartext, and why theater loses to honesty.
---

This showed up in my longer IRC saga, but it deserves its own line because people still argue about it wrong.

## Facts: what "no security" means on the wire

Classic IRC predates ubiquitous TLS on the wire: protocol documents assume cleartext TCP unless an implementation layers transport security separately.[^rfc2812] Modern deployments often terminate TLS at proxies or use vendor-specific extensions—the baseline remains "know what you actually expose."

TLS itself is standardized (today **TLS 1.3** in RFC 8446; TLS 1.2 still documented in RFC 5246 for historical comparison).[^rfc8446][^rfc5246] The lesson for application authors is not memorizing cipher suites—it is **using maintained implementations** (OS/BoringSSL/OpenSSL/.NET stack) instead of rolling bespoke crypto.

## Facts: why "rolled my own crypto" ages poorly

MITRE **CWE-327** documents *Use of a Broken or Risky Cryptographic Algorithm*—including adopting obsolete primitives or constructing ad-hoc protocols.[^cwe327] OWASP guidance repeatedly warns that custom cryptography combine-and-match tends to fail open under real attackers.[^owasp-crypto]

**Related failure mode:** downgrade attacks and oracle behaviors appear when authenticated encryption + proper key hierarchy are skipped—why TLS evolution focuses on authenticated AEAD ciphersuites and tight handshake rules.

## The setup (IRC-shaped honesty)

IRC is text over TCP in the textbook telling. No encryption in the classic story. No authentication that survives a stiff breeze. If you squint, that is horrifying—but at least it is *honest* horrifying.

## The worse option

Rolling your own crypto-ish layer because you *feel* bad about plaintext — but doing it slightly wrong — is how you get **false confidence**. Users think they are safe. You think you are safe. The attacker thanks you for the predictable mistake.

This aligns with broader engineering consensus: prefer proven protocols implemented by specialists; document threat boundaries instead of improvising them.

## The boring correct stance

Either use a boring, audited stack end-to-end, or be loudly honest about what you are not protecting. "No security" with documentation is often more ethical than "security theater" with marketing.

Concrete checklist:

1. Name the adversary (network observer? malicious server? compromised client?).
2. Pick **TLS or Noise or QUIC** from maintained stacks—not "AES + vibes."
3. Publish what you **do not** mitigate (social engineering, endpoint malware, etc.).

## Where I land

I would rather say "I am not the person to implement your threat model" than ship a half-understood cipher suite because sleep deprivation and hubris formed an alliance.

## TL;DR

Admitting limits is not defeat. Shipping bad crypto because you could not stand the shame of plaintext — that is defeat with extra steps.

[^rfc2812]: IETF RFC 2812 — Internet Relay Chat: Client Protocol (architecture / security considerations point to cleartext assumptions). https://datatracker.ietf.org/doc/html/rfc2812

[^rfc8446]: IETF RFC 8446 — The Transport Layer Security (TLS) Protocol Version 1.3. https://datatracker.ietf.org/doc/html/rfc8446

[^rfc5246]: IETF RFC 5246 — The Transport Layer Security (TLS) Protocol Version 1.2 (historical reference). https://datatracker.ietf.org/doc/html/rfc5246

[^cwe327]: MITRE CWE-327 — Use of a Broken or Risky Cryptographic Algorithm. https://cwe.mitre.org/data/definitions/327.html

[^owasp-crypto]: OWASP Cryptographic Storage Cheat Sheet / guidance on using standard algorithms correctly. https://cheatsheetseries.owasp.org/cheatsheets/Cryptographic_Storage_Cheat_Sheet.html

## References

- [RFC 2812 — IRC Client Protocol](https://datatracker.ietf.org/doc/html/rfc2812)
- [RFC 8446 — TLS 1.3](https://datatracker.ietf.org/doc/html/rfc8446)
- [RFC 5246 — TLS 1.2](https://datatracker.ietf.org/doc/html/rfc5246)
- [CWE-327 — Broken/Risky Crypto Algorithm](https://cwe.mitre.org/data/definitions/327.html)
- [OWASP — Cryptographic Storage Cheat Sheet](https://cheatsheetseries.owasp.org/cheatsheets/Cryptographic_Storage_Cheat_Sheet.html)

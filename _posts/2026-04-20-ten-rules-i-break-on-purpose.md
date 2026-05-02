---
title: Ten rules I break on purpose
tags:
  - opinions
  - dotnet
description: A contrarian checklist with receipts — where each slogan came from and why context matters.
---

These are not recommendations. They are confessions with attitude — each item names the **cultural source** of the rule so you know what you are rejecting.

1. **"Never prefix your packages with your first name."** — NuGet guidance emphasizes discoverability and prefix reservations for *trusted publishers*, not outlawing personal prefixes—but the ecosystem norm skews corporate.[^nuget-brand]

2. **"Always write the ADR before the code."** — Architecture Decision Records formalize intent before implementation spikes; the pattern was popularized to capture *why*, not block prototyping.[^adr]

3. **"Avoid global state."** — Structured programming dogma (Dijkstra-era) and functional idioms both warn against implicit mutable globals—yet every GUI toolkit eventually exposes process-wide singletons (`Application.Current`). Trade honesty about deliberate globals vs accidental ones.

4. **"One repo per concern."** — Martin Fowler's *Monolith First* essay argues microservice partitioning often premature; polyrepos vs monorepos remain tooling choices, not virtues.[^monolith-first]

5. **"Never optimize early."** — Donald Knuth's oft-misquoted line appears in his 1974 *Computing Surveys* paper on structured programming: small efficiencies can hurt readability **unless you know the critical 3%**—not "never optimize."[^knuth]

6. **"Always use the framework's blessed path."** — Framework vendors document golden paths sized for median apps; extension points exist precisely because blessed paths fail edge domains (games, compilers, protocol stacks).

7. **"Write tests first."** — Kent Beck crystallized TDD as red/green/refactor loops; empirical teams blend test-after for exploratory spikes—method books describe ideals, delivery adapts.[^tdd]

8. **"Keep posts short."** — Attention-economy blogging advice; incompatible with posts that carry citations and narrative arcs—pick your audience.

9. **"Never ship a rant."** — Engineering culture prefers sanitized communications; incident reviews prove emotionally honest narratives accelerate learning when paired with facts.

10. **"Be consistent."** — PEP 8 famously quotes *"A foolish consistency is the hobgoblin of little minds"* to defend breaking style when readability improves—consistency serves comprehension, not uniformity.[^pep8]

## The actual moral

Rules are heuristics from someone else's scars. Steal the scars, question the slogans, keep the compiler happy.

[^nuget-brand]: Microsoft Learn — *Package authoring best practices* (IDs, namespaces, consumer trust). https://learn.microsoft.com/nuget/concepts/package-authoring-best-practices

[^adr]: ADR GitHub organization — overview of Architecture Decision Records. https://adr.github.io/

[^monolith-first]: Martin Fowler — *Monolith First* (2015). https://martinfowler.com/bliki/MonolithFirst.html

[^knuth]: Donald E. Knuth — *Structured Programming with `go to` Statements*, ACM Computing Surveys 6(4), Dec 1974 (premature optimization discussion). https://doi.org/10.1145/356635.356640

[^tdd]: Kent Beck — *Test Driven Development: By Example* (Addison-Wesley). Summary reference via Martin Fowler's bliki. https://martinfowler.com/bliki/TestDrivenDevelopment.html

[^pep8]: PEP 8 — Style Guide for Python Code (consistency vs readability). https://peps.python.org/pep-0008/

## References

- [Martin Fowler — Monolith First](https://martinfowler.com/bliki/MonolithFirst.html)
- [Martin Fowler — Test-Driven Development](https://martinfowler.com/bliki/TestDrivenDevelopment.html)
- [Knuth (1974) — DOI 10.1145/356635.356640](https://doi.org/10.1145/356635.356640)
- [ADR GitHub](https://adr.github.io/)
- [PEP 8](https://peps.python.org/pep-0008/)
- [Microsoft Learn — NuGet package authoring](https://learn.microsoft.com/nuget/concepts/package-authoring-best-practices)

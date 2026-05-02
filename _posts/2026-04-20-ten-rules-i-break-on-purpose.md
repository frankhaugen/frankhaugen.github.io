---
title: Ten rules I break on purpose
tags:
  - opinions
  - dotnet
description: A contrarian checklist — not advice for your job interview, unless you want an honest one.
---

These are not recommendations. They are confessions with attitude.

1. **"Never prefix your packages with your first name."** — Already covered. Still doing it.

2. **"Always write the ADR before the code."** — Sometimes the code is the ADR, and the ADR is a Git commit message I will regret.

3. **"Avoid global state."** — I avoid *accidental* global state. Deliberate global state with a scary name is sometimes the truth of the system.

4. **"One repo per concern."** — I respect people who do this. I also have repos that are more like junk drawers with CI attached.

5. **"Never optimize early."** — I profile late, but I will absolutely choose data structures early that do not paint me into a GC corner.

6. **"Always use the framework's blessed path."** — Sometimes the blessed path is a cathedral built for a religion I do not practice.

7. **"Write tests first."** — For tricky concurrency, yes. For "glue this HTTP call to this DTO", sometimes reality is faster than TDD theater.

8. **"Keep posts short."** — Have you met me?

9. **"Never ship a rant."** — If rants did not ship, half of my best bug reports would not exist.

10. **"Be consistent."** — Consistency is a *goal*, not a personality. I would rather be readable than uniform.

## The actual moral

Rules are heuristics from someone else's scars. Steal the scars, question the slogans, keep the compiler happy.

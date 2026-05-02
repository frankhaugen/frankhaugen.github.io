---
title: Thoughts on time and date in applications
tags:
  - DateTime
  - opinions
description: Chat apps, calendars nobody agrees on, timezones, and why I reach for UNIX milliseconds when the world wants a pretty string.
---

I'm making an app with chat, so time actually matters. Most advice online boils down to "pick a format" — cool, thanks — so I wrote this to park my own opinions somewhere I can link instead of re-explaining them in PR comments.

**Fair warning:** this is not ISO-8601 fanfic. It is me being annoyed in a structured way.

### TL;DR

Time and dates are hard. Store **UTC instant** (often as UNIX milliseconds or `DateTimeOffset` with explicit offset), render **local** with explicit timezone rules, and never pretend culture-specific strings round-trip.

---

## Facts: what the standards actually say

**RFC 3339** defines a profile of ISO 8601 for timestamps on the Internet—think `2012-10-03T12:13:00Z`. It exists precisely because naive date strings caused interoperability pain.[^rfc3339]

**ISO 8601** is the broader international standard for representing dates and times; many APIs colloquially say "ISO 8601" when they mean something closer to RFC 3339.[^iso8601wiki]

**POSIX time** counts seconds since the Unix epoch (`1970-01-01T00:00:00Z`), ignoring leap seconds in the usual `time_t` encoding—so "Unix seconds" is not always identical to civil atomic clocks, but it is what most databases and APIs mean when they say `unix`.[^posix]

**.NET specifics:** `DateTimeOffset` captures an instant with an offset; plain `DateTime` can represent UTC, local, or "unspecified," and mixing those modes is a historical footgun Microsoft documents explicitly.[^dtbest] Internally .NET also exposes **ticks** (100 ns intervals since midnight `0001-01-01` UTC on the Gregorian calendar)—finer than milliseconds when you need monotonic-ish stamps inside one machine.[^ticks]

**Popular culture check:** Patrick McKenzie's *Falsehoods programmers believe about time* catalogs edge cases (DST, leap seconds, historical calendar reforms) that invalidate "just parse the string" optimism.[^falsehoods]

---

### Why is time important?

You might think that for a simple chat app, there's no need to use time, but you should always include time. It's a great way to sort, though this might be acheived by some auto-incremented number.

**Fact:** totally ordered message IDs can sort without clock sync; **practice:** you still want human-visible ordering and reconciliation when replicas diverge—timestamps (with sane collision handling) remain the default hammer.

### What DateTime format to use?

There are plenty of libraries to convert time and dates to whatever format you want, so why not just use what you use in your daily life? Let me show you:

Some examples of the same DateTime (seconds an milliseconds omitted, mostly)

DateTime code | Description | My opinion 
--- | --- | ---
2012-10-03T12:13:00Z | UTC time friendly
10-03-12 12:13PM | US/UK short AM/PM | Stupid, because counting 12 + 12 only makes sense when using 
10-03-12 12:13 | US/UK short 24H | Internaly sencible, internationally stupid
10-03-2012 12:13PM | US/UK long AM/PM | see above
03-10-12 12:13 | Short international | What I grew up using, but it's confusing
03-10-2012 12:13 | Long international | The most sencible day-to-day format
2012-10-03 12:13 | Sortable long international | Very useful for sorting
2012-40-3 12:13 | year-week-weekday international | Very useful for class schedules
2012-40-4 12:13 | year-week-weekday US | Who thought it was a good idea to start the week on Sunday?
201210031213 | sortable character safe | Great for document naming or a timestamp on a database-row
100320121213 | non-sortable character safe US | NEVER use this
1349266380 | UNIX time seconds | Somewhat useful
1349259180000 | UNIX time milliseconds | THE best way to use date and time, (see bellow)


> You might be confused by the "snowflaking" of the US in all of this, but it has it's reasons, mostly the unwillingness to risk confusion among it's citizens. However the US Military does use the the UTC time, 

You might say "hey, how about my country/culture's calendar? This is awefully eurocentric", and yes it is completely correct and a good point to make. Here's some examples of calendars from around the world that is in use today:

Date | Decription
--- | --- 
5773-Tishri-18 | Hebrew
1433-Dhu I-Qa'da-18 | Islamic
1391-Mehr-13 | Persian
1934-Asvina-12 | Indian Civil
Dragon-Dog-18 | Chinese variant 1 (not actually used)
4710-8-18 | Chinese variant 2 (China actually use Gregorian)

> there are scores upon scores of others, some only used by tribes in micronesia or in the himalayas, (that there's probably a conversion for anyway, because developers love a challange, or some social anthropologist needs it in their research

I will not denegrade the non-gregorian calanders, and they are useful for those who share them, but I don't think I'll be using Dragon-dog-18, (though if I were, I would have to have some logic to assume it's the closest Dragon in the last 12 years)

### But what about Timezones?

Short answer is: Ignore it, and use a universal time with conversion logic!

Great video about timezones every dev should watch:

<a href="http://www.youtube.com/watch?feature=player_embedded&v=-5wpm-gesOY" target="_blank" rel="noopener noreferrer"><img src="http://img.youtube.com/vi/-5wpm-gesOY/0.jpg" alt="YouTube: timezones for programmers" width="240" height="180"></a>

Tom Scott's piece on YouTube walks through why civil time is political—worth pairing with the Olson/tz database maintained as IANA Time Zone Data.[^tzdb]

## Conclusion: use millisecond UNIX time

The computer counts in this time unit, (.net actually has a unit caller "ticks" that is MUCH smaller)

**Practice recap:**

1. Persist an **unambiguous instant** (UTC / offset-aware / epoch ms).
2. Convert for display with **explicit** timezone rules (`TimeZoneInfo`, Noda Time, etc.).
3. Never treat locale-formatted strings as canonical keys—they are lossy projections.

[^rfc3339]: IETF RFC 3339 — *Date and Time on the Internet: Timestamps*. https://datatracker.ietf.org/doc/html/rfc3339

[^iso8601wiki]: Wikipedia — *ISO 8601* (overview and examples; not the paid standard text itself). https://en.wikipedia.org/wiki/ISO_8601

[^posix]: Open Group Base Specifications — epoch and `time_t` discussion in POSIX / UNIX time material. https://pubs.opengroup.org/onlinepubs/9699919799/

[^dtbest]: Microsoft Learn — *Best practices for working with DateTime and DateTimeOffset in .NET*. https://learn.microsoft.com/dotnet/fundamentals/runtime-library/system/datetime-best-practices

[^ticks]: Microsoft Learn — *DateTime.Ticks property*. https://learn.microsoft.com/dotnet/api/system.datetime.ticks

[^falsehoods]: Patrick McKenzie (Kalzumeus Software), *Falsehoods programmers believe about time*. https://infiniteundo.com/post/25326999628/falsehoods-programmers-believe-about-time

[^tzdb]: IANA — *Time Zone Database* (often packaged as `tzdata`). https://www.iana.org/time-zones

## References

- [RFC 3339](https://datatracker.ietf.org/doc/html/rfc3339)
- [Microsoft Learn — DateTime best practices](https://learn.microsoft.com/dotnet/fundamentals/runtime-library/system/datetime-best-practices)
- [Microsoft Learn — Time zones](https://learn.microsoft.com/dotnet/standard/datetime/time-zone-overview)
- [IANA Time Zone Database](https://www.iana.org/time-zones)
- [Patrick McKenzie — Falsehoods programmers believe about time](https://infiniteundo.com/post/25326999628/falsehoods-programmers-believe-about-time)
- [Tom Scott — YouTube on timezones](https://www.youtube.com/watch?v=-5wpm-gesOY)

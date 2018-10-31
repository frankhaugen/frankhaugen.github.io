# Thoughts on time and date in applications

### TL;DR
I'm working on an app requreing a form of timestamp in a chat, and what type of time to use is a big question, so I disquss it here.

### Why is time important?
You might think that for a simple chat app, there's no need to use time, but you should always include time. It's a great way to sort, though this might be acheived by some auto-incremented number.

### What DateTime format to use?
Some examples of the same DateTime (seconds an milliseconds omitted, mostly):
DateTime code | Description
--- | --- 
10-03-12 12:13AM | US/UK short AM/PM
10-03-12 12:13 | US/UK short 24H
10-03-2012 12:13 | US/UK long AM/PM
03-10-12 12:13 | 
03-10-2012 12:13 | 
2012-10-03 12:13 | 
2012-40-3 12:13 | year-week-weekday international)
2012-40-4 12:13 | year-week-weekday US)
201210031213 | sortable character safe)
100320121213 | 
1349266380 | UNIX time seconds)
1349259180000 | UNIX time milliseconds)
2012-10-03T12:13:00Z | UTC time friendly)

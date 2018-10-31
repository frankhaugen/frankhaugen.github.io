# Thoughts on time and date in applications

### TL;DR
I'm working on an app requreing a form of timestamp in a chat, and what type of time to use is a big question, so I disquss it here.

### Why is time important?
You might think that for a simple chat app, there's no need to use time, but you should always include time. It's a great way to sort, though this might be acheived by some auto-incremented number.

### What DateTime format to use?
Some examples of the same DateTime (seconds an milliseconds omitted, mostly):

DateTime code | Description | My opinion 
--- | --- | ---
10-03-12 12:13PM | US/UK short AM/PM | Stupid, because counting 12 + 12 only makes sense when using 
10-03-12 12:13 | US/UK short 24H | Internaly sencible, internationally stupid
10-03-2012 12:13PM | US/UK long AM/PM | see above
03-10-12 12:13 | Short international | What I grew up using, but it's confusing
03-10-2012 12:13 | Long international | The most sencible day-to-day format
2012-10-03 12:13 | Sortable long international | 
2012-40-3 12:13 | year-week-weekday international
2012-40-4 12:13 | year-week-weekday US
201210031213 | sortable character safe
100320121213 | non-sortable character safe US
1349266380 | UNIX time seconds
1349259180000 | UNIX time milliseconds
2012-10-03T12:13:00Z | UTC time friendly

> You might be confused by the "snowflaking" of the US in all of this, but it has it's reasons, mostly the unwillingness to risk confusion among it's citizens. However the US Military does use the the UTC time, 

You might say "hey, how about my country/culture's calendar? This is awefully eurocentric", and yes it is completely correct and a good point to make. Here's some examples of calendars:

Date | Decription
--- | --- 
5773-Tishri-18 | Hebrew
1433-Dhu I-Qa'da-18 | Islamic
1391-Mehr-13 | Persian
1934-Asvina-12 | Indian Civil
Dragon-Dog-18 | Chinese variant 1
4710-8-18 | Chinese variant 2

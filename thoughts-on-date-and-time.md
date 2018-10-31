# Thoughts on time and date in applications
I'm making an app that has a chat functionality, and so time is an important factor. I couldn't find serious discussions about this excepte, "use X format"; So i made this to gather my thoughts about it.

### TL;DR
Time and dates are hard!

### Why is time important?
You might think that for a simple chat app, there's no need to use time, but you should always include time. It's a great way to sort, though this might be acheived by some auto-incremented number.

### What DateTime format to use?
There are plenty of libraries to convert time and dates to whatever format you want, so why not just use what you use in your daily life? Let me show you:

Some examples of the same DateTime (seconds an milliseconds omitted, mostly)

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

You might say "hey, how about my country/culture's calendar? This is awefully eurocentric", and yes it is completely correct and a good point to make. Here's some examples of calendars from around the world that is in use today:

Date | Decription
--- | --- 
5773-Tishri-18 | Hebrew
1433-Dhu I-Qa'da-18 | Islamic
1391-Mehr-13 | Persian
1934-Asvina-12 | Indian Civil
Dragon-Dog-18 | Chinese variant 1
4710-8-18 | Chinese variant 2






### But what about Timezones?
Short answer is: Ignore it!

Great video about timezones every dev, should watch multiple times:

<a href="http://www.youtube.com/watch?feature=player_embedded&v=-5wpm-gesOY
" target="_blank"><img src="http://img.youtube.com/vi/-5wpm-gesOY/0.jpg" 
alt="IMAGE ALT TEXT HERE" width="240" height="180" border="10" /></a>

Advanced Dialogue for DebugLiz

&Debug1
Hi, My name is Liz.
This is a script reading test.
Answer this question...
What's your favourite snack?
@? 4
Sweets.
Chocolate.
Crisps.
Fruit.
&end

&Debug1A1
Hey mine too!
@goto Debug2
&end

&Debug1A2
Not bad.
@goto Debug2
&end

&Debug1A3
Fair enough.
@goto Debug2
&end

&Debug1A4
@goto Debug3
&end

&Debug2
Come talk to me again sometime.
@# setDialogue Debug4
&end

&Debug3
Oh really?
@? 2
Yeah!
Nah, not really.
&end

&Debug3A1
Sure, whatever.
@goto Debug2
&end

&Debug3A2
I knew it!
@goto Debug2
&end

&Debug4
Hey how's it goin' ?
&end

&Debug5
Hey!
@# setCamera hook1
See those lasers over there...
Try to jump over them.
&end
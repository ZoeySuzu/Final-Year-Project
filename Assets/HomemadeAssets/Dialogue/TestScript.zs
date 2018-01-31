Dialogue for TestScript

Test1
@# setDialogue Test4
Hi, I am a test character.
This is a script reading test.
Answer this question!
@? 2
Can you read this?
Yes.
No.
end

Test1A1
Good that means that this is working.
@# setFriendPoints 1
@>Test2
end

Test1A2
... Umm okay?!..
@# setFriendPoints -1
@>Test3
end

Test2
Thanks for your help!
end

Test3
Thanks for nothing...
end

Test4
@# setDialogue Test5
Hello again!
I need you to tell me something.
@? 2
What did you answer before?
Yes.
No.
end

Test4A1
@%> 1
You did indeed!
@>Test2
else
That's not true!
@>Test3
end

Test4A2
@%< 0
You did indeed!
@>Test2
else
That's not true!
@>Test3
end

Test5
I have nothing left to ask you.
Bye.
end

Test6
Hey!
@# setCamera hook1
See those lasers over there...
Try to jump over them.
end
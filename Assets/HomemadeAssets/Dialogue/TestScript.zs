Dialogue for TestScript

Test1
Hi, I am a test character.
This is a script reading test.
Answer this question!
@? 2
Can you read this?
Yes.
No.
@>Test1A1
@>Test1A2
end

Test1A1
Good that means that this is working.
@# setFriendPoints 1
@# setDialogue Test2
@>Test2
end

Test1A2
... Umm okay?!..
@# setFriendPoints -1
@# setDialogue Test3
@>Test3
end

Test2
Thanks for your help!
end

Test3
You're pretty useless.
end
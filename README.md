Just a small (and also my first) Algorithm to solve the Snowpeak Ruins Ice Puzzle from Zelda Twilight Princess.
I got quite upset that I needed a tutorial to solve this in my playthrough, so I wanted to code this algorithm to show my pure mental dominance.
(https://ibb.co/Y43hQvPf)

---------------------------------------------------

This taught me basic algorithmic thinking and was a VERY fun project.

The logic is nothing special: it just checks for and blocks backtracking and possible ways to move. 
Also, if one block activates the upper button, the block cant be moved anymore.

---------------------------------------------------

Took me only a few days to finish this project. 

If you look into the code and ask yourself why it is one loop with 600 lines of code: thats because of StackOverflow errors. 
When the algorithm is capped down to really low move numbers (50+ moves needed), it always threw them. 
The way I fixed it was to make the whole algorithm run in one loop without any function calls.

And it worked, so can you be mad? No! (I hope? Please :/)
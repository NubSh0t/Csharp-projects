Contains a folder named "2048 ai" which is the exact replica of the intial ai I have made in the python version.

Link to the python version: https://github.com/NubSh0t/Python-projects/blob/main/2048/2048%20ai.py

I have ported the python code in C# specifically to see if the algorithm was to perform better in strict timed scenerios in C# which is a faster language than Python and allows to run the algorithm at higher depth. The whole code is exactly the same, using just the C# language counterparts. However the difference in the performance when running the code is not definative enough just from minimal amount of running (due to the random nature of the game), so a proper test with a decent sample size is necessary to see if the faster speed of C# allows the ai to score higher in the game.

TO DO:

-Attact a paper using a decent sample size with both python and C# variants to test if the performance gets a boost with faster language .

-(Later)make a even more optimised version of 2048 ai in C# (for its comparitive fast speed while running) to run and see if it can reach the 8192 block.

Few optimisations/approaches planned are listed:

  > Using some algorithm like A* star to traverse the game graph rather than expectimax(it allows for a more open approach).

  > Using NN or a adaptive model to score the postion of board rather than hard coding the values manually(which should comparatively work out towards a better performance).

  > Implementing a 2 stage evaluation of board, where a predicted or approximate score is given to a position of the board, before being rigorously tested to give out a finalised hard score of the position(this should allow for higher depths and saving time by pruning).

  > Looking out of Transpositions while evaluating and using a hash table to store different positions. 
  

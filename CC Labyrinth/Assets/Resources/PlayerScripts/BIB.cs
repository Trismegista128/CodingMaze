using UnityEngine;
using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;

//Change only the class name
public class BIB : MonoBehaviour, IPlayerAI
{
    //We are playing in retro style so you can use max 3 characters to call your team 
    //(try using basic ones as the font used in the game may not have the crazy ones)
    //Examples: "MOM", "DAD", "E.T", "YOU" etc.
    private string TeamName = "[BIB]";

    //Chose how would you like your code to be represented on the UI.
    //You can see how each character looks like under Assets/Images/ folder
    //NOTE: In a case two teams will chose the same one we will have a chat on MS_TEAMS (or somewhere) to find the agreement.
    private CharacterType TeamCharacter = CharacterType.Vader;


    private List<DirectionType> _oldSteps = new List<DirectionType>();
    

    public DirectionType RequestMove(DirectionType[] possibleDirections)
    {
        //[Replace the throw exception thingy by your algorithm]
        if (possibleDirections == null) throw new ArgumentException();

        DirectionType direction;
        if (possibleDirections.Any(x => x == DirectionType.Left))
        {
            direction = DirectionType.Left;
        }
        else if (possibleDirections.Any(x => x == DirectionType.Down))
        {
            direction = DirectionType.Down;
        }
        else if (possibleDirections.Any(x => x == DirectionType.Right))
        {
            direction = DirectionType.Right;
        }
        else if (possibleDirections.Any(x => x == DirectionType.Up))
        {
            direction = DirectionType.Up;
        }
        else // This should not be possible but my algorithm are not tested so who knows...
        {
            direction = DirectionType.Left;
        }


        var totalSteps = _oldSteps.Count;
        var repeatCount = 20;
        if (totalSteps > repeatCount * 3 + 1)
        {
            var currentCycle = _oldSteps.GetRange(totalSteps - repeatCount, repeatCount);

            for (int i = 1; i < repeatCount*2; i++)
            {
                var oldCycle = _oldSteps.GetRange(totalSteps - repeatCount - i, repeatCount);

                if(Enumerable.SequenceEqual(currentCycle, oldCycle))
                {
                    var newDirections = possibleDirections.Where(x => x != direction).ToArray();
                    if (newDirections != null && newDirections.Length > 0)
                        return RequestMove(newDirections);
                }
            }
        }

        _oldSteps.Add(direction);
        return direction;

        /*
        Hello my friend!

            Your todays’ challenge is to go through “TheMaze” and to find a secret room with the treasure.
        Just like any other ordinary person inside a maze, you have no idea how this maze looks like, 
        how big it is and where is the treasure you want to find. However, you are not completely clueless.
            
            TheMaze tells you in which direction you can move by giving you options in possibleDirections parameter. 
        There will be always from 1 to 4 options to pick from (Left/Right/Up/Down), never more and never less. It is strongly suggested 
        that you will listen to what TheMaze says and always pick one of the possibilities otherwise you will 
        lose the game faster than you want.

            Completely blinded, guided by TheMaze you must go through unknown area with a hope that finally you will
        reach the goal and won’t get lost in the labyrinth, wandering in its corridors. Plan wisely, the problem
        may seem trivial, but is it so?

        Have fun and good luck!

    --- YOUR GOAL ---
            - Chose your "TeamName" (max 3 characters)
            - Chose your "TeamCharacter" (one from the enum values)
            - Write your ULTIMATE SUPER MIGHTY ALGORITHM to go through any maze.

    --- CODING HINTS ---

            - Picking other than provided by The Maze option will result in game over.
            - The Maze will provide possible movements only for your next step.
            - Once your step will be done, you will get new possible directions(including the one from which you’ve came to current position)
            - Instance of your class will be maintained between method calls (you can have global variables)
            - You can create more methods if you want, however only your algorithm will be using them.
            - The order of possible options within possibleDirections array is unknown and may be random (that shouldn't be important).

    --- WHEN FINISHED ---

            Once you finish, all needed from you is this class as a plain text (not .dll but >>> *.cs file). 
            Send this *.cs file to PKUB (name it somehow recognizable).


    --- MORE INFO ---

            If you need more informations and some examples, please check the "TheMazeGuide.pptx" included in this repository. */
    }
}


using System.Linq;
using UnityEngine;
using Assets.Scripts;

//Change only the class name
public class AAA : MonoBehaviour, IPlayerAI
{
    //We are playing in retro style so you can use max 3 characters to call your team 
    //(try using basic ones as the font used in the game may not have the crazy ones)
    //Examples: "MOM", "DAD", "E.T", "YOU" etc.
    private string TeamName = "AAA";

    private DirectionType previousMovement;
    private bool firstMovePlayed = false;
    private int nbRightTurnsInARow = 0;

    //Chose how would you like your code to be represented on the UI.
    //You can see how each character looks like under Assets/Images/ folder
    //NOTE: In a case two teams will chose the same one we will have a chat on MS_TEAMS (or somewhere) to find the agreement.
    private CharacterType TeamCharacter = CharacterType.Teacher;


    public DirectionType RequestMove(DirectionType[] possibleDirections)
    {
        //[Replace the throw exception thingy by your algorithm]

        // First move
        if (!firstMovePlayed)
        {
            var newMove = possibleDirections.First();
            previousMovement = newMove;
            firstMovePlayed = true;
            return newMove;
        }

        //Not first moves anymore:
        // turn right if possible
        if (possibleDirections.Contains(TurnRight(previousMovement)))
        {
            var newMove = TurnRight(previousMovement);
            // If we have turned right many times in a row it could be that we are stuck in a loop
            if (nbRightTurnsInARow > 5)
            {
                if (possibleDirections.Contains(TurnLeft(previousMovement)))
                {
                    newMove = TurnLeft(previousMovement);
                    nbRightTurnsInARow = 0;
                    previousMovement = newMove;
                    return newMove;
                }
                if (possibleDirections.Contains(previousMovement))
                {
                    newMove = previousMovement;
                    nbRightTurnsInARow = 0;
                    previousMovement = newMove;
                    return newMove;
                }
            }
            nbRightTurnsInARow++;
            previousMovement = newMove;
            return newMove;
        }

        // not possible to turn right but there was a crossroad, so we go straight
        if (possibleDirections.Length > 2 && possibleDirections.Contains(previousMovement))
        {
            var newMove = previousMovement;
            previousMovement = newMove;
            return newMove;
        }

        // not possible to turn right. Can only move in 1 axis. We continue straight
        if (possibleDirections.Length == 2 && possibleDirections.Contains(previousMovement))
        {
            var newMove = previousMovement;
            previousMovement = newMove;
            return newMove;
        }

        // Dead end. Go back (only way to go)
        if (possibleDirections.Length == 1)
        {
            var newMove = possibleDirections.First();
            nbRightTurnsInARow = 0;
            previousMovement = newMove;
            return newMove;
        }

        // Corner where we cant turn right
        if (possibleDirections.Length == 2 && !possibleDirections.Contains(TurnRight(previousMovement)) && !possibleDirections.Contains(previousMovement))
        {
            var newMove = TurnLeft(previousMovement);
            nbRightTurnsInARow = 0;
            previousMovement = newMove;
            return newMove;
        }

        // In case we missed a case, return the first available:
        return possibleDirections.First();


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

    private DirectionType TurnLeft(DirectionType previousDirection)
    {
        switch (previousDirection)
        {
            case DirectionType.Down: return DirectionType.Right;
            case DirectionType.Left: return DirectionType.Down;
            case DirectionType.Right: return DirectionType.Up;
            case DirectionType.Up: return DirectionType.Left;
            default: return DirectionType.Left;
        }
    }

    private DirectionType TurnRight(DirectionType previousDirection)
    {
        switch (previousDirection)
        {
            case DirectionType.Down: return DirectionType.Left;
            case DirectionType.Left: return DirectionType.Up;
            case DirectionType.Right: return DirectionType.Down;
            case DirectionType.Up: return DirectionType.Right;
            default: return DirectionType.Right;
        }

    }
}


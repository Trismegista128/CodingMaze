using UnityEngine;
using Assets.Scripts;
using System.Collections.Generic;
using System;

//Change only the class name
public class OneTwoThree : MonoBehaviour, IPlayerAI
{
    //We are playing in retro style so you can use max 3 characters to call your team 
    //(try using basic ones as the font used in the game may not have the crazy ones)
    //Examples: "MOM", "DAD", "E.T", "YOU" etc.
    private string TeamName = "123";

    //Chose how would you like your code to be represented on the UI.
    //You can see how each character looks like under Assets/Images/ folder
    //NOTE: In a case two teams will chose the same one we will have a chat on MS_TEAMS (or somewhere) to find the agreement.
    private CharacterType TeamCharacter = CharacterType.Vader;
    private DirectionType previousMove = DirectionType.Up;

    public DirectionType RequestMove(DirectionType[] possibleDirections)
    {
        var shiftedMove = CorrectiveMove(previousMove);
        var nextMove = ChooseNextMove(possibleDirections, shiftedMove);

        previousMove = nextMove;
        return nextMove;
    }

    private int CorrectiveMove(DirectionType previousMove)
    {
        if (previousMove == DirectionType.Up) return 0;
        if (previousMove == DirectionType.Right) return 1;
        if (previousMove == DirectionType.Down) return 2;
        if (previousMove == DirectionType.Left) return 3;
        return 0;
    }

    private DirectionType ChooseNextMove(DirectionType[] possibleDirections, int shiftedMove)
    {
        List<DirectionType> myList = new List<DirectionType>();

        foreach (var item in possibleDirections)
        {
            myList.Add(item);
        }

        if (shiftedMove == 0)
        {
            if (myList.Contains(DirectionType.Right)) return DirectionType.Right;
            if (myList.Contains(DirectionType.Up)) return DirectionType.Up;
            if (myList.Contains(DirectionType.Left)) return DirectionType.Left;
            if (myList.Contains(DirectionType.Down)) return DirectionType.Down;
        }
        if (shiftedMove == 1)
        {
            if (myList.Contains(DirectionType.Right)) return DirectionType.Down;
            if (myList.Contains(DirectionType.Up)) return DirectionType.Left;
            if (myList.Contains(DirectionType.Left)) return DirectionType.Up;
            if (myList.Contains(DirectionType.Down)) return DirectionType.Right;
        }
        if (shiftedMove == 2)
        {
            if (myList.Contains(DirectionType.Right)) return DirectionType.Left;
            if (myList.Contains(DirectionType.Up)) return DirectionType.Up;
            if (myList.Contains(DirectionType.Left)) return DirectionType.Right;
            if (myList.Contains(DirectionType.Down)) return DirectionType.Down;
        }
        if (shiftedMove == 3)
        {
            if (myList.Contains(DirectionType.Right)) return DirectionType.Up;
            if (myList.Contains(DirectionType.Up)) return DirectionType.Right;
            if (myList.Contains(DirectionType.Left)) return DirectionType.Down;
            if (myList.Contains(DirectionType.Down)) return DirectionType.Left;

        }

        return DirectionType.Right + shiftedMove % 3;
    }
}

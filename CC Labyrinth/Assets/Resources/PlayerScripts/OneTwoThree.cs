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
    private bool isFirstRun = true;

    public DirectionType RequestMove(DirectionType[] possibleDirections)
    {
        if (isFirstRun)
        {
            var firstDirection = GetFirstDirection(possibleDirections);
            isFirstRun = false;
            return firstDirection;
        }

        var shiftedMove = CorrectiveMove(previousMove);
        var preferedMove = ChooseNextMove(possibleDirections, shiftedMove);
        var nextMove = CorrectMove(possibleDirections, preferedMove);

        previousMove = nextMove;
        return nextMove;
    }

    private DirectionType GetFirstDirection(DirectionType[] possibleDirections)
    {
        var prioritizedDirectionsList = new List<DirectionType> { DirectionType.Right, DirectionType.Up, DirectionType.Left, DirectionType.Down };
        List<DirectionType> possibleDirectionsList = new List<DirectionType>();
        foreach (var item in possibleDirections)
        {
            possibleDirectionsList.Add(item);
        }

        foreach (var item in prioritizedDirectionsList)
        {
            if (possibleDirectionsList.Contains(item)) return item;
        }

        return possibleDirectionsList[new System.Random().Next(1, possibleDirectionsList.Count)];
    }

    private DirectionType CorrectMove(DirectionType[] possibleDirections, DirectionType preferedMove)
    {
        List<DirectionType> possibleDirectionsList = new List<DirectionType>();
        List<DirectionType> prioritizedDirectionsList = new List<DirectionType>();

        foreach (var item in possibleDirections)
        {
            possibleDirectionsList.Add(item);
        }

        if (preferedMove == DirectionType.Down)
        {
            prioritizedDirectionsList.Add(DirectionType.Down);
            prioritizedDirectionsList.Add(DirectionType.Right);
            prioritizedDirectionsList.Add(DirectionType.Up);
            prioritizedDirectionsList.Add(DirectionType.Left);
        }
        else if (preferedMove == DirectionType.Right)
        {
            prioritizedDirectionsList.Add(DirectionType.Right);
            prioritizedDirectionsList.Add(DirectionType.Up);
            prioritizedDirectionsList.Add(DirectionType.Left);
            prioritizedDirectionsList.Add(DirectionType.Down);
        }
        else if (preferedMove == DirectionType.Up)
        {
            prioritizedDirectionsList.Add(DirectionType.Up);
            prioritizedDirectionsList.Add(DirectionType.Left);
            prioritizedDirectionsList.Add(DirectionType.Down);
            prioritizedDirectionsList.Add(DirectionType.Right);
        }
        else if (preferedMove == DirectionType.Left)
        {
            prioritizedDirectionsList.Add(DirectionType.Left);
            prioritizedDirectionsList.Add(DirectionType.Down);
            prioritizedDirectionsList.Add(DirectionType.Right);
            prioritizedDirectionsList.Add(DirectionType.Up);
        }

        foreach (var item in prioritizedDirectionsList)
        {
            if (possibleDirectionsList.Contains(item)) return item;
        }

        return possibleDirectionsList[new System.Random().Next(1, possibleDirectionsList.Count)];
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

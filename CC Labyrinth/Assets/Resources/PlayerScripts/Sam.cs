using UnityEngine;
using Assets.Scripts;
using System.Collections.Generic;

//Change only the class name
public class Sam : MonoBehaviour, IPlayerAI
{
    //We are playing in retro style so you can use max 3 characters to call your team 
    //(try using basic ones as the font used in the game may not have the crazy ones)
    //Examples: "MOM", "DAD", "E.T", "YOU" etc.
    private string TeamName = "SAM";

    //Chose how would you like your code to be represented on the UI.
    //You can see how each character looks like under Assets/Images/ folder
    //NOTE: In a case two teams will chose the same one we will have a chat on MS_TEAMS (or somewhere) to find the agreement.
    private CharacterType TeamCharacter = CharacterType.VanDame;

    static DirectionType lastDir;
    static Stack<(List<DirectionType>, int)> crossroadStack = new Stack<(List<DirectionType>, int)>();
    static bool firstMove = true;

    static DirectionType getCounterDirection(DirectionType direction)
    {
        switch (direction)
        {
            case DirectionType.Up:
                return DirectionType.Down;
            case DirectionType.Left:
                return DirectionType.Right;
            case DirectionType.Down:
                return DirectionType.Up;
            case DirectionType.Right:
                return DirectionType.Left;
        }
        return default;
    }

    public DirectionType RequestMove(DirectionType[] possibleDirectionTypes)
    {
        List<DirectionType> possibleDirectionTypesList = new List<DirectionType>();
        foreach (DirectionType dir in possibleDirectionTypes)
            possibleDirectionTypesList.Add(dir);

        DirectionType nextDir = default;
        bool stackPushed = false;

        switch (possibleDirectionTypesList.Count)
        {
            case 2:
                if (possibleDirectionTypesList[0] == getCounterDirection(lastDir))
                    nextDir = possibleDirectionTypesList[0];
                else
                    nextDir = possibleDirectionTypesList[1];
                break;
            case 1:
                nextDir = possibleDirectionTypesList[0];
                if(crossroadStack.Count > 0)
                    crossroadStack.Pop();
                break;
            default:
                bool dirExhausted = false;
                List<DirectionType> lastCrossroad = crossroadStack.Peek().Item1;

                if (lastCrossroad.Count < crossroadStack.Peek().Item2)
                {
                    foreach (DirectionType dir in possibleDirectionTypesList)
                    {
                        dirExhausted = true;
                        if (getCounterDirection(lastDir) == dir)
                        {
                            if (firstMove == false)
                                continue;
                        }
                        if (lastCrossroad.Contains(dir) == false)
                        {
                            nextDir = dir;
                            dirExhausted = false;
                            break;
                        }
                    }
                }
                else
                    dirExhausted = true;

                if (dirExhausted == true)
                {
                    crossroadStack.Pop();
                    nextDir = possibleDirectionTypesList[Random.Range(0, possibleDirectionTypesList.Count-1)];
                    break;
                }

                lastCrossroad.Add(nextDir);
                if (stackPushed == false)
                    crossroadStack.Push((new List<DirectionType>(), possibleDirectionTypesList.Count - 1));
                break;
        }

        lastDir = nextDir;

        if (firstMove == true)
            firstMove = false;

        return nextDir;
    }
}


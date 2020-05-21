using UnityEngine;
using Assets.Scripts;
using System;

public class ThrowExceptionScript : MonoBehaviour, IPlayerAI
{
    public DirectionType RequestMove(DirectionType[] possibleDirections)
    {
        throw new ArgumentNullException();
        return possibleDirections[0];
    }
}

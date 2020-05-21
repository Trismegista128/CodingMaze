using UnityEngine;
using Assets.Scripts;

public class PaluScript : MonoBehaviour, IPlayerAI
{
    public DirectionType RequestMove(DirectionType[] possibleDirections)
    {
        return possibleDirections[1];
    }
}

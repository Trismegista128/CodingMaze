using UnityEngine;
using Assets.Scripts;
using Random = System.Random;

public class IHateMazes : MonoBehaviour, IPlayerAI
{
    private string TeamName = "IHM";
    private CharacterType TeamCharacter = CharacterType.Teacher;
    Random rnd = new Random();
    public DirectionType RequestMove(DirectionType[] possibleDirections)
    {
        return possibleDirections[rnd.Next(possibleDirections.Length)];
    }
}


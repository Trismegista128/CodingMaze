using UnityEngine;
using Assets.Scripts;

namespace Assets.Resources
{
    public class TakeFirstOption : MonoBehaviour, IPlayerAI
    {
        public DirectionType RequestMove(DirectionType[] possibleDirections)
        {
            return possibleDirections[0];
        }
    }
}

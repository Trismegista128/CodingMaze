using Assets.Scripts;
using UnityEngine;

namespace Assets.Resources.PlayerScripts
{
    public class MoveRight : MonoBehaviour, IPlayerAI
    {
        public DirectionType RequestMove(DirectionType[] direction)
        {
            return DirectionType.Right;
        }
    }
}

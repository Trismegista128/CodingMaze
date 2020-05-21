using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class PlayerAI: MonoBehaviour, IPlayerAI
    {
        public string MyName = "ABC";

        private int counter = -1;
        private List<DirectionType> order = new List<DirectionType> {
            DirectionType.Down,
            DirectionType.Up,
            DirectionType.Up,
            DirectionType.Up,
            DirectionType.Up,
            DirectionType.Down,
            DirectionType.Down,
            DirectionType.Down,
            DirectionType.Down,
            DirectionType.Down,
            DirectionType.Right,
            DirectionType.Right,
            DirectionType.Right,
            DirectionType.Up,
            DirectionType.Up,
        };

        public DirectionType RequestMove(DirectionType[] possibleDirections)
        {
            counter++;
            return order[counter];
        }
    }
}

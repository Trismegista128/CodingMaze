using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class PlayerAI : MonoBehaviour
    {
        public string MyName = "ABC";

        private int counter = -1;
        private List<Direction> order = new List<Direction> {
            Direction.Down,
            Direction.Up,
            Direction.Up,
            Direction.Up,
            Direction.Up,
            Direction.Down,
            Direction.Down,
            Direction.Down,
            Direction.Down,
            Direction.Down,
            Direction.Right,
            Direction.Right,
            Direction.Right,
            Direction.Up,
            Direction.Up,
        };

        public Direction RequestMove(Direction[] possibleDirections)
        {
            counter++;
            return order[counter];
        }
    }
}

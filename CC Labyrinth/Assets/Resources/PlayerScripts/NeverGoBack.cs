using UnityEngine;
using Assets.Scripts;
using System.Linq;

namespace Assets.Resources
{
    public class NeverGoBack : MonoBehaviour, IPlayerAI
    {
        private DirectionType lastMovement;
        private bool isFirstCall = true;

        public DirectionType RequestMove(DirectionType[] possibleDirections)
        {
            if (isFirstCall)
            {
                isFirstCall = false;
                lastMovement = possibleDirections[0];
                return lastMovement;
            }

            if (CanContinue(possibleDirections))
                return lastMovement;

            if (IfMustReturn(possibleDirections))
            {
                lastMovement = possibleDirections.First();
                return lastMovement;
            }

            var goBackDirection = GetOpositeDirection(lastMovement);
            var possibilities = possibleDirections.Where(x => x != goBackDirection).ToList();

            lastMovement = possibilities.First();
            return lastMovement;
        }

        private bool CanContinue(DirectionType[] possibleDirections)
        {
            return possibleDirections.Any(x => x == lastMovement);
        }

        private bool IfMustReturn(DirectionType[] possibleDirections)
        {
            return possibleDirections.Count() == 1 && possibleDirections.First() == GetOpositeDirection(lastMovement);
        }

        private DirectionType GetOpositeDirection(DirectionType direction)
        {
            switch (direction)
            {
                case DirectionType.Down:
                    return DirectionType.Up;
                case DirectionType.Left:
                    return DirectionType.Right;
                case DirectionType.Up:
                    return DirectionType.Down;
                default:
                    return DirectionType.Left;
            }
        }
    }
}
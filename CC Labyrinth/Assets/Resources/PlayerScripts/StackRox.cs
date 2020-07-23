using UnityEngine;
using Assets.Scripts;
using System.Linq;
using System.Collections.Generic;
using Random = System.Random;

namespace Assets.Resources
{
    public class StackRox : MonoBehaviour, IPlayerAI
    {
        public enum DirectionCase
        {
            Enterance,
            Used,
            NotUsed,
            Invalid
        }

        public class FieldInfo
        {
            public DirectionCase Left;
            public DirectionCase Right;
            public DirectionCase Up;
            public DirectionCase Down;

            public FieldInfo Clone()
            {
                var newInfo = new FieldInfo();
                newInfo.Left = this.Left;
                newInfo.Right = this.Right;
                newInfo.Up = this.Up;
                newInfo.Down = this.Down;

                return newInfo;
            }
        }

        Random rnd = new Random();

        private bool isFirstMovement = true;

        private List<FieldInfo> MovementStack;
        private DirectionType? lastMovement = null;
        private bool returned = false;

        public DirectionType RequestMove(DirectionType[] possibleDirections)
        {
            var currentField = new FieldInfo();
            if (isFirstMovement)
            {
                MovementStack = new List<FieldInfo>();
                isFirstMovement = false;
                returned = false;
                lastMovement = null;
            }

            if (returned)
            {
                var stackField = MovementStack.Last();
                var clonedField = stackField.Clone();

                MovementStack.RemoveAt(MovementStack.Count() - 1);

                //We returned so we do not want to re-enter this location anymore.
                currentField = MarkDirectionAs(DirectionCase.Invalid, clonedField, lastMovement.Value);
            }
            else
            {
                currentField = CreateField(possibleDirections);

                if (lastMovement.HasValue)
                    //We moved forward so we want to keep track on what was the enterance.
                    currentField = MarkDirectionAs(DirectionCase.Enterance, currentField, lastMovement.Value);
            }

            //We are searching for not used directions, at this point we should not have "enterances" selected.
            var chosenDirection = SelectRandomDirection(currentField);

            if(chosenDirection.HasValue)
            {
                returned = false;
                MovementStack.Add(currentField);
            }
            else
            {
                returned = true;
                chosenDirection = SelectReturn(currentField);
            }

            lastMovement = chosenDirection;
            return chosenDirection.Value;
        }

        private FieldInfo CreateField(DirectionType[] possibleDirections)
        {
            var fieldInfo = new FieldInfo();

            fieldInfo.Down = possibleDirections.Contains(DirectionType.Down) ? DirectionCase.NotUsed : DirectionCase.Invalid;
            fieldInfo.Left = possibleDirections.Contains(DirectionType.Left) ? DirectionCase.NotUsed : DirectionCase.Invalid;
            fieldInfo.Right = possibleDirections.Contains(DirectionType.Right) ? DirectionCase.NotUsed : DirectionCase.Invalid;
            fieldInfo.Up = possibleDirections.Contains(DirectionType.Up) ? DirectionCase.NotUsed : DirectionCase.Invalid;

            return fieldInfo;
        }

        private DirectionType? SelectRandomDirection(FieldInfo info)
        {
            var directions = new List<DirectionType>();

            if (info.Left == DirectionCase.NotUsed) directions.Add(DirectionType.Left);
            if (info.Right == DirectionCase.NotUsed) directions.Add(DirectionType.Right);
            if (info.Up == DirectionCase.NotUsed) directions.Add(DirectionType.Up);
            if (info.Down == DirectionCase.NotUsed) directions.Add(DirectionType.Down);

            if(directions.Any())
            {
                var index = rnd.Next(directions.Count());
                return directions[index];
            }

            return null;
        }

        private FieldInfo MarkDirectionAs(DirectionCase directionCase, FieldInfo info, DirectionType direction)
        {
            switch (direction)
            {
                case DirectionType.Down:
                    info.Up = directionCase;
                    break;
                case DirectionType.Left:
                    info.Right = directionCase;
                    break;
                case DirectionType.Right:
                    info.Left = directionCase;
                    break;
                case DirectionType.Up:
                    info.Down = directionCase;
                    break;
            }
            return info;
        }

        private DirectionType SelectReturn(FieldInfo info)
        {
            if (info.Left == DirectionCase.Enterance) return DirectionType.Left;
            if (info.Right == DirectionCase.Enterance) return DirectionType.Right;
            if (info.Up == DirectionCase.Enterance) return DirectionType.Up;

            return DirectionType.Down;
        }
    }
}

using UnityEngine;
using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using Random = System.Random;

//Change only the class name
public class BIB : MonoBehaviour, IPlayerAI
{
    private string TeamName = "[BIB]";
    private CharacterType TeamCharacter = CharacterType.Elvis;

    private char[,] maze = new char[200, 200];
    private Point position = new Point(100, 100);
    private DirectionType lastMove;
    private readonly Random random = new Random();

    public DirectionType RequestMove(DirectionType[] possibleDirections)
    {
        var newMove = GetUnexploredDirection(possibleDirections);
        if (newMove == null)
            newMove = GetDirectionNoBack(possibleDirections);

        UpdateMaze(possibleDirections);
        UpdatePosition(newMove.Value);
        return lastMove = newMove.Value;
    }

    private void UpdateMaze(DirectionType[] possibleDirections)
    {
        maze[position.Y, position.X] = '-';

        if (!possibleDirections.Any(x => x == DirectionType.Left))
            maze[position.Y, position.X - 1] = '#';
        if (!possibleDirections.Any(x => x == DirectionType.Up))
            maze[position.Y - 1, position.X] = '#';
        if (!possibleDirections.Any(x => x == DirectionType.Right))
            maze[position.Y, position.X + 1] = '#';
        if (!possibleDirections.Any(x => x == DirectionType.Down))
            maze[position.Y + 1, position.X] = '#';
    }

    private void UpdatePosition(DirectionType directions)
    {
        switch (directions)
        {
            case DirectionType.Left:
                position.X--;
                break;
            case DirectionType.Up:
                position.Y--;
                break;
            case DirectionType.Right:
                position.X++;
                break;
            case DirectionType.Down:
                position.Y++;
                break;
            default:
                return;
        }
    }

    private DirectionType GetDirectionNoBack(DirectionType[] possibleDirections)
    {
        var newMove = GetRandomDirection(possibleDirections);

        if (possibleDirections.Length > 1)
        {
            var oppositeDirection = GetOppositeDirection(newMove);
            if (oppositeDirection == lastMove)
            {
                var newList = possibleDirections.ToList();
                newList.Remove(newMove);
                return GetRandomDirection(newList.ToArray());
            }
        }
        return newMove;
    }

    private DirectionType GetRandomDirection(DirectionType[] possibleDirections) => possibleDirections[random.Next(possibleDirections.Length)];

    private static DirectionType GetOppositeDirection(DirectionType direction)
    {
        switch (direction)
        {
            case DirectionType.Left:
                return DirectionType.Right;
            case DirectionType.Up:
                return DirectionType.Down;
            case DirectionType.Right:
                return DirectionType.Left;
            case DirectionType.Down:
                return DirectionType.Up;
            default:
                throw new InvalidEnumArgumentException();
        }
    }

    private DirectionType? GetUnexploredDirection(DirectionType[] possibleDirections)
    {
        if (possibleDirections.Any(x => x == DirectionType.Left) && maze[position.Y, position.X - 1] != '-')
            return DirectionType.Left;
        if (possibleDirections.Any(x => x == DirectionType.Down) && maze[position.Y + 1, position.X] != '-')
            return DirectionType.Down;
        if (possibleDirections.Any(x => x == DirectionType.Right) && maze[position.Y, position.X + 1] != '-')
            return DirectionType.Right;
        if (possibleDirections.Any(x => x == DirectionType.Up) && maze[position.Y - 1, position.X] != '-')
            return DirectionType.Up;
        return null;
    }
}
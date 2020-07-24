using UnityEngine;
using Assets.Scripts;
using System.Collections.Generic;
using System;
using System.Linq;

public static class QueueExtensions
{
    public static void Enqueue<T>(this Queue<T> queue, T[] items)
    {
        foreach (var item in items)
            queue.Enqueue(item);
    }
}

public static class DirectionTypeExtensions
{
    public static DirectionType Reverse(this DirectionType dir)
    {
        switch (dir)
        {
            case DirectionType.Right:
                return DirectionType.Left;
            case DirectionType.Up:
                return DirectionType.Down;
            case DirectionType.Left:
                return DirectionType.Right;
            case DirectionType.Down:
                return DirectionType.Up;
            default:
                throw new ArgumentException();
        }
    }
}

public class Vertex
{
    public int X { get; }
    public int Y { get; }
    public bool Explored { get; set; }

    public Vertex(int x, int y)
    {
        X = x;
        Y = y;
        Explored = false;
    }

    public static bool operator ==(Vertex v, Vertex w)
    {
        if (v is null && w is null) 
            return false;
        return v.X == w.X && v.Y == w.Y;
    }

    public static bool operator !=(Vertex v, Vertex w)
    {
        if (v is null || w is null)
            return true;
        return v.X != w.Y || v.Y != w.Y;
    }

    public override bool Equals(object obj)
    {
        return obj is Vertex vertex &&
               X == vertex.X &&
               Y == vertex.Y;
    }

    public override int GetHashCode()
    {
        var hashCode = 1861411795;
        hashCode = hashCode * -1521134295 + X.GetHashCode();
        hashCode = hashCode * -1521134295 + Y.GetHashCode();
        return hashCode;
    }
}

public static class VertexExtensions
{
    public static Vertex Translate(this Vertex v, DirectionType dir)
    {
        switch (dir)
        {
            case DirectionType.Right:
                return new Vertex(v.X + 1, v.Y);
            case DirectionType.Up:
                return new Vertex(v.X, v.Y - 1);
            case DirectionType.Left:
                return new Vertex(v.X - 1, v.Y);
            case DirectionType.Down:
                return new Vertex(v.X, v.Y + 1);
            default:
                throw new ArgumentException();
        }
    }
}

public class Edge
{
    public Vertex Start { get; }
    public Vertex End { get; }
    public DirectionType Weight { get; }

    public Edge(Vertex start, Vertex end, DirectionType weight)
    {
        Start = start;
        End = end;
        Weight = weight;
    }

    public static bool operator ==(Edge e1, Edge e2)
    {
        return e1.Start == e2.Start && e1.End == e2.End;
    }

    public static bool operator !=(Edge e1, Edge e2)
    {
        return e1.Start == e2.Start && e1.End == e2.End;
    }

    public override bool Equals(object obj)
    {
        return obj is Edge edge &&
               Start ==  edge.Start &&
               End == edge.End &&
               Weight == edge.Weight;
    }

    public override int GetHashCode()
    {
        var hashCode = 937802146;
        hashCode = hashCode * -1521134295 + Start.GetHashCode();
        hashCode = hashCode * -1521134295 + End.GetHashCode();
        hashCode = hashCode * -1521134295 + Weight.GetHashCode();
        return hashCode;
    }
}

public class Graph
{
    private Dictionary<Vertex, List<Vertex>> adjacencyList;
    private List<Edge> edgesList;

    public Graph()
    {
        adjacencyList = new Dictionary<Vertex, List<Vertex>>();
        edgesList = new List<Edge>();
    }

    public void addEdge(Edge edge)
    {
        if (!edgesList.Contains(edge))
        {
            bool containsEnd = Contains(edge.End);
            if (!containsEnd)
                adjacencyList.Add(edge.End, new List<Vertex>() { });
            if (!Contains(edge.Start))
                adjacencyList.Add(edge.Start, new List<Vertex>() { edge.End });
            else
            {
                Vertex end;
                if (containsEnd)
                    end = adjacencyList.Keys.First(x => x == edge.End);
                else
                    end = edge.End;
                adjacencyList[edge.Start].Add(end);
            }
            edgesList.Add(edge);
        }
    }

    public bool Contains(Vertex v)
    {
        return adjacencyList.ContainsKey(v);
    }

    /// <summary>
    /// Returns a collection of vertices adjacent to the input vertex
    /// </summary>
    /// <returns></returns>
    public IList<Vertex> adj(Vertex v)
    {
        if (Contains(v))
            return adjacencyList[v];
        else
            return null;
    }
}

public static class GraphExtensions
{
    /// <summary>
    /// Checks if all vertices adjacent to the input one have been explored
    /// </summary>
    /// <param name="g"></param>
    /// <param name="v"></param>
    /// <returns></returns>
    public static bool adjExplored(this Graph g, Vertex v)
    {
        if (!v.Explored)
            return false;

        var adj = g.adj(v);
        return adj.All(x => x.Explored);
    }
}

public class Player
{
    private Graph mazeGraph;
    private List<KeyValuePair<Vertex, List<DirectionType>>> crossroads;
    private Vertex currentPosition;

    public Player()
    {
        mazeGraph = new Graph();
        crossroads = new List<KeyValuePair<Vertex, List<DirectionType>>>();
        currentPosition = new Vertex(0, 0);
    }

    public void Move(DirectionType dir)
    {
        currentPosition = getAdjacentPosition(dir);
    }

    public void Observe(DirectionType[] possibleDirections)
    {
        if(!currentPosition.Explored)
        {
            currentPosition.Explored = true;
            foreach (var dir in possibleDirections)
            {
                var edge = new Edge(currentPosition, getAdjacentPosition(dir), dir);
                mazeGraph.addEdge(edge);
            }
            if (possibleDirections.Length > 1)
            {
                var crossroad = new KeyValuePair<Vertex, List<DirectionType>>(currentPosition, new List<DirectionType>());
                crossroads.Add(crossroad);
            }
        }
    }
    
    /// <summary>
    /// Returns array of successive moves to perform
    /// </summary>
    /// <returns></returns>
    public DirectionType[] ComputeNextMoves()
    {
        var adjacentPositions = mazeGraph.adj(currentPosition);
        var dest = adjacentPositions.FirstOrDefault(x => !x.Explored);
        if(!(dest is null))
        {
            // compute direction from currentPosition to dest
            var deltaX = dest.X - currentPosition.X;
            var deltaY = dest.Y - currentPosition.Y;
            DirectionType direction = deltaX == 0 ?
                                        (deltaY > 0 ? DirectionType.Down : DirectionType.Up) :
                                        (deltaX > 0 ? DirectionType.Right : DirectionType.Left);
            if (crossroads.Count != 0)
            {
                // update path to last crossroad
                var lastCrossroad = crossroads.Last();
                lastCrossroad.Value.Add(direction.Reverse());
            }
            return new DirectionType[] { direction };
        }
        else
        {
            // backtrack to last unexplored crossroads
            KeyValuePair<Vertex, List<DirectionType>> lastUnexploredCrossroad;
            int index = crossroads.Count;
            do
            {
                if (index == 0)
                    return new DirectionType[] { };
                index--;
                lastUnexploredCrossroad = crossroads[index];
            }
            while (mazeGraph.adjExplored(lastUnexploredCrossroad.Key) == true);

            var nextMoves = new List<DirectionType>();
            // string paths to previous crossroads together
            for(int i = crossroads.Count - 1; i >= index; i--)
            {
                crossroads[i].Value.Reverse();
                nextMoves.AddRange(crossroads[i].Value);
                crossroads[i].Value.Clear();
            }
            // remove already explored paths
            for (int i = crossroads.Count - 1; i > index; i--)
                crossroads.RemoveAt(i);
            return nextMoves.ToArray();
        }
    }

    private Vertex getAdjacentPosition(DirectionType dir)
    {
        var newVertex = currentPosition.Translate(dir);
        var existingVertex = mazeGraph.adj(currentPosition)?.FirstOrDefault(x => x == newVertex);
        if (existingVertex is null)
            return newVertex;
        else
            return existingVertex;
    }
}

//Change only the class name
public class TheExplorer : MonoBehaviour, IPlayerAI
{
    //We are playing in retro style so you can use max 3 characters to call your team 
    //(try using basic ones as the font used in the game may not have the crazy ones)
    //Examples: "MOM", "DAD", "E.T", "YOU" etc.
    private string TeamName = "EXP";

    //Chose how would you like your code to be represented on the UI.
    //You can see how each character looks like under Assets/Images/ folder
    //NOTE: In a case two teams will chose the same one we will have a chat on MS_TEAMS (or somewhere) to find the agreement.
    private CharacterType TeamCharacter = CharacterType.Scout;

    private Player player = new Player();
    private Queue<DirectionType> nextMoves = new Queue<DirectionType>();
    private System.Random rnd = new System.Random();


    public DirectionType RequestMove(DirectionType[] possibleDirections)
    {
        //[Replace the throw exception thingy by your algorithm]
        player.Observe(possibleDirections);

        if (nextMoves.Count == 0)
            nextMoves.Enqueue(player.ComputeNextMoves());

        if (nextMoves.Count != 0)
        {
            var nextMove = nextMoves.Dequeue();
            player.Move(nextMove);
            return nextMove;
        }
        else
            return possibleDirections[rnd.Next(possibleDirections.Length)]; // tribute to LMOC

        /*
        Hello my friend!

            Your todays’ challenge is to go through “TheMaze” and to find a secret room with the treasure.
        Just like any other ordinary person inside a maze, you have no idea how this maze looks like, 
        how big it is and where is the treasure you want to find. However, you are not completely clueless.
            
            TheMaze tells you in which direction you can move by giving you options in possibleDirections parameter. 
        There will be always from 1 to 4 options to pick from (Left/Right/Up/Down), never more and never less. It is strongly suggested 
        that you will listen to what TheMaze says and always pick one of the possibilities otherwise you will 
        lose the game faster than you want.

            Completely blinded, guided by TheMaze you must go through unknown area with a hope that finally you will
        reach the goal and won’t get lost in the labyrinth, wandering in its corridors. Plan wisely, the problem
        may seem trivial, but is it so?

        Have fun and good luck!

    --- YOUR GOAL ---
            - Chose your "TeamName" (max 3 characters)
            - Chose your "TeamCharacter" (one from the enum values)
            - Write your ULTIMATE SUPER MIGHTY ALGORITHM to go through any maze.

    --- CODING HINTS ---

            - Picking other than provided by The Maze option will result in game over.
            - The Maze will provide possible movements only for your next step.
            - Once your step will be done, you will get new possible directions(including the one from which you’ve came to current position)
            - Instance of your class will be maintained between method calls (you can have global variables)
            - You can create more methods if you want, however only your algorithm will be using them.
            - The order of possible options within possibleDirections array is unknown and may be random (that shouldn't be important).

    --- WHEN FINISHED ---

            Once you finish, all needed from you is this class as a plain text (not .dll but >>> *.cs file). 
            Send this *.cs file to PKUB (name it somehow recognizable).


    --- MORE INFO ---

            If you need more informations and some examples, please check the "TheMazeGuide.pptx" included in this repository. */
    }
}


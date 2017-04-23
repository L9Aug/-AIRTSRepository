using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ASImplementation<T>
{
    public delegate float Heuristic(T from, T to);
    public delegate void PathReceive(List<AStarInfo<T>> path);
    public AStarInfo<T> start;
    public AStarInfo<T> Destination;
    public List<AStarInfo<T>> open = new List<AStarInfo<T>>();
    public List<AStarInfo<T>> closed = new List<AStarInfo<T>>();

    public List<AStarInfo<T>> AStar(AStarInfo<T> start, AStarInfo<T> destination, Heuristic heuristic)
    {
        Destination = destination;
        List<AStarInfo<T>> temp = new List<AStarInfo<T>>();
        bool destinationFound = false;
        bool noValidPath = false;
        open.Add(start);

        AddConnectionsToOpen(start, heuristic);

        while (!destinationFound && !noValidPath)
        {
            AStarInfo<T> current = GetLowestETC();
            if (current != null && current != destination)
            {
                AddConnectionsToOpen(current, heuristic);
            }
            else if (current == destination)
            {
                destinationFound = true;
            }
            else
            {
                noValidPath = true;
            }
        }

        if (destinationFound)
        {
            AStarInfo<T> tempDestination = destination;
            while (tempDestination != start && tempDestination != null)
            {
                temp.Add(tempDestination);
                //tempTile.SetColour(Color.magenta);
                tempDestination = (AStarInfo<T>)tempDestination.root;
            }

            temp.Reverse();
        }

        return temp;
    }

    public IEnumerator ASTAR(AStarInfo<T> start, AStarInfo<T> destination, Heuristic heuristic, PathReceive path)
    {
        List<AStarInfo<T>> temp = new List<AStarInfo<T>>();
        bool destinationFound = false;
        bool noValidPath = false;
        open.Add(start);

        AddConnectionsToOpen(start, heuristic);

        while (!destinationFound && !noValidPath)
        {
            yield return null;
            AStarInfo<T> current = GetLowestETC();
            if (current != null && current != destination)
            {
                AddConnectionsToOpen(current, heuristic);
            }
            else if (current == destination)
            {
                destinationFound = true;
            }
            else
            {
                noValidPath = true;
            }
        }

        if (destinationFound)
        {
            AStarInfo<T> tempDestination = destination;
            while (tempDestination != start && tempDestination != null)
            {
                temp.Add(tempDestination);
                //tempTile.SetColour(Color.magenta);
                tempDestination = (AStarInfo<T>)tempDestination.root;
            }

            temp.Reverse();
        }
        if(path != null)
        path(temp);
        //return temp;
    }

    public List<AStarInfo<T>> AStar(Heuristic heuristic)
    {
        List<AStarInfo<T>> temp = new List<AStarInfo<T>>();
        bool destinationFound = false;
        bool noValidPath = false;
        open.Add(start);

        AddConnectionsToOpen(start, heuristic);

        while (!destinationFound && !noValidPath)
        {
            AStarInfo<T> T = GetLowestETC();
            if (T != null && T != Destination)
            {
                AddConnectionsToOpen(T, heuristic);
            }
            else if (T == Destination)
            {
                destinationFound = true;
            }
            else
            {
                noValidPath = true;
            }
        }

        if (destinationFound)
        {
            AStarInfo<T> tempTile = Destination;
            while (tempTile != start && tempTile != null)
            {
                temp.Add(tempTile);
                //tempTile.SetColour(Color.magenta);
                tempTile = (AStarInfo<T>)tempTile.root;
            }

            temp.Reverse();
        }

        return temp;
    }

    AStarInfo<T> GetLowestETC()
    {
        if (open.Count > 0)
        {
            AStarInfo<T> tempTile = open[0];
            float lowestCost = tempTile.ETC;
            if (open.Count > 1)
            {
                for (int i = 1; i < open.Count; ++i)
                {
                    if (open[i].ETC < lowestCost)
                    {
                        lowestCost = open[i].ETC;
                        tempTile = open[i];
                    }
                }
            }
            return tempTile;
        }
        return null;
    }

    void AddConnectionsToOpen(AStarInfo<T> obj, Heuristic heuristic)
    {

        if (obj != null)
        {
            foreach (AStarInfo<T> c in obj.Connections)
            {
                //If the tested tile has connections it is not impassable
                if (c.Connections.Count > 0)
                {
                    if (!open.Contains(c) && !closed.Contains(c))
                    {
                        open.Add(c);
                        c.costSoFar = obj.costSoFar + (c.cost / 2f) + (obj.cost / 2f);
                        c.heuristic = heuristic(c.current, Destination.current);
                        c.ETC = c.costSoFar + c.heuristic;
                        c.root = obj;
                    }
                    else if (c.costSoFar > obj.costSoFar + (c.cost / 2f) + (obj.cost / 2f))
                    {
                        c.costSoFar = obj.costSoFar + (c.cost / 2f) + (obj.cost / 2f);
                        c.heuristic = heuristic(c.current, Destination.current);
                        c.ETC = c.costSoFar + c.heuristic;
                        c.root = obj;
                        if (closed.Contains(c))
                        {
                            closed.Remove(c);
                            open.Add(c);
                        }
                    }
                }
            }
            open.Remove(obj);
            closed.Add(obj);
        }
    }

}

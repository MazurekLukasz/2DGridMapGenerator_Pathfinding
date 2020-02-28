using System.Collections.Generic;
using UnityEngine;

public static class Algorithms
{

    public static List<Node> AStar(GridMap Grid)
    {
        List<Node> Open = new List<Node>();
        List<Node> Closed = new List<Node>();
        InitNodes(Grid);


        Open.Add(Grid.StartNode);
        Node Current = Grid.StartNode;

        Current.GCost = 0;
        Current.HCost = CalculateDistance(Grid.StartNode, Grid.EndNode, Grid);
        Current.CalculateFCost();

        // loop
        while (Open.Count > 0)
        {
            Current = NodeWithLowestFCost(Open);

            if (Current == Grid.EndNode)
            {
                return SetPath(Grid.EndNode, Grid);
            }// path has been found

            Open.Remove(Current);
            Closed.Add(Current);

            // determine the neighbours
            foreach (Node neighbour in GetNeighbourhood(Current, Grid))
            {
                if (Closed.Contains(neighbour))
                {
                    continue;
                }

                if (neighbour.Occupied)
                {
                    Closed.Add(neighbour);
                    continue;
                }

                int tmpCost = Current.GCost + CalculateDistance(Current, neighbour, Grid);
                if (tmpCost < neighbour.GCost)
                {
                    neighbour.ParentNode = Current;
                    neighbour.GCost = tmpCost;
                    neighbour.HCost = CalculateDistance(neighbour, Grid.EndNode, Grid);
                    neighbour.CalculateFCost();

                    if (!Open.Contains(neighbour))
                    {
                        Open.Add(neighbour);
                    }
                }
            }
        }
        return null;
    }

    public static List<Node> Dijkstra(GridMap grid)
    {
        InitNodes(grid); // distance from start node for all nodes = max
        grid.StartNode.GCost = 0;

        List<Node> Q = MapToNodeList(grid);
        while (Q.Count > 0)
        {
            Node u = NodeWithLowestGCost(Q);
            if (u == grid.EndNode)
            {
                return SetPath(grid.EndNode, grid);
            }// path has been found

            Q.Remove(u);

            foreach (var neighbour in GetNeighbourhood(u, grid))
            {
                if (neighbour.Occupied)
                {
                    Q.Remove(neighbour);
                    continue;
                }
                if ((u.GCost + 1) < neighbour.GCost)
                {
                    neighbour.GCost = u.GCost + 1;
                    neighbour.ParentNode = u;
                }
            }
        }
        return null;
    }




    private static List<Node> MapToNodeList(GridMap g)
    {
        List<Node> list = new List<Node>();
        foreach (var item in g.GetGridMap())
        {
            list.Add(item.GetComponent<Node>());
        }
        return list;
    }

    private static Node NodeWithLowestFCost(List<Node> list)
    {
        Node lowest = list[0];

        foreach (Node node in list)
        {
            if (node.FCost < lowest.FCost)
            {
                lowest = node;
            }
        }
        return lowest;
    }

    private static Node NodeWithLowestGCost(List<Node> list)
    {
        Node lowest = list[0];

        foreach (Node node in list)
        {
            if (node.GCost < lowest.GCost)
            {
                lowest = node;
            }
        }
        return lowest;
    }

    private static int CalculateDistance(Node a, Node b,GridMap Grid)
    {
        int xDist = Mathf.Abs(a.x - b.x);
        int yDist = Mathf.Abs(a.y - b.y);
        return xDist + yDist;
    }

    private static List<Node> SetPath(Node endNode,GridMap Grid)
    {
        List<Node> path = new List<Node>();
        path.Add(Grid.EndNode);

        Node current = Grid.EndNode;

        while (current.ParentNode != null)
        {
            path.Add(current.ParentNode);
            current = current.ParentNode;
        }
        path.Reverse();
        return path;
    }

    private static List<Node> GetNeighbourhood(Node current, GridMap Grid)
    {
        List<Node> neighbours = new List<Node>();
        // Right
        if (current.x + 1 < Grid.MapSize)
        {
            neighbours.Add(Grid.ReturnNode(current.x + 1, current.y));
        }
        // Left
        if (current.x - 1 >= 0)
        {
            neighbours.Add(Grid.ReturnNode(current.x - 1, current.y));
        }
        // Up
        if (current.y + 1 < Grid.MapSize)
        {
            neighbours.Add(Grid.ReturnNode(current.x, current.y + 1));
        }
        //down
        if (current.y - 1 >= 0)
        {
            neighbours.Add(Grid.ReturnNode(current.x, current.y - 1));
        }

        //foreach (var item in neighbours)
        //{
        //    if (!open.Contains(item) && !closed.Contains(item))
        //    {
        //        item.GCost = int.MaxValue;
        //        item.CalculateFCost();
        //    }
        //}

        return neighbours;
    }

    private static void InitNodes(GridMap Grid)
    {
        for (int x = 0; x < Grid.MapSize; x++)
        {
            for (int y = 0; y < Grid.MapSize; y++)
            {
                Grid.ReturnNode(x, y).GCost = int.MaxValue;
                Grid.ReturnNode(x, y).CalculateFCost();
                //Grid.ReturnNode(x, y).ParentNode = null;
            }
        }
    }
}

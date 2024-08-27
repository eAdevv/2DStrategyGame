using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoSingleton<PathFinder>
{

    // A* Algorithm
    public List<Node> FindPath(Vector2Int start, Vector2Int end)
    {
        Node startNode = GridManager.Instance.GetGridCellByPosition(start);
        Node endNode = GridManager.Instance.GetGridCellByPosition((end));


        if (startNode == null || endNode == null || endNode.IsUsed)
        {
            return null;
        }

        startNode.IsUsed = false;

        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];

            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].FCost < currentNode.FCost || (openList[i].FCost == currentNode.FCost && openList[i].HCost < currentNode.HCost))
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == endNode)
            {
                return RetracePath(startNode, endNode);
            }


            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (neighbor.IsUsed || closedList.Contains(neighbor))
                {
                    continue;
                }

                int newMovementCostToNeighbor = currentNode.GCost + GetDistance(currentNode, neighbor);

                if (newMovementCostToNeighbor < neighbor.GCost || !openList.Contains(neighbor))
                {
                    neighbor.GCost = newMovementCostToNeighbor;
                    neighbor.HCost = GetDistance(neighbor, endNode);
                    neighbor.Parent = currentNode;

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }

    private List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        Vector2Int[] directions = {
            new Vector2Int(0, 1),  // Up
            new Vector2Int(1, 0),  // Right
            new Vector2Int(0, -1), // Down
            new Vector2Int(-1, 0)  // Left
        };

        foreach (Vector2Int direction in directions)
        {
            Node neighbor = GridManager.Instance.GetGridCellByPosition(node.Position + direction);
            if (neighbor != null)
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    private int GetDistance(Node a, Node b)
    {
        int distanceX = Mathf.Abs(a.Position.x - b.Position.x);
        int distanceY = Mathf.Abs(a.Position.y - b.Position.y);
        return distanceX + distanceY;
    }

    private List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        path.Reverse();
        return path;
    }
}

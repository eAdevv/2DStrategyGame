using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PathFinder : MonoSingleton<PathFinder>
{

    // A* Algorithm
    public List<Node> FindPath(Vector2Int start, Vector2Int end)
    {
        // We must take start and target node for find shortest Path.
        Node startNode = GridManager.Instance.GetGridCellByPosition(start);
        Node endNode = GridManager.Instance.GetGridCellByPosition((end));

        startNode.IsUsed = false;

        // If the final cell is busy, take nearest available cell.
        if (endNode.IsUsed)
        {
            endNode = FindNearestAvailableNode(endNode);
        }

        if (startNode == null || endNode == null || endNode.IsUsed)
        {
            return null;
        }

        List<Node> openList = new List<Node>(); // Nodes to find
        HashSet<Node> closedList = new HashSet<Node>(); // Processed Nodes
        openList.Add(startNode);

        // This loop runs until the openList is empty.
        // The open list contains nodes waiting to be discovered.
        while (openList.Count > 0)
        {
            //From the nodes in the open list, the best one according to the cost calculation is selected.
            //This node is selected according to the cost F (daily cost + estimated cost).
            //If the costs are equal, the cost H (estimated cost) is taken into account.
            ///////////////////////////////
            Node currentNode = openList[0];

            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].FCost < currentNode.FCost || (openList[i].FCost == currentNode.FCost && openList[i].HCost < currentNode.HCost))
                {
                    currentNode = openList[i];
                }
            }
            ///////////////////////////////

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // If the current node is the target node,
            // the path has been found and the
            // RetracePath function is called to retrace the path from the start node to the destination node.
            if (currentNode == endNode)
            {
                endNode.IsUsed = true;
                return RetracePath(startNode, endNode);
            }

            // Check the neighbors of the current node.
            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                // If the node is already used or in the closed list, it is skipped.
                if (neighbor.IsUsed || closedList.Contains(neighbor))
                {
                    continue;
                }

                // A new movement cost is calculated.
                // This cost is the cost of moving from the current node to the neighboring node.
                int newMovementCostToNeighbor = currentNode.GCost + GetDistance(currentNode, neighbor);

                // If this new cost is lower than the current cost of the neighboring node or if the neighboring node is not on the open list,
                // the costs of the neighboring node are updated and the node is added to the open list.
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

    // If the last cell is a unit or a structure, we find the nearest empty cell and take it.
    private Node FindNearestAvailableNode(Node node)
    {
        Queue<Node> nodesToCheck = new Queue<Node>();
        HashSet<Node> checkedNodes = new HashSet<Node>();

        nodesToCheck.Enqueue(node);
        checkedNodes.Add(node);

        while (nodesToCheck.Count > 0)
        {
            Node currentNode = nodesToCheck.Dequeue();

            if (currentNode != null && !currentNode.IsUsed)
            {
                return currentNode;
            }

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (!checkedNodes.Contains(neighbor))
                {
                    nodesToCheck.Enqueue(neighbor);
                    checkedNodes.Add(neighbor);
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

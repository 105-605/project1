using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    PathRequestManager requestManager;
    PathfindingGrid grid;

    private void Awake()
    {
        grid = GetComponent<PathfindingGrid>();
        requestManager = GetComponent<PathRequestManager>();
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Vector3[] wayppoints = new Vector3[0];
        bool pathSuccess = false;

        PathfindingGrid.Node startNode = grid.NodeFromWorldPoint(startPos);
        PathfindingGrid.Node targetNode = grid.NodeFromWorldPoint(targetPos);

        if (startNode.walkable && targetNode.walkable)
        {
            Heap<PathfindingGrid.Node> openSet = new Heap<PathfindingGrid.Node>(grid.MaxSize());
            HashSet<PathfindingGrid.Node> closedSet = new HashSet<PathfindingGrid.Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                PathfindingGrid.Node currentNode = openSet.RemoveFirst();

                closedSet.Add(currentNode);

                if (targetNode == currentNode)
                {
                    pathSuccess = true;
                    break;
                }

                foreach (PathfindingGrid.Node neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                        continue;
                    int newMovementCostToNeighbour = currentNode.gCost + Distance(currentNode, neighbour);

                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = Distance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }


            }
        }
        yield return null;
        if (pathSuccess)
        {
            wayppoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(wayppoints, pathSuccess);
    }

    Vector3[] RetracePath (PathfindingGrid.Node startNode, PathfindingGrid.Node endNode)
    {
        List<PathfindingGrid.Node> path = new List<PathfindingGrid.Node>();
        PathfindingGrid.Node currentNode = endNode;
        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<PathfindingGrid.Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i=1;i<path.Count;i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);

            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    int Distance (PathfindingGrid.Node node1, PathfindingGrid.Node node2)
    {
        int distX = Mathf.Abs(node1.gridX - node2.gridX);
        int distY = Mathf.Abs(node1.gridY - node2.gridY);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);
        return 14 * distX + 10 * (distY - distX);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    PathfindingGrid grid;

    private void Awake()
    {
        grid = GetComponent<PathfindingGrid>();
    }

    void FindPath(UnityEngine.Vector3 startPos, UnityEngine.Vector3 targetPos)
    {
        PathfindingGrid.Node startNode = grid.NodeFromWorldPoint(startPos);
        PathfindingGrid.Node targetNode = grid.NodeFromWorldPoint(targetPos);

        List<PathfindingGrid.Node> openSet = new List<PathfindingGrid.Node>();
        HashSet<PathfindingGrid.Node> closedSet = new HashSet<PathfindingGrid.Node>();
        openSet.Add(startNode);
    
        while (openSet.Count > 0)
        {
            PathfindingGrid.Node currentNode = openSet[0];
            for (int i=1;i<openSet.Count;i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (targetNode == currentNode)
            {
                return;
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Numerics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PathfindingGrid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public UnityEngine.Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;
  
    float nodeDiameter;
    int gridSizeX, gridSizeY;

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    private void Update()
   {
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        UnityEngine.Vector3 worldBottomLeft = transform.position - UnityEngine.Vector3.right * gridWorldSize.x / 2 - UnityEngine.Vector3.up * gridWorldSize.y / 2;
        
        for (int x=0;x<gridSizeX;x++)
        {
            for (int y=0;y<gridSizeY;y++)
            {
                UnityEngine.Vector3 worldPoint = worldBottomLeft + UnityEngine.Vector3.right * (x * nodeDiameter + nodeRadius) + UnityEngine.Vector3.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }
    public List<Node> GetNeighbours(Node node)
    {

    }
    public Node NodeFromWorldPoint(UnityEngine.Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY= (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeX - 1) * percentY);
        return grid[x, y];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new UnityEngine.Vector3(gridWorldSize.x, gridWorldSize.y, 1));


       if (grid != null)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPosition, new UnityEngine.Vector3(1f*(nodeDiameter-0.1f), 1f*(nodeDiameter-0.1f), 1));
            }
        }
    }

    public class Node
    {
        public int gridX, gridY;
        public bool walkable;
        public UnityEngine.Vector3 worldPosition;
        public int gCost;
        public int hCost;

        public Node(bool _walkable, UnityEngine.Vector3 _worldPos, int _gridX, int _gridY)
        {
            walkable = _walkable;
            worldPosition = _worldPos;
            gridX = _gridX;
            gridY = _gridY;
        }

        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarTriggerGrid : MonoBehaviour
{
	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	CustomNode[,] grid;

	float nodeDiameter;
	int gridSizeX, gridSizeY;

	/*Vector2[] colliderCells =
    {
        new Vector2(40, 56),
        new Vector2(41, 56),
        new Vector2(42, 56),
        new Vector2(43, 56),
        new Vector2(44, 56),
        new Vector2(45, 56),
        new Vector2(46, 56),
        new Vector2(47, 56),
        new Vector2(48, 56),
        new Vector2(49, 56),
    };*/

	//Vector2[] colliderCells = new Vector2[188];
	List<Vector2> colliderCells = new List<Vector2>();

	void Awake()
	{
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
		CreateGrid();
	}

	void CreateGrid()
	{
		grid = new CustomNode[gridSizeX, gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

		for (int x = 0; x < gridSizeX; x++)
		{
			for (int y = 0; y < gridSizeY; y++)
			{
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
				//if (colliderCells.Contains(new Vector2(x, y))) walkable = false;
				if (!walkable) colliderCells.Add(new Vector2(x, y));
				grid[x, y] = new CustomNode(walkable, worldPoint, x, y);
			}
		}
	}

	public List<CustomNode> GetNeighbours(CustomNode node)
	{
		List<CustomNode> neighbours = new List<CustomNode>();

		for (int x = -1; x <= 1; x++)
		{
			for (int y = -1; y <= 1; y++)
			{
				if (x == 0 && y == 0)
					continue;

				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
				{
					neighbours.Add(grid[checkX, checkY]);
				}
			}
		}

		return neighbours;
	}


	public CustomNode NodeFromWorldPoint(Vector3 worldPosition)
	{
		float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
		float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
		return grid[x, y];
	}

	public List<CustomNode> path;
	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

		if (grid != null)
		{
			for (int x = 0; x < grid.GetLength(0); x++)
			{
				for (int y = 0; y < grid.GetLength(1); y++)
				{
					CustomNode n = grid[x, y];
					Gizmos.color = (n.walkable) ? Color.white : Color.red;
					if (path != null)
						if (path.Contains(n))
						{
							Gizmos.color = Color.green;
							Gizmos.DrawCube(n.worldPosition + new Vector3(0, 1, 0), Vector3.one * (nodeDiameter - .1f));
						}
					if (colliderCells.Contains(new Vector2(x, y)))
					{
						Gizmos.color = Color.blue;
						Gizmos.DrawCube(n.worldPosition + new Vector3(0, 1, 0), Vector3.one * (nodeDiameter - .1f));
					}
					//Gizmos.DrawCube(n.worldPosition + new Vector3(0, 1, 0), Vector3.one * (nodeDiameter - .1f));
				}
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPathfinding : MonoBehaviour
{
	public Transform seeker, target;
	public AStarTriggerGrid grid;

	public float speed = 100.0f;
	public float nextWaypointDistance = 3f;
	int currentWaypoint = 0;
	bool reachedEndOfPath = false;
	public Rigidbody2D rb;

	[SerializeField] EnemyMovement enemyMovement;

	void Awake()
	{
		
	}

	private void Start()
	{
		InvokeRepeating("UpdatePath", 0f, 0.5f);
	}

	void UpdatePath()
	{
		if (target == null) return;
		FindPath(seeker.position, target.position);
	}

	private void FixedUpdate()
	{
		if (grid.path == null) return;

		if (currentWaypoint >= grid.path.Count)
		{
			reachedEndOfPath = true;
			return;
		}
		reachedEndOfPath = false;

		Vector3 direction = (grid.path[currentWaypoint].worldPosition - transform.position).normalized;
		//playerController.SetMoveDir(direction);
		enemyMovement.SetFlow(new Vector2(direction.x, direction.y));


		float distance = Vector3.Distance(rb.position, grid.path[currentWaypoint].worldPosition);

		if (distance < nextWaypointDistance)
			currentWaypoint++;
	}

	void FindPath(Vector3 startPos, Vector3 targetPos)
	{
		CustomNode startCustomNode = grid.NodeFromWorldPoint(startPos);
		CustomNode targetCustomNode = grid.NodeFromWorldPoint(targetPos);

		List<CustomNode> openSet = new List<CustomNode>();
		HashSet<CustomNode> closedSet = new HashSet<CustomNode>();
		openSet.Add(startCustomNode);

		while (openSet.Count > 0)
		{
			CustomNode node = openSet[0];
			for (int i = 1; i < openSet.Count; i++)
			{
				if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
				{
					if (openSet[i].hCost < node.hCost)
						node = openSet[i];
				}
			}

			openSet.Remove(node);
			closedSet.Add(node);

			if (node == targetCustomNode)
			{
				RetracePath(startCustomNode, targetCustomNode);
				currentWaypoint = 0;
				return;
			}

			foreach (CustomNode neighbour in grid.GetNeighbours(node))
			{
				if (!neighbour.walkable || closedSet.Contains(neighbour))
				{
					continue;
				}

				int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
				if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
				{
					neighbour.gCost = newCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, targetCustomNode);
					neighbour.parent = node;

					if (!openSet.Contains(neighbour))
						openSet.Add(neighbour);
				}
			}
		}
	}

	void RetracePath(CustomNode startCustomNode, CustomNode endCustomNode)
	{
		List<CustomNode> path = new List<CustomNode>();
		CustomNode currentCustomNode = endCustomNode;

		while (currentCustomNode != startCustomNode)
		{
			path.Add(currentCustomNode);
			currentCustomNode = currentCustomNode.parent;
		}
		path.Reverse();

		grid.path = path;

	}

	int GetDistance(CustomNode nodeA, CustomNode nodeB)
	{
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if (dstX > dstY)
			return 14 * dstY + 10 * (dstX - dstY);
		return 14 * dstX + 10 * (dstY - dstX);
	}
}

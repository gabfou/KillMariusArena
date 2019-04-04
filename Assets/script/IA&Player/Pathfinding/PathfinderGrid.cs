using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Pathfinder;

public class PathfinderGrid : MonoBehaviour
{
	public float sizeX = 100;
	public float sizeY = 100;
	public float distanceBetweenNode = 0.5f;
	public List<Profile> profiles = new List<Profile>();
	[HideInInspector] public Node[,] allNodes;
	
	public class Profile
	{
		public int index;
		public bool isflying = false;

		public Profile(Character c)
		{
			isflying = c.flying;
		}

		public static bool operator ==(Profile a, Profile b)
		{
			return (a.isflying == b.isflying);
		}

		public static bool operator !=(Profile a, Profile b)
		{
			return (!(a == b));
		}

	}


	private void OnEnable()
	{
		GameManager.instance.pathfinderGrid = this;
		GenerateGrid();
	}

	void CreateBaseGrid()
	{
		int x = 0;
		int y = 0;
		Vector2 start = (Vector2)transform.position - new Vector2(sizeX / 2, sizeY / 2);
		Vector2 end = (Vector2)transform.position + new Vector2(sizeX / 2, sizeY / 2);
		Vector2 pos = start;
		allNodes = new Node[(int)(sizeX / distanceBetweenNode) + 1, (int)(sizeY / distanceBetweenNode) + 1];

		while (pos.y <= end.y)
		{
			pos.x = start.x;
			x = 0;
			while (pos.x < end.x)
			{
				allNodes[x,y] = new Node(pos);
				if (x > 0)
					allNodes[x,y].LinkConnection(allNodes[x - 1, y], 6);
				if (y > 0)
					allNodes[x,y].LinkConnection(allNodes[x, y - 1], 0);
				if (x > 0 && y > 0)
					allNodes[x,y].LinkConnection(allNodes[x - 1, y - 1], 7);
				if (y > 0 && x < allNodes.GetLength(0) - 1)
					allNodes[x,y].LinkConnection(allNodes[x + 1, y - 1], 1);
				pos.x += distanceBetweenNode;
				x++;
			}
			pos.y += distanceBetweenNode;
			y++;
		}	
	}


	void DestroyOnWall()
	{
		RaycastHit2D[] results = new RaycastHit2D[10];
		for (int x = 0; x < allNodes.GetLength(0); x++)
		{
			for (int y = 0; y < allNodes.GetLength(1); y++)
			{
				if (allNodes[x,y] != null)
				{
					if ((Physics2D.CircleCastNonAlloc(allNodes[x,y].pos, distanceBetweenNode, Vector2.down, results, 0, 1 << LayerMask.NameToLayer("Ground")) > 0))
					{
						allNodes[x,y].DeleteAllConnections();
						allNodes[x,y] = null;
					}
				}
			}
		}
	}


	void InitProfile(Profile profile)
	{
		foreach (Node node in allNodes)
		{
			node.isvalid.Add(true);
		}
	}
	
	public void GenerateGrid()
	{
		Debug.Log("Creating Base Grid");
		CreateBaseGrid();
		Debug.Log("DestroyOnWall");
		DestroyOnWall();
		for (int i = 0; i < profiles.Count; i++)
		{
			Debug.Log("InitProfile" + i);
			InitProfile(profiles[i]);
		}
		Debug.Log("Finish!");
	}

	public int CreateProfile(Character c)
	{
		Profile p = new Profile(c);
		int index = -1;
		if (!profiles.Any(p2 =>{if (p == p2) index = p2.index;  return (p == p2);}))
		{
			index = profiles.Count;
			profiles.Add(p);
		}
		return index;
	}

	void OnDrawGizmosSelected()
	{
		if(allNodes == null)
		{
			return;
		}
		foreach (Node n in allNodes)
		{
			if (n != null)
			for (int j = 0; j < n.connections.Length; j++)
			{
				if(n.connections[j] != null)
				{
					Gizmos.color = (lastPath.Contains(n) && lastPath.Contains(n.connections[j])) ? Color.blue : Color.green;
					Gizmos.DrawLine(n.pos, n.connections[j].pos);
				}
			}
		}
		foreach (Node n in allNodes)
		{
			if (n != null)
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawCube(n.pos, Vector3.one * 0.2f);
			}
		}
	}
	

	public Node FindClosestNode(Vector2 pos, int distanceMax = 4)
	{
		int x = (int)((pos.x - allNodes[0,0].pos.x) / distanceBetweenNode);
		int y = (int)((pos.y - allNodes[0,0].pos.y) / distanceBetweenNode);
		x = Mathf.Clamp(x, 0, allNodes.GetLength(0) - 1);
		y = Mathf.Clamp(y, 0, allNodes.GetLength(1) - 1);

		// if null check around

		int distance = 1;
		while (allNodes[x,y] == null && distance <= distanceMax) 
		{
			if (x + distance < allNodes.GetLength(0) - 1)
			{
				if (allNodes[x + distance, y] != null)
					return (allNodes[x + distance, y]);
				if (y + distance < allNodes.GetLength(1) - 1 && allNodes[x + distance, y + distance] != null)
					return (allNodes[x + distance, y + distance]);
				if (y + distance < -1 && allNodes[x + distance, y - distance] != null)
					return (allNodes[x + distance, y - distance]);
			}
			if (x - distance > -1)
			{
				if (allNodes[x - distance, y] != null)
					return (allNodes[x + distance, y]);
				if (y + distance < allNodes.GetLength(1) - 1 && allNodes[x - distance, y + distance] != null)
					return (allNodes[x + distance, y + distance]);
				if (y + distance < -1 && allNodes[x - distance, y - distance] != null)
					return (allNodes[x + distance, y - distance]);
			}
			if (y + distance < allNodes.GetLength(1) - 1 && allNodes[x - distance, y + distance] != null)
					return (allNodes[x, y + distance]);
			if (y + distance < -1 && allNodes[x - distance, y - distance] != null)
					return (allNodes[x, y - distance]);
			distance++;
		}
		return (allNodes[x,y]);
	}

	
	void ConstructGetAstarPath(Node Start, Node End, ref List<Node> path)
	{

		while(End != null && End != Start)
		{
			path.Insert(0, End);
			End = End.parent;
		}
		lastPath = path;
	}
	

	// Add

	List<Node> Banish = new List<Node>();
	List<Node> lastPath = new List<Node>();
	List<Node> openList = new List<Node>();
	public void GetAStar(Node Start, Node End, int profileIndex, ref List<Node> path, int LimitDepht = 500)
	{
		if (End == null || Start == null)
			return ;
		openList.ForEach(n => n.isInOpenList = false);
		openList.Clear();
		Banish.ForEach(n => n.isInBanish = false);
		Banish.Clear();
		Start.gcost = 0;
		Start.isInOpenList = true;
		openList.Add(Start);
		Start.parent =  null;

		while (openList.Count > 0 && LimitDepht-- > 0)
		{
			Node current = openList[0];


			openList.Remove(current);
			current.isInOpenList = false;
			current.isInBanish = true;
			Banish.Add(current);
			foreach(Node n in current.connections)
			{
				if (n == End)
				{
					n.parent = current;
					ConstructGetAstarPath(Start, n, ref path);
					return ;
				}
				if (n != null)
				{
					int newcost = current.gcost + 1;
					if (n.isInBanish != false || n.isInOpenList != false)
					{
						if (n.gcost <= newcost)
							continue ;
						else if (n.isInBanish == false)// ca c'est pas ouf niveaux opti
						{
							Banish.Remove(n);
							n.isInBanish = false;
						}
						else
							openList.Remove(n);
					}
					else
						n.h = Start.heuristic(n, End);
					n.gcost = newcost;
					n.parent = current;
					int tmp = openList.FindIndex(n2 => n2.h > n.h);
					if (tmp == -1)
						tmp = openList.Count;
					n.isInOpenList = true;
					openList.Insert(tmp, n);
				}
			}
		}
		Debug.Log("GetAStar FAILLLLLL!!!! NBBanish = " + Banish.Count);
	}
}
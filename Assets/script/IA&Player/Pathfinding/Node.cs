using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace Pathfinder
{
	public class Node : IComparer<float>
	{
		public List<bool> isvalid = new List<bool>();
		public Vector2 pos;
		float height = 0;
		public float h = Mathf.Infinity;
		public int gcost = int.MaxValue;
		public Node parent = null;
		public bool isInOpenList = false;
		public  bool isInBanish = false;

		// for refenrece ils sont mids dans le sens des aiguille d'une montre en partan de celui au dessu
		public Node[] connections;
		public enum NodePosition {up = 0, upRight = 1, right = 2, lowRight = 3, low = 4, lowLeft = 5, left = 6, upLeft = 7};

		public Node(Vector2 pos)
		{
			this.pos = pos;
			height = Physics2D.Raycast(pos, Vector2.down).distance;
			connections = new Node[8];
		}

		public void heuristic()
		{

		}

		public int Compare(float x, float y)
		{
			return 0;
		}



		// index 1 & 2 are the index of the node to link, indexWhereToLink1 is the index where the first node will store the second
		public void LinkConnection(Node nodeToLink, int indexWhereToLink)
		{
			if (nodeToLink == null)
				return ;
			connections[indexWhereToLink] = nodeToLink;
			nodeToLink.connections[(indexWhereToLink + 4) % 8] = this;
		}

		public void DeleteAllConnections()
		{
			for (int i = 0; i < 8; i++)
			{
				if (connections[i] != null)
				{
					connections[i].connections[(i + 4) % 8] = null;
					connections[i] = null;
				}
			}
		}

		public float heuristic(Node next, Node Cible)
		{
			return (Vector2.Distance(next.pos, Cible.pos));
		}

		public Node NextNodeByDir(int x, int y)
		{
			switch(y)
			{
				case (1):
					switch(x)
					{
						case (1):
							return connections[(int)NodePosition.upRight];
						case (-1):
							return  connections[(int)NodePosition.upLeft];
						case (0):
							return connections[(int)NodePosition.up];
					}
					break ;

				case (-1):
					switch(x)
					{
						case (1):
							return connections[(int)NodePosition.lowRight];
						case (-1):
							return  connections[(int)NodePosition.lowLeft];
						case (0):
							return connections[(int)NodePosition.low];
					}
					break ;

				case (0):
					switch(x)
					{
						case (1):
							return connections[(int)NodePosition.right];
						case (-1):
							return  connections[(int)NodePosition.left];
					}
					break ;
			}

		return null;
		}

		public override string ToString()
		{
			return (pos.ToString());
		}

		public static bool operator ==(Node a, Node b)
		{
			if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null))
			{
				return object.ReferenceEquals(a, b);
			}
			return (a.pos == b.pos);
		}

		public static bool operator !=(Node a, Node b)
		{
			return (!(a == b));
		}
	}
}
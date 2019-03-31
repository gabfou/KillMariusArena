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
    
    Node CenterNode;
    List<Node> allNodes = new List<Node>();
    
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


    private void OnEnable() {
        GameManager.instance.pathfinderGrid = this;
    }

    void CreateBaseGrid()
    {
        CenterNode = new Node(transform.position);

        Stack<Node> toAddNeighbord = new Stack<Node>();
        Stack<Node> justAdded = new Stack<Node>();
        toAddNeighbord.Push(CenterNode);
        allNodes.Clear();

        while (toAddNeighbord.Count != 0)
        {
            while (toAddNeighbord.Count != 0)
            {
                Node n = toAddNeighbord.Pop();
                allNodes.Add(n);
                // GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = n.pos;
                if (Mathf.Abs(n.pos.x - transform.position.x) < sizeX / 2 && Mathf.Abs(n.pos.y - transform.position.y) < sizeY / 2)
                    n.AddAllNeighbor(distanceBetweenNode, justAdded);
            }
            while(justAdded.Count != 0)
                toAddNeighbord.Push(justAdded.Pop());
        }


    }


    void DestroyOnWall()
    {
        RaycastHit2D[] results = new RaycastHit2D[10];
        allNodes.RemoveAll(node => {
            if ((Physics2D.CircleCastNonAlloc(node.pos, distanceBetweenNode, Vector2.down, results, 0, 1 << LayerMask.NameToLayer("Ground")) > 0))
            {
                node.DeleteAllConnections();
                return true;
            }
            return false;
            });
    }


    void InitProfile(Profile profile)
    {
        allNodes.ForEach( node =>
        {
            node.isvalid.Add(true);
        });
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

    int CreateProfile(Character c)
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
        for (int i = 0; i < allNodes.Count; i++)
        {
            for (int j = 0; j < allNodes[i].connections.Length; j++)
            {
                if(allNodes[i].connections[j] != null)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(allNodes[i].pos, allNodes[i].connections[j].pos);
                }
            }
        }
        for (int i = 0; i < allNodes.Count; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(allNodes[i].pos, Vector3.one * 0.2f);
        }
    }

    public Node FindClosestNode(Vector2 pos)
    {
        Node r = CenterNode;
        float dist;
        float tmp;
        dist = Vector2.Distance(CenterNode.pos, pos);
        allNodes.ForEach(n => 
        {
            if ((tmp = Vector2.Distance(n.pos, pos)) < dist)
            {
                dist = tmp;
                r = n;
            }
        });
        return r;
    }

    List<Node> Banish = new List<Node>();
    public List<Node> GetAStar(Node Start, Node End, int profileIndex, int LimitDepht = int.MaxValue)
    {
        List<Node> path = new List<Node>();
        Banish.Clear();
        Node tmp;
        float heuristic;
        int i = -1;
        path.Add(Start);
        Banish.Add(Start);

        while (Start != End && ++i < LimitDepht)
        {
            Banish.Add(Start);
            heuristic = Mathf.Infinity;
            tmp = null;
            foreach(Node n in Start.connections)
            {
                if (Start.heuristic(n, End) < heuristic && !Banish.Contains(n))
                    tmp = n;
            }
            if (tmp == null)
            {
                if (path.Count < 2)
                {
                    Debug.Log("GetAStar FAILLLLLL!!!!");
                    return null;
                }
                Start = path[path.Count - 1];
            }
            else
                Start = tmp;
        }
        return path;
    }


}
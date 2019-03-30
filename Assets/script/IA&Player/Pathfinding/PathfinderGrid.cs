using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathfinderGrid : MonoBehaviour
{
    public float sizeX = 100;
    public float sizeY = 100;
    public float distanceBetweenNode = 0.5f;

    Node CenterNode;
    List<Node> allNodes = new List<Node>();
    
    public class Node
    {
        public Vector2 pos;

        // for refenrece ils sont mids dans le sens des aiguille d'une montre en partan de celui au dessu
        public Node[] connections;
        public enum NodePosition {up = 0, upRight = 1, right = 2, lowRight = 3, low = 4, lowLeft = 5, left = 6, upLeft = 7};

        public Node(Vector2 pos)
        {
            this.pos = pos;
            connections = new Node[8];
        }


        void AddConnection(int index, Vector2 newPos,  Stack<Node> newNode = null)
        {
            connections[index] = new Node(newPos);
            connections[index].connections[(index + 4) % 8] = this;
            if (newNode != null)
                newNode.Push(connections[index]);
        }

        // index 1 & 2 are the index of the node to link, indexWhereToLink1 is the index where the first node will store the second
        void LinkConnection(int index1, int index2, int indexWhereToLink1)
        {
            if (connections[index1] == null || connections[index2] == null)
                return ;
            connections[index1].connections[indexWhereToLink1] = connections[index2];
            connections[index2].connections[(indexWhereToLink1 + 4) % 8] = connections[index1];
        }

        void DeleteAllConnections()
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

        public void AddAllNeighbor(float dist, Stack<Node> newNode = null)
        {
            if (connections[1] == null)
                AddConnection(1, pos + new Vector2(1, 1) * dist, newNode);

            if (connections[3] == null)
                AddConnection(3, pos + new Vector2(1, -1) * dist, newNode);

            if (connections[5] == null)
                AddConnection(5, pos + new Vector2(-1, -1) * dist, newNode);

            if (connections[7] == null)
                AddConnection(7, pos + new Vector2(-1, 1) * dist, newNode);


            LinkConnection(0, 1, 3);
            LinkConnection(1, 2, 3);
            LinkConnection(2, 3, 5);
            LinkConnection(3, 4, 5);
            LinkConnection(4, 5, 7);
            LinkConnection(5, 6, 7);
            LinkConnection(6, 7, 1);
            LinkConnection(7, 0, 1);

            LinkConnection(1, 3, 4);
            LinkConnection(3, 5, 6);
            LinkConnection(5, 7, 0);
            LinkConnection(7, 1, 2);
        }
        
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
            if ((Physics2D.CircleCastNonAlloc(node.pos, distanceBetweenNode / 2, Vector2.down, results) > 0))
            {
                node.DeleteAllConnections();
                return true;
            }
            return false;
            });
    }

    
    public void GenerateGrid()
    {
        Debug.Log("Creating Base Grid");
        CreateBaseGrid();
        Debug.Log("Finish!");
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


}
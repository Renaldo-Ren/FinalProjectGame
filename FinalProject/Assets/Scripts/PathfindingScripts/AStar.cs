using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStar : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;
    private Vector3Int startPos, goalPos;
    private Node current;
    private HashSet<Node> openlist;
    private HashSet<Node> closedlist;
    private Stack<Vector3> path;
    private Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();
    private static HashSet<Vector3Int> noDiagonalTiles = new HashSet<Vector3Int>();
    private List<Vector3Int> waterTiles = new List<Vector3Int>();

    public static HashSet<Vector3Int> NoDiagonalTiles { get => noDiagonalTiles; }
    public Tilemap MyTilemap { get => tilemap;  }

    public Stack<Vector3> Algorithm(Vector3 start, Vector3 goal)
    {
        startPos = tilemap.WorldToCell(start);
        goalPos = tilemap.WorldToCell(goal);
        //if(current == null)
        //{

        //}
        current = GetNode(startPos);
        openlist = new HashSet<Node>();
        closedlist = new HashSet<Node>();
        openlist.Add(current); 
        path = null;
        while (openlist.Count > 0 && path == null)
        {
            List<Node> neighbors = FindNeighbors(current.Position);

            ExamineNeighbors(neighbors, current);

            UpdateCurrentTile(ref current);

            path = GeneratePath(current);
        }
        if(path != null)
        {
            return path;
        }
        return null;
    }

    private List<Node> FindNeighbors(Vector3Int parentPosition)
    {
        List<Node> neighbors = new List<Node>();
        for(int x = -1; x <= 1; x++) 
        {
            for(int y = -1; y <= 1; y++)
            {
                if (y != 0 || x != 0)
                {
                    Vector3Int neighborPos = new Vector3Int(parentPosition.x - x, parentPosition.y - y, parentPosition.z);
                    if (neighborPos != startPos && !GameManage.MyInstance.Blocked.Contains(neighborPos) && tilemap.GetTile(neighborPos))
                    {
                        Node neighbor = GetNode(neighborPos);
                        neighbors.Add(neighbor);
                    }
                }
            }
        }
        return neighbors;
    }

    private void ExamineNeighbors(List<Node> neighbors, Node current)
    {
        for(int i = 0; i< neighbors.Count; i++)
        {
            Node neighbor = neighbors[i];
            //if (!ConnectedDiagonally(current,neighbor))
            //{
            //    continue;
            //}
            int gScore = DetermineGScore(neighbors[i].Position, current.Position); 
            
            //if(gScore == 14 && noDiagonalTiles.Contains(neighbor.Position) && noDiagonalTiles.Contains(current.Position))
            //{
            //    continue;
            //}
            
            if (openlist.Contains(neighbor))
            { 
                if (current.G + gScore < neighbor.G) 
                {
                    CalcValues(current, neighbor, gScore);
                }
            }
            else if (!closedlist.Contains(neighbor))
            {
                CalcValues(current, neighbor, gScore); 
                openlist.Add(neighbor);
            }
        }
    }
    private void CalcValues(Node parent, Node neighbor, int cost)
    {
        neighbor.Parent = parent; 
        neighbor.G = parent.G + cost; 
        neighbor.H = ((Mathf.Abs((neighbor.Position.x - goalPos.x)) + Mathf.Abs((neighbor.Position.y - goalPos.y))) * 10); 
        neighbor.F = neighbor.G + neighbor.H; 
    }
    private int DetermineGScore(Vector3Int neighbor, Vector3Int current)
    {
        int gScore = 0;
        int x = current.x - neighbor.x;
        int y = current.y - neighbor.y;
        if(Mathf.Abs(x-y)%2 == 1) 
        {
            gScore = 10;
        }
        else
        {
            gScore = 14;
        }
        return gScore;
    }
    private void UpdateCurrentTile( ref Node current)
    {
        openlist.Remove(current);
        closedlist.Add(current);
        if(openlist.Count > 0)
        {
            current = openlist.OrderBy(x => x.F).First();
        }
    }
    private Node GetNode(Vector3Int position)
    {
        if (allNodes.ContainsKey(position)) 
        {
            return allNodes[position]; 
        }
        else 
        {
            Node node = new Node(position);
            allNodes.Add(position, node);
            return node;
        }
    }
    
    private bool ConnectedDiagonally(Node currentNode, Node neighbor)
    {
        Vector3Int direct = currentNode.Position - neighbor.Position;
        Vector3Int left = new Vector3Int(current.Position.x + (direct.x * -1), current.Position.y, current.Position.z);
        Vector3Int top = new Vector3Int(current.Position.x, current.Position.y + (direct.y * -1), current.Position.z);
        Vector3Int right = new Vector3Int(current.Position.x + (direct.x * 1), current.Position.y, current.Position.z);
        Vector3Int bottom = new Vector3Int(current.Position.x, current.Position.y + (direct.y * 1), current.Position.z);
        if ((waterTiles.Contains(left) && waterTiles.Contains(top)) || 
            (waterTiles.Contains(left) && waterTiles.Contains(bottom)) || 
            (waterTiles.Contains(right) && waterTiles.Contains(bottom)) || 
            (waterTiles.Contains(right) && waterTiles.Contains(top)))
        {
            return false;
        }
        return true;
    }
    private Stack<Vector3> GeneratePath(Node current)
    {
        if(current.Position == goalPos)
        {
            Stack<Vector3> finalPath = new Stack<Vector3>();
            while(current.Position != startPos)
            {
                finalPath.Push(current.Position);
                current = current.Parent;
            }
            return finalPath;
         }
        return null;
    }
}

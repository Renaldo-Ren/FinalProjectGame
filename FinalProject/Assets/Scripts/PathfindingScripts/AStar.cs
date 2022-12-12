using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStar : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;
    //private Tile[] tiles;
    //private RuleTile water;
    //private Camera camera;
    //private LayerMask layerMask;
    //private bool start, goal;
    private Vector3Int startPos, goalPos;
    private Node current;
    
    
    private HashSet<Node> openlist;
    private HashSet<Node> closedlist;
    private Stack<Vector3> path;
    //private HashSet<Vector3Int> changedTiles = new HashSet<Vector3Int>();
    private Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();
    private static HashSet<Vector3Int> noDiagonalTiles = new HashSet<Vector3Int>();
    private List<Vector3Int> waterTiles = new List<Vector3Int>();

    public static HashSet<Vector3Int> NoDiagonalTiles { get => noDiagonalTiles; }
    public Tilemap MyTilemap { get => tilemap; /*set => tilemap = value;*/ }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        RaycastHit2D hit = Physics2D.Raycast(camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, layerMask);
    //        if(hit.collider != null)
    //        {
    //            Vector3 mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
    //            Vector3Int clickPos = tilemap.WorldToCell(mouseWorldPos);
    //            ChangeTile(clickPos);
    //        }
    //    }
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        //run algorithm
    //        Algorithm();
    //    }
    //}
    //private void InitiaizedAlgo()
    //{
    //    current = GetNode(startPos);
    //    openlist = new HashSet<Node>();
    //    closedlist = new HashSet<Node>();
    //    //Adding start to the open list
    //    openlist.Add(current);
    //}
    public Stack<Vector3> Algorithm(Vector3 start, Vector3 goal)
    {
        //ClearAStar();
        startPos = tilemap.WorldToCell(start);
        goalPos = tilemap.WorldToCell(goal);

        //if (current == null)
        //{
        //    InitiaizedAlgo();
        //}
        current = GetNode(startPos);
        openlist = new HashSet<Node>(); //Creates an open list for nodes that we might want to look at later
        closedlist = new HashSet<Node>(); //Creates a closed list for nodes that we have examined
        openlist.Add(current); //Adding the current node to the open list
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
    //private void Algorithm()
    //{
    //    if(current == null) //if the current node is null
    //    {
    //        InitiaizedAlgo();
    //    }
    //    while(openlist.Count > 0 && path == null)
    //    {
    //        List<Node> neighbors = FindNeighbors(current.Position);
    //        ExamineNeighbors(neighbors, current);
    //        UpdateCurrentTile(ref current);
    //        path = GeneratePath(current);
    //    }

    //    //AStarDebugger.myinstance.createtiles(openlist, closedlist, startPos, goalPos);
    //}
    private List<Node> FindNeighbors(Vector3Int parentPosition)
    {
        List<Node> neighbors = new List<Node>();
        for(int x = -1; x <= 1; x++) //These two for loops makes sure that we all nodes aroun our current node
        {
            for(int y = -1; y <= 1; y++)
            {
                if (y != 0 || x != 0) //to make sure not put parent as the neighbor
                {
                    Vector3Int neighborPos = new Vector3Int(parentPosition.x - x, parentPosition.y - y, parentPosition.z);
                    if (neighborPos != startPos && !GameManage.MyInstance.Blocked.Contains(neighborPos))
                    {
                        //Add the neighbor node
                        Node neighbor = GetNode(neighborPos);
                        neighbors.Add(neighbor);
                    }
                }
                    
                //if (y != 0 || x != 0) //to make sure not put parent as the neighbor
                //{
                //    if(neighborPos != startPos && !waterTiles.Contains(neighborPos) && tilemap.GetTile(neighborPos)) //to not add start as neighbor and ignore waterTiles in neighbor position and check if the tile is exist on the tilemap
                //    {
                //        //Add the neighbor node
                //        Node neighbor = GetNode(neighborPos);
                //        neighbors.Add(neighbor);
                //    }
                    
                //}
            }
        }
        return neighbors;
    }

    private void ExamineNeighbors(List<Node> neighbors, Node current)
    {
        for(int i = 0; i< neighbors.Count; i++)
        {
            Node neighbor = neighbors[i]; //take one of the neighbor from the neighbors list
            if (!ConnectedDiagonally(current,neighbor))
            {
                continue; //skip the negihbor node that no need to be examine
            }
            int gScore = DetermineGScore(neighbors[i].Position, current.Position); //calculate g score
            
            if(gScore == 14 && noDiagonalTiles.Contains(neighbor.Position) && noDiagonalTiles.Contains(current.Position))
            {
                continue;
            }
            
            if (openlist.Contains(neighbor)) //if the neighbor is already on the openlist
            { //check if the path through the current node is a better alternative
                //recalculate the g score
                if (current.G + gScore < neighbor.G) //if the G score is lower, then make the current node the parent of the neighbor
                {
                    CalcValues(current, neighbor, gScore); //when there is a better alternative, change the parent and  recalculate is again
                }
            }
            else if (!closedlist.Contains(neighbor)) //if there are node that is not in the closedlist and also not in openlist, so basically a new neighbor node
            {
                CalcValues(current, neighbor, gScore); //calculate again for the new neighbors
                openlist.Add(neighbor); //add new neighbor to the openlist
            }
        }
    }
    private void CalcValues(Node parent, Node neighbor, int cost)
    {
        neighbor.Parent = parent; //set start as parent
        neighbor.G = parent.G + cost; //calculate neighbor G score
        neighbor.H = ((Mathf.Abs((neighbor.Position.x - goalPos.x)) + Mathf.Abs((neighbor.Position.y - goalPos.y))) * 10); //calculate neighbor H score
        neighbor.F = neighbor.G + neighbor.H; //Calculate negihbor F score
    }
    private int DetermineGScore(Vector3Int neighbor, Vector3Int current)
    {
        int gScore = 0;
        int x = current.x - neighbor.x;
        int y = current.y - neighbor.y;
        if(Mathf.Abs(x-y)%2 == 1) //if in vertical and horizontal
        {
            gScore = 10;
        }
        else //if in diagonal
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
            current = openlist.OrderBy(x => x.F).First(); //run through the whole list and find node with lowest F score and it will be set as the current node
        }
    }
    private Node GetNode(Vector3Int position)
    {
        if (allNodes.ContainsKey(position)) //if all nodes have exsit then return what it have found
        {
            return allNodes[position]; //return the one we have created
        }
        else //if does not exist, then create it and return it
        {
            Node node = new Node(position);
            allNodes.Add(position, node);
            return node;
        }
    }
    //public void ChangeTileType(TileButton button)
    //{
    //    tileType = button.myTileType;
    //    
    //}
    //private void ChangeTile(Vector3Int clickPos)
    //{
    //    if(tileType == TileType.WATER)
    //    {
    //        tilemap.SetTile(clickPos, water);
    //        waterTiles.Add(clickPos);
    //    }
    //    else
    //    {
    //        if(tileType == TileType.START)
    //        {
    //            if (start)
    //            {
    //                tilemap.SetTile(startPos, tiles[3]);
    //            }
    //            start = true;
    //            startPos = clickPos;
    //        }
    //        else if (tileType == TileType.GOAL)
    //        {
    //            if (goal)
    //            {
    //                tilemap.SetTile(goalPos, tiles[3]);
    //            }
    //            goal = true;
    //            goalPos = clickPos;
    //        }
    //        tilemap.SetTile(clickPos, tiles[(int)tileType]);
    //    }
    //    changedTiles.Add(clickPos);
    //}
    private bool ConnectedDiagonally(Node currentNode, Node neighbor)
    {
        Vector3Int direct = currentNode.Position - neighbor.Position;
        Vector3Int first = new Vector3Int(current.Position.x + (direct.x * -1), current.Position.y, current.Position.z);
        Vector3Int second = new Vector3Int(current.Position.x, current.Position.y + (direct.y * -1), current.Position.z);
        Vector3Int right = new Vector3Int(current.Position.x + (direct.x * 1), current.Position.y, current.Position.z);
        Vector3Int bottom = new Vector3Int(current.Position.x, current.Position.y + (direct.y * 1), current.Position.z);
        if (waterTiles.Contains(first) || waterTiles.Contains(second)) //If the first and second position is water, then it is not walkable path
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
    //private void Reseting()
    //{
    //    foreach(Vector3Int position in allNodes.Keys)
    //    {
    //        tilemap.SetTile(position, null);
    //    }
    //    foreach(Vector3Int position in changedTiles)
    //    {
    //        tilemap.SetTile(position, tiles[3]);
    //    }
    //    foreach (Vector3Int position in path)
    //    {
    //        tilemap.SetTile(position, tiles[3]);
    //    }
    //    tilemap.SetTile(startPos, tiles[3]);
    //    tilemap.SetTile(goalPos, tiles[3]);
    //    allNodes.Clear();
    //    start = false;
    //    goal = false;
    //    path = null;
    //    current = null;
    //}
    private void ClearAStar()
    {
        if (current != null)
        {
            allNodes.Clear();
            closedlist.Clear();
            openlist.Clear();
            path = null;
            current = null;
        }
    }
}

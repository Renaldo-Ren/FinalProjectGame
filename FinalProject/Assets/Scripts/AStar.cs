using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType { START, GOAL, WATER, GRASS, PATH}
public class AStar : MonoBehaviour
{
    private TileType tileType; //decide what tile being placed
    private Tilemap tilemap;
    private Tile[] tiles;
    private RuleTile water;
    private Camera camera;
    private LayerMask layerMask;
    private Vector3Int startPos, goalPos;
    private Node current;
    private List<Vector3Int> waterTiles = new List<Vector3Int>();


    private HashSet<Node> openlist;
    private Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, layerMask);
            if(hit.collider != null)
            {
                Vector3 mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int clickPos = tilemap.WorldToCell(mouseWorldPos);
                ChangeTile(clickPos);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //run algorithm
            Algorithm();
        }
    }
    private void InitiaizedAlgo()
    {
        current = GetNode(startPos);
        openlist = new HashSet<Node>();
        //Adding start to the open list
        openlist.Add(current);
    }
    private void Algorithm()
    {
        if(current == null) //if the current node is null
        {
            InitiaizedAlgo();
        }
        List<Node> neighbors = FindNeighbors(current.Position);
        ExamineNeighbors(neighbors, current);
        //AStarDebugger.myinstance.createtiles(openlist, startPos, goalPos);
    }
    private List<Node> FindNeighbors(Vector3Int parentPosition)
    {
        List<Node> neighbors = new List<Node>();
        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                Vector3Int neighborPos = new Vector3Int(parentPosition.x - x, parentPosition.y - y, parentPosition.z);
                if(y != 0 || x != 0) //to make sure not put parent as the neighbor
                {
                    if(neighborPos != startPos && !waterTiles.Contains(neighborPos) && tilemap.GetTile(neighborPos)) //to not add start as neighbor and ignore waterTiles in neighbor position and check if the tile is exist on the tilemap
                    {
                        //Add the neighbor node
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
            openlist.Add(neighbors[i]);
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
    private void ChangeTile(Vector3Int clickPos)
    {
        if(tileType == TileType.WATER)
        {
            tilemap.SetTile(clickPos, water);
            waterTiles.Add(clickPos);
        }
        else
        {
            if(tileType == TileType.START)
            {
                startPos = clickPos;
            }
            else if (tileType == TileType.GOAL)
            {
                goalPos = clickPos;
            }
            tilemap.SetTile(clickPos, tiles[(int)tileType]);
        }
    }
}

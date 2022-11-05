using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NoDiagonalTile : Tile
{
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        AStar.NoDiagonalTiles.Add(position);
        return base.StartUp(position, tilemap, go);
    }
#if UNITY_EDITOR
    [MenuItem("Assets/Create/2D/Tiles/NoDiagonalTile")]
    public static void CreateNoDiagonalTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save NoDiagonalTile", "NoDiagonalTile", "asset", "Save NoDiagonalTile", "Assets");
        if (path == "")
        {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<NoDiagonalTile>(), path);
    }

#endif
}

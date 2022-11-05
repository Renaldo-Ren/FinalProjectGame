using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterTile : Tile
{
    //[SerializeField]
    //private Sprite[] waterSprites;
    //[SerializeField]
    //private Sprite preview;
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        GameManage.MyInstance.Blocked.Add(position);
        return base.StartUp(position, tilemap, go);
    }
#if UNITY_EDITOR
    [MenuItem("Assets/Create/2D/Tiles/WaterTile")]
    public static void CreateWaterTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save WaterTile", "WaterTile", "asset", "Save WaterTile", "Assets");
        if (path == "")
        {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<WaterTile>(), path);
    }

#endif
}

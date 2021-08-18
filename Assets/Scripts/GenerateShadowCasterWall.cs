using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateShadowCasterWall : MonoBehaviour
{
    public Tilemap walls;
    TileBase[] wallTiles;
    public GameObject fakeWallTile; // not really a 'tile', but a sprite for replacement
    void Start()
    {
        Debug.Log("bounds = " + walls.cellBounds);
        /*wallTiles = walls.GetTilesBlock(new BoundsInt(walls.origin, walls.size));
        for (int i = 0; i < wallTiles.Length; i++)
        {
            Instantiate(fakeWallTile, wallTiles[i]., Quaternion.identity);
        }*/
        Debug.Log("xmin = " + walls.cellBounds.xMin + " xmax = " + walls.cellBounds.xMax + " ymin = " + walls.cellBounds.yMin + " ymax = " + walls.cellBounds.yMax);
        for (int y = walls.cellBounds.yMin; y < walls.cellBounds.yMax; y++)
        {
            for (int x = walls.cellBounds.xMin; x < walls.cellBounds.xMax; x++)
            {
                if (walls.GetTile(new Vector3Int(x, y, 0)) != null)
                    Instantiate(fakeWallTile, new Vector3(x + 0.5f, y + 0.5f, 4), Quaternion.identity);
                    //Debug.Log("tile name = " + walls.GetTile(new Vector3Int(x, y, 0)).name);
            }
        }
    }
}

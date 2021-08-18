using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CollectedItems : MonoBehaviour
{
    public class CollectionItem
    {
        public TileBase item;
        public Vector3Int pos; // the position of the tile which shows item's quantity
        public int quantity;
        public TileBase quantityTile;

        public CollectionItem(TileBase item, Vector3Int pos, int quantity, TileBase quantityTile)
        {
            this.item = item;
            this.pos = pos;
            this.quantity = quantity;
            this.quantityTile = quantityTile;
        }
    }


    Tilemap stuffTilemap;
    public GameObject player;
    TileBase currentTile;
    public Grid grid;
    float cellSizeX, cellSizeY;
    public Dictionary<string, CollectionItem> collection;
    public TileBase[] quantityTiles;

    public TileBase dt64;
    public TileBase dt68;
    public TileBase dt74;
    public TileBase dt76;
    public TileBase dt77;
    public TileBase dt92;
    public TileBase dt94;
    public TileBase dt95;

    public TileBase qnt0;
    public TileBase qnt1;
    public TileBase qnt2;
    public TileBase qnt3;
    public TileBase qnt4;
    public TileBase qnt5;
    public TileBase qnt6;
    public TileBase qnt7;
    public TileBase qnt8;
    public TileBase qnt9;

    public ProceduralTilemapLeft leftTilemap;



    void Awake()
    {
        stuffTilemap = GetComponent<Tilemap>();
        grid = transform.parent.gameObject.GetComponent<Grid>();
        cellSizeX = grid.cellSize.x * transform.localScale.x;
        cellSizeY = grid.cellSize.y * transform.localScale.y;


        quantityTiles = new TileBase[10];
        quantityTiles[0] = qnt0;
        quantityTiles[1] = qnt1;
        quantityTiles[2] = qnt2;
        quantityTiles[3] = qnt3;
        quantityTiles[4] = qnt4;
        quantityTiles[5] = qnt5;
        quantityTiles[6] = qnt6;
        quantityTiles[7] = qnt7;
        quantityTiles[8] = qnt8;
        quantityTiles[9] = qnt9;


        collection = new Dictionary<string, CollectionItem>();
        collection["Dungeon_Tileset_64"] = new CollectionItem(dt64, new Vector3Int(2, 1, 0), 0, quantityTiles[0]);
        collection["Dungeon_Tileset_68"] = new CollectionItem(dt68, new Vector3Int(2, 2, 0), 0, quantityTiles[0]);
        collection["Dungeon_Tileset_74"] = new CollectionItem(dt74, new Vector3Int(2, 3, 0), 0, quantityTiles[0]);
        collection["Dungeon_Tileset_76"] = new CollectionItem(dt76, new Vector3Int(2, 4, 0), 0, quantityTiles[0]);
        collection["Dungeon_Tileset_77"] = new CollectionItem(dt77, new Vector3Int(2, 5, 0), 0, quantityTiles[0]);
        collection["Dungeon_Tileset_92"] = new CollectionItem(dt92, new Vector3Int(2, 6, 0), 0, quantityTiles[0]);
        collection["Dungeon_Tileset_94"] = new CollectionItem(dt94, new Vector3Int(2, 7, 0), 0, quantityTiles[0]);
        collection["Dungeon_Tileset_95"] = new CollectionItem(dt95, new Vector3Int(2, 8, 0), 0, quantityTiles[0]);
    }

    float ClosestValue(float value, float unitSize)
    {
        int numberOfUnits = (int)Mathf.Floor(Mathf.Abs(value) / unitSize);

        if (Mathf.Abs(value) == numberOfUnits * unitSize)
            return value;

        if (Mathf.Abs(Mathf.Abs(value) - numberOfUnits * unitSize) < unitSize / 2)
            return Mathf.Sign(value) * numberOfUnits * unitSize;

        return Mathf.Sign(value) * (numberOfUnits + 1) * unitSize;
    }

    Vector3 ClosestCell(Vector3 position, float cellSizeX, float cellSizeY)
    {
        return new Vector3(ClosestValue(position.x, cellSizeX), ClosestValue(position.y, cellSizeY));
    }

    Vector3Int V3ToV3Int(Vector3 v3)
    {
        return new Vector3Int((int)v3.x, (int)v3.y, (int)v3.z);
    }

    void Update()
    {
        Vector3Int closestCellIndex = V3ToV3Int(ClosestCell(player.transform.position - new Vector3(0.5f, 0.5f, 0), cellSizeX, cellSizeY));
        if ((currentTile = stuffTilemap.GetTile(closestCellIndex)) != null)
        {
            if (!collection.ContainsKey(currentTile.name))  // this condition should not be true unless you add some other types of tiles in the Stuff tilemap
                collection[currentTile.name].quantity = 1;
            else if (collection[currentTile.name].quantity < 8)
            {
                collection[currentTile.name].quantity++;
                collection[currentTile.name].quantityTile = quantityTiles[collection[currentTile.name].quantity];
                leftTilemap.UpdateTileCount(currentTile.name);
            }
            stuffTilemap.SetTile(closestCellIndex, null);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            foreach (string k in collection.Keys)
            {
                if (collection[k].quantity != 0)
                    Debug.Log("of element " + k + " there are " + collection[k].quantity);
            }
        }
    }
}

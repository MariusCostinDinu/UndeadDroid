using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralTilemapLeft : MonoBehaviour
{
    public Tilemap tilemap;
    public Tilemap tilemapBackground;
    public TileBase testTile;

    public TileBase menuTop;
    public TileBase menuRight;
    public TileBase menuBottom;
    public TileBase menuLeft;
    public TileBase menuTopLeft;
    public TileBase menuTopRight;
    public TileBase menuBottomRight;
    public TileBase menuBottomLeft;
    public TileBase menuBackground;

    public Grid grid;

    Camera mainCamera;

    float cellSizeX;
    float cellSizeY;

    Vector3 topLeftPoint;
    Vector3 topLeftWorldCoords;
    Vector3Int topLeftGridCoords;
    Vector3 offsetToClosestCorner;

    public CollectedItems collectedItems;

    void Awake()
    {
        cellSizeX = grid.cellSize.x * transform.localScale.x;
        cellSizeY = grid.cellSize.y * transform.localScale.y;

        mainCamera = FindObjectOfType<Camera>();
    }

    private void Start()
    {
        mainCamera.orthographicSize = 5;
        topLeftPoint = new Vector3(-(float)mainCamera.orthographicSize * Screen.width / Screen.height, mainCamera.orthographicSize, 0) + new Vector3(0, 0, 0);
        topLeftWorldCoords = ClosestCorner(topLeftPoint, cellSizeX, cellSizeY);
        topLeftGridCoords = new Vector3Int((int)(topLeftWorldCoords.x / cellSizeX), (int)(topLeftWorldCoords.y / cellSizeY), 0);
        offsetToClosestCorner = topLeftPoint - topLeftWorldCoords;
        grid.transform.position += offsetToClosestCorner;

        int itemVariety = collectedItems.collection.Keys.Count;
        tilemap.SetTile(RelativeToTopLeft(new Vector3Int(0, 0, 0)), menuTopRight);
        tilemap.SetTile(RelativeToTopLeft(new Vector3Int(1, 0, 0)), menuTop);
        tilemap.SetTile(RelativeToTopLeft(new Vector3Int(2, 0, 0)), menuTop);
        tilemap.SetTile(RelativeToTopLeft(new Vector3Int(3, 0, 0)), menuTopLeft);

        tilemap.SetTile(RelativeToTopLeft(new Vector3Int(0, itemVariety + 1, 0)), menuBottomRight);
        tilemap.SetTile(RelativeToTopLeft(new Vector3Int(1, itemVariety + 1, 0)), menuBottom);
        tilemap.SetTile(RelativeToTopLeft(new Vector3Int(2, itemVariety + 1, 0)), menuBottom);
        tilemap.SetTile(RelativeToTopLeft(new Vector3Int(3, itemVariety + 1, 0)), menuBottomLeft);

        for (int i = 1; i <= itemVariety; i++)
        {
            tilemap.SetTile(RelativeToTopLeft(new Vector3Int(0, i, 0)), menuRight);
            tilemap.SetTile(RelativeToTopLeft(new Vector3Int(3, i, 0)), menuLeft);
            tilemapBackground.SetTile(RelativeToTopLeft(new Vector3Int(1, i, 0)), menuBackground);
        }

        foreach (string item in collectedItems.collection.Keys) // collectedItems.collection.Keys == itemVariety
        {
            tilemap.SetTile(RelativeToTopLeft(collectedItems.collection[item].pos - new Vector3Int(1, 0, 0)), collectedItems.collection[item].item);
            UpdateTileCount(item);
        }
    }

    void Update()
    {
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

    Vector3 ClosestCorner(Vector3 position, float cellSizeX, float cellSizeY)
    {
        return new Vector3(ClosestValue(position.x, cellSizeX), ClosestValue(position.y, cellSizeY));
    }

    Vector3Int RelativeToTopLeft(Vector3Int gridCoords)
    {
        return new Vector3Int(-topLeftGridCoords.x - 1, topLeftGridCoords.y + 1, 0) - gridCoords;
    }
    Vector3 RelativeToTopLeft(Vector3 gridCoords)
    {
        return new Vector3(-topLeftGridCoords.x - 1, topLeftGridCoords.y + 1, 0) - gridCoords;
    }

    public void UpdateTileCount(string currentItem)
    {
        tilemap.SetTile(RelativeToTopLeft(collectedItems.collection[currentItem].pos), collectedItems.collection[currentItem].quantityTile);
    }
}

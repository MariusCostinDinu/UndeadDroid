using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityStandardAssets.CrossPlatformInput;

public class ProceduralTilemap : MonoBehaviour
{
    public enum ToolType
    {
        M1911,
        MP40,
        SHOTGUN,
        SHIELD_1,
        SHIELD_2,
        SHIELD_3
    }

    struct AnimControllers
    {
        public RuntimeAnimatorController controllerForTrigger;
        public RuntimeAnimatorController controllerForNoTrigger;

        public AnimControllers(RuntimeAnimatorController cft, RuntimeAnimatorController cfnt)
        {
            this.controllerForTrigger = cft;
            this.controllerForNoTrigger = cfnt;
        }
    }

    [Tooltip("The Tilemap to draw onto")]
    public Tilemap tilemap;
    public Tilemap tilemapBackground;
    [Tooltip("The Tile to draw (use a RuleTile for best results)")]
    public TileBase edgeTile;

    public TileBase m1911Tile;
    public TileBase mp40Tile;
    public TileBase shotgunTile;

    public TileBase shield1Tile;
    public TileBase shield2Tile;
    public TileBase shield3Tile;

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

    //public Sprite ch_m1911;
    //public Sprite ch_mp40;
    //public Sprite ch_shotgun;
    public Sprite ch_shield1;
    public Sprite ch_shield2;
    public Sprite ch_shield3;

    public Animator animator;
    public RuntimeAnimatorController ch_m1911TriggerAnimCtrl;
    public RuntimeAnimatorController ch_m1911NoTriggerAnimCtrl;
    public RuntimeAnimatorController ch_mp40TriggerAnimCtrl;
    public RuntimeAnimatorController ch_mp40NoTriggerAnimCtrl;
    public RuntimeAnimatorController ch_shotgunTriggerAnimCtrl;
    public RuntimeAnimatorController ch_shotgunNoTriggerAnimCtrl;


    Dictionary<Vector3, ToolType> tools;
    public ToolType currentTool;
    bool isDefensive;
    Dictionary<ToolType, Sprite> skins;
    Dictionary<ToolType, AnimControllers> animCtrls;

    Camera mainCamera;
    public GameObject mainCharacter;

    float cellSizeX;
    float cellSizeY;

    Vector3 topRightPoint;
    Vector3 topRightWorldCoords;
    Vector3Int topRightGridCoords;
    Vector3 offsetToClosestCorner;

    public bool isTrigger;
    public int triggerStartFrame;

    // NU schimba dimensiunea ecranului la runtime !!
    void Start()
    {
        int minSize = Mathf.Min(Screen.height, Screen.width);
        //grid.cellSize = new Vector3(minSize / 1920.0f, minSize / 1000.0f, 0);

        cellSizeX = grid.cellSize.x * transform.localScale.x;
        cellSizeY = grid.cellSize.y * transform.localScale.y;

        mainCamera = FindObjectOfType<Camera>();

        mainCamera.orthographicSize = 5;

        topRightPoint = new Vector3((float)mainCamera.orthographicSize * Screen.width / Screen.height, mainCamera.orthographicSize, 0) - grid.transform.position + mainCamera.transform.position;
        topRightWorldCoords = ClosestCorner(topRightPoint, cellSizeX, cellSizeY);
        topRightGridCoords = new Vector3Int((int)(topRightWorldCoords.x / cellSizeX), (int)(topRightWorldCoords.y / cellSizeY), 0);
        offsetToClosestCorner = topRightPoint - topRightWorldCoords;
        grid.transform.position += offsetToClosestCorner;

        tools = new Dictionary<Vector3, ToolType>();

        tools[RelativeToTopRight(new Vector3(2, 1, 0))] = ToolType.M1911;
        tools[RelativeToTopRight(new Vector3(2, 2, 0))] = ToolType.MP40;
        tools[RelativeToTopRight(new Vector3(2, 3, 0))] = ToolType.SHOTGUN;

        tools[RelativeToTopRight(new Vector3(1, 1, 0))] = ToolType.SHIELD_1;
        tools[RelativeToTopRight(new Vector3(1, 2, 0))] = ToolType.SHIELD_2;
        tools[RelativeToTopRight(new Vector3(1, 3, 0))] = ToolType.SHIELD_3;

        currentTool = ToolType.M1911;
        isDefensive = false;

        skins = new Dictionary<ToolType, Sprite>();

        //skins[ToolType.M1911] = ch_m1911;
        //skins[ToolType.MP40] = ch_mp40;
        //skins[ToolType.SHOTGUN] = ch_shotgun;
        skins[ToolType.SHIELD_1] = ch_shield1;
        skins[ToolType.SHIELD_2] = ch_shield2;
        skins[ToolType.SHIELD_3] = ch_shield3;


        animCtrls = new Dictionary<ToolType, AnimControllers>();

        animCtrls[ToolType.M1911] = new AnimControllers(ch_m1911TriggerAnimCtrl, ch_m1911NoTriggerAnimCtrl);
        animCtrls[ToolType.MP40] = new AnimControllers(ch_mp40TriggerAnimCtrl, ch_mp40NoTriggerAnimCtrl);
        animCtrls[ToolType.SHOTGUN] = new AnimControllers(ch_shotgunTriggerAnimCtrl, ch_shotgunNoTriggerAnimCtrl);


        tilemap.SetTile(RelativeToTopRight(new Vector3Int(0, 0, 0)), menuTopRight);
        tilemap.SetTile(RelativeToTopRight(new Vector3Int(0, 1, 0)), menuRight);
        tilemap.SetTile(RelativeToTopRight(new Vector3Int(0, 2, 0)), menuRight);
        tilemap.SetTile(RelativeToTopRight(new Vector3Int(0, 3, 0)), menuRight);
        tilemap.SetTile(RelativeToTopRight(new Vector3Int(0, 4, 0)), menuBottomRight);

        tilemap.SetTile(RelativeToTopRight(new Vector3Int(3, 0, 0)), menuTopLeft);
        tilemap.SetTile(RelativeToTopRight(new Vector3Int(3, 1, 0)), menuLeft);
        tilemap.SetTile(RelativeToTopRight(new Vector3Int(3, 2, 0)), menuLeft);
        tilemap.SetTile(RelativeToTopRight(new Vector3Int(3, 3, 0)), menuLeft);
        tilemap.SetTile(RelativeToTopRight(new Vector3Int(3, 4, 0)), menuBottomLeft);

        tilemap.SetTile(RelativeToTopRight(new Vector3Int(1, 0, 0)), menuTop);
        tilemap.SetTile(RelativeToTopRight(new Vector3Int(2, 0, 0)), menuTop);
        tilemap.SetTile(RelativeToTopRight(new Vector3Int(1, 4, 0)), menuBottom);
        tilemap.SetTile(RelativeToTopRight(new Vector3Int(2, 4, 0)), menuBottom);


        tilemap.SetTile(RelativeToTopRight(new Vector3Int(2, 1, 0)), m1911Tile);
        tilemap.SetTile(RelativeToTopRight(new Vector3Int(2, 2, 0)), mp40Tile);
        tilemap.SetTile(RelativeToTopRight(new Vector3Int(2, 3, 0)), shotgunTile);

        tilemap.SetTile(RelativeToTopRight(new Vector3Int(1, 1, 0)), shield1Tile);
        tilemap.SetTile(RelativeToTopRight(new Vector3Int(1, 2, 0)), shield2Tile);
        tilemap.SetTile(RelativeToTopRight(new Vector3Int(1, 3, 0)), shield3Tile);


        tilemapBackground.SetTile(RelativeToTopRight(new Vector3Int(1, 1, 0)), menuBackground);
        tilemapBackground.SetTile(RelativeToTopRight(new Vector3Int(1, 2, 0)), menuBackground);
        tilemapBackground.SetTile(RelativeToTopRight(new Vector3Int(1, 3, 0)), menuBackground);

        tilemapBackground.SetTile(RelativeToTopRight(new Vector3Int(2, 1, 0)), menuBackground);
        tilemapBackground.SetTile(RelativeToTopRight(new Vector3Int(2, 2, 0)), menuBackground);
        tilemapBackground.SetTile(RelativeToTopRight(new Vector3Int(2, 3, 0)), menuBackground);
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

    Vector3Int RelativeToTopRight(Vector3Int gridCoords)
    {
        return new Vector3Int(topRightGridCoords.x - 1, topRightGridCoords.y - 1, 0) - gridCoords;
    }
    Vector3 RelativeToTopRight(Vector3 gridCoords)
    {
        return new Vector3(topRightGridCoords.x - 1, topRightGridCoords.y - 1, 0) - gridCoords;
    }


    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        if (CrossPlatformInputManager.GetButtonDown("Fire") && !isDefensive)
        {
            isTrigger = true;
            triggerStartFrame = Time.frameCount;
        }

        //if (Input.GetKey(KeyCode.Space))
        if (!isDefensive)
        {
            if (CrossPlatformInputManager.GetButton("Fire"))
            {
                mainCharacter.GetComponent<Animator>().runtimeAnimatorController = animCtrls[currentTool].controllerForTrigger;
                isTrigger = true;
            }
            else
            {
                mainCharacter.GetComponent<Animator>().runtimeAnimatorController = animCtrls[currentTool].controllerForNoTrigger;
                isTrigger = false;
            }
        } else
        {
            mainCharacter.GetComponent<SpriteRenderer>().sprite = skins[currentTool];
            mainCharacter.GetComponent<Animator>().runtimeAnimatorController = null;
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 _mousePos = new Vector3(mousePos.x, mousePos.y, 0);
        Vector3Int mouseCellInt = grid.WorldToCell(_mousePos);
        Vector3 mouseCell = new Vector3(mouseCellInt.x, mouseCellInt.y, 0);

        if (Input.GetMouseButtonDown(0) && tools.ContainsKey(mouseCell))
        {
            currentTool = tools[mouseCell];
            //mainCharacter.GetComponent<SpriteRenderer>().sprite = skins[tools[mouseCell]];
            isDefensive = (
                currentTool == ToolType.SHIELD_1 ||
                currentTool == ToolType.SHIELD_2 ||
                currentTool == ToolType.SHIELD_3
            );
        }
    }
}

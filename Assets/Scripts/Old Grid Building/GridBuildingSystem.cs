//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Tilemaps;
//using UnityEngine.EventSystems;

//public enum TileType
//{
//    Empty,
//    White,
//    Green,
//    Red
//}

//public class GridBuildingSystem : MonoBehaviour
//{
//    public static GridBuildingSystem current;

//    public GridLayout gridLayout;
//    public Tilemap MainTilemap;
//    public Tilemap TempTilemap;

//    private static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

//    private Building temp;
//    private Vector3 prevPos;
//    private BoundsInt prevArea;

//    #region Unity Methods
//    private void Awake()
//    {
//        current = this;
//    }

//    private void Start()
//    {
//        string tilePath = @"Tiles\";
//        tileBases.Add(TileType.Empty, null);
//        tileBases.Add(TileType.White, Resources.Load<TileBase>(tilePath + "White"));
//        tileBases.Add(TileType.Green, Resources.Load<TileBase>(tilePath + "Green"));
//        tileBases.Add(TileType.Red, Resources.Load<TileBase>(tilePath + "Red"));
//    }

//    private void Update()
//    {
//        if (!temp)
//        {
//            return;
//        }

//        if (Input.GetMouseButton(0))
//        {
//            if (EventSystem.current.IsPointerOverGameObject(0))
//            {
//                return;
//            }

//            if (!temp.Placed)
//            {
//                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//                Vector3Int cellPos = gridLayout.LocalToCell(touchPos);

//                if (prevPos != cellPos)
//                {
//                    temp.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos
//                        + new Vector3(.5f, .5f, 0f));
//                    prevPos = cellPos;
//                    FollowBuilding();
//                }
//            }

            
//        }

//        else if (Input.GetKeyDown(KeyCode.Space))
//        {
//            Debug.Log("Trying to place...");
//            if (temp.CanBePlaced())
//            {
//                temp.Place();
//                Debug.Log("Building placed.");
//            }
//            else
//            {
//                Debug.LogWarning("Cannot place here!");
//            }
//        }
//        else if (Input.GetKeyDown(KeyCode.Escape))
//        {
//            ClearArea();
//            Destroy(temp.gameObject);
//        }
//    }

//    #endregion

//    #region Tilemap Management
//    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
//    {
//        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
//        int counter = 0;

//        foreach (var v in area.allPositionsWithin)
//        {
//            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
//            array[counter] = tilemap.GetTile(pos);
//            counter++;

//            Debug.Log($"Tile under building: {tilemap.GetTile(pos)}");
//        }

//        return array;
//    }

//    private static void SetTilesBlock(BoundsInt area, TileType type, Tilemap tilemap)
//    {
//        int size = area.size.x * area.size.y * area.size.z;
//        TileBase[] tileArray = new TileBase[size];
//        FillTiles(tileArray, type);
//        tilemap.SetTilesBlock(area, tileArray);
//    }

//    private static void FillTiles(TileBase[] arr, TileType type)
//    {
//        for (int i = 0; i < arr.Length; i++)
//        {
//            arr[i] = tileBases[type];
//        }
//    }

//    #endregion

//    #region Building Placement

//    public void InitializeWithBuilding(GameObject building)
//    {
//        temp = Instantiate(building, Vector3.zero, Quaternion.identity).GetComponent<Building>();
//        FollowBuilding();
//    }

//    private void ClearArea()
//    {
//        TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];
//        FillTiles(toClear, TileType.Empty);
//        TempTilemap.SetTilesBlock(prevArea, toClear);
//    }

//    private void FollowBuilding()
//    {
//        ClearArea();

//        temp.area.position = gridLayout.WorldToCell(temp.gameObject.transform.position);
//        BoundsInt buildingArea = temp.area;

//        TileBase[] baseArray = GetTilesBlock(buildingArea, MainTilemap);

//        int size = baseArray.Length;
//        TileBase[] tileArray = new TileBase[size];

//        for (int i = 0; i < baseArray.Length; i++)
//        {
//            if (baseArray[i] == tileBases[TileType.White])
//            {
//                tileArray[i] = tileBases[TileType.Green];
//            }
//            else
//            {
//                FillTiles(tileArray, TileType.Red);
//                break;
//            }
//        }
//        TempTilemap.SetTilesBlock(buildingArea, tileArray);
//        prevArea = buildingArea;
//    }

//    public bool CanTakeArea(BoundsInt area)
//    {
//        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);
//        foreach (var b in baseArray)
//        {
//            if (b != null)
//            {
//                Debug.Log($"Comparing tile: {b.name} vs White: {tileBases[TileType.White]?.name}");
//            }

//            if (b != tileBases[TileType.White])
//            {
//                Debug.LogWarning("Tile mismatch! Cannot place here.");
//                return false;
//            }
//        }

//        return true;
//    }

//    public void TakeArea(BoundsInt area)
//    {
//        SetTilesBlock(area, TileType.Empty, TempTilemap);
//        SetTilesBlock(area, TileType.Green, MainTilemap);
//    }

//    #endregion


//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Create a grid where the game takes place
public class GridController : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab;
    List<TileGameObject> tiles = new List<TileGameObject>();
    public float xDistance = 1f, xOffset = 0.422f, yDistance=1.75f; //old 2

    //Generate the grid and store the tiles GO in a list
    public void GenerateGrid(int high, int length)
    {
        bool leftGrid = true;
        int id = 0;
        for (int y = 0; y < high; y++)
        {
            for (int x = 0; x < length; x++)
            {
                TileGameObject tileGO = Instantiate(tilePrefab, new Vector3(x * xDistance + (leftGrid==true ? 0:xOffset), y * yDistance, 0.1f), Quaternion.identity, this.transform).GetComponent<TileGameObject>();
                if (!leftGrid) tileGO.GetComponent<SpriteRenderer>().flipX = true;
                tileGO.Initialize(id, new Vector2Int(x * 10 + (leftGrid == true ? 0 : 5), y * 10));
                tiles.Add(tileGO);
                id++;
            }
            leftGrid = !leftGrid;
        }

        GenerateClearTileList();
    }

    public void ClearTiles()
    {
        foreach (TileGameObject tile in tiles)
        {
            tile.SetWalkable(true);
        }
    }

    //Create a clean list of tiles from the tile GO and store it in AStarUtils
    private void GenerateClearTileList()
    {
        List<Tile> tileList = new List<Tile>();

        int id = 0;
        foreach (TileGameObject tileGameObject in tiles)
        {
            Tile tile = new Tile(tileGameObject);
            tileList.Add(tile);
            id++;
        }
        AStarUtils.ClearTileList = tileList;
    }
}

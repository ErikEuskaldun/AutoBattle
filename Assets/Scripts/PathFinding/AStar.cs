using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A* logic
public class AStar
{
    //Variables
    protected List<Tile> tiles = new List<Tile>();
    protected List<Tile> actualPath = new List<Tile>();
    protected Tile endTile;

    //Initialize pathfinding by assigning the base values
    protected AStar(List<Tile> tiles)
    {
        this.tiles = tiles;
        foreach (Tile t in tiles)
            SetNeighbors(t);
    }

    //Assign the neighbors to the tile
    private void SetNeighbors(Tile tile)
    {
        int posX = tile.gameObject.Position.x;
        int posY = tile.gameObject.Position.y;
        Tile t = default;

        for (int y = -1; y <= 1; y++)
            for (int i = 0; i <= 1; i++)
            {
                Vector2Int v = new Vector2Int(posX + ((y == 0 ? 10 : 5) * (i == 0 ? -1 : 1)), posY + (y * 10));
                t = FindTile(v);
                if (t != null)
                    tile.neighbors.Add(t);
            }
    }

    //Find a tile from the local list of tiles based on a position
    protected Tile FindTile(Vector2Int position)
    {
        foreach (Tile item in tiles)
            if (item.gameObject.Position == position)
                return item;
        return null;
    }

    //Find a tile from the local list of tiles based on an id
    protected Tile FindTile(int id)
    {
        foreach (Tile item in tiles)
            if (item.gameObject.Id == id)
                return item;
        return null;
    }

    //Clear the tile data for a new search
    protected void ClearTiles()
    {
        foreach (Tile t in tiles)
            t.Clear();
    }

    //Find a path to the specified tile and return the path
    protected List<Tile> FindPath(Tile standingTile, Tile endTile)
    {
        ClearTiles();

        Tile actualTile = standingTile;

        this.endTile = endTile;

        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        //Starting Tile
        actualTile.gCost = AStarUtils.CalculateDistance(actualTile, endTile);
        actualTile.hCost = 0;
        actualTile.CalculateFCost();
        Tile tile = actualTile;
        openList.Add(tile);

        while (tile != endTile)
        {
            foreach (Tile t in tile.neighbors)
            {
                if (closedList.Contains(t))
                    continue;
                if (!t.gameObject.isWalkable)
                {
                    closedList.Add(t);
                    continue;
                }
                //Calculate cost
                int newGCost = AStarUtils.CalculateDistance(t, endTile);
                if (newGCost < t.gCost)
                {
                    t.parentTile = tile;
                    t.gCost = newGCost;
                }
                t.hCost = t.parentTile.hCost + 1;

                t.CalculateFCost();
                if (!openList.Contains(t)) openList.Add(t);
            }
            openList.Remove(tile);
            closedList.Add(tile);
            if (openList.Count != 0)
                tile = GetMinFCost(openList);
            else
            {
                actualPath = new List<Tile>();
                Debug.Log("No path [Destination locked]");
                return null;
            }
        }
        return GeneratePath();
    }

    //Generate a path from the last tile to the actual
    private List<Tile> GeneratePath()
    {
        actualPath = new List<Tile>();
        List<Tile> path = new List<Tile>();
        Tile tile = endTile;

        while (tile.parentTile != null)
        {
            path.Add(tile);
            tile = tile.parentTile;
        }

        path.Reverse();
        actualPath = path;

        return path;
    }

    //Randomly select the tile with the lowest F cost among all tiles with the same cost
    private Tile GetMinFCost(List<Tile> openList)
    {
        int FCost = int.MaxValue;
        List<Tile> minFTiles = new List<Tile>();

        foreach (Tile t in openList)
        {
            if (t.fCost < FCost)
            {
                minFTiles = new List<Tile>();
                FCost = t.fCost;
                minFTiles.Add(t);
            }
            else if (t.fCost == FCost)
                minFTiles.Add(t);
        }

        Tile tile = minFTiles[Random.Range(0, minFTiles.Count - 1)];
        return tile;
    }
}

using System.Collections.Generic;
using UnityEngine;

//More accessible A* script for creature use
public class PathFinding : AStar
{
    Creature creature;
    //Current creature standing tile
    private Tile standingTile;
    //Properties
    public Tile StandingTile { get => standingTile; }

    //Constructor calls AStar
    public PathFinding(List<Tile> tiles, Creature creatureReference) : base(tiles)
    {
        creature = creatureReference;
    }

    //Changes the standing tile to a new tile and sets the new tile as unable to walk
    public void MoveToTile(Tile tile)
    {
        if (standingTile != null)
            standingTile.gameObject.SetWalkable(true);
        SetStandingTile(tile);
    }

    public void SetStandingTile(Tile tile)
    {
        standingTile = tile;
        standingTile.gameObject.SetWalkable(false);
    }

    public void Void()
    {
        if (standingTile == null)
            return;
        standingTile.gameObject.SetWalkable(true);
        standingTile = null;
    }

    //Initialize the creature at a random grid position in the team zone
    public Vector3 InstantiatePlayerRandom(Team team)
    {
        Tile randomTile;
        do
        {
            randomTile = tiles[Random.Range(0, tiles.Count - 1)];
        } while (!randomTile.gameObject.isWalkable || (randomTile.gameObject.Zone != team));

        MoveToTile(randomTile);
        
        return standingTile.gameObject.transform.position;
    }

    public void InstantiatePlayerTileID(int id)
    {
        Tile tile = FindTile(id);
        MoveToTile(tile);
    }

    //Find the nearest path to get in range of an opponent
    public List<Tile> FindBestPathAllOponents(List<KeyValuePair<Creature, int>> opponents, int range)
    {
        if (range > 2)
            range = 2;
        //standingTile.gameObject.isWalkable = true;
        List<Tile> tilesToCheck = new List<Tile>();
        List<Tile> bestPath = null;
        int bestPathSteps = int.MaxValue;

        foreach (KeyValuePair<Creature, int> target in opponents)
        {
            Creature c = target.Key;
            Tile targetTileLocal = FindTile(c.StandingTile.gameObject.Id);
            RecursiveTilesToCheck(ref tilesToCheck, targetTileLocal, range);
        }

        foreach (Tile tile in tilesToCheck)
        {
            if (AStarUtils.CalculateDistance(standingTile, tile) < bestPathSteps)
            {
                List<Tile> checkPath = FindPath(standingTile, tile);
                if (checkPath == null)
                    continue;
                if (checkPath.Count < bestPathSteps)
                {
                    bestPathSteps = checkPath.Count;
                    bestPath = checkPath;
                }
            }
        }

        return bestPath;
    }

    //Extension of FindBestPathAllOponents
    private void RecursiveTilesToCheck(ref List<Tile> tilesToCheck, Tile target, int times)
    {
        if (times == 0)
            return;
        foreach (Tile tile in target.neighbors)
        {
            RecursiveTilesToCheck(ref tilesToCheck, tile, times - 1);
            if (!tilesToCheck.Contains(tile) && tile.gameObject.isWalkable)
            {
                tilesToCheck.Add(tile);
            }
        }
    }
}

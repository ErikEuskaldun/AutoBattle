using System.Collections.Generic;
using UnityEngine;

//Static A* script for convenient use of certain functions and variables across all scripts
public static class AStarUtils
{
    //Variables
    private static List<Tile> clearTileList = new List<Tile>();
    //Properties
    public static List<Tile> ClearTileList { set => clearTileList = value; }

    //Calculate the distance in tiles between two tiles
    public static int CalculateDistance(Tile a, Tile b)
    {
        int yDistance = Mathf.Abs(a.gameObject.Position.y - b.gameObject.Position.y);
        float xDistance = Mathf.Abs(a.gameObject.Position.x - b.gameObject.Position.x);
        xDistance = xDistance - yDistance * 0.5f;
        xDistance = xDistance < 0 ? 0 : xDistance;

        int distance = (int)xDistance + (int)yDistance;

        return distance/10;
    }

    //Get a new list of tiles based on those from the class
    public static List<Tile> GetClearTileList()
    {
        List<Tile> tileList = new List<Tile>();

        foreach (Tile t in clearTileList)
            tileList.Add(new Tile(t.gameObject));

        return tileList;
    }
}

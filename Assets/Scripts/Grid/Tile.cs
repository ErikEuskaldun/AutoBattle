using System.Collections.Generic;

//Individual tile info for A* usage
public class Tile
{
    //Variables
    public int gCost = int.MaxValue;
    public int hCost = 0;
    public int fCost = 0;
    public Tile parentTile;
    //References
    public List<Tile> neighbors = new List<Tile>();
    public TileGameObject gameObject;

    //Constructor that assigns the GameObject to which this tile is linked
    public Tile(TileGameObject tileGameObject)
    {
        gameObject = tileGameObject;
    }

    //Clear all values
    public void Clear()
    {
        gCost = int.MaxValue;
        hCost = 0;
        fCost = 0;
        parentTile = null;
    }

    //Calculates the F cost based on G + H
    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
}

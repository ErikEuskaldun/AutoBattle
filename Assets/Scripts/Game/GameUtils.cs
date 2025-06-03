using UnityEngine;

//Static class with utility functions for general use
public static class GameUtils
{
    //Get the representative color for each type
    public static Color GetTypeColor(Type type)
    {
        switch (type)
        {
            case Type.Null:
                return Color.white;
            case Type.Water:
                return Color.blue;
            case Type.Fire:
                return Color.red;
            case Type.Grass:
                return Color.green;
            case Type.Dark:
                return Color.gray;
            case Type.Light:
                return Color.yellow;
            default:
                return Color.white;
        }
    }

    public static int GetPrice(CreatureScriptable creature)
    {
        int price = (int)creature.rarity;
        if (creature.phase == 2)
            price *= 2;
        else if (creature.phase == 3)
            price *= 4;

        return price;
    }

    public static Color GetRarityColor(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common:
                return Color.white;
            case Rarity.Uncommon:
                return Color.green;
            case Rarity.Rare:
                return Color.blue;
            case Rarity.Epic:
                return new Color(0.5f,0,1);
            case Rarity.Legendary:
                return new Color(1, 0.5f, 0);
            default:
                return Color.black;
        }
    }
}

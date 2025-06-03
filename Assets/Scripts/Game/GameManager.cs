using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int high, length;
    public GridController grid;
    public bool isGameActive = false;
    public event Action<bool> OnGameStateChange;

    private void Awake()
    {
        grid.GenerateGrid(high, length);
        GetComponent<CreatureSpawner>().GenerateCreatures();
        GameVariables.ResetVariables();
        GameElements.gameManager = this;
        FindFirstObjectByType<Shop>().ReloadShop();
    }

    public void StartGame()
    {
        if (isGameActive || GameElements.GetCreatures(Team.Player).Count==0)
            return;
        isGameActive = true;
        OnGameStateChange.Invoke(isGameActive);

        foreach (Creature creature in GameElements.GetCreaturesAll())
        {
            creature.StartMoving();
        }

        foreach (Creature creature in GameElements.GetCreatures(Team.Player))
        {
            creature.GetComponent<PlayerCreature>().StartDraging();
            creature.GetComponent<Collider2D>().enabled = false;
        }
    }

    public void ResetScenary()
    {
        FindFirstObjectByType<CreatureSpawner>().GenerateCreatures();
        Player player = FindFirstObjectByType<Player>();
        player.SetCreatures();
        player.DeployedChanged();
    }

    public void EndGame()
    {
        isGameActive = false;
        OnGameStateChange.Invoke(isGameActive);
        //Destroy Remains
        List<Creature> remainingCreatures = GameElements.GetCreaturesAll();

        foreach (Creature creature in remainingCreatures)
        {
            GameElements.DieCreature(creature);
            creature.DestroyCreature();
        }
        DeleteAttacks();
        GameElements.ClearDeadCreatures();
        Shop shop = FindFirstObjectByType<Shop>();
        shop.ReloadShop();
        shop.GiveXp(1);
    }

    private void DeleteAttacks()
    {
        GameObject projectileController = GameObject.FindGameObjectWithTag("ProjectileController");
        foreach (Transform child in projectileController.transform)
        {
            child.GetComponent<Projectile>().Destroy();
        }
    }

    public void MoveCreatureBenchToBench(PlayerCreature creatureA, PlayerCreature creatureB)
    {
        BenchSlot benchA = creatureA.GetBench();
        BenchSlot benchB = creatureB.GetBench();
        creatureB.UnBench();

        creatureA.Drop(benchB);
        creatureB.Drop(benchA);
    }

    public void MoveCreatureTileToTile(PlayerCreature creatureA, PlayerCreature creatureB)
    {
        TileGameObject tileA = creatureA.StandingTile.gameObject;
        TileGameObject tileB = creatureB.StandingTile.gameObject;
        creatureA.AI.Pathfinding.Void();

        creatureB.Drop(tileA);
        creatureA.Drop(tileB);
    }

    public void MoveCreatureTileToBench(PlayerCreature creatureTile, PlayerCreature creatureBench)
    {
        TileGameObject tile = creatureTile.StandingTile.gameObject;
        BenchSlot bench = creatureBench.GetBench();
        creatureBench.UnBench();
        creatureTile.AI.Pathfinding.Void();
        creatureTile.ui.ClearLine();
        GameElements.RemoveCreature(creatureTile);

        creatureBench.Drop(tile);
        creatureTile.Drop(bench);
    }
}

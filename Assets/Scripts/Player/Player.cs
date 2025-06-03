using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int credits = 0;
    public int maxDeploySlots = 1;
    public List<PlayerCreature> creatures = new List<PlayerCreature>();
    public CreatureSpawner spawner;
    public Bench bench;

    public int AvailableDeploySlots { get => maxDeploySlots - GameElements.GetCreatures(Team.Player).Count; }

    public event Action<int> OnCreditsChange;
    public event Action<int, int> OnDeployedChange;

    public void Start()
    {
        spawner = FindFirstObjectByType<CreatureSpawner>();
        bench = FindFirstObjectByType<Bench>();

        InstantiateInGrid(spawner.GetRandomCreature());
        OnCreditsChange.Invoke(credits);
        OnDeployedChange.Invoke(GameElements.GetCreatures(Team.Player).Count, maxDeploySlots);
    }

    public void DeployedChanged()
    {
        OnDeployedChange.Invoke(GameElements.GetCreatures(Team.Player).Count, maxDeploySlots);
    }

    public PlayerCreature InstantiateInGrid(CreatureScriptable creatureInfo)
    {
        PlayerCreature newCreature = spawner.GeneratePlayerCreature(creatureInfo);
        GameElements.AddCreature(newCreature);
        newCreature.AI.SetInRandomPosition();
        newCreature.DrawLine();
        creatures.Add(newCreature);

        return newCreature;
    }

    public bool BuyCreature(CreatureScriptable boughtCreature)
    {
        bool canBuy = false;
        int price = GameUtils.GetPrice(boughtCreature);

        if (credits >= price) //TODO: Check if have space in bench
        {
            BenchSlot freeBench = bench.GetFreeBenchSpace();
            if(freeBench!=null)
            {
                AddCredits(-price);
                InstantiateCreatureInBench(boughtCreature, freeBench);
                canBuy = true;
            }
        }

        return canBuy;
    }

    private void InstantiateCreatureInBench(CreatureScriptable creatureInfo, BenchSlot bench)
    {
        PlayerCreature newCreature = spawner.GeneratePlayerCreature(creatureInfo);
        creatures.Add(newCreature);
        newCreature.InstantiateInBench(bench);
    }

    public void SetCreatures()
    {
        foreach (PlayerCreature creature in creatures)
        {
            if(!creature.IsBenched)
                creature.Respawn();
        }
    }

    public void AddCredits(int value)
    {
        credits += value;
        OnCreditsChange.Invoke(credits);
    }

    public void MaxDeploySlotIncrease(int cuantity)
    {
        maxDeploySlots += cuantity;
        DeployedChanged();
    }

    public void Evolve(List<PlayerCreature> sacrifices)
    {
        CreatureScriptable evolution = sacrifices[0].Evolution;
        bool isSomeoneInGrid = false;
        TileGameObject standingTile = default;

        foreach (PlayerCreature sacrifice in sacrifices)
        {
            if (!sacrifice.IsBenched)
            {
                isSomeoneInGrid = true;
                standingTile = sacrifice.StandingTile.gameObject;
            }
            creatures.Remove(sacrifice);
            sacrifice.Delete();
        }

        if(isSomeoneInGrid)
        {
            PlayerCreature creatureEvo = InstantiateInGrid(evolution);
            creatureEvo.Drop(standingTile);
        }
        else 
            InstantiateCreatureInBench(evolution, bench.GetFreeBenchSpace());
    }
}

using System.Collections.Generic;
using UnityEngine;

public class CreatureSpawner : MonoBehaviour
{
    public GameObject creaturePrefab, playerCreaturePrefab;
    [SerializeField] Transform spawnLocation;
    public List<CreatureScriptable> creatures;
    public int creatureCuantity = 5;

    public void GenerateCreatures()
    {
        GenerateCreatures(creatureCuantity);
    }

    public void TEST_UpgradeDificulty()
    {
        if (creatureCuantity != 5)
            creatureCuantity++;
    }

    public void GenerateCreatures(int cuantity)
    {
        GameElements.Initialize();
        for (int i = 0; i < cuantity; i++)
            InstantantiateEnemyCreature();
    }

    public Creature InstantantiateEnemyCreature()
    {
        Creature creature;
        creature = Instantiate(creaturePrefab, spawnLocation).GetComponent<Creature>();

        creature.Initialize(GetRandomCreature(), Team.Enemy);
        creature.AI.SetInRandomPosition();
        GameElements.AddCreature(creature);

        return creature;
    }

    public PlayerCreature GeneratePlayerCreature(CreatureScriptable creatureInfo)
    {
        PlayerCreature creature;
        creature = Instantiate(playerCreaturePrefab, spawnLocation).GetComponent<PlayerCreature>();
        creature.Initialize(creatureInfo, Team.Player);

        return creature;
    }

    public CreatureScriptable GetRandomCreature()
    {
        int r = Random.Range(0, creatures.Count);
        return creatures[r];
    }
}

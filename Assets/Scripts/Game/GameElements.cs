using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controller of game elements and events
public static class GameElements
{
    //Team list
    private static List<Creature> player, enemy;
    private static List<Creature> deadCreatures;
    public static GameManager gameManager;

    //Clear teams
    public static void Initialize()
    {
        player = new List<Creature>();
        enemy = new List<Creature>();
        deadCreatures = new List<Creature>();
    }

    public static List<Creature> GetDeatCreatures()
    {
        return deadCreatures;
    }

    //Adds a creature to its team
    public static void AddCreature(Creature creature)
    {
        Team team = creature.Team;
        if (team == Team.Player)
            player.Add(creature);
        else
            enemy.Add(creature);
    }

    //Gets the opposite team
    public static Team GetOpponentTeam(Team team)
    {
        switch (team)
        {
            case Team.Player:
                return Team.Enemy;
            case Team.Enemy:
                return Team.Player;
            default:
                return Team.Null;
        }
    }

    public static void DieCreature(Creature creature)
    {
        RemoveCreature(creature);
        deadCreatures.Add(creature);
    }

    //Removes a creature from its team
    public static void RemoveCreature(Creature creature)
    {
        Team team = creature.Team;
        if (team == Team.Player)
            player.Remove(creature);
        else
            enemy.Remove(creature);
    }

    //Checks if a team has won
    public static Team TestTeamWin()
    {
        if(enemy.Count ==0)
            return Team.Player;
        else if (player.Count == 0)
            return Team.Enemy;

        return Team.Null;
    }

    public static void ClearDeadCreatures()
    {
        foreach (Creature creature in deadCreatures)
            if (creature.Team == Team.Enemy)
                GameObject.Destroy(creature.gameObject);

        deadCreatures = new List<Creature>();
    }

    //Gets the list of creatures from the specified team
    public static List<Creature> GetCreatures(Team team)
    {
        if (team == Team.Player)
            return player;
        else
            return enemy;
    }

    public static Creature GetCreatureInTile(int tileId)
    {
        foreach(Creature c in GetCreaturesAll())
        {
            if (c.StandingTile.gameObject.Id == tileId)
                return c;
        }

        return null;
    }

    public static List<Creature> GetCreaturesAll()
    {
        List<Creature> creatures = new List<Creature>(enemy);
        creatures.AddRange(player);
        return creatures;
    }
}

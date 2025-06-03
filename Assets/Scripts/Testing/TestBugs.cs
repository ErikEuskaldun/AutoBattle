using System.Collections.Generic;
using UnityEngine;

public class TestBugs : MonoBehaviour
{
    public TMPro.TMP_Text txtPlayers;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        string players = "=ALIVE=\n";
        List<Creature> creatures = GameElements.GetCreaturesAll();
        List<Creature> dead = GameElements.GetDeatCreatures();
        foreach (Creature c in creatures)
        {
            players += c.Team + " " + c.CreatureName + "\n";
        }
        players += "=DEAD=\n";
        foreach (Creature c in dead)
        {
            players += c.Team + " " + c.CreatureName + "\n";
        }

        txtPlayers.text = players;
    }
}

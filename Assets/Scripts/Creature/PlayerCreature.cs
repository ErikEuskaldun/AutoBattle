using System.Collections.Generic;
using UnityEngine;

public class PlayerCreature : Creature
{
    int benchId = -1;

    public bool IsBenched { get => benchId != -1; }
    private Player player;

    private void Awake()
    {
        player = FindFirstObjectByType<Player>();
    }

    private void Start()
    {
        CheckEvolution();
    }

    private void CheckEvolution()
    {
        if (this.Phase == 3)
            return;

        List<PlayerCreature> sameCreature = new List<PlayerCreature>();
        sameCreature.Add(this);
        foreach (PlayerCreature creature in player.creatures)
        {
            if(creature.Id == this.Id && creature != this)
            {
                sameCreature.Add(creature);
                if (sameCreature.Count == 3)
                {
                    player.Evolve(sameCreature);
                    return;
                }
            }
        }
    }

    public void Drop(TileGameObject tile)
    {
        if (tile.Position.x > 35)
            SetBackInPlace();
        else if (tile.isWalkable) //ALLWAIS END HERE WHEN DROPED
        {
            if (player.AvailableDeploySlots == 0 && !GameElements.GetCreatures(Team.Player).Contains(this))
                SetBackInPlace();
            else
            {
                if (IsBenched)
                    UnBench();
                ai.SetInitialPosition(tile.Id);
                if (!GameElements.GetCreatures(Team.Player).Contains(this))
                    GameElements.AddCreature(this);
                DrawLine();
                player.DeployedChanged();
            }
        }
        else //IS SOMEONE THERE
        {
            PlayerCreature creatureInTile = GameElements.GetCreatureInTile(tile.Id).GetComponent<PlayerCreature>();
            if (creatureInTile != this)
            {
                if(IsBenched)
                    GameElements.gameManager.MoveCreatureTileToBench(creatureInTile, this);
                else
                    GameElements.gameManager.MoveCreatureTileToTile(this, creatureInTile);
            }
            else SetBackInPlace();
        }
    }

    public void Drop(BenchSlot bench)
    {
        if (bench.Creature == null) //HAVE TO END HERE
        {
            //if(GameElements.GetCreatures(Team.Player).Count == 1) To mantain at least 1 
            if (IsBenched)
                UnBench();
            bench.BenchCreature(this);
            if (GameElements.GetCreatures(Team.Player).Contains(this))
            {
                GameElements.RemoveCreature(this);
                ai.Pathfinding.Void();
                player.DeployedChanged();
            }
            benchId = bench.Id;
        }
        else
        {
            PlayerCreature creatureInBench = bench.Creature;
            if(creatureInBench!=this)
            {
                if (IsBenched)
                    GameElements.gameManager.MoveCreatureBenchToBench(this, creatureInBench);
                else
                    GameElements.gameManager.MoveCreatureTileToBench(this, creatureInBench);
            }
            else
                SetBackInPlace();
        }
    }

    public void DrawLine()
    {
        Creature targetCreature = ai.GetNearestObjective();
        if (targetCreature != null)
            ui.DrawLine(this.transform.position, targetCreature.gameObject.transform.position);
    }

    public void SetBackInPlace()
    {
        if(IsBenched)
        {
            Vector3 benchPosition = GetBench().transform.position;
            this.transform.position = new Vector3(benchPosition.x, benchPosition.y, 0f);
        }
        else
        {
            Vector3 tilePosition = StandingTile.gameObject.transform.position;
            this.transform.position = new Vector3(tilePosition.x, tilePosition.y, 0f);
            DrawLine();
        }
    }

    public void StartDraging()
    {
        ui.ClearLine();
    }

    public void Respawn()
    {
        gameObject.SetActive(true);
        isAlive = true;
        ai.SetInitialPosition(ai.InitialTile.gameObject.Id);
        actualHp = MaxHp;
        ui.ReloadHpUI(actualHp, MaxHp);
        GetComponent<Collider2D>().enabled = true;
        GetComponent<SpriteRenderer>().flipX = false;
        GameElements.AddCreature(this);
        DrawLine();
    }

    public void Delete()
    {
        if(IsBenched)
            UnBench();
        else
        {
            GameElements.RemoveCreature(this);
            ai.Pathfinding.Void();
            player.DeployedChanged();
        }
        Destroy(this.gameObject);
    }
    
    public BenchSlot GetBench()
    {
        return FindFirstObjectByType<Bench>().GetBench(benchId);
    }

    public void InstantiateInBench(BenchSlot bench)
    {
        bench.BenchCreature(this);
        benchId = bench.Id;
    }

    public void UnBench()
    {
        BenchSlot bench = GetBench();
        bench.ClearBench();
        benchId = -1;
    }
}

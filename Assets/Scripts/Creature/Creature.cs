using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//Creature entity main script
[RequireComponent(typeof(CreatureAI),typeof(CreatureUI))]
public class Creature : MonoBehaviour
{
    //Creature Info
    private CreatureScriptable creatureInfo;
    protected int actualHp;
    private Team team;
    protected bool isAlive = true;

    //Creature References
    protected CreatureAI ai;
    public CreatureUI ui;

    //Properties
    public Tile StandingTile { get => ai.Pathfinding.StandingTile; }
    public Team Team { get => team; }
    public int Id { get => creatureInfo.id; }
    public int Range { get => creatureInfo.range; }
    public float Speed { get => creatureInfo.speed; }
    public int MaxHp { get => creatureInfo.hp; }
    public string CreatureName { get => creatureInfo.creatureName; }
    public int Phase { get => creatureInfo.phase; }
    public CreatureScriptable Evolution { get => creatureInfo.evolution; }
    public MoveScriptable Move { get => creatureInfo.basicMove; }
    public Type Type { get => creatureInfo.type; }
    public float AttackSpeed { get => creatureInfo.attackSpeed; }
    public int Damage { get => creatureInfo.damage; }
    public bool IsAlive { get => isAlive; }
    public CreatureAI AI { get => ai; }

    //Initialice creature form a scriptable into a team
    public void Initialize(CreatureScriptable creatureScriptable, Team team)
    {
        ai = GetComponent<CreatureAI>();
        ui = GetComponent<CreatureUI>();

        creatureInfo = creatureScriptable;
        GetComponent<SpriteRenderer>().sprite = creatureScriptable.sprite;
        this.team = team;

        ai.Initialize();

        actualHp = MaxHp;
        ui.ReloadHpUI(actualHp, MaxHp);
        ui.TestSetTeam(team);

        if (team == Team.Enemy)
            SetViewPosition(Direction.Left);
    }

    public void StartMoving()
    {
        ai.StartAI();
    }

    public void SetViewPosition(Direction direction)
    {
        SpriteRenderer sprite = this.GetComponent<SpriteRenderer>();
        if (direction == Direction.Right)
            sprite.flipX = false;
        else if (direction == Direction.Left)
            sprite.flipX = true;
    }

    //Hurt creature
    public void ReduceHp(int value)
    {
        actualHp -= value;
        if(actualHp<=0)
            Die();

        ui.ReloadHpUI(actualHp, MaxHp);
    }

    private void Die()
    {
        GameElements.DieCreature(this);
        ai.StopAllCoroutines();
        DestroyCreature();
        TestWin();
    }

    public void DestroyCreature()
    {
        ai.ClearOcupedTiles();
        isAlive = false;
        ai.Pathfinding.Void();
        this.gameObject.SetActive(false);
    }

    //TODO: WIN CHECK BY OTHER STRIPT (GAME CONTROLLER)
    private void TestWin()
    {
        Team team = GameElements.TestTeamWin();
        if (team != Team.Null)
            FindFirstObjectByType<TestScoreboard>().TeamWin(team);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Creature Movement and action controlles (AI)
public class CreatureAI : MonoBehaviour
{
    //Variables
    private Tile initialTile;
    //References
    private PathFinding pathfinding;
    private Creature creature;
    //Combat necessary
    private Creature target = default;
    //Properties
    public PathFinding Pathfinding { get => pathfinding; }
    public Tile InitialTile { get => initialTile; }

    //Asigns references an initialices pathfinding (AStar) script. Instantiate the creature into the grid
    public void Initialize()
    {
        creature = GetComponent<Creature>();
        pathfinding = new PathFinding(AStarUtils.GetClearTileList(), creature);
    }

    public void SetInRandomPosition()
    {
        Vector3 initialPosition = pathfinding.InstantiatePlayerRandom(creature.Team);
        this.transform.position = new Vector3(initialPosition.x, initialPosition.y, 0f);

        initialTile = pathfinding.StandingTile;
    }

    //Sets initial position to the one with ID past
    public void SetInitialPosition(int tileID)
    {
        pathfinding.InstantiatePlayerTileID(tileID);
        Vector3 tilePosition = pathfinding.StandingTile.gameObject.transform.position;
        this.transform.position = new Vector3(tilePosition.x, tilePosition.y, 0f);

        initialTile = pathfinding.StandingTile;
    }

    public Creature GetNearestObjective()
    {
        List<KeyValuePair<Creature, int>> opponents = GetOpponentsAndDistanceSort();
        if (opponents.Count != 0)
            return opponents[0].Key;
        else return null;
    }

    //Starts the action loop of the creature
    public void StartAI()
    {
        StartCoroutine(FastWait());
    }

    //Decisions made to execute the best plan depending on the moment
    private void ActionLoop()
    {
        if (target!=null && target.IsAlive && CanAttackMainTarget())
            StartCoroutine(Attack());
        else if (CanAttackSomeTarget())
            StartCoroutine(Attack());
        else
        {
            Tile tile = MoveToSomeTarget();
            if (tile != null)
                StartCoroutine(Move(tile));
            else
                StartCoroutine(Wait());
        }
    }

    //Check if the current target can be attacked
    private bool CanAttackMainTarget()
    {
        int distance = AStarUtils.CalculateDistance(pathfinding.StandingTile, target.StandingTile);
        if (distance <= creature.Range)
        {
            return true;
        }
        return false;
    }

    //Get the list of opponents ordered by distance to the player
    private List<KeyValuePair<Creature, int>> GetOpponentsAndDistanceSort()
    {
        List<Creature> opponentCreatures = GameElements.GetCreatures(GameElements.GetOpponentTeam(creature.Team));
        List<KeyValuePair<Creature, int>> creatureDistance = new List<KeyValuePair<Creature, int>>();
        foreach (Creature opponent in opponentCreatures)
        {
            int distance = AStarUtils.CalculateDistance(pathfinding.StandingTile, opponent.StandingTile);
            creatureDistance.Add(new KeyValuePair<Creature, int>(opponent, distance));
        }
        creatureDistance.Sort((x, y) => x.Value.CompareTo(y.Value));

        return creatureDistance;
    }

    //Check if any target is within the creatures range
    private bool CanAttackSomeTarget()
    {
        List<KeyValuePair<Creature, int>> opponents = GetOpponentsAndDistanceSort();
        foreach (KeyValuePair<Creature, int> opponent in opponents)
        {
            if(opponent.Value<=creature.Range)
            {
                target = opponent.Key;
                return true;
            }
        }
        return false;
    }

    //Tries to move to the nearest tile where could attack an opponent
    private Tile MoveToSomeTarget()
    {
        List<KeyValuePair<Creature, int>> opponents = GetOpponentsAndDistanceSort();
        List<Tile> bestPath = pathfinding.FindBestPathAllOponents(opponents, creature.Range);
        if (bestPath == null)
            return null;
        else
            return bestPath[0];
    }

    private void CalculateViewDirection(Vector2Int initialPosition, Vector2Int targetPosition)
    {
        int xDistance = initialPosition.x - targetPosition.x;
        if (xDistance > 0)
            creature.SetViewPosition(Direction.Left);
        else if (xDistance < 0)
            creature.SetViewPosition(Direction.Right);
    }

    //Attacks the target
    private IEnumerator Attack()
    {
        //TESTING
        creature.ui.WriteState("ATTACK");

        FindFirstObjectByType<TestProjectileLauncher>().InstantantiateBasicProjectile(creature, target);
        CalculateViewDirection(creature.StandingTile.gameObject.Position, target.StandingTile.gameObject.Position);

        StartCoroutine(creature.ui.FastWait(10 / creature.AttackSpeed));
        yield return new WaitForSeconds(10 / creature.AttackSpeed);
        ActionLoop();
    }

    //Waits on site for the next action
    private IEnumerator Wait()
    {
        //TESTING
        creature.ui.WriteState("WAIT");

        pathfinding.StandingTile.gameObject.SetWalkable(false);
        StartCoroutine(creature.ui.FastWait(10 / creature.Speed));
        yield return new WaitForSeconds(10/creature.Speed);
        ActionLoop();
    }

    //For error control
    private IEnumerator FastWait()
    {
        //TESTING
        creature.ui.WriteState("FAST_WAIT");
        StartCoroutine(creature.ui.FastWait(0.1f));

        pathfinding.StandingTile.gameObject.SetWalkable(false);
        yield return new WaitForSeconds(0.1f);
        ActionLoop();
    }

    //Moves to the specified tile
    private IEnumerator Move(Tile destination)
    {
        //TESTING
        creature.ui.WriteState("MOVE");

        float startTime = Time.time;
        CalculateViewDirection(creature.StandingTile.gameObject.Position, destination.gameObject.Position);

        Vector3 startingPosition = transform.position;
        pathfinding.MoveToTile(destination);
        float time = 0;

        while (time < 1)
        {
            creature.ui.StateTime(1f-time);
            transform.position = Vector2.Lerp(startingPosition, destination.gameObject.transform.position, time);
            time += Time.deltaTime/30 * creature.Speed;
            yield return new WaitForEndOfFrame();
        }
        transform.position = pathfinding.StandingTile.gameObject.transform.position;

        float elapsedTime = Time.time - startTime;
        elapsedTime = Mathf.Round(elapsedTime * 100.0f) * 0.01f;

        ActionLoop();
    }

    public void ClearOcupedTiles()
    {
        pathfinding.StandingTile.gameObject.SetWalkable(true);
    }
}

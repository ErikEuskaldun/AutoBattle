using UnityEngine;

public class BenchSlot : MonoBehaviour
{
    [SerializeField] int id;
    [SerializeField] PlayerCreature creature = null;

    public int Id { get => id; }
    public PlayerCreature Creature { get => creature; }

    public void BenchCreature(PlayerCreature creature)
    {
        this.creature = creature;
        Vector3 benchPosition = this.transform.position;
        creature.transform.position = new Vector3(benchPosition.x, benchPosition.y, 0f);
    }

    public void ClearBench()
    {
        this.creature = null;
    }
}

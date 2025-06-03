using UnityEngine;

public class Bench : MonoBehaviour
{
    [SerializeField] BenchSlot[] benchSlots;

    private void Start()
    {
    }

    public BenchSlot GetBench(int id)
    {
        return benchSlots[id];
    }

    public BenchSlot GetFreeBenchSpace()
    {
        foreach (BenchSlot bench in benchSlots)
            if (bench.Creature == null)
                return bench;

        return null;
    }
}

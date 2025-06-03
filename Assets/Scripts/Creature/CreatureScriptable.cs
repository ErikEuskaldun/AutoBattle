using UnityEngine;

//All creature info to be used in Creature script
[CreateAssetMenu(fileName = "New Creature", menuName = "Scriptable Objects/New Creature")]
public class CreatureScriptable : ScriptableObject
{
    [Header("Basic Info")]
    public int id;
    public string creatureName;
    public Type type;

    [Header("Stats")]
    public int range;
    public float speed;
    public int ultChargeNeeded;
    public float attackSpeed;
    public int damage;
    public int hp;

    [Header("Other")]
    public Sprite sprite;
    [Range(1, 3)] public int phase;
    public Rarity rarity;
    [Tooltip("Null if dont have more phases")]
    public CreatureScriptable evolution;

    [Header("Moves")]
    public MoveScriptable basicMove;
}
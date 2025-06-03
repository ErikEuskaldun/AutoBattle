using UnityEngine;

[CreateAssetMenu(fileName = "New Move", menuName = "Scriptable Objects/New Move")]
public class MoveScriptable : ScriptableObject
{
    [Header("Basic Info")]
    public int damage = 1;
    [Tooltip("How many tiles per second travels")]
    public float speed = 2f; //2 tile each second
    [Header("Sprites")]
    public MoveSpriteInner innerSprite = MoveSpriteInner.Null;
    public MoveSpriteOuter outerSprite = MoveSpriteOuter.Null;
    [Header("Color ")]
    public bool useCreatureColor = true;
    [Tooltip("To use this color un-check 'Use Creature Color'")]
    public Color innerColor = Color.white;
    [Tooltip("To use this color un-check 'Use Creature Color'")]
    public Color outerColor = Color.white;
}

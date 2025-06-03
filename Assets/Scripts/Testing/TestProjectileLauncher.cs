using UnityEngine;

//Instantiate a projectile from a thower to the target 
public class TestProjectileLauncher : MonoBehaviour
{
    public GameObject projectilePrefab;
    [SerializeField] ProjectileSpriteList spriteList;
    public void InstantantiateBasicProjectile(Creature thrower, Creature target)
    {
        //Projectile Sprite
        MoveScriptable move = thrower.Move;
        Sprite outerSprite = spriteList.GetOuterProjectile(move.outerSprite);
        Sprite innerSprite = spriteList.GetInnerProjectile(move.innerSprite);

        Projectile projectile = Instantiate(projectilePrefab, this.transform).GetComponent<Projectile>();
        if (move.useCreatureColor)
        {
            Color creatureColor = GameUtils.GetTypeColor(thrower.Type);
            projectile.InstantiateProjectile(innerSprite, outerSprite, creatureColor);
        }
        else
            projectile.InstantiateProjectile(innerSprite, move.innerColor, outerSprite, move.outerColor);
        StartCoroutine(projectile.Throw(thrower, target));
    }
}

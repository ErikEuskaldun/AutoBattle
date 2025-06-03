using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpriteList : MonoBehaviour
{
    public Sprite[] innerProjectileSpriteList;
    public Sprite[] outerProjectileSpriteList;

    public Sprite GetInnerProjectile(MoveSpriteInner innerProjectile)
    {
        return innerProjectileSpriteList[(int)innerProjectile];
    }

    public Sprite GetOuterProjectile(MoveSpriteOuter outerProjectile)
    {
        return outerProjectileSpriteList[(int)outerProjectile];
    }
}

using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] SpriteRenderer innerSprite, outerSprite;
    private int attack = 0;

    public bool isActive = true;

    public void InstantiateProjectile(Sprite innerSprite, Sprite outerSprite, Color color)
    {
        float darken = 0.65f;
        Color outerColor = color;
        Color innerColor = new Color(outerColor.r * darken, outerColor.g * darken, outerColor.b * darken);
        InstantiateProjectile(innerSprite, innerColor, outerSprite, outerColor);
    }

    public void InstantiateProjectile(Sprite innerSprite, Color innerColor, Sprite outerSprite, Color outerColor)
    {
        this.innerSprite.sprite = innerSprite;
        this.innerSprite.color = innerColor;
        this.outerSprite.sprite = outerSprite;
        this.outerSprite.color = outerColor;
    }

    public IEnumerator Throw(Creature thrower, Creature target)
    {
        float time = 0;
        Vector2 startPosition = thrower.StandingTile.gameObject.transform.position;
        int damage = thrower.Damage;
        while (time < 1 && target.IsAlive && isActive)
        {
            transform.position = Vector2.Lerp(startPosition, target.transform.position, time);
            LookAtTarget(target.transform.position);
            time += Time.deltaTime * 2;
            yield return new WaitForEndOfFrame();
        }
        if (!isActive) //For early Destroy
            yield break;

        if (target.IsAlive)
            target.ReduceHp(damage);

        Destroy();
    }

    public void LookAtTarget(Vector3 targetPosition)
    {
        if (targetPosition != null)
        {
            Vector3 direction = targetPosition - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // Ajustar la escala para corregir el "flip"
            bool isFlip = direction.x > 0 ? false : true;
            innerSprite.flipY = isFlip;
            outerSprite.flipY = isFlip;
        }
    }

    public void Destroy()
    {
        isActive = false;
        Destroy(this.gameObject);
    }
}

using UnityEngine;

public class CreatureDraggable : MonoBehaviour
{
    Collider2D collider2d;
    SpriteRenderer sprite;
    public LayerMask hitLayers;

    private bool isDraging = false;
    public bool canInteract = true;
    PlayerCreature creature;

    private void Start()
    {
        collider2d = GetComponent<Collider2D>();
        creature = GetComponent<PlayerCreature>();
        sprite = GetComponent<SpriteRenderer>();
        if (creature.Team != Team.Player)
            canInteract = false;
    }

    private void OnMouseDown()
    {
        if (GameVariables.spriteInteractionLocked)
            return;

        isDraging = true;
        creature.StartDraging();
        sprite.sortingOrder = 200;
    }

    private void OnMouseDrag()
    {
        if (!canInteract || GameVariables.spriteInteractionLocked)
            return;

        transform.position = MouseWorldPosition();
    }

    private void OnMouseUp()
    {
        if (!canInteract || !isDraging || GameVariables.spriteInteractionLocked)
            return;

        sprite.sortingOrder = 100; //TODO: SET A FIXED ORDER LAYER DEPENDING ON THE TILE DEPTH

        isDraging = false;
        collider2d.enabled = false;

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, hitLayers);

        if (hit.collider != null)
        {
            int layerHit = hit.collider.gameObject.layer;
            if (layerHit == LayerMask.NameToLayer("Tile"))
            {
                if(GameElements.gameManager.isGameActive)
                    creature.SetBackInPlace();
                else
                {
                    TileGameObject tile = hit.transform.GetComponent<TileGameObject>();
                    creature.Drop(tile);
                }
            }
            else if(layerHit == LayerMask.NameToLayer("Bench"))
            {
                BenchSlot benchSlot = hit.transform.GetComponent<BenchSlot>();
                creature.Drop(benchSlot);
            }
        }
        else creature.SetBackInPlace();

        collider2d.enabled = true;
    }

    private Vector3 MouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
}

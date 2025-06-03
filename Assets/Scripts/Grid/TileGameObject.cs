using UnityEngine;
using TMPro;

//GameObject of the tile
public class TileGameObject : MonoBehaviour
{
    [SerializeField] Vector2Int position;
    [SerializeField] int id;
    [SerializeField] Team zone;
    //[SerializeField] Creature standingCreature = null;
    //Variables
    public bool isWalkable = true;
    private bool isMouseIn = false;
    //Properties
    public Vector2Int Position { get => position; }
    public int Id { get => id; }
    public Team Zone { get => zone; }
    //public Creature StandingCreature { get => standingCreature; }

    //UI
    [SerializeField] TMP_Text positionName;
    [SerializeField] SpriteRenderer tileBG;

    //Toggle tile walkability
    private void Update()
    {
        if (isMouseIn && Input.GetMouseButtonDown(1))
        {
            if (GameVariables.spriteInteractionLocked)
                return;
            isWalkable = !isWalkable;
            SetWalkable(isWalkable);
        }
    }

    //Initialize the class with the obtained values
    public void Initialize(int id, Vector2Int position)
    {
        this.id = id;
        this.position = position;
        //positionName.text = position.ToString();
        SetZone();
    }

    private void SetZone()
    {
        if (position.x > 40)
            zone = Team.Enemy;
        else if (position.x < 40)
            zone = Team.Player;
        else zone = Team.Null;
    }

    //Display the original color of the tile (black = non-walkable, white = walkable)
    public void Clear()
    {
        SetWalkable(isWalkable);
    }

    //Assign a color to the tile
    public void SetColor(Color color)
    {
        tileBG.color = color;
    }

    public void SetWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;
        if (isWalkable)
            tileBG.color = new Color(0f, 0f, 0f, 0f);
        else
            SetColor(Color.gray);
    }

    /*public void SetStandingCreature(Creature creature)
    {
        standingCreature = creature;
        SetWalkable(false);
    }*/

    //Show the tile as highlighted
    private void OnMouseEnter()
    {
        if (GameVariables.spriteInteractionLocked)
            return;
        isMouseIn = true;
        SetColor(Color.yellow);
    }

    //Display the original color of the tile
    private void OnMouseExit()
    {
        if (GameVariables.spriteInteractionLocked)
            return;
        isMouseIn = false;
        Clear();
    }
}

using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] TMP_Text txtCredits, txtDeployedCreatures;

    private void Awake()
    {
        Player player = GetComponent<Player>();
        player.OnCreditsChange += UpdateCredits;
        player.OnDeployedChange += UpdateDeployedCreatures;
    }

    private void UpdateCredits(int value)
    {
        txtCredits.text = value + "c";
    }

    private void UpdateDeployedCreatures(int actual, int max)
    {
        txtDeployedCreatures.text = "Creatures " + actual + "/" + max;
    }
}

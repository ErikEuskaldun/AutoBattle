using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreatureBuy : MonoBehaviour
{
    [SerializeField] CreatureScriptable creatureInfo;
    [SerializeField] Image image;
    [SerializeField] Image imgBG;
    [SerializeField] TMP_Text txtPrice;
    [SerializeField] Button button;
    [SerializeField] bool isShopOpen = false;

    public void SetCreature(CreatureScriptable scriptable)
    {
        image.color = Color.white;
        creatureInfo = scriptable;
        image.sprite = scriptable.sprite;
        txtPrice.text = GameUtils.GetPrice(scriptable) + "c";
        imgBG.color = GameUtils.GetRarityColor(scriptable.rarity);
        if(isShopOpen)
            button.interactable = true;
    }

    public void SetNull()
    {
        creatureInfo = null;
        image.sprite = null;
        txtPrice.text = "";
        button.interactable = false;
        imgBG.color = Color.gray;
        image.color = Color.clear;
    }

    public void Buy()
    {
        bool haveBought = FindFirstObjectByType<Player>().BuyCreature(creatureInfo);
        if (haveBought)
            SetNull();
    }

    public void OpenShop()
    {
        isShopOpen = true;
        button.interactable = true;
    }
}

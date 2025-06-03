using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    //UI
    [SerializeField] GameObject shopUI;
    [SerializeField] Button btnShop;
    [SerializeField] TMP_Text txtShopLevel;
    [SerializeField] TMP_Text txtShopXp;
    [SerializeField] Slider slShopXpBar;

    //Other
    [SerializeField] List<CreatureScriptable> creatureBuyList = new List<CreatureScriptable>();
    [SerializeField] List<CreatureBuy> buySlots = new List<CreatureBuy>();
    [SerializeField] Player player;
    int shopLevel = 1, shopXp = 0, shopMaxLevel = 8;

    void Start()
    {
        GameElements.gameManager.OnGameStateChange += EnableBuy;
        ReloadLevelUI();
    }

    public void BuyShopReload()
    {
        int price = 2;
        if(player.credits>=price)
        {
            player.AddCredits(-1);
            ReloadShop();
        }
    }

    public void ReloadShop()
    {
        foreach (CreatureBuy slot in buySlots)
        {
            int random = Random.Range(0, creatureBuyList.Count);
            CreatureScriptable randomCreature = creatureBuyList[random];
            slot.SetCreature(randomCreature);
        }
    }

    private void EnableBuy(bool isGameActive)
    {
        btnShop.interactable = !isGameActive;
    }

    public void OpenShop(bool isOpen)
    {
        shopUI.SetActive(isOpen);
        GameVariables.spriteInteractionLocked = isOpen;
    }

    public void BuyXp()
    {
        if (shopLevel == shopMaxLevel)
            return;

        int price = 2;
        if(player.credits>=price)
        {
            player.AddCredits(-2);
            GiveXp(2);
        }
    }

    public void GiveXp(int xp)
    {
        if (shopLevel == shopMaxLevel)
            return;

        shopXp += xp;
        if(shopXp>=XpRequired(shopLevel+1))
        {
            shopXp = shopXp - XpRequired(shopLevel + 1);
            LevelUpShop();
        }
        ReloadLevelUI();
    }

    public void ReloadLevelUI()
    {
        txtShopLevel.text = shopLevel.ToString();
        float xpPercent;
        if (shopLevel==shopMaxLevel)
        {
            txtShopXp.text = "MAX";
            xpPercent = 1;
        }
        else
        {
            txtShopXp.text = shopXp + "/" + XpRequired(shopLevel+1);
            xpPercent = (float)shopXp / XpRequired(shopLevel + 1);
        }
        slShopXpBar.value = xpPercent;
    }

    public void LevelUpShop()
    {
        if (shopLevel == shopMaxLevel)
            return;

        shopLevel++;
        switch (shopLevel)
        {
            case 2: 
            case 3:
            case 5:
            case 6:
            case 8:
                player.MaxDeploySlotIncrease(1);
                break;
            case 4: buySlots[2].OpenShop();
                break;
            case 7: buySlots[3].OpenShop();
                break;
        }
    }

    private int XpRequired(int level)
    {
        switch (level)
        {
            case 2: return 1; //+Slot
            case 3: return 5; //+Slot
            case 4: return 10; //+Shop
            case 5: return 20; //+Slot
            case 6: return 35; //+Slot
            case 7: return 55; //+Shop
            case 8: return 80; //+Slot
            default: return -1;
        }
    }

}

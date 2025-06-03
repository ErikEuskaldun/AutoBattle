using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class TestScoreboard : MonoBehaviour
{
    public TMP_Text txtScore;
    int playerPoints = 0;
    int enemyPoints = 0;

    public bool autoRun = false;
    [SerializeField] Button btnStart, btnAuto;

    //Scores and starts a new game
    public void TeamWin(Team team)
    {
        if (team == Team.Player)
        {
            playerPoints++;
            FindFirstObjectByType<Player>().AddCredits(2);
            FindFirstObjectByType<CreatureSpawner>().TEST_UpgradeDificulty();
        }
        else
        {
            enemyPoints++;
            FindFirstObjectByType<Player>().AddCredits(1);
        }
            

        txtScore.text = playerPoints + ":" + enemyPoints;

        GameElements.gameManager.EndGame();
        GameElements.gameManager.ResetScenary();
        if (autoRun)
            GameElements.gameManager.StartGame();
    }

    public void ToggleAuto()
    {
        autoRun = !autoRun;
        btnAuto.GetComponent<Animator>().SetBool("isActivated", autoRun);
        btnStart.interactable = !autoRun;
        if (autoRun == true) GameElements.gameManager.StartGame();
    }

    //Game speed manager
    public void SetSpeed(float speed)
    {
        Time.timeScale = speed;
    }
}

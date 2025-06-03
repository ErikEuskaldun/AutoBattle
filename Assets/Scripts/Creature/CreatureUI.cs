using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

//Creature UI elements controller and updater
public class CreatureUI : MonoBehaviour
{
    [SerializeField] Slider slHp;
    public SpriteRenderer testTeamColor;
    [SerializeField] LineRenderer line;
    [SerializeField] TMP_Text txtState;
    [SerializeField] Slider slState;

    //Reload HP indicator
    public void ReloadHpUI(int actualHp, int maxHp)
    {
        float value = (float)actualHp / maxHp;
        slHp.value = value;
    }

    //TODO: SHOW TEAM IN OTHER WAY
    public void TestSetTeam(Team team)
    {
        testTeamColor.color = team == Team.Player ? Color.green : Color.red;
    }

    public void DrawLine(Vector3 initialPosition, Vector3 endPosition)
    {
        line.enabled = true;
        line.SetPosition(0, initialPosition);
        line.SetPosition(1, endPosition);
    }

    public void ClearLine()
    {
        line.enabled = false;
    }

    public void WriteState(string state)
    {
        txtState.text = state;
    }

    public void StateTime(float percent)
    {
        slState.value = percent;
    }

    public IEnumerator FastWait(float seconds)
    {
        float second = 0f;
        float multiplier = 1 / seconds;
        do
        {
            slState.value = 1- (second*multiplier);
            second += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        } while (second<seconds);
        
    }
}
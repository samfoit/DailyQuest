using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    public GameObject rewardPanel;
    public GameObject background;
    public int coins = 0;

    public void SetReward(int reward)
    {
        coins = reward;
        rewardPanel.SetActive(true);
        background.SetActive(true);
    }

    public void GetReward()
    {
        PlayerStats.instance.money += coins;
        rewardPanel.SetActive(false);
        background.SetActive(false);
        Timer.instance.StopCountdown();
    }
}

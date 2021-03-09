using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerReward : MonoBehaviour
{
    public Text reward;
    public RewardManager manager;

    private void OnEnable()
    {
        reward.text = "0";
        StartCoroutine(AnimateText(manager.coins));
    }

    IEnumerator AnimateText(int amount)
    {
        int count = 0;
        for (int i = 0; i < amount; i++)
        {
            count = i + 1;
            reward.text = count.ToString();
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    public Text time;
    public Text level;
    public Text deaths;

    private void OnEnable()
    {
        time.text = GameManager.instance.time.ToString();
        level.text = PlayerStats.instance.level.ToString();
        deaths.text = GameManager.instance.deaths.ToString();
    }

    public void Continue()
    {
        PlayerStats.instance.Reset();
        GameManager.instance.GodRoll();
        SceneManager.LoadScene("Game");
    }
}

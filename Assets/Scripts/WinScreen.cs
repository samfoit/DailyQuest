using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class WinScreen : MonoBehaviour
{
    public Text time;
    public Text level;
    public Text deaths;

    public Transform startPosition;
    public Collider2D world;

    private void OnEnable()
    {
        time.text = GameManager.instance.time.ToString();
        level.text = PlayerStats.instance.level.ToString();
        deaths.text = GameManager.instance.deaths.ToString();
    }

    public void Continue()
    {
        PlayerStats.instance.gameObject.transform.localPosition = new Vector3(-41f, 3.4f, 0);
        FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D = world;
        PlayerStats.instance.Reset();
        GameManager.instance.GodRoll();
        gameObject.SetActive(false);
        GameManager.instance.isTalking = false;
        FindObjectOfType<CastleInnerExit>().appear = true;
        FindObjectOfType<CastleOuterExit>().appear = true;
    }
}

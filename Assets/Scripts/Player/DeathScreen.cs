using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    public static DeathScreen instance;
    public GameObject deathScreen;
    public Text respawn; 

    public Collider2D worldBoundary;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void Death()
    {
        PlayerStats.instance.GetComponent<SpriteRenderer>().enabled = false;
        GameManager.instance.isTalking = true;
        Time.timeScale = 0;
        Input.ResetInputAxes();
        deathScreen.SetActive(true);
        PlayerStats.instance.Reset();
        GameManager.instance.UpdateDeaths();
        LootMenu.instance.DisableLoot();
        StartCoroutine(RespawnAnimation());
    }

    IEnumerator RespawnAnimation()
    {
        respawn.text = "Respawn in 3";
        yield return new WaitForSecondsRealtime(1f);
        respawn.text = "Respawn in 2";
        yield return new WaitForSecondsRealtime(1f);
        respawn.text = "Respawn in 1";
        yield return new WaitForSecondsRealtime(0.4f);
        TryAgain();
    }

    public void TryAgain()
    {
        Time.timeScale = 1;
        PlayerStats.instance.StartAgain();
        SceneManager.LoadScene("Game");
        GameManager.instance.isTalking = false;
        PlayerController.instance.stun = false;
        deathScreen.SetActive(false);
        FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D = worldBoundary;
        PlayerStats.instance.GetComponent<SpriteRenderer>().enabled = true;
        PlayerStats.instance.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }

}

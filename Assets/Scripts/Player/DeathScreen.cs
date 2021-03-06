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
        StartCoroutine(RespawnAnimation());
    }

    IEnumerator RespawnAnimation()
    {
        respawn.text = "Respawning in 3";
        yield return new WaitForSecondsRealtime(1f);
        respawn.text = "Respawning in 2";
        yield return new WaitForSecondsRealtime(1f);
        respawn.text = "Respawning in 1";
        yield return new WaitForSecondsRealtime(0.4f);
        TryAgain();
    }

    public void TryAgain()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        for (int i = 0; i < enemies.Length; i++)
        {
            Destroy(enemies[i].gameObject);
        }
        Time.timeScale = 1;
        PlayerStats.instance.StartAgain();
        GameManager.instance.isTalking = false;
        PlayerController.instance.stun = false;
        deathScreen.SetActive(false);
        FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D = worldBoundary;
        PlayerStats.instance.GetComponent<SpriteRenderer>().enabled = true;
        PlayerStats.instance.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

        // so ugly this is all absolutely garbage code
        FindObjectOfType<CaveExit>().Restart();
        FindObjectOfType<CastleOuterExit>().Restart();
        FindObjectOfType<CastleInnerExit>().Restart();
        TriggerSpawner[] test = FindObjectsOfType<TriggerSpawner>();
        for (int i = 0; i < test.Length; i++)
        {
            test[i].Spawn();
        }
    }

}

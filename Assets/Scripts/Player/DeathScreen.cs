using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class DeathScreen : MonoBehaviour
{
    public static DeathScreen instance;
    public GameObject deathScreen;

    public Collider2D worldBoundary;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void Death()
    {
        GameManager.instance.isTalking = true;
        Time.timeScale = 0;
        Input.ResetInputAxes();
        deathScreen.SetActive(true);
        PlayerStats.instance.Reset();
        GameManager.instance.UpdateDeaths();
        LootMenu.instance.DisableLoot();
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
        PlayerStats.instance.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }

}

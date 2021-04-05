using UnityEngine;
using UnityEngine.UI;

public class Cutscene : MonoBehaviour
{
    public Image topImage;
    public Image bottomImage;

    private bool fadeIn = false;

    private void OnEnable()
    {
        DestroyAllEnemies();
        PlayerController.instance.stun = true;
        GameManager.instance.isTalking = true;
        GameManager.instance.DeactivateJoysticks();
        fadeIn = true;
    }

    private void OnDisable()
    {
        GameManager.instance.ActivateJoysticks();
        fadeIn = false;
        topImage.fillAmount = 0;
        bottomImage.fillAmount = 0;
    }

    private void Update()
    {
        if (fadeIn)
        {
            topImage.fillAmount = Mathf.MoveTowards(topImage.fillAmount, 1f, Time.unscaledDeltaTime);
            bottomImage.fillAmount = Mathf.MoveTowards(bottomImage.fillAmount, 1f, Time.unscaledDeltaTime);
        }
    }

    private void DestroyAllEnemies()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        for (int i = 0; i < enemies.Length; i++)
        {
            Destroy(enemies[i].gameObject);
        }
    }
}

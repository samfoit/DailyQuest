using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemy : MonoBehaviour
{
    private void Awake()
    {
        if (PlayerPrefs.HasKey("tutorialSlime"))
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetString("tutorialSlime", "false");
    }
}

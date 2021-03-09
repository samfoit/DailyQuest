using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public GameObject quest;

    // Update is called once per frame
    void Update()
    {
        if (OrientationManager.portrait)
        {
            quest.SetActive(true);
        }
        else
        {
            quest.SetActive(false);
        }
    }
}

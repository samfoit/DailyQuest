using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] Slider hpSlider;
    [SerializeField] Slider mpSlider;
    [SerializeField] Slider expSlider;


    public static StatsUI instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        hpSlider.value = PlayerStats.instance.currentHealth / PlayerStats.instance.maxHealth;
        mpSlider.value = PlayerStats.instance.currentMana / PlayerStats.instance.maxMana;
        if (PlayerStats.instance.level < 20)
        {
            expSlider.value = PlayerStats.instance.currentExp / PlayerStats.instance.expToNextLevel[PlayerStats.instance.level];
        }
        else
        {
            expSlider.value = 1;
        }
    }

    public void ChangeStatSliders()
    {
        hpSlider.value = PlayerStats.instance.currentHealth / PlayerStats.instance.maxHealth;
        mpSlider.value = PlayerStats.instance.currentMana / PlayerStats.instance.maxMana;
        if (PlayerStats.instance.level < 20)
        {
            expSlider.value = PlayerStats.instance.currentExp / PlayerStats.instance.expToNextLevel[PlayerStats.instance.level];
        }
        else
        {
            expSlider.value = 1;
        }
    }
}

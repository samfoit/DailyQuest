using UnityEngine;
using UnityEngine.UI;

public class QuestHeader : MonoBehaviour
{
    [SerializeField] Text levelText;
    [SerializeField] Image expFiller;

    private int level;
    private bool animateLevel = false;

    private void OnEnable()
    {
        level = PlayerStats.instance.level;
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
        UndoButton.instance.transparent = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerStats.instance.level < 20)
        {
            levelText.text = "Level " + level;

            if (level < PlayerStats.instance.level)
            {
                animateLevel = true;
            }

            if (level > PlayerStats.instance.level)
            {
                level = PlayerStats.instance.level;
            }

            if (animateLevel)
            {
                expFiller.fillAmount = Mathf.MoveTowards(expFiller.fillAmount, 1, 0.015f);
                if (expFiller.fillAmount == 1)
                {
                    level++;
                    animateLevel = false;
                    expFiller.fillAmount = 0;
                }
            }

            if (expFiller.fillAmount != PlayerStats.instance.currentExp / PlayerStats.instance.expToNextLevel[PlayerStats.instance.level] && !animateLevel)
            {
                expFiller.fillAmount = Mathf.MoveTowards(expFiller.fillAmount,
                    PlayerStats.instance.currentExp / PlayerStats.instance.expToNextLevel[PlayerStats.instance.level], 0.015f);
            }
        }
        else
        {
            levelText.text = "Level " + level;
            levelText.color = new Color(1f, 1f, 0f);
            expFiller.fillAmount = 1;
            expFiller.color = new Color(1f, 1f, 0f);
        }
    }
}

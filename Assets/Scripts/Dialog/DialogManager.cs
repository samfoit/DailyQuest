using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using Cinemachine;

public class DialogManager : MonoBehaviour
{
    public GameObject dBox;
    public Text dialogText;

    public bool dialogActive;

    public static DialogManager instance;

    public string[] dialogLines;

    public int currentLine = 0;

    public bool isAnimatingText = false;

    private bool bossText = false;

    private PlayableDirector playable;

    // Start is called before the first frame update
    void Start()
    {
        dBox.SetActive(false);
        instance = this;
        playable = FindObjectOfType<PlayableDirector>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (dialogActive && !isAnimatingText)
            {
                ShowDialog(dialogLines, bossText);
            }
            else if (dialogActive && isAnimatingText)
            {
                ShowFullText();
            }
        }
    }

    public void ShowBox()
    {
        if (dBox != null)
        {
            dBox.SetActive(true);
        }
        currentLine = 0;
        dialogActive = true;
        GameManager.instance.isTalking = true;
    }

    public void ShowDialog(string[] dialog, bool boss)
    {
        bossText = boss;
        dialogLines = dialog;
        GameManager.instance.DeactivateJoysticks();

        if (currentLine >= dialogLines.Length)
        {
            StartCoroutine(DisableDialog());
        }
        else
        {
            StartCoroutine(AnimateText(currentLine));
            currentLine++;
        }
    }

    IEnumerator SkipTextAnimation()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        if (isAnimatingText)
        {
            ShowFullText();
        }
    }

    IEnumerator AnimateText(int currentLine)
    {
        dialogText.text = "";

        foreach(char letter in dialogLines[currentLine])
        {
            isAnimatingText = true;
            dialogText.text += letter;
            yield return new WaitForSecondsRealtime(0.07f);
        }

        isAnimatingText = false;
    }

    public void ShowFullText()
    {
        int line = currentLine - 1;
        dialogText.text = dialogLines[line];
        StopAllCoroutines();
        isAnimatingText = false;
    }

    private IEnumerator DisableDialog()
    {
        FindObjectOfType<CinemachineVirtualCamera>().Follow = PlayerStats.instance.transform;
        dBox.SetActive(false);
        dialogActive = false;
        GameManager.instance.ActivateJoysticks();
        Time.timeScale = 1;

        yield return new WaitForSecondsRealtime(1f);
        if (bossText)
        {
            playable = FindObjectOfType<PlayableDirector>();
            playable.Play();
        }
        else
        {
            GameManager.instance.isTalking = false;
            PlayerController.instance.stun = false;
        }
    }

    public void DisableTalkingOnDeath()
    {
        dBox.SetActive(false);
        dialogActive = false;
        GameManager.instance.ActivateJoysticks();
        GameManager.instance.isTalking = false;
    }
}

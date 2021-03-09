using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialog : MonoBehaviour
{
    public string[] dialog;

    public GameObject dialogButton;


    // Start is called before the first frame update
    void Start()
    {
        if (dialogButton != null)
        {
            dialogButton.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            dialogButton.SetActive(true);
            gameObject.GetComponent<NPCMovement>().DontWalk();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            dialogButton.SetActive(false);
            gameObject.GetComponent<NPCMovement>().Walk();
        }
    }

    public void ActivateDialog()
    {
        DialogManager.instance.ShowBox();
        DialogManager.instance.ShowDialog(dialog, false);
        dialogButton.SetActive(false);
    }
}

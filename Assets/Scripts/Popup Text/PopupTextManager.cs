using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupTextManager : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public GameObject expTextPrefab;
    public GameObject levelUpTextPrefab;
    public GameObject hpTextPrefab;

    public static PopupTextManager instance;

    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectsOfType<PopupTextManager>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ShowDamageText(float value, Transform position)
    {
        int damage = Mathf.RoundToInt(value);

        GameObject damageText = Instantiate(damageTextPrefab, transform);
        damageText.transform.position = position.position;
        damageText.GetComponent<FloatingText>().SetDamageText(damage);
    }

    public void ShowExpText(float value, Transform position)
    {
        int exp = Mathf.RoundToInt(value);

        GameObject expText = Instantiate(expTextPrefab, transform);
        expText.transform.position = position.position;
        expText.GetComponent<FloatingText>().SetExpText(exp);
    }

    public void ShowLevelUpText(Transform position)
    {
        GameObject lvlUpText = Instantiate(levelUpTextPrefab, transform);
        lvlUpText.transform.position = position.position;
    }

    public void ShowHpText(float value, Transform position)
    {
        int amount = Mathf.RoundToInt(value);

        GameObject hp = Instantiate(hpTextPrefab, transform);
        hp.transform.position = position.position;
        hp.GetComponent<FloatingText>().SetHpText(amount);
    }
}

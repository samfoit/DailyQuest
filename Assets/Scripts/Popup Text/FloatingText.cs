using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    public Text text;
    public Vector2 speed = new Vector2(0, 5);
    private RectTransform pos;
    private float timer;
    private float lifeTime = 0.3f;
    private Vector2 offset = new Vector3(0, 75f);

    private void Start()
    {
        timer = lifeTime;
        pos = GetComponent<RectTransform>();
        pos.anchoredPosition = pos.anchoredPosition + offset;
    }

    private void Update()
    {
        timer = Mathf.MoveTowards(timer, 0, 0.015f);
        pos.anchoredPosition += Vector2.up * speed;

        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetDamageText(int damage)
    {
        text.text = "-" + damage.ToString();
    }

    public void SetExpText(int exp)
    {
        text.text = "+" + exp.ToString() + "xp";
    }

    public void SetGoldText(int amount)
    {
        text.text = "+" + amount.ToString();
    }

    public void SetHpText(int hpAmount)
    {
        text.text = "+" + hpAmount.ToString() + "HP";
    }

}

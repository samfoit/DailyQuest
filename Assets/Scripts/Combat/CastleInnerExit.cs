using UnityEngine;
using UnityEngine.Tilemaps;

public class CastleInnerExit : MonoBehaviour
{
    public Tilemap outerEntrance;
    public Tilemap oECollider;
    public Tilemap dark;

    public bool appear = false;

    // Update is called once per frame
    void Update()
    {
        if (appear)
        {
            outerEntrance.color = new Color(outerEntrance.color.r, outerEntrance.color.g, outerEntrance.color.b, Mathf.MoveTowards(outerEntrance.color.a, 1, Time.deltaTime));
            oECollider.color = new Color(oECollider.color.r, oECollider.color.g, oECollider.color.b, Mathf.MoveTowards(oECollider.color.a, 1, Time.deltaTime));
            dark.color = new Color(dark.color.r, dark.color.g, dark.color.b, Mathf.MoveTowards(dark.color.a, 0, Time.unscaledDeltaTime));
            if (outerEntrance.color.a == 0 && oECollider.color.a == 1)
            {
                appear = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            appear = true;
            FindObjectOfType<CastleInnerEntrance>().CancelEntrance();
        }
    }

    public void CancelExit()
    {
        appear = false;
    }

    public void Restart()
    {
        appear = true;
        FindObjectOfType<CastleInnerEntrance>().CancelEntrance();
    }
}

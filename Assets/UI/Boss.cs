using UnityEngine;
using UnityEngine.Playables;

public class Boss : MonoBehaviour
{
    public string[] dialog;
    private Enemy stats;
    private bool enraged = false;
    private SpriteRenderer sprite;
    private EnemyShooter shooter;

    public GameObject rageProjectile;
    public GameObject rageRadialProjectile;

    private void Start()
    {
        stats = GetComponent<Enemy>();
        shooter = GetComponent<EnemyShooter>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (stats.currentHealth < (stats.maxHealth / 2) && !enraged)
        {
            Rage();
        }
    }

    public void Victory()
    {
        Time.timeScale = 0;
        DialogManager.instance.ShowBox();
        DialogManager.instance.ShowDialog(dialog, true);
    }

    private void Rage()
    {
        shooter.SetProjectile(rageProjectile);
        shooter.SetRadial(rageRadialProjectile);
        sprite.color = new Color(1f, 0f, 0f);
        GetComponent<NPCMovement>().enabled = true;

        enraged = true;
    }
}

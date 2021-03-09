public class Ability : ElementalDamage
{
    public bool radial = false;

    private void Start()
    {
        if (!radial)
        {
            PlayerStats.instance.currentMana -= PlayerStats.instance.maxMana / 3;
            StatsUI.instance.ChangeStatSliders();
        }
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<Enemy>().TakeDamage(PlayerStats.instance.level);
            if (lightning && radial)
            {
                if (PlayerStats.instance.currentHealth + (PlayerStats.instance.level * 5) < PlayerStats.instance.maxHealth)
                {
                    PlayerStats.instance.currentHealth += PlayerStats.instance.level * 5;
                    StatsUI.instance.ChangeStatSliders();
                    PopupTextManager.instance.ShowHpText(PlayerStats.instance.level * 5, transform);
                }
                else
                {
                    PopupTextManager.instance.ShowHpText(PlayerStats.instance.maxHealth - PlayerStats.instance.currentHealth, transform);
                    PlayerStats.instance.currentHealth = PlayerStats.instance.maxHealth;
                    StatsUI.instance.ChangeStatSliders();
                }
            }
        }
    }
}

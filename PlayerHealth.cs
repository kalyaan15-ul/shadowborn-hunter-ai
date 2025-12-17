using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;   

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth / (float)maxHealth);

        // TakeDamage(80); Added for testing purpose
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
            currentHealth = 0;

        healthBar.SetHealth(currentHealth / (float)maxHealth);

        if (currentHealth == 0)
        {
            Debug.Log("PLAYER DIED!");
            UIManager.Instance.ShowLoseScreen();
            Time.timeScale = 0f; // stop game
        }
    }

}

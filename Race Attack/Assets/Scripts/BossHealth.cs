using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public static BossHealth Instance { get; private set; }
    
    [Header("Health Settings")]
    public int maxHealth = 35;
    public int currentHealth;
    
    [Header("UI Reference")]
    public HealthBar healthBar;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        currentHealth = maxHealth;
        
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth); // Don't go below 0
        
        Debug.Log($"Boss took {damage} damage! Current HP: {currentHealth}/{maxHealth}");
        
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
        
        if (currentHealth <= 0)
        {
            OnBossDeath();
        }
    }
    
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(maxHealth, currentHealth); // Don't exceed max
        
        Debug.Log($"Boss healed {amount} HP! Current HP: {currentHealth}/{maxHealth}");
        
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
    }
    
    private void OnBossDeath()
    {
        Debug.Log("BOSS DEFEATED!");
        // TODO: Check win condition (need racer at finish line)

         gameObject.SetActive(false);
    }
    
    public bool IsDead()
    {
        return currentHealth <= 0;
    }
}
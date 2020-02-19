using UnityEngine;
using UnityEngine.Events;

public class HealthBehaviour : MonoBehaviour, ITakeDamage
{
    [SerializeField]
    float maxHealth;
    [SerializeField]
    UnityEvent onDeath;
    [SerializeField]
    float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }
    public void OnDamageTaken(float Damage)
    {
        currentHealth-=Damage;
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (onDeath.GetPersistentEventCount()>0)
            onDeath.Invoke();
        else
            gameObject.SetActive(false);
    }
}


interface ITakeDamage
{
    void OnDamageTaken(float Damage);
}

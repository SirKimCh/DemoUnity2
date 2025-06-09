using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float startingHealth = 3f;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool isDead = false;
    
    protected virtual void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }

    public virtual void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
        Debug.Log(currentHealth);
        if (currentHealth > 0)
        {
            if (anim != null)
            {
                anim.SetTrigger("IsHit");
            }
        }
        else
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        if (isDead) return;
        
        isDead = true;

        if (anim != null)
        {
            anim.SetTrigger("IsDead");
        }
        foreach (var component in GetComponents<MonoBehaviour>())
        {
            if (component != this)
            {
                component.enabled = false;
            }
        }
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach(var col in colliders)
        {
            col.enabled = false;
        }
        Destroy(gameObject, 1f);
    }
}
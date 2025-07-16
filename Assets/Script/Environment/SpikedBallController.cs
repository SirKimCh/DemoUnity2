using UnityEngine;

public class SpikedBallController : MonoBehaviour
{
    [SerializeField] private float damage = 1f; 
    [SerializeField] private float lifetime = 5f; 
    [SerializeField] private float knockbackForce = 5f; 

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHeath playerHealth = collision.gameObject.GetComponent<PlayerHeath>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamege(damage);
                PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                    knockbackDirection.y += 0.5f; 
                    playerMovement.ApplyKnockback(knockbackDirection.normalized, knockbackForce);
                }
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Terrain"))
        {
            Destroy(gameObject);
        }
    }
}
using UnityEngine;

public class BeeBullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 3f;
    private Transform player;
    private Vector2 targetDirection;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player != null)
        {
            targetDirection = (player.position - transform.position).normalized;
        }
        else
        {
            targetDirection = transform.right; 
        }

        GetComponent<Rigidbody2D>().velocity = targetDirection * speed;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHeath>()?.TakeDamege(1);
            Destroy(gameObject);
        }
        else if (!collision.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
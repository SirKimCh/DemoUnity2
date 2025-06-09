using UnityEngine;

public class BeeManager : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;

    private Transform player;
    private Animator anim;
    private float cooldownTimer = Mathf.Infinity;
    private EnemyHealth enemyHealth;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (enemyHealth.currentHealth <= 0) return;
        cooldownTimer += Time.deltaTime;
        if (player == null) return; 
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer < attackRange)
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                Attack();
            }
        }
    }

    private void Attack()
    {
        anim.SetTrigger("IsAtk");
        if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void ShootBullet()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
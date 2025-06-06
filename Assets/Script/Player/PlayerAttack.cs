using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private GameObject playerBulletPrefab;
    [SerializeField] private Transform firePoint; 
    [SerializeField] private SpriteRenderer playerSpriteRenderer; 
    
    public void PerformAttack()
    {
        GameObject bullet = Instantiate(playerBulletPrefab, firePoint.position, firePoint.rotation);
        PlayerBullet bulletScript = bullet.GetComponent<PlayerBullet>();

        if (bulletScript != null)
        {
            Vector2 fireDirection = playerSpriteRenderer.flipX ? Vector2.left : Vector2.right;
            bulletScript.SetDirection(fireDirection);
        }
    }
}
using UnityEngine;
using System.Collections;

public class GhostManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator ghostAnimator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D ghostCollider;

    [Header("Behavior Settings")]
    [SerializeField] private float pushForce = 3f;
    [SerializeField] private float invisibilityCooldown = 3f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float minSpawnDistanceFromPlayer = 5f;
    [SerializeField] public Transform checkPointGhost;

    private Transform playerTransform;
    private bool isPlayerInArea = false;
    private bool isVisible = false;
    private bool isActionInProgress = false;

    void Start()
    {
        MakeInvisible();
    }

    void Update()
    {
        if (isPlayerInArea && !isActionInProgress)
        {
            StartCoroutine(GhostActionCycle());
        }
        if (isVisible && ghostAnimator.GetBool("IsChasing"))
        {
            ChasePlayer();
        }
    }

    private IEnumerator GhostActionCycle()
    {
        isActionInProgress = true;
        Vector2 spawnPosition = CalculateSpawnPosition();
        transform.position = spawnPosition;
        MakeVisible();
        ghostAnimator.SetTrigger("Appear"); 
        yield return new WaitForSeconds(0.5f);
        if (isPlayerInArea) 
        {
            ghostAnimator.SetBool("IsChasing", true); 
        }
        else
        {
            PlayerExitedArea(); 
        }
    }

    private void ChasePlayer()
    {
        if (playerTransform == null)
        {
            PlayerExitedArea();
            return;
        }
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, 
            moveSpeed * Time.deltaTime);
        FlipSprite(direction);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isVisible && collision.gameObject.CompareTag("Player"))
        {
            ghostAnimator.SetBool("IsChasing", false); 

            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
                playerMovement.ApplyPush(pushDirection, pushForce);
            }
            
            StartCoroutine(DisappearAndCooldown());
        }
    }
    
    public void PlayerEnteredArea(Transform player)
    {
        playerTransform = player;
        isPlayerInArea = true;
    }

    public void PlayerExitedArea()
    {
        isPlayerInArea = false;
        playerTransform = null;
        if (isVisible)
        {
            StartCoroutine(DisappearAndCooldown(0)); 
        }
        else
        {
            StopAllCoroutines();
            isActionInProgress = false;
        }
    }

    private IEnumerator DisappearAndCooldown(float cooldown = -1)
    {
        if (cooldown < 0)
        {
            cooldown = invisibilityCooldown;
        }

        ghostAnimator.SetBool("IsChasing", false);
        ghostAnimator.SetTrigger("Disappear"); 
        yield return new WaitForSeconds(0.5f);
        MakeInvisible();
        yield return new WaitForSeconds(cooldown);

        isActionInProgress = false; 
    }
    private Vector2 CalculateSpawnPosition()
    {
        if (playerTransform == null) return transform.position;

        Vector2 spawnPosition;
        float checkPointRadius = checkPointGhost.GetComponent<CircleCollider2D>().radius;
        do
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized * Random.Range(0, checkPointRadius);
            spawnPosition = (Vector2)checkPointGhost.position + randomDirection;
        }
        while (Vector2.Distance(spawnPosition, playerTransform.position) < minSpawnDistanceFromPlayer);
        
        return spawnPosition;
    }
    
    private void FlipSprite(Vector2 direction)
    {
        if (direction.x > 0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (direction.x < -0.01f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void MakeVisible()
    {
        isVisible = true;
        spriteRenderer.enabled = true;
        ghostCollider.enabled = true;
    }

    private void MakeInvisible()
    {
        isVisible = false;
        spriteRenderer.enabled = false;
        ghostCollider.enabled = false;
    }
    
    public bool IsVisible()
    {
        return isVisible;
    }
}
using UnityEngine;

public class BatManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator batAnimator;
    [SerializeField] private Rigidbody2D batRigidbody;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform checkPointBat;

    [Header("Behavior Settings")]
    [SerializeField] private float attackRange = 8f;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float retreatDistance = 5f;

    private Vector3 startPosition;
    private Vector3 retreatTargetPosition;
    private bool isRetreating = false;

    private enum BatState { Idle, Chasing, Retreating, Returning }
    private BatState currentState;

    void Start()
    {
        if (checkPointBat != null)
        {
            startPosition = checkPointBat.position;
        }
        else
        {
            startPosition = transform.position;
        }

        if (playerTransform == null)
        {
            var playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                playerTransform = playerObject.transform;
            }
        }
        
        currentState = BatState.Idle;
        batAnimator.SetBool("IsMove", false); 
    }

    void Update()
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        
        HandleStates(distanceToPlayer);
        FlipSprite();
    }

    private void HandleStates(float distanceToPlayer)
    {
        switch (currentState)
        {
            case BatState.Idle:
                if (distanceToPlayer < attackRange)
                {
                    currentState = BatState.Chasing;
                    batAnimator.SetBool("IsMove", true);
                }
                break;

            case BatState.Chasing:
                if (distanceToPlayer >= attackRange)
                {
                    currentState = BatState.Returning;
                }
                else 
                {
                    transform.position = Vector2.MoveTowards(transform.position, 
                        playerTransform.position, moveSpeed * Time.deltaTime);
                }
                break;
            
            case BatState.Retreating:
                transform.position = Vector2.MoveTowards(transform.position, 
                    retreatTargetPosition, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, retreatTargetPosition) < 0.1f)
                {
                    isRetreating = false;
                    currentState = BatState.Chasing;
                }
                break;

            case BatState.Returning:
                transform.position = Vector2.MoveTowards(transform.position, 
                    startPosition, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, startPosition) < 0.1f)
                {
                    currentState = BatState.Idle;
                    batAnimator.SetBool("IsMove", false);
                }
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentState == BatState.Chasing && !isRetreating && collision.gameObject.CompareTag("Player"))
        {
            currentState = BatState.Retreating;
            isRetreating = true; 
            Vector2 retreatDirection = (transform.position - playerTransform.position).normalized;
            retreatTargetPosition = transform.position + (Vector3)retreatDirection * retreatDistance;
        }
    }

    private void FlipSprite()
    {
        Vector3 targetPos = playerTransform.position;

        if (currentState == BatState.Returning || currentState == BatState.Idle)
        {
            targetPos = startPosition;
        }
        
        if (transform.position.x > targetPos.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
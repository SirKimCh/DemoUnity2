using UnityEngine;
using System.Collections;

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
    private bool isPlayerInRange = false;
    private bool isRetreating = false;
    private bool isReturning = false;
    
    private enum BatState { Idle, TransitionToMove, Moving, TransitionToIdle, Retreating, Returning }
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
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        
        currentState = BatState.Returning;
        isReturning = true;
    }

    void Update()
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        isPlayerInRange = distanceToPlayer < attackRange;
        
        HandleStates();
        FlipSprite();
    }

    private void HandleStates()
    {
        switch (currentState)
        {
            case BatState.Idle:
                if (isPlayerInRange && !isRetreating)
                {
                    currentState = BatState.TransitionToMove;
                    batAnimator.SetInteger("State", 0); 
                }
                break;

            case BatState.Moving:
                transform.position = Vector2.MoveTowards(transform.position, 
                    playerTransform.position, moveSpeed * Time.deltaTime);
                if (!isPlayerInRange && !isReturning)
                {
                    currentState = BatState.TransitionToIdle;
                    batAnimator.SetInteger("State", 2); 
                }
                break;
            
            case BatState.Retreating:
                transform.position = Vector2.MoveTowards(transform.position, 
                    retreatTargetPosition, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, retreatTargetPosition) < 0.1f)
                {
                    isRetreating = false;
                    currentState = BatState.Moving;
                    batAnimator.SetInteger("State", 1);
                }
                break;

            case BatState.Returning:
                transform.position = Vector2.MoveTowards(transform.position, 
                    startPosition, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, startPosition) < 0.1f)
                {
                    isReturning = false;
                    currentState = BatState.TransitionToIdle;
                    batAnimator.SetInteger("State", 2);
                }
                break;
        }
    }
    
    public void OnOutAnimationComplete()
    {
        currentState = BatState.Moving;
        batAnimator.SetInteger("State", 1);
    }

    public void OnInAnimationComplete()
    {
        currentState = BatState.Idle;
        batAnimator.SetInteger("State", 3);

        if (!isPlayerInRange)
        {
            isReturning = true;
            currentState = BatState.Returning;
            batAnimator.SetInteger("State", 1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && currentState == BatState.Moving && !isRetreating)
        {
            isRetreating = true;
            currentState = BatState.Retreating;
            Vector2 retreatDirection = (transform.position - playerTransform.position).normalized;
            retreatTargetPosition = transform.position + (Vector3)retreatDirection * retreatDistance;
        }
    }

    private void FlipSprite()
    {
        Vector3 targetPos = startPosition;
        if (currentState == BatState.Moving || currentState == BatState.Retreating)
        {
            targetPos = playerTransform.position;
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
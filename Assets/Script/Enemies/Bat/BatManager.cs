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
    [SerializeField] private float retreatDistance = 10f;
    [SerializeField] private float retreatDuration = 1.5f; 

    private Vector3 startPosition;
    private bool isActionLocked = false;

    private enum BatState { Idle, Chasing, Returning }
    private BatState currentState;

    void Start()
    {
        if (checkPointBat != null) startPosition = checkPointBat.position;
        else startPosition = transform.position;

        var playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null) playerTransform = playerObject.transform;
        
        currentState = BatState.Idle;
    }

    void Update()
    {
        if (isActionLocked) return;

        if (playerTransform == null) 
        {
            if(currentState != BatState.Idle) currentState = BatState.Returning;
        }

        HandleStates();
        FlipSprite();
    }

    private void HandleStates()
    {
        float distanceToPlayer = playerTransform != null ? Vector2.Distance(transform.position, 
            playerTransform.position) : float.MaxValue;

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
                    batAnimator.SetInteger("State", 1);
                }
                else
                {
                    transform.position = Vector2.MoveTowards(transform.position, 
                        playerTransform.position, moveSpeed * Time.deltaTime);
                }
                break;

            case BatState.Returning:
                transform.position = Vector2.MoveTowards(transform.position, 
                    startPosition, moveSpeed * Time.deltaTime);
                
                if (Vector2.Distance(transform.position, startPosition) < 0.1f)
                {
                    currentState = BatState.Idle;
                    batAnimator.SetBool("IsMove", false);
                    batAnimator.SetInteger("State", 0); 
                }
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentState == BatState.Chasing && !isActionLocked && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(RetreatCoroutine());
        }
    }

    private IEnumerator RetreatCoroutine()
    {
        isActionLocked = true;
        Vector2 retreatDirection = (transform.position - playerTransform.position).normalized;
        Vector3 retreatTargetPosition = transform.position + (Vector3)retreatDirection * retreatDistance;

        float timer = 0;
        while (timer < retreatDuration)
        {
            transform.position = Vector2.Lerp(transform.position, retreatTargetPosition, 
                timer / retreatDuration);
            timer += Time.deltaTime;
            yield return null; 
        }

        isActionLocked = false;
    }

    private void FlipSprite()
    {
        if (playerTransform == null) return;

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
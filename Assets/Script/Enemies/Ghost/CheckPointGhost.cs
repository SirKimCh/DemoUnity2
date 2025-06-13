using UnityEngine;

public class CheckPointGhost : MonoBehaviour
{
    [SerializeField] private GhostManager ghostManager;
    [SerializeField] private float radius = 10f;

    private void Start()
    {
        transform.parent = null; 
        GetComponent<CircleCollider2D>().isTrigger = true;
        GetComponent<CircleCollider2D>().radius = radius;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ghostManager.PlayerEnteredArea(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ghostManager.PlayerExitedArea();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
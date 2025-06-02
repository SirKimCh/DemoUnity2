using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawController : MonoBehaviour
{
    [SerializeField] private Transform checkPoint1; 
    [SerializeField] private Transform checkPoint2; 
    [SerializeField] private Rigidbody2D sawRigidbody;
    [SerializeField] private float moveSpeed = 4f;
    private Transform targetPoint;
    
    void Start()
    {
        if (checkPoint1 != null && checkPoint2 != null)
        {
            transform.position = checkPoint1.position;
            targetPoint = checkPoint2;
        }
    }
    
    void Update()
    {
        if (checkPoint1 == null || checkPoint2 == null) return;
        Vector2 newPosition = Vector2.MoveTowards(sawRigidbody.position, targetPoint.position, 
            moveSpeed * Time.fixedDeltaTime);
        sawRigidbody.MovePosition(newPosition);
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            if (targetPoint == checkPoint1)
            {
                targetPoint = checkPoint2;
            }
            else
            {
                targetPoint = checkPoint1;
            }
        }
    }
}

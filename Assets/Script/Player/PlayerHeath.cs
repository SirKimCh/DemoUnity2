using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeath : MonoBehaviour
{
   [SerializeField] private float startHeath;
   public float currentHeath {get; private set;}
   
   [SerializeField] private PlayerMovement playerMovement; 
   [SerializeField] private float knockbackForce = 5f; 

   private void Awake()
   {
       currentHeath = startHeath;
   }
   
   private void TakeDamege(float _damage) 
   {
       currentHeath = Mathf.Clamp(currentHeath - _damage, 0, startHeath);
       if (currentHeath > 0)
       {
          
       }
       else
       {
          
       }
   }

   private void Update()
   {
       if (Input.GetKeyDown(KeyCode.Z)) 
       {
           TakeDamege(1); 
       }
   }
   
    private void OnTriggerEnter2D(Collider2D other)
    {
         if (other.CompareTag("Trap")) 
         {
              TakeDamege(1); 

              if (playerMovement != null)
              {
                  Vector2 knockbackDirection = (transform.position - other.transform.position).normalized;
                  
                  knockbackDirection.y = Mathf.Max(knockbackDirection.y, 0.3f); 
                  knockbackDirection = knockbackDirection.normalized;

                  playerMovement.ApplyKnockback(knockbackDirection, knockbackForce);
              }
         }
    }
}
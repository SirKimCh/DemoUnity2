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
   [SerializeField] private GameManager gameManager; 

   private void Awake()
   {
       currentHeath = startHeath;
   }
   
   private void TakeDamege(float _damage) 
   {
       currentHeath = Mathf.Clamp(currentHeath - _damage, 0, startHeath);
       if (currentHeath <= 0)
       {
           if (gameManager != null)
           {
               gameManager.GameOver();
           }
           this.enabled = false;
           if (playerMovement != null)
           {
               playerMovement.enabled = false;
           }
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
                  Vector2 knockbackDirection = (transform.position - other.transform.position);
                  if (knockbackDirection.sqrMagnitude < 0.001f) 
                  {
                      knockbackDirection = new Vector2(UnityEngine.Random.Range(-1f, 1f) < 0 ? -1f : 1f, 0.2f);
                      if (knockbackDirection.x == 0) knockbackDirection.x = 1f;
                  }
                  knockbackDirection = knockbackDirection.normalized; 
                  knockbackDirection.y += 0.5f;
                  knockbackDirection = knockbackDirection.normalized; 
                  playerMovement.ApplyKnockback(knockbackDirection, knockbackForce);
              }
         }
    }
}
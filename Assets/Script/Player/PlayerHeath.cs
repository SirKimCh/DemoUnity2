using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeath : MonoBehaviour
{
   [SerializeField] private float startHeath;
   public float currentHeath {get; private set;}
   
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
         }
    }
}

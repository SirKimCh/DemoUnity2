using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;

    void Update()
    {
        Movement();
        UpdateAnimator();
    }

    private void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        playerRigidbody.velocity = new Vector2(horizontal * moveSpeed, playerRigidbody.velocity.y);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpForce);
        }
    }

    private void UpdateAnimator()
    {
        if (playerRigidbody.velocity.x < 0)
        {
            playerSpriteRenderer.flipX = true;
        }
        else if (playerRigidbody.velocity.x > 0)
        {
            playerSpriteRenderer.flipX = false;
        }

        if (playerRigidbody.velocity.x == 0 & playerRigidbody.velocity.y == 0)
        {
            playerAnimator.SetInteger("State", 0);
        }

        if (playerRigidbody.velocity.x > 0)
        {
            playerAnimator.SetInteger("State", 1);
        }
    }
}
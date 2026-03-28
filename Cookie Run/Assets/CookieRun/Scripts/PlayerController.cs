using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public GameManager gameManager;
    public float jumpForce = 10f;
    public float slideTime = 0.5f;

    private int jumpCount = 0;
    private bool isGrounded = false;
    private bool isDead = false;
    private bool isSliding = false;
    private CapsuleCollider2D playerCollider;

    private Vector2 colliderSize;
    private Vector2 colliderOffset;

    private Rigidbody2D playerRigidbody;
    private Animator animator;

    private List<Collider2D> groundColliders = new List<Collider2D>();

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();

        gameManager = FindAnyObjectByType<GameManager>();

        colliderSize = playerCollider.size;
        colliderOffset = playerCollider.offset;

    }


    private void Update()
    {
        groundColliders.RemoveAll(c => c == null);
        isGrounded = groundColliders.Count > 0;

        if (isDead)
        {
            return;
        }

        isGrounded = groundColliders.Count > 0;

        if (!isSliding && Input.GetButtonDown("Slide") && isGrounded)
        {
            StartSlide();
        }
        if (isSliding && (Input.GetButtonUp("Slide") || !isGrounded))
        {
            EndSlide();
        }

        if (!isSliding)
        {
            if (Input.GetButtonDown("Jump") &&
                jumpCount < 2)
            {
                jumpCount++;
                playerRigidbody.linearVelocity = Vector2.zero;
                playerRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            }
            else if (Input.GetButtonUp("Jump") &&
                playerRigidbody.linearVelocity.y > 0)
            {
                playerRigidbody.linearVelocity *= 0.5f;
            }
        }

        animator.SetBool("Grounded", isGrounded);
        animator.SetBool("Slide", isSliding);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform") &&
            collision.contacts[0].normal.y > 0.7f)
        {
            if (!groundColliders.Contains(collision.collider))
            {
                groundColliders.Add(collision.collider);
            }
            jumpCount = 0;

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform"))
        {
            if (groundColliders.Contains(collision.collider))
            {
                groundColliders.Remove(collision.collider);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dead") && !isDead)
        {
            Die();
        }

        if (collision.CompareTag("Obstacle"))
        {
            gameManager.DecreaseHP(30f);
            collision.gameObject.SetActive(false);
        }

        if (collision.CompareTag("Coin"))
        {
            gameManager.AddHP(10f);
            collision.gameObject.SetActive(false);
        }
    }


    private void Die()
    {
        animator.SetTrigger("Die");
        playerRigidbody.linearVelocity = Vector2.zero;
        playerRigidbody.bodyType = RigidbodyType2D.Kinematic;

        isDead = true;
        isSliding = false;

        playerCollider.size = colliderSize;
        playerCollider.offset = colliderOffset;

        gameManager.OnPlayerDead();
    }

    public void DieFromHp()
    {
        if (isDead)
            return;

        Die();
    }

    private void StartSlide()
    {
        isSliding = true;

        playerCollider.size = new Vector2(colliderSize.x, colliderSize.y * 0.5f);
        playerCollider.offset = new Vector2(colliderOffset.x, colliderOffset.y - 0.3f);

    }

    private void EndSlide()
    {
        isSliding = false;

        playerCollider.size = colliderSize;
        playerCollider.offset = colliderOffset;
    }

}

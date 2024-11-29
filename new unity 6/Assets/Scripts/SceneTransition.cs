using UnityEngine;
using UnityEngine.UI; // Include this for UI elements
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Alive,
        Dead,
        Respawning
    }

    [Header("Player Settings")]
    public float moveSpeed = 5f;          // Movement speed of the player
    public float jumpForce = 10f;         // Force applied when jumping
    public float deathTransitionDuration = 1.0f; // Duration for the death transition

    [Header("References")]
    public GameObject deadSpritePrefab;    // Prefab for the dead sprite
    public AudioClip deathSound;            // Sound effect for death
    public Animator animator;                // Animator for the player
    public Text gameOverText;               // UI Text for Game Over message

    private Rigidbody2D rb;                 // Rigidbody2D for physics
    private PlayerState currentState = PlayerState.Alive; // Current state of the player
    private bool isGrounded;                // Check if the player is on the ground
    private float respawnDelay = 2f;        // Delay before respawning
    private Vector3 respawnPosition;        // Position to respawn the player

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        respawnPosition = transform.position; // Set initial respawn position
        gameOverText.gameObject.SetActive(false); // Hide Game Over text at start
    }

    void Update()
    {
        if (currentState == PlayerState.Alive)
        {
            MovePlayer();
            JumpPlayer();
            CheckForDeath();
        }
    }

    private void MovePlayer()
    {
        float move = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y); // Apply horizontal movement

        // Update animation based on movement
        animator.SetFloat("Speed", Mathf.Abs(move));
    }

    private void JumpPlayer()
    {
        // Check if the player is on the ground before allowing jumping
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse); // Apply jump force
            animator.SetTrigger("Jump"); // Trigger jump animation
        }
    }

    private void CheckForDeath()
    {
        // Example condition for death (could be based on collision with enemies, obstacles, etc.)
        if (transform.position.y < -5) // If the player falls off the screen
        {
            Die();
        }
    }

    private void Die()
    {
        if (currentState == PlayerState.Alive)
        {
            currentState = PlayerState.Dead; // Change state to dead
            animator.SetTrigger("Die"); // Trigger death animation
            AudioSource.PlayClipAtPoint(deathSound, transform.position); // Play death sound
            StartCoroutine(TransitionToDeadSprite());
        }
    }

    private IEnumerator TransitionToDeadSprite()
    {
        // Smoothly move the sprite downward to simulate falling
        float elapsedTime = 0f;
        Vector3 initialPosition = transform.position;
        rb.isKinematic = true; // Disable physics for smooth transition

        while (elapsedTime < deathTransitionDuration)
        {
            float t = elapsedTime / deathTransitionDuration;
            transform.position = Vector3.Lerp(initialPosition, initialPosition + new Vector3(0, -1, 0), t); // Fall down
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Instantiate dead sprite and destroy the live sprite
        GameObject deadSprite = Instantiate(deadSpritePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject); // Destroy the live sprite
        StartCoroutine(ShowGameOver()); // Show game over UI after death
    }

    private IEnumerator ShowGameOver()
    {
        yield return new WaitForSeconds(1f); // Wait for a moment before showing Game Over
        gameOverText.gameObject.SetActive(true); // Activate the Game Over UI text
        yield return new WaitForSeconds(respawnDelay); // Wait before respawning

        Respawn(); // Call respawn method
    }

    private void Respawn()
    {
        currentState = PlayerState.Respawning; // Change state to respawning
        Instantiate(deadSpritePrefab, respawnPosition, Quaternion.identity); // Instantiate the player at the respawn position
        gameOverText.gameObject.SetActive(false); // Hide Game Over text
        Destroy(gameObject); // Destroy the dead sprite
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player collides with the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Set isGrounded to true
        }

        // Additional collision checks can be added here
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the player exits the ground collision
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // Set isGrounded to false
        }
    }
}

using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Platform : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpForce = 12.5f;
    
    [Header("Audio Settings")]
    public AudioClip jumpSound;
    
    [Header("Visual Effects")]
    public GameObject jumpEffect;
    public float effectDuration = 1f;

    private AudioSource audioSource;

    protected virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if collision is from above (player landing on platform)
        if (collision.relativeVelocity.y <= 0f)
        {
            Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();
            
            if (rb != null && IsValidCollision(collision))
            {
                ApplyJump(rb);
                PlayEffects();
            }
        }
    }

    private bool IsValidCollision(Collision2D collision)
    {
        // Ensure the collision is coming from above
        Vector2 hitDirection = collision.contacts[0].normal;
        return hitDirection.y > 0.5f; // Normal pointing upward indicates collision from above
    }

    private void ApplyJump(Rigidbody2D rb)
    {
        Vector2 velocity = rb.linearVelocity;
        velocity.y = jumpForce;
        rb.linearVelocity = velocity;
    }

    private void PlayEffects()
    {
        // Play jump sound
        if (jumpSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }

        // Spawn visual effect
        if (jumpEffect != null)
        {
            GameObject effect = Instantiate(jumpEffect, transform.position, Quaternion.identity);
            Destroy(effect, effectDuration);
        }
    }

    public void SetJumpForce(float newJumpForce)
    {
        jumpForce = newJumpForce;
    }

    public float GetJumpForce()
    {
        return jumpForce;
    }
}

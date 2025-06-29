using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public float movementSpeed = 10f;
    public float gyroSensitivity = 3f;
    public float keyboardSensitivity = 1f;
    
    [Header("Physics Settings")]
    public float maxVelocityX = 15f;
    
    private float movement = 0f;
    private Rigidbody2D rb;
    private bool isGyroEnabled = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing!");
            return;
        }
        
        // Initialize gyroscope if supported
        InitializeGyroscope();
    }

    private void InitializeGyroscope()
    {
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
            isGyroEnabled = Input.gyro.enabled;
            
            if (isGyroEnabled)
            {
                Debug.Log("Gyroscope initialized successfully");
            }
            else
            {
                Debug.LogWarning("Gyroscope supported but failed to enable");
            }
        }
        else
        {
            Debug.Log("Gyroscope not supported on this device");
        }
    }

    void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        // Use gyroscope on mobile platforms if available
        if (isGyroEnabled && Application.isMobilePlatform)
        {
            float accX = Input.gyro.gravity.x;
            movement = Mathf.Clamp(accX * movementSpeed * gyroSensitivity, -movementSpeed, movementSpeed);
        }
        else
        {
            // Use keyboard input for desktop/editor
            float horizontalInput = Input.GetAxis("Horizontal");
            movement = horizontalInput * movementSpeed * keyboardSensitivity;
        }
    }

    void FixedUpdate()
    {
        if (rb == null) return;
        
        Vector2 velocity = rb.linearVelocity;
        velocity.x = movement;
        
        // Clamp horizontal velocity to prevent excessive speed
        velocity.x = Mathf.Clamp(velocity.x, -maxVelocityX, maxVelocityX);
        
        rb.linearVelocity = velocity;
    }

    public void SetMovementEnabled(bool enabled)
    {
        this.enabled = enabled;
        if (!enabled && rb != null)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }
}
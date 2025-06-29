using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public float movementSpeed = 10f;
    public float gyroSensitivity = 3f;
    float movement = 0f;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Aktifkan gyro jika perangkat mendukung
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
        }
    }

    void Update()
    {
        // Prioritaskan gyro jika tersedia dan perangkat mendukung
        if (Input.gyro.enabled && Application.isMobilePlatform)
        {
            float accX = Input.gyro.gravity.x;
            Debug.Log("Akselerasi X (Gyro): " + accX);
            movement = Mathf.Clamp(accX * movementSpeed * gyroSensitivity, -movementSpeed, movementSpeed);
        }
        else
        {
            // Gunakan keyboard untuk emulator/editor
            movement = Input.GetAxis("Horizontal") * movementSpeed;
        }
    }

    void FixedUpdate()
    {
        Vector2 velocity = rb.linearVelocity;
        velocity.x = movement;
        rb.linearVelocity = velocity;
    }
}
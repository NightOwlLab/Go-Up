using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public float movementSpeed = 10f;
    public float gyroSensitivity = 3f;
    float movement = 0f;
    Rigidbody2D rb;
    bool isMobile;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Deteksi apakah berjalan di perangkat mobile (bukan editor/emulator)
        isMobile = SystemInfo.deviceType == DeviceType.Handheld;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMobile)
        {
            // Ambil input dari gyro/accelerometer
            float accX = Input.acceleration.x;
            Debug.Log("Akselerasi X: " + accX);
            movement = Mathf.Clamp(accX * movementSpeed * gyroSensitivity, -movementSpeed, movementSpeed);
        }
        else
        {
            // Ambil input dari keyboard (editor atau emulator)
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

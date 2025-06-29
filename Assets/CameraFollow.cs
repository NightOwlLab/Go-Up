using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;
    
    [Header("Follow Settings")]
    public float smoothSpeed = 0.3f;
    public float verticalOffset = 2f;
    public bool useSmoothing = true;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("CameraFollow: Target is not assigned!");
            return;
        }

        // Only follow when player moves upward
        float targetY = target.position.y + verticalOffset;
        
        if (targetY > transform.position.y)
        {
            Vector3 targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);
            
            if (useSmoothing)
            {
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothSpeed);
            }
            else
            {
                transform.position = targetPosition;
            }
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ResetPosition()
    {
        if (target != null)
        {
            transform.position = new Vector3(transform.position.x, target.position.y + verticalOffset, transform.position.z);
        }
    }
}

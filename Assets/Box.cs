using UnityEngine;

public class Box : Platform
{
    // Box inherits all functionality from Platform
    // Can add specific Box behavior here if needed
    
    void Start()
    {
        // Call base Start if needed
        base.Start();
        
        // Box-specific initialization can go here
        // For example: special visual effects, different jump force, etc.
    }
}

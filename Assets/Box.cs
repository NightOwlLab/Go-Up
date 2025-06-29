using UnityEngine;

public class Box : Platform
{
    // Box inherits all functionality from Platform
    // Can add specific Box behavior here if needed
    
    protected override void Start()
    {
        // Call base Start
        base.Start();
        
        // Box-specific initialization can go here
        // For example: special visual effects, different jump force, etc.
    }
}

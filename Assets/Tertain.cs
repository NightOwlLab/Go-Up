using UnityEngine;

public class Tertain : Platform
{
    // Tertain inherits all functionality from Platform
    // Can add specific Tertain behavior here if needed
    
    protected override void Start()
    {
        // Call base Start
        base.Start();
        
        // Tertain-specific initialization can go here
        // For example: different visual effects, terrain-specific behavior, etc.
    }
}

using System;

namespace Lab02
{
    // Simple inheritance - Samurai extends Shinobi
    public class Samurai : Shinobi
    {
        // Constructor - just call base constructor
        public Samurai(string name, int x, int y) : base(name, x, y)
        {
            // That's it! 
            // Base class automatically:
            // - Sets characterType = "Samurai" (via this.GetType().Name)
            // - Loads sprites from "assets/Samurai/" folder
            // - Initializes all properties and animation states
            // 
            // All methods (Walk, Run, Attack, Jump) are inherited!
            // No code duplication needed!
        }
        
        // All other functionality is inherited from Shinobi:
        // - Walk(), Run(), Attack(), Jump(), SetIdle()
        // - UpdateAnimation(), GetCurrentSprite()
        // - Smart sprite loading
        // - Physics and animation system
    }
}
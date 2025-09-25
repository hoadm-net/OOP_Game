using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lab02
{
    public class Samurai : Shinobi
    {
        // Simple inheritance - just call base constructor
        // Sprite loading is automatically handled by base class!
        public Samurai(string name, int x, int y) : base(name, x, y)
        {
            // Base constructor will automatically:
            // 1. Set characterType = "Samurai" (via this.GetType().Name)
            // 2. Load sprites from "assets/Samurai/" folder
            // 3. Initialize all properties and states
            
            // That's it! All functionality inherited from Shinobi
        }
    }
}

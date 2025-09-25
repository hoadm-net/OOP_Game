using System;

namespace Lab03
{
    // Samurai: Slower but stronger than Shinobi
    public class Samurai : Shinobi
    {
        // Constructor - call base constructor
        public Samurai(string name, int x, int y) : base(name, x, y)
        {
            // Base constructor handles sprite loading automatically
        }

        // Override virtual methods to customize Samurai behavior
        protected override int GetWalkSpeed() => 1;    // Slower walk (Shinobi: 2)
        protected override int GetRunSpeed() => 3;     // Slower run (Shinobi: 4)  
        protected override int GetAttackPower() => 80; // Stronger attack (Shinobi: 50)
        protected override int GetJumpDistance() => 40; // Shorter jump (Shinobi: 60)

        // Override Attack method for custom behavior
        public override void Attack()
        {
            base.Attack(); // Call parent's attack logic first
            
            // Add Samurai-specific behavior
            Console.WriteLine($"??? {this.Name} performs a powerful samurai strike! (Power: {this.GetAttackPower()})");
        }

        // Override Jump method for custom behavior  
        public override void Jump()
        {
            base.Jump(); // Call parent's jump logic first
            
            // Add Samurai-specific behavior
            Console.WriteLine($"? {this.Name} makes a heavy samurai leap! (Distance: {this.GetJumpDistance()})");
        }

        // Override stats display
        public override string GetStats()
        {
            return $"??? SAMURAI - Walk: {GetWalkSpeed()}, Run: {GetRunSpeed()}, Attack: {GetAttackPower()}, Jump: {GetJumpDistance()}";
        }
    }
}
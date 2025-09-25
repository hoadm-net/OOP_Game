using System;

namespace Lab04
{
    // Shinobi class extends abstract Character
    // Does NOT implement IShieldable (no shield ability)
    public class Shinobi : Character
    {
        // Constructor
        public Shinobi(string name, int x, int y) : base(name, x, y)
        {
            // Base constructor handles sprite loading automatically
        }

        // MUST implement all abstract methods from Character
        public override int GetWalkSpeed() => 2;     // Balanced speed
        public override int GetRunSpeed() => 4;      // Balanced speed
        public override int GetAttackPower() => 50;  // Balanced power
        public override int GetJumpDistance() => 60; // Balanced jump
        public override string GetCharacterType() => "NINJA";

        // Override Attack for Shinobi-specific behavior
        public override void Attack()
        {
            base.Attack(); // Call abstract class implementation
        }

        // Override Jump for Shinobi-specific behavior
        public override void Jump()
        {
            base.Jump(); // Call abstract class implementation
        }
    }
}
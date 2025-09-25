using System;

namespace Lab04
{
    // Samurai class extends abstract Character AND implements IShieldable interface
    public class Samurai : Character, IShieldable
    {
        // IShieldable interface properties
        public bool IsShielding { get; private set; }
        public int ShieldDuration { get; private set; }
        private int maxShieldDuration = 100;

        // Constructor
        public Samurai(string name, int x, int y) : base(name, x, y)
        {
            // Initialize shield properties
            IsShielding = false;
            ShieldDuration = maxShieldDuration;
        }

        // MUST implement all abstract methods from Character
        public override int GetWalkSpeed() => 1;    // Slower but stronger
        public override int GetRunSpeed() => 3;     // Slower but stronger
        public override int GetAttackPower() => 80; // High attack power
        public override int GetJumpDistance() => 40; // Short jump
        public override string GetCharacterType() => "SAMURAI";

        // MUST implement all interface methods from IShieldable
        public void ActivateShield()
        {
            if (CanUseShield())
            {
                IsShielding = true;
                SetAction("Shield", false); // Shield is a loop animation
                Console.WriteLine($"??? {Name} activates samurai shield!");
            }
            else
            {
                Console.WriteLine($"? {Name} cannot use shield - insufficient energy!");
            }
        }

        public void DeactivateShield()
        {
            if (IsShielding)
            {
                IsShielding = false;
                SetIdle();
                Console.WriteLine($"?? {Name} deactivates shield!");
            }
        }

        public bool CanUseShield()
        {
            return ShieldDuration > 10 && !IsInOneShotAnimation();
        }

        // Override methods for Samurai-specific behavior
        public override void Attack()
        {
            if (IsShielding)
            {
                DeactivateShield(); // Must lower shield to attack
            }
            
            base.Attack(); // Call abstract class implementation
            Console.WriteLine($"??? {Name} performs a powerful samurai strike!");
        }

        public override void Jump()
        {
            if (IsShielding)
            {
                DeactivateShield(); // Must lower shield to jump
            }
            
            base.Jump(); // Call abstract class implementation
            Console.WriteLine($"? {Name} makes a heavy samurai leap!");
        }

        public override void Walk()
        {
            if (IsShielding)
            {
                // Shielding slows down movement
                if (CurrentAction != "Shield")
                {
                    SetAction("Shield", false);
                }
                X += GetWalkSpeed() / 2; // Half speed while shielding
                
                // Consume shield energy while moving
                ShieldDuration = Math.Max(0, ShieldDuration - 2);
                if (ShieldDuration <= 0)
                {
                    DeactivateShield();
                }
            }
            else
            {
                base.Walk(); // Normal walk
                
                // Restore shield energy while not shielding
                ShieldDuration = Math.Min(maxShieldDuration, ShieldDuration + 1);
            }
        }

        public override void Run()
        {
            if (IsShielding)
            {
                DeactivateShield(); // Cannot run while shielding
            }
            
            base.Run(); // Call abstract class implementation
            
            // Restore shield energy while running
            ShieldDuration = Math.Min(maxShieldDuration, ShieldDuration + 1);
        }

        public override void SetIdle()
        {
            if (!IsShielding) // Only set idle if not actively shielding
            {
                base.SetIdle();
            }
            
            // Restore shield energy while idle
            ShieldDuration = Math.Min(maxShieldDuration, ShieldDuration + 2);
        }

        // Override stats to include shield information
        public override string GetStats()
        {
            string shieldStatus = IsShielding ? "ACTIVE" : "INACTIVE";
            return $"{GetCharacterType()} - Walk: {GetWalkSpeed()}, Run: {GetRunSpeed()}, Attack: {GetAttackPower()}, Jump: {GetJumpDistance()}, Shield: {shieldStatus} ({ShieldDuration}/{maxShieldDuration})";
        }
    }
}
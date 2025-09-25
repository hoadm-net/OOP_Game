using System;

namespace Lab04
{
    // Fighter class extends abstract Character AND implements IShieldable interface
    // Also has mana system (multiple inheritance concepts)
    public class Fighter : Character, IShieldable
    {
        // Fighter-specific properties
        public int Mana { get; private set; } = 100;
        public int MaxMana { get; private set; } = 100;

        // IShieldable interface properties
        public bool IsShielding { get; private set; }
        public int ShieldDuration { get; private set; }
        private int maxShieldDuration = 80; // Fighter has less shield duration than Samurai

        // Constructor
        public Fighter(string name, int x, int y) : base(name, x, y)
        {
            // Initialize Fighter-specific properties
            Mana = 100;
            MaxMana = 100;
            IsShielding = false;
            ShieldDuration = maxShieldDuration;
        }

        // MUST implement all abstract methods from Character
        public override int GetWalkSpeed() => 3;    // Fastest
        public override int GetRunSpeed() => 5;     // Fastest
        public override int GetAttackPower() => 60; // Balanced
        public override int GetJumpDistance() => 80; // Longest jump
        public override string GetCharacterType() => "FIGHTER";

        // MUST implement all interface methods from IShieldable
        public void ActivateShield()
        {
            if (CanUseShield())
            {
                IsShielding = true;
                Mana -= 15; // Shield costs mana for Fighter
                SetAction("Shield", false); // Shield is a loop animation
                Console.WriteLine($"? {Name} activates magical fighter shield! (Mana: {Mana}/{MaxMana})");
            }
            else
            {
                Console.WriteLine($"? {Name} cannot use shield - insufficient mana or energy!");
            }
        }

        public void DeactivateShield()
        {
            if (IsShielding)
            {
                IsShielding = false;
                SetIdle();
                Console.WriteLine($"?? {Name} deactivates magical shield!");
            }
        }

        public bool CanUseShield()
        {
            return Mana >= 15 && ShieldDuration > 5 && !IsInOneShotAnimation();
        }

        // Fighter-specific method - restore mana
        public void RestoreMana(int amount = 25)
        {
            Mana = Math.Min(Mana + amount, MaxMana);
            Console.WriteLine($"? {Name} restores mana! Current: {Mana}/{MaxMana}");
        }

        // Override methods for Fighter-specific behavior with mana system
        public override void Attack()
        {
            if (IsShielding)
            {
                DeactivateShield(); // Must lower shield to attack
            }

            if (Mana >= 15) // Mana-powered attack
            {
                base.Attack(); // Call abstract class implementation
                Mana -= 15;
                Console.WriteLine($"?? {Name} uses mana-powered attack! Power: {GetAttackPower()}, Mana: {Mana}/{MaxMana}");
            }
            else // Low mana - weaker attack
            {
                base.Attack();
                Console.WriteLine($"?? {Name} attacks weakly - low mana! Mana: {Mana}/{MaxMana}");
            }
        }

        public override void Jump()
        {
            if (IsShielding)
            {
                DeactivateShield(); // Must lower shield to jump
            }

            if (Mana >= 10) // Mana-boosted jump
            {
                base.Jump(); // Call abstract class implementation
                Mana -= 10;
                Console.WriteLine($"?? {Name} uses mana-boosted jump! Distance: {GetJumpDistance()}, Mana: {Mana}/{MaxMana}");
            }
            else // Normal jump without mana
            {
                base.Jump();
                Console.WriteLine($"?? {Name} normal jump - low mana! Mana: {Mana}/{MaxMana}");
            }
        }

        public override void Walk()
        {
            if (IsShielding)
            {
                // Shielding costs mana over time for Fighter
                if (CurrentAction != "Shield")
                {
                    SetAction("Shield", false);
                }
                X += GetWalkSpeed() / 2; // Half speed while shielding
                
                // Consume mana and shield energy while moving with shield
                Mana = Math.Max(0, Mana - 1);
                ShieldDuration = Math.Max(0, ShieldDuration - 1);
                
                if (Mana <= 0 || ShieldDuration <= 0)
                {
                    DeactivateShield();
                }
            }
            else
            {
                base.Walk(); // Normal walk
                
                // Restore mana and shield energy while not shielding
                Mana = Math.Min(MaxMana, Mana + 1);
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
            
            // Restore mana and shield energy while running
            Mana = Math.Min(MaxMana, Mana + 1);
            ShieldDuration = Math.Min(maxShieldDuration, ShieldDuration + 1);
        }

        public override void SetIdle()
        {
            if (!IsShielding) // Only set idle if not actively shielding
            {
                base.SetIdle();
            }
            
            // Restore mana and shield energy while idle
            Mana = Math.Min(MaxMana, Mana + 2);
            ShieldDuration = Math.Min(maxShieldDuration, ShieldDuration + 2);
        }

        // Override stats to include mana and shield information
        public override string GetStats()
        {
            string shieldStatus = IsShielding ? "ACTIVE" : "INACTIVE";
            return $"{GetCharacterType()} - Walk: {GetWalkSpeed()}, Run: {GetRunSpeed()}, Attack: {GetAttackPower()}, Jump: {GetJumpDistance()}, Mana: {Mana}/{MaxMana}, Shield: {shieldStatus} ({ShieldDuration}/{maxShieldDuration})";
        }

        // Override GetInfo to include mana
        public new string GetInfo()
        {
            return $"{Name} - Health: {Health} - Mana: {Mana}/{MaxMana} - Action: {CurrentAction} - Position: ({X}, {Y})";
        }
    }
}
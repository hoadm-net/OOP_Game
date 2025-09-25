using System;

namespace Lab03
{
    // Fighter: Balanced stats with mana system
    public class Fighter : Shinobi
    {
        // Additional property specific to Fighter
        public int Mana { get; private set; } = 100;
        public int MaxMana { get; private set; } = 100;

        // Constructor - call base constructor
        public Fighter(string name, int x, int y) : base(name, x, y)
        {
            // Initialize Fighter-specific properties
            this.Mana = 100;
            this.MaxMana = 100;
        }

        // Override virtual methods - Fighter has balanced stats
        protected override int GetWalkSpeed() => 3;    // Fastest walk (Shinobi: 2, Samurai: 1)
        protected override int GetRunSpeed() => 5;     // Fastest run (Shinobi: 4, Samurai: 3)
        protected override int GetAttackPower() => 60; // Medium attack (between Shinobi: 50, Samurai: 80)
        protected override int GetJumpDistance() => 80; // Longest jump (Shinobi: 60, Samurai: 40)

        // Override Attack method with mana system
        public override void Attack()
        {
            if (this.Mana >= 15) // Mana-powered attack
            {
                base.Attack(); // Call parent's attack logic using 'base'
                
                this.Mana -= 15; // Use 'this' to refer to current object
                Console.WriteLine($"?? {this.Name} uses mana attack! Power: {this.GetAttackPower()}, Mana: {this.Mana}/{this.MaxMana}");
            }
            else // Low mana - weaker attack
            {
                base.Attack(); // Still call base logic
                Console.WriteLine($"?? {this.Name} attacks weakly - no mana! Mana: {this.Mana}/{this.MaxMana}");
            }
        }

        // Override Jump method with mana boost
        public override void Jump()
        {
            if (this.Mana >= 10) // Mana-boosted jump
            {
                base.Jump(); // Call parent's jump logic using 'base'
                
                this.Mana -= 10; // Consume mana using 'this'
                Console.WriteLine($"?? {this.Name} uses mana boost jump! Distance: {this.GetJumpDistance()}, Mana: {this.Mana}/{this.MaxMana}");
            }
            else // Normal jump without mana
            {
                base.Jump(); // Call base jump
                Console.WriteLine($"?? {this.Name} normal jump - no mana! Mana: {this.Mana}/{this.MaxMana}");
            }
        }

        // Fighter-specific method - restore mana
        public void RestoreMana(int amount = 20)
        {
            this.Mana = Math.Min(this.Mana + amount, this.MaxMana);
            Console.WriteLine($"? {this.Name} restores mana! Current: {this.Mana}/{this.MaxMana}");
        }

        // Override Walk to gradually restore mana (passive ability)
        public override void Walk()
        {
            base.Walk(); // Call parent's walk logic using 'base'
            
            // Fighter passive: walking restores mana slowly
            if (this.Mana < this.MaxMana)
            {
                this.Mana = Math.Min(this.Mana + 1, this.MaxMana); // Restore 1 mana per walk
            }
        }

        // Override stats display to include mana
        public override string GetStats()
        {
            return $"?? FIGHTER - Walk: {GetWalkSpeed()}, Run: {GetRunSpeed()}, Attack: {GetAttackPower()}, Jump: {GetJumpDistance()}, Mana: {this.Mana}/{this.MaxMana}";
        }

        // Override GetInfo to include mana information
        public new string GetInfo() // 'new' keyword to hide base method
        {
            return $"{this.Name} - Health: {this.Health} - Mana: {this.Mana}/{this.MaxMana} - Action: {this.CurrentAction} - Stats: {this.GetStats()} - Position: ({this.X}, {this.Y})";
        }
    }
}
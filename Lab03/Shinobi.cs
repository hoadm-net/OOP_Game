using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;

namespace Lab03
{
    public class Shinobi
    {
        // Properties - Encapsulation
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Health { get; set; }
        public string CurrentAction { get; private set; }
        
        // Animation properties
        public int CurrentFrame { get; private set; }
        public int FrameCount { get; private set; }
        private int frameTimer;
        private const int FRAME_DELAY = 4; // Animation speed

        // One-shot animation properties (Attack, Jump)
        private bool isOneShotAnimation;
        private bool hasCompletedOneShot;

        // Jump physics
        private int jumpStartX, jumpStartY;
        private bool isJumping;

        // Smart sprite loading
        private Dictionary<string, Image> sprites;
        private string characterType;

        // Constructor
        public Shinobi(string name, int x, int y)
        {
            Name = name;
            X = x;
            Y = y;
            Health = 100;
            
            // Smart sprite loading based on class name
            characterType = this.GetType().Name; // "Shinobi", "Samurai", or "Fighter"
            LoadSprites();
            SetIdle();
        }

        // Smart sprite loading - automatically loads sprites based on character type
        private void LoadSprites()
        {
            sprites = new Dictionary<string, Image>();
            try
            {
                string spritePath = Path.Combine(Application.StartupPath, "..", "..", "..", "assets", characterType);
                
                sprites["Idle"] = Image.FromFile(Path.Combine(spritePath, "Idle.png"));
                sprites["Walk"] = Image.FromFile(Path.Combine(spritePath, "Walk.png"));
                sprites["Run"] = Image.FromFile(Path.Combine(spritePath, "Run.png"));
                sprites["Attack"] = Image.FromFile(Path.Combine(spritePath, "Attack.png"));
                sprites["Jump"] = Image.FromFile(Path.Combine(spritePath, "Jump.png"));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cannot load {characterType} sprites: " + ex.Message);
            }
        }

        // Public method to get current sprite (for Form to use)
        public Image GetCurrentSprite()
        {
            return sprites.ContainsKey(CurrentAction) ? sprites[CurrentAction] : null;
        }

        // Cleanup method
        public void Dispose()
        {
            if (sprites != null)
            {
                foreach (var sprite in sprites.Values)
                {
                    sprite?.Dispose();
                }
                sprites.Clear();
            }
        }

        // Virtual properties - can be overridden by child classes
        protected virtual int GetWalkSpeed() => 2;     // Shinobi: moderate speed
        protected virtual int GetRunSpeed() => 4;      // Shinobi: moderate speed
        protected virtual int GetAttackPower() => 50;  // Shinobi: moderate power
        protected virtual int GetJumpDistance() => 60; // Shinobi: moderate jump

        // Get frame count for each action
        private int GetFrameCount(string action)
        {
            switch (action)
            {
                case "Idle": return 6;
                case "Walk": return 8;
                case "Run": return 8;
                case "Attack": return 5;
                case "Jump": return 12;
                default: return 1;
            }
        }

        // Update animation frame
        public void UpdateAnimation()
        {
            frameTimer++;
            if (frameTimer >= FRAME_DELAY)
            {
                frameTimer = 0;
                
                if (isOneShotAnimation)
                {
                    UpdateOneShotAnimation();
                }
                else
                {
                    // Loop animation (Idle, Walk, Run)
                    CurrentFrame = (CurrentFrame + 1) % FrameCount;
                }
            }
        }

        private void UpdateOneShotAnimation()
        {
            if (CurrentFrame < FrameCount - 1)
            {
                CurrentFrame++;
                
                // Update jump physics
                if (CurrentAction == "Jump" && isJumping)
                {
                    UpdateJumpPhysics();
                }
            }
            else
            {
                // Animation completed, return to Idle
                hasCompletedOneShot = true;
                SetIdle();
            }
        }

        private void UpdateJumpPhysics()
        {
            // Calculate position based on parabolic jump arc
            // Use virtual method for jump distance!
            float progress = (float)CurrentFrame / (FrameCount - 1); // 0.0 to 1.0
            
            // Horizontal movement - uses virtual GetJumpDistance()
            X = jumpStartX + (int)(progress * GetJumpDistance());
            
            // Vertical movement (parabolic arc)
            float height = 4 * progress * (1 - progress); // Parabola: 4*t*(1-t)
            Y = jumpStartY - (int)(height * 40); // Max height = 40 pixels
        }

        // Virtual methods - can be overridden by child classes for different behaviors
        public virtual void Walk()
        {
            if (CurrentAction != "Walk")
            {
                SetAction("Walk", false);
            }
            // Use virtual method for speed - polymorphism in action!
            X += GetWalkSpeed();
        }

        public virtual void Run()
        {
            if (CurrentAction != "Run")
            {
                SetAction("Run", false);
            }
            // Use virtual method for speed - polymorphism in action!
            X += GetRunSpeed();
        }

        public virtual void Attack()
        {
            if (!isOneShotAnimation)
            {
                SetAction("Attack", true);
                // Virtual attack power will be used by child classes
                Console.WriteLine($"{Name} attacks with power {GetAttackPower()}!");
            }
        }

        public virtual void Jump()
        {
            if (!isOneShotAnimation)
            {
                SetAction("Jump", true);
                InitializeJumpPhysics();
                Console.WriteLine($"{Name} jumps {GetJumpDistance()} pixels!");
            }
        }

        public virtual void SetIdle()
        {
            SetAction("Idle", false);
            isJumping = false;
        }

        // Helper method to set action state
        private void SetAction(string action, bool oneShot)
        {
            CurrentAction = action;
            FrameCount = GetFrameCount(action);
            CurrentFrame = 0;
            frameTimer = 0;
            isOneShotAnimation = oneShot;
            hasCompletedOneShot = false;
        }

        private void InitializeJumpPhysics()
        {
            jumpStartX = X;
            jumpStartY = Y;
            isJumping = true;
        }

        // Check if currently in one-shot animation
        public bool IsInOneShotAnimation()
        {
            return isOneShotAnimation && !hasCompletedOneShot;
        }

        // Get current frame rectangle for sprite rendering
        public Rectangle GetCurrentFrameRect()
        {
            const int frameWidth = 128;  // Each frame is 128x128
            const int frameHeight = 128;
            
            return new Rectangle(
                CurrentFrame * frameWidth, 
                0, 
                frameWidth, 
                frameHeight
            );
        }

        // Virtual method for getting character stats - can be overridden
        public virtual string GetStats()
        {
            return $"Walk: {GetWalkSpeed()}, Run: {GetRunSpeed()}, Attack: {GetAttackPower()}, Jump: {GetJumpDistance()}";
        }

        // Get character information
        public string GetInfo()
        {
            return $"{Name} - Health: {Health} - Action: {CurrentAction} - Stats: {GetStats()} - Position: ({X}, {Y})";
        }
    }
}
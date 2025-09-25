using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;

namespace Lab04
{
    // Abstract base class - cannot be instantiated directly
    public abstract class Character
    {
        // Common properties for all characters
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

        // One-shot animation properties
        private bool isOneShotAnimation;
        private bool hasCompletedOneShot;

        // Jump physics
        private int jumpStartX, jumpStartY;
        private bool isJumping;

        // Smart sprite loading
        private Dictionary<string, Image> sprites;
        private string characterType;

        // Constructor
        protected Character(string name, int x, int y)
        {
            Name = name;
            X = x;
            Y = y;
            Health = 100;
            
            // Smart sprite loading based on actual class name
            characterType = this.GetType().Name;
            LoadSprites();
            SetIdle();
        }

        // Smart sprite loading
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
                
                // Try to load Shield sprite for characters that support it
                try
                {
                    sprites["Shield"] = Image.FromFile(Path.Combine(spritePath, "Shield.png"));
                }
                catch
                {
                    // Shield sprite not available for this character
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cannot load {characterType} sprites: " + ex.Message);
            }
        }

        // Public method to get current sprite
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

        // Abstract methods - MUST be implemented by derived classes
        public abstract int GetWalkSpeed();
        public abstract int GetRunSpeed();
        public abstract int GetAttackPower();
        public abstract int GetJumpDistance();
        public abstract string GetCharacterType();

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
                case "Shield": return 4;
                default: return 1;
            }
        }

        // Common animation update logic
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
                    // Loop animation (Idle, Walk, Run, Shield)
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
            float progress = (float)CurrentFrame / (FrameCount - 1);
            
            // Horizontal movement - uses abstract method
            X = jumpStartX + (int)(progress * GetJumpDistance());
            
            // Vertical movement (parabolic arc)
            float height = 4 * progress * (1 - progress);
            Y = jumpStartY - (int)(height * 40);
        }

        // Virtual methods - can be overridden but have default implementation
        public virtual void Walk()
        {
            if (CurrentAction != "Walk")
            {
                SetAction("Walk", false);
            }
            X += GetWalkSpeed(); // Uses abstract method
        }

        public virtual void Run()
        {
            if (CurrentAction != "Run")
            {
                SetAction("Run", false);
            }
            X += GetRunSpeed(); // Uses abstract method
        }

        public virtual void Attack()
        {
            if (!isOneShotAnimation)
            {
                SetAction("Attack", true);
                Console.WriteLine($"{Name} ({GetCharacterType()}) attacks with power {GetAttackPower()}!");
            }
        }

        public virtual void Jump()
        {
            if (!isOneShotAnimation)
            {
                SetAction("Jump", true);
                InitializeJumpPhysics();
                Console.WriteLine($"{Name} ({GetCharacterType()}) jumps {GetJumpDistance()} pixels!");
            }
        }

        public virtual void SetIdle()
        {
            SetAction("Idle", false);
            isJumping = false;
        }

        // Helper method to set action state
        protected void SetAction(string action, bool oneShot)
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
            const int frameWidth = 128;
            const int frameHeight = 128;
            
            return new Rectangle(
                CurrentFrame * frameWidth, 
                0, 
                frameWidth, 
                frameHeight
            );
        }

        // Virtual method for getting character stats
        public virtual string GetStats()
        {
            return $"{GetCharacterType()} - Walk: {GetWalkSpeed()}, Run: {GetRunSpeed()}, Attack: {GetAttackPower()}, Jump: {GetJumpDistance()}";
        }

        // Get character information
        public string GetInfo()
        {
            return $"{Name} - Health: {Health} - Action: {CurrentAction} - Position: ({X}, {Y})";
        }
    }
}
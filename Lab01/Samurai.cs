using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lab01
{
    public class Samurai
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

        // Constructor
        public Samurai(string name, int x, int y)
        {
            Name = name;
            X = x;
            Y = y;
            Health = 100;
            SetIdle();
        }

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
            // Total distance: 60 pixels (30 walk steps)
            float progress = (float)CurrentFrame / (FrameCount - 1); // 0.0 to 1.0
            
            // Horizontal movement
            X = jumpStartX + (int)(progress * 60);
            
            // Vertical movement (parabolic arc)
            float height = 4 * progress * (1 - progress); // Parabola: 4*t*(1-t)
            Y = jumpStartY - (int)(height * 40); // Max height = 40 pixels
        }

        // Character actions - continuous (loop until key release)
        public void Walk()
        {
            if (CurrentAction != "Walk")
            {
                SetAction("Walk", false);
            }
            X += 2; // Slow movement
        }

        public void Run()
        {
            if (CurrentAction != "Run")
            {
                SetAction("Run", false);
            }
            X += 4; // Fast movement
        }

        // Character actions - one-shot (complete animation then return to Idle)
        public void Attack()
        {
            if (!isOneShotAnimation)
            {
                SetAction("Attack", true);
            }
        }

        public void Jump()
        {
            if (!isOneShotAnimation)
            {
                SetAction("Jump", true);
                InitializeJumpPhysics();
            }
        }

        public void SetIdle()
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

        // Get character information
        public string GetInfo()
        {
            return $"{Name} - Health: {Health} - Action: {CurrentAction} - Frame: {CurrentFrame}/{FrameCount} - Position: ({X}, {Y})";
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab02
{
    public partial class Form1 : Form
    {
        // Much simpler! Just one reference to current character
        private Shinobi currentCharacter;
        private Shinobi ninja;
        private Samurai samurai;
        private Timer animationTimer;
        private int lastX, lastY; // Previous position for invalidation

        public Form1()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            // Enable double buffering to reduce flicker
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | 
                         ControlStyles.UserPaint | 
                         ControlStyles.DoubleBuffer | 
                         ControlStyles.ResizeRedraw, true);
            this.UpdateStyles();

            // Create character objects - inheritance in action!
            ninja = new Shinobi("Shadow Ninja", 100, 200);
            samurai = new Samurai("Blade Samurai", 100, 200); // Inherits everything!
            
            // Start with Samurai to show inheritance working
            currentCharacter = samurai;
            UpdateLastPosition();

            // Setup animation timer - 60 FPS
            SetupAnimationTimer();

            // Set initial title
            UpdateTitle();
        }

        private void SetupAnimationTimer()
        {
            animationTimer = new Timer();
            animationTimer.Interval = 16; // ~60 FPS (1000/60 ≈ 16ms)
            animationTimer.Tick += AnimationTimer_Tick;
            animationTimer.Start();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            // Simple! Just update current character - polymorphism at work
            currentCharacter.UpdateAnimation();
            
            // Invalidate old and new regions for efficient redraw
            InvalidateCharacterRegions();
            
            UpdateLastPosition();
        }

        private void UpdateLastPosition()
        {
            lastX = currentCharacter.X;
            lastY = currentCharacter.Y;
        }

        private void InvalidateCharacterRegions()
        {
            const int margin = 10;
            const int size = 148;
            
            Rectangle oldRect = new Rectangle(lastX - margin, lastY - margin, size, size);
            Rectangle newRect = new Rectangle(currentCharacter.X - margin, currentCharacter.Y - margin, size, size);
            
            this.Invalidate(oldRect);
            this.Invalidate(newRect);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Clear background
            e.Graphics.Clear(Color.LightBlue);

            // Draw current character - much simpler!
            DrawCharacter(e.Graphics);

            // Draw controls help
            DrawControlsHelp(e.Graphics);
        }

        private void DrawCharacter(Graphics graphics)
        {
            // Get sprite from current character - inheritance handles the rest!
            var sprite = currentCharacter.GetCurrentSprite();
            if (sprite != null)
            {
                var sourceRect = currentCharacter.GetCurrentFrameRect();
                var destRect = new Rectangle(currentCharacter.X, currentCharacter.Y, 128, 128);
                
                // Use nearest neighbor for crisp pixel art
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                graphics.DrawImage(sprite, destRect, sourceRect, GraphicsUnit.Pixel);
            }
        }

        private void DrawControlsHelp(Graphics graphics)
        {
            using (Font smallFont = new Font("Arial", 9))
            {
                string controls = "Hold: W=Walk | R=Run | Tap: A=Attack | J=Jump | Press: I=Idle | Space=Reset | C=Change Character";
                graphics.DrawString(controls, smallFont, Brushes.DarkBlue, 10, this.Height - 40);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            // Handle character switching
            if (e.KeyCode == Keys.C)
            {
                SwitchCharacter();
                return;
            }

            // Handle actions - unified for both characters! Polymorphism magic!
            HandleInput(e.KeyCode);
            UpdateTitle();
        }

        private void SwitchCharacter()
        {
            // Switch between characters - polymorphism allows this!
            if (currentCharacter == ninja)
                currentCharacter = samurai;
            else
                currentCharacter = ninja;
            
            // Sync positions when switching
            currentCharacter.X = lastX;
            currentCharacter.Y = lastY;
            currentCharacter.SetIdle();
            
            this.Invalidate(); // Full repaint when switching
        }

        private void HandleInput(Keys keyCode)
        {
            // Same code works for both Shinobi and Samurai - inheritance power!
            switch (keyCode)
            {
                case Keys.I: currentCharacter.SetIdle(); break;
                case Keys.W: currentCharacter.Walk(); break;
                case Keys.R: currentCharacter.Run(); break;
                case Keys.A: currentCharacter.Attack(); break;
                case Keys.J: currentCharacter.Jump(); break;
                case Keys.Space: ResetCharacter(); break;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            // Only handle KeyUp for continuous actions (Walk, Run)
            switch (e.KeyCode)
            {
                case Keys.W: // Walk
                case Keys.R: // Run
                    if (!currentCharacter.IsInOneShotAnimation())
                    {
                        currentCharacter.SetIdle();
                    }
                    break;
            }

            UpdateTitle();
        }

        private void ResetCharacter()
        {
            currentCharacter.X = 100;
            currentCharacter.Y = 200;
            currentCharacter.SetIdle();
            
            // Force full repaint to clear ghost sprites
            this.Invalidate();
        }

        private void UpdateTitle()
        {
            string characterType = currentCharacter.GetType().Name;
            this.Text = $"Lab02 - Inheritance Demo: {characterType} - {currentCharacter.Name} | Health: {currentCharacter.Health} | Action: {currentCharacter.CurrentAction}";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Cleanup resources - each character handles its own sprites
            CleanupResources();
        }

        private void CleanupResources()
        {
            animationTimer?.Stop();
            animationTimer?.Dispose();
            
            // Each character cleans up its own sprites
            ninja?.Dispose();
            samurai?.Dispose();
        }
    }
}

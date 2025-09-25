using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab03
{
    public partial class Form1 : Form
    {
        // Polymorphism in action - same reference type, different behaviors!
        private Shinobi currentCharacter;
        private Shinobi ninja;
        private Samurai samurai;
        private Fighter fighter;
        private Timer animationTimer;
        private int lastX, lastY; // Previous position for invalidation
        private int characterIndex = 0; // 0: ninja, 1: samurai, 2: fighter

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

            // Create character objects - polymorphism demo!
            ninja = new Shinobi("Shadow Ninja", 100, 200);        // Base class
            samurai = new Samurai("Blade Samurai", 100, 200);     // Slower, stronger
            fighter = new Fighter("Swift Fighter", 100, 200);     // Balanced with mana
            
            // Start with ninja
            currentCharacter = ninja;
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
            // Polymorphism: same method call, different behaviors!
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
            e.Graphics.Clear(Color.LightGreen);

            // Draw current character
            DrawCharacter(e.Graphics);

            // Draw controls help
            DrawControlsHelp(e.Graphics);

            // Draw character stats
            DrawCharacterStats(e.Graphics);
        }

        private void DrawCharacter(Graphics graphics)
        {
            // Polymorphism: same method call works for all character types!
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
                string controls = "Hold: W=Walk | R=Run | Tap: A=Attack | J=Jump | Press: I=Idle | Space=Reset | C=Change Character | M=Restore Mana";
                graphics.DrawString(controls, smallFont, Brushes.DarkBlue, 10, this.Height - 40);
            }
        }

        private void DrawCharacterStats(Graphics graphics)
        {
            using (Font font = new Font("Arial", 10, FontStyle.Bold))
            {
                string characterType = currentCharacter.GetType().Name.ToUpper();
                string stats = currentCharacter.GetStats();
                
                // Draw character type
                graphics.DrawString($"Current: {characterType}", font, Brushes.Red, 10, 10);
                
                // Draw stats - polymorphism shows different values!
                graphics.DrawString(stats, new Font("Arial", 9), Brushes.DarkGreen, 10, 30);
                
                // Draw demo message
                graphics.DrawString("🎯 POLYMORPHISM DEMO: Same method calls, different behaviors!", 
                    new Font("Arial", 9, FontStyle.Italic), Brushes.Purple, 10, 50);
                
                // Draw comparison
                string comparison = GetCharacterComparison();
                graphics.DrawString(comparison, new Font("Arial", 8), Brushes.DarkBlue, 10, 70);
            }
        }

        private string GetCharacterComparison()
        {
            return "NINJA: Balanced | SAMURAI: Slow+Strong | FIGHTER: Fast+Mana";
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            // Handle special keys
            switch (e.KeyCode)
            {
                case Keys.C: // Change character
                    SwitchCharacter();
                    return;
                case Keys.M: // Restore mana (Fighter only)
                    RestoreMana();
                    return;
            }

            // Handle actions - polymorphism magic! Same calls, different behaviors!
            HandleInput(e.KeyCode);
            UpdateTitle();
        }

        private void SwitchCharacter()
        {
            // Cycle through characters
            characterIndex = (characterIndex + 1) % 3;
            
            switch (characterIndex)
            {
                case 0: currentCharacter = ninja; break;
                case 1: currentCharacter = samurai; break;
                case 2: currentCharacter = fighter; break;
            }
            
            // Sync positions when switching
            currentCharacter.X = lastX;
            currentCharacter.Y = lastY;
            currentCharacter.SetIdle(); // Polymorphism: same call, might behave differently
            
            this.Invalidate(); // Full repaint when switching
        }

        private void RestoreMana()
        {
            // Only Fighter has mana - demonstrate type checking
            if (currentCharacter is Fighter fighterChar)
            {
                fighterChar.RestoreMana();
            }
            else
            {
                Console.WriteLine($"{currentCharacter.Name} doesn't use mana!");
            }
        }

        private void HandleInput(Keys keyCode)
        {
            // Polymorphism in action! Same method calls, different behaviors based on character type!
            switch (keyCode)
            {
                case Keys.I: 
                    currentCharacter.SetIdle(); // Same call, same behavior
                    break;
                case Keys.W: 
                    currentCharacter.Walk();    // Same call, DIFFERENT speeds!
                    break;
                case Keys.R: 
                    currentCharacter.Run();     // Same call, DIFFERENT speeds!
                    break;
                case Keys.A: 
                    currentCharacter.Attack();  // Same call, DIFFERENT power & effects!
                    break;
                case Keys.J: 
                    currentCharacter.Jump();    // Same call, DIFFERENT distances!
                    break;
                case Keys.Space: 
                    ResetCharacter(); 
                    break;
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
                        currentCharacter.SetIdle(); // Polymorphism: same call
                    }
                    break;
            }

            UpdateTitle();
        }

        private void ResetCharacter()
        {
            currentCharacter.X = 100;
            currentCharacter.Y = 200;
            currentCharacter.SetIdle(); // Polymorphism: same call
            
            // Special handling for Fighter mana reset
            if (currentCharacter is Fighter fighterChar)
            {
                fighterChar.RestoreMana(100); // Full restore
            }
            
            // Force full repaint to clear ghost sprites
            this.Invalidate();
        }

        private void UpdateTitle()
        {
            string characterType = currentCharacter.GetType().Name;
            string info = currentCharacter is Fighter fighter ? fighter.GetInfo() : currentCharacter.GetInfo();
            this.Text = $"Lab03 - Virtual Methods & Polymorphism: {characterType} - {info}";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Cleanup resources - polymorphism in cleanup too!
            CleanupResources();
        }

        private void CleanupResources()
        {
            animationTimer?.Stop();
            animationTimer?.Dispose();
            
            // Each character cleans up its own sprites - polymorphism
            ninja?.Dispose();
            samurai?.Dispose();
            fighter?.Dispose();
        }
    }
}

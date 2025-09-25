using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab04
{
    public partial class Form1 : Form
    {
        // Abstract class polymorphism - Character reference can hold any derived type
        private Character currentCharacter;
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

            // Create character objects - abstract class polymorphism!
            ninja = new Shinobi("Shadow Ninja", 100, 200);        // No shield ability
            samurai = new Samurai("Blade Samurai", 100, 200);     // Has shield ability (IShieldable)
            fighter = new Fighter("Swift Fighter", 100, 200);     // Has shield + mana (IShieldable + custom properties)
            
            // Start with ninja (no shield)
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
            // Abstract class polymorphism: same method call, implemented differently by each class
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
            e.Graphics.Clear(Color.LightCyan);

            // Draw current character
            DrawCharacter(e.Graphics);

            // Draw controls help
            DrawControlsHelp(e.Graphics);
        }

        private void DrawCharacter(Graphics graphics)
        {
            // Abstract class polymorphism: same method call works for all derived types
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
                string controls = "Hold: W=Walk | R=Run | Tap: A=Attack | J=Jump | S=Shield | Press: I=Idle | Space=Reset | C=Change Character | M=Restore Mana";
                graphics.DrawString(controls, smallFont, Brushes.DarkBlue, 10, this.Height - 40);
            }
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
                case Keys.S: // Shield (IShieldable interface)
                    ToggleShield();
                    return;
            }

            // Handle actions - abstract class polymorphism in action!
            HandleInput(e.KeyCode);
            UpdateTitle();
        }

        private void SwitchCharacter()
        {
            // Cycle through characters
            characterIndex = (characterIndex + 1) % 3;
            
            switch (characterIndex)
            {
                case 0: currentCharacter = ninja; break;      // Shinobi: no interfaces
                case 1: currentCharacter = samurai; break;    // Samurai: implements IShieldable
                case 2: currentCharacter = fighter; break;    // Fighter: implements IShieldable + has mana
            }
            
            // Sync positions when switching
            currentCharacter.X = lastX;
            currentCharacter.Y = lastY;
            currentCharacter.SetIdle(); // Abstract method call
            
            this.Invalidate(); // Full repaint when switching
        }

        private void ToggleShield()
        {
            // Interface polymorphism - check if current character implements IShieldable
            if (currentCharacter is IShieldable shieldableChar)
            {
                if (shieldableChar.IsShielding)
                {
                    shieldableChar.DeactivateShield();
                }
                else
                {
                    shieldableChar.ActivateShield();
                }
            }
            else
            {
                Console.WriteLine($"{currentCharacter.Name} ({currentCharacter.GetCharacterType()}) does not have shield ability!");
            }
        }

        private void RestoreMana()
        {
            // Type checking - only Fighter has mana system
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
            // Abstract class polymorphism! Same method calls, different implementations!
            switch (keyCode)
            {
                case Keys.I: 
                    currentCharacter.SetIdle(); // Abstract method - implemented by each class
                    break;
                case Keys.W: 
                    currentCharacter.Walk();    // Abstract method - different speeds & behaviors!
                    break;
                case Keys.R: 
                    currentCharacter.Run();     // Abstract method - different speeds & behaviors!
                    break;
                case Keys.A: 
                    currentCharacter.Attack();  // Virtual method - different power & effects!
                    break;
                case Keys.J: 
                    currentCharacter.Jump();    // Virtual method - different distances & effects!
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
                        // Check if character is shielding
                        if (currentCharacter is IShieldable shieldable && shieldable.IsShielding)
                        {
                            // Don't set idle if shielding - let shield continue
                        }
                        else
                        {
                            currentCharacter.SetIdle(); // Abstract method call
                        }
                    }
                    break;
            }

            UpdateTitle();
        }

        private void ResetCharacter()
        {
            currentCharacter.X = 100;
            currentCharacter.Y = 200;
            currentCharacter.SetIdle(); // Abstract method call
            
            // Interface-specific resets
            if (currentCharacter is IShieldable shieldableChar)
            {
                shieldableChar.DeactivateShield(); // Interface method
            }
            
            // Type-specific resets
            if (currentCharacter is Fighter fighterChar)
            {
                fighterChar.RestoreMana(100); // Full restore
            }
            
            // Force full repaint to clear ghost sprites
            this.Invalidate();
        }

        private void UpdateTitle()
        {
            string characterType = currentCharacter.GetCharacterType(); // Abstract method
            string shieldInfo = "";
            
            // Interface polymorphism - check shield status
            if (currentCharacter is IShieldable shieldable)
            {
                shieldInfo = shieldable.IsShielding ? " [SHIELDING]" : " [Shield Available]";
            }
            else
            {
                shieldInfo = " [No Shield]";
            }
            
            this.Text = $"Lab04 - Abstract Classes & Interfaces: {characterType}{shieldInfo}";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Cleanup resources - abstract class method
            CleanupResources();
        }

        private void CleanupResources()
        {
            animationTimer?.Stop();
            animationTimer?.Dispose();
            
            // Abstract class polymorphism in cleanup
            ninja?.Dispose();
            samurai?.Dispose();
            fighter?.Dispose();
        }
    }
}

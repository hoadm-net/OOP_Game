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
        // Polymorphism demonstration - single Character reference
        private Character character;
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

            // Polymorphism in action! Character reference can hold any derived type
            CreateCharacter();
            UpdateLastPosition();

            // Setup animation timer - 60 FPS
            SetupAnimationTimer();

            // Set initial title
            UpdateTitle();
        }

        private void CreateCharacter()
        {
            // Clean up previous character
            character?.Dispose();

            // Polymorphism demonstration - same reference type, different objects!
            switch (characterIndex)
            {
                case 0:
                    character = new Shinobi("Shadow Ninja", 100, 200);      // No interfaces
                    break;
                case 1:
                    character = new Samurai("Blade Samurai", 100, 200);     // Implements IShieldable
                    break;
                case 2:
                    character = new Fighter("Swift Fighter", 100, 200);     // Implements IShieldable + has mana
                    break;
            }
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
            // Polymorphism: same method call, different implementations based on actual object type
            character.UpdateAnimation();
            
            // Invalidate old and new regions for efficient redraw
            InvalidateCharacterRegions();
            
            UpdateLastPosition();
        }

        private void UpdateLastPosition()
        {
            lastX = character.X;
            lastY = character.Y;
        }

        private void InvalidateCharacterRegions()
        {
            const int margin = 10;
            const int size = 148;
            
            Rectangle oldRect = new Rectangle(lastX - margin, lastY - margin, size, size);
            Rectangle newRect = new Rectangle(character.X - margin, character.Y - margin, size, size);
            
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
            // Polymorphism: same method call works for all derived types
            var sprite = character.GetCurrentSprite();
            if (sprite != null)
            {
                var sourceRect = character.GetCurrentFrameRect();
                var destRect = new Rectangle(character.X, character.Y, 128, 128);
                
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

            // Handle actions - polymorphism in action!
            HandleInput(e.KeyCode);
            UpdateTitle();
        }

        private void SwitchCharacter()
        {
            // Cycle through character types
            characterIndex = (characterIndex + 1) % 3;
            
            // Store current position
            int currentX = character.X;
            int currentY = character.Y;
            
            // Polymorphism demonstration: Create new instance of different type
            CreateCharacter();
            
            // Restore position for new character
            character.X = currentX;
            character.Y = currentY;
            character.SetIdle(); // Polymorphic method call
            
            this.Invalidate(); // Full repaint when switching
        }

        private void ToggleShield()
        {
            // Interface polymorphism - runtime type checking
            if (character is IShieldable shieldableChar)
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
            // Note: Shinobi doesn't implement IShieldable, so shield key does nothing
        }

        private void RestoreMana()
        {
            // Type-specific functionality - runtime type checking
            if (character is Fighter fighterChar)
            {
                fighterChar.RestoreMana();
            }
            // Note: Only Fighter has mana system
        }

        private void HandleInput(Keys keyCode)
        {
            // Polymorphism in action! Same method calls, different behaviors based on actual object type
            switch (keyCode)
            {
                case Keys.I: 
                    character.SetIdle(); // Abstract method - each class implements differently
                    break;
                case Keys.W: 
                    character.Walk();    // Virtual method - different speeds based on GetWalkSpeed()!
                    break;
                case Keys.R: 
                    character.Run();     // Virtual method - different speeds based on GetRunSpeed()!
                    break;
                case Keys.A: 
                    character.Attack();  // Virtual method - different behaviors per class!
                    break;
                case Keys.J: 
                    character.Jump();    // Virtual method - different distances based on GetJumpDistance()!
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
                    if (!character.IsInOneShotAnimation())
                    {
                        // Check if character is shielding (interface polymorphism)
                        if (character is IShieldable shieldable && shieldable.IsShielding)
                        {
                            // Don't set idle if shielding - let shield continue
                        }
                        else
                        {
                            character.SetIdle(); // Polymorphic method call
                        }
                    }
                    break;
            }

            UpdateTitle();
        }

        private void ResetCharacter()
        {
            character.X = 100;
            character.Y = 200;
            character.SetIdle(); // Polymorphic method call
            
            // Interface-specific resets (runtime type checking)
            if (character is IShieldable shieldableChar)
            {
                shieldableChar.DeactivateShield(); // Interface method
            }
            
            // Type-specific resets (runtime type checking)
            if (character is Fighter fighterChar)
            {
                fighterChar.RestoreMana(100); // Full restore
            }
            
            // Force full repaint to clear ghost sprites
            this.Invalidate();
        }

        private void UpdateTitle()
        {
            // Polymorphism: GetCharacterType() returns different values based on actual object type
            string characterType = character.GetCharacterType(); // Abstract method implementation
            string actualType = character.GetType().Name; // Runtime type information
            string shieldInfo = "";
            
            // Interface polymorphism - check shield status
            if (character is IShieldable shieldable)
            {
                shieldInfo = shieldable.IsShielding ? " [SHIELDING]" : " [Shield Available]";
            }
            else
            {
                shieldInfo = " [No Shield]";
            }
            
            this.Text = $"Lab04 - Polymorphism Demo: Character reference -> {actualType} ({characterType}){shieldInfo}";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Cleanup resources - polymorphic method call
            CleanupResources();
        }

        private void CleanupResources()
        {
            animationTimer?.Stop();
            animationTimer?.Dispose();
            
            // Polymorphic cleanup
            character?.Dispose();
        }
    }
}

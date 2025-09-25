using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab01
{
    public partial class Form1 : Form
    {
        private Shinobi myNinja;
        private Samurai mySamurai;
        private bool isUsingShinobi = false; // Switch between character types
        private Dictionary<string, Image> shinobiSprites;
        private Dictionary<string, Image> samuraiSprites;
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

            // Create both character objects
            myNinja = new Shinobi("Shadow Ninja", 100, 200);
            mySamurai = new Samurai("Blade Samurai", 100, 200);
            
            UpdateLastPosition();

            // Load sprite sheets for both characters
            LoadAllSpriteSheets();

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
            // Update animation for current character
            if (isUsingShinobi)
            {
                myNinja.UpdateAnimation();
            }
            else
            {
                mySamurai.UpdateAnimation();
            }
            
            // Invalidate old and new regions for efficient redraw
            InvalidateCharacterRegions();
            
            UpdateLastPosition();
        }

        private void UpdateLastPosition()
        {
            if (isUsingShinobi)
            {
                lastX = myNinja.X;
                lastY = myNinja.Y;
            }
            else
            {
                lastX = mySamurai.X;
                lastY = mySamurai.Y;
            }
        }

        private void InvalidateCharacterRegions()
        {
            const int margin = 10;
            const int size = 148;
            
            int currentX = isUsingShinobi ? myNinja.X : mySamurai.X;
            int currentY = isUsingShinobi ? myNinja.Y : mySamurai.Y;
            
            Rectangle oldRect = new Rectangle(lastX - margin, lastY - margin, size, size);
            Rectangle newRect = new Rectangle(currentX - margin, currentY - margin, size, size);
            
            this.Invalidate(oldRect);
            this.Invalidate(newRect);
        }

        private void LoadAllSpriteSheets()
        {
            // Load Shinobi sprites
            shinobiSprites = new Dictionary<string, Image>();
            try
            {
                string shinobiPath = System.IO.Path.Combine(Application.StartupPath, "..", "..", "..", "assets", "Shinobi");
                shinobiSprites["Idle"] = Image.FromFile(System.IO.Path.Combine(shinobiPath, "Idle.png"));
                shinobiSprites["Walk"] = Image.FromFile(System.IO.Path.Combine(shinobiPath, "Walk.png"));
                shinobiSprites["Run"] = Image.FromFile(System.IO.Path.Combine(shinobiPath, "Run.png"));
                shinobiSprites["Attack"] = Image.FromFile(System.IO.Path.Combine(shinobiPath, "Attack.png"));
                shinobiSprites["Jump"] = Image.FromFile(System.IO.Path.Combine(shinobiPath, "Jump.png"));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot load Shinobi sprites: " + ex.Message);
            }

            // Load Samurai sprites
            samuraiSprites = new Dictionary<string, Image>();
            try
            {
                string samuraiPath = System.IO.Path.Combine(Application.StartupPath, "..", "..", "..", "assets", "Samurai");
                samuraiSprites["Idle"] = Image.FromFile(System.IO.Path.Combine(samuraiPath, "Idle.png"));
                samuraiSprites["Walk"] = Image.FromFile(System.IO.Path.Combine(samuraiPath, "Walk.png"));
                samuraiSprites["Run"] = Image.FromFile(System.IO.Path.Combine(samuraiPath, "Run.png"));
                samuraiSprites["Attack"] = Image.FromFile(System.IO.Path.Combine(samuraiPath, "Attack.png"));
                samuraiSprites["Jump"] = Image.FromFile(System.IO.Path.Combine(samuraiPath, "Jump.png"));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot load Samurai sprites: " + ex.Message);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Clear background
            e.Graphics.Clear(Color.LightBlue);

            // Draw current character
            DrawCharacter(e.Graphics);

            // Draw controls help
            DrawControlsHelp(e.Graphics);
        }

        private void DrawCharacter(Graphics graphics)
        {
            Dictionary<string, Image> currentSprites = isUsingShinobi ? shinobiSprites : samuraiSprites;
            string currentAction = isUsingShinobi ? myNinja.CurrentAction : mySamurai.CurrentAction;
            Rectangle currentFrameRect = isUsingShinobi ? myNinja.GetCurrentFrameRect() : mySamurai.GetCurrentFrameRect();
            int currentX = isUsingShinobi ? myNinja.X : mySamurai.X;
            int currentY = isUsingShinobi ? myNinja.Y : mySamurai.Y;

            if (currentSprites.ContainsKey(currentAction))
            {
                var spriteSheet = currentSprites[currentAction];
                var destRect = new Rectangle(currentX, currentY, 128, 128);
                
                // Use nearest neighbor for crisp pixel art
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                graphics.DrawImage(spriteSheet, destRect, currentFrameRect, GraphicsUnit.Pixel);
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

            // Handle actions for current character
            if (isUsingShinobi)
            {
                HandleShinobiInput(e.KeyCode);
            }
            else
            {
                HandleSamuraiInput(e.KeyCode);
            }

            UpdateTitle();
        }

        private void SwitchCharacter()
        {
            isUsingShinobi = !isUsingShinobi;
            
            // Sync positions when switching
            if (isUsingShinobi)
            {
                myNinja.X = mySamurai.X;
                myNinja.Y = mySamurai.Y;
                myNinja.SetIdle();
            }
            else
            {
                mySamurai.X = myNinja.X;
                mySamurai.Y = myNinja.Y;
                mySamurai.SetIdle();
            }
            
            this.Invalidate(); // Full repaint when switching
        }

        private void HandleShinobiInput(Keys keyCode)
        {
            switch (keyCode)
            {
                case Keys.I: myNinja.SetIdle(); break;
                case Keys.W: myNinja.Walk(); break;
                case Keys.R: myNinja.Run(); break;
                case Keys.A: myNinja.Attack(); break;
                case Keys.J: myNinja.Jump(); break;
                case Keys.Space: ResetCharacter(); break;
            }
        }

        private void HandleSamuraiInput(Keys keyCode)
        {
            switch (keyCode)
            {
                case Keys.I: mySamurai.SetIdle(); break;
                case Keys.W: mySamurai.Walk(); break;
                case Keys.R: mySamurai.Run(); break;
                case Keys.A: mySamurai.Attack(); break;
                case Keys.J: mySamurai.Jump(); break;
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
                    if (isUsingShinobi)
                    {
                        if (!myNinja.IsInOneShotAnimation())
                            myNinja.SetIdle();
                    }
                    else
                    {
                        if (!mySamurai.IsInOneShotAnimation())
                            mySamurai.SetIdle();
                    }
                    break;
            }

            UpdateTitle();
        }

        private void ResetCharacter()
        {
            if (isUsingShinobi)
            {
                myNinja.X = 100;
                myNinja.Y = 200;
                myNinja.SetIdle();
            }
            else
            {
                mySamurai.X = 100;
                mySamurai.Y = 200;
                mySamurai.SetIdle();
            }
            
            // Force full repaint to clear ghost sprites
            this.Invalidate();
        }

        private void UpdateTitle()
        {
            string characterInfo = isUsingShinobi ? 
                $"{myNinja.Name} | Health: {myNinja.Health} | Action: {myNinja.CurrentAction}" :
                $"{mySamurai.Name} | Health: {mySamurai.Health} | Action: {mySamurai.CurrentAction}";
                
            this.Text = $"Lab01 - {characterInfo}";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Cleanup resources
            CleanupResources();
        }

        private void CleanupResources()
        {
            animationTimer?.Stop();
            animationTimer?.Dispose();
            
            // Cleanup Shinobi sprites
            if (shinobiSprites != null)
            {
                foreach (var sprite in shinobiSprites.Values)
                {
                    sprite?.Dispose();
                }
            }
            
            // Cleanup Samurai sprites
            if (samuraiSprites != null)
            {
                foreach (var sprite in samuraiSprites.Values)
                {
                    sprite?.Dispose();
                }
            }
        }
    }
}

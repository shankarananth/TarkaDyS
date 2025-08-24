/*
 * File: SplashScreen.cs
 * Author: Shankar Ananth Asokan
 * Purpose: Simple logo-only splash screen for application startup
 * Date Created: 24-Aug-2025
 * Date Modified: 24-Aug-2025
 * 
 * Description: Minimal splash screen that displays only the application logo
 * for a few seconds during startup. Automatically loads logo from Resources
 * folder and falls back to text if no image is found.
 */

using System;
using System.Drawing;
using System.Windows.Forms;
using TarkaDyS.Utilities;

namespace TarkaDyS.Forms
{
    /// <summary>
    /// Simple logo-only splash screen for application startup
    /// Shows logo for 3 seconds then automatically closes
    /// </summary>
    public partial class SplashScreen : Form
    {
        #region Private Fields
        private PictureBox pictureBoxLogo;
        private System.Windows.Forms.Timer displayTimer;
        #endregion

        #region Constructor
        public SplashScreen()
        {
            InitializeComponent();
            LoadLogoAndSetup();
            SetupTimer();
        }
        #endregion

        #region Form Designer Code
        /// <summary>
        /// Designer-generated initialization code
        /// Sets up the basic form layout and controls
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBoxLogo = new PictureBox();
            this.displayTimer = new System.Windows.Forms.Timer();
            
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.SuspendLayout();
            
            // 
            // pictureBoxLogo - Main logo display control
            // 
            this.pictureBoxLogo.BackColor = Color.Transparent;
            this.pictureBoxLogo.Dock = DockStyle.Fill;
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBoxLogo.TabIndex = 0;
            this.pictureBoxLogo.TabStop = false;
            
            // 
            // displayTimer - Controls splash screen duration
            // 
            this.displayTimer.Interval = 3000; // 3 seconds display time
            this.displayTimer.Tick += DisplayTimer_Tick;
            
            // 
            // SplashScreen - Main form properties
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(300, 200); // Default size, adjusted by logo
            this.ControlBox = false;               // No control buttons
            this.FormBorderStyle = FormBorderStyle.None; // Borderless window
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SplashScreen";
            this.ShowIcon = false;                 // No icon in taskbar
            this.ShowInTaskbar = false;            // Don't show in taskbar
            this.StartPosition = FormStartPosition.CenterScreen; // Center on screen
            this.TopMost = true;                   // Always on top
            this.Controls.Add(this.pictureBoxLogo);
            
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.ResumeLayout(false);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Load application logo and configure form size
        /// Called after form initialization to avoid designer issues
        /// </summary>
        private void LoadLogoAndSetup()
        {
            var resourcesFolder = ResourceHelper.GetResourcesFolder();
            System.Diagnostics.Debug.WriteLine($"SplashScreen: Looking for logo in: {resourcesFolder}");
            System.Diagnostics.Debug.WriteLine($"SplashScreen: Resources folder exists: {System.IO.Directory.Exists(resourcesFolder)}");
            
            // Attempt to load logo from Resources folder
            var logo = ResourceHelper.TryLoadLogo();
            if (logo != null)
            {
                System.Diagnostics.Debug.WriteLine($"SplashScreen: Logo loaded successfully! Size: {logo.Size}");
                this.pictureBoxLogo.Image = logo;
                
                // Resize form to optimal size for logo display
                var logoSize = GetOptimalLogoSize(logo.Size);
                this.ClientSize = logoSize;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("SplashScreen: No logo found - creating fallback text logo");
                CreateFallbackTextLogo();
                this.ClientSize = new Size(300, 200);
            }
            
            // Apply application icon if available
            ResourceHelper.ApplyAppIcon(this);
        }

        /// <summary>
        /// Calculate optimal splash screen size based on logo dimensions
        /// Maintains aspect ratio while limiting maximum size
        /// </summary>
        /// <param name="logoSize">Original logo dimensions</param>
        /// <returns>Optimal form size for splash screen</returns>
        private Size GetOptimalLogoSize(Size logoSize)
        {
            const int maxWidth = 400;   // Maximum splash screen width
            const int maxHeight = 300;  // Maximum splash screen height
            
            double aspectRatio = (double)logoSize.Width / logoSize.Height;
            
            int width = Math.Min(logoSize.Width, maxWidth);
            int height = (int)(width / aspectRatio);
            
            // Adjust if height exceeds maximum
            if (height > maxHeight)
            {
                height = maxHeight;
                width = (int)(height * aspectRatio);
            }
            
            return new Size(width, height);
        }

        /// <summary>
        /// Setup display timer to automatically close splash screen
        /// Timer starts when form is shown to user
        /// </summary>
        private void SetupTimer()
        {
            this.Shown += (sender, e) => displayTimer.Start();
        }

        /// <summary>
        /// Create fallback text-based logo when image is not available
        /// Generates simple bitmap with application name
        /// </summary>
        private void CreateFallbackTextLogo()
        {
            Bitmap logoBitmap = new Bitmap(300, 200);
            using (Graphics g = Graphics.FromImage(logoBitmap))
            {
                g.Clear(Color.White);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                
                // Draw application name centered
                using (Font logoFont = new Font("Segoe UI", 28F, FontStyle.Bold))
                using (SolidBrush brush = new SolidBrush(Color.DarkBlue))
                {
                    var textSize = g.MeasureString("TarkaDyS", logoFont);
                    var x = (300 - textSize.Width) / 2;
                    var y = (200 - textSize.Height) / 2;
                    g.DrawString("TarkaDyS", logoFont, brush, new PointF(x, y));
                }
            }
            
            this.pictureBoxLogo.Image = logoBitmap;
        }

        /// <summary>
        /// Handle display timer tick - close splash screen
        /// </summary>
        private void DisplayTimer_Tick(object sender, EventArgs e)
        {
            displayTimer.Stop();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        #endregion

        #region Dispose Pattern
        /// <summary>
        /// Clean up resources when form is disposed
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                displayTimer?.Dispose();
                pictureBoxLogo?.Image?.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
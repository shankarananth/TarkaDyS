/*
 * File: MainForm.Designer.cs
 * Author: Shankar Ananth Asokan
 * Purpose: Main form designer with Models menu
 * Date Created: 2025-08-23
 * Date Modified: 2025-08-23
 */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace TarkaDyS
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Menu and UI components
        /// </summary>
        private MenuStrip menuStrip;
        private ToolStripMenuItem modelsToolStripMenuItem;
        private ToolStripMenuItem menuFirstOrderPid;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem menuAbout;
        private Panel mainPanel;
        private Label lblWelcome;
        private Label lblInstructions;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            menuStrip = new MenuStrip();
            modelsToolStripMenuItem = new ToolStripMenuItem();
            menuFirstOrderPid = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            menuAbout = new ToolStripMenuItem();
            mainPanel = new Panel();
            lblWelcome = new Label();
            lblInstructions = new Label();
            menuStrip.SuspendLayout();
            mainPanel.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(20, 20);
            menuStrip.Items.AddRange(new ToolStripItem[] { modelsToolStripMenuItem, menuAbout });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Padding = new Padding(10, 4, 0, 4);
            menuStrip.Size = new Size(1250, 40);
            menuStrip.TabIndex = 0;
            // 
            // modelsToolStripMenuItem
            // 
            modelsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { menuFirstOrderPid, toolStripSeparator1 });
            modelsToolStripMenuItem.Font = new Font("Segoe UI", 10F);
            modelsToolStripMenuItem.Name = "modelsToolStripMenuItem";
            modelsToolStripMenuItem.Size = new Size(93, 32);
            modelsToolStripMenuItem.Text = "&Models";
            // 
            // menuFirstOrderPid
            // 
            menuFirstOrderPid.Name = "menuFirstOrderPid";
            menuFirstOrderPid.ShortcutKeys = Keys.Control | Keys.D1;
            menuFirstOrderPid.Size = new Size(422, 36);
            menuFirstOrderPid.Text = "&First Order Process with PID";
            menuFirstOrderPid.Click += MenuFirstOrderPid_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(419, 6);
            // 
            // menuAbout
            // 
            menuAbout.Name = "menuAbout";
            menuAbout.Size = new Size(78, 32);
            menuAbout.Text = "&About";
            menuAbout.Click += MenuAbout_Click;
            // 
            // mainPanel
            // 
            mainPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            mainPanel.BackColor = Color.WhiteSmoke;
            mainPanel.BorderStyle = BorderStyle.FixedSingle;
            mainPanel.Controls.Add(lblWelcome);
            mainPanel.Controls.Add(lblInstructions);
            mainPanel.Location = new Point(15, 50);
            mainPanel.Margin = new Padding(4, 4, 4, 4);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(1220, 684);
            mainPanel.TabIndex = 1;
            // 
            // lblWelcome
            // 
            lblWelcome.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblWelcome.ForeColor = Color.DarkBlue;
            lblWelcome.Location = new Point(38, 50);
            lblWelcome.Margin = new Padding(4, 0, 4, 0);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Size = new Size(1125, 62);
            lblWelcome.TabIndex = 0;
            lblWelcome.Text = "TarkaDyS - Process Dynamic Simulator";
            lblWelcome.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblInstructions
            // 
            lblInstructions.Font = new Font("Segoe UI", 12F);
            lblInstructions.ForeColor = Color.DarkSlateGray;
            lblInstructions.Location = new Point(38, 150);
            lblInstructions.Margin = new Padding(4, 0, 4, 0);
            lblInstructions.Name = "lblInstructions";
            lblInstructions.Size = new Size(1125, 375);
            lblInstructions.TabIndex = 1;
            lblInstructions.Text = resources.GetString("lblInstructions.Text");
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1250, 750);
            Controls.Add(mainPanel);
            Controls.Add(menuStrip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip;
            Margin = new Padding(4, 4, 4, 4);
            MinimumSize = new Size(994, 611);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TarkaDyS - Process Dynamic Simulator";
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            mainPanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        private void MenuAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "TarkaDyS - Professional PID Control System Simulation\n\n" +
                "Author: Shankar Ananth Asokan\n" +
                "Version: Professional Edition\n" +
                "Date: 2025-08-23\n\n" +
                "Features:\n" +
                "• Multiple PID algorithms (BasicPID, I-PD, PI-D)\n" +
                "• Professional control system features\n" +
                "• Real-time parameter tuning\n" +
                "• Single instance model management\n" +
                "• Anti-windup protection\n" +
                "• Configurable limits and plot controls\n\n" +
                "Perfect for control system education, research, and industrial training!",
                "About TarkaDyS",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        #endregion
    }
}
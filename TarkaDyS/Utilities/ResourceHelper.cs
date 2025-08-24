/*
 * File: ResourceHelper.cs
 * Author: Shankar Ananth Asokan
 * Purpose: Utility class for loading application resources (logos, icons)
 * Date Created: 24-Aug-2025
 * Date Modified: 24-Aug-2025
 * 
 * Description: Provides methods to load application assets from the Resources folder.
 * Supports multiple image formats and provides fallback handling.
 * Automatically detects and loads logos and icons for splash screen and forms.
 */

using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TarkaDyS.Utilities
{
    /// <summary>
    /// Resource management utility for loading application assets
    /// </summary>
    public static class ResourceHelper
    {
        #region Private Fields
        /// <summary>Supported logo file formats in order of preference</summary>
        private static readonly string[] LogoCandidates = new[]
        {
            "TarkaDyS_Logo.png",
            "TarkaDyS_Logo.jpg",
            "TarkaDyS_Logo.jpeg",
            "TarkaDyS_Logo.bmp"
        };
        #endregion

        #region Public Methods
        /// <summary>
        /// Get the path to the Resources folder
        /// Checks multiple possible locations for development and deployment
        /// </summary>
        /// <returns>Path to Resources folder</returns>
        public static string GetResourcesFolder()
        {
            // Primary location: Application base directory
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var resources = Path.Combine(baseDir, "Resources");
            if (Directory.Exists(resources)) return resources;

            // Fallback location: Application startup path
            var startupResources = Path.Combine(Application.StartupPath, "Resources");
            if (Directory.Exists(startupResources)) return startupResources;

            // Return primary path even if it doesn't exist (for creation)
            return resources;
        }

        /// <summary>
        /// Try to load the application icon from Resources folder
        /// </summary>
        /// <returns>Icon object if found, null if not found</returns>
        public static Icon? TryLoadAppIcon()
        {
            try
            {
                var folder = GetResourcesFolder();
                var iconPath = Path.Combine(folder, "TarkaDyS_Icon.ico");
                if (File.Exists(iconPath))
                {
                    return new Icon(iconPath);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ResourceHelper: Error loading app icon: {ex.Message}");
            }
            return null;
        }

        /// <summary>
        /// Try to load the application logo from Resources folder
        /// Supports multiple image formats with detailed debugging
        /// </summary>
        /// <returns>Image object if found, null if not found</returns>
        public static Image? TryLoadLogo()
        {
            try
            {
                var folder = GetResourcesFolder();
                System.Diagnostics.Debug.WriteLine($"ResourceHelper: Checking folder: {folder}");
                System.Diagnostics.Debug.WriteLine($"ResourceHelper: Folder exists: {Directory.Exists(folder)}");
                
                // List all files in Resources folder for debugging
                if (Directory.Exists(folder))
                {
                    var files = Directory.GetFiles(folder);
                    System.Diagnostics.Debug.WriteLine($"ResourceHelper: Files in Resources folder:");
                    foreach (var file in files)
                    {
                        var fileInfo = new FileInfo(file);
                        System.Diagnostics.Debug.WriteLine($"  - {Path.GetFileName(file)} ({fileInfo.Length:N0} bytes)");
                    }
                }
                
                // Try each supported logo format
                foreach (var fileName in LogoCandidates)
                {
                    var filePath = Path.Combine(folder, fileName);
                    System.Diagnostics.Debug.WriteLine($"ResourceHelper: Checking: {filePath}");
                    
                    if (File.Exists(filePath))
                    {
                        var fileInfo = new FileInfo(filePath);
                        System.Diagnostics.Debug.WriteLine($"ResourceHelper: File found! Size: {fileInfo.Length:N0} bytes");
                        
                        try
                        {
                            // Load image with proper disposal pattern
                            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                            var image = Image.FromStream(fileStream);
                            System.Diagnostics.Debug.WriteLine($"ResourceHelper: Logo loaded successfully! Dimensions: {image.Width}×{image.Height}");
                            return image;
                        }
                        catch (Exception loadEx)
                        {
                            System.Diagnostics.Debug.WriteLine($"ResourceHelper: Error loading image: {loadEx.Message}");
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"ResourceHelper: File not found: {filePath}");
                    }
                }
                
                System.Diagnostics.Debug.WriteLine("ResourceHelper: No logo files found in any supported format");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ResourceHelper: Error in TryLoadLogo: {ex.Message}");
            }
            return null;
        }

        /// <summary>
        /// Apply application icon to a form if available
        /// Safe method that handles errors gracefully
        /// </summary>
        /// <param name="form">Form to apply icon to</param>
        public static void ApplyAppIcon(Form form)
        {
            try
            {
                var icon = TryLoadAppIcon();
                if (icon != null)
                {
                    form.Icon = icon;
                    System.Diagnostics.Debug.WriteLine($"ResourceHelper: Icon applied to {form.Name}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ResourceHelper: Error applying icon to {form.Name}: {ex.Message}");
            }
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;

namespace WinFontSwitcher
{
    public partial class MainWindow
    {
        private static readonly List<KeyValuePair<string, string>> DefaultFonts = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("Segoe UI (TrueType)", "segoeui.ttf"),
            new KeyValuePair<string, string>("Segoe UI Black (TrueType)", "seguibl.ttf"),
            new KeyValuePair<string, string>("Segoe UI Black Italic (TrueType)", "seguibli.ttf"),
            new KeyValuePair<string, string>("Segoe UI Bold (TrueType)", "segoeuib.ttf"),
            new KeyValuePair<string, string>("Segoe UI Bold Italic (TrueType)", "segoeuiz.ttf"),
            new KeyValuePair<string, string>("Segoe UI Emoji (TrueType)", "seguiemj.ttf"),
            new KeyValuePair<string, string>("Segoe UI Historic (TrueType)", "seguihis.ttf"),
            new KeyValuePair<string, string>("Segoe UI Italic (TrueType)", "segoeuii.ttf"),
            new KeyValuePair<string, string>("Segoe UI Light (TrueType)", "segoeuil.ttf"),
            new KeyValuePair<string, string>("Segoe UI Light Italic (TrueType)", "seguili.ttf"),
            new KeyValuePair<string, string>("Segoe UI Semibold (TrueType)", "seguisb.ttf"),
            new KeyValuePair<string, string>("Segoe UI Semibold Italic (TrueType)", "seguisbi.ttf"),
            new KeyValuePair<string, string>("Segoe UI Semilight (TrueType)", "segoeuisl.ttf"),
            new KeyValuePair<string, string>("Segoe UI Semilight Italic (TrueType)", "seguisli.ttf"),
            new KeyValuePair<string, string>("Segoe UI Symbol (TrueType)", "seguisym.ttf"),
            new KeyValuePair<string, string>("Segoe MDL2 Assets (TrueType)", "segmdl2.ttf"),
            new KeyValuePair<string, string>("Segoe Print (TrueType)", "segoepr.ttf"),
            new KeyValuePair<string, string>("Segoe Print Bold (TrueType)", "segoeprb.ttf"),
            new KeyValuePair<string, string>("Segoe Script (TrueType)", "segoesc.ttf"),
            new KeyValuePair<string, string>("Segoe Script Bold (TrueType)", "segoescb.ttf")
        };

        private string _currentFont;

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                _currentFont = GetCurrentFont();
                ResetButton.IsEnabled = _currentFont != "Segoe UI";
                ListFonts.SelectedValue = _currentFont;
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private static string GetCurrentFont()
        {
            using (var key =
                Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\FontSubstitutes")
            )
            {
                var font = (key?.GetValue("Segoe UI") ?? "Segoe UI") as string;
                return font;
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ResetFont();
                if (MessageBox.Show(
                        "You need to restart your computer to be able to use your new font.\nDo you want to restart it now?",
                        "Apply your changes",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question
                    ) == MessageBoxResult.Yes)
                {
                    Process.Start("shutdown", "/r /t 0");
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newFont = ListFonts.SelectedValue.ToString();
                SetFont(newFont);
                _currentFont = newFont;
                ApplyButton.IsEnabled = false;
                ResetButton.IsEnabled = _currentFont != "Segoe UI";
                if (MessageBox.Show(
                        $"You need to restart your computer to be able to use \"{newFont}\" as your new font.\nDo you want to restart it now?",
                        "Apply your changes",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question
                    ) == MessageBoxResult.Yes)
                {
                    Process.Start("shutdown", "/r /t 0");
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void ListFonts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyButton.IsEnabled = ListFonts.SelectedValue.ToString() != _currentFont;
        }

        private static void SetFont(string font)
        {
            if (string.IsNullOrWhiteSpace(font))
                return;
            if (font == "Segoe UI")
                ResetFont();
            else
            {
                using (
                    var key = Registry.LocalMachine.OpenSubKey(
                        @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\FontSubstitutes",
                        true))
                {
                    if (key == null)
                        throw new Exception("Unable to access the registry key.");
                    key.SetValue("Segoe UI", font, RegistryValueKind.String);
                }

                using (
                    var key = Registry.LocalMachine.OpenSubKey(
                        @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts",
                        true
                    ))
                {
                    if (key == null)
                        throw new Exception("Unable to access the registry key.");
                    foreach (var keyValuePair in DefaultFonts)
                        key.SetValue(keyValuePair.Key, "", RegistryValueKind.String);
                }
            }
        }

        private static void ResetFont()
        {
            using (
                var key = Registry.LocalMachine.OpenSubKey(
                    @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts",
                    true
                ))
            {
                if (key == null)
                    throw new Exception("Unable to access the registry key!");
                foreach (var keyValuePair in DefaultFonts)
                    key.SetValue(keyValuePair.Key, keyValuePair.Value, RegistryValueKind.String);
            }

            using (
                var key = Registry.LocalMachine.OpenSubKey(
                    @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\FontSubstitutes",
                    true))
            {
                if (key == null)
                    throw new Exception("Unable to access the registry key!");
                key.DeleteValue("Segoe UI");
            }
        }

        private static void ShowError(string msg)
        {
            MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
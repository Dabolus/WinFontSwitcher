using System;
using System.Windows;
using System.Windows.Controls;

namespace WinFontSwitcher
{
    public partial class FontSwitcherView
    {

        private string _currentFont;

        public FontSwitcherView()
        {
            InitializeComponent();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            /*try
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
            }*/
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            /*try
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
            }*/
        }
        /*
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
        }*/

        private static void ShowError(string msg)
        {
            MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
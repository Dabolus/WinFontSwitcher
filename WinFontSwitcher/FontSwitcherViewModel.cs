using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace System.Runtime.CompilerServices {
    internal sealed class CallerMemberNameAttribute : Attribute { }
}

namespace WinFontSwitcher {
    public class FontSwitcherViewModel : INotifyPropertyChanged {
        private readonly FontSwitcherModel _fs;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        // This variables and computations are merely for the UI, so we can leave them out of the model
        public string MainUIVisibility => _isShowingPreview ? "Hidden" : "Visible";
        public string TestFontUIVisibility => _isShowingPreview ? "Visible" : "Hidden";

        public string TestFontButtonText =>
            _isShowingPreview ? Properties.Resources.GoBackButton : Properties.Resources.TestFontButton;

        private bool _isShowingPreview;

        public ICommand ApplyCommand { get; set; }
        public ICommand ResetCommand { get; set; }

        public ICommand TestFontCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        public FontSwitcherViewModel() {
            try {
                _fs = new FontSwitcherModel();
                ApplyCommand = new RelayCommand(ApplyFont);
                ResetCommand = new RelayCommand(ResetFont);
                TestFontCommand = new RelayCommand(OpenFontPreview);
                ExitCommand = new RelayCommand(Exit);
            }
            catch (Exception e) {
                ShowError(string.Format(Properties.Resources.UnableToInitializeError, e.Message));
                Environment.Exit(1);
            }
        }

        private void OpenFontPreview(object ignored) {
            _isShowingPreview = !_isShowingPreview;
            NotifyPropertyChanged("MainUIVisibility");
            NotifyPropertyChanged("TestFontUIVisibility");
            NotifyPropertyChanged("TestFontButtonText");
        }

        private void ApplyFont(object ignored) {
            try {
                _fs.ApplyFont();
                ShowFontAppliedPrompt(SelectedPrimaryFont.Key);
            }
            catch (Exception e) {
                ShowError(e.Message);
            }
        }

        private void ResetFont(object ignored) {
            try {
                _fs.ResetFont();
                ShowFontResetPrompt();
            }
            catch (Exception e) {
                ShowError(e.Message);
            }
        }

        private void Exit(object ignored) => Environment.Exit(0);

        public IList<KeyValuePair<string, string>> RegistrySystemFonts => _fs.RegistrySystemFonts;

        public IList<MutableKeyVal<string, bool>> FontsToReplace => _fs.FontsToReplace;

        public KeyValuePair<string, string> SelectedPrimaryFont {
            get => _fs.SelectedPrimaryFont;
            set {
                _fs.SelectedPrimaryFont = value;
                NotifyPropertyChanged();
            }
        }

        public KeyValuePair<string, string> SelectedFallbackFont {
            get => _fs.SelectedFallbackFont;
            set {
                _fs.SelectedFallbackFont = value;
                NotifyPropertyChanged();
            }
        }

        private static void ShowError(string msg) =>
            MessageBox.Show(msg, Properties.Resources.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);

        private static void ShowFontAppliedPrompt(string fontName) {
            if (MessageBox.Show(
                    string.Format(Properties.Resources.FontAppliedPromptContent, fontName),
                    Properties.Resources.FontAppliedPromptTitle,
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                ) == MessageBoxResult.Yes)
                Process.Start("shutdown", "/r /t 0");
        }

        private static void ShowFontResetPrompt() {
            if (MessageBox.Show(
                    Properties.Resources.FontResetPromptContent,
                    Properties.Resources.FontAppliedPromptTitle,
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                ) == MessageBoxResult.Yes)
                Process.Start("shutdown", "/r /t 0");
        }
    }
}
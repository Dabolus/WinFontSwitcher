using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string MainUIVisibility => _isShowingPreview ? "Hidden" : "Visible";
        public string TestFontUIVisibility => _isShowingPreview ? "Visible" : "Hidden";

        public string TestFontButtonText =>
            _isShowingPreview ? Properties.Resources.GoBackButton : Properties.Resources.TestFontButton;

        private bool _isShowingPreview;

        public ICommand ApplyCommand { get; set; }

        public ICommand TestFontCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        public FontSwitcherViewModel() {
            _fs = new FontSwitcherModel();
            ApplyCommand = new RelayCommand(_fs.ApplyFont, param => true);
            TestFontCommand = new RelayCommand(OpenFontPreview);
            ExitCommand = new RelayCommand(Exit);
        }

        private void OpenFontPreview(object ignored) {
            _isShowingPreview = !_isShowingPreview;
            NotifyPropertyChanged("MainUIVisibility");
            NotifyPropertyChanged("TestFontUIVisibility");
            NotifyPropertyChanged("TestFontButtonText");
        }

        private void Exit(object ignored) {
            Environment.Exit(0);
        }

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
    }
}
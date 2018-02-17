using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace System.Runtime.CompilerServices
{
    internal sealed class CallerMemberNameAttribute : Attribute
    {
    }
}

namespace WinFontSwitcher
{
    public class FontSwitcherViewModel : INotifyPropertyChanged
    {
        private readonly FontSwitcherModel _fs;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand ApplyCommand { get; set; }

        public ICommand TestFontCommand { get; set; }

        public FontSwitcherViewModel()
        {
            _fs = new FontSwitcherModel();
            ApplyCommand = new RelayCommand(_fs.ApplyFont, param => true);
            TestFontCommand = new RelayCommand(OpenFontPreview);
        }

        private void OpenFontPreview(object ignored)
        {
            MessageBox.Show("Ciao", "Test");
        }

        public IList<KeyValuePair<string, string>> RegistrySystemFonts => _fs.RegistrySystemFonts;

        public IList<MutableKeyVal<string, bool>> FontsToReplace => _fs.FontsToReplace;

        public KeyValuePair<string, string> SelectedPrimaryFont
        {
            get => _fs.SelectedPrimaryFont;
            set
            {
                _fs.SelectedPrimaryFont = value;
                NotifyPropertyChanged();
            }
        }

        public KeyValuePair<string, string> SelectedFallbackFont
        {
            get => _fs.SelectedFallbackFont;
            set
            {
                _fs.SelectedFallbackFont = value;
                NotifyPropertyChanged();
            }
        }
    }
}
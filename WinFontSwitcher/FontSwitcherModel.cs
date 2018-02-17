using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;

namespace WinFontSwitcher {
    public class FontSwitcherModel {
        private static readonly IList<KeyValuePair<string, string>> DefaultSegoeFonts =
            new List<KeyValuePair<string, string>> {
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

        public IList<MutableKeyVal<string, bool>> FontsToReplace =
            new List<MutableKeyVal<string, bool>> {
                new MutableKeyVal<string, bool>("Segoe UI (TrueType)", true),
                new MutableKeyVal<string, bool>("Segoe UI Black (TrueType)", true),
                new MutableKeyVal<string, bool>("Segoe UI Black Italic (TrueType)", true),
                new MutableKeyVal<string, bool>("Segoe UI Bold (TrueType)", true),
                new MutableKeyVal<string, bool>("Segoe UI Bold Italic (TrueType)", true),
                new MutableKeyVal<string, bool>("Segoe UI Emoji (TrueType)", true),
                new MutableKeyVal<string, bool>("Segoe UI Historic (TrueType)", true),
                new MutableKeyVal<string, bool>("Segoe UI Italic (TrueType)", true),
                new MutableKeyVal<string, bool>("Segoe UI Light (TrueType)", true),
                new MutableKeyVal<string, bool>("Segoe UI Light Italic (TrueType)", true),
                new MutableKeyVal<string, bool>("Segoe UI Semibold (TrueType)", true),
                new MutableKeyVal<string, bool>("Segoe UI Semibold Italic (TrueType)", true),
                new MutableKeyVal<string, bool>("Segoe UI Semilight (TrueType)", true),
                new MutableKeyVal<string, bool>("Segoe UI Semilight Italic (TrueType)", true),
                new MutableKeyVal<string, bool>("Segoe UI Symbol (TrueType)", true),
                new MutableKeyVal<string, bool>("Segoe MDL2 Assets (TrueType)", true),
                new MutableKeyVal<string, bool>("Segoe Print (TrueType)", true),
                new MutableKeyVal<string, bool>("Segoe Print Bold (TrueType)", true),
                new MutableKeyVal<string, bool>("Segoe Script (TrueType)", true),
                new MutableKeyVal<string, bool>("Segoe Script Bold (TrueType)", true)
            };


        public FontSwitcherModel() {
            RegistrySystemFonts = GetSystemFonts();
            InitializeRegistry();
            var originalSegoe = DefaultSegoeFonts.First();
            SelectedPrimaryFont = originalSegoe;
            SelectedFallbackFont = new KeyValuePair<string, string>($"{originalSegoe.Key} Backup", originalSegoe.Value);
        }

        // This method "backups" the original Segoe fonts so that they can still be used
        private void InitializeRegistry() {
            using (var key =
                Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts", true)
            ) {
                if (key == null)
                    throw new Exception(Properties.Resources.NoRegistryAccessException);
                foreach (var segoeFont in DefaultSegoeFonts)
                    key.SetValue($"{segoeFont.Key} Backup", segoeFont.Value, RegistryValueKind.String);
            }
        }

        private static IList<KeyValuePair<string, string>> GetSystemFonts() {
            var fontsList = new List<KeyValuePair<string, string>>();
            try {
                using (var key =
                    Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts")
                ) {
                    if (key == null)
                        throw new Exception(Properties.Resources.NoRegistryAccessException);
                    var fonts = key.GetValueNames();
                    fontsList.AddRange(fonts.Select(font =>
                        new KeyValuePair<string, string>(font, key.GetValue(font).ToString())));
                }
            }
            catch (Exception) {
                // ignored
            }

            return fontsList;
        }

        public void ApplyFont() {
            using (var key =
                Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts", true)
            ) {
                if (key == null)
                    throw new Exception(Properties.Resources.NoRegistryAccessException);
                foreach (var fontToReplace in FontsToReplace)
                    if (fontToReplace.Value)
                        key.SetValue(fontToReplace.Key, SelectedPrimaryFont.Value, RegistryValueKind.String);
            }

            using (var key =
                Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\FontSubstitutes", true)
            ) {
                if (key == null)
                    throw new Exception(Properties.Resources.NoRegistryAccessException);
                key.SetValue(SelectedPrimaryFont.Key, SelectedFallbackFont.Key, RegistryValueKind.String);
            }
        }

        public IList<KeyValuePair<string, string>> RegistrySystemFonts;

        public KeyValuePair<string, string> SelectedPrimaryFont;
        public KeyValuePair<string, string> SelectedFallbackFont;
    }
}
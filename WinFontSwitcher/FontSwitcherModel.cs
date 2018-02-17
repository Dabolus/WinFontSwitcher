using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;

namespace WinFontSwitcher
{
    public class FontSwitcherModel
    {
        private static readonly IList<KeyValuePair<string, string>> DefaultSegoeUIFonts =
            new List<KeyValuePair<string, string>>
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

        public IList<MutableKeyVal<string, bool>> FontsToReplace =
            new List<MutableKeyVal<string, bool>>
            {
                new MutableKeyVal<string, bool>("Segoe UI (TrueType)", false),
                new MutableKeyVal<string, bool>("Segoe UI Black (TrueType)", false),
                new MutableKeyVal<string, bool>("Segoe UI Black Italic (TrueType)", false),
                new MutableKeyVal<string, bool>("Segoe UI Bold (TrueType)", false),
                new MutableKeyVal<string, bool>("Segoe UI Bold Italic (TrueType)", false),
                new MutableKeyVal<string, bool>("Segoe UI Emoji (TrueType)", false),
                new MutableKeyVal<string, bool>("Segoe UI Historic (TrueType)", false),
                new MutableKeyVal<string, bool>("Segoe UI Italic (TrueType)", false),
                new MutableKeyVal<string, bool>("Segoe UI Light (TrueType)", false),
                new MutableKeyVal<string, bool>("Segoe UI Light Italic (TrueType)", false),
                new MutableKeyVal<string, bool>("Segoe UI Semibold (TrueType)", false),
                new MutableKeyVal<string, bool>("Segoe UI Semibold Italic (TrueType)", false),
                new MutableKeyVal<string, bool>("Segoe UI Semilight (TrueType)", false),
                new MutableKeyVal<string, bool>("Segoe UI Semilight Italic (TrueType)", false),
                new MutableKeyVal<string, bool>("Segoe UI Symbol (TrueType)", false),
                new MutableKeyVal<string, bool>("Segoe MDL2 Assets (TrueType)", false),
                new MutableKeyVal<string, bool>("Segoe Print (TrueType)", false),
                new MutableKeyVal<string, bool>("Segoe Print Bold (TrueType)", false),
                new MutableKeyVal<string, bool>("Segoe Script (TrueType)", false),
                new MutableKeyVal<string, bool>("Segoe Script Bold (TrueType)", false)
            };


        public FontSwitcherModel()
        {
            RegistrySystemFonts = GetSystemFonts();
            SelectedPrimaryFont = DefaultSegoeUIFonts.First();
        }

        private static IList<KeyValuePair<string, string>> GetSystemFonts()
        {
            var fontsList = new List<KeyValuePair<string, string>>();
            try
            {
                using (var key =
                    Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts")
                )
                {
                    var fonts = key?.GetValueNames() ?? new string[0];
                    fontsList.AddRange(fonts.Select(font =>
                        new KeyValuePair<string, string>(font, key.GetValue(font).ToString())));
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return fontsList;
        }

        public void ApplyFont(object font)
        {
        }

        public IList<KeyValuePair<string, string>> RegistrySystemFonts;

        public KeyValuePair<string, string> SelectedPrimaryFont;
        public KeyValuePair<string, string> SelectedFallbackFont;
    }
}
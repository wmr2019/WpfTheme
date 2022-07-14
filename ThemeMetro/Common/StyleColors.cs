using System.Windows;
using System.Windows.Media;

namespace ThemeMetro.Common
{
    public static class StyleColors
    {
        public static SolidColorBrush GetWindowActiveBorderBrush() => Application.Current.FindResource("Blue0006") as SolidColorBrush;

        public static SolidColorBrush GetWindowInactiveBorderBrush() => Application.Current.FindResource("Black0009") as SolidColorBrush;
    }
}

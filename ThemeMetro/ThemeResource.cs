using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using ThemeCore.Models;

namespace ThemeMetro
{
    public class ThemeResource
    {
        private static readonly object _lockObj = new object();

        public static ThemeResource Current
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (_lockObj)
                {
                    if (_instance != null)
                        return _instance;

                    _instance = new ThemeResource();
                    return _instance;
                }
            }
        }
        private static ThemeResource _instance;

        private ThemeResource() { }

        public IList<Theme> GetThemes()
        {
            var themes = new List<Theme>()
            {
                new Theme
                {
                    Id = "ThemeMetroDark",
                    Name = "ThemeMetro",
                    Code = "Dark",
                    Text = "黑色",
                    Color = new SolidColorBrush(Colors.Black), // 此颜色对应 DarkBrush 中的 SystemColor
                    Resources = new ResourceDictionary() { Source = new Uri("/ThemeMetro;component/Themes/ThemeMetroDark.xaml", UriKind.Relative) }
                },
                new Theme
                {
                    Id = "ThemeMetroLight",
                    Name = "ThemeMetro",
                    Code = "Light",
                    Text = "白色",
                    Color = new SolidColorBrush(Colors.White), // 此颜色对应 LightBrush 中的 SystemColor
                    Resources = new ResourceDictionary() { Source = new Uri("/ThemeMetro;component/Themes/ThemeMetroLight.xaml", UriKind.Relative) }
                }
            };

            return themes;
        }
    }
}

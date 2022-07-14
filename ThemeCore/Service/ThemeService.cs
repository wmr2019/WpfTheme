using System.Linq;
using System.Windows;
using ThemeCore.Models;

namespace ThemeCore.ThemeService
{
    public class ThemeService
    {
        private static readonly object _lockObj = new object();

        public static ThemeService Current
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (_lockObj)
                {
                    if (_instance != null)
                        return _instance;

                    _instance = new ThemeService();
                    return _instance;
                }
            }
        }
        private static ThemeService _instance;

        private ThemeService() { }

        public void ChangeTheme(Application app, Theme theme)
        {
            if (app == null) { return; }
            if (theme == null) { return; }

            var query = $"/{theme.Name};";
            var obj = app.Resources.MergedDictionaries.FirstOrDefault(i => i.Source.OriginalString.StartsWith(query));
            if (obj != null && obj.Source.OriginalString != theme.Resources.Source.OriginalString)
            {
                app.Resources.BeginInit();
                app.Resources.MergedDictionaries.Remove(obj);
                app.Resources.MergedDictionaries.Add(theme.Resources);
                app.Resources.EndInit();
            }

            if (obj == null)
            {
                app.Resources.BeginInit();
                app.Resources.MergedDictionaries.Add(theme.Resources);
                app.Resources.EndInit();
            }
        }
    }
}

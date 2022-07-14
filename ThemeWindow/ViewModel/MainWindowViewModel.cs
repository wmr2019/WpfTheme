using System.Linq;
using System.Windows;
using ThemeCore.Models;
using ThemeCore.ThemeService;
using ThemeMetro;
using Unity;
using WTLib.Collections.ObjectModel;
using WTLib.Mvvm;

namespace ThemeWindow.ViewModel
{
    public class MainWindowViewModel : ObservableObject
    {
        public ObservableRangeCollection<Theme> Themes { get; } = new ObservableRangeCollection<Theme>();

        public Theme SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                if (SetProperty(ref _selectedTheme, value) && value != null)
                {
                    ThemeService.Current.ChangeTheme(Application.Current, SelectedTheme);
                }
            }
        }
        private Theme _selectedTheme = null;

        public MainWindowViewModel(IUnityContainer container)
        {
            Initialize();
        }

        private void Initialize()
        {
            var themes = ThemeResource.Current.GetThemes();
            Themes.ReplaceRange(themes);
            if (themes.Any())
            {
                SelectedTheme = themes.First();
            }
        }
    }
}

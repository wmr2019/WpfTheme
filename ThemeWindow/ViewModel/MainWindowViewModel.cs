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
        public ObservableRangeCollection<Theme> TestThemes { get; } = new ObservableRangeCollection<Theme>()
        {
            new Theme { Code ="Dark1" },
            new Theme { Code ="Dark2" },
            new Theme { Code ="Dark3" },
            new Theme { Code ="Dark4" },
            new Theme { Code ="Dark5" },
            new Theme { Code ="Dark6" },
            new Theme { Code ="Dark7" },
            new Theme { Code ="Dark8" },
            new Theme { Code ="Dark9" },
            new Theme { Code ="Dark10" },
            new Theme { Code ="Dark11" },
            new Theme { Code ="Dark12" },
            new Theme { Code ="Dark13" },
            new Theme { Code ="Dark14" },
            new Theme { Code ="Dark15" },
            new Theme { Code ="Dark16" },
            new Theme { Code ="Dark17" },
            new Theme { Code ="Dark18" },
            new Theme { Code ="Dark19" },
            new Theme { Code ="Dark20" },
            new Theme { Code ="Dark21" },
            new Theme { Code ="Dark22" },
            new Theme { Code ="Dark23" },
            new Theme { Code ="Dark24" },
            new Theme { Code ="Dark25" },
            new Theme { Code ="Dark26" },
            new Theme { Code ="Dark27" },
            new Theme { Code ="Dark28" },
            new Theme { Code ="Dark29" },
            new Theme { Code ="Dark30" },
        };

        public Theme SelectedTestTheme
        {
            get => _selectedTheme;
            set => SetProperty(ref _selectedTheme, value);
        }
        private Theme _selectedTestTheme = null;

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

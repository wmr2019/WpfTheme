using System.Windows;
using ThemeWindow.ViewModel;
using Unity;

namespace ThemeWindow
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var container = new UnityContainer();

            var mainWindowVM = container.Resolve<MainWindowViewModel>();
            var mainWindow = container.Resolve<MainWindow>();
            mainWindow.DataContext = mainWindowVM;
            Current.MainWindow = mainWindow;

            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }
    }
}

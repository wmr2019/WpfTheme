using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ThemeWindow.Models;

namespace ThemeWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public List<Account> Accounts { get; } = new List<Account>
        {
            new Account { Id = "1001", Code = "Leo" },
            new Account { Id = "1002", Code = "Ulrica" },
            new Account { Id = "1003", Code = "Lilith" },
            new Account { Id = "1004", Code = "Aaron" },
        };
    }
}

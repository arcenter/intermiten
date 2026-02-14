using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace intermiten.Pages
{
    public partial class HomePage : Page
    {
        private MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

        public HomePage()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                LoadTimeTableData();
                Load_IconButton_MouseDown();
            };
        }
    }
}
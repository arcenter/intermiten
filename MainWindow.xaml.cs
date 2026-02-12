using intermiten.Core;
using intermiten.Pages;
using System.Reflection;
using System.Windows;

namespace intermiten
{
    public partial class MainWindow : Window
    {
        public HomePage homePage;

        public MainWindow()
        {
            InitializeComponent();
            SourceInitialized += Resize_SourceInitialized;

            Loaded += (s, e) =>
            {
                var attr = Assembly.GetExecutingAssembly().GetCustomAttribute<BuildVersionAttribute>();
                versionLabel.Content = $"version {attr?.Version}" ?? "unknown";

                homePage = new HomePage();
                PageTransition.DoPageTransition(homePage);
            };

            Closed += (s, e) =>
            {
                homePage.FileSave();
            };
        }
    }
}
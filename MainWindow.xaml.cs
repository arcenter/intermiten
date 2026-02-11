using intermiten.Core;
using intermiten.Pages;
using System.IO;
using System.Reflection;
using System.Text.Json;
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
                File.WriteAllText($"{AppContext.BaseDirectory}\\data.json", JsonSerializer.Serialize(homePage.checked_courses));
            };
        }
    }
}
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace intermiten.UserControls
{
    public partial class CourseListItem : UserControl
    {
        readonly MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

        public bool IsChecked = false;

        public CourseListItem(string courseName)
        {
            InitializeComponent();
            courseNameTextbox.Text = courseName;
        }

        private void Background_MouseEnter(object sender, MouseEventArgs e)
        {
            Background.BeginAnimation(OpacityProperty, new DoubleAnimation(0.2, TimeSpan.FromMilliseconds(250)));
        }

        public void Background_MouseLeave(object sender, MouseEventArgs e)
        {
            Background.BeginAnimation(OpacityProperty, new DoubleAnimation(0, TimeSpan.FromMilliseconds(250)));
        }

        private void Background_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                ToggleCheckboxStatus();
        }

        public void ToggleCheckboxStatus()
        {
            ToggleCheckboxStatus(!IsChecked);
        }
        
        public async void ToggleCheckboxStatus(bool toggle)
        {
            IsChecked = toggle;
            checkMark.BeginAnimation(OpacityProperty, new DoubleAnimation(IsChecked ? 1 : 0, TimeSpan.FromMilliseconds(250)));
            if (IsChecked)
            {
                mainWindow.homePage.checked_courses.Add(courseNameTextbox.Text);
                mainWindow.homePage.ReloadTimeTable();
            }
            else
            {
                mainWindow.homePage.checked_courses.Remove(courseNameTextbox.Text);
                mainWindow.homePage.checked_courses.Add(courseNameTextbox.Text);
                mainWindow.homePage.RemoveFromTimeTable();
                mainWindow.homePage.checked_courses.Remove(courseNameTextbox.Text);
            }
        }
    }
}
using System.Windows;
using System.Windows.Controls;

namespace intermiten.UserControls
{
    public partial class TimetableBox : UserControl
    {
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

        public TimetableBox(List<string> courses)
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                foreach (string course in courses)
                {
                    if (!mainWindow.homePage.course_to_CourseListItem.ContainsKey(course)) continue;
                    if (mainWindow.homePage.course_to_CourseListItem[course].IsChecked)
                        stackPanel.Children.Add(new CourseBox(course));
                }
            };
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            try
            {
                if (System.Windows.Forms.SystemInformation.MouseWheelScrollLines == -1)
                    e.Handled = false;
                else
                {
                    ScrollViewer SenderScrollViewer = (ScrollViewer)sender;
                    SenderScrollViewer.ScrollToVerticalOffset(SenderScrollViewer.VerticalOffset - e.Delta);
                    e.Handled = true;
                }
            }
            catch { }
        }
    }
}
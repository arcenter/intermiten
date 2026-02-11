using Fernandezja.ColorHashSharp;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace intermiten.UserControls
{
    public partial class CourseBox : UserControl
    {
        readonly MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        public string courseName;

        public CourseBox(string course)
        {
            InitializeComponent();
            
            this.courseName = course;

            Loaded += (s, e) =>
            {
                string[] course_attributes = course.Split(" - ");

                string courseName = "";
                foreach (string word in course_attributes[1].Split(' '))
                    if (word.Length > 4) courseName += $"{word[..4]}. ";
                    else courseName += $"{word} ";

                courseNameTextBox.Text = courseName;
                courseIDTextbox.Text = $"{course_attributes[0]}";
                sectionRoomTextBox.Text = $"{course_attributes[2]} | {course_attributes[3]}";

                var drawingColor = new ColorHash().Rgb(course);
                Background.Background = new SolidColorBrush(Color.FromArgb(drawingColor.A, drawingColor.R, drawingColor.G, drawingColor.B));

                if (mainWindow.homePage.checked_courses.Last() == course)
                    BeginAnimation(OpacityProperty, new DoubleAnimation(1, TimeSpan.FromMilliseconds(500)) { EasingFunction = new QuarticEase() { EasingMode = EasingMode.EaseInOut } });
                else Opacity = 1;
            };
        }

        private void Background_MouseEnter(object sender, MouseEventArgs e)
        {
            Background.BeginAnimation(OpacityProperty, new DoubleAnimation(1, TimeSpan.FromMilliseconds(250)));
        }

        public void Background_MouseLeave(object sender, MouseEventArgs e)
        {
            Background.BeginAnimation(OpacityProperty, new DoubleAnimation(0.75, TimeSpan.FromMilliseconds(250)));
        }
    }
}
using intermiten.UserControls;
using System.IO;
using System.Text.Json;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows;

namespace intermiten.Pages
{
    public partial class HomePage : Page
    {
        Dictionary<string, Dictionary<string, List<string>>> timetable;
        public List<string> checked_courses = [];

        private void LoadTimeTableData()
        {
            timetable = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, List<string>>>>(File.ReadAllText("C:/repos/intermiten/Resources/Data/timetable.json"));

            LoadSidebarItems();

            string path = $"{AppContext.BaseDirectory}\\data.json";
            if (File.Exists(path))
            {
                List<string> checked_courses = JsonSerializer.Deserialize<List<string>>(File.ReadAllText(path));
                checked_courses = [.. checked_courses.Distinct()];
                foreach (string course in checked_courses)
                    course_to_CourseListItem[course].ToggleCheckboxStatus();
            }

            UpdateList();
            LoadBaseTimeTable();
            ReloadTimeTable();
        }

        public void LoadBaseTimeTable()
        {
            int day_no = 0;
            int hour_no = 1;

            DateTime hourDT = DateTime.Now.Date + TimeSpan.FromHours(7) + TimeSpan.FromMinutes(30);

            foreach (string hour in timetable[timetable.Keys.ElementAt(0)].Keys)
            {
                Label hour_number_label = new()
                {
                    Content = hour,
                    Foreground = (SolidColorBrush)mainWindow.FindResource("light-a0"),
                    HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center,
                    VerticalContentAlignment = System.Windows.VerticalAlignment.Center,
                    FontFamily = new(new Uri("pack://intermiten:,,,/Resources/Fonts/Manrope.ttf"), "Manrope Regular"),
                    Padding = new Thickness(0),
                    Margin = new Thickness(0)
                };

                Label hours_label = new()
                {
                    Content = $"{hourDT.ToString("hh:mm")} - {(hourDT + TimeSpan.FromMinutes(50)).ToString("hh:mm")}",
                    Foreground = (SolidColorBrush)mainWindow.FindResource("light-a0"),
                    HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center,
                    VerticalContentAlignment = System.Windows.VerticalAlignment.Center,
                    FontFamily = new(new Uri("pack://intermiten:,,,/Resources/Fonts/Manrope.ttf"), "Manrope Regular"),
                    FontSize = 9,
                    Opacity = 0.75,
                    Padding = new Thickness(0),
                    Margin = new Thickness(0)
                };

                hourDT += TimeSpan.FromMinutes(55);

                StackPanel hour_stackpanel = new()
                {
                    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Center
                };
                hour_stackpanel.Children.Add(hour_number_label);
                hour_stackpanel.Children.Add(hours_label);

                Grid.SetColumn(hour_stackpanel, hour_no);
                Grid.SetRow(hour_stackpanel, day_no);

                timetableGrid.Children.Add(hour_stackpanel);


                Line line = new()
                {
                    Stroke = (SolidColorBrush)mainWindow.FindResource("light-a0"),
                    Opacity = 0.1,
                    StrokeThickness = 1
                };
                line.SetBinding(Line.Y2Property, new Binding("ActualHeight")
                {
                    Source = timetableGrid,
                    Mode = BindingMode.OneWay
                });
                Grid.SetRowSpan(line, timetableGrid.RowDefinitions.Count);
                Grid.SetColumn(line, hour_no);
                timetableGrid.Children.Add(line);

                hour_no++;
            }

            day_no = 1;
            hour_no = 0;
            foreach (string day in timetable.Keys)
            {
                Label label = new()
                {
                    Content = day[..3],
                    Foreground = (SolidColorBrush)mainWindow.FindResource("light-a0"),
                    HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center,
                    VerticalContentAlignment = System.Windows.VerticalAlignment.Center,
                    FontFamily = new(new Uri("pack://intermiten:,,,/Resources/Fonts/Manrope.ttf"), "Manrope Regular")
                };

                Grid.SetColumn(label, hour_no);
                Grid.SetRow(label, day_no);

                timetableGrid.Children.Add(label);


                Line line = new()
                {
                    Stroke = (SolidColorBrush)mainWindow.FindResource("light-a0"),
                    Opacity = 0.1,
                    StrokeThickness = 1
                };
                line.SetBinding(Line.X2Property, new Binding("ActualWidth")
                {
                    Source = timetableGrid,
                    Mode = BindingMode.OneWay
                });
                Grid.SetColumnSpan(line, timetableGrid.ColumnDefinitions.Count);
                Grid.SetRow(line, day_no);
                timetableGrid.Children.Add(line);

                day_no++;
            }
        }

        public void ReloadTimeTable()
        {
            for (int index = timetableGrid.Children.Count-1;  index >= 0; index--)
                if (timetableGrid.Children[index] is TimetableBox ttb)
                    timetableGrid.Children.Remove(ttb);

            int day_no = 1;
            int hour_no;
            
            foreach (string day in timetable.Keys)
            {
                hour_no = 1;
                foreach (string hour in timetable[day].Keys)
                {
                    TimetableBox ttb = new(timetable[day][hour]);

                    Grid.SetColumn(ttb, hour_no);
                    Grid.SetRow(ttb, day_no);

                    timetableGrid.Children.Add(ttb);

                    hour_no++;
                }
                day_no++;
            }
        }

        public async Task RemoveFromTimeTable()
        {
            List<CourseBox> list = [];
            foreach (object obj in timetableGrid.Children)
                if (obj is TimetableBox ttb)
                    foreach (object obj2 in ttb.stackPanel.Children)
                        if (obj2 is CourseBox courseBox && courseBox.courseName == mainWindow.homePage.checked_courses.Last())
                        {
                            courseBox.BeginAnimation(OpacityProperty, new DoubleAnimation(0, TimeSpan.FromMilliseconds(500)) { EasingFunction = new QuarticEase() { EasingMode = EasingMode.EaseInOut } });
                            list.Add(courseBox);
                        }
            await Task.Delay(501);
            foreach (CourseBox courseBox in list)
                courseBox.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
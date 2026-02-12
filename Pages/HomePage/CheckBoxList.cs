using intermiten.UserControls;
using System.Windows.Controls;

namespace intermiten.Pages
{
    public partial class HomePage : Page
    {
        public List<CourseListItem> courses;
        public Dictionary<string, CourseListItem> course_to_CourseListItem;

        private void LoadSidebarItems()
        {
            // A `course` is every unique course+id+section+room string

            courses = new();
            course_to_CourseListItem = new();
            List<string> _courses = [];

            foreach (string day in timetable.Keys)
                foreach (string hour in timetable[day].Keys)
                    foreach (string item in timetable[day][hour])
                        _courses.Add(item);

            _courses = [.. _courses.Distinct()];

            foreach (string course in _courses)
            {
                CourseListItem cli = new(course);
                courses.Add(cli);
                course_to_CourseListItem[course] = cli;
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateList();
        }

        private void UpdateList()
        {
            sidebarCourseSectionList.Children.Clear();
            
            int index = 0;
            foreach (CourseListItem cli in courses)
                if (cli.courseNameTextbox.Text.Contains(searchBox.Text, StringComparison.CurrentCultureIgnoreCase))
                    if (cli.IsChecked)
                        sidebarCourseSectionList.Children.Insert(index++, cli);
                    else if (searchBox.Text.Length >= 3)
                        sidebarCourseSectionList.Children.Add(cli);
        }
    }
}

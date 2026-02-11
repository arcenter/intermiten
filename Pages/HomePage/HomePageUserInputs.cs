using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace intermiten.Pages
{
    public partial class HomePage : Page
    {
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            mainWindow.KeyUp += Page_KeyUp;
            mainWindow.KeyDown += Page_KeyDown;
            mainWindow.MouseDown += Window_MouseDown;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            mainWindow.KeyUp -= Page_KeyUp;
            mainWindow.KeyDown -= Page_KeyDown;
            mainWindow.MouseDown -= Window_MouseDown;
        }

        // Keyboard -------------------------------------------------------------------------------------------------------

        private void Page_KeyDown(object sender, KeyEventArgs e) { }

        private void Page_KeyUp(object sender, KeyEventArgs e) { }

        // Mouse ----------------------------------------------------------------------------------------------------------

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) { }

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
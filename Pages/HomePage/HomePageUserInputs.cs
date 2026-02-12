using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                if (e.Key == Key.E)
                {
                    ExportImage();
                }
                else if (e.Key == Key.S)
                {
                    if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                        FileSaveAs();
                    else
                        FileSave();
                }
                else if (e.Key == Key.O)
                {
                    FileOpen();
                }
            }
        }


        private void Page_KeyUp(object sender, KeyEventArgs e) { }

        // Mouse ----------------------------------------------------------------------------------------------------------

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) { }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
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
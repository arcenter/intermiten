using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace intermiten
{
    public partial class MainWindow : Window
    {
        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        // Buttons --------------------------------------------------------------------------------------------------------

        private void MinimizeButton_IsMouseDirectlyOverChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            MinimizeButton.Background.BeginAnimation(Brush.OpacityProperty, new DoubleAnimation(((bool)e.NewValue) ? 1 : 0, TimeSpan.FromMilliseconds(250)));
            MinimizeX1.BeginAnimation(OpacityProperty, new DoubleAnimation(((bool)e.NewValue) ? 1 : 0.25, TimeSpan.FromMilliseconds(250)));
        }

        private void MinimizeButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_IsMouseDirectlyOverChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            MaximizeButton.Background.BeginAnimation(Brush.OpacityProperty, new DoubleAnimation(((bool)e.NewValue) ? 1 : 0, TimeSpan.FromMilliseconds(250)));
            MaximizeRect.BeginAnimation(OpacityProperty, new DoubleAnimation(((bool)e.NewValue) ? 1 : 0.25, TimeSpan.FromMilliseconds(250)));
        }

        private void MaximizeButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                WindowBorder.BorderThickness = new Thickness(1);
                ResizeRectangles.IsHitTestVisible = true;
            }
            else
            {
                WindowState = WindowState.Maximized;
                WindowBorder.BorderThickness = new Thickness(0);
                ResizeRectangles.IsHitTestVisible = false;
            }

        }

        private void CloseButton_IsMouseDirectlyOverChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            CloseButton.Background.BeginAnimation(Brush.OpacityProperty, new DoubleAnimation(((bool)e.NewValue) ? 1 : 0, TimeSpan.FromMilliseconds(250)));
            X1.BeginAnimation(OpacityProperty, new DoubleAnimation(((bool)e.NewValue) ? 1 : 0.25, TimeSpan.FromMilliseconds(250)));
            X2.BeginAnimation(OpacityProperty, new DoubleAnimation(((bool)e.NewValue) ? 1 : 0.25, TimeSpan.FromMilliseconds(250)));
        }

        private void CloseButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}
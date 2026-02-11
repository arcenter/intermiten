using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;

namespace intermiten
{
    public partial class MainWindow : Window
    {
        private const int WM_SYSCOMMAND = 0x112;
        private HwndSource hwndSource;

        private enum ResizeDirection
        {
            Left = 61441,
            Right = 61442,
            Top = 61443,
            TopLeft = 61444,
            TopRight = 61445,
            Bottom = 61446,
            BottomLeft = 61447,
            BottomRight = 61448,
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private void Resize_SourceInitialized(object sender, EventArgs e)
        {
            hwndSource = PresentationSource.FromVisual((Visual)sender) as HwndSource;
        }

        private void ResizeWindow(ResizeDirection direction)
        {
            SendMessage(hwndSource.Handle, WM_SYSCOMMAND, (IntPtr)direction, IntPtr.Zero);
        }

        protected void ResetCursor(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
            {
                this.Cursor = Cursors.Arrow;
                FrameView.IsHitTestVisible = true;
            }
        }

        protected void Resize(object sender, MouseButtonEventArgs e)
        {
            if (WindowState == WindowState.Maximized) return;

            FrameView.IsHitTestVisible = false;
            var clickedShape = sender as Shape;

            switch (clickedShape.Name)
            {
                case "ResizeN":
                    this.Cursor = Cursors.SizeNS;
                    ResizeWindow(ResizeDirection.Top);
                    break;
                case "ResizeE":
                    this.Cursor = Cursors.SizeWE;
                    ResizeWindow(ResizeDirection.Right);
                    break;
                case "ResizeS":
                    this.Cursor = Cursors.SizeNS;
                    ResizeWindow(ResizeDirection.Bottom);
                    break;
                case "ResizeW":
                    this.Cursor = Cursors.SizeWE;
                    ResizeWindow(ResizeDirection.Left);
                    break;
                case "ResizeNW":
                    this.Cursor = Cursors.SizeNWSE;
                    ResizeWindow(ResizeDirection.TopLeft);
                    break;
                case "ResizeNE":
                    this.Cursor = Cursors.SizeNESW;
                    ResizeWindow(ResizeDirection.TopRight);
                    break;
                case "ResizeSE":
                    this.Cursor = Cursors.SizeNWSE;
                    ResizeWindow(ResizeDirection.BottomRight);
                    break;
                case "ResizeSW":
                    this.Cursor = Cursors.SizeNESW;
                    ResizeWindow(ResizeDirection.BottomLeft);
                    break;
                default:
                    break;
            }
        }

        protected void DisplayResizeCursor(object sender, MouseEventArgs e)
        {
            if (WindowState == WindowState.Maximized) return;
            Cursor = (sender as Shape).Name switch
            {
                "ResizeN" or "ResizeS" => Cursors.SizeNS,
                "ResizeE" or "ResizeW" => Cursors.SizeWE,
                "ResizeNW" or "ResizeSE" => Cursors.SizeNWSE,
                "ResizeNE" or "ResizeSW" => Cursors.SizeNESW,
                _ => Cursors.Arrow
            };
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (FrameView.IsHitTestVisible) return;
            FrameView.IsHitTestVisible = true;
            ResetCursor(null, null);
        }
    }
}
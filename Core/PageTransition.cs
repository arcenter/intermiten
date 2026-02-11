using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
    
namespace intermiten.Core
{
    public static class PageTransition
    {
        private static MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        
        public async static void DoPageTransition(Page newPage)
        {
            mainWindow.FrameView.BeginAnimation(Frame.OpacityProperty, new DoubleAnimation(0, TimeSpan.FromMilliseconds(250)));
            await Task.Delay(250);
            mainWindow.FrameView.Navigate(newPage);
            await Task.Delay(150);
            mainWindow.FrameView.BeginAnimation(Frame.OpacityProperty, new DoubleAnimation(1, TimeSpan.FromMilliseconds(250)));
        }

        public async static void GoBack()
        {
            mainWindow.FrameView.BeginAnimation(Frame.OpacityProperty, new DoubleAnimation(0, TimeSpan.FromMilliseconds(250)));
            await Task.Delay(250);
            mainWindow.FrameView.GoBack();
            await Task.Delay(150);
            mainWindow.FrameView.BeginAnimation(Frame.OpacityProperty, new DoubleAnimation(1, TimeSpan.FromMilliseconds(250)));
        }
    }
}
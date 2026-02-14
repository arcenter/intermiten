using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace intermiten.UserControls
{
    public partial class IconButton : UserControl
    {
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(DrawingImage), typeof(IconButton), new PropertyMetadata(null));
        public DrawingImage Source
        {
            get { return (DrawingImage)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty ImagePaddingProperty = DependencyProperty.Register("ImagePadding", typeof(int), typeof(IconButton), new PropertyMetadata(4));
        public int ImagePadding
        {
            get { return (int)GetValue(ImagePaddingProperty); }
            set
            {
                SetValue(ImagePaddingProperty, value);
                ((Border)Content).Padding = new Thickness(value);
            }
        }

        public IconButton()
        {
            InitializeComponent();
        }

        private void IconButton_IsMouseDirectlyOverChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var border = (Border)sender;

            (border).Background.BeginAnimation(Brush.OpacityProperty, new DoubleAnimation(((bool)e.NewValue) ? 1 : 0, TimeSpan.FromMilliseconds(250)));

            if (border.Child is Image img && img.Source is DrawingImage drawingImg)
                img.Source = new DrawingImage(RecolorDrawing(
                    drawingImg.Drawing,
                    (SolidColorBrush)mainWindow.FindResource(((bool)e.NewValue)
                        ? "primary-a0"
                        : "surface-a50"
                    )
                ));
        }

        private static Drawing RecolorDrawing(Drawing drawing, Brush newColor)
        {
            if (drawing is GeometryDrawing geoDrawing)
            {
                var newBrush = newColor ?? geoDrawing.Brush;
                var newPen   = geoDrawing.Pen?.Clone();

                if (newPen != null)
                {
                    newPen.Brush = newColor;
                    return new GeometryDrawing(null, newPen, geoDrawing.Geometry);
                }

                return new GeometryDrawing(newBrush, null, geoDrawing.Geometry);
            }

            if (drawing is DrawingGroup group)
            {
                var newGroup = new DrawingGroup
                {
                    ClipGeometry = group.ClipGeometry,
                    Opacity = group.Opacity
                };

                foreach (var child in group.Children)
                    newGroup.Children.Add(RecolorDrawing(child, newColor));
                
                return newGroup;
            }

            return drawing;
        }
    }
}

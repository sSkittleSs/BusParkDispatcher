using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BusParkDispatcher.Controls
{
    class IconButton : Button
    {
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Brush), typeof(IconButton), new UIPropertyMetadata(Brushes.Black));
        public static readonly DependencyProperty DarkenedColorProperty = DependencyProperty.Register("DarkenedColor", typeof(Brush), typeof(IconButton), new UIPropertyMetadata(Brushes.Black));
        public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register("BackgroundColor", typeof(Brush), typeof(IconButton), new UIPropertyMetadata(Brushes.White));
        public static readonly DependencyProperty DarkenedBackgroundColorProperty = DependencyProperty.Register("DarkenedBackgroundColor", typeof(Brush), typeof(IconButton), new UIPropertyMetadata(Brushes.White));


        // #FFDA3B00
        static IconButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IconButton), new FrameworkPropertyMetadata(typeof(IconButton)));
        }

        public Brush Color { get; set; }
        public Brush DarkenedColor { get; set; }
        public Brush BackgroundColor { get; set; }
        public Brush DarkenedBackgroundColor { get; set; }
    }
}

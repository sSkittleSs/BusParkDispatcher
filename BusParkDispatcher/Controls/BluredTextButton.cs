using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BusParkDispatcher.Controls
{
    class BluredTextButton : Button
    {
        public static readonly DependencyProperty BluredForegroundProperty = DependencyProperty.Register("BluredForeground", typeof(Brush), typeof(BluredTextButton), new UIPropertyMetadata(Brushes.Blue));
        public static readonly DependencyProperty BlurVisibilityProperty = DependencyProperty.Register("BlurVisibility", typeof(Visibility), typeof(BluredTextButton), new UIPropertyMetadata(Visibility.Hidden));

        static BluredTextButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BluredTextButton), new FrameworkPropertyMetadata(typeof(BluredTextButton)));
        }

        public Brush BluredForeground { get; set; }
        public Visibility BlurVisibility { get; set; }
    }
}

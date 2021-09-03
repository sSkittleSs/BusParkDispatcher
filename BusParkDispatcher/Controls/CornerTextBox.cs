using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BusParkDispatcher.Controls
{
    class CornerTextBox : TextBox
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(CornerTextBox), new UIPropertyMetadata(new CornerRadius(2)));
        public static readonly DependencyProperty WatermarkForegroundProperty = DependencyProperty.Register("WatermarkForeground", typeof(Brush), typeof(CornerTextBox), new UIPropertyMetadata(Brushes.Gray));
        public static readonly DependencyProperty WatermarkTextProperty = DependencyProperty.Register("WatermarkText", typeof(string), typeof(CornerTextBox), new UIPropertyMetadata(""));
        public static readonly DependencyProperty WatermarkVisibilityProperty = DependencyProperty.Register("WatermarkVisibility", typeof(Visibility), typeof(CornerTextBox), new UIPropertyMetadata(Visibility.Collapsed));

        static CornerTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CornerTextBox), new FrameworkPropertyMetadata(typeof(CornerTextBox)));
        }

        public CornerRadius CornerRadius { get; set; }
        public Brush WatermarkForeground { get; set; }
        public string WatermarkText { get; set; }
        public Visibility WatermarkVisibility { get; set; }
    }
}

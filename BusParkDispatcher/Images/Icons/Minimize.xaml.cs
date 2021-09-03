using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BusParkDispatcher.Images.Icons
{
    public partial class Minimize : UserControl
    {
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Brush), typeof(Minimize), new UIPropertyMetadata(Brushes.Black));
        public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register("BackgroundColor", typeof(Brush), typeof(Minimize), new UIPropertyMetadata(Brushes.White));

        public Brush Color { set; get; }
        public Brush BackgroundColor { set; get; }

        public Minimize()
        {
            InitializeComponent();
        }
    }
}

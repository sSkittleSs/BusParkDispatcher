using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BusParkDispatcher.Images.Icons
{
    public partial class Exit : UserControl
    {
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Brush), typeof(Exit), new UIPropertyMetadata(Brushes.Black));
        public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register("BackgroundColor", typeof(Brush), typeof(Exit), new UIPropertyMetadata(Brushes.White));

        public Brush Color { set; get; }
        public Brush BackgroundColor { set; get; }

        public Exit()
        {
            InitializeComponent();
        }
    }
}

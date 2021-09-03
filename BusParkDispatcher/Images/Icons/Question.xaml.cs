using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BusParkDispatcher.Images.Icons
{
    public partial class Question : UserControl
    {
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Brush), typeof(Question), new UIPropertyMetadata(Brushes.Black));
        public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register("BackgroundColor", typeof(Brush), typeof(Question), new UIPropertyMetadata(Brushes.White));

        public Brush Color { set; get; }
        public Brush BackgroundColor { set; get; }

        public Question()
        {
            InitializeComponent();
        }
    }
}

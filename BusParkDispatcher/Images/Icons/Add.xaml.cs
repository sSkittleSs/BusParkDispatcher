using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BusParkDispatcher.Images.Icons
{
    public partial class Add : UserControl
    {
        // #FF77DD00
        // #FF66BB00

        // #FFE7E7E7
        // #FFD3D3D8
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Brush), typeof(Add), new UIPropertyMetadata((SolidColorBrush)new BrushConverter().ConvertFrom("#FF77DD00")));
        public static readonly DependencyProperty DarkenedColorProperty = DependencyProperty.Register("DarkenedColor", typeof(Brush), typeof(Add), new UIPropertyMetadata((SolidColorBrush)new BrushConverter().ConvertFrom("#FF66BB00")));
        public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register("BackgroundColor", typeof(Brush), typeof(Add), new UIPropertyMetadata((SolidColorBrush)new BrushConverter().ConvertFrom("#FFE7E7E7")));
        public static readonly DependencyProperty DarkenedBackgroundColorProperty = DependencyProperty.Register("DarkenedBackgroundColor", typeof(Brush), typeof(Add), new UIPropertyMetadata((SolidColorBrush)new BrushConverter().ConvertFrom("#FFD3D3D8")));

        public Brush Color { set; get; }
        public Brush DarkenedColor { set; get; }
        public Brush BackgroundColor { set; get; }
        public Brush DarkenedBackgroundColor { set; get; }

        public Add()
        {
            InitializeComponent();
        }
    }
}

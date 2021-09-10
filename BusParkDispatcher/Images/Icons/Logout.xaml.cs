﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BusParkDispatcher.Images.Icons
{
    public partial class Logout : UserControl
    {
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Brush), typeof(Logout), new UIPropertyMetadata(Brushes.Black));
        public static readonly DependencyProperty InnerBackgroundColorProperty = DependencyProperty.Register("InnerBackgroundColor", typeof(Brush), typeof(Logout), new UIPropertyMetadata(Brushes.White));
        public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register("BackgroundColor", typeof(Brush), typeof(Logout), new UIPropertyMetadata(Brushes.White));

        public Brush Color { set; get; }
        public Brush InnerBackgroundColor { set; get; }
        public Brush BackgroundColor { set; get; }

        public Logout()
        {
            InitializeComponent();
        }
    }
}

using System.Reflection;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace BusParkDispatcher.Controls
{
    class DatePickerWatermarked : DatePicker
    {
        public string Watermark { get; set; }

        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            SetWatermark();
        }

        private void SetWatermark()
        {
            FieldInfo fiTextBox = typeof(DatePicker).GetField("_textBox", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiTextBox != null)
            {
                DatePickerTextBox dateTextBox = (DatePickerTextBox)fiTextBox.GetValue(this);
                if (dateTextBox != null)
                {
                    if (string.IsNullOrWhiteSpace(this.Watermark))
                    {
                        this.Watermark = "Custom watermark";
                    }

                    var partWatermark = dateTextBox.Template.FindName("PART_Watermark", dateTextBox) as ContentControl;
                    if (partWatermark != null)
                    {
                        partWatermark.Foreground = new SolidColorBrush(Colors.Gray);
                        partWatermark.Content = this.Watermark;
                    }
                }
            }
        }
    }
}

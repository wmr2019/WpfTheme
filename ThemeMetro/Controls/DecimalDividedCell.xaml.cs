using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ThemeMetro.Controls
{
    /// <summary>
    /// Interaction logic for DecimalDividedCell.xaml
    /// </summary>
    public partial class DecimalDividedCell : ContentControl
    {
        public string StringFormat { get; set; }

        public DecimalDividedCell()
        {
            StringFormat = "{0:p2}";
            InitializeComponent();
        }

        public decimal Value
        {
            get => (decimal)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public decimal Value2
        {
            get => (decimal)GetValue(Value2Property);
            set => SetValue(Value2Property, value);
        }

        public string Format
        {
            get => (string)GetValue(FormatProperty);
            set => SetValue(FormatProperty, value);
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(decimal), typeof(DecimalDividedCell),
                new FrameworkPropertyMetadata(0m, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ValueChangedCallback));

        public static readonly DependencyProperty Value2Property =
            DependencyProperty.Register("Value2", typeof(decimal), typeof(DecimalDividedCell),
                new FrameworkPropertyMetadata(0m, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Value2ChangedCallback));

        public static readonly DependencyProperty FormatProperty =
            DependencyProperty.Register("Format", typeof(string), typeof(DecimalDividedCell),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.None, FormatChangedCallback));

        public static void ValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DecimalDividedCell cell)
            {
                if (e.NewValue == e.OldValue) return;
                cell.SetDividedMark((decimal)e.NewValue, cell.Value2);
            }
        }

        public static void Value2ChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DecimalDividedCell cell)
            {
                if (e.NewValue == e.OldValue) return;
                cell.SetDividedMark(cell.Value, (decimal)e.NewValue);
            }
        }

        public static void FormatChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DecimalDividedCell cell)
            {
                if (e.NewValue == e.OldValue) return;
                if (e.NewValue == null) return;
                cell.StringFormat = string.Concat("{0:", e.NewValue, "}");
            }
        }

        public void SetDividedMark(decimal valueA, decimal valueB)
        {
            if (valueB == 0 || valueA == 0)
            {
                ValueText.Text = "0";
                ValueText.Foreground = new SolidColorBrush(Color.FromRgb(212, 202, 199));
            }
            else
            {
                ValueText.Text = string.Format(this.StringFormat, valueA / valueB);

                if (valueA > 0)
                {
                    ValueText.Foreground = new SolidColorBrush(Color.FromRgb(255, 60, 60));
                }
                else
                {
                    ValueText.Foreground = new SolidColorBrush(Color.FromRgb(0, 221, 0));
                }
            }
        }
    }
}

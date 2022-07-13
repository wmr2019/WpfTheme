using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ThemeMetro.Controls
{
    /// <summary>
    /// Interaction logic for DecimalCell.xaml
    /// </summary>
    public partial class DecimalCell : ContentControl
    {
        public string StringFormat { get; set; }

        public DecimalCell()
        {
            StringFormat = "{0:F0}";
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

        public int Place
        {
            get => (int)GetValue(PlaceProperty);
            set => SetValue(PlaceProperty, value);
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(decimal), typeof(DecimalCell),
                new FrameworkPropertyMetadata(0m, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ValueChangedCallback));

        public static readonly DependencyProperty Value2Property =
            DependencyProperty.Register("Value2", typeof(decimal), typeof(DecimalCell),
                new FrameworkPropertyMetadata(0m, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Value2ChangedCallback));

        public static readonly DependencyProperty PlaceProperty =
            DependencyProperty.Register("Place", typeof(int), typeof(DecimalCell),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.None, PlaceChangedCallback));

        public static void ValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DecimalCell cell)
            {
                if (e.NewValue == e.OldValue) return;
                cell.SetMark((decimal)e.NewValue, cell.Value2);
            }
        }

        public static void Value2ChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DecimalCell cell)
            {
                if (e.NewValue == e.OldValue) return;
                cell.SetMark(cell.Value, (decimal)e.NewValue);
            }
        }

        public static void PlaceChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DecimalCell cell)
            {
                if (e.NewValue == e.OldValue) return;
                cell.StringFormat = string.Concat("{0:F", e.NewValue, "}");
                cell.SetMark(cell.Value, cell.Value2);
            }
        }

        public void SetMark(decimal valueA, decimal valueB)
        {
            //if (valueA == 0)
            //{
            //    ValueText.Text = "-";
            //    ValueText.Foreground = new SolidColorBrush(Color.FromRgb(212, 202, 199));
            //    return;
            //}

            ValueText.Text = string.Format(StringFormat, valueA);

            if (valueA > valueB)
            {
                ValueText.Foreground = new SolidColorBrush(Color.FromRgb(255, 60, 60));
            }
            else if (valueA < valueB)
            {
                ValueText.Foreground = new SolidColorBrush(Color.FromRgb(0, 221, 0));
            }
            else
            {
                ValueText.Foreground = new SolidColorBrush(Color.FromRgb(212, 202, 199));
            }
        }
    }
}

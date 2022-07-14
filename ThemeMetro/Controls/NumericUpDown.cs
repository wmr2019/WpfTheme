using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace ThemeMetro.Controls
{
    [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_ButtonUp", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_ButtonDown", Type = typeof(ButtonBase))]
    public class NumericUpDown : Control
    {
        enum SelectionType
        {
            None,
            EndCaret,
            EndSelection,
            SpecifyCaret,
            SpecifySelection,
        }

        private TextBox PART_TextBox = new TextBox();
        private SelectionType _selectionType = SelectionType.None;
        private int _currentCaretIndex = 0;

        public bool IsKeyChanged { get; set; }

        public NumericUpDown() { }

        static NumericUpDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(NumericUpDown),
                new FrameworkPropertyMetadata(typeof(NumericUpDown)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_TextBox") is TextBox textBox)
            {
                PART_TextBox = textBox;
                PART_TextBox.PreviewKeyDown -= TextBox_PreviewKeyDown;
                PART_TextBox.PreviewKeyDown += TextBox_PreviewKeyDown;

                PART_TextBox.TextChanged -= TextBox_TextChanged;
                PART_TextBox.TextChanged += TextBox_TextChanged;

                PART_TextBox.GotFocus -= PART_TextBox_GotFocus;
                PART_TextBox.GotFocus += PART_TextBox_GotFocus;
                PART_TextBox.LostFocus -= PART_TextBox_LostFocus;
                PART_TextBox.LostFocus += PART_TextBox_LostFocus;
                PART_TextBox.LostKeyboardFocus -= PART_TextBox_LostKeyboardFocus;
                PART_TextBox.LostKeyboardFocus += PART_TextBox_LostKeyboardFocus;

                PART_TextBox.Text = GetValueByDecimalDigit(DecimalDigit, Value);
            }
            if (GetTemplateChild("PART_ButtonUp") is ButtonBase PART_ButtonUp)
            {
                PART_ButtonUp.Click -= ButtonUp_Click;
                PART_ButtonUp.Click += ButtonUp_Click;
            }
            if (GetTemplateChild("PART_ButtonDown") is ButtonBase PART_ButtonDown)
            {
                PART_ButtonDown.Click -= ButtonDown_Click;
                PART_ButtonDown.Click += ButtonDown_Click;
            }
            this.GotKeyboardFocus += NumericUpDown_GotKeyboardFocus;
        }

        #region TextBox
        public event Action<string> TextChanged;

        private void PART_TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            PART_TextBox.PreviewMouseDown += PART_TextBox_PreviewMouseDown;
        }

        private void PART_TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SelectAll();
            PART_TextBox.PreviewMouseDown -= PART_TextBox_PreviewMouseDown;
        }

        private void PART_TextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            PART_TextBox.Focus();
            e.Handled = true;
        }

        private void NumericUpDown_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            this.PART_TextBox.Focus();
        }

        private void PART_TextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text.EndsWith("."))
                    textBox.Text = textBox.Text.TrimEnd('.');
                if (textBox.Text == "-")
                    textBox.Text = "0";
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                e.Handled = false;
                return;
            }
            if (!(sender is TextBox textBox))
                return;
            // 减号
            if (e.Key == Key.OemMinus || e.Key == Key.Subtract)
            {
                e.Handled = true;
                if (this.MinValue >= 0)
                    return;

                IsKeyChanged = true;
                _selectionType = SelectionType.EndCaret;
                textBox.Text = "-";
                return;
            }
            //点
            if (e.Key == Key.Decimal || e.Key == Key.OemPeriod)
            {
                e.Handled = true;
                if (this.DecimalDigit == 0)
                    return;
                if (string.IsNullOrEmpty(textBox.Text))
                    return;
                if (textBox.Text.EndsWith("-"))
                    return;
                if (textBox.Text.Contains("."))
                {
                    if (textBox.SelectedText.Contains("."))
                    {
                        IsKeyChanged = true;
                        _selectionType = SelectionType.SpecifyCaret;
                        _currentCaretIndex = textBox.CaretIndex + 1;
                        textBox.Text = textBox.Text.Replace(textBox.SelectedText, ".");
                    }
                    return;
                }
                if (textBox.SelectedText.Length > 0)
                {
                    IsKeyChanged = true;
                    _selectionType = SelectionType.SpecifyCaret;
                    _currentCaretIndex = textBox.CaretIndex + 1;
                    textBox.Text = textBox.Text.Replace(textBox.SelectedText, ".");
                    return;
                }

                _selectionType = SelectionType.EndCaret;
                e.Handled = false;
                return;
            }
            //数字
            if ((e.Key >= Key.D0 && e.Key <= Key.D9)
                || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
            {
                _selectionType = SelectionType.SpecifyCaret;
                _currentCaretIndex = textBox.CaretIndex + 1;
                e.Handled = false;
                return;
            }
            // 向上
            if (e.Key == Key.Up)
            {
                e.Handled = false;
                _selectionType = SelectionType.EndCaret;
                DoIncrease();
                return;
            }
            //向下
            if (e.Key == Key.Down)
            {
                e.Handled = false;
                _selectionType = SelectionType.EndCaret;
                DoDecrease();
                return;
            }
            // 向左
            if (e.Key == Key.Left)
            {
                e.Handled = false;
                return;
            }
            // 向右
            if (e.Key == Key.Right)
            {
                e.Handled = false;
                return;
            }
            // 回退
            if (e.Key == Key.Back)
            {
                IsKeyChanged = true;
                e.Handled = false;
                _selectionType = SelectionType.SpecifyCaret;
                if (textBox.SelectedText.Length > 0)
                {
                    _currentCaretIndex = textBox.CaretIndex;
                }
                else
                {
                    _currentCaretIndex = textBox.CaretIndex - 1;
                }
                return;
            }
            // 删除
            if (e.Key == Key.Delete)
            {
                IsKeyChanged = true;
                _selectionType = SelectionType.SpecifyCaret;
                _currentCaretIndex = textBox.CaretIndex;
                e.Handled = false;
                return;
            }

            e.Handled = true;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (decimal.TryParse(textBox.Text, out decimal result))
                {
                    Value = result;
                }
                else
                {
                    Value = 0m;
                }

                if (_selectionType == SelectionType.EndCaret)
                    textBox.CaretIndex = textBox.Text.Length;
                if (_selectionType == SelectionType.SpecifyCaret && _currentCaretIndex >= 0)
                    textBox.CaretIndex = _currentCaretIndex;
                if (_selectionType == SelectionType.EndSelection && textBox.Text.Length > 0)
                    textBox.Select(textBox.Text.Length - 1, 1);
                if (!IsKeyChanged)
                    _selectionType = SelectionType.None;
                this.TextChanged?.Invoke(textBox.Text);
            }
        }

        #endregion

        #region Value DP
        public decimal Value
        {
            get { return (decimal)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(decimal),
            typeof(NumericUpDown),
            new FrameworkPropertyMetadata(
                0m,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnValueChangedCallback,
                CoerceValueCallback),
            ValidateValueCallback);

        private static void OnValueChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is NumericUpDown s)
            {
                if (s.IsKeyChanged)
                {
                    s.IsKeyChanged = false;
                    if (s.PART_TextBox.Text == "")
                        return;
                    if (s.PART_TextBox.Text == "-")
                        return;
                    if (s.PART_TextBox.Text.EndsWith("."))
                        return;
                }
                if (e.OldValue != e.NewValue)
                {
                    s.PART_TextBox.Text = s.GetValueByDecimalDigit(s.DecimalDigit, e.NewValue);
                    s.RaiseEvent(new RoutedEventArgs(NumericUpDown.ValueChangedEvent, s));
                }
            }
        }

        private static object CoerceValueCallback(DependencyObject sender, object value)
        {
            return GetValidValue(sender, value);
        }

        private static bool ValidateValueCallback(object value)
        {
            if (value == null)
                return false;
            if ((value is decimal val)
                && val >= decimal.MinValue
                && val <= decimal.MaxValue)
                return true;
            return false;
        }
        #endregion

        #region ButtonUp
        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            this.DoIncrease();
            SelectAll();
        }

        private void DoIncrease()
        {
            if (this.Value >= this.MaxValue) return;

            if (Increment <= 0)
                Increment = 1;

            Value = Value + Increment;
            decimal adjustment = (decimal)(((Value * 10000) % (10000 * Increment)) / 10000);
            if (adjustment * 10000 < 0.0001m)
            {
                return;
            }

            Value = Value - adjustment;
            this.RaiseEvent(new RoutedEventArgs(NumericUpDown.IncreasingEvent, this));
        }
        #endregion

        #region ButtonDown
        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            DoDecrease();
            SelectAll();
        }

        private void DoDecrease()
        {
            if (this.Value <= this.MinValue) return;

            if (Increment <= 0)
                Increment = 1;

            Value = Value - Increment;

            decimal adjustment = (decimal)(((Value * 10000) % (10000 * Increment)) / 10000);
            if (adjustment * 10000 < 0.0001m)
            {
                return;
            }

            Value = Value + Increment - adjustment;
            this.RaiseEvent(new RoutedEventArgs(NumericUpDown.DecreasingEvent, this));
        }
        #endregion

        #region CaretBrush DP
        // 光标颜色
        public Brush CaretBrush
        {
            get => this.GetValue<Brush>(CaretBrushProperty);
            set => this.SetValue(CaretBrushProperty, value);
        }

        public static readonly DependencyProperty CaretBrushProperty =
            DependencyProperty.Register("CaretBrush", typeof(Brush), typeof(NumericUpDown));
        #endregion

        #region ButtonVisibility DP
        // 是否显示递增/减按钮
        public Visibility ButtonVisibility
        {
            get => this.GetValue<Visibility>(ButtonVisibilityProperty);
            set => this.SetValue(ButtonVisibilityProperty, value);
        }

        public static readonly DependencyProperty ButtonVisibilityProperty =
            DependencyProperty.Register(
                "ButtonVisibility",
                typeof(Visibility),
                typeof(NumericUpDown),
                new PropertyMetadata(Visibility.Visible));
        #endregion

        #region MaxValue DP
        public decimal MaxValue
        {
            get { return (decimal)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
            "MaxValue",
            typeof(decimal),
            typeof(NumericUpDown),
            new FrameworkPropertyMetadata(
                decimal.MaxValue, MaxValueChangedCallback, CoerceMaxValueCallback));

        private static void MaxValueChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is NumericUpDown s)
            {
                s.CoerceValue(NumericUpDown.MinValueProperty);
                s.CoerceValue(NumericUpDown.ValueProperty);
            }
        }

        private static object CoerceMaxValueCallback(DependencyObject sender, object value)
        {
            if ((sender is NumericUpDown s) && (value is decimal v))
            {
                if (v < s.MinValue)
                    return s.MinValue;
            }
            return value;
        }
        #endregion

        #region MinValue DP
        public decimal MinValue
        {
            get { return (decimal)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(
            "MinValue",
            typeof(decimal),
            typeof(NumericUpDown),
            new FrameworkPropertyMetadata(0m, MinValueChangedCallback, CoerceMinValueCallback));

        private static void MinValueChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is NumericUpDown s)
            {
                s.CoerceValue(NumericUpDown.MaxValueProperty);
                s.CoerceValue(NumericUpDown.ValueProperty);
            }
        }

        private static object CoerceMinValueCallback(DependencyObject sender, object value)
        {
            if ((sender is NumericUpDown s) && (value is decimal v))
            {
                if (v > s.MaxValue)
                    return s.MaxValue;
            }
            return value;
        }
        #endregion

        #region DecimalDigit DP
        public int DecimalDigit
        {
            get => this.GetValue<int>(DecimalDigitProperty);
            set => this.SetValue(DecimalDigitProperty, value);
        }

        public static readonly DependencyProperty DecimalDigitProperty = DependencyProperty.Register(
            "DecimalDigit",
            typeof(int),
            typeof(NumericUpDown),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion

        #region Increment DP
        public decimal Increment
        {
            get { return (decimal)GetValue(IncrementProperty); }
            set { SetValue(IncrementProperty, value); }
        }

        public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register(
            "Increment",
            typeof(decimal),
            typeof(NumericUpDown),
            new FrameworkPropertyMetadata(
                1m,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                null,
                CoerceIncrementCallback));

        private static object CoerceIncrementCallback(DependencyObject sender, object value)
        {
            return GetValidValue(sender, value);
        }
        #endregion

        #region ValueChanged Event
        public event RoutedEventHandler ValueChanged
        {
            add
            {
                this.AddHandler(ValueChangedEvent, value);
            }
            remove
            {
                this.RemoveHandler(ValueChangedEvent, value);
            }
        }

        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            "ValueChanged",
            RoutingStrategy.Direct,
            typeof(RoutedEventHandler),
            typeof(NumericUpDown));
        #endregion

        #region Increasing Event
        public event RoutedEventHandler Increasing
        {
            add
            {
                this.AddHandler(IncreasingEvent, value);
            }
            remove
            {
                this.RemoveHandler(IncreasingEvent, value);
            }
        }

        public static readonly RoutedEvent IncreasingEvent = EventManager.RegisterRoutedEvent(
            "Increasing",
            RoutingStrategy.Direct,
           typeof(RoutedEventHandler),
           typeof(NumericUpDown));
        #endregion

        #region Decreasing Event
        public event RoutedEventHandler Decreasing
        {
            add
            {
                this.AddHandler(DecreasingEvent, value);
            }
            remove
            {
                this.RemoveHandler(DecreasingEvent, value);
            }
        }

        public static readonly RoutedEvent DecreasingEvent = EventManager.RegisterRoutedEvent(
            "Decreasing",
            RoutingStrategy.Direct,
            typeof(RoutedEventHandler),
            typeof(NumericUpDown));
        #endregion

        #region Help
        private static object GetValidValue(DependencyObject sender, object value)
        {
            if ((sender is NumericUpDown s) && (value is decimal v))
            {
                if (v < s.MinValue)
                    return s.MinValue;
                if (v > s.MaxValue)
                    return s.MaxValue;
            }
            return value;
        }

        private string GetValueByDecimalDigit(int decimalDigit, object value)
        {
            if (value == null)
                return "";

            var valueString = value.ToString();
            var indexOf = valueString.IndexOf(".");
            if (indexOf < 0)
                return valueString;

            if (decimalDigit == 0)
                return valueString.Substring(0, indexOf);

            if (valueString.Length > indexOf + decimalDigit + 1)
                return valueString.Substring(0, indexOf + decimalDigit + 1);

            return valueString;
        }
        #endregion

        public void SelectAll()
        {
            PART_TextBox.SelectAll();
        }
    }
}

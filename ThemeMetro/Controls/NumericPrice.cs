/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：ThemeMetro
*   文件名称 ：NumericPrice.cs
*   =================================
*   创 建 者 ：mingrui.wu
*   创建日期 ：7/12/2022 3:41:51 PM 
*   功能描述 ：
*   使用说明 ：
*   =================================
*   修  改 者 ：
*   修改日期 ：
*   修改内容 ：
*   =================================
*  
***************************************************************************/
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using ThemeMetro.Common;

namespace ThemeMetro.Controls
{
    [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_ButtonUp", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_ButtonDown", Type = typeof(ButtonBase))]
    public class NumericPrice : Control
    {
        enum TextStatus
        {
            Select,
            Edit,
            Write,
        }

        private TextBox PART_TextBox = new TextBox();
        private TextStatus _textStatus = TextStatus.Select;
        private int _selectedLength = 0;
        private bool _isDoubleClick = false;

        public bool IsKeyChanged { get; set; }

        public NumericPrice() { }

        static NumericPrice()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(NumericPrice),
                new FrameworkPropertyMetadata(typeof(NumericPrice)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_TextBox") is TextBox textBox)
            {
                PART_TextBox = textBox;
                PART_TextBox.GotFocus -= TextBox_GotFocus;
                PART_TextBox.GotFocus += TextBox_GotFocus;
                PART_TextBox.LostKeyboardFocus -= TextBox_LostKeyboardFocus;
                PART_TextBox.LostKeyboardFocus += TextBox_LostKeyboardFocus;
                PART_TextBox.PreviewMouseUp -= TextBox_PreviewMouseUp;
                PART_TextBox.PreviewMouseUp += TextBox_PreviewMouseUp;
                PART_TextBox.PreviewKeyDown -= TextBox_PreviewKeyDown;
                PART_TextBox.PreviewKeyDown += TextBox_PreviewKeyDown;
                PART_TextBox.PreviewMouseDoubleClick -= TextBox_PreviewMouseDoubleClick;
                PART_TextBox.PreviewMouseDoubleClick += TextBox_PreviewMouseDoubleClick;
                PART_TextBox.KeyUp -= TextBox_KeyUp;
                PART_TextBox.KeyUp += TextBox_KeyUp;
                PART_TextBox.TextChanged -= TextBox_TextChanged;
                PART_TextBox.TextChanged += TextBox_TextChanged;
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
            this.GotKeyboardFocus += NumericPrice_GotKeyboardFocus;
        }

        private void NumericPrice_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            PART_TextBox.Focus();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!(sender is TextBox tbx))
                return;

            if (tbx.Text.Length > 0)
            {
                _textStatus = TextStatus.Select;
                tbx.Select(tbx.Text.Length - 1, 1);
            }
            else
            {
                _textStatus = TextStatus.Write;
            }
        }

        private void TextBox_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is TextBox tbx))
                return;

            if (_isDoubleClick)
            {
                _isDoubleClick = false;
                return;
            }
            if (tbx.Text.Length > 0)
            {
                if (tbx.SelectedText == null || tbx.SelectedText.Length == 0)
                {
                    _textStatus = TextStatus.Select;
                    tbx.Select(tbx.Text.Length - 1, 1);
                    tbx.Focus();
                }
                else
                {
                    // 当输入框中数据长度超过输入框的长度时，鼠标点击输入框外，TextBox焦点不会失去，
                    // 为了强制失去焦点，故调用ScrollToLine，
                    // 说明：但这不是标准修复这个问题，以后还需深入调研
                    tbx.ScrollToLine(0);
                }
            }
            else
            {
                _textStatus = TextStatus.Write;
            }
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (!(sender is TextBox tbx))
                return;

            if (_textStatus == TextStatus.Select && tbx.Text.Length > 0)
                tbx.Select(tbx.Text.Length - 1, 1);

            if (_textStatus == TextStatus.Write && tbx.Text.Length > 0)
                tbx.CaretIndex = tbx.Text.Length;

            if (_textStatus == TextStatus.Edit
                && tbx.Text.Length > 0
                && tbx.Text.Length >= _selectedLength)
            {
                tbx.Select(tbx.Text.Length - _selectedLength, _selectedLength);
            }
        }

        #region TextBox
        public event System.Action<string> TextChanged;

        private void TextBox_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is TextBox tbx))
                return;

            _isDoubleClick = true;
            _textStatus = TextStatus.Write;
            tbx.SelectAll();
        }

        private void TextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox tbx)
            {
                if (tbx.Text.EndsWith("."))
                    tbx.Text = tbx.Text.TrimEnd('.');
                if (tbx.Text == "-")
                    tbx.Text = "0";
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //System.Console.WriteLine($"TextBox_PreviewKeyDown: {e.Key}");
            if (e.Key == Key.Tab)
            {
                e.Handled = false;
                return;
            }
            if (!(sender is TextBox tbx))
                return;
            // 减号
            if (e.Key == Key.OemMinus || e.Key == Key.Subtract)
            {
                e.Handled = true;
                if (this.MinValue >= 0)
                    return;
                IsKeyChanged = true;
                _textStatus = TextStatus.Write;
                tbx.Text = "-";
                return;
            }
            //点
            if (e.Key == Key.Decimal || e.Key == Key.OemPeriod)
            {
                e.Handled = true;
                if (this.DecimalDigit == 0)
                    return;
                if (string.IsNullOrEmpty(tbx.Text))
                    return;
                if (tbx.Text.EndsWith("-"))
                    return;
                if (tbx.Text.Contains("."))
                {
                    if (tbx.SelectedText.Contains("."))
                    {
                        IsKeyChanged = true;
                        if (_textStatus == TextStatus.Select)
                            tbx.Text = tbx.Text.Replace(tbx.SelectedText, ".");
                    }
                    return;
                }
                if (_textStatus == TextStatus.Select && tbx.SelectedText.Length > 0)
                {
                    IsKeyChanged = true;
                    tbx.Text = tbx.Text.Replace(tbx.SelectedText, ".");
                    return;
                }
                if (_textStatus == TextStatus.Edit)
                {
                    IsKeyChanged = true;
                    _textStatus = TextStatus.Write;
                    tbx.Text = string.Concat(tbx.Text, ".");
                    e.Handled = true;
                    return;
                }
                e.Handled = false;
                return;
            }
            //数字
            if ((e.Key >= Key.D0 && e.Key <= Key.D9)
                || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
            {
                if (_textStatus == TextStatus.Select)
                {
                    if (tbx.Text.Length == 1)
                    {
                        _textStatus = TextStatus.Write;
                    }
                    else
                    {
                        _selectedLength = 1;
                        _textStatus = TextStatus.Edit;
                    }
                    e.Handled = false;
                    return;
                }

                if (_textStatus == TextStatus.Edit)
                {
                    var txtLen = tbx.Text.Length;
                    if (this.DecimalDigit <= 0)
                    {
                        _selectedLength += 1;
                        if (txtLen - _selectedLength >= 0)
                        {
                            var txtHead = tbx.Text.Substring(0, txtLen - _selectedLength);
                            var txtTail = tbx.Text.Substring(txtLen - _selectedLength, _selectedLength);
                            tbx.Text = string.Concat(txtHead, txtTail.Substring(1, _selectedLength - 1), InputProvider.GetStringNumber(e.Key));
                        }
                    }
                    else
                    {
                        string txt = null;
                        var index = tbx.Text.IndexOf(".");
                        if (index > 0)
                        {
                            txt = tbx.Text.Replace(".", string.Empty);
                        }
                        else
                        {
                            txt = tbx.Text;
                        }

                        if (!tbx.SelectedText.Contains("."))
                        {
                            _selectedLength += 1;
                        }

                        if (txt.Length - _selectedLength >= 0)
                        {
                            var txtHead = txt.Substring(0, txt.Length - _selectedLength);
                            var txtTail = txt.Substring(txt.Length - _selectedLength, _selectedLength);
                            var newTxt = string.Concat(txtHead, txtTail.Substring(1, _selectedLength - 1), InputProvider.GetStringNumber(e.Key));

                            if (index > 0 && index >= txt.Length - _selectedLength)
                                _selectedLength += 1;

                            if (index > 0)
                            {
                                tbx.Text = newTxt.Insert(index, ".");
                            }
                            else
                            {
                                tbx.Text = newTxt;
                            }
                        }
                    }

                    if (_selectedLength == txtLen)
                        _textStatus = TextStatus.Write;

                    e.Handled = true;
                    return;
                }

                if (_textStatus == TextStatus.Write)
                {
                    e.Handled = false;
                    return;
                }
            }
            // 向上
            if (e.Key == Key.Up)
            {
                _textStatus = TextStatus.Select;
                e.Handled = false;
                DoIncrease();
                return;
            }
            //向下
            if (e.Key == Key.Down)
            {
                _textStatus = TextStatus.Select;
                e.Handled = false;
                DoDecrease();
                return;
            }
            // 回退
            if (e.Key == Key.Back)
            {
                _textStatus = TextStatus.Write;
                if (tbx.Text.Length > 0)
                    IsKeyChanged = true;
                e.Handled = false;
                return;
            }
            // 删除
            if (e.Key == Key.Delete)
            {
                _textStatus = TextStatus.Select;
                IsKeyChanged = true;
                e.Handled = false;
                return;
            }
            // 回车
            if (e.Key == Key.Enter)
            {
                if (Keyboard.FocusedElement is UIElement focusElement)
                {
                    try
                    {
                        TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
                        focusElement.MoveFocus(request);
                    }
                    catch { }
                }
                e.Handled = true;
                return;
            }
            e.Handled = true;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                //System.Console.WriteLine($"TextBox_TextChanged: {textBox.Text}");
                var text = GetLegalText(textBox.Text);
                if (textBox.Text != text)
                {
                    textBox.Text = text;
                    return;
                }

                if (decimal.TryParse(textBox.Text, out decimal result))
                {
                    if (Value != result)
                    {
                        if (result > MaxValue)
                        {
                            Value = MaxValue;
                            textBox.Text = MaxValue.ToString();
                        }
                        else if (result < MinValue)
                        {
                            Value = MinValue;
                            textBox.Text = MinValue.ToString();
                        }
                        else
                        {
                            Value = result;
                        }
                    }
                }
                else
                {
                    Value = 0m;
                }
                IsKeyChanged = false;
                TextChanged?.Invoke(textBox.Text);
            }
        }
        #endregion

        #region Value DP
        public object OldValue { get; set; }

        public decimal Value
        {
            get { return (decimal)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(decimal),
            typeof(NumericPrice),
            new FrameworkPropertyMetadata(
                0m,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnValueChangedCallback,
                CoerceValueCallback),
            ValidateValueCallback);

        private static void OnValueChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is NumericPrice s)
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
                if (e.NewValue != null && !s.PART_TextBox.Text.Equals(e.NewValue.ToString()))
                {
                    s.OldValue = e.NewValue;
                    s.PART_TextBox.Text = s.GetValueByDecimalDigit(s.DecimalDigit, e.NewValue);
                    s.IsSelectedAll = false;
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
        private bool _isUpDown = false;

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            _isUpDown = true;
            this.DoIncrease();
            SelectAll();
        }

        public void DoIncrease()
        {
            if (this.Value >= this.MaxValue)
                return;
            if (Increment <= 0)
                Increment = 1;
            Value = Value + Increment;
            decimal adjustment = (decimal)(((Value * 10000) % (10000 * Increment)) / 10000);
            if (adjustment * 10000 < 0.0001m)
                return;
            Value = Value - adjustment;
        }
        #endregion

        #region ButtonDown
        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            _isUpDown = true;
            DoDecrease();
            SelectAll();
        }

        public void DoDecrease()
        {
            if (this.Value <= this.MinValue)
                return;
            if (Increment <= 0)
                Increment = 1;
            Value = Value - Increment;
            decimal adjustment = (decimal)(((Value * 10000) % (10000 * Increment)) / 10000);
            if (adjustment * 10000 < 0.0001m)
                return;
            Value = Value + Increment - adjustment;
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
            DependencyProperty.Register("CaretBrush", typeof(Brush), typeof(NumericPrice));
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
                typeof(NumericPrice),
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
            typeof(NumericPrice),
            new FrameworkPropertyMetadata(
                decimal.MaxValue, MaxValueChangedCallback, CoerceMaxValueCallback));

        private static void MaxValueChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is NumericPrice s)
            {
                s.CoerceValue(NumericPrice.MinValueProperty);
                s.CoerceValue(NumericPrice.ValueProperty);
            }
        }

        private static object CoerceMaxValueCallback(DependencyObject sender, object value)
        {
            if ((sender is NumericPrice s) && (value is decimal v))
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
            typeof(NumericPrice),
            new FrameworkPropertyMetadata(0m, MinValueChangedCallback, CoerceMinValueCallback));

        private static void MinValueChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is NumericPrice s)
            {
                s.CoerceValue(NumericPrice.MaxValueProperty);
                s.CoerceValue(NumericPrice.ValueProperty);
            }
        }

        private static object CoerceMinValueCallback(DependencyObject sender, object value)
        {
            if ((sender is NumericPrice s) && (value is decimal v))
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
            typeof(NumericPrice),
            new FrameworkPropertyMetadata(
                0,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                null,
                CoerceDecimalDigitCallback));

        private static object CoerceDecimalDigitCallback(DependencyObject sender, object value)
        {
            if (sender is NumericPrice s)
            {
                // 当该控件，用CellEditingTemplate中时，会出现先获取到Value值，后才拿到DecimalDigit值，
                // 导致PART_TextBox.Text的内容不对。故此处在拿到DecimalDigit后要使用OldValue重新初始
                // 化PART_TextBox.Text的内容。
                int newDecimalDigit = (int)value;
                if (s.DecimalDigit != newDecimalDigit)
                    s.PART_TextBox.Text = s.GetValueByDecimalDigit(newDecimalDigit, s.OldValue);
            }
            return value;
        }

        private string GetLegalText(string text)
        {
            if (DecimalDigit > 0
                && !string.IsNullOrEmpty(text)
                && text.Contains("."))
            {
                var list = text.Split('.');
                if (list.Length > 1)
                {
                    if (list[1].Length > DecimalDigit)
                    {
                        return text.Substring(0, text.Length - list[1].Length + DecimalDigit);
                    }
                }
            }

            return text;
        }
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
            typeof(NumericPrice),
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

        #region 设置焦点、且输入框全选
        public bool IsSelectedAll
        {
            get => this.GetValue<bool>(IsSelectedAllProperty);
            set => this.SetValue(IsSelectedAllProperty, value);
        }

        public static readonly DependencyProperty IsSelectedAllProperty = DependencyProperty.Register(
            "IsSelectedAll",
            typeof(bool),
            typeof(NumericPrice),
            new FrameworkPropertyMetadata(
                false,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                null,
                IsSelectedAllCallback));

        private static object IsSelectedAllCallback(DependencyObject sender, object value)
        {
            if (sender is NumericPrice s && value is bool d && d)
            {
                s.Focus();
                s.SelectAll();
            }
            return value;
        }
        #endregion

        #region Help
        private static object GetValidValue(DependencyObject sender, object value)
        {
            if ((sender is NumericPrice s) && (value is decimal v))
            {
                if (s._isUpDown)
                {
                    s._isUpDown = false;
                }
                else if (string.IsNullOrEmpty(s.PART_TextBox.Text))
                {
                    return value;
                }
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
            _textStatus = TextStatus.Write;
            PART_TextBox.SelectAll();
        }
    }
}

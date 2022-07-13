/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：ThemeMetro.Behaviors
*   文件名称 ：TextBoxBehavior.cs
*   =================================
*   创 建 者 ：mingrui.wu
*   创建日期 ：7/12/2022 8:40:44 PM 
*   功能描述 ：
*   使用说明 ：
*   =================================
*   修 改 者 ：
*   修改日期 ：
*   修改内容 ：
*   =================================
*  
***************************************************************************/
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ThemeCore.Common;
using ThemeMetro.Common;

namespace ThemeMetro.Controls.Behaviors
{
    /// <summary>
    /// 文本框行为，支持水印、图标和圆角
    /// </summary>
    public class TextBoxBehavior
    {
        #region Watermark
        public const string PART_WatermarkName = "PART_Watermark";

        /// <summary>
        /// 设置为空时的文本内容
        /// </summary>
        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.RegisterAttached(
                "Watermark",
                typeof(string),
                typeof(TextBoxBehavior),
                new PropertyMetadata(OnWatermarkPropertyChanged));

        public static readonly DependencyProperty WatermarkColorProperty =
            DependencyProperty.RegisterAttached(
                "WatermarkColor",
                typeof(Brush),
                typeof(TextBoxBehavior),
                new PropertyMetadata(OnWatermarkColorPropertyChanged));

        public static readonly DependencyProperty WatermarkFontSizeProperty =
            DependencyProperty.RegisterAttached(
                "WatermarkFontSize",
                typeof(double),
                typeof(TextBoxBehavior),
                new FrameworkPropertyMetadata(
                12D,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                null,
                CoerceFontSizeCallback));

        public static void SetWatermark(UIElement obj, string value) => obj.SetValue(WatermarkProperty, value);

        public static string GetWatermark(UIElement obj) => obj.GetValue<string>(WatermarkProperty);

        public static void SetWatermarkColor(UIElement obj, Brush value) => obj.SetValue(WatermarkColorProperty, value);

        public static Brush GetWatermarkColor(UIElement obj) => obj.GetValue<Brush>(WatermarkColorProperty);

        public static void SetWatermarkFontSize(UIElement obj, double value) => obj.SetValue(WatermarkFontSizeProperty, value);

        public static double GetWatermarkFontSize(UIElement obj) => obj.GetValue<double>(WatermarkFontSizeProperty);

        private static void OnWatermarkPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs arg)
        {
            if (arg == null) return;
            if (!(source is TextBox textBox)) return;
            var tb = textBox.FindChildrenFromTemplate<TextBlock>(PART_WatermarkName);
            if (tb != null)
            {
                tb.Text = GetWatermark(textBox);
                tb.Foreground = GetWatermarkColor(textBox);
                tb.FontSize = GetWatermarkFontSize(textBox);
            }
        }

        private static void OnWatermarkColorPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs arg)
        {
            if (arg == null) return;
            if (!(source is TextBox textBox)) return;
            var tb = textBox.FindChildrenFromTemplate<TextBlock>(PART_WatermarkName);
            if (tb != null)
            {
                tb.Text = GetWatermark(textBox);
                tb.Foreground = GetWatermarkColor(textBox);
                tb.FontSize = GetWatermarkFontSize(textBox);
            }
        }

        private static object CoerceFontSizeCallback(DependencyObject source, object value)
        {
            if (!(source is TextBox textBox)) return value;
            if (!(value is double size)) return value;

            var tb = textBox.FindChildrenFromTemplate<TextBlock>(PART_WatermarkName);
            if (tb != null)
            {
                tb.Text = GetWatermark(textBox);
                tb.Foreground = GetWatermarkColor(textBox);
                tb.FontSize = size;
            }

            return value;
        }
        #endregion

        #region CornerRadius
        /// <summary>
        /// 设置圆角
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
           DependencyProperty.RegisterAttached(
               "CornerRadius",
               typeof(CornerRadius),
               typeof(TextBoxBehavior),
               new PropertyMetadata(default(CornerRadius), OnCornerRaduisPropertyChanged));

        public static void SetCornerRadius(UIElement obj, CornerRadius value) => obj.SetValue(CornerRadiusProperty, value);

        public static CornerRadius GetCornerRadius(UIElement obj) => obj.GetValue<CornerRadius>(CornerRadiusProperty);

        private static void OnCornerRaduisPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs arg)
        {
            if (source is Control control)
            {
                control.Initialized -= OnInitizedToSetCornerRadius;
                control.Initialized += OnInitizedToSetCornerRadius;
            }
        }

        private static void OnInitizedToSetCornerRadius(object sender, EventArgs args)
        {
            if (sender is Control control)
            {
                try
                {
                    var border = control.FindChildrenFromTemplate<Border>(StyleConstants.PART_BorderName);
                    if (border != null)
                        border.CornerRadius = GetCornerRadius(control);
                }
                catch { }
            }
        }
        #endregion

        #region Icon
        /// <summary>
        /// 设置Icon
        /// </summary>
        public static readonly DependencyProperty IconProperty =
           DependencyProperty.RegisterAttached(
               "Icon",
               typeof(FrameworkElement),
               typeof(TextBoxBehavior),
               new PropertyMetadata(OnIconPropertyChanged));

        /// <summary>
        /// 设置IconDock
        /// </summary>
        public static readonly DependencyProperty IconDockProperty =
           DependencyProperty.RegisterAttached("IconDock", typeof(Dock), typeof(TextBoxBehavior), new PropertyMetadata(Dock.Left));

        public static void SetIcon(UIElement obj, FrameworkElement value) => obj.SetValue(IconProperty, value);

        public static FrameworkElement GetIcon(UIElement obj) => obj.GetValue<FrameworkElement>(IconProperty);

        public static void SetIconDock(UIElement obj, Dock value) => obj.SetValue(IconDockProperty, value);

        public static Dock GetIconDock(UIElement obj) => obj.GetValue<Dock>(IconDockProperty);

        private static void OnIconPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs arg)
        {
            if (source is Control control)
            {
                control.Initialized -= OnInitizedToSetIcon;
                control.Initialized += OnInitizedToSetIcon;
            }
        }

        private static void OnInitizedToSetIcon(object sender, EventArgs args)
        {
            if (sender is Control control)
            {
                try
                {
                    var cc = control.FindChildrenFromTemplate<ContentControl>(StyleConstants.PART_IconHostName);
                    if (cc != null)
                    {
                        cc.Content = GetIcon(control);
                        DockPanel.SetDock(cc, GetIconDock(sender as UIElement));
                    }
                }
                catch { }
            }
        }
        #endregion

        #region InputFilter
        public static readonly DependencyProperty InputFilterProperty =
            DependencyProperty.RegisterAttached(
                "InputFilter",
                typeof(InputFilter),
                typeof(TextBoxBehavior),
                new PropertyMetadata(InputFilter.None, OnInputFilterPropertyChanged));

        public static void SetInputFilter(UIElement obj, object value) => obj.SetValue(InputFilterProperty, value);

        public static InputFilter GetInputFilter(UIElement obj) => obj.GetValue<InputFilter>(InputFilterProperty);

        private static void OnInputFilterPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs arg)
        {
            if (!(source is TextBox textBox)) return;
            if (!(arg.NewValue is InputFilter)) return;

            InputFilter inputFilter = (InputFilter)arg.NewValue;
            KeyEventHandler keyHandle = (s, e) =>
            {
                if (inputFilter == InputFilter.None) return;
                if (e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Left || e.Key == Key.Right)
                    return;
                if (inputFilter == InputFilter.OnlyInteger)
                {
                    if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
                        return;
                }
                e.Handled = true;
            };
            textBox.PreviewKeyDown -= keyHandle;
            textBox.PreviewKeyDown += keyHandle;
        }
        #endregion
    }
}

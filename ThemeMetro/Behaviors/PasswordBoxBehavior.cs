/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：ThemeMetro.Behaviors
*   文件名称 ：PasswordBoxBehavior.cs
*   =================================
*   创 建 者 ：mingrui.wu
*   创建日期 ：7/12/2022 8:37:59 PM 
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
using ThemeMetro.Common;

namespace ThemeMetro.Controls.Behaviors
{
    /// <summary>
    /// 密码框行为，支持水印和圆角
    /// </summary>
    public class PasswordBoxBehavior
    {
        /// <summary>
        /// 设置密码
        /// </summary>
        /// <remarks>此处需要默认双向绑定</remarks>
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached(
                "Password",
                typeof(string),
                typeof(PasswordBoxBehavior),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPasswordPropertyChanged));

        /// <summary>
        /// 设置是否有效，默认为false，如果为true将启用密码绑定
        /// </summary>
        public static readonly DependencyProperty IsEnableProperty =
            DependencyProperty.RegisterAttached(
                "IsEnable",
                typeof(bool),
                typeof(PasswordBoxBehavior),
                new PropertyMetadata(false, Attach));

        private static readonly DependencyProperty IsUpdatingProperty =
            DependencyProperty.RegisterAttached(
                "IsUpdating", typeof(bool), typeof(PasswordBoxBehavior));

        public static void SetIsEnable(DependencyObject dp, bool value)
        {
            dp.SetValue(IsEnableProperty, value);
        }

        public static bool GetIsEnable(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsEnableProperty);
        }

        public static string GetPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject dp, string value)
        {
            dp.SetValue(PasswordProperty, value);
        }

        private static bool GetIsUpdating(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsUpdatingProperty);
        }

        private static void SetIsUpdating(DependencyObject dp, bool value)
        {
            dp.SetValue(IsUpdatingProperty, value);
        }

        private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            passwordBox.PasswordChanged -= PasswordChanged;

            if (!(bool)GetIsUpdating(passwordBox))
            {
                passwordBox.Password = (string)e.NewValue;
            }
            passwordBox.PasswordChanged += PasswordChanged;
        }

        private static void Attach(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;

            if (passwordBox == null)
                return;

            if ((bool)e.OldValue)
            {
                passwordBox.PasswordChanged -= PasswordChanged;
            }

            if ((bool)e.NewValue)
            {
                passwordBox.PasswordChanged += PasswordChanged;
            }
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            SetIsUpdating(passwordBox, true);
            SetPassword(passwordBox, passwordBox.Password);
            SetIsUpdating(passwordBox, false);
        }

        public const string PART_WatermarkName = "PART_Watermark";
        private const string PART_Title = "PART_Title";

        /// <summary>
        /// 设置为空时的文本内容
        /// </summary>
        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.RegisterAttached(
                "Watermark",
                typeof(string),
                typeof(PasswordBoxBehavior),
                new PropertyMetadata(OnWatermarkPropertyChanged));

        public static void SetWatermark(UIElement obj, string value) => obj.SetValue(WatermarkProperty, value);

        public static string GetWatermark(UIElement obj) => obj.GetValue<string>(WatermarkProperty);

        /// <summary>
        /// 设置Icon
        /// </summary>
        public static readonly DependencyProperty IconProperty =
           DependencyProperty.RegisterAttached(
               "Icon",
               typeof(FrameworkElement),
               typeof(PasswordBoxBehavior),
               new PropertyMetadata(OnIconPropertyChanged));

        public static void SetIcon(UIElement obj, FrameworkElement value) => obj.SetValue(IconProperty, value);

        public static FrameworkElement GetIcon(UIElement obj) => obj.GetValue<FrameworkElement>(IconProperty);

        private static void OnWatermarkPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs arg)
        {
            if (source is PasswordBox pwd)
            {
                pwd.Initialized -= OnInitizedToSetWatermarkVisibility;
                pwd.Initialized += OnInitizedToSetWatermarkVisibility;
                pwd.PasswordChanged -= OnPasswordChanged;
                pwd.PasswordChanged += OnPasswordChanged;
                pwd.IsKeyboardFocusedChanged -= PasswordBox_IsKeyboardFocusedChanged;
                pwd.IsKeyboardFocusedChanged += PasswordBox_IsKeyboardFocusedChanged;
                pwd.Unloaded += delegate
                {
                    pwd.Initialized -= OnInitizedToSetWatermarkVisibility;
                    pwd.PasswordChanged -= OnPasswordChanged;
                    pwd.IsKeyboardFocusedChanged -= PasswordBox_IsKeyboardFocusedChanged;
                };
            }
        }

        private static void PasswordBox_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is PasswordBox pwd)
            {
                try
                {
                    var waterMark = pwd.FindChildrenFromTemplate<TextBlock>(PART_WatermarkName);
                    var title = pwd.FindChildrenFromTemplate<TextBlock>(PART_Title);
                    if (pwd.IsKeyboardFocused)
                    {
                        if (title != null)
                            title.Visibility = Visibility.Visible;
                        waterMark.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        if (title != null)
                            title.Visibility = string.IsNullOrEmpty(pwd.Password) ? Visibility.Collapsed : Visibility.Visible;
                        waterMark.Visibility = string.IsNullOrEmpty(pwd.Password) ? Visibility.Visible : Visibility.Collapsed;
                    }
                }
                catch { }
            }
        }

        private static void OnInitizedToSetWatermarkVisibility(object sender, EventArgs args)
        {
            if (sender is PasswordBox pwd)
            {
                try
                {
                    var tb = pwd.FindChildrenFromTemplate<TextBlock>(PART_WatermarkName);
                    if (tb != null)
                    {
                        var title = pwd.FindChildrenFromTemplate<TextBlock>(PART_Title);
                        tb.Text = GetWatermark(pwd);
                        tb.Visibility = string.IsNullOrEmpty(pwd.Password) ? Visibility.Visible : Visibility.Collapsed;
                        if (title != null)
                            title.Visibility = Visibility.Collapsed;
                    }
                }
                catch { }
            }
        }
        private static void OnPasswordChanged(object sender, RoutedEventArgs args)
        {
            if (sender is PasswordBox pwd)
            {
                try
                {
                    var tb = pwd.FindChildrenFromTemplate<TextBlock>(PART_WatermarkName);
                    var title = pwd.FindChildrenFromTemplate<TextBlock>(PART_Title);
                    if (tb == null) return;

                    tb.Visibility = string.IsNullOrEmpty(pwd.Password) && !pwd.IsKeyboardFocused ? Visibility.Visible : Visibility.Collapsed;
                    if (title != null)
                        title.Visibility = string.IsNullOrEmpty(pwd.Password) && !pwd.IsKeyboardFocused ? Visibility.Collapsed : Visibility.Visible;
                }
                catch { }
            }
        }

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
            if (sender is Control ctrl)
            {
                try
                {
                    var cc = ctrl.FindChildrenFromTemplate<ContentControl>(StyleConstants.PART_IconHostName);
                    if (cc != null)
                    {
                        cc.Content = GetIcon(ctrl);
                        DockPanel.SetDock(cc, Dock.Left);
                    }
                }
                catch { }
            }
        }
    }
}

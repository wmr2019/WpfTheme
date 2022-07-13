/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：ThemeMetro.Behaviors
*   文件名称 ：WindowChromeBehavior.cs
*   =================================
*   创 建 者 ：mingrui.wu
*   创建日期 ：7/12/2022 8:42:50 PM 
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
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shell;
using ThemeMetro.Common;

namespace ThemeMetro.Controls.Behaviors
{
    public static class WindowChromeBehavior
    {
        public const string WINDOW_BORDER = "PART_WindowBorder";
        public const string CONTENT_CONTROL = "PART_ContentControl";
        public const string TITLE = "PART_Title";
        public const string TITLE_BORDER = "PART_TitleBorder";
        public const string CONTENT_CONTROL_BORDER = "PART_ContentControl_Border";
        public const string CLOSE_BUTTON = "PART_CloseButton";
        public const string MAXIMIZE_BUTTON = "PART_MaximizeButton";
        public const string MINIMIZE_BUTTON = "PART_MinimizeButton";

        /// <summary>
        /// 设置是否可用，默认是false，即不启用定制样式
        /// </summary>
        public static readonly DependencyProperty IsEnableProperty =
            DependencyProperty.RegisterAttached(
                "IsEnable",
                typeof(bool),
                typeof(WindowChromeBehavior),
                new PropertyMetadata(OnIsEnablePropertyChange));

        /// <summary>
        /// 设置窗体标题栏的高度
        /// </summary>
        public static readonly DependencyProperty TitleHeightProperty =
            DependencyProperty.RegisterAttached(
                "TitleHeight",
                typeof(int),
                typeof(WindowChromeBehavior),
                new PropertyMetadata(24, OnTitleHeightPropertyChange));

        /// <summary>
        /// 设置标题栏的背景色
        /// </summary>
        public static readonly DependencyProperty TitleBackgroundProperty =
            DependencyProperty.RegisterAttached(
                "TitleBackground",
                typeof(Brush),
                typeof(WindowChromeBehavior),
                new PropertyMetadata(Brushes.Transparent, OnTitleBackgroundPropertyChange));

        /// <summary>
        /// 设置标题栏的字体大小
        /// </summary>
        public static readonly DependencyProperty TitleFontSizeProperty =
            DependencyProperty.RegisterAttached(
                "TitleFontSize",
                typeof(int),
                typeof(WindowChromeBehavior),
                new PropertyMetadata(14));

        /// <summary>
        /// 设置标题的内容
        /// </summary>
        public static readonly DependencyProperty TitleContentProperty =
            DependencyProperty.RegisterAttached(
                "TitleContent",
                typeof(FrameworkElement),
                typeof(WindowChromeBehavior),
                new PropertyMetadata(OnTitleContentPropertyChange));

        public static readonly DependencyProperty TitleContentVerticalAlignmentProperty =
            DependencyProperty.RegisterAttached(
                "TitleContentVerticalAlignment",
                typeof(VerticalAlignment),
                typeof(WindowChromeBehavior),
                new PropertyMetadata(VerticalAlignment.Center, OnTitleContentVerticalAlignmentPropertyChange));

        public static readonly DependencyProperty TitleForegroundProperty =
            DependencyProperty.RegisterAttached(
                "TitleForeground",
                typeof(Brush),
                typeof(WindowChromeBehavior),
                new PropertyMetadata(Brushes.Black));

        /// <summary>
        /// 设置窗体命令按钮的Padding
        /// </summary>
        public static readonly DependencyProperty WindowCommandButtonPaddingProperty =
            DependencyProperty.RegisterAttached(
                "WindowCommandButtonPadding",
                typeof(Thickness),
                typeof(WindowChromeBehavior),
                new PropertyMetadata(new Thickness(16, 9, 16, 9), OnWindowCommandButtonPaddingPropertyChange));

        public static readonly DependencyProperty WindowCommandButtonMouseOverColorProperty =
            DependencyProperty.RegisterAttached(
                "WindowCommandButtonMouseOverColor",
                typeof(Brush),
                typeof(WindowChromeBehavior),
                new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E5E5E5"))));

        /// <summary>
        /// 设置最小化按钮是否可见
        /// </summary>
        public static readonly DependencyProperty MinimizeButtonVisibleWhenInToolWindowModeProperty =
            DependencyProperty.RegisterAttached(
                "MinimizeButtonVisibleWhenInToolWindowMode",
                typeof(bool),
                typeof(WindowChromeBehavior),
                new PropertyMetadata(false));

        /// <summary>
        /// 设置窗体边框画刷
        /// </summary>
        public static readonly DependencyProperty BorderBrushProperty =
            DependencyProperty.RegisterAttached(
                "BorderBrush",
                typeof(SolidColorBrush),
                typeof(WindowChromeBehavior),
                new PropertyMetadata(ThemeColor.GetWindowActiveBorderBrush()));

        private static void OnTitleHeightPropertyChange(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (e == null) return;
            if (!(e.NewValue is int)) return;
            if (!(obj is Window window)) return;

            var height = (int)e.NewValue;
            try
            {
                var windowBorder = window.FindChildrenFromTemplate<Border>(WINDOW_BORDER);
                var titleBorder = windowBorder.FindChildren<Border>(TITLE_BORDER);
                titleBorder.Height = height;
            }
            catch { }

            Action setSize = () =>
            {
                try
                {
                    var chrome = WindowChrome.GetWindowChrome(window);
                    if (chrome != null) chrome.CaptionHeight = height - 5;
                }
                catch { }
            };
            if (window.IsInitialized)
            {
                setSize();
            }
            else
            {
                window.Initialized += delegate
                {
                    setSize();
                };
            }
        }

        private static void OnTitleBackgroundPropertyChange(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is Brush)) return;
            if (!(obj is Window window)) return;
            try
            {
                var brush = (Brush)e.NewValue;
                var windowBorder = window.FindChildrenFromTemplate<Border>(WINDOW_BORDER);
                var titleBorder = windowBorder.FindChildren<Border>(TITLE_BORDER);
                titleBorder.Background = brush;
            }
            catch { }
        }

        private static void OnWindowCommandButtonPaddingPropertyChange(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is Thickness)) return;
            if (!(obj is Window window)) return;
            window.Initialized += delegate
            {
                try
                {
                    var padding = (Thickness)e.NewValue;
                    var windowBorder = window.FindChildrenFromTemplate<Border>(WINDOW_BORDER);
                    windowBorder.FindChildren<Button>().ToList().ForEach(x => x.Padding = padding);
                }
                catch { }
            };
        }

        private static void OnTitleContentVerticalAlignmentPropertyChange(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is VerticalAlignment)) return;
            if (!(obj is Window window)) return;
            window.Initialized += delegate
            {
                try
                {
                    var va = (VerticalAlignment)e.NewValue;
                    var contentControl = window.FindChildrenFromTemplate<Border>(WINDOW_BORDER)?.FindChildren<ContentControl>(TITLE);
                    contentControl.VerticalAlignment = va;
                }
                catch { }
            };
        }

        private static void OnTitleContentPropertyChange(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is FrameworkElement)) return;
            if (!(obj is Window window)) return;
            try
            {
                var el = (FrameworkElement)e.NewValue;
                var contentControl = window.FindChildrenFromTemplate<Border>(WINDOW_BORDER)?.FindChildren<ContentControl>(TITLE);
                contentControl.Content = el;
            }
            catch { }
        }

        private static void OnIsEnablePropertyChange(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is bool)) return;
            if (!(bool)e.NewValue) return;
            if (!(obj is Window window)) return;
            if (window.WindowStyle == WindowStyle.None) //如果是none无需加载
                return;

            var windowStyle = window.WindowStyle;
            Action init = () =>
            {
                try
                {
                    var content = window.Content as UIElement;
                    var windowBorder = window.FindChildrenFromTemplate<Border>(WINDOW_BORDER);

                    var titleBorder = windowBorder.FindChildren<Border>(TITLE_BORDER);
                    TextElement.SetFontSize(titleBorder, GetTitleFontSize(window));
                    TextElement.SetForeground(titleBorder, GetTitleForeground(window));
                    titleBorder.Background = GetTitleBackground(obj as UIElement);
                    titleBorder.Height = GetTitleHeight(obj as UIElement);

                    var windowContentControl = windowBorder.FindChildren<ContentControl>(CONTENT_CONTROL);

                    windowContentControl.Content = content;
                    window.Content = windowBorder.Parent;
                    //注册命令按钮事件

                    var buttons = windowBorder.FindChildren<Button>().ToList();
                    buttons.ForEach(x => x.Foreground = GetTitleForeground(window));
                    var maxBtn = buttons.First(x => x.Name == MAXIMIZE_BUTTON);
                    var minBtn = buttons.First(x => x.Name == MINIMIZE_BUTTON);
                    if (windowStyle == WindowStyle.ToolWindow)
                    {
                        maxBtn.Visibility = Visibility.Collapsed;

                        minBtn.Visibility = GetMinimizeButtonVisibleWhenInToolWindowMode(window) ? Visibility.Visible : Visibility.Collapsed;
                        window.ResizeMode = ResizeMode.CanMinimize;
                    }
                    else
                    {
                        maxBtn.Click += (s, arg) => window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;

                    }
                    minBtn.Click += (s, arg) => window.WindowState = WindowState.Minimized;
                    buttons.First(x => x.Name == CLOSE_BUTTON).Click += (s, arg) => window.Close();
                }
                catch { }
            };

            if (window.IsInitialized)
            {
                init();
            }
            else
            {
                window.Initialized += delegate
                {
                    init();
                };
            }

            window.WindowStyle = WindowStyle.None;
            // window.AllowsTransparency = true;

            WindowChrome chrome = new WindowChrome
            {
                ResizeBorderThickness = new Thickness(5),
                CornerRadius = new CornerRadius(0),
                UseAeroCaptionButtons = false,
                GlassFrameThickness = new Thickness(0),
                NonClientFrameEdges = NonClientFrameEdges.None,
            };
            window.SizeChanged += delegate
            {
                try
                {
                    var windowBorder = window.FindChildrenFromTemplate<Border>(WINDOW_BORDER);
                    var titleBorder = windowBorder.FindChildren<Border>(TITLE_BORDER);
                    var newThickness = window.WindowState == WindowState.Maximized
                        ? new Thickness(window.Padding.Left + 5, window.Padding.Top + 5, window.Padding.Right + 5, window.Padding.Bottom)
                        : window.Padding;
                    titleBorder.Padding = newThickness;
                    titleBorder.VerticalAlignment = VerticalAlignment.Bottom;
                    titleBorder.Height = window.WindowState == WindowState.Maximized ? titleBorder.Height + 5 : GetTitleHeight(window);
                    windowBorder.FindChildren<Border>(CONTENT_CONTROL_BORDER).Padding = newThickness;
                }
                catch { }
            };

            WindowChrome.SetWindowChrome(window, chrome);
        }

        public static void SetMinimizeButtonVisibleWhenInToolWindowMode(UIElement el, bool value) => el.SetValue(MinimizeButtonVisibleWhenInToolWindowModeProperty, value);

        public static bool GetMinimizeButtonVisibleWhenInToolWindowMode(UIElement el) => el.GetValue<bool>(MinimizeButtonVisibleWhenInToolWindowModeProperty);

        public static void SetIsEnable(UIElement el, bool value) => el.SetValue(IsEnableProperty, value);

        public static bool GetIsEnable(UIElement el) => el.GetValue<bool>(IsEnableProperty);

        public static void SetTitleContentVerticalAlignment(UIElement el, VerticalAlignment value) => el.SetValue(TitleContentVerticalAlignmentProperty, value);

        public static VerticalAlignment GetTitleContentVerticalAlignment(UIElement el) => el.GetValue<VerticalAlignment>(TitleContentVerticalAlignmentProperty);

        public static void SetWindowCommandButtonPadding(UIElement el, Thickness value) => el.SetValue(WindowCommandButtonPaddingProperty, value);

        public static Thickness GetWindowCommandButtonPadding(UIElement el) => el.GetValue<Thickness>(WindowCommandButtonPaddingProperty);

        public static void SetWindowCommandButtonMouseOverColor(UIElement el, Brush value) => el.SetValue(WindowCommandButtonMouseOverColorProperty, value);

        public static Brush GetWindowCommandButtonMouseOverColor(UIElement el) => el.GetValue<Brush>(WindowCommandButtonMouseOverColorProperty);

        public static void SetTitleFontSize(UIElement el, int value) => el.SetValue(TitleFontSizeProperty, value);

        public static int GetTitleFontSize(UIElement el) => el.GetValue<int>(TitleFontSizeProperty);

        public static void SetTitleHeight(UIElement el, int value) => el.SetValue(TitleHeightProperty, value);

        public static int GetTitleHeight(UIElement el) => el.GetValue<int>(TitleHeightProperty);

        public static void SetTitleBackground(UIElement el, Brush value) => el.SetValue(TitleBackgroundProperty, value);

        public static Brush GetTitleBackground(UIElement el) => el.GetValue<Brush>(TitleBackgroundProperty);

        public static void SetTitleContent(UIElement el, FrameworkElement value) => el.SetValue(TitleContentProperty, value);

        public static FrameworkElement GetTitleContent(UIElement el) => el.GetValue<FrameworkElement>(TitleContentProperty);

        public static void SetTitleForeground(UIElement el, Brush value) => el.SetValue(TitleForegroundProperty, value);

        public static Brush GetTitleForeground(UIElement el) => el.GetValue<Brush>(TitleForegroundProperty);

        public static void SetBorderBrush(UIElement el, SolidColorBrush value) => el.SetValue(BorderBrushProperty, value);

        public static SolidColorBrush GetBorderBrush(UIElement el) => el.GetValue<SolidColorBrush>(BorderBrushProperty);
    }
}

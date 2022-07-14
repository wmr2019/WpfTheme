﻿using System.Windows;
using System.Windows.Input;

namespace ThemeMetro.Controls.Behaviors
{
    public class WindowBehavior
    {
        #region Blink
        public static readonly DependencyProperty IsBlinkEnableProperty =
            DependencyProperty.RegisterAttached(
                "IsBlinkEnable",
                typeof(bool),
                typeof(WindowBehavior),
                new PropertyMetadata(false, OnIsBlinkEnablePropertyChanged));

        public static bool GetIsBlinkEnable(DependencyObject obj) => obj.GetValue<bool>(IsBlinkEnableProperty);

        public static void SetIsBlinkEnable(DependencyObject obj, object value) => obj.SetValue(IsBlinkEnableProperty, value);

        private static void OnIsBlinkEnablePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is Window window))
                return;
            if (!(e.NewValue is bool))
                return;

            var isEnable = (bool)e.NewValue;
            if (isEnable)
                window.FlashWindow();
            else
                window.StopFlashingWindow();
        }

        public static readonly DependencyProperty CloseBlinkWhenDeactiveProperty =
            DependencyProperty.RegisterAttached(
                "CloseBlinkWhenDeactive",
                typeof(bool),
                typeof(WindowBehavior),
                new PropertyMetadata(false, OnCloseBlinkWhenDeactiveChange));

        public static bool GetCloseBlinkWhenDeactive(DependencyObject obj) => obj.GetValue<bool>(CloseBlinkWhenDeactiveProperty);

        public static void SetCloseBlinkWhenDeactive(DependencyObject obj, object value) => obj.SetValue(CloseBlinkWhenDeactiveProperty, value);

        private static void OnCloseBlinkWhenDeactiveChange(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is Window window))
                return;
            if (!(e.NewValue is bool))
                return;
            var isEnable = (bool)e.NewValue;
            if (!isEnable) return;
            window.Activated += OnWindowActivated;
            window.StopFlashingWindow();
        }

        private static void OnWindowActivated(object sender, System.EventArgs e)
        {
            if (sender is Window window)
                window.StopFlashingWindow();
        }
        #endregion

        #region Esc To Close

        public static readonly DependencyProperty EscToCloseProperty =
            DependencyProperty.RegisterAttached(
                "EscToClose",
                typeof(bool),
                typeof(WindowBehavior),
                new PropertyMetadata(false, new PropertyChangedCallback(OnEscToClosePropertyChanged)));

        private static void OnEscToClosePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is Window window))
                return;
            if (!(e.NewValue is bool)) return;
            bool enable = (bool)e.NewValue;
            if (!enable) return;
            window.KeyDown -= OnWatchWindow_Esc_KeyDown;
            window.KeyDown += OnWatchWindow_Esc_KeyDown;
            window.Unloaded += delegate
            {
                window.KeyDown -= OnWatchWindow_Esc_KeyDown;
            };
        }

        public static void SetEscToClose(DependencyObject obj, bool enable) => obj.SetValue(EscToCloseProperty, enable);

        public static void GetEscToClose(DependencyObject obj) => obj.GetValue<bool>(EscToCloseProperty);

        private static void OnWatchWindow_Esc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                (sender as Window)?.Close();
        }
        #endregion
    }
}

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ThemeMetro.Controls.Behaviors
{
    /// <summary>
    /// TabItem附加行为
    /// </summary>
    public class TabItemBehavior
    {
        public static readonly DependencyProperty IsActiveEnableProperty =
        DependencyProperty.RegisterAttached("IsActiveEnable", typeof(bool), typeof(TabItemBehavior)
            , new PropertyMetadata(OnIsActiveEnablePropertyChanged));

        public static bool GetIsActiveEnable(DependencyObject obj)
            => obj.GetValue<bool>(IsActiveEnableProperty);

        public static void SetIsActiveEnable(DependencyObject obj, object value)
            => obj.SetValue(IsActiveEnableProperty, value);

        public static readonly DependencyProperty IsActiveProperty =
          DependencyProperty.RegisterAttached("IsActive", typeof(bool), typeof(TabItemBehavior));

        public static bool GetIsActive(DependencyObject obj)
            => obj.GetValue<bool>(IsActiveProperty);

        public static void SetIsActive(DependencyObject obj, object value)
            => obj.SetValue(IsActiveProperty, value);

        private static void OnIsActiveEnablePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is TabItem tabItem)) return;
            if (e == null) return;
            if (e.NewValue == null) return;
            if (!(e.NewValue is bool))
                return;
            if (!(bool)e.NewValue)
                return;
            tabItem.MouseLeftButtonUp -= TabItem_MouseLeftButtonDown;
            tabItem.MouseLeftButtonUp += TabItem_MouseLeftButtonDown;
            //tabItem.GotKeyboardFocus -= TabItem_GotKeyboardFocus;
            //tabItem.GotKeyboardFocus += TabItem_GotKeyboardFocus;
            //tabItem.LostFocus -= TabItem_LostFocus;
            //tabItem.LostFocus += TabItem_LostFocus;
            //tabItem.LostKeyboardFocus -= TabItem_LostKeyboardFocus;
            //tabItem.LostKeyboardFocus += TabItem_LostKeyboardFocus;
        }

        private static void TabItem_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            SetIsActive(sender as TabItem, false);
        }

        private static void TabItem_LostFocus(object sender, RoutedEventArgs e)
        {
            SetIsActive(sender as TabItem, false);
        }

        private static void TabItem_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            SetIsActive(sender as TabItem, true);
        }

        private static void TabItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetIsActive(sender as TabItem, true);
        }
    }
}

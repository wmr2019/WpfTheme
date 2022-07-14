using System.Windows;
using System.Windows.Input;

namespace ThemeMetro.Controls.Behaviors
{
    /// <summary>
    /// 支持回车转换tab http://madprops.org/blog/enter-to-tab-as-an-attached-property/
    /// </summary>
    public class EnterKeyTraversal
    {
        public static bool GetIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnabledProperty, value);
        }

        static void ue_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement ue && e.Key == Key.Enter)
            {
                e.Handled = true;
                try
                {
                    ue.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
                catch { }
            }
        }

        private static void ue_Unloaded(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement ue)
            {
                ue.Unloaded -= ue_Unloaded;
                ue.PreviewKeyDown -= ue_PreviewKeyDown;
            }
        }

        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached(
                "IsEnabled",
                typeof(bool),
                typeof(EnterKeyTraversal),
                new UIPropertyMetadata(false, IsEnabledChanged));

        static void IsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement ue)
            {
                if (e.NewValue != null && (bool)e.NewValue)
                {
                    ue.Unloaded += ue_Unloaded;
                    ue.PreviewKeyDown += ue_PreviewKeyDown;
                }
                else
                {
                    ue.PreviewKeyDown -= ue_PreviewKeyDown;
                }
            }
        }
    }
}

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ThemeMetro.Controls.Behaviors
{
    public class ButtonBehavior
    {
        #region 禁用键盘的回车键
        public static DependencyProperty DisableKeyboardProperty = DependencyProperty.RegisterAttached(
            "DisableKeyboard", typeof(bool), typeof(ButtonBehavior), new FrameworkPropertyMetadata(false, OnDisableKeyboardChanged));

        public static bool GetDisableKeyboard(DependencyObject target)
        {
            return (bool)target.GetValue(DisableKeyboardProperty);
        }

        public static void SetDisableKeyboard(DependencyObject target, bool value)
        {
            target.SetValue(DisableKeyboardProperty, value);
        }

        private static void OnDisableKeyboardChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (!(target is Button button))
                return;

            if ((bool)e.NewValue == true)
            {
                button.PreviewKeyDown -= Button_PreviewKeyDown;
                button.PreviewKeyDown += Button_PreviewKeyDown;
            }
        }

        private static void Button_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        #endregion
    }
}

/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：ThemeMetro.Behaviors
*   文件名称 ：ButtonBehavior.cs
*   =================================
*   创 建 者 ：mingrui.wu
*   创建日期 ：7/12/2022 7:15:04 PM 
*   功能描述 ：
*   使用说明 ：
*   =================================
*   修 改 者 ：
*   修改日期 ：
*   修改内容 ：
*   =================================
*  
***************************************************************************/
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

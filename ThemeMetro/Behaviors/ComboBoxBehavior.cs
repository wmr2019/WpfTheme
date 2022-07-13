/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：ThemeMetro.Behaviors
*   文件名称 ：ComboBoxBehavior.cs
*   =================================
*   创 建 者 ：mingrui.wu
*   创建日期 ：7/12/2022 7:16:10 PM 
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
using ThemeCore.Common;

namespace ThemeMetro.Controls.Behaviors
{
    public class ComboBoxBehavior
    {
        public const string PART_EMPTY_TextBlock = "PART_EMPTY_TextBlock";

        public static readonly DependencyProperty EmtpyTextProperty =
            DependencyProperty.RegisterAttached(
                "EmtpyText",
                typeof(string),
                typeof(ComboBoxBehavior),
                new PropertyMetadata(null, OnEmtpyTextPropertyChanged));

        public static string GetEmtpyText(DependencyObject obj) => obj.GetValue<string>(EmtpyTextProperty);

        public static void SetEmtpyText(DependencyObject obj, object value) => obj.SetValue(EmtpyTextProperty, value);

        private static void OnEmtpyTextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is ComboBox comboBox))
                return;
            if (!(e.NewValue is string txt))
                return;

            comboBox.Initialized -= ComboBox_Initialized;
            comboBox.Initialized += ComboBox_Initialized;
        }

        private static void ComboBox_Initialized(object sender, System.EventArgs e)
        {
            if (!(sender is ComboBox comboBox))
                return;
            try
            {
                var tb = comboBox.FindChildrenFromTemplate<TextBlock>(PART_EMPTY_TextBlock);
                if (tb != null)
                    tb.Text = GetEmtpyText(comboBox);
            }
            catch { }
        }

        public static readonly DependencyProperty InputFilterProperty =
           DependencyProperty.RegisterAttached(
               "InputFilter",
               typeof(InputFilter),
               typeof(ComboBoxBehavior),
               new PropertyMetadata(InputFilter.None));

        public static void SetInputFilter(UIElement obj, object value) => obj.SetValue(InputFilterProperty, value);

        public static InputFilter GetInputFilter(UIElement obj) => obj.GetValue<InputFilter>(InputFilterProperty);
    }
}

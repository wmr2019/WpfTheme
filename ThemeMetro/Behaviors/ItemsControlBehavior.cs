/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：ThemeMetro.Behaviors
*   文件名称 ：ItemsControlBehavior.cs
*   =================================
*   创 建 者 ：mingrui.wu
*   创建日期 ：7/12/2022 8:37:06 PM 
*   功能描述 ：
*   使用说明 ：
*   =================================
*   修 改 者 ：
*   修改日期 ：
*   修改内容 ：
*   =================================
*  
***************************************************************************/
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using ThemeCore.Common;

namespace ThemeMetro.Controls.Behaviors
{
    /// <summary>
    /// ItemsControlBehavior附加行为
    /// </summary>
    public class ItemsControlBehavior
    {
        #region Active
        public static readonly DependencyProperty ItemMoveEnableProperty =
            DependencyProperty.RegisterAttached(
                "ItemMoveEnable",
                typeof(bool),
                typeof(ItemsControlBehavior),
                new PropertyMetadata(false, ItemMoveEnablePropertyChanged));

        public static readonly DependencyProperty ItemToMoveUpCommandProperty =
            DependencyProperty.RegisterAttached(
                "ItemToMoveUpCommand", typeof(ICommand), typeof(ItemsControlBehavior));

        public static readonly DependencyProperty ItemToMoveDownCommandProperty =
            DependencyProperty.RegisterAttached(
                "ItemToMoveDownCommand", typeof(ICommand), typeof(ItemsControlBehavior));

        public static ICommand GetItemToMoveUpCommand(DependencyObject obj) => obj.GetValue<ICommand>(ItemToMoveUpCommandProperty);
        public static ICommand GetItemToMoveDownCommand(DependencyObject obj) => obj.GetValue<ICommand>(ItemToMoveDownCommandProperty);
        public static void SetItemToMoveUpCommand(DependencyObject obj, object value) => obj.SetValue(ItemToMoveUpCommandProperty, value);
        public static void SetItemToMoveDownCommand(DependencyObject obj, object value) => obj.SetValue(ItemToMoveDownCommandProperty, value);
        public static bool GetItemMoveEnable(DependencyObject obj) => obj.GetValue<bool>(ItemMoveEnableProperty);
        public static void SetItemMoveEnable(DependencyObject obj, object value) => obj.SetValue(ItemMoveEnableProperty, value);

        private static void ItemMoveEnablePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs arg)
        {
            if (!(arg.NewValue is bool)) return;
            if (arg.NewValue == null) return;
            if ((bool)arg.NewValue == false) return;
            if (!(obj is Selector selector)) return;

            SetItemToMoveUpCommand(obj, new RelayCommand(() =>
            {
                if (selector.ItemsSource is IList list)
                {
                    try
                    {
                        var selectedIndex = selector.SelectedIndex;
                        var itemToMoveDown = selector.Items[selectedIndex];
                        list.RemoveAt(selectedIndex);
                        list.Insert(selectedIndex - 1, itemToMoveDown);
                        selector.SelectedIndex = selectedIndex - 1;
                        if (selector is DataGrid)
                            (selector as DataGrid).ScrollIntoView(selector.SelectedItem);
                    }
                    catch { }
                }
            }, () => selector.SelectedIndex > 0));

            SetItemToMoveDownCommand(obj, new RelayCommand(() =>
            {
                if (selector.ItemsSource is IList list)
                {
                    try
                    {
                        var selectedIndex = selector.SelectedIndex;
                        var itemToMoveDown = selector.Items[selectedIndex];
                        list.RemoveAt(selectedIndex);
                        list.Insert(selectedIndex + 1, itemToMoveDown);
                        selector.SelectedIndex = selectedIndex + 1;
                        selector.UpdateLayout();
                        if (selector is DataGrid)
                            (selector as DataGrid).ScrollIntoView(selector.SelectedItem);
                    }
                    catch { }
                }
            }, () => selector.SelectedIndex >= 0 && selector.SelectedIndex < selector.Items.Count - 1));
        }
        #endregion
    }
}

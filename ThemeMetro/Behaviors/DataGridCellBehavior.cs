/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：ThemeMetro.Behaviors
*   文件名称 ：DataGridCellBehavior.cs
*   =================================
*   创 建 者 ：mingrui.wu
*   创建日期 ：7/12/2022 7:24:13 PM 
*   功能描述 ：
*   使用说明 ：
*   =================================
*   修  改 者 ：
*   修改日期 ：
*   修改内容 ：
*   =================================
*  
***************************************************************************/
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace ThemeMetro.Controls.Behaviors
{
    public class DataGridCellBehavior
    {
        #region ShowBorderWhenMouseOver
        public static readonly DependencyProperty ShowBorderWhenMouseOverProperty =
            DependencyProperty.RegisterAttached(
                "ShowBorderWhenMouseOver", typeof(bool), typeof(DataGridCellBehavior));

        public static bool GetShowBorderWhenMouseOver(DependencyObject obj)
            => obj.GetValue<bool>(ShowBorderWhenMouseOverProperty);

        public static void SetShowBorderWhenMouseOver(DependencyObject obj, object value)
            => obj.SetValue(ShowBorderWhenMouseOverProperty, value);
        #endregion

        #region SelectFieldCommand
        public static readonly DependencyProperty SelectFieldCommandProperty
            = DependencyProperty.RegisterAttached(
                "SelectFieldCommand",
                typeof(ICommand),
                typeof(DataGridCellBehavior),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectFieldCommandPropertyChanged));

        public static ICommand GetSelectFieldCommand(DependencyObject obj) => obj.GetValue<ICommand>(SelectFieldCommandProperty);

        public static void SetSelectFieldCommand(DependencyObject obj, object value) => obj.SetValue(SelectFieldCommandProperty, value);

        private static void OnSelectFieldCommandPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs arg)
        {
            if (!(obj is DataGridCell cell)) return;
            if (!(arg.NewValue is ICommand command)) return;
            cell.PreviewMouseDown -= SelectField_PreviewMouseDown;
            cell.PreviewMouseDown += SelectField_PreviewMouseDown;
        }

        private static void SelectField_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is DataGridCell cell))
                return;
            if (cell.Column is DataGridTextColumn txtCol && txtCol.Binding is Binding binding)
            {
                try
                {
                    if (GetSelectFieldCommand(cell) is ICommand command)
                        command.Execute(binding.Path.Path);
                }
                catch { }
            }
            else if (cell.Column is DataGridTemplateColumn templateColumn)
            {
                try
                {
                    if (GetSelectFieldCommand(cell) is ICommand command)
                    {
                        if (!string.IsNullOrEmpty(cell.Column.SortMemberPath))
                        {
                            command.Execute(cell.Column.SortMemberPath);
                        }
                        else
                        {
                            command.Execute(DataGridTemplateColumnBehavior.GetBindingPath(templateColumn));
                        }
                    }
                }
                catch { }
            }
        }
        #endregion

        #region ClickCellCommand
        public static readonly DependencyProperty ClickCellCommandProperty
            = DependencyProperty.RegisterAttached(
                "ClickCellCommand",
                typeof(ICommand),
                typeof(DataGridCellBehavior),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnClickCellCommandPropertyChanged));

        public static ICommand GetClickCellCommand(DependencyObject obj) => obj.GetValue<ICommand>(ClickCellCommandProperty);

        public static void SetClickCellCommand(DependencyObject obj, object value) => obj.SetValue(ClickCellCommandProperty, value);

        private static void OnClickCellCommandPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs arg)
        {
            if (!(obj is DataGridCell cell)) return;
            if (!(arg.NewValue is ICommand command)) return;
            cell.PreviewMouseDown -= ClickCell_PreviewMouseDown;
            cell.PreviewMouseDown += ClickCell_PreviewMouseDown;
        }

        private static void ClickCell_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is DataGridCell cell))
                return;
            try
            {
                if (GetClickCellCommand(cell) is ICommand command)
                    command.Execute(cell.DataContext);
            }
            catch { }
        }
        #endregion

        #region 解决点击最后一行，垂直滚动条下拉问题
        public static readonly DependencyProperty DisableSlideProperty
            = DependencyProperty.RegisterAttached(
                "DisableSlide",
                typeof(bool),
                typeof(DataGridCellBehavior),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnDisableSlidePropertyChanged));

        public static bool GetDisableSlide(DependencyObject obj) => obj.GetValue<bool>(DisableSlideProperty);

        public static void SetDisableSlide(DependencyObject obj, object value) => obj.SetValue(DisableSlideProperty, value);

        private static void OnDisableSlidePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs arg)
        {
            if (!(obj is DataGridCell cell)) return;
            if (arg.NewValue is bool isDisabled && isDisabled)
            {
                cell.RequestBringIntoView -= DisableSlide_RequestBringIntoView;
                cell.RequestBringIntoView += DisableSlide_RequestBringIntoView;
            }
        }

        private static void DisableSlide_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            if (e != null)
                e.Handled = true;
        }
        #endregion
    }
}

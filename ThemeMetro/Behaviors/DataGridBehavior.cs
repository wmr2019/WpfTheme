/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：ThemeMetro.Behaviors
*   文件名称 ：DataGridBehavior.cs
*   =================================
*   创 建 者 ：mingrui.wu
*   创建日期 ：7/12/2022 7:18:02 PM 
*   功能描述 ：
*   使用说明 ：
*   =================================
*   修  改 者 ：
*   修改日期 ：
*   修改内容 ：
*   =================================
*  
***************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using ThemeMetro.Models;

namespace ThemeMetro.Controls.Behaviors
{
    public class DataGridBehavior
    {
        #region ShowRowNumber
        public static DependencyProperty DisplayRowNumberProperty =
            DependencyProperty.RegisterAttached("DisplayRowNumber",
                                                typeof(bool),
                                                typeof(DataGridBehavior),
                                                new FrameworkPropertyMetadata(false, OnDisplayRowNumberChanged));

        public static bool GetDisplayRowNumber(DependencyObject target)
        {
            return (bool)target.GetValue(DisplayRowNumberProperty);
        }

        public static void SetDisplayRowNumber(DependencyObject target, bool value)
        {
            target.SetValue(DisplayRowNumberProperty, value);
        }

        private static void OnDisplayRowNumberChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (!(target is DataGrid dataGrid))
                return;

            if ((bool)e.NewValue == true)
            {
                try
                {
                    dataGrid.HeadersVisibility = DataGridHeadersVisibility.All;
                    void loadedRowHandler(object sender, DataGridRowEventArgs ea)
                    {
                        if (GetDisplayRowNumber(dataGrid) == false)
                        {
                            dataGrid.LoadingRow -= loadedRowHandler;
                            return;
                        }
                        var idx = ea.Row.GetIndex() + 1;
                        ea.Row.Header = idx;
                        //这里需要设定宽度，否则在datagrid从隐藏状态切换到显示状态会导致左上角header被第一列columnheader给遮盖
                        dataGrid.RowHeaderWidth = 48;
                    }
                    dataGrid.LoadingRow += loadedRowHandler;

                    void itemsChangedHandler(object sender, ItemsChangedEventArgs ea)
                    {
                        if (GetDisplayRowNumber(dataGrid) == false)
                        {
                            dataGrid.ItemContainerGenerator.ItemsChanged -= itemsChangedHandler;
                            return;
                        }
                        GetVisualChildCollection<DataGridRow>(dataGrid).ForEach(d => d.Header = d.GetIndex() + 1);
                    }
                    dataGrid.ItemContainerGenerator.ItemsChanged += itemsChangedHandler;
                }
                catch { }
            }
        }

        private static List<T> GetVisualChildCollection<T>(object parent) where T : Visual
        {
            List<T> visualCollection = new List<T>();
            GetVisualChildCollection(parent as DependencyObject, visualCollection);
            return visualCollection;
        }

        private static void GetVisualChildCollection<T>(DependencyObject parent, List<T> visualCollection) where T : Visual
        {
            try
            {
                int count = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < count; i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                    if (child is T)
                    {
                        visualCollection.Add(child as T);
                    }
                    if (child != null)
                    {
                        GetVisualChildCollection(child, visualCollection);
                    }
                }
            }
            catch { }
        }
        #endregion

        #region SelectAllButtonTemplate
        //https://coderelief.net/2011/01/04/style-microsofts-datagrid-selectall-button-using-attached-properties/

        public static readonly DependencyProperty SelectAllButtonTemplateProperty =
            DependencyProperty.RegisterAttached("SelectAllButtonTemplate",
                typeof(ControlTemplate), typeof(DataGridBehavior),
                new UIPropertyMetadata(null, OnSelectAllButtonTemplateChanged));

        public static ControlTemplate GetSelectAllButtonTemplate(DataGrid obj)
        {
            return (ControlTemplate)obj.GetValue(SelectAllButtonTemplateProperty);
        }

        public static void SetSelectAllButtonTemplate(DataGrid obj, ControlTemplate value)
        {
            obj.SetValue(SelectAllButtonTemplateProperty, value);
        }

        private static void OnSelectAllButtonTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is DataGrid dataGrid))
            {
                return;
            }

            void handler(object sender, EventArgs args)
            {
                DependencyObject dep = dataGrid;
                try
                {
                    while (dep != null && VisualTreeHelper.GetChildrenCount(dep) != 0
                        && !(dep is Button && ((Button)dep).Command == DataGrid.SelectAllCommand))
                    {
                        dep = VisualTreeHelper.GetChild(dep, 0);
                    }

                    if (dep is Button button)
                    {
                        ControlTemplate template = GetSelectAllButtonTemplate(dataGrid);
                        button.Template = template;
                        dataGrid.LayoutUpdated -= handler;
                    }
                }
                catch { }
            }
            dataGrid.LayoutUpdated -= handler;
            dataGrid.LayoutUpdated += handler;
        }
        #endregion

        #region ScrollToEndAfterRowAdded
        public static readonly DependencyProperty ScrollToEndAfterRowAddedProperty =
            DependencyProperty.RegisterAttached("ScrollToEndAfterRowAdded",
                                                typeof(bool),
                                                typeof(DataGridBehavior),
                                                new PropertyMetadata(false, OnScrollToEndAfterRowAddedPropertyValueChanged));


        public static bool GetScrollToEndAfterRowAdded(DependencyObject dependencyObject)
            => dependencyObject.GetValue<bool>(ScrollToEndAfterRowAddedProperty);

        public static void SetScrollToEndAfterRowAdded(DependencyObject target, bool value)
            => target.SetValue(ScrollToEndAfterRowAddedProperty, value);

        private static void OnScrollToEndAfterRowAddedPropertyValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is DataGrid dataGrid)) return;
            if (!(e.NewValue is bool)) return;
            if (!(bool)e.NewValue) return;

            dataGrid.Loaded -= DataGrid_Loaded;
            dataGrid.Loaded += DataGrid_Loaded;
        }

        private static void DataGrid_Loaded(object sender, EventArgs e)
        {
            if (!(sender is DataGrid dataGrid))
                return;

            if (!(dataGrid.ItemsSource is INotifyCollectionChanged))
                return;
            if (dataGrid.ItemsSource is INotifyCollectionChanged cv)
            {
                void changedEventHandler(object s, NotifyCollectionChangedEventArgs arg)
                {
                    if (arg.Action == NotifyCollectionChangedAction.Add)
                        foreach (var item in arg.NewItems)
                        {
                            try
                            {
                                dataGrid.ScrollIntoView(item);
                            }
                            catch { }
                        }
                }
                cv.CollectionChanged -= changedEventHandler;
                cv.CollectionChanged += changedEventHandler;
                dataGrid.Unloaded += delegate
                {
                    cv.CollectionChanged -= changedEventHandler;
                };
            }
            dataGrid.Loaded -= DataGrid_Loaded; //load事件只允许订阅一次，所以此处需要取消订阅
        }
        #endregion

        #region MultiSelect Binding
        //https://stackoverflow.com/questions/22868445/select-multiple-items-from-a-datagrid-in-an-mvvm-wpf-project
        public static readonly DependencyProperty SelectedItemsSourceProperty =
            DependencyProperty.RegisterAttached("SelectedItemsSource",
                                                typeof(IList),
                                                typeof(DataGridBehavior),
                                                new PropertyMetadata(null, OnSelectedItemsSourcePropertyValueChanged));

        public static IEnumerable GetSelectedItemsSource(DependencyObject dependencyObject)
            => dependencyObject.GetValue<IEnumerable>(SelectedItemsSourceProperty);

        public static void SetSelectedItemsSource(DependencyObject target, IEnumerable value)
            => target.SetValue(SelectedItemsSourceProperty, value);

        private static void OnSelectedItemsSourcePropertyValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is DataGrid dataGrid)) return;
            dataGrid.SelectionChanged -= DataGrid_SelectionChanged;
            dataGrid.SelectionChanged += DataGrid_SelectionChanged;
        }

        private static void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var dataGrid = sender as DataGrid;
                dataGrid.SetValue(SelectedItemsSourceProperty, dataGrid.SelectedItems);
            }
            catch { }
        }
        #endregion

        #region DataColumnConfig
        /// <summary>
        /// 需要配置的列集合
        /// </summary>
        public static readonly DependencyProperty ConfigColumnsProperty =
            DependencyProperty.RegisterAttached("ConfigColumns",
                                                typeof(IEnumerable),
                                                typeof(DataGridBehavior),
                                                new PropertyMetadata(null, OnConfigColumnsPropertyValueChanged));

        public static IEnumerable GetConfigColumns(DependencyObject dependencyObject)
            => dependencyObject.GetValue<IEnumerable>(ConfigColumnsProperty);

        public static void SetConfigColumns(DependencyObject target, IEnumerable value)
            => target.SetValue(ConfigColumnsProperty, value);

        private static void OnConfigColumnsPropertyValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is DataGrid dataGrid)) return;
            if (!(e.NewValue is IEnumerable<DataGridColumnConfig> configs)) return;

            var dpd = DependencyPropertyDescriptor.FromProperty(DataGridColumn.DisplayIndexProperty, typeof(DataGridColumn));

            void loadDataGrid(object sender, RoutedEventArgs eventarg)
            {
                if (!(configs is IList<DataGridColumnConfig> configList))
                    return;

                try
                {
                    // 删除不需要的设置显示或隐藏的列
                    var excludeColumns = ExcludeConfigColumns(d);
                    // 设置默认隐藏列
                    var hideColumns = HideColumns(d);

                    if (configList.Count == 0)
                    {
                        // 当本地没有存储列隐藏的配置文件时，需要设置“DefaultHideColumnNames”中默认隐藏的列
                        // 故此处是默认加载列的配置
                        foreach (var col in dataGrid.Columns)
                        {
                            if (col.Header != null)
                            {
                                var config = new DataGridColumnConfig
                                {
                                    Name = col.Header.ToString(),
                                };
                                configList.Add(config);
                                SetDefaultColumnConfig(
                                    col, config, hideColumns, excludeColumns);
                                if (config.IsEnable)
                                {
                                    BindColumnConfigChangedEvent(config, col);
                                }
                                if (dataGrid.CanUserReorderColumns)
                                {
                                    dpd.AddValueChanged(col, delegate
                                    {
                                        // 更新列的显示顺序
                                        config.DisplayIndex = col.DisplayIndex;
                                    });
                                }
                            }
                        }
                    }
                    else
                    {
                        // 移除“本地列配置文件”中无效的列
                        var conList = configList.ToList();
                        foreach (var item in conList)
                        {
                            var column = dataGrid.Columns.FirstOrDefault(c => c.Header?.ToString() == item.Name);
                            if (column == null)
                            {
                                configList.Remove(item);
                            }
                        }
                        // 插入新列到“本地列配置文件”中
                        var sortConfigs = configList.OrderBy(c => c.DisplayIndex).ToList();
                        int index = 0;
                        foreach (var item in sortConfigs)
                        {
                            // 为了避免由于列的删除造成DisplayIndex值越界问题，故需要重新初始化列顺序。
                            item.DisplayIndex = index++;
                        }
                        foreach (var col in dataGrid.Columns)
                        {
                            if (col.Header == null)
                                continue;
                            var config = configList.FirstOrDefault(c => c.Name == col.Header.ToString());
                            if (config == null)
                            {
                                config = new DataGridColumnConfig
                                {
                                    Name = col.Header.ToString(),
                                    DisplayIndex = col.DisplayIndex,
                                };
                                configList.Add(config);
                                SetDefaultColumnConfig(
                                    col, config, hideColumns, excludeColumns);

                                // 插入
                                var sortArray = sortConfigs.ToArray();
                                bool isAdd = false;
                                for (int i = 0; i < sortArray.Length; i++)
                                {
                                    if (col.DisplayIndex <= sortArray[i].DisplayIndex && !isAdd)
                                    {
                                        sortConfigs.Insert(i, config);
                                        isAdd = true;
                                    }
                                    if (isAdd)
                                    {
                                        sortArray[i].DisplayIndex += 1;
                                    }
                                }
                                if (sortConfigs.Count < configList.Count)
                                {
                                    sortConfigs.Add(config);
                                }
                            }
                            else if (config.IsEnable)
                            {
                                // 初始化列显示
                                col.Visibility = config.Visible ? Visibility.Visible : Visibility.Collapsed;
                            }
                        }
                        foreach (var config in sortConfigs)
                        {
                            var col = dataGrid.Columns.FirstOrDefault(c => c.Header?.ToString() == config.Name);
                            if (col == null)
                                continue;
                            if (config.IsEnable)
                            {
                                BindColumnConfigChangedEvent(config, col);
                            }
                            if (dataGrid.CanUserReorderColumns)
                            {
                                // 初始化列的显示顺序
                                if (config.DisplayIndex < 0
                                    || config.DisplayIndex >= dataGrid.Columns.Count)
                                {
                                    config.DisplayIndex = dataGrid.Columns.Count - 1;
                                }
                                col.DisplayIndex = config.DisplayIndex;
                                dpd.AddValueChanged(col, delegate
                                {
                                    // 更新列的显示顺序
                                    config.DisplayIndex = col.DisplayIndex;
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Load DataGridColumn error: {ex.Message}");
                }
            }
            dataGrid.Loaded -= loadDataGrid;
            dataGrid.Loaded += loadDataGrid;
        }

        private static void SetDefaultColumnConfig(
            DataGridColumn col,
            DataGridColumnConfig config,
            string[] hideColumns,
            string[] excludeColumns)
        {
            // 初始化默认隐藏列
            if (hideColumns.Contains(config.Name))
            {
                config.Visible = false;
                config.IsDefaultHide = true;
            }
            // 初始化忽略配置列
            if (excludeColumns.Contains(config.Name))
            {
                config.IsEnable = false;
            }
            // 初始化列显示
            if (config.IsEnable)
            {
                col.Visibility = config.Visible ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private static void BindColumnConfigChangedEvent(DataGridColumnConfig config, DataGridColumn col)
        {
            void changed(object s, PropertyChangedEventArgs arg)
            {
                if (config.IsEnable && arg.PropertyName == "Visible")
                {
                    col.Visibility = config.Visible ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            config.PropertyChanged -= changed;
            config.PropertyChanged += changed;
        }

        #region 不需要配置的列名称
        public static readonly DependencyProperty ExcludeConfigColumnNamesProperty =
            DependencyProperty.RegisterAttached("ExcludeConfigColumnNames",
                                                typeof(string),
                                                typeof(DataGridBehavior));

        public static string GetExcludeConfigColumnNames(DependencyObject dependencyObject)
            => dependencyObject.GetValue<string>(ExcludeConfigColumnNamesProperty);

        public static void SetExcludeConfigColumnNames(DependencyObject target, string value)
            => target.SetValue(ExcludeConfigColumnNamesProperty, value);

        private static string[] ExcludeConfigColumns(DependencyObject d)
        {
            if (GetExcludeConfigColumnNames(d) is string excludeNames && !string.IsNullOrEmpty(excludeNames))
            {
                return excludeNames.Split(',');
            }
            return new string[] { };
        }
        #endregion

        #region 需要配置，但默认不显示的列名称
        public static readonly DependencyProperty DefaultHideColumnNamesProperty =
            DependencyProperty.RegisterAttached("DefaultHideColumnNames",
                                                typeof(string),
                                                typeof(DataGridBehavior));

        public static string GetDefaultHideColumnNames(DependencyObject dependencyObject)
            => dependencyObject.GetValue<string>(DefaultHideColumnNamesProperty);

        public static void SetDefaultHideColumnNames(DependencyObject target, string value)
            => target.SetValue(DefaultHideColumnNamesProperty, value);

        private static string[] HideColumns(DependencyObject d)
        {
            if (GetDefaultHideColumnNames(d) is string hideNames
                && !string.IsNullOrEmpty(hideNames))
            {
                return hideNames.Split(',');
            }
            return new string[] { };
        }
        #endregion

        #endregion

        #region FocusWhenDeleteRow
        public static DependencyProperty FocusAfterUnloadRowProperty =
          DependencyProperty.RegisterAttached("FocusAfterUnloadRow",
                                              typeof(bool),
                                              typeof(DataGridBehavior),
                                              new FrameworkPropertyMetadata(false, OnFocusAfterUnloadRowPropertyValueChanged));

        private static void OnFocusAfterUnloadRowPropertyValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs arg)
        {
            if (!(obj is DataGrid datagrid)) return;
            if (!(arg.NewValue is bool value)) return;
            if (!value) return;
            void handle(object s, DataGridRowEventArgs e)
            {
                var win = Window.GetWindow(datagrid);
                if (win != null)
                    datagrid.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.ContextIdle, new Action(() =>
                    {
                        try
                        {
                            FocusManager.SetFocusedElement(Window.GetWindow(datagrid), datagrid);
                        }
                        catch { }
                    }));
            }
            datagrid.UnloadingRow -= handle;
            datagrid.UnloadingRow += handle;
        }

        public static bool GetFocusAfterUnloadRow(DependencyObject obj)
            => obj.GetValue<bool>(FocusAfterUnloadRowProperty);

        public static void SetFocusAfterUnloadRow(DependencyObject obj, bool value)
            => obj.SetValue(FocusAfterUnloadRowProperty, value);
        #endregion
    }
}

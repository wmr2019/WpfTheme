/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：ThemeMetro.Behaviors
*   文件名称 ：DocumentViewerBehavior.cs
*   =================================
*   创 建 者 ：mingrui.wu
*   创建日期 ：7/12/2022 7:26:48 PM 
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ThemeMetro.Common;

namespace ThemeMetro.Controls.Behaviors
{
    public class DocumentViewerBehavior
    {
        public const string PART_PreviousPageButton_Name = "PART_PreviousPageButton";
        public const string PART_NextPageButton_Name = "PART_NextPageButton";
        public const string PART_PageNumberNumericUpDown_Name = "PART_PageNumberNumericUpDown";
        public const string PART_ZoominButton_Name = "PART_ZoominButton";
        public const string PART_ZoomoutButton_Name = "PART_ZoomoutButton";
        public const string PART_ZoomComboBox_Name = "PART_ZoomComboBox";

        public static readonly DependencyProperty IsEnableProperty =
            DependencyProperty.RegisterAttached(
                "IsEnable",
                typeof(bool),
                typeof(DocumentViewerBehavior),
                new PropertyMetadata(false, OnIsEnablePropertyChanged));

        public static bool GetIsEnable(DependencyObject obj) => obj.GetValue<bool>(IsEnableProperty);

        public static void SetIsEnable(DependencyObject obj, object value) => obj.SetValue(IsEnableProperty, value);

        private static void OnIsEnablePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is DocumentViewer dv))
                return;
            if (!(e.NewValue is bool))
                return;
            if (!(bool)e.NewValue)
                return;
            dv.Loaded -= OnDocumentViewerLoaded;
            dv.Loaded += OnDocumentViewerLoaded;
        }

        private static void OnDocumentViewerLoaded(object sender, RoutedEventArgs e)
        {
            if (!(sender is DocumentViewer dv))
                return;

            if (dv.Template.FindName(PART_PreviousPageButton_Name, dv) is Button previousPagebutton)
            {
                previousPagebutton.Command = new RelayCommand(() =>
                {
                    dv.PreviousPage();//前移
                }, () => dv.CanGoToPreviousPage);
            }
            if (dv.Template.FindName(PART_NextPageButton_Name, dv) is Button nextPagebutton)
            {
                nextPagebutton.Command = new RelayCommand(() =>
                {
                    dv.NextPage();//后移
                }, () => dv.CanGoToNextPage);
            }
            if (dv.Template.FindName(PART_PageNumberNumericUpDown_Name, dv) is NumericUpDown pnNumUpDown)
            {
                RoutedEventHandler pageChange = (s, arg) =>
                {
                    if (pnNumUpDown.Value == dv.MasterPageNumber)
                        return;
                    pnNumUpDown.Value = dv.MasterPageNumber;
                };
                pnNumUpDown.ValueChanged += pageChange;
                pnNumUpDown.ValueChanged += pageChange;

                KeyEventHandler pageNumInput = (s, arg) =>
                {
                    if (arg.Key == Key.Return && pnNumUpDown.Tag != null)
                        dv.GoToPage((int)pnNumUpDown.Tag);
                };
                Action<string> pageNumTxtChange = s => pnNumUpDown.Tag = string.IsNullOrEmpty(s) ? 1 : int.Parse(s);
                pnNumUpDown.TextChanged -= pageNumTxtChange;
                pnNumUpDown.TextChanged += pageNumTxtChange;
                pnNumUpDown.PreviewKeyDown -= pageNumInput;
                pnNumUpDown.PreviewKeyDown += pageNumInput;
            }
            if (dv.Template.FindName(PART_ZoomComboBox_Name, dv) is ComboBox zoomComboBox)
            {
                if (zoomComboBox.Template.FindName("PART_EditableTextBox", zoomComboBox) is TextBox zoomTB)
                {
                    Binding zoomBinding = new Binding("Zoom");
                    zoomBinding.StringFormat = "{0}%";
                    zoomBinding.Source = dv;
                    zoomBinding.Mode = BindingMode.OneWay;
                    zoomBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    zoomTB.SetBinding(TextBox.TextProperty, zoomBinding);
                    KeyEventHandler zoomChange = (s, arg) =>
                    {
                        if (arg.Key == Key.Return)
                        {
                            double x;
                            if (double.TryParse((s as TextBox).Text?.TrimEnd('%'), out x))
                                dv.Zoom = Math.Round(x, 0);
                        }
                    };
                    zoomTB.PreviewKeyDown -= zoomChange;
                    zoomTB.PreviewKeyDown += zoomChange;
                }
                SelectionChangedEventHandler selectionChangedEventHandler = (s, arg) =>
                {
                    double x;
                    if (double.TryParse(zoomComboBox.SelectedItem?.ToString().TrimEnd('%'), out x))
                        dv.Zoom = x;
                };
                zoomComboBox.SelectionChanged -= selectionChangedEventHandler;
                zoomComboBox.SelectionChanged += selectionChangedEventHandler;
            }
            if (dv.Template.FindName(PART_ZoominButton_Name, dv) is Button zoomInButton)
            {
                zoomInButton.Command = new RelayCommand(() =>
                {
                    dv.IncreaseZoom();
                }, () => dv.CanIncreaseZoom);
            }
            if (dv.Template.FindName(PART_ZoomoutButton_Name, dv) is Button zoomOutButton)
            {
                zoomOutButton.Command = new RelayCommand(() =>
                {
                    dv.DecreaseZoom();
                }, () => dv.CanDecreaseZoom);
            }
        }
    }
}

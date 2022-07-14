using System.Windows;
using System.Windows.Controls;

namespace ThemeMetro.Controls.Behaviors
{
    /// <summary>
    /// TreeViewBehavior
    /// </summary>
    public class TreeViewBehavior
    {
        public static readonly DependencyProperty SelectedItemBindingEnableProperty =
            DependencyProperty.RegisterAttached(
                "SelectedItemBindingEnable",
                typeof(bool), typeof(TreeViewBehavior),
                new PropertyMetadata(false, OnSelectedItemBindingEnablePropertyChanged));

        public static bool GetSelectedItemBindingEnable(DependencyObject obj) => obj.GetValue<bool>(SelectedItemBindingEnableProperty);

        public static void SetSelectedItemBindingEnable(DependencyObject obj, object value) => obj.SetValue(SelectedItemBindingEnableProperty, value);

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.RegisterAttached(
                "SelectedItem",
                typeof(object),
                typeof(TreeViewBehavior),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemPropertyChanged));

        public static object GetSelectedItem(DependencyObject obj) => obj.GetValue<object>(SelectedItemProperty);

        public static void SetSelectedItem(DependencyObject obj, object value) => obj.SetValue(SelectedItemProperty, value);

        private static void OnSelectedItemBindingEnablePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is TreeView treeView)) return;
            if (!(e.NewValue is bool)) return;
            treeView.SelectedItemChanged -= TreeView_SelectedItemChanged;
            treeView.SelectedItemChanged += TreeView_SelectedItemChanged;
        }

        private static void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SetSelectedItem(sender as TreeView, e.NewValue);
        }

        private static void OnSelectedItemPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            //if (!(sender is TreeView)) return;

            //if (!GetSelectedItemBindingEnable(sender)) return;
            //var item = e.NewValue as TreeViewItem;
            //if (item != null)
            //{
            //    item.IsSelected = true;
            //  //  item.SetValue(TreeViewItem.IsSelectedProperty, true);
            //}
        }
    }
}

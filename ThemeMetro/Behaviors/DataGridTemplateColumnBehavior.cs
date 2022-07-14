using System.Windows;
using System.Windows.Controls;

namespace ThemeMetro.Controls.Behaviors
{
    public class DataGridTemplateColumnBehavior
    {
        #region BindingPath

        public static DependencyProperty BindingPathProperty = DependencyProperty.RegisterAttached("BindingPath",
            typeof(string),
            typeof(DataGridTemplateColumnBehavior),
            new FrameworkPropertyMetadata(null));

        public static string GetBindingPath(DependencyObject target)
        {
            if (target == null)
                return null;

            if (target is DataGridTemplateColumn column && column.Header != null)
                return column.Header.ToString();

            return (string)target.GetValue(BindingPathProperty);
        }

        public static void SetBindingPath(DependencyObject target, string value)
        {
            target.SetValue(BindingPathProperty, value);
        }
        #endregion
    }
}

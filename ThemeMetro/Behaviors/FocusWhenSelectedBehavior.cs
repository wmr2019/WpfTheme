using System.Windows;
using System.Windows.Controls;

namespace ThemeMetro.Controls.Behaviors
{
    public class FocusWhenSelectedBehavior
    {
        public static DependencyProperty FocusWhenSelectedProperty = DependencyProperty.RegisterAttached(
            "FocusWhenSelected",
            typeof(bool),
            typeof(FocusWhenSelectedBehavior),
            new PropertyMetadata(false, new PropertyChangedCallback(OnFocusWhenSelectedChanged)));

        public static bool GetFocusWhenSelected(DependencyObject obj)
        {
            return (bool)obj.GetValue(FocusWhenSelectedProperty);
        }

        public static void SetFocusWhenSelected(DependencyObject obj, bool value)
        {
            obj.SetValue(FocusWhenSelectedProperty, value);
        }

        private static void OnFocusWhenSelectedChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is ListBox listbox)
            {
                SelectionChangedEventHandler handler = (s, e) =>
                {
                    try
                    {
                        listbox.UpdateLayout();
                        var listBoxItem = (ListBoxItem)listbox.ItemContainerGenerator.ContainerFromItem(listbox.SelectedItem);
                        if (listBoxItem != null)
                            listBoxItem.Focus();
                    }
                    catch { }
                };
                listbox.SelectionChanged -= handler;
                listbox.SelectionChanged += handler;
                listbox.Unloaded += delegate
                {
                    listbox.SelectionChanged -= handler;
                };
            }
        }
    }
}

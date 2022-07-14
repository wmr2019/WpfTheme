using System.Windows;
using System.Windows.Data;

namespace ThemeCore.Common
{
    public sealed class DependencyVariable<T> : DependencyObject
    {
        public static DependencyProperty ValueProperty { get; } =
            DependencyProperty.Register("Value", typeof(T), typeof(DependencyVariable<T>));

        public T Value
        {
            get { return (T)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public void SetBinding(Binding binding)
        {
            BindingOperations.SetBinding(this, ValueProperty, binding);
        }

        public void SetBinding(object dataContext, string propertyPath)
        {
            SetBinding(new Binding(propertyPath) { Source = dataContext });
        }
    }
}

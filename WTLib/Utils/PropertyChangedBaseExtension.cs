namespace QiCheng
{
    using Caliburn.Micro;
    using System;
    using System.Runtime.CompilerServices;

    public static class PropertyChangedBaseExtension
    {
        public static bool SetWhenDifference<T>(
            this PropertyChangedBase viewModel,
            ref T oldValue,
            T newValue,
            [CallerMemberName] string propertyName = null)
        {
            if (viewModel == null) return false;
            if (typeof(T).IsValueType && newValue.Equals(oldValue))
                return false;
            if (Object.ReferenceEquals(oldValue, newValue))
                return false;
            return viewModel.Set(ref oldValue, newValue, propertyName);
        }

        public static void Activate(this Screen screen)
        {
            var window = (screen.GetView() as System.Windows.Window);
            if (window != null)
            {
                if (window.WindowState == System.Windows.WindowState.Minimized)
                    window.WindowState = System.Windows.WindowState.Normal;
                window.Activate();
            }
        }

        public static void RestoreIfMinimize(this Screen screen)
        {
            var window = (screen.GetView() as System.Windows.Window);
            if (window != null)
            {
                if (window.WindowState == System.Windows.WindowState.Minimized)
                    window.WindowState = System.Windows.WindowState.Normal;
            }
        }
    }
}

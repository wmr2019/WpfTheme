using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ThemeMetro.Controls
{
    public static class DependencyObjectExtension
    {
        /// <summary>
        /// 返回依赖属性的当前有效值
        /// </summary>
        public static T GetValue<T>(this DependencyObject element, DependencyProperty dp) => (T)element.GetValue(dp);

        public static T FindChildren<T>(this DependencyObject element, string name)
            where T : FrameworkElement
        {
            var items = FindChildren<T>(element);
            return items.OfType<FrameworkElement>().FirstOrDefault(x => x.Name == name) as T;
        }

        public static IEnumerable<T> FindChildren<T>(this DependencyObject element)
           where T : DependencyObject
        {
            var count = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i);
                var t = child as T;
                if (t != null)
                    yield return t;
                var children = FindChildren<T>(child);
                foreach (var item in children)
                    yield return item;
            }
        }

        public static T FindParent<T>(this DependencyObject obj)
          where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(obj);
            if (parent == null) return null;
            T result = parent as T;
            if (result != null) return result;
            return FindParent<T>(parent);
        }

        /// <summary>
        /// 从控件模板中获取指定名称的元素
        /// </summary>
        public static T FindChildrenFromTemplate<T>(this Control control, string name)
            where T : FrameworkElement
        {
            if (control.Template == null) return null;
            var result = control.Template.FindName(name, control) as T;
            if (result != null)
                return result;
            control.ApplyTemplate();//应用模板，否则取不到对应的模板元素
            return control.Template.FindName(name, control) as T;
        }

        public static FrameworkElement FindChild(this DependencyObject obj, string childName)
        {
            if (obj == null) return null;

            var queue = new Queue<DependencyObject>();
            queue.Enqueue(obj);

            while (queue.Count > 0)
            {
                obj = queue.Dequeue();

                var childCount = VisualTreeHelper.GetChildrenCount(obj);
                for (var i = 0; i < childCount; i++)
                {
                    var child = VisualTreeHelper.GetChild(obj, i);

                    var fe = child as FrameworkElement;
                    if (fe != null && fe.Name == childName)
                    {
                        return fe;
                    }

                    queue.Enqueue(child);
                }
            }

            return null;
        }
    }
}

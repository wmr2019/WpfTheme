using System.Windows;
using System.Windows.Media;

namespace ThemeCore.Models
{
    public class Theme
    {
        // 主题唯一Id
        public string Id { get; set; }
        // 主题名称
        public string Name { get; set; }
        // 主题颜色代码
        public string Code { get; set; }
        // 主题颜色名称
        public string Text { get; set; }
        // 主题颜色
        public SolidColorBrush Color { get; set; }
        // 主题资源
        public ResourceDictionary Resources { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using ThemeCore.Common;

namespace ThemeMetro.Controls
{
    /// <summary>
    /// Interaction logic for CompleteComboBox.xaml
    /// </summary>
    public partial class CompleteComboBox : ComboBox
    {
        readonly SerialDisposable disposable = new SerialDisposable();

        //static CompleteComboBox()
        //{
        //    DefaultStyleKeyProperty.OverrideMetadata(
        //        typeof(CompleteComboBox),
        //        new FrameworkPropertyMetadata(typeof(CompleteComboBox)));
        //}

        TextBox editableTextBoxCache;
        public TextBox EditableTextBox
        {
            get
            {
                if (editableTextBoxCache == null)
                {
                    const string name = "PART_EditableTextBox";
                    editableTextBoxCache = (TextBox)this.FindChild(name);
                }
                return editableTextBoxCache;
            }
        }

        /// <summary>
        /// Gets text to match with the query from an item.
        /// Never null.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        string TextFromItem(object item)
        {
            if (item == null) return "";

            var d = new DependencyVariable<string>();
            d.SetBinding(item, TextSearch.GetTextPath(this));
            return d.Value ?? "";
        }

        #region Setting
        static readonly DependencyProperty settingProperty =
            DependencyProperty.Register(
                "Setting",
                typeof(DelayFilter),
                typeof(CompleteComboBox)
            );

        public static DependencyProperty SettingProperty
        {
            get { return settingProperty; }
        }

        public DelayFilter Setting
        {
            get { return (DelayFilter)GetValue(SettingProperty); }
            set { SetValue(SettingProperty, value); }
        }

        DelayFilter SettingOrDefault
        {
            get { return Setting ?? DelayFilter.Default; }
        }
        #endregion

        #region OnTextChanged
        long revisionId;

        struct TextBoxStatePreserver
            : IDisposable
        {
            readonly TextBox textBox;
            readonly int selectionStart;
            readonly int selectionLength;
            readonly string text;

            public void Dispose()
            {
                textBox.Text = text;
                textBox.Select(selectionStart, selectionLength);
            }

            public TextBoxStatePreserver(TextBox textBox)
            {
                this.textBox = textBox;
                selectionStart = textBox.SelectionStart;
                selectionLength = textBox.SelectionLength;
                text = textBox.Text;
            }
        }

        static int CountWithMax<X>(IEnumerable<X> xs, Func<X, bool> predicate, int maxCount)
        {
            var count = 0;
            foreach (var x in xs)
            {
                if (predicate(x))
                {
                    count++;
                    if (count > maxCount) return count;
                }
            }
            return count;
        }

        void Unselect()
        {
            var textBox = EditableTextBox;
            textBox.Select(textBox.SelectionStart + textBox.SelectionLength, 0);
        }

        void UpdateFilter(Func<object, bool> filter)
        {
            using (new TextBoxStatePreserver(EditableTextBox))
            using (Items.DeferRefresh())
            {
                Items.Filter = item => filter(item);
            }
        }

        void OpenDropDown(Func<object, bool> filter)
        {
            UpdateFilter(filter);
            IsDropDownOpen = true;
            Unselect();
        }

        void OnTextChangedCore()
        {
            var text = Text;
            if (string.IsNullOrEmpty(text))
            {
                IsDropDownOpen = false;
                SelectedItem = null;

                using (Items.DeferRefresh())
                {
                    Items.Filter = null;
                }
            }
            else if (SelectedItem != null && TextFromItem(SelectedItem) == text)
            {
                //...
            }
            else
            {
                using (new TextBoxStatePreserver(EditableTextBox))
                {
                    SelectedItem = null;
                }

                var setting = SettingOrDefault;
                var filter = setting.GetFilter(text, TextFromItem);

                //var maxCount = setting.MaxSuggestionCount;
                //var count = CountWithMax(ItemsSource.Cast<object>(), filter, maxCount);
                //if (count > maxCount) return;

                OpenDropDown(filter);
            }
        }

        void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var id = unchecked(++revisionId);
            var setting = SettingOrDefault;

            if (setting.Delay <= TimeSpan.Zero)
            {
                OnTextChangedCore();
                return;
            }

            disposable.Content =
                new Timer(
                    state =>
                    {
                        Dispatcher.InvokeAsync(() =>
                        {
                            if (revisionId != id) return;
                            OnTextChangedCore();
                        });
                    },
                    null,
                    setting.Delay,
                    Timeout.InfiniteTimeSpan
                );
        }
        #endregion

        void ComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && e.Key == Key.Space)
            {
                // 输入空格式触发过滤，且过滤是忽略空格
                var setting = SettingOrDefault;
                var filter = setting.GetFilter(Text, TextFromItem);
                OpenDropDown(filter);
                e.Handled = true;
            }
        }

        public CompleteComboBox()
        {
            InitializeComponent();

            AddHandler(
                TextBoxBase.TextChangedEvent,
                new TextChangedEventHandler(OnTextChanged)
            );
        }

        protected override void OnDropDownOpened(EventArgs e)
        {
            base.OnDropDownOpened(e);
            if (SelectedItem != null)
            {
                // 添加过滤的目的是：为了解决当输入的Text内容是下拉列表项时，当鼠标点击显示下列列表内容时，显示所有Items。
                UpdateFilter(x => true);
            }
            else
            {
                var setting = SettingOrDefault;
                var filter = setting.GetFilter(Text, TextFromItem);
                OpenDropDown(filter);
            }
        }

        protected override void OnDropDownClosed(EventArgs e)
        {
            if (Text == null)
                return;
            if (string.IsNullOrEmpty(Text.Trim()))
                return;
            if (SelectedItem == null || TextFromItem(SelectedItem) != Text)
            {
                var setting = SettingOrDefault;
                var filter = setting.Find(Text, TextFromItem);

                foreach (var item in ItemsSource)
                {
                    var find = filter(item);
                    if (find != null)
                    {
                        // 解决手动输入下拉列表项时，下单提示没有选中选项
                        SelectedItem = find;
                        return;
                    }
                }
            }

            // 修复当筛选数据后，给SelectedItem赋值不成功的问题（因赋值的Value不在筛选后的列表中）。
            var def = SettingOrDefault;
            if (def != null)
                UpdateFilter(def.GetFilter(null, TextFromItem));
        }
    }
}

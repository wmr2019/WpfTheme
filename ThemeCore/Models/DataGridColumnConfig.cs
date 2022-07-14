﻿using System.ComponentModel;

namespace ThemeCore.Models
{
    public class DataGridColumnConfig : INotifyPropertyChanged
    {
        public string Name
        {
            get => _name;
            set => SetProperty(nameof(Name), ref _name, value);
        }
        private string _name;

        #region 隐藏
        /// <summary>
        /// 获取或设置是否显示列
        /// </summary>
        public bool Visible
        {
            get => _visible;
            set => SetProperty(nameof(Visible), ref _visible, value);
        }
        private bool _visible = true;

        public bool IsEnable
        {
            get => _isEnable;
            set => SetProperty(nameof(IsEnable), ref _isEnable, value);
        }
        private bool _isEnable = true;

        /// <summary>
        /// 标识该列默认是隐藏的
        /// 某些列默认是隐藏的,所以列在"恢复到默认"时,需要隐藏该列
        /// </summary>
        public bool IsDefaultHide { get; set; }

        public void SetDefault()
        {
            Visible = !IsDefaultHide;
        }
        #endregion

        #region 列显示顺序
        public int DisplayIndex { get; set; } = int.MinValue;
        #endregion

        #region PropertyChanged
        private void SetProperty<T>(string property, ref T curr, T newValue)
        {
            curr = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public override bool Equals(object obj)
        {
            if (obj is DataGridColumnConfig other)
                return Name == other.Name;
            return false;
        }

        public override int GetHashCode()
        {
            return Name == null ? base.GetHashCode() : Name.GetHashCode();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}

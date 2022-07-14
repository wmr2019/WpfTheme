/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：ThemeWindow.Models
*   文件名称 ：Account.cs
*   =================================
*   创 建 者 ：mingrui.wu
*   创建日期 ：7/14/2022 9:30:33 AM 
*   功能描述 ：
*   使用说明 ：
*   =================================
*   修  改 者 ：
*   修改日期 ：
*   修改内容 ：
*   =================================
*  
***************************************************************************/
using WTLib.Mvvm;

namespace ThemeWindow.Models
{
    public class Account : ObservableObject
    {
        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        private string _id = null;

        public string Code
        {
            get => _code;
            set => SetProperty(ref _code, value);
        }
        private string _code = null;
    }
}

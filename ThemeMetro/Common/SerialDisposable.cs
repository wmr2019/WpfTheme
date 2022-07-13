/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：ThemeMetro.Common
*   文件名称 ：SerialDisposable.cs
*   =================================
*   创 建 者 ：mingrui.wu
*   创建日期 ：7/12/2022 3:57:07 PM 
*   功能描述 ：
*   使用说明 ：
*   =================================
*   修 改 者 ：
*   修改日期 ：
*   修改内容 ：
*   =================================
*  
***************************************************************************/
using System;

namespace ThemeMetro.Common
{
    public sealed class SerialDisposable : IDisposable
    {
        IDisposable _content;

        public IDisposable Content
        {
            get { return _content; }
            set
            {
                if (_content != null)
                {
                    _content.Dispose();
                }

                _content = value;
            }
        }

        public void Dispose()
        {
            Content = null;
        }
    }
}

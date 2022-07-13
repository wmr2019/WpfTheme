/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：ThemeMetro.Common
*   文件名称 ：InputProvider.cs
*   =================================
*   创 建 者 ：mingrui.wu
*   创建日期 ：7/12/2022 3:56:06 PM 
*   功能描述 ：
*   使用说明 ：
*   =================================
*   修 改 者 ：
*   修改日期 ：
*   修改内容 ：
*   =================================
*  
***************************************************************************/
using System.Windows.Input;

namespace ThemeMetro.Common
{
    public class InputProvider
    {
        public static string GetStringNumber(Key key)
        {
            switch (key)
            {
                case Key.D0: return "0";
                case Key.NumPad0: return "0";
                case Key.D1: return "1";
                case Key.NumPad1: return "1";
                case Key.D2: return "2";
                case Key.NumPad2: return "2";
                case Key.D3: return "3";
                case Key.NumPad3: return "3";
                case Key.D4: return "4";
                case Key.NumPad4: return "4";
                case Key.D5: return "5";
                case Key.NumPad5: return "5";
                case Key.D6: return "6";
                case Key.NumPad6: return "6";
                case Key.D7: return "7";
                case Key.NumPad7: return "7";
                case Key.D8: return "8";
                case Key.NumPad8: return "8";
                case Key.D9: return "9";
                case Key.NumPad9: return "9";
            }

            return "";
        }

        public static int GetNumber(Key key)
        {
            switch (key)
            {
                case Key.D0: return 0;
                case Key.NumPad0: return 0;
                case Key.D1: return 1;
                case Key.NumPad1: return 1;
                case Key.D2: return 2;
                case Key.NumPad2: return 2;
                case Key.D3: return 3;
                case Key.NumPad3: return 3;
                case Key.D4: return 4;
                case Key.NumPad4: return 4;
                case Key.D5: return 5;
                case Key.NumPad5: return 5;
                case Key.D6: return 6;
                case Key.NumPad6: return 6;
                case Key.D7: return 7;
                case Key.NumPad7: return 7;
                case Key.D8: return 8;
                case Key.NumPad8: return 8;
                case Key.D9: return 9;
                case Key.NumPad9: return 9;
            }

            return 0;
        }
    }
}

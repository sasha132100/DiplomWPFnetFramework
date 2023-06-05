using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiplomWPFnetFramework.DataBase;

namespace DiplomWPFnetFramework.Classes
{
    static class SystemContext
    {
        public static Users User { get; set; } = null;
        public static bool isGuest { get; set; }
        public static Items Item { get; set; } = null;
        public static bool isChange { get; set; }
        public static Items NewItem { get; set; } = null;
        public static Photo Photo { get; set; } = null;
        public static bool isFromFolder { get; set; }
        public static bool isChangeTitleName { get; set; }
        public static Items Folder { get; set; } = null;
        public static Items SelectedItem { get; set; } = null;
        public static string WindowType { get; set; } = "";
        public static string FromWhichWindowIsCalled { get; set; } = "";
    }
}

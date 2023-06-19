using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DiplomWPFnetFramework.DataBase;

namespace DiplomWPFnetFramework.Classes
{
    static class SystemContext
    {
        public static User User { get; set; } = null;
        public static bool isGuest { get; set; }
        public static Item Item { get; set; } = null;
        public static bool isChange { get; set; }
        public static Item NewItem { get; set; } = null;
        public static Photo Photo { get; set; } = null;
        public static bool isFromFolder { get; set; }
        public static bool isChangeTitleName { get; set; }
        public static Item Folder { get; set; } = null;
        public static Item SelectedItem { get; set; } = null;
        public static string WindowType { get; set; } = "";
        public static string FromWhichWindowIsCalled { get; set; } = "";
        public static bool isFolderNeedToShow { get; set; } = true;
        public static bool isCollectionNeedToShow { get; set; } = true;
        public static bool isCreditCardNeedToShow { get; set; } = true;
        public static bool isDocumentNeedToShow { get; set; } = true;
        public static Page PageForLoadContent { get; set; } = null;
        public static bool isFromHiddenFiles { get; set; }
        public static TemplateObject TemplateObject { get; set; } = null;
        public static string ObjectType { get; set; } = "";
        public static string TemplateObjectTitle { get; set; } = "";
        public static Template Template { get; set; } = null;
        public static bool isSystemStart { get; set; } = true;
        public static Window loginWindow { get; set; } = null;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DiplomWPFnetFramework.Classes
{
    public static class SystemContextService
    {
        public static void MakeAllElementsShowable()
        {
            SystemContext.isCollectionNeedToShow = true;
            SystemContext.isDocumentNeedToShow = true;
            SystemContext.isCreditCardNeedToShow = true;
            SystemContext.isFolderNeedToShow = true;
        }
    }
}

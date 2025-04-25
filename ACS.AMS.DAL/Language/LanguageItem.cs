using ACS.AMS.DAL.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.AMS.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public class LanguageItem
    {
        /// <summary>
        /// 
        /// </summary>
        public LanguageItem(LanguageContentTable content)
        {

            foreach (var c in content.LanguageContentLineItemTable)
                lineItems.Add(c.LanguageID, c.LanguageContent);
        }

        private SortedDictionary<int, string> lineItems = new SortedDictionary<int, string>();

        public SortedDictionary<int, string> LineItems
        {
            get
            {
                return lineItems;
            }
        }
    }
}
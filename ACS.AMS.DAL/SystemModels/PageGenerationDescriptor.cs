using ACS.WebAppPageGenerator.Models.SystemModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACS.AMS.DAL;

namespace ACS.WebAppPageGenerator.Models.SystemModels
{
    public class PageGenerationDescriptor
    {
        public static BasePageModel GetPage(string pageName, string subPageName, PageTypes pageType, int userID, BaseEntityObject entityInstance = null)
        {
            //based on page name create the page controls and return

            var entityType = EntityHelper.GetEntityObjectType(pageName, pageType == PageTypes.Index);
            if(entityInstance == null) 
                entityInstance = Activator.CreateInstance(entityType) as BaseEntityObject;

            //create the model based on the page types
            switch (pageType)
            {
                case PageTypes.Index:
                    throw new NotImplementedException();

                case PageTypes.Create:
                    return GetCreatePage(pageName, subPageName, userID, entityType, entityInstance);

                case PageTypes.Edit:
                    throw new NotImplementedException();

                case PageTypes.Details:
                    throw new NotImplementedException();

                default:
                    throw new NotImplementedException();
            }
        }

        private static BasePageModel GetCreatePage(string pageName, string subPageName, int userID, Type entityType, BaseEntityObject entityInstance)
        {
            switch (pageName)
            {
                //case "Asset":
                //    {
                //        EntryPageModel tabPageModel = new EntryPageModel()
                //        {
                //            ObjectType = entityType,
                //            PageTitle = pageName,
                //            PageName = pageName,
                //            SubPageName = subPageName,
                //            EntityInstance = entityInstance
                //        };

                //        //load the required fields into the page
                //        tabPageModel.PageFields = entityInstance.GetCreateScreenControls(subPageName, userID);

                //        return tabPageModel;
                //    }

                default:
                    {
                        EntryPageModel newModel = new EntryPageModel()
                        {
                            ObjectType = entityType,
                            PageTitle = pageName,
                            PageName = pageName,
                            SubPageName = subPageName,
                            EntityInstance = entityInstance
                        };

                        //load the required fields into the page
                        newModel.PageFields = entityInstance.GetCreateScreenControls(subPageName, userID);

                        return newModel;
                    }
            }
        }
    }
}

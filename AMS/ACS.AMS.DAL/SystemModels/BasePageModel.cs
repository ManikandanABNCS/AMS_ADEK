using ACS.AMS.DAL;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace ACS.WebAppPageGenerator.Models.SystemModels
{
    public class BasePageModel
    {
        public int CompanyID { get; set; }

        public Type ObjectType { get; set; }

        public string PageName { get; set; }

        public string SubPageName { get; set; }

        public string PageTitle { get; set; } = "List Page";

        public string ControllerName { get; set; }

        public bool Isdynamic { get; set; } = true;

        public IFormCollection FormCollection { get; set; }

        public BaseEntityObject EntityInstance { get; set; }

        public PageFieldModelCollection PageFields { get; set; } = new PageFieldModelCollection();

        public BasePageModel()
        {
            ObjectType = null;
            EntityInstance = null;
        }

        public BasePageModel(Type objectType)
        {
            ObjectType = objectType;
        }
    }
}
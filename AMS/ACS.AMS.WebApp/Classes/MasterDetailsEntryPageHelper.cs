using ACS.AMS.DAL;
using ACS.AMS.DAL.DBModel;
using ACS.WebAppPageGenerator.Models.SystemModels;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing.Drawing2D;
using System.Reflection;

namespace ACS.AMS.WebApp
{
    public class MasterDetailsEntryPageHelper : EntryPageHelper
    {
        public MasterDetailsEntryPageHelper(EntryPageModel model) : base(model)
        {

        }

        public override void CreatePageControls(IHtmlHelper<BasePageModel> htmlHelper, RazorPage page, bool readOnlyPage = false, bool QuickCreation = false)
        {
            base.CreatePageControls(htmlHelper, page, readOnlyPage, QuickCreation);

            //create details section controls

            //Create Grid for handling items
        }
    }
}
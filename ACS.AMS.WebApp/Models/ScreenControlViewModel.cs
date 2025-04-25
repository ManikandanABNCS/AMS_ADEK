using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACS.AMS.DAL;
using ACS.AMS.DAL.DBModel;

namespace ACS.AMS.WebApp.Models
{
    public class ScreenControlViewModel : MultiColumnViewModel
    {
        public ASelectionControlQueryTable ScreenControlQueryObject { get; private set; }
        //public ScreenControlQueryTable ScreenControlQueryObjects { get; private set; }
        public List<string> DisplayFields { get; private set; }

        public string SelectedItemViewField { get; private set; }
        public string SelectedItemDisplayField { get; private set; }

        /// <summary>
        /// Load the details from DB using control name
        /// </summary>
        /// <param name="controlname"></param>
        /// <param name="controlFieldName"></param>
        /// <param name="valueFieldname"></param>
        public ScreenControlViewModel(string controlName, string controlFieldName, string valueFieldname) : base(controlFieldName, null)
        {
            base.ValueFieldName = valueFieldname;
            //ScreenControlQueryObject = ScreenControlQueryTable.GetScreenControlQuery(null, controlName);

            LoadRequiredDetails();
        }
        public ScreenControlViewModel(ASelectionControlQueryTable controlQueryTable, string controlFieldName) : base(controlFieldName, null)
        {
            base.ValueFieldName = controlQueryTable.ValueFieldName;
            ScreenControlQueryObject = controlQueryTable;

            LoadRequiredDetails();
        }

        public ScreenControlViewModel(ASelectionControlQueryTable controlQueryTable, string controlFieldName, string valueFieldname) : base(controlFieldName, null)
        {
            base.ValueFieldName = valueFieldname;
            ScreenControlQueryObject = controlQueryTable;

            LoadRequiredScreenControlQueryObjectsDetails();
        }

        private void LoadRequiredDetails()
        {
            if (ScreenControlQueryObject != null)
            {
                DisplayFields = ScreenControlQueryObject.DisplayFields.Split(new char[] { ',' }).ToList();
                SelectedItemDisplayField = ScreenControlQueryObject.SelectedItemDisplayField;
                PlaceholderText = "<-- All " + Language.GetString(ScreenControlQueryObject.ControlName) + " -->";

                DataReadActionName = "GetDynamicControlDataForMCombobox";
                DataReadControllerName = "DataService";
                DataReadScriptFunctionName = "";

                RouteValues = new { ScreenControlQueryID = ScreenControlQueryObject.SelectionControlQueryID };
            }
        }
        private void LoadRequiredScreenControlQueryObjectsDetails()
        {
            if (ScreenControlQueryObject != null)
            {
                DisplayFields = ScreenControlQueryObject.DisplayFields.Split(new char[] { ',' }).ToList();
                //SelectedItemViewField = ScreenControlQueryObject.SelectedItemDisplayField;
                SelectedItemDisplayField = ScreenControlQueryObject.SelectedItemDisplayField;
                PlaceholderText = "<-- All " + Language.GetString(ScreenControlQueryObject.ControlName) + " -->";

                DataReadActionName = "GetDynamicControlDataForMCombobox";
                DataReadControllerName = "DataService";
                DataReadScriptFunctionName = "";

                RouteValues = new { ScreenControlQueryID = ScreenControlQueryObject.SelectionControlQueryID };
            }
        }
    }
}
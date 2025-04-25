namespace ACS.WebAppPageGenerator.Models.SystemModels
{
    public class PageFieldModel
    {
        public string FieldName { get; set; }

        public string DisplayLabel { get; set; }

        public bool IsHidden { get; set; }

        public bool IsMandatory { get; set; }

        public object DefaultValue { get; set; }

        public Type PropertyType { get; set; }

        public int StringMaxLength { get; set; } = 100;

        public PageControlTypes ControlType { get; set; } = PageControlTypes.Textbox;

        public string ChangeMethodName { get; set; }
        
        public string ControlName { get; set; }

        public bool ReadOnly { get; set; } = false;

        #region Foreign Key Selection

        public string CascadeFrom { get; set; }
        
        public string DataReadScriptFunctionName { get; set; }

        public bool IsMasterCreation { get; set; } = false;

        #endregion
    }
}

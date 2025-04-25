namespace ACS.AMS.WebApp
{
    public class GridButton
    {
        public string Caption { get; set; }

        public string ButtonID { get; set; }

        public string ScriptFunction { get; set; }

        public static GridButton CreateNewRecordButton(string controllerName, string caption = "",
            string scriptFunction = "", string addRequestString = "")
        {
            if (string.IsNullOrEmpty(scriptFunction))
                scriptFunction = String.Format("loadContentPage(\"/{0}/Create/{1}\")", controllerName, addRequestString);

            if (string.IsNullOrEmpty(caption))
                caption = "Add new " + controllerName;

            return new GridButton() { ScriptFunction = scriptFunction, Caption = caption, ButtonID = "createNewRecordButton" };
        }
    }
}
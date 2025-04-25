using Hi5Soft.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Text;
using System.Windows.Forms;

namespace ACS.AMS.TestApp
{
    public class DummyServiceManager : IServiceManager
    {
        public System.Diagnostics.EventLog EventLog
        {
            get { return null; }
        }

        public string GetAppSettings(string settingName)
        {
            return ConfigurationManager.AppSettings[settingName];
        }

        public string GetConnectionString(string connectionName)
        {
            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }

        public IServiceHandler GetServiceHandler(string serviceName)
        {
            return null;
        }

        public void HandleException(Exception ex)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                var newEx = ex;
                int noOfChars = 50;

                stringBuilder.AppendLine(new String('*', noOfChars));
                stringBuilder.AppendLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

                while (newEx != null)
                {
                    stringBuilder.AppendLine(new String('-', noOfChars));

                    stringBuilder.AppendLine(newEx.Message);
                    stringBuilder.AppendLine(newEx.StackTrace);

                    newEx = newEx.InnerException;
                }

                System.IO.File.AppendAllText(System.IO.Path.Combine(Application.StartupPath, "Error.txt"), stringBuilder.ToString());
            }
            catch(Exception ex1)
            { }
            //System.Windows.Forms.MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
        }
    }
}

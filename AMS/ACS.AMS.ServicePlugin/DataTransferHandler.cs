using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hi5Soft.Services;

namespace ACS.AMS.ServicePlugin
{
    public class DataTransferHandler : Hi5Soft.Services.BaseThreadServicePluginHandler
    {
        public DataTransferHandler()
        {

        }
        public override string ServiceName => "AMSNotificationService";
        public static int ServiceSyncIntervel { get; set; } //= 5;
        public static string ConnectionString { get; set; }
        public static string smtpURL { get; set; }
        public static int smtpPort { get; set; }
        public static string smtpUsername { get; set; }
        public static string smtpPwd { get; set; }
        public static bool ssl { get; set; }
        public override void InitPlugin(IServiceManager serviceManager)
        {

            base.InitPlugin(serviceManager);
        }
        public override bool LoadPlugin()
        {
            ServiceSyncIntervel = int.Parse(base.ServiceManager.GetAppSettings("AgentIntervel"));
            ConnectionString = base.ServiceManager.GetConnectionString("SqlConnection");
            smtpPort = int.Parse(base.ServiceManager.GetAppSettings("smtpPort"));
            smtpURL = base.ServiceManager.GetAppSettings("smtpURL");
            smtpUsername = base.ServiceManager.GetAppSettings("smtpUsername");
            smtpPwd = base.ServiceManager.GetAppSettings("smtpPwd");
            ssl = base.ServiceManager.GetAppSettings("smtpSSL") == "true" ? true : false;
            return base.LoadPlugin();
        }

        protected override void Run()
        {
            while (true)
            {
                try
                {
                    SyncAllData();
                }
                catch (ThreadAbortException ex)
                {
                    return;
                }
                catch (Exception ex)
                {
                    base.ServiceManager.HandleException(ex);
                }

                base.ServiceManager.HandleException(new Exception($"Goto Sleep for {ServiceSyncIntervel} minutes "));
                Thread.Sleep(ServiceSyncIntervel * 1000 * 60);
            }
        }
        public void SyncAllData()
        {

            try
            {
                NotificationInputHandler.GenerateInputData(base.ServiceManager);
            }
            catch (Exception ex)
            {
                base.ServiceManager.HandleException(ex);
            }
            try
            {
                NotificationHandler.SendNotification(smtpPort, smtpUsername, smtpURL, smtpPwd, ssl, base.ServiceManager);
            }
            catch (Exception ex)
            {
                base.ServiceManager.HandleException(ex);
            }
        }
    }
}

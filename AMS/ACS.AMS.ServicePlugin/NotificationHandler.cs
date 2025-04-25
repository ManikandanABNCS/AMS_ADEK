using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.IO;
using System.Reflection;
using System.Net.Mail;
using System.Net;
using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Web;
using Hi5Soft.Services;     

namespace ACS.AMS.ServicePlugin
{
    public class NotificationHandler
    {
        //public NotificationHandler()
        //{

        //}
        public static void SendNotification(int smptport, string username, string url, string smtpPwd, bool ssl, IServiceManager services)
        {
            try
            {
                using (SqlConnection cn = DBHandler.GetConnection())
                {
                    DataTable tbl = DBHandler.GetDataTable(cn, "aprc_GetNotifications", true, null);

                    if (tbl.Rows.Count > 0)
                    {
                        SqlCommand ucmd = new SqlCommand("aprc_GetNotifications_Update", cn);
                        ucmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cn.Open();

                        foreach (DataRow dr in tbl.Rows)
                        {
                            try
                            {
                                var notificationType = dr["NotificationTypeID"] + "";
                                string response = "";

                                switch (notificationType)
                                {
                                    //case "1":
                                    //    {
                                    //        HttpClient httpClient = new HttpClient();
                                    //        string toAddr = dr["ToAddress"] + "";
                                    //        if (!toAddr.StartsWith("+"))
                                    //            toAddr = "+" + toAddr;

                                    //        string content = dr["Content"] + "";
                                    //        content = UrlEncoder.Default.Encode(content);
                                    //        string requestURL = $"https://api.smsglobal.com/http-api.php?action=sendsms&user=tcyflzzq&password=4sSVwPF8&from=WS-Helpdesk&to={toAddr}&text={content}";
                                    //        var res = httpClient.GetAsync(requestURL).Result;

                                    //        response = res.Content.ReadAsStringAsync().Result;
                                    //    }
                                    //    break;

                                    case "2":
                                        {
                                            string toAddr = dr["ToAddress"] + "";
                                            string Subject = dr["Subject"] + "";
                                            string Body = dr["Content"] + "";

                                            string BccAddress = dr["BCCAddress"] + "";
                                            string CCAddress = dr["CCAddress"] + "";
                                            STMPExecute(toAddr, BccAddress, CCAddress, Subject, null, Body, smptport, username, url, smtpPwd, ssl, services);
                                            //await SendGridHelper.Execute(toAddr, BccAddress, CCAddress, Subject, null, Body);

                                            //response = await res.Body.ReadAsStringAsync();
                                        }
                                        break;

                                        //case "4":
                                        //    var message = new FirebaseAdmin.Messaging.Message()
                                        //    {
                                        //        Notification = new Notification
                                        //        {
                                        //            Title = dr["Subject"] + "", // "Test Notification",
                                        //            Body = dr["Content"] + "", //"This is a test notification"
                                        //        },
                                        //        Token = dr["ToAddress"] + ""
                                        //        //"ceGgG_68TAuzv6kJbgKKrQ:APA91bG8IGKZyq7CdUIqcfsAtB_vGNMekilFW3Qv6FefETDdGE6FfZw0kniFHziIaTprjKVPvVMvS5sVTp5HRKLAd3brIHi1OEd_xcPZTON2dnwx-jPDt-kzLs294lXFbtMPA1kegrK1"
                                        //    };

                                        //    // Send the message
                                        //    response = FirebaseMessaging.DefaultInstance.SendAsync(message).Result;

                                        //    break;
                                }

                                ucmd.Parameters.Clear();
                                ucmd.Parameters.AddWithValue("@NotificationDataID", dr["NotificationDataID"]);
                                ucmd.Parameters.AddWithValue("@Response", response);
                                ucmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                services.HandleException(ex);

                                //NotificationServiceManager.Instance.ServiceManager.HandleException(ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                services.HandleException(ex);
                //NotificationServiceManager.Instance.ServiceManager.HandleException(ex);
            }
            finally
            {
                //timer1.Enabled = true;
            }
        }
        public static void STMPExecute(string toAddress, string bCCAddress, string cCAddress, string subject, string plainTextContent, string htmlContent, int port, string userName, string url, string pwd, bool ssl, IServiceManager services)
        {

            MailMessage message = new MailMessage();

            message.From = new MailAddress(userName);
            AddMailAddresses(message.To, toAddress);
            AddMailAddresses(message.CC, cCAddress);
            AddMailAddresses(message.Bcc, bCCAddress);
            message.Subject = subject;
            message.Body = htmlContent;
            // System.Net.Mail.Attachment attachment;
            //attachment = new System.Net.Mail.Attachment(FilePath);
            // message.Attachments.Add(attachment);
            message.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient(url, port);
            smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

            smtp.EnableSsl = ssl;
            smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            if (!string.IsNullOrEmpty(userName))
            {
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential(userName, pwd);

            }
            smtp.Send(message);

        }
        private static void AddMailAddresses(MailAddressCollection addressCollection, string emailIDs)
        {
            if (!string.IsNullOrEmpty(emailIDs))
            {
                emailIDs = emailIDs.Trim();

                var idCollection = emailIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var id in idCollection)
                {
                    string newID = id.Trim();

                    if (!string.IsNullOrEmpty(newID))
                    {
                        addressCollection.Add(newID);
                    }
                }
            }
        }
    }
}

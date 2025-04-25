using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Web;
using Hi5Soft.Services;

namespace ACS.AMS.ServicePlugin
{
    public class NotificationInputHandler
    {
        public static void GenerateInputData(IServiceManager services)
        {
            using (SqlConnection cn = DBHandler.GetConnection())
            {
                cn.Open();
                DataTable tbl = DBHandler.GetDataTable(cn, "aprc_GetNotificationInputs", true, null);

                if (tbl.Rows.Count > 0)
                {
                    foreach (DataRow row in tbl.Rows)
                    {
                        var queryType = row["QueryType"] + "";
                        var query = row["QueryString"] + "";
                        var AllowHtml = row["AllowHtmlContent"] + "";
                        var notificationTypeID = row["NotificationTypeID"] + "";
                        //Load the Input Data from database
                        SqlCommand dataPullCmd = new SqlCommand(query, cn);
                        var reminderCount = row["ReminderMailCount"] + "";

                        if (string.Compare(queryType, "View", true) == 0)
                        {
                            string whereCon = "";
                            if (row.IsNull("SYSDataID1") == false)
                            {
                                whereCon += " SYSDataID1 = @SYSDataID1 ";
                                dataPullCmd.Parameters.AddWithValue("@SYSDataID1", row["SYSDataID1"]);
                            }

                            if (row.IsNull("SYSDataID2") == false)
                            {
                                dataPullCmd.Parameters.AddWithValue("@SYSDataID2", row["SYSDataID2"]);
                                whereCon += (string.IsNullOrEmpty(whereCon) ? "" : " AND ") + " SYSDataID2 = @SYSDataID2 ";
                            }

                            if (row.IsNull("SYSDataID3") == false)
                            {
                                dataPullCmd.Parameters.AddWithValue("@SYSDataID3", row["SYSDataID3"]);
                                whereCon += (string.IsNullOrEmpty(whereCon) ? "" : " AND ") + " SYSDataID3 = @SYSDataID3 ";
                            }
                            if (row.IsNull("SYSUserID") == false)
                            {
                                dataPullCmd.Parameters.AddWithValue("@SYSUserID", row["SYSUserID"]);
                                whereCon += (string.IsNullOrEmpty(whereCon) ? "" : " AND ") + " SYSUserID = @SYSUserID ";
                            }

                            dataPullCmd.CommandText = $"SELECT * FROM {query} WHERE {whereCon}";
                            dataPullCmd.CommandType = System.Data.CommandType.Text;
                        }
                        else
                        {
                            dataPullCmd.CommandType = System.Data.CommandType.StoredProcedure;

                            dataPullCmd.Parameters.AddWithValue("@SYSDataID1", row["SYSDataID1"]);
                            dataPullCmd.Parameters.AddWithValue("@SYSDataID2", row["SYSDataID2"]);
                            dataPullCmd.Parameters.AddWithValue("@SYSDataID3", row["SYSDataID3"]);
                            dataPullCmd.Parameters.AddWithValue("@SYSUserID", row["SYSUserID"]);
                        }
                        services.HandleException(new Exception($"Query - {dataPullCmd.CommandText} "));
                        //NotificationServiceManager.Instance.ServiceManager.HandleException(new Exception($"Query - {dataPullCmd.CommandText} "));

                        SqlDataAdapter dataAdapter = new SqlDataAdapter(dataPullCmd);
                        DataTable currentDt = new DataTable();
                        dataAdapter.Fill(currentDt);

                        //Create the message 
                        if (currentDt.Rows.Count > 0)
                        {
                            //prepare header, footer
                            StringBuilder header = new StringBuilder(row["TemplateHeaderBodyContent"] + "");
                            StringBuilder footer = new StringBuilder(row["TemplateFooterBodyContent"] + "");
                            StringBuilder subjectData = new StringBuilder(row["TemplateSubject"] + "");
                            string detailsheader = row["TemplateTableBodyContentHeader"] + "";
                            string details = row["TemplateTableBodyContent"] + "";
                            string detailsfooter = row["TemplateTableBodyContentFooter"] + "";
                            foreach (DataColumn col in currentDt.Columns)
                            {
                                header = header.Replace($"##{col.ColumnName}##", currentDt.Rows[0][col] + "");
                                footer = footer.Replace($"##{col.ColumnName}##", currentDt.Rows[0][col] + "");
                                subjectData = subjectData.Replace($"##{col.ColumnName}##", currentDt.Rows[0][col] + "");
                            }

                            StringBuilder detailsList = new StringBuilder((details.Length + 50) * currentDt.Rows.Count);

                            foreach (DataRow msgRow in currentDt.Rows)
                            {
                                string newdetails = details;
                                foreach (DataColumn col in currentDt.Columns)
                                {
                                    newdetails = newdetails.Replace($"##{col.ColumnName.ToUpper()}##", msgRow[col] + "");
                                }

                                detailsList.Append(newdetails);
                            }

                            //Save the message into Notification Data
                            string toAddress = currentDt.Rows[0]["SYSToAddresses"] + "";
                            string ccAddress = currentDt.Rows[0]["SYSCCAddresses"] + "";
                            string bccAddress = currentDt.Rows[0]["SYSBCCAddresses"] + "";

                            if (row.IsNull("ToAddress") == false)
                                toAddress += ";" + row["ToAddress"];
                            if (row.IsNull("CCAddress") == false)
                                ccAddress += ";" + row["CCAddress"];
                            if (row.IsNull("BCCAddress") == false)
                                bccAddress += ";" + row["BCCAddress"];

                            string subject = subjectData.ToString();
                            if(string.Compare(reminderCount,"0")!=0)
                            {
                                subject = string.Concat(subjectData.ToString(), "  Reminder-", reminderCount);
                            }
                            string Content = header.ToString() + detailsheader + detailsList.ToString() + detailsfooter + footer.ToString();
                            SqlCommand insertMsgCmd = new SqlCommand("prc_InsertNotificationData", cn);
                            insertMsgCmd.CommandType = System.Data.CommandType.StoredProcedure;
                            insertMsgCmd.Parameters.AddWithValue("@ToAddress", toAddress);
                            insertMsgCmd.Parameters.AddWithValue("@CCAddress", ccAddress);
                            insertMsgCmd.Parameters.AddWithValue("@BCCAddress", bccAddress);
                            insertMsgCmd.Parameters.AddWithValue("@Content", Content);
                            insertMsgCmd.Parameters.AddWithValue("@Subject", subject); //subjectData.ToString());
                            insertMsgCmd.Parameters.AddWithValue("@NotificationTypeID", int.Parse(notificationTypeID));
                            insertMsgCmd.ExecuteNonQuery();
                        }

                        SqlCommand ucmd = new SqlCommand("aprc_GetNotificationInputs_Update", cn);
                        ucmd.CommandType = System.Data.CommandType.StoredProcedure;
                        ucmd.Parameters.Clear();
                        ucmd.Parameters.AddWithValue("@NotificationInputID", row["NotificationInputID"]);
                        ucmd.Parameters.AddWithValue("@Response", "");
                        ucmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}

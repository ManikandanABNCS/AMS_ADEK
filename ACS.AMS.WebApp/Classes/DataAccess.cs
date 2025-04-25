using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using ACS.AMS.DAL.DBContext;

namespace ACS.AMS.WebApp
{
    public class DataAccess
    {
        SqlConnection con;
        SqlCommand sqlComm;

        public SqlTransaction myTrans;
        bool IsOpen = false;
        
        public DataAccess()
        {
            con = new SqlConnection();
            con.ConnectionString = GetConnectionString();
        }


        #region ExecuteQuery

        public DataSet ExecuteQuery(string storeProcName, IList<SqlParameter> parameter)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = storeProcName;
            foreach (SqlParameter Param in parameter)
            {
                cmd.Parameters.Add(Param);
            }
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = cmd;
            sda.Fill(ds);
            return ds;
        }
       
        #endregion

        #region Data Table

        public SqlCommand CreateCommand()
        {
            return con.CreateCommand();
        }

        public DataTable GetDateTable(SqlCommand cmd)
        {
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            
            DataTable tbl = new DataTable();
            ad.Fill(tbl);

            return tbl;
        }

        #endregion

        #region ExecuteNonQuery

        public int ExecuteNonQuery(string storeProcName, IList<SqlParameter> parameter)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = storeProcName;
            foreach (SqlParameter Param in parameter)
            {
                cmd.Parameters.Add(Param);
            }
            int count = cmd.ExecuteNonQuery();
            con.Close();
            return count;
        }

        #endregion

        #region GetConnection

        public string GetConnectionString()
        {
            string conStr = string.Empty;

            if (System.Configuration.ConfigurationManager.ConnectionStrings["AMSConnection"] != null)
                conStr = System.Configuration.ConfigurationManager.ConnectionStrings["AMSConnection"].ToString();
            
            return conStr;
        }
        
        #endregion

        #region connectionOpenTransaction
        public void connectionOpenTransaction()
        {
            sqlComm = new SqlCommand();
            con.Open();
            sqlComm.Connection = con;
            sqlComm.CommandTimeout = 180;
            myTrans = con.BeginTransaction(IsolationLevel.ReadUncommitted);
        }
        #endregion

        #region CommitTransaction
        public void CommitTransaction()
        {
            myTrans.Commit();
            con.Close();
        }
        #endregion

        #region RollbackTransaction
        public void RollbackTransaction()
        {
            myTrans.Rollback();
            con.Close();
        }
        #endregion

        #region ExcuteNonQueryTransaction

        public int ExcuteNonQueryTransaction(string storedProcName, IList<SqlParameter> parameters)
        {
            sqlComm.Parameters.Clear();
            sqlComm.Transaction = myTrans;
            sqlComm.CommandText = storedProcName;
            sqlComm.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter param in parameters)
            {
                sqlComm.Parameters.Add(param);
            }
            int cnt = sqlComm.ExecuteNonQuery();
            return cnt;
        }

        public DataSet ExcuteQueryTransaction(string storedProcName, IList<SqlParameter> parameters)
        {
            sqlComm.Parameters.Clear();
            sqlComm.Transaction = myTrans;
            sqlComm.CommandText = storedProcName;
            sqlComm.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter param in parameters)
            {
                sqlComm.Parameters.Add(param);
            }

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet dsResult = new DataSet();
            da.SelectCommand = sqlComm;
            da.Fill(dsResult);
            return dsResult;

        }
        #endregion

    }
}
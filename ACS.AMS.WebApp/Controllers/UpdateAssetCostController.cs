using ACS.AMS.DAL;
using ACS.AMS.DAL.DBModel;
using ACS.AMS.DAL.DBContext;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACS.AMS.WebApp.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using NPOI.POIFS.FileSystem;
using ACS.WebAppPageGenerator.Models.SystemModels;
using System.Net.Http.Headers;
using ACS.AMS.WebApp.Classes;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using Microsoft.EntityFrameworkCore;
using MathNet.Numerics;
using Telerik.SvgIcons;
using System.Net;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Data.SqlClient;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Org.BouncyCastle.Utilities;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using static SQLite.SQLite3;

namespace ACS.AMS.WebApp.Controllers
{
    public class UpdateAssetCostController : ACSBaseController
    {
        public IActionResult Index()
        {
            if (!base.HasRights(RightNames.UpdateAssetCost, UserRightValue.View))
                return RedirectToAction("UnauthorizedPage");
            IndexPageModel indexPage = new IndexPageModel();
            indexPage.EntityInstance = new BaseEntityObject();
            indexPage.EntityInstance.CurrentPageID = SessionUser.Current.GetNextPageID();
            UpdateAssetCostDataModel.RemoveModel(indexPage.EntityInstance.CurrentPageID);

            base.TraceLog("UpdateAssetCost Index", $"{SessionUser.Current.Username} -UpdateAssetCost Index page requested");
            return PartialView(indexPage);
            //return PartialView();

        }
        protected virtual IndexPageModel GetEntryPageModel( int currentPageID)
        {
            IndexPageModel indexPage = new IndexPageModel();
            indexPage.EntityInstance = new BaseEntityObject();
            indexPage.EntityInstance.CurrentPageID = currentPageID;
            
            return indexPage;
        }
        public IActionResult _Index([DataSourceRequest] DataSourceRequest request, string PoNumber, string InvoiceNo, string polineNo,int currentPageID)
        {
            UpdateAssetCostDataModel model = UpdateAssetCostDataModel.GetModel(currentPageID);
            if (model.LineItems.Count == 0)
            {
                return Json(GetOrderItem(request, PoNumber, InvoiceNo, polineNo,currentPageID,model));
            }
            else
            {
                var dsResult = request.ToDataSourceResult(model.LineItems.AsQueryable());
                this.TraceLog("GenerateAssetFromPO Index", $"{SessionUser.Current.Username} - GenerateAssetFromPO Index page Data Fetch");

                return Json(dsResult);

            }
        }
       

        private object GetOrderItem([DataSourceRequest] DataSourceRequest request, string PoNumber, string InvoiceNo, string polineNo,int currentPageID,UpdateAssetCostDataModel data)
        {
            var query = AssetNewView.GetAssetListAgainstPO(_db, PoNumber, InvoiceNo, polineNo);
            var datas = (from b in query
                         select new UpdateAssetCostModel
                         {
                             AssetID = b.AssetID,
                             Barcode = b.Barcode,
                             AssetDesc = b.AssetDescription,
                             CategoryName = b.CategoryHierarchy,
                             CurrentCost = b.PurchasePrice,
                             //PurchasePrice = b.PurchasePrice,
                             PONumber = b.PONumber,
                             InvoiceNo = b.InvoiceNo,
                             DOFPO_LINE_NUM = b.DOFPO_LINE_NUM,
                             ID = b.AssetID,
                             AdditionalCost=0
                         }
                       );

            foreach (var lst in datas)
            {
                data.LineItems.Add(lst);
            }
            
            return datas.ToDataSourceResult(request);
        }

        [HttpPost]
        public IActionResult _update([DataSourceRequest] DataSourceRequest request, UpdateAssetCostModel product, int currentPageID)
        {
            //UpdateAssetCostModel product=new UpdateAssetCostModel();
            if (product != null && ModelState.IsValid)
            {
                UpdateAssetCostDataModel model = UpdateAssetCostDataModel.GetModel(currentPageID);
                var data = model.LineItems.Where(a => a.Barcode == product.Barcode).FirstOrDefault();
                data.AdditionalCost=product.AdditionalCost;
              
                //UpdateAssetCostModel.a(product);
            }

            return Json(new[] { product }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult Index(IFormCollection data)
        {
            if (!base.HasRights(RightNames.UpdateAssetCost, UserRightValue.Edit))
                return GotoUnauthorizedPage();

            try
            {
                int currentPageID = int.Parse(data["CurrentPageID"] + "");

                string orderNo = string.Empty;
                base.TraceLog("GenerateAssetFromPO Index-post", $"{SessionUser.Current.Username} -Index-Post request ");

                UpdateAssetCostDataModel model = UpdateAssetCostDataModel.GetModel(currentPageID);
                if (model.LineItems.Count > 0)
                {
                    var ponumber = data["PONumber"];
                    string invoiceNo = data["InvoiceNo"] == "-" ? string.Empty : data["InvoiceNo"];
                    string polineNo = data["POLineNo"] == "-" ? string.Empty : data["POLineNo"];
                    var massID = data["MassAdditionID"];
                    var remarks = data["Remarks"];
                    List<string> barcodeList = model.LineItems.Select(a => a.AssetID + "").ToList();
                    List<string> costList = model.LineItems.Select(a => a.AdditionalCost + "").ToList();

                    var BarcodeList = System.String.Join(",", barcodeList);
                    var CostResult = System.String.Join(",", costList);

  
                    var result = SaveData(_db, "prc_UpdateAssetCost", ponumber, BarcodeList, CostResult, SessionUser.Current.UserID, massID, invoiceNo, polineNo, remarks);

                    if (!string.IsNullOrEmpty(result))
                    {

                        return base.ErrorActionResult(result);
                    }

                    UpdateAssetCostDataModel.RemoveModel(currentPageID);

                    return PartialView("SuccessAction");
                }
                else
                {
                    return base.ErrorActionResult("Please enter additional cost for any asset");
                }

            }
            catch (ValidationException ex)
            {
                ViewData["FocusControl"] = ex.FieldName;
                ModelState.AddModelError("12", ex.Message);
            }
            catch (Exception ex)
            {

                return base.ErrorActionResult(ex);
            }
            return PartialView(data);
        }
        public static string SaveData(AMSContext _db, string procedureName, string ponumber, string barcode, string additionalCost, int userID,
                        string MassAdditional, string? InvoiceNo = null, string? POLineNo = null, string? Remarks = null)

        {
            int checkCnt = 0;
            string resultStatus = string.Empty;
            string duplicateValues = string.Empty;

            //data import
            using (SqlConnection cn = new SqlConnection(AMSContext.ConnectionString))
            {
                DataSet dataSet = new DataSet();
                DataTable dataTable = new DataTable();
                cn.Open();
                //var trans = cn.BeginTransaction();
                bool transactionSuccess = true;

                try
                {
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText = procedureName;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //  cmd.Transaction = trans;
                    DataTable dt = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                    cmd.Parameters.Clear();

                    cmd.Parameters.AddWithValue("@PONumber", ponumber);
                    cmd.Parameters.AddWithValue("@InvoiceNo", InvoiceNo);
                    cmd.Parameters.AddWithValue("@POLineNo", POLineNo);
                    cmd.Parameters.AddWithValue("@MassAdditional", MassAdditional);
                    cmd.Parameters.AddWithValue("@Remarks", Remarks);
                    cmd.Parameters.AddWithValue("@Barcode", barcode);
                    cmd.Parameters.AddWithValue("@AdditionalCost", additionalCost);
                    cmd.Parameters.AddWithValue("@UserID", SessionUser.Current.UserID);

                    try
                    {
                        dt.Clear();
                        adapter.Fill(dt);
                    }
                    catch (Exception ex)
                    {
                        var errorID = ApplicationErrorLogTable.SaveException(ex, "Update asset Cost Upload-Web, At record: " + checkCnt);
                        transactionSuccess = false;
                        //    trans.Rollback();
                        resultStatus = resultStatus + $"Error ID: {errorID}" + Environment.NewLine;
                        //return ex.Message;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow newDataRow in dt.Rows)
                        {
                            var err = newDataRow[0] + "";
                            if (!string.IsNullOrWhiteSpace(err))
                            {
                                resultStatus = resultStatus + err + Environment.NewLine;

                                transactionSuccess = false;
                                // trans.Rollback();
                                break;
                            }
                            else
                            {
                            }
                        }
                    }

                    //if (transactionSuccess == false) break;


                    if (transactionSuccess)
                    {
                        //  trans.Commit();
                        // trans.Dispose();

                    }
                }
                catch (Exception ex)
                {
                    //sqlTrans.Rollback();
                    ApplicationErrorLogTable.SaveException(ex, "Update asset Cost" + "Web");
                    return ex.Message;
                }
                finally
                {
                    cn.Dispose();
                    cn.Close();
                }


                return resultStatus;
            }
        }
       
    }
}
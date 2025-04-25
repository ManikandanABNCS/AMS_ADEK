using ACS.AMS.WebApp.Products.Mobile;
using ACS.LMS.WebApp.Models.Mobile;
using System.Collections.Generic;

namespace ACS.AMS.WebApp.Models.Mobile
{
    public class DataProcessDataResponse : BaseResponse
    {
       
    }
    public class LocationDataProcessData : BaseResponse
    {
        public LocationData Location { get; set; }
    }
    public class CategoryDataProcessData : BaseResponse
    {
        public CategoryData Category { get; set; }
    }
    public class AssetConditionDataProcessData : BaseResponse
    {
        public AssetConditionData AssetCondition { get; set; }
    }
    public class ApprovalRoleDataProcessData : BaseResponse
    {
        public ApprovalRoleData ApprovalRole { get; set; }
    }
    public class ManufacturerDataProcessData : BaseResponse
    {
        public ManufacturerData manufacturer { get; set; }
    }
    public class ModelDataProcessData : BaseResponse
    {
        public ModelData model { get; set; }
    }
    public class DapartmentDataProcessData : BaseResponse
    {
        public DepartmentData department { get; set; }
    }
    public class SectionDataProcessData : BaseResponse
    {
        public SectionData section { get; set; }
    }
    public class SupplierDataProcessData : BaseResponse
    {
        public SupplierData supplier { get; set; }
    }
    public class ProductDataProcessData : BaseResponse
    {
        public ProductData Product { get; set; }
    }
    public class CustodianDataProcessData : BaseResponse
    {
        public CustodianData Custodian { get; set; }
    }
    public class AssetDataProcessData : BaseResponse
    {
        public AssetData AssetData { get; set; }
    }
    public class AssetDisposalDataProcessData : BaseResponse
    {
        public List<DisposalAssetData> DisposalData { get; set; }
    }
    //public class UpdateAssetDataProcessData : BaseResponse
    //{
    //    public UpdateAssetDate AssetUpdate { get; set; }
    //}
}

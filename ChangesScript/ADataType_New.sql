if not exists(select FieldTypeCode from AFieldTypeTable where FieldTypeCode='13')
begin 
insert into AFieldTypeTable(FieldTypeCode,FieldTypeDesc)
values('13','DefaultParamter')
end 
go 
alter table screenFilterLineItemTable drop constraint FK__ScreenFil__Field__44D52468
go 
alter table screenFilterLineItemTable add constraint FK_screenFilterLineItemTable_FieldTypeID FOREIGN KEY (FieldTypeID)
    REFERENCES AFieldTypeTable(FieldTypeID); 
	go 
	ALTER Procedure [dbo].[rprc_AssetSummary]
(
	@LanguageID int=1,
	@UserId int =1,
	@CompanyID nvarchar(max)=1003,
	@ClassificationID int =NULL,
	@Query nvarchar(max)=null
)
as 
Begin 

--SET @LanguageID = 1;
--SET	@UserId  =1;
--SET	@CompanyID =1003;
--SET @ClassificationID  =NULL;
--SET	@Query=null

Declare @DynamicQuery nvarchar(max)
Declare @ParamDefinition AS NVarchar(max) = null
--Test Table 
--Declare @Sample Table (queries nvarchar(max))
--Get Parent Child Count 
Set @DynamicQuery= 'Declare @ParentChildCountTable Table (ParentAssetID int,ChildCount int) ' ;
Set @DynamicQuery=@DynamicQuery+ 'insert into @ParentChildCountTable(ParentAssetID,ChildCount) Select map.ParentAssetID,count(ParentAssetID) From AssetTable A 
join AssetMappingTable Map on A.AssetID=Map.ParentAssetID and Map.statusid=1 Group by map.ParentAssetID  ' ;
								  
-- Department Mapping Details 
 Declare @UserDepartmentMapping nvarchar(10)  
								Select @UserDepartmentMapping= ConfiguarationValue from ConfigurationTable where ConfiguarationName='UserDepartmentMapping'
-- Category Mapping Details 
  Declare @UserCategoryMapping nvarchar(10) 
       Select @UserCategoryMapping= ConfiguarationValue from ConfigurationTable where ConfiguarationName='UserCategoryMapping' ;	
							  
Declare @DepartID as bit 
	Select @DepartID= isnull(IsAllDepartmentMapping,0) from PersonTable where PersonID=@UserID
--Location Mapping Details 
 Declare @UserLocationMapping nvarchar(10)
								  Select @UserLocationMapping= ConfiguarationValue from ConfigurationTable where ConfiguarationName='UserLocationMapping' ;	
		
Set @DynamicQuery=@DynamicQuery+' select A.Barcode, A.AssetCode,A.AssetDescription, A.CategoryName , A.LocationName,a.DepartmentCode,a. DepartmentName,
a.SectionCode,a.SectionDescription,a.CustodianName,A.PurchasePrice,A.MappedAssetID,A.RFIDTagCode,A.PurchaseDate ,A.WarrantyExpiryDate ,a.Model,A.ReferenceCode,A.SerialNo,A.DeliveryNote,
a.AssetConditionCode,a. AssetCondition, a.SupplierCode,a. SupplierName,A.CreatedDateTime,A.Make,A.Capacity,A.PONumber,A.ComissionDate,Parent.ChildCount,
a.ProductCode,a.ProductName,AssetRemarks, case when MappedAssetID=''NULL'' then NULL else MappedAssetID End  as MappedAssetNo , DVT.DepreciationName as DepreciationName,DVT.DepreciationStartDate as DepreciationStartDate,DVT.DepreciationPeriod as DepreciationPeriod,A. Accumulatedvalue ,A.NetValue,
a. Manufacturer,NetworkID as OwnedLeased,InvoiceNo,InvoiceDate,partialDisposalTotalValue,case when CreateFromHHT=0 then ''False'' else ''True'' End as CreateFromHHT,a. CreateBy as CreatedBy,a. ModifedBy,A.LastModifiedDateTime as ModifedDate,DisposalReferenceNo,
DisposedDateTime,DisposedRemarks,DisposalValue,a.CompanyCode,a. CompanyName, a. DepreciationEndDate,
a.FirstLevelCategoryName,a.SecondLevelCategoryName,
a.FirstLevelLocationName,a.SecondLevelLocationName,
A.LocationHierarchy,A.CategoryHierarchy,A.Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,
A.Attribute6,A.Attribute7,A.Attribute8,A.Attribute9,A.Attribute10,A.Attribute11,A.Attribute12,A.Attribute13,A.Attribute14,
A.Attribute15,A.Attribute16,a.CustodianType,a.UploadedImagePath as AssetImage, A.Latitude, A.Longitude,
A.Attribute17,A.Attribute18,Attribute19,Attribute20,Attribute21,Attribute22,Attribute23,Attribute24,Attribute25,Attribute26,
Attribute27,Attribute28,Attribute29,Attribute30,Attribute31,Attribute32,Attribute33,Attribute34,Attribute35,Attribute36,Attribute37,Attribute38,Attribute39,Attribute40
from AssetView A 
left join AssetDepreciationTable DVT on a.AssetID=DVT.AssetID 

Left join @ParentChildCountTable Parent on A.assetID=Parent.ParentAssetID 
where A.StatusID=1 and A.companyID in (Select Value from Split('''+@companyID+''','','')) '
								   if(@ClassificationID=2)
								   Begin
									set @DynamicQuery=@DynamicQuery+ 'and (a.PurchaseDate < ''2014-09-01'')'
								   End 
								   if(@ClassificationID=3)
								   Begin
									set @DynamicQuery=@DynamicQuery+ 'and (a.PurchaseDate>''2014-08-31'')'
								   End 
								    if (upper(@UserDepartmentMapping)='TRUE' and @DepartID=0)
								   begin 
								    set @DynamicQuery=@DynamicQuery+' and (a.DepartmentID in (select departmentID from UserDepartmentMappingTable where PersonID =@UserId and StatusID=1))'
								   End 
								   if upper(@UserCategoryMapping)='TRUE' 
								   begin 
								   set @DynamicQuery=@DynamicQuery+' and (A.CategoryL1 in (select CategoryID from userCategoryMappingTable where personID=@UserId and StatusID=1))'
								   End 
								   if upper(@UserLocationMapping)='TRUE' 
								   begin 
								   set @DynamicQuery=@DynamicQuery+' and (A.LocationL1 in (select LocationID from UserLocationMappingTaable where personID=@UserId and StatusID=1))'
								   End 
if (@Query is not null and @Query<>'' ) 
Begin
	set @DynamicQuery=@DynamicQuery+' and ' +@Query 
	End 
	SET @ParamDefinition = '@LanguageID int,@UserId int'
	
	--set @DynamicQuery=@DynamicQuery+'Order By A.AssetID asc'
	print @DynamicQuery
	--insert into @Sample values (@DynamicQuery)
	--select * from @Sample

	exec sp_executesql @DynamicQuery,@ParamDefinition,@LanguageID ,@UserId
End 
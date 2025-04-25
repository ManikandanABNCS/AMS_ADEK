create table SystemConfigurationTable
(
	SystemconfigurationID int not null primary key identity(1,1),
	IsEnableTigger bit default(0) not null 
)
go 
insert into SystemConfigurationTable(IsEnableTigger) values(1)
go
--ALTER PROCEDURE [dbo].[Zprc_DOF_BulkLocationInsert]
--as 
--begin 
--update Systemconfigurationtable set IsEnableTigger=0 

--INSERT INTO LocationTable(ParentLocationID, LocationCode, CreatedBy)
--select NULL, DOF_CITY,1
--from zDOFLocationTable a 
--left join LocationTable b on a.DOF_CITY=b.LocationCode and b.ParentLocationID is null 
--where b.LocationID is null 
--group by DOF_CITY

--INSERT INTO LocationDescriptionTable(LocationID,LocationDescription,LanguageID,CreatedBy,CreatedDateTime)
-- select b.LocationID,DOF_CITY_DESC,1,1,getdate()
-- from zDOFLocationTable a 
--left join LocationTable b on a.DOF_CITY=b.LocationCode and b.ParentLocationID is null 
--left join LocationDescriptionTable c on b.locationID=C.LocationID 
--where b.LocationID is not null and b.ParentLocationID is null and c.LocationDescriptionID is null
--group by DOF_CITY_DESC,b.LocationID

--update a set 
--a.LocationDescription=c1.DOF_CITY_DESC
--from LocationDescriptionTable a 
--join LocationTable b on a.locationid=b.locationID  and ParentLocationID is null 
--join zDOFLocationTable c1 on b.locationcode=C1.DOF_CITY


--INSERT INTO LocationTable(ParentLocationID, LocationCode, CreatedBy)
--Select c.LocationID,a.DOF_SITE,1
--from zDOFLocationTable a 
--join LocationTable c on  a.DOF_CITY=c.LocationCode and c.parentLocationID is null 
--left join LocationTable b on a.DOF_SITE=b.LocationCode and b.ParentLocationID=c.LocationID
--where b.LocationID is null 
--group by DOF_SITE,c.LocationID

--INSERT INTO LocationDescriptionTable(LocationID,LocationDescription,LanguageID,CreatedBy,CreatedDateTime)
--Select b.LocationID,a.DOF_SITE_DESC,1,1,getdate()
--From zDOFLocationTable a 
--join LocationTable c on  a.DOF_CITY=c.LocationCode and c.parentLocationID is null 
--join LocationTable b on a.DOF_SITE=b.LocationCode and b.ParentLocationID=c.LocationID
--left join LocationDescriptionTable d on b.locationID=d.LocationID 
--where b.LocationID is not null and d.LocationDescriptionID is null and b.ParentLocationID is not null
--group by b.LocationID,a.DOF_SITE_DESC

--update a set 
--a.LocationDescription=c.DOF_SITE_DESC
--from LocationDescriptionTable a 
--join LocationTable b on a.locationid=b.locationID 
--join (select c1.*,d.LocationID
--from zDOFLocationTable c1 
--left join LocationTable c on c1.DOF_CITY=c.LocationCode and c.ParentLocationID is null
--left join LocationTable d on c1.DOF_SITE=d.LocationCode and d.ParentLocationID=c.LocationID
--join LocationDescriptionTable a on d.locationid=a.locationID 
--)c on b.LocationID=c.LocationID


--INSERT INTO LocationTable(ParentLocationID, LocationCode, CreatedBy)
--Select d.LocationID,a.DOF_DEPT,1
--from zDOFLocationTable a 
--join LocationTable c on a.DOF_CITY=c.LocationCode and c.ParentLocationID is null 
--join LocationTable d on a.DOF_SITE=d.LocationCode and d.ParentLocationID=c.LocationID
--left join LocationTable b on a.DOF_DEPT=b.LocationCode and b.ParentLocationID=d.LocationID

----left join LocationTable c on b.ParentLocationID=c.LocationID and a.DOF_SITE=c.LocationCode
----left join LocationTable D on c.ParentLocationID=D.LocationID and a.DOF_CITY=d.LocationCode and d.ParentLocationID is null
--where b.LocationID is null 
--group by DOF_DEPT,d.LocationID

--INSERT INTO LocationDescriptionTable(LocationID,LocationDescription,LanguageID,CreatedBy,CreatedDateTime)
--Select b.LocationID,a.DOF_DEPT_DESC,1,1,getdate()
--from zDOFLocationTable a 
-- join LocationTable c on a.DOF_CITY=c.LocationCode and c.ParentLocationID is null 
-- join LocationTable d on a.DOF_SITE=d.LocationCode and d.ParentLocationID=c.LocationID
-- join LocationTable b on a.DOF_DEPT=b.LocationCode and b.ParentLocationID=d.LocationID
-- --join LocationTable c on b.ParentLocationID=c.LocationID and a.DOF_SITE=c.LocationCode
-- --join LocationTable D on c.ParentLocationID=D.LocationID and a.DOF_CITY=d.LocationCode and d.ParentLocationID is null
-- Left join LocationDescriptionTable e on b.locationID=e.LocationID 
--where  b.LocationID is not null and e.LocationDescriptionID is null and b.ParentLocationID is not null
--group by DOF_DEPT_DESC,b.LocationID

--update a set 
--a.LocationDescription=c.DOF_DEPT_DESC
--from LocationDescriptionTable a 
--join LocationTable b on a.locationid=b.locationID 
--join (select c1.*,e.LocationID
--from zDOFLocationTable c1 
--left join LocationTable c on c1.DOF_CITY=c.LocationCode and c.ParentLocationID is null
--left join LocationTable d on c1.DOF_SITE=d.LocationCode and d.ParentLocationID=c.LocationID
--left join LocationTable e on c1.DOF_DEPT=e.LocationCode and e.ParentLocationID=d.LocationID
--join LocationDescriptionTable a on e.locationid=a.locationID ) c on b.LocationID=c.LocationID
----join zDOFLocationTable c1 on b.locationcode=C1.DOF_DEPT
----left join LocationTable c on c1.DOF_CITY=c.LocationCode and c.ParentLocationID is null 
----left join LocationTable d on c1.DOF_SITE=d.LocationCode and d.ParentLocationID=c.LocationID


--update a set 
--a.parentlocationid=c.locationID,
--a.OracleLocationID=c.LOCATION_ID,
--a.Attribute16='DOF services updated PArent Location and OracleLocationID'
--from LocationTable a 
--join (select c1.*,e.LocationID
--from zDOFLocationTable c1 
-- join LocationTable f on c1.DOF_ROOM=f.LocationCode AND f.ParentLocationID is null and f.OracleLocationID is null
--  join LocationTable c on c1.DOF_CITY=c.LocationCode and c.ParentLocationID is null
-- join LocationTable d on c1.DOF_SITE=d.LocationCode and d.ParentLocationID=c.LocationID
-- join LocationTable e on c1.DOF_DEPT=e.LocationCode and e.ParentLocationID=d.LocationID
--)  c on a.Locationcode=c.DOF_ROOM and a.OracleLocationID is null 

--INSERT INTO LocationTable(ParentLocationID, LocationCode, CreatedBy,OracleLocationID)
--Select e.LocationID,a.DOF_ROOM,1,a.LOCATION_ID
--from zDOFLocationTable a 
--left join LocationTable c on a.DOF_CITY=c.LocationCode and c.ParentLocationID is null 
--left join LocationTable d on a.DOF_SITE=d.LocationCode and d.ParentLocationID=c.LocationID
--left join LocationTable e on a.DOF_DEPT=e.LocationCode and e.ParentLocationID=d.LocationID
--left join LocationTable b on a.LOCATION_ID=b.OracleLocationID and b.ParentLocationID=e.LocationID
----left join LocationTable c on b.ParentLocationID=c.LocationID and a.DOF_DEPT=c.LocationCode
----left join LocationTable D on c.ParentLocationID=D.LocationID and a.DOF_SITE=d.LocationCode
----left join LocationTable e on d.ParentLocationID=e.LocationID and a.DOF_CITY=e.LocationCode and e.ParentLocationID is null
--where  b.LocationID is null 
--group by DOF_ROOM,e.LocationID,a.LOCATION_ID


--INSERT INTO LocationDescriptionTable(LocationID,LocationDescription,LanguageID,CreatedBy,CreatedDateTime)
--Select b.LocationID,a.DOF_ROOM_DESC,1,1,getdate()
--from zDOFLocationTable a 

-- left join LocationTable c on a.DOF_CITY=c.LocationCode and c.ParentLocationID is null 
--left join LocationTable d on a.DOF_SITE=d.LocationCode and d.ParentLocationID=c.LocationID
--left join LocationTable e on a.DOF_DEPT=e.LocationCode and e.ParentLocationID=d.LocationID
-- join LocationTable b on a.LOCATION_ID=b.OracleLocationID and b.ParentLocationID=e.LocationID
-- --join LocationTable c on b.ParentLocationID=c.LocationID and a.DOF_DEPT=c.LocationCode
-- --join LocationTable D on c.ParentLocationID=D.LocationID and a.DOF_SITE=d.LocationCode
-- --join LocationTable e on d.ParentLocationID=e.LocationID and a.DOF_CITY=e.LocationCode and e.ParentLocationID is null
-- Left join LocationDescriptionTable f on b.locationID=f.LocationID 
--where  b.LocationID is not null and f.LocationDescriptionID is null and b.ParentLocationID is not null
--group by DOF_ROOM_desc,b.LocationID


--update a set 
--a.LocationDescription=c1.DOF_ROOM_desc
--from LocationDescriptionTable a 
--join LocationTable b on a.locationid=b.locationID 
--join zDOFLocationTable c1 on b.OracleLocationID=C1.LOCATION_ID
--left join LocationTable c on c1.DOF_CITY=c.LocationCode and c.ParentLocationID is null 
--left join LocationTable d on c1.DOF_SITE=d.LocationCode and d.ParentLocationID=c.LocationID
--left join LocationTable e on c1.DOF_DEPT=e.LocationCode and e.ParentLocationID=d.LocationID

----Select * into zDOFLocationTable_bk23Feb2024_1 from zDOFLocationTable
--truncate table zDOFLocationTable

--exec [Prc_InsertTempLocation] NULL

--update Systemconfigurationtable set IsEnableTigger=1 
--End 
--go 

ALTER Procedure [dbo].[Prc_InsertTempCategory]
	@CategoryID int=Null
As 
BEGIN 

   Select * into #temp2 from CategoryHierarchicalView where @CategoryID is null or @CategoryID=ChildID
  
  Select * into #temp3 from CategoryNewHierarchicalView where @CategoryID is null or @CategoryID=ChildID
   IF(@CategoryID IS NULL)
	BEGIN
		TRUNCATE TABLE tmp_CategoryHierarchicalView
		TRUNCATE TABLE tmp_CategoryNewHierarchicalView
	END
	ELSE
	BEGIN
		DELETE FROM tmp_CategoryHierarchicalView where ChildID = @CategoryID
		DELETE FROM tmp_CategoryNewHierarchicalView where ChildID = @CategoryID
	END
	INSERT INTO tmp_CategoryHierarchicalView
		SELECT * FROM #temp2
		INSERT INTO tmp_CategoryNewHierarchicalView
		SELECT * FROM #temp3

		drop table #temp2
		drop table #temp3
END
go 
ALTER Trigger  [dbo].[trg_Update_tmpCategoryView]  on [dbo].[CategoryTable] 
for UPDATE
As
Begin 

	Declare @CategoryID int
	Select @CategoryID=CategoryID from inserted
	if update(Categorycode) or update(CategoryName) or update(CategoryTypeID)  or update(StatusID)
	Begin 
	 exec Prc_InsertTempCategory @CategoryID
	end 

	--Select @CategoryID=CategoryID from inserted
	--If update(CategoryCode) OR update(StatusID)
	-- Begin 
	--	  exec Prc_InsertTempCategory @CategoryID
	-- End 
END 
go 

ALTER Trigger [dbo].[trg_Ins_CategoryCodeAutoGeneration] on [dbo].[CategoryTable] 
for Insert
As
Begin 
 Declare @CodeTable Table(code nvarchar(50))
 Declare @prefix as nvarchar(50),@Mapping as nvarchar(10) 

 select @prefix= ConfiguarationValue  from ConfigurationTable (nolock) where ConfiguarationName='MasterCodeAutoGenerate'
 Select @Mapping=ConfiguarationValue from ConfigurationTable  (nolock) where ConfiguarationName='UserCategoryMapping'
	 if @prefix='true'
	 Begin 
		IF Exists(select CategoryCode from INSERTED where CategoryCode='-')
		Begin 	
		  insert into @CodeTable
			 exec [dbo].[prc_GetAutoCodeGeneration] 'Category',1
			 
		  Update CategoryTable set CategoryCode=(select code from @CodeTable) where CategoryID=(select CategoryID from INSERTED)
		  
		  IF Exists(select CodeName from CodeConfigurationTable where CodeName='Category')
		   Begin 
		      update CodeConfigurationTable set CodeValue=CodeValue+1 where CodeName='Category'
		   End
		End
	End 
	 --If upper(@Mapping)='TRUE'
	 --Begin 
	 --   Declare @CategoryID int ,@Createdby int ,@ParentCategoryID int 
		-- select @CategoryID=CategoryID,@Createdby=CreatedBy,@ParentCategoryID=ParentCategoryID from Inserted 
		 
		-- If @ParentCategoryID is null or @ParentCategoryID='' 
		-- Begin 
		
		--	 IF not exists(select CategoryID from UserCategoryMappingTable where CategoryID=@CategoryID and PersonID=@Createdby)
		--	 Begin
			 
		--	   Insert into UserCategoryMappingTable(PersonID,CategoryID,StatusID)
		--	   values(@Createdby,@CategoryID,1)
		--	 End
		-- end 
		-- Else 
		-- Begin
		-- Select @ParentCategoryID=FirstLevel from CategoryHierarchicalView where childID=@ParentCategoryID and LanguageID=1
		
		-- --IF not exists(select CategoryID from UserCategoryMappingTable where CategoryID=@ParentCategoryID and PersonID=@Createdby)
		-- --Begin 
			
		--	-- Insert into UserCategoryMappingTable(PersonID,CategoryID,StatusID)
		--	-- values(@Createdby,@ParentCategoryID,1)
		-- ----select CreatedBy,CategoryID,1 From Inserted 
		-- --End
		-- End 		 
	 --End 
	  Declare @CategoryID int
	 select @CategoryID=CategoryID from Inserted 
	 if(@CategoryID is not null)
	 Begin
	    exec Prc_InsertTempCategory @CategoryID
	 end 
End 
go 



ALTER Trigger [dbo].[trg_Ins_LocationCodeAutoGeneration] on [dbo].[LocationTable] 
for Insert
As
Begin 
print 'a'
Declare @LocationID int
select @LocationID=LocationID from Inserted 
if exists(select IsEnableTigger from systemconfigurationtable where IsEnableTigger=1)
Begin 
exec [Prc_InsertTempLocation] @LocationID
end 
 --Declare @CodeTable Table(code nvarchar(50))
 --Declare @prefix as nvarchar(50)
 -- Declare @UserLocationMapping nvarchar(10)
 --select @prefix= ConfiguarationValue  from ConfigurationTable (nolock) where ConfiguarationName='MasterCodeAutoGenerate'
	-- if @prefix='true'
	-- Begin 
	--	IF Exists(select LocationCode from INSERTED where LocationCode='-')
	--	Begin 	
	--	  insert into @CodeTable
	--		 exec [dbo].[prc_GetAutoCodeGeneration] 'Location',1
			 
	--	  Update LocationTable set LocationCode=(select code from @CodeTable) where LocationID=(select LocationId from INSERTED)
		  
	--	  IF Exists(select CodeName from CodeConfigurationTable where CodeName='Location')
	--	   Begin 
	--	      update CodeConfigurationTable set CodeValue=CodeValue+1 where CodeName='Location'
	--	   End
	--	End
	--End 
	--Select @UserLocationMapping= ConfiguarationValue from ConfigurationTable where ConfiguarationName='UserLocationMapping'
	--	IF UPPER(@UserLocationMapping)='TRUE'
	--BEGIN 
	--     Declare @LocationID int ,@Createdby int ,@ParentLocationID int 
	--	 select @LocationID=LocationID,@Createdby=CreatedBy,@ParentLocationID=ParentlocationID from Inserted 
	--	print @LocationID
	--	print @Createdby
	--	print @ParentLocationID
	--	 If @ParentLocationID is null or @ParentLocationID='' 
	--	 Begin 
	--	     IF not exists(select LocationID from UserLocationMappingTable where LocationID=@LocationID and PersonID=@Createdby)
	--		 Begin
	--		   Insert into UserLocationMappingTable(PersonID,LocationID,StatusID)
	--		   values(@Createdby,@LocationID,1)
	--		 End 
	--	 End 
	--	 Else 
	--	 Begin 
		  
		
	--	 Select @ParentLocationID=FirstLevel from LocationHierarchicalView where childID=@LocationID and LanguageID=1
	--	 IF not exists(select LocationID from UserLocationMappingTable where LocationID=@ParentLocationID and PersonID=@Createdby)
	--	 Begin 
	--		Insert into UserLocationMappingTable(PersonID,LocationID,StatusID)
	--		values(@Createdby,@ParentLocationID,1)
			 
	--	 end 
		
	--END 
	-- End 
End 
go 


ALTER Trigger  [dbo].[trg_Update_tmpLocationView] on [dbo].[LocationTable] 
for UPDATE
As
Begin 
    print '1'
	Declare @LocationID int
	Select @LocationID=LocationID from inserted
	if exists(select IsEnableTigger from systemconfigurationtable where IsEnableTigger=1)
Begin
	If update(LocationCode) OR update(StatusID) or update(LocationName) OR update(LocationtypeID)
	 Begin 
		  exec [Prc_InsertTempLocation] @LocationID
	 End 
	 End 
END 
go 


ALTER Procedure [dbo].[Prc_InsertTempLocation]
	@LocationID int=Null
As 
BEGIN 

   Select * into #temp3 from LocationHierarchicalView where @LocationID is null or @LocationID=ChildID
    Select * into #temp4 from LocationNewHierarchicalView where @LocationID is null or @LocationID=ChildID
   IF(@LocationID IS NULL)
	BEGIN
		TRUNCATE TABLE tmp_LocationHierarchicalView
		TRUNCATE TABLE tmp_LocationNewHierarchicalView
	END
	ELSE
	BEGIN
		DELETE FROM tmp_LocationHierarchicalView where ChildID = @LocationID
		DELETE FROM tmp_LocationNewHierarchicalView where ChildID = @LocationID
	END
	INSERT INTO tmp_LocationHierarchicalView
		SELECT * FROM #temp3

INSERT INTO tmp_LocationNewHierarchicalView
		SELECT * FROM #temp4

		drop table #temp3
		drop table #temp4

END
go 
select * into tmp_CategoryNewHierarchicalView from CategoryNewHierarchicalView
go 
select * into tmp_LocationNewHierarchicalView from LocationNewHierarchicalView
go 
ALTER VIEW [dbo].[AssetNewView] 
AS 
	
	SELECT A.AssetID,A.AssetCode,A.Barcode,A.RFIDTagCode,A.LocationID,A.DepartmentID,A.SectionID,A.CustodianID,A.SupplierID,A.AssetConditionID,A.PONumber,A.PurchaseDate,A.PurchasePrice,A.ComissionDate,A.WarrantyExpiryDate,
	A.DisposalReferenceNo,A.DisposedDateTime,A.DisposedRemarks,A.AssetRemarks,A.DepreciationClassID,A.DepreciationFlag,A.SalvageValue,A.VoucherNo,A.StatusID,A.CreatedBy,A.CreatedDateTime,A.LastModifiedBy,
	A.LastModifiedDateTime,A.AssetDescription,A.ReferenceCode,A.SerialNo,A.NetworkID,A.InvoiceNo,A.DeliveryNote,A.Make,A.Capacity,A.MappedAssetID,A.CreateFromHHT,A.DisposalValue,
	A.MailAlert,A.partialDisposalTotalValue,A.IsTransfered,A.CategoryID,A.TransferTypeID,A.UploadedDocumentPath,A.UploadedImagePath,A.AssetApproval,A.ReceiptNumber,A.InsertedToOracle,
	A.InvoiceDate,A.DistributionID,A.ProductID,A.CompanyID,A.SyncDateTime,A.Attribute1,A.Attribute2,A.Attribute3,A.Attribute4,A.Attribute5,A.Attribute6,A.Attribute7,A.Attribute8,A.Attribute9,A.Attribute10,A.Attribute11,A.Attribute12,
	A.Attribute13,A.Attribute14,A.Attribute15,A.Attribute16,A.ManufacturerID,A.ModelID,A.QFAssetCode,A.DOFPO_LINE_NUM,A.DOFMASS_ADDITION_ID,A.ERPUpdateType,A.DOFPARENT_MASS_ADDITION_ID,A.DOFFIXED_ASSETS_UNITS,A.zDOF_Asset_Updated,A.Latitude,A.Longitude,
	A.DisposalTypeID,c.CategoryName,A.CurrentCost,A.ProceedofSales,A.SoldTo,A.AllowTransfer,A.CostOfRemoval,
	c.CategoryNameHierarchy AS CategoryHierarchy, C.childCode AS CategoryCode,LV.ChildCode AS LocationCode,LV.LocationName,
	CASE WHEN LV.PID4 IS NULL OR LV.PID4 ='' THEN '' ELSE LV.PID4 END AS LocationHierarchy,D.DepartmentCode,d.DepartmentName AS DepartmentName,
	S.SectionCode,s.SectionName as SectionDescription,cu.PersonCode AS CustodianCode,Cu.PersonFirstName +''+Cu.PersonLastName AS CustodianName ,MDT.ModelName Model,
	AC.AssetConditionCode,AC.AssetConditionName AS AssetCondition, SU.PartyCode as SupplierCode,SU.PartyName as suppliername ,
	P.ProductCode,p.ProductName AS ProductName ,
	--MFDT.ManufacturerName Manufacturer,
	PE.PersonFirstName+'-'+PE.PersonLastName AS CreateBy, M.PersonFirstName+'-'+M.PersonLastName AS ModifedBy,
	CO.CompanyCode,CO.CompanyName AS CompanyName,
	--DED.DepreciationEndDay DepreciationEndDate,
	--depd.EndDay as DepreciationEndDate,
	--Dep.DepreciationEndDate  AS DepreciationEndDate,
	--'' as DepreciationEndDate,
	LV.L1Desc FirstLevelLocationName,LV.L2Desc SecondLevelLocationName,LV.L3Desc ThirdLevelLocationName, 
	LV.L4Desc FourthLevelLocationName, 	LV.L5Desc FifthLevelLocationName, 	LV.L6Desc SixthLevelLocationName, 
	C.L1Desc FirstLevelCategoryName, 	C.L2Desc SecondLevelCategoryName,	C.L3Desc ThirdLevelCategoryName,
	C.L4Desc FourthLevelCategoryName,C.L5Desc FifthLevelCategoryName,C.L6Desc SixthLevelCategoryName,
	--C.LanguageID,--isnull(A.PurchasePrice,0)-isnull(dep.AssetValueAfterDepreciation,0.00) as Accumulatedvalue,isnull(dep.AssetValueAfterDepreciation,0.00) as NetValue,
--	ISNULL(dep.AccumulatedValue,0.00) AS Accumulatedvalue,ISNULL(dep.NetValue,0.00) AS NetValue,
	--CASE WHEN A.NetbookValue is null and  ISNULL(dep.AccumulatedValue,0.00)=0 THEN ISNULL(A.PurchasePrice,0) ELSE ISNULL(A.NetbookValue,0.00) END AS NetValue,
	ST.Status,IsScanned,
	CAST(LV.Level1ID AS VARCHAR(50)) LocationL1 ,CAST(LV.Level2ID AS VARCHAR(50)) LocationL2,CAST(LV.Level3ID AS VARCHAR(50)) LocationL3,
	CAST(LV.Level4ID AS VARCHAR(50)) LocationL4,CAST(LV.Level5ID AS VARCHAR(50)) LocationL5,CAST(LV.Level6ID AS VARCHAR(50))  LocationL6,
	CAST(C.Level1ID AS VARCHAR(50)) CategoryL1,CAST(C.Level2ID AS VARCHAR(50)) CategoryL2,CAST(C.Level3ID AS VARCHAR(50)) CategoryL3,
	CAST(C.Level4ID AS VARCHAR(50)) CategoryL4,CAST(C.Level5ID AS VARCHAR(50)) CategoryL5,CAST(C.Level6ID AS VARCHAR(50)) CategoryL6,
--	CTT.CustodianType,
	--Category Attributes
	c.Attribute1 AS CategoryAttribute1,c.Attribute2 AS CategoryAttribute2,c.Attribute3 AS CategoryAttribute3,c.Attribute4 AS CategoryAttribute4,
	c.Attribute5 AS CategoryAttribute5 ,c.Attribute6 AS CategoryAttribute6,
	c.Attribute7 AS CategoryAttribute7,c.Attribute8 AS CategoryAttribute8,c.Attribute9 AS CategoryAttribute9,c.Attribute10 AS CategoryAttribute10,
	c.Attribute11 as CategoryAttribute11,c.Attribute12 as CategoryAttribute12,c.Attribute13 as CategoryAttribute13,c.Attribute14 as CategoryAttribute14,
	c.Attribute15 as CategoryAttribute15,c.Attribute16 as CategoryAttribute16,
	--Location Attributes
	LV.Attribute1 AS LocationAttribute1,LV.Attribute2 AS LocationAttribute2,LV.Attribute3 AS LocationAttribute3,LV.Attribute4 AS LocationAttribute4,
	LV.Attribute5 AS LocationAttribute5 ,LV.Attribute6 AS LocationAttribute6,
	LV.Attribute7 AS LocationAttribute7,LV.Attribute8 AS LocationAttribute8,LV.Attribute9 AS LocationAttribute9,LV.Attribute10 AS LocationAttribute10,
	LV.Attribute11 AS LocationAttribute11,LV.Attribute12 AS LocationAttribute12,LV.Attribute13 AS LocationAttribute13,LV.Attribute14 AS LocationAttribute14,
	LV.Attribute15 AS LocationAttribute15,LV.Attribute16 AS LocationAttribute16,
	--a.DisposalTransactionID AS DisposalTransactionID,
	A.Attribute17,A.Attribute18,Attribute19,Attribute20,Attribute21,Attribute22,Attribute23,Attribute24,Attribute25,Attribute26,
	Attribute27,Attribute28,Attribute29,Attribute30,Attribute31,Attribute32,Attribute33,Attribute34,Attribute35,Attribute36,Attribute37,Attribute38,Attribute39,Attribute40,lv.LocationType,c.CategoryType
	--UsefulAssetLife, AssetAge
	
	 FROM AssetTable A 
	LEFT JOIN tmp_CategoryNewHierarchicalView AS c ON A.CategoryID = c.ChildId --AND C.LanguageID IN (1)
	LEFT JOIN ProductTable P ON P.ProductID=A.ProductID AND c.childid=p.categoryID
	LEFT JOIN CompanyTable CO ON CO.CompanyID=A.CompanyID 
	LEFT JOIN StatusTable ST ON A.StatusID=ST.StatusID	
	LEFT JOIN tmp_LocationNewHierarchicalView LV ON A.LocationID = LV.ChildId --AND LV.LanguageID IN (1)
	LEFT JOIN DepartmentTable D ON D.DepartmentID=A.DepartmentID 
	LEFT JOIN SectionTable S ON S.SectionID=A.SectionID  
	LEFT JOIN PersonTable Cu ON A.CustodianID=Cu.PersonID 
	LEFT JOIN PersonTable PE ON A.CreatedBy=PE.PersonID 
	LEFT JOIN PersonTable M ON A.LastModifiedBy=M.PersonID 
	LEFT JOIN AssetConditionTable AC ON a.AssetConditionID=AC.AssetConditionID
	LEFT JOIN PartyTable SU ON SU.PartyID = A.SupplierID  and partyTypeID=2
	left join ModelTable MDT on A.ModelID=mdt.ModelID
	

	
GO
ALTER View [dbo].[ApprovalHistoryView] 
  as 
  Select b.ApprovalRoleName,case when c.StatusID = dbo.fnGetActiveStatusID() then 'Approved' else c.Status end as ApprovalStatus,p.PersonFirstName+'-'+p.PersonLastName as ApprovedBy ,a.LastModifiedDateTime as ApprovedDatetime ,a.*
  ,Ltrim([FileName]) as DocumentName,
  Case when p.PersonID is not null then p.PersonFirstName+'-'+p.PersonLastName else t3.personName end as UserName
 From ApprovalHistoryTable a 

  join ApprovalRoleTable b on a.ApprovalRoleID=b.ApprovalRoleID
  left join StatusTable c on a.StatusID=c.StatusID
  left join PersonTable p on a.LastModifiedBy=p.PersonID
  left join CategoryTypeTable CT on a.CategoryTypeID=ct.categorytypeID
  left join (SELECT a.ObjectKeyID,
   STUFF((SELECT '; ' + US.DocumentName 
          FROM DocumentTable US
          WHERE US.ObjectKeyID = a.ObjectKeyID and us.TransactionType  like '%Approval%'
          ORDER BY Filename
          FOR XML PATH('')), 1, 1, '') [FileName]
FROM DocumentTable a join ApprovalHistoryTable b on a.ObjectKeyID=b.ApprovalHistoryID and a.TransactionType like '%Approval%' 
GROUP BY a.ObjectKeyID) approvaldoc on a.ApprovalHistoryID=approvaldoc.ObjectkeyID 
Left join (
select t2.approvalRoleID,t2.LocationID,t2.CategoryTypeID, STUFF((select ','+PersonName 
from UserApprovalRoleMappingView t1
where t1.approvalRoleID=t2.approvalRoleID and t1.LocationID=t2.LocationID and t1.CategorytypeID=t2.CategorytypeID
order by personName 
 FOR XML PATH('')), 1, 1, '') personName
 from UserApprovalRoleMappingView t2  group by t2.approvalRoleID,t2.LocationID,t2.CategorytypeID
) t3 on b.ApprovalRoleID=t3.ApprovalRoleID and case when b.ApprovalLocationTypeID=5 then a.FromLocationID else a.ToLocationID end =t3.LocationID  --and ( a.FromLocationID=t3.LocationID or  a.ToLocationID=t3.LocationID)
and case when CT.IsAllCategoryType=1 then t3.CategoryTypeID else a.CategorytypeID end =t3.CategoryTypeID 
GO
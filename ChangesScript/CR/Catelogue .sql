IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'CatalogueImage' AND Object_ID = Object_ID(N'CategoryTable'))
Begin 
alter table CategoryTable add CatalogueImage nvarchar(max) null
END
GO
IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'CatalogueImage' AND Object_ID = Object_ID(N'ProductTable'))
Begin 
alter table ProductTable add CatalogueImage nvarchar(max) null
END
GO
IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'VirtualBarcode' AND Object_ID = Object_ID(N'ProductTable'))
Begin 
alter table ProductTable add VirtualBarcode nvarchar(100) null
END
GO
IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'Specification' AND Object_ID = Object_ID(N'ProductTable'))
Begin 
alter table ProductTable add Specification nvarchar(max) null
END
GO
IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'Remarks' AND Object_ID = Object_ID(N'ProductTable'))
Begin 
alter table ProductTable add Remarks nvarchar(max) null
END
GO
IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'ProductID' AND Object_ID = Object_ID(N'ZDOF_UserPODataTable'))
Begin 
alter table ZDOF_UserPODataTable add ProductID int null foreign key references ProductTable(ProductID)
END
GO

If not exists(select Importfield from ImportTemplateNewTable where ImportField='CatalogueImage' and EntityID=(select EntityID from EntityTable where EntityName='Category') and ImportTemplateTypeID=1)
begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,
DataSize,DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'CatalogueImage',4,1,0,100,NULL,0,100,1,0,NULL,EntityID
From EntityTable where EntityName='Category'
End 
go 
If not exists(select Importfield from ImportTemplateNewTable where ImportField='CatalogueImage' and EntityID=(select EntityID from EntityTable where EntityName='Category') and ImportTemplateTypeID=2)
begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,
DataSize,DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'CatalogueImage',4,1,0,100,NULL,0,100,2,0,NULL,EntityID
From EntityTable where EntityName='Category'
End 
go 

If not exists(select Importfield from ImportTemplateNewTable where ImportField='CatalogueImage' and EntityID=(select EntityID from EntityTable where EntityName='Product') and ImportTemplateTypeID=1)
begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,
DataSize,DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'CatalogueImage',4,1,0,100,NULL,0,100,1,0,NULL,EntityID
From EntityTable where EntityName='Product'
End 
go 
If not exists(select Importfield from ImportTemplateNewTable where ImportField='CatalogueImage' and EntityID=(select EntityID from EntityTable where EntityName='Product') and ImportTemplateTypeID=2)
begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,
DataSize,DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'CatalogueImage',4,1,0,100,NULL,0,100,2,0,NULL,EntityID
From EntityTable where EntityName='Product'
End 
go 
If not exists(select Importfield from ImportTemplateNewTable where ImportField='Remarks' and EntityID=(select EntityID from EntityTable where EntityName='Product') and ImportTemplateTypeID=1)
begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,
DataSize,DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'Remarks',5,1,0,100,NULL,0,100,1,0,NULL,EntityID
From EntityTable where EntityName='Product'
End 
go 
If not exists(select Importfield from ImportTemplateNewTable where ImportField='Remarks' and EntityID=(select EntityID from EntityTable where EntityName='Product') and ImportTemplateTypeID=2)
begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,
DataSize,DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'Remarks',5,1,0,100,NULL,0,100,2,0,NULL,EntityID
From EntityTable where EntityName='Product'
End 
go 
If not exists(select Importfield from ImportTemplateNewTable where ImportField='VirtualBarcode' and EntityID=(select EntityID from EntityTable where EntityName='Product') and ImportTemplateTypeID=1)
begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,
DataSize,DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'VirtualBarcode',6,1,0,100,NULL,0,100,1,0,NULL,EntityID
From EntityTable where EntityName='Product'
End 
go 
If not exists(select Importfield from ImportTemplateNewTable where ImportField='VirtualBarcode' and EntityID=(select EntityID from EntityTable where EntityName='Product') and ImportTemplateTypeID=2)
begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,
DataSize,DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'VirtualBarcode',6,1,0,100,NULL,0,100,2,0,NULL,EntityID
From EntityTable where EntityName='Product'
End 
go 
If not exists(select Importfield from ImportTemplateNewTable where ImportField='Specification' and EntityID=(select EntityID from EntityTable where EntityName='Product') and ImportTemplateTypeID=1)
begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,
DataSize,DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'Specification',7,1,0,100,NULL,0,100,1,0,NULL,EntityID
From EntityTable where EntityName='Product'
End 
go 
If not exists(select Importfield from ImportTemplateNewTable where ImportField='Specification' and EntityID=(select EntityID from EntityTable where EntityName='Product') and ImportTemplateTypeID=2)
begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,
DataSize,DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'Specification',7,1,0,100,NULL,0,100,2,0,NULL,EntityID
From EntityTable where EntityName='Product'
End 
go 

ALTER VIEW [dbo].[AssetNewView] 
AS 	
SELECT A.AssetID,A.AssetCode,A.Barcode,A.RFIDTagCode,A.LocationID,A.DepartmentID,A.SectionID,A.CustodianID,A.SupplierID,
	A.AssetConditionID,A.PONumber,A.PurchaseDate,A.PurchasePrice,A.ComissionDate,A.WarrantyExpiryDate,
	A.DisposalReferenceNo,A.DisposedDateTime,A.DisposedRemarks,A.AssetRemarks,A.DepreciationClassID,A.DepreciationFlag,
	A.SalvageValue,A.VoucherNo,A.StatusID,A.CreatedBy,A.CreatedDateTime,A.LastModifiedBy,
	A.LastModifiedDateTime,A.AssetDescription,A.ReferenceCode,A.SerialNo,A.NetworkID,A.InvoiceNo,A.DeliveryNote,A.Make,A.Capacity,A.MappedAssetID,A.CreateFromHHT,A.DisposalValue,
	A.MailAlert,A.partialDisposalTotalValue,A.IsTransfered,A.CategoryID,A.TransferTypeID,A.UploadedDocumentPath,A.UploadedImagePath,A.AssetApproval,A.ReceiptNumber,A.InsertedToOracle,
	A.InvoiceDate,A.DistributionID,A.ProductID,A.CompanyID,A.SyncDateTime,A.Attribute1,A.Attribute2,A.Attribute3,A.Attribute4,A.Attribute5,A.Attribute6,A.Attribute7,A.Attribute8,A.Attribute9,A.Attribute10,A.Attribute11,A.Attribute12,
	A.Attribute13,A.Attribute14,A.Attribute15,A.Attribute16,A.ManufacturerID,A.ModelID,A.QFAssetCode,A.DOFPO_LINE_NUM,A.DOFMASS_ADDITION_ID,A.ERPUpdateType,A.DOFPARENT_MASS_ADDITION_ID,A.DOFFIXED_ASSETS_UNITS,A.zDOF_Asset_Updated,A.Latitude,A.Longitude,
	A.DisposalTypeID,c.CategoryName,A.CurrentCost,A.ProceedofSales,A.SoldTo,A.AllowTransfer,A.CostOfRemoval,
	c.CategoryNameHierarchy AS CategoryHierarchy, C.CategoryCodeHierarchy AS CategoryCode,LV.LocationCodeHierarchy AS LocationCode,LV.LocationName,
	CASE WHEN LV.LocationNameHierarchy IS NULL OR LV.LocationNameHierarchy ='' THEN '' ELSE LV.LocationNameHierarchy END AS LocationHierarchy,D.DepartmentCode,d.DepartmentName AS DepartmentName,
	S.SectionCode,s.SectionName as SectionDescription,
	cu.PersonCode AS CustodianCode,Cu.PersonFirstName +''+Cu.PersonLastName AS CustodianName,
	MDT.ModelName Model,
	AC.AssetConditionCode,AC.AssetConditionName AS AssetCondition, SU.PartyCode as SupplierCode,SU.PartyName as suppliername ,
	P.ProductCode,p.ProductName AS ProductName ,
	PE.PersonFirstName+'-'+PE.PersonLastName AS CreateBy, M.PersonFirstName+'-'+M.PersonLastName AS ModifedBy,
	CO.CompanyCode,CO.CompanyName AS CompanyName,
	LV.L1LocationName FirstLevelLocationName,LV.L2LocationName SecondLevelLocationName,LV.L3LocationName ThirdLevelLocationName, 
	LV.L4LocationName FourthLevelLocationName, 	LV.L5LocationName FifthLevelLocationName, 	LV.L6LocationName SixthLevelLocationName, 
	C.L1CategoryName FirstLevelCategoryName, 	C.L2CategoryName SecondLevelCategoryName,	C.L3CategoryName ThirdLevelCategoryName,
	C.L4CategoryName FourthLevelCategoryName,C.L5CategoryName FifthLevelCategoryName,C.L6CategoryName SixthLevelCategoryName,
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
	Attribute27,Attribute28,Attribute29,Attribute30,Attribute31,Attribute32,Attribute33,Attribute34,Attribute35,Attribute36,Attribute37,Attribute38,Attribute39,Attribute40,
	lv.LocationType,c.CategoryType,
	LV.Level2ID [MappedLocationID],
	C.Level2ID [MappedCategoryID],DisposalTransactionID,P.virtualBarcode,P.specification  

	FROM AssetTable A 
	LEFT JOIN tmp_CategoryNewHierarchicalView AS c ON A.CategoryID = c.CategoryID --AND C.LanguageID IN (1)
	LEFT JOIN ProductTable P ON P.ProductID = A.ProductID AND c.CategoryID=p.categoryID
	LEFT JOIN CompanyTable CO ON CO.CompanyID=A.CompanyID 
	LEFT JOIN StatusTable ST ON A.StatusID=ST.StatusID	
	LEFT JOIN tmp_LocationNewHierarchicalView LV ON A.LocationID = LV.LocationID --AND LV.LanguageID IN (1)
	LEFT JOIN DepartmentTable D ON D.DepartmentID=A.DepartmentID 
	LEFT JOIN SectionTable S ON S.SectionID=A.SectionID  
	LEFT JOIN PersonTable Cu ON A.CustodianID=Cu.PersonID 
	LEFT JOIN PersonTable PE ON A.CreatedBy=PE.PersonID 
	LEFT JOIN PersonTable M ON A.LastModifiedBy=M.PersonID 
	LEFT JOIN AssetConditionTable AC ON a.AssetConditionID=AC.AssetConditionID
	LEFT JOIN PartyTable SU ON SU.PartyID = A.SupplierID  and partyTypeID=2
	left join ModelTable MDT on A.ModelID=mdt.ModelID

	LEFT JOIN (SELECT DISTINCT CategoryID FROM UserCategoryMappingTable) CMAP ON CMAP.CategoryID = C.Level2ID
	LEFT JOIN (SELECT DISTINCT LocationID FROM UserLocationMappingTable) LMAP ON LMAP.LocationID = LV.Level2ID

	WHERE CMAP.CategoryID IS NOT NULL AND LMAP.LocationID IS NOT NULL
GO

ALTER PROC [dbo].[IPRC_ExceImportCategory]
(
	@CategoryCode		nvarchar(50)=null,
	@CategoryDescription nvarchar(200),
	@ParentCategoryCode	nvarchar(50)=null,
	@ImportTypeID int,
	@UserID int=1,
	@Attribute1 nvarchar(100)=null,
	@Attribute2 nvarchar(100)=null,
	@Attribute3 nvarchar(100)=null,
	@Attribute4 nvarchar(100)=null,
	@Attribute5 nvarchar(100)=null,
	@Attribute6 nvarchar(100)=null,
	@Attribute7 nvarchar(100)=null,
	@Attribute8 nvarchar(100)=null,
	@Attribute9 nvarchar(100)=null,
	@Attribute10 nvarchar(100)=null,
	@Attribute11 nvarchar(100)=null,
	@Attribute12 nvarchar(100)=null,
	@Attribute13 nvarchar(100)=null,
	@Attribute14 nvarchar(100)=null,
	@Attribute15 nvarchar(100)=null,
	@Attribute16 nvarchar(100)=null,
	@CatalogueImage  nvarchar(max)=null
)
AS
BEGIN

	DECLARE @ErrorToBeReturned nvarchar(max);
	Declare @parentCategoryID int =null;
	Declare @CategoryID int=null;
	DECLARE @ConfigValue nvarchar(max);
	Declare @ImportExcelNotAllowCreateReferenceFieldNewEntry nvarchar(10)

	Select @ImportExcelNotAllowCreateReferenceFieldNewEntry=ConfiguarationValue from configurationTable where ConfiguarationName='ImportExcelNotAllowCreateReferenceFieldNewEntry'
	set @CategoryCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@CategoryCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
set @CategoryDescription=REPLACE(@CategoryDescription,'''','''')
		if(@ImportTypeID=1)
		BEGIN

		if(@ParentCategoryCode is not null and @ParentCategoryCode!='')
			BEGIN			
				--Insert parent category
				if(@ImportExcelNotAllowCreateReferenceFieldNewEntry='false')
				Begin
				IF NOT EXISTS (SELECT * FROM CategoryTable where CategoryCode=@ParentCategoryCode and StatusID=1)
				BEGIN
					INSERT INTO CategoryTable(CategoryCode,StatusID, CreatedBy,CategoryName,CatalogueImage)
						VALUES(@ParentCategoryCode, 1, @UserID,@ParentCategoryCode,@CatalogueImage)
						set @parentCategoryID=@@IDENTITY;

						Insert into CategoryDescriptionTable(CategoryID,CategoryDescription,LanguageID,CreatedBy,CreatedDateTime)
						Select C.CategoryID,c.CategoryCode,L.LanguageID,C.CreatedBy,C.CreatedDateTime From CategoryTable C join 
								LanguageTable L on 1=1 left join CategoryDescriptionTable MD on MD.CategoryID=C.CategoryID and MD.languageid=L.languageID 
								where MD.LanguageID is null
								 and C.CategoryCode=@ParentCategoryCode
				END
				ELSE
				BEGIN
				   select @parentCategoryID=CategoryID from CategoryTable where CategoryCode=@ParentCategoryCode and StatusID=1
				   if(@ParentCategoryCode is not null and @parentCategoryID is null)

				   return @ParentCategoryCode+'- Parent Category Is not available;'
				END
				END
				Else
				if(@ParentCategoryCode is not null and @ParentCategoryCode!='')
				BEGIN
				   return @ParentCategoryCode+'- Parent Category Is not available;'
				  END
			END
			

		if NOT EXISTS (SELECT * FROM CategoryTable WHERE CategoryCode = @CategoryCode AND ParentCategoryID=@parentCategoryID and StatusID=1)
				BEGIN
			Select @ConfigValue=ConfiguarationValue from configurationTable where ConfiguarationName='MasterCodeAutoGenerate'
			IF(@ConfigValue='false')
			BEGIN
				INSERT INTO CategoryTable(CategoryCode,ParentCategoryID,StatusID, CreatedBy,CreatedDateTime,
			Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6,Attribute7,Attribute8,Attribute9,Attribute10,Attribute11,Attribute12,
			Attribute13,Attribute14,Attribute15,Attribute16,CategoryName,CatalogueImage) values(@CategoryCode,@parentCategoryID,1,@UserID,getDate(),@Attribute1,@Attribute2,@Attribute3,@Attribute4,@Attribute5,
			@Attribute6,@Attribute7,@Attribute8,@Attribute9,@Attribute10,@Attribute11,@Attribute12,
			@Attribute13,@Attribute14,@Attribute15,@Attribute16,@CategoryDescription,@CatalogueImage)

				--Insert category Description
					Insert into CategoryDescriptionTable(CategoryID,CategoryDescription,LanguageID,CreatedBy,CreatedDateTime)
						Select C.CategoryID,@CategoryDescription,L.LanguageID,C.CreatedBy,C.CreatedDateTime From CategoryTable C join 
						LanguageTable L on 1=1 left join CategoryDescriptionTable MD on MD.CategoryID=C.CategoryID and MD.languageid=L.languageID 
						where MD.LanguageID is null
						and C.CategoryCode=@CategoryCode
			END
			ELSE
			BEGIN
			
		INSERT INTO CategoryTable(CategoryCode,ParentCategoryID,StatusID, CreatedBy,CreatedDateTime,
			Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6,Attribute7,Attribute8,Attribute9,Attribute10,Attribute11,Attribute12,
			Attribute13,Attribute14,Attribute15,Attribute16,CategoryName,CatalogueImage) values('-',@parentCategoryID,1,@UserID,getDate(),@Attribute1,@Attribute2,@Attribute3,@Attribute4,@Attribute5,
			@Attribute6,@Attribute7,@Attribute8,@Attribute9,@Attribute10,@Attribute11,@Attribute12,
			@Attribute13,@Attribute14,@Attribute15,@Attribute16,@CategoryDescription,@CatalogueImage)

			set @CategoryID=@@IDENTITY;
		--Insert Department Description
					Insert into CategoryDescriptionTable(CategoryID,CategoryDescription,LanguageID,CreatedBy,CreatedDateTime)
						Select C.CategoryID,@CategoryDescription,L.LanguageID,C.CreatedBy,C.CreatedDateTime From CategoryTable C join 
						LanguageTable L on 1=1 left join CategoryDescriptionTable MD on MD.CategoryID=C.CategoryID and MD.languageid=L.languageID 
						where MD.LanguageID is null
						and C.CategoryID=@CategoryID
			END
			End
			Else
				   SET @ErrorToBeReturned = @CategoryCode+'- already Exists;'


		End
			ELSE if(@ImportTypeID=2)
		BEGIN		
				UPDATE A SET 
					CategoryDescription = @CategoryDescription					
					from CategoryDescriptionTable A join CategoryTable B on A.CategoryID=B.CategoryID
				WHERE B.CategoryCode = @CategoryCode

				Update CategoryTable set Attribute1=@Attribute1,Attribute2=@Attribute2,Attribute3=@Attribute3,Attribute4=@Attribute4,Attribute5=@Attribute5,
				Attribute6=@Attribute6,Attribute7=@Attribute7,Attribute8=@Attribute8,Attribute9=@Attribute9,Attribute10=@Attribute10,Attribute11=@Attribute11,
				Attribute12=@Attribute12,Attribute13=@Attribute13,Attribute14=@Attribute14,Attribute15=@Attribute15,Attribute16=@Attribute16,
				CategoryName=@CategoryDescription
				
				where CategoryCode=@CategoryCode
		END
	SELECT @ErrorToBeReturned ErrorToBeReturned;
END  
go 


ALTER PROC [dbo].[IPRC_ExceImportProduct]
(
	@ProductCode		nvarchar(50)=null,
	@ProductDescription nvarchar(200),
	@CategoryCode	nvarchar(50)=null,
	@ImportTypeID int,
	@UserID int=1,
	@Attribute1 nvarchar(100)=null,
	@Attribute2 nvarchar(100)=null,
	@Attribute3 nvarchar(100)=null,
	@Attribute4 nvarchar(100)=null,
	@Attribute5 nvarchar(100)=null,
	@Attribute6 nvarchar(100)=null,
	@Attribute7 nvarchar(100)=null,
	@Attribute8 nvarchar(100)=null,
	@Attribute9 nvarchar(100)=null,
	@Attribute10 nvarchar(100)=null,
	@Attribute11 nvarchar(100)=null,
	@Attribute12 nvarchar(100)=null,
	@Attribute13 nvarchar(100)=null,
	@Attribute14 nvarchar(100)=null,
	@Attribute15 nvarchar(100)=null,
	@Attribute16 nvarchar(100)=null,
	@CatalogueImage  nvarchar(max)=null,
	@Remarks nvarchar(max)=null,
	@VirtualBarcode nvarchar(max)=null,
	@Specification nvarchar(max)=null
)
AS
BEGIN

	DECLARE @ErrorToBeReturned nvarchar(max);
	Declare @CategoryID int =null;
	Declare @ProductID int=null;
	DECLARE @ConfigValue nvarchar(max);
	Declare @ImportExcelNotAllowCreateReferenceFieldNewEntry nvarchar(10)

	Select @ImportExcelNotAllowCreateReferenceFieldNewEntry=ConfiguarationValue from configurationTable where ConfiguarationName='ImportExcelNotAllowCreateReferenceFieldNewEntry'
	set @ProductCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@ProductCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', '');
		set @CategoryCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@CategoryCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
		set @ProductDescription=REPLACE(@ProductDescription,'''','''')
		if(@ImportTypeID=1)
		BEGIN

		if(@CategoryCode is not null and @CategoryCode!='')
			BEGIN			
				--Insert Department
				if(@ImportExcelNotAllowCreateReferenceFieldNewEntry='false')
				Begin
				IF NOT EXISTS (SELECT * FROM CategoryTable where CategoryCode=@CategoryCode and statusID=1)
				BEGIN
					INSERT INTO CategoryTable(CategoryCode,StatusID, CreatedBy,CategoryName)
						VALUES(@CategoryCode, 1, @UserID,@CategoryCode)
						set @CategoryID=@@IDENTITY;

						Insert into CategoryDescriptionTable(CategoryID,CategoryDescription,LanguageID,CreatedBy,CreatedDateTime)
						Select C.CategoryID,c.CategoryCode,L.LanguageID,C.CreatedBy,C.CreatedDateTime From CategoryTable C join 
								LanguageTable L on 1=1 left join CategoryDescriptionTable MD on MD.CategoryID=C.CategoryID and MD.languageid=L.languageID 
								where MD.LanguageID is null
								 and C.CategoryCode=@CategoryCode
				END
				ELSE
				BEGIN
				   select @CategoryID=CategoryID from CategoryTable where CategoryCode=@CategoryCode and statusID=1
				   if(@CategoryID is null)

				   return @CategoryCode+'- Category Is not available;'
				END
				END
				Else
				IF NOT EXISTS (SELECT * FROM CategoryTable where CategoryCode=@CategoryCode and statusID=1)
				BEGIN
				  return @CategoryCode+'- Category Is not available;'
				  END				 
			END
			

		if NOT EXISTS (SELECT * FROM ProductTable WHERE ProductCode = @ProductCode AND CategoryID=@CategoryID and statusID=1)
				BEGIN
			Select @ConfigValue=ConfiguarationValue from configurationTable where ConfiguarationName='MasterCodeAutoGenerate'
			IF(@ConfigValue='false')
			BEGIN
				INSERT INTO ProductTable(ProductCode,CategoryID,StatusID, CreatedBy,CreatedDateTime,IsInventoryItem,IsSerialNumberRequired,
			Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6,Attribute7,Attribute8,Attribute9,Attribute10,Attribute11,Attribute12,
			Attribute13,Attribute14,Attribute15,Attribute16,ProductName,CatalogueImage,Remarks,VirtualBarcode,Specification) values(@Productcode,@CategoryID,1,@UserID,getDate(),0,0,@Attribute1,@Attribute2,@Attribute3,@Attribute4,@Attribute5,
			@Attribute6,@Attribute7,@Attribute8,@Attribute9,@Attribute10,@Attribute11,@Attribute12,
			@Attribute13,@Attribute14,@Attribute15,@Attribute16,@ProductDescription,@CatalogueImage,@Remarks,@VirtualBarcode,@Specification)

				--Insert Product Description
					Insert into ProductDescriptionTable(ProductID,ProductDescription,LanguageID,CreatedBy,CreatedDateTime)
						Select C.ProductID,@ProductDescription,L.LanguageID,C.CreatedBy,C.CreatedDateTime From ProductTable C join 
						LanguageTable L on 1=1 left join ProductDescriptionTable MD on MD.ProductID=C.ProductID and MD.languageid=L.languageID 
						where MD.LanguageID is null
						and C.ProductCode=@ProductCode
			END
			ELSE
			BEGIN
			
		INSERT INTO ProductTable(ProductCode,CategoryID,StatusID, CreatedBy,CreatedDateTime,IsInventoryItem,IsSerialNumberRequired,
			Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6,Attribute7,Attribute8,Attribute9,Attribute10,Attribute11,Attribute12,
			Attribute13,Attribute14,Attribute15,Attribute16,ProductName,CatalogueImage,Remarks,VirtualBarcode,Specification) values('-',@CategoryID,1,@UserID,getDate(),0,0,@Attribute1,@Attribute2,@Attribute3,@Attribute4,@Attribute5,
			@Attribute6,@Attribute7,@Attribute8,@Attribute9,@Attribute10,@Attribute11,@Attribute12,
			@Attribute13,@Attribute14,@Attribute15,@Attribute16,@ProductDescription,@CatalogueImage,@Remarks,@VirtualBarcode,@Specification)

			set @ProductID=@@IDENTITY;
		--Insert Product Description
					Insert into ProductDescriptionTable(ProductID,ProductDescription,LanguageID,CreatedBy,CreatedDateTime)
						Select C.ProductID,@ProductDescription,L.LanguageID,C.CreatedBy,C.CreatedDateTime From ProductTable C join 
						LanguageTable L on 1=1 left join ProductDescriptionTable MD on MD.ProductID=C.ProductID and MD.languageid=L.languageID 
						where MD.LanguageID is null
						and C.ProductID=@ProductID
			END
			End
			Else
				   SET @ErrorToBeReturned = @ProductCode+'- already Exists;'


		End
			ELSE if(@ImportTypeID=2)
		BEGIN	
	Print  @ProductCode	
		select @ProductID=ProductId from ProductTable where ProductCode=@ProductCode and statusID=1
			Print  @ProductID	
				UPDATE ProductDescriptionTable SET 
					ProductDescription = @ProductDescription	
				WHERE ProductID = @ProductID

				 select @CategoryID=CategoryID from CategoryTable where CategoryCode=@CategoryCode and statusID=1
				   if(@CategoryID is null)

				   return @CategoryCode+'- Category Is not available;'
				Update ProductTable set CategoryID=@CategoryID, Attribute1=@Attribute1,Attribute2=@Attribute2,Attribute3=@Attribute3,Attribute4=@Attribute4,Attribute5=@Attribute5,
				Attribute6=@Attribute6,Attribute7=@Attribute7,Attribute8=@Attribute8,Attribute9=@Attribute9,Attribute10=@Attribute10,Attribute11=@Attribute11,
				Attribute12=@Attribute12,Attribute13=@Attribute13,Attribute14=@Attribute14,Attribute15=@Attribute15,Attribute16=@Attribute16 
				,ProductName=@ProductDescription,
				CatalogueImage=case when @CatalogueImage is null or @CatalogueImage ='' then CatalogueImage else @CatalogueImage end,
				Remarks=case when @Remarks is null or @Remarks ='' then Remarks else @Remarks end,
				VirtualBarcode=case when @VirtualBarcode is null or @VirtualBarcode ='' then VirtualBarcode else @VirtualBarcode end,
				Specification=case when @Specification is null or @Specification ='' then Specification else @Specification end
				where ProductCode=@ProductCode
		END
	SELECT @ErrorToBeReturned ErrorToBeReturned;
END  
go 

ALTER Procedure [dbo].[IPRC_AMSExceImportBulkAssets]
(
	@AssetCode nvarchar(100)=NULL,
	@Barcode nvarchar(100)=NULL,
	@DepartmentCode nvarchar(100)=NULL,
	@SectionCode nvarchar(100)=NULL,
	@CustodianCode nvarchar(100)=NULL,
	@ModelCode nvarchar(100)=NULL,
	@PONumber nvarchar(100)=NULL,
	@ReferenceCode nvarchar(100)=null,
	@SerialNo nvarchar(100)=NULL,
	@PurchaseDate nvarchar(100)=NULL,
	@WarrantyExpiryDate nvarchar(100)=NULL,
	@CategoryCode nvarchar(100)=NULL,
	@LocationCode nvarchar(100)=NULL,
	@VirtualBarcode nvarchar(max)=NULL,
	@AssetConditionCode nvarchar(100)=NULL,
	@PurchasePrice nvarchar(100)=NULL,
	@DeliveryNote nvarchar(100)=null,
	@RFIDTagCode nvarchar(100)=null,
	@SupplierCode nvarchar(100)=NULL,
	@AssetRemarks nvarchar(max)=null,
	@InvoiceNo nvarchar(100)=null,
	@InvoiceDate nvarchar(100)=null,
	@ManufacturerCode nvarchar(100)=NULL,
	@CommissionDate nvarchar(100)=null,
	@VoucherNo nvarchar(100)=null,
	@Make nvarchar(100)=null,
	@Capacity nvarchar(100)=null,
	@MappedAssetID nvarchar(100)=null,
	@ReceiptNumber nvarchar(100)=null,
	@ImportTypeID int,
	@UserID int,
	@LanguageID int,
	@CompanyID int,	
	@Attribute1 nvarchar(100)=null,
	@Attribute2 nvarchar(100)=null,
	@Attribute3 nvarchar(100)=null,
	@Attribute4 nvarchar(100)=null,
	@Attribute5 nvarchar(100)=null,
	@Attribute6 nvarchar(100)=null,
	@Attribute7 nvarchar(100)=null,
	@Attribute8 nvarchar(100)=null,
	@Attribute9 nvarchar(100)=null,
	@Attribute10 nvarchar(100)=null,
	@Attribute11 nvarchar(100)=null,
	@Attribute12 nvarchar(100)=null,
	@Attribute13 nvarchar(100)=null,
	@Attribute14 nvarchar(100)=null,
	@Attribute15 nvarchar(100)=null,
	@Attribute16 nvarchar(100)=null,
    @QFAssetCode nvarchar(100)=NULL,
	@Attribute17 nvarchar(100)=NULL,
	@Attribute18 nvarchar(100)=NULL,
	@Attribute19 nvarchar(100)=NULL,
	@Attribute20 nvarchar(100)=NULL,
	@Attribute21 nvarchar(100)=NULL,
	@Attribute22 nvarchar(100)=NULL,
	@Attribute23 nvarchar(100)=NULL,
	@Attribute24 nvarchar(100)=NULL,
	@Attribute25 nvarchar(100)=NULL,
	@Attribute26 nvarchar(100)=NULL,
	@Attribute27 nvarchar(100)=NULL,
	@Attribute28 nvarchar(100)=NULL,
	@Attribute29 nvarchar(100)=NULL,
	@Attribute30 nvarchar(100)=NULL,
	@Attribute31 nvarchar(100)=NULL,
	@Attribute32 nvarchar(100)=NULL,
	@Attribute33 nvarchar(100)=NULL,
	@Attribute34 nvarchar(100)=NULL,
	@Attribute35 nvarchar(100)=NULL,
	@Attribute36 nvarchar(100)=NULL,
	@Attribute37 nvarchar(100)=NULL,
	@Attribute38 nvarchar(100)=NULL,
	@Attribute39 nvarchar(100)=NULL,
	@Attribute40 nvarchar(100)=NULL,
	@L1LocationCode nvarchar(100)=NULL,
	@L2LocationCode nvarchar(100)=NULL,
	@L1CategoryCode nvarchar(100)=NULL,
	@UploadedDocumentPath  nvarchar(max)=NULL,
	@ImportExcelFileName nvarchar(max)=NULL
	)
as 
Begin 

set @AssetCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@AssetCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
set @DepartmentCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@DepartmentCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
set @SectionCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@SectionCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
set @CustodianCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@CustodianCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
set @ModelCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@ModelCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
set @CategoryCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@CategoryCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
set @LocationCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@LocationCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
set @AssetConditionCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@AssetConditionCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
set @SupplierCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@SupplierCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
set @ManufacturerCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@ManufacturerCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
Declare @ProductCode nvarchar(max) 
--set @ProductCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@VirtualBarcode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
set @ProductCode=@VirtualBarcode

IF(@PurchaseDate='')
BEGIN
set @PurchaseDate=null
END
IF(@CommissionDate='')
BEGIN
set @CommissionDate=null
END
IF(@WarrantyExpiryDate='')
BEGIN
set @WarrantyExpiryDate=null
END
IF(@InvoiceDate='')
BEGIN
set @InvoiceDate=null
END
if(@PurchasePrice='-')
Begin 
set @PurchasePrice=NULL
End 
 	--Declartion part 
	Declare @DepartmentID int,@SectionID int,@CustodianID int,@SupplierID int,@AssetConditionID int, @CategoryID int, @ManufacturerID int,@ModelID int ,@ProductID int ,@LocationID int  ,@DateFormat bit ,@numberFormat bit 
	Declare @ManufacturerCategoryMapping nvarchar(50),@CategoryManufacturerModelMapping nvarchar(50),@ImportExcelNotAllowCreateReferenceFieldNewEntry nvarchar(50) ,@BarcodeEqualAssetCode nvarchar(50),
	@IsBarcodeSettingApplyImportAsset nvarchar(50),@BarcodeAutoGenerateEnable nvarchar(50),@LocationMandatory nvarchar(50),@ReturnMessage NVARCHAR(MAX),@AssetApproval int 
	DECLARE @MESSAGETABLE TABLE(TEXT NVARCHAR(MAX))
	select @ManufacturerCategoryMapping=ConfiguarationValue From ConfigurationTable where ConfiguarationName='ManufacturerCategoryMapping'
	select @CategoryManufacturerModelMapping=ConfiguarationValue From ConfigurationTable where ConfiguarationName='ModelManufacturerCategoryMapping'
	select @ImportExcelNotAllowCreateReferenceFieldNewEntry=ConfiguarationValue From ConfigurationTable where ConfiguarationName='ImportExcelNotAllowCreateReferenceFieldNewEntry'
	select @BarcodeEqualAssetCode=ConfiguarationValue From ConfigurationTable where ConfiguarationName='BarcodeEqualAssetCode'
	select @IsBarcodeSettingApplyImportAsset=ConfiguarationValue From ConfigurationTable where ConfiguarationName='IsBarcodeSettingApplyImportAsset'
	select @BarcodeAutoGenerateEnable=ConfiguarationValue From ConfigurationTable where ConfiguarationName='BarcodeAutoGenerateEnable'
	select @LocationMandatory=ConfiguarationValue From ConfigurationTable where ConfiguarationName='LocationMandatory'

	select @AssetApproval=case when ConfiguarationValue='true' then 1 else 0 end From ConfigurationTable where ConfiguarationName='AssetApproval'
 
	IF @CategoryCode IS NOT NULL AND (@L1CategoryCode IS NULL OR @L1CategoryCode='')
	BEGIN 
	   SElect @Barcode+'- L1 Category Code is must be add if Update/Create Category Code ' as ReturnMessage
				Return 
	END 
	IF @LocationCode IS NOT NULL AND ((@L1LocationCode IS NULL OR @L1LocationCode='') or (@L2LocationCode IS NULL OR @L2LocationCode=''))
	BEGIN 
	   SElect @Barcode+'- L1 Location Code is must be add if Update/Create Location Code ' as ReturnMessage
				Return 
	END 
	IF @VirtualBarcode IS NOT NULL AND (@CategoryCode IS NULL OR @CategoryCode='')
	BEGIN 
	   SElect @Barcode+'- Category Code is must be add if update/Create Asset Description ' as ReturnMessage
				Return 
	END
	IF @ModelCode IS NOT NULL AND (@ManufacturerCode IS NULL OR @ManufacturerCode='')
	BEGIN 
	   SElect @Barcode+'- Manufacturer Code is must be add if update/Create Model Code ' as ReturnMessage
				Return 
	END 
	IF @CustodianCode IS NOT NULL AND (@DepartmentCode IS NULL OR @DepartmentCode='')
	BEGIN 
	   SElect @Barcode+'- Department Code is must be add if Update/Create Custodian Code ' as ReturnMessage
				Return 
	END 
	IF @SectionCode IS NOT NULL AND (@DepartmentCode IS NULL OR @DepartmentCode='')
	BEGIN 
	   SElect @Barcode+'- Department Code is must be add if Update/Create Section Code ' as ReturnMessage
				Return 
	END 
	If ((@CommissionDate is not null and @CommissionDate!=''))
		Begin
		  SET DATEFORMAT dmy;
		  SELECT @DateFormat= ISDATE(@CommissionDate)
		  If @DateFormat=0
		   BEGIN 
		     SElect @Barcode+'- Comission Date is invalid' as ReturnMessage
				Return 
		   END 
		End 
			If ((@WarrantyExpiryDate is not null and @WarrantyExpiryDate!=''))
		Begin
		  SET DATEFORMAT dmy;
		  SELECT @DateFormat= ISDATE(@WarrantyExpiryDate)
		  If @DateFormat=0
		   BEGIN 
		     SElect @Barcode+'- Warranty Expiry Date is invalid' as ReturnMessage
				Return 
		   END 
		End
		--print @PurchasePrice
		If ((@PurchasePrice is not null and @PurchasePrice!=''))
		Begin
		  SET DATEFORMAT dmy;
		  SELECT @numberFormat= isnumeric(@PurchasePrice)
		  If @numberFormat=0
		   BEGIN 
		     SElect @Barcode+'- Purchase Price is invalid' as ReturnMessage
				Return 
		   END
		   Else
			BEGIN
				If convert(decimal(18,5),@PurchasePrice)<0
					BEGIN
						SElect @Barcode+'- Purchase Price should not be negative' as ReturnMessage
						Return	
					END
			END
		End 

		If ((@PurchaseDate is not null and @PurchaseDate!=''))
		Begin
		  SET DATEFORMAT dmy;
		  SELECT @DateFormat= ISDATE(@PurchaseDate)
		  If @DateFormat=0
		   BEGIN 
		     SElect @Barcode+'-  Purchase Date is invalid' as ReturnMessage
				Return 
		   END 
		End 
		If ((@InvoiceDate is not null and @InvoiceDate!=''))
		Begin
		  SET DATEFORMAT dmy;
		  SELECT @DateFormat= ISDATE(@InvoiceDate)
		  If @DateFormat=0
		   BEGIN 
		     SElect @Barcode+'- Invoice Date is invalid' as ReturnMessage
				Return 
		   END 
		End 

		if ((@PurchaseDate is null or @PurchaseDate=' ') and (@WarrantyExpiryDate is not null and  @WarrantyExpiryDate!=''))
					 Begin 
					      SElect 'Without Purchase date warranty expiry month should not be added For'+@Barcode as ReturnMessage
					      Return 
					 End 
					 else if ((@PurchaseDate is not null and @PurchaseDate!='') and (@WarrantyExpiryDate is not null and  @WarrantyExpiryDate!=''))
					 Begin 
					      Declare @res bit 
						  Select @res=ISDATE(@WarrantyExpiryDate)
						  IF @res=1 
						  begin
						   IF CONVERT(DATETIME,@PURCHASEDATE,103)>CONVERT(DATETIME,@WarrantyExpiryDate,103)
						   Begin 
					      SElect 'warranty expiry month should be in greater then Purchase Date  '+@Barcode as ReturnMessage
					      Return
						  End
						  End
						 
					 End 
	
	 Declare @catL1 int
					 if @L1CategoryCode is not null or @L1CategoryCode !=''
					 Begin
						 if  exists(select CategoryCode from CategoryTable where CategoryCode=@L1CategoryCode and ParentCategoryID is null and StatusID=1)
						 Begin 
							 select @catL1=CategoryID from CategoryTable where CategoryCode=@L1CategoryCode and ParentCategoryID is null and StatusID=1
						 End 
						 else
						  Begin 
							  SElect 'given L1 Category code is not avaliable in db  '+@Barcode as ReturnMessage
							  Return
						  End 
					 End 
					 print @catL1
					 IF (@CategoryCode is not NULL and @CategoryCode!='')
					BEGIN 		 
					  IF EXISTS(SELECT CATEGORYCODE FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1 and ParentCategoryID=@catL1)
					  BEGIN 		 
						SELECT @CATEGORYID=CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1  and ParentCategoryID=@catL1
					  END
					  ELSE 
					  BEGIN
		  
					   IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
					   BEGIN 
					   print 'test'
						INSERT INTO CATEGORYTABLE(CATEGORYCODE,STATUSID,CREATEDBY,CREATEDDATETIME,CategoryName,ParentCategoryID) 
						VALUES(@CATEGORYCODE,1,@USERID,GETDATE(),@CategoryCode,@catL1)			
						SET @CATEGORYID = SCOPE_IDENTITY()			
						INSERT INTO CATEGORYDESCRIPTIONTABLE (CATEGORYID,CATEGORYDESCRIPTION,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
						Select @CATEGORYID,@CATEGORYCODE,LanguageID,@USERID,GETDATE() from LanguageTable
						END 
						ELSE 
						BEGIN
			  
						   INSERT INTO @MESSAGETABLE(TEXT)VALUES('CategoryCode Cant allow Create Data')
						END 
					  END 
					END 
					Declare @locL1 int,@LocL2 int
					 if @L1LocationCode is not null or @L1LocationCode !=''
					 Begin
						 if  exists(select locationcode from LocationTable where LocationCode=@L1LocationCode and ParentLocationID is null and StatusID=1)
						 Begin 
							 select @locL1=locationID from LocationTable where LocationCode=@L1LocationCode and ParentLocationID is null and StatusID=1
						 End 
						 else
						  Begin 
							  SElect 'given L1 Location code is not avaliable in db  '+@Barcode as ReturnMessage
							  Return
						  End 
					 End 
					 if @L2LocationCode is not null or @L2LocationCode !=''
					 Begin
						 if  exists(select locationcode from LocationTable where LocationCode=@L2LocationCode and ParentLocationID =@locL1 and StatusID=1)
						 Begin 
							 select @locL2=locationID from LocationTable where LocationCode=@L2LocationCode and ParentLocationID =@locL1 and StatusID=1
						 End 
						 else
						  Begin 
							  SElect 'given L2 Location code is not avaliable in db  '+@Barcode as ReturnMessage
							  Return
						  End 
					 End 
					  IF (@LOCATIONCODE is not NULL and @LOCATIONCODE!='')
						BEGIN 
						 IF EXISTS(SELECT LOCATIONCODE FROM LocationNewHierarchicalView WHERE LOCATIONCODE=@LOCATIONCODE and Level2ID=@LocL2 and StatusID=1)
						  BEGIN  
							SELECT @LOCATIONID=LOCATIONID FROM LocationNewHierarchicalView WHERE LOCATIONCODE=@LOCATIONCODE and StatusID=1 and Level2ID=@LocL2
						  END
						  ELSE 
						  BEGIN
							IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
						   BEGIN 
							INSERT INTO LOCATIONTABLE(LOCATIONCODE,STATUSID,CREATEDBY,CREATEDDATETIME,LocationName,ParentLocationID) 
							VALUES(@LOCATIONCODE,1,@USERID,GETDATE(),@LocationCode,@LocL2)
			
							SET @LOCATIONID = SCOPE_IDENTITY()
			
							INSERT INTO LOCATIONDESCRIPTIONTABLE (LOCATIONID,LOCATIONDESCRIPTION,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
							Select @LOCATIONID,@LOCATIONCODE,LanguageID,@USERID,GETDATE() from LanguageTable
							End 
							ELSE 
							BEGIN 
							INSERT INTO @MESSAGETABLE(TEXT)VALUES('LocationCode Cant allow Create Data')
							END 
						  END 
		END 
	Declare @ErrorID int 
	Declare @ErrorMsg nvarchar(max)
	If @ImportTypeID=1
	begin
		 exec Prc_AssetCreationValidation @userID,@CategoryCode,@LocationCode,null,null,null,@DepartmentCode,@SerialNo,@ManufacturerCode,null,'ExcelAssetInsert',@ErrorID output,@ErrorMsg output
		if @ErrorID>0
		Begin
			 Select @ErrorMsg
			 Return
		End 
		if upper(@BarcodeAutoGenerateEnable)='TRUE' and upper(@IsBarcodeSettingApplyImportAsset)='TRUE'
		begin 
		  set @Barcode='-';
		End 
		if UPPER(@BarcodeEqualAssetCode)='TRUE' and upper(@IsBarcodeSettingApplyImportAsset)='TRUE'
		Begin 
		set @AssetCode=@Barcode
		End 
	end 
	Else 
	Begin 
	  Declare @AssetID int 
	  Select @AssetID=AssetID from AssetTable where barcode=@Barcode


	  exec Prc_AssetModificationValidation @userID,@AssetID,@CategoryCode,@LocationCode,null,null,null,@DepartmentCode,@SerialNo,@ManufacturerCode,null,'ExcelAssetInsert',@ErrorID output,@ErrorMsg output
		
		if @ErrorID>0
		Begin
			 Select @ErrorMsg
			 Return
		End 
	End 
	
	----CategoryTable
	-- IF (@CategoryCode is not NULL and @CategoryCode!='')
	--	BEGIN 		 
	--	  IF EXISTS(SELECT CATEGORYCODE FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1)
	--	  BEGIN 		 
	--		SELECT @CATEGORYID=CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1 
	--	  END
	--	  ELSE 
	--	  BEGIN
		  
	--	   IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
	--	   BEGIN 
	--	    INSERT INTO CATEGORYTABLE(CATEGORYCODE,STATUSID,CREATEDBY,CREATEDDATETIME,CategoryName) 
	--		VALUES(@CATEGORYCODE,1,@USERID,GETDATE(),@CategoryCode)			
	--		SET @CATEGORYID = SCOPE_IDENTITY()			
	--		INSERT INTO CATEGORYDESCRIPTIONTABLE (CATEGORYID,CATEGORYDESCRIPTION,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
	--		Select @CATEGORYID,@CATEGORYCODE,LanguageID,@USERID,GETDATE() from LanguageTable
	--		END 
	--		ELSE 
	--		BEGIN
			  
	--		   INSERT INTO @MESSAGETABLE(TEXT)VALUES('CategoryCode Cant allow Create Data')
	--		END 
	--	  END 
	--	END 
		
	----LOCATUION MASTER
	-- IF (@LOCATIONCODE is not NULL and @LOCATIONCODE!='')
	--	BEGIN 
	--	 IF EXISTS(SELECT LOCATIONCODE FROM LOCATIONTABLE WHERE LOCATIONCODE=@LOCATIONCODE and StatusID=1)
	--	  BEGIN  
	--		SELECT @LOCATIONID=LOCATIONID FROM LOCATIONTABLE WHERE LOCATIONCODE=@LOCATIONCODE and StatusID=1
	--	  END
	--	  ELSE 
	--	  BEGIN
	--	    IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
	--	   BEGIN 
	--	    INSERT INTO LOCATIONTABLE(LOCATIONCODE,STATUSID,CREATEDBY,CREATEDDATETIME,LocationName) 
	--		VALUES(@LOCATIONCODE,1,@USERID,GETDATE(),@LocationCode)
			
	--		SET @LOCATIONID = SCOPE_IDENTITY()
			
	--		INSERT INTO LOCATIONDESCRIPTIONTABLE (LOCATIONID,LOCATIONDESCRIPTION,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
	--		Select @LOCATIONID,@LOCATIONCODE,LanguageID,@USERID,GETDATE() from LanguageTable
	--		End 
	--		ELSE 
	--		BEGIN 
	--		INSERT INTO @MESSAGETABLE(TEXT)VALUES('LocationCode Cant allow Create Data')
	--		END 
	--	  END 
	--	END 
	 --DEPARTMENT MASTER 
	 IF(@DEPARTMENTCODE!=''  AND @DEPARTMENTCODE is not NULL)
		  BEGIN 
			IF EXISTS(SELECT DEPARTMENTCODE FROM DEPARTMENTTABLE WHERE DEPARTMENTCODE=@DEPARTMENTCODE and StatusID=1)
			BEGIN 
			  SELECT @DEPARTMENTID=DEPARTMENTID FROM DEPARTMENTTABLE WHERE DEPARTMENTCODE=@DEPARTMENTCODE  and StatusID=1
			END 
			ELSE 
			BEGIN 
			  IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
		   BEGIN 	
				INSERT INTO DEPARTMENTTABLE(DEPARTMENTCODE,STATUSID,CREATEDBY,CREATEDDATETIME,DepartmentName)
				VALUES(@DEPARTMENTCODE,1,@USERID,GETDATE(),@DepartmentCode)

				SET @DEPARTMENTID = SCOPE_IDENTITY()
				INSERT INTO DEPARTMENTDESCRIPTIONTABLE(DEPARTMENTID,DEPARTMENTDESCRIPTION,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
				select @DEPARTMENTID,@DEPARTMENTCODE,LanguageID,@USERID,GETDATE() from LanguageTable
				End 
				ELSE 
				BEGIN
				   INSERT INTO @MESSAGETABLE(TEXT)VALUES('DepartmentCode Cant allow Create Data')
				END 
			END 
		  END 
	 --SECTION MASTER
     IF (@SECTIONCODE!='' AND @SECTIONCODE is not NULL)
			BEGIN 
			 IF EXISTS (SELECT SECTIONCODE FROM SECTIONTABLE WHERE SECTIONCODE=@SECTIONCODE AND 
								DEPARTMENTID in (SELECT DEPARTMENTID FROM DEPARTMENTTABLE WHERE DEPARTMENTCODE=@DEPARTMENTCODE and StatusID=1) and StatusID=1)
			 BEGIN
				SELECT @SECTIONID=SECTIONID FROM SECTIONTABLE WHERE SECTIONCODE=@SECTIONCODE AND DEPARTMENTID in (SELECT DEPARTMENTID FROM DEPARTMENTTABLE WHERE DEPARTMENTCODE=@DEPARTMENTCODE and StatusID=1) and StatusID=1
			    
			 END
			 ELSE 
			 BEGIN
			   IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
		   BEGIN 	
				 INSERT INTO SECTIONTABLE (SECTIONCODE,STATUSID,CREATEDBY,CREATEDDATETIME,DEPARTMENTID,SectionName) 
				 VALUES(@SECTIONCODE,1,@USERID,GETDATE(),@DEPARTMENTID,@SectionCode)

				  SET @SECTIONID = SCOPE_IDENTITY()
				  
				  INSERT INTO SECTIONDESCRIPTIONTABLE(SECTIONID,SECTIONDESCRIPTION,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
				  Select @SECTIONID,@SECTIONCODE,LanguageID,@USERID,GETDATE() from LanguageTable
				End 
				ELSE 
			BEGIN
			   INSERT INTO @MESSAGETABLE(TEXT)VALUES('SectionCode Cant allow Create Data')
			END 

			 END 
			END 
	-- CUSTODIAN MASTER 
	 IF @CUSTODIANCODE is not NULL and @CUSTODIANCODE!=''
			BEGIN
			  IF EXISTS(SELECT PERSONCODE FROM PERSONTABLE WHERE PERSONCODE=@CUSTODIANCODE AND (USERTYPEID=2 OR USERTYPEID=3) and StatusID=1)
			  BEGIN
				SELECT @CUSTODIANID=PERSONID FROM PERSONTABLE WHERE PERSONCODE=@CUSTODIANCODE AND  (USERTYPEID=2 OR USERTYPEID=3) and StatusID=1
			  END 
			  ELSE 
			  BEGIN 
			    IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
		   BEGIN 	

			    INSERT INTO User_LoginUserTable(UserName,Password,PasswordSalt,LastActivityDate,LastLoginDate,LastLoggedInDate,ChangePasswordAtNextLogin,IsLockedOut,IsDisabled,IsApproved)
			VALUES(@CUSTODIANCODE ,'Mod6/JMHjHeKXDkUK/zd7PfLlJg=','BZxI8E2lroNt28VMhZsyyaNZha8=',GETDATE(),GETDATE(),GETDATE(),1,0,0,1 )

			INSERT INTO PersonTable(PersonID, PersonFirstName, PersonLastName, PersonCode, AllowLogin, DepartmentID, UserTypeID, StatusID, Culture,CreatedBy,CreatedDateTime,CreatedFrom) 
				VALUES(@@IDENTITY, @CUSTODIANCODE, @CUSTODIANCODE, @CUSTODIANCODE, 0, @DepartmentID, 2, 1, 'en-GB', @UserID,GETDATE(),'APP')
			 END 
			ELSE 
			BEGIN
			   INSERT INTO @MESSAGETABLE(TEXT)VALUES('Custodiancode Cant allow Create Data')
			END 
			  END 

			END
	--ASSET cONDTION MASTER
	 IF @ASSETCONDITIONCODE is not NULL AND @ASSETCONDITIONCODE!=''
			BEGIN 
			 IF EXISTS (SELECT ASSETCONDITIONCODE FROM ASSETCONDITIONTABLE WHERE ASSETCONDITIONCODE=@ASSETCONDITIONCODE and StatusID=1)
			 BEGIN
				SELECT @ASSETCONDITIONID=ASSETCONDITIONID FROM ASSETCONDITIONTABLE WHERE ASSETCONDITIONCODE=@ASSETCONDITIONCODE  and StatusID=1
			 END 
			 ELSE 
			 BEGIN
			   IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
		   BEGIN 
				 INSERT INTO ASSETCONDITIONTABLE(ASSETCONDITIONCODE,STATUSID,CREATEDBY,CREATEDDATETIME,AssetConditionName)
				 VALUES(@ASSETCONDITIONCODE,1,@USERID,GETDATE(),@AssetConditionCode)

				  SET @ASSETCONDITIONID = SCOPE_IDENTITY()

				  INSERT INTO ASSETCONDITIONDESCRIPTIONTABLE(ASSETCONDITIONID,ASSETCONDITIONDESCRIPTION,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
				  Select @ASSETCONDITIONID,@ASSETCONDITIONCODE,LanguageID,@USERID,GETDATE() from LanguageTable
				  END 
			ELSE 
			BEGIN
			   INSERT INTO @MESSAGETABLE(TEXT)VALUES('AssetConditionCode Cant allow Create Data')
			END 
			 END 
			END 
	 -- SUPPLIER TABLE 
	 IF @SUPPLIERCODE is not NULL AND @SUPPLIERCODE!='' 
			 BEGIN
			 IF EXISTS(SELECT PartyCode FROM PartyTable WHERE PartyCode=@SUPPLIERCODE and StatusID=1)
			 BEGIN 
				SELECT @SUPPLIERID =partyID FROM PartyTable WHERE PartyCode=@SUPPLIERCODE and StatusID=1 and PartyTypeID=1
			 END 
			 ELSE 
			 BEGIN 
			   IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
		   BEGIN 	
				INSERT INTO PartyTable(PartyCode,STATUSID,CREATEDBY,CREATEDDATETIME,PartyName,PartyTypeID)
				VALUES(@SUPPLIERCODE,1,@USERID,GETDATE(),@SupplierCode,(select PartyTypeID from PartyTypeTable where PartyType='Supplier'))
				
				SET @SUPPLIERID = SCOPE_IDENTITY()
				--INSERT INTO part(SUPPLIERID,SUPPLIERDESCRIPTION,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
				--Select @SUPPLIERID,@SUPPLIERCODE,LanguageID,@USERID,GETDATE() from LanguageTable
				END 
			ELSE 
			BEGIN
			   INSERT INTO @MESSAGETABLE(TEXT)VALUES('SupplierCode Cant allow Create Data')
			END 
				END
			 END 
	 
	 --AssetDescription
	 declare @assetDescription nvarchar(max)
	 IF(@VirtualBarcode is not NULL and @VirtualBarcode!='')                                                                
	  BEGIN 
		IF EXISTS(SELECT ProductName FROM ProductTable WHERE virtualBarcode=@VirtualBarcode
		AND CATEGORYID in (SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1) and StatusID=1)
			BEGIN
			--print 'ProductID'
			SELECT @PRODUCTID=ProductID,@assetDescription=ProductName FROM ProductTable WHERE virtualBarcode=@VirtualBarcode
			AND CATEGORYID in (SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1) and StatusID=1
			--FROM PRODUCTVIEW WHERE replace(productName,' ','')=@ProductCode AND LANGUAGEID=1 and StatusID=1
			--AND CATEGORYID in (SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1)
			END 
		ELSE 
		BEGIN
		IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
		BEGIN 
		if not exists(select CategoryCode from Categorytable where CategoryCode=@CategoryCode)
		Begin 
		insert into Categorytable(CategoryCode,StatusID,CreatedBy,CreatedDateTime,CategoryName) 
			values(@CategoryCode,1,@USERID,getdate(),@CategoryCode)
			SET @CategoryID = SCOPE_IDENTITY()
				 
			insert into CategoryDescriptionTable(CategoryID,CategoryDescription,languageID,CreatedBy,CreatedDateTime)
			select @CategoryID,@CategoryCode,LanguageID,@USERID,getdate() from languageTable
				   
			INSERT INTO PRODUCTTABLE(PRODUCTCODE,STATUSID,CATEGORYID,CREATEDBY,CREATEDDATETIME,ProductName,VirtualBarcode)
		VALUES(@ProductCode,1,@CATEGORYID,@USERID,getdate(),@ProductCode,@ProductCode)
				 
			SET @ProductID = SCOPE_IDENTITY()
				 
			INSERT INTO PRODUCTDESCRIPTIONTABLE (PRODUCTDESCRIPTION,PRODUCTID,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
			Select @ProductCode,@ProductID,LanguageID,@USERID,getdate() from LanguageTable
		End 
		Else 
		Begin 
		SELECT @CATEGORYID= CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE
		END  

		INSERT INTO PRODUCTTABLE(PRODUCTCODE,STATUSID,CATEGORYID,CREATEDBY,CREATEDDATETIME,ProductName,VirtualBarcode)
		VALUES(@ProductCode,1,@CATEGORYID,@USERID,getdate(),@ProductCode,@ProductCode)
			SET @ProductID = SCOPE_IDENTITY()
			set @assetDescription=@VirtualBarcode
			INSERT INTO PRODUCTDESCRIPTIONTABLE (PRODUCTDESCRIPTION,PRODUCTID,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
			Select @VirtualBarcode,@ProductID,LanguageID,@USERID,getdate() from LanguageTable
			END 
	ELSE 
	BEGIN
		INSERT INTO @MESSAGETABLE(TEXT)VALUES('ProductCode Cant allow Create Data')
	END 
		END 
	END

	 -- MANUFACTURER MASTER 
	 IF @MANUFACTURERCODE is not NULL AND @MANUFACTURERCODE!=''
			BEGIN 
				if upper(@ManufacturerCategoryMapping)='TRUE'
				BEGIN 									
			   IF EXISTS (SELECT MANUFACTURERCODE FROM MANUFACTURERTABLE WHERE MANUFACTURERCODE=@MANUFACTURERCODE AND 
						CATEGORYID=(SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1) and StatusID=1)
			   BEGIN 
					SELECT @MANUFACTURERID =MANUFACTURERID FROM MANUFACTURERTABLE WHERE MANUFACTURERCODE=@MANUFACTURERCODE AND 
						CATEGORYID=(SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1) and StatusID=1
			   END 
			   ELSE 
			   BEGIN 

			   IF @CATEGORYCODE is not NULL and @CATEGORYCODE!='' 
			   BEGIN 
				 if not exists(select CategoryCode from Categorytable where CategoryCode=@CategoryCode and StatusID=1)
				Begin 
				  IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
		   BEGIN 	
				insert into Categorytable(CategoryCode,StatusID,CreatedBy,CreatedDateTime,CategoryName) 
				   values(@CategoryCode,1,@USERID,getdate(),@CategoryCode)
				   SET @CategoryID = SCOPE_IDENTITY()
				      insert into CategoryDescriptionTable(CategoryID,CategoryDescription,languageID,CreatedBy,CreatedDateTime)
				   Select @CategoryID,@CategoryCode,LanguageID,@USERID,getdate() from LanguageTable
				   End 
				   Else 
				   BEGIN
			   INSERT INTO @MESSAGETABLE(TEXT)VALUES('CategoryCode Cant allow Create Data')
				END 
				End 
				Else 
				Begin 
				SELECT @CATEGORYID= CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1
				END  
				END 
				  IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
		   BEGIN 
				INSERT INTO MANUFACTURERTABLE(MANUFACTURERCODE,StatusID,CREATEDBY,CREATEDDATETIME,CATEGORYID,ManufacturerName)
				VALUES(@MANUFACTURERCODE,1,@USERID,getdate(),@CategoryID,@ManufacturerCode)
				SET @MANUFACTURERID = SCOPE_IDENTITY()
				INSERT INTO MANUFACTURERDESCRIPTIONTABLE(MANUFACTURERID,MANUFACTURERDESCRIPTION,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
				Select @ManufacturerID,@MANUFACTURERCODE,LanguageID,@USERID,GETDATE() from LanguageTable
				END 
			ELSE 
			BEGIN
			   INSERT INTO @MESSAGETABLE(TEXT)VALUES('ManufacturerCode Cant allow Create Data')
			END 
			   END 
			  END 
			  ELSE 
			  BEGIN 
				  IF EXISTS (SELECT MANUFACTURERCODE FROM MANUFACTURERTABLE WHERE MANUFACTURERCODE=@MANUFACTURERCODE and StatusID=1)
			   BEGIN 
					SELECT @MANUFACTURERID =MANUFACTURERID FROM MANUFACTURERTABLE WHERE MANUFACTURERCODE=@MANUFACTURERCODE  and StatusID=1
			   END 
			   ELSE 
			   BEGIN 
			     IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
		   BEGIN 	

					INSERT INTO MANUFACTURERTABLE(MANUFACTURERCODE,StatusID,CREATEDBY,CREATEDDATETIME,ManufacturerName)
					VALUES(@MANUFACTURERCODE,1,@USERID,getdate(),@ManufacturerCode)
					SET @MANUFACTURERID = SCOPE_IDENTITY()
					INSERT INTO MANUFACTURERDESCRIPTIONTABLE(MANUFACTURERID,MANUFACTURERDESCRIPTION,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
					Select @ManufacturerID,@MANUFACTURERCODE,LanguageID,@USERID,GETDATE() from LanguageTable				
END 
			ELSE 
			BEGIN
			   INSERT INTO @MESSAGETABLE(TEXT)VALUES('ManufacturerCode Cant allow Create Data')
			END 
			   END 
			  END 
			END 
			-- MODEL MASTER 
	 IF @MODELCODE is not NULL AND @MODELCODE!=''
			BEGIN
			if upper(@CategoryManufacturerModelMapping)='TRUE' 
			BEGIN 
			
			 IF EXISTS(SELECT MODELCODE FROM  MODELTABLE WHERE MODELCODE=@MODELCODE and StatusID=1 AND CATEGORYID=(SELECT CATEGORYID FROM CATEGORYTABLE C WHERE C.CATEGORYCODE=@CATEGORYCODE and StatusID=1) 
			    AND MANUFACTURERID=(SELECT MANUFACTURERID FROM MANUFACTURERTABLE WHERE  MANUFACTURERCODE=@MANUFACTURERCODE and CATEGORYID=(SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1)  and StatusID=1))
				BEGIN 
				
				  SELECT @MODELID=MODELID FROM MODELTABLE WHERE CATEGORYID=(SELECT CATEGORYID FROM CATEGORYTABLE C WHERE C.CATEGORYCODE=@CATEGORYCODE and StatusID=1) 
			    AND MANUFACTURERID=(SELECT MANUFACTURERID FROM MANUFACTURERTABLE WHERE  MANUFACTURERCODE=@MANUFACTURERCODE and CATEGORYID=(SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1)  and StatusID=1) and StatusID=1
				END 
				ELSE 
				BEGIN 
				 IF @CATEGORYCODE!=NULL and @CATEGORYCODE!='' 
			   BEGIN 
				 if not exists(select CategoryCode from Categorytable where CategoryCode=@CategoryCode and StatusID=1)
				Begin 
				  IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
		   BEGIN 	
				insert into Categorytable(CategoryCode,StatusID,CreatedBy,CreatedDateTime,CategoryName) 
				   values(@CategoryCode,1,@USERID,getdate(),@CategoryCode)
				   SET @CategoryID = SCOPE_IDENTITY()
				      insert into CategoryDescriptionTable(CategoryID,CategoryDescription,languageID,CreatedBy,CreatedDateTime)
				   Select @CategoryID,@CategoryCode,LanguageID,@USERID,getdate() from LanguageTable
				   End 
				   ELSE 
					BEGIN
					   INSERT INTO @MESSAGETABLE(TEXT)VALUES('CategoryCode Cant allow Create Data')
					END		
				End 
				Else 
				Begin 
				SELECT @CATEGORYID= CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE
				END  
				END 
				IF @MANUFACTURERCODE is not NULL AND @MANUFACTURERCODE!='' 
				BEGIN 
					IF EXISTS(SELECT MANUFACTURERCODE FROM MANUFACTURERTABLE WHERE MANUFACTURERCODE=@MANUFACTURERCODE AND 
					CATEGORYID in (SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1) and StatusID=1)
					BEGIN 
					 SELECT @MANUFACTURERID=MANUFACTURERID FROM MANUFACTURERTABLE WHERE MANUFACTURERCODE=@MANUFACTURERCODE and StatusID=1 AND 
					CATEGORYID in (SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1)
					END 
					ELSE 
					BEGIN 
					  IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
		   BEGIN 	
					  INSERT INTO MANUFACTURERTABLE(MANUFACTURERCODE,CATEGORYID,STATUSID,CREATEDBY,CREATEDDATETIME,ManufacturerName)
					  VALUES(@MANUFACTURERCODE,@CATEGORYID,1,@USERID,GETDATE(),@ManufacturerCode)
					   SET @MANUFACTURERID = SCOPE_IDENTITY()
					   INSERT INTO MANUFACTURERDESCRIPTIONTABLE(MANUFACTURERID,MANUFACTURERDESCRIPTION,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
					   Select @MANUFACTURERID,@MANUFACTURERCODE,LanguageID,@USERID,GETDATE() from LanguageTable
					   End 
					   Else 
					   BEGIN
						  INSERT INTO @MESSAGETABLE(TEXT)VALUES('ManufacturerCode Cant allow Create Data')
						END		
					END 
					  IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
		   BEGIN 
					INSERT INTO MODELTABLE(MODELCODE,STATUSID,CREATEDBY,CREATEDDATETIME,MANUFACTURERID,CATEGORYID,ModelName)
					VALUES(@MODELCODE,1,@USERID,GETDATE(),@MANUFACTURERID,@CATEGORYID,@ModelCode)
					SET @MODELID=SCOPE_IDENTITY()
					INSERT INTO MODELDESCRIPTIONTABLE(MODELID,MODELDESCRIPTION,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
					Select @MODELID,@MODELCODE,LanguageID,@USERID,GETDATE() from LanguageTable
					End 
					Else 
					BEGIN
			   INSERT INTO @MESSAGETABLE(TEXT)VALUES('ModelCode Cant allow Create Data')
			END
				END 
				END 
				END 
				ELSE 
				BEGIN 
				 IF EXISTS(SELECT MODELCODE FROM  MODELTABLE WHERE MODELCODE=@MODELCODE  and StatusID=1
			    AND MANUFACTURERID=(SELECT MANUFACTURERID FROM MANUFACTURERTABLE WHERE  MANUFACTURERCODE=@MANUFACTURERCODE and StatusID=1))
				BEGIN 
				  SELECT @MODELID=MODELID FROM MODELTABLE WHERE  MODELCODE=@MODELCODE  AND 
			     MANUFACTURERID=(SELECT MANUFACTURERID FROM MANUFACTURERTABLE WHERE  MANUFACTURERCODE=@MANUFACTURERCODE and StatusID=1) and StatusID=1
				END 
				ELSE 
				BEGIN 
					IF @MANUFACTURERCODE is not NULL AND @MANUFACTURERCODE!='' 
				BEGIN 
					IF EXISTS(SELECT MANUFACTURERCODE FROM MANUFACTURERTABLE WHERE MANUFACTURERCODE=@MANUFACTURERCODE and StatusID=1 )
					BEGIN 
					 SELECT @MANUFACTURERID=MANUFACTURERID FROM MANUFACTURERTABLE WHERE MANUFACTURERCODE=@MANUFACTURERCODE  and StatusID=1
					END 
					ELSE 
					BEGIN 
					  IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
					 BEGIN 	
					  INSERT INTO MANUFACTURERTABLE(MANUFACTURERCODE,STATUSID,CREATEDBY,CREATEDDATETIME,ManufacturerName)
					  VALUES(@MANUFACTURERCODE,1,@USERID,GETDATE(),@ManufacturerCode)
					   SET @MANUFACTURERID = SCOPE_IDENTITY()
					   INSERT INTO MANUFACTURERDESCRIPTIONTABLE(MANUFACTURERID,MANUFACTURERDESCRIPTION,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
					   Select @MANUFACTURERID,@MANUFACTURERCODE,LanguageID,@USERID,GETDATE() from LanguageTable
					   End 					   
					ELSE 
					BEGIN
					   INSERT INTO @MESSAGETABLE(TEXT)VALUES('ManufacturerCode Cant allow Create Data')
					END		
					END 
					  IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
					 BEGIN 	
					INSERT INTO MODELTABLE(MODELCODE,STATUSID,CREATEDBY,CREATEDDATETIME,MANUFACTURERID,ModelName)
					VALUES(@MODELCODE,1,@USERID,GETDATE(),@MANUFACTURERID,@ModelCode)
					SET @MODELID=SCOPE_IDENTITY()
					INSERT INTO MODELDESCRIPTIONTABLE(MODELID,MODELDESCRIPTION,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
					Select @MODELID,@MODELCODE,LanguageID,@USERID,GETDATE() from LanguageTable
					End 
					Else 
					BEGIN
					 INSERT INTO @MESSAGETABLE(TEXT)VALUES('ModelCode Cant allow Create Data')
					END
				END 
				END 
			END 
			END 
			IF @ImportTypeID=1 
			BEGIN 
			 IF UPPER(@LocationMandatory)='TRUE' AND (@LOCATIONID IS NULL OR  @LOCATIONID ='') 
			 BEGIN 
				INSERT INTO @MESSAGETABLE(TEXT)VALUES('lOCATIONCODE MANDATORY DATA FOR'+@Barcode)
			 END 
			ELSE 
			BEGIN
		       If exists(select barcode from assettable where barcode=@barcode )
			   Begin 
			     INSERT INTO @MESSAGETABLE(TEXT)VALUES('Already Exists this barcode :'+@Barcode)
			   End 
			   Else 
			   Begin 
			   print 'Insert'
			      If((@Barcode is not null and @Barcode!='') and (@AssetCode is not null and @AssetCode!='') and (@ProductID is not null and @ProductID!='') and (@CategoryID is not null and @CategoryID!='')
				  and (@ASSETDESCRIPTION is not null and @ASSETDESCRIPTION!=''))

				  Begin 
					INSERT INTO ASSETTABLE (ASSETCODE,BARCODE,DEPARTMENTID,SECTIONID,CUSTODIANID,CATEGORYID,LOCATIONID,PRODUCTID,MANUFACTURERID,MODELID,SUPPLIERID,ASSETCONDITIONID,PurchasePrice,
					PONUMBER,REFERENCECODE,SERIALNO,ASSETDESCRIPTION,DELIVERYNOTE,RFIDTAGCODE,INVOICENO,VOUCHERNO,MAKE,CAPACITY,MAPPEDASSETID,RECEIPTNUMBER,PURCHASEDATE,ComissionDate,WarrantyExpiryDate,
					STATUSID,COMPANYID,CreatedBy,CreatedDateTime,AssetApproval,CreateFromHHT,ERPUpdateType,DepreciationFlag,Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6,Attribute7,Attribute8,Attribute9,Attribute10,Attribute11,Attribute12,
					Attribute13,Attribute14,Attribute15,Attribute16,AssetRemarks,QFAssetCode,A.Attribute17,A.Attribute18,Attribute19,Attribute20,Attribute21,Attribute22,Attribute23,Attribute24,Attribute25,Attribute26,
Attribute27,Attribute28,Attribute29,Attribute30,Attribute31,Attribute32,Attribute33,Attribute34,Attribute35,Attribute36,Attribute37,Attribute38,Attribute39,Attribute40,UploadedDocumentPath,ImportExcelFileName)
					
					VALUES( CASE WHEN UPPER(@BarcodeAutoGenerateEnable)='TRUE' and  UPPER(@IsBarcodeSettingApplyImportAsset)='TRUE' THEN '-' ELSE 
					CASE WHEN  UPPER(@BarcodeEqualAssetCode)='TRUE' and  UPPER(@IsBarcodeSettingApplyImportAsset)='TRUE' THEN @Barcode ELSE @AssetCode END END ,
					CASE WHEN UPPER(@BarcodeAutoGenerateEnable)='TRUE' and  UPPER(@IsBarcodeSettingApplyImportAsset)='TRUE' THEN '-' ELSE @Barcode END
					,@DepartmentID,@SECTIONID,@CUSTODIANID,@CATEGORYID,@LOCATIONID,@PRODUCTID,@MANUFACTURERID,@MODELID,@SUPPLIERID,@ASSETCONDITIONID,case when @PurchasePrice is not null and  @PurchasePrice!=' '  then convert(decimal(18,5),@PurchasePrice) else 0 end ,@PONUMBER,@REFERENCECODE,@SERIALNO,
					@assetDescription,@DELIVERYNOTE,@RFIDTAGCODE,@INVOICENO,@VOUCHERNO,@MAKE,@CAPACITY,@MAPPEDASSETID,@RECEIPTNUMBER,case when @PURCHASEDATE is not null then CONVERT(DATETIME,@PURCHASEDATE,103) else NULL end ,
					case when @CommissionDate is not null then CONVERT(DATETIME,@CommissionDate,103) else null end , case when @WarrantyExpiryDate is not null then CONVERT(DATETIME,@WarrantyExpiryDate,103) else NULL end ,
					1,@companyID,@userID,getdate(),1,0,0,0,@Attribute1,@Attribute2,@Attribute3,@Attribute4,@Attribute5,
			@Attribute6,@Attribute7,@Attribute8,@Attribute9,@Attribute10,@Attribute11,@Attribute12,
			@Attribute13,@Attribute14,@Attribute15,@Attribute16,@AssetRemarks,@QFAssetCode,@Attribute17,@Attribute18,@Attribute19,@Attribute20,@Attribute21,@Attribute22,@Attribute23,@Attribute24,@Attribute25,@Attribute26,
@Attribute27,@Attribute28,@Attribute29,@Attribute30,@Attribute31,@Attribute32,@Attribute33,@Attribute34,@Attribute35,@Attribute36,@Attribute37,@Attribute38,@Attribute39,@Attribute40,@UploadedDocumentPath,@ImportExcelFileName)
			End 
			Else 
			Begin 
			  INSERT INTO @MESSAGETABLE(TEXT)VALUES('Mandatory field are passed with empty '+@Barcode)
			End 
			 End 
			END 
			END 
			ELSE 
			BEGIN 
			IF EXISTS (SELECT barcode FROM dbo.AssetTable WHERE Barcode=@Barcode)
			BEGIN
			   If exists(select barcode  FROM dbo.AssetTable WHERE Barcode=@Barcode and statusID=1)
			   Begin 
				UPDATE ASSETTABLE SET
				--ASSETCODE=CASE WHEN UPPER(@BarcodeEqualAssetCode)='TRUE' THEN ISNULL(@Barcode,ASSETCODE) ELSE ISNULL(@ASSETCODE,ASSETCODE) END,
				ASSETCODE=CASE WHEN @ASSETCODE is null or @ASSETCODE='' then AssetCode else @ASSETCODE end,
				BARCODE=ISNULL(@Barcode,BARCODE),
				DEPARTMENTID=ISNULL(@DEPARTMENTID,DEPARTMENTID),
				SECTIONID=ISNULL(@SECTIONID,SECTIONID),
				CUSTODIANID=ISNULL(@CUSTODIANID,CUSTODIANID),
				CATEGORYID=ISNULL(@CATEGORYID,CATEGORYID),
				LOCATIONID=ISNULL(@LOCATIONID,LOCATIONID),
				PRODUCTID=ISNULL(@PRODUCTID,PRODUCTID),
				MANUFACTURERID=ISNULL(@MANUFACTURERID,MANUFACTURERID),
				MODELID=ISNULL(@MODELID,MODELID),
				SUPPLIERID=ISNULL(@SUPPLIERID,SUPPLIERID),
				ASSETCONDITIONID=ISNULL(@ASSETCONDITIONID,ASSETCONDITIONID),
				PONUMBER=CASE WHEN @PONUMBER IS NULL OR RTRIM(LTRIM(@PONUMBER))='' THEN PONumber ELSE @PONUMBER END ,
				REFERENCECODE=CASE WHEN @REFERENCECODE IS NULL OR RTRIM(LTRIM(@REFERENCECODE))='' THEN REFERENCECODE ELSE @REFERENCECODE END ,
				SERIALNO=CASE WHEN @SERIALNO IS NULL OR RTRIM(LTRIM(@SERIALNO))='' THEN SERIALNO ELSE @SERIALNO END ,
				ASSETDESCRIPTION=CASE WHEN @VirtualBarcode IS NULL OR RTRIM(LTRIM(@VirtualBarcode))='' THEN ASSETDESCRIPTION ELSE @assetDescription END ,
				DELIVERYNOTE=CASE WHEN @DELIVERYNOTE IS NULL OR RTRIM(LTRIM(@DELIVERYNOTE))='' THEN DELIVERYNOTE ELSE @DELIVERYNOTE END,
				RFIDTAGCODE=CASE WHEN @RFIDTAGCODE IS NULL OR RTRIM(LTRIM(@RFIDTAGCODE))='' THEN RFIDTAGCODE ELSE @RFIDTAGCODE END,
				INVOICENO=CASE WHEN @INVOICENO IS NULL OR RTRIM(LTRIM(@INVOICENO))='' THEN INVOICENO ELSE @INVOICENO END,
				VOUCHERNO=CASE WHEN @VOUCHERNO IS NULL OR RTRIM(LTRIM(@VOUCHERNO))='' THEN VOUCHERNO ELSE @VOUCHERNO END,
				MAKE=CASE WHEN @MAKE IS NULL OR RTRIM(LTRIM(@MAKE))='' THEN MAKE ELSE @MAKE END,
				CAPACITY=CASE WHEN @CAPACITY IS NULL OR RTRIM(LTRIM(@CAPACITY))='' THEN CAPACITY ELSE @CAPACITY END,
				MAPPEDASSETID=CASE WHEN @MAPPEDASSETID IS NULL OR RTRIM(LTRIM(@MAPPEDASSETID))='' THEN MAPPEDASSETID ELSE @MAPPEDASSETID END,
				RECEIPTNUMBER=CASE WHEN @RECEIPTNUMBER IS NULL OR RTRIM(LTRIM(@RECEIPTNUMBER))='' THEN RECEIPTNUMBER ELSE @RECEIPTNUMBER END,
				PURCHASEDATE=CASE WHEN @PURCHASEDATE IS NULL OR RTRIM(LTRIM(@PURCHASEDATE))='' THEN PURCHASEDATE ELSE CONVERT(DATETIME,@PURCHASEDATE,103)  END,
				ComissionDate=CASE WHEN @CommissionDate IS NULL OR RTRIM(LTRIM(@CommissionDate))='' THEN ComissionDate ELSE CONVERT(DATETIME,@CommissionDate,103) END,
				WarrantyExpiryDate=CASE WHEN @WarrantyExpiryDate IS NULL OR RTRIM(LTRIM(@WarrantyExpiryDate))='' THEN WarrantyExpiryDate ELSE CONVERT(DATETIME,@WarrantyExpiryDate,103) END,			
				Attribute1=CASE WHEN @Attribute1 IS NULL OR RTRIM(LTRIM(@Attribute1))='' THEN Attribute1 ELSE @Attribute1 END,
				Attribute2=CASE WHEN @Attribute2 IS NULL OR RTRIM(LTRIM(@Attribute2))='' THEN Attribute2 ELSE @Attribute2 END,
				Attribute3=CASE WHEN @Attribute3 IS NULL OR RTRIM(LTRIM(@Attribute3))='' THEN Attribute3 ELSE @Attribute3 END,
				Attribute4=CASE WHEN @Attribute4 IS NULL OR RTRIM(LTRIM(@Attribute4))='' THEN Attribute4 ELSE @Attribute4 END,
				Attribute5=CASE WHEN @Attribute5 IS NULL OR RTRIM(LTRIM(@Attribute5))='' THEN Attribute5 ELSE @Attribute5 END,
				Attribute6=CASE WHEN @Attribute6 IS NULL OR RTRIM(LTRIM(@Attribute6))='' THEN Attribute6 ELSE @Attribute6 END,
				Attribute7=CASE WHEN @Attribute7 IS NULL OR RTRIM(LTRIM(@Attribute7))='' THEN Attribute7 ELSE @Attribute7 END,
				Attribute8=CASE WHEN @Attribute8 IS NULL OR RTRIM(LTRIM(@Attribute8))='' THEN Attribute8 ELSE @Attribute8 END,
				Attribute9=CASE WHEN @Attribute9 IS NULL OR RTRIM(LTRIM(@Attribute9))='' THEN Attribute9 ELSE @Attribute9 END,
				Attribute10=CASE WHEN @Attribute10 IS NULL OR RTRIM(LTRIM(@Attribute10))='' THEN Attribute10 ELSE @Attribute10 END,
				Attribute11=CASE WHEN @Attribute11 IS NULL OR RTRIM(LTRIM(@Attribute11))='' THEN Attribute11 ELSE @Attribute11 END,
				Attribute12=CASE WHEN @Attribute12 IS NULL OR RTRIM(LTRIM(@Attribute12))='' THEN Attribute12 ELSE @Attribute12 END,
				Attribute13=CASE WHEN @Attribute13 IS NULL OR RTRIM(LTRIM(@Attribute13))='' THEN Attribute13 ELSE @Attribute13 END,
				Attribute14=CASE WHEN @Attribute14 IS NULL OR RTRIM(LTRIM(@Attribute14))='' THEN Attribute14 ELSE @Attribute14 END,
				Attribute15=CASE WHEN @Attribute15 IS NULL OR RTRIM(LTRIM(@Attribute15))='' THEN Attribute15 ELSE @Attribute15 END,
				Attribute16=CASE WHEN @Attribute16 IS NULL OR RTRIM(LTRIM(@Attribute16))='' THEN Attribute16 ELSE @Attribute16 END,
				AssetRemarks=CASE WHEN @AssetRemarks IS NULL OR RTRIM(LTRIM(@AssetRemarks))='' THEN AssetRemarks ELSE @AssetRemarks END,
				QFAssetCode=CASE WHEN @QFAssetCode IS NULL OR RTRIM(LTRIM(@QFAssetCode))='' THEN QFAssetCode ELSE @QFAssetCode END,
				LastModifiedDateTime=getdate(),
				LastModifiedBy=@UserID ,
				Attribute17=CASE WHEN @Attribute17 IS NULL OR RTRIM(LTRIM(@Attribute17))='' THEN Attribute17 ELSE @Attribute17 END,
				Attribute18=CASE WHEN @Attribute18 IS NULL OR RTRIM(LTRIM(@Attribute18))='' THEN Attribute18 ELSE Attribute18 END,
				Attribute19=CASE WHEN @Attribute19 IS NULL OR RTRIM(LTRIM(@Attribute19))='' THEN Attribute19 ELSE @Attribute19 END,
				Attribute20=CASE WHEN @Attribute20 IS NULL OR RTRIM(LTRIM(@Attribute20))='' THEN Attribute20 ELSE @Attribute20 END,
			    Attribute21=CASE WHEN @Attribute21 IS NULL OR RTRIM(LTRIM(@Attribute21))='' THEN Attribute21 ELSE @Attribute21 END,
				Attribute22=CASE WHEN @Attribute22 IS NULL OR RTRIM(LTRIM(@Attribute22))='' THEN Attribute22 ELSE @Attribute22 END,
				Attribute23=CASE WHEN @Attribute23 IS NULL OR RTRIM(LTRIM(@Attribute23))='' THEN Attribute23 ELSE @Attribute23 END,
				Attribute24=CASE WHEN @Attribute24 IS NULL OR RTRIM(LTRIM(@Attribute24))='' THEN Attribute24 ELSE @Attribute24 END,
				Attribute25=CASE WHEN @Attribute25 IS NULL OR RTRIM(LTRIM(@Attribute25))='' THEN Attribute25 ELSE @Attribute25 END,
				Attribute26=CASE WHEN @Attribute26 IS NULL OR RTRIM(LTRIM(@Attribute26))='' THEN Attribute26 ELSE @Attribute26 END,
				Attribute27=CASE WHEN @Attribute27 IS NULL OR RTRIM(LTRIM(@Attribute27))='' THEN Attribute27 ELSE @Attribute27 END,
				Attribute28=CASE WHEN @Attribute28 IS NULL OR RTRIM(LTRIM(@Attribute28))='' THEN Attribute28 ELSE @Attribute28 END,
				Attribute29=CASE WHEN @Attribute29 IS NULL OR RTRIM(LTRIM(@Attribute29))='' THEN Attribute29 ELSE @Attribute29 END,
				Attribute30=CASE WHEN @Attribute30 IS NULL OR RTRIM(LTRIM(@Attribute30))='' THEN Attribute30 ELSE @Attribute30 END,
				Attribute31=CASE WHEN @Attribute31 IS NULL OR RTRIM(LTRIM(@Attribute31))='' THEN Attribute31 ELSE @Attribute31 END,
				Attribute32=CASE WHEN @Attribute32 IS NULL OR RTRIM(LTRIM(@Attribute32))='' THEN Attribute32 ELSE @Attribute32 END,
				Attribute33=CASE WHEN @Attribute33 IS NULL OR RTRIM(LTRIM(@Attribute33))='' THEN Attribute33 ELSE @Attribute33 END,
				Attribute34=CASE WHEN @Attribute34 IS NULL OR RTRIM(LTRIM(@Attribute34))='' THEN Attribute34 ELSE @Attribute34 END,
				Attribute35=CASE WHEN @Attribute35 IS NULL OR RTRIM(LTRIM(@Attribute35))='' THEN Attribute35 ELSE @Attribute35 END,
				Attribute36=CASE WHEN @Attribute36 IS NULL OR RTRIM(LTRIM(@Attribute36))='' THEN Attribute36 ELSE @Attribute36 END,
				Attribute37=CASE WHEN @Attribute37 IS NULL OR RTRIM(LTRIM(@Attribute37))='' THEN Attribute37 ELSE @Attribute37 END,
				Attribute38=CASE WHEN @Attribute38 IS NULL OR RTRIM(LTRIM(@Attribute38))='' THEN Attribute38 ELSE @Attribute38 END,
				Attribute39=CASE WHEN @Attribute39 IS NULL OR RTRIM(LTRIM(@Attribute39))='' THEN Attribute39 ELSE @Attribute39 END,
				Attribute40=CASE WHEN @Attribute40 IS NULL OR RTRIM(LTRIM(@Attribute40))='' THEN Attribute40 ELSE @Attribute40 END,
				PurchasePrice=case when @PurchasePrice is not null and  @PurchasePrice!=' '  then convert(decimal(18,5),@PurchasePrice) else PurchasePrice end,
				importExcelFileName=case when @ImportExcelFileName is not null and  @ImportExcelFileName!=' '  then @ImportExcelFileName else importExcelFileName end
				 WHERE BARCODE=@BARCODE 
				 End 
				 Else 
				 Begin 
				  INSERT INTO @MESSAGETABLE(TEXT)VALUES(@Barcode + '- This barcode is not Active status')
				 End 
			 END
			 ELSE
			 BEGIN 
				 INSERT INTO @MESSAGETABLE(TEXT)VALUES(@Barcode + '- This barcode not available in Assettable')
			 END 
			END 
			--select * from @MESSAGETABLE

			Select @ReturnMessage = COALESCE(@ReturnMessage + ', ' + Text, Text)  from @MESSAGETABLE
			select @ReturnMessage as ReturnMessage
        
			 
End
go 

ALTER VIEW [dbo].[AssetNewView] 
AS 	
SELECT A.AssetID,A.AssetCode,A.Barcode,A.RFIDTagCode,A.LocationID,A.DepartmentID,A.SectionID,A.CustodianID,A.SupplierID,
	A.AssetConditionID,A.PONumber,A.PurchaseDate,A.PurchasePrice,A.ComissionDate,A.WarrantyExpiryDate,
	A.DisposalReferenceNo,A.DisposedDateTime,A.DisposedRemarks,A.AssetRemarks,A.DepreciationClassID,A.DepreciationFlag,
	A.SalvageValue,A.VoucherNo,A.StatusID,A.CreatedBy,A.CreatedDateTime,A.LastModifiedBy,
	A.LastModifiedDateTime,A.AssetDescription,A.ReferenceCode,A.SerialNo,A.NetworkID,A.InvoiceNo,A.DeliveryNote,A.Make,A.Capacity,A.MappedAssetID,A.CreateFromHHT,A.DisposalValue,
	A.MailAlert,A.partialDisposalTotalValue,A.IsTransfered,A.CategoryID,A.TransferTypeID,A.UploadedDocumentPath,A.UploadedImagePath,A.AssetApproval,A.ReceiptNumber,A.InsertedToOracle,
	A.InvoiceDate,A.DistributionID,A.ProductID,A.CompanyID,A.SyncDateTime,A.Attribute1,A.Attribute2,A.Attribute3,A.Attribute4,A.Attribute5,A.Attribute6,A.Attribute7,A.Attribute8,A.Attribute9,A.Attribute10,A.Attribute11,A.Attribute12,
	A.Attribute13,A.Attribute14,A.Attribute15,A.Attribute16,A.ManufacturerID,A.ModelID,A.QFAssetCode,A.DOFPO_LINE_NUM,A.DOFMASS_ADDITION_ID,A.ERPUpdateType,A.DOFPARENT_MASS_ADDITION_ID,A.DOFFIXED_ASSETS_UNITS,A.zDOF_Asset_Updated,A.Latitude,A.Longitude,
	A.DisposalTypeID,c.CategoryName,A.CurrentCost,A.ProceedofSales,A.SoldTo,A.AllowTransfer,A.CostOfRemoval,
	c.CategoryNameHierarchy AS CategoryHierarchy, C.CategoryCodeHierarchy AS CategoryCode,LV.LocationCodeHierarchy AS LocationCode,LV.LocationName,
	CASE WHEN LV.LocationNameHierarchy IS NULL OR LV.LocationNameHierarchy ='' THEN '' ELSE LV.LocationNameHierarchy END AS LocationHierarchy,D.DepartmentCode,d.DepartmentName AS DepartmentName,
	S.SectionCode,s.SectionName as SectionDescription,
	cu.PersonCode AS CustodianCode,Cu.PersonFirstName +''+Cu.PersonLastName AS CustodianName,
	MDT.ModelName Model,
	AC.AssetConditionCode,AC.AssetConditionName AS AssetCondition, SU.PartyCode as SupplierCode,SU.PartyName as suppliername ,
	P.ProductCode,p.ProductName AS ProductName ,
	PE.PersonFirstName+'-'+PE.PersonLastName AS CreateBy, M.PersonFirstName+'-'+M.PersonLastName AS ModifedBy,
	CO.CompanyCode,CO.CompanyName AS CompanyName,
	LV.L1LocationName FirstLevelLocationName,LV.L2LocationName SecondLevelLocationName,LV.L3LocationName ThirdLevelLocationName, 
	LV.L4LocationName FourthLevelLocationName, 	LV.L5LocationName FifthLevelLocationName, 	LV.L6LocationName SixthLevelLocationName, 
	C.L1CategoryName FirstLevelCategoryName, 	C.L2CategoryName SecondLevelCategoryName,	C.L3CategoryName ThirdLevelCategoryName,
	C.L4CategoryName FourthLevelCategoryName,C.L5CategoryName FifthLevelCategoryName,C.L6CategoryName SixthLevelCategoryName,
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
	Attribute27,Attribute28,Attribute29,Attribute30,Attribute31,Attribute32,Attribute33,Attribute34,Attribute35,Attribute36,Attribute37,Attribute38,Attribute39,Attribute40,
	lv.LocationType,c.CategoryType,
	LV.Level2ID [MappedLocationID],
	C.Level2ID [MappedCategoryID],DisposalTransactionID,P.VirtualBarcode,P.specification  

	FROM AssetTable A 
	LEFT JOIN tmp_CategoryNewHierarchicalView AS c ON A.CategoryID = c.CategoryID --AND C.LanguageID IN (1)
	LEFT JOIN ProductTable P ON P.ProductID = A.ProductID AND c.CategoryID=p.categoryID
	LEFT JOIN CompanyTable CO ON CO.CompanyID=A.CompanyID 
	LEFT JOIN StatusTable ST ON A.StatusID=ST.StatusID	
	LEFT JOIN tmp_LocationNewHierarchicalView LV ON A.LocationID = LV.LocationID --AND LV.LanguageID IN (1)
	LEFT JOIN DepartmentTable D ON D.DepartmentID=A.DepartmentID 
	LEFT JOIN SectionTable S ON S.SectionID=A.SectionID  
	LEFT JOIN PersonTable Cu ON A.CustodianID=Cu.PersonID 
	LEFT JOIN PersonTable PE ON A.CreatedBy=PE.PersonID 
	LEFT JOIN PersonTable M ON A.LastModifiedBy=M.PersonID 
	LEFT JOIN AssetConditionTable AC ON a.AssetConditionID=AC.AssetConditionID
	LEFT JOIN PartyTable SU ON SU.PartyID = A.SupplierID  and partyTypeID=2
	left join ModelTable MDT on A.ModelID=mdt.ModelID

	LEFT JOIN (SELECT DISTINCT CategoryID FROM UserCategoryMappingTable) CMAP ON CMAP.CategoryID = C.Level2ID
	LEFT JOIN (SELECT DISTINCT LocationID FROM UserLocationMappingTable) LMAP ON LMAP.LocationID = LV.Level2ID

	WHERE CMAP.CategoryID IS NOT NULL AND LMAP.LocationID IS NOT NULL
GO

if not exists(select ConfiguarationName from configurationTable where ConfiguarationName='SystemDateFormat')
Begin
 Insert into configurationTable(ConfiguarationName,ConfiguarationValue,ToolTipName,DataType,DropDownValue,
MinValue,MaxValue,ConfiguarationType,DisplayOrderID,
DefaultValue,DisplayConfiguration,SuffixText,CatagoryName,Images,
RequiredLicenseName,CompanyName,CategoryName)
Values('SystemDateFormat','dd/MM/yyyy','SystemDateFormat','String','dd/MM/yyyy MM/dd/yyyy yyyy/MM/dd',NULL,NULL,'C',138,NULL,1,NULL,'Common',NULL,NULL,NULL,'Common')
end 
go 


If not exists(select Importfield from ImportTemplateNewTable where ImportField='VirtualBarcode' and EntityID=(select EntityID from EntityTable where EntityName='Asset') and ImportTemplateTypeID=1)
begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,
DataSize,DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'VirtualBarcode',4,1,0,100,NULL,0,100,1,0,NULL,EntityID
From EntityTable where EntityName='Asset'
End 
go 
If not exists(select Importfield from ImportTemplateNewTable where ImportField='VirtualBarcode' and EntityID=(select EntityID from EntityTable where EntityName='Asset') and ImportTemplateTypeID=2)
begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,
DataSize,DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'VirtualBarcode',4,1,0,100,NULL,0,100,2,0,NULL,EntityID
From EntityTable where EntityName='Asset'
End 
go 
ALTER Procedure [dbo].[IPRC_AMSExceImportBulkAssets]
(
	@AssetCode nvarchar(100)=NULL,
	@Barcode nvarchar(100)=NULL,
	@DepartmentCode nvarchar(100)=NULL,
	@SectionCode nvarchar(100)=NULL,
	@CustodianCode nvarchar(100)=NULL,
	@ModelCode nvarchar(100)=NULL,
	@PONumber nvarchar(100)=NULL,
	@ReferenceCode nvarchar(100)=null,
	@SerialNo nvarchar(100)=NULL,
	@PurchaseDate nvarchar(100)=NULL,
	@WarrantyExpiryDate nvarchar(100)=NULL,
	@CategoryCode nvarchar(100)=NULL,
	@LocationCode nvarchar(100)=NULL,
	@VirtualBarcode nvarchar(max)=NULL,
	@AssetDescription nvarchar(max)=NULL,
	@AssetConditionCode nvarchar(100)=NULL,
	@PurchasePrice nvarchar(100)=NULL,
	@DeliveryNote nvarchar(100)=null,
	@RFIDTagCode nvarchar(100)=null,
	@SupplierCode nvarchar(100)=NULL,
	@AssetRemarks nvarchar(max)=null,
	@InvoiceNo nvarchar(100)=null,
	@InvoiceDate nvarchar(100)=null,
	@ManufacturerCode nvarchar(100)=NULL,
	@CommissionDate nvarchar(100)=null,
	@VoucherNo nvarchar(100)=null,
	@Make nvarchar(100)=null,
	@Capacity nvarchar(100)=null,
	@MappedAssetID nvarchar(100)=null,
	@ReceiptNumber nvarchar(100)=null,
	@ImportTypeID int,
	@UserID int,
	@LanguageID int,
	@CompanyID int,	
	@Attribute1 nvarchar(100)=null,
	@Attribute2 nvarchar(100)=null,
	@Attribute3 nvarchar(100)=null,
	@Attribute4 nvarchar(100)=null,
	@Attribute5 nvarchar(100)=null,
	@Attribute6 nvarchar(100)=null,
	@Attribute7 nvarchar(100)=null,
	@Attribute8 nvarchar(100)=null,
	@Attribute9 nvarchar(100)=null,
	@Attribute10 nvarchar(100)=null,
	@Attribute11 nvarchar(100)=null,
	@Attribute12 nvarchar(100)=null,
	@Attribute13 nvarchar(100)=null,
	@Attribute14 nvarchar(100)=null,
	@Attribute15 nvarchar(100)=null,
	@Attribute16 nvarchar(100)=null,
    @QFAssetCode nvarchar(100)=NULL,
	@Attribute17 nvarchar(100)=NULL,
	@Attribute18 nvarchar(100)=NULL,
	@Attribute19 nvarchar(100)=NULL,
	@Attribute20 nvarchar(100)=NULL,
	@Attribute21 nvarchar(100)=NULL,
	@Attribute22 nvarchar(100)=NULL,
	@Attribute23 nvarchar(100)=NULL,
	@Attribute24 nvarchar(100)=NULL,
	@Attribute25 nvarchar(100)=NULL,
	@Attribute26 nvarchar(100)=NULL,
	@Attribute27 nvarchar(100)=NULL,
	@Attribute28 nvarchar(100)=NULL,
	@Attribute29 nvarchar(100)=NULL,
	@Attribute30 nvarchar(100)=NULL,
	@Attribute31 nvarchar(100)=NULL,
	@Attribute32 nvarchar(100)=NULL,
	@Attribute33 nvarchar(100)=NULL,
	@Attribute34 nvarchar(100)=NULL,
	@Attribute35 nvarchar(100)=NULL,
	@Attribute36 nvarchar(100)=NULL,
	@Attribute37 nvarchar(100)=NULL,
	@Attribute38 nvarchar(100)=NULL,
	@Attribute39 nvarchar(100)=NULL,
	@Attribute40 nvarchar(100)=NULL,
	@L1LocationCode nvarchar(100)=NULL,
	@L2LocationCode nvarchar(100)=NULL,
	@L1CategoryCode nvarchar(100)=NULL,
	@UploadedDocumentPath  nvarchar(max)=NULL,
	@ImportExcelFileName nvarchar(max)=NULL
	)
as 
Begin 

set @AssetCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@AssetCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
set @DepartmentCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@DepartmentCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
set @SectionCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@SectionCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
set @CustodianCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@CustodianCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
set @ModelCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@ModelCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
set @CategoryCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@CategoryCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
set @LocationCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@LocationCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
set @AssetConditionCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@AssetConditionCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
set @SupplierCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@SupplierCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
set @ManufacturerCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@ManufacturerCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
Declare @ProductCode nvarchar(max) ,@productVirtualBarcode nvarchar(max)

set @ProductCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@AssetDescription, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
set @productVirtualBarcode=@VirtualBarcode

IF(@PurchaseDate='')
BEGIN
set @PurchaseDate=null
END
IF(@CommissionDate='')
BEGIN
set @CommissionDate=null
END
IF(@WarrantyExpiryDate='')
BEGIN
set @WarrantyExpiryDate=null
END
IF(@InvoiceDate='')
BEGIN
set @InvoiceDate=null
END
if(@PurchasePrice='-')
Begin 
set @PurchasePrice=NULL
End 
 	--Declartion part 
	Declare @DepartmentID int,@SectionID int,@CustodianID int,@SupplierID int,@AssetConditionID int, @CategoryID int, @ManufacturerID int,@ModelID int ,@ProductID int ,@LocationID int  ,@DateFormat bit ,@numberFormat bit 
	Declare @ManufacturerCategoryMapping nvarchar(50),@CategoryManufacturerModelMapping nvarchar(50),@ImportExcelNotAllowCreateReferenceFieldNewEntry nvarchar(50) ,@BarcodeEqualAssetCode nvarchar(50),
	@IsBarcodeSettingApplyImportAsset nvarchar(50),@BarcodeAutoGenerateEnable nvarchar(50),@LocationMandatory nvarchar(50),@ReturnMessage NVARCHAR(MAX),@AssetApproval int 
	,@ValidateProduct int 

	set @ValidateProduct=0

	DECLARE @MESSAGETABLE TABLE(TEXT NVARCHAR(MAX))
	select @ManufacturerCategoryMapping=ConfiguarationValue From ConfigurationTable where ConfiguarationName='ManufacturerCategoryMapping'
	select @CategoryManufacturerModelMapping=ConfiguarationValue From ConfigurationTable where ConfiguarationName='ModelManufacturerCategoryMapping'
	select @ImportExcelNotAllowCreateReferenceFieldNewEntry=ConfiguarationValue From ConfigurationTable where ConfiguarationName='ImportExcelNotAllowCreateReferenceFieldNewEntry'
	select @BarcodeEqualAssetCode=ConfiguarationValue From ConfigurationTable where ConfiguarationName='BarcodeEqualAssetCode'
	select @IsBarcodeSettingApplyImportAsset=ConfiguarationValue From ConfigurationTable where ConfiguarationName='IsBarcodeSettingApplyImportAsset'
	select @BarcodeAutoGenerateEnable=ConfiguarationValue From ConfigurationTable where ConfiguarationName='BarcodeAutoGenerateEnable'
	select @LocationMandatory=ConfiguarationValue From ConfigurationTable where ConfiguarationName='LocationMandatory'

	select @AssetApproval=case when ConfiguarationValue='true' then 1 else 0 end From ConfigurationTable where ConfiguarationName='AssetApproval'
   
	 if @ImportTypeID=1 
	 begin
		 if (@AssetDescription is null or @AssetDescription='') and (@VirtualBarcode is null or @VirtualBarcode='')
		 Begin 
				SElect @Barcode+'- Virtual Barcode/Asset Description should be provided ' as ReturnMessage
		   Return 
		 End 
		 else if (@AssetDescription is not null or @AssetDescription!='') and (@VirtualBarcode is not null or @VirtualBarcode!='')
		 Begin
		    set @ValidateProduct=2
		 End 
		 else 
		 begin
		 set @ValidateProduct=1
		 End 
	 End 

	IF @CategoryCode IS NOT NULL AND (@L1CategoryCode IS NULL OR @L1CategoryCode='')
	BEGIN 
	   SElect @Barcode+'- L1 Category Code is must be add if Update/Create Category Code ' as ReturnMessage
				Return 
	END 
	IF @LocationCode IS NOT NULL AND ((@L1LocationCode IS NULL OR @L1LocationCode='') or (@L2LocationCode IS NULL OR @L2LocationCode=''))
	BEGIN 
	   SElect @Barcode+'- L1 Location Code is must be add if Update/Create Location Code ' as ReturnMessage
				Return 
	END 
	IF @ValidateProduct=2 AND (@CategoryCode IS NULL OR @CategoryCode='')
	BEGIN 
	   SElect @Barcode+'- Category Code is must be add if update/Create Asset Description ' as ReturnMessage
				Return 
	END
	IF @ModelCode IS NOT NULL AND (@ManufacturerCode IS NULL OR @ManufacturerCode='')
	BEGIN 
	   SElect @Barcode+'- Manufacturer Code is must be add if update/Create Model Code ' as ReturnMessage
				Return 
	END 
	IF @CustodianCode IS NOT NULL AND (@DepartmentCode IS NULL OR @DepartmentCode='')
	BEGIN 
	   SElect @Barcode+'- Department Code is must be add if Update/Create Custodian Code ' as ReturnMessage
				Return 
	END 
	IF @SectionCode IS NOT NULL AND (@DepartmentCode IS NULL OR @DepartmentCode='')
	BEGIN 
	   SElect @Barcode+'- Department Code is must be add if Update/Create Section Code ' as ReturnMessage
				Return 
	END 
	If ((@CommissionDate is not null and @CommissionDate!=''))
		Begin
		  SET DATEFORMAT dmy;
		  SELECT @DateFormat= ISDATE(@CommissionDate)
		  If @DateFormat=0
		   BEGIN 
		     SElect @Barcode+'- Comission Date is invalid' as ReturnMessage
				Return 
		   END 
		End 
			If ((@WarrantyExpiryDate is not null and @WarrantyExpiryDate!=''))
		Begin
		  SET DATEFORMAT dmy;
		  SELECT @DateFormat= ISDATE(@WarrantyExpiryDate)
		  If @DateFormat=0
		   BEGIN 
		     SElect @Barcode+'- Warranty Expiry Date is invalid' as ReturnMessage
				Return 
		   END 
		End
		--print @PurchasePrice
		If ((@PurchasePrice is not null and @PurchasePrice!=''))
		Begin
		  SET DATEFORMAT dmy;
		  SELECT @numberFormat= isnumeric(@PurchasePrice)
		  If @numberFormat=0
		   BEGIN 
		     SElect @Barcode+'- Purchase Price is invalid' as ReturnMessage
				Return 
		   END
		   Else
			BEGIN
				If convert(decimal(18,5),@PurchasePrice)<0
					BEGIN
						SElect @Barcode+'- Purchase Price should not be negative' as ReturnMessage
						Return	
					END
			END
		End 

		If ((@PurchaseDate is not null and @PurchaseDate!=''))
		Begin
		  SET DATEFORMAT dmy;
		  SELECT @DateFormat= ISDATE(@PurchaseDate)
		  If @DateFormat=0
		   BEGIN 
		     SElect @Barcode+'-  Purchase Date is invalid' as ReturnMessage
				Return 
		   END 
		End 
		If ((@InvoiceDate is not null and @InvoiceDate!=''))
		Begin
		  SET DATEFORMAT dmy;
		  SELECT @DateFormat= ISDATE(@InvoiceDate)
		  If @DateFormat=0
		   BEGIN 
		     SElect @Barcode+'- Invoice Date is invalid' as ReturnMessage
				Return 
		   END 
		End 

		if ((@PurchaseDate is null or @PurchaseDate=' ') and (@WarrantyExpiryDate is not null and  @WarrantyExpiryDate!=''))
					 Begin 
					      SElect 'Without Purchase date warranty expiry month should not be added For'+@Barcode as ReturnMessage
					      Return 
					 End 
					 else if ((@PurchaseDate is not null and @PurchaseDate!='') and (@WarrantyExpiryDate is not null and  @WarrantyExpiryDate!=''))
					 Begin 
					      Declare @res bit 
						  Select @res=ISDATE(@WarrantyExpiryDate)
						  IF @res=1 
						  begin
						   IF CONVERT(DATETIME,@PURCHASEDATE,103)>CONVERT(DATETIME,@WarrantyExpiryDate,103)
						   Begin 
					      SElect 'warranty expiry month should be in greater then Purchase Date  '+@Barcode as ReturnMessage
					      Return
						  End
						  End
						 
					 End 
	
	 Declare @catL1 int
					 if @L1CategoryCode is not null or @L1CategoryCode !=''
					 Begin
						 if  exists(select CategoryCode from CategoryTable where CategoryCode=@L1CategoryCode and ParentCategoryID is null and StatusID=1)
						 Begin 
							 select @catL1=CategoryID from CategoryTable where CategoryCode=@L1CategoryCode and ParentCategoryID is null and StatusID=1
						 End 
						 else
						  Begin 
							  SElect 'given L1 Category code is not avaliable in db  '+@Barcode as ReturnMessage
							  Return
						  End 
					 End 
					 print @catL1
					 IF (@CategoryCode is not NULL and @CategoryCode!='')
					BEGIN 		 
					  IF EXISTS(SELECT CATEGORYCODE FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1 and ParentCategoryID=@catL1)
					  BEGIN 		 
						SELECT @CATEGORYID=CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1  and ParentCategoryID=@catL1
					  END
					  ELSE 
					  BEGIN
		  
					   IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
					   BEGIN 
					   print 'test'
						INSERT INTO CATEGORYTABLE(CATEGORYCODE,STATUSID,CREATEDBY,CREATEDDATETIME,CategoryName,ParentCategoryID) 
						VALUES(@CATEGORYCODE,1,@USERID,GETDATE(),@CategoryCode,@catL1)			
						SET @CATEGORYID = SCOPE_IDENTITY()			
						INSERT INTO CATEGORYDESCRIPTIONTABLE (CATEGORYID,CATEGORYDESCRIPTION,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
						Select @CATEGORYID,@CATEGORYCODE,LanguageID,@USERID,GETDATE() from LanguageTable
						END 
						ELSE 
						BEGIN
			  
						   INSERT INTO @MESSAGETABLE(TEXT)VALUES('CategoryCode Cant allow Create Data')
						END 
					  END 
					END 
					Declare @locL1 int,@LocL2 int
					 if @L1LocationCode is not null or @L1LocationCode !=''
					 Begin
						 if  exists(select locationcode from LocationTable where LocationCode=@L1LocationCode and ParentLocationID is null and StatusID=1)
						 Begin 
							 select @locL1=locationID from LocationTable where LocationCode=@L1LocationCode and ParentLocationID is null and StatusID=1
						 End 
						 else
						  Begin 
							  SElect 'given L1 Location code is not avaliable in db  '+@Barcode as ReturnMessage
							  Return
						  End 
					 End 
					 if @L2LocationCode is not null or @L2LocationCode !=''
					 Begin
						 if  exists(select locationcode from LocationTable where LocationCode=@L2LocationCode and ParentLocationID =@locL1 and StatusID=1)
						 Begin 
							 select @locL2=locationID from LocationTable where LocationCode=@L2LocationCode and ParentLocationID =@locL1 and StatusID=1
						 End 
						 else
						  Begin 
							  SElect 'given L2 Location code is not avaliable in db  '+@Barcode as ReturnMessage
							  Return
						  End 
					 End 
					  IF (@LOCATIONCODE is not NULL and @LOCATIONCODE!='')
						BEGIN 
						 IF EXISTS(SELECT LOCATIONCODE FROM LocationNewHierarchicalView WHERE LOCATIONCODE=@LOCATIONCODE and Level2ID=@LocL2 and StatusID=1)
						  BEGIN  
							SELECT @LOCATIONID=LOCATIONID FROM LocationNewHierarchicalView WHERE LOCATIONCODE=@LOCATIONCODE and StatusID=1 and Level2ID=@LocL2
						  END
						  ELSE 
						  BEGIN
							IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
						   BEGIN 
							INSERT INTO LOCATIONTABLE(LOCATIONCODE,STATUSID,CREATEDBY,CREATEDDATETIME,LocationName,ParentLocationID) 
							VALUES(@LOCATIONCODE,1,@USERID,GETDATE(),@LocationCode,@LocL2)
			
							SET @LOCATIONID = SCOPE_IDENTITY()
			
							INSERT INTO LOCATIONDESCRIPTIONTABLE (LOCATIONID,LOCATIONDESCRIPTION,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
							Select @LOCATIONID,@LOCATIONCODE,LanguageID,@USERID,GETDATE() from LanguageTable
							End 
							ELSE 
							BEGIN 
							INSERT INTO @MESSAGETABLE(TEXT)VALUES('LocationCode Cant allow Create Data')
							END 
						  END 
		END 
	Declare @ErrorID int 
	Declare @ErrorMsg nvarchar(max)
	If @ImportTypeID=1
	begin
		 exec Prc_AssetCreationValidation @userID,@CategoryCode,@LocationCode,null,null,null,@DepartmentCode,@SerialNo,@ManufacturerCode,null,'ExcelAssetInsert',@ErrorID output,@ErrorMsg output
		if @ErrorID>0
		Begin
			 Select @ErrorMsg
			 Return
		End 
		if upper(@BarcodeAutoGenerateEnable)='TRUE' and upper(@IsBarcodeSettingApplyImportAsset)='TRUE'
		begin 
		  set @Barcode='-';
		End 
		if UPPER(@BarcodeEqualAssetCode)='TRUE' and upper(@IsBarcodeSettingApplyImportAsset)='TRUE'
		Begin 
		set @AssetCode=@Barcode
		End 
	end 
	Else 
	Begin 
	  Declare @AssetID int 
	  Select @AssetID=AssetID from AssetTable where barcode=@Barcode


	  exec Prc_AssetModificationValidation @userID,@AssetID,@CategoryCode,@LocationCode,null,null,null,@DepartmentCode,@SerialNo,@ManufacturerCode,null,'ExcelAssetInsert',@ErrorID output,@ErrorMsg output
		
		if @ErrorID>0
		Begin
			 Select @ErrorMsg
			 Return
		End 
	End 
	
	----CategoryTable
	-- IF (@CategoryCode is not NULL and @CategoryCode!='')
	--	BEGIN 		 
	--	  IF EXISTS(SELECT CATEGORYCODE FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1)
	--	  BEGIN 		 
	--		SELECT @CATEGORYID=CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1 
	--	  END
	--	  ELSE 
	--	  BEGIN
		  
	--	   IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
	--	   BEGIN 
	--	    INSERT INTO CATEGORYTABLE(CATEGORYCODE,STATUSID,CREATEDBY,CREATEDDATETIME,CategoryName) 
	--		VALUES(@CATEGORYCODE,1,@USERID,GETDATE(),@CategoryCode)			
	--		SET @CATEGORYID = SCOPE_IDENTITY()			
	--		INSERT INTO CATEGORYDESCRIPTIONTABLE (CATEGORYID,CATEGORYDESCRIPTION,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
	--		Select @CATEGORYID,@CATEGORYCODE,LanguageID,@USERID,GETDATE() from LanguageTable
	--		END 
	--		ELSE 
	--		BEGIN
			  
	--		   INSERT INTO @MESSAGETABLE(TEXT)VALUES('CategoryCode Cant allow Create Data')
	--		END 
	--	  END 
	--	END 
		
	----LOCATUION MASTER
	-- IF (@LOCATIONCODE is not NULL and @LOCATIONCODE!='')
	--	BEGIN 
	--	 IF EXISTS(SELECT LOCATIONCODE FROM LOCATIONTABLE WHERE LOCATIONCODE=@LOCATIONCODE and StatusID=1)
	--	  BEGIN  
	--		SELECT @LOCATIONID=LOCATIONID FROM LOCATIONTABLE WHERE LOCATIONCODE=@LOCATIONCODE and StatusID=1
	--	  END
	--	  ELSE 
	--	  BEGIN
	--	    IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
	--	   BEGIN 
	--	    INSERT INTO LOCATIONTABLE(LOCATIONCODE,STATUSID,CREATEDBY,CREATEDDATETIME,LocationName) 
	--		VALUES(@LOCATIONCODE,1,@USERID,GETDATE(),@LocationCode)
			
	--		SET @LOCATIONID = SCOPE_IDENTITY()
			
	--		INSERT INTO LOCATIONDESCRIPTIONTABLE (LOCATIONID,LOCATIONDESCRIPTION,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
	--		Select @LOCATIONID,@LOCATIONCODE,LanguageID,@USERID,GETDATE() from LanguageTable
	--		End 
	--		ELSE 
	--		BEGIN 
	--		INSERT INTO @MESSAGETABLE(TEXT)VALUES('LocationCode Cant allow Create Data')
	--		END 
	--	  END 
	--	END 
	 --DEPARTMENT MASTER 
	 IF(@DEPARTMENTCODE!=''  AND @DEPARTMENTCODE is not NULL)
		  BEGIN 
			IF EXISTS(SELECT DEPARTMENTCODE FROM DEPARTMENTTABLE WHERE DEPARTMENTCODE=@DEPARTMENTCODE and StatusID=1)
			BEGIN 
			  SELECT @DEPARTMENTID=DEPARTMENTID FROM DEPARTMENTTABLE WHERE DEPARTMENTCODE=@DEPARTMENTCODE  and StatusID=1
			END 
			ELSE 
			BEGIN 
			  IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
		   BEGIN 	
				INSERT INTO DEPARTMENTTABLE(DEPARTMENTCODE,STATUSID,CREATEDBY,CREATEDDATETIME,DepartmentName)
				VALUES(@DEPARTMENTCODE,1,@USERID,GETDATE(),@DepartmentCode)

				SET @DEPARTMENTID = SCOPE_IDENTITY()
				INSERT INTO DEPARTMENTDESCRIPTIONTABLE(DEPARTMENTID,DEPARTMENTDESCRIPTION,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
				select @DEPARTMENTID,@DEPARTMENTCODE,LanguageID,@USERID,GETDATE() from LanguageTable
				End 
				ELSE 
				BEGIN
				   INSERT INTO @MESSAGETABLE(TEXT)VALUES('DepartmentCode Cant allow Create Data')
				END 
			END 
		  END 
	 --SECTION MASTER
     IF (@SECTIONCODE!='' AND @SECTIONCODE is not NULL)
			BEGIN 
			 IF EXISTS (SELECT SECTIONCODE FROM SECTIONTABLE WHERE SECTIONCODE=@SECTIONCODE AND 
								DEPARTMENTID in (SELECT DEPARTMENTID FROM DEPARTMENTTABLE WHERE DEPARTMENTCODE=@DEPARTMENTCODE and StatusID=1) and StatusID=1)
			 BEGIN
				SELECT @SECTIONID=SECTIONID FROM SECTIONTABLE WHERE SECTIONCODE=@SECTIONCODE AND DEPARTMENTID in (SELECT DEPARTMENTID FROM DEPARTMENTTABLE WHERE DEPARTMENTCODE=@DEPARTMENTCODE and StatusID=1) and StatusID=1
			    
			 END
			 ELSE 
			 BEGIN
			   IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
		   BEGIN 	
				 INSERT INTO SECTIONTABLE (SECTIONCODE,STATUSID,CREATEDBY,CREATEDDATETIME,DEPARTMENTID,SectionName) 
				 VALUES(@SECTIONCODE,1,@USERID,GETDATE(),@DEPARTMENTID,@SectionCode)

				  SET @SECTIONID = SCOPE_IDENTITY()
				  
				  INSERT INTO SECTIONDESCRIPTIONTABLE(SECTIONID,SECTIONDESCRIPTION,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
				  Select @SECTIONID,@SECTIONCODE,LanguageID,@USERID,GETDATE() from LanguageTable
				End 
				ELSE 
			BEGIN
			   INSERT INTO @MESSAGETABLE(TEXT)VALUES('SectionCode Cant allow Create Data')
			END 

			 END 
			END 
	-- CUSTODIAN MASTER 
	 IF @CUSTODIANCODE is not NULL and @CUSTODIANCODE!=''
			BEGIN
			  IF EXISTS(SELECT PERSONCODE FROM PERSONTABLE WHERE PERSONCODE=@CUSTODIANCODE AND (USERTYPEID=2 OR USERTYPEID=3) and StatusID=1)
			  BEGIN
				SELECT @CUSTODIANID=PERSONID FROM PERSONTABLE WHERE PERSONCODE=@CUSTODIANCODE AND  (USERTYPEID=2 OR USERTYPEID=3) and StatusID=1
			  END 
			  ELSE 
			  BEGIN 
			    IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
		   BEGIN 	

			    INSERT INTO User_LoginUserTable(UserName,Password,PasswordSalt,LastActivityDate,LastLoginDate,LastLoggedInDate,ChangePasswordAtNextLogin,IsLockedOut,IsDisabled,IsApproved)
			VALUES(@CUSTODIANCODE ,'Mod6/JMHjHeKXDkUK/zd7PfLlJg=','BZxI8E2lroNt28VMhZsyyaNZha8=',GETDATE(),GETDATE(),GETDATE(),1,0,0,1 )

			INSERT INTO PersonTable(PersonID, PersonFirstName, PersonLastName, PersonCode, AllowLogin, DepartmentID, UserTypeID, StatusID, Culture,CreatedBy,CreatedDateTime,CreatedFrom) 
				VALUES(@@IDENTITY, @CUSTODIANCODE, @CUSTODIANCODE, @CUSTODIANCODE, 0, @DepartmentID, 2, 1, 'en-GB', @UserID,GETDATE(),'APP')
			 END 
			ELSE 
			BEGIN
			   INSERT INTO @MESSAGETABLE(TEXT)VALUES('Custodiancode Cant allow Create Data')
			END 
			  END 

			END
	--ASSET cONDTION MASTER
	 IF @ASSETCONDITIONCODE is not NULL AND @ASSETCONDITIONCODE!=''
			BEGIN 
			 IF EXISTS (SELECT ASSETCONDITIONCODE FROM ASSETCONDITIONTABLE WHERE ASSETCONDITIONCODE=@ASSETCONDITIONCODE and StatusID=1)
			 BEGIN
				SELECT @ASSETCONDITIONID=ASSETCONDITIONID FROM ASSETCONDITIONTABLE WHERE ASSETCONDITIONCODE=@ASSETCONDITIONCODE  and StatusID=1
			 END 
			 ELSE 
			 BEGIN
			   IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
		   BEGIN 
				 INSERT INTO ASSETCONDITIONTABLE(ASSETCONDITIONCODE,STATUSID,CREATEDBY,CREATEDDATETIME,AssetConditionName)
				 VALUES(@ASSETCONDITIONCODE,1,@USERID,GETDATE(),@AssetConditionCode)

				  SET @ASSETCONDITIONID = SCOPE_IDENTITY()

				  INSERT INTO ASSETCONDITIONDESCRIPTIONTABLE(ASSETCONDITIONID,ASSETCONDITIONDESCRIPTION,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
				  Select @ASSETCONDITIONID,@ASSETCONDITIONCODE,LanguageID,@USERID,GETDATE() from LanguageTable
				  END 
			ELSE 
			BEGIN
			   INSERT INTO @MESSAGETABLE(TEXT)VALUES('AssetConditionCode Cant allow Create Data')
			END 
			 END 
			END 
	 -- SUPPLIER TABLE 
	 IF @SUPPLIERCODE is not NULL AND @SUPPLIERCODE!='' 
			 BEGIN
			 IF EXISTS(SELECT PartyCode FROM PartyTable WHERE PartyCode=@SUPPLIERCODE and StatusID=1)
			 BEGIN 
				SELECT @SUPPLIERID =partyID FROM PartyTable WHERE PartyCode=@SUPPLIERCODE and StatusID=1 and PartyTypeID=1
			 END 
			 ELSE 
			 BEGIN 
			   IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
		   BEGIN 	
				INSERT INTO PartyTable(PartyCode,STATUSID,CREATEDBY,CREATEDDATETIME,PartyName,PartyTypeID)
				VALUES(@SUPPLIERCODE,1,@USERID,GETDATE(),@SupplierCode,(select PartyTypeID from PartyTypeTable where PartyType='Supplier'))
				
				SET @SUPPLIERID = SCOPE_IDENTITY()
				--INSERT INTO part(SUPPLIERID,SUPPLIERDESCRIPTION,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
				--Select @SUPPLIERID,@SUPPLIERCODE,LanguageID,@USERID,GETDATE() from LanguageTable
				END 
			ELSE 
			BEGIN
			   INSERT INTO @MESSAGETABLE(TEXT)VALUES('SupplierCode Cant allow Create Data')
			END 
				END
			 END 
	 
	 --AssetDescription
	 declare @assetDesc nvarchar(max)

	 IF((@VirtualBarcode is not NULL and @VirtualBarcode!='') or (@ValidateProduct=2))                                                                
	  BEGIN 
		IF EXISTS(SELECT ProductName FROM ProductTable WHERE virtualBarcode=@VirtualBarcode
		AND CATEGORYID in (SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1) and StatusID=1)
			BEGIN
			--print 'ProductID'
			SELECT @PRODUCTID=ProductID,@assetDesc=ProductName FROM ProductTable WHERE virtualBarcode=@VirtualBarcode
			AND CATEGORYID in (SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1) and StatusID=1
			--FROM PRODUCTVIEW WHERE replace(productName,' ','')=@ProductCode AND LANGUAGEID=1 and StatusID=1
			--AND CATEGORYID in (SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1)
			END 
		ELSE 
		BEGIN
		IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
		BEGIN 
		if not exists(select CategoryCode from Categorytable where CategoryCode=@CategoryCode)
		Begin 
		insert into Categorytable(CategoryCode,StatusID,CreatedBy,CreatedDateTime,CategoryName) 
			values(@CategoryCode,1,@USERID,getdate(),@CategoryCode)
			SET @CategoryID = SCOPE_IDENTITY()
				 
			insert into CategoryDescriptionTable(CategoryID,CategoryDescription,languageID,CreatedBy,CreatedDateTime)
			select @CategoryID,@CategoryCode,LanguageID,@USERID,getdate() from languageTable
				   
			INSERT INTO PRODUCTTABLE(PRODUCTCODE,STATUSID,CATEGORYID,CREATEDBY,CREATEDDATETIME,ProductName,VirtualBarcode)
		VALUES(@VirtualBarcode,1,@CATEGORYID,@USERID,getdate(),@VirtualBarcode,@VirtualBarcode)
				 
			SET @ProductID = SCOPE_IDENTITY()
				set @assetDesc=@VirtualBarcode
			INSERT INTO PRODUCTDESCRIPTIONTABLE (PRODUCTDESCRIPTION,PRODUCTID,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
			Select @VirtualBarcode,@ProductID,LanguageID,@USERID,getdate() from LanguageTable
		End 
		Else 
		Begin 
		SELECT @CATEGORYID= CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE
		END  

		INSERT INTO PRODUCTTABLE(PRODUCTCODE,STATUSID,CATEGORYID,CREATEDBY,CREATEDDATETIME,ProductName,VirtualBarcode)
		VALUES(@VirtualBarcode,1,@CATEGORYID,@USERID,getdate(),@VirtualBarcode,@VirtualBarcode)
			SET @ProductID = SCOPE_IDENTITY()
			set @assetDescription=@VirtualBarcode
			INSERT INTO PRODUCTDESCRIPTIONTABLE (PRODUCTDESCRIPTION,PRODUCTID,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
			Select @VirtualBarcode,@ProductID,LanguageID,@USERID,getdate() from LanguageTable
			END 
	ELSE 
	BEGIN
		INSERT INTO @MESSAGETABLE(TEXT)VALUES('ProductCode Cant allow Create Data')
	END 
		END 
	END

	  IF((@ASSETDESCRIPTION is not NULL and @ASSETDESCRIPTION!='')and (@VirtualBarcode is null or @VirtualBarcode=''))                                                               
	  BEGIN 
		IF EXISTS(SELECT ProductName FROM ProductTable WHERE replace(ProductName,' ','')=@ProductCode
		AND CATEGORYID in (SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1) and StatusID=1)
			BEGIN
			--print 'ProductID'
			SELECT @PRODUCTID=ProductID,@assetDesc=ProductName FROM ProductTable WHERE replace(ProductName,' ','')=replace(@ProductCode,' ','')
			AND CATEGORYID in (SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1) and StatusID=1
			--FROM PRODUCTVIEW WHERE replace(productName,' ','')=@ProductCode AND LANGUAGEID=1 and StatusID=1
			--AND CATEGORYID in (SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1)
			END 
		ELSE 
		BEGIN
		IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
		BEGIN 
		if not exists(select CategoryCode from Categorytable where CategoryCode=@CategoryCode)
		Begin 
		insert into Categorytable(CategoryCode,StatusID,CreatedBy,CreatedDateTime,CategoryName) 
			values(@CategoryCode,1,@USERID,getdate(),@CategoryCode)
			SET @CategoryID = SCOPE_IDENTITY()
				 
			insert into CategoryDescriptionTable(CategoryID,CategoryDescription,languageID,CreatedBy,CreatedDateTime)
			select @CategoryID,@CategoryCode,LanguageID,@USERID,getdate() from languageTable
				   
			INSERT INTO PRODUCTTABLE(PRODUCTCODE,STATUSID,CATEGORYID,CREATEDBY,CREATEDDATETIME,ProductName)
		VALUES(@ProductCode,1,@CATEGORYID,@USERID,getdate(),@ProductCode)
				 
			SET @ProductID = SCOPE_IDENTITY()
			set @assetDesc=@ProductCode
				 
			INSERT INTO PRODUCTDESCRIPTIONTABLE (PRODUCTDESCRIPTION,PRODUCTID,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
			Select @ProductCode,@ProductID,LanguageID,@USERID,getdate() from LanguageTable
		End 
		Else 
		Begin 
		SELECT @CATEGORYID= CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE
		END  

		INSERT INTO PRODUCTTABLE(PRODUCTCODE,STATUSID,CATEGORYID,CREATEDBY,CREATEDDATETIME,ProductName)
		VALUES(@ProductCode,1,@CATEGORYID,@USERID,getdate(),@ProductCode)
			SET @ProductID = SCOPE_IDENTITY()
				set @assetDesc=@ProductCode
			INSERT INTO PRODUCTDESCRIPTIONTABLE (PRODUCTDESCRIPTION,PRODUCTID,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
			Select @ASSETdESCRIPTION,@ProductID,LanguageID,@USERID,getdate() from LanguageTable
			END 
	ELSE 
	BEGIN
		INSERT INTO @MESSAGETABLE(TEXT)VALUES('ProductCode Cant allow Create Data')
	END 
		END 
	END

	 -- MANUFACTURER MASTER 
	 IF @MANUFACTURERCODE is not NULL AND @MANUFACTURERCODE!=''
			BEGIN 
				if upper(@ManufacturerCategoryMapping)='TRUE'
				BEGIN 									
			   IF EXISTS (SELECT MANUFACTURERCODE FROM MANUFACTURERTABLE WHERE MANUFACTURERCODE=@MANUFACTURERCODE AND 
						CATEGORYID=(SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1) and StatusID=1)
			   BEGIN 
					SELECT @MANUFACTURERID =MANUFACTURERID FROM MANUFACTURERTABLE WHERE MANUFACTURERCODE=@MANUFACTURERCODE AND 
						CATEGORYID=(SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1) and StatusID=1
			   END 
			   ELSE 
			   BEGIN 

			   IF @CATEGORYCODE is not NULL and @CATEGORYCODE!='' 
			   BEGIN 
				 if not exists(select CategoryCode from Categorytable where CategoryCode=@CategoryCode and StatusID=1)
				Begin 
				  IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
		   BEGIN 	
				insert into Categorytable(CategoryCode,StatusID,CreatedBy,CreatedDateTime,CategoryName) 
				   values(@CategoryCode,1,@USERID,getdate(),@CategoryCode)
				   SET @CategoryID = SCOPE_IDENTITY()
				      insert into CategoryDescriptionTable(CategoryID,CategoryDescription,languageID,CreatedBy,CreatedDateTime)
				   Select @CategoryID,@CategoryCode,LanguageID,@USERID,getdate() from LanguageTable
				   End 
				   Else 
				   BEGIN
			   INSERT INTO @MESSAGETABLE(TEXT)VALUES('CategoryCode Cant allow Create Data')
				END 
				End 
				Else 
				Begin 
				SELECT @CATEGORYID= CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1
				END  
				END 
				  IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
		   BEGIN 
				INSERT INTO MANUFACTURERTABLE(MANUFACTURERCODE,StatusID,CREATEDBY,CREATEDDATETIME,CATEGORYID,ManufacturerName)
				VALUES(@MANUFACTURERCODE,1,@USERID,getdate(),@CategoryID,@ManufacturerCode)
				SET @MANUFACTURERID = SCOPE_IDENTITY()
				INSERT INTO MANUFACTURERDESCRIPTIONTABLE(MANUFACTURERID,MANUFACTURERDESCRIPTION,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
				Select @ManufacturerID,@MANUFACTURERCODE,LanguageID,@USERID,GETDATE() from LanguageTable
				END 
			ELSE 
			BEGIN
			   INSERT INTO @MESSAGETABLE(TEXT)VALUES('ManufacturerCode Cant allow Create Data')
			END 
			   END 
			  END 
			  ELSE 
			  BEGIN 
				  IF EXISTS (SELECT MANUFACTURERCODE FROM MANUFACTURERTABLE WHERE MANUFACTURERCODE=@MANUFACTURERCODE and StatusID=1)
			   BEGIN 
					SELECT @MANUFACTURERID =MANUFACTURERID FROM MANUFACTURERTABLE WHERE MANUFACTURERCODE=@MANUFACTURERCODE  and StatusID=1
			   END 
			   ELSE 
			   BEGIN 
			     IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
		   BEGIN 	

					INSERT INTO MANUFACTURERTABLE(MANUFACTURERCODE,StatusID,CREATEDBY,CREATEDDATETIME,ManufacturerName)
					VALUES(@MANUFACTURERCODE,1,@USERID,getdate(),@ManufacturerCode)
					SET @MANUFACTURERID = SCOPE_IDENTITY()
					INSERT INTO MANUFACTURERDESCRIPTIONTABLE(MANUFACTURERID,MANUFACTURERDESCRIPTION,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
					Select @ManufacturerID,@MANUFACTURERCODE,LanguageID,@USERID,GETDATE() from LanguageTable				
END 
			ELSE 
			BEGIN
			   INSERT INTO @MESSAGETABLE(TEXT)VALUES('ManufacturerCode Cant allow Create Data')
			END 
			   END 
			  END 
			END 
			-- MODEL MASTER 
	 IF @MODELCODE is not NULL AND @MODELCODE!=''
			BEGIN
			if upper(@CategoryManufacturerModelMapping)='TRUE' 
			BEGIN 
			
			 IF EXISTS(SELECT MODELCODE FROM  MODELTABLE WHERE MODELCODE=@MODELCODE and StatusID=1 AND CATEGORYID=(SELECT CATEGORYID FROM CATEGORYTABLE C WHERE C.CATEGORYCODE=@CATEGORYCODE and StatusID=1) 
			    AND MANUFACTURERID=(SELECT MANUFACTURERID FROM MANUFACTURERTABLE WHERE  MANUFACTURERCODE=@MANUFACTURERCODE and CATEGORYID=(SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1)  and StatusID=1))
				BEGIN 
				
				  SELECT @MODELID=MODELID FROM MODELTABLE WHERE CATEGORYID=(SELECT CATEGORYID FROM CATEGORYTABLE C WHERE C.CATEGORYCODE=@CATEGORYCODE and StatusID=1) 
			    AND MANUFACTURERID=(SELECT MANUFACTURERID FROM MANUFACTURERTABLE WHERE  MANUFACTURERCODE=@MANUFACTURERCODE and CATEGORYID=(SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1)  and StatusID=1) and StatusID=1
				END 
				ELSE 
				BEGIN 
				 IF @CATEGORYCODE!=NULL and @CATEGORYCODE!='' 
			   BEGIN 
				 if not exists(select CategoryCode from Categorytable where CategoryCode=@CategoryCode and StatusID=1)
				Begin 
				  IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
		   BEGIN 	
				insert into Categorytable(CategoryCode,StatusID,CreatedBy,CreatedDateTime,CategoryName) 
				   values(@CategoryCode,1,@USERID,getdate(),@CategoryCode)
				   SET @CategoryID = SCOPE_IDENTITY()
				      insert into CategoryDescriptionTable(CategoryID,CategoryDescription,languageID,CreatedBy,CreatedDateTime)
				   Select @CategoryID,@CategoryCode,LanguageID,@USERID,getdate() from LanguageTable
				   End 
				   ELSE 
					BEGIN
					   INSERT INTO @MESSAGETABLE(TEXT)VALUES('CategoryCode Cant allow Create Data')
					END		
				End 
				Else 
				Begin 
				SELECT @CATEGORYID= CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE
				END  
				END 
				IF @MANUFACTURERCODE is not NULL AND @MANUFACTURERCODE!='' 
				BEGIN 
					IF EXISTS(SELECT MANUFACTURERCODE FROM MANUFACTURERTABLE WHERE MANUFACTURERCODE=@MANUFACTURERCODE AND 
					CATEGORYID in (SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1) and StatusID=1)
					BEGIN 
					 SELECT @MANUFACTURERID=MANUFACTURERID FROM MANUFACTURERTABLE WHERE MANUFACTURERCODE=@MANUFACTURERCODE and StatusID=1 AND 
					CATEGORYID in (SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1)
					END 
					ELSE 
					BEGIN 
					  IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
		   BEGIN 	
					  INSERT INTO MANUFACTURERTABLE(MANUFACTURERCODE,CATEGORYID,STATUSID,CREATEDBY,CREATEDDATETIME,ManufacturerName)
					  VALUES(@MANUFACTURERCODE,@CATEGORYID,1,@USERID,GETDATE(),@ManufacturerCode)
					   SET @MANUFACTURERID = SCOPE_IDENTITY()
					   INSERT INTO MANUFACTURERDESCRIPTIONTABLE(MANUFACTURERID,MANUFACTURERDESCRIPTION,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
					   Select @MANUFACTURERID,@MANUFACTURERCODE,LanguageID,@USERID,GETDATE() from LanguageTable
					   End 
					   Else 
					   BEGIN
						  INSERT INTO @MESSAGETABLE(TEXT)VALUES('ManufacturerCode Cant allow Create Data')
						END		
					END 
					  IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
		   BEGIN 
					INSERT INTO MODELTABLE(MODELCODE,STATUSID,CREATEDBY,CREATEDDATETIME,MANUFACTURERID,CATEGORYID,ModelName)
					VALUES(@MODELCODE,1,@USERID,GETDATE(),@MANUFACTURERID,@CATEGORYID,@ModelCode)
					SET @MODELID=SCOPE_IDENTITY()
					INSERT INTO MODELDESCRIPTIONTABLE(MODELID,MODELDESCRIPTION,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
					Select @MODELID,@MODELCODE,LanguageID,@USERID,GETDATE() from LanguageTable
					End 
					Else 
					BEGIN
			   INSERT INTO @MESSAGETABLE(TEXT)VALUES('ModelCode Cant allow Create Data')
			END
				END 
				END 
				END 
				ELSE 
				BEGIN 
				 IF EXISTS(SELECT MODELCODE FROM  MODELTABLE WHERE MODELCODE=@MODELCODE  and StatusID=1
			    AND MANUFACTURERID=(SELECT MANUFACTURERID FROM MANUFACTURERTABLE WHERE  MANUFACTURERCODE=@MANUFACTURERCODE and StatusID=1))
				BEGIN 
				  SELECT @MODELID=MODELID FROM MODELTABLE WHERE  MODELCODE=@MODELCODE  AND 
			     MANUFACTURERID=(SELECT MANUFACTURERID FROM MANUFACTURERTABLE WHERE  MANUFACTURERCODE=@MANUFACTURERCODE and StatusID=1) and StatusID=1
				END 
				ELSE 
				BEGIN 
					IF @MANUFACTURERCODE is not NULL AND @MANUFACTURERCODE!='' 
				BEGIN 
					IF EXISTS(SELECT MANUFACTURERCODE FROM MANUFACTURERTABLE WHERE MANUFACTURERCODE=@MANUFACTURERCODE and StatusID=1 )
					BEGIN 
					 SELECT @MANUFACTURERID=MANUFACTURERID FROM MANUFACTURERTABLE WHERE MANUFACTURERCODE=@MANUFACTURERCODE  and StatusID=1
					END 
					ELSE 
					BEGIN 
					  IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
					 BEGIN 	
					  INSERT INTO MANUFACTURERTABLE(MANUFACTURERCODE,STATUSID,CREATEDBY,CREATEDDATETIME,ManufacturerName)
					  VALUES(@MANUFACTURERCODE,1,@USERID,GETDATE(),@ManufacturerCode)
					   SET @MANUFACTURERID = SCOPE_IDENTITY()
					   INSERT INTO MANUFACTURERDESCRIPTIONTABLE(MANUFACTURERID,MANUFACTURERDESCRIPTION,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
					   Select @MANUFACTURERID,@MANUFACTURERCODE,LanguageID,@USERID,GETDATE() from LanguageTable
					   End 					   
					ELSE 
					BEGIN
					   INSERT INTO @MESSAGETABLE(TEXT)VALUES('ManufacturerCode Cant allow Create Data')
					END		
					END 
					  IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
					 BEGIN 	
					INSERT INTO MODELTABLE(MODELCODE,STATUSID,CREATEDBY,CREATEDDATETIME,MANUFACTURERID,ModelName)
					VALUES(@MODELCODE,1,@USERID,GETDATE(),@MANUFACTURERID,@ModelCode)
					SET @MODELID=SCOPE_IDENTITY()
					INSERT INTO MODELDESCRIPTIONTABLE(MODELID,MODELDESCRIPTION,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
					Select @MODELID,@MODELCODE,LanguageID,@USERID,GETDATE() from LanguageTable
					End 
					Else 
					BEGIN
					 INSERT INTO @MESSAGETABLE(TEXT)VALUES('ModelCode Cant allow Create Data')
					END
				END 
				END 
			END 
			END 
			IF @ImportTypeID=1 
			BEGIN 
			 IF UPPER(@LocationMandatory)='TRUE' AND (@LOCATIONID IS NULL OR  @LOCATIONID ='') 
			 BEGIN 
				INSERT INTO @MESSAGETABLE(TEXT)VALUES('lOCATIONCODE MANDATORY DATA FOR'+@Barcode)
			 END 
			ELSE 
			BEGIN
		       If exists(select barcode from assettable where barcode=@barcode )
			   Begin 
			     INSERT INTO @MESSAGETABLE(TEXT)VALUES('Already Exists this barcode :'+@Barcode)
			   End 
			   Else 
			   Begin 
			   print 'Insert'
			      If((@Barcode is not null and @Barcode!='') and (@AssetCode is not null and @AssetCode!='') and (@ProductID is not null and @ProductID!='') and (@CategoryID is not null and @CategoryID!='')
				  and (@ASSETDESCRIPTION is not null and @ASSETDESCRIPTION!=''))

				  Begin 
					INSERT INTO ASSETTABLE (ASSETCODE,BARCODE,DEPARTMENTID,SECTIONID,CUSTODIANID,CATEGORYID,LOCATIONID,PRODUCTID,MANUFACTURERID,MODELID,SUPPLIERID,ASSETCONDITIONID,PurchasePrice,
					PONUMBER,REFERENCECODE,SERIALNO,ASSETDESCRIPTION,DELIVERYNOTE,RFIDTAGCODE,INVOICENO,VOUCHERNO,MAKE,CAPACITY,MAPPEDASSETID,RECEIPTNUMBER,PURCHASEDATE,ComissionDate,WarrantyExpiryDate,
					STATUSID,COMPANYID,CreatedBy,CreatedDateTime,AssetApproval,CreateFromHHT,ERPUpdateType,DepreciationFlag,Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6,Attribute7,Attribute8,Attribute9,Attribute10,Attribute11,Attribute12,
					Attribute13,Attribute14,Attribute15,Attribute16,AssetRemarks,QFAssetCode,A.Attribute17,A.Attribute18,Attribute19,Attribute20,Attribute21,Attribute22,Attribute23,Attribute24,Attribute25,Attribute26,
					Attribute27,Attribute28,Attribute29,Attribute30,Attribute31,Attribute32,Attribute33,Attribute34,Attribute35,Attribute36,Attribute37,Attribute38,Attribute39,Attribute40,UploadedDocumentPath,ImportExcelFileName)
					
					VALUES( CASE WHEN UPPER(@BarcodeAutoGenerateEnable)='TRUE' and  UPPER(@IsBarcodeSettingApplyImportAsset)='TRUE' THEN '-' ELSE 
					CASE WHEN  UPPER(@BarcodeEqualAssetCode)='TRUE' and  UPPER(@IsBarcodeSettingApplyImportAsset)='TRUE' THEN @Barcode ELSE @AssetCode END END ,
					CASE WHEN UPPER(@BarcodeAutoGenerateEnable)='TRUE' and  UPPER(@IsBarcodeSettingApplyImportAsset)='TRUE' THEN '-' ELSE @Barcode END
					,@DepartmentID,@SECTIONID,@CUSTODIANID,@CATEGORYID,@LOCATIONID,@PRODUCTID,@MANUFACTURERID,@MODELID,@SUPPLIERID,@ASSETCONDITIONID,case when @PurchasePrice is not null and  @PurchasePrice!=' '  then convert(decimal(18,5),@PurchasePrice) else 0 end ,@PONUMBER,@REFERENCECODE,@SERIALNO,
					@assetDesc,@DELIVERYNOTE,@RFIDTAGCODE,@INVOICENO,@VOUCHERNO,@MAKE,@CAPACITY,@MAPPEDASSETID,@RECEIPTNUMBER,case when @PURCHASEDATE is not null then CONVERT(DATETIME,@PURCHASEDATE,103) else NULL end ,
					case when @CommissionDate is not null then CONVERT(DATETIME,@CommissionDate,103) else null end , case when @WarrantyExpiryDate is not null then CONVERT(DATETIME,@WarrantyExpiryDate,103) else NULL end ,
					case when @AssetApproval=1 then 5 else 1 end,
					@companyID,@userID,getdate(),case when @AssetApproval=1 then 0 else 1 end ,0,0,0,@Attribute1,@Attribute2,@Attribute3,@Attribute4,@Attribute5,
					@Attribute6,@Attribute7,@Attribute8,@Attribute9,@Attribute10,@Attribute11,@Attribute12,
					@Attribute13,@Attribute14,@Attribute15,@Attribute16,@AssetRemarks,@QFAssetCode,@Attribute17,@Attribute18,@Attribute19,@Attribute20,@Attribute21,@Attribute22,@Attribute23,@Attribute24,@Attribute25,@Attribute26,
					@Attribute27,@Attribute28,@Attribute29,@Attribute30,@Attribute31,@Attribute32,@Attribute33,@Attribute34,@Attribute35,@Attribute36,@Attribute37,@Attribute38,@Attribute39,@Attribute40,@UploadedDocumentPath,@ImportExcelFileName)
			End 
			Else 
			Begin 
			  INSERT INTO @MESSAGETABLE(TEXT)VALUES('Mandatory field are passed with empty '+@Barcode)
			End 
			 End 
			END 
			END 
			ELSE 
			BEGIN 
			IF EXISTS (SELECT barcode FROM dbo.AssetTable WHERE Barcode=@Barcode)
			BEGIN
			   If exists(select barcode  FROM dbo.AssetTable WHERE Barcode=@Barcode and statusID=1)
			   Begin 
				UPDATE ASSETTABLE SET
				--ASSETCODE=CASE WHEN UPPER(@BarcodeEqualAssetCode)='TRUE' THEN ISNULL(@Barcode,ASSETCODE) ELSE ISNULL(@ASSETCODE,ASSETCODE) END,
				ASSETCODE=CASE WHEN @ASSETCODE is null or @ASSETCODE='' then AssetCode else @ASSETCODE end,
				BARCODE=ISNULL(@Barcode,BARCODE),
				DEPARTMENTID=ISNULL(@DEPARTMENTID,DEPARTMENTID),
				SECTIONID=ISNULL(@SECTIONID,SECTIONID),
				CUSTODIANID=ISNULL(@CUSTODIANID,CUSTODIANID),
				CATEGORYID=ISNULL(@CATEGORYID,CATEGORYID),
				LOCATIONID=ISNULL(@LOCATIONID,LOCATIONID),
				PRODUCTID=ISNULL(@PRODUCTID,PRODUCTID),
				MANUFACTURERID=ISNULL(@MANUFACTURERID,MANUFACTURERID),
				MODELID=ISNULL(@MODELID,MODELID),
				SUPPLIERID=ISNULL(@SUPPLIERID,SUPPLIERID),
				ASSETCONDITIONID=ISNULL(@ASSETCONDITIONID,ASSETCONDITIONID),
				PONUMBER=CASE WHEN @PONUMBER IS NULL OR RTRIM(LTRIM(@PONUMBER))='' THEN PONumber ELSE @PONUMBER END ,
				REFERENCECODE=CASE WHEN @REFERENCECODE IS NULL OR RTRIM(LTRIM(@REFERENCECODE))='' THEN REFERENCECODE ELSE @REFERENCECODE END ,
				SERIALNO=CASE WHEN @SERIALNO IS NULL OR RTRIM(LTRIM(@SERIALNO))='' THEN SERIALNO ELSE @SERIALNO END ,
				ASSETDESCRIPTION=CASE WHEN @ValidateProduct=0 then ASSETDESCRIPTION else @assetDesc  END ,
				DELIVERYNOTE=CASE WHEN @DELIVERYNOTE IS NULL OR RTRIM(LTRIM(@DELIVERYNOTE))='' THEN DELIVERYNOTE ELSE @DELIVERYNOTE END,
				RFIDTAGCODE=CASE WHEN @RFIDTAGCODE IS NULL OR RTRIM(LTRIM(@RFIDTAGCODE))='' THEN RFIDTAGCODE ELSE @RFIDTAGCODE END,
				INVOICENO=CASE WHEN @INVOICENO IS NULL OR RTRIM(LTRIM(@INVOICENO))='' THEN INVOICENO ELSE @INVOICENO END,
				VOUCHERNO=CASE WHEN @VOUCHERNO IS NULL OR RTRIM(LTRIM(@VOUCHERNO))='' THEN VOUCHERNO ELSE @VOUCHERNO END,
				MAKE=CASE WHEN @MAKE IS NULL OR RTRIM(LTRIM(@MAKE))='' THEN MAKE ELSE @MAKE END,
				CAPACITY=CASE WHEN @CAPACITY IS NULL OR RTRIM(LTRIM(@CAPACITY))='' THEN CAPACITY ELSE @CAPACITY END,
				MAPPEDASSETID=CASE WHEN @MAPPEDASSETID IS NULL OR RTRIM(LTRIM(@MAPPEDASSETID))='' THEN MAPPEDASSETID ELSE @MAPPEDASSETID END,
				RECEIPTNUMBER=CASE WHEN @RECEIPTNUMBER IS NULL OR RTRIM(LTRIM(@RECEIPTNUMBER))='' THEN RECEIPTNUMBER ELSE @RECEIPTNUMBER END,
				PURCHASEDATE=CASE WHEN @PURCHASEDATE IS NULL OR RTRIM(LTRIM(@PURCHASEDATE))='' THEN PURCHASEDATE ELSE CONVERT(DATETIME,@PURCHASEDATE,103)  END,
				ComissionDate=CASE WHEN @CommissionDate IS NULL OR RTRIM(LTRIM(@CommissionDate))='' THEN ComissionDate ELSE CONVERT(DATETIME,@CommissionDate,103) END,
				WarrantyExpiryDate=CASE WHEN @WarrantyExpiryDate IS NULL OR RTRIM(LTRIM(@WarrantyExpiryDate))='' THEN WarrantyExpiryDate ELSE CONVERT(DATETIME,@WarrantyExpiryDate,103) END,			
				Attribute1=CASE WHEN @Attribute1 IS NULL OR RTRIM(LTRIM(@Attribute1))='' THEN Attribute1 ELSE @Attribute1 END,
				Attribute2=CASE WHEN @Attribute2 IS NULL OR RTRIM(LTRIM(@Attribute2))='' THEN Attribute2 ELSE @Attribute2 END,
				Attribute3=CASE WHEN @Attribute3 IS NULL OR RTRIM(LTRIM(@Attribute3))='' THEN Attribute3 ELSE @Attribute3 END,
				Attribute4=CASE WHEN @Attribute4 IS NULL OR RTRIM(LTRIM(@Attribute4))='' THEN Attribute4 ELSE @Attribute4 END,
				Attribute5=CASE WHEN @Attribute5 IS NULL OR RTRIM(LTRIM(@Attribute5))='' THEN Attribute5 ELSE @Attribute5 END,
				Attribute6=CASE WHEN @Attribute6 IS NULL OR RTRIM(LTRIM(@Attribute6))='' THEN Attribute6 ELSE @Attribute6 END,
				Attribute7=CASE WHEN @Attribute7 IS NULL OR RTRIM(LTRIM(@Attribute7))='' THEN Attribute7 ELSE @Attribute7 END,
				Attribute8=CASE WHEN @Attribute8 IS NULL OR RTRIM(LTRIM(@Attribute8))='' THEN Attribute8 ELSE @Attribute8 END,
				Attribute9=CASE WHEN @Attribute9 IS NULL OR RTRIM(LTRIM(@Attribute9))='' THEN Attribute9 ELSE @Attribute9 END,
				Attribute10=CASE WHEN @Attribute10 IS NULL OR RTRIM(LTRIM(@Attribute10))='' THEN Attribute10 ELSE @Attribute10 END,
				Attribute11=CASE WHEN @Attribute11 IS NULL OR RTRIM(LTRIM(@Attribute11))='' THEN Attribute11 ELSE @Attribute11 END,
				Attribute12=CASE WHEN @Attribute12 IS NULL OR RTRIM(LTRIM(@Attribute12))='' THEN Attribute12 ELSE @Attribute12 END,
				Attribute13=CASE WHEN @Attribute13 IS NULL OR RTRIM(LTRIM(@Attribute13))='' THEN Attribute13 ELSE @Attribute13 END,
				Attribute14=CASE WHEN @Attribute14 IS NULL OR RTRIM(LTRIM(@Attribute14))='' THEN Attribute14 ELSE @Attribute14 END,
				Attribute15=CASE WHEN @Attribute15 IS NULL OR RTRIM(LTRIM(@Attribute15))='' THEN Attribute15 ELSE @Attribute15 END,
				Attribute16=CASE WHEN @Attribute16 IS NULL OR RTRIM(LTRIM(@Attribute16))='' THEN Attribute16 ELSE @Attribute16 END,
				AssetRemarks=CASE WHEN @AssetRemarks IS NULL OR RTRIM(LTRIM(@AssetRemarks))='' THEN AssetRemarks ELSE @AssetRemarks END,
				QFAssetCode=CASE WHEN @QFAssetCode IS NULL OR RTRIM(LTRIM(@QFAssetCode))='' THEN QFAssetCode ELSE @QFAssetCode END,
				LastModifiedDateTime=getdate(),
				LastModifiedBy=@UserID ,
				Attribute17=CASE WHEN @Attribute17 IS NULL OR RTRIM(LTRIM(@Attribute17))='' THEN Attribute17 ELSE @Attribute17 END,
				Attribute18=CASE WHEN @Attribute18 IS NULL OR RTRIM(LTRIM(@Attribute18))='' THEN Attribute18 ELSE Attribute18 END,
				Attribute19=CASE WHEN @Attribute19 IS NULL OR RTRIM(LTRIM(@Attribute19))='' THEN Attribute19 ELSE @Attribute19 END,
				Attribute20=CASE WHEN @Attribute20 IS NULL OR RTRIM(LTRIM(@Attribute20))='' THEN Attribute20 ELSE @Attribute20 END,
			    Attribute21=CASE WHEN @Attribute21 IS NULL OR RTRIM(LTRIM(@Attribute21))='' THEN Attribute21 ELSE @Attribute21 END,
				Attribute22=CASE WHEN @Attribute22 IS NULL OR RTRIM(LTRIM(@Attribute22))='' THEN Attribute22 ELSE @Attribute22 END,
				Attribute23=CASE WHEN @Attribute23 IS NULL OR RTRIM(LTRIM(@Attribute23))='' THEN Attribute23 ELSE @Attribute23 END,
				Attribute24=CASE WHEN @Attribute24 IS NULL OR RTRIM(LTRIM(@Attribute24))='' THEN Attribute24 ELSE @Attribute24 END,
				Attribute25=CASE WHEN @Attribute25 IS NULL OR RTRIM(LTRIM(@Attribute25))='' THEN Attribute25 ELSE @Attribute25 END,
				Attribute26=CASE WHEN @Attribute26 IS NULL OR RTRIM(LTRIM(@Attribute26))='' THEN Attribute26 ELSE @Attribute26 END,
				Attribute27=CASE WHEN @Attribute27 IS NULL OR RTRIM(LTRIM(@Attribute27))='' THEN Attribute27 ELSE @Attribute27 END,
				Attribute28=CASE WHEN @Attribute28 IS NULL OR RTRIM(LTRIM(@Attribute28))='' THEN Attribute28 ELSE @Attribute28 END,
				Attribute29=CASE WHEN @Attribute29 IS NULL OR RTRIM(LTRIM(@Attribute29))='' THEN Attribute29 ELSE @Attribute29 END,
				Attribute30=CASE WHEN @Attribute30 IS NULL OR RTRIM(LTRIM(@Attribute30))='' THEN Attribute30 ELSE @Attribute30 END,
				Attribute31=CASE WHEN @Attribute31 IS NULL OR RTRIM(LTRIM(@Attribute31))='' THEN Attribute31 ELSE @Attribute31 END,
				Attribute32=CASE WHEN @Attribute32 IS NULL OR RTRIM(LTRIM(@Attribute32))='' THEN Attribute32 ELSE @Attribute32 END,
				Attribute33=CASE WHEN @Attribute33 IS NULL OR RTRIM(LTRIM(@Attribute33))='' THEN Attribute33 ELSE @Attribute33 END,
				Attribute34=CASE WHEN @Attribute34 IS NULL OR RTRIM(LTRIM(@Attribute34))='' THEN Attribute34 ELSE @Attribute34 END,
				Attribute35=CASE WHEN @Attribute35 IS NULL OR RTRIM(LTRIM(@Attribute35))='' THEN Attribute35 ELSE @Attribute35 END,
				Attribute36=CASE WHEN @Attribute36 IS NULL OR RTRIM(LTRIM(@Attribute36))='' THEN Attribute36 ELSE @Attribute36 END,
				Attribute37=CASE WHEN @Attribute37 IS NULL OR RTRIM(LTRIM(@Attribute37))='' THEN Attribute37 ELSE @Attribute37 END,
				Attribute38=CASE WHEN @Attribute38 IS NULL OR RTRIM(LTRIM(@Attribute38))='' THEN Attribute38 ELSE @Attribute38 END,
				Attribute39=CASE WHEN @Attribute39 IS NULL OR RTRIM(LTRIM(@Attribute39))='' THEN Attribute39 ELSE @Attribute39 END,
				Attribute40=CASE WHEN @Attribute40 IS NULL OR RTRIM(LTRIM(@Attribute40))='' THEN Attribute40 ELSE @Attribute40 END,
				PurchasePrice=case when @PurchasePrice is not null and  @PurchasePrice!=' '  then convert(decimal(18,5),@PurchasePrice) else PurchasePrice end,
				importExcelFileName=case when @ImportExcelFileName is not null and  @ImportExcelFileName!=' '  then @ImportExcelFileName else importExcelFileName end
				 WHERE BARCODE=@BARCODE 
				 End 
				 Else 
				 Begin 
				  INSERT INTO @MESSAGETABLE(TEXT)VALUES(@Barcode + '- This barcode is not Active status')
				 End 
			 END
			 ELSE
			 BEGIN 
				 INSERT INTO @MESSAGETABLE(TEXT)VALUES(@Barcode + '- This barcode not available in Assettable')
			 END 
			END 
			--select * from @MESSAGETABLE

			Select @ReturnMessage = COALESCE(@ReturnMessage + ', ' + Text, Text)  from @MESSAGETABLE
			select @ReturnMessage as ReturnMessage
        
			 
End
go 

ALTER trigger [dbo].[trg_Ins_AssetDataTable] on [dbo].[ZDOF_UserPODataTable] 
after Insert
As
Begin
   Declare @ProductID int,@categoryID int,@qty int ,@zDOFPurchaseOrderID int ,@PDHeaderID nvarchar(300) ,@PoLineItemID nvarchar(300),@ProductName nvarchar(max),@UnitCost decimal(18,5),@SupplierID int,@CreatedBy int 
   Declare @DefaultLanguageID INT,@CompanyID	INT,@Cnt int ,@LastBarcodeVal INT,@CompanyCode nvarchar(10),@LocationID int ,@ProductsID int
   Declare @BarcodeAuto bit ,@BarcodeAssetCodeequal bit ,@DepartmentID int 
     Select @BarcodeAuto=case when upper(ConfiguarationValue)='TRUE' then 1 else 0 end  From ConfigurationTable where ConfiguarationName='BarcodeAutoGenerateEnable'
	 Select @BarcodeAssetCodeequal=case when upper(ConfiguarationValue)='TRUE' then 1 else 0 end  From ConfigurationTable where ConfiguarationName='BarcodeEqualAssetCode'
	
	SELECT @DefaultLanguageID = MIN(LanguageID) FROM LanguageTable
	SELECT @CompanyID = MIN(CompanyID) FROM CompanyTable WHERE StatusID = 1
	Select @CompanyCode=left(companyCode,4) From CompanyTable where companyID=@CompanyID
  
    Select @qty=QUANTITY, @zDOFPurchaseOrderID=ZDOFPurchaseOrderID, @PDHeaderID=PO_HEADER_ID, @PoLineItemID=PO_LINE_ID, @ProductName=PO_LINE_DESC,@ProductsID=ProductID,
	@UnitCost=UnitCost,@categoryID=CategoryID,@CreatedBy=CreatedBy,@DepartmentID=DepartmentID,@LocationID=LocationID From Inserted
	
	IF NOT EXISTS(SELECT * FROM SupplierTable WHERE SupplierCode 
		IN(SELECT VENDOR_NUMBER FROM ZDOF_PurchaseOrderTable WHERE VENDOR_NUMBER IS NOT NULL AND ZDOFPurchaseOrderID = @zDOFPurchaseOrderID) )
	BEGIN
		--Generate Supplier data
		INSERT INTO PartyTable(partycode, StatusID, CreatedBy,PartyName,CreatedDateTime,PartyTypeID)
		SELECT VENDOR_NUMBER, 1, @CreatedBy,VENDOR_NAME,getdate(),(select partyTypeID from PartyTypeTable where PartyType='Supplier')
			FROM  ZDOF_PurchaseOrderTable A 
			LEFT JOIN PartyTable B ON A.VENDOR_NUMBER = B.PartyCode
			WHERE A.VENDOR_NUMBER IS NOT NULL AND A.VENDOR_NAME IS NOT NULL AND  A.ZDOFPurchaseOrderID = @zDOFPurchaseOrderID AND AssetsCreated = 0 AND B.PartyID IS NULL

		--INSERT INTO p(SupplierID, SupplierDescription, LanguageID, CreatedBy)
		--SELECT B.SupplierID, VENDOR_NAME, @DefaultLanguageID, @CreatedBy
		--	FROM  ZDOF_PurchaseOrderTable A 
		--	LEFT JOIN SupplierTable B ON A.VENDOR_NUMBER = B.SupplierCode
		--	WHERE A.VENDOR_NUMBER IS NOT NULL AND A.VENDOR_NAME IS NOT NULL AND  A.ZDOFPurchaseOrderID = @zDOFPurchaseOrderID AND AssetsCreated = 0 AND B.SupplierID IS NOT NULL
	END

    IF Not Exists (select ProductName from  ProductTable  where 
						 ProductID=@ProductsID and CategoryID=@CategoryID) 
	Begin 
	   Insert into ProductTable(ProductCode,CategoryID,StatusID,Createdby,CreatedDateTime,ProductName) 
	   values(@PoLineItemID,@categoryID,1,@CreatedBy,getdate(),@ProductName)
	   SET @ProductID = SCOPE_IDENTITY()
	   Insert into ProductDescriptionTable(ProductID,ProductDescription,LanguageID,Createdby,CreatedDatetime)
	   Select @ProductID,@ProductName,LanguageID,@CreatedBy,GETDATE()
	   From LanguageTable
	End 
	Else
	Begin 
	   Select @ProductID=ProductID from ProductTable  where categoryID=@categoryID and ProductID=@ProductsID
	End 
	set @cnt=0

		Select @LastBarcodeVal = BarcodeLastValue from BarcodeAutoSequenceTable where companyID=@CompanyID

			WHILE(@CNT < @qty)
			BEGIN
				SET @CNT = @CNT + 1
			----6065 Barcode sequence is wrong
			--	SELECT @LastBarcodeVal = MAX(CAST(SUBSTRING(Barcode, 5, 7) as int)) FROM AssetTable 
			--		WHERE AssetID <> 6065 AND ISNUMERIC(SUBSTRING(Barcode, 5, 7)) = 1 AND StatusID <> 3

			--	IF((@LastBarcodeVal IS NULL) OR (@LastBarcodeVal < 13247))
			--		SET @LastBarcodeVal  = 13247

			--	SET @LastBarcodeVal  = @LastBarcodeVal  + 1

			INSERT INTO ASSETTABLE (BARCODE,ASSETCODE,CATEGORYID,PRODUCTID,SUPPLIERID,PurchasePrice,
				PONUMBER,ASSETDESCRIPTION,PURCHASEDATE,ComissionDate,
				STATUSID,COMPANYID,CreatedBy,CreatedDateTime,AssetApproval,CreateFromHHT,ERPUpdateType,
				DepreciationFlag,Attribute1,DepartmentID, DOFPO_LINE_NUM,LocationID)
					
				SELECT 
					'-', '-',
					--'AT' + FORMAT(@LastBarcodeVal, '00000000'),
					--'AT' + FORMAT(@LastBarcodeVal, '00000000'),
				--Select case when @BarcodeAuto=1 then '-' else @CompanyCode + FORMAT(@LastBarcodeVal, '000000') end , 
				--case when @BarcodeAuto=1 and @BarcodeAssetCodeequal=1 then '-' else @CompanyCode+ FORMAT(@LastBarcodeVal, '000000') end ,
				@categoryID,@ProductID,@SupplierID,
				@UnitCost,PO_NUMBER,@ProductName,PO_DATE,PO_DATE,1,@CompanyID,@CreatedBy,getdate(),1,0, 5,
				0,VENDOR_SITE_CODE,@DepartmentID, ISNULL(LINE_NUM, 1),@LocationID
				From ZDOF_PurchaseOrderTable where ZDOFPurchaseOrderID = @zDOFPurchaseOrderID

			SET @LastBarcodeVal = @LastBarcodeVal + 1					
		End 

		--Update BarcodeAutoSequenceTable set BarcodeLastValue = @LastBarcodeVal where CompanyID=@CompanyID

		UPDATE ZDOF_PurchaseOrderTable 
					SET GeneratedAssetQty = GeneratedAssetQty + @qty
					WHERE ZDOFPurchaseOrderID = @zDOFPurchaseOrderID

		UPDATE ZDOF_PurchaseOrderTable 
		SET AssetsCreated = 1
		WHERE ZDOFPurchaseOrderID = @zDOFPurchaseOrderID AND CONVERT(INT,ROUND(QUANTITY,0,0),0)<= GeneratedAssetQty			
End 
go 
 
 /*
* Create Function for Split
* Table Name		: Split
* Created Date Time	: 01-June-2017 01:20:00
* Created By		: Saranyaa.T
* App Version		: 5.0.0.0
*/
ALTER FUNCTION [dbo].[Split]    
 (    
  @List nvarchar(max),    
  @SplitOn nvarchar(5)    
 )      
 RETURNS @RtnValue table     
 (    
  Id int identity(1,1),    
  Value nvarchar(100)    
 )     
 AS      
 BEGIN     
  While (Charindex(@SplitOn,@List)>0)    
  Begin    
	   Insert Into @RtnValue (value)    
	   Select Value = ltrim(rtrim(Substring(@List,1,Charindex(@SplitOn,@List)-1)))       		
	   Set @List = Substring(@List,Charindex(@SplitOn,@List)+len(@SplitOn),len(@List))    
  End    
	  Insert Into @RtnValue (Value)    
	  Select Value = ltrim(rtrim(@List))    
	  Return    
 END  
go
 Create Procedure [dbo].[Prc_ValidateBulkExcelRetirement] 
(
 @Barcode			nvarchar(max),
 @UserID			int,
 @ErrorID			int OutPut,
 @ErrorMessage		nvarchar(max) OutPut
)
as 
begin 
	DEclare @BarcodeTable Table(Barcode nvarchar(max),AssetID int,statusID int,locationType nvarchar(100),VerifyMappedLoc int 
								,verifyMappedCategory int,verifyMappedDepartment int)
	Declare @UserDepartmentMapping int ,@UserCategoryMapping int,@UserLocationMapping int ,@StatusID int 
	
	Select @statusID=[dbo].fnGetActiveStatusID()
	--select @Barcode
	select @UserDepartmentMapping= case when ConfiguarationValue='true' then 1 else 0 end from ConfigurationTable where ConfiguarationName='UserDepartmentMapping'
	select @UserCategoryMapping= case when ConfiguarationValue='true' then 1 else 0 end from ConfigurationTable where ConfiguarationName='UserCategoryMapping'
	select @UserLocationMapping= case when ConfiguarationValue='true' then 1 else 0 end from ConfigurationTable where ConfiguarationName='UserLocationMapping'
	
	insert into @BarcodeTable(Barcode,VerifyMappedLoc,verifyMappedCategory,verifyMappedDepartment)
	select value,0,0,0 from dbo.Split(@Barcode,',')
	--select value from dbo.Split(@Barcode,',')

	
	update a 
	set a.AssetID=b.AssetID,
	a.statusID=b.StatusID,
	a.verifyMappedDepartment=1
	--locationType=b.LocationType
	From @BarcodeTable a 
	join
		(select * from  AssetTable where CompanyID in (select companyID from UserCompanyMappingTable where UserID=@UserID) 
			and(@UserDepartmentMapping=0 or (departmentID in (select DepartmentID from UserDepartmentMappingTable where PersonID=@UserID)))
		--and(@UserCategoryMapping=0 or (MappedCategoryID in (select CategoryID from UserCategoryMappingTable where PersonID=@UserID)))
		--and(@UserDepartmentMapping=0 or (MappedLocationID in (select locationID from UserLocationMappingTable where PersonID=@UserID)))
		--and StatusID not in (select StatusID from StatusTable where status in ('Rejected','Delete','Deleted')
		)b on a.Barcode=B.Barcode
	
	update a 
	set a.VerifyMappedLoc=1
	From @BarcodeTable a 
	join
		(select * from  assetnewview where 
		(@UserLocationMapping=0 or (MappedLocationID in (select locationID from UserLocationMappingTable where PersonID=@UserID)))
		)b on a.assetid=B.assetid
	
	update a 
	set a.verifyMappedCategory=1
	From @BarcodeTable a 
	join
		(select * from  assetnewview where 
		(@UserCategoryMapping=0 or (MappedCategoryID in (select CategoryID from UserCategoryMappingTable where PersonID=@UserID)))
		)b on a.assetid=B.assetid
	
	update a 
	set a.locationType=b.LocationType
	From @BarcodeTable a 
	join
		(select * from  assetnewview) b on a.AssetID=b.AssetID
	Set @ErrorMessage=null
	Set @ErrorID=0

	
	if exists (select Barcode from @BarcodeTable where AssetID is null)
	Begin
	   Declare @MissingCategoryCode nvarchar(max)
	   Select @MissingCategoryCode=stuff((Select ',' + T.Barcode from @BarcodeTable T 
	                     where T.AssetID is null FOR XML PATH('')), 1, 1, '')

	    set @ErrorMessage =@MissingCategoryCode +' Barcode(s) are Invalid.'
		Set @ErrorID=1
		return 
	End 

	if exists (select Barcode from @BarcodeTable where statusID<>@statusID)
	Begin
	   Declare @MissingStatusCode nvarchar(max)
	   Select @MissingStatusCode=stuff((Select ',' + T.Barcode from @BarcodeTable T 
	                     where T.statusID<>@statusID FOR XML PATH('')), 1, 1, '')

	    set @ErrorMessage =@MissingStatusCode +' Barcode(s) Status is not Active.'
		Set @ErrorID=2
		return 
	End 
	
	--select * from @BarcodeTable where locationType is null
	if exists (select Barcode from @BarcodeTable where locationType is null)
	Begin
	   Declare @MissingLocationType nvarchar(max)
	   Select @MissingLocationType=stuff((Select ',' + T.Barcode from @BarcodeTable T 
	                     where T. locationType is null FOR XML PATH('')), 1, 1, '')

	    set @ErrorMessage =@MissingLocationType +' locationType Type(s) are Missed.'
		Set @ErrorID=3
		return 
	End 

	if exists (select Barcode from @BarcodeTable where verifyMappedCategory=0)
	Begin
	   Declare @MissingCategoryType nvarchar(max)
	   Select @MissingCategoryType=stuff((Select ',' + T.Barcode from @BarcodeTable T 
	                     where verifyMappedCategory=0 FOR XML PATH('')), 1, 1, '')

	    set @ErrorMessage =@MissingCategoryType +' Category not matched with configured user Mapping.'
		Set @ErrorID=4
		return 
	End 
	if exists (select Barcode from @BarcodeTable where verifyMappedLoc=0)
	Begin
	   Declare @MissingLocation nvarchar(max)
	   Select @MissingLocation=stuff((Select ',' + T.Barcode from @BarcodeTable T 
	                     where verifyMappedLoc=0 FOR XML PATH('')), 1, 1, '')

	    set @ErrorMessage =@MissingLocation +' Location not matched with configured user Mapping.'
		Set @ErrorID=5
		return 
	End 
	if exists (select Barcode from @BarcodeTable where verifyMappedDepartment=0)
	Begin
	   Declare @MissingDepartment nvarchar(max)
	   Select @MissingDepartment=stuff((Select ',' + T.Barcode from @BarcodeTable T 
	                     where verifyMappedDepartment=0 FOR XML PATH('')), 1, 1, '')

	    set @ErrorMessage =@MissingDepartment +' Department not matched with configured user Mapping.'
		Set @ErrorID=6
		return 
	End 
	IF (SELECT COUNT(DISTINCT locationType) FROM @BarcodeTable group by locationType) > 1
	Begin
	   --Declare @MissingLocType nvarchar(max)
	   --Select @MissingLocType=stuff((Select ',' + T.Barcode from @BarcodeTable T 
	   --                  where verifyMappedDepartment =0 FOR XML PATH('')), 1, 1, '')

	    set @ErrorMessage = ' Barcode have different Locationtype..'
		Set @ErrorID=7
		return 
	End 
	End 

	go 
	alter PROC [dbo].[zDOF_AssetIntegrationDetails]
	@AssetID INT
AS
BEGIN
	SELECT AV.AssetCode, A.TransactionType, A.ErrorMessage as Messages, A.ErrorDateTime
		FROM [zDOFIntegrationErrors] A
		LEFT JOIN AssetNewView AV ON AV.AssetID = A.EntityID
		WHERE A.EntityID = @AssetID
			AND A.EntityType = 'AssetTable'
	 union all 
	 SELECT AV.AssetCode, A.TransactionType, A.TransactionMessage as Messages, A.TransactionDateTime
		FROM zDOFIntegrationLogTable A
		LEFT JOIN AssetNewView AV ON AV.AssetID = A.EntityID
		WHERE A.EntityID = @AssetID
			AND A.EntityType = 'AssetTable'
END
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
a.ProductCode,a.ProductName,AssetRemarks, case when MappedAssetID=''NULL'' then NULL else MappedAssetID End  as MappedAssetNo , DVT.DepreciationName as DepreciationName,DVT.DepreciationStartDate as DepreciationStartDate,DVT.DepreciationPeriod as DepreciationPeriod,
NetworkID as OwnedLeased,InvoiceNo,InvoiceDate,partialDisposalTotalValue,case when CreateFromHHT=0 then ''False'' else ''True'' End as CreateFromHHT,a. CreateBy as CreatedBy,a. ModifedBy,A.LastModifiedDateTime as ModifedDate,DisposalReferenceNo,
DisposedDateTime,DisposedRemarks,DisposalValue,a.CompanyCode,a. CompanyName, 
a.FirstLevelCategoryName,a.SecondLevelCategoryName,
a.FirstLevelLocationName,a.SecondLevelLocationName,
A.LocationHierarchy,A.CategoryHierarchy,A.Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,
A.Attribute6,A.Attribute7,A.Attribute8,A.Attribute9,A.Attribute10,A.Attribute11,A.Attribute12,
A.Attribute13,A.Attribute14,
A.Attribute15,A.Attribute16,a.UploadedImagePath as AssetImage, A.Latitude, A.Longitude,
A.Attribute17,A.Attribute18,Attribute19,Attribute20,Attribute21,Attribute22,Attribute23,
Attribute24,Attribute25,Attribute26,Attribute27,Attribute28,Attribute29,Attribute30,Attribute31,Attribute32,Attribute33,
Attribute34,Attribute35,Attribute36,Attribute37,Attribute38,Attribute39,Attribute40
from AssetNewView A 
left join AssetDepreciationTable DVT on a.AssetID=DVT.AssetID 

Left join @ParentChildCountTable Parent on A.assetID=Parent.ParentAssetID 
where A.StatusID not in (500,3) and 1=@LanguageID and A.companyID in (Select Value from Split('''+@companyID+''','','')) '
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
								   set @DynamicQuery=@DynamicQuery+' and (A.LocationL1 in (select LocationID from UserLocationMappingTable where personID=@UserId and StatusID=1))'
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
go 
create view ZDOF_MassAdditionView 
as 
Select MassAdditionID,DOFMassAdditionID,DOFParentMassAdditionID,ItemDescription,PONumber,POLineNo,VendorID,
VendorNumber,InvoiceNumber,InvoiceLineNo,InvoiceDate,ReceiptNumber,FixedAssetUnits,AssetCost,PostingStatus,CreatedDateTime
from ZDOF_MassAdditionTable
where PostingStatus='NEW' and MassAdditionID not in (select MassAdditionID from ZDOF_UpdateAssetCostTable)
go 
create Procedure prc_UpdateAssetCost
(
@PONumber		nvarchar(max)=NULL,
@InvoiceNo		nvarchar(100)=NULL,
@POLineNo		nvarchar(100)=NULL,
@MassAdditional nvarchar(100)=NULL,
@Remarks		nvarchar(max)=NULL,
@Barcode		nvarchar(max)=NULL,
@AdditionalCost nvarchar(max)=NULL,
@UserID			int=NULL
--@ErrorID			int OutPut,
--@ErrorMessage		nvarchar(max) OutPut
)
as 
Begin
	Declare @UpdateTable Table(Barcode int,AdditionalCost decimal(18,5))

	  DECLARE @BarcodeTable TABLE (Value int);
    INSERT INTO @BarcodeTable (Value)
    SELECT TRY_CAST(LTRIM(RTRIM(value)) as int)
    FROM STRING_SPLIT(@Barcode, ',')

	 DECLARE @AdditionalCostTable TABLE (Value DECIMAL(18, 5));
    INSERT INTO @AdditionalCostTable (Value)
    SELECT TRY_CAST(LTRIM(RTRIM(value)) AS DECIMAL(18, 5))
    FROM STRING_SPLIT(@AdditionalCost, ',')

	  IF (SELECT COUNT(*) FROM @BarcodeTable) <> (SELECT COUNT(*) FROM @AdditionalCostTable)
    BEGIN
        RAISERROR ('The number of Barcode and AdditionalCost values must match.', 16, 1);
        RETURN;
    END;
	INSERT INTO @UpdateTable (Barcode, AdditionalCost)
    SELECT b.Value, c.Value
    FROM @BarcodeTable b
    CROSS APPLY (
        SELECT TOP 1 Value
        FROM @AdditionalCostTable
        WHERE Value IS NOT NULL
        EXCEPT
        SELECT AdditionalCost FROM @UpdateTable
    ) c
	Declare @ErrorMessage nvarchar(max)
	--Set @ErrorMessage=null
	--Set @ErrorID=0
	declare @cost decimal(18,5) ,@AddtionalCost decimal(18,5),@PrimarykeyID int 
	select @AddtionalCost=sum(AdditionalCost) from @UpdateTable
	if @AddtionalCost=0
	begin
	set @ErrorMessage =' Please enter additional cost for any asset'
	
	end 

	select @cost=AssetCost from ZDOF_MassAdditionTable where MassAdditionID=@MassAdditional

	if(@cost<@AdditionalCost)
	Begin 
	set @ErrorMessage = @ErrorMessage +'  Additional cost should not be more than'+ cast(@cost as nvarchar(max)) + '. Current: ' + cast(@AdditionalCost as nvarchar(max))
	--Set @ErrorID=2
	end 

	insert into ZDOF_UpdateAssetCostTable(PONumber,InvoiceNo,POLineNo,MassAdditionID,StatusID,Remarks,CreatedBy,CreatedDateTime)
	values(@PONumber,@InvoiceNo,@POLineNo,@MassAdditional,1,@Remarks,@UserID,GETDATE())
	
	SET @PrimarykeyID = SCOPE_IDENTITY()

	insert into ZDOF_UpdateAssetCostLineItemTable(UpdateAssetCostID,AssetID,NewPrice)
	select @PrimarykeyID,Barcode,AdditionalCost
	from @UpdateTable

	select @ErrorMessage as result

End 

go 
if not exists(select MenuName from User_MenuTable where MenuName='UpdateAssetCost' and ParentTransactionID=1)
Begin
Insert into User_MenuTable(MenuName,RightID,TargetObject,ParentMenuID,OrderNo,Image,ParentTransactionID)
select 'UpdateAssetCost',RightID,'/UpdateAssetCost/Index',2,10,'css/images/MenuIcon/UpdateAssetCost.png',1
from User_RightTable where  RightName='UpdateAssetCost'
End 
go 
update MasterGridNewLineItemTable set Format='dd/MM/yyyy' where ColumnType='System.DateTime'
and Format is null

go 
update EntitySingleActionTable set entityquery='Select AT.Code,AT.Name,AT.CodeTitle,AT.NameTitle,AT.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,
AT.ActionType,AT.UserName,    AT.FieldName,AT.OldValue,AT.NewValue,  AT. personID,AT.AuditedObjectKeyValue1,AT.AuditedObjectKeyValue2,
AT.LanguageID  From       (  Select D.Barcode as Code,D.AssetDescription as Name ,''Barcode'' as CodeTitle ,''Asset Description'' as NameTitle, 
AL.EntityName,  Convert(nvarchar,AT.AuditLogTransactionDateTime,103) as AuditLogTransactionDateTime,AT.AuditLogTransactionID,    
Case AL.ActionType   when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''end as ActionType,
P.PersonFirstName+'' ''+P.PersonLastName as UserName,  Case APPROVE.StatusId when 6 then   Case When APPROVELINEITEM.FieldName = ''Status ID'' Then ''Status''  
When APPROVELINEITEM.FieldName = ''Asset Condition ID'' Then ''Asset Condition''       
When APPROVELINEITEM.FieldName = ''Department ID'' Then ''Department Name'' When APPROVELINEITEM.FieldName = ''Section ID'' Then ''Section Name'' 
When APPROVELINEITEM.FieldName = ''Location ID'' Then ''Location name''   When APPROVELINEITEM.FieldName = ''Custodian ID'' Then ''Custodian Name''    
When APPROVELINEITEM.FieldName = ''Category ID'' Then ''Category Name''  When APPROVELINEITEM.FieldName = ''Depreciation Class ID'' Then ''Depreciation Class''   
When APPROVELINEITEM.FieldName = ''Supplier ID'' Then ''Supplier Name'' When APPROVELINEITEM.FieldName = ''Product ID'' Then ''Asset Description''       
When APPROVELINEITEM.FieldName = ''Uploaded Document Path'' Then ''Uploaded Document''   when  APPROVELINEITEM.FieldName = ''Uploaded Image Path''    then ''Uploaded Image''    
when  APPROVELINEITEM.FieldName = ''Network ID''  then ''Owned/ Leased''   Else APPROVELINEITEM.FieldName End      else  Case When ATL.FieldName = ''Status ID'' Then ''Status''
When ATL.FieldName = ''Asset Condition ID'' Then ''Asset Condition''      
When ATL.FieldName = ''Department ID'' Then ''Department Name'' 
When ATL.FieldName = ''Section ID'' Then ''Section Name''   
When ATL.FieldName = ''Location ID'' Then ''Location name''   
When ATL.FieldName = ''Custodian ID'' Then ''Custodian Name''     
When ATL.FieldName = ''Category ID'' Then ''Category Name''  
When ATL.FieldName = ''Depreciation Class ID'' Then ''Depreciation Class''     
When ATL.FieldName = ''Supplier ID'' Then ''Supplier Name'' 
When APPROVELINEITEM.FieldName = ''Product ID'' Then ''Asset Description''      
When ATL.FieldName = ''Uploaded Document Path'' Then ''Uploaded Document'' 
when  ATL.FieldName = ''Uploaded Image Path''  then ''Uploaded Image''       
when  APPROVELINEITEM.FieldName = ''Network ID''  then ''Owned/ Leased''   Else ATL.FieldName End End as FieldName,     

Case  When (ATL.FieldName = ''Status ID'' or APPROVELINEITEM.FieldName=''Status ID'') Then ISNULL((Select SST.Status From StatusTable SST Where convert(varchar(100),SST.StatusID) = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.OldValue else  ATL.OldValue end )),''-'')  
When (ATL.FieldName = ''Asset Condition ID'' or APPROVELINEITEM.FieldName=''Asset Condition ID'') Then ISNULL((Select SST.AssetConditionName From AssetConditionTable SST  Where convert(varchar(100),SST.AssetConditionID) = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.OldValue else  ATL.OldValue end )),''-'')    
When (ATL.FieldName = ''Department ID'' or APPROVELINEITEM.FieldName=''Department ID'') Then ISNULL((Select SST.DepartmentName From DepartmentTable SST  Where convert(varchar(100),SST.DepartmentID) = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.OldValue else  ATL.OldValue end )),''-'')      
When (ATL.FieldName = ''Section ID'' or APPROVELINEITEM.FieldName=''Section ID'') Then ISNULL((Select SST.SectionName From SectionTable SST  Where convert(varchar(100),SST.SectionID) = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.OldValue else  ATL.OldValue end )),''-'')      
When (ATL.FieldName = ''Location ID'' or APPROVELINEITEM.FieldName=''Location ID'') Then ISNULL((Select SST.LocationNameHierarchy From tmp_LocationNewHierarchicalView SST Where languageID=L.LanguageID and convert(varchar(100),SST.LocationID) = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.OldValue else  ATL.OldValue end )),''-'')     
When (ATL.FieldName = ''Custodian ID'' or APPROVELINEITEM.FieldName=''Custodian ID'')   Then ISNULL((Select SST.PersonfirstName From PersonTable SST Where convert(varchar(100),SST.PersonID) = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.OldValue else  ATL.OldValue end )),''-'')      
When (ATL.FieldName = ''Category ID'' or APPROVELINEITEM.FieldName=''Category ID'')   Then ISNULL((Select SST.CategoryNameHierarchy From tmp_CategoryNewHierarchicalView SST Where languageID=L.LanguageID and convert(varchar(100),SST.CategoryID) = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.OldValue else  ATL.OldValue end )),''-'')          
When (ATL.FieldName = ''Depreciation Class ID'' or APPROVELINEITEM.FieldName=''Depreciation Class ID'')  Then ISNULL((Select SST.ClassName From DepreciationClassTable SST Where convert(varchar(100),SST.DepreciationClassID) = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.OldValue else  ATL.OldValue end )),''-'')  
When (ATL.FieldName = ''Supplier ID'' or APPROVELINEITEM.FieldName=''Supplier ID'') Then ISNULL((Select SST.PartyName From PartyTable SST  Where convert(varchar(100),SST.PartyID) =(Case APPROVE.StatusId when 6 then APPROVELINEITEM.OldValue else  ATL.OldValue end )),''-'')    
When (ATL.FieldName = ''Product ID'' or APPROVELINEITEM.FieldName=''Product ID'') Then ISNULL((Select SST.ProductName From ProductTable SST Where convert(varchar(100),SST.productID) =(Case APPROVE.StatusId when 6 then APPROVELINEITEM.OldValue else  ATL.OldValue end )),''-'')      
When (ATL.FieldName = ''Uploaded Document Path'' or APPROVELINEITEM.FieldName=''Uploaded Document Path'')  then   isnull(Case APPROVE.StatusId when 6 then RIGHT( APPROVELINEITEM.OldValue, CHARINDEX( ''\'', REVERSE( APPROVELINEITEM.OldValue ) + ''\'' ) - 1 )  else  RIGHT( ATL.OldValue, CHARINDEX( ''\'', REVERSE( ATL.OldValue ) + ''\'' ) - 1 )    end,''-'' )    when (ATL.FieldName = ''Uploaded Image Path'' or APPROVELINEITEM.FieldName=''Uploaded Image Path'') then isnull(Case APPROVE.StatusId when 6 then   RIGHT( APPROVELINEITEM.OldValue, CHARINDEX( ''\'', REVERSE( APPROVELINEITEM.OldValue ) + ''\'' ) - 1 )   else  RIGHT( ATL.OldValue, CHARINDEX( ''\'', REVERSE( ATL.OldValue ) + ''\'' ) - 1 )    end ,''-'')     
Else ISNULL(Case APPROVE.StatusId when 6 then   case when (APPROVELINEITEM.OldValue is null or APPROVELINEITEM.OldValue='''') then ''-'' else APPROVELINEITEM.OldValue end    else  ATL.OldValue end ,''-'')  End as OldValue,        

Case When (ATL.FieldName = ''Status ID'' or APPROVELINEITEM.FieldName=''Status ID'')  Then ISNULL((Select SST.Status From StatusTable SST Where convert(varchar(100),SST.StatusID) = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.NewValue else  ATL.NewValue end )),''-'')     
when (ATL.FieldName = ''Asset Condition ID'' or APPROVELINEITEM.FieldName=''Asset Condition ID'') Then ISNULL((Select SST.AssetConditionName From AssetConditionTable SST Where convert(varchar(100),SST.AssetConditionID) = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.NewValue else  ATL.NewValue end )),''-'')    
When (ATL.FieldName = ''Department ID'' or APPROVELINEITEM.FieldName=''Department ID'') Then ISNULL((Select SST.DepartmentName From DepartmentTable SST  Where convert(varchar(100),SST.DepartmentID) = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.NewValue else  ATL.NewValue end )),''-'')       
When (ATL.FieldName = ''Section ID'' or APPROVELINEITEM.FieldName=''Section ID'') Then ISNULL((Select SST.SectionName From SectionTable SST  Where convert(varchar(100),SST.SectionID) = (Case APPROVE.StatusId when 6 then  APPROVELINEITEM.NewValue else  ATL.NewValue end )),''-'')       
When (ATL.FieldName = ''Location ID'' or APPROVELINEITEM.FieldName=''Location ID'') Then ISNULL((Select SST.LocationNameHierarchy From tmp_LocationnewHierarchicalView SST Where languageID=L.LanguageID and convert(varchar(100),SST.LocationID) = (Case APPROVE.StatusId  when 6 then APPROVELINEITEM.NewValue else  ATL.NewValue end )),''-'')        
When (ATL.FieldName = ''Custodian ID'' or APPROVELINEITEM.FieldName=''Custodian ID'')  Then ISNULL((Select SST.PersonFirstName From PersonTable SST Where convert(varchar(100),SST.PersonID) = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.NewValue else  ATL.NewValue end )),''-'')   
When (ATL.FieldName = ''Category ID'' or APPROVELINEITEM.FieldName=''Category ID'') Then ISNULL((Select SST.CategoryNameHierarchy From tmp_CategoryNewHierarchicalView SST Where languageID=L.LanguageID and convert(varchar(100),SST.CategoryID) = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.NewValue else  ATL.NewValue end )),''-'')   
When (ATL.FieldName = ''Depreciation Class ID'' or APPROVELINEITEM.FieldName=''Depreciation Class ID'')Then ISNULL((Select SST.ClassName From DepreciationClassTable SST Where convert(varchar(100),SST.DepreciationClassID) = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.NewValue else  ATL.NewValue end )),''-'')       
When (ATL.FieldName = ''Supplier ID'' or APPROVELINEITEM.FieldName=''Supplier ID'') Then ISNULL((Select SST.PartyName From PartyTable SST Where convert(varchar(100),SST.PartyID) =(Case APPROVE.StatusId when 6 then APPROVELINEITEM.NewValue  else  ATL.NewValue end )),''-'')     
When (ATL.FieldName = ''Product ID'' or APPROVELINEITEM.FieldName=''Product ID'') Then ISNULL((Select SST.ProductName From ProductTable SST Where  convert(varchar(100),SST.productID) =(Case APPROVE.StatusId when 6 then APPROVELINEITEM.NewValue  else  ATL.NewValue end )),''-'')     
When (ATL.FieldName = ''Uploaded Document Path'' or APPROVELINEITEM.FieldName=''Uploaded Document Path'')  then   isnull(Case APPROVE.StatusId when 6 then RIGHT( APPROVELINEITEM.NewValue, CHARINDEX( ''\'', REVERSE( APPROVELINEITEM.NewValue ) + ''\'' ) - 1 )  else  RIGHT( ATL.NewValue, CHARINDEX( ''\'', REVERSE( ATL.NewValue ) + ''\'' ) - 1 )    end,''-'' )      
when (ATL.FieldName = ''Uploaded Image Path'' or APPROVELINEITEM.FieldName=''Uploaded Image Path'') then isnull(Case APPROVE.StatusId when 6 then   RIGHT( APPROVELINEITEM.NewValue, CHARINDEX( ''\'', REVERSE( APPROVELINEITEM.NewValue ) + ''\'' ) - 1 )   else  RIGHT( ATL.NewValue, CHARINDEX( ''\'', REVERSE( ATL.NewValue ) + ''\'' ) - 1 )    end ,''-'')    Else ISNULL(Case APPROVE.StatusId when 6 then APPROVELINEITEM.NewValue else  ATL.NewValue end ,''-'')   End as NewValue,   
P.PersonID ,AL.AuditedObjectKeyValue1,Al.AuditedObjectKeyValue2,L.LanguageID  From AssetTable D     
join LanguageTable L on 1=1   join AuditLogTable AL on D.AssetID=convert(int,AL.AuditedObjectKeyValue1)   
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID    
INNER JOIN AuditLogLineItemTable ATL ON AL.AuditLogID=ATL.AuditLogID      
LEFT OUTER JOIN ApprovalTable APPROVE on AL.AuditedObjectKeyValue1 = APPROVE.ObjectKeyValue    
and APPROVE.StatusID = 6 and APPROVE.ActionType in (2) and   cast((convert(CHAR(11),AT.AuditLogTransactionDateTime,101)) as datetime)=cast(convert(char(11),APPROVE.CreatedDatetime,101) as datetime)    
LEFT OUTER JOIN ApprovalLineItemTable APPROVELINEITEM on APPROVE.ApprovalID = APPROVELINEITEM.ApprovalID    
join PersonTable P on P.PersonID=AT.UserID   where AL.EntityName=''AssetTable''  and AT.url not like ''%AssetMapping%''     
and AT.url not like''%AttachDetachApproval%'' and AT.url not like''%TransferAsset%''  and AT.url not like ''%PartialDisposedAsset%'' 
and AT.url not like ''%TransferCategory%'' and ATL.Fieldname!=''Asset Description'') AT    
where AT.FieldName not in (''Asset Approval'',''Mail Alert'') and at.oldValue!=at.NewValue'
where entityname='AssetTable'
go 

  
ALTER trigger [dbo].[trg_Ins_AssetDataTable] on [dbo].[ZDOF_UserPODataTable] 
after Insert
As
Begin
   Declare @ProductID int,@categoryID int,@qty int ,@zDOFPurchaseOrderID int ,@PDHeaderID nvarchar(300) ,@PoLineItemID nvarchar(300),@ProductName nvarchar(max),@UnitCost decimal(18,5),@SupplierID int,@CreatedBy int 
   Declare @DefaultLanguageID INT,@CompanyID	INT,@Cnt int ,@LastBarcodeVal INT,@CompanyCode nvarchar(10),@LocationID int ,@ProductsID int
   Declare @BarcodeAuto bit ,@BarcodeAssetCodeequal bit ,@DepartmentID int 
     Select @BarcodeAuto=case when upper(ConfiguarationValue)='TRUE' then 1 else 0 end  From ConfigurationTable where ConfiguarationName='BarcodeAutoGenerateEnable'
	 Select @BarcodeAssetCodeequal=case when upper(ConfiguarationValue)='TRUE' then 1 else 0 end  From ConfigurationTable where ConfiguarationName='BarcodeEqualAssetCode'
	
	SELECT @DefaultLanguageID = MIN(LanguageID) FROM LanguageTable
	SELECT @CompanyID = MIN(CompanyID) FROM CompanyTable WHERE StatusID = 1
	Select @CompanyCode=left(companyCode,4) From CompanyTable where companyID=@CompanyID
  
    Select @qty=QUANTITY, @zDOFPurchaseOrderID=ZDOFPurchaseOrderID, @PDHeaderID=PO_HEADER_ID, @PoLineItemID=PO_LINE_ID, @ProductName=PO_LINE_DESC,@ProductsID=ProductID,
	@UnitCost=UnitCost,@categoryID=CategoryID,@CreatedBy=CreatedBy,@DepartmentID=DepartmentID,@LocationID=LocationID From Inserted
	
	IF NOT EXISTS(SELECT * FROM SupplierTable WHERE SupplierCode 
		IN(SELECT VENDOR_NUMBER FROM ZDOF_PurchaseOrderTable WHERE VENDOR_NUMBER IS NOT NULL AND ZDOFPurchaseOrderID = @zDOFPurchaseOrderID) )
	BEGIN
		--Generate Supplier data
		INSERT INTO PartyTable(partycode, StatusID, CreatedBy,PartyName,CreatedDateTime,PartyTypeID)
		SELECT VENDOR_NUMBER, 1, @CreatedBy,VENDOR_NAME,getdate(),(select partyTypeID from PartyTypeTable where PartyType='Supplier')
			FROM  ZDOF_PurchaseOrderTable A 
			LEFT JOIN PartyTable B ON A.VENDOR_NUMBER = B.PartyCode
			WHERE A.VENDOR_NUMBER IS NOT NULL AND A.VENDOR_NAME IS NOT NULL AND  A.ZDOFPurchaseOrderID = @zDOFPurchaseOrderID AND AssetsCreated = 0 AND B.PartyID IS NULL

		--INSERT INTO p(SupplierID, SupplierDescription, LanguageID, CreatedBy)
		--SELECT B.SupplierID, VENDOR_NAME, @DefaultLanguageID, @CreatedBy
		--	FROM  ZDOF_PurchaseOrderTable A 
		--	LEFT JOIN SupplierTable B ON A.VENDOR_NUMBER = B.SupplierCode
		--	WHERE A.VENDOR_NUMBER IS NOT NULL AND A.VENDOR_NAME IS NOT NULL AND  A.ZDOFPurchaseOrderID = @zDOFPurchaseOrderID AND AssetsCreated = 0 AND B.SupplierID IS NOT NULL
	END

    IF Not Exists (select ProductName from  ProductTable  where 
						 ProductID=@ProductsID and CategoryID=@CategoryID) 
	Begin 
	   Insert into ProductTable(ProductCode,CategoryID,StatusID,Createdby,CreatedDateTime,ProductName) 
	   values(@PoLineItemID,@categoryID,1,@CreatedBy,getdate(),@ProductName)
	   SET @ProductID = SCOPE_IDENTITY()
	   Insert into ProductDescriptionTable(ProductID,ProductDescription,LanguageID,Createdby,CreatedDatetime)
	   Select @ProductID,@ProductName,LanguageID,@CreatedBy,GETDATE()
	   From LanguageTable
	End 
	Else
	Begin 
	   Select @ProductID=ProductID from ProductTable  where categoryID=@categoryID and ProductID=@ProductsID
	End 
	set @cnt=0

		Select @LastBarcodeVal = BarcodeLastValue from BarcodeAutoSequenceTable where companyID=@CompanyID

			WHILE(@CNT < @qty)
			BEGIN
				SET @CNT = @CNT + 1
			----6065 Barcode sequence is wrong
			--	SELECT @LastBarcodeVal = MAX(CAST(SUBSTRING(Barcode, 5, 7) as int)) FROM AssetTable 
			--		WHERE AssetID <> 6065 AND ISNUMERIC(SUBSTRING(Barcode, 5, 7)) = 1 AND StatusID <> 3

			--	IF((@LastBarcodeVal IS NULL) OR (@LastBarcodeVal < 13247))
			--		SET @LastBarcodeVal  = 13247

			--	SET @LastBarcodeVal  = @LastBarcodeVal  + 1

			INSERT INTO ASSETTABLE (BARCODE,ASSETCODE,CATEGORYID,PRODUCTID,SUPPLIERID,PurchasePrice,
				PONUMBER,ASSETDESCRIPTION,PURCHASEDATE,ComissionDate,
				STATUSID,COMPANYID,CreatedBy,CreatedDateTime,AssetApproval,CreateFromHHT,ERPUpdateType,
				DepreciationFlag,Attribute1,DepartmentID, DOFPO_LINE_NUM,LocationID)
					
				SELECT 
					'-', '-',
					--'AT' + FORMAT(@LastBarcodeVal, '00000000'),
					--'AT' + FORMAT(@LastBarcodeVal, '00000000'),
				--Select case when @BarcodeAuto=1 then '-' else @CompanyCode + FORMAT(@LastBarcodeVal, '000000') end , 
				--case when @BarcodeAuto=1 and @BarcodeAssetCodeequal=1 then '-' else @CompanyCode+ FORMAT(@LastBarcodeVal, '000000') end ,
				@categoryID,@ProductID,@SupplierID,
				@UnitCost,PO_NUMBER,@ProductName,PO_DATE,PO_DATE,1,@CompanyID,@CreatedBy,getdate(),1,0, 5,
				0,VENDOR_SITE_CODE,@DepartmentID, ISNULL(LINE_NUM, 1),@LocationID
				From ZDOF_PurchaseOrderTable where ZDOFPurchaseOrderID = @zDOFPurchaseOrderID

			SET @LastBarcodeVal = @LastBarcodeVal + 1					
		End 

		--Update BarcodeAutoSequenceTable set BarcodeLastValue = @LastBarcodeVal where CompanyID=@CompanyID

		UPDATE ZDOF_PurchaseOrderTable 
					SET GeneratedAssetQty = GeneratedAssetQty + @qty
					WHERE ZDOFPurchaseOrderID = @zDOFPurchaseOrderID

		UPDATE ZDOF_PurchaseOrderTable 
		SET AssetsCreated = 1
		WHERE ZDOFPurchaseOrderID = @zDOFPurchaseOrderID AND CONVERT(INT,ROUND(QUANTITY,0,0),0)<= GeneratedAssetQty			
End 
go 
Create Table AttributeDefinitionTable
(
AttributeDefinitionID int not null primary key identity(1,1),
EntityID int not null foreign key references EntityTable(EntityID),
AttributeName varchar(100) not null,
DisplayTitle nvarchar(200) not null,
ToolTipName nvarchar(100)  null,
DataType tinyint not null foreign key references DataTypeTable(DataTypeID),
DisplayControl tinyint not null foreign key references DisplayControlTypeTable(DisplayControlTypeID),
IsMandatory bit not null default(0),
StringSize int not null default(50),
MinValue varchar(50) NULL,
MaxValue varchar(50) NULL,
StepIncrement float NULL,
DisplayOrderID int not null,
DefaultValue nvarchar(2000) NULL,
SelectionFieldValues nvarchar(4000) NULL,
SelectQuery nvarchar(2000) NULL,
ValueField varchar(100) null,
DisplayField varchar(100) NULL,
RequiredOnHHT bit not null default(0),
MaintainHHTDataOnSeparateTable   bit not null default(0),
MinDateValue smalldatetime NULL,
MaxDateValue smalldatetime NULL,
VisibleToTransferAssetScreen  bit not null default(0),
RangeValidateRequired  bit not null default(0),
RangeValidateFrom int NULL foreign key references AttributeDefinitionTable(AttributeDefinitionID),
ComparisonOperator nvarchar(20) NULL,
CasCadingFieldRequired bit not null default(0),
CasCadingFieldName nvarchar(20) NULL,
StatusID int not null foreign key references StatusTable(StatusID),
CreatedBy int not null foreign key references User_LoginUserTable(UserID),
CreatedDateTime SmallDateTime not null,
ModifiedBy int  null foreign key references User_LoginUserTable(UserID),
ModifiedDateTime SmallDateTime  null
)
go 
If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='AttributeDefinition' and ParentTransactionID=1)
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO,ParentTransactionID) Values(
'AttributeDefinition',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='AttributeDefinition'),'/MasterPage/Index?pageName=AttributeDefinition',
(Select MenuID from USER_MENUTABLE where MenuName='Templates' ),15,1);
End
Go

Alter table ScreenFilterLineItemTable drop constraint FK__ScreenFil__Field__44D52468
go 
Alter table ScreenFilterLineItemTable add constraint  FK_ScreenFilterLineItem_AFieldTypeTable
    FOREIGN KEY (FieldTypeID) REFERENCES AFieldTypeTable(FieldTypeID)
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
a.ProductCode,a.ProductName,AssetRemarks, case when MappedAssetID=''NULL'' then NULL else MappedAssetID End  as MappedAssetNo , DVT.DepreciationName as DepreciationName,DVT.DepreciationStartDate as DepreciationStartDate,DVT.DepreciationPeriod as DepreciationPeriod,
NetworkID as OwnedLeased,InvoiceNo,InvoiceDate,partialDisposalTotalValue,case when CreateFromHHT=0 then ''False'' else ''True'' End as CreateFromHHT,a. CreateBy as CreatedBy,a. ModifedBy,A.LastModifiedDateTime as ModifedDate,DisposalReferenceNo,
DisposedDateTime,DisposedRemarks,DisposalValue,a.CompanyCode,a. CompanyName, 
a.FirstLevelCategoryName,a.SecondLevelCategoryName,
a.FirstLevelLocationName,a.SecondLevelLocationName,
A.LocationHierarchy,A.CategoryHierarchy,A.Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,
A.Attribute6,A.Attribute7,A.Attribute8,A.Attribute9,A.Attribute10,A.Attribute11,A.Attribute12,
A.Attribute13,A.Attribute14,
A.Attribute15,A.Attribute16,a.UploadedImagePath as AssetImage, A.Latitude, A.Longitude,
A.Attribute17,A.Attribute18,Attribute19,Attribute20,Attribute21,Attribute22,Attribute23,
Attribute24,Attribute25,Attribute26,Attribute27,Attribute28,Attribute29,Attribute30,Attribute31,Attribute32,Attribute33,
Attribute34,Attribute35,Attribute36,Attribute37,Attribute38,Attribute39,Attribute40
from AssetNewView A 
left join AssetDepreciationTable DVT on a.AssetID=DVT.AssetID 

Left join @ParentChildCountTable Parent on A.assetID=Parent.ParentAssetID 
where A.StatusID not in (500,3) and 1=@LanguageID and A.companyID in (Select Value from Split('''+@companyID+''','','')) '
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
								   set @DynamicQuery=@DynamicQuery+' and (A.mappedCategoryID in (select CategoryID from userCategoryMappingTable where personID=@UserId and StatusID=1))'
								   End 
								   if upper(@UserLocationMapping)='TRUE' 
								   begin 
								   set @DynamicQuery=@DynamicQuery+' and (A.mappedLocationID in (select LocationID from UserLocationMappingTable where personID=@UserId and StatusID=1))'
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
go 


ALTER  Procedure [dbo].[rprc_DisposedAssetReport]
(

@LanguageID INT=1,
@UserId INT =1,
@CompanyID nvarchar(max)=1003,
@ClassificationID int =NULL,
@Query nvarchar(max)=null
)
as 
Begin
Declare @DynamicQuery nvarchar(max)
Declare @ParamDefinition AS NVarchar(max) = null
--Test Table 
--Declare @Sample Table (queries nvarchar(max))

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

----Get Depreciation Details 
--Set @DynamicQuery= 'Declare @DepreciationDetailsTable Table(AssetID int,currentValue decimal(18,2)) ' ;
--Set @DynamicQuery=@DynamicQuery+ ' insert into @DepreciationDetailsTable(AssetID,currentValue) Select AssetID,(select top 1 AssetValueAfterDepreciation From DepreciationLineItemTable De  
--          join DepreciationTable Dp on De.DepreciationID=Dp.DepreciationID and Dp.StatusID=1 where de.AssetID=A.AssetID order by  depreciationlineitemid desc )
--         From AssetTable A  where A.StatusID=4 ' ;
         
Set @DynamicQuery= 'Select A.Barcode,A.AssetCode,A.CategoryName,A.LocationName ,
        A. DepartmentName,
        A.SectionDescription as  SectionName,A.CustodianName,A.PurchasePrice , 
        A.RFIDTagCode,A.PurchaseDate ,A.WarrantyExpiryDate ,A.Model,A.ReferenceCode,A.SerialNo,A.DeliveryNote, A.AssetCondition, 
         A.SupplierName,A.CreatedDateTime,A.Make,A.Capacity,A.PONumber,A.ComissionDate,A.DisposalValue  ,a.DisposalReferenceNo,
        a.DisposedDateTime,A.DisposedRemarks,A.partialDisposalTotalValue,A.ProductName as AssetDescription,
      
		
		A.MappedAssetID,A. CompanyName ,C.DisposalTypeDescription
        From AssetnewView A (nolock) 
		Left join DisposalTypeTable b on a.DisposalTypeID=B.DisposalTypeID
		Left join DisposalTypeDescriptionTable C on B.DisposalTypeID=C.DisposalTypeID and C.LanguageID=1
       -- Left Join (select * from DepreciationValueTable())DVT on a.AssetID=DVT.AssetID       
        where A.StatusID in (4,250) and 1=@LanguageID and A.companyID in (Select Value from Split('''+@companyID+''','','')) '
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
          set @DynamicQuery=@DynamicQuery+' and (a.DepartmentID in (select departmentID from UserDepartmentMappingTable where PersonID = @UserID))'
           End 
            if upper(@UserCategoryMapping)='TRUE' 
           begin 
          set @DynamicQuery=@DynamicQuery+' and (A.mappedCategoryID in (select * from ChildCategoryMappingTable(@UserId)))'
           End 
             if upper(@UserLocationMapping)='TRUE' 
           begin 
          set @DynamicQuery=@DynamicQuery+' and (A.mappedlocationID in (select * from ChildLocationMappingTable(@UserId)))'
           End 
if (@Query is not null and @Query<>'' ) 
Begin
 set @DynamicQuery=@DynamicQuery+' and ' +@Query 
 End 
 SET @ParamDefinition = '@LanguageID int,@UserId int'

 exec sp_executesql @DynamicQuery,@ParamDefinition,@LanguageID,@UserId                     
End 
go 

if not exists(select RightgroupName from user_RightgroupTable where Rightgroupname='AssetCatalogue')
Begin
Insert into user_RightgroupTable(RightGroupName,RightGroupDesc)
values('AssetCatalogue','AssetCatalogue')
update user_RightgroupTable set parentRightGroupID=RightGroupID from user_RightgroupTable where Rightgroupname='AssetCatalogue'
End
go
If not exists(select RightName from user_Righttable where Rightname='SearchByPhoto')
Begin
Insert into user_Righttable(RightName,RightDescription,ValueType,DisplayRight,RightGroupID,IsActive,IsDeleted)
select 'SearchByPhoto','SearchByPhoto',1,1,RightGroupID,1,0
from user_RightgroupTable where Rightgroupname='AssetCatalogue'
end 
go 
If not exists(select RightName from user_Righttable where Rightname='SearchByVirtualBarcode')
Begin
Insert into user_Righttable(RightName,RightDescription,ValueType,DisplayRight,RightGroupID,IsActive,IsDeleted)
select 'SearchByVirtualBarcode','SearchByVirtualBarcode',1,1,RightGroupID,1,0
from user_RightgroupTable where Rightgroupname='AssetCatalogue'
end 
go 
if not exists(select menuname from User_MenuTable where MenuName='AssetCatalogue' and TargetObject='HandlerNull()')
Begin
Insert into User_MenuTable(MenuName,RightID,TargetObject,OrderNo,Image)
values('AssetCatalogue',NULL,'HandlerNull()',11,'css/images/MenuIcon/Master.png')
End 
go 

if not exists(select menuname from User_MenuTable where MenuName='SearchByPhoto' and TargetObject='/SearchByPhoto/Index')
Begin
Insert into User_MenuTable(MenuName,RightID,TargetObject,ParentMenuID,OrderNo,Image,ParentTransactionID)
select 'SearchByPhoto',RightID,'/SearchByPhoto/Index',(select menuID from user_menuTable where menuname='AssetCatalogue'),1,'css/images/MenuIcon/BarcodePrinting.png',1
from user_Righttable where rightname='SearchByPhoto'
End 
go 
if not exists(select menuname from User_MenuTable where MenuName='SearchByVirtualBarcode' and TargetObject='/SearchByVirtualBarcode/Index')
Begin
Insert into User_MenuTable(MenuName,RightID,TargetObject,ParentMenuID,OrderNo,Image,ParentTransactionID)
select 'SearchByVirtualBarcode',RightID,'/SearchByVirtualBarcode/Index',(select menuID from user_menuTable where menuname='AssetCatalogue'),2,'css/images/MenuIcon/BarcodePrinting.png',1
from user_Righttable where rightname='SearchByVirtualBarcode'
End 
go 

update User_MenuTable set Image='css/images/MenuIcon/catalogue.png' where MenuName='AssetCatalogue'
go 
update User_MenuTable set Image='css/images/MenuIcon/Searchbyphoto.png' where MenuName='SearchByPhoto'
go 
update User_MenuTable set Image='css/images/MenuIcon/Searchbyvirtualbarcode.png' where MenuName='SearchByVirtualBarcode'
go 


IF EXISTS(select * FROM sys.views where name = 'User_RoleRightView')
begin 
	drop view dbo.User_RoleRightView
end 
Go 
Create view [dbo].[User_RoleRightView]
as 
 
  Select a.*,b.RightName,b.RightGroupID
  From user_RightTable b left join
  User_RoleRightTable a  on a.RightID=b.RightID
  
GO


IF EXISTS(select * FROM sys.views where name = 'User_UserRightView')
begin 
	drop view dbo.User_UserRightView
end 
Go 
CREATE view [dbo].[User_UserRightView]
as 
 
  Select a.*,b.RightName,b.RightGroupID
  From user_RightTable b left join
  User_userRightTable a on a.RightID=b.RightID

GO

IF EXISTS(select * FROM sys.views where name = 'UserRightView')
begin 
	drop view dbo.UserRightView
end 
Go 
CREATE View [dbo].[UserRightView]
as 
Select a.*,b.RightGroupName
From user_RightTable a 
join user_RightGroupTable b on a.RightGroupID=b.RightGroupID
where a.IsActive=1
GO
IF EXISTS(select * FROM sys.views where name = 'PersonView')
begin 
	drop view dbo.PersonView
end 
Go 
CREATE View [dbo].[PersonView] 
as 
 Select A.[PersonID]
      ,A.[PersonCode]
      ,A.[PersonFirstName]
      ,A.[PersonLastName]
      ,A.[PersonFirstNameLanguageTwo]
      ,A.[PersonLastNameLanguageTwo]
      ,A.[EMailID]
      ,A.[DepartmentID]
      ,A.[DOJ]
      ,A.[Gender]
      ,A.[Culture]
      ,A.[UserTypeID]
      ,A.[MobileNo]
      ,A.[WhatsAppMobileNo]
      ,A.[StatusID]
	  ,B.Username as Username, C.Status, D.UserType
	From PersonTable a 
	join User_LoginUserTable b on b.userID=A.personID
	join StatusTable C on a.StatusID=C.StatusID
	Left Join DepartmentTable DP ON DP.DepartmentID = A.DepartmentID
	Left join UserTypeTable D on a.UserTypeID=D.UserTypeID
GO
IF EXISTS(select * FROM sys.views where name = 'SecondLevelLocationTable')
begin 
	drop view dbo.SecondLevelLocationTable
end 
Go 
Create View SecondLevelLocationTable 
as  
Select a.LocationCode,a.LocationName,b.LocationName AS ParentLocation,a.LocationID
   from LocationTable a 
   join LocationTable b on a.ParentLocationID=b.LocationID
   where b.ParentLocationID is null and a.StatusID!=3 and b.StatusID!=3
   go 

   
ALTER View [dbo].[SecondLevelLocationTable] 
as  
Select a.LocationCode,a.LocationName,b.LocationName AS ParentLocation,a.LocationID,b.LocationName+'/'+a.LocationName as SecondLevelLocationName
   from LocationTable a 
   join LocationTable b on a.ParentLocationID=b.LocationID
   where b.ParentLocationID is null and a.StatusID!=3 and b.StatusID!=3
GO


Create View [dbo].[CategoryHierarchicalView]
As

WITH Tree_CTE(CategoryID, CategoryLevel, CategoryIDHierarchy, L1, L2, L3, L4, L5, L6, StatusID, CategoryCodeHierarchy,CategoryNameHierarchy,
		Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6, Attribute7, Attribute8, Attribute9, Attribute10,
		Attribute11,Attribute12,Attribute13,Attribute14,Attribute15,Attribute16)
AS
(
    SELECT CategoryID, 1 CategoryLevel, CAST(CategoryID as nvarchar(max)) CategoryIDHierarchy,
		CAST(CategoryID AS INT) L1, CAST(NULL AS INT) L2, CAST(NULL AS INT) L3, CAST(NULL AS INT) L4, CAST(NULL AS INT) L5, CAST(NULL AS INT) L6,
		StatusID, CAST(CategoryCode as nvarchar(MAX)) CategoryCode,CAST(CategoryName as nvarchar(MAX)) CategoryName,
		Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6,
		Attribute7, Attribute8, Attribute9, Attribute10,
		Attribute11,Attribute12,Attribute13,Attribute14,Attribute15,Attribute16
		FROM CategoryTable WHERE ParentCategoryID IS NULL
    UNION ALL
    SELECT ChildNode.CategoryID, CategoryLevel+1, CategoryIDHierarchy + '/' + CAST(ChildNode.CategoryID as nvarchar(max)),
		Tree_CTE.L1,
		CASE WHEN CategoryLevel > 1 THEN Tree_CTE.L2 ELSE CASE WHEN CategoryLevel = 1 THEN ChildNode.CategoryID ELSE NULL END END,
		CASE WHEN CategoryLevel > 2 THEN Tree_CTE.L3 ELSE CASE WHEN CategoryLevel = 2 THEN ChildNode.CategoryID ELSE NULL END END,
		CASE WHEN CategoryLevel > 3 THEN Tree_CTE.L4 ELSE CASE WHEN CategoryLevel = 3 THEN ChildNode.CategoryID ELSE NULL END END,
		CASE WHEN CategoryLevel > 4 THEN Tree_CTE.L5 ELSE CASE WHEN CategoryLevel = 4 THEN ChildNode.CategoryID ELSE NULL END END,
		CASE WHEN CategoryLevel > 5 THEN Tree_CTE.L6 ELSE CASE WHEN CategoryLevel = 5 THEN ChildNode.CategoryID ELSE NULL END END,
		ChildNode.StatusID, CategoryCodeHierarchy + '/' + ChildNode.CategoryCode,CategoryNameHierarchy+'/'+ChildNode.categoryname,
		ISNULL(ChildNode.Attribute1, Tree_CTE.Attribute1) Attribute1,
		ISNULL(ChildNode.Attribute2, Tree_CTE.Attribute2) Attribute3,
		ISNULL(ChildNode.Attribute3, Tree_CTE.Attribute3) Attribute3,
		ISNULL(ChildNode.Attribute4, Tree_CTE.Attribute4) Attribute4,
		ISNULL(ChildNode.Attribute5, Tree_CTE.Attribute5) Attribute5,
		ISNULL(ChildNode.Attribute6, Tree_CTE.Attribute6) Attribute6,
		ISNULL(ChildNode.Attribute7, Tree_CTE.Attribute7) Attribute7,
		ISNULL(ChildNode.Attribute8, Tree_CTE.Attribute8) Attribute8,
		ISNULL(ChildNode.Attribute9, Tree_CTE.Attribute9) Attribute9,
		ISNULL(ChildNode.Attribute10, Tree_CTE.Attribute10) Attribute10,
		ISNULL(ChildNode.Attribute11, Tree_CTE.Attribute11) Attribute11,
		ISNULL(ChildNode.Attribute12, Tree_CTE.Attribute12) Attribute12,
		ISNULL(ChildNode.Attribute13, Tree_CTE.Attribute13) Attribute13,
		ISNULL(ChildNode.Attribute14, Tree_CTE.Attribute14) Attribute14,
		ISNULL(ChildNode.Attribute15, Tree_CTE.Attribute15) Attribute15,
		ISNULL(ChildNode.Attribute16, Tree_CTE.Attribute16) Attribute16

		FROM CategoryTable AS ChildNode
    INNER JOIN Tree_CTE
    ON ChildNode.ParentCategoryID = Tree_CTE.CategoryID
)
SELECT A.CategoryID ChildID, A.CategoryID,
			A.CategoryNameHierarchy,
			A.CategoryID ParentCategoryID, CONVERT(nvarchar,A.L1) FirstLevel, CONVERT(nvarchar,A.L2) SecondLevel, CONVERT(nvarchar,A.L3) ThirdLevel, A.CategoryLevel [Level],
			b.categoryname CategoryName,  A.CategoryCodeHierarchy ChildCode, A.StatusID,
			A.CategoryIDHierarchy PID2, A.CategoryIDHierarchy AllLevelIDs,
			A.L1 Level1ID, A.L2 Level2ID, A.L3 Level3ID, A.L4 Level4ID, A.L5 Level5ID, A.L6 Level6ID,
			b1.categoryname L1Desc, b2.categoryname L2Desc, b3.categoryname L3Desc, 
			b4.categoryname L4Desc, b5.categoryname L5Desc, b6.categoryname L6Desc,
			a.Attribute1,a.Attribute2,a.Attribute3,a.Attribute4,a.Attribute5,a.Attribute6,
			a.Attribute7, a.Attribute8, a.Attribute9, a.Attribute10,a.Attribute11,a.Attribute12,a.Attribute13,
			a.Attribute14,a.Attribute15,a.Attribute16
			FROM Tree_CTE A
			left join  Categorytable b on a.categoryid=b.categoryID
			left join  Categorytable b1 on a.L1=b1.categoryID
			left join  Categorytable b2 on a.l2=b2.categoryID
			left join  Categorytable b3 on a.l3=b3.categoryID
			left join  Categorytable b4 on a.L4=b4.categoryID
			left join  Categorytable b5 on a.L5=b5.categoryID
			left join  Categorytable b6 on a.L6=b6.categoryID
GO



create VIEW [dbo].[LocationHierarchicalView]
AS
WITH Tree_CTE(LocationID, LocationLevel, LocationIDHierarchy, L1, L2, L3, L4, L5, L6, StatusID, LocationCodeHierarchy, CompanyID,LocationNameHierarchy,
		Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6, Attribute7, Attribute8, Attribute9, Attribute10,
		Attribute11,Attribute12, Attribute13, Attribute14, Attribute15, Attribute16)
AS
(
    SELECT locationID, 1 LocationLevel, CAST(LocationID as nvarchar(max)) LocationIDHierarchy,
		CAST(LocationID AS INT) L1, CAST(NULL AS INT) L2, CAST(NULL AS INT) L3, CAST(NULL AS INT) L4, CAST(NULL AS INT) L5, CAST(NULL AS INT) L6,
		StatusID, CAST(LocationCode as nvarchar(MAX)) LocationCode, CompanyID,CAST(LocationName as nvarchar(MAX)) LocationName, 
		Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6, Attribute7, Attribute8, Attribute9, Attribute10,
		Attribute11,Attribute12, Attribute13, Attribute14, Attribute15, Attribute16
		FROM LocationTable WHERE ParentLocationID IS NULL
    UNION ALL
    SELECT ChildNode.LocationID, LocationLevel+1, LocationIDHierarchy + '/' + CAST(ChildNode.LocationID as nvarchar(max)),
		Tree_CTE.L1,
		CASE WHEN LocationLevel > 1 THEN Tree_CTE.L2 ELSE CASE WHEN LocationLevel = 1 THEN ChildNode.LocationID ELSE NULL END END,
		CASE WHEN LocationLevel > 2 THEN Tree_CTE.L3 ELSE CASE WHEN LocationLevel = 2 THEN ChildNode.LocationID ELSE NULL END END,
		CASE WHEN LocationLevel > 3 THEN Tree_CTE.L4 ELSE CASE WHEN LocationLevel = 3 THEN ChildNode.LocationID ELSE NULL END END,
		CASE WHEN LocationLevel > 4 THEN Tree_CTE.L5 ELSE CASE WHEN LocationLevel = 4 THEN ChildNode.LocationID ELSE NULL END END,
		CASE WHEN LocationLevel > 5 THEN Tree_CTE.L6 ELSE CASE WHEN LocationLevel = 5 THEN ChildNode.LocationID ELSE NULL END END,
		ChildNode.StatusID, LocationCodeHierarchy + '/' + ChildNode.LocationCode,ChildNode.CompanyID,LocationNameHierarchy + '/' + ChildNode.LocationName,
		ISNULL(ChildNode.Attribute1, Tree_CTE.Attribute1) Attribute1,
		ISNULL(ChildNode.Attribute2, Tree_CTE.Attribute2) Attribute3,
		ISNULL(ChildNode.Attribute3, Tree_CTE.Attribute3) Attribute3,
		ISNULL(ChildNode.Attribute4, Tree_CTE.Attribute4) Attribute4,
		ISNULL(ChildNode.Attribute5, Tree_CTE.Attribute5) Attribute5,
		ISNULL(ChildNode.Attribute6, Tree_CTE.Attribute6) Attribute6,
		ISNULL(ChildNode.Attribute7, Tree_CTE.Attribute7) Attribute7,
		ISNULL(ChildNode.Attribute8, Tree_CTE.Attribute8) Attribute8,
		ISNULL(ChildNode.Attribute9, Tree_CTE.Attribute9) Attribute9,
		ISNULL(ChildNode.Attribute10, Tree_CTE.Attribute10) Attribute10,
		ISNULL(ChildNode.Attribute11, Tree_CTE.Attribute11) Attribute11,
		ISNULL(ChildNode.Attribute12, Tree_CTE.Attribute12) Attribute12,
		ISNULL(ChildNode.Attribute13, Tree_CTE.Attribute13) Attribute13,
		ISNULL(ChildNode.Attribute14, Tree_CTE.Attribute14) Attribute14,
		ISNULL(ChildNode.Attribute15, Tree_CTE.Attribute15) Attribute15,
		ISNULL(ChildNode.Attribute16, Tree_CTE.Attribute16) Attribute16
		FROM LocationTable AS ChildNode
    INNER JOIN Tree_CTE
    ON ChildNode.ParentLocationID = Tree_CTE.LocationID
)
SELECT A.LocationID ChildID, 
			a.LocationNameHierarchy PID4,
			A.LocationID ParentLocationID,CONVERT(nvarchar,A.L1) FirstLevel, CONVERT(nvarchar,A.L2) SecondLevel,CONVERT(nvarchar, A.L3) ThirdLevel, A.LocationLevel [Level],
			LD.LocationName LocationName,  A.LocationCodeHierarchy ChildCode, A.StatusID,
			A.LocationIDHierarchy PID2, A.LocationIDHierarchy AllLevelIDs,
			A.L1 Level1ID, A.L2 Level2ID, A.L3 Level3ID, A.L4 Level4ID, A.L5 Level5ID, A.L6 Level6ID,
			L1D.LocationName L1Desc, L2D.LocationName L2Desc, L3D.LocationName L3Desc, 
			L4D.LocationName L4Desc, L5D.LocationName L5Desc, L6D.LocationName L6Desc,ld.CompanyID,
			a.Attribute1,a.Attribute2,a.Attribute3,a.Attribute4,a.Attribute5,a.Attribute6,
			a.Attribute7, a.Attribute8, a.Attribute9, a.Attribute10
			,a.Attribute11,a.Attribute12,
			a.Attribute13, a.Attribute14, a.Attribute15, a.Attribute16,LT.LocationType
			FROM Tree_CTE A
			
LEFT JOIN LocationTable LD ON A.LocationID = LD.LocationID  
LEFT JOIN LocationTable L1D ON A.L1 = L1D.LocationID 
LEFT JOIN LocationTable L2D ON A.L2 = L2D.LocationID 
LEFT JOIN LocationTable L3D ON A.L3 = L3D.LocationID 
LEFT JOIN LocationTable L4D ON A.L4 = L4D.LocationID 
LEFT JOIN LocationTable L5D ON A.L5 = L5D.LocationID
LEFT JOIN LocationTable L6D ON A.L6 = L6D.LocationID 
left join LocationTypeTable LT on L2D.LocationTypeID=LT.LocationTypeID

GO



create VIEW [dbo].[AssetView] 
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
	Attribute27,Attribute28,Attribute29,Attribute30,Attribute31,Attribute32,Attribute33,Attribute34,Attribute35,Attribute36,Attribute37,Attribute38,Attribute39,Attribute40,lv.LocationType
	--UsefulAssetLife, AssetAge
	
	 FROM AssetTable A 
	LEFT JOIN CategoryHierarchicalView AS c ON A.CategoryID = c.ChildId --AND C.LanguageID IN (1)
	LEFT JOIN ProductTable P ON P.ProductID=A.ProductID AND c.childid=p.categoryID
	LEFT JOIN CompanyTable CO ON CO.CompanyID=A.CompanyID 
	LEFT JOIN StatusTable ST ON A.StatusID=ST.StatusID	
	LEFT JOIN LocationHierarchicalView LV ON A.LocationID = LV.ChildId --AND LV.LanguageID IN (1)
	LEFT JOIN DepartmentTable D ON D.DepartmentID=A.DepartmentID 
	LEFT JOIN SectionTable S ON S.SectionID=A.SectionID  
	LEFT JOIN PersonTable Cu ON A.CustodianID=Cu.PersonID 
	LEFT JOIN PersonTable PE ON A.CreatedBy=PE.PersonID 
	LEFT JOIN PersonTable M ON A.LastModifiedBy=M.PersonID 
	LEFT JOIN AssetConditionTable AC ON a.AssetConditionID=AC.AssetConditionID
	LEFT JOIN PartyTable SU ON SU.PartyID = A.SupplierID  and partyTypeID=2
	left join ModelTable MDT on A.ModelID=mdt.ModelID
	

	
GO





ALTER View [dbo].[SecondLevelLocationTable] 
as  
Select a.LocationCode,a.LocationName,b.LocationName AS ParentLocation,a.LocationID,b.LocationName+'/'+a.LocationName as SecondLevelLocationName,l.LocationType
   from LocationTable a 
   join LocationTable b on a.ParentLocationID=b.LocationID
   left join LocationTypeTable l ON A.LocationTypeID=L.LocationTypeID
   where b.ParentLocationID is null and a.StatusID!=3 and b.StatusID!=3
GO

  create view AssetTransferApprovalView 
  as 
   
   Select ApprovalHistoryID,ApproveWorkFlowID,ApproveWorkFlowLineItemID,
		ApproveModuleID,ApprovalRoleID,a.TransactionID as ApprovalTransactionID,
		OrderNo,a.Remarks as ApprovalRemarks,FromLocationID,ToLocationID,
		FromLocationTypeID,ToLocationTypeID,a.StatusID as ApprovalStatusID,
        a.CreatedBy as ApprovalCreatedBy,a.CreatedDateTime as ApprovalCreatedDateTime,
		LastModifiedBy,LastModifiedDateTime,ObjectKeyID1,EmailsecrectKey ,b.*, ROW_NUMBER() over( 
         PARTITION BY a.transactionid , a.approvemoduleID
         ORDER BY Orderno asc 
     ) AS SerialNo ,p.PersonFirstName+'-'+p.PersonLastName as CreatedUSer,s.Status as ApprovalStatus
	From ApprovalHistoryTable a join TransactionTable b on a.TransactionID=b.TransactionID 
	join PersonTable p on b.CreatedBy=p.PersonID
	join StatusTable s on a.StatusID=s.StatusID
	where a.ApproveModuleID=5 and a.StatusID=101
	Go 

	  Create View ApprovalHistoryView 
  as 
  Select b.ApprovalRoleName,c.Status as ApprovalStatus,p.PersonFirstName+'-'+p.PersonLastName as ApprovedBy ,a.LastModifiedDateTime as ApprovedDatetime ,a.*
  From ApprovalHistoryTable a 
  join ApprovalRoleTable b on a.ApprovalRoleID=b.ApprovalRoleID
  left join StatusTable c on a.StatusID=c.StatusID
  left join PersonTable p on a.LastModifiedBy=p.PersonID
  Go 
  Create View TransactionLineItemViewForTransfer
  as 
  Select a1.*,old.LocationName as OldLocationName,New.LocationName as NewLocationName,
  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubType,
ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,Remarks,
TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
OldDept.DepartmentName as OldDepartmentName,NewDept.DepartmentName as NewDepartmentName,OldCate.CategoryName as OldCategoryName,
Oldprod.ProductName as OldProductName,newprod.ProductName as NewProductName,OldSec.SectionName as oldSectionName,newsec.SectionName as NewSectionName,

newCate.CategoryName as NewCategoryName
  From TransactionTable a 
  join TransactionLineItemTable b on a.TransactionID=b.TransactionID 
  join AssetView a1 on b.AssetID=a1.AssetID 
  left join LocationTable Old on b.FromLocationID=Old.LocationID
  left join LocationTable new on b.ToLocationID=new.LocationID
  Left join DepartmentTable OldDept on b.FromDepartmentID=OldDept.DepartmentID
  left join DepartmentTable NewDept on b.ToDepartmentID=NewDept.DepartmentID
  Left join CategoryTable OldCate on b.FromCategoryID=OldCate.CategoryID
  left join CategoryTable newCate on b.ToCategoryID=newCate.CategoryID
   Left join ProductTable Oldprod on b.FromProductID=Oldprod.ProductID
  left join ProductTable newprod on b.ToProductID=newprod.ProductID
  LEft join SectionTable OldSec on b.FromSectionID=oldsec.SectionID
  left join SectionTable newsec on b.ToSectionID=newsec.SectionID
  
  go 

  alter View TransactionLineItemViewForTransfer
  as 
  Select a1.*,old.LocationName as OldLocationName,New.LocationName as NewLocationName,
  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubType,
ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,Remarks,
TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
OldDept.DepartmentName as OldDepartmentName,NewDept.DepartmentName as NewDepartmentName,OldCate.CategoryName as OldCategoryName,
Oldprod.ProductName as OldProductName,newprod.ProductName as NewProductName,OldSec.SectionName as oldSectionName,newsec.SectionName as NewSectionName,

newCate.CategoryName as NewCategoryName
  From TransactionTable a 
  join TransactionLineItemTable b on a.TransactionID=b.TransactionID 
  join AssetView a1 on b.AssetID=a1.AssetID 
  left join LocationTable Old on b.FromLocationID=Old.LocationID
  left join LocationTable new on b.ToLocationID=new.LocationID
  Left join DepartmentTable OldDept on b.FromDepartmentID=OldDept.DepartmentID
  left join DepartmentTable NewDept on b.ToDepartmentID=NewDept.DepartmentID
  Left join CategoryTable OldCate on b.FromCategoryID=OldCate.CategoryID
  left join CategoryTable newCate on b.ToCategoryID=newCate.CategoryID
   Left join ProductTable Oldprod on b.FromProductID=Oldprod.ProductID
  left join ProductTable newprod on b.ToProductID=newprod.ProductID
  LEft join SectionTable OldSec on b.FromSectionID=oldsec.SectionID
  left join SectionTable newsec on b.ToSectionID=newsec.SectionID

  go 

  Create View ApprovalHistoryView 
  as 
  Select b.ApprovalRoleName,c.Status as ApprovalStatus,p.PersonFirstName+'-'+p.PersonLastName as ApprovedBy ,a.LastModifiedDateTime as ApprovedDatetime ,a.*
  From ApprovalHistoryTable a 
  join ApprovalRoleTable b on a.ApprovalRoleID=b.ApprovalRoleID
  left join StatusTable c on a.StatusID=c.StatusID
  left join PersonTable p on a.LastModifiedBy=p.PersonID
  go 
  Create view TransactionView 
as 

SElect a.*,b.TransactionTypeName,c.Status,p.PersonFirstName+'-'+p.PersonLastName as CreatedUSer
From TransactionTable a 
join TransactionTypeTable b on a.TransactionTypeID=b.TransactionTypeID 
join  StatusTable c on a.StatusID=c.StatusID
join PersonTable p on a.CreatedBy=p.PersonID

go 


CREATE view [dbo].[AssetRetirementApprovalView] 
  as 
   
   Select ApprovalHistoryID,ApproveWorkFlowID,ApproveWorkFlowLineItemID,
		ApproveModuleID,ApprovalRoleID,a.TransactionID as ApprovalTransactionID,
		OrderNo,a.Remarks as ApprovalRemarks,FromLocationID,ToLocationID,
		FromLocationTypeID,ToLocationTypeID,a.StatusID as ApprovalStatusID,
        a.CreatedBy as ApprovalCreatedBy,a.CreatedDateTime as ApprovalCreatedDateTime,
		LastModifiedBy,LastModifiedDateTime,ObjectKeyID1,EmailsecrectKey ,b.*, ROW_NUMBER() over( 
         PARTITION BY a.transactionid , a.approvemoduleID
         ORDER BY Orderno asc 
     ) AS SerialNo ,p.PersonFirstName+'-'+p.PersonLastName as CreatedUSer,s.Status as ApprovalStatus
	From ApprovalHistoryTable a join TransactionTable b on a.TransactionID=b.TransactionID 
	join PersonTable p on b.CreatedBy=p.PersonID
	join StatusTable s on a.StatusID=s.StatusID
	where a.ApproveModuleID=10 and a.StatusID=101
GO
ALTER view [dbo].[AssetRetirementApprovalView] 
  as 

   Select a.ApprovalHistoryID,ApproveWorkFlowID,ApproveWorkFlowLineItemID,
		a.ApproveModuleID,ApprovalRoleID,a.TransactionID as ApprovalTransactionID,
		a.OrderNo,a.Remarks as ApprovalRemarks,FromLocationID,ToLocationID,
		FromLocationTypeID,ToLocationTypeID,a.StatusID as ApprovalStatusID,
        a.CreatedBy as ApprovalCreatedBy,a.CreatedDateTime as ApprovalCreatedDateTime,
		LastModifiedBy,LastModifiedDateTime,ObjectKeyID1,EmailsecrectKey ,b.*, ROW_NUMBER() over( 
         PARTITION BY a.transactionid , a.approvemoduleID
         ORDER BY a.Orderno asc 
     ) AS SerialNo ,p.PersonFirstName+'-'+p.PersonLastName as CreatedUSer,s.Status as ApprovalStatus
	From ApprovalHistoryTable a join TransactionTable b on a.TransactionID=b.TransactionID 
	join (select TransactionID,ApproveModuleID,max(Orderno) as MaxOrderNo from ApprovalHistoryTable  where ApproveModuleID=10 and StatusID=101 
	group by TransactionID,ApproveModuleID )c on a.TransactionID=c.TransactionID
	join PersonTable p on b.CreatedBy=p.PersonID
	join StatusTable s on a.StatusID=s.StatusID
	where a.ApproveModuleID=10 and a.StatusID=101 and a.OrderNo<c.MaxOrderNo
	
	GO

	Create View FinalLevelRetirementView
	 as 
	 Select a.*,b.TransactionNo 
	 From ApprovalhistoryTable a 
	 join TransactionTable b on a.transactionid=b.transactionid
	 where a.approvemoduleID=10  and  b.transactiontypeID=10 and b.postingStatusID=11 and a.statusID=101
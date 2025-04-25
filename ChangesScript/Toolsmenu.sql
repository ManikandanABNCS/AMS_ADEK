

CREATE TABLE [dbo].[TransactionSubTypeTable](
	[TransactionSubTypeID] [int] NOT NULL primary key,
	[TransactionSubTypeCode] [nvarchar](100) NOT NULL,
	[TransactionSubTypeName] [nvarchar](500) NOT NULL
	)
	go 
	
	EXEC sp_rename 'TransactionTable.TransactionSubType', 'TransactionSubTypeID', 'COLUMN';
	GO 
	ALTER TABLE TransactionTable ALTER COLUMN TransactionSubTypeID INT NULL
	GO 
	ALTER TABLE TransactionTable ADD CONSTRAINT Fk_TransactionTable_TransactionSubTypeID FOREIGN KEY ( TransactionSubTypeID) REFERENCES TransactionSubTypeTable(TransactionSubTypeID)
	GO


ALTER VIEW [dbo].[LocationNewHierarchicalView]
AS
WITH Tree_CTE(LocationID, LocationLevel, LocationIDHierarchy, L1, L2, L3, L4, L5, L6, StatusID, LocationCodeHierarchy,LocationNameHierarchy)
AS
(
    SELECT LocationID,1 LocationLevel, CAST(LocationID as nvarchar(max)) LocationIDHierarchy,
		CAST(LocationID AS INT) L1, CAST(NULL AS INT) L2, CAST(NULL AS INT) L3, CAST(NULL AS INT) L4, CAST(NULL AS INT) L5, CAST(NULL AS INT) L6,
		StatusID, CAST(LocationCode as nvarchar(MAX)) LocationCode,CAST(LocationName as nvarchar(MAX)) LocationName
		FROM LocationTable WHERE ParentLocationID IS NULL 
    UNION ALL
    SELECT ChildNode.LocationID, LocationLevel+1, 
		LocationIDHierarchy + '/' + CAST(ChildNode.LocationID as nvarchar(max)),
		Tree_CTE.L1,
		CASE WHEN LocationLevel > 1 THEN Tree_CTE.L2 ELSE CASE WHEN LocationLevel = 1 THEN ChildNode.LocationID ELSE NULL END END,
		CASE WHEN LocationLevel > 2 THEN Tree_CTE.L3 ELSE CASE WHEN LocationLevel = 2 THEN ChildNode.LocationID ELSE NULL END END,
		CASE WHEN LocationLevel > 3 THEN Tree_CTE.L4 ELSE CASE WHEN LocationLevel = 3 THEN ChildNode.LocationID ELSE NULL END END,
		CASE WHEN LocationLevel > 4 THEN Tree_CTE.L5 ELSE CASE WHEN LocationLevel = 4 THEN ChildNode.LocationID ELSE NULL END END,
		CASE WHEN LocationLevel > 5 THEN Tree_CTE.L6 ELSE CASE WHEN LocationLevel = 5 THEN ChildNode.LocationID ELSE NULL END END,
		ChildNode.StatusID, LocationCodeHierarchy + '/' + ChildNode.LocationCode,LocationNameHierarchy + '/' + ChildNode.LocationName
		
		FROM LocationTable AS ChildNode
    INNER JOIN Tree_CTE
    ON ChildNode.ParentLocationID = Tree_CTE.LocationID 
)
SELECT A.LocationID , 
		LD.LocationCode, A.LocationCodeHierarchy,
		LD.LocationName, A.LocationNameHierarchy ,
			LD.ParentLocationID, A.LocationLevel [Level], A.StatusID,
			A.LocationIDHierarchy AllLevelIDs,
			A.L1 Level1ID, A.L2 Level2ID, A.L3 Level3ID, A.L4 Level4ID, A.L5 Level5ID, A.L6 Level6ID,
			L1D.LocationName L1LocationName, L2D.LocationName L2LocationName, L3D.LocationName L3LocationName, 
			L4D.LocationName L4LocationName, L5D.LocationName L5LocationName, L6D.LocationName L6LocationName,LD.CompanyID,
			ISNULL(LD.Attribute1, L2D.Attribute1) Attribute1,
			ISNULL(LD.Attribute2, L2D.Attribute2) Attribute2,
			ISNULL(LD.Attribute3, L2D.Attribute3) Attribute3,
			ISNULL(LD.Attribute4, L2D.Attribute4) Attribute4,
			ISNULL(LD.Attribute5, L2D.Attribute5) Attribute5,
			ISNULL(LD.Attribute6, L2D.Attribute6) Attribute6,
			ISNULL(LD.Attribute7, L2D.Attribute7) Attribute7,
			ISNULL(LD.Attribute8, L2D.Attribute8) Attribute8,
			ISNULL(LD.Attribute9, L2D.Attribute9) Attribute9,
			ISNULL(LD.Attribute10, L2D.Attribute10) Attribute10,
			ISNULL(LD.Attribute11, L2D.Attribute11) Attribute11,
			ISNULL(LD.Attribute12, L2D.Attribute12) Attribute12,
			ISNULL(LD.Attribute13, L2D.Attribute13) Attribute13,
			ISNULL(LD.Attribute14, L2D.Attribute14) Attribute14,
			ISNULL(LD.Attribute15, L2D.Attribute15) Attribute15,
			ISNULL(LD.Attribute16, L2D.Attribute16) Attribute16,
			LT.LocationTypeName as LocationType,LT.LocationTypeID as LocationTypeID,
			L2D.LocationID [MappedLocationID],L2D.LocationCode as MappedLocationCode,L2D.LocationName as MappedLocationName
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


drop table tmp_LocationNewHierarchicalView
go
select * into tmp_LocationNewHierarchicalView from LocationNewHierarchicalView
go 


ALTER View [dbo].[CategoryNewHierarchicalView]
As

WITH Tree_CTE(CategoryID, CategoryLevel, CategoryIDHierarchy, L1, L2, L3, L4, L5, L6, StatusID, CategoryCodeHierarchy,CategoryNameHierarchy
		)
AS
(
    SELECT CategoryID, 1 CategoryLevel, CAST(CategoryID as nvarchar(max)) CategoryIDHierarchy,
		CAST(CategoryID AS INT) L1, CAST(NULL AS INT) L2, CAST(NULL AS INT) L3, CAST(NULL AS INT) L4, CAST(NULL AS INT) L5, CAST(NULL AS INT) L6,
		StatusID, CAST(CategoryCode as nvarchar(MAX)) CategoryCode,CAST(CategoryName as nvarchar(MAX)) CategoryName		
		FROM CategoryTable WHERE ParentCategoryID IS NULL
    UNION ALL
    SELECT ChildNode.CategoryID, CategoryLevel+1, CategoryIDHierarchy + '/' + CAST(ChildNode.CategoryID as nvarchar(max)),
		Tree_CTE.L1,
		CASE WHEN CategoryLevel > 1 THEN Tree_CTE.L2 ELSE CASE WHEN CategoryLevel = 1 THEN ChildNode.CategoryID ELSE NULL END END,
		CASE WHEN CategoryLevel > 2 THEN Tree_CTE.L3 ELSE CASE WHEN CategoryLevel = 2 THEN ChildNode.CategoryID ELSE NULL END END,
		CASE WHEN CategoryLevel > 3 THEN Tree_CTE.L4 ELSE CASE WHEN CategoryLevel = 3 THEN ChildNode.CategoryID ELSE NULL END END,
		CASE WHEN CategoryLevel > 4 THEN Tree_CTE.L5 ELSE CASE WHEN CategoryLevel = 4 THEN ChildNode.CategoryID ELSE NULL END END,
		CASE WHEN CategoryLevel > 5 THEN Tree_CTE.L6 ELSE CASE WHEN CategoryLevel = 5 THEN ChildNode.CategoryID ELSE NULL END END,
		ChildNode.StatusID, CategoryCodeHierarchy + '/' + ChildNode.CategoryCode,CategoryNameHierarchy+'/'+ChildNode.categoryname
		

		FROM CategoryTable AS ChildNode
    INNER JOIN Tree_CTE
    ON ChildNode.ParentCategoryID = Tree_CTE.CategoryID
)
SELECT A.CategoryID,b.CategoryCode,A.CategoryCodeHierarchy,
			b.CategoryName,A.CategoryNameHierarchy,
			b.ParentCategoryID,  A.CategoryLevel [Level],
			A.StatusID, A.CategoryIDHierarchy AllLevelIDs,
			A.L1 Level1ID, A.L2 Level2ID, A.L3 Level3ID, A.L4 Level4ID, A.L5 Level5ID, A.L6 Level6ID,
			b1.categoryname L1CategoryName, b2.categoryname L2CategoryName, b3.categoryname L3CategoryName, 
			b4.categoryname L4CategoryName, b5.categoryname L5CategoryName, b6.categoryname L6CategoryName,
			ISNULL(b.Attribute1, b2.Attribute1) Attribute1,
		ISNULL(b.Attribute2, b2.Attribute2) Attribute2,
		ISNULL(b.Attribute3, b2.Attribute3) Attribute3,
		ISNULL(b.Attribute4, b2.Attribute4) Attribute4,
		ISNULL(b.Attribute5, b2.Attribute5) Attribute5,
		ISNULL(b.Attribute6, b2.Attribute6) Attribute6,
		ISNULL(b.Attribute7, b2.Attribute7) Attribute7,
		ISNULL(b.Attribute8, b2.Attribute8) Attribute8,
		ISNULL(b.Attribute9, b2.Attribute9) Attribute9,
		ISNULL(b.Attribute10, b2.Attribute10) Attribute10,
		ISNULL(b.Attribute11, b2.Attribute11) Attribute11,
		ISNULL(b.Attribute12, b2.Attribute12) Attribute12,
		ISNULL(b.Attribute13, b2.Attribute13) Attribute13,
		ISNULL(b.Attribute14, b2.Attribute14) Attribute14,
		ISNULL(b.Attribute15, b2.Attribute15) Attribute15,
		ISNULL(b.Attribute16, b2.Attribute16) Attribute16,
			
			CT.CategoryTypeName as CategoryType,CT.CategoryTypeID,
			b2.CategoryID [MappedCategoryID],b2.CategoryCode as MappedCategoryCode,b2.CategoryName as MappedCategoryName
			FROM Tree_CTE A
			left join  Categorytable b on a.categoryid=b.categoryID
			left join  Categorytable b1 on a.L1=b1.categoryID
			left join  Categorytable b2 on a.l2=b2.categoryID
			left join  Categorytable b3 on a.l3=b3.categoryID
			left join  Categorytable b4 on a.L4=b4.categoryID
			left join  Categorytable b5 on a.L5=b5.categoryID
			left join  Categorytable b6 on a.L6=b6.categoryID
			left join CategoryTypeTable CT on b2.categoryTypeID=CT.categoryTypeID

GO


drop table tmp_CategoryNewHierarchicalView
go
select * into tmp_CategoryNewHierarchicalView from CategoryNewHierarchicalView
go
ALTER View [dbo].[CategoryNewView]
as 
	Select CategoryID,CategoryCode,CategoryCodeHierarchy,CategoryName,CategoryNameHierarchy,ParentCategoryID,Level,StatusID,AllLevelIDs,
	Level1ID,Level2ID,Level3ID,Level4ID,Level5ID,Level6ID,L1CategoryName,L2CategoryName,L3CategoryName,L4CategoryName,
	L5CategoryName,L6CategoryName,Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6,Attribute7,Attribute8,Attribute9,
	Attribute10,Attribute11,Attribute12,Attribute13,Attribute14,Attribute15,Attribute16,CategoryType,CategoryTypeID,
	MappedCategoryID,MappedCategoryCode,MappedCategoryName 
	from tmp_CategoryNewHierarchicalView
GO
		
ALTER View [dbo].[LocationNewView]
as
	select LocationID,LocationCode,LocationCodeHierarchy,LocationName,LocationNameHierarchy,ParentLocationID,Level,
	StatusID,AllLevelIDs,Level1ID,Level2ID,Level3ID,Level4ID,Level5ID,Level6ID,L1LocationName,L2LocationName,L3LocationName,L4LocationName,
	L5LocationName,L6LocationName,CompanyID,Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6,Attribute7,
	Attribute8,Attribute9,Attribute10,Attribute11,Attribute12,Attribute13,Attribute14,Attribute15,
	Attribute16,LocationType,LocationTypeID,MappedLocationID,MappedLocationCode,MappedLocationName 
	from tmp_LocationNewHierarchicalView
GO



if not exists(select Rightname from User_RightTable where RightName='EntityCode')
Begin
Insert into User_RightTable(RightName,RightDescription,ValueType,DisplayRight,RightGroupID,IsActive,IsDeleted)
select 'EntityCode','CodeConfiguration',95,1,RightGroupID,1,0
from User_RightGroupTable where rightgroupid=(select RightgroupID from User_RightGroupTable where RightGroupName='Tools')
end 
go 
if not exists(select Rightname from User_RightTable where RightName='MasterGridLineItem')
Begin
Insert into User_RightTable(RightName,RightDescription,ValueType,DisplayRight,RightGroupID,IsActive,IsDeleted)
select 'MasterGridLineItem','MasterGridLineItem',95,1,RightGroupID,1,0
from User_RightGroupTable where rightgroupid=(select RightgroupID from User_RightGroupTable where RightGroupName='Tools')
end 
go 
if not exists(select Rightname from User_RightTable where RightName='ASelectionControlQuery')
Begin
Insert into User_RightTable(RightName,RightDescription,ValueType,DisplayRight,RightGroupID,IsActive,IsDeleted)
select 'ASelectionControlQuery','SelectionControlQuery',95,1,RightGroupID,1,0
from User_RightGroupTable where rightgroupid=(select RightgroupID from User_RightGroupTable where RightGroupName='Tools')
end 
go 
if not exists(select Rightname from User_RightTable where RightName='FieldType')
Begin
Insert into User_RightTable(RightName,RightDescription,ValueType,DisplayRight,RightGroupID,IsActive,IsDeleted)
select 'FieldType','FieldType',95,1,RightGroupID,1,0
from User_RightGroupTable where rightgroupid=(select RightgroupID from User_RightGroupTable where RightGroupName='Tools')
end 
go

if not exists(select menuname from User_MenuTable where MenuName='FieldType' and ParentTransactionID=1)
Begin 
 insert into User_MenuTable(MenuName,RightID,TargetObject,ParentMenuID,OrderNo,image,ParentTransactionID)
 select 'FieldType',RightID,'/MasterPage/Index?pageName=FieldType',(select menuid from User_MenuTable  where MenuName='Tools'),10,'css/images/MenuIcon/UserRights.png',1
 from User_RightTable where RightName='FieldType'
end 
go 
if not exists(select menuname from User_MenuTable where MenuName='SelectionControlQuery' and ParentTransactionID=1)
Begin 
 insert into User_MenuTable(MenuName,RightID,TargetObject,ParentMenuID,OrderNo,image,ParentTransactionID)
 select 'SelectionControlQuery',RightID,'/MasterPage/Index?pageName=ASelectionControlQuery',(select menuid from User_MenuTable  where MenuName='Tools'),10,'css/images/MenuIcon/UserRights.png',1
 from User_RightTable where RightName='ASelectionControlQuery'
end 
go 
if not exists(select menuname from User_MenuTable where MenuName='ColumnConfigurations' and ParentTransactionID=1)
Begin 
 insert into User_MenuTable(MenuName,RightID,TargetObject,ParentMenuID,OrderNo,image,ParentTransactionID)
 select 'ColumnConfigurations',RightID,'/MasterPage/IndexPageInLineEdit?pageName=MasterGridLineItem',(select menuid from User_MenuTable  where MenuName='Tools'),10,'css/images/MenuIcon/UserRights.png',1
 from User_RightTable where RightName='MasterGridLineItem'
end 
go 
if not exists(select menuname from User_MenuTable where MenuName='CodeConfiguration' and ParentTransactionID=1)
Begin 
 insert into User_MenuTable(MenuName,RightID,TargetObject,ParentMenuID,OrderNo,image,ParentTransactionID)
 select 'CodeConfiguration',RightID,'/MasterPage/Index?pageName=EntityCode',(select menuid from User_MenuTable  where MenuName='Tools'),10,'css/images/MenuIcon/UserRights.png',1
 from User_RightTable where RightName='EntityCode'
end 
go 
if not exists(select Rightname from User_RightTable where RightName='BarcodeAutoSequence')
Begin
Insert into User_RightTable(RightName,RightDescription,ValueType,DisplayRight,RightGroupID,IsActive,IsDeleted)
select 'BarcodeAutoSequence','BarcodeAutoSequence',95,1,RightGroupID,1,0
from User_RightGroupTable where rightgroupid=(select RightgroupID from User_RightGroupTable where RightGroupName='Tools')
end 
go 

if not exists(select menuname from User_MenuTable where MenuName='BarcodeAutoSequence' and ParentTransactionID=1)
Begin 
 insert into User_MenuTable(MenuName,RightID,TargetObject,ParentMenuID,OrderNo,image,ParentTransactionID)
 select 'BarcodeAutoSequence',RightID,'/MasterPage/Index?pageName=BarcodeAutoSequence',(select menuid from User_MenuTable  where MenuName='Tools'),10,'css/images/MenuIcon/UserRights.png',1
 from User_RightTable where RightName='BarcodeAutoSequence'
end 
go



ALTER View [dbo].[CategoryNewHierarchicalView]
As

WITH Tree_CTE(CategoryID, CategoryLevel, CategoryIDHierarchy, L1, L2, L3, L4, L5, L6, StatusID, CategoryCodeHierarchy,CategoryNameHierarchy
		)
AS
(
    SELECT CategoryID, 1 CategoryLevel, CAST(CategoryID as nvarchar(max)) CategoryIDHierarchy,
		CAST(CategoryID AS INT) L1, CAST(NULL AS INT) L2, CAST(NULL AS INT) L3, CAST(NULL AS INT) L4, CAST(NULL AS INT) L5, CAST(NULL AS INT) L6,
		StatusID, CAST(CategoryCode as nvarchar(MAX)) CategoryCode,CAST(CategoryName as nvarchar(MAX)) CategoryName		
		FROM CategoryTable WHERE ParentCategoryID IS NULL
    UNION ALL
    SELECT ChildNode.CategoryID, CategoryLevel+1, CategoryIDHierarchy + '/' + CAST(ChildNode.CategoryID as nvarchar(max)),
		Tree_CTE.L1,
		CASE WHEN CategoryLevel > 1 THEN Tree_CTE.L2 ELSE CASE WHEN CategoryLevel = 1 THEN ChildNode.CategoryID ELSE NULL END END,
		CASE WHEN CategoryLevel > 2 THEN Tree_CTE.L3 ELSE CASE WHEN CategoryLevel = 2 THEN ChildNode.CategoryID ELSE NULL END END,
		CASE WHEN CategoryLevel > 3 THEN Tree_CTE.L4 ELSE CASE WHEN CategoryLevel = 3 THEN ChildNode.CategoryID ELSE NULL END END,
		CASE WHEN CategoryLevel > 4 THEN Tree_CTE.L5 ELSE CASE WHEN CategoryLevel = 4 THEN ChildNode.CategoryID ELSE NULL END END,
		CASE WHEN CategoryLevel > 5 THEN Tree_CTE.L6 ELSE CASE WHEN CategoryLevel = 5 THEN ChildNode.CategoryID ELSE NULL END END,
		ChildNode.StatusID, CategoryCodeHierarchy + '/' + ChildNode.CategoryCode,CategoryNameHierarchy+'/'+ChildNode.categoryname
		

		FROM CategoryTable AS ChildNode
    INNER JOIN Tree_CTE
    ON ChildNode.ParentCategoryID = Tree_CTE.CategoryID
)
SELECT A.CategoryID,b.CategoryCode,A.CategoryCodeHierarchy,
			b.CategoryName,A.CategoryNameHierarchy,
			b.ParentCategoryID,  A.CategoryLevel [Level],
			A.StatusID, A.CategoryIDHierarchy AllLevelIDs,
			A.L1 Level1ID, A.L2 Level2ID, A.L3 Level3ID, A.L4 Level4ID, A.L5 Level5ID, A.L6 Level6ID,
			b1.categoryname L1CategoryName, b2.categoryname L2CategoryName, b3.categoryname L3CategoryName, 
			b4.categoryname L4CategoryName, b5.categoryname L5CategoryName, b6.categoryname L6CategoryName,
			ISNULL(b.Attribute1, b2.Attribute1) Attribute1,
		ISNULL(b.Attribute2, b2.Attribute2) Attribute2,
		ISNULL(b.Attribute3, b2.Attribute3) Attribute3,
		ISNULL(b.Attribute4, b2.Attribute4) Attribute4,
		ISNULL(b.Attribute5, b2.Attribute5) Attribute5,
		ISNULL(b.Attribute6, b2.Attribute6) Attribute6,
		ISNULL(b.Attribute7, b2.Attribute7) Attribute7,
		ISNULL(b.Attribute8, b2.Attribute8) Attribute8,
		ISNULL(b.Attribute9, b2.Attribute9) Attribute9,
		ISNULL(b.Attribute10, b2.Attribute10) Attribute10,
		ISNULL(b.Attribute11, b2.Attribute11) Attribute11,
		ISNULL(b.Attribute12, b2.Attribute12) Attribute12,
		ISNULL(b.Attribute13, b2.Attribute13) Attribute13,
		ISNULL(b.Attribute14, b2.Attribute14) Attribute14,
		ISNULL(b.Attribute15, b2.Attribute15) Attribute15,
		ISNULL(b.Attribute16, b2.Attribute16) Attribute16,
			
			CT.CategoryTypeName as CategoryType,CT.CategoryTypeID,
			b2.CategoryID [MappedCategoryID],b2.CategoryCode as MappedCategoryCode,b2.CategoryName as MappedCategoryName
			FROM Tree_CTE A
			left join  Categorytable b on a.categoryid=b.categoryID
			left join  Categorytable b1 on a.L1=b1.categoryID
			left join  Categorytable b2 on a.l2=b2.categoryID
			left join  Categorytable b3 on a.l3=b3.categoryID
			left join  Categorytable b4 on a.L4=b4.categoryID
			left join  Categorytable b5 on a.L5=b5.categoryID
			left join  Categorytable b6 on a.L6=b6.categoryID
			left join CategoryTypeTable CT on b2.categoryTypeID=CT.categoryTypeID

GO


drop table tmp_CategoryNewHierarchicalView
go
select *  into  tmp_CategoryNewHierarchicalView from CategoryNewHierarchicalView
go 


ALTER VIEW [dbo].[LocationNewHierarchicalView]
AS
WITH Tree_CTE(LocationID, LocationLevel, LocationIDHierarchy, L1, L2, L3, L4, L5, L6, StatusID, LocationCodeHierarchy,LocationNameHierarchy)
AS
(
    SELECT LocationID,1 LocationLevel, CAST(LocationID as nvarchar(max)) LocationIDHierarchy,
		CAST(LocationID AS INT) L1, CAST(NULL AS INT) L2, CAST(NULL AS INT) L3, CAST(NULL AS INT) L4, CAST(NULL AS INT) L5, CAST(NULL AS INT) L6,
		StatusID, CAST(LocationCode as nvarchar(MAX)) LocationCode,CAST(LocationName as nvarchar(MAX)) LocationName
		FROM LocationTable WHERE ParentLocationID IS NULL 
    UNION ALL
    SELECT ChildNode.LocationID, LocationLevel+1, 
		LocationIDHierarchy + '/' + CAST(ChildNode.LocationID as nvarchar(max)),
		Tree_CTE.L1,
		CASE WHEN LocationLevel > 1 THEN Tree_CTE.L2 ELSE CASE WHEN LocationLevel = 1 THEN ChildNode.LocationID ELSE NULL END END,
		CASE WHEN LocationLevel > 2 THEN Tree_CTE.L3 ELSE CASE WHEN LocationLevel = 2 THEN ChildNode.LocationID ELSE NULL END END,
		CASE WHEN LocationLevel > 3 THEN Tree_CTE.L4 ELSE CASE WHEN LocationLevel = 3 THEN ChildNode.LocationID ELSE NULL END END,
		CASE WHEN LocationLevel > 4 THEN Tree_CTE.L5 ELSE CASE WHEN LocationLevel = 4 THEN ChildNode.LocationID ELSE NULL END END,
		CASE WHEN LocationLevel > 5 THEN Tree_CTE.L6 ELSE CASE WHEN LocationLevel = 5 THEN ChildNode.LocationID ELSE NULL END END,
		ChildNode.StatusID, LocationCodeHierarchy + '/' + ChildNode.LocationCode,LocationNameHierarchy + '/' + ChildNode.LocationName
		
		FROM LocationTable AS ChildNode
    INNER JOIN Tree_CTE
    ON ChildNode.ParentLocationID = Tree_CTE.LocationID 
)
SELECT A.LocationID , 
		LD.LocationCode, A.LocationCodeHierarchy,
		LD.LocationName, A.LocationNameHierarchy ,
			LD.ParentLocationID, A.LocationLevel [Level], A.StatusID,
			A.LocationIDHierarchy AllLevelIDs,
			A.L1 Level1ID, A.L2 Level2ID, A.L3 Level3ID, A.L4 Level4ID, A.L5 Level5ID, A.L6 Level6ID,
			L1D.LocationName L1LocationName, L2D.LocationName L2LocationName, L3D.LocationName L3LocationName, 
			L4D.LocationName L4LocationName, L5D.LocationName L5LocationName, L6D.LocationName L6LocationName,LD.CompanyID,
			ISNULL(LD.Attribute1, L2D.Attribute1) Attribute1,
			ISNULL(LD.Attribute2, L2D.Attribute2) Attribute2,
			ISNULL(LD.Attribute3, L2D.Attribute3) Attribute3,
			ISNULL(LD.Attribute4, L2D.Attribute4) Attribute4,
			ISNULL(LD.Attribute5, L2D.Attribute5) Attribute5,
			ISNULL(LD.Attribute6, L2D.Attribute6) Attribute6,
			ISNULL(LD.Attribute7, L2D.Attribute7) Attribute7,
			ISNULL(LD.Attribute8, L2D.Attribute8) Attribute8,
			ISNULL(LD.Attribute9, L2D.Attribute9) Attribute9,
			ISNULL(LD.Attribute10, L2D.Attribute10) Attribute10,
			ISNULL(LD.Attribute11, L2D.Attribute11) Attribute11,
			ISNULL(LD.Attribute12, L2D.Attribute12) Attribute12,
			ISNULL(LD.Attribute13, L2D.Attribute13) Attribute13,
			ISNULL(LD.Attribute14, L2D.Attribute14) Attribute14,
			ISNULL(LD.Attribute15, L2D.Attribute15) Attribute15,
			ISNULL(LD.Attribute16, L2D.Attribute16) Attribute16,
			LT.LocationTypeName as LocationType,LT.LocationTypeID as LocationTypeID,
			L2D.LocationID [MappedLocationID],L2D.LocationCode as MappedLocationCode,L2D.LocationName as MappedLocationName
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

drop table tmp_LocationNewHierarchicalView
go
select *  into  tmp_LocationNewHierarchicalView from LocationNewHierarchicalView
go 


ALTER View [dbo].[CategoryNewView]
as 
	Select CategoryID,CategoryCode,CategoryCodeHierarchy,CategoryName,CategoryNameHierarchy,ParentCategoryID,Level,StatusID,AllLevelIDs,
	Level1ID,Level2ID,Level3ID,Level4ID,Level5ID,Level6ID,L1CategoryName,L2CategoryName,L3CategoryName,L4CategoryName,
	L5CategoryName,L6CategoryName,Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6,Attribute7,Attribute8,Attribute9,
	Attribute10,Attribute11,Attribute12,Attribute13,Attribute14,Attribute15,Attribute16,CategoryType,CategoryTypeID,
	MappedCategoryID,MappedCategoryCode,MappedCategoryName 
	from tmp_CategoryNewHierarchicalView
GO
		
ALTER View [dbo].[LocationNewView]
as
	select LocationID,LocationCode,LocationCodeHierarchy,LocationName,LocationNameHierarchy,ParentLocationID,Level,
	StatusID,AllLevelIDs,Level1ID,Level2ID,Level3ID,Level4ID,Level5ID,Level6ID,L1LocationName,L2LocationName,L3LocationName,L4LocationName,
	L5LocationName,L6LocationName,CompanyID,Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6,Attribute7,
	Attribute8,Attribute9,Attribute10,Attribute11,Attribute12,Attribute13,Attribute14,Attribute15,
	Attribute16,LocationType,LocationTypeID,MappedLocationID,MappedLocationCode,MappedLocationName 
	from tmp_LocationNewHierarchicalView
GO


If not exists(select Entityname from EntityTable where EntityName='AssetTransfer')
begin 
insert into EntityTable(EntityID,EntityName)
select max(entityid)+1,'AssetTransfer'
from EntityTable
end 
go 
If not exists(select Entityname from EntityTable where EntityName='AssetRetirement')
begin 
insert into EntityTable(EntityID,EntityName)
select max(entityid)+1,'AssetRetirement'
from EntityTable
end 
go 
if not exists(select Importfield from ImportTemplateNewTable where ImportField='Barcode' and 
		EntityID=(select EntityId from EntityTable where EntityName='AssetTransfer'))
Begin 
 Insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,DataFormat,
					IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select 'Barcode',1,1,1,100,NULL,0,100,2,1,NULL,EntityID
from EntityTable where EntityName='AssetTransfer'
End 
go 
if not exists(select Importfield from ImportTemplateNewTable where ImportField='RoomCode' and 
		EntityID=(select EntityId from EntityTable where EntityName='AssetTransfer'))
Begin 
 Insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,DataFormat,
					IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select 'RoomCode',2,1,1,100,NULL,0,100,2,1,NULL,EntityID
from EntityTable where EntityName='AssetTransfer'
End 
go 
if not exists(select Importfield from ImportTemplateNewTable where ImportField='CustodianCode' and 
		EntityID=(select EntityId from EntityTable where EntityName='AssetTransfer'))
Begin 
 Insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,DataFormat,
					IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select 'CustodianCode',3,1,1,100,NULL,0,100,2,1,NULL,EntityID
from EntityTable where EntityName='AssetTransfer'
End 
go 
if not exists(select Importfield from ImportTemplateNewTable where ImportField='DepartmentCode' and 
		EntityID=(select EntityId from EntityTable where EntityName='AssetTransfer'))
Begin 
 Insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,DataFormat,
					IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select 'DepartmentCode',4,1,1,100,NULL,0,100,2,1,NULL,EntityID
from EntityTable where EntityName='AssetTransfer'
End 
go 
if not exists(select Importfield from ImportTemplateNewTable where ImportField='SectionCode' and 
		EntityID=(select EntityId from EntityTable where EntityName='AssetTransfer'))
Begin 
 Insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,DataFormat,
					IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select 'SectionCode',5,1,1,100,NULL,0,100,2,1,NULL,EntityID
from EntityTable where EntityName='AssetTransfer'
End 
go 

if not exists(select Importfield from ImportTemplateNewTable where ImportField='Barcode' and 
		EntityID=(select EntityId from EntityTable where EntityName='AssetRetirement'))
Begin 
 Insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,DataFormat,
					IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select 'Barcode',1,1,1,100,NULL,0,100,2,1,NULL,EntityID
from EntityTable where EntityName='AssetRetirement'
End 
go 
if not exists(select Importfield from ImportTemplateNewTable where ImportField='RetirementValue' and 
		EntityID=(select EntityId from EntityTable where EntityName='AssetRetirement'))
Begin 
 Insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,DataFormat,
					IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select 'RetirementValue',2,1,1,100,NULL,0,100,2,1,NULL,EntityID
from EntityTable where EntityName='AssetRetirement'
End 
go 
if not exists(select Importfield from ImportTemplateNewTable where ImportField='RetirementReferencesNo' and 
		EntityID=(select EntityId from EntityTable where EntityName='AssetRetirement'))
Begin 
 Insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,DataFormat,
					IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select 'RetirementReferencesNo',3,1,1,100,NULL,0,100,2,1,NULL,EntityID
from EntityTable where EntityName='AssetRetirement'
End 
go 
if not exists(select Importfield from ImportTemplateNewTable where ImportField='RetirementRemarks' and 
		EntityID=(select EntityId from EntityTable where EntityName='AssetRetirement'))
Begin 
 Insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,DataFormat,
					IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select 'RetirementRemarks',4,1,1,100,NULL,0,100,2,1,NULL,EntityID
from EntityTable where EntityName='AssetRetirement'
End 
go 

if not exists(select Importfield from ImportTemplateNewTable where ImportField='ProceedOfSales' and 
		EntityID=(select EntityId from EntityTable where EntityName='AssetRetirement'))
Begin 
 Insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,DataFormat,
					IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select 'ProceedOfSales',5,1,1,100,NULL,0,100,2,1,NULL,EntityID
from EntityTable where EntityName='AssetRetirement'
End 
go 
if not exists(select Importfield from ImportTemplateNewTable where ImportField='CostOfRemoval' and 
		EntityID=(select EntityId from EntityTable where EntityName='AssetRetirement'))
Begin 
 Insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,DataFormat,
					IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select 'CostOfRemoval',6,1,1,100,NULL,0,100,2,1,NULL,EntityID
from EntityTable where EntityName='AssetRetirement'
End 
go 
if not exists(select Importfield from ImportTemplateNewTable where ImportField='RetirementTypeCode' and 
		EntityID=(select EntityId from EntityTable where EntityName='AssetRetirement'))
Begin 
 Insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,DataFormat,
					IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select 'RetirementTypeCode',7,1,1,100,NULL,0,100,2,1,NULL,EntityID
from EntityTable where EntityName='AssetRetirement'
End 
go 
if not exists(select Importfield from ImportTemplateNewTable where ImportField='RetirementDate' and 
		EntityID=(select EntityId from EntityTable where EntityName='AssetRetirement'))
Begin 
 Insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,DataFormat,
					IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select 'RetirementDate',8,1,1,100,NULL,0,100,2,1,NULL,EntityID
from EntityTable where EntityName='AssetRetirement'
End 
go 

IF NOT EXISTS (SELECT *  FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME = 'TransactionLineItemTable' AND COLUMN_NAME = 'CustodianID')
BEGIN
	  ALTER TABLE TransactionLineItemTable
			ADD CustodianID INT REFERENCES User_LoginUserTable(UserID)
End 
go 
IF NOT EXISTS (SELECT *  FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME = 'TransactionLineItemTable' AND COLUMN_NAME = 'DepartmentID')
BEGIN
	  ALTER TABLE TransactionLineItemTable
			ADD DepartmentID INT REFERENCES DepartmentTable(DepartmentID)
End 
go 
IF NOT EXISTS (SELECT *  FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME = 'TransactionLineItemTable' AND COLUMN_NAME = 'SectionID')
BEGIN
	  ALTER TABLE TransactionLineItemTable
			ADD SectionID INT REFERENCES SectionTable(SectionID)
End 
IF NOT EXISTS (SELECT *  FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME = 'TransactionLineItemTable' AND COLUMN_NAME = 'TransferDate')
BEGIN
	  ALTER TABLE TransactionLineItemTable
			ADD TransferDate smallDatetime 
End 
go 
IF NOT EXISTS (SELECT *  FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME = 'TransactionLineItemTable' AND COLUMN_NAME = 'FromAssetConditionID')
BEGIN
	  ALTER TABLE TransactionLineItemTable
			ADD FromAssetConditionID INT REFERENCES AssetConditionTable(AssetConditionID) 
End 
go 
IF NOT EXISTS (SELECT *  FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME = 'TransactionLineItemTable' AND COLUMN_NAME = 'ToAssetConditionID')
BEGIN
	  ALTER TABLE TransactionLineItemTable
			ADD ToAssetConditionID INT REFERENCES AssetConditionTable(AssetConditionID) 
End 
go 

if not exists(select ConfiguarationName from ConfigurationTable where ConfiguarationName='SelectedCategoriesareMandatorySerialNumberInAssetScreen')
begin
  Insert into ConfigurationTable(ConfiguarationName,ConfiguarationValue,ToolTipName,DataType,DropDownValue,MinValue,MaxValue,
ConfiguarationType,DisplayOrderID,DefaultValue,DisplayConfiguration,SuffixText,
CatagoryName,CategoryName)
values('SelectedCategoriesareMandatorySerialNumberInAssetScreen','','SelectedCategoriesareMandatory','MultiSelect',NULL,
NULL,100,'C',131,1,1,null,'AssetSettings','AssetSettings')
End 
GO 
if not exists(select ConfiguarationName from ConfigurationTable where ConfiguarationName='SelectedManufacturerareMandatorySerialNumberInAssetScreen')
begin
  Insert into ConfigurationTable(ConfiguarationName,ConfiguarationValue,ToolTipName,DataType,DropDownValue,MinValue,MaxValue,
ConfiguarationType,DisplayOrderID,DefaultValue,DisplayConfiguration,SuffixText,
CatagoryName,CategoryName)
values('SelectedManufacturerareMandatorySerialNumberInAssetScreen','','SelectedManufacturerareMandatory','MultiSelect',NULL,
NULL,100,'C',132,1,1,null,'AssetSettings','AssetSettings')
End 
GO 
if not exists(select ConfiguarationName from ConfigurationTable where ConfiguarationName='PreferredLevelLocationMapping')
begin
  Insert into ConfigurationTable(ConfiguarationName,ConfiguarationValue,ToolTipName,DataType,DropDownValue,MinValue,MaxValue,
ConfiguarationType,DisplayOrderID,DefaultValue,DisplayConfiguration,SuffixText,
CatagoryName,CategoryName)
values('PreferredLevelLocationMapping','2','PreferredLevelLocationMapping','Numeric',NULL,
1,6,'C',133,1,1,null,'AssetSettings','AssetSettings')
End 
GO 
if not exists(select ConfiguarationName from ConfigurationTable where ConfiguarationName='PreferredLevelCategoryMapping')
begin
  Insert into ConfigurationTable(ConfiguarationName,ConfiguarationValue,ToolTipName,DataType,DropDownValue,MinValue,MaxValue,
ConfiguarationType,DisplayOrderID,DefaultValue,DisplayConfiguration,SuffixText,
CatagoryName,CategoryName)
values('PreferredLevelCategoryMapping','2','PreferredLevelCategoryMapping','Numeric',NULL,
1,6,'C',134,1,1,null,'AssetSettings','AssetSettings')
End 
GO 



CREATE TABLE [dbo].[WebServiceLogTable](
	[WebServiceLogID] [bigint] IDENTITY(1,1) NOT NULL,
	[MethodName] [varchar](250) NULL,
	[RequestedDateTime] [datetime] NOT NULL,
	[DeviceIMEI] [varchar](250) NULL,
	[UserID] [int] NULL,
	[AppVersion] [varchar](250) NULL,
	[WarehouseCode] [varchar](250) NULL,
	[Parameters] [varchar](max) NULL,
	[TimeTakenToCompleteService] [int] NULL,
	[Response] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[WebServiceLogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[WebServiceLogTable] ADD  DEFAULT (getdate()) FOR [RequestedDateTime]
GO



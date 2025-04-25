if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='L1LocCode' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='Location')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'L1LocCode','L1LocCode',100,NULL,1,1,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='Location'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='L1Desc' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='Location')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'L1Desc','L1Desc',100,NULL,1,2,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='Location'
	End 
	go 
		if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='L2LocCode' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='Location')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'L2LocCode','L2LocCode',100,NULL,1,3,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='Location'
	End 
	go 
if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='L2Desc' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='Location')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'L2Desc','L2Desc',100,NULL,1,4,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='Location'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='L3LocCode' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='Location')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'L3LocCode','L3LocCode',100,NULL,1,5,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='Location'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='L3Desc' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='Location')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'L3Desc','L3Desc',100,NULL,1,6,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='Location'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='L4locCode' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='Location')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'L4locCode','L4locCode',100,NULL,1,7,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='Location'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='L4Desc' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='Location')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'L4Desc','L4Desc',100,NULL,1,8,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='Location'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='L5locCode' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='Location')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'L5locCode','L5locCode',100,NULL,1,9,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='Location'
	End 
	go
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='L5Desc' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='Location')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'L5Desc','L5Desc',100,NULL,1,10,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='Location'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='L6locCode' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='Location')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'L6locCode','L6locCode',100,NULL,1,11,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='Location'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='L6Desc' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='Location')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'L6Desc','L6Desc',100,NULL,1,12,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='Location'
	End 
	go 
	If not exists(select mastergridname from MasterGridNewTable where mastergridname='Category')
	Begin
	Insert into MasterGridNewTable(MasterGridName,EntityName)
	Values('Category','CategoryTable')
	End
	go 

if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='L1CategoryCode' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='Category')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'L1CategoryCode','L1CategoryCode',100,NULL,1,1,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='Category'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='L1Desc' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='Category')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'L1Desc','L1Desc',100,NULL,1,2,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='Category'
	End 
	go 
		if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='L2CategoryCode' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='Category')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'L2CategoryCode','L2CategoryCode',100,NULL,1,3,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='Category'
	End 
	go 
if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='L2Desc' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='Category')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'L2Desc','L2Desc',100,NULL,1,4,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='Category'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='L3CategoryCode' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='Category')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'L3CategoryCode','L3CategoryCode',100,NULL,1,5,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='Category'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='L3Desc' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='Category')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'L3Desc','L3Desc',100,NULL,1,6,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='Category'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='L4CategoryCode' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='Category')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'L4CategoryCode','L4CategoryCode',100,NULL,1,7,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='Category'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='L4Desc' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='Category')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'L4Desc','L4Desc',100,NULL,1,8,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='Category'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='L5CategoryCode' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='Category')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'L5CategoryCode','L5CategoryCode',100,NULL,1,9,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='Category'
	End 
	go
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='L5Desc' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='Category')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'L5Desc','L5Desc',100,NULL,1,10,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='Category'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='L6CategoryCode' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='Category')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'L6CategoryCode','L6CategoryCode',100,NULL,1,11,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='Category'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='L6Desc' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='Category')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'L6Desc','L6Desc',100,NULL,1,12,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='Category'
	End 
	go 

alter PROCEDURE [dbo].[prc_ExportToLocation]
(
@UserID INT 
)
AS 
BEGIN
DECLARE @UserLocMapping BIT 
SELECT @UserLocMapping= CASE WHEN ConfiguarationValue='true' THEN 1 ELSE 0 END  FROM dbo.ConfigurationTable WHERE ConfiguarationName='UserLocationMapping'

SELECT b.LocationCode AS L1LocCode,a.L1LocationName as L1Desc, 
CASE WHEN c.LocationCode IS NULL THEN '' ELSE c.LocationCode END  AS L2LocCode ,
CASE WHEN a.L2LocationName  IS NULL THEN '' ELSE a.L2LocationName END  AS L2Desc ,
CASE WHEN d.LocationCode IS NULL THEN '' ELSE d.LocationCode END  AS L3LocCode ,
CASE WHEN a.L3LocationName IS NULL THEN '' ELSE a.L3LocationName END  AS L3Desc ,
CASE WHEN e.LocationCode IS NULL THEN '' ELSE e.LocationCode END  AS L4locCode ,
CASE WHEN a.L4LocationName IS NULL THEN '' ELSE a.L4LocationName END  AS L4Desc,
CASE WHEN f.LocationCode IS NULL THEN '' ELSE f.LocationCode END  AS L5locCode ,
CASE WHEN a.L5LocationName IS NULL THEN '' ELSE a.L5LocationName END  AS L5Desc ,
CASE WHEN g.LocationCode IS NULL THEN '' ELSE g.LocationCode END  AS L6locCode ,
CASE WHEN a.L6LocationName IS NULL THEN '' ELSE a.L6LocationName END  AS L6Desc 
FROM LocationnewHierarchicalView a 
LEFT JOIN dbo.LocationTable b ON a.Level1ID=b.LocationID
LEFT JOIN dbo.LocationTable c ON a.Level2ID=c.LocationID
LEFT JOIN dbo.LocationTable d ON a.Level3ID=d.LocationID
LEFT JOIN dbo.LocationTable e ON a.Level4ID=e.LocationID
LEFT JOIN dbo.LocationTable f ON a.Level5ID=f.LocationID
LEFT JOIN dbo.LocationTable g ON a.Level6ID=g.LocationID
WHERE a.StatusID=1 AND 
(@UserLocMapping=0 OR (@UserLocMapping=1 AND a.MappedLocationID IN (SELECT locationid FROM dbo.UserLocationMappingTable WHERE PersonID IN (@UserID))))

END 
go 
create PROCEDURE [dbo].[prc_ExportToCategory]
(
@UserID INT 
)
AS 
BEGIN
DECLARE @UserCategoryMapping BIT 
SELECT @UserCategoryMapping= CASE WHEN ConfiguarationValue='true' THEN 1 ELSE 0 END  FROM dbo.ConfigurationTable WHERE ConfiguarationName='UserCategoryMapping'

SELECT b.CategoryCode AS L1CategoryCode,a.L1CategoryName as L1Desc, 
CASE WHEN c.CategoryCode IS NULL THEN '' ELSE c.CategoryCode END  AS L2CategoryCode ,
CASE WHEN a.L2CategoryName  IS NULL THEN '' ELSE a.L2CategoryName END  AS L2Desc ,
CASE WHEN d.CategoryCode IS NULL THEN '' ELSE d.CategoryCode END  AS L3CategoryCode ,
CASE WHEN a.L3CategoryName IS NULL THEN '' ELSE a.L3CategoryName END  AS L3Desc ,
CASE WHEN e.CategoryCode IS NULL THEN '' ELSE e.CategoryCode END  AS L4CategoryCode ,
CASE WHEN a.L4CategoryName IS NULL THEN '' ELSE a.L4CategoryName END  AS L4Desc,
CASE WHEN f.CategoryCode IS NULL THEN '' ELSE f.CategoryCode END  AS L5CategoryCode ,
CASE WHEN a.L5CategoryName IS NULL THEN '' ELSE a.L5CategoryName END  AS L5Desc ,
CASE WHEN g.CategoryCode IS NULL THEN '' ELSE g.CategoryCode END  AS L6CategoryCode ,
CASE WHEN a.L6CategoryName IS NULL THEN '' ELSE a.L6CategoryName END  AS L6Desc 
FROM CategorynewHierarchicalView a 
LEFT JOIN dbo.CategoryTable b ON a.Level1ID=b.CategoryID
LEFT JOIN dbo.CategoryTable c ON a.Level2ID=c.CategoryID
LEFT JOIN dbo.CategoryTable d ON a.Level3ID=d.CategoryID
LEFT JOIN dbo.CategoryTable e ON a.Level4ID=e.CategoryID
LEFT JOIN dbo.CategoryTable f ON a.Level5ID=f.CategoryID
LEFT JOIN dbo.CategoryTable g ON a.Level6ID=g.CategoryID
WHERE a.StatusID=1 AND 
(@UserCategoryMapping=0 OR (@UserCategoryMapping=1 AND a.MappedCategoryID IN (SELECT Categoryid FROM dbo.UserCategoryMappingTable WHERE PersonID IN (@UserID))))

END 
go 
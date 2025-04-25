Create Table DashboardTypeTable
(
  DashboardTypeID int not null primary key identity(1,1),
  DashboardType nvarchar(100) not null
)
go
if not exists(select DashboardType from DashboardTypeTable where DashboardType='Count')
BEgin 
insert into DashboardTypeTable(DashboardType) values('Count')
End 
go 
if not exists(select DashboardType from DashboardTypeTable where DashboardType='PieChart')
BEgin 
insert into DashboardTypeTable(DashboardType) values('PieChart')
End 
go 
if not exists(select DashboardType from DashboardTypeTable where DashboardType='BarChart-Withoutseries')
BEgin 
insert into DashboardTypeTable(DashboardType) values('BarChart-Withoutseries')
End 
go 
if not exists(select DashboardType from DashboardTypeTable where DashboardType='BarChart-Withseries')
BEgin 
insert into DashboardTypeTable(DashboardType) values('BarChart-Withseries')
End 
go 
if not exists(select DashboardType from DashboardTypeTable where DashboardType='BarChart-Withseries')
BEgin 
insert into DashboardTypeTable(DashboardType) values('BarChart-Withseries')
End 
go 
if not exists(select DashboardType from DashboardTypeTable where DashboardType='LineChart')
BEgin 
insert into DashboardTypeTable(DashboardType) values('LineChart')
End 
go 


Create Table DashboardTemplateTable 
(
	DashboardTemplateID int not null primary key identity(1,1),
	DashboardTemplateName nvarchar(100) not null,
	QueryString nvarchar(100) not null,
	Remarks nvarchar(max) null,
	StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references user_LoginUserTable(UserID),
	CreatedDateTime SmallDatetime not null,
	LastModifiedBy int  null foreign key references user_LoginUserTable(UserID),
	LastModifiedDateTime SmallDatetime  null
)
go 

Create table DashboardTemplateFieldTable
(
  DashboardTemplateFieldID int not null primary key identity(1,1),
  DashboardTemplateID int not null foreign key references DashboardTemplateTable(DashboardTemplateID),
  FieldName  nvarchar(200) not null,
  QueryFieldName nvarchar(200) not null,
--BackgroundTileColorCode nvarchar(200) null,
  DisplayTitle nvarchar(200) not null,
  FieldDataType nvarchar(200) not null,
  DefaultFormat  nvarchar(200)  null,
  RedirectPageName nvarchar(100) NULL,
  StatusID int not null foreign key references StatusTable(StatusID),
  CreatedBy int not null foreign key references user_LoginUserTable(UserID),
  CreatedDateTime SmallDatetime not null,
  LastModifiedBy int  null foreign key references user_LoginUserTable(UserID),
  LastModifiedDateTime SmallDatetime  null
)
go 

Create table DashboardTemplateFilterFieldTable
(
	  DashboardTemplateFilterFieldID  int not null primary key identity(1,1),
	  DashboardTemplateID int not null foreign key references DashboardTemplateTable(DashboardTemplateID),
	  FieldName  nvarchar(200) not null,
	  DisplayTitle nvarchar(200) not null,
	  OrderNo int not null,
	  IsMandatory bit null default(0),
	  IsFixedFilter bit null default(0),
	  ScreenFilterTypeID tinyint not null foreign key references ScreenFilterTypeTable(ScreenFilterTypeID),
	  FieldTypeID int not null foreign key references AFieldTypeTable(FieldTypeID),
	  StatusID int not null foreign key references StatusTable(StatusID),
	  CreatedBy int not null foreign key references user_LoginUserTable(UserID),
	  CreatedDateTime SmallDatetime not null,
	  LastModifiedBy int  null foreign key references user_LoginUserTable(UserID),
	  LastModifiedDateTime SmallDatetime  null
)
go 
select * from User_MenuTable Order by 1 desc 

 If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='DashboardTemplate')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('DashboardTemplate','DashboardTemplate',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Templates'),1,0);
End
Go
If not exists(SELECT MenuName FROM USER_MENUTABLE where MenuName='DashboardTemplate' and ParentTransactionID=1)
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO,ParentTransactionID) Values(

'DashboardTemplate',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='DashboardTemplate'),'/DashboardTemplate/Index',
(Select MenuID from USER_MENUTABLE where MenuName='Templates' ),8,1);
end
go

select * from USER_MENUTABLE order by 1 desc 


select * from MasterGridNewTable Order by 1 desc 
If not exists(select Mastergridname from MasterGridNewTable where MasterGridName='DashboardTemplate')
Begin
 insert into MasterGridNewTable(MasterGridName,EntityName)
 values('DashboardTemplate','DashboardTemplateTable')
End 
go 
select * from MasterGridNewLineItemTable Order by 1 desc 
If not exists(select FieldName from MasterGridNewLineItemTable where FieldName='DashboardTemplateName' and MasterGridID=(select MasterGridID from MasterGridNewTable where MasterGridName='DashboardTemplate'))
Begin
 insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
 select 'DashboardTemplateName','DashboardTemplateName',100,NULL,1,1,'System.String',NULL,MasterGridID
 from MasterGridNewTable where MasterGridName='DashboardTemplate'
end
go 
If not exists(select FieldName from MasterGridNewLineItemTable where FieldName='QueryString' and MasterGridID=(select MasterGridID from MasterGridNewTable where MasterGridName='DashboardTemplate'))
Begin
 insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
 select 'QueryString','QueryString',100,NULL,1,2,'System.String',NULL,MasterGridID
 from MasterGridNewTable where MasterGridName='DashboardTemplate'
end
go 
If not exists(select FieldName from MasterGridNewLineItemTable where FieldName='Remarks' and MasterGridID=(select MasterGridID from MasterGridNewTable where MasterGridName='DashboardTemplate'))
Begin
 insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
 select 'Remarks','Remarks',100,NULL,1,3,'System.String',NULL,MasterGridID
 from MasterGridNewTable where MasterGridName='DashboardTemplate'
end
go

If not exists(select FieldName from MasterGridNewLineItemTable where FieldName='Status.Status' and MasterGridID=(select MasterGridID from MasterGridNewTable where MasterGridName='DashboardTemplate'))
Begin
 insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
 select 'Status.Status','Status',100,NULL,1,4,'System.String',NULL,MasterGridID
 from MasterGridNewTable where MasterGridName='DashboardTemplate'
end
go
If not exists(select FieldName from MasterGridNewLineItemTable where FieldName='CreatedByNavigation.UserName' and MasterGridID=(select MasterGridID from MasterGridNewTable where MasterGridName='DashboardTemplate'))
Begin
 insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
 select 'CreatedByNavigation.UserName','CreatedBy',100,NULL,1,5,'System.String',NULL,MasterGridID
 from MasterGridNewTable where MasterGridName='DashboardTemplate'
end
go
If not exists(select FieldName from MasterGridNewLineItemTable where FieldName='CreatedDateTime' and MasterGridID=(select MasterGridID from MasterGridNewTable where MasterGridName='DashboardTemplate'))
Begin
 insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
 select 'CreatedDateTime','CreatedOn',100,'dd/MM/yyyy hh:mm:ss',1,6,'System.DateTime',NULL,MasterGridID
 from MasterGridNewTable where MasterGridName='DashboardTemplate'
end
go

select * from User_MenuTable order by 1 desc 

update User_MenuTable set TargetObject='/DashboardTemplate/Index' where MenuName='DashboardTemplate'
go

create View [dbo].[vwDashboardTemplateObjects]
AS
	SELECT * 
		FROM  [vwSystemTemplateObjects]
		WHERE
			(ObjectName LIKE 'dvw%' OR ObjectName LIKE 'dprc%') and 
			ObjectName not in (select QueryString from DashboardTemplateTable where StatusID = dbo.fnGetActiveStatusID())
GO

Create table DashboardMappingTable
(
	  DashboardMappingID int not null primary key identity(1,1),
	  DashboardTemplateID int not null foreign key references DashboardTemplateTable(DashboardTemplateID),
	  DasboardMappingName nvarchar(100) not null,
	  DashboardBackGrounndColors nvarchar(max) not null,
	  DashboardHeight nvarchar(100) not null default('300px'),
	  DashboardWeight nvarchar(100) not null default('300px'),
	  DashboardTypeID int not null foreign key references DashboardTypeTable(DashboardTypeID),
	  XAxisTitle nvarchar(100) NULL,
      YAxisTitle nvarchar(100) NULL,
	  StatusID int not null foreign key references StatusTable(StatusID),
	  CreatedBy int not null foreign key references user_LoginUserTable(UserID),
	  CreatedDateTime SmallDatetime not null,
	  LastModifiedBy int  null foreign key references user_LoginUserTable(UserID),
	  LastModifiedDateTime SmallDatetime  null

)
go 


Create table DashboardFieldMappingTable
(
  DashboardFieldMappingID int not null primary key identity(1,1),
  DashboardMappingID int not null foreign key references DashboardMappingTable(DashboardMappingID),
  DashboardTemplateFieldID int not null foreign key references DashboardTemplateFieldTable(DashboardTemplateFieldID),
  FieldName  nvarchar(200) not null,
  DisplayTitle nvarchar(200) not null,
  ColorCode nvarchar(200)  null,
  XAxisField nvarchar(100)  null, -- procedureor view fieldsname 
  YAxisField nvarchar(100)  null,
  IconPath nvarchar(max) NULL,
  RedirectPageName nvarchar(100) NULL,
  StatusID int not null foreign key references StatusTable(StatusID),
  CreatedBy int not null foreign key references user_LoginUserTable(UserID),
  CreatedDateTime SmallDatetime not null,
  LastModifiedBy int  null foreign key references user_LoginUserTable(UserID),
  LastModifiedDateTime SmallDatetime  null
)
 If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='DashboardMapping')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('DashboardMapping','DashboardMapping',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Templates'),1,0);
End
Go
If not exists(SELECT MenuName FROM USER_MENUTABLE where MenuName='DashboardMapping' and ParentTransactionID=1)
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO,ParentTransactionID) Values(

'DashboardMapping',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='DashboardMapping'),'/DashboardMapping/Index',
(Select MenuID from USER_MENUTABLE where MenuName='Templates' ),8,1);
end
go

If not exists(select Mastergridname from MasterGridNewTable where MasterGridName='DashboardMapping')
Begin
 insert into MasterGridNewTable(MasterGridName,EntityName)
 values('DashboardMapping','DashboardMappingTable')
End 
go 

If not exists(select FieldName from MasterGridNewLineItemTable where FieldName='DasboardMappingName' and MasterGridID=(select MasterGridID from MasterGridNewTable where MasterGridName='DashboardMapping'))
Begin
 insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
 select 'DasboardMappingName','DasboardMappingName',100,NULL,1,1,'System.String',NULL,MasterGridID
 from MasterGridNewTable where MasterGridName='DashboardMapping'
end
go 
If not exists(select FieldName from MasterGridNewLineItemTable where FieldName='DashboardTemplate.DashboardTemplateName' and MasterGridID=(select MasterGridID from MasterGridNewTable where MasterGridName='DashboardMapping'))
Begin
 insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
 select 'DashboardTemplate.DashboardTemplateName','DashboardTemplateName',100,NULL,1,2,'System.String',NULL,MasterGridID
 from MasterGridNewTable where MasterGridName='DashboardMapping'
end
go 
If not exists(select FieldName from MasterGridNewLineItemTable where FieldName='DashboardType.DashboardType' and MasterGridID=(select MasterGridID from MasterGridNewTable where MasterGridName='DashboardMapping'))
Begin
 insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
 select 'DashboardType.DashboardType','DashboardType',100,NULL,1,3,'System.String',NULL,MasterGridID
 from MasterGridNewTable where MasterGridName='DashboardMapping'
end
go

If not exists(select FieldName from MasterGridNewLineItemTable where FieldName='Status.Status' and MasterGridID=(select MasterGridID from MasterGridNewTable where MasterGridName='DashboardMapping'))
Begin
 insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
 select 'Status.Status','Status',100,NULL,1,4,'System.String',NULL,MasterGridID
 from MasterGridNewTable where MasterGridName='DashboardMapping'
end
go
If not exists(select FieldName from MasterGridNewLineItemTable where FieldName='CreatedByNavigation.UserName' and MasterGridID=(select MasterGridID from MasterGridNewTable where MasterGridName='DashboardMapping'))
Begin
 insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
 select 'CreatedByNavigation.UserName','CreatedBy',100,NULL,1,5,'System.String',NULL,MasterGridID
 from MasterGridNewTable where MasterGridName='DashboardMapping'
end
go
If not exists(select FieldName from MasterGridNewLineItemTable where FieldName='CreatedDateTime' and MasterGridID=(select MasterGridID from MasterGridNewTable where MasterGridName='DashboardMapping'))
Begin
 insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
 select 'CreatedDateTime','CreatedOn',100,'dd/MM/yyyy hh:mm:ss',1,6,'System.DateTime',NULL,MasterGridID
 from MasterGridNewTable where MasterGridName='DashboardMapping'
end
go



ALTER View [dbo].[vwDashboardTemplateObjects]
AS
select * from (
	SELECT 
		id ObjectID,
		name as ObjectName,
		(case when type='p' then 'Procedure' else 'View' end ) as ObjectType
		FROM sysobjects 
		where type IN('p', 'v') 
			AND(name LIKE 'dvw%' OR name LIKE 'dprc%' OR name LIKE 'dvw%' OR name LIKE 'dprc%') 
		)x where
			ObjectName not in (select QueryString from DashboardTemplateTable where StatusID = dbo.fnGetActiveStatusID())
GO

Alter table DashboardTemplateFilterFieldTable add QueryField nvarchar(1000) not null
go 

if not exists(select FieldTypeDesc from AFieldTypeTable where FieldTypeDesc='Integer')
Begin
   Insert into AFieldTypeTable(FieldTypeCode,FieldTypeDesc)
   values('1','Integer')
End 

go 
if not exists(select FieldTypeDesc from AFieldTypeTable where FieldTypeDesc='Float')
Begin
   Insert into AFieldTypeTable(FieldTypeCode,FieldTypeDesc)
   values('2','Float')
End 
go 
if not exists(select FieldTypeDesc from AFieldTypeTable where FieldTypeDesc='String')
Begin
   Insert into AFieldTypeTable(FieldTypeCode,FieldTypeDesc)
   values('3','String')
End 
go 

Alter table DashboardFieldMappingTable add CategoriesField nvarchar(max) null 
go 

-- ==============================================================================================================================================
-- Author:  Saranya
-- Create date: 23-MAY-2017 10:00:00 AM
-- Description: Procedure for getting record count for dashboard
-- **********************************************************************************************************************************************
-- Date Time			Author			Description
-- **********************************************************************************************************************************************
-- 12-May-2020 15:00	Balakrishnan	For performance required indexes are created and query optimized
-- **********************************************************************************************************************************************
ALTER Procedure  [dbo].[dprc_DashboardCount](
	@LangaugeID int = 1, 
	@UserID int = 1, 
	@CompanyID int = 1003
) 
as 
Begin  
	Declare @UserDepartmentMapping bit,@UserLocationMapping bit, @UserCategoryMapping bit,@AssetApproval bit,@AssetMutipleApproval bit ,@TransferAssetApproval bit
	,@MultipleAssetTransferApproval bit,@LocationEnable bit,@WarrantyNotificationDay int, @TotalAssetCount int ,@ActiveAssetCount int ,@DisposedAssetCount int,
	@warrantyCount int 

	select @UserDepartmentMapping= case when lower(ConfiguarationValue)='false' then 0 else 1 end  from ConfigurationTable where ConfiguarationName='UserDepartmentMapping'
	select @UserLocationMapping= case when lower(ConfiguarationValue)='false' then 0 else 1 end  from ConfigurationTable where ConfiguarationName='UserLocationMapping'
	select @UserCategoryMapping= case when lower(ConfiguarationValue)='false' then 0 else 1 end  from ConfigurationTable where ConfiguarationName='UserCategoryMapping'

	DEclare @LocationMappingTable Table(LocationID int)
	Declare @CategoryMappingTable Table(CategoryID int)
	insert into @LocationMappingTable(LocationID)
	select LocationID  from ChildLocationMappingTable(@UserID)
	insert into @CategoryMappingTable(CategoryID)
	select CategoryID from ChildCategoryMappingTable(@UserID)

	if((select COUNT(*) FROM @LocationMappingTable) = 0) SET @UserLocationMapping = 0
	if((select COUNT(*) FROM @CategoryMappingTable) = 0) SET @UserCategoryMapping = 0

	select @LocationEnable = case when lower(ConfiguarationValue)='false' then 0 else 1 end  from ConfigurationTable where ConfiguarationName='LocationEnable'
	select @AssetApproval = case when lower(ConfiguarationValue)='false' then 0 else 1 end  from ConfigurationTable where ConfiguarationName='AssetApproval'
	select @AssetMutipleApproval= case when lower(ConfiguarationValue)='false' then 0 else 1 end  from ConfigurationTable where ConfiguarationName='AssetApprovalBasedOnWorkFlow'
	select @TransferAssetApproval= case when lower(ConfiguarationValue)='false' then 0 else 1 end  from ConfigurationTable where ConfiguarationName='TransferAssetApproval'
	select @MultipleAssetTransferApproval= case when lower(ConfiguarationValue)='false' then 0 else 1 end  from ConfigurationTable where ConfiguarationName='TransferAssetApprovalBasedOnWorkFlow'
	select @WarrantyNotificationDay= 
		case 
			when ConfiguarationValue='AssetWarrantyNotificationBefore30days' then 30 
			when ConfiguarationValue='AssetWarrantyNotificationBefore60days' then 60 
			when ConfiguarationValue='AssetWarrantyNotificationBefore90days' then 90 
			else 0 
			end    
		from ConfigurationTable where ConfiguarationName='WarrantyAlertNotificationDays'

	Select @TotalAssetCount=COUNT(AssetID) 
		from AssetTable 
		where (@UserCategoryMapping=0 or CategoryID in (select CategoryID from @CategoryMappingTable )) 
		and (@LocationEnable = 1 OR @UserLocationMapping = 0 or LocationID in (select LocationID from @LocationMappingTable))
		and (@UserDepartmentMapping=0 or DepartmentID in (Select DepartmentID from UserDepartmentMappingTable where PersonID=@UserID))
		--and (@AssetApproval=0 or AssetID  in (select ObjectkeyValue from ApprovalTable where StatusID=5 and ActionType in (2,3,4)))
		AND CompanyID=@CompanyID and statusID!=3 

	Select @ActiveAssetCount=COUNT(AssetID) 
		from AssetTable 
		where (@UserCategoryMapping=0 or CategoryID in (select CategoryID from @CategoryMappingTable )) 
		and (@LocationEnable=1 and @UserLocationMapping=0 or LocationID in (select locationid from @LocationMappingTable))
		and (@UserDepartmentMapping=0 or DepartmentID in (Select DepartmentID from UserDepartmentMappingTable where PersonID=@UserID))
		and (@AssetApproval=0 or AssetID  not in (select ObjectkeyValue from ApprovalTable where StatusID=5 and ActionType in (1)))
		and CompanyID=@CompanyID and statusID not in (3,4)

	Select @DisposedAssetCount=COUNT(AssetID) 
		from AssetTable 
		where (@UserCategoryMapping=0 or CategoryID in (select CategoryID from @CategoryMappingTable )) 
		and (@LocationEnable=1 and @UserLocationMapping=0 or LocationID in (select locationid from @LocationMappingTable))
		and (@UserDepartmentMapping=0 or DepartmentID in (Select DepartmentID from UserDepartmentMappingTable where PersonID=@UserID))
		and CompanyID=@CompanyID and statusID=4

	Select @warrantyCount=COUNT(AssetID) 
		from AssetTable 
		where (@UserCategoryMapping=0 or CategoryID in (select CategoryID from @CategoryMappingTable )) 
		and (@LocationEnable=1 and @UserLocationMapping=0 or LocationID in (select locationid from @LocationMappingTable))
		and (@UserDepartmentMapping=0 or DepartmentID in (Select DepartmentID from UserDepartmentMappingTable where PersonID=@UserID))
		and (@AssetApproval=0 or AssetID  not in (select ObjectkeyValue from ApprovalTable where StatusID=5 and ActionType in (1)))
		and CompanyID=@CompanyID and statusID not in (3,4) 
		and (WarrantyExpiryDate>=DATEADD(day,-1,getdate()) and WarrantyExpiryDate<=DATEADD(day,@WarrantyNotificationDay+1,getdate()))
	
	--delete from @LocationMappingTable
	--drop table @CategoryMappingTable
	Select @TotalAssetCount as TotalAssetCount,@ActiveAssetCount as ActiveAssetCount,@DisposedAssetCount as DisposedAssetCount,@warrantyCount as AssetWarrantyExpiryCount

End 

CREATE TABLE EmailSignatureTable
(
  EmailSignatureID int not null primary key identity(1,1),
  EmailSignatureCode nvarchar(100) not null,
  EmailSignatureContent nvarchar(max) not null,
  StatusID int NOT NULL foreign key references StatusTable(StatusID),
  CreatedBy int NOT NULL Foreign key references User_LoginUserTable(UserID),
  CreatedDateTime smalldatetime NOT NULL,
  LastModifiedBy int NULL  Foreign key references User_LoginUserTable(UserID),
  LastModifiedDateTime smalldatetime NULL
)
Go 
Create table AttachmentFormatTable
(
  AttachmentFormatID int not null Primary key, 
  AttachmentFormatCode nvarchar(100) not null, 
  AttachmentFormat Nvarchar(100) Not null
)
Go 

DROP TABLE NotificationTemplateNotificationTypeTable
GO 
DROP TABLE NotificationTemplateTable
GO 
 
CREATE TABLE NotificationTemplateTable(
	NotificationTemplateID int IDENTITY(1,1) NOT NULL pRIMARY KEY,
	TemplateName nvarchar(50) NOT NULL,
	TemplateSubject nvarchar(max) NOT NULL,
	NotificationTypeID int NOT NULL,
	NotificationModuleID int NOT NULL,
	ToAddress nvarchar(max) NULL,
	CCAddress nvarchar(max) NULL,
	BCCAddress nvarchar(max) NULL,
	TemplateHeaderBodyContent nvarchar(max)  NULL,
	TemplateDetailsBodyContent nvarchar(max)  NULL,
	TemplateFooterBodyContent nvarchar(max)  NULL,
	EmailSignatureID  int NULL foreign key references EmailSignatureTable(EmailSignatureID),
	StatusID int NOT NULL foreign key references StatusTable(StatusID),
	CreatedBy int NOT NULL Foreign key references User_LoginUserTable(UserID),
	CreatedDateTime smalldatetime NOT NULL,
	LastModifiedBy int NULL  Foreign key references User_LoginUserTable(UserID),
	LastModifiedDateTime smalldatetime NULL
	)

	Go 
	Create Table NotificationReportAttachmentTable
	(
	  NotificationReportAttachmentID int not null Primary key identity(1,1),
	  NotificationTemplateID int not null foreign key references NotificationTemplateTable(NotificationTemplateID),
	  AttachmentFormatID int not NULL foreign key references AttachmentFormatTable(AttachmentFormatID),
	  ReportID int not null foreign key references ReportTable(ReportID),
	  SourceField1 nvarchar(50) not null,
	  SourceField2 nvarchar(50) not null,
	  SourceField3 nvarchar(50) not null,
	  DestinationField1 nvarchar(50) not null,
	   DestinationField2 nvarchar(50) not null,
	    DestinationField3 nvarchar(50) not null
	)
	gO 
	CREATE TABLE NotificationTemplateNotificationTypeTable
	(
	  NotificationTemplateNotificationTypeID int not null Primary key identity(1,1),
	   NotificationTemplateID int not null foreign key references NotificationTemplateTable(NotificationTemplateID),
	   NotificationTypeID int not null foreign key references NotificationTypeTable(NotificationTypeID),
	   StatusID int NOT NULL foreign key references StatusTable(StatusID)
	)
	go
	Alter table NotificationTypeTable Add AllowHTMLContent bit default(0) 
Go 

If not exists(select NotificationType from NotificationTypeTable where NotificationType='SMS')
Begin 
   Insert into NotificationTypeTable(NotificationType,NotificationTypeID) Values('SMS',1)
end 
Go 

If not exists(select NotificationType from NotificationTypeTable where NotificationType='Email')
Begin 
   Insert into NotificationTypeTable(NotificationType,NotificationTypeID) Values('Email',2)
end 
Go 
If not exists(select NotificationType from NotificationTypeTable where NotificationType='Whatsapp')
Begin 
   Insert into NotificationTypeTable(NotificationType,NotificationTypeID) Values('Whatsapp',3)
end 
Go 
If not exists(select NotificationType from NotificationTypeTable where NotificationType='Push')
Begin 
   Insert into NotificationTypeTable(NotificationType,NotificationTypeID) Values('Push',4)
end 
Go 
If not exists(select NotificationType from NotificationTypeTable where NotificationType='Bell')
Begin 
   Insert into NotificationTypeTable(NotificationType,NotificationTypeID) Values('Bell',5)
end 
Go 

CREATE TABLE NotificationModuleFieldTable(
NotificationFieldID   int not null Primary key identity(1,1),
NotificationModuleID int not null foreign key references NotificationModuleTable(NotificationModuleID),
FieldName nvarchar(500) Not null,
DisplayName nvarchar(500) Not null,
StatusID int NOT NULL foreign key references StatusTable(StatusID),
	CreatedBy int NOT NULL Foreign key references User_LoginUserTable(UserID),
	CreatedDateTime smalldatetime NOT NULL,
)

CREATE TABLE NotificationModuleFieldTable(
NotificationFieldID   int not null Primary key identity(1,1),
NotificationModuleID int not null foreign key references NotificationModuleTable(NotificationModuleID),
FieldName nvarchar(500) Not null,
DisplayName nvarchar(500) Not null,
StatusID int NOT NULL foreign key references StatusTable(StatusID),
	CreatedBy int NOT NULL Foreign key references User_LoginUserTable(UserID),
	CreatedDateTime smalldatetime NOT NULL,
)
go 
alter VIEW [dbo].[vwReportTemplateObjects]
AS
	SELECT 
		id ObjectID,
		name as ObjectName,
		(case when type='p' then 'Procedure' else 'View' end ) as ObjectType
		FROM sysobjects 
		where type IN('p', 'v') AND(name LIKE 'rvw%' OR name LIKE 'rprc%') and 
			name not in (select QueryString from ReportTemplateTable where StatusID = 50)
GO
Alter table NotificationModuleTable add QueryType nvarchar(max) NULL
go 
Alter table NotificationModuleTable add QueryString nvarchar(max) NULL
go 

Alter table NotificationModuleTable add ReportTemplateID  int NULL Foreign key references ReportTemplateTable(ReportTemplateID)
go 
update user_menutable set TargetObject='/NotificationModule/Index?pageName=NotificationModule' where TargetObject='/MasterPage/Index?pageName=NotificationModule'
Go 
Alter table ScreenFilterTable add ReportTemplateID int  null foreign key references ReportTemplateTable(ReportTemplateID)

select * from dbo.[vwReportTemplateObjects] 

CREATE TABLE NotificationModuleFieldTable(
NotificationFieldID   int not null Primary key identity(1,1),
NotificationModuleID int not null foreign key references NotificationModuleTable(NotificationModuleID),
FieldName nvarchar(500) Not null,
DisplayName nvarchar(500) Not null,
StatusID int NOT NULL foreign key references StatusTable(StatusID),
	CreatedBy int NOT NULL Foreign key references User_LoginUserTable(UserID),
	CreatedDateTime smalldatetime NOT NULL,
)
go 
alter VIEW [dbo].[vwReportTemplateObjects]
AS
	SELECT 
		id ObjectID,
		name as ObjectName,
		(case when type='p' then 'Procedure' else 'View' end ) as ObjectType
		FROM sysobjects 
		where type IN('p', 'v') AND(name LIKE 'rvw%' OR name LIKE 'rprc%') and 
			name not in (select QueryString from ReportTemplateTable where StatusID = 50)
GO
Alter table NotificationModuleTable add QueryType nvarchar(max) NULL
go 
Alter table NotificationModuleTable add QueryString nvarchar(max) NULL
go 
Alter table  NotificationTemplateTable add foreign key(NotificationModuleID) references NotificationModuleTable(NotificationModuleID)
Go 
Alter table  NotificationTemplateTable add foreign key(NotificationTypeID) references NotificationTypeTable(NotificationTypeID)
Go 

Alter table NotificationModuleTable add ReportTemplateID  int NULL Foreign key references ReportTemplateTable(ReportTemplateID)
go 
update user_menutable set TargetObject='/NotificationModule/Index?pageName=NotificationModule' where TargetObject='/MasterPage/Index?pageName=NotificationModule'
Go 
Alter table ScreenFilterTable add ReportTemplateID int not null foreign key references ReportTemplateTable(ReportTemplateID)
go 
CREATE view rvwAssetSummary 
as 
Select * from Assetview 
Go 
--select * from ReportTemplateFieldTable
If not exists(select ReportTemplateCategoryName  from ReportTemplateCategoryTable where ReportTemplateCategoryName='DynamicReportingTesting')
Begin 
   Insert into ReportTemplateCategoryTable(ReportTemplateCategoryName,StatusID,CreatedBy,CreatedDateTime)
   values('DynamicReportingTesting',50,2,getdate())
End                     
Go 
if not exists(select ReportTemplatename from ReportTemplateTable where ReportTemplateName='AssetReport')
Begin 
 insert into ReportTemplateTable(ReportTemplateName,QueryString,StatusID,createdby,CreatedDateTime,ReportTemplateCategoryID,QueryType)
 Select 'AssetReport','rvwAssetSummary',50,2,GETDATE(),ReportTemplateCategoryID,'View'
 FRom ReportTemplateCategoryTable where ReportTemplateCategoryName='DynamicReportingTesting'
end 
Go
Insert into ReportTemplateFieldTable (FieldName,ReportTemplateID,
QueryFieldName,DisplayTitle,FieldDataType,DefaultFormat,DefaultWidth,
Expression,FooterSumRequired,StatusID,CreatedBy,CreatedDateTime,GroupSumRequired)
Select [Column Name],(select ReportTemplateID from ReportTemplateTable where ReportTemplateName='rvwAssetSummary'),[Column Name],[Column Name],
Case when [Data type]='nvarchar' or [Data type]='varchar' then 'System.String' 
     when [Data type]='int' or [Data type]='tinyint' then 'System.Int32' 
	 When [Data type]='smalldatetime' or [Data type]='date' then  'System.DateTime'
	 When [Data type]='bit' then 'System.Boolean'
	 When [Data type]='decimal' then 'System.Decimal' else null end ,
Case when [Data type]='smalldatetime' or [Data type]='date' then '=Parameters!DateFormat.Value' else NULL end , 
'1.000000',NULL,0,50,2,getdate(),0 from (
SELECT
    c.name 'Column Name',
    t.Name 'Data type',
    c.max_length 'Max Length',
    c.precision ,
    c.scale ,
    c.is_nullable,
    ISNULL(i.is_primary_key, 0) 'Primary Key',
ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS Orderno

FROM    
    sys.columns c
INNER JOIN
    sys.types t ON c.user_type_id = t.user_type_id
LEFT OUTER JOIN
    sys.index_columns ic ON ic.object_id = c.object_id AND ic.column_id = c.column_id
LEFT OUTER JOIN
    sys.indexes i ON ic.object_id = i.object_id AND ic.index_id = i.index_id
WHERE
    c.object_id = OBJECT_ID('rvwAssetSummary') and c.name not like '%id%'
	) x 
Go 


insert into ScreenFilterTable(ScreenFilterName,ShowDynamicFields,StatusID,CreateDateTime,
ParentID,ParentType,ReportTemplateID)
Select 'AssetReport',0,50,GETDATE(),1,'ReportTemplate',ReportTemplateID
From ReportTemplateTable where ReportTemplateName='AssetReport'
go 


--Insert into ScreenFilterTypeTable(ScreenFilterTypeCode,ScreenFilterTypeName,ScreenFilterTypeID)
--Values(1,'Sigle Filter',1)
--Go 
--Insert into ScreenFilterTypeTable(ScreenFilterTypeCode,ScreenFilterTypeName,ScreenFilterTypeID)
--Values(2,'Range Filter',2)
--Go 

--exec tmp_Cleantable 'ScreenFilterLineItemTable'
insert into ScreenFilterLineItemTable(
DisplayName,FieldTypeID,
QueryField,StatusID,CreateDateTime,ScreenFilterID,
IsFixedFilter,ScreenFilterTypeID,IsMandatory,OrderNo)

Select [column Name],NULL,[column Name],50,getdate(),(select ScreenFilterID from ScreenFilterTable where ScreenFilterName='AssetReport'),
1,1,0,Orderno from
(
SELECT
    c.name 'Column Name',
    t.Name 'Data type',
    c.max_length 'Max Length',
    c.precision ,
    c.scale ,
    c.is_nullable,
    ISNULL(i.is_primary_key, 0) 'Primary Key',
ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS Orderno

FROM    
    sys.columns c
INNER JOIN
    sys.types t ON c.user_type_id = t.user_type_id
LEFT OUTER JOIN
    sys.index_columns ic ON ic.object_id = c.object_id AND ic.column_id = c.column_id
LEFT OUTER JOIN
    sys.indexes i ON ic.object_id = i.object_id AND ic.index_id = i.index_id
WHERE
    c.object_id = OBJECT_ID('rvwAssetSummary') and c.name not like '%id%'
	) x 
	go 
	if not exists(select ScreenFilterTypeName from ScreenFilterTypeTable where ScreenFilterTypeName='Sigle Filter')
begin 
Insert into ScreenFilterTypeTable(ScreenFilterTypeCode,ScreenFilterTypeName)
Values(1,'Sigle Filter')
end 
Go 
if not exists(select ScreenFilterTypeName from ScreenFilterTypeTable where ScreenFilterTypeName='Range Filter')
begin 
Insert into ScreenFilterTypeTable(ScreenFilterTypeCode,ScreenFilterTypeName)
Values(1,'Range Filter')
end 
Go 
if not exists(select AttachmentFormat from AttachmentFormatTable where AttachmentFormat='PDF')
begin 
  Insert into AttachmentFormatTable(AttachmentFormatID,AttachmentFormatCode,AttachmentFormat)
  Values(1,'PDF','PDF')
End 
	--select * from ScreenFilterTypeTable

	--Select * from NotificationModuleTable
	--select * from NotificationModuleFieldTable

	--exec tmp_CleanTable 'NotificationModuleFieldTable'
	--go 
	--exec tmp_CleanTable 'NotificationModuleTable'
	--Go 
	--exec tmp_CleanTable 'NotificationTemplateNotificationTypeTable'
	--Go
	--exec tmp_CleanTable 'NotificationTemplateTable'
	--Go 

	--select * from vwReportTemplateObjects
	If not exists(select PaperSizeOrientation  from ReportPaperSizeTable where PaperSizeOrientation='A4 - Portrait')
Begin
   insert into ReportPaperSizeTable(PaperSizeOrientation,PageWidth,PageHeight)
   Values('A4 - Portrait',8.270000,11.690000)
End 
Go 
If not exists(select PaperSizeOrientation  from ReportPaperSizeTable where PaperSizeOrientation='A4 - Landscape')
Begin
   insert into ReportPaperSizeTable(PaperSizeOrientation,PageWidth,PageHeight)
   Values('A4 - Landscape',11.690000,8.270000)
End 
Go
insert into ReportTable(ReportTemplateID,ReportPaperSizeID,ReportName,ReportTitle,ReportPageHeight,ReportPageWidth,ReportLeftMargin,
ReportRightMargin,ReportTopMargin,ReportBottomMargin,RDLFileName,StatusID,CreatedBy,CreatedDateTime)
Select ReportTemplateID, (Select ReportPaperSizeID from ReportPaperSizeTable where PaperSizeOrientation='A4 - Portrait'),'AssetReport','AssetReport',
'0.000000','0.000000','1.000000','1.000000','1.000000','1.000000',NULL,50,2,getdate()
From ReportTemplateTable where ReportTemplateName='AssetReport'



--12-01-2024 NotificationTemplate update scripts



IF NOT EXISTS(SELECT * FROM sys.all_columns where name = 'TemplateCode' and object_id = OBJECT_ID('NotificationTemplateTable'))
BEGIN
	alter table NotificationTemplateTable Add TemplateCode varchar(100) 
END


IF NOT EXISTS(SELECT AttachmentFormatCode FROM AttachmentFormatTable WHERE AttachmentFormatCode='Excel')
BEGIN
INSERT into AttachmentFormatTable(AttachmentFormatID,AttachmentFormatCode,AttachmentFormat)Values(2,'Excel','Excel')
END

IF NOT EXISTS(SELECT * FROM sys.all_columns where name = 'IsAttachmentRequired' and object_id = OBJECT_ID('NotificationTypeTable'))
BEGIN
	alter table NotificationTypeTable Add IsAttachmentRequired bit default 0
END

IF OBJECT_ID('NotificationTemplateFieldTable') IS NULL
BEGIN
CREATE TABLE dbo.NotificationTemplateFieldTable(
	NotificationTemplateFieldID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	NotificationTemplateID int NOT NULL REFERENCES NotificationTemplateTable (NotificationTemplateID),
	NotificationFieldID int NOT NULL REFERENCES NotificationModuleFieldTable (NotificationFieldID),
	StatusID int NOT NULL REFERENCES StatusTable (StatusID)
	)
End
go 


IF OBJECT_ID('NotificationInputTable') IS NULL
BEGIN
	CREATE TABLE NotificationInputTable(
		NotificationInputID		INT IDENTITY(1, 1) PRIMARY KEY,

		NotificationTemplateID	INT NOT NULL REFERENCES NotificationTemplateTable,

		SYSDataID1				BIGINT NOT NULL, 
		SYSDataID2				BIGINT , 
		SYSDataID3				BIGINT , 

		IsCompleted				BIT NOT NULL DEFAULT 0,
		CreatedDateTime			smalldatetime NOT NULL DEFAULT GetDate(),
		CompletedDateTime		smalldatetime 
	)
END
GO


DROP VIEW IF EXISTS vwSystemTemplateObjects
GO

CREATE VIEW [vwSystemTemplateObjects]
AS
	SELECT 
		id ObjectID,
		name as ObjectName,
		(case when type='p' then 'Procedure' else 'View' end ) as ObjectType
		FROM sysobjects 
		where type IN('p', 'v') 
			AND(name LIKE 'rvw%' OR name LIKE 'rprc%' OR name LIKE 'nvw%' OR name LIKE 'nprc%') 
			--AND name not in (select QueryString from ReportTemplateTable where StatusID = 50)
GO



ALTER VIEW [dbo].[vwReportTemplateObjects]
AS
	SELECT * 
		FROM  [vwSystemTemplateObjects]
		--WHERE
		--	(ObjectName LIKE 'rvw%' OR ObjectName LIKE 'rprc%') and 
		--	ObjectName not in (select QueryString from ReportTemplateTable where StatusID = 50)
GO

DROP PROC IF EXISTS [aprc_GetPushNotifications_Update]
GO

DROP PROC IF EXISTS [aprc_GetNotifications_Update]
GO

CREATE PROC [dbo].[aprc_GetNotifications_Update]
	@NotificationDataID		INT,
	@Response				VARCHAR(MAX)
AS
BEGIN
	UPDATE [NotificationDataTable]
		SET IsDelivered = 1,
			FilePath = SUBSTRING(@Response, 1, 500)
		WHERE NotificationDataID = @NotificationDataID;
END

DROP PROC IF EXISTS [aprc_GetPushNotifications]
GO

DROP PROC IF EXISTS [aprc_GetNotifications]
GO

CREATE PROC [dbo].[aprc_GetNotifications]
AS
BEGIN
	SELECT * FROM [NotificationDataTable]
		WHERE IsDelivered = 0 AND CreationDatedatetime > GetDate() - 0.5
END
GO

DROP PROC IF EXISTS [aprc_GetNotificationInputs]
GO

CREATE PROC [dbo].[aprc_GetNotificationInputs]
AS
BEGIN
	SELECT DISTINCT A.NotificationInputID, A.SYSDataID1, A.SYSDataID2, A.SYSDataID3, 
			M.QueryType, M.QueryString, T.*
		FROM NotificationInputTable A
		LEFT JOIN NotificationTemplateTable T ON T.NotificationTemplateID = A.NotificationTemplateID
		LEFT JOIN NotificationModuleTable M ON M.NotificationModuleID = T.NotificationModuleID
		WHERE A.IsCompleted = 0
END
GO


DROP PROC IF EXISTS [aprc_GetNotificationInputs_Update]
GO

CREATE PROC [dbo].[aprc_GetNotificationInputs_Update]
	@NotificationInputID	INT,
	@Response				VARCHAR(MAX)
AS
BEGIN
	UPDATE NotificationInputTable
		SET IsCompleted = 1,
			CompletedDateTime = GetDate()
		WHERE NotificationInputID = @NotificationInputID;
END
GO

DROP PROC IF EXISTS [prc_InsertNotificationData]
GO

CREATE PROC [dbo].[prc_InsertNotificationData](
	@ToAddress	nvarchar(500),
	@CCAddress	nvarchar(500) = NULL,
	@BCCAddress	nvarchar(500) = NULL,
	@Content	TEXT,
	@Subject	nvarchar(500),
	@NotificationTypeID	int
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	INSERT INTO [NotificationDataTable](ToAddress, CCAddress, BCCAddress, Content, Subject, NotificationTypeID)
		VALUES(@ToAddress, @CCAddress, @BCCAddress, @Content, @Subject, @NotificationTypeID)
END
GO

--SELECT * FROM NotificationTypeTable 
DROP FUNCTION IF EXISTS fn_GetServerURL
GO

CREATE FUNCTION fn_GetServerURL()
RETURNS VARCHAR(1000)
AS
BEGIN
	RETURN 'http://13.71.105.174:8107';
END
GO


ALTER PROC [dbo].[aprc_GetNotificationInputs]
AS
BEGIN
	SELECT DISTINCT A.NotificationInputID, A.SYSDataID1, A.SYSDataID2, A.SYSDataID3, 
			M.QueryType, M.QueryString, T.*,TT.IsAttachmentRequired,TT.AllowHtmlContent,X.TemplateTableBodyContentHeader,X.TemplateTableBodyContent,X.TemplateTableBodyContentFooter
		FROM NotificationInputTable A
		LEFT JOIN NotificationTemplateTable T ON T.NotificationTemplateID = A.NotificationTemplateID
		LEFT JOIN NotificationModuleTable M ON M.NotificationModuleID = T.NotificationModuleID
		LEFT JOIN NotificationTypeTable TT ON TT.NotificationTypeID = T.NotificationTypeID
		LEFT JOIN (Select A.NotificationTemplateID,'<table style="border: 1px solid black;border-collapse: collapse;"><tbody>'
		+'<tr>'+STRING_AGG('<td style="width:100px;border: 1px solid black;border-collapse: collapse;">'+(B.FieldName)+'</td>','')+'</tr>' as TemplateTableBodyContentHeader,
		'<tr>'+STRING_AGG('<td style="width:100px;border: 1px solid black;border-collapse: collapse;">##'+UPPER(B.FieldName)+'##</td>','')+'</tr>' as TemplateTableBodyContent,
		'</tbody></table>' as TemplateTableBodyContentFooter
		from NotificationTemplateFieldTable A
		Left Join NotificationModuleFieldTable B on A.NotificationFieldID=B.NotificationFieldID
		WHERE A.StatusID=5 AND B.StatusID=5
		Group by  A.NotificationTemplateID
		)X ON X.NotificationTemplateID=T.NotificationTemplateID

		WHERE A.IsCompleted = 0
END
GO



IF (object_id('NotificationDataTable', 'U') is  null) 
begin
Create Table NotificationDataTable
(
NotificationDataID int Primary Key IDENTITY(1,1) NOT NULL,
ToAddress nvarchar(200) NOT null,
BCCAddress nvarchar(200),
CCAddress nvarchar(200),
Content nvarchar(max) NOT null,
Subject nvarchar(500),
NotificationTypeID int NOT NULL  CONSTRAINT FK_NotificationDataTablee_NotificationTypeID FOREIGN KEY(NotificationTypeID)
REFERENCES dbo.NotificationTypeTable (NotificationTypeID),
FilePath nvarchar(max),
ReferenceID int,
IsDelivered bit not null default 0,
StatusID int not null Default 5 CONSTRAINT FK_NotificationDataTablee_StatusID FOREIGN KEY(StatusID)
REFERENCES dbo.StatusTable (StatusID),
CreatedBy int not null default 1  CONSTRAINT FK_NotificationDataTablee_CreatedBy FOREIGN KEY(CreatedBy)
REFERENCES dbo.User_LoginUserTable (UserID),
CreationDatedatetime smalldatetime NOT NULL default getDate()
)
End 
go 



ALTER TABLE NotificationTemplateTable ALTER COLUMN TemplateCode VARCHAR (100) NOT NULL ;


ALTER TABLE NotificationTemplateTable
ADD CONSTRAINT UC_NotificationTemplateTable_TemplateCode UNIQUE (TemplateCode);
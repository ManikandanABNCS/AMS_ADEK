CREATE TABLE [dbo].[ImportTemplateNewTable](
	[ImportTemplateID] [int] IDENTITY(1,1) NOT NULL,
	[ImportField] [nvarchar](500) NOT NULL,
	[DispalyOrderID] [int] NOT NULL,
	[IsDisplay] [bit] NOT NULL,
	[IsMandatory] [bit] NOT NULL,
	[DataSize] [int] NOT NULL,
	[DataFormat] [nvarchar](50) NULL,
	[IsForeignKey] [bit] NOT NULL,
	[Width] [int] NOT NULL,
	[ImportTemplateTypeID] [int] NOT NULL,
	[IsUnique] [bit] NOT NULL,
	[ReferenceEntityID] [int] NULL,
	[EntityID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ImportTemplateID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ImportTemplateNewTable] ADD  DEFAULT ((1)) FOR [IsDisplay]
GO
ALTER TABLE [dbo].[ImportTemplateNewTable] ADD  DEFAULT ((0)) FOR [IsMandatory]
GO
ALTER TABLE [dbo].[ImportTemplateNewTable] ADD  DEFAULT ((0)) FOR [IsForeignKey]
GO
ALTER TABLE [dbo].[ImportTemplateNewTable] ADD  DEFAULT ((0)) FOR [IsUnique]
GO
ALTER TABLE [dbo].[ImportTemplateNewTable]  WITH CHECK ADD FOREIGN KEY([EntityID])
REFERENCES [dbo].[EntityTable] ([EntityID])
GO
ALTER TABLE [dbo].[ImportTemplateNewTable]  WITH CHECK ADD FOREIGN KEY([ImportTemplateTypeID])
REFERENCES [dbo].[ImportTemplateTypeTable] ([ImportTemplateTypeID])
GO
insert into EntityTable(EntityName)
select EntityName
from [AMS_Client].dbo.EntityTable
go
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,
DataSize,DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select ImportField,DispalyOrderID,IsDisplay,IsMandatory,
DataSize,DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID
from [AMS_Client].dbo.ImportTemplateTable
go 



CREATE TABLE [dbo].[ImportFormatLineItemNewTable](
	[ImportFormatLineItemID] [int] IDENTITY(1,1) NOT NULL,
	[ImportFormatID] [int] NOT NULL,
	[ImportTemplateID] [int] NOT NULL,
	[DisplayOrderID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ImportFormatLineItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ImportFormatNewTable]    Script Date: 2/29/2024 11:14:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImportFormatNewTable](
	[ImportFormatID] [int] IDENTITY(1,1) NOT NULL,
	[TamplateName] [nvarchar](500) NOT NULL,
	[FormatPath] [nvarchar](max) NULL,
	[FormatExtension] [nvarchar](50) NOT NULL,
	[ImportTemplateTypeID] [int] NOT NULL,
	[StatusID] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDateTime] [smalldatetime] NOT NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDateTime] [smalldatetime] NULL,
	[EntityID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ImportFormatID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[ImportFormatLineItemNewTable]  WITH CHECK ADD FOREIGN KEY([ImportFormatID])
REFERENCES [dbo].[ImportFormatNewTable] ([ImportFormatID])
GO
ALTER TABLE [dbo].[ImportFormatLineItemNewTable]  WITH CHECK ADD FOREIGN KEY([ImportTemplateID])
REFERENCES [dbo].[ImportTemplateNewTable] ([ImportTemplateID])
GO
ALTER TABLE [dbo].[ImportFormatNewTable]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[PersonTable] ([PersonID])
GO
ALTER TABLE [dbo].[ImportFormatNewTable]  WITH CHECK ADD FOREIGN KEY([EntityID])
REFERENCES [dbo].[EntityTable] ([EntityID])
GO
ALTER TABLE [dbo].[ImportFormatNewTable]  WITH CHECK ADD FOREIGN KEY([ImportTemplateTypeID])
REFERENCES [dbo].[ImportTemplateTypeTable] ([ImportTemplateTypeID])
GO
ALTER TABLE [dbo].[ImportFormatNewTable]  WITH CHECK ADD FOREIGN KEY([LastModifiedBy])
REFERENCES [dbo].[PersonTable] ([PersonID])
GO
ALTER TABLE [dbo].[ImportFormatNewTable]  WITH CHECK ADD FOREIGN KEY([StatusID])
REFERENCES [dbo].[StatusTable] ([StatusID])
GO


select * from MasterGridNewTable

update MasterGridNewTable set EntityName='ImportFormatNewTable' where EntityName='ImportFormatTable' 
go 
if not exists(select RightName  from User_RightTable where RightName='ExcelImport')
begin 
 insert into User_RightTable (
RightName,
RightDescription,
ValueType,
DisplayRight,
RightGroupID,
IsActive,
IsDeleted)
select 'ExcelImport','ExcelImport',95,1,RightGroupID,1,0
from User_RightGroupTable where RightGroupDesc='Others'
end 
go 
if not exists(select menuname from User_MenuTable where MenuName='ExcelImport')
Begin
  insert into User_MenuTable(MenuName,RightID,TargetObject,ParentMenuID,
OrderNo,ParentTransactionID)
select 'ExcelImport',RightID,'/ExcelImport/Import',6,2,1
from User_RightTable where RightName='ExcelImport'

End 
go
Create PROC [dbo].[IPRC_ImportAssetCondition]
(
	@AssetConditionCode nvarchar(100)=null,
	@AssetConditionName nvarchar(100),
	@LanguageID int,
	@ImportTypeID int,
	@UserID int=1,
	@CompanyID int ,
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
	@Attribute16 nvarchar(100)=null
)
AS
BEGIN
	Declare @AssetConditionID int=null;
	DECLARE @ErrorToBeReturned nvarchar(max);
	DECLARE @ConfigValue nvarchar(max);
	set @AssetConditionCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@AssetConditionCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
	set @AssetConditionName=REPLACE(@AssetConditionName,'''','''')
		if(@ImportTypeID=1)
		BEGIN
		if NOT EXISTS (SELECT * FROM AssetConditionTable where AssetConditionCode=@AssetConditionCode and StatusID=1)
			BEGIN
			Select @ConfigValue=ConfiguarationValue from configurationTable where ConfiguarationName='MasterCodeAutoGenerate'
			IF(@ConfigValue='false')
			BEGIN
			insert into AssetConditionTable(AssetConditionCode,StatusID,CreatedBy,CreatedDateTime,
			Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6,Attribute7,Attribute8,Attribute9,Attribute10,Attribute11,Attribute12,
			Attribute13,Attribute14,Attribute15,Attribute16,AssetConditionName) values(@AssetConditionCode,1,@UserID,getDate(),@Attribute1,@Attribute2,@Attribute3,@Attribute4,@Attribute5,
			@Attribute6,@Attribute7,@Attribute8,@Attribute9,@Attribute10,@Attribute11,@Attribute12,
			@Attribute13,@Attribute14,@Attribute15,@Attribute16,@AssetConditionName)

			--Insert Asset Condition Description
				Insert into AssetConditionDescriptionTable(AssetConditionID,AssetConditionDescription,LanguageID,CreatedBy,CreatedDateTime)
				Select C.AssetConditionID,@AssetConditionName,L.LanguageID,C.CreatedBy,C.CreatedDateTime From AssetConditionTable C join 
						LanguageTable L on 1=1 left join AssetConditionDescriptionTable MD on MD.AssetConditionID=C.AssetConditionID and MD.languageid=L.languageID 
						where MD.LanguageID is null	and C.AssetConditionCode=@AssetConditionCode
			END
			ELSE
			BEGIN
			
			insert into AssetConditionTable(AssetConditionCode,StatusID,CreatedBy,CreatedDateTime,
			Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6,Attribute7,Attribute8,Attribute9,Attribute10,Attribute11,Attribute12,
			Attribute13,Attribute14,Attribute15,Attribute16,AssetConditionName) values('-',1,@UserID,getDate(),@Attribute1,@Attribute2,@Attribute3,@Attribute4,@Attribute5,
			@Attribute6,@Attribute7,@Attribute8,@Attribute9,@Attribute10,@Attribute11,@Attribute12,
			@Attribute13,@Attribute14,@Attribute15,@Attribute16,@AssetConditionName)

			set @AssetConditionID=@@IDENTITY;
			--Insert Asset Condition Description
				Insert into AssetConditionDescriptionTable(AssetConditionID,AssetConditionDescription,LanguageID,CreatedBy,CreatedDateTime)
				Select C.AssetConditionID,AssetConditionName,L.LanguageID,C.CreatedBy,C.CreatedDateTime From AssetConditionTable C join 
						LanguageTable L on 1=1 left join AssetConditionDescriptionTable MD on MD.AssetConditionID=C.AssetConditionID and MD.languageid=L.languageID 
						where MD.LanguageID is null	and C.AssetConditionID=@AssetConditionID
			END
			End
			Else
				   SET @ErrorToBeReturned = @AssetConditionCode+'- already Exists;'


		End
			ELSE if(@ImportTypeID=2)
		BEGIN
			UPDATE A SET 
					AssetConditionDescription = @AssetConditionName, 
					LastModifiedDateTime = GETDATE(), 
					LastModifiedBy = @UserID
					from AssetConditionDescriptionTable A join AssetConditionTable B on A.AssetConditionID=B.AssetConditionID
				WHERE B.AssetConditionCode = @AssetConditionCode

				Update AssetConditionTable set Attribute1=@Attribute1,Attribute2=@Attribute2,Attribute3=@Attribute3,Attribute4=@Attribute4,Attribute5=@Attribute5,
				Attribute6=@Attribute6,Attribute7=@Attribute7,Attribute8=@Attribute8,Attribute9=@Attribute9,Attribute10=@Attribute10,Attribute11=@Attribute11,
				Attribute12=@Attribute12,Attribute13=@Attribute13,Attribute14=@Attribute14,Attribute15=@Attribute15,Attribute16=@Attribute16,AssetConditionName=@AssetConditionName where AssetConditioncode=@AssetConditioncode
		END
	SELECT @ErrorToBeReturned ErrorToBeReturned;
END  
Go 
Alter table EntityTable add QueryString nvarchar(100) NULL
go
Update EntityTable set QueryString='IPRC_ExceImportAssetCondition' where EntityName='AssetCondition'
go 
update ImportTemplateNewTable set ImportField='AssetConditionDescription' where ImportField='AssetConditionName'
go 

insert into EntityTable(EntityName,QueryString)
select DynamicColumnRequiredName,ImportProcedure
From DynamicColumnRequiredEntityTable where DynamicColumnRequiredName not in ('AssetCondition') and DynamicColumnRequiredEntityID<=15
go 

--update EntityTable set EntityName='Company' where EntityID=17
--go 

update EntityTable set EntityName='Company',EntityID=17,QueryString='IPRC_ExceImportCompany' where EntityID=10
go 
update EntityTable set EntityName='Asset',EntityID=15,QueryString='IPRC_ExceImportBulkAssets' where EntityID=14
go 
update EntityTable set EntityName='Manufacturer',EntityID=23,QueryString='IPRC_ExceImportManufacturer' where EntityID=11
go 
update EntityTable set EntityName='Model',EntityID=25,QueryString='IPRC_ExceImportModel' where EntityID=12
go
update EntityTable set EntityName='Designation',EntityID=29 where EntityID=13
go


insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,(Select EntityId from EntityTable where EntityName='Category')
From ImportTemplateTable where DynamicColumnRequiredEntityID=1
go 

insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,(Select EntityId from EntityTable where EntityName='Location')
From ImportTemplateTable where DynamicColumnRequiredEntityID=2
go 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,(Select EntityId from EntityTable where EntityName='Department')
From ImportTemplateTable where DynamicColumnRequiredEntityID=4
go 

insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,(Select EntityId from EntityTable where EntityName='TransferType')
From ImportTemplateTable where DynamicColumnRequiredEntityID=5
go 

insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,(Select EntityId from EntityTable where EntityName='Custodian')
From ImportTemplateTable where DynamicColumnRequiredEntityID=6
go 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,(Select EntityId from EntityTable where EntityName='Section')
From ImportTemplateTable where DynamicColumnRequiredEntityID=7
go 

insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,(Select EntityId from EntityTable where EntityName='Supplier')
From ImportTemplateTable where DynamicColumnRequiredEntityID=8
go 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,(Select EntityId from EntityTable where EntityName='Product')
From ImportTemplateTable where DynamicColumnRequiredEntityID=9
go 

--insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
--DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
--select ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
--DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,(Select EntityId from EntityTable where EntityName='Product')
--From ImportTemplateTable where DynamicColumnRequiredEntityID=9
--go 

insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,(Select EntityId from EntityTable where EntityName='Company')
From ImportTemplateTable where DynamicColumnRequiredEntityID=17
go 

go 


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
	@Attribute16 nvarchar(100)=null
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
					INSERT INTO CategoryTable(CategoryCode,StatusID, CreatedBy,CategoryName)
						VALUES(@ParentCategoryCode, 1, @UserID,@ParentCategoryCode)
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
			Attribute13,Attribute14,Attribute15,Attribute16,CategoryName) values(@CategoryCode,@parentCategoryID,1,@UserID,getDate(),@Attribute1,@Attribute2,@Attribute3,@Attribute4,@Attribute5,
			@Attribute6,@Attribute7,@Attribute8,@Attribute9,@Attribute10,@Attribute11,@Attribute12,
			@Attribute13,@Attribute14,@Attribute15,@Attribute16,@CategoryDescription)

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
			Attribute13,Attribute14,Attribute15,Attribute16,CategoryName) values('-',@parentCategoryID,1,@UserID,getDate(),@Attribute1,@Attribute2,@Attribute3,@Attribute4,@Attribute5,
			@Attribute6,@Attribute7,@Attribute8,@Attribute9,@Attribute10,@Attribute11,@Attribute12,
			@Attribute13,@Attribute14,@Attribute15,@Attribute16,@CategoryDescription)

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

	
ALTER PROC [dbo].[IPRC_ExceImportAssetCondition]
(
	@AssetConditionCode nvarchar(100)=null,
	@AssetConditionDescription nvarchar(100),
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
	@Attribute16 nvarchar(100)=null
)
AS
BEGIN
	Declare @AssetConditionID int=null;
	DECLARE @ErrorToBeReturned nvarchar(max);
	DECLARE @ConfigValue nvarchar(max);
	set @AssetConditionCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@AssetConditionCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
	set @AssetConditionDescription=REPLACE(@AssetConditionDescription,'''','''')
		if(@ImportTypeID=1)
		BEGIN
		if NOT EXISTS (SELECT * FROM AssetConditionTable where AssetConditionCode=@AssetConditionCode and StatusID=1)
			BEGIN
			Select @ConfigValue=ConfiguarationValue from configurationTable where ConfiguarationName='MasterCodeAutoGenerate'
			IF(@ConfigValue='false')
			BEGIN
			insert into AssetConditionTable(AssetConditionCode,StatusID,CreatedBy,CreatedDateTime,
			Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6,Attribute7,Attribute8,Attribute9,Attribute10,Attribute11,Attribute12,
			Attribute13,Attribute14,Attribute15,Attribute16,AssetConditionName) values(@AssetConditionCode,1,@UserID,getDate(),@Attribute1,@Attribute2,@Attribute3,@Attribute4,@Attribute5,
			@Attribute6,@Attribute7,@Attribute8,@Attribute9,@Attribute10,@Attribute11,@Attribute12,
			@Attribute13,@Attribute14,@Attribute15,@Attribute16,@AssetConditionDescription)

			--Insert Asset Condition Description
				Insert into AssetConditionDescriptionTable(AssetConditionID,AssetConditionDescription,LanguageID,CreatedBy,CreatedDateTime)
				Select C.AssetConditionID,@AssetConditionDescription,L.LanguageID,C.CreatedBy,C.CreatedDateTime From AssetConditionTable C join 
						LanguageTable L on 1=1 left join AssetConditionDescriptionTable MD on MD.AssetConditionID=C.AssetConditionID and MD.languageid=L.languageID 
						where MD.LanguageID is null	and C.AssetConditionCode=@AssetConditionCode
			END
			ELSE
			BEGIN
			
			insert into AssetConditionTable(AssetConditionCode,StatusID,CreatedBy,CreatedDateTime,
			Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6,Attribute7,Attribute8,Attribute9,Attribute10,Attribute11,Attribute12,
			Attribute13,Attribute14,Attribute15,Attribute16,AssetConditionName) values('-',1,@UserID,getDate(),@Attribute1,@Attribute2,@Attribute3,@Attribute4,@Attribute5,
			@Attribute6,@Attribute7,@Attribute8,@Attribute9,@Attribute10,@Attribute11,@Attribute12,
			@Attribute13,@Attribute14,@Attribute15,@Attribute16,@AssetConditionDescription)

			set @AssetConditionID=@@IDENTITY;
			--Insert Asset Condition Description
				Insert into AssetConditionDescriptionTable(AssetConditionID,AssetConditionDescription,LanguageID,CreatedBy,CreatedDateTime)
				Select C.AssetConditionID,@AssetConditionDescription,L.LanguageID,C.CreatedBy,C.CreatedDateTime From AssetConditionTable C join 
						LanguageTable L on 1=1 left join AssetConditionDescriptionTable MD on MD.AssetConditionID=C.AssetConditionID and MD.languageid=L.languageID 
						where MD.LanguageID is null	and C.AssetConditionID=@AssetConditionID
			END
			End
			Else
				   SET @ErrorToBeReturned = @AssetConditionCode+'- already Exists;'


		End
			ELSE if(@ImportTypeID=2)
		BEGIN
			UPDATE A SET 
					AssetConditionDescription = @AssetConditionDescription, 
					LastModifiedDateTime = GETDATE(), 
					LastModifiedBy = @UserID
					from AssetConditionDescriptionTable A join AssetConditionTable B on A.AssetConditionID=B.AssetConditionID
				WHERE B.AssetConditionCode = @AssetConditionCode

				Update AssetConditionTable set Attribute1=@Attribute1,Attribute2=@Attribute2,Attribute3=@Attribute3,Attribute4=@Attribute4,Attribute5=@Attribute5,
				Attribute6=@Attribute6,Attribute7=@Attribute7,Attribute8=@Attribute8,Attribute9=@Attribute9,Attribute10=@Attribute10,Attribute11=@Attribute11,
				Attribute12=@Attribute12,Attribute13=@Attribute13,Attribute14=@Attribute14,Attribute15=@Attribute15,Attribute16=@Attribute16,
				AssetConditionName=@AssetConditionDescription
				where AssetConditioncode=@AssetConditioncode
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
	@Attribute16 nvarchar(100)=null
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
			Attribute13,Attribute14,Attribute15,Attribute16,ProductName) values(@Productcode,@CategoryID,1,@UserID,getDate(),0,0,@Attribute1,@Attribute2,@Attribute3,@Attribute4,@Attribute5,
			@Attribute6,@Attribute7,@Attribute8,@Attribute9,@Attribute10,@Attribute11,@Attribute12,
			@Attribute13,@Attribute14,@Attribute15,@Attribute16,@ProductDescription)

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
			Attribute13,Attribute14,Attribute15,Attribute16,ProductName) values('-',@CategoryID,1,@UserID,getDate(),0,0,@Attribute1,@Attribute2,@Attribute3,@Attribute4,@Attribute5,
			@Attribute6,@Attribute7,@Attribute8,@Attribute9,@Attribute10,@Attribute11,@Attribute12,
			@Attribute13,@Attribute14,@Attribute15,@Attribute16,@ProductDescription)

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
				,ProductName=@ProductDescription
				where ProductCode=@ProductCode
		END
	SELECT @ErrorToBeReturned ErrorToBeReturned;
END  
go 


ALTER PROC [dbo].[IPRC_ExceImportLocation]
(
	@LocationCode		nvarchar(50)=null,
	@LocationDescription nvarchar(200),
	@ParentLocationCode	nvarchar(50)=null,
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
	@Attribute16 nvarchar(100)=null
)
AS
BEGIN

	DECLARE @ErrorToBeReturned nvarchar(max);
	Declare @parentLocationID int =null;
	Declare @LocationID int=null;
	DECLARE @ConfigValue nvarchar(max),@CompanyID int 
	Declare @ImportExcelNotAllowCreateReferenceFieldNewEntry nvarchar(10)

	Select @ImportExcelNotAllowCreateReferenceFieldNewEntry=ConfiguarationValue from configurationTable where ConfiguarationName='ImportExcelNotAllowCreateReferenceFieldNewEntry'
	set @LocationCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@LocationCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
    Select Top 1 @CompanyID=CompanyID from CompanyTable

	set @LocationDescription=REPLACE(@LocationDescription,'''','''')
		if(@ImportTypeID=1)
		BEGIN

		if(@ParentLocationCode is not null and @ParentLocationCode!='')
			BEGIN			
				--Insert parent Location
				if(@ImportExcelNotAllowCreateReferenceFieldNewEntry='false')
				Begin
				IF NOT EXISTS (SELECT * FROM LocationTable where LocationCode=@ParentLocationCode and StatusID=1)
				BEGIN
					INSERT INTO LocationTable(LocationCode,StatusID, CreatedBy,LocationName,CompanyID)
						VALUES(@ParentLocationCode, 1, @UserID,@ParentLocationCode,@CompanyID)
						set @parentLocationID=@@IDENTITY;

						Insert into LocationDescriptionTable(LocationID,LocationDescription,LanguageID,CreatedBy,CreatedDateTime)
						Select C.LocationID,c.LocationCode,L.LanguageID,C.CreatedBy,C.CreatedDateTime From LocationTable C join 
								LanguageTable L on 1=1 left join LocationDescriptionTable MD on MD.LocationID=C.LocationID and MD.languageid=L.languageID 
								where MD.LanguageID is null
								 and C.LocationCode=@ParentLocationCode
				END
				ELSE
				BEGIN
				   select @parentLocationID=LocationID from LocationTable where LocationCode=@ParentLocationCode  and StatusID=1
				   if(@ParentLocationCode is not null and @parentLocationID is null)

				   return @ParentLocationCode+'- Parent Location Is not available;'
				END
				END
				Else
				if(@ParentLocationCode is not null and @ParentLocationCode!='')
				BEGIN
				  return @ParentLocationCode+'- Parent Location Is not available;'
				  END
			END
			

		if NOT EXISTS (SELECT * FROM LocationTable WHERE LocationCode = @LocationCode AND ParentLocationID=@parentLocationID  and StatusID=1)
				BEGIN
			Select @ConfigValue=ConfiguarationValue from configurationTable where ConfiguarationName='MasterCodeAutoGenerate'
			IF(@ConfigValue='false')
			BEGIN
				INSERT INTO LocationTable(LocationCode,ParentLocationID,StatusID, CreatedBy,CreatedDateTime,
			Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6,Attribute7,Attribute8,Attribute9,Attribute10,Attribute11,Attribute12,
			Attribute13,Attribute14,Attribute15,Attribute16,LocationName,CompanyID) values(@LocationCode,@parentLocationID,1,@UserID,getDate(),@Attribute1,@Attribute2,@Attribute3,@Attribute4,@Attribute5,
			@Attribute6,@Attribute7,@Attribute8,@Attribute9,@Attribute10,@Attribute11,@Attribute12,
			@Attribute13,@Attribute14,@Attribute15,@Attribute16,@LocationDescription,@CompanyID)

				--Insert Location Description
					Insert into LocationDescriptionTable(LocationID,LocationDescription,LanguageID,CreatedBy,CreatedDateTime)
						Select C.LocationID,@LocationDescription,L.LanguageID,C.CreatedBy,C.CreatedDateTime From LocationTable C join 
						LanguageTable L on 1=1 left join LocationDescriptionTable MD on MD.LocationID=C.LocationID and MD.languageid=L.languageID 
						where MD.LanguageID is null
						and C.LocationCode=@LocationCode
			END
			ELSE
			BEGIN
			
		INSERT INTO LocationTable(LocationCode,ParentLocationID,StatusID, CreatedBy,CreatedDateTime,
			Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6,Attribute7,Attribute8,Attribute9,Attribute10,Attribute11,Attribute12,
			Attribute13,Attribute14,Attribute15,Attribute16,CompanyID,LocationName) values('-',@parentLocationID,1,@UserID,getDate(),@Attribute1,@Attribute2,@Attribute3,@Attribute4,@Attribute5,
			@Attribute6,@Attribute7,@Attribute8,@Attribute9,@Attribute10,@Attribute11,@Attribute12,
			@Attribute13,@Attribute14,@Attribute15,@Attribute16,@CompanyID,@LocationDescription)

			set @LocationID=@@IDENTITY;
		--Insert Department Description
					Insert into LocationDescriptionTable(LocationID,LocationDescription,LanguageID,CreatedBy,CreatedDateTime)
						Select C.LocationID,@LocationDescription,L.LanguageID,C.CreatedBy,C.CreatedDateTime From LocationTable C join 
						LanguageTable L on 1=1 left join LocationDescriptionTable MD on MD.LocationID=C.LocationID and MD.languageid=L.languageID 
						where MD.LanguageID is null
						and C.LocationID=@LocationID
			END
			End
			Else
				   SET @ErrorToBeReturned = @LocationCode+'- already Exists;'


		End
			ELSE if(@ImportTypeID=2)
		BEGIN		
				UPDATE A SET 
					LocationDescription = @LocationDescription					
					from LocationDescriptionTable A join LocationTable B on A.LocationID=B.LocationID
				WHERE B.LocationCode = @LocationCode

				Update LocationTable set Attribute1=@Attribute1,Attribute2=@Attribute2,Attribute3=@Attribute3,Attribute4=@Attribute4,Attribute5=@Attribute5,
				Attribute6=@Attribute6,Attribute7=@Attribute7,Attribute8=@Attribute8,Attribute9=@Attribute9,Attribute10=@Attribute10,Attribute11=@Attribute11,
				Attribute12=@Attribute12,Attribute13=@Attribute13,Attribute14=@Attribute14,Attribute15=@Attribute15,Attribute16=@Attribute16,CompanyID=@CompanyID,
				LocationName=@LocationDescription

				
				where LocationCode=@LocationCode
		END
	SELECT @ErrorToBeReturned ErrorToBeReturned;
END  

go 

	
ALTER PROC [dbo].[IPRC_ExceImportDepartment]
(
	@DepartmentCode nvarchar(100)=null,
	@DepartmentDescription nvarchar(100),
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
	@Attribute16 nvarchar(100)=null
)
AS
BEGIN

	DECLARE @ErrorToBeReturned nvarchar(max);
	Declare @DepartmentID int=null;
	DECLARE @ConfigValue nvarchar(max);
	set @DepartmentCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@DepartmentCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
	set @DepartmentDescription=REPLACE(@DepartmentDescription,'''','''')
		if(@ImportTypeID=1)
		BEGIN
		if NOT EXISTS (SELECT * FROM DepartmentTable where DepartmentCode=@DepartmentCode and StatusID=1)
			BEGIN
			Select @ConfigValue=ConfiguarationValue from configurationTable where ConfiguarationName='MasterCodeAutoGenerate'
			IF(@ConfigValue='false')
			BEGIN
			insert into DepartmentTable(DepartmentCode,StatusID,CreatedBy,CreatedDateTime,
			Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6,Attribute7,Attribute8,Attribute9,Attribute10,Attribute11,Attribute12,
			Attribute13,Attribute14,Attribute15,Attribute16,DepartmentName) values(@DepartmentCode,1,@UserID,getDate(),@Attribute1,@Attribute2,@Attribute3,@Attribute4,@Attribute5,
			@Attribute6,@Attribute7,@Attribute8,@Attribute9,@Attribute10,@Attribute11,@Attribute12,
			@Attribute13,@Attribute14,@Attribute15,@Attribute16,@DepartmentDescription)

			--Insert Department Description
				Insert into DepartmentDescriptionTable(DepartmentID,DepartmentDescription,LanguageID,CreatedBy,CreatedDateTime)
				Select C.DepartmentID,@DepartmentDescription,L.LanguageID,C.CreatedBy,C.CreatedDateTime From DepartmentTable C join 
						LanguageTable L on 1=1 left join DepartmentDescriptionTable MD on MD.DepartmentID=C.DepartmentID and MD.languageid=L.languageID 
						where MD.LanguageID is null	and C.DepartmentCode=@DepartmentCode
			END
			ELSE
			BEGIN
			
		insert into DepartmentTable(DepartmentCode,StatusID,CreatedBy,CreatedDateTime,
			Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6,Attribute7,Attribute8,Attribute9,Attribute10,Attribute11,Attribute12,
			Attribute13,Attribute14,Attribute15,Attribute16,DepartmentName) values('-',1,@UserID,getDate(),@Attribute1,@Attribute2,@Attribute3,@Attribute4,@Attribute5,
			@Attribute6,@Attribute7,@Attribute8,@Attribute9,@Attribute10,@Attribute11,@Attribute12,
			@Attribute13,@Attribute14,@Attribute15,@Attribute16,@DepartmentDescription)

			set @DepartmentID=@@IDENTITY;
		--Insert Department Description
				Insert into DepartmentDescriptionTable(DepartmentID,DepartmentDescription,LanguageID,CreatedBy,CreatedDateTime)
				Select C.DepartmentID,@DepartmentDescription,L.LanguageID,C.CreatedBy,C.CreatedDateTime From DepartmentTable C join 
						LanguageTable L on 1=1 left join DepartmentDescriptionTable MD on MD.DepartmentID=C.DepartmentID and MD.languageid=L.languageID 
						where MD.LanguageID is null	and C.DepartmentID=@DepartmentID
			END
			End
			Else
				   SET @ErrorToBeReturned = @DepartmentCode+'- already Exists;'


		End
			ELSE if(@ImportTypeID=2)
		BEGIN
			UPDATE A SET 
					DepartmentDescription = @DepartmentDescription, 
					LastModifiedDateTime = GETDATE(), 
					LastModifiedBy = @UserID
					from DepartmentDescriptionTable A join DepartmentTable B on A.DepartmentID=B.DepartmentID
				WHERE B.DepartmentCode = @DepartmentCode

				Update DepartmentTable set Attribute1=@Attribute1,Attribute2=@Attribute2,Attribute3=@Attribute3,Attribute4=@Attribute4,Attribute5=@Attribute5,
				Attribute6=@Attribute6,Attribute7=@Attribute7,Attribute8=@Attribute8,Attribute9=@Attribute9,Attribute10=@Attribute10,Attribute11=@Attribute11,
				Attribute12=@Attribute12,Attribute13=@Attribute13,Attribute14=@Attribute14,Attribute15=@Attribute15,Attribute16=@Attribute16 
				,DepartmentName=@DepartmentDescription
				where DepartmentCode=@DepartmentCode
		END
	SELECT @ErrorToBeReturned ErrorToBeReturned;
END  
go 


ALTER PROC [dbo].[IPRC_ExceImportTransferType]
(
	@TransferTypeCode nvarchar(100)=null,
	@TransferTypeDescription nvarchar(100),
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
	@Attribute16 nvarchar(100)=null
)
AS
BEGIN
	Declare @TransferTypeID int=null;
	DECLARE @ErrorToBeReturned nvarchar(max);
	DECLARE @ConfigValue nvarchar(max);
	set @TransferTypeCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@TransferTypeCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
	set @TransferTypeDescription=replace(@TransferTypeDescription,'''','''')
		if(@ImportTypeID=1)
		BEGIN
		if NOT EXISTS (SELECT * FROM TransferTypeTable where TransferTypeCode=@TransferTypeCode and StatusID=1)
			BEGIN
			Select @ConfigValue=ConfiguarationValue from configurationTable where ConfiguarationName='MasterCodeAutoGenerate'
			IF(@ConfigValue='false')
			BEGIN
			insert into TransferTypeTable(TransferTypeCode,StatusID,CreatedBy,CreatedDateTime,
			Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6,Attribute7,Attribute8,Attribute9,Attribute10,Attribute11,Attribute12,
			Attribute13,Attribute14,Attribute15,Attribute16,TransferTypeName) values(@TransferTypeCode,1,@UserID,getDate(),@Attribute1,@Attribute2,@Attribute3,@Attribute4,@Attribute5,
			@Attribute6,@Attribute7,@Attribute8,@Attribute9,@Attribute10,@Attribute11,@Attribute12,
			@Attribute13,@Attribute14,@Attribute15,@Attribute16,@TransferTypeDescription)

			--Insert TransferType Description
				Insert into TransferTypeDescriptionTable(TransferTypeID,TransferTypeDescription,LanguageID,CreatedBy,CreatedDateTime)
				Select C.TransferTypeID,@TransferTypeDescription,L.LanguageID,C.CreatedBy,C.CreatedDateTime From TransferTypeTable C join 
						LanguageTable L on 1=1 left join TransferTypeDescriptionTable MD on MD.TransferTypeID=C.TransferTypeID and MD.languageid=L.languageID 
						where MD.LanguageID is null	and C.TransferTypeCode=@TransferTypeCode
			END
			ELSE
			BEGIN
			
			insert into TransferTypeTable(TransferTypeCode,StatusID,CreatedBy,CreatedDateTime,
			Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6,Attribute7,Attribute8,Attribute9,Attribute10,Attribute11,Attribute12,
			Attribute13,Attribute14,Attribute15,Attribute16,TransferTypeName) values('-',1,@UserID,getDate(),@Attribute1,@Attribute2,@Attribute3,@Attribute4,@Attribute5,
			@Attribute6,@Attribute7,@Attribute8,@Attribute9,@Attribute10,@Attribute11,@Attribute12,
			@Attribute13,@Attribute14,@Attribute15,@Attribute16,@TransferTypeDescription)

			set @TransferTypeID=@@IDENTITY;
			--Insert TransferType Description
				Insert into TransferTypeDescriptionTable(TransferTypeID,TransferTypeDescription,LanguageID,CreatedBy,CreatedDateTime)
				Select C.TransferTypeID,@TransferTypeDescription,L.LanguageID,C.CreatedBy,C.CreatedDateTime From TransferTypeTable C join 
						LanguageTable L on 1=1 left join TransferTypeDescriptionTable MD on MD.TransferTypeID=C.TransferTypeID and MD.languageid=L.languageID 
						where MD.LanguageID is null	and C.TransferTypeID=@TransferTypeID
			END
			End
			Else
				   SET @ErrorToBeReturned = @TransferTypeCode+'- already Exists;'


		End
			ELSE if(@ImportTypeID=2)
		BEGIN
			UPDATE A SET 
					TransferTypeDescription = @TransferTypeDescription, 
					LastModifiedDateTime = GETDATE(), 
					LastModifiedBy = @UserID
					from TransferTypeDescriptionTable A join TransferTypeTable B on A.TransferTypeID=B.TransferTypeID
				WHERE B.TransferTypeCode = @TransferTypeCode

				Update TransferTypeTable set Attribute1=@Attribute1,Attribute2=@Attribute2,Attribute3=@Attribute3,Attribute4=@Attribute4,Attribute5=@Attribute5,
				Attribute6=@Attribute6,Attribute7=@Attribute7,Attribute8=@Attribute8,Attribute9=@Attribute9,Attribute10=@Attribute10,Attribute11=@Attribute11,
				Attribute12=@Attribute12,Attribute13=@Attribute13,Attribute14=@Attribute14,Attribute15=@Attribute15,Attribute16=@Attribute16 
				,TransferTypeName=@TransferTypeDescription
				where TransferTypecode=@TransferTypecode
		END
	SELECT @ErrorToBeReturned ErrorToBeReturned;
END  
go 

ALTER PROC [dbo].[IPRC_ExceImportSection]
(
	@SectionCode		nvarchar(50)=null,
	@SectionDescription nvarchar(200),
	@DepartmentCode	nvarchar(50)=null,
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
	@Attribute16 nvarchar(100)=null
)
AS
BEGIN

	DECLARE @ErrorToBeReturned nvarchar(max);
	Declare @DepartmentID int =null;
	Declare @SectionID int=null;
	DECLARE @ConfigValue nvarchar(max);
	Declare @ImportExcelNotAllowCreateReferenceFieldNewEntry nvarchar(10)

	Select @ImportExcelNotAllowCreateReferenceFieldNewEntry=ConfiguarationValue from configurationTable where ConfiguarationName='ImportExcelNotAllowCreateReferenceFieldNewEntry'
	set @SectionCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@SectionCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
	set @SectionDescription=REPLACE(@SectionDescription,'''','''')
		if(@ImportTypeID=1)
		BEGIN

		if(@DepartmentCode is not null and @DepartmentCode!='')
			BEGIN			
				--Insert Department
				if(@ImportExcelNotAllowCreateReferenceFieldNewEntry='false')
				Begin
				IF NOT EXISTS (SELECT * FROM DepartmentTable where DepartmentCode=@DepartmentCode and statusID=1)
				BEGIN
					INSERT INTO DepartmentTable(DepartmentCode,StatusID, CreatedBy,DepartmentName)
						VALUES(@DepartmentCode, 1, @UserID,@DepartmentCode)
						set @DepartmentID=@@IDENTITY;

						Insert into DepartmentDescriptionTable(DepartmentID,DepartmentDescription,LanguageID,CreatedBy,CreatedDateTime)
						Select C.DepartmentID,c.DepartmentCode,L.LanguageID,C.CreatedBy,C.CreatedDateTime From DepartmentTable C join 
								LanguageTable L on 1=1 left join DepartmentDescriptionTable MD on MD.DepartmentID=C.DepartmentID and MD.languageid=L.languageID 
								where MD.LanguageID is null
								 and C.DepartmentCode=@DepartmentCode
				END
				ELSE
				BEGIN
				   select @DepartmentID=DepartmentID from DepartmentTable where DepartmentCode=@DepartmentCode and statusID=1
				   if(@DepartmentCode is not null and @DepartmentID is null)

				   return @DepartmentCode+'- Department Is not available;'
				END
				END
				Else
				IF NOT EXISTS (SELECT * FROM DepartmentTable where DepartmentCode=@DepartmentCode and statusID=1)
				BEGIN
				  return @DepartmentCode+'- Department Is not available;'
				  END				 
			END
			

		if NOT EXISTS (SELECT * FROM SectionTable WHERE SectionCode = @SectionCode AND DepartmentID=@DepartmentID and statusID=1)
				BEGIN
			Select @ConfigValue=ConfiguarationValue from configurationTable where ConfiguarationName='MasterCodeAutoGenerate'
			IF(@ConfigValue='false')
			BEGIN
				INSERT INTO SectionTable(SectionCode,DepartmentID,StatusID, CreatedBy,CreatedDateTime,
			Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6,Attribute7,Attribute8,Attribute9,Attribute10,Attribute11,Attribute12,
			Attribute13,Attribute14,Attribute15,Attribute16,SectionName) values(@Sectioncode,@DepartmentID,1,@UserID,getDate(),@Attribute1,@Attribute2,@Attribute3,@Attribute4,@Attribute5,
			@Attribute6,@Attribute7,@Attribute8,@Attribute9,@Attribute10,@Attribute11,@Attribute12,
			@Attribute13,@Attribute14,@Attribute15,@Attribute16,@SectionDescription)

				--Insert Section Description
					Insert into SectionDescriptionTable(SectionID,SectionDescription,LanguageID,CreatedBy,CreatedDateTime)
						Select C.SectionID,@SectionDescription,L.LanguageID,C.CreatedBy,C.CreatedDateTime From SectionTable C join 
						LanguageTable L on 1=1 left join SectionDescriptionTable MD on MD.SectionID=C.SectionID and MD.languageid=L.languageID 
						where MD.LanguageID is null
						and C.SectionCode=@SectionCode
			END
			ELSE
			BEGIN
			
		INSERT INTO SectionTable(SectionCode,DepartmentID,StatusID, CreatedBy,CreatedDateTime,
			Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6,Attribute7,Attribute8,Attribute9,Attribute10,Attribute11,Attribute12,
			Attribute13,Attribute14,Attribute15,Attribute16,SectionName) values('-',@DepartmentID,1,@UserID,getDate(),@Attribute1,@Attribute2,@Attribute3,@Attribute4,@Attribute5,
			@Attribute6,@Attribute7,@Attribute8,@Attribute9,@Attribute10,@Attribute11,@Attribute12,
			@Attribute13,@Attribute14,@Attribute15,@Attribute16,@SectionDescription)

			set @SectionID=@@IDENTITY;
		--Insert Section Description
					Insert into SectionDescriptionTable(SectionID,SectionDescription,LanguageID,CreatedBy,CreatedDateTime)
						Select C.SectionID,@SectionDescription,L.LanguageID,C.CreatedBy,C.CreatedDateTime From SectionTable C join 
						LanguageTable L on 1=1 left join SectionDescriptionTable MD on MD.SectionID=C.SectionID and MD.languageid=L.languageID 
						where MD.LanguageID is null
						and C.SectionID=@SectionID
			END
			End
			Else
				   SET @ErrorToBeReturned = @SectionCode+'- already Exists;'


		End
			ELSE if(@ImportTypeID=2)
		BEGIN		
				UPDATE A SET 
					SectionDescription = @SectionDescription					
					from SectionDescriptionTable A join SectionTable B on A.SectionID=B.SectionID
				WHERE B.SectionCode = @SectionCode

				Update SectionTable set Attribute1=@Attribute1,Attribute2=@Attribute2,Attribute3=@Attribute3,Attribute4=@Attribute4,Attribute5=@Attribute5,
				Attribute6=@Attribute6,Attribute7=@Attribute7,Attribute8=@Attribute8,Attribute9=@Attribute9,Attribute10=@Attribute10,Attribute11=@Attribute11,
				Attribute12=@Attribute12,Attribute13=@Attribute13,Attribute14=@Attribute14,Attribute15=@Attribute15,Attribute16=@Attribute16
				,SectionName=@SectionDescription
				where SectionCode=@SectionCode
		END
	SELECT @ErrorToBeReturned ErrorToBeReturned;
END  
go 
alter table Locationtable alter column OracleLocationID nvarchar(100)
go 
ALTER PROC [dbo].[IPRC_ExceImportDesignation]
(
	@DesignationCode nvarchar(100)=null,
	@DesignationDescription nvarchar(100),
	@ImportTypeID int,
	@UserID int=1	
)
AS
BEGIN
	Declare @DesignationID int=null;
	DECLARE @ErrorToBeReturned nvarchar(max);
	DECLARE @ConfigValue nvarchar(max);
	set @DesignationCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@DesignationCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
	set @DesignationDescription=REPLACE(@DesignationDescription,'''','''')
		if(@ImportTypeID=1)
		BEGIN
		if NOT EXISTS (SELECT * FROM DesignationTable where DesignationCode=@DesignationCode and StatusID=1)
			BEGIN
			Select @ConfigValue=ConfiguarationValue from configurationTable where ConfiguarationName='MasterCodeAutoGenerate'
			IF(@ConfigValue='false')
			BEGIN
			insert into DesignationTable(DesignationCode,StatusID,CreatedBy,CreatedDateTime,DesignationName) values(@DesignationCode,1,@UserID,getDate(),@DesignationDescription)

			--Insert Asset Condition Description
				Insert into DesignationDescriptionTable(DesignationID,DesignationDescription,LanguageID,CreatedBy,CreatedDateTime)
				Select C.DesignationID,@DesignationDescription,L.LanguageID,C.CreatedBy,C.CreatedDateTime From DesignationTable C join 
						LanguageTable L on 1=1 left join DesignationDescriptionTable MD on MD.DesignationID=C.DesignationID and MD.languageid=L.languageID 
						where MD.LanguageID is null	and C.DesignationCode=@DesignationCode
			END
			ELSE
			BEGIN
			
			insert into DesignationTable(DesignationCode,StatusID,CreatedBy,CreatedDateTime,DesignationName) values('-',1,@UserID,getDate(),@DesignationDescription)

			set @DesignationID=@@IDENTITY;
			--Insert Asset Condition Description
				Insert into DesignationDescriptionTable(DesignationID,DesignationDescription,LanguageID,CreatedBy,CreatedDateTime)
				Select C.DesignationID,@DesignationDescription,L.LanguageID,C.CreatedBy,C.CreatedDateTime From DesignationTable C join 
						LanguageTable L on 1=1 left join DesignationDescriptionTable MD on MD.DesignationID=C.DesignationID and MD.languageid=L.languageID 
						where MD.LanguageID is null	and C.DesignationID=@DesignationID
			END
			End
			Else
				   SET @ErrorToBeReturned = @DesignationCode+'- already Exists;'


		End
			ELSE if(@ImportTypeID=2)
		BEGIN
			UPDATE A SET 
					DesignationDescription = @DesignationDescription, 
					LastModifiedDateTime = GETDATE(), 
					LastModifiedBy = @UserID
					from DesignationDescriptionTable A join DesignationTable B on A.DesignationID=B.DesignationID
				WHERE B.DesignationCode = @DesignationCode

				update a set DesignationName=@DesignationDescription
				from DesignationTable 	WHERE a.DesignationCode = @DesignationCode
		END
	SELECT @ErrorToBeReturned ErrorToBeReturned;
END  
go 

alter table Locationtable alter column OracleLocationID nvarchar(100)
go 




ALTER Procedure [dbo].[rprc_AssetRetirementReceiptHeader]
(
  @TransactionID int 
)
as 
Begin 
  select  P1.PersonFirstName+'-'+p1.PersonLastName as SenderName,L1.PID4 as FromLocationName,
      p1.EMailID as SenderEmailID,P1.MobileNo as SenderPhoneno,a.Remarks as Comments,AP.SenderJobTitle as SenderJobTitle,AP.ApproveDate,Ap.SignaturePath
	  ,Ap.FromStamp,ap.ToStamp,a.TransactionNo,a.TransactionNo as Receiptyear,RE.RetirementTypeName--'GS/AM/'+convert(nvarchar(100),YEAR(a.CreatedDateTime))+'/AR/' + TransactionNo  as Receiptyear
  from TransactionTable a 
	join (select * from (select  ROW_NUMBER() over( 
         PARTITION BY transactionid  Order by AssetID asc) as serialNo,* from TransactionLineItemTable)y where y.SerialNo=1 ) TL on a.TransactionID=TL.TransactionID
	join PersonTable P1 on a.CreatedBy=p1.PersonID
	Left join DesignationTable D on P1.DesignationID=d.DesignationID
    Left join RetirementTypeTable RE on TL.RetirementTypeID=RE.RetirementTypeID
	Left join LocationNewHierarchicalView L1 on TL.FromLocationID=L1.ChildID
    Left join (
			select A2.PersonFirstName+'-'+A2.PersonLastName as AreaApproverName,A1.LastModifiedDateTime as ApproveDate,
		   A1.Remarks as Comments,A1.TransactionID,A2.SignaturePath,FL.StampPath as FromStamp,TL.StampPath as ToStamp,DesignationName as SenderJobTitle
		   from ApprovalHistoryTable A1
		   join PersonTable A2 on A1.LastModifiedBy=A2.PersonID
		   Left join DesignationTable D on a2.DesignationID=D.DesignationID
		   Left join LocationTable FL on A1.FromLocationID=FL.LocationID
		   Left join LocationTable TL on A1.ToLocationID=TL.LocationID
				where A1.OrderNo=1 and a1.ApproveModuleID=10
		   )AP on a.TransactionID=AP.TransactionID
		  WHERE A.TRANSACTIONID=@TransactionID
End 
go
ALTER Procedure [dbo].[rprc_AssetTransferReceiptHeader]
(
  @TransactionID int 
)
as 
Begin 

   Select P1.PersonFirstName+'-'+p1.PersonLastName as SenderName,L1.PID4 as FromLocationName,L2.PID4 as ToLocationName,P2.PersonFirstName+'-'+p2.PersonLastName as ReceiverName,
    p1.EMailID as SenderEmailID,P1.MobileNo as SenderPhoneno,a.Remarks as Comments,D.DesignationName as SenderJobTitle,D1.DesignationName as ReceiverJobTitle,
	P2.EmailID as ReceiverEmailID,P2.MobileNo as ReceiverPhoneNo,T.TransferTypeName,ap.ApproveDate,ap.SignaturePath,
	ap.FromStamp,aP.ToStamp, TransactionNo as Receiptyear,--'GS/AM/'+convert(nvarchar(100),YEAR(a.CreatedDateTime))+'/AT/'+TransactionNo as Receiptyear
	ap2.lvl2Signature,ap2.Approve2Date
	From TransactionTable a 
	join (select * from (select  ROW_NUMBER() over( 
         PARTITION BY transactionid  Order by AssetID asc) as serialNo,* from TransactionLineItemTable)y where y.SerialNo=1 ) TL on a.TransactionID=TL.TransactionID
	join PersonTable P1 on a.CreatedBy=p1.PersonID
	Left join DesignationTable D on P1.DesignationID=d.DesignationID
	Left join TransferTypeTable T on TL.TransferTypeID=T.TransferTypeID
	left join (SELECT * FROM (select  ROW_NUMBER() over( 
         PARTITION BY a.transactionid , a.approvemoduleID
         ORDER BY Orderno DESC ) AS SerialNo,* from approvalHistorytable a ) X where X.SerialNo=1 and statusid = dbo.fnGetActiveStatusID() ) b on a.transactionid=b.transactionid 	
   Left join personTable p2 on  b.LastModifiedBy=p2.personID
   Left join DesignationTable D1 on p2.DesignationID=D1.DesignationID
   Left join LocationNewHierarchicalView L1 on TL.FromLocationID=L1.ChildID
   Left join LocationNewHierarchicalView L2 on TL.ToLocationID=L2.ChildID
   Left join (
			select A2.PersonFirstName+'-'+A2.PersonLastName as AreaApproverName,convert(nvarchar(1000),A1.LastModifiedDateTime,103) as ApproveDate,
		    A1.Remarks as Comments,A1.TransactionID,A2.SignaturePath,FL.StampPath as FromStamp,TL.StampPath as ToStamp
		   from ApprovalHistoryTable A1
		   join PersonTable A2 on A1.LastModifiedBy=A2.PersonID
		   Left join LocationTable FL on A1.FromLocationID=FL.LocationID
           Left join LocationTable TL on A1.ToLocationID=TL.LocationID
		    where A1.OrderNo=1 and a1.ApproveModuleID=5
		   )AP on a.TransactionID=AP.TransactionID
	Left join (
			select A2.PersonFirstName+'-'+A2.PersonLastName as Area2ApproverName,convert(nvarchar(1000),A1.LastModifiedDateTime,103) as Approve2Date,
		    A1.Remarks as Comments,A1.TransactionID,A2.SignaturePath as lvl2Signature
			from ApprovalHistoryTable A1
			 join PersonTable A2 on A1.LastModifiedBy=A2.PersonID
			Left join LocationTable FL on A1.FromLocationID=FL.LocationID
			Left join LocationTable TL on A1.ToLocationID=TL.LocationID
			where A1.OrderNo=2 and a1.ApproveModuleID=5
		   )AP2 on a.TransactionID=AP2.TransactionID
	  where a.TransactionID=@TransactionID
End 
go 
delete from UserGridNewColumnTable where MasterGridID in (select MasterGridID from MasterGridNewTable where MasterGridName='Asset')
go
 delete from MasterGridNewLineItemTable where MasterGridID in (select MasterGridID from MasterGridNewTable where MasterGridName='Asset')
 go 
 update MasterGridNewTable set EntityName='AssetNewView' where MasterGridName='Asset'
 go 

insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
select COLUMN_NAME,COLUMN_NAME,'100', case when DATA_TYPE='smalldatetime' then 'dd/MM/yyyy' else NULL end,1,ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS displayOrderID,
case when  DATA_TYPE='smalldatetime' or  DATA_TYPE='date' then 'System.DateTime' 
when DATA_TYPE='varchar' or  DATA_TYPE='nvarchar' then 'System.String' 
when DATA_TYPE='decimal' then 'System.Decimal' else DATA_TYPE end ,NULL,(select MasterGridID from MasterGridNewTable where MasterGridName='Asset')
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME='AssetNewView' 
 and COLUMN_NAME not like 'Attribute%' and COLUMN_NAME not like 'Create%'  and COLUMN_NAME not like 'Last%'
 and COLUMN_NAME not like 'Status%' and COLUMN_NAME not in (replace(table_name,'NewView','ID'))
 and COLUMN_NAME not like 'DOF%' and DATA_TYPE not in ('int','bit') and COLUMN_NAME not like 'CategoryL%'
 and COLUMN_NAME not like 'LocationL%' and COLUMN_NAME not like '%Attribute%'and COLUMN_NAME not like '%path%'
 and COLUMN_NAME not in ('ERPUpdateType','QFAssetCode','SyncDateTime','DistributionID')


 go 


delete from UserGridNewColumnTable where MasterGridID in (select MasterGridID from MasterGridNewTable where MasterGridName='AssetTransfer')
go
 delete from MasterGridNewLineItemTable where MasterGridID in (select MasterGridID from MasterGridNewTable where MasterGridName='AssetTransfer')
 go 
 update MasterGridNewTable set EntityName='AssetNewView' where MasterGridName='AssetTransfer'
 go 

insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
select COLUMN_NAME,COLUMN_NAME,'100', case when DATA_TYPE='smalldatetime' then 'dd/MM/yyyy' else NULL end,1,ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS displayOrderID,
case when  DATA_TYPE='smalldatetime' or  DATA_TYPE='date' then 'System.DateTime' 
when DATA_TYPE='varchar' or  DATA_TYPE='nvarchar' then 'System.String' 
when DATA_TYPE='decimal' then 'System.Decimal' else DATA_TYPE end ,NULL,(select MasterGridID from MasterGridNewTable where MasterGridName='AssetTransfer')
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME='AssetNewView' 
 and COLUMN_NAME not like 'Attribute%' and COLUMN_NAME not like 'Create%'  and COLUMN_NAME not like 'Last%'
 and COLUMN_NAME not like 'Status%' and COLUMN_NAME not in (replace(table_name,'NewView','ID'))
 and COLUMN_NAME not like 'DOF%' and DATA_TYPE not in ('int','bit') and COLUMN_NAME not like 'CategoryL%'
 and COLUMN_NAME not like 'LocationL%' and COLUMN_NAME not like '%Attribute%'and COLUMN_NAME not like '%path%'
 and COLUMN_NAME not in ('ERPUpdateType','QFAssetCode','SyncDateTime','DistributionID')

 select * from MasterGridNewLineItemTable where MasterGridID=(select MasterGridID from MasterGridNewTable where MasterGridName='AssetTransfer')

 update MasterGridNewLineItemTable set IsDefault=0 where FieldName not in ('Barcode','CategoryHierarchy','LocationHierarchy','DepartmentName','CustodianName','ProductName','PurchaseDate','PurchasePrice')
 and  MasterGridID=(select MasterGridID from MasterGridNewTable where MasterGridName='AssetTransfer')
 go 

 delete from UserGridNewColumnTable where MasterGridID in (select MasterGridID from MasterGridNewTable where MasterGridName='AssetRetirement')
go
 delete from MasterGridNewLineItemTable where MasterGridID in (select MasterGridID from MasterGridNewTable where MasterGridName='AssetRetirement')
 go 
 update MasterGridNewTable set EntityName='AssetNewView' where MasterGridName='AssetRetirement'
 go 

 insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
select COLUMN_NAME,COLUMN_NAME,'100', case when DATA_TYPE='smalldatetime' then 'dd/MM/yyyy' else NULL end,1,ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS displayOrderID,
case when  DATA_TYPE='smalldatetime' or  DATA_TYPE='date' then 'System.DateTime' 
when DATA_TYPE='varchar' or  DATA_TYPE='nvarchar' then 'System.String' 
when DATA_TYPE='decimal' then 'System.Decimal' else DATA_TYPE end ,NULL,(select MasterGridID from MasterGridNewTable where MasterGridName='AssetRetirement')
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME='AssetNewView' 
 and COLUMN_NAME not like 'Attribute%' and COLUMN_NAME not like 'Create%'  and COLUMN_NAME not like 'Last%'
 and COLUMN_NAME not like 'Status%' and COLUMN_NAME not in (replace(table_name,'NewView','ID'))
 and COLUMN_NAME not like 'DOF%' and DATA_TYPE not in ('int','bit') and COLUMN_NAME not like 'CategoryL%'
 and COLUMN_NAME not like 'LocationL%' and COLUMN_NAME not like '%Attribute%'and COLUMN_NAME not like '%path%'
 and COLUMN_NAME not in ('ERPUpdateType','QFAssetCode','SyncDateTime','DistributionID')

 select * from MasterGridNewLineItemTable where MasterGridID=(select MasterGridID from MasterGridNewTable where MasterGridName='AssetRetirement')

 update MasterGridNewLineItemTable set IsDefault=0 where FieldName not in ('Barcode','CategoryHierarchy','LocationHierarchy','DepartmentName','CustodianName','ProductName','PurchaseDate','PurchasePrice')
 and  MasterGridID=(select MasterGridID from MasterGridNewTable where MasterGridName='AssetRetirement')
 go 
  update MasterGridNewLineItemTable set DisplayName='TemplateName' where FieldName='TamplateName' and MasterGridID=(select MasterGridID from MasterGridNewTable where MasterGridName='ImportFormat')
  go 
  If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='GenerateAssetFromPO')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('GenerateAssetFromPO','GenerateAssetFromPO',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Transaction'),1,0);
End
Go
If not exists(SELECT MenuName FROM USER_MENUTABLE where MenuName='GenerateAssetFromPO' and ParentTransactionID=1)
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO,ParentTransactionID) Values(

'GenerateAssetFromPO',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='GenerateAssetFromPO'),'/GenerateAssetFromPO/Index',
(Select MenuID from USER_MENUTABLE where MenuName='Transaction' ),8,1);
end
go

Update MasterGridNewLineItemTable set IsDefault=1 where FieldName='ProductName' and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='Product')
go 

ALTER Trigger [dbo].[trg_Ins_ProductCodeAutoGeneration] on [dbo].[ProductTable] 
for Insert
As
Begin 
 Declare @CodeTable Table(code nvarchar(50))
 Declare @prefix as nvarchar(50),@ProductID int,@ProductName nvarchar(max) 

 select @prefix= ConfiguarationValue  from ConfigurationTable (nolock) where ConfiguarationName='MasterCodeAutoGenerate'
	 if @prefix='true'
	 Begin 
		IF Exists(select ProductCode from INSERTED where ProductCode='-')
		Begin 	
		  insert into @CodeTable
			 exec [dbo].[prc_GetAutoCodeGeneration] 'Product',1
			 
		  Update ProductTable set ProductCode=(select code from @CodeTable) where ProductID=(select ProductID from INSERTED)		  
		  
		  IF Exists(select CodeName from CodeConfigurationTable where CodeName='Product')
		   Begin 
		      update CodeConfigurationTable set CodeValue=CodeValue+1 where CodeName='Product'
		   End

		End
	End 
	if update(ProductName)
	Begin 
	   select @ProductID=ProductID,@ProductName=ProductName from inserted

	   update AssetTable set AssetDescription=@ProductName where ProductID=@ProductID
	End 
End 

go 

 If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='Location')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('Location','Location',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Master'),1,0);
End
Go
If not exists(SELECT MenuName FROM USER_MENUTABLE where MenuName='Location' and ParentTransactionID=1)
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO,ParentTransactionID) Values(

'Location',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='Location'),'/Location/Create?pageName=Location',
(Select MenuID from USER_MENUTABLE where MenuName='Master' ),8,1);
end
go
update User_MenuTable set TargetObject='/TreeView/IndexPage?pageName=Location' where MenuName='Location' and parentTransactionid=1


go 


ALTER Procedure [dbo].[iprc_AMSExcelImportCustodian]
(
 @PersonCode nvarchar(100) =NULL,
 @FirstName nvarchar(100)=NULL,
 @LastName nvarchar(100)=NULL,
 @DOJ nvarchar(100)=NULL,
 @MobileNo nvarchar(100)=NULL,
 @Gender nvarchar(10)=NULL,
 @DepartmentCode nvarchar(100)=NULL,
 @EmailID nvarchar(100)=NULL,
 @DesignationName nvarchar(100)=NULL,
 @SignatureImage nvarchar(max)=NULL,
 @ImportTypeID int,
	@UserID int=1
) 
As 
Begin 
	DECLARE @DepartmentID INT,@DesignationID int ,@statusID int 
	DECLARE @ErrorToBeReturned nvarchar(max);
	DECLARE @ConfigValue nvarchar(max);
	Declare @ImportExcelNotAllowCreateReferenceFieldNewEntry nvarchar(10)
	Select @ImportExcelNotAllowCreateReferenceFieldNewEntry=ConfiguarationValue from configurationTable where ConfiguarationName='ImportExcelNotAllowCreateReferenceFieldNewEntry'
	set @personCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@personCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
	set @FirstName=REPLACE(@FirstName,'''','''')
	set @LastName=replace(@LastName,'''','''')
	DECLARE @MESSAGETABLE TABLE(TEXT NVARCHAR(MAX))
	Select @statusID=dbo.[fnGetActiveStatusID]()

	IF(LEN(@Gender) > 1) SET @Gender = SUBSTRING(@Gender, 1, 1)

	if(@DepartmentCode is not null and @DepartmentCode!='')
	BEGIN
		select @DepartmentID=DepartmentID from DepartmentTable where DepartmentCode=@DepartmentCode and StatusID=@statusID

		--Insert Department
		if(@ImportExcelNotAllowCreateReferenceFieldNewEntry='false')
		Begin
			IF (@DepartmentID IS NULL)
			BEGIN
				INSERT INTO DepartmentTable(DepartmentCode,StatusID, CreatedBy,DepartmentName,CreatedDateTime)
					select @DepartmentCode, @statusID, @UserID,@DepartmentCode,getdate()
				set @DepartmentID=@@IDENTITY;

				Insert into DepartmentDescriptionTable(DepartmentID,DepartmentDescription,LanguageID,CreatedBy,CreatedDateTime)
				Select C.DepartmentID,c.DepartmentCode,L.LanguageID,C.CreatedBy,C.CreatedDateTime From DepartmentTable C join 
						LanguageTable L on 1=1 left join DepartmentDescriptionTable MD on MD.DepartmentID=C.DepartmentID and MD.languageid=L.languageID 
						where MD.LanguageID is null
							and C.DepartmentCode=@DepartmentCode
			END
			ELSE
			BEGIN
				if(@DepartmentCode is not null and @DepartmentID is null)

				return @DepartmentCode+'- Department Is not available;'
			END
		END
		Else
		IF (@DepartmentID IS NULL)
		BEGIN
			return @DepartmentCode+'- Department Is not available;'
		END				 
	END

	if(@DesignationName is not null and @DesignationName!='')
	BEGIN
		select @DesignationID=DesignationID from DesignationTable where DesignationName=@DesignationName and statusID=@statusID
		--Insert Department
		if(@ImportExcelNotAllowCreateReferenceFieldNewEntry='false')
		Begin
			IF (@DesignationID IS NULL)
			BEGIN
				INSERT INTO DesignationTable(DEsignationCode,StatusID, CreatedBy,DesignationName,CreatedDateTime)
					select @designationName,@statusID, @UserID,@designationName,GETDATE()

				set @DepartmentID=@@IDENTITY;

				Insert into DesignationDescriptionTable(DesignationID,DesignationDescription,LanguageID,CreatedBy,CreatedDateTime)
				Select C.DEsignationID,c.DesignationCode,L.LanguageID,C.CreatedBy,C.CreatedDateTime From DesignationTable C join 
						LanguageTable L on 1=1 left join DesignationDescriptionTable MD on MD.DesignationID=C.DesignationID and MD.languageid=L.languageID 
						where MD.LanguageID is null
							and C.DesignationName=@designationName
			END
			ELSE
			BEGIN
				if(@DesignationName is not null and @DesignationID is null)
					return @DesignationName+'- Designation Is not available;'
			END
		END
		Else
		IF (@DesignationID IS NULL)
		BEGIN
			return @DesignationName+'- Designation Is not available;'
		END				 
	END

	if(@ImportTypeID=1)
		   BEGIN
		    if NOT EXISTS (SELECT * FROM PersonTable WHERE PersonCode = @PersonCode)
			BEGIN
				INSERT INTO User_LoginUserTable(UserName,Password,PasswordSalt,LastActivityDate,LastLoginDate,LastLoggedInDate,ChangePasswordAtNextLogin,IsLockedOut,IsDisabled,IsApproved)
					VALUES(@PersonCode ,'Mod6/JMHjHeKXDkUK/zd7PfLlJg=','BZxI8E2lroNt28VMhZsyyaNZha8=',GETDATE(),GETDATE(),GETDATE(),1,0,0,1 )

				INSERT INTO PersonTable(PersonID, PersonFirstName, PersonLastName, PersonCode, AllowLogin, DepartmentID, UserTypeID, StatusID, Culture,EmailID,MobileNo,DesignationID,DOJ,Gender,SignaturePath) 
					VALUES(@@IDENTITY, @FirstName, @LastName, @PersonCode, 0, @DepartmentID, 3, 1, 'en-GB',@EmailID,@MobileNo,@DesignationID,case when @DOJ is not null then CONVERT(DATETIME,@DOJ,103) else null end,@Gender,@SignatureImage)
			END
			else 
			Begin
			    INSERT INTO @MESSAGETABLE(TEXT)VALUES('Person code "' + @PersonCode + '" already exists' )
			End 
			End
			ELSE if(@ImportTypeID=2)
			
		BEGIN	
		if exists(select PersonCode from PersonTable where personcode=@PersonCode) 
		Begin 
		UPDATE PersonTable 
			SET PersonFirstName = case when @FirstName is not null then @FirstName else PersonFirstName end ,  
			PersonLastName=case when @LastName is not null then @LastName else PersonLastName end  ,
			EmailID=case when @EmailID is not null then @EmailID else EmailID end ,
			MobileNo=case when @MobileNo is not null then @MobileNo else mobileNo end ,
			DepartmentID=isnull(@DepartmentID,DepartmentID),
			Designationid=isnull(@DesignationID,Designationid),
			doj=case when @DOJ is not null then CONVERT(DATETIME,@DOJ,103) else doj end,
			gender=case when @Gender is not null then @Gender else Gender end ,
			SignaturePath=case when @SignatureImage is not null then @SignatureImage else SignaturePath end 

			 WHERE PersonCode = @PersonCode
			 End 
			 Else 
			 Begin
			   INSERT INTO @MESSAGETABLE(TEXT)VALUES(@PersonCode + '- This PersonCode not available in PErsonTable')
			 End 

	End 
	DEclare @ReturnMessage nvarchar(max)

	Select @ReturnMessage = COALESCE(@ReturnMessage + ', ' + Text, Text)  from @MESSAGETABLE
			select @ReturnMessage as ReturnMessage

end 
go 
update User_MenuTable set TargetObject='/MasterPage/Index?pageName=Category' where MenuName='Category' and ParentTransactionID=1 
go 
update User_MenuTable set TargetObject='/MasterPage/Index?pageName=Location' where MenuName='Location' and ParentTransactionID=1 
go 

ALTER Procedure [dbo].[Iprc_UserApprovalRoleMapping]
(
   @LocationCode      nvarchar(100) = NULL,
   @ApprovalRoleCode  nvarchar(100) = NULL,
   @UserCode	      nvarchar(100) = NULL,
   @ImportTypeID int,
   @UserID int
)
as 
Begin 
set @LocationCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@LocationCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
set @ApprovalRoleCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@ApprovalRoleCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
set @UserCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@UserCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
Declare @LocationID int,@PersonID int ,@ApprovalRoleID int,@StatusID int 
select  @StatusID= [dbo].[fnGetActiveStatusID]()
 
 IF (@LOCATIONCODE is not NULL and @LOCATIONCODE!='')
  BEGIN 
    IF EXISTS(SELECT LOCATIONCODE FROM LOCATIONTABLE WHERE LOCATIONCODE=@LOCATIONCODE and StatusID=@statusID)
	BEGIN  
		SELECT @LOCATIONID=LOCATIONID FROM LOCATIONTABLE WHERE LOCATIONCODE=@LOCATIONCODE and StatusID=@statusID
	END
Else 
Begin 
     SElect @LOCATIONCODE+'- Location Code not avaliable in table please add it ' as ReturnMessage
	Return 
End 

  End 
IF (@ApprovalRoleCode is not NULL and @ApprovalRoleCode!='')
  BEGIN
  IF EXISTS(SELECT ApprovalRoleCode FROM ApprovalRoleTable WHERE ApprovalRoleCode=@ApprovalRoleCode and StatusID=@statusID)
	BEGIN  
		SELECT @ApprovalRoleID=ApprovalRoleID FROM ApprovalRoleTable WHERE ApprovalRoleCode=@ApprovalRoleCode and StatusID=@statusID
	END
Else 
Begin 
     SElect @ApprovalRoleCode+'- Approval Role Code not avaliable in table please add it ' as ReturnMessage
	Return 
End 

End 
IF (@UserCode is not NULL and @UserCode!='')
  BEGIN
  IF EXISTS(SELECT PersonCode FROM PersonTable WHERE PersonCode=@UserCode and StatusID=@statusID)
	BEGIN  
		SELECT @PersonID=PersonID FROM  PersonTable WHERE PersonCode=@UserCode and StatusID=@statusID
	END
END

IF(@PersonID IS NULL)
Begin 
     SElect @UserCode+'- User Code not avaliable in table please add it ' as ReturnMessage
	Return 
End 

If not exists (select childid from LocationNewHierarchicalView  where childid=@LocationID and level=2) 
Begin 
  SElect @LocationCode+'- LocationCode must be n Second level please check it  ' as ReturnMessage
	Return 

end 

If @ImportTypeID=1
begin
 If((@LocationID is not null and @LocationID!='') and 
		(@ApprovalRoleID is not null and @ApprovalRoleID !='') and (@PersonID is not null and @PersonID!=''))
Begin 

If not Exists(SElect ApprovalRoleID from UserApprovalRoleMappingTable where UserID=@PersonID 
		and Locationid=@LocationID and ApprovalRoleID=@ApprovalRoleID and StatusID=@statusID)
Begin 
  Insert into UserApprovalRoleMappingTable(UserID,LocationID,ApprovalRoleID,StatusID)
		values(@PersonID,@LocationID,@ApprovalRoleID,@StatusID)
End 
else 
Begin
   SElect @UserCode + '-[' + CAST(@userID as varchar(50)) + ']-' + @LocationCode+'-'+@ApprovalRoleCode+'- Already Exists' as ReturnMessage
	Return 
 
end 
End 
End 
End 
go 
if not exists(select RightName from User_RightTable where RightName='ApplicationErrorLog')
begin
 Insert into User_RightTable(RightName,RightDescription,ValueType,DisplayRight,RightGroupID,IsActive,IsDeleted)
 select 'ApplicationErrorLog','ApplicationErrorLog',49,1,RightGroupID,1,0
 from User_RightGroupTable where RightGroupName='Tools'
end 
go 
if not exists(select MenuName from User_MenuTable where MenuName='ErrorLog' and ParentTransactionID=1)
Begin 
   insert into User_MenuTable(MenuName,RightID,TargetObject,ParentMenuID,OrderNo,ParentTransactionID)
   select 'ApplicationErrorLog',RightID,'/MasterPage/Index?pageName=ApplicationErrorLog',(select menuid from User_MenuTable where MenuName='Tools'),6,1
   from User_RightTable where RightName='ApplicationErrorLog'
end 
go 
update User_RightTable set RightName='AssetRetirementFinalApproval',RightDescription='AssetRetirementFinalApproval' where RightName='assetdisposalfinalapproval'
go 
if not exists(select MenuName from User_MenuTable where MenuName='Configuration' and ParentTransactionID=1)
Begin 
   insert into User_MenuTable(MenuName,RightID,TargetObject,ParentMenuID,OrderNo,ParentTransactionID)
   select 'Configuration',RightID,'/Configuration/Edit',(select menuid from User_MenuTable where MenuName='Tools'),6,1
   from User_RightTable where RightName='Configuration'
end 
go 

Alter table ConfigurationTable add CategoryName nvarchar(100) NULL
go 
update ConfigurationTable set CategoryName=CatagoryName
GO 


CREATE TABLE [dbo].[ReportTemplateCategoryTable](
	[ReportTemplateCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[ReportTemplateCategoryName] [nvarchar](150) NOT NULL,
	[StatusID] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDateTime] [smalldatetime] NOT NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDateTime] [smalldatetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ReportTemplateCategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ReportTemplateCategoryTable] ADD  DEFAULT ((5)) FOR [StatusID]
GO
ALTER TABLE [dbo].[ReportTemplateCategoryTable] ADD  DEFAULT (getdate()) FOR [CreatedDateTime]
GO
ALTER TABLE [dbo].[ReportTemplateCategoryTable]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[User_LoginUserTable] ([UserID])
GO
ALTER TABLE [dbo].[ReportTemplateCategoryTable]  WITH CHECK ADD FOREIGN KEY([LastModifiedBy])
REFERENCES [dbo].[User_LoginUserTable] ([UserID])
GO
ALTER TABLE [dbo].[ReportTemplateCategoryTable]  WITH CHECK ADD  CONSTRAINT [FK_ReportTemplateCategoryTable_StatusID] FOREIGN KEY([StatusID])
REFERENCES [dbo].[StatusTable] ([StatusID])
GO
ALTER TABLE [dbo].[ReportTemplateCategoryTable] CHECK CONSTRAINT [FK_ReportTemplateCategoryTable_StatusID]
GO

IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'ReportTemplateCategoryID' AND Object_ID = Object_ID(N'ReportTemplateTable'))
Begin 
alter table ReportTemplateTable add  ReportTemplateCategoryID int foreign key references reportTemplateCategoryTable(ReportTemplateCategoryID)
END
GO
IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'TemplateDescription' AND Object_ID = Object_ID(N'ReportTemplateTable'))
Begin 
alter table ReportTemplateTable add  TemplateDescription nvarchar(500) NULL
END
GO
IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'QueryType' AND Object_ID = Object_ID(N'ReportTemplateTable'))
Begin 
alter table ReportTemplateTable add  QueryType nvarchar(100) NULL
END
GO
IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'RightID' AND Object_ID = Object_ID(N'ReportTemplateTable'))
Begin 
alter table ReportTemplateTable add  RightID  int foreign key references user_RightTable(RightID)
END
GO


if not exists(Select reporttemplatecategoryname from Reporttemplatecategorytable where reporttemplatecategoryname='Standard Report')
Begin 
 insert into Reporttemplatecategorytable(reporttemplatecategoryname,statusid,createdby,createddatetime)
 values('Standard Report',1,1,getdate())
end 
go 

update ReportTemplateTable set QueryString='rvw_AssetSummary',querytype='View',Templatedescription=ReportTemplateName
,ReporttemplateCategoryID=(select ReportTemplateCategoryID from ReportTemplateCategoryTable where ReportTemplateCategoryName='Standard Report')
where ReportTemplateName='AssetReport'
go 
update ReportTemplateTable set QueryString='rvw_AssetRetirement',querytype='View',Templatedescription=ReportTemplateName
,ReporttemplateCategoryID=(select ReportTemplateCategoryID from ReportTemplateCategoryTable where ReportTemplateCategoryName='Standard Report')
where ReportTemplateName='DisposedAssetReport'
go 
update ReportTemplateTable set QueryString='rvw_AssetTransfer',querytype='View',Templatedescription=ReportTemplateName
,ReporttemplateCategoryID=(select ReportTemplateCategoryID from ReportTemplateCategoryTable where ReportTemplateCategoryName='Standard Report')
where ReportTemplateName='AssetTransferReport'
go 

alter view [dbo].[rvw_AssetSummary] 
as 
Select * from AssetNewView where  StatusID not in (select statusid from StatusTable where Status not in ('Disposed','Delete','Deleted'))
GO
create view [dbo].[rvw_AssetRetirement] 
as 
Select * from AssetNewView where StatusID  in (select statusid from StatusTable where Status not in ('Disposed'))
GO

alter view [dbo].[rvw_AssetTransfer]
		as 
			select * from TransactionLineItemViewForTransfer where TransactionTypeID=5
GO
update ReportTemplateTable set ReporttemplateCategoryID=(select ReportTemplateCategoryID from ReportTemplateCategoryTable where ReportTemplateCategoryName='Standard Report')
where ReporttemplateCategoryID is null
go 
CREATE TABLE [dbo].[ScreenFilterTypeTable](
	[ScreenFilterTypeID] [tinyint] NOT NULL,
	[ScreenFilterTypeCode] [varchar](50) NOT NULL,
	[ScreenFilterTypeName] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ScreenFilterTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'ParentID' AND Object_ID = Object_ID(N'ScreenFiltertable'))
Begin 
alter table ScreenFiltertable add  ParentID  int null
END
GO

IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'ParentType' AND Object_ID = Object_ID(N'ScreenFiltertable'))
Begin 
alter table ScreenFiltertable add  ParentType varchar(250)
END
GO
IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'ReportTemplateID' AND Object_ID = Object_ID(N'ScreenFiltertable'))
Begin 
alter table ScreenFiltertable add  ReportTemplateID  int null foreign key references ReportTemplateTable(ReportTemplateID)
END
GO

IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'DisplayName' AND Object_ID = Object_ID(N'ScreenFilterLineItemTable'))
Begin 
alter table ScreenFilterLineItemTable add  DisplayName varchar(250)
END
GO

IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'IsFixedFilter' AND Object_ID = Object_ID(N'ScreenFilterLineItemTable'))
Begin 
alter table ScreenFilterLineItemTable add  IsFixedFilter bit default(0)
END
GO
IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'IsMandatory' AND Object_ID = Object_ID(N'ScreenFilterLineItemTable'))
Begin 
alter table ScreenFilterLineItemTable add  IsMandatory bit default(0)
END
GO
IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'OrderNo' AND Object_ID = Object_ID(N'ScreenFilterLineItemTable'))
Begin 
alter table ScreenFilterLineItemTable add  OrderNo int
END
GO
IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'ScreenFilterTypeID' AND Object_ID = Object_ID(N'ScreenFilterLineItemTable'))
Begin 
alter table ScreenFilterLineItemTable add  ScreenFilterTypeID tinyint 
END
GO


CREATE TABLE [dbo].[ReportTemplateFileTable](
	[ReportTemplateFileID] [int] IDENTITY(1,1) NOT NULL,
	[ReportTemplateFileName] [nvarchar](150) NOT NULL,
	[StatusID] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDateTime] [smalldatetime] NOT NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDateTime] [smalldatetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ReportTemplateFileID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ReportTemplateFileTable] ADD  DEFAULT ((5)) FOR [StatusID]
GO
ALTER TABLE [dbo].[ReportTemplateFileTable] ADD  DEFAULT (getdate()) FOR [CreatedDateTime]
GO
ALTER TABLE [dbo].[ReportTemplateFileTable]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[User_LoginUserTable] ([UserID])
GO
ALTER TABLE [dbo].[ReportTemplateFileTable]  WITH CHECK ADD FOREIGN KEY([LastModifiedBy])
REFERENCES [dbo].[User_LoginUserTable] ([UserID])
GO
ALTER TABLE [dbo].[ReportTemplateFileTable]  WITH CHECK ADD  CONSTRAINT [FK_ReportTemplateFileTable_StatusID] FOREIGN KEY([StatusID])
REFERENCES [dbo].[StatusTable] ([StatusID])
GO
ALTER TABLE [dbo].[ReportTemplateFileTable] CHECK CONSTRAINT [FK_ReportTemplateFileTable_StatusID]
GO

update ScreenFilterTable set ReportTemplateID=6,parentid=6 where ScreenFilterName='AssetSummaryReport'
update ScreenFilterTable set ReportTemplateID=2,parentid=2 where ScreenFilterName='DisposedAssetReport'
update ScreenFilterTable set ReportTemplateID=9,parentid=9 where ScreenFilterName='AssetTransferReport'
if not exists(select screenfiltertypeCode from ScreenFilterTypeTable where ScreenFilterTypeName='Single Filter')
Begin 
insert into ScreenFilterTypeTable(ScreenFilterTypeID,ScreenFilterTypeCode,ScreenFilterTypeName)
values(1,1,'Single Filter')
end 
go 
if not exists(select screenfiltertypeCode from ScreenFilterTypeTable where ScreenFilterTypeName='Range Filter')
Begin 
insert into ScreenFilterTypeTable(ScreenFilterTypeID,ScreenFilterTypeCode,ScreenFilterTypeName)
values(2,2,'Range Filter')
end 
go 
update ScreenFilterLineItemTable set ScreenFilterTypeID=1,IsFixedFilter=0,IsMandatory=0

update a set 
a.OrderNo=b.Row_Number
from
ScreenFilterLineItemTable a 
join (
select *, ROW_NUMBER() OVER(Partition by screenfilterid ORDER BY FieldName) AS Row_Number from ScreenFilterLineItemTable) b on a.ScreenFilterLineItemID=b.ScreenFilterLineItemID
go 

if not exists(select RightName from User_RightTable where RightName='Period')
begin
 Insert into User_RightTable(RightName,RightDescription,ValueType,DisplayRight,RightGroupID,IsActive,IsDeleted)
 select 'Period','Period',95,1,RightGroupID,1,0
 from User_RightGroupTable where RightGroupName='Master'
end 
go 
if not exists(select MenuName from User_MenuTable where MenuName='Period' and ParentTransactionID=1)
Begin 
   insert into User_MenuTable(MenuName,RightID,TargetObject,ParentMenuID,OrderNo,ParentTransactionID)
   select 'Period',RightID,'/MasterPage/Index?pageName=Period',(select menuid from User_MenuTable where MenuName='Master'),6,1
   from User_RightTable where RightName='Period'
end 
go 

if not exists(select RightName from User_RightTable where RightName='DepreciationClass')
begin
 Insert into User_RightTable(RightName,RightDescription,ValueType,DisplayRight,RightGroupID,IsActive,IsDeleted)
 select 'DepreciationClass','DepreciationClass',95,1,RightGroupID,1,0
 from User_RightGroupTable where RightGroupName='Master'
end 
go 
if not exists(select MenuName from User_MenuTable where MenuName='DepreciationClass' and ParentTransactionID=1)
Begin 
   insert into User_MenuTable(MenuName,RightID,TargetObject,ParentMenuID,OrderNo,ParentTransactionID)
   select 'DepreciationClass',RightID,'/DepreciationClass/Index?pageName=DepreciationClass',(select menuid from User_MenuTable where MenuName='Master'),6,1
   from User_RightTable where RightName='DepreciationClass'
end 
go 
if not exists(select RightName from User_RightTable where RightName='Depreciation')
begin
 Insert into User_RightTable(RightName,RightDescription,ValueType,DisplayRight,RightGroupID,IsActive,IsDeleted)
 select 'Depreciation','Depreciation',95,1,RightGroupID,1,0
 from User_RightGroupTable where RightGroupName='Transaction'
end 
go 
if not exists(select MenuName from User_MenuTable where MenuName='Depreciation' and ParentTransactionID=1)
Begin 
   insert into User_MenuTable(MenuName,RightID,TargetObject,ParentMenuID,OrderNo,ParentTransactionID)
   select 'Depreciation',RightID,'/Depreciation/Index',(select menuid from User_MenuTable where MenuName='Transaction'),6,1
   from User_RightTable where RightName='Depreciation'
end 
go 
If not exists(select MethodName from DepreciationMethodTable where MethodName='MM')
Begin 
insert into DepreciationMethodTable(MethodName,Description)
values('MM','Monthly depreciation based on months')
End 
Go 
If not exists(select MethodName from DepreciationMethodTable where MethodName='MD')
Begin 
insert into DepreciationMethodTable(MethodName,Description)
values('MD','Monthly depreciation based on actual days')
End 
Go 
If not exists(select MethodName from DepreciationMethodTable where MethodName='MYP')
Begin 
insert into DepreciationMethodTable(MethodName,Description)
values('MYP','Monthly Depreciation based on yearly percentage ')
End 
Go 
If not exists(Select MastergridName from MasterGridNewTable where MasterGridName='Depreciation')
Begin
 insert into MasterGridNewTable(MasterGridName,EntityName)
 values('Depreciation','DepreciationLineItemTable')
End
go 
if not exists(select Fieldname from MasterGridNewLineItemTable where FieldName='Asset.Barcode' and MasterGridID=(select MasterGridID from  MasterGridNewTable where MasterGridName='Depreciation'))
Begin 
insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
Select 'Asset.Barcode','Barcode',200,NULL,1,1,'System.String',NULL,MasterGridID
From MasterGridNewTable where MasterGridName='Depreciation'
end 
go 
if not exists(select Fieldname from MasterGridNewLineItemTable where FieldName='Asset.AssetCode' and MasterGridID=(select MasterGridID from  MasterGridNewTable where MasterGridName='Depreciation'))
Begin 
insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
Select 'Asset.AssetCode','AssetCode',200,NULL,1,2,'System.String',NULL,MasterGridID
From MasterGridNewTable where MasterGridName='Depreciation'
end 
go 
if not exists(select Fieldname from MasterGridNewLineItemTable where FieldName='Product.ProductName' and MasterGridID=(select MasterGridID from  MasterGridNewTable where MasterGridName='Depreciation'))
Begin 
insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
Select 'Asset.Product.ProductName','ProductName',200,NULL,1,3,'System.String',NULL,MasterGridID
From MasterGridNewTable where MasterGridName='Depreciation'
end 
go 
if not exists(select Fieldname from MasterGridNewLineItemTable where FieldName='Category.CategoryName' and MasterGridID=(select MasterGridID from  MasterGridNewTable where MasterGridName='Depreciation'))
Begin 
insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
Select 'Category.CategoryName','CategoryName',200,NULL,1,4,'System.String',NULL,MasterGridID
From MasterGridNewTable where MasterGridName='Depreciation'
end 
go 
if not exists(select Fieldname from MasterGridNewLineItemTable where FieldName='Category.CategoryName' and MasterGridID=(select MasterGridID from  MasterGridNewTable where MasterGridName='Depreciation'))
Begin 
insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
Select 'Category.CategoryName','CategoryName',200,NULL,1,4,'System.String',NULL,MasterGridID
From MasterGridNewTable where MasterGridName='Depreciation'
end 
go 
if not exists(select Fieldname from MasterGridNewLineItemTable where FieldName='Asset.PurchasePrice' and MasterGridID=(select MasterGridID from  MasterGridNewTable where MasterGridName='Depreciation'))
Begin 
insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
Select 'Asset.PurchasePrice','PurchasePrice',200,NULL,1,5,'System.DateTime',NULL,MasterGridID
From MasterGridNewTable where MasterGridName='Depreciation'
end 
go 
if not exists(select Fieldname from MasterGridNewLineItemTable where FieldName='DepreciationLineItem.DepreciationValue' and MasterGridID=(select MasterGridID from  MasterGridNewTable where MasterGridName='Depreciation'))
Begin 
insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
Select 'DepreciationValue','DepreciationValue',200,NULL,1,6,'System.Decimal',NULL,MasterGridID
From MasterGridNewTable where MasterGridName='Depreciation'
end 
go 
if not exists(select Fieldname from MasterGridNewLineItemTable where FieldName='DepreciationLineItem.AssetValueAfterDepreciation' and MasterGridID=(select MasterGridID from  MasterGridNewTable where MasterGridName='Depreciation'))
Begin 
insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
Select 'AssetValueAfterDepreciation','AssetValueAfterDepreciation',200,NULL,1,7,'System.Decimal',NULL,MasterGridID
From MasterGridNewTable where MasterGridName='Depreciation'
end 
go 
if not exists(select Fieldname from MasterGridNewLineItemTable where FieldName='DepreciationLineItem.AssetValueBeforeDepreciation' and MasterGridID=(select MasterGridID from  MasterGridNewTable where MasterGridName='Depreciation'))
Begin 
insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
Select 'AssetValueBeforeDepreciation','AssetValueBeforeDepreciation',200,NULL,1,8,'System.Decimal',NULL,MasterGridID
From MasterGridNewTable where MasterGridName='Depreciation'
end 
go 


alter table DepreciationTable add IsUpdateEquation bit default(0)
go 
alter table DepreciationTable add IsUpdateIMALL bit default(0)
go 
create view [dbo].[rvw_AssetRetirement] 
as 
Select * from Assetview where StatusID  in (250)
GO
if not exists(select RightName from User_RightTable where RightName='BarcodePrinting')
begin
 Insert into User_RightTable(RightName,RightDescription,ValueType,DisplayRight,RightGroupID,IsActive,IsDeleted)
 select 'BarcodePrinting','BarcodePrinting',95,1,RightGroupID,1,0
 from User_RightGroupTable where RightGroupName='BarcodePrinting'
end 
go 
if not exists(select MenuName from User_MenuTable where MenuName='BarcodePrinting' and ParentTransactionID=1)
Begin 
   insert into User_MenuTable(MenuName,RightID,TargetObject,ParentMenuID,OrderNo,ParentTransactionID)
   select 'BarcodePrinting',RightID,'/BarcodePrinting/Index',(select menuid from User_MenuTable where MenuName='BarcodePrinting' and rightid is null ),6,1
   from User_RightTable where RightName='BarcodePrinting'
end 
go
if not exists(select mastergridName from MasterGridNewTable where MasterGridName='BarcodePrinting') 
Begin 
insert into MasterGridNewTable(MasterGridName,EntityName)
values('BarcodePrinting','AssetNewView')
End 
Go 

insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
select COLUMN_NAME,COLUMN_NAME,'100', case when DATA_TYPE='smalldatetime' then 'dd/MM/yyyy' else NULL end,1,ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS displayOrderID,
case when  DATA_TYPE='smalldatetime' or  DATA_TYPE='date' then 'System.DateTime' 
when DATA_TYPE='varchar' or  DATA_TYPE='nvarchar' then 'System.String' 
when DATA_TYPE='decimal' then 'System.Decimal' else DATA_TYPE end ,NULL,(select MasterGridID from MasterGridNewTable where MasterGridName='BarcodePrinting')
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME='AssetNewView' 
 and COLUMN_NAME not like 'Attribute%' and COLUMN_NAME not like 'Create%'  and COLUMN_NAME not like 'Last%'
 and COLUMN_NAME not like 'Status%' and COLUMN_NAME not in (replace(table_name,'NewView','ID'))
 and COLUMN_NAME not like 'DOF%' and DATA_TYPE not in ('int','bit') and COLUMN_NAME not like 'CategoryL%'
 and COLUMN_NAME not like 'LocationL%' and COLUMN_NAME not like '%Attribute%'and COLUMN_NAME not like '%path%'
 and COLUMN_NAME not in ('ERPUpdateType','QFAssetCode','SyncDateTime','DistributionID')
 go 
 if not exists(select MenuName from User_MenuTable where MenuName='BarcodeLabel' and ParentTransactionID=1)
Begin 
   insert into User_MenuTable(MenuName,RightID,TargetObject,ParentMenuID,OrderNo,ParentTransactionID)
   select 'BarcodeLabel',RightID,'/MasterPage/Index?pageName=BarcodeFormats',(select menuid from User_MenuTable where MenuName='Master'),6,1
   from User_RightTable where RightName='BarcodeFormat'
end 
go 

if not exists(select MenuName from User_MenuTable where MenuName='ConstructBarcode' and ParentTransactionID=1)
Begin 
   insert into User_MenuTable(MenuName,RightID,TargetObject,ParentMenuID,OrderNo,ParentTransactionID)
   select 'ConstructBarcode',RightID,'/ConstructBarcode/Index',(select menuid from User_MenuTable where MenuName='BarcodePrinting' and rightid is null ),6,1
   from User_RightTable where RightName='ConstructBarcode'
end 
go
if not exists(select MenuName from User_MenuTable where MenuName='DepreciationApproval' and ParentTransactionID=1)
Begin 
   insert into User_MenuTable(MenuName,RightID,TargetObject,ParentMenuID,OrderNo,ParentTransactionID)
   select 'DepreciationApproval',RightID,'/DepreciationApproval/Index',(select menuid from User_MenuTable where MenuName='Approval' and rightid is null ),6,1
   from User_RightTable where RightName='DepreciationApproval'
end 
go

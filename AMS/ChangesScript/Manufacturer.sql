IF OBJECT_ID('ManufacturerTable') IS NULL
BEGIN
  Create table ManufacturerTable
  (
    ManufacturerID int not null primary key identity(1,1),
	ManufacturerCode nvarchar(100) Not NULL,
    ManufacturerName nvarchar(max) Not NULL,
	CategoryID int null foreign key references CategoryTable(CategoryID),
	StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL,
	 Attribute1 nvarchar(200) NULL,
	 Attribute2 nvarchar(200) NULL,
	 Attribute3 nvarchar(200) NULL,
	 Attribute4 nvarchar(200) NULL,
	 Attribute5 nvarchar(200) NULL,
	 Attribute6 nvarchar(200) NULL,
	 Attribute7 nvarchar(200) NULL,
	 Attribute8 nvarchar(200) NULL,
	 Attribute9 nvarchar(200) NULL,
	 Attribute10 nvarchar(200) NULL,
	 Attribute11 nvarchar(200) NULL,
	 Attribute12 nvarchar(200) NULL,
	 Attribute13 nvarchar(200) NULL,
	 Attribute14 nvarchar(200) NULL,
	 Attribute15 nvarchar(200) NULL,
	 Attribute16 nvarchar(200) NULL
  )
End 
Go 
IF OBJECT_ID('ManufacturerDescriptionTable') IS NULL
BEGIN
  Create table ManufacturerDescriptionTable
  (
    ManufacturerDescriptionID int not null primary key identity(1,1),
	ManufacturerID int not null foreign key references ManufacturerTable(ManufacturerID),
	ManufacturerDescription nvarchar(max) not null,
	LanguageID int not null foreign key references LanguageTable(LanguageID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL
  )
End 
Go 
EXEC sp_RENAME 'ModelTable.PartyID' , 'ManufacturerID', 'COLUMN'
go
Alter table ModelTable drop constraint FK__ModelTabl__Party__3AD6B8E2
go 
Alter table ModelTable Add constraint FK__ModelTabl__Manuf__6ADAD1BF  Foreign key (ManufacturerID) references ManufacturerTable(ManufacturerID)
go 
Alter table AssetTable drop constraint FK_AssetTable_ManufacturerTable
go 
Alter table AssetTable add constraint FK_AssetTable_ManufacturerTable Foreign key (ManufacturerID) references ManufacturerTable(ManufacturerID)


If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='Manufacturer')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('Manufacturer','Manufacturer',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Master'),1,0);
End
Go

If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='Manufacturer' and ParentTransactionID=1)
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO,ParentTransactionID) Values(
'Manufacturer',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='Manufacturer'),'/MasterPage/Index?pageName=Manufacturer',
(Select MenuID from USER_MENUTABLE where MenuName='Master' ),10,1);
End
Go

Alter table ManufacturerTable add ManufacturerName nvarchar(max) NULL
go 
update A
set a.ManufacturerName=b.ManufacturerDescription

from ManufacturerTable a join ManufacturerDescriptionTable b on a.ManufacturerID=b.ManufacturerID

go 
Alter table ManufacturerTable alter column  ManufacturerName nvarchar(max) not NULL
go 


ALTER PROC [dbo].[IPRC_ExceImportManufacturer]
(
	@ManufacturerCode nvarchar(50)=null,
	@ManufacturerDescription nvarchar(200),
	@CategoryCode	nvarchar(50)=null,
	@ImportTypeID int,
	@UserID int=1,
	@WebSite nvarchar(200)=null,
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
	Declare @ManufacturerID int=null;
	DECLARE @ConfigValue nvarchar(max);
	Declare @ImportExcelNotAllowCreateReferenceFieldNewEntry nvarchar(10)
	Declare @ManufacturerCategoryMapping nvarchar(10)


	Select @ImportExcelNotAllowCreateReferenceFieldNewEntry=ConfiguarationValue from configurationTable where ConfiguarationName='ImportExcelNotAllowCreateReferenceFieldNewEntry'
	Select @ManufacturerCategoryMapping=ConfiguarationValue from configurationTable where ConfiguarationName='@ManufacturerCategoryMapping'
		set @CategoryCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@CategoryCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
	set @ManufacturerCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@ManufacturerCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
	set @ManufacturerDescription=REPLACE(@ManufacturerDescription,'''','''')
		if(@ImportTypeID=1)
		BEGIN

		if(@CategoryCode is not null and @CategoryCode!='')
			BEGIN			
				--Insert Department
				if(@ImportExcelNotAllowCreateReferenceFieldNewEntry='false')
				Begin
				IF NOT EXISTS (SELECT * FROM CategoryTable where CategoryCode=@CategoryCode and StatusID=1)
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
				   select @CategoryID=CategoryID from CategoryTable where CategoryCode=@CategoryCode  and StatusID=1
				   if(@CategoryCode is not null and @CategoryID is null)

				   return @CategoryCode+'- Category Is not available;'
				END
				END
				Else
				IF NOT EXISTS (SELECT * FROM CategoryTable where CategoryCode=@CategoryCode  and StatusID=1)
				BEGIN
				  return @CategoryCode+'- Category Is not available;'
				  END				 
			END
			ELSE
			IF(@ManufacturerCategoryMapping='true')
			BEGIN
			  return 'Category Is not available;'
			END
			

		if NOT EXISTS (SELECT * FROM ManufacturerTable WHERE ManufacturerCode = @ManufacturerCode  and StatusID=1)
				BEGIN
			Select @ConfigValue=ConfiguarationValue from configurationTable where ConfiguarationName='MasterCodeAutoGenerate'
			IF(@ConfigValue='false')
			BEGIN
				INSERT INTO ManufacturerTable(ManufacturerCode,CategoryID,StatusID, CreatedBy,CreatedDateTime,WebSite,
			Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6,Attribute7,Attribute8,Attribute9,Attribute10,Attribute11,Attribute12,
			Attribute13,Attribute14,Attribute15,Attribute16,ManufacturerName) values(@Manufacturercode,@CategoryID,1,@UserID,getDate(),@WebSite,@Attribute1,@Attribute2,@Attribute3,@Attribute4,@Attribute5,
			@Attribute6,@Attribute7,@Attribute8,@Attribute9,@Attribute10,@Attribute11,@Attribute12,
			@Attribute13,@Attribute14,@Attribute15,@Attribute16,@ManufacturerDescription)

				--Insert Manufacturer Description
					Insert into ManufacturerDescriptionTable(ManufacturerID,ManufacturerDescription,LanguageID,CreatedBy,CreatedDateTime)
						Select C.ManufacturerID,@ManufacturerDescription,L.LanguageID,C.CreatedBy,C.CreatedDateTime From ManufacturerTable C join 
						LanguageTable L on 1=1 left join ManufacturerDescriptionTable MD on MD.ManufacturerID=C.ManufacturerID and MD.languageid=L.languageID 
						where MD.LanguageID is null
						and C.ManufacturerCode=@ManufacturerCode
			END
			ELSE
			BEGIN
			
	INSERT INTO ManufacturerTable(ManufacturerCode,CategoryID,StatusID, CreatedBy,CreatedDateTime,WebSite,
			Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6,Attribute7,Attribute8,Attribute9,Attribute10,Attribute11,Attribute12,
			Attribute13,Attribute14,Attribute15,Attribute16,ManufacturerName) values('-',@CategoryID,1,@UserID,getDate(),@WebSite,@Attribute1,@Attribute2,@Attribute3,@Attribute4,@Attribute5,
			@Attribute6,@Attribute7,@Attribute8,@Attribute9,@Attribute10,@Attribute11,@Attribute12,
			@Attribute13,@Attribute14,@Attribute15,@Attribute16,@ManufacturerDescription)
			
			set @ManufacturerID=@@IDENTITY;
		--Insert Manufacturer Description
					Insert into ManufacturerDescriptionTable(ManufacturerID,ManufacturerDescription,LanguageID,CreatedBy,CreatedDateTime)
						Select C.ManufacturerID,@ManufacturerDescription,L.LanguageID,C.CreatedBy,C.CreatedDateTime From ManufacturerTable C join 
						LanguageTable L on 1=1 left join ManufacturerDescriptionTable MD on MD.ManufacturerID=C.ManufacturerID and MD.languageid=L.languageID 
						where MD.LanguageID is null
						and C.ManufacturerID=@ManufacturerID
			END
			End
			Else
				   SET @ErrorToBeReturned = @ManufacturerCode+'- already Exists;'


		End
			ELSE if(@ImportTypeID=2)
		BEGIN		
				UPDATE A SET 
					ManufacturerDescription = @ManufacturerDescription					
					from ManufacturerDescriptionTable A join ManufacturerTable B on A.ManufacturerID=B.ManufacturerID
				WHERE B.ManufacturerCode = @ManufacturerCode

				Update ManufacturerTable set Attribute1=@Attribute1,Attribute2=@Attribute2,Attribute3=@Attribute3,Attribute4=@Attribute4,Attribute5=@Attribute5,
				Attribute6=@Attribute6,Attribute7=@Attribute7,Attribute8=@Attribute8,Attribute9=@Attribute9,Attribute10=@Attribute10,Attribute11=@Attribute11,
				Attribute12=@Attribute12,Attribute13=@Attribute13,Attribute14=@Attribute14,Attribute15=@Attribute15,Attribute16=@Attribute16 
				,manufacturerName=@ManufacturerDescription
				
				where ManufacturerCode=@ManufacturerCode
		END
	SELECT @ErrorToBeReturned ErrorToBeReturned;
END  
go 



ALTER PROC [dbo].[IPRC_ExceImportModel]
(
	@ModelCode nvarchar(50)=null,
	@ModelDescription nvarchar(200),
	@CategoryCode	nvarchar(50)=null,
	@ManufacturerCode	nvarchar(50)=null,
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
	Declare @ManufacturerID int=null;
	Declare @ModelID int=null;
	DECLARE @ConfigValue nvarchar(max);
	Declare @ImportExcelNotAllowCreateReferenceFieldNewEntry nvarchar(10)
	Declare @ModelManufacturerCategoryMapping nvarchar(10)


	Select @ImportExcelNotAllowCreateReferenceFieldNewEntry=ConfiguarationValue from configurationTable where ConfiguarationName='ImportExcelNotAllowCreateReferenceFieldNewEntry'
	Select @ModelManufacturerCategoryMapping=ConfiguarationValue from configurationTable where ConfiguarationName='ModelManufacturerCategoryMapping'

	set @ModelCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@ModelCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
		set @ManufacturerCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@ManufacturerCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
		set @CategoryCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@CategoryCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
		set @ModelDescription=REPLACE(@ModelDescription,'''','''')
		if(@ImportTypeID=1)
		BEGIN

		if(@CategoryCode is not null and @CategoryCode!='')
			BEGIN			
				--Insert Department
				if(@ImportExcelNotAllowCreateReferenceFieldNewEntry='false')
				Begin
				IF NOT EXISTS (SELECT * FROM CategoryTable where CategoryCode=@CategoryCode and StatusID=1)
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
				   select @CategoryID=CategoryID from CategoryTable where CategoryCode=@CategoryCode  and StatusID=1
				   if(@CategoryCode is not null and @CategoryID is null)

				   return @CategoryCode+'- Category Is not available;'
				END
				END
				Else
				IF NOT EXISTS (SELECT * FROM CategoryTable where CategoryCode=@CategoryCode)
				BEGIN
				  return @CategoryCode+'- Category Is not available;'
				  END				 
			END
			ELSE
			IF(@ModelManufacturerCategoryMapping='true')
			BEGIN
			  return 'Category Is not available;'
			END



			if(@ManufacturerCode is not null and @ManufacturerCode!='')
			BEGIN
				IF NOT EXISTS (SELECT * FROM ManufacturerTable where ManufacturerCode=@ManufacturerCode  and StatusID=1)
				BEGIN
					INSERT INTO ManufacturerTable(ManufacturerCode,StatusID, CreatedBy,CategoryID,ManufacturerName)
						VALUES(@ManufacturerCode, 1, @UserID,@CategoryCode,@ManufacturerCode)
						set @ManufacturerID=@@IDENTITY;

						Insert into ManufacturerDescriptionTable(ManufacturerID,ManufacturerDescription,LanguageID,CreatedBy,CreatedDateTime)
						Select C.ManufacturerID,c.ManufacturerCode,L.LanguageID,C.CreatedBy,C.CreatedDateTime From ManufacturerTable C join 
								LanguageTable L on 1=1 left join ManufacturerDescriptionTable MD on MD.ManufacturerID=C.ManufacturerID and MD.languageid=L.languageID 
								where MD.LanguageID is null
								 and C.ManufacturerCode=@ManufacturerCode
				END
				ELSE
				BEGIN
				   select @ManufacturerID=ManufacturerID from ManufacturerTable where ManufacturerCode=@ManufacturerCode  and StatusID=1
				   if(@ManufacturerCode is not null and @ManufacturerID is null)

				   return @ManufacturerCode+'- Manufacturer Is not available;'
				END				
								 
			END
			Else
				IF NOT EXISTS (SELECT * FROM CategoryTable where CategoryCode=@CategoryCode)
				BEGIN
				  return @ManufacturerCode+'- Manufacturer Is not available;'
				 END

		if NOT EXISTS (SELECT * FROM ModelTable WHERE ModelCode = @ModelCode  and StatusID=1)
				BEGIN
			Select @ConfigValue=ConfiguarationValue from configurationTable where ConfiguarationName='MasterCodeAutoGenerate'
			IF(@ConfigValue='false')
			BEGIN
				INSERT INTO ModelTable(ModelCode,ManufacturerID,CategoryID,StatusID, CreatedBy,CreatedDateTime,
			Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6,Attribute7,Attribute8,Attribute9,Attribute10,Attribute11,Attribute12,
			Attribute13,Attribute14,Attribute15,Attribute16,ModelName) values(@Modelcode,@ManufacturerID,@CategoryID,1,@UserID,getDate(),@Attribute1,@Attribute2,@Attribute3,@Attribute4,@Attribute5,
			@Attribute6,@Attribute7,@Attribute8,@Attribute9,@Attribute10,@Attribute11,@Attribute12,
			@Attribute13,@Attribute14,@Attribute15,@Attribute16,@ModelDescription)

				--Insert Model Description
					Insert into ModelDescriptionTable(ModelID,ModelDescription,LanguageID,CreatedBy,CreatedDateTime)
						Select C.ModelID,@ModelDescription,L.LanguageID,C.CreatedBy,C.CreatedDateTime From ModelTable C join 
						LanguageTable L on 1=1 left join ModelDescriptionTable MD on MD.ModelID=C.ModelID and MD.languageid=L.languageID 
						where MD.LanguageID is null
						and C.ModelCode=@ModelCode
			END
			ELSE
			BEGIN
			
	INSERT INTO ModelTable(ModelCode,ManufacturerID,CategoryID,StatusID, CreatedBy,CreatedDateTime,
			Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6,Attribute7,Attribute8,Attribute9,Attribute10,Attribute11,Attribute12,
			Attribute13,Attribute14,Attribute15,Attribute16,ModelName) values('-', @ManufacturerID,@CategoryID,1,@UserID,getDate(),@Attribute1,@Attribute2,@Attribute3,@Attribute4,@Attribute5,
			@Attribute6,@Attribute7,@Attribute8,@Attribute9,@Attribute10,@Attribute11,@Attribute12,
			@Attribute13,@Attribute14,@Attribute15,@Attribute16,@ModelDescription)

			set @ModelID=@@IDENTITY;
		--Insert Model Description
					Insert into ModelDescriptionTable(ModelID,ModelDescription,LanguageID,CreatedBy,CreatedDateTime)
						Select C.ModelID,@ModelDescription,L.LanguageID,C.CreatedBy,C.CreatedDateTime From ModelTable C join 
						LanguageTable L on 1=1 left join ModelDescriptionTable MD on MD.ModelID=C.ModelID and MD.languageid=L.languageID 
						where MD.LanguageID is null
						and C.ModelID=@ModelID
			END
			End
			Else
				   SET @ErrorToBeReturned = @ModelCode+'- already Exists;'


		End
			ELSE if(@ImportTypeID=2)
		BEGIN		
				UPDATE A SET 
					ModelDescription = @ModelDescription					
					from ModelDescriptionTable A join ModelTable B on A.ModelID=B.ModelID
				WHERE B.ModelCode = @ModelCode

				Update ModelTable set Attribute1=@Attribute1,Attribute2=@Attribute2,Attribute3=@Attribute3,Attribute4=@Attribute4,Attribute5=@Attribute5,
				Attribute6=@Attribute6,Attribute7=@Attribute7,Attribute8=@Attribute8,Attribute9=@Attribute9,Attribute10=@Attribute10,Attribute11=@Attribute11,
				Attribute12=@Attribute12,Attribute13=@Attribute13,Attribute14=@Attribute14,Attribute15=@Attribute15,Attribute16=@Attribute16 
				,ModelName=@ModelDescription
				where ModelCode=@ModelCode
		END
	SELECT @ErrorToBeReturned ErrorToBeReturned;
END  
go 

ALTER Procedure [dbo].[IPRC_ExceImportBulkAssets]
(
	@AssetCode nvarchar(100)=Null,
	@Barcode nvarchar(100)=Null,
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
	@AssetDescription nvarchar(max)=NULL,
	@AssetConditionCode nvarchar(100)=NULL,
	@PurchasePrice nvarchar(100)=NULL,
	@DeliveryNote nvarchar(100)=null,
	@RFIDTagCode nvarchar(100)=null,
	@SupplierCode nvarchar(100)=null,
	@AssetRemarks nvarchar(max)=null,
	@InvoiceNo nvarchar(100)=null,
	@InvoiceDate nvarchar(100)=null,
	@ManufacturerCode nvarchar(100)=NULL,
	@ComissionDate nvarchar(100)=null,
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
	@Attribute40 nvarchar(100)=NULL
	
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
set @ProductCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@AssetDescription, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
IF(@PurchaseDate='')
BEGIN
set @PurchaseDate=null
END

 	--Declartion part 
	Declare @DepartmentID int,@SectionID int,@CustodianID int,@SupplierID int,@AssetConditionID int, @CategoryID int, @ManufacturerID int,@ModelID int ,@ProductID int ,@LocationID int 
	Declare @ManufacturerCategoryMapping nvarchar(50),@CategoryManufacturerModelMapping nvarchar(50),@ImportExcelNotAllowCreateReferenceFieldNewEntry nvarchar(50) ,@BarcodeEqualAssetCode nvarchar(50),
	@IsBarcodeSettingApplyImportAsset nvarchar(50),@BarcodeAutoGenerateEnable nvarchar(50),@LocationMandatory nvarchar(50),@ReturnMessage NVARCHAR(MAX)
	DECLARE @MESSAGETABLE TABLE(TEXT NVARCHAR(MAX))
	select @ManufacturerCategoryMapping=ConfiguarationValue From ConfigurationTable where ConfiguarationName='ManufacturerCategoryMapping'
	select @CategoryManufacturerModelMapping=ConfiguarationValue From ConfigurationTable where ConfiguarationName='ModelManufacturerCategoryMapping'
	select @ImportExcelNotAllowCreateReferenceFieldNewEntry=ConfiguarationValue From ConfigurationTable where ConfiguarationName='ImportExcelNotAllowCreateReferenceFieldNewEntry'
	select @BarcodeEqualAssetCode=ConfiguarationValue From ConfigurationTable where ConfiguarationName='BarcodeEqualAssetCode'
	select @IsBarcodeSettingApplyImportAsset=ConfiguarationValue From ConfigurationTable where ConfiguarationName='IsBarcodeSettingApplyImportAsset'
	select @BarcodeAutoGenerateEnable=ConfiguarationValue From ConfigurationTable where ConfiguarationName='BarcodeAutoGenerateEnable'
	select @LocationMandatory=ConfiguarationValue From ConfigurationTable where ConfiguarationName='LocationMandatory'

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

	If @ImportTypeID=1
	begin
	if upper(@BarcodeAutoGenerateEnable)='TRUE' and upper(@IsBarcodeSettingApplyImportAsset)='TRUE'
	begin 
	  set @Barcode='-';
	End 
	if UPPER(@BarcodeEqualAssetCode)='TRUE' and upper(@IsBarcodeSettingApplyImportAsset)='TRUE'
	Begin 
	set @AssetCode=@Barcode
	End 
	end 
	
	--CategoryTable
	 IF (@CategoryCode is not NULL and @CategoryCode!='')
		BEGIN 		 
		  IF EXISTS(SELECT CATEGORYCODE FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1)
		  BEGIN 		 
			SELECT @CATEGORYID=CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1 
		  END
		  ELSE 
		  BEGIN
		  
		   IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
		   BEGIN 
		    INSERT INTO CATEGORYTABLE(CATEGORYCODE,STATUSID,CREATEDBY,CREATEDDATETIME,CategoryName) 
			VALUES(@CATEGORYCODE,1,@USERID,GETDATE(),@CategoryCode)			
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
		
	--LOCATUION MASTER
	 IF (@LOCATIONCODE is not NULL and @LOCATIONCODE!='')
		BEGIN 
		 IF EXISTS(SELECT LOCATIONCODE FROM LOCATIONTABLE WHERE LOCATIONCODE=@LOCATIONCODE and StatusID=1)
		  BEGIN  
			SELECT @LOCATIONID=LOCATIONID FROM LOCATIONTABLE WHERE LOCATIONCODE=@LOCATIONCODE and StatusID=1
		  END
		  ELSE 
		  BEGIN
		    IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
		   BEGIN 
		    INSERT INTO LOCATIONTABLE(LOCATIONCODE,STATUSID,CREATEDBY,CREATEDDATETIME,LocationName) 
			VALUES(@LOCATIONCODE,1,@USERID,GETDATE(),@LocationCode)
			
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

			INSERT INTO PersonTable(PersonID, PersonFirstName, PersonLastName, PersonCode, AllowLogin, DepartmentID, UserTypeID, StatusID, Culture) 
				VALUES(@@IDENTITY, @CUSTODIANCODE, @CUSTODIANCODE, @CUSTODIANCODE, 0, @DepartmentID, 2, 1, 'en-GB')
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
			 IF EXISTS(SELECT SUPPLIERCODE FROM SUPPLIERTABLE WHERE SUPPLIERCODE=@SUPPLIERCODE and StatusID=1)
			 BEGIN 
				SELECT @SUPPLIERID =SUPPLIERID FROM SUPPLIERTABLE WHERE SUPPLIERCODE=@SUPPLIERCODE and StatusID=1
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
	 IF(@ASSETDESCRIPTION is not NULL and @ASSETDESCRIPTION!='')                                                                                                                                                                                                                                                            
			BEGIN 
			 IF EXISTS(SELECT Productdescription FROM ProductDescriptionTable a join ProductTable b on a.ProductId=B.ProductID WHERE ProductCode=@ProductCode AND LANGUAGEID=1 
				AND CATEGORYID in (SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1) and StatusID=1)
			 BEGIN 
				SELECT @PRODUCTID=PRODUCTID FROM PRODUCTVIEW WHERE ProductCode=@ProductCode AND LANGUAGEID=1 and StatusID=1
				AND CATEGORYID in (SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1)
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
				 
				 INSERT INTO PRODUCTDESCRIPTIONTABLE (PRODUCTDESCRIPTION,PRODUCTID,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
				 Select @ASSETdESCRIPTION,@ProductID,LanguageID,@USERID,getdate() from LanguageTable
				

				End 
				Else 
				Begin 
				SELECT @CATEGORYID= CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE
				END  

				INSERT INTO PRODUCTTABLE(PRODUCTCODE,STATUSID,CATEGORYID,CREATEDBY,CREATEDDATETIME,ProductName)
				VALUES(@ProductCode,1,@CATEGORYID,@USERID,getdate(),@ProductCode)
				 SET @ProductID = SCOPE_IDENTITY()
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
Attribute27,Attribute28,Attribute29,Attribute30,Attribute31,Attribute32,Attribute33,Attribute34,Attribute35,Attribute36,Attribute37,Attribute38,Attribute39,Attribute40)
					
					VALUES( CASE WHEN UPPER(@BarcodeAutoGenerateEnable)='TRUE' and  UPPER(@IsBarcodeSettingApplyImportAsset)='TRUE' THEN '-' ELSE 
					CASE WHEN  UPPER(@BarcodeEqualAssetCode)='TRUE' and  UPPER(@IsBarcodeSettingApplyImportAsset)='TRUE' THEN @Barcode ELSE @AssetCode END END ,
					CASE WHEN UPPER(@BarcodeAutoGenerateEnable)='TRUE' and  UPPER(@IsBarcodeSettingApplyImportAsset)='TRUE' THEN '-' ELSE @Barcode END
					,@DepartmentID,@SECTIONID,@CUSTODIANID,@CATEGORYID,@LOCATIONID,@PRODUCTID,@MANUFACTURERID,@MODELID,@SUPPLIERID,@ASSETCONDITIONID,case when @PurchasePrice is not null and  @PurchasePrice!=' '  then convert(decimal(18,5),@PurchasePrice) else 0 end ,@PONUMBER,@REFERENCECODE,@SERIALNO,
					@ASSETDESCRIPTION,@DELIVERYNOTE,@RFIDTAGCODE,@INVOICENO,@VOUCHERNO,@MAKE,@CAPACITY,@MAPPEDASSETID,@RECEIPTNUMBER,case when @PURCHASEDATE is not null then CONVERT(DATETIME,@PURCHASEDATE,103) else NULL end ,
					case when @ComissionDate is not null then CONVERT(DATETIME,@ComissionDate,103) else null end , case when @WarrantyExpiryDate is not null then CONVERT(DATETIME,@WarrantyExpiryDate,103) else NULL end ,
					1,@companyID,@userID,getdate(),1,0,0,0,@Attribute1,@Attribute2,@Attribute3,@Attribute4,@Attribute5,
			@Attribute6,@Attribute7,@Attribute8,@Attribute9,@Attribute10,@Attribute11,@Attribute12,
			@Attribute13,@Attribute14,@Attribute15,@Attribute16,@AssetRemarks,@QFAssetCode,@Attribute17,@Attribute18,@Attribute19,@Attribute20,@Attribute21,@Attribute22,@Attribute23,@Attribute24,@Attribute25,@Attribute26,
@Attribute27,@Attribute28,@Attribute29,@Attribute30,@Attribute31,@Attribute32,@Attribute33,@Attribute34,@Attribute35,@Attribute36,@Attribute37,@Attribute38,@Attribute39,@Attribute40)
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
				ASSETDESCRIPTION=CASE WHEN @ASSETDESCRIPTION IS NULL OR RTRIM(LTRIM(@ASSETDESCRIPTION))='' THEN ASSETDESCRIPTION ELSE @ASSETDESCRIPTION END ,
				DELIVERYNOTE=CASE WHEN @DELIVERYNOTE IS NULL OR RTRIM(LTRIM(@DELIVERYNOTE))='' THEN DELIVERYNOTE ELSE @DELIVERYNOTE END,
				RFIDTAGCODE=CASE WHEN @RFIDTAGCODE IS NULL OR RTRIM(LTRIM(@RFIDTAGCODE))='' THEN RFIDTAGCODE ELSE @RFIDTAGCODE END,
				INVOICENO=CASE WHEN @INVOICENO IS NULL OR RTRIM(LTRIM(@INVOICENO))='' THEN INVOICENO ELSE @INVOICENO END,
				VOUCHERNO=CASE WHEN @VOUCHERNO IS NULL OR RTRIM(LTRIM(@VOUCHERNO))='' THEN VOUCHERNO ELSE @VOUCHERNO END,
				MAKE=CASE WHEN @MAKE IS NULL OR RTRIM(LTRIM(@MAKE))='' THEN MAKE ELSE @MAKE END,
				CAPACITY=CASE WHEN @CAPACITY IS NULL OR RTRIM(LTRIM(@CAPACITY))='' THEN CAPACITY ELSE @CAPACITY END,
				MAPPEDASSETID=CASE WHEN @MAPPEDASSETID IS NULL OR RTRIM(LTRIM(@MAPPEDASSETID))='' THEN MAPPEDASSETID ELSE @MAPPEDASSETID END,
				RECEIPTNUMBER=CASE WHEN @RECEIPTNUMBER IS NULL OR RTRIM(LTRIM(@RECEIPTNUMBER))='' THEN RECEIPTNUMBER ELSE @RECEIPTNUMBER END,
				PURCHASEDATE=CASE WHEN @PURCHASEDATE IS NULL OR RTRIM(LTRIM(@PURCHASEDATE))='' THEN PURCHASEDATE ELSE CONVERT(DATETIME,@PURCHASEDATE,103)  END,
				ComissionDate=CASE WHEN @ComissionDate IS NULL OR RTRIM(LTRIM(@ComissionDate))='' THEN ComissionDate ELSE CONVERT(DATETIME,@ComissionDate,103) END,
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
				Attribute40=CASE WHEN @Attribute40 IS NULL OR RTRIM(LTRIM(@Attribute40))='' THEN Attribute40 ELSE @Attribute40 END
				 WHERE BARCODE=@BARCODE 
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
Create Procedure iprc_ExcelPOLineItemUpdate 
(
   @Barcode   nvarchar(100) = NULL,
   @PONumber  nvarchar(100) = NULL,
   @POLineNo  nvarchar(100) = NULL,
   @ImportTypeID int,
   @UserID int,
   @LanguageID int=1,
   @CompanyID int=1003
)
as 
BEGIN 
    IF exists(select barcode from AssetTable where barcode=@Barcode) 
	BEGIN
	     update AssetTable set 
		 PONumber=@PONumber,
		 DOFPO_LINE_NUM=@POLineNo
		 Where Barcode=@Barcode
	END 
	ELSE 
	BEGIN 
	SElect @Barcode+'- given barcode not available in db . ' as ReturnMessage
				Return 
	END 
END 
Go 



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
update EntityTable set QueryString='IPRC_ExceImportDesignation' where EntityName='Designation'
go 

If not exists(select EntityName from EntityTable where EntityName='POLineItemUpdate') 
Begin
  insert into EntityTable(EntityID,EntityName,QueryString)
  select max(entityID)+1 ,'POLineItemUpdate','iprc_ExcelPOLineItemUpdate'
  from EntityTable 
End 
go 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,(Select EntityId from EntityTable where EntityName='POLineItemUpdate')
From ImportTemplateTable where DynamicColumnRequiredEntityID=(select DynamicColumnRequiredEntityID from DynamicColumnRequiredEntityTable where DynamicColumnRequiredName='POLineItemUpdate')
go 


insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,(Select EntityId from EntityTable where EntityName='Model')
From ImportTemplateTable where DynamicColumnRequiredEntityID=25
go 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,(Select EntityId from EntityTable where EntityName='Manufacturer')
From ImportTemplateTable where DynamicColumnRequiredEntityID=23	
go 




insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select 'DesignationCode',1,1,1,100,NULL,0,100,1,1,null,EntityID
from EntityTable where EntityName='Designation'
go 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select 'DesignationCode',1,1,1,100,NULL,0,100,2,1,null,EntityID
from EntityTable where EntityName='Designation'
go 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select 'DesignationDescription',1,1,1,100,NULL,0,100,1,1,null,EntityID
from EntityTable where EntityName='Designation'
go 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select 'DesignationDescription',1,1,1,100,NULL,0,100,2,1,null,EntityID
from EntityTable where EntityName='Designation'

go 


insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select 'Barcode',1,1,1,100,NULL,0,100,2,1,null,EntityID
from EntityTable where EntityName='POLineItemUpdate'
go 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select 'PONumber',2,1,1,100,NULL,0,100,2,0,null,EntityID
from EntityTable where EntityName='POLineItemUpdate'
go 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select 'POLineNo',3,1,1,100,NULL,0,100,2,0,null,EntityID
from EntityTable where EntityName='POLineItemUpdate'

insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,(Select EntityId from EntityTable where EntityName='Asset')
From ImportTemplateTable where DynamicColumnRequiredEntityID=15
if not exists(select PartyType from PartyTypeTable where PartyType='Supplier')
Begin 
  insert into PartyTypeTable(PartyTypeID,PartyTypeCode,PartyType)
  values(1,'Su','Supplier')
End 
go 
 Alter table partyTable Alter Column TradeName nvarchar(MAx) NULL
 go 
 Alter table partyTable Alter Column CountryID int NULL
 go 
  Alter table partyTable Alter Column ContactPerson nvarchar(MAx) NULL
 go 
   Alter table partyTable Alter Column Telephone1 nvarchar(MAx) NULL
 go 



 
Create PROC [dbo].[IPRC_ImportExcelSupplier]
(
	@SupplierCode nvarchar(100)=null,
	@SupplierDescription nvarchar(100),
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
	@TradeName  nvarchar(100)=null,
	@CountryCode  nvarchar(100)=null,
	@ContactPerson  nvarchar(100)=null,
	@ContactPersonMobile  nvarchar(100)=null,
	@Email  nvarchar(100)=null,
	@ContactPersonEmail  nvarchar(100)=null,
	@Telephone1  nvarchar(100)=null,
	@Telephone2 nvarchar(100)=null,
	@NotificationEmail  nvarchar(100)=null,
	@Mobile  nvarchar(100)=null,
	@Fax  nvarchar(100)=null,
	@TRNNo  nvarchar(100)=null,
	@Remark  nvarchar(100)=null,
	@TaxFileNo  nvarchar(100)=null,
	@CorporateRegistartionNo  nvarchar(100)=null
)
AS
BEGIN
	Declare @SupplierID int=null,@CountryID int 
	DECLARE @ErrorToBeReturned nvarchar(max);
	DECLARE @ConfigValue nvarchar(max);
	set @SupplierCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@SupplierCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
	set @SupplierDescription=REPLACE(@SupplierDescription,'''','''')
	If @CountryCode is not null or @CountryCode!='' 
	Begin
	  if exists(select CountryCode from CountryTable where CountryCode=@CountryCode)
	  Begin 
	     Select @CountryID=CountryID from CountryTable where CountryCode=@CountryCode 
	  end 
	  else 
	  Begin
	     Insert into CountryTable(CountryCode,StatusID,CreatedBy,CreatedDateTime)
		select @CountryCode, dbo.fnGetActiveStatusID(),@UserID,GETDATE()

	  End 
	    Select @CountryID=CountryID from CountryTable where CountryCode=@CountryCode 
	end 
		
		if(@ImportTypeID=1)
		BEGIN
		if NOT EXISTS (SELECT * FROM PartyTable where PartyCode=@SupplierCode and StatusID=1)
			BEGIN
			Select @ConfigValue=ConfiguarationValue from configurationTable where ConfiguarationName='MasterCodeAutoGenerate'
			IF(@ConfigValue='false')
			BEGIN
			insert into PartyTable(PartyCode,StatusID,CreatedBy,CreatedDateTime,
			Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6,Attribute7,Attribute8,Attribute9,Attribute10,Attribute11,Attribute12,
			Attribute13,Attribute14,Attribute15,Attribute16, TradeName,CountryID,ContactPerson,ContactPersonMobile,Email,ContactPersonEmail,Telephone1,Telephone2,
			NotificationEmail,Mobile,Fax,TRNNo,Remark,TaxFileNo,CorporateRegistartionNo,PartyTypeID) 
			select @SupplierCode,1,@UserID,getDate(),@Attribute1,@Attribute2,@Attribute3,@Attribute4,@Attribute5,
			@Attribute6,@Attribute7,@Attribute8,@Attribute9,@Attribute10,@Attribute11,@Attribute12,
			@Attribute13,@Attribute14,@Attribute15,@Attribute16,@TradeName,@CountryID,@ContactPerson,@ContactPersonMobile,@Email,
			@ContactPersonEmail,@Telephone1,@Telephone2,@NotificationEmail,@Mobile,@Fax,@TRNNo,@Remark,@TaxFileNo,@CorporateRegistartionNo,
			(Select PartyTypeID from PartyTypeTable where PartyType='Supplier')

			--Insert Asset Condition Description
				Insert into PartyDescriptionTable(PartyDescription,PartyID,LanguageID,CreatedBy,CreatedDateTime)
				Select C.PartyID,@SupplierDescription,L.LanguageID,C.CreatedBy,C.CreatedDateTime From PartyTable C join 
						LanguageTable L on 1=1 left join PartyDescriptionTable MD on MD.PartyID=C.PartyID and MD.languageid=L.languageID 
						where MD.LanguageID is null	and C.PartyCode=@SupplierCode
			END
			ELSE
			BEGIN
			
			insert into PartyTable(PartyCode,StatusID,CreatedBy,CreatedDateTime,
			Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6,Attribute7,Attribute8,Attribute9,Attribute10,Attribute11,Attribute12,
			Attribute13,Attribute14,Attribute15,Attribute16, TradeName,CountryID,ContactPerson,ContactPersonMobile,Email,ContactPersonEmail,Telephone1,Telephone2,
			NotificationEmail,Mobile,Fax,TRNNo,Remark,TaxFileNo,CorporateRegistartionNo,PartyTypeID) 
			select '-',1,@UserID,getDate(),@Attribute1,@Attribute2,@Attribute3,@Attribute4,@Attribute5,
			@Attribute6,@Attribute7,@Attribute8,@Attribute9,@Attribute10,@Attribute11,@Attribute12,
			@Attribute13,@Attribute14,@Attribute15,@Attribute16,@TradeName,@CountryID,@ContactPerson,@ContactPersonMobile,@Email,
			@ContactPersonEmail,@Telephone1,@Telephone2,@NotificationEmail,@Mobile,@Fax,@TRNNo,@Remark,@TaxFileNo,@CorporateRegistartionNo,
			(Select PartyTypeID from PartyTypeTable where PartyType='Supplier')

			set @SupplierID=@@IDENTITY;
			--Insert Asset Condition Description
				Insert into PartyDescriptionTable(PartyDescription,PartyID,LanguageID,CreatedBy,CreatedDateTime)
				Select C.PartyID,@SupplierDescription,L.LanguageID,C.CreatedBy,C.CreatedDateTime From PartyTable C join 
						LanguageTable L on 1=1 left join PartyDescriptionTable MD on MD.PartyID=C.PartyID and MD.languageid=L.languageID 
						where MD.LanguageID is null	and C.PartyCode=@SupplierCode
			END
			End
			Else
				   SET @ErrorToBeReturned = @SupplierCode+'- already Exists;'


		End
			ELSE if(@ImportTypeID=2)
		BEGIN
			UPDATE A SET 
					PartyDescription = @SupplierDescription, 
					LastModifiedDateTime = GETDATE(), 
					LastModifiedBy = @UserID
					from PartyDescriptionTable A join PartyTable B on A.PartyID=B.PartyID
				WHERE B.PartyCode = @SupplierCode

				Update PartyTable set Attribute1=@Attribute1,Attribute2=@Attribute2,Attribute3=@Attribute3,Attribute4=@Attribute4,Attribute5=@Attribute5,
				Attribute6=@Attribute6,Attribute7=@Attribute7,Attribute8=@Attribute8,Attribute9=@Attribute9,Attribute10=@Attribute10,Attribute11=@Attribute11,
				Attribute12=@Attribute12,Attribute13=@Attribute13,Attribute14=@Attribute14,Attribute15=@Attribute15,Attribute16=@Attribute16 ,
				TradeName=@TradeName,CountryID=@CountryID,ContactPerson=@ContactPerson,ContactPersonMobile=@ContactPersonMobile,Email=@Email,
				ContactPersonEmail=@ContactPersonEmail,Telephone1=@Telephone1,Telephone2=@Telephone2,
			NotificationEmail=@NotificationEmail,Mobile=@Mobile,Fax=@Fax,TRNNo=@TRNNo,Remark=@Remark,TaxFileNo=@TaxFileNo,CorporateRegistartionNo=@CorporateRegistartionNo
				where PartyCode=@Suppliercode
		END
	SELECT @ErrorToBeReturned ErrorToBeReturned;
END  
go 
update EntityTable set QueryString='IPRC_ImportExcelSupplier' where EntityName='Supplier'
select * from EntityTable
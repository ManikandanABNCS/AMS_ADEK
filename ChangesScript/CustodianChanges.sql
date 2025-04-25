delete from ImportTemplateNewTable where EntityID=(select EntityID from EntityTable where EntityName='Custodian')
go 

if not exists(select ImportField from ImportTemplateNewTable where ImportField='PersonCode' and entityid =(select EntityID from EntityTable where EntityName='Custodian') and ImportTemplateTypeID=1 )
Begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'PersonCode',1,1,1,100,NULL,0,100,1,1,NULL,EntityID
From EntityTable where EntityName='custodian'
End 
go 
if not exists(select ImportField from ImportTemplateNewTable where ImportField='FirstName' and entityid =(select EntityID from EntityTable where EntityName='Custodian') and ImportTemplateTypeID=1 )
Begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'FirstName',2,1,1,100,NULL,0,100,1,0,NULL,EntityID
From EntityTable where EntityName='custodian'
End 
go 
if not exists(select ImportField from ImportTemplateNewTable where ImportField='LastName' and entityid =(select EntityID from EntityTable where EntityName='Custodian') and ImportTemplateTypeID=1 )
Begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'LastName',3,1,1,100,NULL,0,100,1,0,NULL,EntityID
From EntityTable where EntityName='custodian'
End 
go 
if not exists(select ImportField from ImportTemplateNewTable where ImportField='DOJ' and entityid =(select EntityID from EntityTable where EntityName='Custodian') and ImportTemplateTypeID=1 )
Begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'DOJ',4,1,1,100,NULL,0,100,1,0,NULL,EntityID
From EntityTable where EntityName='custodian'
End 
go 
if not exists(select ImportField from ImportTemplateNewTable where ImportField='MobileNo' and entityid =(select EntityID from EntityTable where EntityName='Custodian') and ImportTemplateTypeID=1 )
Begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'MobileNo',5,1,1,100,NULL,0,100,1,0,NULL,EntityID
From EntityTable where EntityName='custodian'
End 
go 
if not exists(select ImportField from ImportTemplateNewTable where ImportField='Gender' and entityid =(select EntityID from EntityTable where EntityName='Custodian') and ImportTemplateTypeID=1 )
Begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'Gender',6,1,1,100,NULL,0,100,1,0,NULL,EntityID
From EntityTable where EntityName='custodian'
End 
go
if not exists(select ImportField from ImportTemplateNewTable where ImportField='DepartmentCode' and entityid =(select EntityID from EntityTable where EntityName='Custodian') and ImportTemplateTypeID=1 )
Begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'DepartmentCode',7,1,0,100,NULL,0,100,1,0,NULL,EntityID
From EntityTable where EntityName='custodian'
End 
go
if not exists(select ImportField from ImportTemplateNewTable where ImportField='EmailID' and entityid =(select EntityID from EntityTable where EntityName='Custodian') and ImportTemplateTypeID=1 )
Begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'EmailID',8,1,1,100,NULL,0,100,1,0,NULL,EntityID
From EntityTable where EntityName='custodian'
End 
go
if not exists(select ImportField from ImportTemplateNewTable where ImportField='DesignationName' and entityid =(select EntityID from EntityTable where EntityName='Custodian') and ImportTemplateTypeID=1 )
Begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'DesignationName',9,1,0,100,NULL,0,100,1,0,NULL,EntityID
From EntityTable where EntityName='custodian'
End 
go
if not exists(select ImportField from ImportTemplateNewTable where ImportField='SignatureImage' and entityid =(select EntityID from EntityTable where EntityName='Custodian') and ImportTemplateTypeID=1 )
Begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'SignatureImage',10,1,0,100,NULL,0,100,1,0,NULL,EntityID
From EntityTable where EntityName='custodian'
End 
go




if not exists(select ImportField from ImportTemplateNewTable where ImportField='PersonCode' and entityid =(select EntityID from EntityTable where EntityName='Custodian') and ImportTemplateTypeID=2 )
Begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'PersonCode',1,1,1,100,NULL,0,100,2,1,NULL,EntityID
From EntityTable where EntityName='custodian'
End 
go 
if not exists(select ImportField from ImportTemplateNewTable where ImportField='FirstName' and entityid =(select EntityID from EntityTable where EntityName='Custodian') and ImportTemplateTypeID=2 )
Begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'FirstName',2,1,0,100,NULL,0,100,2,0,NULL,EntityID
From EntityTable where EntityName='custodian'
End 
go 
if not exists(select ImportField from ImportTemplateNewTable where ImportField='LastName' and entityid =(select EntityID from EntityTable where EntityName='Custodian') and ImportTemplateTypeID=2 )
Begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'LastName',3,1,0,100,NULL,0,100,2,0,NULL,EntityID
From EntityTable where EntityName='custodian'
End 
go 
if not exists(select ImportField from ImportTemplateNewTable where ImportField='DOJ' and entityid =(select EntityID from EntityTable where EntityName='Custodian') and ImportTemplateTypeID=2 )
Begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'DOJ',4,1,0,100,NULL,0,100,2,0,NULL,EntityID
From EntityTable where EntityName='custodian'
End 
go 
if not exists(select ImportField from ImportTemplateNewTable where ImportField='MobileNo' and entityid =(select EntityID from EntityTable where EntityName='Custodian') and ImportTemplateTypeID=2 )
Begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'MobileNo',5,1,0,100,NULL,0,100,2,0,NULL,EntityID
From EntityTable where EntityName='custodian'
End 
go 
if not exists(select ImportField from ImportTemplateNewTable where ImportField='Gender' and entityid =(select EntityID from EntityTable where EntityName='Custodian') and ImportTemplateTypeID=2 )
Begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'Gender',6,1,0,100,NULL,0,100,2,0,NULL,EntityID
From EntityTable where EntityName='custodian'
End 
go
if not exists(select ImportField from ImportTemplateNewTable where ImportField='DepartmentCode' and entityid =(select EntityID from EntityTable where EntityName='Custodian') and ImportTemplateTypeID=2 )
Begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'DepartmentCode',7,1,0,100,NULL,0,100,2,0,NULL,EntityID
From EntityTable where EntityName='custodian'
End 
go
if not exists(select ImportField from ImportTemplateNewTable where ImportField='EmailID' and entityid =(select EntityID from EntityTable where EntityName='Custodian') and ImportTemplateTypeID=2 )
Begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'EmailID',8,1,0,100,NULL,0,100,2,0,NULL,EntityID
From EntityTable where EntityName='custodian'
End 
go
if not exists(select ImportField from ImportTemplateNewTable where ImportField='DesignationName' and entityid =(select EntityID from EntityTable where EntityName='Custodian') and ImportTemplateTypeID=2 )
Begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'DesignationName',9,1,0,100,NULL,0,100,2,0,NULL,EntityID
From EntityTable where EntityName='custodian'
End 
go
if not exists(select ImportField from ImportTemplateNewTable where ImportField='SignatureImage' and entityid =(select EntityID from EntityTable where EntityName='Custodian') and ImportTemplateTypeID=2 )
Begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'SignatureImage',10,1,0,100,NULL,0,100,2,0,NULL,EntityID
From EntityTable where EntityName='custodian'
End 
go

create Procedure iprc_AMSExcelImportCustodian
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

	if(@DepartmentCode is not null and @DepartmentCode!='')
			BEGIN			
				--Insert Department
				if(@ImportExcelNotAllowCreateReferenceFieldNewEntry='false')
				Begin
				IF NOT EXISTS (SELECT * FROM DepartmentTable where DepartmentCode=@DepartmentCode and statusID=@statusID)
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
				   select @DepartmentID=DepartmentID from DepartmentTable where DepartmentCode=@DepartmentCode and statusID=@statusID
				   if(@DepartmentCode is not null and @DepartmentID is null)

				   return @DepartmentCode+'- Department Is not available;'
				END
				END
				Else
				IF NOT EXISTS (SELECT * FROM DepartmentTable where DepartmentCode=@DepartmentCode and statusID=@statusID)
				BEGIN
				  return @DepartmentCode+'- Department Is not available;'
				  END				 
			END
		if(@DesignationName is not null and @DesignationName!='')
			BEGIN			
				--Insert Department
				if(@ImportExcelNotAllowCreateReferenceFieldNewEntry='false')
				Begin
				IF NOT EXISTS (SELECT * FROM DesignationTable where DEsignationName=@designationName and statusID=@statusID)
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
				   select @DesignationID=DesignationID from DesignationTable where DesignationName=@DesignationName and statusID=@statusID
				   if(@DesignationName is not null and @DesignationName is null)

				   return @DesignationName+'- Designation Is not available;'
				END
				END
				Else
				IF NOT EXISTS (SELECT * FROM DesignationTable where DesignationName=@DesignationName and statusID=@statusID)
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
					VALUES(@@IDENTITY, @FirstName, @LastName, @PersonCode, 0, @DepartmentID, 2, 1, 'en-GB',@EmailID,@MobileNo,@DesignationID,case when @DOJ is not null then CONVERT(DATETIME,@DOJ,103) else null end,@Gender,@SignatureImage)
			END
			else 
			Begin
			    INSERT INTO @MESSAGETABLE(TEXT)VALUES('Already Exists this barcode :'+@PersonCode)
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
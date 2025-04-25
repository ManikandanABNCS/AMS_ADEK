--select * from ImportTemplateNewTable
--select * from EntityTable
if not exists(select QueryString from  EntityTable where EntityName='ImportAssetTransfer')
Begin
Insert into EntityTable(EntityID,EntityName,QueryString)
select max(entityId)+1,'ImportAssetTransfer','iprc_ImportAssetTransfer'
from EntityTable   
end 
go 
if not exists(select Importfield from ImportTemplateNewTable where ImportField='Barcode' and 
		EntityID=(select EntityId from EntityTable where EntityName='ImportAssetTransfer'))
Begin 
 Insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,DataFormat,
					IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select 'Barcode',1,1,1,100,NULL,0,100,2,1,NULL,EntityID
from EntityTable where EntityName='ImportAssetTransfer'
End 
go 
if not exists(select Importfield from ImportTemplateNewTable where ImportField='ToLocationCode' and 
		EntityID=(select EntityId from EntityTable where EntityName='ImportAssetTransfer'))
Begin 
 Insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,DataFormat,
					IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select 'ToLocationCode',2,1,1,100,NULL,0,100,2,0,NULL,EntityID
from EntityTable where EntityName='ImportAssetTransfer'
End 
go 
if not exists(select Importfield from ImportTemplateNewTable where ImportField='ToCustodianCode' and 
		EntityID=(select EntityId from EntityTable where EntityName='ImportAssetTransfer'))
Begin 
 Insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,DataFormat,
					IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select 'ToCustodianCode',3,1,0,100,NULL,0,100,2,0,NULL,EntityID
from EntityTable where EntityName='ImportAssetTransfer'
End 
go 
if not exists(select Importfield from ImportTemplateNewTable where ImportField='ToDepartmentCode' and 
		EntityID=(select EntityId from EntityTable where EntityName='ImportAssetTransfer'))
Begin 
 Insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,DataFormat,
					IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select 'ToDepartmentCode',4,1,0,100,NULL,0,100,2,0,NULL,EntityID
from EntityTable where EntityName='ImportAssetTransfer'
End 
go 
if not exists(select Importfield from ImportTemplateNewTable where ImportField='ToSectionCode' and 
		EntityID=(select EntityId from EntityTable where EntityName='ImportAssetTransfer'))
Begin 
 Insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,DataFormat,
					IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select 'ToSectionCode',5,1,0,100,NULL,0,100,2,0,NULL,EntityID
from EntityTable where EntityName='ImportAssetTransfer'
End 
go 
if not exists(select Importfield from ImportTemplateNewTable where ImportField='TransferRemarks' and 
		EntityID=(select EntityId from EntityTable where EntityName='ImportAssetTransfer'))
Begin 
 Insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,DataFormat,
					IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select 'TransferRemarks',6,1,0,100,NULL,0,100,2,0,NULL,EntityID
from EntityTable where EntityName='ImportAssetTransfer'
End 
go 
if not exists(select Importfield from ImportTemplateNewTable where ImportField='ToAssetConditionCode' and 
		EntityID=(select EntityId from EntityTable where EntityName='ImportAssetTransfer'))
Begin 
 Insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,DataFormat,
					IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select 'ToAssetConditionCode',7,1,0,100,NULL,0,100,2,0,NULL,EntityID
from EntityTable where EntityName='ImportAssetTransfer'
End 
go 
if not exists(select Importfield from ImportTemplateNewTable where ImportField='TransferTypeCode' and 
		EntityID=(select EntityId from EntityTable where EntityName='ImportAssetTransfer'))
Begin 
 Insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,DataFormat,
					IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select 'TransferTypeCode',8,1,0,100,NULL,0,100,2,0,NULL,EntityID
from EntityTable where EntityName='ImportAssetTransfer'
End 
go 
if not exists(select Importfield from ImportTemplateNewTable where ImportField='TransferDate' and 
		EntityID=(select EntityId from EntityTable where EntityName='ImportAssetTransfer'))
Begin 
 Insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,DataFormat,
					IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select 'TransferDate',9,1,0,100,NULL,0,100,2,0,NULL,EntityID
from EntityTable where EntityName='ImportAssetTransfer'
End 
go 

create procedure iprc_ImportAssetTransfer
(
@Barcode			nvarchar(100) =NULL,
@ToLocationCode		nvarchar(100) =NULL,
@ToCustodianCode	nvarchar(100) =NULL,
@ToDepartmentCode	nvarchar(100) =NULL,
@ToSectionCode		nvarchar(100)=null,
@TransferRemarks	nvarchar(max)=null,
@ToAssetConditionCode	nvarchar(100)=null,
@TransferTypeCode	nvarchar(100)=null,
@TransferDate		nvarchar(100)=null,
@ImportTypeID int,
@UserID int,
@LanguageID int,
@CompanyID int	

)
as
Begin 
Declare @AssetID int ,@statusID int ,@SectionID int ,@custodianID int ,@DateFormat bit ,@TolocationID int ,@TosectionID int ,@TodepartmentID int ,@ToCustodianID int ,
	 @ToassetConditionID int ,@TransferTypeID int 
	
	IF(@TransferDate='')
	BEGIN
	set @TransferDate=null
	END
	If ((@TransferDate is not null and @TransferDate!=''))
		Begin
		  SET DATEFORMAT dmy;
		  SELECT @DateFormat= ISDATE(@TransferDate)
		  If @DateFormat=0
		   BEGIN 
		     SElect @Barcode+'- @Transfer Date is invalid' as ReturnMessage
				Return 
		   END 
		End 
	
	select @statusID=[dbo].fnGetActiveStatusID()

	Select @AssetID=AssetID from AssetTable where barcode=@barcode and statusID=@statusID
	
	Declare @ErrorID int ,@ErrorMsg nvarchar(max)
 
	exec Prc_AssetTransferValidation @userID,@AssetID,@ToLocationCode,Null,@ToDepartmentCode,NULL,'ExcelAssetTransfer',@ErrorID output,@ErrorMsg output
		if @ErrorID>0
		Begin
			 Select @ErrorMsg as ReturnMessage
			 Return
		End
	
		Select @TolocationID=LocationID from LocationTable where LocationCode=@TolocationID

			IF exists(select LocationID from LocationTable where ParentLocationID=@TolocationID)
			Begin 
			 SElect @Barcode+'- given To Location Code is not an last node. Please give the location last node.' as ReturnMessage
				Return 
			End 
		 if not exists(select DepartmentID from DepartmentTable where DepartmentCode=@ToDepartmentCode and StatusID=@statusID)
	 Begin
	  SElect @Barcode+'- given Department Code is not avaliable in db.' as ReturnMessage
	  Return 
	 End 
	 Else 
	 Begin
	 Select @ToDepartmentID = DepartmentID from DepartmentTable where DepartmentCode=@ToDepartmentCode and StatusID=@statusID
	 end  
	 if not exists(select sectionID from SectionTable where SectionCode=@ToSectionCode and StatusID=@statusID)
	 Begin
	  SElect @Barcode+'- given Section Code is not avaliable in db.' as ReturnMessage
	  Return 
	 End 
	 Else 
	 Begin
	 Select @TosectionID = sectionID from SectionTable where SectionCode=@ToSectionCode and StatusID=@statusID
	 end 
	 if not exists(select PersonID from PersonTable where PersonCode=@ToCustodianCode and StatusID=@statusID)
	 Begin
	  SElect @Barcode+'- given Custodian Code is not avaliable in db.' as ReturnMessage
	  Return 
	 End 
	 Else 
	 Begin
	 Select @ToCustodianID = PersonID from PersonTable where PersonCode=@ToCustodianCode and StatusID=@statusID
	 end 
	 if not exists(select AssetConditionCode from AssetConditionTable where AssetConditionCode=@ToAssetConditionCode and StatusID=@statusID)
	 Begin
	  SElect @Barcode+'- given Asset Condition Code is not avaliable in db.' as ReturnMessage
	  Return 
	 End 
	 Else 
	 Begin
	 Select @ToassetConditionID = AssetConditionID  from AssetConditionTable where AssetConditionCode=@ToAssetConditionCode and StatusID=@statusID
	 end 
	 if not exists(select TransferTypeID from TransferTypeTable where TransferTypeCode=@TransferTypeCode and StatusID=@statusID)
	 Begin
	  SElect @Barcode+'- given Transfer Type Code is not avaliable in db.' as ReturnMessage
	  Return 
	 End 
	 Else 
	 Begin
	 Select @TransferTypeID = TransferTypeID from TransferTypeTable where TransferTypeCode=@TransferTypeCode and StatusID=@statusID
	 end 

	 Declare @pad char='0'
	 Declare @CodeLength int =5
	 Declare @LastValue int ,@TransCode nvarchar(100),@codePrefix nvarchar(100),@PrimaryID int 

	 Select @LastValue=LastUsedNo,@codePrefix=CodePrefix from EntityCodeTable where EntityCodeName='InternalAssetTransfer'
	 select @TransCode=@codePrefix+replicate(@pad ,@CodeLength-len(convert(varchar(100),@LastValue)))+convert(varchar(100),@LastValue)
	 
	 Insert into TransactionTable(TransactionNo,TransactionTypeID,CreatedFrom,Remarks,TransactionDate,
	StatusID,PostingStatusID,CreatedBy,CreatedDateTime)
	values(@TransCode,11,'ExcelAssetTransfer',@TransferRemarks,case when @TransferDate is not null then CONVERT(DATETIME,@TransferDate,103) else null end,
	@statusID,1,@UserID,GETDATE())
	SET @PrimaryID=SCOPE_IDENTITY()
	insert into TransactionLineItemTable(TransactionID,AssetID,FromLocationID,ToLocationID,FromDepartmentID,ToDepartmentID,FromSectionID,ToSectionID,FromCustodianID,ToCustodianID,
	FromAssetConditionID,ToAssetConditionID,StatusID,CreatedBy,CreatedDateTime,TransferTypeID,TransferDate)
	select @PrimaryID,AssetID,LocationID,@TolocationID,DepartmentID,@TodepartmentID,SectionID,@TosectionID,CustodianID,@ToCustodianID,AssetConditionID,@ToassetConditionID,
	@statusID,@UserID,GETDATE(),@TransferTypeID,case when @TransferDate is not null then CONVERT(DATETIME,@TransferDate,103) else getdate() end
	From AssetTable where Barcode=@Barcode

	Update AssetTable set 
	LocationID=@TolocationID,
	SectionID=@TosectionID,
	CustodianID=@ToCustodianID,
	DepartmentID=@TodepartmentID,
	AssetConditionID=@ToassetConditionID
	where Barcode=@Barcode

	select NULL as ReturnMessage
End 
go 
IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'CreatedFrom' AND Object_ID = Object_ID(N'PersonTable'))
Begin
Alter table PersonTable add CreatedFrom nvarchar(50) NULL
End 
go
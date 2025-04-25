
Create table ApprovalTransactionHistoryTable
(
	ApprovalTransactionHistoryID int not null primary key Identity(1,1),
	ApprovalHistoryID int not null foreign key references ApprovalHistoryTable(ApprovalHistoryID),
	Remarks nvarchar(max) NULL,
	StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references user_LoginUSerTable(UserID),
	CreatedDateTime SmallDateTime not null
)
go
If not exists(Select StatusID from StatusTable where Status='ReApproval')
Begin
Insert into StatusTable(StatusID,Status)
values (550,'ReApproval')
End
Go
ALTER VIEW [dbo].[TransactionApprovalView] 
 as 
 Select ApprovalHistoryID,ApproveWorkFlowID,ApproveWorkFlowLineItemID,a.ApproveModuleID,a.ApprovalRoleID,a.TransactionID as ApprovalTransactionID,
		OrderNo,a.Remarks as ApprovalRemarks,FromLocationID,ToLocationID,FromLocationTypeID,ToLocationTypeID,a.StatusID as ApprovalStatusID,
        a.CreatedBy as ApprovalCreatedBy,a.CreatedDateTime as ApprovalCreatedDateTime,a.LastModifiedBy,a.LastModifiedDateTime,ObjectKeyID1,EmailsecrectKey ,
		b.TransactionID,TransactionNo,TransactionTypeID,TransactionSubTypeName,ReferenceNo,b.CreatedFrom,SourceTransactionID,SourceDocumentNo,b.Remarks,TransactionDate,
		TransactionValue,b.StatusID,PostingStatusID,VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,b.CreatedBy,b.CreatedDateTime, 
		Dense_rank() over( PARTITION BY a.transactionid , a.approvemoduleID ORDER BY Orderno asc ) AS SerialNo ,
		p.PersonFirstName+'-'+p.PersonLastName as CreatedUSer,s.Status as ApprovalStatus,
		case when a.ApproveModuleID=10 then  isnull(c.UpdateRetirementDetailsForEachAssets,0)
		     when a.ApproveModuleID=5 or  a.ApproveModuleID=11 then isnull(c.UpdateDestinationlocationsForTransfer,0) else cast(0 as bit)  end as enableUpdate,
			 isnull(DD.DelegatedEmployeeID,D.UserID) as UserID
	From ApprovalHistoryTable a join TransactionTable b on a.TransactionID=b.TransactionID 
	join ApprovalRoleTable c on a.ApprovalRoleID=c.ApprovalRoleID
	join PersonTable p on b.CreatedBy=p.PersonID
	join StatusTable s on a.StatusID=s.StatusID
	join CategoryTypeTable CT on a.CategoryTypeID=CT.CategoryTypeID
	Left join ApprovalHistoryMappedUser D on D.ApprovalRoleID=c.ApprovalRoleID  and 
	case when c.ApprovalLocationTypeID=5 then 
	    a.FromLocationID else a.ToLocationID end =D.LocationID and a.TransactionID=D.TransactionID and a.ApproveModuleID=D.ApproveModuleID
	Left join (select * from DelegateRoleTable where  getdate() between EffectiveStartDate and EffectiveEndDate) DD on DD.EmployeeID=D.UserID
	Left join TransactionSubTypeTable TST on b.TransactionSubTypeID=TST.TransactionSubTypeID
	where a.StatusID in (150,550)
GO


if not exists(select Importfield from ImportTemplateNewTable where ImportField='L1LocationCode' and 
		EntityID=(select EntityId from EntityTable where EntityName='ImportAssetTransfer'))
Begin 
 Insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,DataFormat,
					IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select 'L1LocationCode',10,1,1,100,NULL,0,100,2,1,NULL,EntityID
from EntityTable where EntityName='ImportAssetTransfer'
End 
go 
if not exists(select Importfield from ImportTemplateNewTable where ImportField='L2LocationCode' and 
		EntityID=(select EntityId from EntityTable where EntityName='ImportAssetTransfer'))
Begin 
 Insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,DataFormat,
					IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select 'L2LocationCode',11,1,1,100,NULL,0,100,2,1,NULL,EntityID
from EntityTable where EntityName='ImportAssetTransfer'
End 
go 

ALTER procedure [dbo].[iprc_ImportAssetTransfer]
(
@Barcode			nvarchar(max) =NULL,--'AT24105428,AT24105427,AT24105426',
@ToLocationCode		nvarchar(100) =NULL,--'FLGF-RM00070601',
@ToCustodianCode	nvarchar(100) =null,
@ToDepartmentCode	nvarchar(100) =null,
@ToSectionCode		nvarchar(100)=null,
@TransferRemarks	nvarchar(max)=null,
@ToAssetConditionCode	nvarchar(100)=null,
@TransferTypeCode	nvarchar(100)=null,
@TransferDate		nvarchar(100)=null,
@ImportTypeID int=2,
@UserID int=1,
@L1LocationCode nvarchar(100)=NULL,
@L2LocationCode nvarchar(100)=NULL
--@LanguageID int,
--@CompanyID int	

)
as
Begin 
Declare @AssetID int ,@statusID int ,@SectionID int ,@custodianID int ,@DateFormat bit ,@TolocationID int ,@TosectionID int ,@TodepartmentID int ,@ToCustodianID int ,
	 @ToassetConditionID int ,@TransferTypeID int ,@AssetIDS nvarchar(max),@ApprovalFlag bit 
Declare @SingleAssetValidationTable Table (ErrorID int,ErrorMsg nvarchar(max))

IF @ToLocationCode IS NOT NULL AND ((@L1LocationCode IS NULL OR @L1LocationCode='') or (@L2LocationCode IS NULL OR @L2LocationCode=''))
	BEGIN 
	   SElect @Barcode+'- L1/L2 Location Code is must be add if Asset Transfer ' as ReturnMessage
				Return 
	END 

		Declare @locL1 int,@LocL2 int
					 if @L1LocationCode is not null or @L1LocationCode !=''
					 Begin
						 if  exists(select locationcode from LocationTable where LocationCode=@L1LocationCode and ParentLocationID is null and StatusID=1)
						 Begin 
							 select @locL1=locationID from LocationTable where LocationCode=@L1LocationCode and ParentLocationID is null and StatusID=1
						 End 
						 else
						  Begin 
							  SElect 'given L1 Location code is not avaliable in db  '+@Barcode as ReturnMessage
							  Return
						  End 
					 End 
					 if @L2LocationCode is not null or @L2LocationCode !=''
					 Begin
						 if  exists(select locationcode from LocationTable where LocationCode=@L2LocationCode and ParentLocationID =@locL1 and StatusID=1)
						 Begin 
							 select @locL2=locationID from LocationTable where LocationCode=@L2LocationCode and ParentLocationID =@locL1 and StatusID=1
						 End 
						 else
						  Begin 
							  SElect 'given L2 Location code is not avaliable in db  '+@Barcode as ReturnMessage
							  Return
						  End 
					 End 

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
	Declare @GivenBarcodeTable Table(Barcode nvarchar(max),AssetID int,StatusID int,
	CategoryID int,CategoryTypeID int,LoCationTypeID int,L2LoCationID int,LoCationID int,LocationType nvarchar(max)  )
	
	insert into @GivenBarcodeTable(Barcode)
	select value from split(@barcode,',')
	
	update a set 
	a.AssetID=b.AssetID,
	a.StatusID=b.StatusID,
	a.CategoryID=b.CategoryID,
	a.LoCationID=b.LocationID
	from @GivenBarcodeTable a
	Left join AssetTable b on a.Barcode=b.Barcode

	update a set
	a.CategoryTypeID=b.CategoryTypeID
	from @GivenBarcodeTable a 
	LEft join CategoryNewHierarchicalView b on a.CategoryID=B.CategoryID

	update a set
	a.LocationTypeID=b.LocationTypeID,
	a.L2LoCationID=b.MappedLocationID,
	a.LocationType=b.LocationType
	from @GivenBarcodeTable a 
	LEft join LocationNewHierarchicalView b on a.LocationID=B.LocationID
	
	Declare @ErrorID int ,@ErrorMessage nvarchar(max)

	If exists(select AssetID from @GivenBarcodeTable where AssetID is null) 
	Begin
	  Declare @MissingAssetID nvarchar(max)
		Select @MissingAssetID=stuff((Select ',' + T.Barcode from @GivenBarcodeTable T 
							 where T.AssetID is null and T.Barcode=a.Barcode FOR XML PATH('')), 1, 1, '') from @GivenBarcodeTable a 
		Set @ErrorID=1
		select @MissingAssetID +' Barcode(s) are Invalid.'  as ReturnMessage
		return
	End 
	If exists(select AssetID from @GivenBarcodeTable where StatusID !=@statusID) 
	Begin
	  Declare @MissingStatusID nvarchar(max)
		Select @MissingStatusID=stuff((Select ',' + T.Barcode from @GivenBarcodeTable T 
							 where T.StatusID !=@statusID and T.Barcode=a.Barcode FOR XML PATH('')), 1, 1, '') from @GivenBarcodeTable a 
		Set @ErrorID=2
		set @ErrorMessage=@MissingStatusID+' Barcode(s) are Status are invalid.'
		select @ErrorMessage as ReturnMessage
		return
	End 

	Select @TolocationID=LocationID from LocationNewHierarchicalView where LocationCode=@ToLocationCode and Level1ID=@locL1 and Level2ID=@LocL2

	if(@TolocationID is null or @TolocationID='')
	Begin 
	  SElect 'given To Location code is not avaliable in db combination of L1 and L2 Locations  '+@Barcode as ReturnMessage
		Return
	end 
	IF exists(select LocationID from LocationTable where ParentLocationID=@TolocationID)
			Begin 
			Set @ErrorID=3
			 SElect @ErrorMessage=@ToLocationCode+'- given To Location Code is not an last node. Please give the location last node.' 
			select @ErrorMessage as ReturnMessage
			Return 
			End 
	if @ToDepartmentCode is not null and @ToDepartmentCode!=''
		Begin 
		 if not exists(select DepartmentID from DepartmentTable where DepartmentCode=@ToDepartmentCode and StatusID=@statusID)
		  Begin
		  Set @ErrorID=2
	
		  SElect @ErrorMessage=@ToDepartmentCode+'- given Department Code is not avaliable in db.' 
		 select @ErrorMessage as ReturnMessage
		 Return 
		 End 
		 Else 
		 Begin
		 Select @ToDepartmentID = DepartmentID from DepartmentTable where DepartmentCode=@ToDepartmentCode and StatusID=@statusID
		 end  
	 End 
	if @ToSectionCode is not null and @ToSectionCode!=''
		Begin 
	 if not exists(select sectionID from SectionTable where SectionCode=@ToSectionCode and StatusID=@statusID)
	 Begin
	  Set @ErrorID=3
	  SElect @ErrorMessage=@ToSectionCode+'- given Section Code is not avaliable in db.' 
	 select @ErrorMessage as ReturnMessage
	 Return 
	 End 
	 Else 
	 Begin
	 Select @TosectionID = sectionID from SectionTable where SectionCode=@ToSectionCode and StatusID=@statusID
	 end 
	 End 
	if @ToCustodianCode is not null and @ToCustodianCode!=''
		Begin
	 if not exists(select PersonID from PersonTable where PersonCode=@ToCustodianCode and StatusID=@statusID)
	 Begin
	  Set @ErrorID=4
	  SElect @ErrorMessage=@ToCustodianCode+'- given Custodian Code is not avaliable in db.' 
	  select @ErrorMessage as ReturnMessage
	  Return 
	 End 
	 Else 
	 Begin
	 Select @ToCustodianID = PersonID from PersonTable where PersonCode=@ToCustodianCode and StatusID=@statusID
	 end 
	 End 
	if @ToAssetConditionCode is not null and @ToAssetConditionCode!=''
		Begin
	 if not exists(select AssetConditionCode from AssetConditionTable where AssetConditionCode=@ToAssetConditionCode and StatusID=@statusID)
	 Begin
	  SElect @ErrorMessage=@ToAssetConditionCode+'- given Asset Condition Code is not avaliable in db.'
	  select @ErrorMessage as ReturnMessage
	  Return 
	 End 
	 Else 
	 Begin
	 Select @ToassetConditionID = AssetConditionID  from AssetConditionTable where AssetConditionCode=@ToAssetConditionCode and StatusID=@statusID
	 end 
	 End 
	if @TransferTypeCode is not null and @TransferTypeCode!=''
		Begin
	 if not exists(select TransferTypeID from TransferTypeTable where TransferTypeCode=@TransferTypeCode and StatusID=@statusID)
	 Begin
	  SElect @ErrorMessage=@ToAssetConditionCode+'- given Transfer Type Code is not avaliable in db.'
	  
	  select @ErrorMessage as ReturnMessage
	  Return 
	 End 
	 Else 
	 Begin
	 Select @TransferTypeID = TransferTypeID from TransferTypeTable where TransferTypeCode=@TransferTypeCode and StatusID=@statusID
	 end 
	 End
	-- select * from @GivenBarcodeTable
	Select @AssetIDS=stuff((Select ',' + cast(T.AssetID as nvarchar(max)) from @GivenBarcodeTable T 
							 where T.StatusID =@statusID and T.Barcode=a.Barcode FOR XML PATH('')), 1, 1, '') from @GivenBarcodeTable a 

	exec [prc_ValidateForTransaction] @AssetIDS,@TolocationID,11,'InternalAssetTransfer',@ErrorID output,@ErrorMessage output
	
	if @ErrorID>0
		Begin
			 Select @ErrorMessage as ReturnMessage
			 Return
		End
	print 'common vadation'
	set @ApprovalFlag=0

	Declare @FromLocationType nvarchar(max),@ToLocationType nvarchar(max),@FromLocationTypeID int ,@ToLocationTypeID int,@FromLoationID int ,@CategoryTypeID int 
	select top 1 @FromLocationType=LocationType,@FromLocationTypeID=LocationTypeID,@FromLoationID=L2LoCationID,@CategoryTypeID=CategoryTypeID from @GivenBarcodeTable 
	select @ToLocationType=Locationtype from tmp_LocationNewHierarchicalView where LocationCode=@ToLocationCode
	select @ToLocationTypeID=LocationTypeID from LocationTypeTable where LocationTypeName=@ToLocationType

		If(@FromLocationType='Head Quarters' and @ToLocationType='Head Quarters')
		begin 
		set @ApprovalFlag=1 
		End 

		DECLARE @GetAssetID CURSOR
		SET @GetAssetID = CURSOR FOR
		SELECT AssetID
		FROM @GivenBarcodeTable 
		OPEN @GetAssetID
		FETCH NEXT
		FROM @GetAssetID INTO @AssetID
		WHILE @@FETCH_STATUS = 0
		BEGIN
		exec Prc_AssetTransferValidation @userID,@AssetID,@ToLocationCode,Null,NULL,@ToDepartmentCode,'ExcelAssetTransfer',@ErrorID output,@ErrorMessage output
		Insert into @SingleAssetValidationTable(ErrorID,ErrorMsg)
		values(@ErrorID,@ErrorMessage)

		FETCH NEXT
		FROM @GetAssetID INTO @AssetID
		END
		CLOSE @GetAssetID
		DEALLOCATE @GetAssetID
		--select * from @SingleAssetValidationTable
		if exists(SELECT ErrorID from @SingleAssetValidationTable where ErrorMsg is not null)
		Begin
		print 'we'
		--SELECT ErrorID from @SingleAssetValidationTable where ErrorID!=0
		 Declare @MissingErrorID nvarchar(max)
		Select @MissingErrorID=stuff((Select ',' + T.ErrorMsg from @SingleAssetValidationTable T 
							 where ErrorMsg is not null and T.ErrorID=a.ErrorID FOR XML PATH('')), 1, 1, '') from @SingleAssetValidationTable a where ErrorMsg is not null
		print @MissingErrorID
		Set @ErrorID=6
		--set @ErrorMessage=@MissingAssetID
		select @MissingErrorID as ReturnMessage
		return
		end 

		 Declare @pad char='0'
		 Declare @CodeLength int =5
		 Declare @LastValue int ,@TransCode nvarchar(100),@codePrefix nvarchar(100),@PrimaryID int 

		 Select @LastValue=LastUsedNo+1,@codePrefix=CodePrefix from EntityCodeTable where EntityCodeName='InternalAssetTransfer'
		 select @TransCode=@codePrefix+replicate(@pad ,@CodeLength-len(convert(varchar(100),@LastValue)))+convert(varchar(100),@LastValue)
	 print 'ooo'
		 Insert into TransactionTable(TransactionNo,TransactionTypeID,CreatedFrom,Remarks,TransactionDate,
		StatusID,PostingStatusID,CreatedBy,CreatedDateTime)
		values(@TransCode,11,'ExcelAssetTransfer',@TransferRemarks,case when @TransferDate is not null then CONVERT(DATETIME,@TransferDate,103) else null end,
		case when @ApprovalFlag=0 then  @statusID else 150 end ,1,@UserID,GETDATE())
	
		SET @PrimaryID=SCOPE_IDENTITY()

		insert into TransactionLineItemTable(TransactionID,AssetID,FromLocationID,ToLocationID,FromDepartmentID,ToDepartmentID,FromSectionID,ToSectionID,FromCustodianID,ToCustodianID,
		FromAssetConditionID,ToAssetConditionID,StatusID,CreatedBy,CreatedDateTime,TransferTypeID,TransferDate)
		select @PrimaryID,AssetID,LocationID,@TolocationID,DepartmentID,@TodepartmentID,SectionID,@TosectionID,CustodianID,@ToCustodianID,AssetConditionID,@ToassetConditionID,
		case when @ApprovalFlag=0 then  @statusID else 150 end ,@UserID,GETDATE(),@TransferTypeID,case when @TransferDate is not null then CONVERT(DATETIME,@TransferDate,103) else getdate() end
		From AssetTable where AssetID in (select AssetID from @GivenBarcodeTable)

		if @ApprovalFlag=0
		Begin
		print 'welcome'
			Update AssetTable set 
			LocationID=@TolocationID,
			SectionID=isnull(@TosectionID,SectionID),
			CustodianID=isnull(@ToCustodianID,CustodianID),
			DepartmentID=isnull(@TodepartmentID,DepartmentID),
			AssetConditionID=isnull(@ToassetConditionID,AssetConditionID)
			where AssetID in (select AssetID from @GivenBarcodeTable)
		end 

		update EntityCodeTable set LastUsedNo=LastUsedNo+1 where EntityCodeName='InternalAssetTransfer'
	   if @ApprovalFlag=1
	Begin
	
	If exists(select ApproveWorkflowID from ApproveWorkflowTable where ApproveModuleID=11 and FromLocationTypeID=@FromLocationTypeID 
	and ToLocationTypeID=@ToLocationTypeID and StatusID=@statusID)
	Begin
	
	  Insert into ApprovalHistoryTable(ApproveWorkFlowID,ApproveWorkFlowLineItemID,ApproveModuleID,ApprovalRoleID,TransactionID,
		OrderNo,FromLocationID,ToLocationID,FromLocationTypeID,ToLocationTypeID,StatusID,CreatedBy,CreatedDateTime,CategoryTypeID)
		select a.ApproveWorkflowID,b.ApproveWorkFlowLineItemID,11,ApprovalRoleID,@PrimaryID,OrderNo,@FromLoationID,@TolocationID,@FromLocationTypeID,@ToLocationTypeID,
		150,@UserID,GETDATE(),@CategoryTypeID
		from ApproveWorkflowTable a join ApproveWorkflowLineITemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID where ApproveModuleID=11 and FromLocationTypeID=@FromLocationTypeID 
		and ToLocationTypeID=@ToLocationTypeID and a.StatusID=@statusID and b.StatusID=@statusID

		Update AssetTable set  statusID=150 where AssetID in (select AssetID from @GivenBarcodeTable)
	End 
	End 
	print 'yyyy'
	   select NULL as ReturnMessage
End 

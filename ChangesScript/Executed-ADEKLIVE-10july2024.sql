ALTER view [dbo].[nvwAssetRetirement_ForAfterApproval]
as 
select a1.AssetCode,a1.Barcode,a1.CategoryName,a1.LocationName,a1.DepartmentName,a1.SectionDescription,a1.CustodianName,a1.AssetDescription,
  a1.AssetCondition,a1.suppliername,
  
  old.LocationName as OldLocationName,
  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubTypeName ,
ReferenceNo,a.CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
FORMATMESSAGE( dbo.fn_GetServerURL()+'TransactionApproval/EmailView?id=%d' , a.TransactionID)  ApprovalURL,
--Notification supporting fields
		A.TransactionID SYSDataID1, NULL SYSDataID2, NULL SYSDataID3,
		p.EMailID SYSToAddresses, '' SYSCCAddresses, '' SYSBCCAddresses,
		NULL SYSToMobileNos, NULL SYSWhatsAppMobileNos,b.DisposalValue, b.DisposalReferencesNo,b.DisposalRemarks,b.ProceedOfSales,b.CostOfRemoval,
		b.DisposalDate,RE.RetirementTypeName,NULL as SYSUserID
 From TransactionTable a 
  join TransactionLineItemTable b on a.TransactionID=b.TransactionID 
  join AssetNewView a1 on b.AssetID=a1.AssetID 
  --left join ApprovalHistoryTable AH on a.TransactionID=AH.TransactionID and ah.StatusID=150
  left join LocationTable Old on b.FromLocationID=Old.LocationID
  Left join RetirementTypeTable RE on b.RetirementTypeID=RE.RetirementTypeID
  Left join PersonTable p on a.CreatedBy=p.PersonID and p.EMailID is not null
  Left join TransactionSubTypeTable TST on a.TransactionSubTypeID=TST.TransactionSubTypeID
  where  a.TransactionTypeID=10 --a.StatusID = dbo.fnGetActiveStatusID() and
GO

ALTER view [dbo].[nvwAssetRetirement_ForBeforeApproval]
as 
  select  a1.AssetCode,a1.Barcode,a1.CategoryName,a1.LocationName,a1.DepartmentName,a1.SectionDescription,a1.CustodianName,a1.AssetDescription,
  a1.AssetCondition,a1.suppliername,
  
  old.LocationName as OldLocationName,
  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubTypeName,
ReferenceNo,a.CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
 case when ah.ApproveModuleID=10 and ad.UpdateRetirementDetailsForEachAssets=1   then dbo.fn_GetServerURL() else 

 FORMATMESSAGE( dbo.fn_GetServerURL()+'TransactionApproval/EmailEdit?id=%d&UserID=%d' , ah.ApprovalHistoryID,R.UserID) end
 
   ApprovalURL,
--Notification supporting fields
		A.TransactionID SYSDataID1, AH.ApprovalHistoryID SYSDataID2, NULL SYSDataID3,R.UserID as SYSUserID,
		R.EMailID SYSToAddresses, '' SYSCCAddresses, '' SYSBCCAddresses,
		NULL SYSToMobileNos, NULL SYSWhatsAppMobileNos,b.DisposalValue, b.DisposalReferencesNo,b.DisposalRemarks,b.ProceedOfSales,b.CostOfRemoval,
		b.DisposalDate
		
  From TransactionTable a 
  join TransactionLineItemTable b on a.TransactionID=b.TransactionID 
  join AssetNewView a1 on b.AssetID=a1.AssetID 
  left join ApprovalHistoryTable AH on a.TransactionID=AH.TransactionID --and ah.StatusID=150
  left join LocationTable Old on b.FromLocationID=Old.LocationID
   Left join CategoryTypeTable CT on ah.CategoryTypeID=Ct.CategoryTypeID
 Left join ApprovalRoleTable AD on AH.ApprovalRoleID=AD.ApprovalRoleID and ad.statusID=1
 Left join ApprovalHistoryMappedUser D on D.ApprovalRoleID=AD.ApprovalRoleID  and 
							case when AD.ApprovalLocationTypeID=5 then 
					AH.FromLocationID else AH.ToLocationID end =D.LocationID and AH.TransactionID=D.TransactionID and AH.ApproveModuleID=D.ApproveModuleID
					join ValidateAccessRightsView  R  on D.UserID=R.UserID  and RightName= 'AssetRetirementApproval' and  R.EMailID is not null
  Left join TransactionSubTypeTable TST on a.TransactionSubTypeID=TST.TransactionSubTypeID
WHERE A.StatusID  in (150,200) and a.TransactionTypeID=10

GO

ALTER view [dbo].[nvwAssetRetirement_ForNotification]
as 
  select  a1.AssetCode,a1.Barcode,a1.CategoryName,a1.LocationName,a1.DepartmentName,a1.SectionDescription,a1.CustodianName,a1.AssetDescription,
  a1.AssetCondition,a1.suppliername,
  
  old.LocationName as OldLocationName,
  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubTypeName,
ReferenceNo,a.CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
FORMATMESSAGE( dbo.fn_GetServerURL()+'TransactionApproval/EmailEdit?id=%d&UserID=%d' , ah.ApprovalHistoryID,R.UserID) ApprovalURL,
--Notification supporting fields
		A.TransactionID SYSDataID1, AH.ApprovalHistoryID SYSDataID2, NULL SYSDataID3,R.UserID as SYSUserID,
		R.EMailID SYSToAddresses, '' SYSCCAddresses, '' SYSBCCAddresses,
		NULL SYSToMobileNos, NULL SYSWhatsAppMobileNos,b.DisposalValue, b.DisposalReferencesNo,b.DisposalRemarks,b.ProceedOfSales,b.CostOfRemoval,
		b.DisposalDate,(SELECT EMAILID FROM PERSONTABLE WHERE PERSONID=AH.CREATEDBY) as ApprovedBy,convert(nvarchar(100),ah.CreatedDateTime,103) as ApprovedDate
		
  From TransactionTable a 
  join TransactionLineItemTable b on a.TransactionID=b.TransactionID 
  join AssetNewView a1 on b.AssetID=a1.AssetID 
  left join ApprovalHistoryTable AH on a.TransactionID=AH.TransactionID --and ah.StatusID=150
   Left join ApprovalRoleTable AD on AH.ApprovalRoleID=AD.ApprovalRoleID and ad. statusID=1
    Left join CategoryTypeTable CT on ah.CategoryTypeID=Ct.CategoryTypeID
  left join LocationTable Old on b.FromLocationID=Old.LocationID
  Left join ApprovalHistoryMappedUser D on D.ApprovalRoleID=AD.ApprovalRoleID  and 
		case when AD.ApprovalLocationTypeID=5 then 
		AH.FromLocationID else AH.ToLocationID end =D.LocationID and AH.TransactionID=D.TransactionID and AH.ApproveModuleID=D.ApproveModuleID
 join ValidateAccessRightsView  R  on D.UserID=R.UserID  and RightName= 'AssetRetirementApproval' and  R.EMailID is not null
  Left join TransactionSubTypeTable TST on a.TransactionSubTypeID=TST.TransactionSubTypeID
WHERE a.TransactionTypeID=10

GO

ALTER View [dbo].[nvwAssetTransfer_ForAfterApproval]
as 
Select a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubTypeName,
ReferenceNo,a.CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
a1.*,P.EMailID as SYSToAddresses, '' SYSCCAddresses, '' SYSBCCAddresses,
		NULL SYSToMobileNos, NULL SYSWhatsAppMobileNos,A.TransactionID SYSDataID1, NULL SYSDataID2, NULL SYSDataID3,old.LocationName as OldLocationName,New.LocationName as NewLocationName,
		Room.LocationName as RoomName,
		CAse when a.TransactionTypeID=5 then 
		FORMATMESSAGE( dbo.fn_GetServerURL()+'TransactionApproval/EmailView?id=%d' , a.TransactionID) else FORMATMESSAGE( dbo.fn_GetServerURL()+'TransactionApproval/EmailView?id=%d' , a.TransactionID) end   ApprovalURL,NULL as SYSUserID
  From TransactionTable a 
  join TransactionLineItemTable b on a.TransactionID=b.TransactionID 
  join AssetNewView a1 on b.AssetID=a1.AssetID 
   --left join ApprovalHistoryTable AH on a.TransactionID=AH.TransactionID and ah.StatusID = dbo.fnGetActiveStatusID()
   Left join PersonTable p on a.CreatedBy=p.PersonID and p.EMailID is not null
    left join LocationTable Old on b.FromLocationID=Old.LocationID
	left join LocationTable new on b.ToLocationID=new.LocationID
	Left join LocationTable Room on b.RoomID=Room.LocationID
	Left join TransactionSubTypeTable TST on a.TransactionSubTypeID=TST.TransactionSubTypeID
	where  a.TransactionTypeID in (5,11)
GO

ALTER view [dbo].[nvwAssetTransfer_ForBeforeApproval]
as 
  select  a1.*,old.LocationName as OldLocationName,New.LocationName as NewLocationName,
  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubTypeName,
ReferenceNo,a.CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
OldDept.DepartmentName as OldDepartmentName,NewDept.DepartmentName as NewDepartmentName,OldCate.CategoryName as OldCategoryName,
Oldprod.ProductName as OldProductName,newprod.ProductName as NewProductName,OldSec.SectionName as oldSectionName,newsec.SectionName as NewSectionName,

newCate.CategoryName as NewCategoryName,
 case when ah.ApproveModuleID=5 and ad.UpdateDestinationLocationsForTransfer=1   then dbo.fn_GetServerURL() else 
  case when ah.ApproveModuleID=11 then FORMATMESSAGE( dbo.fn_GetServerURL()+'TransactionApproval/EmailEdit?id=%d&UserID=%d', ah.ApprovalHistoryID,R.UserID)
  else FORMATMESSAGE( dbo.fn_GetServerURL()+'TransactionApproval/EmailEdit?id=%d&UserID=%d' , ah.ApprovalHistoryID,R.UserID)
 
 end end   ApprovalURL,
--NULL ApprovalURL,
--Notification supporting fields
		A.TransactionID SYSDataID1, AH.ApprovalHistoryID SYSDataID2, '' SYSDataID3,R.UserID as SYSUserID,
		R.EMailID SYSToAddresses, '' SYSCCAddresses, '' SYSBCCAddresses,
		NULL SYSToMobileNos, NULL SYSWhatsAppMobileNos,AD.ApprovalRoleID
		
  From TransactionTable a 
  join TransactionLineItemTable b on a.TransactionID=b.TransactionID 
  join AssetNewView a1 on b.AssetID=a1.AssetID 
  left join ApprovalHistoryTable AH on a.TransactionID=AH.TransactionID --and ah.StatusID=1501
  Left join CategoryTypeTable CT on ah.CategoryTypeID=Ct.CategoryTypeID
  Left join ApprovalRoleTable AD on AH.ApprovalRoleID=AD.ApprovalRoleID and ad. statusID=1
  left join LocationTable Old on b.FromLocationID=Old.LocationID
  left join LocationTable new on b.ToLocationID=new.LocationID
  Left join DepartmentTable OldDept on b.FromDepartmentID=OldDept.DepartmentID
  left join DepartmentTable NewDept on b.ToDepartmentID=NewDept.DepartmentID
  Left join CategoryTable OldCate on b.FromCategoryID=OldCate.CategoryID
  left join CategoryTable newCate on b.ToCategoryID=newCate.CategoryID
   Left join ProductTable Oldprod on b.FromProductID=Oldprod.ProductID
  left join ProductTable newprod on b.ToProductID=newprod.ProductID
  LEft join SectionTable OldSec on b.FromSectionID=oldsec.SectionID
  left join SectionTable newsec on b.ToSectionID=newsec.SectionID
  Left join ApprovalHistoryMappedUser D on D.ApprovalRoleID=AD.ApprovalRoleID  and 
							case when AD.ApprovalLocationTypeID=5 then 
					AH.FromLocationID else AH.ToLocationID end =D.LocationID and AH.TransactionID=D.TransactionID and AH.ApproveModuleID=D.ApproveModuleID
					join ValidateAccessRightsView  R  on D.UserID=R.UserID  and RightName= 'AssetTransferApproval' and  R.EMailID is not null
  Left join TransactionSubTypeTable TST on a.TransactionSubTypeID=TST.TransactionSubTypeID
WHERE A.StatusID in (150,200) and a.TransactionTypeID in (5,11)

GO

ALTER view [dbo].[nvwAssetTransfer_ForNotification]
as 
  select  a1.*,old.LocationName as OldLocationName,New.LocationName as NewLocationName,
  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubTypeName,
ReferenceNo,a.CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
OldDept.DepartmentName as OldDepartmentName,NewDept.DepartmentName as NewDepartmentName,OldCate.CategoryName as OldCategoryName,
Oldprod.ProductName as OldProductName,newprod.ProductName as NewProductName,OldSec.SectionName as oldSectionName,newsec.SectionName as NewSectionName,

newCate.CategoryName as NewCategoryName,
case when ah.ApproveModuleID=5 then 
 FORMATMESSAGE( dbo.fn_GetServerURL()+'TransactionApproval/EmailEdit?id=%d&UserID=%d' , ah.ApprovalHistoryID,R.UserID)
 else  FORMATMESSAGE( dbo.fn_GetServerURL()+'TransactionApproval/EmailEdit?id=%d&UserID=%d' , ah.ApprovalHistoryID,R.UserID)
 end 
 ApprovalURL,

--Notification supporting fields
		A.TransactionID SYSDataID1, AH.ApprovalHistoryID SYSDataID2, '' SYSDataID3,R.UserID as SYSUserID,
		  R.EMailID SYSToAddresses,
		  '' SYSCCAddresses, '' SYSBCCAddresses,
		NULL SYSToMobileNos, NULL SYSWhatsAppMobileNos,(SELECT EMAILID FROM PERSONTABLE WHERE PERSONID=AH.CREATEDBY) as ApprovedBy,convert(nvarchar(100),ah.CreatedDateTime,103) as ApprovedDate
		
  From TransactionTable a 
  join TransactionLineItemTable b on a.TransactionID=b.TransactionID 
  join AssetNewView a1 on b.AssetID=a1.AssetID 
  left join ApprovalHistoryTable AH on a.TransactionID=AH.TransactionID 
  Left join CategoryTypeTable CT on ah.CategoryTypeID=Ct.CategoryTypeID
  left join LocationTable Old on b.FromLocationID=Old.LocationID
  Left join ApprovalRoleTable AD on AH.ApprovalRoleID=AD.ApprovalRoleID and ad. statusID=1
  left join LocationTable new on b.ToLocationID=new.LocationID
  Left join DepartmentTable OldDept on b.FromDepartmentID=OldDept.DepartmentID
  left join DepartmentTable NewDept on b.ToDepartmentID=NewDept.DepartmentID
  Left join CategoryTable OldCate on b.FromCategoryID=OldCate.CategoryID
  left join CategoryTable newCate on b.ToCategoryID=newCate.CategoryID
  Left join ProductTable Oldprod on b.FromProductID=Oldprod.ProductID
  left join ProductTable newprod on b.ToProductID=newprod.ProductID
  LEft join SectionTable OldSec on b.FromSectionID=oldsec.SectionID
  left join SectionTable newsec on b.ToSectionID=newsec.SectionID
  Left join ApprovalHistoryMappedUser D on D.ApprovalRoleID=AD.ApprovalRoleID  and 
		case when AD.ApprovalLocationTypeID=5 then 
		AH.FromLocationID else AH.ToLocationID end =D.LocationID and AH.TransactionID=D.TransactionID and AH.ApproveModuleID=D.ApproveModuleID
  join ValidateAccessRightsView  R  on D.UserID=R.UserID  and RightName= 'AssetTransferApproval' and  R.EMailID is not null
  Left join TransactionSubTypeTable TST on a.TransactionSubTypeID=TST.TransactionSubTypeID
  


WHERE  a.TransactionTypeID in (5,11)

GO

ALTER Procedure [dbo].[prc_ValidateForTransaction]
(
   @FromAssetID				nvarchar(max)	 = NULL,
   @ToLocationID			int				 = NULL,
   @ModuleID				int              = NULL,
   @RightName				nvarchar(100)	 = NULL,
   @ErrorID					int OutPut,
   @ErrorMessage			nvarchar(max) OutPut
)
as 
Begin
	DECLARE @ASSETTRANSFER INT = 5,
			@ASSETRETIREMENT INT = 10,
			@INTERNALASSETTRANSFER INT = 11, 
			@ASSETADDITION INT = 4,
			@ASSETMAINTENANCE INT = 15,
			@ASSETMAINTENANCEREQUEST INT = 16

	Declare @UpdateCount int,@ApprovalCnt int ,@statusID int,@CategoryTypeCnt int --@modelID int ,
		,@CategoryType nvarchar(max),@ID int,@FromLocationtypeID int, @ToLocationTypeID int,@ApprovalWorkflowID int ,@HQID int,@MappedToLocationID int

	select @statusID=[dbo].fnGetActiveStatusID()
	--select * from LocationTypeTable

	select @HQID=locationtypeid from LocationTypeTable where LocationTypeName='Head Quarters' and StatusID=@statusID

	Set @ErrorID=0
	Set @ErrorMessage=null

	declare @TransactionLineItemTable table (AssetID int,FromLocationL2 int ,ToLocationL2 int,
			LocationType nvarchar(100),CategoryType nvarchar(100),ApprovalWorkFlowID int,Barcode nvarchar(max),
			MappedCategryName nvarchar(max) )
	
	insert into @TransactionLineItemTable(AssetID,FromLocationL2,ToLocationL2,LocationType,CategoryType,ApprovalWorkFlowID,Barcode,MappedCategryName)
	select AssetID,LocationL2,@ToLocationID,LocationType,categorytype,null,Barcode,B.CategoryName 
	From AssetNewView a join CategoryTable b on a.CategoryID=b.CategoryID where assetid in (select value from Split(@fromAssetID,',')) 

	IF(@ModuleID = @INTERNALASSETTRANSFER)
	BEGIN
		select @MappedToLocationID =MappedLocationID from LocationNewView where LocationID=@ToLocationID
		update @TransactionLineItemTable set ToLocationL2=@MappedToLocationID
		
		If @ToLocationID is null or @ToLocationID=''
		Begin 
		Set @ErrorID=15
		Set @ErrorMessage='Selected Loction Code is not Preferred Location Level.'
		return
		end 

		If not exists(select AssetID From @TransactionLineItemTable where FromLocationL2=ToLocationL2)
		Begin
		Set @ErrorID=14
		Set @ErrorMessage='Selected From Location and To Location should be Same.'
		return
		End
		Else 
		Begin 
		Select @ToLocationTypeID=LocationTypeID from LocationTable where LocationID=@MappedToLocationID
			if @HQID!=@ToLocationTypeID
			Begin 
			  If exists(select LocationID from LocationTable where ParentLocationID=@ToLocationID)
			  Begin 
				Set @ErrorID=16
				Set @ErrorMessage='Non HQ Location should be selected end level.'
				return
			  End 
			End
		
		end 
		--Validate HQ, only requires 2nd level locations
		--NON HQ validate end level should be selected
		--Validate from and to parent location should be same
		--Set @ErrorID = 0
		--Set @ErrorMessage=null
		--return

	END
	ELSE
	BEGIN
		if not exists(select mappedLocationID from LocationNewView where MappedLocationID=@ToLocationID )
		Begin 
			Declare @LocationCode nvarchar(max)
			Select @LocationCode=LocationCode from LocationTable where LocationID=@ToLocationID

			Set @ErrorID=1
			Set @ErrorMessage=@LocationCode+'- Selected Loction Code is not Preferred Location Level.'
			return
		end 
	END

	
	--select * from @TransactionLineItemTable
	Select @CategoryTypeCnt=count(CategoryType) from @TransactionLineItemTable group by LocationType
	if(@CategoryTypeCnt>1)
	begin
		if not exists(select CategoryTypeName from CategoryTypeTable where IsAllCategoryType=1 and statusID=@statusID)
		Begin
		Set @ErrorID=2
		Set @ErrorMessage='Selected Assets have both IT and NON-IT so CategoryType-All Should be created.'
		return
		End 
		Select @CategoryType=CategoryTypeName from CategoryTypeTable where IsAllCategoryType=1 and statusID=@statusID
		update @TransactionLineItemTable set CategoryType=@CategoryType
	End 
	Else 
	Begin 
		Select @CategoryType=CategoryType from @TransactionLineItemTable group by CategoryType
	end 
	--select * from @TransactionLineItemTable
	--select count(FromLocationL2) from @TransactionLineItemTable group by FromLocationL2
	if(select count(FromLocationL2) from (select FromLocationL2 from @TransactionLineItemTable group by FromLocationL2)x)>1
	begin 
		Set @ErrorID=3
		Set @ErrorMessage='Selected Asset have different Mapped Location please select same second level location .'
		return
	End
	if(select count(LocationType) from (select LocationType from @TransactionLineItemTable group by LocationType)x)>1
	begin 
		Set @ErrorID=4
		Set @ErrorMessage='Selected Asset have different location type please select same location type.'
		return
	End
	IF exists(select LocationType from @TransactionLineItemTable where LocationType is null )
	Begin 
	Set @ErrorID=13
		Set @ErrorMessage='Selected Asset must be map with location type.'
		return
	End 
	--if @moduleID=11 
	--begin
	--	select @ToLocationID =MappedLocationID from LocationNewView where LocationID=@ToLocationID
	--	update @TransactionLineItemTable set ToLocationL2=@ToLocationID
	--end

	Select @FromLocationtypeID=LocationTypeID from LocationTypeTable 
		where LocationTypeName=(select top 1 LocationType from @TransactionLineItemTable)

	Select @ToLocationTypeID=LocationTypeID from LocationTable where LocationID = (select top 1 ToLocationL2 from @TransactionLineItemTable)
	print @ToLocationID
	IF @ToLocationTypeID is null or @ToLocationTypeID=''
	Begin 
	Set @ErrorID=14
		Set @ErrorMessage='Selected To Location must be map with location type.'
		return
	End 
	Declare @CheckVal bit 
	set @CheckVal=0

	IF @HQID=@FromLocationtypeID and @HQID=@ToLocationTypeID
	begin
	set @CheckVal=1
	end 

	if @moduleID=11 and(@CheckVal=0)
	begin
		Set @ErrorID=0
		set @ErrorMessage=null
	    return
	End 
	print @CheckVal
	if not exists(select b.ApprovalRoleID from ApproveWorkflowTable a 
				  join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
				  where a.StatusID=@statusID and b.StatusID=@statusID and 
				  a.ApproveModuleID=(select ApproveModuleID from ApproveModuleTable where ApproveModuleID=@moduleID and StatusID=@statusID)
				  and a.FromLocationTypeID=@FromLocationtypeID and 
				  case when @moduleID=5 or @moduleID=11 then a.ToLocationTypeID else NULL end=@ToLocationTypeID)
			Begin 
				Declare @FromLocationType nvarchar(100),@ToLocationType nvarchar(100)
				Select top 1 @FromLocationType=Locationtype from @TransactionLineItemTable 
				Select @ToLocationType=LocationType From LocationNewView where LocationID=@ToLocationID
				if @moduleID=5 or @moduleID=11 
				Begin 
				Set @ErrorMessage ='Workflow not defined for From LocationType-'+@FromLocationType+' and To Location Type - '+ @ToLocationType+'.'
				End 
				Else 
				Begin 
				Set @ErrorMessage ='Workflow not defined for From LocationType-'+@FromLocationType+'.'
				End 
				Set @ErrorID=5
				return
			End 
	select @ApprovalWorkflowID=a.ApproveWorkflowID from ApproveWorkflowTable a 
				  join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
				  where a.StatusID=@statusID and b.StatusID=@statusID and 
				  a.ApproveModuleID=(select ApproveModuleID from ApproveModuleTable where ApproveModuleID=@moduleID and StatusID=@statusID)
				  and a.FromLocationTypeID=@FromLocationtypeID and 
				  case when @moduleID=5 or @moduleID=11 then a.ToLocationTypeID else NULL end=@ToLocationTypeID
    
	update @TransactionLineItemTable set ApprovalWorkFlowID=@ApprovalWorkflowID
	 
	SElect @moduleID=ApproveModuleID from ApproveWorkflowTable where ApproveWorkflowID=@ApprovalWorkFlowID
	
	Declare @updateTable table(updateRole bit,ApprovalRoleID int,moduleName nvarchar(100),FromLocationType nvarchar(100),ToLocationType nvarchar(100),ModuleID int)
	  
	
	if @moduleID=5 
	Begin
	   if exists (select top 1 assetID from @TransactionLineItemTable where FromLocationL2=ToLocationL2)
	   begin 
	     Set @ErrorID=12
		Set @ErrorMessage='From and To Location are the Same , Please select different Location.'
		return
	   End 
	end 

	Select @ApprovalCnt=count(OrderNo) from 
	ApproveWorkflowTable a join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
	where a.StatusID=@statusID and b.StatusID=@statusID and a.ApproveWorkflowID=@ApprovalWorkFlowID

	if not exists(select * from @TransactionLineItemTable where CategoryType is not null and CategoryType!='') 
	begin 
		Declare @MissingCategoryName nvarchar(max)
		Select @MissingCategoryName=stuff((Select ',' + T.MappedCategryName from @TransactionLineItemTable T 
							 where T.CategoryType is null and T.AssetID=a.AssetID FOR XML PATH('')), 1, 1, '') from @TransactionLineItemTable a 
		Set @ErrorID=6
		set @ErrorMessage=@MissingCategoryName+' Category Type(s) are Missed.'
		return
	End 

	insert into @updateTable(updateRole,ApprovalRoleID,moduleName,FromLocationType,ToLocationType,ModuleID)
		Select case when a.ApproveModuleID=5 or a.ApproveModuleID=11 then UpdateDestinationLocationsForTransfer 
				when  a.ApproveModuleID=10 then UpdateRetirementDetailsForEachAssets else cast(0 as bit) end as updateRole, 
		b.ApprovalRoleID,d.ModuleName,FL.LocationTypeName,TL.LocationTypeName,a.ApproveWorkflowID FRom ApproveWorkflowTable a 
		join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
		join ApproveModuleTable D on a.ApproveModuleID=d.ApproveModuleID
		join ApprovalRoleTable C on b.ApprovalRoleID=C.ApprovalRoleID
		Join LocationTypeTable FL on a.FromLocationTypeID=FL.LocationTypeID
		Left join LocationTypeTable TL on a.ToLocationTypeID=TL.LocationTypeID
		where a.ApproveWorkflowID=@ApprovalWorkFlowID and a.StatusID=@statusID and b.StatusID=@statusID and c.StatusID=@statusID

	Declare @MissedModuleName nvarchar(100),@MissedFromLocationtype nvarchar(100),@MissedToLocationtype nvarchar(100),@MissedModuleID int,@errorMsg nvarchar(max)
	Select top 1 @MissedModuleName=moduleName,@MissedFromLocationtype=FromLocationType,@MissedToLocationtype=ToLocationType,@MissedModuleID=ModuleID from @updateTable
	If(@moduleID=5 or @moduleID=11 or @moduleID=10)
	Begin
		if not exists(select * from @updateTable where updateRole=1)
		Begin 
			Set @ErrorID=7
				If @MissedModuleID=5 or @MissedModuleID=11
				Begin
				Set @errorMsg=@MissingCategoryName+' Update option not provided for modulename : '+@MissedModuleName+',From Location Type :'+@MissedFromLocationtype+',To Location type : '+@MissedToLocationtype+'.'
				End 
				Else 
				Begin
				Set @errorMsg=@MissingCategoryName+' Update option not provided for modulename : '+@MissedModuleName+',From Location Type :'+@MissedFromLocationtype+'.'
				End
			set @ErrorMessage=@errorMsg
			return
		End 
		if (select updateRole from @updateTable group by updateRole having count(updateRole)>1)>1
		Begin 
			Set @ErrorID=8
				If @MissedModuleID=5 or @MissedModuleID=11
				Begin
				Set @errorMsg=@MissingCategoryName+' Update option shouldnot be enabled for more than one, modulename : '+@MissedModuleName+',From Location Type :'+@MissedFromLocationtype+',To Location type : '+@MissedToLocationtype+'.'
				End 
				Else 
				Begin
				Set @errorMsg=@MissingCategoryName+' Update option shouldnot be enabled for more than one, modulename : '+@MissedModuleName+',From Location Type :'+@MissedFromLocationtype+'.'
				End
			set @ErrorMessage=@errorMsg
			return
		End 
	End 
	 Declare @validationTable Table(UserID int,LocationID int,ApprovalRoleID int,CategoryTypeCnt int ) 
	 Declare @validationTable1 Table(UserID int,LocationID int,ApprovalRoleID int,CategoryTypeCnt int ) 
	 Declare @validationTable2 Table(UserID int,LocationID int,ApprovalRoleID int,CategoryTypeCnt int ) 

	insert into @validationTable(UserID,LocationID,ApprovalRoleID,CategoryTypeCnt)
		select userID,LocationID,a.ApprovalRoleID,count(d.categoryTypeID) as categorytypeCnt 
		from ApproveWorkflowLineItemTable a 
		join @TransactionLineItemTable T on a.ApproveWorkFlowID=T.ApprovalWorkFlowID
		join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
		join CategoryTypeTable CT on T.CategoryType=Ct.categoryTypeName
		join 
		(
			select a1.*,b1.IsAllCategoryType from UserApprovalRoleMappingTable a1  
			join CategoryTypeTable b1 on a1.CategoryTypeID=b1.CategoryTypeID
			where a1.StatusID=@statusID and b1.StatusID=@statusID
		) D on  case when b.ApprovalLocationTypeID=5 then 
		T.FromLocationL2 when b.ApprovalLocationTypeID=10 then T.ToLocationL2 else T.FromLocationL2 end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID 
		and ct.CategoryTypeID = (case when CT.IsAllCategoryType=0 and D.IsAllCategoryType=1 then CT.CategoryTypeID else D.CategoryTypeID end)
			where ApproveWorkFlowID=@ApprovalWorkFlowID and CT.statusID=@statusID and a.StatusID=@statusID
			and b.StatusID=@statusID and D.StatusID=@statusID
			group by userID,LocationID,a.ApprovalRoleID

	if(select count(*) from (
			select  a.ApprovalRoleID  from @validationTable a group by a.ApprovalRoleID)x) !=@ApprovalCnt
	Begin 
		Declare @MissingApprovalRoleName nvarchar(max),@ModuleName nvarchar(max)
		
		Select @MissingApprovalRoleName=stuff((Select ',' + b.ApprovalRoleName From ApproveWorkflowLineItemTable a Join ApprovalRoleTable b on a.ApprovalRoleID=b.ApprovalRoleID
		where ApproveWorkFlowID=@ApprovalWorkFlowID and a.StatusID=@statusID and b.StatusID=@statusID and a.ApprovalRoleID not in(select ApprovalRoleID from @validationTable)
		and b.ApprovalRoleID=p.ApprovalRoleID FOR XML PATH('')), 1, 1, '') from ApprovalRoleTable P
		
		Select @ModuleName=b.ModuleName from ApproveWorkflowTable a 
			join ApproveModuleTable b on a.ApproveModuleID=b.ApproveModuleID 
				where ApproveWorkFlowID=@ApprovalWorkFlowID and a.StatusID=@statusID
		 
		Set @ErrorMessage ='Missing user(s) for the role : '+@MissingApprovalRoleName+' , ModuleName :'+@ModuleName+'.'
		Set @ErrorID=9
		return
	end 
    Else 
	Begin 
	insert into @validationTable1(UserID,LocationID,ApprovalRoleID,CategoryTypeCnt)
		select a.userID,a.LocationID,a.ApprovalRoleID,categorytypeCnt
		From @validationTable a  
		join [ValidateAccessRightsView]  b  on a.UserID=b.UserID --and a.LocationID=b.LocationID and a.ApprovalRoleID=b.ApprovalRoleID
		and b.RightName=@RightName
	if(select count(*) from (
		select  a.ApprovalRoleID  from @validationTable1 a group by a.ApprovalRoleID)x )!=@ApprovalCnt
	Begin 
		Set @ErrorID=10
		Declare @MissingUserName nvarchar(max)

		Select @MissingUserName=stuff((Select ',' + PersonCode from @validationTable a join PersonTable b on a.UserID=b.PersonID 
		where userID not in (select userID from @validationTable1) and b.personID=p.PersonID FOR XML PATH('')), 1, 1, '') from PersonTable P
		
		Set @ErrorMessage ='Approval Rights not provided for the user(s)- '+@MissingUserName +'.'
		return
	END
	if (select count(*) from (
		select  a.ApprovalRoleID  from @validationTable1 a group by a.ApprovalRoleID)x  )=@ApprovalCnt
	begin 
		insert into @validationTable2(UserID,LocationID,ApprovalRoleID,CategoryTypeCnt)
		select a.userID,a.LocationID,a.ApprovalRoleID,categorytypeCnt
		From @validationTable a  
		join persontable p on a.userID=p.personID and p.EMailID is not null and p.StatusID=@statusID
 if(select count(*) from (
	select  a.ApprovalRoleID  from @validationTable2 a group by a.ApprovalRoleID)x  )!=@ApprovalCnt
	 Begin 
		Declare @MissingUserEmail nvarchar(max)

		Select @MissingUserEmail=stuff((Select ',' + PersonCode from @validationTable a join PersonTable b on a.UserID=b.PersonID 
			where userID not in (select userID from @validationTable2) and b.personID=p.PersonID FOR XML PATH('')), 1, 1, '') from PersonTable P
		
		Set @ErrorMessage ='EmailID not assigned to the mapped workflow user(s)- '+@MissingUserEmail +'.'
		Set @ErrorID=11
		RETURN
	 END 
	END 
	END
		
		
  end 
  Go

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
@UserID int=1
--@LanguageID int,
--@CompanyID int	

)
as
Begin 
Declare @AssetID int ,@statusID int ,@SectionID int ,@custodianID int ,@DateFormat bit ,@TolocationID int ,@TosectionID int ,@TodepartmentID int ,@ToCustodianID int ,
	 @ToassetConditionID int ,@TransferTypeID int ,@AssetIDS nvarchar(max),@ApprovalFlag bit 
Declare @SingleAssetValidationTable Table (ErrorID int,ErrorMsg nvarchar(max))
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

	Select @TolocationID=LocationID from LocationTable where LocationCode=@ToLocationCode

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
Go

ALTER Procedure [dbo].[Prc_AssetTransferValidation]
(
	@UserID				int					=	NULL,
	@AssetID			int					=	NULL,
	@LocationCode		nvarchar(max)		=	NULL,
	@LocationID			int					=	NULL,
	@DepartmentID		int					=	NULL,
	@DepartmentCode		nvarchar(max)		=	NULL,
	@DataProcessedBy	nvarchar(50)		=   null, 
	@ErrorID			int					OutPut,
	@ErrorMsg			nvarchar(max)		Output
)
as 
Begin 
	Declare @LocationLevel int,@UserLocationMapping int,@UserDepartmentMapping int ,@AssetLocationID int,@AssetDepartmentID int
	,@Barcode nvarchar(max),@ActiveStatusID int

	Select  @ActiveStatusID=[dbo].fnGetActiveStatusID()
	Set @ErrorID	= 0
	set @ErrorMsg= null

	IF not exists(select AssetID from AssetTable where AssetID=@AssetID and StatusID=@ActiveStatusID) 
	Begin
	Select @Barcode=Barcode from  AssetTable where AssetID=@AssetID 
	set @ErrorID=7
		Set @ErrorMsg= @Barcode+'- given Barcode is not an active status.'
		return	
	End 

	Select @AssetLocationID=LocationID,@AssetDepartmentID=DepartmentID
		From AssetTable where AssetID=@AssetID
   
	
	IF ((@DepartmentID is null or @DepartmentID ='') and (@DepartmentCode is null or @DepartmentCode=''))
	Begin
	 Set @DepartmentID=@AssetDepartmentID
	End 

	Declare @UserLocationTable Table(LocationID int)
	Declare @UserDepartmentTable Table(DepartmentID int)

	Select @LocationLevel=ConfiguarationValue from ConfigurationTable where ConfiguarationName='PreferredLevelLocationMapping'

	Select @UserLocationMapping=case when ConfiguarationValue='true' then 1 else 0 end from ConfigurationTable
		where ConfiguarationName='UserLocationMapping'
	Select @UserDepartmentMapping=case when ConfiguarationValue='true' then 1 else 0 end from ConfigurationTable
		where ConfiguarationName='UserDepartmentMapping'		
	
	Insert into @UserLocationTable(LocationID)
	Select LocationID from UserLocationMappingTable where PersonID=@UserID

	Insert into @UserDepartmentTable(DepartmentID)
	Select DepartmentID from UserDepartmentMappingTable where PersonID=@UserID

	

	IF @LocationID is null or @LocationID=''
	Begin 
		IF @LocationCode is null or @LocationCode=''
		Begin
			set @errorID=1
			set @ErrorMsg ='Location Code is Missed.'
		return
		End
		Else 
		Begin 
		Select @LocationID=LocationID from LocationTable where LocationCode=@LocationCode
			IF @LocationID is null or @LocationID=''
			Begin
				set @errorID=2
				set @ErrorMsg =@LocationCode+'- Location Code not valid.'
			End 
			Else 
			Begin 
				If exists(select LocationID from LocationNewView where LocationID=@LocationID and Level<@LocationLevel)
				begin
				set @errorID=3
				set @ErrorMsg =@LocationCode+'- Location Code level must be give '+ @LocationLevel + 'Level.'
				end 
			End 
		End 
	End 
	IF @LocationCode is null or @LocationCode=''
	Begin
	  Select @LocationCode=LocationCode from LocationTable where LocationID=@LocationID
	End 
	IF ((@DepartmentID is null or @DepartmentID='') and (@DepartmentCode is not null or @DepartmentCode!=''))
	BEgin 
		 IF not exists(select DEpartmentID from DepartmentTable where DepartmentCode=@DepartmentCode)
		 Begin
			  Set @ErrorID=4
			  Set @ErrorMsg=@DepartmentCode+ '- Department Code is valid.'
			  return
		 End 
		 Else
		 Begin 
			SElect @DepartmentID=DepartmentID from DepartmentTable where DepartmentCode=@DepartmentCode
			 
		 End 
	End 
	IF ((@DepartmentID is not null or @DepartmentID!='') and (@DepartmentCode is  null or @DepartmentCode=''))
	BEgin  
		SElect @DepartmentCode=DepartmentCode from DepartmentTable where DepartmentID=@DepartmentID
	End 
	If @DepartmentID is not null
	BEgin 
		If not Exists(Select DepartmentID from DepartmentTable where DepartmentID=@DepartmentID
			and DepartmentID in (select DepartmentID from @UserDepartmentTable))
		BEgin 
		set @ErrorID=6
		Set @ErrorMsg= @DepartmentCode+'- given DepartmentCode not mapped with User.'
		return	
		End 
	End 

	If not Exists(select LocationID from LocationNewView 
		where LocationID=@LocationID and MappedLocationID in (select LocationID from @UserLocationTable))
	Begin 
		set @ErrorID=7
		Set @ErrorMsg=@LocationCode+'- given LocationCode not mapped with User.'
		return		
	End
	IF @AssetLocationID=@LocationID
	Begin
	 set @ErrorID=8
		Set @ErrorMsg=@LocationCode+'- Asset Location and To Location are the same.'
		return	
	End 	
End 
go 
create Procedure [dbo].[prc_ImportTemplateUserAgainstList]
  (
  @UserID int 
  )
  as 
  Begin 
	Declare  @UserRightTable Table(ActionName nvarchar(100),Rightvalue int)
	Insert into @UserRightTable(ActionName,RightValue)
	values('Create',2)
	Insert into @UserRightTable(ActionName,RightValue)
	values('Edit',4)
	Declare @EntityTable Table(EntityID int,EntityName nvarchar(max),RightName nvarchar(max))
	  insert into @EntityTable(EntityID,EntityName,RightName)
	
	 select EntityID,EntityName,case when EntityName='POLineItemUpdate' then 'Asset'
	  when EntityName='UserApprovalRoleMapping' then 'UserMaster'
	  when EntityName='UserLocationMapping' then 'UserMaster'
	  when EntityName='UserCategoryMapping' then 'UserMaster'
	  when EntityName='CustomAsset' then 'Asset'
	  when EntityName='ImportAssetTransfer' then 'InternalAssetTransfer'
	  when EntityName='Custodian' then 'UserMaster'
	  Else EntityName end 
	  from EntityTable where QueryString is not null 

	Declare @UserUserRightTable Table(RightName nvarchar(100),RightValue int,AvailabilityRight nvarchar(10),UserID nvarchar(100))
	insert into @UserUserRightTable(RightName,RightValue,AvailabilityRight,UserID)
	 select * from (
	Select  Rightname,DR.RightValue,
	case when UR.RightValue&DR.RightValue=DR.RightValue then 'Y' Else 'N' End as AvailabilityRight,ur.UserID
	from User_RightTable a join User_RightGroupTable b on a.rightgroupID=b.RightGroupID 
	Left join User_UserRightTable UR on a.RightID=UR.RightID
	Left join PersonTable R on UR.userID=R.PersonID
	cross join @UserRightTable DR 
	where PersonID=@UserID and rightname in (select RightName from @EntityTable)
	and UR.RightValue>0 and(@userID is null or @userID='' or R.PersonID=@userID)
	and IsActive=1 and IsDeleted=0
	union all 
	Select  Rightname,DR.RightValue,
	case when UR.RightValue&DR.RightValue=DR.RightValue then 'Y' Else 'N' End as AvailabilityRight,
	p.PersonID as UserID
	from User_RightTable a join User_RightGroupTable b on a.rightgroupID=b.RightGroupID 
	Left join User_RoleRightTable UR on a.RightID=UR.RightID
	left join User_RoleTable R on ur.RoleID=r.RoleID
	left join User_UserRoleTable RU on R.RoleID=RU.RoleID
	Left join PersonTable p on RU.userID=p.PersonID
	cross join @UserRightTable DR 
	where PersonID=@UserID and rightname in (select RightName from @EntityTable)
	and UR.RightValue>0 and(@userID is null or @userID='' or p.PersonID=@userID)
	and a.IsActive=1 and a.IsDeleted=0
	) x 
	group by RightName,RightValue,AvailabilityRight,UserID


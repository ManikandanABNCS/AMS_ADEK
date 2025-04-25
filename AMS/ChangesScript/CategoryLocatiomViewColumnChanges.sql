create Procedure [dbo].[prc_GetAssetDetails]
(
 @LocationID int = null,
  @CategoryID int = null
) 
as 
begin 
   Declare @L2LocID int, @LocTypeID int,@categoryTypeID int,@L2catID int 
    select @L2LocID=Level2ID from tmp_LocationNewHierarchicalView where LocationID=@LocationID
   select @LocTypeID=LocationTypeID from LocationTable where LocationID=@L2LocID
   select @L2catID=Level2ID from tmp_CategoryNewHierarchicalView where CategoryID=@CategoryID
   select @categoryTypeID=CategoryTypeID from CategoryTable where CategoryID=@L2catID
    
   select @LocTypeID as LocationTypeID,@L2LocID as L2LocationID,@categoryTypeID as CategoryTypeID
End 
GO

 Alter Function [dbo].[GetSecondLevelLocationValue]
(
@LocationID int 
) returns int 
as 
begin
 declare @L2ID int 

 select @L2ID=Level2ID from LocationNewHierarchicalView where LocationID=@LocationID
 return @L2ID
end 
GO



alter View [dbo].[TransactionLineItemViewForTransfer]
  as 

	Select a1.*,old.LocationNameHierarchy as OldLocationName,New.LocationNameHierarchy as NewLocationName,
	a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubTypeName,
	ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
	TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
	VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
	OldDept.DepartmentName as OldDepartmentName,NewDept.DepartmentName as NewDepartmentName,OldCate.CategoryName as OldCategoryName,
	Oldprod.ProductName as OldProductName,newprod.ProductName as NewProductName,OldSec.SectionName as oldSectionName,newsec.SectionName as NewSectionName,
	newCate.CategoryName as NewCategoryName
		From TransactionTable a 
		join TransactionLineItemTable b on a.TransactionID=b.TransactionID 
		join AssetNewView a1 on b.AssetID=a1.AssetID 
		left join tmp_LocationNewHierarchicalView Old on b.FromLocationID=Old.LocationID
		left join tmp_LocationNewHierarchicalView new on b.ToLocationID=new.LocationID
		Left join DepartmentTable OldDept on b.FromDepartmentID=OldDept.DepartmentID
		left join DepartmentTable NewDept on b.ToDepartmentID=NewDept.DepartmentID
		Left join CategoryTable OldCate on b.FromCategoryID=OldCate.CategoryID
		left join CategoryTable newCate on b.ToCategoryID=newCate.CategoryID
		Left join ProductTable Oldprod on b.FromProductID=Oldprod.ProductID
		left join ProductTable newprod on b.ToProductID=newprod.ProductID
		LEft join SectionTable OldSec on b.FromSectionID=oldsec.SectionID
		left join SectionTable newsec on b.ToSectionID=newsec.SectionID
		Left join TransactionSubTypeTable TST on a.TransactionSubTypeID=TST.TransactionSubTypeID
  --drop table tmp_LocationNewHierarchicalView
GO

alter View [dbo].[nvwAssetTransfer_ForNotification]
as 
  select  a1.*,old.LocationName as OldLocationName,New.LocationName as NewLocationName,
  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubTypeName,
ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
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

alter View [dbo].[nvwAssetRetirement_ForNotification]
as 
  select  a1.AssetCode,a1.Barcode,a1.CategoryName,a1.LocationName,a1.DepartmentName,a1.SectionDescription,a1.CustodianName,a1.AssetDescription,
  a1.AssetCondition,a1.suppliername,
  
  old.LocationName as OldLocationName,
  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubTypeName,
ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
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

Alter View [dbo].[nvwAssetRetirement_ForBeforeApproval]
as 
  select  a1.AssetCode,a1.Barcode,a1.CategoryName,a1.LocationName,a1.DepartmentName,a1.SectionDescription,a1.CustodianName,a1.AssetDescription,
  a1.AssetCondition,a1.suppliername,
  
  old.LocationName as OldLocationName,
  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubTypeName,
ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
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

alter View [dbo].[nvwAssetTransfer_ForBeforeApproval]
as 
  select  a1.*,old.LocationName as OldLocationName,New.LocationName as NewLocationName,
  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubTypeName,
ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
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


alter View [dbo].[nvwAssetRetirement_ForAfterApproval]
as 
select a1.AssetCode,a1.Barcode,a1.CategoryName,a1.LocationName,a1.DepartmentName,a1.SectionDescription,a1.CustodianName,a1.AssetDescription,
  a1.AssetCondition,a1.suppliername,
  
  old.LocationName as OldLocationName,
  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubTypeName ,
ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
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
  where a.StatusID = dbo.fnGetActiveStatusID() and a.TransactionTypeID=10
GO

alter View [dbo].[nvwAssetTransfer_ForAfterApproval]
as 
Select a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubTypeName,
ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
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

create VIEW [dbo].[TransactionApprovalView] 
 as 
 Select ApprovalHistoryID,ApproveWorkFlowID,ApproveWorkFlowLineItemID,a.ApproveModuleID,a.ApprovalRoleID,a.TransactionID as ApprovalTransactionID,
		OrderNo,a.Remarks as ApprovalRemarks,FromLocationID,ToLocationID,FromLocationTypeID,ToLocationTypeID,a.StatusID as ApprovalStatusID,
        a.CreatedBy as ApprovalCreatedBy,a.CreatedDateTime as ApprovalCreatedDateTime,a.LastModifiedBy,a.LastModifiedDateTime,ObjectKeyID1,EmailsecrectKey ,
		b.TransactionID,TransactionNo,TransactionTypeID,TransactionSubTypeName,ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,b.Remarks,TransactionDate,
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
	where a.StatusID=150
GO


alter view [dbo].[TransactionView] 
as 

SElect a.TransactionID,a.TransactionNo,a.TransactionTypeID,TST.TransactionSubTypeName,
a.ReferenceNo,a.CreatedFrom,a.SourceTransactionID,a.SourceDocumentNo,
a.Remarks,a.TransactionDate,a.TransactionValue,a.StatusID,
a.PostingStatusID,a.VerifiedBy,a.VerifiedDateTime,a.PostedBy,
a.PostedDateTime,a.CreatedBy,a.CreatedDateTime
,b.TransactionTypeName, case when c.StatusID = dbo.fnGetActiveStatusID() 
			then 'Approved' else c.Status end as Status,p.PersonFirstName+'-'+p.PersonLastName as CreatedUSer
From TransactionTable a 
join TransactionTypeTable b on a.TransactionTypeID=b.TransactionTypeID 
join  StatusTable c on a.StatusID=c.StatusID
join PersonTable p on a.CreatedBy=p.PersonID
Left join TransactionSubTypeTable TST on a.TransactionSubTypeID=TST.TransactionSubTypeID

GO

Alter Procedure [dbo].[Iprc_UserApprovalRoleMapping]
(
   @LocationCode      nvarchar(100) = NULL,
   @ApprovalRoleCode  nvarchar(100) = NULL,
   @PersonCode	      nvarchar(100) = NULL,
   @CategoryTypeCode  nvarchar(100) = null,
   @ImportTypeID int,
   @UserID int
)
as 
Begin 
set @LocationCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@LocationCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
set @ApprovalRoleCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@ApprovalRoleCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
set @PersonCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@PersonCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
Declare @LocationID int,@PersonID int ,@ApprovalRoleID int,@StatusID int,@categoryTypeID int 
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
IF (@PersonCode is not NULL and @PersonCode!='')
  BEGIN
  IF EXISTS(SELECT PersonCode FROM PersonTable WHERE PersonCode=@PersonCode and StatusID=@statusID)
	BEGIN  
		SELECT @PersonID=PersonID FROM  PersonTable WHERE PersonCode=@PersonCode and StatusID=@statusID
	END
END

IF(@PersonID IS NULL)
Begin 
     SElect @PersonCode+'- User Code not avaliable in table please add it ' as ReturnMessage
	Return 
End 

If not exists (select LocationID from LocationNewHierarchicalView  where LocationID=@LocationID and level=2) 
Begin 
  SElect @LocationCode+'- LocationCode must be n Second level please check it  ' as ReturnMessage
	Return 

end 

IF (@CategoryTypeCode is not NULL and @CategoryTypeCode!='')
  BEGIN
  IF EXISTS(SELECT CategoryTypeCode FROM CategoryTypeTable WHERE CategoryTypeCode=@CategoryTypeCode and StatusID=@statusID)
	BEGIN  
		SELECT @categoryTypeID=CategoryTypeID FROM CategoryTypeTable WHERE CategoryTypeCode=@CategoryTypeCode and StatusID=@statusID
	END
Else 
Begin 
     SElect @CategoryTypeCode+'- Category Type Code not avaliable in table please add it ' as ReturnMessage
	Return 
End 
End 

If @ImportTypeID=1
begin
	 If((@LocationID is not null and @LocationID!='') and 
			(@ApprovalRoleID is not null and @ApprovalRoleID !='') and (@PersonID is not null and @PersonID!='') and (@categoryTypeID is not null and @categoryTypeID!=''))
	Begin 

			If not Exists(SElect ApprovalRoleID from UserApprovalRoleMappingTable where UserID=@PersonID 
					and Locationid=@LocationID and ApprovalRoleID=@ApprovalRoleID and CategoryTypeID=@categoryTypeID and StatusID=@statusID)
			Begin 
			  Insert into UserApprovalRoleMappingTable(UserID,LocationID,ApprovalRoleID,StatusID,CategoryTypeID)
					values(@PersonID,@LocationID,@ApprovalRoleID,@StatusID,@categoryTypeID)
			End 
		else 
		Begin
		   SElect @PersonCode + '-[' + CAST(@userID as varchar(50)) + ']-' + @LocationCode+'-'+@ApprovalRoleCode +'-'+ @CategoryTypeCode +'- Already Exists' as ReturnMessage
			Return 
 
		end 
	End 
	Else 
	Begin 
	  SElect @PersonCode + '-[' + CAST(@userID as varchar(50)) + ']-' + @LocationCode+'-'+@ApprovalRoleCode +'-'+ @CategoryTypeCode +'- Should be mandatory' as ReturnMessage
			Return 
	end 
End 
Else 
Begin 
   Declare  @TypeID int 
   Select @TypeID=CategoryTypeID from UserApprovalRoleMappingTable  where UserID=@PersonID 
					and Locationid=@LocationID and ApprovalRoleID=@ApprovalRoleID  and StatusID=@statusID
					If(@TypeID is null or @TypeID='') 
					Begin 
					update UserApprovalRoleMappingTable set CategoryTypeID=@categoryTypeID where  UserID=@PersonID 
					and Locationid=@LocationID and ApprovalRoleID=@ApprovalRoleID  and StatusID=@statusID
					end 
					Else 
					Begin 
					     SElect @PersonCode + '-[' + CAST(@userID as varchar(50)) + ']-' + @LocationCode+'-'+@ApprovalRoleCode +'-'+ @CategoryTypeCode +'-Category Type should be nullable for update to the Approval role user.' as ReturnMessage
							Return 
					End 
end
End 
GO

Alter Procedure [dbo].[Iprc_UserLocationMapping]
(
 @PersonCode     nvarchar(100)  = NULL, 
 @LocationCode nvarchar(100)  = null,
  @ImportTypeID int,
   @UserID int

)
as 
Begin 
set @LocationCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@LocationCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
set @PersonCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@PersonCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
Declare @LocationID int,@PersonID int ,@StatusID int 
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
IF (@PersonCode is not NULL and @PersonCode!='')
  BEGIN
  IF EXISTS(SELECT PersonCode FROM PersonTable WHERE PersonCode=@PersonCode and StatusID=@statusID)
	BEGIN  
		SELECT @PersonID=PersonID FROM  PersonTable WHERE PersonCode=@PersonCode and StatusID=@statusID
	END
Else 
Begin 
     SElect @PersonCode+'- User Code not avaliable in table please add it ' as ReturnMessage
	Return 
End 

End 
If not exists (select LocationID from LocationNewHierarchicalView  where LocationID=@LocationID and level=2) 
Begin 
  SElect @LocationCode+'- LocationCode must be n Second level please check it  ' as ReturnMessage
	Return 

end 

If @ImportTypeID=1
begin
 If((@LocationID is not null and @LocationID!='') and (@PersonID is not null and @PersonID!=''))
Begin 
If not Exists(SElect LocationID from UserLocationMappingTable where PersonID=@userID and Locationid=@LocationID  and StatusID=@statusID)
Begin 
  Insert into UserLocationMappingTable(PersonID,LocationID,StatusID)
values(@PersonID,@LocationID,@StatusID)
End 
else 
Begin
   SElect @PersonCode+'-'+@LocationCode+'-Already Exists' as ReturnMessage
	Return 
 
end 
End 
End 
end 
GO

Alter Procedure [dbo].[Iprc_UserCategoryMapping]
(
 @PersonCode     nvarchar(100)  = NULL, 
 @CategoryCode nvarchar(100)  = null,
 @ImportTypeID int,
 @UserID int

)
as 
Begin 
set @CategoryCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@CategoryCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
set @PersonCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@PersonCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
Declare @categoryID int,@PersonID int ,@StatusID int 
select  @StatusID= [dbo].[fnGetActiveStatusID]()
 IF (@CategoryCode is not NULL and @CategoryCode!='')
  BEGIN 
    IF EXISTS(SELECT CAtegoryCode FROM CategoryTable WHERE CategoryCode=@CategoryCode and StatusID=@statusID)
	BEGIN  
		SELECT @categoryID=CategoryID FROM CategoryTable WHERE CategoryCode=@CategoryCode and StatusID=@statusID
	END
Else 
Begin 
     SElect @CategoryCode+'- Category Code not avaliable in table please add it ' as ReturnMessage
	Return 
End 

  End 
IF (@PersonCode is not NULL and @PersonCode!='')
  BEGIN
  IF EXISTS(SELECT PersonCode FROM PersonTable WHERE PersonCode=@PersonCode and StatusID=@statusID)
	BEGIN  
		SELECT @PersonID=PersonID FROM  PersonTable WHERE PersonCode=@PersonCode and StatusID=@statusID
	END
Else 
Begin 
     SElect @PersonCode+'- User Code not avaliable in table please add it ' as ReturnMessage
	Return 
End 

End 
If not exists (select CategoryID from CategoryNewHierarchicalView  where CategoryID=@categoryID and level=2) 
Begin 
  SElect @CategoryCode+'- Category Code must be n Second level please check it  ' as ReturnMessage
	Return 

end 

If @ImportTypeID=1
begin
 If((@categoryID is not null and @categoryID!='') and (@PersonID is not null and @PersonID!=''))
Begin 
If not Exists(SElect @categoryID from UserCategoryMappingTable where PersonID=@userID and CategoryID=@categoryID  and StatusID=@statusID)
Begin 
  Insert into UserCategoryMappingTable(PersonID,CategoryID,StatusID)
values(@PersonID,@categoryID,@StatusID)
End 
else 
Begin
   SElect @PersonCode+'-'+@CategoryCode+'-Already Exists' as ReturnMessage
	Return 
 
end 
End 
End 
end 
GO

create Procedure [dbo].[prc_GetAssetDetails]
(
 @LocationID int = null,
  @CategoryID int = null
) 
as 
begin 
   Declare @L2LocID int, @LocTypeID int,@categoryTypeID int,@L2catID int 
    select @L2LocID=Level2ID from tmp_LocationNewHierarchicalView where LocationID=@LocationID
   select @LocTypeID=LocationTypeID from LocationTable where LocationID=@L2LocID
   select @L2catID=Level2ID from tmp_CategoryNewHierarchicalView where CategoryID=@CategoryID
   select @categoryTypeID=CategoryTypeID from CategoryTable where CategoryID=@L2catID
    
   select @LocTypeID as LocationTypeID,@L2LocID as L2LocationID,@categoryTypeID as CategoryTypeID
End 
GO

create Procedure [dbo].[prc_GetCategoryType]
(
 @CategoryID int = null
) 
as 
Begin 
  Declare @categoryTypeID int,@L2catID int 
   select @L2catID=Level2ID from tmp_CategoryNewHierarchicalView where CategoryID=@CategoryID
   select @categoryTypeID=CategoryTypeID from CategoryTable where CategoryID=@L2catID
   select @categoryTypeID as CategoryTypeID
End 
GO

create Procedure [dbo].[prc_GetLocationType]
(
 @LocationID int = null,
  @CategoryID int = null
) 
as 
begin 
   Declare @L2LocID int, @LocTypeID int,@categoryTypeID int,@L2catID int 
    select @L2LocID=Level2ID from tmp_LocationNewHierarchicalView where LocationID=@LocationID
   select @LocTypeID=LocationTypeID from LocationTable where LocationID=@L2LocID
   select @L2catID=Level2ID from tmp_CategoryNewHierarchicalView where CategoryID=@CategoryID
   select @categoryTypeID=CategoryTypeID from CategoryTable where CategoryID=@L2catID
    
   select @LocTypeID as LocationTypeID,@L2LocID as L2LocationID,@categoryTypeID as CategoryTypeID
End 
GO

Alter Procedure [dbo].[Prc_InsertTempCategory]
	@CategoryID int=Null
As 
BEGIN 

   Select * into #temp2 from CategoryHierarchicalView where @CategoryID is null or @CategoryID=ChildID
  
  Select * into #temp3 from CategoryNewHierarchicalView where @CategoryID is null or @CategoryID=CategoryID
   IF(@CategoryID IS NULL)
	BEGIN
		TRUNCATE TABLE tmp_CategoryHierarchicalView
		TRUNCATE TABLE tmp_CategoryNewHierarchicalView
	END
	ELSE
	BEGIN
		DELETE FROM tmp_CategoryHierarchicalView where ChildID = @CategoryID
		DELETE FROM tmp_CategoryNewHierarchicalView where CategoryID = @CategoryID
	END
	INSERT INTO tmp_CategoryHierarchicalView
		SELECT * FROM #temp2
		INSERT INTO tmp_CategoryNewHierarchicalView
		SELECT * FROM #temp3

		drop table #temp2
		drop table #temp3
END
GO

Alter Procedure [dbo].[Prc_InsertTempLocation]
	@LocationID int=Null
As 
BEGIN 

   Select * into #temp3 from LocationHierarchicalView where @LocationID is null or @LocationID=ChildID
    Select * into #temp4 from LocationNewHierarchicalView where @LocationID is null or @LocationID=LocationID
   IF(@LocationID IS NULL)
	BEGIN
		TRUNCATE TABLE tmp_LocationHierarchicalView
		TRUNCATE TABLE tmp_LocationNewHierarchicalView
	END
	ELSE
	BEGIN
		DELETE FROM tmp_LocationHierarchicalView where ChildID = @LocationID
		DELETE FROM tmp_LocationNewHierarchicalView where LocationID = @LocationID
	END
	INSERT INTO tmp_LocationHierarchicalView
		SELECT * FROM #temp3

INSERT INTO tmp_LocationNewHierarchicalView
		SELECT * FROM #temp4

		drop table #temp3
		drop table #temp4

END
GO


create Procedure [dbo].[Prc_ValidateBulkAssetAddition] 
(
  @LocationCode   nvarchar(max) =NULL,
  @CategoryCode   nvarchar(max) = null,
  @ErrorID        int OutPut,
  @ErrorMessage   nvarchar(max) OutPut
)
as 
Begin 
    Declare @L2LocID int, @LocTypeID int,@statusID int,@ApproveworkflowID int,@categoryTypeID int,@L2catID int 
   ,@workflowCnt int ,@approvalCnt int,@CategoryCount int,@LocationCount int ,@CategoryTypeCnt int ,@LocationTypeName nvarchar(100)
   Select @statusID=[dbo].fnGetActiveStatusID()
   
   Declare @LocationIDTable Table(LocationCode nvarchar(300),LocationID int,L2LocationID int,LocationTypeID int )
   Declare @CategoryIDTable Table(CategoryCode nvarchar(300),CategoryID int ,CategoryTypeID int)
   Declare @CategoryTypeTable Table (CategoryTypeID int)

   
   Insert into @CategoryIDTable(CategoryCode)
   select value from Split(@CategoryCode,',')
   group by value
  
   insert into @LocationIDTable(LocationCode)
   select value from Split(@LocationCode,',')
   group by value

   Update a 
	   set a.CategoryID=b.CategoryID
	   From @CategoryIDTable a 
	   join CategoryTable b on a.CategoryCode=b.CategoryCode
	   Where b.StatusID=@statusID

	Update a 
	   set a.LocationID=b.LocationID
	   From @LocationIDTable a 
	   join LocationTable b on a.LocationCode=b.LocationCode
	   Where b.StatusID=@statusID

	Set @ErrorMessage=null
	Set @ErrorID=0

   if exists (select CategoryID from @CategoryIDTable where CategoryID is null)
	Begin
	   Declare @MissingCategoryCode nvarchar(max)
	   Select @MissingCategoryCode=stuff((Select ',' + T.Categorycode from @CategoryIDTable T 
	                     where T.CategoryID is null FOR XML PATH('')), 1, 2, '')

	    set @ErrorMessage =@MissingCategoryCode +' Category code(s) are Invalid.'
		Set @ErrorID=1
		return 
	End 

	if exists (select LocationID from @LocationIDTable where LocationID is null)
	Begin
		Declare @MissingLocationCode nvarchar(max)
		Select @MissingLocationCode=stuff((Select ',' + T.LocationCode from @LocationIDTable T 
	                     where T.LocationID is null  FOR XML PATH('')), 1, 2, '') 
		Set @ErrorMessage =@MissingLocationCode + ' Location code(s) are Invalid.'
		Set @ErrorID=2
		return
	End 

	Update a 
	   set a.CategoryTypeID=b.CategoryTypeID
	   From @CategoryIDTable a 
	   join CategoryNewHierarchicalView b on a.CategoryID=b.CategoryID
	   Where b.StatusID=@statusID
	
	Update a 
	   set a.LocationTypeID=b.LocationTypeID,
	   a.L2LocationID=b.MappedLocationID
	   From @LocationIDTable a 
	   join LocationNewHierarchicalView b on a.LocationID=b.locationid
	   Where b.StatusID=@statusID

		-- select * from @LocationIDTable
	if exists (Select CategoryTypeID from @CategoryIDTable where CategoryTypeID is null) 
	Begin
		Declare @MissingCategoryType nvarchar(max)
		Select @MissingCategoryType=stuff((Select ',' + T.CategoryCode from @CategoryIDTable T 
	                     where T.CategoryTypeID is null and T.CategoryID=a.CategoryID FOR XML PATH('')), 1, 1, '') from @CategoryIDTable a 
		Set @ErrorMessage =@MissingCategoryType + +' Category Type(s) are Missed.'
		Set @ErrorID=3
		return
	End 
   if exists (select LocationTypeID from @LocationIDTable where LocationTypeID is null )
	Begin 
		Declare @MissingLocationType nvarchar(max)
		Select @MissingLocationType=stuff((Select ',' +T.LocationCode from @LocationIDTable T 
	                     where T.LocationTypeID is null and T.LocationID=a.LocationID FOR XML PATH('')), 1, 1, '') from @LocationIDTable a 
		Set @ErrorMessage =@MissingLocationType + ' - Location Type(s) are Missed.'
		Set @ErrorID=8
		return
	End 
	if(select count(L2LocationID) from @LocationIDTable group by L2LocationID )>1
	Begin 
		--Declare @DifferentLocCode nvarchar(max)
		--Select @DifferentLocCode=stuff((Select T.LocationCode from @c T 
	 --                    where T.CategoryTypeID is null and T.CategoryID=a.CategoryID FOR XML PATH('')), 1, 1, '') from @CategoryIDTable a
		Set @ErrorMessage ='Location Mismatch: Same Second level Location only allowed here.'
		Set @ErrorID=3
		return
	End 
	Else 
	Begin 

	   select @L2LocID=L2LocationID from @LocationIDTable group by L2LocationID
	   select @LocTypeID=LocationTypeID from @LocationIDTable where L2LocationID=@L2LocID
	   Select @LocationTypeName=LocationTypeName from LocationTypeTable where LocationTypeID=@LocTypeID
	  
	   --print @L2LocID
	   --print @LocTypeID
	   --print @LocationTypeName
	   if(Select Count(CategoryTypeID) from @CategoryIDTable group by CategoryTypeID)>1
			  begin
				Select @categoryTypeID=CategoryTypeID from CategoryTypeTable where IsAllCategoryType=1 and statusID=@statusID
			  End 
			  Else 
			  Begin 
			  Select @categoryTypeID=CategoryTypeID from @CategoryIDTable group by CategoryTypeID
			  end 
	   end 

	if not exists( select b.ApprovalRoleID from ApproveWorkflowTable a 
					join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
						where a.StatusID=@statusID and b.StatusID=@statusID and 
						a.ApproveModuleID=(select ApproveModuleID from ApproveModuleTable where ModuleName='AssetAddition' and StatusID=@statusID)
						and a.FromLocationTypeID=@LocTypeID)
	begin 
	print 'welcome'
		Set @ErrorMessage ='Workflow not defined for LocationType-'+@LocationTypeName+'.'
		Set @ErrorID=4
		return
	end 
	Else 
	Begin 
	select @ApproveworkflowID=b.ApproveWorkFlowID from ApproveWorkflowTable a 
		join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
		where a.StatusID=@statusID and b.StatusID=@statusID and 
		a.ApproveModuleID=(select ApproveModuleID from ApproveModuleTable where ModuleName='AssetAddition' and StatusID=@statusID)
		and a.FromLocationTypeID=@LocTypeID

	declare @TransactionLineItemTable table (FromLocationL2 int ,LocationTypeID int,CategoryTypeID int,ApprovalWorkFlowID int )
	insert into @TransactionLineItemTable(FromLocationL2,LocationTypeID,CategoryTypeID,ApprovalWorkFlowID)
		values(@L2LocID,@LocTypeID,@categoryTypeID,@ApproveworkflowID)

	Declare @validationTable Table(UserID int,LocationID int,ApprovalRoleID int,CategoryTypeCnt int ) 

	Select @workflowCnt=Count(*) from ApproveWorkflowLineItemTable where ApproveWorkFlowID=@ApproveworkflowID and StatusID=@statusID

	insert into @validationTable(UserID,LocationID,ApprovalRoleID,CategoryTypeCnt)
	select userID,LocationID,a.ApprovalRoleID,count(d.categoryTypeID) as categorytypeCnt 
		from ApproveWorkflowLineItemTable a 
		join @TransactionLineItemTable T on a.ApproveWorkFlowID=T.ApprovalWorkFlowID
		join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
		join CategoryTypeTable CT on T.CategoryTypeID=Ct.CategoryTypeID
		join 
		(
		select a1.*,b1.IsAllCategoryType from UserApprovalRoleMappingTable a1  
		join CategoryTypeTable b1 on a1.CategoryTypeID=b1.CategoryTypeID
		where a1.StatusID=@statusID and b1.StatusID=@statusID
		) D on  T.FromLocationL2 =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID 
		and ct.CategoryTypeID = (case when CT.IsAllCategoryType=0 and D.IsAllCategoryType=1 then CT.CategoryTypeID else D.CategoryTypeID end)
		where ApproveWorkFlowID=@ApproveworkflowID and CT.statusID=@statusID AND d.StatusID=@statusID and a.statusID=@statusID
		group by userID,LocationID,a.ApprovalRoleID

	select @approvalCnt= count(*) from (
	select  a.ApprovalRoleID  from @validationTable a group by a.ApprovalRoleID) x
	if(isnull((@workflowCnt),0)=isnull(@approvalCnt,0))
	begin 
		Declare @validationTable1 Table(UserID int,LocationID int,ApprovalRoleID int,CategoryTypeCnt int ) 
		Declare @validationTable2 Table(UserID int,LocationID int,ApprovalRoleID int,CategoryTypeCnt int ) 
					
		insert into @validationTable1(UserID,LocationID,ApprovalRoleID,CategoryTypeCnt)
		select a.userID,a.LocationID,a.ApprovalRoleID,categorytypeCnt
			From @validationTable a  
			join [ValidateAccessRightsView] b  on a.UserID=b.UserID --and a.LocationID=b.LocationID and a.ApprovalRoleID=b.ApprovalRoleID
			and b.RightName='AssetApproval'

	if(select count(*) from (
	select  a.ApprovalRoleID  from @validationTable1 a group by a.ApprovalRoleID)x )!=@approvalCnt
	Begin 
		Declare @MissingUserName nvarchar(max)
		Select @MissingUserName=stuff((Select ',' + PersonCode from @validationTable a join PersonTable b on a.UserID=b.PersonID 
		where userID not in (select userID from @validationTable1) and b.personID=p.PersonID FOR XML PATH('')), 1, 1, '') from PersonTable P
		
		Set @ErrorMessage ='Approval Rights not provided for the user(s)- '+@MissingUserName +'.'
		Set @ErrorID=5
		return
	end 
	if (select count(*) from (
	select  a.ApprovalRoleID  from @validationTable1 a group by a.ApprovalRoleID)x  )=@approvalCnt
	begin 
		insert into @validationTable2(UserID,LocationID,ApprovalRoleID,CategoryTypeCnt)
		select a.userID,a.LocationID,a.ApprovalRoleID,categorytypeCnt
		From @validationTable a  
		join persontable p on a.userID=p.personID and p.EMailID is not null and p.StatusID=@statusID

			if(select count(*) from (
			select  a.ApprovalRoleID  from @validationTable2 a group by a.ApprovalRoleID)x  )!=@approvalCnt
			 Begin 
				Declare @MissingUserEmail nvarchar(max)
				Select @MissingUserEmail=stuff((Select ',' + PersonCode from @validationTable a join PersonTable b on a.UserID=b.PersonID 
					where userID not in (select userID from @validationTable2) and b.personID=p.PersonID FOR XML PATH('')), 1, 1, '') from PersonTable P

				Set @ErrorMessage ='EmailID not assigned to the mapped workflow user(s)- '+@MissingUserEmail +'.'
				Set @ErrorID=6

				Return
			end 
		End 

	End 
	Else 
		Begin 
		Declare @MissingApprovalRoleName nvarchar(max),@ModuleName nvarchar(max)
		Select @MissingApprovalRoleName=stuff((Select ',' + b.ApprovalRoleName From ApproveWorkflowLineItemTable a Join ApprovalRoleTable b on a.ApprovalRoleID=b.ApprovalRoleID
			where ApproveWorkFlowID=@ApproveworkflowID and a.StatusID=@statusID and b.StatusID=@statusID and a.ApprovalRoleID not in(select ApprovalRoleID from @validationTable)
				and b.ApprovalRoleID=p.ApprovalRoleID FOR XML PATH('')), 1, 1, '') from ApprovalRoleTable P
		Select @ModuleName=b.ModuleName from ApproveWorkflowTable a join ApproveModuleTable b on a.ApproveModuleID=b.ApproveModuleID where ApproveWorkFlowID=@ApproveworkflowID and a.StatusID=@statusID
		 
		Set @ErrorMessage ='Missing user(s) for the role : '+@MissingApprovalRoleName+' , ModuleName :'+@ModuleName+', LocationType : '+@LocationTypeName+'.'
		Set @ErrorID=7
		End 
	End 

End 

GO

Alter Procedure [dbo].[prc_ValidateForTransaction]
(
   @FromAssetID				nvarchar(max)	 = NULL,
   @ToLocationID			int				 = NULL,
   @FromLocationTypeID		int				 = NULL,
   @ToLocationTypeID		Int				 = NULL,
   @ApprovalWorkFlowID		int              = NULL,
   @ErrorID					int OutPut,
   @ErrorMessage			nvarchar(max) OutPut
)
as 
Begin
	Declare @UpdateCount int,@AprpovalCnt int ,@StausID int,@RightName nvarchar(100),@modelID int ,@CategoryTypeCnt int ,@CategoryType nvarchar(max),@ID int 
	select @StausID=[dbo].fnGetActiveStatusID()
	
	SElect @modelID=ApproveModuleID from ApproveWorkflowTable where ApproveWorkflowID=@ApprovalWorkFlowID
	select @RightName=case when @modelID=5  then 'AssetTransferApproval' when @modelID=11 then 'InternalAssetTransferApproval' else 'AssetRetirementApproval' end 
	
	Declare @updateTable table(updateRole bit,ApprovalRoleID int,moduleName nvarchar(100),FromLocationType nvarchar(100),ToLocationType nvarchar(100),ModuleID int)
	  
	if @modelID=11 
	begin
		select @ToLocationID =Level2id from LocationNewHierarchicalView where LocationID=@ToLocationID
	end

	declare @TransactionLineItemTable table (AssetID int,FromLocationL2 int ,ToLocationL2 int,
	LocationType nvarchar(100),CategoryType nvarchar(100),ApprovalWorkFlowID int,Barcode nvarchar(max),MappedCategryName nvarchar(max) )
	  
	insert into @TransactionLineItemTable(AssetID,FromLocationL2,ToLocationL2,LocationType,CategoryType,ApprovalWorkFlowID,Barcode,MappedCategryName)
	select AssetID,LocationL2,@ToLocationID,LocationType,categorytype,@ApprovalWorkFlowID,Barcode,B.CategoryName 
	From AssetNewView a join CategoryTable b on a.MappedCategoryID=b.CategoryID where assetid in (select value from Split(@fromAssetID,',')) 

	Select @AprpovalCnt=count(OrderNo) from 
	ApproveWorkflowTable a join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
	where a.StatusID=@StausID and b.StatusID=@StausID and a.ApproveWorkflowID=@ApprovalWorkFlowID

	Select @CategoryTypeCnt=count(CategoryType) from @TransactionLineItemTable group by LocationType
	if(@CategoryTypeCnt>1)
	begin
		Select @CategoryType=CategoryTypeName from CategoryTypeTable where IsAllCategoryType=1 and statusID=@StausID
		update @TransactionLineItemTable set CategoryType=@CategoryType
	End 
	Else 
	Begin 
		Select @CategoryType=CategoryType from @TransactionLineItemTable group by CategoryType
	end 

	Set @ErrorID=0
	Set @ErrorMessage=null

	if not exists(select * from @TransactionLineItemTable where CategoryType is not null and CategoryType!='') 
	begin 
		Declare @MissingCategoryName nvarchar(max)
		Select @MissingCategoryName=stuff((Select ',' + T.MappedCategryName from @TransactionLineItemTable T 
							 where T.CategoryType is null and T.AssetID=a.AssetID FOR XML PATH('')), 1, 1, '') from @TransactionLineItemTable a 
		Set @ErrorID=1
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
		where a.ApproveWorkflowID=@ApprovalWorkFlowID and a.StatusID=@StausID and b.StatusID=@StausID and c.StatusID=@StausID

	Declare @MissedModuleName nvarchar(100),@MissedFromLocationtype nvarchar(100),@MissedToLocationtype nvarchar(100),@MissedModuleID int,@errorMsg nvarchar(max)
	Select top 1 @MissedModuleName=moduleName,@MissedFromLocationtype=FromLocationType,@MissedToLocationtype=ToLocationType,@MissedModuleID=ModuleID from @updateTable
	If(@modelID=5 or @modelID=11 or @modelID=10)
	Begin
		if not exists(select * from @updateTable where updateRole=1)
		Begin 
			Set @ErrorID=2
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
			Set @ErrorID=3
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
			where a1.StatusID=@StausID and b1.StatusID=@StausID
		) D on  case when b.ApprovalLocationTypeID=5 then 
		T.FromLocationL2 when b.ApprovalLocationTypeID=10 then T.ToLocationL2 else T.FromLocationL2 end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID 
		and ct.CategoryTypeID = (case when CT.IsAllCategoryType=0 and D.IsAllCategoryType=1 then CT.CategoryTypeID else D.CategoryTypeID end)
			where ApproveWorkFlowID=@ApprovalWorkFlowID and CT.statusID=@StausID and a.StatusID=@StausID
			and b.StatusID=@StausID and D.StatusID=@StausID
				group by userID,LocationID,a.ApprovalRoleID

	if(select count(*) from (
			select  a.ApprovalRoleID  from @validationTable a group by a.ApprovalRoleID)x) !=@AprpovalCnt
	Begin 
		Declare @MissingApprovalRoleName nvarchar(max),@ModuleName nvarchar(max)
		
		Select @MissingApprovalRoleName=stuff((Select ',' + b.ApprovalRoleName From ApproveWorkflowLineItemTable a Join ApprovalRoleTable b on a.ApprovalRoleID=b.ApprovalRoleID
		where ApproveWorkFlowID=@ApprovalWorkFlowID and a.StatusID=@StausID and b.StatusID=@StausID and a.ApprovalRoleID not in(select ApprovalRoleID from @validationTable)
		and b.ApprovalRoleID=p.ApprovalRoleID FOR XML PATH('')), 1, 1, '') from ApprovalRoleTable P
		
		Select @ModuleName=b.ModuleName from ApproveWorkflowTable a join ApproveModuleTable b on a.ApproveModuleID=b.ApproveModuleID where ApproveWorkFlowID=@ApprovalWorkFlowID and a.StatusID=@StausID
		 
		Set @ErrorMessage ='Missing user(s) for the role : '+@MissingApprovalRoleName+' , ModuleName :'+@ModuleName+'.'
		Set @ErrorID=4
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
		select  a.ApprovalRoleID  from @validationTable1 a group by a.ApprovalRoleID)x )!=@AprpovalCnt
	Begin 
		Set @ErrorID=5
		Declare @MissingUserName nvarchar(max)

		Select @MissingUserName=stuff((Select ',' + PersonCode from @validationTable a join PersonTable b on a.UserID=b.PersonID 
		where userID not in (select userID from @validationTable1) and b.personID=p.PersonID FOR XML PATH('')), 1, 1, '') from PersonTable P
		
		Set @ErrorMessage ='Approval Rights not provided for the user(s)- '+@MissingUserName +'.'
		return
	END
	if (select count(*) from (
		select  a.ApprovalRoleID  from @validationTable1 a group by a.ApprovalRoleID)x  )=@AprpovalCnt
	begin 
		insert into @validationTable2(UserID,LocationID,ApprovalRoleID,CategoryTypeCnt)
		select a.userID,a.LocationID,a.ApprovalRoleID,categorytypeCnt
		From @validationTable a  
		join persontable p on a.userID=p.personID and p.EMailID is not null and p.StatusID=@StausID
 if(select count(*) from (
	select  a.ApprovalRoleID  from @validationTable2 a group by a.ApprovalRoleID)x  )!=@AprpovalCnt
	 Begin 
		Declare @MissingUserEmail nvarchar(max)

		Select @MissingUserEmail=stuff((Select ',' + PersonCode from @validationTable a join PersonTable b on a.UserID=b.PersonID 
			where userID not in (select userID from @validationTable2) and b.personID=p.PersonID FOR XML PATH('')), 1, 1, '') from PersonTable P
		
		Set @ErrorMessage ='EmailID not assigned to the mapped workflow user(s)- '+@MissingUserEmail +'.'
		Set @ErrorID=6
		RETURN
	 END 
	END 
	END
		
		
  end  
GO

Alter Procedure [dbo].[Prc_ValidateUserWiseTransactions]
(
	@UserID   int	= NULL,
	@ModuleID int	= NULL
)
as 
Begin
	Declare @StatusID int 
	SElect @StatusID=[dbo].[fnGetActiveStatusID]()
     Select TransactionID
		From TransactionLineItemTable a 
		join tmp_LocationNewHierarchicalView b on a.FromLocationID=b.LocationID
		where b.StatusID not in (500,3) and TransactionID in
			(
				Select TransactionID from TransactionTable where (@ModuleID is null  or @moduleID='' or TransactionTypeID=@ModuleID)
			)
			and b.Level2ID in 
			(
			   select LocationID from UserLocationMappingTable where userID=@UserID
			) or a.ToLocationID in (select LocationID from UserLocationMappingTable where userID=@UserID)


end 
GO
ALTER Procedure [dbo].[rprc_AssetRetirementReceiptHeader]
(
  @TransactionID int 
)
as 
Begin 
  select  P1.PersonFirstName+'-'+p1.PersonLastName as SenderName,L1.LocationNameHierarchy as FromLocationName,
      p1.EMailID as SenderEmailID,P1.MobileNo as SenderPhoneno,a.Remarks as Comments,AP.SenderJobTitle as SenderJobTitle,AP.ApproveDate,Ap.SignaturePath
	  ,Ap.FromStamp,ap.ToStamp,a.TransactionNo,a.TransactionNo as Receiptyear,RE.RetirementTypeName--'GS/AM/'+convert(nvarchar(100),YEAR(a.CreatedDateTime))+'/AR/' + TransactionNo  as Receiptyear
  from TransactionTable a 
	join (select * from (select  ROW_NUMBER() over( 
         PARTITION BY transactionid  Order by AssetID asc) as serialNo,* from TransactionLineItemTable)y where y.SerialNo=1 ) TL on a.TransactionID=TL.TransactionID
	join PersonTable P1 on a.CreatedBy=p1.PersonID
	Left join DesignationTable D on P1.DesignationID=d.DesignationID
    Left join RetirementTypeTable RE on TL.RetirementTypeID=RE.RetirementTypeID
	Left join LocationNewHierarchicalView L1 on TL.FromLocationID=L1.locationiD
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

   Select P1.PersonFirstName+'-'+p1.PersonLastName as SenderName,L1.LocationNameHierarchy as FromLocationName,L2.LocationNameHierarchy as ToLocationName,P2.PersonFirstName+'-'+p2.PersonLastName as ReceiverName,
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
   Left join LocationNewHierarchicalView L1 on TL.FromLocationID=L1.LocationID
   Left join LocationNewHierarchicalView L2 on TL.ToLocationID=L2.LocationID
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

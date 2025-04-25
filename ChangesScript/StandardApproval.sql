 --CREATE VIEW TransactionApprovalView 
 --as 
 --Select ApprovalHistoryID,ApproveWorkFlowID,ApproveWorkFlowLineItemID,a.ApproveModuleID,a.ApprovalRoleID,a.TransactionID as ApprovalTransactionID,
	--	OrderNo,a.Remarks as ApprovalRemarks,FromLocationID,ToLocationID,FromLocationTypeID,ToLocationTypeID,a.StatusID as ApprovalStatusID,
 --       a.CreatedBy as ApprovalCreatedBy,a.CreatedDateTime as ApprovalCreatedDateTime,a.LastModifiedBy,a.LastModifiedDateTime,ObjectKeyID1,EmailsecrectKey ,
	--	b.TransactionID,TransactionNo,TransactionTypeID,TransactionSubType,ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,b.Remarks,TransactionDate,
	--	TransactionValue,b.StatusID,PostingStatusID,VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,b.CreatedBy,b.CreatedDateTime, 
	--	Dense_rank() over( PARTITION BY a.transactionid , a.approvemoduleID ORDER BY Orderno asc ) AS SerialNo ,
	--	p.PersonFirstName+'-'+p.PersonLastName as CreatedUSer,s.Status as ApprovalStatus,
	--	case when a.ApproveModuleID=10 then  isnull(c.UpdateRetirementDetailsForEachAssets,0)
	--	     when a.ApproveModuleID=5 or  a.ApproveModuleID=11 then isnull(c.UpdateDestinationlocationsForTransfer,0) end as enableUpdate,
	--		 isnull(DD.DelegatedEmployeeID,D.UserID) as UserID
	--From ApprovalHistoryTable a join TransactionTable b on a.TransactionID=b.TransactionID 
	--join ApprovalRoleTable c on a.ApprovalRoleID=c.ApprovalRoleID
	--join PersonTable p on b.CreatedBy=p.PersonID
	--join StatusTable s on a.StatusID=s.StatusID
	--join CategoryTypeTable CT on a.CategoryTypeID=CT.CategoryTypeID
	--Left join ApprovalHistoryMappedUser D on D.ApprovalRoleID=c.ApprovalRoleID  and 
	--case when c.ApprovalLocationTypeID=5 then 
	--    a.FromLocationID else a.ToLocationID end =D.LocationID and a.TransactionID=D.TransactionID and a.ApproveModuleID=D.ApproveModuleID
	--Left join (select * from DelegateRoleTable where  getdate() between EffectiveStartDate and EffectiveEndDate) DD on DD.EmployeeID=D.UserID
	--where a.StatusID=150
	--go 

	if not exists(select Mastergridname from MasterGridNewTable where MasterGridName='TransactionApproval') 
Begin
  Insert into MasterGridNewTable(MasterGridName,EntityName)
  values('TransactionApproval','TransactionApprovalView')
end 
go

if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='TransactionNo' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='TransactionApproval')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'TransactionNo','TransactionNo',100,NULL,1,1,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='TransactionApproval'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='CreatedUSer' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='TransactionApproval')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'CreatedUSer','CreatedUSer',100,NULL,1,2,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='TransactionApproval'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='CreatedDateTime' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='TransactionApproval')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'CreatedDateTime','CreatedDateTime',100,'dd/MM/yyyy hh:mm:ss',1,3,'System.DateTime',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='TransactionApproval'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='ApprovalStatus' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='TransactionApproval')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'ApprovalStatus','ApprovalStatus',100,NULL,1,4,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='TransactionApproval'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='enableUpdate' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='TransactionApproval')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'enableUpdate','enableUpdate',100,NULL,0,5,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='TransactionApproval'
	End 
	go 

	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='ApprovalHistoryID' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='TransactionApproval')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'ApprovalHistoryID','ApprovalHistoryID',100,NULL,0,6,'System.Int32',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='TransactionApproval'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='TransactionID' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='TransactionApproval')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'TransactionID','TransactionID',100,NULL,0,7,'System.Int32',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='TransactionApproval'
	End 
	go 
	
--	select * from User_MenuTable where MenuName='AssetRetirementApproval' Order by 1 desc 

	update User_MenuTable set TargetObject='/TransactionApproval/Index?pageName=AssetTransferApproval' where MenuName='AssetTransferApproval' and ParentTransactionID=1
	go 
	
	update User_MenuTable set TargetObject='/TransactionApproval/Index?pageName=InternalAssetTransferApproval' where MenuName='InternalAssetTransferApproval' and ParentTransactionID=1
	go 
	update User_MenuTable set TargetObject='/TransactionApproval/Index?pageName=AssetRetirementApproval' where MenuName='AssetRetirementApproval' and ParentTransactionID=1
	go 

	if not exists(select Mastergridname from MasterGridNewTable where MasterGridName='ApprovalHistory') 
	Begin
	  Insert into MasterGridNewTable(MasterGridName,EntityName)
	  values('ApprovalHistory','ApprovalHistoryView')
	end 
go

if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='ApprovalRoleName' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='ApprovalHistory')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'ApprovalRoleName','ApprovalRoleName',100,NULL,1,1,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='ApprovalHistory'
	End 
	go 

	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='UserName' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='ApprovalHistory')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'UserName','UserName',100,NULL,1,2,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='ApprovalHistory'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='ApprovalStatus' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='ApprovalHistory')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'ApprovalStatus','ApprovalStatus',100,NULL,1,3,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='ApprovalHistory'
	End 
	go 
		if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='Remarks' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='ApprovalHistory')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'Remarks','Remarks',100,NULL,1,4,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='ApprovalHistory'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='DocumentName' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='ApprovalHistory')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'DocumentName','DocumentName',100,NULL,1,5,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='ApprovalHistory'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='ApprovedBy' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='ApprovalHistory')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'ApprovedBy','ApprovedBy',100,NULL,1,6,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='ApprovalHistory'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='ApprovedDatetime' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='ApprovalHistory')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'ApprovedDatetime','ApprovedDatetime',100,'dd/MM/yyyy hh:mm:ss',1,7,'System.DateTime',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='ApprovalHistory'
	End 
	go 

	if not exists(select Mastergridname from MasterGridNewTable where MasterGridName='TransactionLineItem') 
	Begin
	  Insert into MasterGridNewTable(MasterGridName,EntityName)
	  values('TransactionLineItem','TransactionLineItemViewForTransfer')
	end 
go
if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='Barcode' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='TransactionLineItem')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'Barcode','Barcode',100,NULL,1,1,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='TransactionLineItem'
	End 
	go 

	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='CategoryHierarchy' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='TransactionLineItem')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'CategoryHierarchy','CategoryHierarchy',100,NULL,1,2,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='TransactionLineItem'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='AssetDescription' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='TransactionLineItem')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'AssetDescription','AssetDescription',100,NULL,1,3,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='TransactionLineItem'
	End 
	go 
		if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='OldLocationName' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='TransactionLineItem')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'OldLocationName','OldLocationName',100,NULL,1,4,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='TransactionLineItem'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='NewLocationName' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='TransactionLineItem')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'NewLocationName','NewLocationName',100,NULL,1,6,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='TransactionLineItem'
	End 
	go 

	
ALTER View [dbo].[PersonView] 
as 
 Select A.[PersonID]
      ,A.[PersonCode]
      ,A.[PersonFirstName]
      ,A.[PersonLastName]
      ,A.[PersonFirstNameLanguageTwo]
      ,A.[PersonLastNameLanguageTwo]
      ,A.[EMailID]
      ,A.[DepartmentID]
      ,A.[DOJ]
      ,A.[Gender]
      ,A.[Culture]
      ,A.[UserTypeID]
      ,A.[MobileNo]
      ,A.[WhatsAppMobileNo]
      ,A.[StatusID]
	  ,B.Username as Username, C.Status, D.UserType
	From PersonTable a 
	join User_LoginUserTable b on b.userID=A.personID
	join StatusTable C on a.StatusID=C.StatusID
	Left Join DepartmentTable DP ON DP.DepartmentID = A.DepartmentID
	Left join UserTypeTable D on a.UserTypeID=D.UserTypeID
	where a.StatusID=[dbo].fnGetActiveStatusID()
GO

	update user_righttable set  ValueType=95 where RightName='AssetRetirementApproval'
	go 

	ALTER Procedure [dbo].[prc_GetSecondLevelLocationValue]
(
@LocationID int,
@AssetIDS nvarchar(max)
)
as 
begin 
declare @L2ID int,@loc int,@parentLoc int,@toparentid int,@returnMsg nvarchar(max) ,@countAsset int ,@CheckCount int 
--select @level=Level from LocationNewHierarchicalView where ChildID=@FromLocationID
Declare @secondLevelTable Table(LocationL2 int)
insert into @secondLevelTable(LocationL2) 
Select LocationL2 from AssetNewView where assetid in (select value from [dbo].Split(@AssetIDS,','))
group by LocationL2


--select @parentLoc=ParentLocationID from LocationTable where locationid=@loc
select @toparentid=Level2ID from tmp_LocationHierarchicalView where ChildID=@LocationID
select @countAsset=count(*) from @secondLevelTable
select @CheckCount=count(*) from @secondLevelTable where LocationL2=@toparentid

if(@countAsset!=@CheckCount)
 --select @L2ID=Level2ID from LocationNewHierarchicalView where ChildID=@LocationID
 --select @L2ID as SecondLevelID
 set @returnMsg='Selected Location Parent Level not matched'
end 
select @returnMsg as returnMsg
go 


--ALTER View [dbo].[nvwAssetTransfer_ForBeforeApproval]
--as 
--  select  a1.*,old.LocationName as OldLocationName,New.LocationName as NewLocationName,
--  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubType,
--ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
--TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
--VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
--OldDept.DepartmentName as OldDepartmentName,NewDept.DepartmentName as NewDepartmentName,OldCate.CategoryName as OldCategoryName,
--Oldprod.ProductName as OldProductName,newprod.ProductName as NewProductName,OldSec.SectionName as oldSectionName,newsec.SectionName as NewSectionName,

--newCate.CategoryName as NewCategoryName,
-- case when ah.ApproveModuleID=5 and ad.UpdateDestinationLocationsForTransfer=1   then dbo.fn_GetServerURL() else 
--  case when ah.ApproveModuleID=11 then FORMATMESSAGE( dbo.fn_GetServerURL()+'TransactionApproval/EmailEdit?id=%d&UserID=%d', ah.ApprovalHistoryID,R.UserID)
--  else FORMATMESSAGE( dbo.fn_GetServerURL()+'TransactionApproval/EmailEdit?id=%d&UserID=%d' , ah.ApprovalHistoryID,R.UserID)
 
-- end end   ApprovalURL,
----NULL ApprovalURL,
----Notification supporting fields
--		A.TransactionID SYSDataID1, AH.ApprovalHistoryID SYSDataID2, '' SYSDataID3,R.UserID as SYSUserID,
--		R.EMailID SYSToAddresses, '' SYSCCAddresses, '' SYSBCCAddresses,
--		NULL SYSToMobileNos, NULL SYSWhatsAppMobileNos,AD.ApprovalRoleID
		
--  From TransactionTable a 
--  join TransactionLineItemTable b on a.TransactionID=b.TransactionID 
--  join AssetNewView a1 on b.AssetID=a1.AssetID 
--  left join ApprovalHistoryTable AH on a.TransactionID=AH.TransactionID --and ah.StatusID=1501
--  Left join CategoryTypeTable CT on ah.CategoryTypeID=Ct.CategoryTypeID
--  Left join ApprovalRoleTable AD on AH.ApprovalRoleID=AD.ApprovalRoleID and ad. statusID=1
--  left join LocationTable Old on b.FromLocationID=Old.LocationID
--  left join LocationTable new on b.ToLocationID=new.LocationID
--  Left join DepartmentTable OldDept on b.FromDepartmentID=OldDept.DepartmentID
--  left join DepartmentTable NewDept on b.ToDepartmentID=NewDept.DepartmentID
--  Left join CategoryTable OldCate on b.FromCategoryID=OldCate.CategoryID
--  left join CategoryTable newCate on b.ToCategoryID=newCate.CategoryID
--   Left join ProductTable Oldprod on b.FromProductID=Oldprod.ProductID
--  left join ProductTable newprod on b.ToProductID=newprod.ProductID
--  LEft join SectionTable OldSec on b.FromSectionID=oldsec.SectionID
--  left join SectionTable newsec on b.ToSectionID=newsec.SectionID
--  Left join ApprovalHistoryMappedUser D on D.ApprovalRoleID=AD.ApprovalRoleID  and 
--							case when AD.ApprovalLocationTypeID=5 then 
--					AH.FromLocationID else AH.ToLocationID end =D.LocationID and AH.TransactionID=D.TransactionID and AH.ApproveModuleID=D.ApproveModuleID
--					join ValidateAccessRightsView  R  on D.UserID=R.UserID  and RightName= 'AssetTransferApproval' and  R.EMailID is not null
--  --Left join UserRightValueView P2 on P2.ApprovalRoleID = AH.ApprovalRoleID  and RightName = 'AssetTransferApproval'
--  --and case when AD.ApprovalLocationTypeID=5 then 
--		--AH.FromLocationID else AH.ToLocationID end =P2.LocationID  and 
--		--case when CT.IsAllCategoryType=1 then P2.CategoryTypeID else AH.CategorytypeID end =P2.CategoryTypeID  and p2.EMailID is not null
----
--WHERE A.StatusID in (150,200) and a.TransactionTypeID in (5,11)

--GO

--ALTER View [dbo].[nvwAssetTransfer_ForAfterApproval]
--as 
--Select a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubType,
--ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
--TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
--VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
--a1.*,P.EMailID as SYSToAddresses, '' SYSCCAddresses, '' SYSBCCAddresses,
--		NULL SYSToMobileNos, NULL SYSWhatsAppMobileNos,A.TransactionID SYSDataID1, NULL SYSDataID2, NULL SYSDataID3,old.LocationName as OldLocationName,New.LocationName as NewLocationName,
--		Room.LocationName as RoomName,
--		CAse when a.TransactionTypeID=5 then 
--		FORMATMESSAGE( dbo.fn_GetServerURL()+'TransactionApproval/EmailView?id=%d' , a.TransactionID) else FORMATMESSAGE( dbo.fn_GetServerURL()+'TransactionApproval/EmailView?id=%d' , a.TransactionID) end   ApprovalURL,NULL as SYSUserID
--  From TransactionTable a 
--  join TransactionLineItemTable b on a.TransactionID=b.TransactionID 
--  join AssetNewView a1 on b.AssetID=a1.AssetID 
--   --left join ApprovalHistoryTable AH on a.TransactionID=AH.TransactionID and ah.StatusID = dbo.fnGetActiveStatusID()
--   Left join PersonTable p on a.CreatedBy=p.PersonID and p.EMailID is not null
--    left join LocationTable Old on b.FromLocationID=Old.LocationID
--	left join LocationTable new on b.ToLocationID=new.LocationID
--	Left join LocationTable Room on b.RoomID=Room.LocationID
--	where  a.TransactionTypeID in (5,11)
--GO


--ALTER View [dbo].[nvwAssetTransfer_ForNotification]
--as 
--  select  a1.*,old.LocationName as OldLocationName,New.LocationName as NewLocationName,
--  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubType,
--ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
--TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
--VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
--OldDept.DepartmentName as OldDepartmentName,NewDept.DepartmentName as NewDepartmentName,OldCate.CategoryName as OldCategoryName,
--Oldprod.ProductName as OldProductName,newprod.ProductName as NewProductName,OldSec.SectionName as oldSectionName,newsec.SectionName as NewSectionName,

--newCate.CategoryName as NewCategoryName,
--case when ah.ApproveModuleID=5 then 
-- FORMATMESSAGE( dbo.fn_GetServerURL()+'TransactionApproval/EmailEdit?id=%d&UserID=%d' , ah.ApprovalHistoryID,R.UserID)
-- else  FORMATMESSAGE( dbo.fn_GetServerURL()+'TransactionApproval/EmailEdit?id=%d&UserID=%d' , ah.ApprovalHistoryID,R.UserID)
-- end 
-- ApprovalURL,

----Notification supporting fields
--		A.TransactionID SYSDataID1, AH.ApprovalHistoryID SYSDataID2, '' SYSDataID3,R.UserID as SYSUserID,
		 
--		  R.EMailID SYSToAddresses,
--		  '' SYSCCAddresses, '' SYSBCCAddresses,
--		NULL SYSToMobileNos, NULL SYSWhatsAppMobileNos,(SELECT EMAILID FROM PERSONTABLE WHERE PERSONID=AH.CREATEDBY) as ApprovedBy,convert(nvarchar(100),ah.CreatedDateTime,103) as ApprovedDate
		
--  From TransactionTable a 
--  join TransactionLineItemTable b on a.TransactionID=b.TransactionID 
--  join AssetNewView a1 on b.AssetID=a1.AssetID 
--  left join ApprovalHistoryTable AH on a.TransactionID=AH.TransactionID 
--   Left join CategoryTypeTable CT on ah.CategoryTypeID=Ct.CategoryTypeID
--  left join LocationTable Old on b.FromLocationID=Old.LocationID
--  Left join ApprovalRoleTable AD on AH.ApprovalRoleID=AD.ApprovalRoleID and ad. statusID=1
--  left join LocationTable new on b.ToLocationID=new.LocationID
--  Left join DepartmentTable OldDept on b.FromDepartmentID=OldDept.DepartmentID
--  left join DepartmentTable NewDept on b.ToDepartmentID=NewDept.DepartmentID
--  Left join CategoryTable OldCate on b.FromCategoryID=OldCate.CategoryID
--  left join CategoryTable newCate on b.ToCategoryID=newCate.CategoryID
--   Left join ProductTable Oldprod on b.FromProductID=Oldprod.ProductID
--  left join ProductTable newprod on b.ToProductID=newprod.ProductID
--  LEft join SectionTable OldSec on b.FromSectionID=oldsec.SectionID
--  left join SectionTable newsec on b.ToSectionID=newsec.SectionID
--    Left join ApprovalHistoryMappedUser D on D.ApprovalRoleID=AD.ApprovalRoleID  and 
--							case when AD.ApprovalLocationTypeID=5 then 
--					AH.FromLocationID else AH.ToLocationID end =D.LocationID and AH.TransactionID=D.TransactionID and AH.ApproveModuleID=D.ApproveModuleID
--					join ValidateAccessRightsView  R  on D.UserID=R.UserID  and RightName= 'AssetTransferApproval' and  R.EMailID is not null
--  -- Left join UserRightValueView P2 on P2.ApprovalRoleID = AH.ApprovalRoleID  and RightName = 'AssetTransferApproval'
--  --and case when AD.ApprovalLocationTypeID=5 then 
--		--AH.FromLocationID else AH.ToLocationID end =P2.LocationID and 
--		--case when CT.IsAllCategoryType=1 then P2.CategoryTypeID else AH.CategorytypeID end =P2.CategoryTypeID   and p2.EMailID is not null


--WHERE  a.TransactionTypeID in (5,11)

--GO




--ALTER View [dbo].[nvwAssetRetirement_ForAfterApproval]
--as 
--select a1.AssetCode,a1.Barcode,a1.CategoryName,a1.LocationName,a1.DepartmentName,a1.SectionDescription,a1.CustodianName,a1.AssetDescription,
--  a1.AssetCondition,a1.suppliername,
  
--  old.LocationName as OldLocationName,
--  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubType,
--ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
--TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
--VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
--FORMATMESSAGE( dbo.fn_GetServerURL()+'TransactionApproval/EmailView?id=%d' , a.TransactionID)  ApprovalURL,
----Notification supporting fields
--		A.TransactionID SYSDataID1, NULL SYSDataID2, NULL SYSDataID3,
--		p.EMailID SYSToAddresses, '' SYSCCAddresses, '' SYSBCCAddresses,
--		NULL SYSToMobileNos, NULL SYSWhatsAppMobileNos,b.DisposalValue, b.DisposalReferencesNo,b.DisposalRemarks,b.ProceedOfSales,b.CostOfRemoval,
--		b.DisposalDate,RE.RetirementTypeName,NULL as SYSUserID
-- From TransactionTable a 
--  join TransactionLineItemTable b on a.TransactionID=b.TransactionID 
--  join AssetNewView a1 on b.AssetID=a1.AssetID 
--  --left join ApprovalHistoryTable AH on a.TransactionID=AH.TransactionID and ah.StatusID=150
--  left join LocationTable Old on b.FromLocationID=Old.LocationID
--  Left join RetirementTypeTable RE on b.RetirementTypeID=RE.RetirementTypeID
--  Left join PersonTable p on a.CreatedBy=p.PersonID and p.EMailID is not null

--  where a.StatusID = dbo.fnGetActiveStatusID() and a.TransactionTypeID=10
--GO




--   ALTER View [dbo].[nvwAssetRetirement_ForBeforeApproval]
--as 
--  select  a1.AssetCode,a1.Barcode,a1.CategoryName,a1.LocationName,a1.DepartmentName,a1.SectionDescription,a1.CustodianName,a1.AssetDescription,
--  a1.AssetCondition,a1.suppliername,
  
--  old.LocationName as OldLocationName,
--  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubType,
--ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
--TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
--VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
-- case when ah.ApproveModuleID=5 and ad.UpdateRetirementDetailsForEachAssets=1   then dbo.fn_GetServerURL() else 

-- FORMATMESSAGE( dbo.fn_GetServerURL()+'TransactionApproval/EmailEdit?id=%d&UserID=%d' , ah.ApprovalHistoryID,R.UserID) end
 
--   ApprovalURL,
----Notification supporting fields
--		A.TransactionID SYSDataID1, AH.ApprovalHistoryID SYSDataID2, NULL SYSDataID3,R.UserID as SYSUserID,
--		R.EMailID SYSToAddresses, '' SYSCCAddresses, '' SYSBCCAddresses,
--		NULL SYSToMobileNos, NULL SYSWhatsAppMobileNos,b.DisposalValue, b.DisposalReferencesNo,b.DisposalRemarks,b.ProceedOfSales,b.CostOfRemoval,
--		b.DisposalDate
		
--  From TransactionTable a 
--  join TransactionLineItemTable b on a.TransactionID=b.TransactionID 
--  join AssetNewView a1 on b.AssetID=a1.AssetID 
--  left join ApprovalHistoryTable AH on a.TransactionID=AH.TransactionID --and ah.StatusID=150
--  left join LocationTable Old on b.FromLocationID=Old.LocationID
--   Left join CategoryTypeTable CT on ah.CategoryTypeID=Ct.CategoryTypeID
-- Left join ApprovalRoleTable AD on AH.ApprovalRoleID=AD.ApprovalRoleID and ad.statusID=1
-- Left join ApprovalHistoryMappedUser D on D.ApprovalRoleID=AD.ApprovalRoleID  and 
--							case when AD.ApprovalLocationTypeID=5 then 
--					AH.FromLocationID else AH.ToLocationID end =D.LocationID and AH.TransactionID=D.TransactionID and AH.ApproveModuleID=D.ApproveModuleID
--					join ValidateAccessRightsView  R  on D.UserID=R.UserID  and RightName= 'AssetRetirementApproval' and  R.EMailID is not null
--  --Left join UserRightValueView P2 on P2.ApprovalRoleID = AH.ApprovalRoleID  and RightName = 'AssetRetirementApproval'
--  --and case when AD.ApprovalLocationTypeID=5 then 
--		--AH.FromLocationID else AH.ToLocationID end =P2.LocationID  and 
--		--case when CT.IsAllCategoryType=1 then P2.CategoryTypeID else AH.CategorytypeID end =P2.CategoryTypeID   and p2.EMailID is not null 
--  ----LEFT JOIN (SELECT ApprovalRoleID, 
--  ------STRING_AGG(EmailID, ';') EMailIDs,
--  ---- stuff((select ';'+Emailid from UserRightValueView PL where PL.ApprovalRoleID=p2.ApprovalRoleID group by EMailID  FOR XML PATH('')), 1, 1, '') as EMailIDs--,
--  ------LocationID

--		----	FROM UserRightValueView P2 where RightName = 'AssetRetirementApproval'
  
--		----		AND ApprovalRoleID IS NOT NULL
--		----		AND EmailID IS NOT NULL
--		----	GROUP BY ApprovalRoleID) UGRP ON UGRP.ApprovalRoleID = AH.ApprovalRoleID
--WHERE A.StatusID  in (150,200) and a.TransactionTypeID=10

--GO



--ALTER View [dbo].[nvwAssetRetirement_ForNotification]
--as 
--  select  a1.AssetCode,a1.Barcode,a1.CategoryName,a1.LocationName,a1.DepartmentName,a1.SectionDescription,a1.CustodianName,a1.AssetDescription,
--  a1.AssetCondition,a1.suppliername,
  
--  old.LocationName as OldLocationName,
--  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubType,
--ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
--TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
--VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
--FORMATMESSAGE( dbo.fn_GetServerURL()+'TransactionApproval/EmailEdit?id=%d&UserID=%d' , ah.ApprovalHistoryID,R.UserID) ApprovalURL,
----Notification supporting fields
--		A.TransactionID SYSDataID1, AH.ApprovalHistoryID SYSDataID2, NULL SYSDataID3,R.UserID as SYSUserID,
--		R.EMailID SYSToAddresses, '' SYSCCAddresses, '' SYSBCCAddresses,
--		NULL SYSToMobileNos, NULL SYSWhatsAppMobileNos,b.DisposalValue, b.DisposalReferencesNo,b.DisposalRemarks,b.ProceedOfSales,b.CostOfRemoval,
--		b.DisposalDate,(SELECT EMAILID FROM PERSONTABLE WHERE PERSONID=AH.CREATEDBY) as ApprovedBy,convert(nvarchar(100),ah.CreatedDateTime,103) as ApprovedDate
		
--  From TransactionTable a 
--  join TransactionLineItemTable b on a.TransactionID=b.TransactionID 
--  join AssetNewView a1 on b.AssetID=a1.AssetID 
--  left join ApprovalHistoryTable AH on a.TransactionID=AH.TransactionID --and ah.StatusID=150
--   Left join ApprovalRoleTable AD on AH.ApprovalRoleID=AD.ApprovalRoleID and ad. statusID=1
--    Left join CategoryTypeTable CT on ah.CategoryTypeID=Ct.CategoryTypeID
--  left join LocationTable Old on b.FromLocationID=Old.LocationID
--  Left join ApprovalHistoryMappedUser D on D.ApprovalRoleID=AD.ApprovalRoleID  and 
--							case when AD.ApprovalLocationTypeID=5 then 
--					AH.FromLocationID else AH.ToLocationID end =D.LocationID and AH.TransactionID=D.TransactionID and AH.ApproveModuleID=D.ApproveModuleID
--					join ValidateAccessRightsView  R  on D.UserID=R.UserID  and RightName= 'AssetRetirementApproval' and  R.EMailID is not null
--  --Left join UserRightValueView P2 on P2.ApprovalRoleID = AH.ApprovalRoleID  and RightName = 'AssetTransferApproval'
--  --and case when AD.ApprovalLocationTypeID=5 then 
--		--AH.FromLocationID else AH.ToLocationID end =P2.LocationID and 
--		--case when CT.IsAllCategoryType=1 then P2.CategoryTypeID else AH.CategorytypeID end =P2.CategoryTypeID   and p2.EMailID is not null
--  ----LEFT JOIN (SELECT ApprovalRoleID, 
--  ------STRING_AGG(EmailID, ';') EMailIDs,
--  ---- stuff((select ';'+Emailid from UserRightValueView PL where PL.ApprovalRoleID=p2.ApprovalRoleID group by EMailID  FOR XML PATH('')), 1, 1, '') as EMailIDs--,
--  ------LocationID

--		----	FROM UserRightValueView P2 where RightName = 'AssetRetirementApproval'
  
--		----		AND ApprovalRoleID IS NOT NULL
--		----		AND EmailID IS NOT NULL
--		----	GROUP BY ApprovalRoleID) UGRP ON UGRP.ApprovalRoleID = AH.ApprovalRoleID
--WHERE a.TransactionTypeID=10

--GO

--ALTER View [dbo].[TransactionLineItemViewForTransfer]
--  as 
---- select * into tmp_LocationNewHierarchicalView from LocationNewHierarchicalView

--  Select a1.*,old.PID4 as OldLocationName,New.PID4 as NewLocationName,
--  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubType,
--ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
--TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
--VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
--OldDept.DepartmentName as OldDepartmentName,NewDept.DepartmentName as NewDepartmentName,OldCate.CategoryName as OldCategoryName,
--Oldprod.ProductName as OldProductName,newprod.ProductName as NewProductName,OldSec.SectionName as oldSectionName,newsec.SectionName as NewSectionName,

--newCate.CategoryName as NewCategoryName
--  From TransactionTable a 
--  join TransactionLineItemTable b on a.TransactionID=b.TransactionID 
--  join AssetNewView a1 on b.AssetID=a1.AssetID 
--  left join tmp_LocationNewHierarchicalView Old on b.FromLocationID=Old.ChildID
--  left join tmp_LocationNewHierarchicalView new on b.ToLocationID=new.ChildID
--  Left join DepartmentTable OldDept on b.FromDepartmentID=OldDept.DepartmentID
--  left join DepartmentTable NewDept on b.ToDepartmentID=NewDept.DepartmentID
--  Left join CategoryTable OldCate on b.FromCategoryID=OldCate.CategoryID
--  left join CategoryTable newCate on b.ToCategoryID=newCate.CategoryID
--   Left join ProductTable Oldprod on b.FromProductID=Oldprod.ProductID
--  left join ProductTable newprod on b.ToProductID=newprod.ProductID
--  LEft join SectionTable OldSec on b.FromSectionID=oldsec.SectionID
--  left join SectionTable newsec on b.ToSectionID=newsec.SectionID

--  --drop table tmp_LocationNewHierarchicalView
--GO

--ALTER view [dbo].[TransactionView] 
--as 

--SElect a.TransactionID,a.TransactionNo,a.TransactionTypeID,a.TransactionSubType,
--a.ReferenceNo,a.CreatedFrom,a.SourceTransactionID,a.SourceDocumentNo,
--a.Remarks,a.TransactionDate,a.TransactionValue,a.StatusID,
--a.PostingStatusID,a.VerifiedBy,a.VerifiedDateTime,a.PostedBy,
--a.PostedDateTime,a.CreatedBy,a.CreatedDateTime
--,b.TransactionTypeName, case when c.StatusID = dbo.fnGetActiveStatusID() 
--			then 'Approved' else c.Status end as Status,p.PersonFirstName+'-'+p.PersonLastName as CreatedUSer
--From TransactionTable a 
--join TransactionTypeTable b on a.TransactionTypeID=b.TransactionTypeID 
--join  StatusTable c on a.StatusID=c.StatusID
--join PersonTable p on a.CreatedBy=p.PersonID

--GO



--   ALTER View [dbo].[nvwAssetRetirement_ForBeforeApproval]
--as 
--  select  a1.AssetCode,a1.Barcode,a1.CategoryName,a1.LocationName,a1.DepartmentName,a1.SectionDescription,a1.CustodianName,a1.AssetDescription,
--  a1.AssetCondition,a1.suppliername,
  
--  old.LocationName as OldLocationName,
--  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubType,
--ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
--TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
--VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
-- case when ah.ApproveModuleID=10 and ad.UpdateRetirementDetailsForEachAssets=1   then dbo.fn_GetServerURL() else 

-- FORMATMESSAGE( dbo.fn_GetServerURL()+'TransactionApproval/EmailEdit?id=%d&UserID=%d' , ah.ApprovalHistoryID,R.UserID) end
 
--   ApprovalURL,
----Notification supporting fields
--		A.TransactionID SYSDataID1, AH.ApprovalHistoryID SYSDataID2, NULL SYSDataID3,R.UserID as SYSUserID,
--		R.EMailID SYSToAddresses, '' SYSCCAddresses, '' SYSBCCAddresses,
--		NULL SYSToMobileNos, NULL SYSWhatsAppMobileNos,b.DisposalValue, b.DisposalReferencesNo,b.DisposalRemarks,b.ProceedOfSales,b.CostOfRemoval,
--		b.DisposalDate
		
--  From TransactionTable a 
--  join TransactionLineItemTable b on a.TransactionID=b.TransactionID 
--  join AssetNewView a1 on b.AssetID=a1.AssetID 
--  left join ApprovalHistoryTable AH on a.TransactionID=AH.TransactionID --and ah.StatusID=150
--  left join LocationTable Old on b.FromLocationID=Old.LocationID
--   Left join CategoryTypeTable CT on ah.CategoryTypeID=Ct.CategoryTypeID
-- Left join ApprovalRoleTable AD on AH.ApprovalRoleID=AD.ApprovalRoleID and ad.statusID=1
-- Left join ApprovalHistoryMappedUser D on D.ApprovalRoleID=AD.ApprovalRoleID  and 
--							case when AD.ApprovalLocationTypeID=5 then 
--					AH.FromLocationID else AH.ToLocationID end =D.LocationID and AH.TransactionID=D.TransactionID and AH.ApproveModuleID=D.ApproveModuleID
--					join ValidateAccessRightsView  R  on D.UserID=R.UserID  and RightName= 'AssetRetirementApproval' and  R.EMailID is not null
--  --Left join UserRightValueView P2 on P2.ApprovalRoleID = AH.ApprovalRoleID  and RightName = 'AssetRetirementApproval'
--  --and case when AD.ApprovalLocationTypeID=5 then 
--		--AH.FromLocationID else AH.ToLocationID end =P2.LocationID  and 
--		--case when CT.IsAllCategoryType=1 then P2.CategoryTypeID else AH.CategorytypeID end =P2.CategoryTypeID   and p2.EMailID is not null 
--  ----LEFT JOIN (SELECT ApprovalRoleID, 
--  ------STRING_AGG(EmailID, ';') EMailIDs,
--  ---- stuff((select ';'+Emailid from UserRightValueView PL where PL.ApprovalRoleID=p2.ApprovalRoleID group by EMailID  FOR XML PATH('')), 1, 1, '') as EMailIDs--,
--  ------LocationID

--		----	FROM UserRightValueView P2 where RightName = 'AssetRetirementApproval'
  
--		----		AND ApprovalRoleID IS NOT NULL
--		----		AND EmailID IS NOT NULL
--		----	GROUP BY ApprovalRoleID) UGRP ON UGRP.ApprovalRoleID = AH.ApprovalRoleID
--WHERE A.StatusID  in (150,200) and a.TransactionTypeID=10

--GO


update ApproveModuleTable set StatusID=1 where ModuleName='AssetAddition'
go 
If not exists(select EntityCodeName from EntityCodeTable where EntityCodeName='AssetAddition')
begin 
 insert into EntityCodeTable(EntityCodeName,CodePrefix,CodeSuffix,CodeFormatString,LastUsedNo,UseDateTime)
 values('AssetAddition','AA' ,NULL,'{0:00000}',0,0)
end 
go 
--create Procedure Prc_ValidateAssetAddition 
--(
--  @LocationID   int =NULL,
--  @CategoryID   int = null
--)
--as 
--Begin
--   Declare @ErrorMsg nvarchar(max),@L2LocID int, @LocTypeID int,@statusID int,@ApproveworkflowID int,@categoryTypeID int,@L2catID int 
--   ,@workflowCnt int ,@approvalCnt int
--   Select @statusID=[dbo].fnGetActiveStatusID()

--   select @L2LocID=Level2ID from tmp_LocationNewHierarchicalView where ChildID=@LocationID
--   select @LocTypeID=LocationTypeID from LocationTable where LocationID=@L2LocID
--   select @L2catID=Level2ID from tmp_CategoryNewHierarchicalView where ChildID=@CategoryID
--   select @categoryTypeID=CategoryTypeID from CategoryTable where CategoryID=@L2catID

   
--   if not exists( select b.ApprovalRoleID
--		   from ApproveWorkflowTable a 
--		   join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
--			 where a.StatusID=@statusID and b.StatusID=@statusID and 
--				a.ApproveModuleID=(select ApproveModuleID from ApproveModuleTable where ModuleName='AssetAddition' and StatusID=@statusID)
--				and a.FromLocationTypeID=@LocTypeID)
--	begin 
--		 if @ErrorMsg is not null or @ErrorMsg!=''
--			  Begin
--			   set @ErrorMsg =@ErrorMsg+ ',Selected Location Type not created workflow . please create the workflow'
--			   end 
--			   else 
--			   begin 
--			   set @ErrorMsg = ' Selected Location Type not created workflow . please create the workflow'
--			   end 
--	end 
--	Else 
--	Begin 
--	   select @ApproveworkflowID=b.ApproveWorkFlowID from ApproveWorkflowTable a 
--		   join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
--			 where a.StatusID=@statusID and b.StatusID=@statusID and 
--				a.ApproveModuleID=(select ApproveModuleID from ApproveModuleTable where ModuleName='AssetAddition' and StatusID=@statusID)
--				and a.FromLocationTypeID=@LocTypeID

--          declare @TransactionLineItemTable table (FromLocationL2 int ,LocationTypeID int,CategoryTypeID int,ApprovalWorkFlowID int )
  
--		  insert into @TransactionLineItemTable(FromLocationL2,LocationTypeID,CategoryTypeID,ApprovalWorkFlowID)
--		  values(@L2LocID,@LocTypeID,@categoryTypeID,@ApproveworkflowID)

--		  Declare @validationTable Table(UserID int,LocationID int,ApprovalRoleID int,CategoryTypeCnt int ) 

--		   Select @workflowCnt=Count(*) from ApproveWorkflowLineItemTable where ApproveWorkFlowID=@ApproveworkflowID and StatusID=@statusID

--		   insert into @validationTable(UserID,LocationID,ApprovalRoleID,CategoryTypeCnt)
--			select userID,LocationID,a.ApprovalRoleID,count(d.categoryTypeID) as categorytypeCnt 
--			   from ApproveWorkflowLineItemTable a 
--			   join @TransactionLineItemTable T on a.ApproveWorkFlowID=T.ApprovalWorkFlowID
--			   join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
--			   join CategoryTypeTable CT on T.CategoryTypeID=Ct.CategoryTypeID
--			   join 
--			   (
--			   select a1.*,b1.IsAllCategoryType from UserApprovalRoleMappingTable a1  
--						join CategoryTypeTable b1 on a1.CategoryTypeID=b1.CategoryTypeID
--						where a1.StatusID=@statusID and b1.StatusID=@statusID
--						) D on  T.FromLocationL2 =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID 
--				and ct.CategoryTypeID  
--				in ( case when CT.IsAllCategoryType=1 and D.IsAllCategoryType=1 then D.CategoryTypeID 
--				when  CT.IsAllCategoryType=0 and D.IsAllCategoryType=0 then  D.CategoryTypeID
--				when CT.IsAllCategoryType=0 and D.IsAllCategoryType=1 then ( select categoryTypeID from CategoryTypeTable where StatusID=@statusID and IsAllCategoryType=0 and CategoryTypeID=ct.CategoryTypeID)
--				when CT.IsAllCategoryType=1 and D.IsAllCategoryType=0 then (D.CategoryTypeID) end)
--				where ApproveWorkFlowID=@ApproveworkflowID and CT.statusID=@statusID AND d.StatusID=@statusID and a.statusID=@statusID
--				  group by userID,LocationID,a.ApprovalRoleID

--			   select @approvalCnt= count(*) from (
--			  select  a.ApprovalRoleID  from @validationTable a group by a.ApprovalRoleID) x
--			  if(isnull((@workflowCnt),0)=isnull(@approvalCnt,0))
--			  begin 
--				    Declare @validationTable1 Table(UserID int,LocationID int,ApprovalRoleID int,CategoryTypeCnt int ) 
--					Declare @validationTable2 Table(UserID int,LocationID int,ApprovalRoleID int,CategoryTypeCnt int ) 
					
--					insert into @validationTable1(UserID,LocationID,ApprovalRoleID,CategoryTypeCnt)
--					select a.userID,a.LocationID,a.ApprovalRoleID,categorytypeCnt
--					From @validationTable a  
--					join [ValidateAccessRightsView]  b  on a.UserID=b.UserID --and a.LocationID=b.LocationID and a.ApprovalRoleID=b.ApprovalRoleID
--					and b.RightName='AssetApproval'

--					if(select count(*) from (
--						select  a.ApprovalRoleID  from @validationTable1 a group by a.ApprovalRoleID)x )!=@approvalCnt
--					  Begin 
--						  if @ErrorMsg is not null or @ErrorMsg!=''
--							 Begin
--							 set @ErrorMsg =@ErrorMsg+', Mapped User Role not set authorized Page'
--							 End 
--							 else 
--							 begin 
--							 set @ErrorMsg ='Mapped User Role not set authorized Page'
--							 end 
--					 end 
--					 if (select count(*) from (
--							select  a.ApprovalRoleID  from @validationTable1 a group by a.ApprovalRoleID)x  )=@approvalCnt
--					 begin 
--				         insert into @validationTable2(UserID,LocationID,ApprovalRoleID,CategoryTypeCnt)
--						 select a.userID,a.LocationID,a.ApprovalRoleID,categorytypeCnt
--							From @validationTable a  
--							join persontable p on a.userID=p.personID and p.EMailID is not null and p.StatusID=@statusID
						  
--							  if(select count(*) from (
--								select  a.ApprovalRoleID  from @validationTable2 a group by a.ApprovalRoleID)x  )!=@approvalCnt
--							  Begin 
--								   if @ErrorMsg is not null or @ErrorMsg!=''
--									Begin
--									 set @ErrorMsg =@ErrorMsg+', EmailID not assigned to the mapped workflow user'
--									 End 
--									 else 
--									 Begin 
--									 set @ErrorMsg =' EmailID not assigned to the mapped workflow user'
--									 end 
--							  end 
--					End 

--			  end 
--			  else 
--			  begin 
--			   if @ErrorMsg is not null or @ErrorMsg!=''
--			  Begin
--			   set @ErrorMsg =@ErrorMsg+ ',Category type of the selected assets are not matched with mapped role'
--			   end 
--			   else 
--			   begin 
--			   set @ErrorMsg = 'Category type of the selected assets are not matched with mapped role'
--			   end 
			  
--			  end 
		
--		End 
--		Select @ErrorMsg as ErrorMsg

--end 
--go 
--Create Procedure prc_GetAssetDetails
--(
-- @LocationID int = null,
--  @CategoryID int = null
--) 
--as 
--begin 
--   Declare @L2LocID int, @LocTypeID int,@categoryTypeID int,@L2catID int 
--    select @L2LocID=Level2ID from tmp_LocationNewHierarchicalView where ChildID=@LocationID
--   select @LocTypeID=LocationTypeID from LocationTable where LocationID=@L2LocID
--   select @L2catID=Level2ID from tmp_CategoryNewHierarchicalView where ChildID=@CategoryID
--   select @categoryTypeID=CategoryTypeID from CategoryTable where CategoryID=@L2catID
    
--   select @LocTypeID as LocationTypeID,@L2LocID as L2LocationID,@categoryTypeID as CategoryTypeID
--End 
--go 
--Create Procedure [dbo].[Prc_ValidateBulkAssetAddition] 
--(
--  @LocationCode   nvarchar(max) =NULL,
--  @CategoryCode   nvarchar(max) = null
--)
--as 
--Begin 
--    Declare @ErrorMsg nvarchar(max),@L2LocID int, @LocTypeID int,@statusID int,@ApproveworkflowID int,@categoryTypeID int,@L2catID int 
--   ,@workflowCnt int ,@approvalCnt int,@CategoryCount int,@LocationCount int ,@CategoryTypeCnt int 
--   Select @statusID=[dbo].fnGetActiveStatusID()
   
--   Declare @LocationIDTable Table(LocationID int )
--   Declare @CategoryIDTable Table(CategoryID int )
--   Declare @CategoryTypeTable Table (CategoryTypeID int)

--   select @CategoryCount=count(*) from (
--   select value from Split(@CategoryCode,',')
--   group by value
--   ) x 
--   select @LocationCount=count(*) from (
--   select value from Split(@LocationCode,',')
--   group by value
--   ) x 

--   Insert into @CategoryIDTable(CategoryID)
--   select value from Split(@CategoryCode,',')
--   group by value
   
--   Insert into @LocationIDTable(LocationID)
--   select value from Split(@LocationCode,',')
--   group by value

--   if(select count(*) from (
--				select  CategoryID  from @CategoryIDTable a group by a.CategoryID)x) !=@CategoryCount
--	Begin
--	    set @ErrorMsg ='Some new Category avaliable in given details,Cant allow to create data'
--	End 
--	if(select count(*) from (
--				select  LocationID  from @LocationIDTable a group by a.LocationID)x) !=@LocationCount
--	Begin
--		 if @ErrorMsg is not null or @ErrorMsg!=''
--			 Begin
--			    set @ErrorMsg =@ErrorMsg+', Some new Location avaliable in given details,Cant allow to create data'
--			 End 
--			 else 
--			 begin 
--				set @ErrorMsg ='Some new Location avaliable in given details,Cant allow to create data'
--			end 
--	End 

--	Insert into @CategoryTypeTable(CategoryTypeID)
--		Select CategoryTypeID from CategoryTable where CategoryID 
--				in (select Level2ID  from tmp_CategoryNewHierarchicalView where ChildID in (select CategoryID from @CategoryIDTable))
		
--		if(select count(*) from (
--				select  CategoryTypeID  from @CategoryTypeTable a group by a.CategoryTypeID)x) !=@CategoryCount
--	Begin
--		 if @ErrorMsg is not null or @ErrorMsg!=''
--			 Begin
--			    set @ErrorMsg =@ErrorMsg+', Category Type is not assigned for the selected asset category'
--			 End 
--			 else 
--			 begin 
--				set @ErrorMsg ='Category Type is not assigned for the selected asset category'
--			end 
--	End 
   
--	if(select count(Level2ID) from tmp_LocationNewHierarchicalView where ChildID in (select LocationID from @LocationIDTable))>1
--	Begin 
--	if @ErrorMsg is not null or @ErrorMsg!=''
--			 Begin
--			    set @ErrorMsg =@ErrorMsg+',Location Mismatch: Same Second level Location  only allowed here'
--			 End 
--			 else 
--			 begin 
--				set @ErrorMsg ='Location Mismatch: Same Second level Location  only allowed here'
--			end 
--	End 
--	Else 
--	Begin 
--	   select  @L2LocID=Level2ID from tmp_LocationNewHierarchicalView where ChildID in (select LocationID from @LocationIDTable)
--	   select @LocTypeID=LocationTypeID from LocationTable where LocationID=@L2LocID
--	   Select @CategoryTypeCnt=count(CategoryTypeID) from @CategoryTypeTable group by CategoryTypeID
--	   if(@CategoryTypeCnt>1)
--			  begin
--				Select @categoryTypeID=CategoryTypeID from CategoryTypeTable where IsAllCategoryType=1 and statusID=@statusID
				
--			  End 
--			  Else 
--			  Begin 
--			  Select @categoryTypeID=CategoryTypeID from @CategoryTypeTable group by CategoryTypeID
--			  end 
--	   end 
--	     if not exists( select b.ApprovalRoleID
--		   from ApproveWorkflowTable a 
--		   join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
--			 where a.StatusID=@statusID and b.StatusID=@statusID and 
--				a.ApproveModuleID=(select ApproveModuleID from ApproveModuleTable where ModuleName='AssetAddition' and StatusID=@statusID)
--				and a.FromLocationTypeID=@LocTypeID)
--	begin 
--		 if @ErrorMsg is not null or @ErrorMsg!=''
--			  Begin
--			   set @ErrorMsg =@ErrorMsg+ ',Selected Location Type not created workflow . please create the workflow'
--			   end 
--			   else 
--			   begin 
--			   set @ErrorMsg = ' Selected Location Type not created workflow . please create the workflow'
--			   end 
--	end 
--	Else 
--	Begin 
--	   select @ApproveworkflowID=b.ApproveWorkFlowID from ApproveWorkflowTable a 
--		   join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
--			 where a.StatusID=@statusID and b.StatusID=@statusID and 
--				a.ApproveModuleID=(select ApproveModuleID from ApproveModuleTable where ModuleName='AssetAddition' and StatusID=@statusID)
--				and a.FromLocationTypeID=@LocTypeID

--          declare @TransactionLineItemTable table (FromLocationL2 int ,LocationTypeID int,CategoryTypeID int,ApprovalWorkFlowID int )
  
--		  insert into @TransactionLineItemTable(FromLocationL2,LocationTypeID,CategoryTypeID,ApprovalWorkFlowID)
--		  values(@L2LocID,@LocTypeID,@categoryTypeID,@ApproveworkflowID)
--		 Declare @validationTable Table(UserID int,LocationID int,ApprovalRoleID int,CategoryTypeCnt int ) 

--		   Select @workflowCnt=Count(*) from ApproveWorkflowLineItemTable where ApproveWorkFlowID=@ApproveworkflowID and StatusID=@statusID

--		   insert into @validationTable(UserID,LocationID,ApprovalRoleID,CategoryTypeCnt)
--			select userID,LocationID,a.ApprovalRoleID,count(d.categoryTypeID) as categorytypeCnt 
--			   from ApproveWorkflowLineItemTable a 
--			   join @TransactionLineItemTable T on a.ApproveWorkFlowID=T.ApprovalWorkFlowID
--			   join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
--			   join CategoryTypeTable CT on T.CategoryTypeID=Ct.CategoryTypeID
--			   join 
--			   (
--			   select a1.*,b1.IsAllCategoryType from UserApprovalRoleMappingTable a1  
--						join CategoryTypeTable b1 on a1.CategoryTypeID=b1.CategoryTypeID
--						where a1.StatusID=@statusID and b1.StatusID=@statusID
--						) D on  T.FromLocationL2 =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID 
--				and ct.CategoryTypeID  
--				in ( case when CT.IsAllCategoryType=1 and D.IsAllCategoryType=1 then D.CategoryTypeID 
--				when  CT.IsAllCategoryType=0 and D.IsAllCategoryType=0 then  D.CategoryTypeID
--				when CT.IsAllCategoryType=0 and D.IsAllCategoryType=1 then ( select categoryTypeID from CategoryTypeTable where StatusID=@statusID and IsAllCategoryType=0 and CategoryTypeID=ct.CategoryTypeID)
--				when CT.IsAllCategoryType=1 and D.IsAllCategoryType=0 then (D.CategoryTypeID) end)
--				where ApproveWorkFlowID=@ApproveworkflowID and CT.statusID=@statusID AND d.StatusID=@statusID and a.statusID=@statusID
--				  group by userID,LocationID,a.ApprovalRoleID

--			   select @approvalCnt= count(*) from (
--			  select  a.ApprovalRoleID  from @validationTable a group by a.ApprovalRoleID) x
--			  if(isnull((@workflowCnt),0)=isnull(@approvalCnt,0))
--			  begin 
--				    Declare @validationTable1 Table(UserID int,LocationID int,ApprovalRoleID int,CategoryTypeCnt int ) 
--					Declare @validationTable2 Table(UserID int,LocationID int,ApprovalRoleID int,CategoryTypeCnt int ) 
					
--					insert into @validationTable1(UserID,LocationID,ApprovalRoleID,CategoryTypeCnt)
--					select a.userID,a.LocationID,a.ApprovalRoleID,categorytypeCnt
--					From @validationTable a  
--					join [ValidateAccessRightsView]  b  on a.UserID=b.UserID --and a.LocationID=b.LocationID and a.ApprovalRoleID=b.ApprovalRoleID
--					and b.RightName='AssetApproval'

--					if(select count(*) from (
--						select  a.ApprovalRoleID  from @validationTable1 a group by a.ApprovalRoleID)x )!=@approvalCnt
--					  Begin 
--						  if @ErrorMsg is not null or @ErrorMsg!=''
--							 Begin
--							 set @ErrorMsg =@ErrorMsg+', Mapped User Role not set authorized Page'
--							 End 
--							 else 
--							 begin 
--							 set @ErrorMsg ='Mapped User Role not set authorized Page'
--							 end 
--					 end 
--					 if (select count(*) from (
--							select  a.ApprovalRoleID  from @validationTable1 a group by a.ApprovalRoleID)x  )=@approvalCnt
--					 begin 
--				         insert into @validationTable2(UserID,LocationID,ApprovalRoleID,CategoryTypeCnt)
--						 select a.userID,a.LocationID,a.ApprovalRoleID,categorytypeCnt
--							From @validationTable a  
--							join persontable p on a.userID=p.personID and p.EMailID is not null and p.StatusID=@statusID
						  
--							  if(select count(*) from (
--								select  a.ApprovalRoleID  from @validationTable2 a group by a.ApprovalRoleID)x  )!=@approvalCnt
--							  Begin 
--								   if @ErrorMsg is not null or @ErrorMsg!=''
--									Begin
--									 set @ErrorMsg =@ErrorMsg+', EmailID not assigned to the mapped workflow user'
--									 End 
--									 else 
--									 Begin 
--									 set @ErrorMsg =' EmailID not assigned to the mapped workflow user'
--									 end 
--							  end 
--					End 

--			  end 
--			  else 
--			  begin 
--			   if @ErrorMsg is not null or @ErrorMsg!=''
--			  Begin
--			   set @ErrorMsg =@ErrorMsg+ ',Category type of the selected assets are not matched with mapped role'
--			   end 
--			   else 
--			   begin 
--			   set @ErrorMsg = 'Category type of the selected assets are not matched with mapped role'
--			   end 
			  
--			  end 
		
--		End 
--		Select @ErrorMsg as ErrorMsg

--End 
--go 
if not exists(select menuname from User_MenuTable where MenuName='AssetApproval' and ParentTransactionID=1)
Begin
 insert into User_MenuTable(MenuName,RightID,TargetObject,parentmenuid,orderno,image,ParentTransactionID)
 select 'AssetApproval',RightID,'/TransactionApproval/Index?pageName=AssetApproval',5,9,'css/images/MenuIcon/AssetApproval.png',1
 from User_RightTable where RightName='AssetApproval'
end 
go 
if not exists(select TransactiontypeName from TransactionTypeTable where TransactiontypeName='AssetAddition')
Begin 
insert into TransactionTypeTable(TransactionTypeID,TransactionTypeName,IsSourceTransactionType,TransactionTypeDesc)
values(4,'AssetAddition',0,'AssetAddition')
end 

update user_righttable set  ValueType=95 where RightName='AssetApproval'
	go 

	
--ALTER VIEW [dbo].[TransactionApprovalView] 
-- as 
-- Select ApprovalHistoryID,ApproveWorkFlowID,ApproveWorkFlowLineItemID,a.ApproveModuleID,a.ApprovalRoleID,a.TransactionID as ApprovalTransactionID,
--		OrderNo,a.Remarks as ApprovalRemarks,FromLocationID,ToLocationID,FromLocationTypeID,ToLocationTypeID,a.StatusID as ApprovalStatusID,
--        a.CreatedBy as ApprovalCreatedBy,a.CreatedDateTime as ApprovalCreatedDateTime,a.LastModifiedBy,a.LastModifiedDateTime,ObjectKeyID1,EmailsecrectKey ,
--		b.TransactionID,TransactionNo,TransactionTypeID,TransactionSubType,ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,b.Remarks,TransactionDate,
--		TransactionValue,b.StatusID,PostingStatusID,VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,b.CreatedBy,b.CreatedDateTime, 
--		Dense_rank() over( PARTITION BY a.transactionid , a.approvemoduleID ORDER BY Orderno asc ) AS SerialNo ,
--		p.PersonFirstName+'-'+p.PersonLastName as CreatedUSer,s.Status as ApprovalStatus,
--		case when a.ApproveModuleID=10 then  isnull(c.UpdateRetirementDetailsForEachAssets,0)
--		     when a.ApproveModuleID=5 or  a.ApproveModuleID=11 then isnull(c.UpdateDestinationlocationsForTransfer,0) else 0 end as enableUpdate,
--			 isnull(DD.DelegatedEmployeeID,D.UserID) as UserID
--	From ApprovalHistoryTable a join TransactionTable b on a.TransactionID=b.TransactionID 
--	join ApprovalRoleTable c on a.ApprovalRoleID=c.ApprovalRoleID
--	join PersonTable p on b.CreatedBy=p.PersonID
--	join StatusTable s on a.StatusID=s.StatusID
--	join CategoryTypeTable CT on a.CategoryTypeID=CT.CategoryTypeID
--	Left join ApprovalHistoryMappedUser D on D.ApprovalRoleID=c.ApprovalRoleID  and 
--	case when c.ApprovalLocationTypeID=5 then 
--	    a.FromLocationID else a.ToLocationID end =D.LocationID and a.TransactionID=D.TransactionID and a.ApproveModuleID=D.ApproveModuleID
--	Left join (select * from DelegateRoleTable where  getdate() between EffectiveStartDate and EffectiveEndDate) DD on DD.EmployeeID=D.UserID
--	where a.StatusID=150
--GO

--ALTER Procedure [dbo].[Prc_ValidateBulkAssetAddition] 
--(
--  @LocationCode   nvarchar(max) =NULL,
--  @CategoryCode   nvarchar(max) = null
--)
--as 
--Begin 
--    Declare @ErrorMsg nvarchar(max),@L2LocID int, @LocTypeID int,@statusID int,@ApproveworkflowID int,@categoryTypeID int,@L2catID int 
--   ,@workflowCnt int ,@approvalCnt int,@CategoryCount int,@LocationCount int ,@CategoryTypeCnt int 
--   Select @statusID=[dbo].fnGetActiveStatusID()
   
--   Declare @LocationIDTable Table(LocationID int )
--   Declare @CategoryIDTable Table(CategoryID int )
--   Declare @CategoryTypeTable Table (CategoryTypeID int)

--   select @CategoryCount=count(*) from (
--   select value from Split(@CategoryCode,',')
--   group by value
--   ) x 
--   select @LocationCount=count(*) from (
--   select value from Split(@LocationCode,',')
--   group by value
--   ) x 

--   Insert into @CategoryIDTable(CategoryID)
--   select CategoryID from categoryTable where categorycode in (select value from Split(@CategoryCode,',')) and statusid=1
--   group by CategoryID
   
--   Insert into @LocationIDTable(LocationID)
--    select LocationID from LocationTable where LocationCode in (select value from Split(@LocationCode,',')) and statusid=1
--   group by LocationID
--   --select * from @LocationIDTable 
--   if(select count(*) from (
--				select  CategoryID  from @CategoryIDTable a group by a.CategoryID)x) !=@CategoryCount
--	Begin
--	    set @ErrorMsg ='Some new Category avaliable in given details,Cant allow to create data'
--	End 
--	if(select count(*) from (
--				select  LocationID  from @LocationIDTable a group by a.LocationID)x) !=@LocationCount
--	Begin
--		 if @ErrorMsg is not null or @ErrorMsg!=''
--			 Begin
--			    set @ErrorMsg =@ErrorMsg+', Some new Location avaliable in given details,Cant allow to create data'
--			 End 
--			 else 
--			 begin 
--				set @ErrorMsg ='Some new Location avaliable in given details,Cant allow to create data'
--			end 
--	End 

--	Insert into @CategoryTypeTable(CategoryTypeID)
--		Select CategoryTypeID from CategoryTable where CategoryID 
--				in (select Level2ID  from tmp_CategoryNewHierarchicalView where ChildID in (select CategoryID from @CategoryIDTable))
		
--		if(select count(*) from (
--				select  CategoryTypeID  from @CategoryTypeTable a group by a.CategoryTypeID)x) !=@CategoryCount
--	Begin
--		 if @ErrorMsg is not null or @ErrorMsg!=''
--			 Begin
--			    set @ErrorMsg =@ErrorMsg+', Category Type is not assigned for the selected asset category'
--			 End 
--			 else 
--			 begin 
--				set @ErrorMsg ='Category Type is not assigned for the selected asset category'
--			end 
--	End 
   
--	if(select count(Level2ID) from tmp_LocationNewHierarchicalView where ChildID in (select LocationID from @LocationIDTable))>1
--	Begin 
--	if @ErrorMsg is not null or @ErrorMsg!=''
--			 Begin
--			    set @ErrorMsg =@ErrorMsg+',Location Mismatch: Same Second level Location  only allowed here'
--			 End 
--			 else 
--			 begin 
--				set @ErrorMsg ='Location Mismatch: Same Second level Location  only allowed here'
--			end 
--	End 
--	Else 
--	Begin 
--	   select  @L2LocID=Level2ID from tmp_LocationNewHierarchicalView where ChildID in (select LocationID from @LocationIDTable)
--	   select @LocTypeID=LocationTypeID from LocationTable where LocationID=@L2LocID
--	   Select @CategoryTypeCnt=count(CategoryTypeID) from @CategoryTypeTable group by CategoryTypeID
--	   if(@CategoryTypeCnt>1)
--			  begin
--				Select @categoryTypeID=CategoryTypeID from CategoryTypeTable where IsAllCategoryType=1 and statusID=@statusID
				
--			  End 
--			  Else 
--			  Begin 
--			  Select @categoryTypeID=CategoryTypeID from @CategoryTypeTable group by CategoryTypeID
--			  end 
--	   end 
--	     if not exists( select b.ApprovalRoleID
--		   from ApproveWorkflowTable a 
--		   join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
--			 where a.StatusID=@statusID and b.StatusID=@statusID and 
--				a.ApproveModuleID=(select ApproveModuleID from ApproveModuleTable where ModuleName='AssetAddition' and StatusID=@statusID)
--				and a.FromLocationTypeID=@LocTypeID)
--	begin 
--		 if @ErrorMsg is not null or @ErrorMsg!=''
--			  Begin
--			   set @ErrorMsg =@ErrorMsg+ ',Selected Location Type not created workflow . please create the workflow'
--			   end 
--			   else 
--			   begin 
--			   set @ErrorMsg = ' Selected Location Type not created workflow . please create the workflow'
--			   end 
--	end 
--	Else 
--	Begin 
--	   select @ApproveworkflowID=b.ApproveWorkFlowID from ApproveWorkflowTable a 
--		   join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
--			 where a.StatusID=@statusID and b.StatusID=@statusID and 
--				a.ApproveModuleID=(select ApproveModuleID from ApproveModuleTable where ModuleName='AssetAddition' and StatusID=@statusID)
--				and a.FromLocationTypeID=@LocTypeID

--          declare @TransactionLineItemTable table (FromLocationL2 int ,LocationTypeID int,CategoryTypeID int,ApprovalWorkFlowID int )
  
--		  insert into @TransactionLineItemTable(FromLocationL2,LocationTypeID,CategoryTypeID,ApprovalWorkFlowID)
--		  values(@L2LocID,@LocTypeID,@categoryTypeID,@ApproveworkflowID)
--		 Declare @validationTable Table(UserID int,LocationID int,ApprovalRoleID int,CategoryTypeCnt int ) 

--		   Select @workflowCnt=Count(*) from ApproveWorkflowLineItemTable where ApproveWorkFlowID=@ApproveworkflowID and StatusID=@statusID

--		   insert into @validationTable(UserID,LocationID,ApprovalRoleID,CategoryTypeCnt)
--			select userID,LocationID,a.ApprovalRoleID,count(d.categoryTypeID) as categorytypeCnt 
--			   from ApproveWorkflowLineItemTable a 
--			   join @TransactionLineItemTable T on a.ApproveWorkFlowID=T.ApprovalWorkFlowID
--			   join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
--			   join CategoryTypeTable CT on T.CategoryTypeID=Ct.CategoryTypeID
--			   join 
--			   (
--			   select a1.*,b1.IsAllCategoryType from UserApprovalRoleMappingTable a1  
--						join CategoryTypeTable b1 on a1.CategoryTypeID=b1.CategoryTypeID
--						where a1.StatusID=@statusID and b1.StatusID=@statusID
--						) D on  T.FromLocationL2 =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID 
--				and ct.CategoryTypeID  
--				in ( case when CT.IsAllCategoryType=1 and D.IsAllCategoryType=1 then D.CategoryTypeID 
--				when  CT.IsAllCategoryType=0 and D.IsAllCategoryType=0 then  D.CategoryTypeID
--				when CT.IsAllCategoryType=0 and D.IsAllCategoryType=1 then ( select categoryTypeID from CategoryTypeTable where StatusID=@statusID and IsAllCategoryType=0 and CategoryTypeID=ct.CategoryTypeID)
--				when CT.IsAllCategoryType=1 and D.IsAllCategoryType=0 then (D.CategoryTypeID) end)
--				where ApproveWorkFlowID=@ApproveworkflowID and CT.statusID=@statusID AND d.StatusID=@statusID and a.statusID=@statusID
--				  group by userID,LocationID,a.ApprovalRoleID

--			   select @approvalCnt= count(*) from (
--			  select  a.ApprovalRoleID  from @validationTable a group by a.ApprovalRoleID) x
--			  if(isnull((@workflowCnt),0)=isnull(@approvalCnt,0))
--			  begin 
--				    Declare @validationTable1 Table(UserID int,LocationID int,ApprovalRoleID int,CategoryTypeCnt int ) 
--					Declare @validationTable2 Table(UserID int,LocationID int,ApprovalRoleID int,CategoryTypeCnt int ) 
					
--					insert into @validationTable1(UserID,LocationID,ApprovalRoleID,CategoryTypeCnt)
--					select a.userID,a.LocationID,a.ApprovalRoleID,categorytypeCnt
--					From @validationTable a  
--					join [ValidateAccessRightsView]  b  on a.UserID=b.UserID --and a.LocationID=b.LocationID and a.ApprovalRoleID=b.ApprovalRoleID
--					and b.RightName='AssetApproval'

--					if(select count(*) from (
--						select  a.ApprovalRoleID  from @validationTable1 a group by a.ApprovalRoleID)x )!=@approvalCnt
--					  Begin 
--						  if @ErrorMsg is not null or @ErrorMsg!=''
--							 Begin
--							 set @ErrorMsg =@ErrorMsg+', Mapped User Role not set authorized Page'
--							 End 
--							 else 
--							 begin 
--							 set @ErrorMsg ='Mapped User Role not set authorized Page'
--							 end 
--					 end 
--					 if (select count(*) from (
--							select  a.ApprovalRoleID  from @validationTable1 a group by a.ApprovalRoleID)x  )=@approvalCnt
--					 begin 
--				         insert into @validationTable2(UserID,LocationID,ApprovalRoleID,CategoryTypeCnt)
--						 select a.userID,a.LocationID,a.ApprovalRoleID,categorytypeCnt
--							From @validationTable a  
--							join persontable p on a.userID=p.personID and p.EMailID is not null and p.StatusID=@statusID
						  
--							  if(select count(*) from (
--								select  a.ApprovalRoleID  from @validationTable2 a group by a.ApprovalRoleID)x  )!=@approvalCnt
--							  Begin 
--								   if @ErrorMsg is not null or @ErrorMsg!=''
--									Begin
--									 set @ErrorMsg =@ErrorMsg+', EmailID not assigned to the mapped workflow user'
--									 End 
--									 else 
--									 Begin 
--									 set @ErrorMsg =' EmailID not assigned to the mapped workflow user'
--									 end 
--							  end 
--					End 

--			  end 
--			  else 
--			  begin 
--			   if @ErrorMsg is not null or @ErrorMsg!=''
--			  Begin
--			   set @ErrorMsg =@ErrorMsg+ ',Category type of the selected assets are not matched with mapped role'
--			   end 
--			   else 
--			   begin 
--			   set @ErrorMsg = 'Category type of the selected assets are not matched with mapped role'
--			   end 
			  
--			  end 
		
--		End 
--		Select @ErrorMsg as ErrorMsg

--End 
--go 

ALTER VIEW [dbo].[LocationNewHierarchicalView]
AS
WITH Tree_CTE(LocationID, LocationLevel, LocationIDHierarchy, L1, L2, L3, L4, L5, L6, StatusID, LocationCodeHierarchy, CompanyID,LocationNameHierarchy,
		Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6, Attribute7, Attribute8, Attribute9, Attribute10,
		Attribute11,Attribute12, Attribute13, Attribute14, Attribute15, Attribute16)
AS
(
    SELECT LocationID, 1 LocationLevel, CAST(LocationID as nvarchar(max)) LocationIDHierarchy,
		CAST(LocationID AS INT) L1, CAST(NULL AS INT) L2, CAST(NULL AS INT) L3, CAST(NULL AS INT) L4, CAST(NULL AS INT) L5, CAST(NULL AS INT) L6,
		StatusID, CAST(LocationCode as nvarchar(MAX)) LocationCode, CompanyID,CAST(LocationName as nvarchar(MAX)) LocationName, 
		Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6, Attribute7, Attribute8, Attribute9, Attribute10,
		Attribute11,Attribute12, Attribute13, Attribute14, Attribute15, Attribute16
		FROM LocationTable WHERE ParentLocationID IS NULL
    UNION ALL
    SELECT ChildNode.LocationID, LocationLevel+1, LocationIDHierarchy + '/' + CAST(ChildNode.LocationID as nvarchar(max)),
		Tree_CTE.L1,
		CASE WHEN LocationLevel > 1 THEN Tree_CTE.L2 ELSE CASE WHEN LocationLevel = 1 THEN ChildNode.LocationID ELSE NULL END END,
		CASE WHEN LocationLevel > 2 THEN Tree_CTE.L3 ELSE CASE WHEN LocationLevel = 2 THEN ChildNode.LocationID ELSE NULL END END,
		CASE WHEN LocationLevel > 3 THEN Tree_CTE.L4 ELSE CASE WHEN LocationLevel = 3 THEN ChildNode.LocationID ELSE NULL END END,
		CASE WHEN LocationLevel > 4 THEN Tree_CTE.L5 ELSE CASE WHEN LocationLevel = 4 THEN ChildNode.LocationID ELSE NULL END END,
		CASE WHEN LocationLevel > 5 THEN Tree_CTE.L6 ELSE CASE WHEN LocationLevel = 5 THEN ChildNode.LocationID ELSE NULL END END,
		ChildNode.StatusID, LocationCodeHierarchy + '/' + ChildNode.LocationCode,ChildNode.CompanyID,LocationNameHierarchy + '/' + ChildNode.LocationName,
		ISNULL(ChildNode.Attribute1, Tree_CTE.Attribute1) Attribute1,
		ISNULL(ChildNode.Attribute2, Tree_CTE.Attribute2) Attribute3,
		ISNULL(ChildNode.Attribute3, Tree_CTE.Attribute3) Attribute3,
		ISNULL(ChildNode.Attribute4, Tree_CTE.Attribute4) Attribute4,
		ISNULL(ChildNode.Attribute5, Tree_CTE.Attribute5) Attribute5,
		ISNULL(ChildNode.Attribute6, Tree_CTE.Attribute6) Attribute6,
		ISNULL(ChildNode.Attribute7, Tree_CTE.Attribute7) Attribute7,
		ISNULL(ChildNode.Attribute8, Tree_CTE.Attribute8) Attribute8,
		ISNULL(ChildNode.Attribute9, Tree_CTE.Attribute9) Attribute9,
		ISNULL(ChildNode.Attribute10, Tree_CTE.Attribute10) Attribute10,
		ISNULL(ChildNode.Attribute11, Tree_CTE.Attribute11) Attribute11,
		ISNULL(ChildNode.Attribute12, Tree_CTE.Attribute12) Attribute12,
		ISNULL(ChildNode.Attribute13, Tree_CTE.Attribute13) Attribute13,
		ISNULL(ChildNode.Attribute14, Tree_CTE.Attribute14) Attribute14,
		ISNULL(ChildNode.Attribute15, Tree_CTE.Attribute15) Attribute15,
		ISNULL(ChildNode.Attribute16, Tree_CTE.Attribute16) Attribute16
		FROM LocationTable AS ChildNode
    INNER JOIN Tree_CTE
    ON ChildNode.ParentLocationID = Tree_CTE.LocationID
)
SELECT A.LocationID ChildID, 
			a.LocationNameHierarchy PID4,
			A.LocationID ParentLocationID,CONVERT(nvarchar,A.L1) FirstLevel, CONVERT(nvarchar,A.L2) SecondLevel,CONVERT(nvarchar, A.L3) ThirdLevel, A.LocationLevel [Level],
			LD.LocationName LocationName,  A.LocationCodeHierarchy ChildCode, A.StatusID,
			A.LocationIDHierarchy PID2, A.LocationIDHierarchy AllLevelIDs,
			A.L1 Level1ID, A.L2 Level2ID, A.L3 Level3ID, A.L4 Level4ID, A.L5 Level5ID, A.L6 Level6ID,
			L1D.LocationName L1Desc, L2D.LocationName L2Desc, L3D.LocationName L3Desc, 
			L4D.LocationName L4Desc, L5D.LocationName L5Desc, L6D.LocationName L6Desc,ld.CompanyID,
			a.Attribute1,a.Attribute2,a.Attribute3,a.Attribute4,a.Attribute5,a.Attribute6,
			a.Attribute7, a.Attribute8, a.Attribute9, a.Attribute10
			,a.Attribute11,a.Attribute12,
			a.Attribute13, a.Attribute14, a.Attribute15, a.Attribute16,LT.LocationTypeName as LocationType,LT.LocationTypeID as LocationTypeID,
			L2D.LocationID [MappedLocationID],L2D.LocationCode as MappedLocationCode,L2D.LocationName as MappedLocationName
			FROM Tree_CTE A
			
LEFT JOIN LocationTable LD ON A.LocationID = LD.LocationID  
LEFT JOIN LocationTable L1D ON A.L1 = L1D.LocationID 
LEFT JOIN LocationTable L2D ON A.L2 = L2D.LocationID 
LEFT JOIN LocationTable L3D ON A.L3 = L3D.LocationID 
LEFT JOIN LocationTable L4D ON A.L4 = L4D.LocationID 
LEFT JOIN LocationTable L5D ON A.L5 = L5D.LocationID
LEFT JOIN LocationTable L6D ON A.L6 = L6D.LocationID 
left join LocationTypeTable LT on L2D.LocationTypeID=LT.LocationTypeID

GO

drop table tmp_LocationNewHierarchicalView
go 
select * into tmp_LocationNewHierarchicalView from LocationNewHierarchicalView
go 

ALTER View [dbo].[CategoryNewHierarchicalView]
As

WITH Tree_CTE(CategoryID, CategoryLevel, CategoryIDHierarchy, L1, L2, L3, L4, L5, L6, StatusID, CategoryCodeHierarchy,CategoryNameHierarchy,
		Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6, Attribute7, Attribute8, Attribute9, Attribute10,
		Attribute11,Attribute12,Attribute13,Attribute14,Attribute15,Attribute16)
AS
(
    SELECT CategoryID, 1 CategoryLevel, CAST(CategoryID as nvarchar(max)) CategoryIDHierarchy,
		CAST(CategoryID AS INT) L1, CAST(NULL AS INT) L2, CAST(NULL AS INT) L3, CAST(NULL AS INT) L4, CAST(NULL AS INT) L5, CAST(NULL AS INT) L6,
		StatusID, CAST(CategoryCode as nvarchar(MAX)) CategoryCode,CAST(CategoryName as nvarchar(MAX)) CategoryName,
		Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6,
		Attribute7, Attribute8, Attribute9, Attribute10,
		Attribute11,Attribute12,Attribute13,Attribute14,Attribute15,Attribute16
		FROM CategoryTable WHERE ParentCategoryID IS NULL
    UNION ALL
    SELECT ChildNode.CategoryID, CategoryLevel+1, CategoryIDHierarchy + '/' + CAST(ChildNode.CategoryID as nvarchar(max)),
		Tree_CTE.L1,
		CASE WHEN CategoryLevel > 1 THEN Tree_CTE.L2 ELSE CASE WHEN CategoryLevel = 1 THEN ChildNode.CategoryID ELSE NULL END END,
		CASE WHEN CategoryLevel > 2 THEN Tree_CTE.L3 ELSE CASE WHEN CategoryLevel = 2 THEN ChildNode.CategoryID ELSE NULL END END,
		CASE WHEN CategoryLevel > 3 THEN Tree_CTE.L4 ELSE CASE WHEN CategoryLevel = 3 THEN ChildNode.CategoryID ELSE NULL END END,
		CASE WHEN CategoryLevel > 4 THEN Tree_CTE.L5 ELSE CASE WHEN CategoryLevel = 4 THEN ChildNode.CategoryID ELSE NULL END END,
		CASE WHEN CategoryLevel > 5 THEN Tree_CTE.L6 ELSE CASE WHEN CategoryLevel = 5 THEN ChildNode.CategoryID ELSE NULL END END,
		ChildNode.StatusID, CategoryCodeHierarchy + '/' + ChildNode.CategoryCode,CategoryNameHierarchy+'/'+ChildNode.categoryname,
		ISNULL(ChildNode.Attribute1, Tree_CTE.Attribute1) Attribute1,
		ISNULL(ChildNode.Attribute2, Tree_CTE.Attribute2) Attribute3,
		ISNULL(ChildNode.Attribute3, Tree_CTE.Attribute3) Attribute3,
		ISNULL(ChildNode.Attribute4, Tree_CTE.Attribute4) Attribute4,
		ISNULL(ChildNode.Attribute5, Tree_CTE.Attribute5) Attribute5,
		ISNULL(ChildNode.Attribute6, Tree_CTE.Attribute6) Attribute6,
		ISNULL(ChildNode.Attribute7, Tree_CTE.Attribute7) Attribute7,
		ISNULL(ChildNode.Attribute8, Tree_CTE.Attribute8) Attribute8,
		ISNULL(ChildNode.Attribute9, Tree_CTE.Attribute9) Attribute9,
		ISNULL(ChildNode.Attribute10, Tree_CTE.Attribute10) Attribute10,
		ISNULL(ChildNode.Attribute11, Tree_CTE.Attribute11) Attribute11,
		ISNULL(ChildNode.Attribute12, Tree_CTE.Attribute12) Attribute12,
		ISNULL(ChildNode.Attribute13, Tree_CTE.Attribute13) Attribute13,
		ISNULL(ChildNode.Attribute14, Tree_CTE.Attribute14) Attribute14,
		ISNULL(ChildNode.Attribute15, Tree_CTE.Attribute15) Attribute15,
		ISNULL(ChildNode.Attribute16, Tree_CTE.Attribute16) Attribute16

		FROM CategoryTable AS ChildNode
    INNER JOIN Tree_CTE
    ON ChildNode.ParentCategoryID = Tree_CTE.CategoryID
)
SELECT A.CategoryID ChildID, A.CategoryID,
			A.CategoryNameHierarchy,
			A.CategoryID ParentCategoryID, CONVERT(nvarchar,A.L1) FirstLevel, CONVERT(nvarchar,A.L2) SecondLevel, CONVERT(nvarchar,A.L3) ThirdLevel, A.CategoryLevel [Level],
			b.categoryname CategoryName,  A.CategoryCodeHierarchy ChildCode, A.StatusID,
			A.CategoryIDHierarchy PID2, A.CategoryIDHierarchy AllLevelIDs,
			A.L1 Level1ID, A.L2 Level2ID, A.L3 Level3ID, A.L4 Level4ID, A.L5 Level5ID, A.L6 Level6ID,
			b1.categoryname L1Desc, b2.categoryname L2Desc, b3.categoryname L3Desc, 
			b4.categoryname L4Desc, b5.categoryname L5Desc, b6.categoryname L6Desc,
			a.Attribute1,a.Attribute2,a.Attribute3,a.Attribute4,a.Attribute5,a.Attribute6,
			a.Attribute7, a.Attribute8, a.Attribute9, a.Attribute10,a.Attribute11,a.Attribute12,a.Attribute13,
			a.Attribute14,a.Attribute15,a.Attribute16,CT.CategoryTypeName as CategoryType,CT.CategoryTypeID,
			b2.CategoryID [MappedCategoryID],b2.CategoryCode as MappedCategoryCode,b2.CategoryName as MappedCategoryName
			FROM Tree_CTE A
			left join  Categorytable b on a.categoryid=b.categoryID
			left join  Categorytable b1 on a.L1=b1.categoryID
			left join  Categorytable b2 on a.l2=b2.categoryID
			left join  Categorytable b3 on a.l3=b3.categoryID
			left join  Categorytable b4 on a.L4=b4.categoryID
			left join  Categorytable b5 on a.L5=b5.categoryID
			left join  Categorytable b6 on a.L6=b6.categoryID
			left join CategoryTypeTable CT on b2.categoryTypeID=CT.categoryTypeID

GO

drop table tmp_CategoryNewHierarchicalView
go 
select * into tmp_CategoryNewHierarchicalView from [CategoryNewHierarchicalView]
go 

create Procedure [dbo].[Prc_ValidateBulkAssetAddition] 
(
  @LocationCode   nvarchar(max) =NULL,
  @CategoryCode   nvarchar(max) = null,
  @ErrorID        nvarchar(50) OutPut,
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
	   Select @MissingCategoryCode=stuff((Select T.Categorycode from @CategoryIDTable T 
	                     where T.CategoryID is null and T.categoryID=a.CategoryID FOR XML PATH('')), 1, 1, '') from @categoryIDTable a 

	    set @ErrorMessage =@MissingCategoryCode +' Category code(s) are Invalid.'
		Set @ErrorID=1
		return 
	End 

	if exists (select LocationID from @LocationIDTable where LocationID is null)
	Begin
		Declare @MissingLocationCode nvarchar(max)
		Select @MissingLocationCode=stuff((Select T.LocationCode from @LocationIDTable T 
	                     where T.LocationID is null and T.LocationID=a.LocationID FOR XML PATH('')), 1, 1, '') from @LocationIDTable a 
		Set @ErrorMessage =@MissingLocationCode + +' Location code(s) are Invalid.'
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
	   join LocationNewHierarchicalView b on a.LocationID=b.ChildID
	   Where b.StatusID=@statusID

		
	if exists (Select CategoryTypeID from @CategoryIDTable where CategoryTypeID is null) 
	Begin
		Declare @MissingCategoryType nvarchar(max)
		Select @MissingCategoryType=stuff((Select T.CategoryCode from @CategoryIDTable T 
	                     where T.CategoryTypeID is null and T.CategoryID=a.CategoryID FOR XML PATH('')), 1, 1, '') from @CategoryIDTable a 
		Set @ErrorMessage =@MissingCategoryType + +' Category Type(s) are Missed.'
		Set @ErrorID=3
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
	   select @LocTypeID=LocationTypeID from @LocationIDTable where LocationID=@L2LocID
	   Select @LocationTypeName=LocationTypeName from LocationTypeTable where LocationTypeID=@LocTypeID

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
		Select @MissingUserName=stuff((select PersonCode from @validationTable a join PersonTable b on a.UserID=b.PersonID 
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
				Select @MissingUserEmail=stuff((select PersonCode from @validationTable a join PersonTable b on a.UserID=b.PersonID 
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
		Select @MissingApprovalRoleName=stuff((Select b.ApprovalRoleName From ApproveWorkflowLineItemTable a Join ApprovalRoleTable b on a.ApprovalRoleID=b.ApprovalRoleID
			where ApproveWorkFlowID=@ApproveworkflowID and a.StatusID=@statusID and b.StatusID=@statusID and a.ApprovalRoleID not in(select ApprovalRoleID from @validationTable)
				and b.ApprovalRoleID=p.ApprovalRoleID FOR XML PATH('')), 1, 1, '') from ApprovalRoleTable P
		Select @ModuleName=b.ModuleName from ApproveWorkflowTable a join ApproveModuleTable b on a.ApproveModuleID=b.ApproveModuleID where ApproveWorkFlowID=@ApproveworkflowID and a.StatusID=@statusID
		 
		Set @ErrorMessage ='Missing user(s) for the role : '+@MissingApprovalRoleName+' , ModuleName :'+@ModuleName+', LocationType : '+@LocationTypeName+'.'
		Set @ErrorID=7
		End 
	End 
End 
go 
create Procedure [dbo].[Prc_ValidateAssetAddition] 
(
  @LocationID		int =NULL,
  @CategoryID		int = null,
  @ErrorID			nvarchar(50) OutPut,
  @ErrorMessage		nvarchar(max) OutPut
)
as 
Begin
	Declare @LocatioCode nvarchar(max),@CategoryCode nvarchar(max)

	Select @LocatioCode=LocationCode from LocationTable where LocationID=@LocationID
	Select @CategoryCode=CategoryCode from CategoryTable where CategoryID=@CategoryID
	exec [Prc_ValidateBulkAssetAddition] @LocatioCode,@CategoryCode,@ErrorID,@ErrorMessage

end 
go 
Create View [dbo].[LocationForUserMappingView] 
as  
	Select a.LocationCode,a.LocationName,b.LocationName AS ParentLocation,a.LocationID,b.LocationName+'/'+a.LocationName as SecondLevelLocationName,l.LocationTypeName as LocationType
	   from LocationTable a 
	   join LocationTable b on a.ParentLocationID=b.LocationID
	   left join LocationTypeTable l ON A.LocationTypeID=L.LocationTypeID
	   where b.ParentLocationID is null and a.StatusID not in (500,3) and b.StatusID not in (500,3)
GO
drop view SecondLevelLocationTable
go 
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Prc_ValidateAssetAddition')
	DROP PROCEDURE Prc_ValidateAssetAddition
GO
create Procedure [dbo].[Prc_ValidateAssetAddition] 
(
  @LocationID		int =NULL,
  @CategoryID		int = null,
  @ErrorID			int OutPut,
  @ErrorMessage		nvarchar(max) OutPut
)
as 
Begin
	Declare @LocatioCode nvarchar(max),@CategoryCode nvarchar(max)

	Select @LocatioCode=LocationCode from LocationTable where LocationID=@LocationID
	Select @CategoryCode=CategoryCode from CategoryTable where CategoryID=@CategoryID
	exec [Prc_ValidateBulkAssetAddition] @LocatioCode,@CategoryCode,@ErrorID,@ErrorMessage

end 
go 
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Prc_ValidateBulkAssetAddition')
	DROP PROCEDURE Prc_ValidateBulkAssetAddition
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
	   Select @MissingCategoryCode=stuff((Select T.Categorycode from @CategoryIDTable T 
	                     where T.CategoryID is null and T.categoryID=a.CategoryID FOR XML PATH('')), 1, 1, '') from @categoryIDTable a 

	    set @ErrorMessage =@MissingCategoryCode +' Category code(s) are Invalid.'
		Set @ErrorID=1
		return 
	End 

	if exists (select LocationID from @LocationIDTable where LocationID is null)
	Begin
		Declare @MissingLocationCode nvarchar(max)
		Select @MissingLocationCode=stuff((Select T.LocationCode from @LocationIDTable T 
	                     where T.LocationID is null and T.LocationID=a.LocationID FOR XML PATH('')), 1, 1, '') from @LocationIDTable a 
		Set @ErrorMessage =@MissingLocationCode + +' Location code(s) are Invalid.'
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
	   join LocationNewHierarchicalView b on a.LocationID=b.ChildID
	   Where b.StatusID=@statusID

		
	if exists (Select CategoryTypeID from @CategoryIDTable where CategoryTypeID is null) 
	Begin
		Declare @MissingCategoryType nvarchar(max)
		Select @MissingCategoryType=stuff((Select T.CategoryCode from @CategoryIDTable T 
	                     where T.CategoryTypeID is null and T.CategoryID=a.CategoryID FOR XML PATH('')), 1, 1, '') from @CategoryIDTable a 
		Set @ErrorMessage =@MissingCategoryType + +' Category Type(s) are Missed.'
		Set @ErrorID=3
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
	   --select * from @LocationIDTable
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
		Select @MissingUserName=stuff((select PersonCode from @validationTable a join PersonTable b on a.UserID=b.PersonID 
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
				Select @MissingUserEmail=stuff((select PersonCode from @validationTable a join PersonTable b on a.UserID=b.PersonID 
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
		Select @MissingApprovalRoleName=stuff((Select b.ApprovalRoleName From ApproveWorkflowLineItemTable a Join ApprovalRoleTable b on a.ApprovalRoleID=b.ApprovalRoleID
			where ApproveWorkFlowID=@ApproveworkflowID and a.StatusID=@statusID and b.StatusID=@statusID and a.ApprovalRoleID not in(select ApprovalRoleID from @validationTable)
				and b.ApprovalRoleID=p.ApprovalRoleID FOR XML PATH('')), 1, 1, '') from ApprovalRoleTable P
		Select @ModuleName=b.ModuleName from ApproveWorkflowTable a join ApproveModuleTable b on a.ApproveModuleID=b.ApproveModuleID where ApproveWorkFlowID=@ApproveworkflowID and a.StatusID=@statusID
		 
		Set @ErrorMessage ='Missing user(s) for the role : '+@MissingApprovalRoleName+' , ModuleName :'+@ModuleName+', LocationType : '+@LocationTypeName+'.'
		Set @ErrorID=7
		End 
	End 

End 

go 

ALTER Procedure [dbo].[prc_ValidateForTransaction]
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
		select @ToLocationID =Level2id from LocationNewHierarchicalView where ChildID=@ToLocationID
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
		Select @MissingCategoryName=stuff((Select T.MappedCategryName from @TransactionLineItemTable T 
							 where T.CategoryType is null and T.AssetID=a.AssetID FOR XML PATH('')), 1, 1, '') from @TransactionLineItemTable a 
		Set @ErrorID=1
		set @ErrorMessage=@MissingCategoryName+' Category Type(s) are Missed.'
		return
	End 

	insert into @updateTable(updateRole,ApprovalRoleID,moduleName,FromLocationType,ToLocationType,ModuleID)
		Select case when a.ApproveModuleID=5 or a.ApproveModuleID=11 then UpdateDestinationLocationsForTransfer else UpdateRetirementDetailsForEachAssets end as updateRole, 
		b.ApprovalRoleID,d.ModuleName,FL.LocationTypeName,TL.LocationTypeName,a.ApproveWorkflowID FRom ApproveWorkflowTable a 
		join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
		join ApproveModuleTable D on a.ApproveModuleID=d.ApproveModuleID
		join ApprovalRoleTable C on b.ApprovalRoleID=C.ApprovalRoleID
		Join LocationTypeTable FL on a.FromLocationTypeID=FL.LocationTypeID
		Left join LocationTypeTable TL on a.ToLocationTypeID=TL.LocationTypeID
		where a.ApproveWorkflowID=@ApprovalWorkFlowID and a.StatusID=@StausID and b.StatusID=@StausID and c.StatusID=@StausID

	Declare @MissedModuleName nvarchar(100),@MissedFromLocationtype nvarchar(100),@MissedToLocationtype nvarchar(100),@MissedModuleID int,@errorMsg nvarchar(max)
	Select top 1 @MissedModuleName=moduleName,@MissedFromLocationtype=FromLocationType,@MissedToLocationtype=ToLocationType,@MissedModuleID=ModuleID from @updateTable
	
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
		T.FromLocationL2 else T.ToLocationL2 end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID 
		and ct.CategoryTypeID = (case when CT.IsAllCategoryType=0 and D.IsAllCategoryType=1 then CT.CategoryTypeID else D.CategoryTypeID end)
			where ApproveWorkFlowID=@ApprovalWorkFlowID and CT.statusID=@StausID and a.StatusID=@StausID
			and b.StatusID=@StausID and D.StatusID=@StausID
				group by userID,LocationID,a.ApprovalRoleID

	if(select count(*) from (
			select  a.ApprovalRoleID  from @validationTable a group by a.ApprovalRoleID)x) !=@AprpovalCnt
	Begin 
		Declare @MissingApprovalRoleName nvarchar(max),@ModuleName nvarchar(max)
		
		Select @MissingApprovalRoleName=stuff((Select b.ApprovalRoleName From ApproveWorkflowLineItemTable a Join ApprovalRoleTable b on a.ApprovalRoleID=b.ApprovalRoleID
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

		Select @MissingUserName=stuff((select PersonCode from @validationTable a join PersonTable b on a.UserID=b.PersonID 
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

		Select @MissingUserEmail=stuff((select PersonCode from @validationTable a join PersonTable b on a.UserID=b.PersonID 
			where userID not in (select userID from @validationTable2) and b.personID=p.PersonID FOR XML PATH('')), 1, 1, '') from PersonTable P
		
		Set @ErrorMessage ='EmailID not assigned to the mapped workflow user(s)- '+@MissingUserEmail +'.'
		Set @ErrorID=6
		RETURN
	 END 
	END 
	END
		
		
  end  
  go


  

ALTER Procedure [dbo].[prc_ValidateForTransaction]
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
		select @ToLocationID =Level2id from LocationNewHierarchicalView where ChildID=@ToLocationID
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
  go 
  
ALTER Procedure [dbo].[Prc_ValidateBulkAssetAddition] 
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
	   join LocationNewHierarchicalView b on a.LocationID=b.ChildID
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

go 
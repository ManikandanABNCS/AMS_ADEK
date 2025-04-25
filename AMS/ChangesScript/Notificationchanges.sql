Alter table NotificationInputTable add SYSUserID int foreign key references user_loginuserTable(UserID)
go 

ALTER PROC [dbo].[aprc_GetNotificationInputs]
AS
BEGIN
 
 exec prc_RemainderMainInsertData
 UPDATE DelegateRoleTable set StatusID=500 Where EffectiveEndDate<DATEADD(day, -1, GETDATE())

select a.*,'<td style="width:100px;border: 1px solid black;border-collapse: collapse;">'+UPPER(FieldName)+'</td>'  as FieldTitle,
'<td style="width:100px;border: 1px solid black;border-collapse: collapse;">##'+UPPER(FieldName)+'##</td>' as FieldValue,b.NotificationTemplateID into #tmpNotificationModuleFieldTable
		from NotificationModuleFieldTable a join NotificationTemplateFieldTable b  on A.NotificationFieldID=B.NotificationFieldID
		where A.StatusID = dbo.fnGetActiveStatusID() AND B.StatusID = dbo.fnGetActiveStatusID()

	SELECT DISTINCT A.NotificationInputID, A.SYSDataID1, A.SYSDataID2, A.SYSDataID3, 
			M.QueryType, M.QueryString, T.*,TT.IsAttachmentRequired,TT.AllowHtmlContent,X.TemplateTableBodyContentHeader,X.TemplateTableBodyContent,X.TemplateTableBodyContentFooter,A.ReminderMailCount
		,A.SYSUserID
		FROM NotificationInputTable A
		LEFT JOIN NotificationTemplateTable T ON T.NotificationTemplateID = A.NotificationTemplateID
		LEFT JOIN NotificationModuleTable M ON M.NotificationModuleID = T.NotificationModuleID
		LEFT JOIN NotificationTypeTable TT ON TT.NotificationTypeID = T.NotificationTypeID
		Left join (Select A.NotificationTemplateID,'<table style="border: 1px solid black;border-collapse: collapse;"><tbody>'
		+'<tr>'+STUFF((  SELECT '' + FieldTitle AS "text()"
          FROM #tmpNotificationModuleFieldTable US
          WHERE US.NotificationTemplateID = a.NotificationTemplateID 
          ORDER BY NotificationTemplateID
            FOR XML PATH(''),TYPE).value('./text()[1]','NVARCHAR(MAX)'),1,0,'')+'</tr>' as TemplateTableBodyContentHeader,
		'<tr>'+STUFF((  SELECT '' + FieldValue AS "text()"
          FROM #tmpNotificationModuleFieldTable US
          WHERE US.NotificationTemplateID = a.NotificationTemplateID 
          ORDER BY NotificationTemplateID
            FOR XML PATH(''),TYPE).value('./text()[1]','NVARCHAR(MAX)'),1,0,'')+'</tr>' as TemplateTableBodyContent,
		'</tbody></table>' as TemplateTableBodyContentFooter
		
		from #tmpNotificationModuleFieldTable A
		
		Group by  A.NotificationTemplateID) X ON X.NotificationTemplateID=T.NotificationTemplateID
		--LEFT JOIN (Select A.NotificationTemplateID,'<table style="border: 1px solid black;border-collapse: collapse;"><tbody>'
		--+'<tr>'+STRING_AGG('<td style="width:100px;border: 1px solid black;border-collapse: collapse;">'+(B.FieldName)+'</td>','')+'</tr>' as TemplateTableBodyContentHeader,
		--'<tr>'+STRING_AGG('<td style="width:100px;border: 1px solid black;border-collapse: collapse;">##'+UPPER(B.FieldName)+'##</td>','')+'</tr>' as TemplateTableBodyContent,
		--'</tbody></table>' as TemplateTableBodyContentFooter
		--from NotificationTemplateFieldTable A
		--Left Join NotificationModuleFieldTable B on A.NotificationFieldID=B.NotificationFieldID
		--WHERE A.StatusID = dbo.fnGetActiveStatusID() AND B.StatusID = dbo.fnGetActiveStatusID()
		--Group by  A.NotificationTemplateID
		--)X ON X.NotificationTemplateID=T.NotificationTemplateID

		WHERE A.IsCompleted = 0
		drop table #tmpNotificationModuleFieldTable
END
go 

ALTER View [dbo].[UserRightValueView]
AS
SELECT  P.PersonID UserID, P.PersonCode, P.PersonFirstName, P.PersonLastName, P.EMailID, P.MobileNo, 
			P.WhatsAppMobileNo ,x.RightValue, x.RightName,x.ApprovalRoleID,x.LocationID,X.CategoryTypeID
			from (
			Select 
		isnull(DD.DelegatedEmployeeID,RU.UserID) as UserID,RR.RightValue, A.RightName,UA.ApprovalRoleID,UA.LocationID,UA.CategoryTypeID
	FROM User_RightTable A
	LEFT JOIN User_RoleRightTable RR ON RR.RightID = A.RightID
	LEFT JOIN User_UserRoleTable RU ON RU.RoleID = RR.RoleID
	--LEFT JOIN  PersonTable P ON P.PersonID = RU.UserID
	
	LEFT JOIN UserApprovalRoleMappingTable UA ON RU.UserID=UA.UserID AND UA.StatusID=dbo.fnGetActiveStatusID()
	--LEFT JOIN DepartmentTable DP ON DP.DepartmentID = P.DepartmentID
	Left join (select * from DelegateRoleTable where  getdate() between EffectiveStartDate and EffectiveEndDate) DD on DD.EmployeeID=RU.UserID
	WHERE RR.RightValue > 0
	) x join PersonTable p on x.userID=p.PersonID
		AND x.userID IS NOT NULL
		--AND A.RightName = 'ApproveUniformRequest'
UNION
SELECT  P.PersonID UserID, P.PersonCode, P.PersonFirstName, P.PersonLastName, P.EMailID, P.MobileNo, 
			P.WhatsAppMobileNo,x.RightValue, x.RightName,x.ApprovalRoleID,x.LocationID,X.CategoryTypeID from(
			
			Select 
		isnull(DD.DelegatedEmployeeID,UR.UserID) as UserID,UR.RightValue, A.RightName,UA.ApprovalRoleID,UA.LocationID,UA.CategoryTypeID
	FROM User_RightTable A
	LEFT JOIN User_UserRightTable UR ON UR.RightID = A.RightID
	--LEFT JOIN PersonTable P ON P.PersonID = UR.UserID
	LEFT JOIN UserApprovalRoleMappingTable UA ON UR.UserID=UA.UserID AND UA.StatusID=dbo.fnGetActiveStatusID()
	Left join (select * from DelegateRoleTable where  getdate() between EffectiveStartDate and EffectiveEndDate) DD on DD.EmployeeID=UR.UserID
	--LEFT JOIN DepartmentTable DP ON DP.DepartmentID = P.DepartmentID
	WHERE UR.RightValue > 0
	)x join PersonTable p on x.userID=p.PersonID
		AND x.userID IS NOT NULL
		--AND A.RightName = 'ApproveUniformRequest'

GO

ALTER Trigger [dbo].[trg_AlertDataInputTable_Ins] On [dbo].[ApprovalHistoryTable]
After Insert 
as 
Begin 
   Declare @NotificationTemplateID int,@moduleID int,@PrimaryID int,@referenceID int ,@ApprovalRoleID int,@FromLocationID int,@TolocationID int,@FromLocationTypeID int 
   ,@TolocationtypeID int ,@CategoyTypeID int ,@StatusID int,@RightName nvarchar(100)

   select @StatusID= [dbo].fnGetActiveStatusID()
   
   If exists(select OrderNo from inserted where OrderNo=1 and StatusID=150)
	Begin 
	   select @moduleID=ApproveModuleID,@PrimaryID=ApprovalHistoryID,@referenceID=TransactionID,@ApprovalRoleID=ApprovalRoleID,@FromLocationID=FromLocationID,@TolocationID=ToLocationID
	     ,@FromLocationTypeID=FromLocationTypeID,@TolocationtypeID=ToLocationTypeID,@CategoyTypeID=CategoryTypeID  from inserted 
		IF @moduleID=5 or @moduleID=11
		begin 
		   select @NotificationTemplateID= NotificationTemplateID from NotificationTemplateTable where 
		   TemplateCode='AssetTransferBeforeApproval'
		   set @RightName=case when @moduleID=5 then 'AssetTransferApproval' else 'InternalAssetTransferApproval' end 
		end 
	   IF @moduleID=10
		begin 
		   select @NotificationTemplateID= NotificationTemplateID from NotificationTemplateTable where 
		   TemplateCode='AssetRetirementBeforeApproval'
		   Set @RightName='AssetRetirementApproval'
		end 

	   if @NotificationTemplateID is not null and @NotificationTemplateID!=''
	   begin 
	   
	   Insert into NotificationInputTable(NotificationTemplateID,SYSDataID1,SYSDataID2,SYSDataID3,SYSUserID)
	  -- values(@NotificationTemplateID,@referenceID,@PrimaryID,NULL)
	  select @NotificationTemplateID,@referenceID,@PrimaryID,NULL,D.UserID
	   from inserted a 
	   Left join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
	   Left join CategoryTypeTable CT on a.CategoryTypeID=Ct.CategoryTypeID
	   Left join UserRightValueView D on  case when b.ApprovalLocationTypeID=5 then 
		a.FromLocationID else a.ToLocationID end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID and 
		 case when CT.IsAllCategoryType=1 then D.CategoryTypeID else a.CategorytypeID end =D.CategoryTypeID  and RightName=@RightName
	  where ApprovalHistoryID=@PrimaryID and CT.statusID=@StatusID and b.StatusID=@StatusID --and d.StatusID=@StatusID
 
	   End 
	End 
End 

go 

ALTER Trigger [dbo].[trg_AlertDataInputTable_Upt] On [dbo].[ApprovalHistoryTable]
After update 
as 
Begin 
   Declare @NotificationTemplateID int,@moduleID int,@PrimaryID int,@referenceID int,@OrderID int,@CreatedDatetime smallDateTime,@createdBy int,
   @NextOrderID int ,@maxOrderID int,@RightName nvarchar(100),@statusID int 

  select @StatusID= [dbo].fnGetActiveStatusID()
   
   if exists(select i.statusID from inserted i join Deleted D on i.ApprovalHistoryID=D.ApprovalHistoryID  
			where i.StatusID = dbo.fnGetActiveStatusID() and d.StatusID=150) 
	Begin 
		select @moduleID=i.ApproveModuleID,@PrimaryID=i.ApprovalHistoryID,@referenceID=i.TransactionID,@OrderID=i.OrderNo,@CreatedDatetime=i.CreatedDateTime,@createdBy=i.CreatedBy
	         From inserted i join Deleted D on i.ApprovalHistoryID=D.ApprovalHistoryID  where i.StatusID = dbo.fnGetActiveStatusID() and d.StatusID=150   
			
			select @maxOrderID=max(OrderNo) from ApprovalHistoryTable where TransactionID=@referenceID and ApproveModuleID=@moduleID
			
			--requestor notification
			 IF @moduleID=5 or @moduleID=11 
				begin 
				   select @NotificationTemplateID= NotificationTemplateID from NotificationTemplateTable where 
				   TemplateCode='AssetTransferRequestorNotification'
				    set @RightName=case when @moduleID=5 then 'AssetTransfer' else 'InternalAssetTransfer' end 
				end 
			   IF @moduleID=10
				begin 
				   select @NotificationTemplateID= NotificationTemplateID from NotificationTemplateTable where 
				   TemplateCode='AssetRetirementRequestorNotification'
				    SET @RightName='AssetRetirement'
				end 
				if @NotificationTemplateID is not null and @NotificationTemplateID!=''
				   begin 
				   Insert into NotificationInputTable(NotificationTemplateID,SYSDataID1,SYSDataID2,SYSDataID3,SYSUserID)
				   values( @NotificationTemplateID,@referenceID,null,null,null)
				   End 
		   --Level wise notification
			 If @OrderID>1
			 Begin 
			 IF @moduleID=5 or @moduleID=11 
				begin 
				   select @NotificationTemplateID= NotificationTemplateID from NotificationTemplateTable where 
				   TemplateCode='AssetTransferNotification'
				    set @RightName=case when @moduleID=5 then 'AssetTransfer' else 'InternalAssetTransfer' end 
				end 
			   IF @moduleID=10
				begin 
				   select @NotificationTemplateID= NotificationTemplateID from NotificationTemplateTable where 
				   TemplateCode='AssetRetirementNotification'
				    SET @RightName='AssetRetirement'
				end 
				if @NotificationTemplateID is not null and @NotificationTemplateID!=''
				   begin 
				   Insert into NotificationInputTable(NotificationTemplateID,SYSDataID1,SYSDataID2,SYSDataID3,SYSUserID)
				   select @NotificationTemplateID,TransactionID,ApprovalHistoryID,NULL,NULL
				      from ApprovalHistoryTable where StatusID=dbo.fnGetActiveStatusID() and TransactionID=@referenceID and ApproveModuleID=@moduleID 
					    AND  ORDERNO>= CASE WHEN @maxOrderID=@OrderID THEN @ORDERID ELSE @ORDERID-1 END 
					 --and ApprovalHistoryID!=@PrimaryID
				   End 
		    End 
		  
		 
	  IF EXISTS(select OrderNo From ApprovalHistoryTable where OrderNo=@OrderID+1 and StatusID=150 and ApproveModuleID=@moduleID and TransactionID=@referenceID
	        and CreatedDateTime=@CreatedDatetime)
		Begin 
		    set @NotificationTemplateID=NULL

		    select @moduleID=i.ApproveModuleID,@PrimaryID=i.ApprovalHistoryID,@referenceID=i.TransactionID,@NextOrderID=i.OrderNo,@CreatedDatetime=i.CreatedDateTime
		     From ApprovalHistoryTable i where OrderNo=@OrderID+1 and StatusID=150 and ApproveModuleID=@moduleID and TransactionID=@referenceID
	         and CreatedDateTime=@CreatedDatetime
		    
			--next wise notification
				 IF @moduleID=5 or @moduleID=11 
				begin 
				   select @NotificationTemplateID= NotificationTemplateID from NotificationTemplateTable where 
				   TemplateCode='AssetTransferBeforeApproval'
				    SET @RightName=case when @moduleID=5 then 'AssetTransferApproval' else 'InternalAssetTransferApproval' end
				end 
			   IF @moduleID=10
				begin 
				   select @NotificationTemplateID= NotificationTemplateID from NotificationTemplateTable where 
				   TemplateCode='AssetRetirementBeforeApproval'
				   Set @RightName='AssetRetirementApproval'
				end 

			   if @NotificationTemplateID is not null and @NotificationTemplateID!=''
			   begin 
			   Insert into NotificationInputTable(NotificationTemplateID,SYSDataID1,SYSDataID2,SYSDataID3,SYSUserID)
			    select @NotificationTemplateID,@referenceID,@PrimaryID,NULL,D.UserID
				   from inserted a 
				   Left join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
				   Left join CategoryTypeTable CT on a.CategoryTypeID=Ct.CategoryTypeID
				   Left join UserRightValueView D on  case when b.ApprovalLocationTypeID=5 then 
					a.FromLocationID else a.ToLocationID end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID and 
					 case when CT.IsAllCategoryType=1 then D.CategoryTypeID else a.CategorytypeID end =D.CategoryTypeID  and RightName=@RightName
				  where ApprovalHistoryID=@PrimaryID and CT.statusID=@StatusID and b.StatusID=@StatusID --and d.StatusID=@StatusID
			   --values(@NotificationTemplateID,@referenceID,@PrimaryID,NULL)
			   End 
	   End
    End 
	Else 
	Begin 
	   if exists(select i.statusID from inserted i join Deleted D on i.ApprovalHistoryID=D.ApprovalHistoryID  where i.StatusID=200 and d.StatusID=150)
	   Begin 
	     select @moduleID=ApproveModuleID,@PrimaryID=ApprovalHistoryID,@referenceID=TransactionID,@createdBy=CreatedBy
	               from inserted 
	     select @maxOrderID=max(OrderNo) from ApprovalHistoryTable where TransactionID=@referenceID and ApproveModuleID=@moduleID
		--rejected notification

			IF @moduleID=5 or @moduleID=11
			begin 
			   select @NotificationTemplateID= NotificationTemplateID from NotificationTemplateTable where 
			   TemplateCode='AssetTransferRejected'
			   SET @RightName=case when @moduleID=5 then 'AssetTransfer' else 'InternalAssetTransfer' end
			end 
		   IF @moduleID=10
			begin 
			   select @NotificationTemplateID= NotificationTemplateID from NotificationTemplateTable where 
			   TemplateCode='AssetRetirementRejected'
			   SET @RightName='AssetRetirement'
			end 

	   if @NotificationTemplateID is not null and @NotificationTemplateID!=''
	   begin 
		   Insert into NotificationInputTable(NotificationTemplateID,SYSDataID1,SYSDataID2,SYSDataID3,SYSUserID)
		   values(@NotificationTemplateID,@referenceID,@PrimaryID,NULL,NULL)
		 --  select @NotificationTemplateID,@referenceID,@PrimaryID,NULL,D.UserID
		 --  from inserted a 
		 --  Left join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
		 --  Left join CategoryTypeTable CT on a.CategoryTypeID=Ct.CategoryTypeID
		 --  Left join UserRightValueView D on  case when b.ApprovalLocationTypeID=5 then 
			--a.FromLocationID else a.ToLocationID end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID and 
			-- case when CT.IsAllCategoryType=1 then D.CategoryTypeID else a.CategorytypeID end =D.CategoryTypeID  and RightName=@RightName
		 -- where ApprovalHistoryID=@PrimaryID and CT.statusID=@StatusID and b.StatusID=@StatusID --and d.StatusID=@StatusID
	   End 

	  
		   --Level wise notification
			 
			 IF @moduleID=5 or @moduleID=11 
				begin 
				   select @NotificationTemplateID= NotificationTemplateID from NotificationTemplateTable where 
				   TemplateCode='AssetTransferFailedNotification'
				   Set @RightName=case when @moduleID=5 then 'AssetTransfer' else 'InternalAssetTransfer' end 
				end 
			   IF @moduleID=10
				begin 
				   select @NotificationTemplateID= NotificationTemplateID from NotificationTemplateTable where 
				   TemplateCode='AssetRetirementFailedNotification'
				   set @RightName='AssetRetirement'
				end 
				if @NotificationTemplateID is not null and @NotificationTemplateID!=''
				   begin 
				   Insert into NotificationInputTable(NotificationTemplateID,SYSDataID1,SYSDataID2,SYSDataID3,SYSUserID)
				   select @NotificationTemplateID,TransactionID,ApprovalHistoryID,NULL,NULL
				      from ApprovalHistoryTable where StatusID=dbo.fnGetActiveStatusID()  and TransactionID=@referenceID and ApproveModuleID=@moduleID 
					  AND  ORDERNO>= CASE WHEN @maxOrderID=@OrderID THEN @ORDERID ELSE @ORDERID-1 END 
					  --and ApprovalHistoryID!=@PrimaryID
				   End 
	   End 
	End 
End 
go 
ALTER View [dbo].[nvwAssetTransfer_ForBeforeApproval]
as 
  select  a1.*,old.LocationName as OldLocationName,New.LocationName as NewLocationName,
  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubType,
ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
OldDept.DepartmentName as OldDepartmentName,NewDept.DepartmentName as NewDepartmentName,OldCate.CategoryName as OldCategoryName,
Oldprod.ProductName as OldProductName,newprod.ProductName as NewProductName,OldSec.SectionName as oldSectionName,newsec.SectionName as NewSectionName,

newCate.CategoryName as NewCategoryName,
 FORMATMESSAGE( dbo.fn_GetServerURL()+'/AssetTransferApproval/EmailEdit?id=%d&UserID=%d' , ah.ApprovalHistoryID,p2.UserID) ApprovalURL,
--NULL ApprovalURL,
--Notification supporting fields
		A.TransactionID SYSDataID1, AH.ApprovalHistoryID SYSDataID2, '' SYSDataID3,p2.UserID as SYSUserID,
		p2.EMailID SYSToAddresses, '' SYSCCAddresses, '' SYSBCCAddresses,
		NULL SYSToMobileNos, NULL SYSWhatsAppMobileNos,AD.ApprovalRoleID
		
  From TransactionTable a 
  join TransactionLineItemTable b on a.TransactionID=b.TransactionID 
  join AssetNewView a1 on b.AssetID=a1.AssetID 
  left join ApprovalHistoryTable AH on a.TransactionID=AH.TransactionID --and ah.StatusID=1501
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
  Left join UserRightValueView P2 on P2.ApprovalRoleID = AH.ApprovalRoleID  and RightName = 'AssetTransferApproval'
  and case when AD.ApprovalLocationTypeID=5 then 
		AH.FromLocationID else AH.ToLocationID end =P2.LocationID
--  LEFT JOIN (SELECT ApprovalRoleID, --STRING_AGG(EmailID, ';') EMailIDs,LocationID
--stuff((select ';'+Emailid from UserRightValueView PL where PL.ApprovalRoleID=p2.ApprovalRoleID group by EMailID  FOR XML PATH('')), 1, 1, '') as EMailIDs--,
--  --LocationID

--			FROM UserRightValueView P2 where RightName = 'AssetTransferApproval'
--				AND ApprovalRoleID IS NOT NULL
--				AND EmailID IS NOT NULL
--			GROUP BY ApprovalRoleID) UGRP ON UGRP.ApprovalRoleID = AH.ApprovalRoleID
WHERE A.StatusID in (150,200) and a.TransactionTypeID=5

GO



ALTER FUNCTION [dbo].[fn_GetServerURL]()
RETURNS VARCHAR(1000)
AS
BEGIN
declare @url nvarchar(max)
select @url=ConfiguarationValue from ConfigurationTable where ConfiguarationName='EmailURL'
	--RETURN 'http://10.251.110.57:8082';
	return @url
END
go 
if not exists(select EntitycodeName from EntityCodeTable where EntityCodeName='InternalAssetTransfer')
BEgin
 insert into EntityCodeTable(EntityCodeName,CodePrefix,CodeFormatString,LastUsedNo,UseDateTime)
 values('InternalAssetTransfer','IAT','{0:00000}',0,0)
end 
go 
 ALTER View [dbo].[nvwAssetTransfer_ForAfterApproval]
as 
Select a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubType,
ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
a1.*,P.EMailID as SYSToAddresses, '' SYSCCAddresses, '' SYSBCCAddresses,
		NULL SYSToMobileNos, NULL SYSWhatsAppMobileNos,A.TransactionID SYSDataID1, NULL SYSDataID2, NULL SYSDataID3,old.LocationName as OldLocationName,New.LocationName as NewLocationName,
		Room.LocationName as RoomName
  From TransactionTable a 
  join TransactionLineItemTable b on a.TransactionID=b.TransactionID 
  join AssetNewView a1 on b.AssetID=a1.AssetID 
   --left join ApprovalHistoryTable AH on a.TransactionID=AH.TransactionID and ah.StatusID = dbo.fnGetActiveStatusID()
   Left join PersonTable p on a.CreatedBy=p.PersonID
    left join LocationTable Old on b.FromLocationID=Old.LocationID
	left join LocationTable new on b.ToLocationID=new.LocationID
	Left join LocationTable Room on b.RoomID=Room.LocationID
	where  a.TransactionTypeID in (5,11)
GO

ALTER View [dbo].[nvwAssetTransfer_ForNotification]
as 
  select  a1.*,old.LocationName as OldLocationName,New.LocationName as NewLocationName,
  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubType,
ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
OldDept.DepartmentName as OldDepartmentName,NewDept.DepartmentName as NewDepartmentName,OldCate.CategoryName as OldCategoryName,
Oldprod.ProductName as OldProductName,newprod.ProductName as NewProductName,OldSec.SectionName as oldSectionName,newsec.SectionName as NewSectionName,

newCate.CategoryName as NewCategoryName,	
 FORMATMESSAGE( dbo.fn_GetServerURL()+'AssetTransferApproval/EmailEdit?id=%d&UserID=%d' , ah.ApprovalHistoryID,p2.UserID) ApprovalURL,

--Notification supporting fields
		A.TransactionID SYSDataID1, AH.ApprovalHistoryID SYSDataID2, '' SYSDataID3,p2.UserID as SYSUserID,
		 
		  p2.EMailID SYSToAddresses,
		  '' SYSCCAddresses, '' SYSBCCAddresses,
		NULL SYSToMobileNos, NULL SYSWhatsAppMobileNos,(SELECT EMAILID FROM PERSONTABLE WHERE PERSONID=AH.CREATEDBY) as ApprovedBy,convert(nvarchar(100),ah.CreatedDateTime,103) as ApprovedDate
		
  From TransactionTable a 
  join TransactionLineItemTable b on a.TransactionID=b.TransactionID 
  join AssetNewView a1 on b.AssetID=a1.AssetID 
  left join ApprovalHistoryTable AH on a.TransactionID=AH.TransactionID 
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
   Left join UserRightValueView P2 on P2.ApprovalRoleID = AH.ApprovalRoleID  and RightName = 'AssetTransferApproval'
  and case when AD.ApprovalLocationTypeID=5 then 
		AH.FromLocationID else AH.ToLocationID end =P2.LocationID
--  LEFT JOIN (SELECT ApprovalRoleID, --STRING_AGG(EmailID, ';') EMailIDs,LocationID
--stuff((select ';'+Emailid from UserRightValueView PL where PL.ApprovalRoleID=p2.ApprovalRoleID group by EMailID  FOR XML PATH('')), 1, 1, '') as EMailIDs--,
--  --LocationID

--			FROM UserRightValueView P2 where RightName = 'AssetTransferApproval'
--				AND ApprovalRoleID IS NOT NULL
--				AND EmailID IS NOT NULL
--			GROUP BY ApprovalRoleID) UGRP ON UGRP.ApprovalRoleID = AH.ApprovalRoleID

WHERE  a.TransactionTypeID in (5,11)

GO
ALTER View [dbo].[nvwAssetTransfer_ForBeforeApproval]
as 
  select  a1.*,old.LocationName as OldLocationName,New.LocationName as NewLocationName,
  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubType,
ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
OldDept.DepartmentName as OldDepartmentName,NewDept.DepartmentName as NewDepartmentName,OldCate.CategoryName as OldCategoryName,
Oldprod.ProductName as OldProductName,newprod.ProductName as NewProductName,OldSec.SectionName as oldSectionName,newsec.SectionName as NewSectionName,

newCate.CategoryName as NewCategoryName,
 FORMATMESSAGE( dbo.fn_GetServerURL()+'AssetTransferApproval/EmailEdit?id=%d&UserID=%d' , ah.ApprovalHistoryID,p2.UserID) ApprovalURL,
--NULL ApprovalURL,
--Notification supporting fields
		A.TransactionID SYSDataID1, AH.ApprovalHistoryID SYSDataID2, '' SYSDataID3,p2.UserID as SYSUserID,
		p2.EMailID SYSToAddresses, '' SYSCCAddresses, '' SYSBCCAddresses,
		NULL SYSToMobileNos, NULL SYSWhatsAppMobileNos,AD.ApprovalRoleID
		
  From TransactionTable a 
  join TransactionLineItemTable b on a.TransactionID=b.TransactionID 
  join AssetNewView a1 on b.AssetID=a1.AssetID 
  left join ApprovalHistoryTable AH on a.TransactionID=AH.TransactionID --and ah.StatusID=1501
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
  Left join UserRightValueView P2 on P2.ApprovalRoleID = AH.ApprovalRoleID  and RightName = 'AssetTransferApproval'
  and case when AD.ApprovalLocationTypeID=5 then 
		AH.FromLocationID else AH.ToLocationID end =P2.LocationID
--  LEFT JOIN (SELECT ApprovalRoleID, --STRING_AGG(EmailID, ';') EMailIDs,LocationID
--stuff((select ';'+Emailid from UserRightValueView PL where PL.ApprovalRoleID=p2.ApprovalRoleID group by EMailID  FOR XML PATH('')), 1, 1, '') as EMailIDs--,
--  --LocationID

--			FROM UserRightValueView P2 where RightName = 'AssetTransferApproval'
--				AND ApprovalRoleID IS NOT NULL
--				AND EmailID IS NOT NULL
--			GROUP BY ApprovalRoleID) UGRP ON UGRP.ApprovalRoleID = AH.ApprovalRoleID
WHERE A.StatusID in (150,200) and a.TransactionTypeID in (5,11)

GO

ALTER View [dbo].[nvwAssetTransfer_ForAfterApproval]
as 
Select a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubType,
ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
a1.*,P.EMailID as SYSToAddresses, '' SYSCCAddresses, '' SYSBCCAddresses,
		NULL SYSToMobileNos, NULL SYSWhatsAppMobileNos,A.TransactionID SYSDataID1, NULL SYSDataID2, NULL SYSDataID3,old.LocationName as OldLocationName,New.LocationName as NewLocationName,
		Room.LocationName as RoomName,FORMATMESSAGE( dbo.fn_GetServerURL()+'AssetTransferApproval/EmailView?id=%d' , a.TransactionID) ApprovalURL
  From TransactionTable a 
  join TransactionLineItemTable b on a.TransactionID=b.TransactionID 
  join AssetNewView a1 on b.AssetID=a1.AssetID 
   --left join ApprovalHistoryTable AH on a.TransactionID=AH.TransactionID and ah.StatusID = dbo.fnGetActiveStatusID()
   Left join PersonTable p on a.CreatedBy=p.PersonID
    left join LocationTable Old on b.FromLocationID=Old.LocationID
	left join LocationTable new on b.ToLocationID=new.LocationID
	Left join LocationTable Room on b.RoomID=Room.LocationID
	where  a.TransactionTypeID in (5,11)
GO



ALTER View [dbo].[nvwAssetTransfer_ForBeforeApproval]
as 
  select  a1.*,old.LocationName as OldLocationName,New.LocationName as NewLocationName,
  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubType,
ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
OldDept.DepartmentName as OldDepartmentName,NewDept.DepartmentName as NewDepartmentName,OldCate.CategoryName as OldCategoryName,
Oldprod.ProductName as OldProductName,newprod.ProductName as NewProductName,OldSec.SectionName as oldSectionName,newsec.SectionName as NewSectionName,

newCate.CategoryName as NewCategoryName,
 FORMATMESSAGE( dbo.fn_GetServerURL()+'AssetTransferApproval/EmailEdit?id=%d&UserID=%d' , ah.ApprovalHistoryID,p2.UserID) ApprovalURL,
--NULL ApprovalURL,
--Notification supporting fields
		A.TransactionID SYSDataID1, AH.ApprovalHistoryID SYSDataID2, '' SYSDataID3,p2.UserID as SYSUserID,
		p2.EMailID SYSToAddresses, '' SYSCCAddresses, '' SYSBCCAddresses,
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
  Left join UserRightValueView P2 on P2.ApprovalRoleID = AH.ApprovalRoleID  and RightName = 'AssetTransferApproval'
  and case when AD.ApprovalLocationTypeID=5 then 
		AH.FromLocationID else AH.ToLocationID end =P2.LocationID  and 
		case when CT.IsAllCategoryType=1 then P2.CategoryTypeID else AH.CategorytypeID end =P2.CategoryTypeID   
--  LEFT JOIN (SELECT ApprovalRoleID, --STRING_AGG(EmailID, ';') EMailIDs,LocationID
--stuff((select ';'+Emailid from UserRightValueView PL where PL.ApprovalRoleID=p2.ApprovalRoleID group by EMailID  FOR XML PATH('')), 1, 1, '') as EMailIDs--,
--  --LocationID

--			FROM UserRightValueView P2 where RightName = 'AssetTransferApproval'
--				AND ApprovalRoleID IS NOT NULL
--				AND EmailID IS NOT NULL
--			GROUP BY ApprovalRoleID) UGRP ON UGRP.ApprovalRoleID = AH.ApprovalRoleID
WHERE A.StatusID in (150,200) and a.TransactionTypeID in (5,11)

GO

ALTER View [dbo].[nvwAssetTransfer_ForNotification]
as 
  select  a1.*,old.LocationName as OldLocationName,New.LocationName as NewLocationName,
  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubType,
ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
OldDept.DepartmentName as OldDepartmentName,NewDept.DepartmentName as NewDepartmentName,OldCate.CategoryName as OldCategoryName,
Oldprod.ProductName as OldProductName,newprod.ProductName as NewProductName,OldSec.SectionName as oldSectionName,newsec.SectionName as NewSectionName,

newCate.CategoryName as NewCategoryName,	
 FORMATMESSAGE( dbo.fn_GetServerURL()+'AssetTransferApproval/EmailEdit?id=%d&UserID=%d' , ah.ApprovalHistoryID,p2.UserID) ApprovalURL,

--Notification supporting fields
		A.TransactionID SYSDataID1, AH.ApprovalHistoryID SYSDataID2, '' SYSDataID3,p2.UserID as SYSUserID,
		 
		  p2.EMailID SYSToAddresses,
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
   Left join UserRightValueView P2 on P2.ApprovalRoleID = AH.ApprovalRoleID  and RightName = 'AssetTransferApproval'
  and case when AD.ApprovalLocationTypeID=5 then 
		AH.FromLocationID else AH.ToLocationID end =P2.LocationID and 
		case when CT.IsAllCategoryType=1 then P2.CategoryTypeID else AH.CategorytypeID end =P2.CategoryTypeID   
--  LEFT JOIN (SELECT ApprovalRoleID, --STRING_AGG(EmailID, ';') EMailIDs,LocationID
--stuff((select ';'+Emailid from UserRightValueView PL where PL.ApprovalRoleID=p2.ApprovalRoleID group by EMailID  FOR XML PATH('')), 1, 1, '') as EMailIDs--,
--  --LocationID

--			FROM UserRightValueView P2 where RightName = 'AssetTransferApproval'
--				AND ApprovalRoleID IS NOT NULL
--				AND EmailID IS NOT NULL
--			GROUP BY ApprovalRoleID) UGRP ON UGRP.ApprovalRoleID = AH.ApprovalRoleID

WHERE  a.TransactionTypeID in (5,11)

GO

ALTER Procedure [dbo].[prc_ValidateForTransaction]
(
   @FromAssetID				nvarchar(max)	 = NULL,
   @ToLocationID			int				 = NULL,
   @FromLocationTypeID		int				 = NULL,
   @ToLocationTypeID		Int				 = NULL,
   @ApprovalWorkFlowID		int              = NULL
)
as 
Begin
  Declare @ErrorMsg nvarchar(max),@UpdateCount int,@AprpovalCnt int ,@StausID int,@RightName nvarchar(100),@modelID int 
  select @StausID=[dbo].fnGetActiveStatusID()
  SElect @modelID=ApproveModuleID from ApproveWorkflowTable where ApproveWorkflowID=@ApprovalWorkFlowID
  select @RightName=case when @modelID=5  then 'AssetTransferApproval' when @modelID=11 then 'InternalAssetTransferApproval' else 'AssetRetirementApproval' end 
  Declare @updateTable table(updateRole bit,ApprovalRoleID int)


  declare @TransactionLineItemTable table (AssetID int,FromLocationL2 int ,ToLocationL2 int,
  LocationType nvarchar(100),CategoryType nvarchar(100),ApprovalWorkFlowID int )
  
  Select @AprpovalCnt=count(OrderNo) from 
  ApproveWorkflowTable a join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
  where a.StatusID=1 and b.StatusID=1 and a.ApproveWorkflowID=@ApprovalWorkFlowID

  --print @AprpovalCnt
  --Declare @ApprovalUserTable Table ()
  insert into @TransactionLineItemTable(AssetID,FromLocationL2,ToLocationL2,LocationType,CategoryType,ApprovalWorkFlowID)
  select AssetID,LocationL2,@ToLocationID,LocationType,categorytype,@ApprovalWorkFlowID
  From AssetNewView where assetid in (select value from Split(@fromAssetID,',')) 

  --select * from @TransactionLineItemTable

  insert into @updateTable(updateRole,ApprovalRoleID)
  Select case when a.ApproveModuleID=5 or a.ApproveModuleID=11 then UpdateDestinationLocationsForTransfer else UpdateRetirementDetailsForEachAssets end as updateRole, 
  b.ApprovalRoleID FRom ApproveWorkflowTable a 
	  join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
	  join ApprovalRoleTable C on b.ApprovalRoleID=C.ApprovalRoleID
	  where a.ApproveWorkflowID=@ApprovalWorkFlowID and a.StatusID=@StausID and b.StatusID=@StausID and c.StatusID=@StausID
  --select * from @updateTable
  
  Declare @validationTable Table(OrderNo int,ApprovalRoleID int) 
	
	insert into @validationTable(OrderNo,ApprovalRoleID)
 select a.OrderNo,a.ApprovalRoleID from ApproveWorkflowLineItemTable a 
   join @TransactionLineItemTable T on a.ApproveWorkFlowID=T.ApprovalWorkFlowID
   join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
   join CategoryTypeTable CT on T.CategoryType=Ct.categoryTypeName
   join UserApprovalRoleMappingTable D on  case when b.ApprovalLocationTypeID=5 then 
	T.FromLocationL2 else T.ToLocationL2 end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID and D.CategoryTypeID=CT.CategoryTypeID
  where ApproveWorkFlowID=@ApprovalWorkFlowID and CT.statusID=@StausID and a.StatusID=@StausID
  and b.StatusID=@StausID and D.StatusID=@StausID
  group by a.OrderNo,a.ApprovalRoleID
  --select * from @validationTable
    if(select isnull(count(a.OrderNo),0) from  @validationTable a
  join ApproveWorkflowLineItemTable b on a.approvalroleID=b.ApprovalRoleID and a.OrderNo=b.OrderNo
  where b.ApproveWorkFlowID=@ApprovalWorkFlowID and b.StatusID=@StausID )!=@AprpovalCnt
  Begin 
     set @ErrorMsg ='Approval role not assigned to the user'
  end 

  insert into @validationTable(OrderNo,ApprovalRoleID)
   select a.OrderNo,a.ApprovalRoleID from ApproveWorkflowLineItemTable a 
   join @TransactionLineItemTable T on a.ApproveWorkFlowID=T.ApprovalWorkFlowID
   join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
   join CategoryTypeTable CT on T.CategoryType=Ct.categoryTypeName
   join UserRightValueView D on  case when b.ApprovalLocationTypeID=5 then 
	T.FromLocationL2 else T.ToLocationL2 end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID and D.CategoryTypeID=CT.CategoryTypeID
  where ApproveWorkFlowID=@ApprovalWorkFlowID and CT.statusID=@StausID and a.StatusID=@StausID
  and b.StatusID=@StausID and D.RightName=@RightName
  group by a.OrderNo,a.ApprovalRoleID

   if(select isnull(count(a.OrderNo),0) from  @validationTable a
  join ApproveWorkflowLineItemTable b on a.approvalroleID=b.ApprovalRoleID and a.OrderNo=b.OrderNo
  where b.ApproveWorkFlowID=@ApprovalWorkFlowID and b.StatusID=@StausID )!=@AprpovalCnt
  Begin 
     set @ErrorMsg ='UserRole not assigned to the mapped workflow user'
  end 

 
   if not exists(select * from @TransactionLineItemTable where CategoryType is not null and CategoryType!='') 
   begin 
   set @ErrorMsg =@ErrorMsg+ ' Category Type is not assigned for the selected asset category'
   end 
   if not exists(select * from @updateTable where updateRole=1)
   begin 
    set @ErrorMsg =@ErrorMsg+ ' Update option for the user is not enabled for the selected workflow '
   end 
     if (select updateRole from @updateTable group by updateRole having count(updateRole)>1)>1
   begin 
    set @ErrorMsg =@ErrorMsg+ ' Update option shouldnot be enabled for more than one approval user '
   end 

   Select @ErrorMsg as ErrorMsg
End 
go 
ALTER View [dbo].[nvwAssetTransfer_ForBeforeApproval]
as 
  select  a1.*,old.LocationName as OldLocationName,New.LocationName as NewLocationName,
  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubType,
ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
OldDept.DepartmentName as OldDepartmentName,NewDept.DepartmentName as NewDepartmentName,OldCate.CategoryName as OldCategoryName,
Oldprod.ProductName as OldProductName,newprod.ProductName as NewProductName,OldSec.SectionName as oldSectionName,newsec.SectionName as NewSectionName,

newCate.CategoryName as NewCategoryName,
 FORMATMESSAGE( dbo.fn_GetServerURL()+'AssetTransferApproval/EmailEdit?id=%d&UserID=%d' , ah.ApprovalHistoryID,p2.UserID) ApprovalURL,
--NULL ApprovalURL,
--Notification supporting fields
		A.TransactionID SYSDataID1, AH.ApprovalHistoryID SYSDataID2, '' SYSDataID3,p2.UserID as SYSUserID,
		p2.EMailID SYSToAddresses, '' SYSCCAddresses, '' SYSBCCAddresses,
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
  Left join UserRightValueView P2 on P2.ApprovalRoleID = AH.ApprovalRoleID  and RightName = 'AssetTransferApproval'
  and case when AD.ApprovalLocationTypeID=5 then 
		AH.FromLocationID else AH.ToLocationID end =P2.LocationID  and 
		case when CT.IsAllCategoryType=1 then P2.CategoryTypeID else AH.CategorytypeID end =P2.CategoryTypeID   
--  LEFT JOIN (SELECT ApprovalRoleID, --STRING_AGG(EmailID, ';') EMailIDs,LocationID
--stuff((select ';'+Emailid from UserRightValueView PL where PL.ApprovalRoleID=p2.ApprovalRoleID group by EMailID  FOR XML PATH('')), 1, 1, '') as EMailIDs--,
--  --LocationID

--			FROM UserRightValueView P2 where RightName = 'AssetTransferApproval'
--				AND ApprovalRoleID IS NOT NULL
--				AND EmailID IS NOT NULL
--			GROUP BY ApprovalRoleID) UGRP ON UGRP.ApprovalRoleID = AH.ApprovalRoleID
WHERE A.StatusID in (150,200) and a.TransactionTypeID in (5,11)

GO

ALTER View [dbo].[nvwAssetTransfer_ForNotification]
as 
  select  a1.*,old.LocationName as OldLocationName,New.LocationName as NewLocationName,
  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubType,
ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
OldDept.DepartmentName as OldDepartmentName,NewDept.DepartmentName as NewDepartmentName,OldCate.CategoryName as OldCategoryName,
Oldprod.ProductName as OldProductName,newprod.ProductName as NewProductName,OldSec.SectionName as oldSectionName,newsec.SectionName as NewSectionName,

newCate.CategoryName as NewCategoryName,	
 FORMATMESSAGE( dbo.fn_GetServerURL()+'AssetTransferApproval/EmailEdit?id=%d&UserID=%d' , ah.ApprovalHistoryID,p2.UserID) ApprovalURL,

--Notification supporting fields
		A.TransactionID SYSDataID1, AH.ApprovalHistoryID SYSDataID2, '' SYSDataID3,p2.UserID as SYSUserID,
		 
		  p2.EMailID SYSToAddresses,
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
   Left join UserRightValueView P2 on P2.ApprovalRoleID = AH.ApprovalRoleID  and RightName = 'AssetTransferApproval'
  and case when AD.ApprovalLocationTypeID=5 then 
		AH.FromLocationID else AH.ToLocationID end =P2.LocationID and 
		case when CT.IsAllCategoryType=1 then P2.CategoryTypeID else AH.CategorytypeID end =P2.CategoryTypeID   
--  LEFT JOIN (SELECT ApprovalRoleID, --STRING_AGG(EmailID, ';') EMailIDs,LocationID
--stuff((select ';'+Emailid from UserRightValueView PL where PL.ApprovalRoleID=p2.ApprovalRoleID group by EMailID  FOR XML PATH('')), 1, 1, '') as EMailIDs--,
--  --LocationID

--			FROM UserRightValueView P2 where RightName = 'AssetTransferApproval'
--				AND ApprovalRoleID IS NOT NULL
--				AND EmailID IS NOT NULL
--			GROUP BY ApprovalRoleID) UGRP ON UGRP.ApprovalRoleID = AH.ApprovalRoleID

WHERE  a.TransactionTypeID in (5,11)

GO
ALTER View [dbo].[vwNotificationTemplateObjects]
AS
	SELECT * 
		FROM  [vwSystemTemplateObjects]
		WHERE
			(ObjectName LIKE 'rvw%' OR ObjectName LIKE 'rprc%' OR ObjectName LIKE 'nvw%' ) 
			--and 
			--ObjectName not in (select QueryString from NotificationModuleTable where StatusID = dbo.fnGetActiveStatusID())
GO



ALTER View [dbo].[nvwAssetTransfer_ForNotification]
as 
  select  a1.*,old.LocationName as OldLocationName,New.LocationName as NewLocationName,
  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubType,
ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
OldDept.DepartmentName as OldDepartmentName,NewDept.DepartmentName as NewDepartmentName,OldCate.CategoryName as OldCategoryName,
Oldprod.ProductName as OldProductName,newprod.ProductName as NewProductName,OldSec.SectionName as oldSectionName,newsec.SectionName as NewSectionName,

newCate.CategoryName as NewCategoryName,
case when ah.ApproveModuleID=5 then 
 FORMATMESSAGE( dbo.fn_GetServerURL()+'AssetTransferApproval/EmailEdit?id=%d&UserID=%d' , ah.ApprovalHistoryID,p2.UserID)
 else  FORMATMESSAGE( dbo.fn_GetServerURL()+'InternalAssetTransferApproval/EmailEdit?id=%d&UserID=%d' , ah.ApprovalHistoryID,p2.UserID)
 end 
 ApprovalURL,

--Notification supporting fields
		A.TransactionID SYSDataID1, AH.ApprovalHistoryID SYSDataID2, '' SYSDataID3,p2.UserID as SYSUserID,
		 
		  p2.EMailID SYSToAddresses,
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
   Left join UserRightValueView P2 on P2.ApprovalRoleID = AH.ApprovalRoleID  and RightName = 'AssetTransferApproval'
  and case when AD.ApprovalLocationTypeID=5 then 
		AH.FromLocationID else AH.ToLocationID end =P2.LocationID and 
		case when CT.IsAllCategoryType=1 then P2.CategoryTypeID else AH.CategorytypeID end =P2.CategoryTypeID   and p2.EMailID is not null
--  LEFT JOIN (SELECT ApprovalRoleID, --STRING_AGG(EmailID, ';') EMailIDs,LocationID
--stuff((select ';'+Emailid from UserRightValueView PL where PL.ApprovalRoleID=p2.ApprovalRoleID group by EMailID  FOR XML PATH('')), 1, 1, '') as EMailIDs--,
--  --LocationID

--			FROM UserRightValueView P2 where RightName = 'AssetTransferApproval'
--				AND ApprovalRoleID IS NOT NULL
--				AND EmailID IS NOT NULL
--			GROUP BY ApprovalRoleID) UGRP ON UGRP.ApprovalRoleID = AH.ApprovalRoleID

WHERE  a.TransactionTypeID in (5,11)

GO



ALTER View [dbo].[nvwAssetTransfer_ForAfterApproval]
as 
Select a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubType,
ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
a1.*,P.EMailID as SYSToAddresses, '' SYSCCAddresses, '' SYSBCCAddresses,
		NULL SYSToMobileNos, NULL SYSWhatsAppMobileNos,A.TransactionID SYSDataID1, NULL SYSDataID2, NULL SYSDataID3,old.LocationName as OldLocationName,New.LocationName as NewLocationName,
		Room.LocationName as RoomName,
		CAse when a.TransactionTypeID=5 then 
		FORMATMESSAGE( dbo.fn_GetServerURL()+'AssetTransferApproval/EmailView?id=%d' , a.TransactionID) else FORMATMESSAGE( dbo.fn_GetServerURL()+'InternalAssetTransferApproval/EmailView?id=%d' , a.TransactionID) end   ApprovalURL
  From TransactionTable a 
  join TransactionLineItemTable b on a.TransactionID=b.TransactionID 
  join AssetNewView a1 on b.AssetID=a1.AssetID 
   --left join ApprovalHistoryTable AH on a.TransactionID=AH.TransactionID and ah.StatusID = dbo.fnGetActiveStatusID()
   Left join PersonTable p on a.CreatedBy=p.PersonID and p.EMailID is not null
    left join LocationTable Old on b.FromLocationID=Old.LocationID
	left join LocationTable new on b.ToLocationID=new.LocationID
	Left join LocationTable Room on b.RoomID=Room.LocationID
	where  a.TransactionTypeID in (5,11)
GO

ALTER View [dbo].[nvwAssetTransfer_ForBeforeApproval]
as 
  select  a1.*,old.LocationName as OldLocationName,New.LocationName as NewLocationName,
  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubType,
ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
OldDept.DepartmentName as OldDepartmentName,NewDept.DepartmentName as NewDepartmentName,OldCate.CategoryName as OldCategoryName,
Oldprod.ProductName as OldProductName,newprod.ProductName as NewProductName,OldSec.SectionName as oldSectionName,newsec.SectionName as NewSectionName,

newCate.CategoryName as NewCategoryName,
 case when ah.ApproveModuleID=5 and ad.UpdateDestinationLocationsForTransfer=1   then dbo.fn_GetServerURL() else 
  case when ah.ApproveModuleID=11 then FORMATMESSAGE( dbo.fn_GetServerURL()+'InternalAssetTransferApproval/EmailEdit?id=%d&UserID=%d', ah.ApprovalHistoryID,p2.UserID)
  else FORMATMESSAGE( dbo.fn_GetServerURL()+'AssetTransferApproval/EmailEdit?id=%d&UserID=%d' , ah.ApprovalHistoryID,p2.UserID)
 
 end end   ApprovalURL,
--NULL ApprovalURL,
--Notification supporting fields
		A.TransactionID SYSDataID1, AH.ApprovalHistoryID SYSDataID2, '' SYSDataID3,p2.UserID as SYSUserID,
		p2.EMailID SYSToAddresses, '' SYSCCAddresses, '' SYSBCCAddresses,
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
  Left join UserRightValueView P2 on P2.ApprovalRoleID = AH.ApprovalRoleID  and RightName = 'AssetTransferApproval'
  and case when AD.ApprovalLocationTypeID=5 then 
		AH.FromLocationID else AH.ToLocationID end =P2.LocationID  and 
		case when CT.IsAllCategoryType=1 then P2.CategoryTypeID else AH.CategorytypeID end =P2.CategoryTypeID  and p2.EMailID is not null
--  LEFT JOIN (SELECT ApprovalRoleID, --STRING_AGG(EmailID, ';') EMailIDs,LocationID
--stuff((select ';'+Emailid from UserRightValueView PL where PL.ApprovalRoleID=p2.ApprovalRoleID group by EMailID  FOR XML PATH('')), 1, 1, '') as EMailIDs--,
--  --LocationID

--			FROM UserRightValueView P2 where RightName = 'AssetTransferApproval'
--				AND ApprovalRoleID IS NOT NULL
--				AND EmailID IS NOT NULL
--			GROUP BY ApprovalRoleID) UGRP ON UGRP.ApprovalRoleID = AH.ApprovalRoleID
WHERE A.StatusID in (150,200) and a.TransactionTypeID in (5,11)

GO
ALTER View [dbo].[nvwAssetRetirement_ForBeforeApproval]
as 
  select  a1.AssetCode,a1.Barcode,a1.CategoryName,a1.LocationName,a1.DepartmentName,a1.SectionDescription,a1.CustodianName,a1.AssetDescription,
  a1.AssetCondition,a1.suppliername,
  
  old.LocationName as OldLocationName,
  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubType,
ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
 FORMATMESSAGE( dbo.fn_GetServerURL()+'AssetRetirementApproval/EmailEdit?id=%d&UserID=%d' , ah.ApprovalHistoryID,p2.UserID)
 
   ApprovalURL,
--Notification supporting fields
		A.TransactionID SYSDataID1, AH.ApprovalHistoryID SYSDataID2, NULL SYSDataID3,p2.UserID as SYSUserID,
		p2.EMailID SYSToAddresses, '' SYSCCAddresses, '' SYSBCCAddresses,
		NULL SYSToMobileNos, NULL SYSWhatsAppMobileNos,b.DisposalValue, b.DisposalReferencesNo,b.DisposalRemarks,b.ProceedOfSales,b.CostOfRemoval,
		b.DisposalDate
		
  From TransactionTable a 
  join TransactionLineItemTable b on a.TransactionID=b.TransactionID 
  join AssetNewView a1 on b.AssetID=a1.AssetID 
  left join ApprovalHistoryTable AH on a.TransactionID=AH.TransactionID --and ah.StatusID=150
  left join LocationTable Old on b.FromLocationID=Old.LocationID
   Left join CategoryTypeTable CT on ah.CategoryTypeID=Ct.CategoryTypeID
 Left join ApprovalRoleTable AD on AH.ApprovalRoleID=AD.ApprovalRoleID and ad.statusID=1
  Left join UserRightValueView P2 on P2.ApprovalRoleID = AH.ApprovalRoleID  and RightName = 'AssetRetirementApproval'
  and case when AD.ApprovalLocationTypeID=5 then 
		AH.FromLocationID else AH.ToLocationID end =P2.LocationID  and 
		case when CT.IsAllCategoryType=1 then P2.CategoryTypeID else AH.CategorytypeID end =P2.CategoryTypeID   and p2.EMailID is not null 
  --LEFT JOIN (SELECT ApprovalRoleID, 
  ----STRING_AGG(EmailID, ';') EMailIDs,
  -- stuff((select ';'+Emailid from UserRightValueView PL where PL.ApprovalRoleID=p2.ApprovalRoleID group by EMailID  FOR XML PATH('')), 1, 1, '') as EMailIDs--,
  ----LocationID

		--	FROM UserRightValueView P2 where RightName = 'AssetRetirementApproval'
  
		--		AND ApprovalRoleID IS NOT NULL
		--		AND EmailID IS NOT NULL
		--	GROUP BY ApprovalRoleID) UGRP ON UGRP.ApprovalRoleID = AH.ApprovalRoleID
WHERE A.StatusID  in (150,200) and a.TransactionTypeID=10

GO




ALTER View [dbo].[nvwAssetTransfer_ForAfterApproval]
as 
Select a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubType,
ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
a1.*,P.EMailID as SYSToAddresses, '' SYSCCAddresses, '' SYSBCCAddresses,
		NULL SYSToMobileNos, NULL SYSWhatsAppMobileNos,A.TransactionID SYSDataID1, NULL SYSDataID2, NULL SYSDataID3,old.LocationName as OldLocationName,New.LocationName as NewLocationName,
		Room.LocationName as RoomName,
		CAse when a.TransactionTypeID=5 then 
		FORMATMESSAGE( dbo.fn_GetServerURL()+'AssetTransferApproval/EmailView?id=%d' , a.TransactionID) else FORMATMESSAGE( dbo.fn_GetServerURL()+'InternalAssetTransferApproval/EmailView?id=%d' , a.TransactionID) end   ApprovalURL,NULL as SYSUserID
  From TransactionTable a 
  join TransactionLineItemTable b on a.TransactionID=b.TransactionID 
  join AssetNewView a1 on b.AssetID=a1.AssetID 
   --left join ApprovalHistoryTable AH on a.TransactionID=AH.TransactionID and ah.StatusID = dbo.fnGetActiveStatusID()
   Left join PersonTable p on a.CreatedBy=p.PersonID and p.EMailID is not null
    left join LocationTable Old on b.FromLocationID=Old.LocationID
	left join LocationTable new on b.ToLocationID=new.LocationID
	Left join LocationTable Room on b.RoomID=Room.LocationID
	where  a.TransactionTypeID in (5,11)
GO


ALTER View [dbo].[nvwAssetRetirement_ForAfterApproval]
as 
select a1.AssetCode,a1.Barcode,a1.CategoryName,a1.LocationName,a1.DepartmentName,a1.SectionDescription,a1.CustodianName,a1.AssetDescription,
  a1.AssetCondition,a1.suppliername,
  
  old.LocationName as OldLocationName,
  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubType,
ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
FORMATMESSAGE( dbo.fn_GetServerURL()+'AssetRetirementApproval/EmailView?id=%d' , a.TransactionID)  ApprovalURL,
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

  where a.StatusID = dbo.fnGetActiveStatusID() and a.TransactionTypeID=10
GO



ALTER View [dbo].[nvwAssetRetirement_ForNotification]
as 
  select  a1.AssetCode,a1.Barcode,a1.CategoryName,a1.LocationName,a1.DepartmentName,a1.SectionDescription,a1.CustodianName,a1.AssetDescription,
  a1.AssetCondition,a1.suppliername,
  
  old.LocationName as OldLocationName,
  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubType,
ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
FORMATMESSAGE( dbo.fn_GetServerURL()+'AssetRetirementApproval/EmailEdit?id=%d&UserID=%d' , ah.ApprovalHistoryID,p2.UserID) ApprovalURL,
--Notification supporting fields
		A.TransactionID SYSDataID1, AH.ApprovalHistoryID SYSDataID2, NULL SYSDataID3,p2.UserID as SYSUserID,
		p2.EMailID SYSToAddresses, '' SYSCCAddresses, '' SYSBCCAddresses,
		NULL SYSToMobileNos, NULL SYSWhatsAppMobileNos,b.DisposalValue, b.DisposalReferencesNo,b.DisposalRemarks,b.ProceedOfSales,b.CostOfRemoval,
		b.DisposalDate,(SELECT EMAILID FROM PERSONTABLE WHERE PERSONID=AH.CREATEDBY) as ApprovedBy,convert(nvarchar(100),ah.CreatedDateTime,103) as ApprovedDate
		
  From TransactionTable a 
  join TransactionLineItemTable b on a.TransactionID=b.TransactionID 
  join AssetNewView a1 on b.AssetID=a1.AssetID 
  left join ApprovalHistoryTable AH on a.TransactionID=AH.TransactionID --and ah.StatusID=150
   Left join ApprovalRoleTable AD on AH.ApprovalRoleID=AD.ApprovalRoleID and ad. statusID=1
    Left join CategoryTypeTable CT on ah.CategoryTypeID=Ct.CategoryTypeID
  left join LocationTable Old on b.FromLocationID=Old.LocationID
  Left join UserRightValueView P2 on P2.ApprovalRoleID = AH.ApprovalRoleID  and RightName = 'AssetTransferApproval'
  and case when AD.ApprovalLocationTypeID=5 then 
		AH.FromLocationID else AH.ToLocationID end =P2.LocationID and 
		case when CT.IsAllCategoryType=1 then P2.CategoryTypeID else AH.CategorytypeID end =P2.CategoryTypeID   and p2.EMailID is not null
  --LEFT JOIN (SELECT ApprovalRoleID, 
  ----STRING_AGG(EmailID, ';') EMailIDs,
  -- stuff((select ';'+Emailid from UserRightValueView PL where PL.ApprovalRoleID=p2.ApprovalRoleID group by EMailID  FOR XML PATH('')), 1, 1, '') as EMailIDs--,
  ----LocationID

		--	FROM UserRightValueView P2 where RightName = 'AssetRetirementApproval'
  
		--		AND ApprovalRoleID IS NOT NULL
		--		AND EmailID IS NOT NULL
		--	GROUP BY ApprovalRoleID) UGRP ON UGRP.ApprovalRoleID = AH.ApprovalRoleID
WHERE a.TransactionTypeID=10

GO


ALTER Procedure [dbo].[prc_ValidateForTransaction]
(
   @FromAssetID				nvarchar(max)	 = NULL,
   @ToLocationID			int				 = NULL,
   @FromLocationTypeID		int				 = NULL,
   @ToLocationTypeID		Int				 = NULL,
   @ApprovalWorkFlowID		int              = NULL
)
as 
Begin
  Declare @ErrorMsg nvarchar(max),@UpdateCount int,@AprpovalCnt int ,@StausID int,@RightName nvarchar(100),@modelID int 
  select @StausID=[dbo].fnGetActiveStatusID()
  SElect @modelID=ApproveModuleID from ApproveWorkflowTable where ApproveWorkflowID=@ApprovalWorkFlowID
  select @RightName=case when @modelID=5  then 'AssetTransferApproval' when @modelID=11 then 'InternalAssetTransferApproval' else 'AssetRetirementApproval' end 
  Declare @updateTable table(updateRole bit,ApprovalRoleID int)


  declare @TransactionLineItemTable table (AssetID int,FromLocationL2 int ,ToLocationL2 int,
  LocationType nvarchar(100),CategoryType nvarchar(100),ApprovalWorkFlowID int )
  
  Select @AprpovalCnt=count(OrderNo) from 
  ApproveWorkflowTable a join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
  where a.StatusID=1 and b.StatusID=1 and a.ApproveWorkflowID=@ApprovalWorkFlowID

  --print @AprpovalCnt
  --Declare @ApprovalUserTable Table ()
  insert into @TransactionLineItemTable(AssetID,FromLocationL2,ToLocationL2,LocationType,CategoryType,ApprovalWorkFlowID)
  select AssetID,LocationL2,@ToLocationID,LocationType,categorytype,@ApprovalWorkFlowID
  From AssetNewView where assetid in (select value from Split(@fromAssetID,',')) 

  --select * from @TransactionLineItemTable

  insert into @updateTable(updateRole,ApprovalRoleID)
  Select case when a.ApproveModuleID=5 or a.ApproveModuleID=11 then UpdateDestinationLocationsForTransfer else UpdateRetirementDetailsForEachAssets end as updateRole, 
  b.ApprovalRoleID FRom ApproveWorkflowTable a 
	  join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
	  join ApprovalRoleTable C on b.ApprovalRoleID=C.ApprovalRoleID
	  where a.ApproveWorkflowID=@ApprovalWorkFlowID and a.StatusID=@StausID and b.StatusID=@StausID and c.StatusID=@StausID
  --select * from @updateTable
  
  Declare @validationTable Table(OrderNo int,ApprovalRoleID int) 
	
	insert into @validationTable(OrderNo,ApprovalRoleID)
 select a.OrderNo,a.ApprovalRoleID from ApproveWorkflowLineItemTable a 
   join @TransactionLineItemTable T on a.ApproveWorkFlowID=T.ApprovalWorkFlowID
   join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
   join CategoryTypeTable CT on T.CategoryType=Ct.categoryTypeName
   join UserApprovalRoleMappingTable D on  case when b.ApprovalLocationTypeID=5 then 
	T.FromLocationL2 else T.ToLocationL2 end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID and D.CategoryTypeID=CT.CategoryTypeID
  where ApproveWorkFlowID=@ApprovalWorkFlowID and CT.statusID=@StausID and a.StatusID=@StausID
  and b.StatusID=@StausID and D.StatusID=@StausID
  group by a.OrderNo,a.ApprovalRoleID
  --select * from @validationTable
    if(select isnull(count(a.OrderNo),0) from  @validationTable a
  join ApproveWorkflowLineItemTable b on a.approvalroleID=b.ApprovalRoleID and a.OrderNo=b.OrderNo
  where b.ApproveWorkFlowID=@ApprovalWorkFlowID and b.StatusID=@StausID )!=@AprpovalCnt
  Begin 
     set @ErrorMsg ='Approval role not assigned to the user'
  end 

  insert into @validationTable(OrderNo,ApprovalRoleID)
   select a.OrderNo,a.ApprovalRoleID from ApproveWorkflowLineItemTable a 
   join @TransactionLineItemTable T on a.ApproveWorkFlowID=T.ApprovalWorkFlowID
   join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
   join CategoryTypeTable CT on T.CategoryType=Ct.categoryTypeName
   join UserRightValueView D on  case when b.ApprovalLocationTypeID=5 then 
	T.FromLocationL2 else T.ToLocationL2 end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID and D.CategoryTypeID=CT.CategoryTypeID
  where ApproveWorkFlowID=@ApprovalWorkFlowID and CT.statusID=@StausID and a.StatusID=@StausID
  and b.StatusID=@StausID and D.RightName=@RightName
  group by a.OrderNo,a.ApprovalRoleID

   if(select isnull(count(a.OrderNo),0) from  @validationTable a
  join ApproveWorkflowLineItemTable b on a.approvalroleID=b.ApprovalRoleID and a.OrderNo=b.OrderNo
  where b.ApproveWorkFlowID=@ApprovalWorkFlowID and b.StatusID=@StausID )!=@AprpovalCnt
  Begin 
     set @ErrorMsg ='UserRole not assigned to the mapped workflow user'
  end 

  insert into @validationTable(OrderNo,ApprovalRoleID)
   select a.OrderNo,a.ApprovalRoleID from ApproveWorkflowLineItemTable a 
   join @TransactionLineItemTable T on a.ApproveWorkFlowID=T.ApprovalWorkFlowID
   join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
   join CategoryTypeTable CT on T.CategoryType=Ct.categoryTypeName
   join UserRightValueView D on  case when b.ApprovalLocationTypeID=5 then 
	T.FromLocationL2 else T.ToLocationL2 end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID and D.CategoryTypeID=CT.CategoryTypeID
  where ApproveWorkFlowID=@ApprovalWorkFlowID and CT.statusID=@StausID and a.StatusID=@StausID and D.EMailID is not null
  and b.StatusID=@StausID and D.RightName=@RightName
  group by a.OrderNo,a.ApprovalRoleID

   if(select isnull(count(a.OrderNo),0) from  @validationTable a
  join ApproveWorkflowLineItemTable b on a.approvalroleID=b.ApprovalRoleID and a.OrderNo=b.OrderNo
  where b.ApproveWorkFlowID=@ApprovalWorkFlowID and b.StatusID=@StausID )!=@AprpovalCnt
  Begin 
     set @ErrorMsg ='EmailID not assigned to the mapped workflow user'
  end 

   if not exists(select * from @TransactionLineItemTable where CategoryType is not null and CategoryType!='') 
   begin 
   set @ErrorMsg =@ErrorMsg+ ' Category Type is not assigned for the selected asset category'
   end 
   if not exists(select * from @updateTable where updateRole=1)
   begin 
    set @ErrorMsg =@ErrorMsg+ ' Update option for the user is not enabled for the selected workflow '
   end 
     if (select updateRole from @updateTable group by updateRole having count(updateRole)>1)>1
   begin 
    set @ErrorMsg =@ErrorMsg+ ' Update option shouldnot be enabled for more than one approval user '
   end 

   Select @ErrorMsg as ErrorMsg
End 
go 


ALTER Procedure [dbo].[prc_GetSecondLevelLocationValue]
(
@LocationID int,
@AssetIDS nvarchar(max)
)
as 
begin 
declare @L2ID int,@loc int,@parentLoc int,@toparentid int,@returnMsg nvarchar(max) 
--select @level=Level from LocationNewHierarchicalView where ChildID=@FromLocationID
Select @loc=LocationID from AssetTable where assetid in (select value from [dbo].Split(@AssetIDS,','))
group by LocationID
select @parentLoc=ParentLocationID from LocationTable where locationid=@loc
select @toparentid=ParentLocationID from LocationTable where locationid=@LocationID

if(@parentLoc!=@toparentid)
 --select @L2ID=Level2ID from LocationNewHierarchicalView where ChildID=@LocationID
 --select @L2ID as SecondLevelID
 set @returnMsg='Selected Location Parent Level not matched'
end 
select @returnMsg as returnMsg
go
ALTER Procedure [dbo].[prc_ValidateForTransaction]
(
   @FromAssetID				nvarchar(max)	 = NULL,
   @ToLocationID			int				 = NULL,
   @FromLocationTypeID		int				 = NULL,
   @ToLocationTypeID		Int				 = NULL,
   @ApprovalWorkFlowID		int              = NULL
)
as 
Begin
  Declare @ErrorMsg nvarchar(max),@UpdateCount int,@AprpovalCnt int ,@StausID int,@RightName nvarchar(100),@modelID int 
  select @StausID=[dbo].fnGetActiveStatusID()
  SElect @modelID=ApproveModuleID from ApproveWorkflowTable where ApproveWorkflowID=@ApprovalWorkFlowID
  select @RightName=case when @modelID=5  then 'AssetTransferApproval' when @modelID=11 then 'InternalAssetTransferApproval' else 'AssetRetirementApproval' end 
  Declare @updateTable table(updateRole bit,ApprovalRoleID int)
  if @modelID=11 
  begin
    select @ToLocationID =Level2id from LocationNewHierarchicalView where ChildID=@ToLocationID
  end

  declare @TransactionLineItemTable table (AssetID int,FromLocationL2 int ,ToLocationL2 int,
  LocationType nvarchar(100),CategoryType nvarchar(100),ApprovalWorkFlowID int )
  
  Select @AprpovalCnt=count(OrderNo) from 
  ApproveWorkflowTable a join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
  where a.StatusID=1 and b.StatusID=1 and a.ApproveWorkflowID=@ApprovalWorkFlowID

  --print @AprpovalCnt
  --Declare @ApprovalUserTable Table ()
  insert into @TransactionLineItemTable(AssetID,FromLocationL2,ToLocationL2,LocationType,CategoryType,ApprovalWorkFlowID)
  select AssetID,LocationL2,@ToLocationID,LocationType,categorytype,@ApprovalWorkFlowID
  From AssetNewView where assetid in (select value from Split(@fromAssetID,',')) 

  --select * from @TransactionLineItemTable

  insert into @updateTable(updateRole,ApprovalRoleID)
  Select case when a.ApproveModuleID=5 or a.ApproveModuleID=11 then UpdateDestinationLocationsForTransfer else UpdateRetirementDetailsForEachAssets end as updateRole, 
  b.ApprovalRoleID FRom ApproveWorkflowTable a 
	  join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
	  join ApprovalRoleTable C on b.ApprovalRoleID=C.ApprovalRoleID
	  where a.ApproveWorkflowID=@ApprovalWorkFlowID and a.StatusID=@StausID and b.StatusID=@StausID and c.StatusID=@StausID
  --select * from @updateTable
  
  Declare @validationTable Table(OrderNo int,ApprovalRoleID int) 
	  Declare @validationTable1 Table(OrderNo int,ApprovalRoleID int) 
  Declare @validationTable2 Table(OrderNo int,ApprovalRoleID int) 

	insert into @validationTable(OrderNo,ApprovalRoleID)
 select a.OrderNo,a.ApprovalRoleID from ApproveWorkflowLineItemTable a 
   join @TransactionLineItemTable T on a.ApproveWorkFlowID=T.ApprovalWorkFlowID
   join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
   join CategoryTypeTable CT on T.CategoryType=Ct.categoryTypeName
   join UserApprovalRoleMappingTable D on  case when b.ApprovalLocationTypeID=5 then 
	T.FromLocationL2 else T.ToLocationL2 end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID and D.CategoryTypeID=CT.CategoryTypeID
  where ApproveWorkFlowID=@ApprovalWorkFlowID and CT.statusID=@StausID and a.StatusID=@StausID
  and b.StatusID=@StausID and D.StatusID=@StausID
  group by a.OrderNo,a.ApprovalRoleID
  --select * from @validationTable
  --print @AprpovalCnt
    if(select isnull(count(a.OrderNo),0) from  @validationTable a
  join ApproveWorkflowLineItemTable b on a.approvalroleID=b.ApprovalRoleID and a.OrderNo=b.OrderNo
  where b.ApproveWorkFlowID=@ApprovalWorkFlowID and b.StatusID=@StausID )!=@AprpovalCnt
  Begin 
     set @ErrorMsg ='Approval role not assigned to the user'
  end 

  insert into @validationTable1(OrderNo,ApprovalRoleID)
   select a.OrderNo,a.ApprovalRoleID from ApproveWorkflowLineItemTable a 
   join @TransactionLineItemTable T on a.ApproveWorkFlowID=T.ApprovalWorkFlowID
   join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
   join CategoryTypeTable CT on T.CategoryType=Ct.categoryTypeName
   join UserRightValueView D on  case when b.ApprovalLocationTypeID=5 then 
	T.FromLocationL2 else T.ToLocationL2 end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID and D.CategoryTypeID=CT.CategoryTypeID
  where ApproveWorkFlowID=@ApprovalWorkFlowID and CT.statusID=@StausID and a.StatusID=@StausID
  and b.StatusID=@StausID and D.RightName=@RightName
  group by a.OrderNo,a.ApprovalRoleID

   if(select isnull(count(a.OrderNo),0) from  @validationTable1 a
  join ApproveWorkflowLineItemTable b on a.approvalroleID=b.ApprovalRoleID and a.OrderNo=b.OrderNo
  where b.ApproveWorkFlowID=@ApprovalWorkFlowID and b.StatusID=@StausID )!=@AprpovalCnt
  Begin 
     set @ErrorMsg =@ErrorMsg+' UserRole not assigned to the mapped workflow user'
  end 

  insert into @validationTable2(OrderNo,ApprovalRoleID)
   select a.OrderNo,a.ApprovalRoleID from ApproveWorkflowLineItemTable a 
   join @TransactionLineItemTable T on a.ApproveWorkFlowID=T.ApprovalWorkFlowID
   join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
   join CategoryTypeTable CT on T.CategoryType=Ct.categoryTypeName
   join UserApprovalRoleMappingTable D on  case when b.ApprovalLocationTypeID=5 then 
	T.FromLocationL2 else T.ToLocationL2 end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID and D.CategoryTypeID=CT.CategoryTypeID
  join persontable p on D.userID=p.personID
   where ApproveWorkFlowID=@ApprovalWorkFlowID and CT.statusID=@StausID and a.StatusID=@StausID  and p.EMailID is not null
  and b.StatusID=@StausID --and D.RightName=@RightName
  group by a.OrderNo,a.ApprovalRoleID

  --select * from  @validationTable2
  --print @AprpovalCnt
   if (select isnull(count(a.OrderNo),0) from @validationTable1 a join ApproveWorkflowLineItemTable b on a.approvalroleID=b.ApprovalRoleID and a.OrderNo=b.OrderNo
  where b.ApproveWorkFlowID=@ApprovalWorkFlowID and b.StatusID=@StausID )=@AprpovalCnt
   begin 
   if(select isnull(count(a.OrderNo),0) from  @validationTable2 a
  join ApproveWorkflowLineItemTable b on a.approvalroleID=b.ApprovalRoleID and a.OrderNo=b.OrderNo
  where b.ApproveWorkFlowID=@ApprovalWorkFlowID and b.StatusID=@StausID )!=@AprpovalCnt
  Begin 
     set @ErrorMsg =@ErrorMsg+' EmailID not assigned to the mapped workflow user'
  end 
  End 

   if not exists(select * from @TransactionLineItemTable where CategoryType is not null and CategoryType!='') 
   begin 
   set @ErrorMsg =@ErrorMsg+ ' Category Type is not assigned for the selected asset category'
   end 
   if not exists(select * from @updateTable where updateRole=1)
   begin 
    set @ErrorMsg =@ErrorMsg+ ' Update option for the user is not enabled for the selected workflow '
   end 
     if (select updateRole from @updateTable group by updateRole having count(updateRole)>1)>1
   begin 
    set @ErrorMsg =@ErrorMsg+ ' Update option shouldnot be enabled for more than one approval user '
   end 

   Select @ErrorMsg as ErrorMsg
End 
go 

ALTER Trigger [dbo].[trg_AlertDataInputTable_Upt] On [dbo].[ApprovalHistoryTable]
After update 
as 
Begin 
   Declare @NotificationTemplateID int,@moduleID int,@PrimaryID int,@referenceID int,@OrderID int,@CreatedDatetime smallDateTime,@createdBy int,
   @NextOrderID int ,@maxOrderID int,@RightName nvarchar(100),@statusID int 

  select @StatusID= [dbo].fnGetActiveStatusID()
   
   if exists(select i.statusID from inserted i join Deleted D on i.ApprovalHistoryID=D.ApprovalHistoryID  
			where i.StatusID = dbo.fnGetActiveStatusID() and d.StatusID=150) 
	Begin 
		select @moduleID=i.ApproveModuleID,@PrimaryID=i.ApprovalHistoryID,@referenceID=i.TransactionID,@OrderID=i.OrderNo,@CreatedDatetime=i.CreatedDateTime,@createdBy=i.CreatedBy
	         From inserted i join Deleted D on i.ApprovalHistoryID=D.ApprovalHistoryID  where i.StatusID = dbo.fnGetActiveStatusID() and d.StatusID=150   
			
			select @maxOrderID=max(OrderNo) from ApprovalHistoryTable where TransactionID=@referenceID and ApproveModuleID=@moduleID
			
			--requestor notification
			 IF @moduleID=5 or @moduleID=11 
				begin 
				   select @NotificationTemplateID= NotificationTemplateID from NotificationTemplateTable where 
				   TemplateCode='AssetTransferRequestorNotification'
				    set @RightName=case when @moduleID=5 then 'AssetTransfer' else 'InternalAssetTransfer' end 
				end 
			   IF @moduleID=10
				begin 
				   select @NotificationTemplateID= NotificationTemplateID from NotificationTemplateTable where 
				   TemplateCode='AssetRetirementRequestorNotification'
				    SET @RightName='AssetRetirement'
				end 
			if @NotificationTemplateID is not null and @NotificationTemplateID!=''
				   begin 
				   print '1'
				   Insert into NotificationInputTable(NotificationTemplateID,SYSDataID1,SYSDataID2,SYSDataID3,SYSUserID)
				   values( @NotificationTemplateID,@referenceID,null,null,null)
				   End 
		   --Level wise notification
			 If @OrderID>1
			 Begin 
			 IF @moduleID=5 or @moduleID=11 
				begin 
				   select @NotificationTemplateID= NotificationTemplateID from NotificationTemplateTable where 
				   TemplateCode='AssetTransferNotification'
				    set @RightName=case when @moduleID=5 then 'AssetTransferApproval' else 'InternalAssetTransferApproval' end 
				end 
			   IF @moduleID=10
				begin 
				   select @NotificationTemplateID= NotificationTemplateID from NotificationTemplateTable where 
				   TemplateCode='AssetRetirementNotification'
				    SET @RightName='AssetRetirementApproval'
				end 
				if @NotificationTemplateID is not null and @NotificationTemplateID!=''
				   begin 
				   print @NotificationTemplateID
				   print @RightName
				   print @ORDERID
				   print @maxOrderID
				   print @referenceID
				   print @moduleID
				   Insert into NotificationInputTable(NotificationTemplateID,SYSDataID1,SYSDataID2,SYSDataID3,SYSUserID)
				   select @NotificationTemplateID,TransactionID,ApprovalHistoryID,NULL,d.UserID
				      from ApprovalHistoryTable a 
				   Left join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
				   Left join CategoryTypeTable CT on a.CategoryTypeID=Ct.CategoryTypeID
				   Left join UserRightValueView D on  case when b.ApprovalLocationTypeID=5 then 
					a.FromLocationID else a.ToLocationID end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID and 
					 case when CT.IsAllCategoryType=1 then D.CategoryTypeID else a.CategorytypeID end =D.CategoryTypeID  and RightName=@RightName
				  where  CT.statusID=@StatusID and b.StatusID=@StatusID and a.StatusID=dbo.fnGetActiveStatusID() and TransactionID=@referenceID and ApproveModuleID=@moduleID 
					    AND  ORDERNO<= CASE WHEN @maxOrderID=@OrderID THEN @ORDERID ELSE @ORDERID-1 END
					  
					  --where StatusID=dbo.fnGetActiveStatusID() and TransactionID=@referenceID and ApproveModuleID=@moduleID 
					  --  AND  ORDERNO>= CASE WHEN @maxOrderID=@OrderID THEN @ORDERID ELSE @ORDERID-1 END 
					 --and ApprovalHistoryID!=@PrimaryID
				   
				  
				  End 
		    End 
		  
		 
	  IF EXISTS(select OrderNo From ApprovalHistoryTable where OrderNo=@OrderID+1 and StatusID=150 and ApproveModuleID=@moduleID and TransactionID=@referenceID
	        and CreatedDateTime=@CreatedDatetime)
		Begin 
		print 'welocme'
		    set @NotificationTemplateID=NULL

		    select @moduleID=i.ApproveModuleID,@PrimaryID=i.ApprovalHistoryID,@referenceID=i.TransactionID,@NextOrderID=i.OrderNo,@CreatedDatetime=i.CreatedDateTime
		     From ApprovalHistoryTable i where OrderNo=@OrderID+1 and StatusID=150 and ApproveModuleID=@moduleID and TransactionID=@referenceID
	         and CreatedDateTime=@CreatedDatetime
		    
			--next wise notification
				 IF @moduleID=5 or @moduleID=11 
				begin 
				print 'welocme12'
				   select @NotificationTemplateID= NotificationTemplateID from NotificationTemplateTable where 
				   TemplateCode='AssetTransferBeforeApproval'
				    SET @RightName=case when @moduleID=5 then 'AssetTransferApproval' else 'InternalAssetTransferApproval' end
				end 
			   IF @moduleID=10
				begin 
				   select @NotificationTemplateID= NotificationTemplateID from NotificationTemplateTable where 
				   TemplateCode='AssetRetirementBeforeApproval'
				   Set @RightName='AssetRetirementApproval'
				end 

			   if @NotificationTemplateID is not null and @NotificationTemplateID!=''
			   begin 
			   print 'welocme12next'
			   print @PrimaryID
			   print @statusID
			   print @RightName
			   print @NotificationTemplateID
			   print @referenceID
			   Insert into NotificationInputTable(NotificationTemplateID,SYSDataID1,SYSDataID2,SYSDataID3,SYSUserID)
			    select @NotificationTemplateID,@referenceID,@PrimaryID,NULL,D.UserID
				   from ApprovalHistoryTable a 
				   Left join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
				   Left join CategoryTypeTable CT on a.CategoryTypeID=Ct.CategoryTypeID
				   Left join UserRightValueView D on  case when b.ApprovalLocationTypeID=5 then 
					a.FromLocationID else a.ToLocationID end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID and 
					 case when CT.IsAllCategoryType=1 then D.CategoryTypeID else a.CategorytypeID end =D.CategoryTypeID  and RightName=@RightName
				  where ApprovalHistoryID=@PrimaryID and CT.statusID=@StatusID and b.StatusID=@StatusID --and d.StatusID=@StatusID
			   --values(@NotificationTemplateID,@referenceID,@PrimaryID,NULL)
			   End 
	   End
    End 
	Else 
	Begin 
	   if exists(select i.statusID from inserted i join Deleted D on i.ApprovalHistoryID=D.ApprovalHistoryID  where i.StatusID=200 and d.StatusID=150)
	   Begin 
	     select @moduleID=ApproveModuleID,@PrimaryID=ApprovalHistoryID,@referenceID=TransactionID,@createdBy=CreatedBy
	               from inserted 
	     select @maxOrderID=max(OrderNo) from ApprovalHistoryTable where TransactionID=@referenceID and ApproveModuleID=@moduleID
		--rejected notification

			IF @moduleID=5 or @moduleID=11
			begin 
			   select @NotificationTemplateID= NotificationTemplateID from NotificationTemplateTable where 
			   TemplateCode='AssetTransferRejected'
			   SET @RightName=case when @moduleID=5 then 'AssetTransfer' else 'InternalAssetTransfer' end
			end 
		   IF @moduleID=10
			begin 
			   select @NotificationTemplateID= NotificationTemplateID from NotificationTemplateTable where 
			   TemplateCode='AssetRetirementRejected'
			   SET @RightName='AssetRetirement'
			end 

	   if @NotificationTemplateID is not null and @NotificationTemplateID!=''
	   begin 
		   Insert into NotificationInputTable(NotificationTemplateID,SYSDataID1,SYSDataID2,SYSDataID3,SYSUserID)
		   values(@NotificationTemplateID,@referenceID,null,NULL,NULL)
		 --  select @NotificationTemplateID,@referenceID,@PrimaryID,NULL,D.UserID
		 --  from inserted a 
		 --  Left join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
		 --  Left join CategoryTypeTable CT on a.CategoryTypeID=Ct.CategoryTypeID
		 --  Left join UserRightValueView D on  case when b.ApprovalLocationTypeID=5 then 
			--a.FromLocationID else a.ToLocationID end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID and 
			-- case when CT.IsAllCategoryType=1 then D.CategoryTypeID else a.CategorytypeID end =D.CategoryTypeID  and RightName=@RightName
		 -- where ApprovalHistoryID=@PrimaryID and CT.statusID=@StatusID and b.StatusID=@StatusID --and d.StatusID=@StatusID
	   End 

	  
		   --Level wise notification
			  If @OrderID>1
			 Begin 
			 IF @moduleID=5 or @moduleID=11 
				begin 
				   select @NotificationTemplateID= NotificationTemplateID from NotificationTemplateTable where 
				   TemplateCode='AssetTransferFailedNotification'
				   Set @RightName=case when @moduleID=5 then 'AssetTransferApproval' else 'InternalAssetTransferApproval' end 
				end 
			   IF @moduleID=10
				begin 
				   select @NotificationTemplateID= NotificationTemplateID from NotificationTemplateTable where 
				   TemplateCode='AssetRetirementFailedNotification'
				   set @RightName='AssetRetirementApproval'
				end 
				if @NotificationTemplateID is not null and @NotificationTemplateID!=''
				   begin 
				   Insert into NotificationInputTable(NotificationTemplateID,SYSDataID1,SYSDataID2,SYSDataID3,SYSUserID)
				   select @NotificationTemplateID,TransactionID,ApprovalHistoryID,NULL,D.UserID
				   from ApprovalHistoryTable a 
				   Left join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
				   Left join CategoryTypeTable CT on a.CategoryTypeID=Ct.CategoryTypeID
				   Left join UserRightValueView D on  case when b.ApprovalLocationTypeID=5 then 
					a.FromLocationID else a.ToLocationID end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID and 
					 case when CT.IsAllCategoryType=1 then D.CategoryTypeID else a.CategorytypeID end =D.CategoryTypeID  and RightName=@RightName
				  where  CT.statusID=@StatusID and b.StatusID=@StatusID and a.StatusID=dbo.fnGetActiveStatusID() and TransactionID=@referenceID and ApproveModuleID=@moduleID 
					    AND  ORDERNO<= CASE WHEN @maxOrderID=@OrderID THEN @ORDERID ELSE @ORDERID-1 END
				   --   from ApprovalHistoryTable where StatusID=dbo.fnGetActiveStatusID()  and TransactionID=@referenceID and ApproveModuleID=@moduleID 
					  --AND  ORDERNO>= CASE WHEN @maxOrderID=@OrderID THEN @ORDERID ELSE @ORDERID-1 END 
					  ----and ApprovalHistoryID!=@PrimaryID
				   End 
		    End
		  
	 

	   End 
	End 
	  
   
End 

go 

ALTER Trigger [dbo].[trg_Ins_LocationCodeAutoGeneration] on [dbo].[LocationTable] 
for Insert
As
Begin 
print 'a'
Declare @LocationID int
select @LocationID=LocationID from Inserted 
if exists(select IsEnableTigger from systemconfigurationtable where IsEnableTigger=1)
Begin 
exec [Prc_InsertTempLocation] @LocationID
end 
 --Declare @CodeTable Table(code nvarchar(50))
 --Declare @prefix as nvarchar(50)
 -- Declare @UserLocationMapping nvarchar(10)
 --select @prefix= ConfiguarationValue  from ConfigurationTable (nolock) where ConfiguarationName='MasterCodeAutoGenerate'
	-- if @prefix='true'
	-- Begin 
	--	IF Exists(select LocationCode from INSERTED where LocationCode='-')
	--	Begin 	
	--	  insert into @CodeTable
	--		 exec [dbo].[prc_GetAutoCodeGeneration] 'Location',1
			 
	--	  Update LocationTable set LocationCode=(select code from @CodeTable) where LocationID=(select LocationId from INSERTED)
		  
	--	  IF Exists(select CodeName from CodeConfigurationTable where CodeName='Location')
	--	   Begin 
	--	      update CodeConfigurationTable set CodeValue=CodeValue+1 where CodeName='Location'
	--	   End
	--	End
	--End 
	--Select @UserLocationMapping= ConfiguarationValue from ConfigurationTable where ConfiguarationName='UserLocationMapping'
	--	IF UPPER(@UserLocationMapping)='TRUE'
	--BEGIN 
	--     Declare @LocationID int ,@Createdby int ,@ParentLocationID int 
	--	 select @LocationID=LocationID,@Createdby=CreatedBy,@ParentLocationID=ParentlocationID from Inserted 
	--	print @LocationID
	--	print @Createdby
	--	print @ParentLocationID
	--	 If @ParentLocationID is null or @ParentLocationID='' 
	--	 Begin 
	--	     IF not exists(select LocationID from UserLocationMappingTable where LocationID=@LocationID and PersonID=@Createdby)
	--		 Begin
	--		   Insert into UserLocationMappingTable(PersonID,LocationID,StatusID)
	--		   values(@Createdby,@LocationID,1)
	--		 End 
	--	 End 
	--	 Else 
	--	 Begin 
		  
		
	--	 Select @ParentLocationID=FirstLevel from LocationHierarchicalView where childID=@LocationID and LanguageID=1
	--	 IF not exists(select LocationID from UserLocationMappingTable where LocationID=@ParentLocationID and PersonID=@Createdby)
	--	 Begin 
	--		Insert into UserLocationMappingTable(PersonID,LocationID,StatusID)
	--		values(@Createdby,@ParentLocationID,1)
			 
	--	 end 
		
	--END 
	-- End 
End 
go 


ALTER Trigger  [dbo].[trg_Update_tmpLocationView] on [dbo].[LocationTable] 
for UPDATE
As
Begin 
    print '1'
	Declare @LocationID int
	Select @LocationID=LocationID from inserted
	if exists(select IsEnableTigger from systemconfigurationtable where IsEnableTigger=1)
Begin
	If update(LocationCode) OR update(StatusID) or update(LocationName) OR update(LocationtypeID)
	 Begin 
		  exec [Prc_InsertTempLocation] @LocationID
	 End 
	 End 
END 
go 

ALTER View [dbo].[TransactionLineItemViewForTransfer]
  as 
-- select * into tmp_LocationNewHierarchicalView from LocationNewHierarchicalView

  Select a1.*,old.PID4 as OldLocationName,New.PID4 as NewLocationName,
  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubType,
ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,Remarks,
TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
OldDept.DepartmentName as OldDepartmentName,NewDept.DepartmentName as NewDepartmentName,OldCate.CategoryName as OldCategoryName,
Oldprod.ProductName as OldProductName,newprod.ProductName as NewProductName,OldSec.SectionName as oldSectionName,newsec.SectionName as NewSectionName,

newCate.CategoryName as NewCategoryName
  From TransactionTable a 
  join TransactionLineItemTable b on a.TransactionID=b.TransactionID 
  join AssetNewView a1 on b.AssetID=a1.AssetID 
  left join tmp_LocationNewHierarchicalView Old on b.FromLocationID=Old.ChildID
  left join tmp_LocationNewHierarchicalView new on b.ToLocationID=new.ChildID
  Left join DepartmentTable OldDept on b.FromDepartmentID=OldDept.DepartmentID
  left join DepartmentTable NewDept on b.ToDepartmentID=NewDept.DepartmentID
  Left join CategoryTable OldCate on b.FromCategoryID=OldCate.CategoryID
  left join CategoryTable newCate on b.ToCategoryID=newCate.CategoryID
   Left join ProductTable Oldprod on b.FromProductID=Oldprod.ProductID
  left join ProductTable newprod on b.ToProductID=newprod.ProductID
  LEft join SectionTable OldSec on b.FromSectionID=oldsec.SectionID
  left join SectionTable newsec on b.ToSectionID=newsec.SectionID

  --drop table tmp_LocationNewHierarchicalView
GO

ALTER Procedure [dbo].[prc_ValidateForTransaction]
(
   @FromAssetID				nvarchar(max)	 = NULL,
   @ToLocationID			int				 = NULL,
   @FromLocationTypeID		int				 = NULL,
   @ToLocationTypeID		Int				 = NULL,
   @ApprovalWorkFlowID		int              = NULL
)
as 
Begin
  Declare @ErrorMsg nvarchar(max),@UpdateCount int,@AprpovalCnt int ,@StausID int,@RightName nvarchar(100),@modelID int 
  select @StausID=[dbo].fnGetActiveStatusID()
  SElect @modelID=ApproveModuleID from ApproveWorkflowTable where ApproveWorkflowID=@ApprovalWorkFlowID
  select @RightName=case when @modelID=5  then 'AssetTransferApproval' when @modelID=11 then 'InternalAssetTransferApproval' else 'AssetRetirementApproval' end 
  Declare @updateTable table(updateRole bit,ApprovalRoleID int)
  if @modelID=11 
  begin
    select @ToLocationID =Level2id from LocationNewHierarchicalView where ChildID=@ToLocationID
  end

  declare @TransactionLineItemTable table (AssetID int,FromLocationL2 int ,ToLocationL2 int,
  LocationType nvarchar(100),CategoryType nvarchar(100),ApprovalWorkFlowID int )
  
  Select @AprpovalCnt=count(OrderNo) from 
  ApproveWorkflowTable a join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
  where a.StatusID=1 and b.StatusID=1 and a.ApproveWorkflowID=@ApprovalWorkFlowID

  print @AprpovalCnt
  --Declare @ApprovalUserTable Table ()
  insert into @TransactionLineItemTable(AssetID,FromLocationL2,ToLocationL2,LocationType,CategoryType,ApprovalWorkFlowID)
  select AssetID,LocationL2,@ToLocationID,LocationType,categorytype,@ApprovalWorkFlowID
  From AssetNewView where assetid in (select value from Split(@fromAssetID,',')) 

  --select * from @TransactionLineItemTable

  insert into @updateTable(updateRole,ApprovalRoleID)
  Select case when a.ApproveModuleID=5 or a.ApproveModuleID=11 then UpdateDestinationLocationsForTransfer else UpdateRetirementDetailsForEachAssets end as updateRole, 
  b.ApprovalRoleID FRom ApproveWorkflowTable a 
	  join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
	  join ApprovalRoleTable C on b.ApprovalRoleID=C.ApprovalRoleID
	  where a.ApproveWorkflowID=@ApprovalWorkFlowID and a.StatusID=@StausID and b.StatusID=@StausID and c.StatusID=@StausID
  --select * from @updateTable
  
  Declare @validationTable Table(OrderNo int,ApprovalRoleID int) 
	  Declare @validationTable1 Table(OrderNo int,ApprovalRoleID int) 
  Declare @validationTable2 Table(OrderNo int,ApprovalRoleID int) 

	insert into @validationTable(OrderNo,ApprovalRoleID)
 select a.OrderNo,a.ApprovalRoleID from ApproveWorkflowLineItemTable a 
   join @TransactionLineItemTable T on a.ApproveWorkFlowID=T.ApprovalWorkFlowID
   join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
   join CategoryTypeTable CT on T.CategoryType=Ct.categoryTypeName
   join UserApprovalRoleMappingTable D on  case when b.ApprovalLocationTypeID=5 then 
	T.FromLocationL2 else T.ToLocationL2 end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID and D.CategoryTypeID=CT.CategoryTypeID
  where ApproveWorkFlowID=@ApprovalWorkFlowID and CT.statusID=@StausID and a.StatusID=@StausID
  and b.StatusID=@StausID and D.StatusID=@StausID
  group by a.OrderNo,a.ApprovalRoleID
  --select * from @validationTable
  --print @AprpovalCnt
    if(select isnull(count(a.OrderNo),0) from  @validationTable a
  join ApproveWorkflowLineItemTable b on a.approvalroleID=b.ApprovalRoleID and a.OrderNo=b.OrderNo
  where b.ApproveWorkFlowID=@ApprovalWorkFlowID and b.StatusID=@StausID )!=@AprpovalCnt
  Begin 
     set @ErrorMsg ='Approval role not assigned to the user'
  end 

  insert into @validationTable1(OrderNo,ApprovalRoleID)
   select a.OrderNo,a.ApprovalRoleID from ApproveWorkflowLineItemTable a 
   join @TransactionLineItemTable T on a.ApproveWorkFlowID=T.ApprovalWorkFlowID
   join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
   join CategoryTypeTable CT on T.CategoryType=Ct.categoryTypeName
   join UserRightValueView D on  case when b.ApprovalLocationTypeID=5 then 
	T.FromLocationL2 else T.ToLocationL2 end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID and D.CategoryTypeID=CT.CategoryTypeID
  where ApproveWorkFlowID=@ApprovalWorkFlowID and CT.statusID=@StausID and a.StatusID=@StausID
  and b.StatusID=@StausID and D.RightName=@RightName
  group by a.OrderNo,a.ApprovalRoleID

   if(select isnull(count(a.OrderNo),0) from  @validationTable1 a
  join ApproveWorkflowLineItemTable b on a.approvalroleID=b.ApprovalRoleID and a.OrderNo=b.OrderNo
  where b.ApproveWorkFlowID=@ApprovalWorkFlowID and b.StatusID=@StausID )!=@AprpovalCnt
  Begin 
  if @ErrorMsg is not null or @ErrorMsg!=''
  Begin
     set @ErrorMsg =@ErrorMsg+' Mapped User Role not set authorized Page'
	 End 
	 else 
	 begin 
	 set @ErrorMsg ='Mapped User Role not set authorized Page'
	 end 
  end 

  insert into @validationTable2(OrderNo,ApprovalRoleID)
   select a.OrderNo,a.ApprovalRoleID from ApproveWorkflowLineItemTable a 
   join @TransactionLineItemTable T on a.ApproveWorkFlowID=T.ApprovalWorkFlowID
   join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
   join CategoryTypeTable CT on T.CategoryType=Ct.categoryTypeName
   join UserApprovalRoleMappingTable D on  case when b.ApprovalLocationTypeID=5 then 
	T.FromLocationL2 else T.ToLocationL2 end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID and D.CategoryTypeID=CT.CategoryTypeID
  join persontable p on D.userID=p.personID
   where ApproveWorkFlowID=@ApprovalWorkFlowID and CT.statusID=@StausID and a.StatusID=@StausID  and p.EMailID is not null
  and b.StatusID=@StausID --and D.RightName=@RightName
  group by a.OrderNo,a.ApprovalRoleID

  --select * from  @validationTable2
  --print @AprpovalCnt
   if (select isnull(count(a.OrderNo),0) from @validationTable1 a join ApproveWorkflowLineItemTable b on a.approvalroleID=b.ApprovalRoleID and a.OrderNo=b.OrderNo
  where b.ApproveWorkFlowID=@ApprovalWorkFlowID and b.StatusID=@StausID )=@AprpovalCnt
   begin 

   if(select isnull(count(a.OrderNo),0) from  @validationTable2 a
  join ApproveWorkflowLineItemTable b on a.approvalroleID=b.ApprovalRoleID and a.OrderNo=b.OrderNo
  where b.ApproveWorkFlowID=@ApprovalWorkFlowID and b.StatusID=@StausID )!=@AprpovalCnt
  Begin 
   if @ErrorMsg is not null or @ErrorMsg!=''
  Begin
     set @ErrorMsg =@ErrorMsg+' EmailID not assigned to the mapped workflow user'
	 End 
	 else 
	 Begin 
	 set @ErrorMsg =' EmailID not assigned to the mapped workflow user'
	 end 
  end 
  End 

   if not exists(select * from @TransactionLineItemTable where CategoryType is not null and CategoryType!='') 
   begin 
     if @ErrorMsg is not null or @ErrorMsg!=''
  Begin
   set @ErrorMsg =@ErrorMsg+ ' Category Type is not assigned for the selected asset category'
   end 
   else 
   begin 
   set @ErrorMsg = ' Category Type is not assigned for the selected asset category'
   end 
   end 
   if not exists(select * from @updateTable where updateRole=1)
   begin 
     if @ErrorMsg is not null or @ErrorMsg!=''
  Begin
    set @ErrorMsg =@ErrorMsg+ ' Update option for the user is not enabled for the selected workflow '
	end 
	else 
	begin 
	  set @ErrorMsg = ' Update option for the user is not enabled for the selected workflow '
	end 
   end 
     if (select updateRole from @updateTable group by updateRole having count(updateRole)>1)>1
   begin 
    if @ErrorMsg is not null or @ErrorMsg!=''
  Begin
    set @ErrorMsg =@ErrorMsg+ ' Update option shouldnot be enabled for more than one approval user '
	end 
	else 
	begin 
	set @ErrorMsg = ' Update option shouldnot be enabled for more than one approval user '
	end 
   end 

   Select @ErrorMsg as ErrorMsg
End 
go 


Create View [dbo].[ApprovalHistoryMappedUser]
as 
select x.*,p1.PersonFirstName+'-'+p1.PersonLastName  as personName from (
select isnull(DD.DelegatedEmployeeID,a.UserID) as UserID,a.ApprovalRoleID,LocationID,TransactionID,a.ApproveModuleID  from 
(
select F1.userID,F1.LocationID,F1.ApprovalRoleID,count(F1.categoryTypeID) as categorytypeCnt,TransactionID,A1.ApproveModuleID 
   from ApprovalHistoryTable  a1 
   join ApproveWorkflowTable B1 on a1.ApproveWorkflowID=B1.ApproveWorkFlowID 
   join ApproveWorkflowLineItemTable C1 on A1.ApproveWorkflowLineItemID=c1.ApproveWorkflowLineItemID
   join ApprovalRoleTable D1 on a1.ApprovalRoleID=D1.ApprovalRoleID
   join CategoryTypeTable E1 on a1.CategoryTypeID=E1.CategoryTypeID
   --lEFT JOIN (select categoryTypeID from CategoryTypeTable where StatusID=1 and IsAllCategoryType=0) G1 ON 
   join UserApprovalRoleMappingTable F1 on F1.ApprovalRoleID=D1.ApprovalRoleID  and 
   Case when D1.ApprovalLocationTypeID=5 then a1.FromLocationID else a1.ToLocationID end =F1.LocationID
   lEFT JOIN (select categoryTypeID from CategoryTypeTable where StatusID=1 and IsAllCategoryType=0) DD1 ON F1.categoryTypeID=dd1.categoryTypeID
   --and case when E1.IsAllCategoryType=1 then (select categoryTypeID from CategoryTypeTable where StatusID=1 and IsAllCategoryType=0)
   --else E1.CategoryTypeID end in (F1.CategoryTypeID)
	where E1.StatusID=dbo.fnGetActiveStatusID()
   group by F1.userID,F1.LocationID,F1.ApprovalRoleID,E1.IsAllCategoryType,A1.ApproveModuleID,TransactionID
   having count(F1.categoryTypeID)=
   (case when E1.IsAllCategoryType=1 then (select count(categoryTypeID) from CategoryTypeTable where StatusID=dbo.fnGetActiveStatusID() and IsAllCategoryType=0)
     else 1 end )
) a Left join (select * from DelegateRoleTable 
			where  getdate() between EffectiveStartDate and EffectiveEndDate) DD on DD.EmployeeID=a.UserID
		) x 
	join PersonTable p1 on x.UserID=p1.PersonID 
	--group by x.UserID,x.ApprovalRoleID,p1.PersonFirstName,p1.PersonLastName,LocationID
	go 
ALTER View [dbo].[ApprovalHistoryView] 
  as 
  Select b.ApprovalRoleName,case when c.StatusID = dbo.fnGetActiveStatusID() then 'Approved' else c.Status end as ApprovalStatus,p.PersonFirstName+'-'+p.PersonLastName as ApprovedBy ,a.LastModifiedDateTime as ApprovedDatetime ,a.*
  ,Ltrim([FileName]) as DocumentName,
  Case when p.PersonID is not null then p.PersonFirstName+'-'+p.PersonLastName else t3.personName end as UserName
 From ApprovalHistoryTable a 

  join ApprovalRoleTable b on a.ApprovalRoleID=b.ApprovalRoleID
  left join StatusTable c on a.StatusID=c.StatusID
  left join PersonTable p on a.LastModifiedBy=p.PersonID
  left join CategoryTypeTable CT on a.CategoryTypeID=ct.categorytypeID
  left join (SELECT a.ObjectKeyID,
   STUFF((SELECT '; ' + US.DocumentName 
          FROM DocumentTable US
          WHERE US.ObjectKeyID = a.ObjectKeyID and us.TransactionType  like '%Approval%'
          ORDER BY Filename
          FOR XML PATH('')), 1, 1, '') [FileName]
		FROM DocumentTable a join ApprovalHistoryTable b on a.ObjectKeyID=b.ApprovalHistoryID and a.TransactionType like '%Approval%' 
		GROUP BY a.ObjectKeyID) approvaldoc on a.ApprovalHistoryID=approvaldoc.ObjectkeyID 
  Left join 
      (
	  select t2.approvalRoleID,t2.LocationID,t2.TransactionID,t2.ApprovemoduleID, STUFF((select ','+PersonName 
			from ApprovalHistoryMappedUser t1 
			where  t1.transactionid=t2.transactionid and t1.approvemoduleID=t2.approvemoduleID and t1.approvalRoleID=t2.approvalRoleID and t1.LocationID=t2.LocationID
			 order by personName   FOR XML PATH('')), 1, 1, '') personName
		 from ApprovalHistoryMappedUser t2  group by t2.approvalRoleID,t2.LocationID,t2.transactionid,t2.approvemoduleID
	   )t3 on a.TransactionID=t3.TransactionID and a.ApproveModuleID=t3.ApproveModuleID and a.approvalRoleID=t3.approvalRoleID 
	   and  case when b.ApprovalLocationTypeID=5 then a.FromLocationID else a.ToLocationID end =t3.LocationID 

--Left join (
--select t2.approvalRoleID,t2.LocationID,t2.CategoryTypeID, STUFF((select ','+PersonName 
--from UserApprovalRoleMappingView t1
--where t1.approvalRoleID=t2.approvalRoleID and t1.LocationID=t2.LocationID and t1.CategorytypeID=t2.CategorytypeID
--order by personName 
-- FOR XML PATH('')), 1, 1, '') personName
-- from UserApprovalRoleMappingView t2  group by t2.approvalRoleID,t2.LocationID,t2.CategorytypeID
--) t3 on b.ApprovalRoleID=t3.ApprovalRoleID and case when b.ApprovalLocationTypeID=5 then a.FromLocationID else a.ToLocationID end =t3.LocationID  --and ( a.FromLocationID=t3.LocationID or  a.ToLocationID=t3.LocationID)
--and case when CT.IsAllCategoryType=1 then t3.CategoryTypeID else a.CategorytypeID end =t3.CategoryTypeID 
GO
ALTER Procedure [dbo].[prc_ValidateAssetCategoryMapping]
(
   @FromAssetID				nvarchar(max)	 = NULL,
   @ToLocationID			int				 = NULL,
   @FromLocationTypeID		int				 = NULL,
   @ToLocationTypeID		Int				 = NULL,
   @ApprovalWorkFlowID		int              = NULL
)
as 
Begin
  
  declare @TransactionLineItemTable table (AssetID int,FromLocationL2 int ,ToLocationL2 int,LocationType nvarchar(100),CategoryType nvarchar(100),ApprovalWorkFlowID int )
  insert into @TransactionLineItemTable(AssetID,FromLocationL2,ToLocationL2,LocationType,CategoryType,ApprovalWorkFlowID)
  select AssetID,LocationL2,@ToLocationID,LocationType,categorytype,@ApprovalWorkFlowID
  From AssetNewView where assetid in (select value from Split(@fromAssetID,',')) 

   Declare @validationTable Table(UserID int,LocationID int,ApprovalRoleID int,CategoryTypeCnt int ) 
   --select * from @TransactionLineItemTable
  Declare @approvalCnt int ,@AssetCnt int ,@workflowCnt int ,@statusID int ,@CategoryTypeCnt int ,@CategoryType nvarchar(max),@ID int 
  Set @ID=1
  select @statusID=[dbo].fnGetActiveStatusID()
  Select @AssetCnt=Count(*) from @TransactionLineItemTable 

  Select @CategoryTypeCnt=count(CategoryType) from @TransactionLineItemTable group by LocationType
  if(@CategoryTypeCnt>1)
	  begin
	    Select @CategoryType=CategoryTypeName from CategoryTypeTable where IsAllCategoryType=1 and statusID=@statusID
		select @ID= count(categorytypeID) from CategoryTypeTable where IsAllCategoryType=0 and statusID=1
	  End 
	  Else 
	  Begin 
	  Select @CategoryType=CategoryType from @TransactionLineItemTable group by CategoryType
	  end 

  Select @workflowCnt=Count(*) from ApproveWorkflowLineItemTable where ApproveWorkFlowID=@ApprovalWorkFlowID and StatusID=@statusID
  --select  @workflowCnt as workflowCnt
  --select @ID as id
  --select @CategoryType as categorytype
  --select @CategoryTypeCnt as categorytypecnt 


   insert into @validationTable(UserID,LocationID,ApprovalRoleID,CategoryTypeCnt)
     select userID,LocationID,a.ApprovalRoleID,count(d.categoryTypeID) as categorytypeCnt 
   from ApproveWorkflowLineItemTable a 
   join @TransactionLineItemTable T on a.ApproveWorkFlowID=T.ApprovalWorkFlowID
   join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
   join CategoryTypeTable CT on T.CategoryType=Ct.categoryTypeName
   join UserApprovalRoleMappingTable D on  case when b.ApprovalLocationTypeID=5 then 
	T.FromLocationL2 else T.ToLocationL2 end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID 
	and case when CT.IsAllCategoryType=1 then (select categoryTypeID from CategoryTypeTable where StatusID=@statusID and IsAllCategoryType=0) else 
	CT.CategoryTypeID end  in (D.CategoryTypeID)
	  where ApproveWorkFlowID=@ApprovalWorkFlowID and CT.statusID=@statusID 
	  group by userID,LocationID,a.ApprovalRoleID
		having count(d.categoryTypeID)=@ID

-- select a.OrderNo,a.ApprovalRoleID from ApproveWorkflowLineItemTable a 
--   join @TransactionLineItemTable T on a.ApproveWorkFlowID=T.ApprovalWorkFlowID
--   join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
--   join CategoryTypeTable CT on T.CategoryType=Ct.categoryTypeName
--   join UserApprovalRoleMappingTable D on  case when b.ApprovalLocationTypeID=5 then 
--	T.FromLocationL2 else T.ToLocationL2 end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID 
--and case when CT.IsAllCategoryType=1 then (select categoryTypeID from CategoryTypeTable where StatusID=@statusID and IsAllCategoryType=0) else 
--CT.CategoryTypeID end  in (D.CategoryTypeID)
--  where ApproveWorkFlowID=@ApprovalWorkFlowID and CT.statusID=@statusID
--  group by a.OrderNo,a.ApprovalRoleID

  --select * from @validationTable
    select @approvalCnt= count(*) from (
  select  a.ApprovalRoleID  from @validationTable a group by a.ApprovalRoleID) x
  --select @approvalCnt appcnt
  --select @workflowCnt cnt
  if(isnull((@workflowCnt),0)=isnull(@approvalCnt,0))
  begin 
    Select 1 as result
  end 
  else 
  begin 
    select 0 as result 
  end 
 
 
end 

go 

create View [dbo].[ValidateAccessRightsView]
AS
SELECT  P.PersonID UserID, P.PersonCode, P.PersonFirstName, P.PersonLastName, P.EMailID, P.MobileNo, 
			P.WhatsAppMobileNo ,x.RightValue, x.RightName
			from (
			Select 
		isnull(DD.DelegatedEmployeeID,RU.UserID) as UserID,RR.RightValue, A.RightName
	FROM User_RightTable A
	LEFT JOIN User_RoleRightTable RR ON RR.RightID = A.RightID
	LEFT JOIN User_UserRoleTable RU ON RU.RoleID = RR.RoleID
	LEFT JOIN  PersonTable P ON P.PersonID = RU.UserID
	
	--LEFT JOIN UserApprovalRoleMappingTable UA ON RU.UserID=UA.UserID AND UA.StatusID=dbo.fnGetActiveStatusID()
	--LEFT JOIN DepartmentTable DP ON DP.DepartmentID = P.DepartmentID
	Left join (select * from DelegateRoleTable where  getdate() between EffectiveStartDate and EffectiveEndDate) DD on DD.EmployeeID=RU.UserID
	WHERE RR.RightValue > 0
	) x join PersonTable p on x.userID=p.PersonID
		AND x.userID IS NOT NULL
		--AND A.RightName = 'ApproveUniformRequest'
UNION
SELECT  P.PersonID UserID, P.PersonCode, P.PersonFirstName, P.PersonLastName, P.EMailID, P.MobileNo, 
			P.WhatsAppMobileNo,x.RightValue, x.RightName--,x.ApprovalRoleID,x.LocationID,X.CategoryTypeID 
			from(
			
			Select 
		isnull(DD.DelegatedEmployeeID,UR.UserID) as UserID,UR.RightValue, A.RightName--,UA.ApprovalRoleID,UA.LocationID,UA.CategoryTypeID
	FROM User_RightTable A
	LEFT JOIN User_UserRightTable UR ON UR.RightID = A.RightID
	LEFT JOIN PersonTable P ON P.PersonID = UR.UserID
	--LEFT JOIN UserApprovalRoleMappingTable UA ON UR.UserID=UA.UserID AND UA.StatusID=dbo.fnGetActiveStatusID()
	Left join (select * from DelegateRoleTable where  getdate() between EffectiveStartDate and EffectiveEndDate) DD on DD.EmployeeID=UR.UserID
	--LEFT JOIN DepartmentTable DP ON DP.DepartmentID = P.DepartmentID
	WHERE UR.RightValue > 0
	)x join PersonTable p on x.userID=p.PersonID
		AND x.userID IS NOT NULL
		--AND A.RightName = 'ApproveUniformRequest'

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
	  Declare @ErrorMsg nvarchar(max),@UpdateCount int,@AprpovalCnt int ,@StausID int,@RightName nvarchar(100),@modelID int ,@CategoryTypeCnt int ,@CategoryType nvarchar(max),@ID int 
	  select @StausID=[dbo].fnGetActiveStatusID()
	  set @ID=1
	  SElect @modelID=ApproveModuleID from ApproveWorkflowTable where ApproveWorkflowID=@ApprovalWorkFlowID
	  select @RightName=case when @modelID=5  then 'AssetTransferApproval' when @modelID=11 then 'InternalAssetTransferApproval' else 'AssetRetirementApproval' end 
	  Declare @updateTable table(updateRole bit,ApprovalRoleID int)
	  
	  if @modelID=11 
	  begin
		select @ToLocationID =Level2id from LocationNewHierarchicalView where ChildID=@ToLocationID
	  end

	  declare @TransactionLineItemTable table (AssetID int,FromLocationL2 int ,ToLocationL2 int,
	  LocationType nvarchar(100),CategoryType nvarchar(100),ApprovalWorkFlowID int )
	  
	  insert into @TransactionLineItemTable(AssetID,FromLocationL2,ToLocationL2,LocationType,CategoryType,ApprovalWorkFlowID)
	  select AssetID,LocationL2,@ToLocationID,LocationType,categorytype,@ApprovalWorkFlowID
	  From AssetNewView where assetid in (select value from Split(@fromAssetID,',')) 

	  Select @AprpovalCnt=count(OrderNo) from 
	  ApproveWorkflowTable a join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
	  where a.StatusID=@StausID and b.StatusID=@StausID and a.ApproveWorkflowID=@ApprovalWorkFlowID

	   Select @CategoryTypeCnt=count(CategoryType) from @TransactionLineItemTable group by LocationType
		 if(@CategoryTypeCnt>1)
			  begin
				Select @CategoryType=CategoryTypeName from CategoryTypeTable where IsAllCategoryType=1 and statusID=@StausID
				select @ID= count(categorytypeID) from CategoryTypeTable where IsAllCategoryType=0 and statusID=@StausID
			  End 
			  Else 
			  Begin 
			  Select @CategoryType=CategoryType from @TransactionLineItemTable group by CategoryType
			  end 

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

		insert into @updateTable(updateRole,ApprovalRoleID)
		  Select 
		  case when a.ApproveModuleID=5 or a.ApproveModuleID=11 then UpdateDestinationLocationsForTransfer else UpdateRetirementDetailsForEachAssets end as updateRole, 
		  b.ApprovalRoleID FRom ApproveWorkflowTable a 
		  join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
		  join ApprovalRoleTable C on b.ApprovalRoleID=C.ApprovalRoleID
		  where a.ApproveWorkflowID=@ApprovalWorkFlowID and a.StatusID=@StausID and b.StatusID=@StausID and c.StatusID=@StausID
   
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

		  Declare @validationTable Table(UserID int,LocationID int,ApprovalRoleID int,CategoryTypeCnt int ) 
		  Declare @validationTable1 Table(UserID int,LocationID int,ApprovalRoleID int,CategoryTypeCnt int ) 
		  Declare @validationTable2 Table(UserID int,LocationID int,ApprovalRoleID int,CategoryTypeCnt int ) 

		 insert into @validationTable(UserID,LocationID,ApprovalRoleID,CategoryTypeCnt)
		 select userID,LocationID,a.ApprovalRoleID,count(d.categoryTypeID) as categorytypeCnt 
		  from ApproveWorkflowLineItemTable a 
		   join @TransactionLineItemTable T on a.ApproveWorkFlowID=T.ApprovalWorkFlowID
		   join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
		   join CategoryTypeTable CT on T.CategoryType=Ct.categoryTypeName
		   join UserApprovalRoleMappingTable D on  case when b.ApprovalLocationTypeID=5 then 
			T.FromLocationL2 else T.ToLocationL2 end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID 
			and 
			case when CT.IsAllCategoryType=1 then 
				(select categoryTypeID from CategoryTypeTable where StatusID=@StausID and IsAllCategoryType=0) else CT.CategoryTypeID end  in (D.CategoryTypeID)
		  where ApproveWorkFlowID=@ApprovalWorkFlowID and CT.statusID=@StausID and a.StatusID=@StausID
				and b.StatusID=@StausID and D.StatusID=@StausID
		  group by userID,LocationID,a.ApprovalRoleID
		  having count(d.categoryTypeID)=@ID

		if(select count(*) from (
				select  a.ApprovalRoleID  from @validationTable a group by a.ApprovalRoleID)x) !=@AprpovalCnt
			  Begin 
				 set @ErrorMsg ='Approval role not assigned to the user'
			  end 
        
		insert into @validationTable1(UserID,LocationID,ApprovalRoleID,CategoryTypeCnt)
				select a.userID,a.LocationID,a.ApprovalRoleID,categorytypeCnt
				From @validationTable a  
				join [ValidateAccessRightsView]  b  on a.UserID=b.UserID --and a.LocationID=b.LocationID and a.ApprovalRoleID=b.ApprovalRoleID
				and b.RightName=@RightName

		if(select count(*) from (
				select  a.ApprovalRoleID  from @validationTable1 a group by a.ApprovalRoleID)x )!=@AprpovalCnt
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
   
		Select @ErrorMsg as ErrorMsg
		
		--select * from @validationTable

		--select * from @validationTable a join UserRightValueView b on a.ApprovalRoleID=b.ApprovalRoleID  where a.userid in (1,2,32148,32149,32163) and 
  end 

go 


ALTER View [dbo].[AssetTransferApprovalView]
  as 
   
  Select ApprovalHistoryID,ApproveWorkFlowID,ApproveWorkFlowLineItemID,
		a.ApproveModuleID,a.ApprovalRoleID,a.TransactionID as ApprovalTransactionID,
		OrderNo,a.Remarks as ApprovalRemarks,FromLocationID,ToLocationID,
		FromLocationTypeID,ToLocationTypeID,a.StatusID as ApprovalStatusID,
        a.CreatedBy as ApprovalCreatedBy,a.CreatedDateTime as ApprovalCreatedDateTime,
		a.LastModifiedBy,a.LastModifiedDateTime,ObjectKeyID1,EmailsecrectKey ,b.*, Dense_rank() over( 
         PARTITION BY a.transactionid , a.approvemoduleID
         ORDER BY Orderno asc 
     ) AS SerialNo ,p.PersonFirstName+'-'+p.PersonLastName as CreatedUSer,s.Status as ApprovalStatus,isnull(c.UpdateDestinationlocationsForTransfer,0) as enableUpdate,
	 isnull(DD.DelegatedEmployeeID,D.UserID) as UserID
	From ApprovalHistoryTable a join TransactionTable b on a.TransactionID=b.TransactionID 
	join ApprovalRoleTable c on a.ApprovalRoleID=c.ApprovalRoleID
	join PersonTable p on b.CreatedBy=p.PersonID
	join StatusTable s on a.StatusID=s.StatusID
	join CategoryTypeTable CT on a.CategoryTypeID=CT.CategoryTypeID
	Left join ApprovalHistoryMappedUser D on D.ApprovalRoleID=c.ApprovalRoleID  and 
	case when c.ApprovalLocationTypeID=5 then 
	    a.FromLocationID else a.ToLocationID end =D.LocationID and a.TransactionID=D.TransactionID and a.ApproveModuleID=D.ApproveModuleID
	--Left join UserApprovalRoleMappingTable D on  case when c.ApprovalLocationTypeID=5 then 
	--a.FromLocationID else a.ToLocationID end =D.LocationID and D.ApprovalRoleID=c.ApprovalRoleID  
	--and case when CT.IsAllCategoryType=1 then D.CategoryTypeID else a.CategorytypeID end =D.CategoryTypeID 

	Left join (select * from DelegateRoleTable where  getdate() between EffectiveStartDate and EffectiveEndDate) DD on DD.EmployeeID=D.UserID
	where a.ApproveModuleID in (5,11) and a.StatusID=150  --and a.OrderNo<c.MaxOrderNo 
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
	    join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
	    join CategoryTypeTable CT on a.CategoryTypeID=Ct.CategoryTypeID
		join ApprovalHistoryMappedUser D on D.ApprovalRoleID=b.ApprovalRoleID  and 
				case when b.ApprovalLocationTypeID=5 then 
	    a.FromLocationID else a.ToLocationID end =D.LocationID and a.TransactionID=D.TransactionID and a.ApproveModuleID=D.ApproveModuleID
		join [ValidateAccessRightsView]  R  on D.UserID=R.UserID  and RightName=@RightName
	 --  Left join UserRightValueView D on  case when b.ApprovalLocationTypeID=5 then 
		--a.FromLocationID else a.ToLocationID end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID and 
		-- case when CT.IsAllCategoryType=1 then D.CategoryTypeID else a.CategorytypeID end =D.CategoryTypeID  and RightName=@RightName
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
   
   Declare @ApprovalMappedUset Table(UserID int,ApprovalRoleID int,LocationID int,TransactionID int,ApproveModuleID int,personName nvarchar(max))
   insert into @ApprovalMappedUset(UserID,ApprovalRoleID,LocationID,TransactionID,ApproveModuleID,personName)
   Select UserID,ApprovalRoleID,LocationID,TransactionID,ApproveModuleID,personName From ApprovalHistoryMappedUser

   Declare @RightValueTable Table (UserID int,PersonCode nvarchar(max),PersonFirstName nvarchar(max),PersonLastName nvarchar(max), EMailID nvarchar(max),MobileNo nvarchar(max),WhatsAppMobileNo nvarchar(max),RightValue varchar(100) ,RightName nvarchar(100))
   Insert into @RightValueTable(UserID,PersonCode,PersonFirstName,PersonLastName,EMailID,MobileNo,WhatsAppMobileNo,RightValue,RightName)
   Select UserID,PersonCode,PersonFirstName,PersonLastName,EMailID,MobileNo,WhatsAppMobileNo,RightValue,RightName from ValidateAccessRightsView
   
   
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
				   --print @NotificationTemplateID
				   --print @RightName
				   --print @ORDERID
				   --print @maxOrderID
				   --print @referenceID
				   --print @moduleID
				   Insert into NotificationInputTable(NotificationTemplateID,SYSDataID1,SYSDataID2,SYSDataID3,SYSUserID)
				   select @NotificationTemplateID,a.TransactionID,ApprovalHistoryID,NULL,d.UserID
				      from ApprovalHistoryTable a 
				   Left join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
				   Left join CategoryTypeTable CT on a.CategoryTypeID=Ct.CategoryTypeID
				   join @ApprovalMappedUset D on D.ApprovalRoleID=b.ApprovalRoleID  and 
						case when b.ApprovalLocationTypeID=5 then 
					a.FromLocationID else a.ToLocationID end =D.LocationID and a.TransactionID=D.TransactionID and a.ApproveModuleID=D.ApproveModuleID
					join @RightValueTable  R  on D.UserID=R.UserID  and RightName=@RightName
				 --  Left join UserRightValueView D on  case when b.ApprovalLocationTypeID=5 then 
					--a.FromLocationID else a.ToLocationID end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID and 
					-- case when CT.IsAllCategoryType=1 then D.CategoryTypeID else a.CategorytypeID end =D.CategoryTypeID  and RightName=@RightName
					where  CT.statusID=@StatusID and b.StatusID=@StatusID and a.StatusID=dbo.fnGetActiveStatusID() and a.TransactionID=@referenceID and a.ApproveModuleID=@moduleID 
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
				   join @ApprovalMappedUset D on D.ApprovalRoleID=b.ApprovalRoleID  and 
							case when b.ApprovalLocationTypeID=5 then 
					a.FromLocationID else a.ToLocationID end =D.LocationID and a.TransactionID=D.TransactionID and a.ApproveModuleID=D.ApproveModuleID
					join @RightValueTable  R  on D.UserID=R.UserID  and RightName=@RightName
				 --  Left join UserRightValueView D on  case when b.ApprovalLocationTypeID=5 then 
					--a.FromLocationID else a.ToLocationID end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID and 
					-- case when CT.IsAllCategoryType=1 then D.CategoryTypeID else a.CategorytypeID end =D.CategoryTypeID  and RightName=@RightName
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
				   select @NotificationTemplateID,a.TransactionID,ApprovalHistoryID,NULL,D.UserID
				   from ApprovalHistoryTable a 
				   Left join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
				   Left join CategoryTypeTable CT on a.CategoryTypeID=Ct.CategoryTypeID
				   join @ApprovalMappedUset D on D.ApprovalRoleID=b.ApprovalRoleID  and 
							case when b.ApprovalLocationTypeID=5 then 
					a.FromLocationID else a.ToLocationID end =D.LocationID and a.TransactionID=D.TransactionID and a.ApproveModuleID=D.ApproveModuleID
					join @RightValueTable  R  on D.UserID=R.UserID  and RightName=@RightName
				 --  Left join UserRightValueView D on  case when b.ApprovalLocationTypeID=5 then 
					--a.FromLocationID else a.ToLocationID end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID and 
					-- case when CT.IsAllCategoryType=1 then D.CategoryTypeID else a.CategorytypeID end =D.CategoryTypeID  and RightName=@RightName
				  where  CT.statusID=@StatusID and b.StatusID=@StatusID and a.StatusID=dbo.fnGetActiveStatusID() and a.TransactionID=@referenceID and a.ApproveModuleID=@moduleID 
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


/****** Object:  View [dbo].[nvwAssetTransfer_ForBeforeApproval]    Script Date: 4/19/2024 3:54:25 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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
  case when ah.ApproveModuleID=11 then FORMATMESSAGE( dbo.fn_GetServerURL()+'InternalAssetTransferApproval/EmailEdit?id=%d&UserID=%d', ah.ApprovalHistoryID,R.UserID)
  else FORMATMESSAGE( dbo.fn_GetServerURL()+'AssetTransferApproval/EmailEdit?id=%d&UserID=%d' , ah.ApprovalHistoryID,R.UserID)
 
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
  --Left join UserRightValueView P2 on P2.ApprovalRoleID = AH.ApprovalRoleID  and RightName = 'AssetTransferApproval'
  --and case when AD.ApprovalLocationTypeID=5 then 
		--AH.FromLocationID else AH.ToLocationID end =P2.LocationID  and 
		--case when CT.IsAllCategoryType=1 then P2.CategoryTypeID else AH.CategorytypeID end =P2.CategoryTypeID  and p2.EMailID is not null
--
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
case when ah.ApproveModuleID=5 then 
 FORMATMESSAGE( dbo.fn_GetServerURL()+'AssetTransferApproval/EmailEdit?id=%d&UserID=%d' , ah.ApprovalHistoryID,R.UserID)
 else  FORMATMESSAGE( dbo.fn_GetServerURL()+'InternalAssetTransferApproval/EmailEdit?id=%d&UserID=%d' , ah.ApprovalHistoryID,R.UserID)
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
  -- Left join UserRightValueView P2 on P2.ApprovalRoleID = AH.ApprovalRoleID  and RightName = 'AssetTransferApproval'
  --and case when AD.ApprovalLocationTypeID=5 then 
		--AH.FromLocationID else AH.ToLocationID end =P2.LocationID and 
		--case when CT.IsAllCategoryType=1 then P2.CategoryTypeID else AH.CategorytypeID end =P2.CategoryTypeID   and p2.EMailID is not null


WHERE  a.TransactionTypeID in (5,11)

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
 FORMATMESSAGE( dbo.fn_GetServerURL()+'AssetRetirementApproval/EmailEdit?id=%d&UserID=%d' , ah.ApprovalHistoryID,R.UserID)
 
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
  --Left join UserRightValueView P2 on P2.ApprovalRoleID = AH.ApprovalRoleID  and RightName = 'AssetRetirementApproval'
  --and case when AD.ApprovalLocationTypeID=5 then 
		--AH.FromLocationID else AH.ToLocationID end =P2.LocationID  and 
		--case when CT.IsAllCategoryType=1 then P2.CategoryTypeID else AH.CategorytypeID end =P2.CategoryTypeID   and p2.EMailID is not null 
  ----LEFT JOIN (SELECT ApprovalRoleID, 
  ------STRING_AGG(EmailID, ';') EMailIDs,
  ---- stuff((select ';'+Emailid from UserRightValueView PL where PL.ApprovalRoleID=p2.ApprovalRoleID group by EMailID  FOR XML PATH('')), 1, 1, '') as EMailIDs--,
  ------LocationID

		----	FROM UserRightValueView P2 where RightName = 'AssetRetirementApproval'
  
		----		AND ApprovalRoleID IS NOT NULL
		----		AND EmailID IS NOT NULL
		----	GROUP BY ApprovalRoleID) UGRP ON UGRP.ApprovalRoleID = AH.ApprovalRoleID
WHERE A.StatusID  in (150,200) and a.TransactionTypeID=10

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
FORMATMESSAGE( dbo.fn_GetServerURL()+'AssetRetirementApproval/EmailEdit?id=%d&UserID=%d' , ah.ApprovalHistoryID,R.UserID) ApprovalURL,
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
  --Left join UserRightValueView P2 on P2.ApprovalRoleID = AH.ApprovalRoleID  and RightName = 'AssetTransferApproval'
  --and case when AD.ApprovalLocationTypeID=5 then 
		--AH.FromLocationID else AH.ToLocationID end =P2.LocationID and 
		--case when CT.IsAllCategoryType=1 then P2.CategoryTypeID else AH.CategorytypeID end =P2.CategoryTypeID   and p2.EMailID is not null
  ----LEFT JOIN (SELECT ApprovalRoleID, 
  ------STRING_AGG(EmailID, ';') EMailIDs,
  ---- stuff((select ';'+Emailid from UserRightValueView PL where PL.ApprovalRoleID=p2.ApprovalRoleID group by EMailID  FOR XML PATH('')), 1, 1, '') as EMailIDs--,
  ------LocationID

		----	FROM UserRightValueView P2 where RightName = 'AssetRetirementApproval'
  
		----		AND ApprovalRoleID IS NOT NULL
		----		AND EmailID IS NOT NULL
		----	GROUP BY ApprovalRoleID) UGRP ON UGRP.ApprovalRoleID = AH.ApprovalRoleID
WHERE a.TransactionTypeID=10

GO
ALTER View [dbo].[AssetRetirementApprovalView] 
  as 

   Select a.ApprovalHistoryID,ApproveWorkFlowID,ApproveWorkFlowLineItemID,
		a.ApproveModuleID,a.ApprovalRoleID,a.TransactionID as ApprovalTransactionID,
		a.OrderNo,a.Remarks as ApprovalRemarks,FromLocationID,ToLocationID,
		FromLocationTypeID,ToLocationTypeID,a.StatusID as ApprovalStatusID,
        a.CreatedBy as ApprovalCreatedBy,a.CreatedDateTime as ApprovalCreatedDateTime,
		a.LastModifiedBy,a.LastModifiedDateTime,ObjectKeyID1,EmailsecrectKey ,b.*, Dense_rank() over( 
         PARTITION BY a.transactionid , a.approvemoduleID
         ORDER BY a.Orderno asc 
     ) AS SerialNo ,p.PersonFirstName+'-'+p.PersonLastName as CreatedUSer,s.Status as ApprovalStatus
	 ,isnull(c.UpdateRetirementDetailsForEachAssets,0) as enableUpdate, isnull(DD.DelegatedEmployeeID,D.UserID) as UserID
	From ApprovalHistoryTable a join TransactionTable b on a.TransactionID=b.TransactionID 
		join ApprovalRoleTable c on a.ApprovalRoleID=c.ApprovalRoleID
		left join CategoryTypeTable CT on a.CategoryTypeID=CT.CategoryTypeID
			Left join ApprovalHistoryMappedUser D on D.ApprovalRoleID=c.ApprovalRoleID  and 
	case when c.ApprovalLocationTypeID=5 then 
	    a.FromLocationID else a.ToLocationID end =D.LocationID and a.TransactionID=D.TransactionID and a.ApproveModuleID=D.ApproveModuleID
	--	Left join UserApprovalRoleMappingTable D on  a.FromLocationID=D.LocationID and D.ApprovalRoleID=c.ApprovalRoleID
	--	and case when CT.IsAllCategoryType=1 then D.CategoryTypeID else a.CategorytypeID end =D.CategoryTypeID 
	--	--case when c.ApprovalLocationTypeID=5 then a.FromLocationID else a.ToLocationID end =D.LocationID and D.ApprovalRoleID=c.ApprovalRoleID
	----join (select TransactionID,ApproveModuleID,max(Orderno) as MaxOrderNo from ApprovalHistoryTable  where ApproveModuleID=10 and StatusID=150 
	----group by TransactionID,ApproveModuleID )c on a.TransactionID=c.TransactionID
	join PersonTable p on b.CreatedBy=p.PersonID
	join StatusTable s on a.StatusID=s.StatusID
	Left join (select * from DelegateRoleTable where  getdate() between EffectiveStartDate and EffectiveEndDate) DD on DD.EmployeeID=D.UserID
	where a.ApproveModuleID=10 and a.StatusID=150 --and a.OrderNo<c.MaxOrderNo
	
GO

Create Procedure [dbo].[Prc_ValidateUserWiseTransactions]
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
		join tmp_LocationNewHierarchicalView b on a.FromLocationID=b.ChildID
		where b.StatusID not in (500,3) and TransactionID in
			(
				Select TransactionID from TransactionTable where (@ModuleID is null  or @moduleID='' or TransactionTypeID=@ModuleID)
			)
			and b.Level2ID in 
			(
			   select LocationID from UserLocationMappingTable where PersonID=@UserID
			) or a.ToLocationID in (select LocationID from UserLocationMappingTable where PersonID=@UserID)


end 
go 
Create View LocationNewView
as
	select * from tmp_LocationNewHierarchicalView
go
Create View CategoryNewView
as 
   Select * from tmp_CategoryNewHierarchicalView
   go 
   ALTER View [dbo].[nvwAssetRetirement_ForBeforeApproval]
as 
  select  a1.AssetCode,a1.Barcode,a1.CategoryName,a1.LocationName,a1.DepartmentName,a1.SectionDescription,a1.CustodianName,a1.AssetDescription,
  a1.AssetCondition,a1.suppliername,
  
  old.LocationName as OldLocationName,
  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubType,
ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
 case when ah.ApproveModuleID=5 and ad.UpdateRetirementDetailsForEachAssets=1   then dbo.fn_GetServerURL() else 

 FORMATMESSAGE( dbo.fn_GetServerURL()+'AssetRetirementApproval/EmailEdit?id=%d&UserID=%d' , ah.ApprovalHistoryID,R.UserID) end
 
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
  --Left join UserRightValueView P2 on P2.ApprovalRoleID = AH.ApprovalRoleID  and RightName = 'AssetRetirementApproval'
  --and case when AD.ApprovalLocationTypeID=5 then 
		--AH.FromLocationID else AH.ToLocationID end =P2.LocationID  and 
		--case when CT.IsAllCategoryType=1 then P2.CategoryTypeID else AH.CategorytypeID end =P2.CategoryTypeID   and p2.EMailID is not null 
  ----LEFT JOIN (SELECT ApprovalRoleID, 
  ------STRING_AGG(EmailID, ';') EMailIDs,
  ---- stuff((select ';'+Emailid from UserRightValueView PL where PL.ApprovalRoleID=p2.ApprovalRoleID group by EMailID  FOR XML PATH('')), 1, 1, '') as EMailIDs--,
  ------LocationID

		----	FROM UserRightValueView P2 where RightName = 'AssetRetirementApproval'
  
		----		AND ApprovalRoleID IS NOT NULL
		----		AND EmailID IS NOT NULL
		----	GROUP BY ApprovalRoleID) UGRP ON UGRP.ApprovalRoleID = AH.ApprovalRoleID
WHERE A.StatusID  in (150,200) and a.TransactionTypeID=10

GO

create view rvw_UserRoleMappingView 
as 
Select f.PersonFirstName+'-'+f.PersonLastName as PersonName,b.CategoryNameHierarchy as locationName,c.ApprovalRoleName,d.CategoryTypeName,e.Status
from UserApprovalRoleMappingTable a 
join tmp_CategoryNewHierarchicalView b on a.LocationID=b.ChildID
join ApprovalRoleTable c on a.ApprovalRoleID=c.ApprovalRoleID
join CategoryTypeTable d on a.CategoryTypeID=d.CategoryTypeID
join StatusTable e on a.StatusID=e.StatusID
join PersonTable f on a.UserID=f.PersonID
update a set 
CategoryTypeID=(select CategoryTypeID from CategoryTypeTable where CategoryTypeName='Non-IT' and statusID=1)
from CategoryTable a 
join 
(select * from CategoryTable where CategoryID in (
select ChildID from CategoryNewHierarchicalView where Level=2 and statusid not in (500,3)
)) b on a.CategoryID=b.CategoryID
go 
ALTER View [dbo].[ApprovalHistoryMappedUser]
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
 
 join (
			   select a1.*,b1.IsAllCategoryType from UserApprovalRoleMappingTable a1  
						join CategoryTypeTable b1 on a1.CategoryTypeID=b1.CategoryTypeID
						where a1.StatusID=dbo.fnGetActiveStatusID() and b1.StatusID=dbo.fnGetActiveStatusID()
						) F1 on F1.ApprovalRoleID=D1.ApprovalRoleID  and F1.StatusID=dbo.fnGetActiveStatusID() and 
   Case when D1.ApprovalLocationTypeID=5 then a1.FromLocationID else a1.ToLocationID end =F1.LocationID and 
   E1.CategoryTypeID in 
  (
	case when E1.IsAllCategoryType=1 and F1.IsAllCategoryType=1 then f1.CategoryTypeID 
	when  e1.IsAllCategoryType=0 and f1.IsAllCategoryType=0 then  f1.CategoryTypeID
	when e1.IsAllCategoryType=0 and f1.IsAllCategoryType=1 then ( select categoryTypeID from CategoryTypeTable where StatusID=dbo.fnGetActiveStatusID() and IsAllCategoryType=0 and CategoryTypeID=e1.CategoryTypeID)
	when e1.IsAllCategoryType=1 and f1.IsAllCategoryType=0 then (f1.CategoryTypeID) end
  )
 -- case when E1.IsAllCategoryType=1 then 
	--			(select categoryTypeID from CategoryTypeTable where StatusID=dbo.fnGetActiveStatusID() and IsAllCategoryType=0) 
	--			  else E1.CategoryTypeID end  in (case when F1.IsAllCategoryType=1 then (select categoryTypeID from CategoryTypeTable where StatusID=dbo.fnGetActiveStatusID() and IsAllCategoryType=0)
	--else F1.CategoryTypeID end)

  -- lEFT JOIN (select categoryTypeID from CategoryTypeTable where StatusID=1 and IsAllCategoryType=0) DD1 ON F1.categoryTypeID=dd1.categoryTypeID
  
	where E1.StatusID=dbo.fnGetActiveStatusID() and c1.StatusID=dbo.fnGetActiveStatusID() and b1.StatusID=dbo.fnGetActiveStatusID() and d1.StatusID=dbo.fnGetActiveStatusID()
   group by F1.userID,F1.LocationID,F1.ApprovalRoleID,E1.IsAllCategoryType,A1.ApproveModuleID,TransactionID
   --having count(F1.categoryTypeID)=
   --(case when E1.IsAllCategoryType=1 then (select count(categoryTypeID) from CategoryTypeTable where StatusID=dbo.fnGetActiveStatusID() and IsAllCategoryType=0)
   --  else 1 end )
) a Left join (select * from DelegateRoleTable 
			where  getdate() between EffectiveStartDate and EffectiveEndDate) DD on DD.EmployeeID=a.UserID
		) x 
	join PersonTable p1 on x.UserID=p1.PersonID 
	--group by x.UserID,x.ApprovalRoleID,p1.PersonFirstName,p1.PersonLastName,LocationID
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
				update @TransactionLineItemTable set CategoryType=@CategoryType
			  End 
			  Else 
			  Begin 
			  Select @CategoryType=CategoryType from @TransactionLineItemTable group by CategoryType
			  end 

	   if not exists(select * from @TransactionLineItemTable where CategoryType is not null and CategoryType!='') 
		begin 
			 if @ErrorMsg is not null or @ErrorMsg!=''
			  Begin
			   set @ErrorMsg =@ErrorMsg+ ', Category Type is not assigned for the selected asset category'
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
				set @ErrorMsg =@ErrorMsg+ ', Update option for the user is not enabled for the selected workflow '
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
				set @ErrorMsg =@ErrorMsg+ ', Update option shouldnot be enabled for more than one approval user '
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
		     join 
   (
   select a1.*,b1.IsAllCategoryType from UserApprovalRoleMappingTable a1  
			join CategoryTypeTable b1 on a1.CategoryTypeID=b1.CategoryTypeID
			where a1.StatusID=@StausID and b1.StatusID=@StausID
			) D on  case when b.ApprovalLocationTypeID=5 then 
	T.FromLocationL2 else T.ToLocationL2 end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID 
	and ct.CategoryTypeID  
	
	in ( case when CT.IsAllCategoryType=1 and D.IsAllCategoryType=1 then D.CategoryTypeID 
	when  CT.IsAllCategoryType=0 and D.IsAllCategoryType=0 then  D.CategoryTypeID
	when CT.IsAllCategoryType=0 and D.IsAllCategoryType=1 then ( select categoryTypeID from CategoryTypeTable where StatusID=@StausID and IsAllCategoryType=0 and CategoryTypeID=ct.CategoryTypeID)
	when CT.IsAllCategoryType=1 and D.IsAllCategoryType=0 then (D.CategoryTypeID) end)
	
		  where ApproveWorkFlowID=@ApprovalWorkFlowID and CT.statusID=@StausID and a.StatusID=@StausID
				and b.StatusID=@StausID and D.StatusID=@StausID
		  group by userID,LocationID,a.ApprovalRoleID
		  --having count(d.categoryTypeID)=@ID

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
					 set @ErrorMsg =@ErrorMsg+', Mapped User Role not set authorized Page'
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
									 set @ErrorMsg =@ErrorMsg+', EmailID not assigned to the mapped workflow user'
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
   select * from @TransactionLineItemTable
  Declare @approvalCnt int ,@AssetCnt int ,@workflowCnt int ,@statusID int ,@CategoryTypeCnt int ,@CategoryType nvarchar(max),@ID int 
  Set @ID=1
  select @statusID=[dbo].fnGetActiveStatusID()
  Select @AssetCnt=Count(*) from @TransactionLineItemTable 

  Select @CategoryTypeCnt=count(CategoryType) from @TransactionLineItemTable group by LocationType
  if(@CategoryTypeCnt>1)
	  begin
	    Select @CategoryType=CategoryTypeName from CategoryTypeTable where IsAllCategoryType=1 and statusID=@statusID
		select @ID= count(categorytypeID) from CategoryTypeTable where IsAllCategoryType=0 and statusID=1
		update @TransactionLineItemTable set CategoryType=@CategoryType

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
   join 
   (
   select a1.*,b1.IsAllCategoryType from UserApprovalRoleMappingTable a1  
			join CategoryTypeTable b1 on a1.CategoryTypeID=b1.CategoryTypeID
			where a1.StatusID=@statusID and b1.StatusID=@statusID
			) D on  case when b.ApprovalLocationTypeID=5 then 
	T.FromLocationL2 else T.ToLocationL2 end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID 
	and ct.CategoryTypeID  
	
	in ( case when CT.IsAllCategoryType=1 and D.IsAllCategoryType=1 then D.CategoryTypeID 
	when  CT.IsAllCategoryType=0 and D.IsAllCategoryType=0 then  D.CategoryTypeID
	when CT.IsAllCategoryType=0 and D.IsAllCategoryType=1 then ( select categoryTypeID from CategoryTypeTable where StatusID=@statusID and IsAllCategoryType=0 and CategoryTypeID=ct.CategoryTypeID)
	when CT.IsAllCategoryType=1 and D.IsAllCategoryType=0 then (D.CategoryTypeID) end)
	


	  where ApproveWorkFlowID=@ApprovalWorkFlowID and CT.statusID=@statusID AND d.StatusID=@statusID and a.statusID=@statusID
	  group by userID,LocationID,a.ApprovalRoleID
		--having count(d.categoryTypeID)=@ID

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

 select * from @validationTable
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
insert into EntityTable(EntityName,QueryString,EntityID)
select DynamicColumnRequiredName,ImportProcedure,38
From DynamicColumnRequiredEntityTable where DynamicColumnRequiredName  in ('CustomAsset') 
go 

insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
select ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,(Select EntityId from EntityTable where EntityName='CustomAsset')
From ImportTemplateTable where DynamicColumnRequiredEntityID=38
go 


ALTER Procedure [dbo].[iprc_QFImportAsset]
(
	@OldBarcode nvarchar(100)=NULL,
	@NewBarcode nvarchar(100)=NULL,
	@AssetCode nvarchar(100)=NULL,
	@DepartmentCode nvarchar(100)=NULL,
	@SectionCode nvarchar(100)=NULL,
	@CustodianCode nvarchar(100)=NULL,
	@ModelCode nvarchar(100)=NULL,
	@PONumber nvarchar(100)=NULL,
	@ReferenceCode nvarchar(100)=NULL,
	@SerialNo nvarchar(100)=NULL,
	@PurchaseDate nvarchar(100)=NULL, 
	@WarrantyExpiryDate nvarchar(100)=NULL,
	@CategoryCode nvarchar(100)=NULL,
	@LocationCode nvarchar(100)=NULL,
	@AssetDescription nvarchar(100)=NULL,
	@AssetConditionCode nvarchar(100)=NULL,
	@PurchasePrice nvarchar(100)=NULL,
	@DeliveryNote nvarchar(100)=NULL,
	@RFIDTagCode nvarchar(100)=NULL,
	@SupplierCode nvarchar(100)=NULL,
	@Remarks nvarchar(100)=NULL,
	@InvoiceNo nvarchar(100)=NULL,
	@InvoiceDate nvarchar(100)=NULL,
	@ManufacturerCode nvarchar(100)=NULL,
	@ComissionDate nvarchar(100)=NULL,
	@VoucherNo nvarchar(100)=NULL,
	@Make nvarchar(100)=NULL,
	@Capacity nvarchar(100)=NULL,
	@MappedAssetID nvarchar(100)=NULL,
	@ReceiptNumber nvarchar(100)=NULL,
	@ImportTypeID int=2,
	@UserID int=1
	--@LanguageID int,
	--@CompanyID int
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
	   SElect @OldBarcode+'- Manufacturer Code is must be add if update/Create Model Code ' as ReturnMessage
				Return 
	END 
	IF @CustodianCode IS NOT NULL AND (@DepartmentCode IS NULL OR @DepartmentCode='')
	BEGIN 
	   SElect @OldBarcode+'- Department Code is must be add if Update/Create Custodian Code ' as ReturnMessage
				Return 
	END 
	IF @SectionCode IS NOT NULL AND (@DepartmentCode IS NULL OR @DepartmentCode='')
	BEGIN 
	   SElect @OldBarcode+'- Department Code is must be add if Update/Create Section Code ' as ReturnMessage
				Return 
	END 

	--Insert into DummyBarcodeTable (Barcode, Description1) Values (@NewBarcode,'firstbarcode')

	-- if the Barcode already Exists or not 
	 IF Exists(SELECT BARCODE FROM ASSETTABLE WHERE BARCODE=@OLDBARCODE)
	 Begin 

	 If  (@CategoryCode is null or @CategoryCode='') and (@AssetDescription is not null or @AssetDescription!='')
	begin
	   Select @CATEGORYID=CategoryID from AssetTable where barcode=@OldBarcode
	   SElect @CategoryCode=CAtegoryCode from CategoryTable where categoryID=@CategoryID
	End 
		--Insert into DummyBarcodeTable (Barcode, Description1) Values (@NewBarcode,'oldbarcodecheck')
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
				SELECT @SECTIONID=SECTIONID FROM SECTIONTABLE WHERE SECTIONCODE=@SECTIONCODE AND DEPARTMENTID=(SELECT DEPARTMENTID FROM DEPARTMENTTABLE WHERE DEPARTMENTCODE=@DEPARTMENTCODE and StatusID=1) and StatusID=1
			    
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
				DECLARE @CID INT
				SELECT @CID = UserID from User_LoginUserTable WHERE UserName = @CUSTODIANCODE;

			  IF EXISTS(SELECT PERSONCODE FROM PERSONTABLE WHERE PersonID = @CID AND (USERTYPEID=2 OR USERTYPEID=3) and StatusID=1)
			  BEGIN
				--SELECT @CUSTODIANID=PERSONID FROM PERSONTABLE WHERE PERSONCODE=@CUSTODIANCODE AND  (USERTYPEID=2 OR USERTYPEID=3) and StatusID=1
				SET @CUSTODIANID = @CID
			  END 
			  ELSE 
			  BEGIN 
			    IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
		   BEGIN 	

			    INSERT INTO User_LoginUserTable(UserName,Password,PasswordSalt,LastActivityDate,LastLoginDate,LastLoggedInDate,ChangePasswordAtNextLogin,IsLockedOut,IsDisabled,IsApproved)
			VALUES(@CUSTODIANCODE ,'Mod6/JMHjHeKXDkUK/zd7PfLlJg=','BZxI8E2lroNt28VMhZsyyaNZha8=',GETDATE(),GETDATE(),GETDATE(),1,0,0,1 )

			INSERT INTO PersonTable(PersonID, PersonFirstName, PersonLastName, PersonCode, AllowLogin, DepartmentID, UserTypeID, StatusID, Culture,CreatedBy,CreatedDateTime) 
				VALUES(@@IDENTITY, @CUSTODIANCODE, @CUSTODIANCODE, @CUSTODIANCODE, 0, @DepartmentID, 2, 1, 'en-GB',@UserID,GETDATE())
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
				INSERT INTO SUPPLIERTABLE(SUPPLIERCODE,STATUSID,CREATEDBY,CREATEDDATETIME)
				VALUES(@SUPPLIERCODE,1,@USERID,GETDATE())
				
				SET @SUPPLIERID = SCOPE_IDENTITY()
				INSERT INTO SUPPLIERDESCRIPTIONTABLE(SUPPLIERID,SUPPLIERDESCRIPTION,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
				Select @SUPPLIERID,@SUPPLIERCODE,LanguageID,@USERID,GETDATE() from LanguageTable
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
				   
				 INSERT INTO PRODUCTTABLE(PRODUCTCODE,STATUSID,CATEGORYID,CREATEDBY,CREATEDDATETIME)
				VALUES(@ProductCode,1,@CATEGORYID,@USERID,getdate())
				 
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
						CATEGORYID in (SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1) and StatusID=1)
			   BEGIN 
					SELECT @MANUFACTURERID =MANUFACTURERID FROM MANUFACTURERTABLE WHERE MANUFACTURERCODE=@MANUFACTURERCODE AND 
						CATEGORYID in (SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1) and StatusID=1
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
			
			 IF EXISTS(SELECT MODELCODE FROM  MODELTABLE WHERE MODELCODE=@MODELCODE and StatusID=1 AND CATEGORYID in (SELECT CATEGORYID FROM CATEGORYTABLE C WHERE C.CATEGORYCODE=@CATEGORYCODE and StatusID=1) 
			    AND MANUFACTURERID in (SELECT MANUFACTURERID FROM MANUFACTURERTABLE WHERE  MANUFACTURERCODE=@MANUFACTURERCODE and CATEGORYID in (SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1)  and StatusID=1))
				BEGIN 
				
				  SELECT @MODELID=MODELID FROM MODELTABLE WHERE CATEGORYID in (SELECT CATEGORYID FROM CATEGORYTABLE C WHERE C.CATEGORYCODE=@CATEGORYCODE and StatusID=1) 
			    AND MANUFACTURERID in (SELECT MANUFACTURERID FROM MANUFACTURERTABLE WHERE  MANUFACTURERCODE=@MANUFACTURERCODE and CATEGORYID in(SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1)  and StatusID=1) and StatusID=1
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
			    AND MANUFACTURERID in (SELECT MANUFACTURERID FROM MANUFACTURERTABLE WHERE  MANUFACTURERCODE=@MANUFACTURERCODE and StatusID=1))
				BEGIN 
				  SELECT @MODELID=MODELID FROM MODELTABLE WHERE  MODELCODE=@MODELCODE  AND 
			     MANUFACTURERID in (SELECT MANUFACTURERID FROM MANUFACTURERTABLE WHERE  MANUFACTURERCODE=@MANUFACTURERCODE and StatusID=1) and StatusID=1
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
			IF @NewBarcode is not null and @NewBarcode !='' 
			Begin 
			--Insert into DummyBarcodeTable (Barcode, Description1) Values (@NewBarcode,'newbarcodeinsertion')
			if not exists(select barcode from assettable where barcode=@NewBarcode)
			begin 
			UPDATE ASSETTABLE SET
			ASSETCODE=CASE WHEN @ASSETCODE IS NULL OR RTRIM(LTRIM(@ASSETCODE))='' THEN ASSETCODE ELSE @ASSETCODE END  ,
			BARCODE=CASE WHEN @NEWBARCODE IS NULL OR RTRIM(LTRIM(@NEWBARCODE))='' THEN BARCODE ELSE @NEWBARCODE END  ,
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
			PURCHASEDATE=CASE WHEN @PURCHASEDATE IS NULL OR RTRIM(LTRIM(@PURCHASEDATE))='' THEN PURCHASEDATE ELSE CONVERT(DATETIME,@PURCHASEDATE,103) END,
			InvoiceDate=CASE WHEN @InvoiceDate IS NULL OR RTRIM(LTRIM(@InvoiceDate))='' THEN InvoiceDate ELSE CONVERT(DATETIME,@InvoiceDate,103) END,
			ComissionDate=CASE WHEN @ComissionDate IS NULL OR RTRIM(LTRIM(@ComissionDate))='' THEN ComissionDate ELSE CONVERT(DATETIME,@ComissionDate,103) END,
			WarrantyExpiryDate=ISNULL(CONVERT(DATETIME,WarrantyExpiryDate), WarrantyExpiryDate),
			LastModifiedBy=@UserID,
			LastModifiedDateTime=GETDATE()

			 WHERE BARCODE=@OLDBARCODE 

			 Update HHT_Audit set Barcode=@NewBarcode where barcode=@OldBarcode

			end 
			else 
			begin 
			   SElect 'New Barcode Already Exists -'+@NewBarcode as ReturnMessage
					      Return 
			end 
			End 
			else 
			Begin 
			  SElect 'New Barcode should be not null -'+@OldBarcode as ReturnMessage
					      Return 
			End 
			 
			 
	 End 
	 Else 
			 Begin 
			 print '22'
			     SElect 'Invalid barcode -'+@OldBarcode as ReturnMessage
					      Return 
			 End 

			 	Select @ReturnMessage = COALESCE(@ReturnMessage + ', ' + Text, Text)  from @MESSAGETABLE
			select @ReturnMessage as ReturnMessage
End

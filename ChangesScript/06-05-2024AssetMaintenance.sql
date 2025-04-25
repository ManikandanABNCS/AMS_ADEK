IF  EXists(select * from EntityCodeTable where EntityCodeName='AssetMaintenance')
BEGIN
Update  EntityCodeTable set CodePrefix='GS/AM/#DateTime.Now.Year#/AM/' where EntityCodeName='AssetMaintenance'
END

If  exists(select RightName from User_RightTable where RightName='AssetTransaction')
Begin 

Update User_RightTable set RightName='AssetMaintenance',RightDescription='AssetMaintenance' where RightName='AssetTransaction'
End
go 

If  exists(select MenuName from User_MenuTable where MenuName='AssetTransaction' and TargetObject='/AssetTransaction/Index?pageName=AssetTransaction')
Begin 

Update User_MenuTable set MenuName='AssetMaintenance',TargetObject='/AssetMaintenance/Index?pageName=AssetMaintenance',Image='css/images/MenuIcon/AssetMaintenance.png'
where MenuName='AssetTransaction' 

End
go 


If NOT exists(select TransactionTypeName from TransactionTypeTable where TransactionTypeName='AssetMaintenance' )
Begin 

Insert into TransactionTypeTable(TransactionTypeID,TransactionTypeName,IsSourceTransactionType,TransactionTypeDesc)Values
(15,'AssetMaintenance',0,'AssetMaintenance')
End
go 

if not exists(select top 1 Isdefault from MasterGridNewLineItemTable where MasterGridId in (select MasterGridId from MasterGridNewTable where MasterGridName='Transaction') and Isdefault=1) 
Begin
update MasterGridNewLineItemTable set Isdefault=1 where MasterGridId in (select MasterGridId from MasterGridNewTable where MasterGridName='Transaction')
END


IF NOT EXISTS (SELECT *  FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME = 'TransactionTable' AND COLUMN_NAME = 'VendorID')
BEGIN
    ALTER TABLE TransactionTable
    ADD VendorID INT REFERENCES PartyTable(PartyID)
END

IF NOT EXISTS (SELECT *  FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME = 'TransactionTable' AND COLUMN_NAME = 'ServiceDoneBy')
BEGINAlter table TransactionTableAdd ServiceDoneBy nvarchar(200) ENDIF NOT EXISTS (SELECT *  FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME = 'TransactionLineItemTable' AND COLUMN_NAME = 'Remarks')
BEGINAlter table TransactionLineItemTableAdd Remarks [varchar](1000)  Default ''ENDIF NOT EXISTS (SELECT *  FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME = 'TransactionLineItemTable' AND COLUMN_NAME = 'AdjustmentValue')
BEGINAlter table TransactionLineItemTableAdd AdjustmentValue int  Default 0 ENDIF NOT EXISTS (SELECT *  FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME = 'TransactionLineItemTable' AND COLUMN_NAME = 'IsAdjustmentNetBook')
BEGINAlter table TransactionLineItemTableAdd IsAdjustmentNetBook bit Default 0 NOT NULLENDIf not exists(select RightName from User_RightTable where RightName='AssetMaintenanceRequest')
Begin 
insert into User_RightTable(RightName,RightDescription,ValueType,DisplayRight,RightGroupID,ISActive,IsDeleted)
Values('AssetMaintenanceRequest','AssetMaintenanceRequest',255,1,(select RightGroupID from User_RightGroupTable where RightGroupName='Transaction'),1,0)
End
go 


If not exists(select MenuName from User_MenuTable where MenuName='AssetMaintenanceRequest' and TargetObject='/AssetMaintenanceRequest/Index?pageName=AssetMaintenanceRequest')
Begin 
insert into User_MenuTable(MenuName,RightID,TargetObject,ParentMenuID,OrderNo,Image,ParentTransactionID)
Values('AssetMaintenanceRequest',(select RightID from User_RightTable where RightName='AssetMaintenanceRequest'),'/AssetMaintenanceRequest/Index?pageName=AssetMaintenanceRequest',
(Select MenuID from User_MenuTable where MenuName='Transaction'),6,'css/images/MenuIcon/AssetMaintenance.png',1)
End
go 


If NOT exists(select TransactionTypeName from TransactionTypeTable where TransactionTypeName='AssetMaintenanceRequest' )
Begin 
Insert into TransactionTypeTable(TransactionTypeID,TransactionTypeName,IsSourceTransactionType,TransactionTypeDesc)Values
(16,'AssetMaintenanceRequest',0,'AssetMaintenanceRequest')
End
go 

IF  EXists(select * from EntityCodeTable where EntityCodeName='AssetMaintenance')
BEGIN
Update  EntityCodeTable set CodePrefix='AM' where EntityCodeName='AssetMaintenance'
END


IF not EXists(select * from EntityCodeTable where EntityCodeName='AssetMaintenanceRequest')
BEGIN
INSERT into EntityCodeTable(EntityCodeName,CodePrefix,Codesuffix,Codeformatstring,LastUSedNo)Values('AssetMaintenanceRequest','AMR',null,'{0:00000}',0)
END

if not exists (select modulename from ApproveModuleTable where ModuleName='AssetMaintenance')
begin
  insert into ApproveModuleTable(ApproveModuleID,ModuleName,StatusID)
  values(15,'AssetMaintenance',500)
end 
go 
if not exists (select modulename from ApproveModuleTable where ModuleName='AssetMaintenanceRequest')
begin
  insert into ApproveModuleTable(ApproveModuleID,ModuleName,StatusID)
  values(16,'AssetMaintenanceRequest',1)
end 
go 

If not exists(select RightName from User_RightTable where RightName='AssetMaintenanceClosure')
Begin 
insert into User_RightTable(RightName,RightDescription,ValueType,DisplayRight,RightGroupID,ISActive,IsDeleted)
Values('AssetMaintenanceClosure','AssetMaintenanceClosure',61,1,(select RightGroupID from User_RightGroupTable where RightGroupName='Transaction'),1,0)
End
go 

If not exists(select MenuName from User_MenuTable where MenuName='AssetMaintenanceClosure' and TargetObject='/AssetMaintenanceClosure/Index?pageName=AssetMaintenanceClosure')
Begin 
insert into User_MenuTable(MenuName,RightID,TargetObject,ParentMenuID,OrderNo,Image,ParentTransactionID)
Values('AssetMaintenanceClosure',(select RightID from User_RightTable where RightName='AssetMaintenanceClosure'),'/AssetMaintenanceClosure/Index?pageName=AssetMaintenanceClosure',
(Select MenuID from User_MenuTable where MenuName='Transaction'),6,'css/images/MenuIcon/AssetMaintenanceClosure.png',1)
End
go 


IF  EXists(select * from EntityCodeTable where EntityCodeName='AMCSchedule')
BEGIN
INSERT into EntityCodeTable(EntityCodeName,CodePrefix,Codesuffix,Codeformatstring,LastUSedNo)Values('AMCSchedule','AMC',null,'{0:00000}',0)
END

If not exists(select RightName from User_RightTable where RightName='AMCSchedule')
Begin 
insert into User_RightTable(RightName,RightDescription,ValueType,DisplayRight,RightGroupID,ISActive,IsDeleted)
Values('AMCSchedule','AMCSchedule',255,1,(select RightGroupID from User_RightGroupTable where RightGroupName='Transaction'),1,0)
End
go 

If not exists(select MenuName from User_MenuTable where MenuName='AMCSchedule' and TargetObject='/AMCSchedule/Index?pageName=AMCSchedule')
Begin 
insert into User_MenuTable(MenuName,RightID,TargetObject,ParentMenuID,OrderNo,Image,ParentTransactionID)
Values('AMCSchedule',(select RightID from User_RightTable where RightName='AMCSchedule'),'/AMCSchedule/Index?pageName=AMCSchedule',
(Select MenuID from User_MenuTable where MenuName='Transaction'),6,'css/images/MenuIcon/AMCSchedule.png',1)
End
go 


If NOT exists(select TransactionTypeName from TransactionTypeTable where TransactionTypeName='AMCSchedule' )
Begin 
Insert into TransactionTypeTable(TransactionTypeID,TransactionTypeName,IsSourceTransactionType,TransactionTypeDesc)Values
(21,'AMCSchedule',0,'AMCSchedule')
End
go 

IF NOT EXISTS (SELECT *  FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME = 'TransactionTable' AND COLUMN_NAME = 'TransactionStartDate')
BEGINAlter table TransactionTableAdd TransactionStartDate datetimeENDgoIF NOT EXISTS (SELECT *  FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME = 'TransactionTable' AND COLUMN_NAME = 'TransactionEndDate')
BEGINAlter table TransactionTableAdd TransactionEndDate datetimeENDgoIF OBJECT_ID('TransactionScheduleTable') IS NULL
BEGIN
CREATE TABLE [dbo].TransactionScheduleTable(
	TransactionScheduleID [int] IDENTITY(1,1) NOT NULL PRIMARY KEY ,
	[ActivityStartDate] [datetime] NOT NULL DEFAULT  getdate(),
	[ActivityEndDate] [datetime] NOT NULL DEFAULT  getdate(),
	[Activity] [varchar](1000) NULL,
	[StatusID] [int] NOT NULL REFERENCES [dbo].[StatusTable] ([StatusID]),
	[CreatedBy] [int] NOT NULL REFERENCES [dbo].[User_LoginUserTable] ([UserID]),
	[CreatedDateTime] [smalldatetime] NOT NULL,
	[LastModifiedBy] [int] NULL REFERENCES [dbo].[User_LoginUserTable] ([UserID]),
	[LastModifiedDateTime] [smalldatetime] NULL
	)
END
GO


IF NOT EXISTS (SELECT *  FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME = 'TransactionScheduleTable' AND COLUMN_NAME = 'TransactionID')
BEGINAlter table TransactionScheduleTableAdd TransactionID int REFERENCES TransactionTable(TransactionID)ENDgo
IF NOT EXISTS (SELECT *  FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME = 'TransactionTable' AND COLUMN_NAME = 'SourceTransactionScheduleID')
BEGINAlter table TransactionTableAdd SourceTransactionScheduleID int REFERENCES TransactionScheduleTable(TransactionScheduleID)ENDgoif not exists (select modulename from ApproveModuleTable where ModuleName='AMCSchedule')
begin
  insert into ApproveModuleTable(ApproveModuleID,ModuleName,StatusID)
  values(17,'AMCSchedule',1)
end 
go 

If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='AssetMaintenanceRequestApproval')BeginInsert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)Values('AssetMaintenanceRequestApproval','AssetMaintenanceRequestApproval',95,1,(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Approval'),1,0)EndgoIf not exists(SELECT MenuName FROM USER_MENUTABLE where MenuName='AssetMaintenanceRequestApproval')BeginInsert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO,ParentTransactionID) Values('AssetMaintenanceRequestApproval',(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='AssetMaintenanceRequestApproval'),'/TransactionApproval/Index?pageName=AssetMaintenanceRequestApproval',(Select MenuID from USER_MENUTABLE where MenuName='Approval' ),9,1)endgoIf not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='AMCScheduleApproval')BeginInsert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)Values('AMCScheduleApproval','AMCScheduleApproval',95,1,(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Approval'),1,0)EndgoIf not exists(SELECT MenuName FROM USER_MENUTABLE where MenuName='AMCScheduleApproval')BeginInsert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO,ParentTransactionID) Values('AMCScheduleApproval',(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='AMCScheduleApproval'),'/TransactionApproval/Index?pageName=AMCScheduleApproval',(Select MenuID from USER_MENUTABLE where MenuName='Approval' ),9,1)endgoALTER Procedure [dbo].[prc_ValidateForTransaction]
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
			@ASSETMAINTENANCEREQUEST INT = 16,
			@AMCSCHEDULE INT = 17

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
	select * from @TransactionLineItemTable
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

  go

IF OBJECT_ID('PostingStatusTable') IS NULL
BEGIN
CREATE TABLE PostingStatusTable(
	PostingStatusID int NOT NULL PRIMARY KEY,
	PostingStatus nvarchar(100) NOT NULL
)
END
go
IF NOT EXISTS(select PostingStatus from PostingStatusTable where PostingStatus='WorkInProgress')
BEGIN
	INSERT into PostingStatusTable(PostingStatusID,PostingStatus)values(1,'WorkInProgress')
END
go
IF NOT EXISTS(select PostingStatus from PostingStatusTable where PostingStatus='CompletedByEndUser')
BEGIN
	INSERT into PostingStatusTable(PostingStatusID,PostingStatus)values(10,'CompletedByEndUser')
END
go
IF NOT EXISTS(select PostingStatus from PostingStatusTable where PostingStatus='WaitingForFinalApproval')
BEGIN
	INSERT into PostingStatusTable(PostingStatusID,PostingStatus)values(11,'WaitingForFinalApproval')
END

go
ALTER TABLE TransactionTable
ADD  FOREIGN KEY (PostingStatusID) REFERENCES PostingStatusTable (PostingStatusID);

go

Update MasterGridNewLineItemTable set Format='dd/MM/yyyy hh:mm:ss'
 where MasterGridID in (
select MasterGridID from MasterGridNewTable where MasterGridName in ('AssetMaintenance','AssetMaintenanceRequest','AssetMaintenanceClosure','AMCSchedule')
) and FieldName in ('VerifiedDateTime','PostedDateTime')

Update MasterGridNewLineItemTable set Format='dd/MM/yyyy'
 where MasterGridID in (
select MasterGridID from MasterGridNewTable where MasterGridName in ('AssetMaintenance','AssetMaintenanceRequest','AssetMaintenanceClosure','AMCSchedule')
) and FieldName in ('TransactionStartDate','TransactionEndDate','TransactionDate')


update User_RoleRightTable set RightValue=53 where RightID in (select RightID from User_RightTable where RightName='AssetMaintenanceClosure')

update User_RightTable set ValueType=53 where RightName='AssetMaintenanceClosure'
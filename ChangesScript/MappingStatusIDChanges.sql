alter View [dbo].[ApprovalHistoryMappedUser]
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
   join UserApprovalRoleMappingTable F1 on F1.ApprovalRoleID=D1.ApprovalRoleID  and F1.StatusID=dbo.fnGetActiveStatusID() and 
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
	  where ApproveWorkFlowID=@ApprovalWorkFlowID and CT.statusID=@statusID AND d.StatusID=@statusID
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

ALTER Function [dbo].[ChildCategoryMappingTable] 
(
@UserID int 
)
Returns 
@Table_Var Table 
(
CategoryID int  
) as 
Begin 
WITH tblChild AS
(
    SELECT *
        FROM CATEGORYTABLE WHERE CATEGORYID in (select CategoryID from userCategoryMappingTable where PersonID = @UserID and StatusID=[dbo].fnGetActiveStatusID())
    UNION ALL
    SELECT CATEGORYTABLE.* FROM CATEGORYTABLE  JOIN tblChild 
			 ON CATEGORYTABLE.PARENTCATEGORYID = tblChild.CATEGORYID
)
insert into @Table_Var(CategoryID) select CATEGORYID from tblChild
		OPTION(MAXRECURSION 32767)
RETURN 
End 
go 

ALTER Function [dbo].[ChildLocationMappingTable] 
(
@UserID int 
)
Returns 
@Table_Var Table 
(
LocationID int  
) as 
Begin 
WITH tblChild AS
(
    SELECT *
        FROM LocationTable WHERE locationID in( select LocationID from userLocationMappingTable where PersonID = @UserID and statusid=[dbo].fnGetActiveStatusID())
    UNION ALL
    SELECT LocationTable.* FROM LocationTable  JOIN tblChild 
    ON LocationTable.parentlocationid = tblChild.locationid
)
insert into @Table_Var(LocationID) SELECT lOCATIONid
    FROM tblChild OPTION(MAXRECURSION 32767)
    Return
    End 
go 
alter Procedure [dbo].[dprc_categorywise](
	@userid int=1,
	@LanguageID int=1,
	@CompanyID int=1003)
as
begin
	SET NOCOUNT OFF;
	Declare @DepartmentTable Table(DepartmentID int) 
	Declare @CategoryTable Table(CategoryID int) 
	declare @LocationTable Table (LocationID int)

	Declare @UserCategoryMapping nvarchar(10)
	Declare @UserLocationMapping nvarchar(10)
	Select @UserCategoryMapping= ConfiguarationValue from ConfigurationTable where ConfiguarationName='UserCategoryMapping'
	Select @UserLocationMapping= ConfiguarationValue from ConfigurationTable where ConfiguarationName='UserLocationMapping'
	
	Declare @UserDepartmentMapping nvarchar(10)
		Select @UserDepartmentMapping= ConfiguarationValue from ConfigurationTable where ConfiguarationName='UserDepartmentMapping'

	Declare @DepartID as bit 
	Select @DepartID= isnull(IsAllDepartmentMapping,0) from PersonTable where PersonID=@UserID

	Select B.L1Desc as CategoryName,Count(A.AssetID) as cnt, B.Level1ID as ParentCategoryID 
	From AssetTable A (nolock)
	LEFT JOIN CategoryHierarchicalView (nolock) B ON B.CategoryID = A.CategoryID and b.LanguageID=@LanguageID
	LEFT JOIN dbo.ChildCategoryMappingTable(@userID) CM ON CM.CategoryID = A.CategoryID
	LEFT JOIN dbo.ChildLocationMappingTable(@UserID) LM ON LM.LocationID = A.LocationID
	
	where (upper(@UserDepartmentMapping)='FALSE' OR @DepartID=1 or  (a.DepartmentID in (select departmentID from UserDepartmentMappingTable where PersonID = @UserID and statusID=1))) and a.StatusID=1
		and (upper(@UserCategoryMapping)='FALSE' or CM.CategoryID IS NOT NULL )--  or a.CategoryID in (select * from ChildCategoryMappingTable(@userID)))
		and (upper(@UserLocationMapping)='FALSE' OR LM.LocationID IS NOT NULL)  --or LocationID in (select * from ChildLocationMappingTable(@UserID)))
		and A.companyID = @companyID --AND b.LanguageID=@LanguageID
	group by B.Level1ID, B.L1Desc
end
GO
alter Procedure [dbo].[dprc_AllDepartmentDetails] 
	@UserID int = 1,
	@LanguageID int = 1,
	@Type int=1,
	@companyId int = 1003
as
Begin 
    Declare @DeptTable Table (DeptID int , DeptName varchar(200))
	Declare @CateTable Table (CatID int ,CateName varchar(200))
	declare @TotalCnt Table (cnt int, DepartmentName varchar(100),categoryName varchar(100),CategoryID int, DepartmentID int,CompName nvarchar(200) )
	Declare @MainCategoryTable Table (CategoryID int, CategoryName varchar(100),ParentCategoryID varchar(100),AssetID int,CompName nvarchar(200))
	Declare @FilterDepartmentTable Table (DeptID int ,Cnt int,DeptName varchar(200),SNo int,CompName nvarchar(200))
	Declare @DepartmentTable Table(DepartmentID int) 
	Declare @DepartID as bit 

	Select @DepartID= isnull(IsAllDepartmentMapping,0) from PersonTable where PersonID=@UserID
	Declare @UserDepartmentMapping nvarchar(10)
	Select @UserDepartmentMapping= ConfiguarationValue from ConfigurationTable where ConfiguarationName='UserDepartmentMapping'

	 IF UPPER(@UserDepartmentMapping)='TRUE'
     BEGIN 
	 insert into @DepartmentTable (DepartmentID)
	  select DepartmentID from UserDepartmentMappingTable where PersonID = @UserID
	 End 
	Declare @UserCategoryMapping nvarchar(10)
    Declare @UserLocationMapping nvarchar(10)
    Select @UserCategoryMapping= ConfiguarationValue from ConfigurationTable where ConfiguarationName='UserCategoryMapping'
    Select @UserLocationMapping= ConfiguarationValue from ConfigurationTable where ConfiguarationName='UserLocationMapping'
	
	if(@DepartID='true' OR UPPER(@UserDepartmentMapping)='FALSE')
    begin 
		--SELECT @UserCategoryMapping, @UserLocationMapping

		insert into @TotalCnt(cnt,DepartmentName,categoryName,CategoryID,DepartmentID,CompName)
			select  count(*) as cnt,
				case when D.DepartmentName is null then 'No Department' Else D.DepartmentName end as DepartmentName,
				B.L1Desc FirstLevelCategoryName, B.Level1ID CategoryL1,
				case when a.DepartmentID is null then 0 else  a.DepartmentID end as DepartmentID,
				C.companyName as CompanyName
				from AssetTable A (nolock)
				LEFT JOIN CategoryHierarchicalView (nolock) B ON B.CategoryID = A.CategoryID
				LEFT JOIN DepartmentTable (nolock) D ON D.DepartmentID = A.DepartmentID
				LEFT JOIN companytable (nolock) C ON C.CompanyID = A.CompanyID
				LEFT JOIN dbo.ChildCategoryMappingTable(@userID) CM ON CM.CategoryID = A.CategoryID
				LEFT JOIN dbo.ChildLocationMappingTable(@UserID) LM ON LM.LocationID = A.LocationID

				where A.StatusID=1 and A.companyid=@companyID 
					and (upper(@UserCategoryMapping)='FALSE' or cm.CategoryID is not null) --or A.CategoryID in (select * from ChildCategoryMappingTable(@userID)))
					and (upper(@UserLocationMapping)='FALSE'  or LM.LocationID is not null) --A.LocationID in (select * from ChildLocationMappingTable(@UserID)))
				--AND (D.LanguageID IS NULL OR D.LanguageID = @LanguageID)
				AND B.LanguageID = @LanguageID
				--AND C.LanguageID = @LanguageID
			group by D.DepartmentName,B.L1Desc, B.Level1ID, a.DepartmentID, C.CompanyName	
	 
	--GEt  All Department 	
	insert into @DeptTable
		select  A.DepartmentID,DepartmentCode
		from DepartmentTable D (nolock) 
		join Assettable A (nolock) on D.DepartmentID=A.DepartmentID
		join companyTable Com on A.companyID=com.companyid
		LEFT JOIN dbo.ChildCategoryMappingTable(@userID) CM ON CM.CategoryID = A.CategoryID
		LEFT JOIN dbo.ChildLocationMappingTable(@UserID) LM ON LM.LocationID = A.LocationID
		where A.StatusID=1 and A.companyID=@CompanyID 
			and (upper(@UserCategoryMapping)='FALSE' or CM.CategoryID is not null) --or A.CategoryID in (select * from ChildCategoryMappingTable(@userID)))
			and (upper(@UserLocationMapping)='FALSE' OR LM.LocationID is not null) --or A.LocationID in (select * from ChildLocationMappingTable(@UserID)))
		group by A.DepartmentID,DepartmentCode
	End
    Else 
	Begin
		insert into @TotalCnt(cnt,DepartmentName,categoryName,CategoryID,DepartmentID,CompName)
			select count(*) as cnt,
				case when D.DepartmentName is null then 'No Department' Else D.DepartmentName end as DepartmentName,
				B.L1Desc FirstLevelCategoryName, B.Level1ID CategoryL1,
				case when a.DepartmentID is null then 0 else  a.DepartmentID end as DepartmentID,
				C.companyname as CompanyName
				from AssetTable A (nolock)
				LEFT JOIN CategoryHierarchicalView (nolock) B ON B.CategoryID = A.CategoryID
				LEFT JOIN departmenttable (nolock) D ON D.DepartmentID = A.DepartmentID
				LEFT JOIN companytable (nolock) C ON C.CompanyID = A.CompanyID
				LEFT JOIN dbo.ChildCategoryMappingTable(@userID) CM ON CM.CategoryID = A.CategoryID
				LEFT JOIN dbo.ChildLocationMappingTable(@UserID) LM ON LM.LocationID = A.LocationID

			join @DepartmentTable TT on a.DepartmentID =TT.DepartmentID 		  
			where A.StatusID=1 and 1=2 and A.companyid=@companyID 
				and (upper(@UserCategoryMapping)='FALSE'  or cm.CategoryID is not null) --A.CategoryID in (select * from ChildCategoryMappingTable(@userID)))
				and (upper(@UserLocationMapping)='FALSE'  or Lm.LocationID is not null)--A.LocationID in (select * from ChildLocationMappingTable(@UserID)))
				--AND (D.LanguageID IS NULL OR D.LanguageID = @LanguageID)
				AND B.LanguageID = @LanguageID
				--AND C.LanguageID = @LanguageID
			group by D.DepartmentName,B.L1Desc, B.Level1ID, a.DepartmentID, C.companyname	
		
		--GEt  All Department 
	insert into @DeptTable
		select  A.DepartmentID,DepartmentCode
		from DepartmentTable D (nolock) join Assettable A (nolock) on D.DepartmentID=A.DepartmentID 
			join @DepartmentTable TT on D.DepartmentID =TT.DepartmentID 
			LEFT JOIN dbo.ChildCategoryMappingTable(@userID) CM ON CM.CategoryID = A.CategoryID
	LEFT JOIN dbo.ChildLocationMappingTable(@UserID) LM ON LM.LocationID = A.LocationID
			--join companyTable Com on A.companyID=com.companyID
		where A.StatusID=1 and A.companyID=@CompanyID 
		and (upper(@UserCategoryMapping)='FALSE' or CM.CategoryID is not null) --or A.CategoryID in (select * from ChildCategoryMappingTable(@userID)))
			and (upper(@UserLocationMapping)='FALSE' OR LM.LocationID is not null) --or A.LocationID in (select * from ChildLocationMappingTable(@UserID)))
		group by A.DepartmentID,DepartmentCode
	End 

	--No Department manaually added in Table 
	if (select count(1) from @TotalCnt where DepartmentName='No Department')>0
	begin 
	--print 'dd'
		insert into @DeptTable values(0,'No Department')
	end 
	
	--Get All parent Node 
	insert into @CateTable
		select CategoryID,CategoryName
		From CategoryHierarchicalView (nolock) where 
		StatusID =1 and Level=1 and LanguageID=@LanguageID
		
   --Check the Filter option 
	IF @Type=1
	  Begin 
	  --Display category against Department with count  	  
		select CateName,DeptName,isnull(a.cnt,0) cnt,CatID,DeptID,Compname from 
		(
			select * from @CateTable C cross join  @DeptTable d 
		)t left  join @TotalCnt A on t.CatID =A.CategoryID and t.DeptID=a.DepartmentID 	
				
	  End 
	  Else 
	  Begin 
	  --Get the deparrtment Based on Department 
		insert into @FilterDepartmentTable(DeptID,cnt,deptname,SNo,CompName)
			  select  F.DepartmentID,F.cnt ,F.DepartmentName,F.SNo,F.compName from (select  row_number() over(  order by sum(cnt) desc ) as SNo ,
			  DepartmentName,sum(cnt) as cnt,DepartmentID,CompName From @TotalCnt group by DepartmentName, DepartmentID,CompName  )F where F.SNo <=@Type		  
			  order by F.cnt desc 
	  --Display category against Department with count  	  
		 select   * from 
		 (
			 select  row_number() over( partition by CateName order by a.cnt desc ) as SNo ,CateName,DeptName,isnull(a.cnt,0) as cnt,CatID,a.DepartmentID as DeptID,a.CompName 
			 from (select * from @CateTable C cross join @FilterDepartmentTable d )t left  join	 
			 @TotalCnt A on t.CatID =A.CategoryID and t.DeptID=a.DepartmentID
		 )t
		where t.SNo <=@Type
	   End 	  	
End 
GO
alter Procedure [dbo].[dprc_AllSecondLevelLocationDetails] 
	@UserID int=1 ,
	@Type int=1,
	@LanguageID int =1,
	@CompanyID int=1003
as 
Begin 
	Declare @LocTable Table (LocID int , LocName varchar(200))
	Declare @CateTable Table (CatID int ,CateName varchar(200))
	declare @TotalCnt Table (cnt int, locationName varchar(100),categoryName varchar(100),CategoryID int, LocationID int,CompName nvarchar(200) )
	
	Declare @FilterDepartmentTable Table (LocID int ,Cnt int,LocName varchar(200),SNo int,CompName nvarchar(200))
	Declare @DepartmentTable Table(DepartmentID int) 
	Declare @DepartID as bit 
	Select @DepartID= isnull(IsAllDepartmentMapping,0) from PersonTable where PersonID=@UserID
	Declare @UserDepartmentMapping nvarchar(10)
	Select @UserDepartmentMapping= ConfiguarationValue from ConfigurationTable where ConfiguarationName='UserDepartmentMapping'

	Declare @UserCategoryMapping nvarchar(10)
    Declare @UserLocationMapping nvarchar(10)
    Select @UserCategoryMapping= ConfiguarationValue from ConfigurationTable where ConfiguarationName='UserCategoryMapping'
    Select @UserLocationMapping= ConfiguarationValue from ConfigurationTable where ConfiguarationName='UserLocationMapping'

	IF UPPER(@UserDepartmentMapping)='TRUE'
	BEGIN 
		insert into @DepartmentTable (DepartmentID)
			select DepartmentID from UserDepartmentMappingTable where PersonID = @UserID
	End 

    if(@DepartID='true' or UPPER(@UserDepartmentMapping)='FALSE')
	begin
		insert into @TotalCnt(cnt,locationName, categoryName, CategoryID, LocationID, CompName)
			select count(assetID) as cnt,
				case when L.Level2ID is null then 'No Location' Else left(L.L2Desc,10) end as locationName,
				B.L1Desc FirstLevelCategoryName, B.Level1ID CategoryL1,
				ISNULL(L.Level2ID, 0) LocationID,
				C.CompanyDescription as CompanyName
			From AssetTable A (nolock)
			LEFT JOIN CategoryHierarchicalView (nolock) B ON B.CategoryID = A.CategoryID
			LEFT JOIN LocationHierarchicalView (nolock) L ON L.ChildID = A.LocationID
			LEFT JOIN CompanyView (nolock) C ON C.CompanyID = A.CompanyID
			LEFT JOIN dbo.ChildCategoryMappingTable(@userID) CM ON CM.CategoryID = A.CategoryID
			LEFT JOIN dbo.ChildLocationMappingTable(@UserID) LM ON LM.LocationID = A.LocationID

			where A.StatusID=1 and A.companyid=@companyID 
				--and a.LanguageID = @LanguageID
				and (upper(@UserCategoryMapping)='FALSE'  or cm.CategoryID is not null) --B.Level1ID in (select CategoryID from usercategorymappingtable where PersonID=@userID))
				and (upper(@UserLocationMapping)='FALSE'  or LM.LocationID is not null) --L.Level1ID in (select LocationID from UserLocationMappingTable where PersonID=@UserID))
				AND (L.LanguageID IS NULL OR L.LanguageID = @LanguageID)
				AND B.LanguageID = @LanguageID
				AND C.LanguageID = @LanguageID
			group by L.L2Desc,L.Level2ID, B.L1Desc, B.Level1ID, C.CompanyDescription

		insert into @LocTable
			SELECT LocationID, left(locationName,10)
				FROM @TotalCnt
				GROUP BY LocationID, locationName
			--select a.LocationL2,a.SecondLevelLocationName
			--from assetview A (nolock)
			--where A.StatusID=1 --and a.LocationL1 in (select locationid from locationtable where ParentLocationID is null)
			--	and  A.companyid=@companyID			
			--and (upper(@UserLocationMapping)='FALSE'  or A.LocationL1 in (select LocationID from UserLocationMappingTable where PersonID=@UserID))
			--group by a.LocationL2,a.SecondLevelLocationName

	End
	Else 
	Begin 
		insert into @TotalCnt(cnt,locationName,categoryName,CategoryID,LocationID,CompName)
			select count(AssetID) as cnt,case when a.SecondLevelLocationName is null then 'No Location' Else left(a.SecondLevelLocationName,10) end as locationName,
				a.FirstLevelCategoryName,a.CategoryL1,
				case when a.LocationL2 is null then 0 else  a.LocationL2 end as LocationID,
				a.CompanyName as CompanyName
			from assetview A (nolock)
			LEFT JOIN dbo.ChildCategoryMappingTable(@userID) CM ON CM.CategoryID = A.CategoryID
	LEFT JOIN dbo.ChildLocationMappingTable(@UserID) LM ON LM.LocationID = A.LocationID

			where A.StatusID=1 and A.companyid=@companyID and a.LanguageID=@LanguageID
			and (upper(@UserCategoryMapping)='FALSE'  or cm.CategoryID is not null)-- A.CategoryL1 in (select CategoryID from userCategoryMappingTable where personid=@userID))
			and (upper(@UserLocationMapping)='FALSE'  or lm.LocationID is not null)--A.LocationL1 in (select LocationID from UserLocationMappingTable where PersonID=@UserID))
			group by a.SecondLevelLocationName,a.FirstLevelCategoryName,a.CategoryL1,a.LocationL2,a.CompanyName

		insert into @LocTable
			select a.LocationL2,left(a.SecondLevelLocationName,10)
			from assetview A (nolock)		
			join @DepartmentTable TT on a.departmentID=TT.DepartmentID
			LEFT JOIN dbo.ChildLocationMappingTable(@UserID) LM ON LM.LocationID = A.LocationID
			where A.StatusID=1 --and  a.LocationL1 in (select locationid from locationtable where ParentLocationID is null)
			and A.companyid=@companyID
			and (upper(@UserLocationMapping)='FALSE'  or lm.LocationID is not null) --A.LocationL1 in (select locationID from UserLocationMappingTable where PersonID=@UserID))
			group by a.LocationL2,a.SecondLevelLocationName
	End 
	if (select count(1) from @TotalCnt where locationName='No Location')>0
	begin 
	    insert into @LocTable values(0,'No Location')
	end 
	--select * from @LocTable
	insert into @CateTable
		select A.CategoryID,B.CategoryDescription as  CategoryName
		From CategoryTable A  (nolock)
		--join LanguageTable L on 1=1
		left join CategoryDescriptionTable B on A.CategoryID=B.CategoryID and B.LanguageID=@LanguageID  
		LEFT JOIN dbo.ChildCategoryMappingTable(@userID) CM ON CM.CategoryID = A.CategoryID
where
		A.StatusID =1 and ParentCategoryID is null --and b.LanguageID=@LanguageID
		and (upper(@UserCategoryMapping)='FALSE'  or cm.CategoryID is not null) --A.CategoryID in (select CategoryID from usercategorymappingtable where personid=@userID))
	 --Check the Filter option 

	 IF @Type = 1
	  Begin 
	  --Display category against Department with count  
		select x.CateName,x.LocName,x.cnt,x.CatID,x.LocID,x.CompName from 
		(
		select CateName,LocName,cnt,CatID,LocID,CompName from (select CatID,CateName,LocID,LocName
		from @CateTable C  cross join 
		@LocTable d  )t left  join @TotalCnt A on t.CatID =A.CategoryID and t.LocID=a.LocationID 	
		group by LocName,LocID, CateName,cnt,CatID,compName
		) x 
	 End 
	  Else 
	  Begin 
	  --Get the deparrtment Based on Department 
		insert into @FilterDepartmentTable(LocID,cnt,LocName,SNo,compName)
			  select  F.LocationID,F.cnt ,F.LocationName,F.SNo,F.compName from (select  row_number() over(  order by sum(cnt) desc  ) as SNo ,
			  LocationName,sum(cnt) as cnt,LocationID,CompName From @TotalCnt group by LocationName, LocationID,compName  )F where F.SNo <=@Type		  
			  order by F.cnt desc 		
			  
	    select x.CateName,x.LocName,x.cnt,x.CatID,x.LocID,X.CompName from 
		(
		select CateName,LocName,a.cnt,CatID,LocID,A.compName from (select CatID,CateName,LocID,Cnt,LocName,SNo,CompName
		from @CateTable C  cross join 
		@FilterDepartmentTable d  )t left  join @TotalCnt A on t.CatID =A.CategoryID and t.LocID=a.LocationID 	
		group by LocName,LocID, CateName,a.cnt,CatID,A.CompName
		) x 
		End 
	End 
	go 
	alter Procedure [dbo].[dprc_categorywise](
	@userid int=1,
	@LanguageID int=1,
	@CompanyID int=1003)
as
begin
	SET NOCOUNT OFF;
	Declare @DepartmentTable Table(DepartmentID int) 
	Declare @CategoryTable Table(CategoryID int) 
	declare @LocationTable Table (LocationID int)

	Declare @UserCategoryMapping nvarchar(10)
	Declare @UserLocationMapping nvarchar(10)
	Select @UserCategoryMapping= ConfiguarationValue from ConfigurationTable where ConfiguarationName='UserCategoryMapping'
	Select @UserLocationMapping= ConfiguarationValue from ConfigurationTable where ConfiguarationName='UserLocationMapping'
	
	Declare @UserDepartmentMapping nvarchar(10)
		Select @UserDepartmentMapping= ConfiguarationValue from ConfigurationTable where ConfiguarationName='UserDepartmentMapping'

	Declare @DepartID as bit 
	Select @DepartID= isnull(IsAllDepartmentMapping,0) from PersonTable where PersonID=@UserID

	Select B.L1Desc as CategoryName,Count(A.AssetID) as cnt, B.Level1ID as ParentCategoryID 
	From AssetTable A (nolock)
	LEFT JOIN CategoryHierarchicalView (nolock) B ON B.CategoryID = A.CategoryID and b.LanguageID=@LanguageID
	LEFT JOIN dbo.ChildCategoryMappingTable(@userID) CM ON CM.CategoryID = A.CategoryID
	LEFT JOIN dbo.ChildLocationMappingTable(@UserID) LM ON LM.LocationID = A.LocationID
	
	where (upper(@UserDepartmentMapping)='FALSE' OR @DepartID=1 or  (a.DepartmentID in (select departmentID from UserDepartmentMappingTable where PersonID = @UserID and statusid=[dbo].fnGetActiveStatusID()))) and a.StatusID=1
		and (upper(@UserCategoryMapping)='FALSE' or CM.CategoryID IS NOT NULL )--  or a.CategoryID in (select * from ChildCategoryMappingTable(@userID)))
		and (upper(@UserLocationMapping)='FALSE' OR LM.LocationID IS NOT NULL)  --or LocationID in (select * from ChildLocationMappingTable(@UserID)))
		and A.companyID = @companyID --AND b.LanguageID=@LanguageID
	group by B.Level1ID, B.L1Desc
end
GO
-- ==============================================================================================================================================
-- Author:  Saranya
-- Create date: 23-MAY-2017 10:00:00 AM
-- Description: Procedure for getting record count for dashboard
-- **********************************************************************************************************************************************
-- Date Time			Author			Description
-- **********************************************************************************************************************************************
-- 12-May-2020 15:00	Balakrishnan	For performance required indexes are created and query optimized
-- **********************************************************************************************************************************************
alter Procedure  [dbo].[dprc_DashboardCount](
	@LangaugeID int = 1, 
	@UserID int = 1, 
	@CompanyID int = 1003
) 
as 
Begin  
	Declare @UserDepartmentMapping bit,@UserLocationMapping bit, @UserCategoryMapping bit,@AssetApproval bit,@AssetMutipleApproval bit ,@TransferAssetApproval bit
	,@MultipleAssetTransferApproval bit,@LocationEnable bit,@WarrantyNotificationDay int, @TotalAssetCount int ,@ActiveAssetCount int ,@DisposedAssetCount int,
	@warrantyCount int,@statusID int 

	select @statusID=dbo.fnGetActiveStatusID()
	select @UserDepartmentMapping= case when lower(ConfiguarationValue)='false' then 0 else 1 end  from ConfigurationTable where ConfiguarationName='UserDepartmentMapping'
	select @UserLocationMapping= case when lower(ConfiguarationValue)='false' then 0 else 1 end  from ConfigurationTable where ConfiguarationName='UserLocationMapping'
	select @UserCategoryMapping= case when lower(ConfiguarationValue)='false' then 0 else 1 end  from ConfigurationTable where ConfiguarationName='UserCategoryMapping'
	Declare @LocMap Table(LocationID int)
	Declare @CatMap Table(CategoryID int)

	insert into @LocMap(LocationID)
	select LocationID  from ChildLocationMappingTable(@UserID)
	insert into @CatMap(CategoryID)
	select CategoryID  from ChildCategoryMappingTable(@UserID)

	if((select COUNT(*) FROM @LocMap) = 0) SET @UserLocationMapping = 0
	if((select COUNT(*) FROM @CatMap) = 0) SET @UserCategoryMapping = 0

	select @LocationEnable = case when lower(ConfiguarationValue)='false' then 0 else 1 end  from ConfigurationTable where ConfiguarationName='LocationEnable'
	select @AssetApproval = case when lower(ConfiguarationValue)='false' then 0 else 1 end  from ConfigurationTable where ConfiguarationName='AssetApproval'
	select @AssetMutipleApproval= case when lower(ConfiguarationValue)='false' then 0 else 1 end  from ConfigurationTable where ConfiguarationName='AssetApprovalBasedOnWorkFlow'
	select @TransferAssetApproval= case when lower(ConfiguarationValue)='false' then 0 else 1 end  from ConfigurationTable where ConfiguarationName='TransferAssetApproval'
	select @MultipleAssetTransferApproval= case when lower(ConfiguarationValue)='false' then 0 else 1 end  from ConfigurationTable where ConfiguarationName='TransferAssetApprovalBasedOnWorkFlow'
	select @WarrantyNotificationDay= 
		case 
			when ConfiguarationValue='AssetWarrantyNotificationBefore30days' then 30 
			when ConfiguarationValue='AssetWarrantyNotificationBefore60days' then 60 
			when ConfiguarationValue='AssetWarrantyNotificationBefore90days' then 90 
			else 0 
			end    
		from ConfigurationTable where ConfiguarationName='WarrantyAlertNotificationDays'

	Select  @TotalAssetCount=SUM(CASE WHEN statusID != 3 THEN 1 ELSE 0 END) ,
			@ActiveAssetCount=SUM(CASE WHEN statusID not in (3,4) THEN 1 ELSE 0 END) 
		from AssetTable A
		LEFT JOIN @CatMap CM ON CM.CategoryID = A.CategoryID
		LEFT JOIN @LocMap LM ON LM.LocationID = A.LocationID
		where (@UserCategoryMapping=0 or CM.CategoryID IS NOT NULL /*CategoryID in (select CategoryID from #CatMap )*/) 
		and (@LocationEnable = 1 OR LM.LocationID IS NOT NULL /*@UserLocationMapping = 0 or LocationID in (select LocationID from #LocMap)*/ )
		and (@UserDepartmentMapping=0 or DepartmentID in (Select DepartmentID from UserDepartmentMappingTable where PersonID=@UserID and StatusID=@statusID))
		--and (@AssetApproval=0 or AssetID  in (select ObjectkeyValue from ApprovalTable where StatusID=5 and ActionType in (2,3,4)))
		AND CompanyID=@CompanyID --and statusID!=3 

	--Select @ActiveAssetCount=COUNT(AssetID) 
	--	from AssetTable 
	--	where (@UserCategoryMapping=0 or CategoryID in (select CategoryID from #CatMap )) 
	--	and (@LocationEnable=1 and @UserLocationMapping=0 or LocationID in (select locationid from #LocMap))
	--	and (@UserDepartmentMapping=0 or DepartmentID in (Select DepartmentID from UserDepartmentMappingTable where PersonID=@UserID))
	--	and (@AssetApproval=0 or AssetID  not in (select ObjectkeyValue from ApprovalTable where StatusID=5 and ActionType in (1)))
	--	and CompanyID=@CompanyID and statusID not in (3,4)

	Select @DisposedAssetCount=COUNT(AssetID) 
		from AssetTable 
		where (@UserCategoryMapping=0 or CategoryID in (select CategoryID from @CatMap )) 
		and (@LocationEnable=1 and @UserLocationMapping=0 or LocationID in (select locationid from @LocMap))
		and (@UserDepartmentMapping=0 or DepartmentID in (Select DepartmentID from UserDepartmentMappingTable where PersonID=@UserID and StatusID=@statusID))
		and CompanyID=@CompanyID and statusID=4

	Select @warrantyCount=COUNT(AssetID) 
		from AssetTable 
		where (@UserCategoryMapping=0 or CategoryID in (select CategoryID from @CatMap )) 
		and (@LocationEnable=1 and @UserLocationMapping=0 or LocationID in (select locationid from @LocMap))
		and (@UserDepartmentMapping=0 or DepartmentID in (Select DepartmentID from UserDepartmentMappingTable where PersonID=@UserID and StatusID=@statusID))
		and (@AssetApproval=0 or AssetID  not in (select ObjectkeyValue from ApprovalTable where StatusID=5 and ActionType in (1)))
		and CompanyID=@CompanyID and statusID not in (3,4) 
		and (WarrantyExpiryDate>=DATEADD(day,-1,getdate()) and WarrantyExpiryDate<=DATEADD(day,@WarrantyNotificationDay+1,getdate()))
	
	Select @TotalAssetCount as TotalAssetCount,@ActiveAssetCount as ActiveAssetCount,@DisposedAssetCount as DisposedAssetCount,@warrantyCount as AssetWarrantyExpiryCount

End 
GO


alter Procedure [dbo].[dprc_DepartmentWiseDetails] --@UserID  = 125 
(
@UserID int =null,
@LanguageID int,
@CompanyID int 
)
as 
Begin 

Declare @DepartID int 
Declare @UserCategoryMapping nvarchar(10)
Declare @UserLocationMapping nvarchar(10)
Select @UserCategoryMapping= ConfiguarationValue from ConfigurationTable where ConfiguarationName='UserCategoryMapping'
Select @UserLocationMapping= ConfiguarationValue from ConfigurationTable where ConfiguarationName='UserLocationMapping'
Select @DepartID= isnull(IsAllDepartmentMapping,0) from PersonTable where PersonID=@UserID
Declare @UserDepartmentMapping nvarchar(10)
 Select @UserDepartmentMapping= ConfiguarationValue from ConfigurationTable where ConfiguarationName='UserDepartmentMapping'
 
Select case when A.DepartmentID is null then 'NoDepartment' else  DD.DepartmentDescription end as DepartmentName,Count(A.AssetID) as cnt, case when A.DepartmentID is null then 0 else A.DepartmentID end as DepartmentID
From [dbo].Assettable A (nolock) 
Join LanguageTable La on 1=1
join (select v.ChildID,AA.AssetID,V.FirstLevel from CategoryHierarchicalView V Join AssetTable AA on V.ChildID=aa.CategoryID) DT  on A.AssetId=DT.AssetID
Join CategoryTable C (nolock) on DT.FirstLevel =C.CategoryID
left join DepartmentTable D (nolock) on A.DepartmentID=D.DepartmentID
Left join DepartmentDescriptionTable DD (nolock) on D.DepartmentID=DD.DepartmentID and La.LanguageID=DD.LanguageID
Left join CompanyTable Com(nolock) on A.companyID=Com.companyID 
	LEFT JOIN dbo.ChildCategoryMappingTable(@userID) CM ON CM.CategoryID = A.CategoryID
	LEFT JOIN dbo.ChildLocationMappingTable(@UserID) LM ON LM.LocationID = A.LocationID
where La.LanguageID=@LanguageID and (upper(@UserDepartmentMapping)='FALSE' OR @DepartID=1 or  (a.DepartmentID in (select departmentID from UserDepartmentMappingTable where PersonID = @UserID and statusid=1)))  and a.StatusID=1 and A.CompanyID=@CompanyID
and (upper(@UserCategoryMapping)='FALSE'  or cm.CategoryID is not null)-- a.CategoryID in (select * from ChildCategoryMappingTable(@userID)))
  and (upper(@UserLocationMapping)='FALSE'  or lm.LocationID is not null) -- LocationID in (select * from ChildLocationMappingTable(@UserID)))
group by DD.DepartmentDescription ,A.DepartmentID
End
go 
alter Procedure [dbo].[dprc_ExpiryNotification]
(
@UserID int,
@CompanyID int 
)as
Begin 
Declare @DeptID int;
Declare @DepartmentTable Table(DepartmentID int) 

Declare @UserDepartmentMapping nvarchar(10)
Declare @UserCategoryMapping nvarchar(10)
Declare @UserLocationMapping nvarchar(10)

Select @UserDepartmentMapping= ConfiguarationValue from ConfigurationTable where ConfiguarationName='UserDepartmentMapping'
Select @UserCategoryMapping= ConfiguarationValue from ConfigurationTable where ConfiguarationName='UserCategoryMapping'
Select @UserLocationMapping= ConfiguarationValue from ConfigurationTable where ConfiguarationName='UserLocationMapping'

IF UPPER(@UserDepartmentMapping)='TRUE'
   BEGIN 
	insert into @DepartmentTable (DepartmentID)
	select DepartmentID from UserDepartmentMappingTable where PersonID = @UserID and StatusID=1 
	END 
Declare @DepartID as bit 
Select @DepartID= isnull(IsAllDepartmentMapping,0) from PersonTable where PersonID=@UserID


Declare @CommonDashBoardTable table(CommonID int identity(1,1),Name varchar(100),Cnt int,axis varchar(100),categoryID int)

	Insert into @CommonDashBoardTable(Name,Cnt)
	Select '1-10',
	count(distinct(Barcode)) from AssetTable A (nolock)  join CompanyTable Com (nolock) on A.companyID=Com.companyID where 
	cast(convert(varchar(50),WarrantyExpiryDate,101) as datetime) between   cast(convert(varchar(50),getdate(),101) as datetime) and 
	cast(dateadd(day, 9, convert(varchar(50),getdate(),101)) as datetime) and A.statusID=1	 
	 and (upper(@UserDepartmentMapping)='FALSE' OR @DepartID =1 or DepartmentID in (select DepartmentID from @DepartmentTable))
	 and (upper(@UserCategoryMapping)='FALSE'  or CategoryID in (select * from ChildCategoryMappingTable(@UserId)))
	 and (upper(@UserLocationMapping)='FALSE'  or LocationID in (select * from ChildLocationMappingTable(@UserId)))
     and A.companyID=@companyID
	Insert into @CommonDashBoardTable(Name,Cnt)

	Select '11-20',
	count(distinct(Barcode)) from AssetTable A (nolock) join CompanyTable Com (nolock) on A.companyID=Com.companyID  where 
	cast(convert(varchar(50),WarrantyExpiryDate,101) as datetime) between cast(dateadd(day, 10, convert(varchar(50),getdate(),101)) as datetime) and cast(dateadd(day, 19, convert(varchar(50),getdate(),101)) as datetime)
	 and A.statusID=1   and A.companyID=@companyID
	 and (upper(@UserDepartmentMapping)='FALSE' OR @DepartID =1 or DepartmentID in (select DepartmentID from @DepartmentTable))
	 and (upper(@UserCategoryMapping)='FALSE'  or CategoryID in (select * from ChildCategoryMappingTable(@UserId)))
	 and (upper(@UserLocationMapping)='FALSE'  or LocationID in (select * from ChildLocationMappingTable(@UserId)))
	Insert into @CommonDashBoardTable(Name,Cnt)

	Select '21-30',

	count(distinct(Barcode)) from AssetTable A  (nolock)  join CompanyTable Com (nolock) on A.companyID=Com.companyID  where cast(convert(varchar(50),WarrantyExpiryDate,101) as datetime) between
	cast(dateadd(day, 20, convert(varchar(50),getdate(),101)) as datetime) and cast(dateadd(day, 29, convert(varchar(50),getdate(),101)) as datetime)
	 and A.statusID=1   and A.companyID=@companyID
	 and (upper(@UserDepartmentMapping)='FALSE' OR @DepartID =1 or DepartmentID in (select DepartmentID from @DepartmentTable))
	 --and (@DeptID is null or @DeptID='' or DepartmentID=@DeptID)
     and (upper(@UserCategoryMapping)='FALSE'  or CategoryID in (select * from ChildCategoryMappingTable(@UserId)))
     and (upper(@UserLocationMapping)='FALSE'  or LocationID in (select * from ChildLocationMappingTable(@UserId)))
	Insert into @CommonDashBoardTable(Name,Cnt)
	Select '31-40',
	count(distinct(Barcode)) from AssetTable A (nolock)  join CompanyTable Com (nolock) on A.companyID=Com.companyID where cast(convert(varchar(50),WarrantyExpiryDate,101) as datetime) between
	cast(dateadd(day, 30, convert(varchar(50),getdate(),101)) as datetime) and cast(dateadd(day, 39, convert(varchar(50),getdate(),101)) as datetime)
	 and A.statusID=1   and A.companyID=@companyID
	and (upper(@UserDepartmentMapping)='FALSE' OR @DepartID =1 or DepartmentID in (select DepartmentID from @DepartmentTable))
    and (upper(@UserCategoryMapping)='FALSE'  or CategoryID in (select * from ChildCategoryMappingTable(@UserId)))
    and (upper(@UserLocationMapping)='FALSE'  or LocationID in (select * from ChildLocationMappingTable(@UserId)))
	 Insert into @CommonDashBoardTable(Name,Cnt)

	Select '41-50',

	count(distinct(Barcode)) from AssetTable A (nolock)  join CompanyTable Com (nolock) on A.companyID=Com.companyID where cast(convert(varchar(50),WarrantyExpiryDate,101) as datetime) between
	cast(dateadd(day, 40, convert(varchar(50),getdate(),101)) as datetime) and cast(dateadd(day, 49, convert(varchar(50),getdate(),101)) as datetime)
	 and A.statusID=1   and A.companyID=@companyID
	and (upper(@UserDepartmentMapping)='FALSE' OR @DepartID =1 or DepartmentID in (select DepartmentID from @DepartmentTable))
    and (upper(@UserCategoryMapping)='FALSE'  or CategoryID in (select * from ChildCategoryMappingTable(@UserId)))
    and (upper(@UserLocationMapping)='FALSE'  or LocationID in (select * from ChildLocationMappingTable(@UserId)))
	 Insert into @CommonDashBoardTable(Name,Cnt)

	Select '51-60',

	count(distinct(Barcode)) from AssetTable A (nolock)  join CompanyTable Com (nolock) on A.companyID=Com.companyID where cast(convert(varchar(50),WarrantyExpiryDate,101) as datetime) between
	cast(dateadd(day, 50, convert(varchar(50),getdate(),101)) as datetime) and cast(dateadd(day, 59, convert(varchar(50),getdate(),101)) as datetime)
	 and A.statusID=1   and A.companyID=@companyID
    and (upper(@UserDepartmentMapping)='FALSE' OR @DepartID =1 or DepartmentID in (select DepartmentID from @DepartmentTable))
    and (upper(@UserCategoryMapping)='FALSE'  or CategoryID in (select * from ChildCategoryMappingTable(@UserId)))
    and (upper(@UserLocationMapping)='FALSE'  or LocationID in (select * from ChildLocationMappingTable(@UserId)))
 Select * from @CommonDashBoardTable
 End 
GO

alter Procedure [dbo].[dprc_locationwise] --@userid = 125
(
@userid int,
@LanguageID int,
@CompanyId int 
)
as
begin


Declare @DepartID as bit 
Select @DepartID= isnull(IsAllDepartmentMapping,0) from PersonTable where PersonID=@UserID
Declare @UserCategoryMapping nvarchar(10)
Declare @UserLocationMapping nvarchar(10)
Declare @UserDepartmentMapping nvarchar(10)
	Select @UserDepartmentMapping= ConfiguarationValue from ConfigurationTable where ConfiguarationName='UserDepartmentMapping'
Select @UserCategoryMapping= ConfiguarationValue from ConfigurationTable where ConfiguarationName='UserCategoryMapping'
Select @UserLocationMapping= ConfiguarationValue from ConfigurationTable where ConfiguarationName='UserLocationMapping'


  Select case when DT.FirstLevel is null then 'No Location' else DT.ChildName end as LocationName,count(a.AssetID) as cnt,DT.FirstLevel as MainParentLocationID
  From AssetTable A (nolock) 
    Join LanguageTable La on 1=1
  join (select v.ChildID, VV.AssetID,V.FirstLevel,v.LanguageID,V.LocationName as ChildName  from LocationHierarchicalView V join ASsetTable VV on V.ChildID=VV.LocationID) DT on a.AssetID =DT.AssetID and DT.LanguageID=La.LanguageID
  left join locationtable L (nolock) on DT.FirstLevel=L.LocationID and l.ParentLocationid is null 
  Join CompanyTable com (nolock) on A.companyID=com.companyID 
 	LEFT JOIN dbo.ChildCategoryMappingTable(@userID) CM ON CM.CategoryID = A.CategoryID
	LEFT JOIN dbo.ChildLocationMappingTable(@UserID) LM ON LM.LocationID = A.LocationID
 where (upper(@UserDepartmentMapping)='FALSE' OR @DepartID=1 or  (a.DepartmentID in (select departmentID from UserDepartmentMappingTable where PersonID = @UserID and StatusID=1))) and a.StatusID=1
   and (upper(@UserCategoryMapping)='FALSE'  or cm.CategoryID is not null) --A.CategoryID in (select * from ChildCategoryMappingTable(@userID)))
	 and (upper(@UserLocationMapping)='FALSE'  or lm.LocationID is not null) --A.LocationID in (select * from ChildLocationMappingTable(@UserID)))
  and a.StatusID=1 and A.companyID=@CompanyID and La.LanguageID=@LanguageID
  group by DT.ChildName ,DT.FirstLevel
end
go 
alter  Procedure [dbo].[dprc_TransactionPendingCount]
(
@UserID int,
@LanguageID int,
@CompanyID int 
)
as 
Begin 
--Check the User wise Department Mapping 
Declare @DepartID as bit 
Declare @DepartmentTable Table(DepartmentID int) 
	Select @DepartID= isnull(IsAllDepartmentMapping,0) from PersonTable where PersonID=@UserID
Declare @UserDepartmentMapping nvarchar(10)
	Select @UserDepartmentMapping= ConfiguarationValue from ConfigurationTable where ConfiguarationName='UserDepartmentMapping'

  Declare @CategoryMappingTable Table(CategoryID int) 
    declare @LocationMappingTable Table (LocationID int)
    Declare @UserCategoryMapping nvarchar(10)
    Declare @UserLocationMapping nvarchar(10)
    Select @UserCategoryMapping= ConfiguarationValue from ConfigurationTable where ConfiguarationName='UserCategoryMapping'
    Select @UserLocationMapping= ConfiguarationValue from ConfigurationTable where ConfiguarationName='UserLocationMapping'
    
IF (UPPER(@UserDepartmentMapping)='FALSE' or @DepartID =1)
Begin 
	select count(A.AssetID) as Cnt,C.CategoryCode as CategoryCode,DT.CategoryName as CategoryName from AssetTable A
	Join LanguageTable L on 1=1
	join (select v.ChildID,AA.AssetID,V.FirstLevel,v.LanguageID,V.CategoryName from CategoryHierarchicalView V Join AssetTable AA on V.ChildID=aa.CategoryID) DT on A.AssetID =DT.AssetID and DT.LanguageID=L.LanguageID
	Join CategoryTable C (nolock) on DT.FirstLevel =C.CategoryID and C.ParentCategoryID is null
	join CompanyTable Com (nolock) on A.companyID=com.companyID	
	Where A.StatusID=1 and A.MappedAssetID=-1 and A.companyID=@CompanyID and l.LanguageID=@LanguageID
	and (upper(@UserCategoryMapping)='FALSE'  or A.CategoryID in (select * from ChildCategoryMappingTable(@userID)))
	 and (upper(@UserLocationMapping)='FALSE'  or A.LocationID in (select * from ChildLocationMappingTable(@UserID)))
	Group by C.CategoryCode,DT.CategoryName
End     
Else
Begin 
select count(A.AssetID) as Cnt,C.CategoryCode as CategoryCode,dt.CategoryName as CategoryName from CategoryTable C	
	Join LanguageTable La on 1=1
	Join (select v.ChildID,AA.AssetID,V.FirstLevel,V.LanguageID,V.CAtegoryName from CategoryHierarchicalView V Join AssetTable AA on V.ChildID=aa.CategoryID) DT  on DT.FirstLevel =C.CategoryID and DT.LanguageID=La.LanguageID	
	 Join (select AssetID,A.StatusID,DepartmentID,MappedAssetID from AssetTable A join CompanyTable Com (nolock) on A.companyID=com.companyID	 WHERE A.statusID=1 and A.companyID=@CompanyID
	and  (upper(@UserDepartmentMapping)='FALSE' OR @DepartID=1 or  (a.DepartmentID in (select departmentID from UserDepartmentMappingTable where PersonID = @UserID and StatusID=1)))
	and (upper(@UserCategoryMapping)='FALSE'  or A.CategoryID in (select * from ChildCategoryMappingTable(@userID)))
	 and (upper(@UserLocationMapping)='FALSE'  or A.LocationID in (select * from ChildLocationMappingTable(@UserID)))
	) A on DT.AssetID=A.AssetID   and La.LanguageID=@LanguageID
	Where A.StatusID=1 and A.MappedAssetID=-1	
	Group by C.CategoryCode,DT.CategoryName		
End 
End 
go 

alter Procedure [dbo].[rprc_AssetSummary]
(
	@LanguageID int=1,
	@UserId int =1,
	@CompanyID nvarchar(max)=1003,
	@ClassificationID int =NULL,
	@Query nvarchar(max)=null
)
as 
Begin 
Declare @DynamicQuery nvarchar(max)
Declare @ParamDefinition AS NVarchar(max) = null
--Test Table 
--Declare @Sample Table (queries nvarchar(max))
--Get Parent Child Count 
Set @DynamicQuery= 'Declare @ParentChildCountTable Table (ParentAssetID int,ChildCount int) ' ;
Set @DynamicQuery=@DynamicQuery+ 'insert into @ParentChildCountTable(ParentAssetID,ChildCount) Select map.ParentAssetID,count(ParentAssetID) From AssetTable A 
join AssetMappingTable Map on A.AssetID=Map.ParentAssetID and Map.statusid=1 Group by map.ParentAssetID  ' ;
								  
-- Department Mapping Details 
 Declare @UserDepartmentMapping nvarchar(10)  
								Select @UserDepartmentMapping= ConfiguarationValue from ConfigurationTable where ConfiguarationName='UserDepartmentMapping'
-- Category Mapping Details 
  Declare @UserCategoryMapping nvarchar(10) 
       Select @UserCategoryMapping= ConfiguarationValue from ConfigurationTable where ConfiguarationName='UserCategoryMapping' ;	
							  
Declare @DepartID as bit 
	Select @DepartID= isnull(IsAllDepartmentMapping,0) from PersonTable where PersonID=@UserID
--Location Mapping Details 
 Declare @UserLocationMapping nvarchar(10)
								  Select @UserLocationMapping= ConfiguarationValue from ConfigurationTable where ConfiguarationName='UserLocationMapping' ;	
		
Set @DynamicQuery=@DynamicQuery+' select A.Barcode, A.AssetCode,A.AssetDescription, A.CategoryName , A.LocationName,a.DepartmentCode,a. DepartmentName,
a.SectionCode,a.SectionDescription,a.CustodianName,A.PurchasePrice,A.MappedAssetID,A.RFIDTagCode,A.PurchaseDate ,A.WarrantyExpiryDate ,a.Model,A.ReferenceCode,A.SerialNo,A.DeliveryNote,
a.AssetConditionCode,a. AssetCondition, a.SupplierCode,a. SupplierName,A.CreatedDateTime,A.Make,A.Capacity,A.PONumber,A.ComissionDate,Parent.ChildCount,
a.ProductCode,a.ProductName,AssetRemarks, case when MappedAssetID=''NULL'' then NULL else MappedAssetID End  as MappedAssetNo , DVT.DepreciationName as DepreciationName,DVT.DepreciationStartDate as DepreciationStartDate,DVT.DepreciationPeriod as DepreciationPeriod,A. Accumulatedvalue ,A.NetValue,
a. Manufacturer,NetworkID as OwnedLeased,InvoiceNo,InvoiceDate,partialDisposalTotalValue,case when CreateFromHHT=0 then ''False'' else ''True'' End as CreateFromHHT,a. CreateBy as CreatedBy,a. ModifedBy,A.LastModifiedDateTime as ModifedDate,DisposalReferenceNo,
DisposedDateTime,DisposedRemarks,DisposalValue,a.CompanyCode,a. CompanyName, a. DepreciationEndDate,
a.FirstLevelCategoryName,a.SecondLevelCategoryName,
a.FirstLevelLocationName,a.SecondLevelLocationName,
A.LocationHierarchy,A.CategoryHierarchy,A.Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,
A.Attribute6,A.Attribute7,A.Attribute8,A.Attribute9,A.Attribute10,A.Attribute11,A.Attribute12,A.Attribute13,A.Attribute14,
A.Attribute15,A.Attribute16,a.CustodianType,a.UploadedImagePath as AssetImage, A.Latitude, A.Longitude,
A.Attribute17,A.Attribute18,Attribute19,Attribute20,Attribute21,Attribute22,Attribute23,Attribute24,Attribute25,Attribute26,
Attribute27,Attribute28,Attribute29,Attribute30,Attribute31,Attribute32,Attribute33,Attribute34,Attribute35,Attribute36,Attribute37,Attribute38,Attribute39,Attribute40
from AssetView A 
left join AssetDepreciationTable DVT on a.AssetID=DVT.AssetID 

Left join @ParentChildCountTable Parent on A.assetID=Parent.ParentAssetID 
where A.StatusID=1 and a.LanguageID=@LanguageID and A.companyID in (Select Value from Split('''+@companyID+''','','')) '
								   if(@ClassificationID=2)
								   Begin
									set @DynamicQuery=@DynamicQuery+ 'and (a.PurchaseDate < ''2014-09-01'')'
								   End 
								   if(@ClassificationID=3)
								   Begin
									set @DynamicQuery=@DynamicQuery+ 'and (a.PurchaseDate>''2014-08-31'')'
								   End 
								    if (upper(@UserDepartmentMapping)='TRUE' and @DepartID=0)
								   begin 
								    set @DynamicQuery=@DynamicQuery+' and (a.DepartmentID in (select departmentID from UserDepartmentMappingTable where PersonID =@UserId and StatusID=1))'
								   End 
								   if upper(@UserCategoryMapping)='TRUE' 
								   begin 
								   set @DynamicQuery=@DynamicQuery+' and (A.CategoryL1 in (select CategoryID from userCategoryMappingTable where personID=@UserId and StatusID=1))'
								   End 
								   if upper(@UserLocationMapping)='TRUE' 
								   begin 
								   set @DynamicQuery=@DynamicQuery+' and (A.LocationL1 in (select LocationID from UserLocationMappingTaable where personID=@UserId and StatusID=1))'
								   End 
if (@Query is not null and @Query<>'' ) 
Begin
	set @DynamicQuery=@DynamicQuery+' and ' +@Query 
	End 
	SET @ParamDefinition = '@LanguageID int,@UserId int'
	
	--set @DynamicQuery=@DynamicQuery+'Order By A.AssetID asc'
	print @DynamicQuery
	--insert into @Sample values (@DynamicQuery)
	--select * from @Sample

	exec sp_executesql @DynamicQuery,@ParamDefinition,@LanguageID ,@UserId
End 
GO
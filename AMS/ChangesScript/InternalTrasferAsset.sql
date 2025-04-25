If not exists(select ModuleName from ApproveModuleTable where ModuleName='InternalAssetTrasfer') 
Begin 
Insert into ApproveModuleTable(ModuleName,StatusID) values('InternalAssetTrasfer',1)
End 
go 
If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='InternalAssetTransfer')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('InternalAssetTransfer','InternalAssetTransfer',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Transaction'),1,0);
End
Go
If not exists(SELECT MenuName FROM USER_MENUTABLE where MenuName='InternalAssetTransfer')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(

'InternalAssetTransfer',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='InternalAssetTransfer'),'/InternalAssetTransfer/Index',
(Select MenuID from USER_MENUTABLE where MenuName='Transaction' ),8);
end
go

if not exists(select TransactionTypeName from TransactionTypeTable where TransactionTypeName='InternalAssetTransfer')
Begin 
Insert into TransactionTypeTable(TransactionTypeID,TransactionTypeName,IsSourceTransactionType,TransactionTypeDesc)
values(11,'InternalAssetTransfer',0,'InternalAssetTransfer')
end 
go 
update user_menutable set parentmenuID=1 where menuname='InternalAssetTransfer'
go 

if not exists(select Mastergridname from MasterGridNewTable where MasterGridName='InternalAssetTransfer') 
Begin
  Insert into MasterGridNewTable(MasterGridName,EntityName)
  values('InternalAssetTransfer','AssetNewView')
end 
go 

insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
select COLUMN_NAME,COLUMN_NAME,'100', case when DATA_TYPE='smalldatetime' then 'dd/MM/yyyy' else NULL end,1,ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS displayOrderID,
case when  DATA_TYPE='smalldatetime' or  DATA_TYPE='date' then 'System.DateTime' 
when DATA_TYPE='varchar' or  DATA_TYPE='nvarchar' then 'System.String' 
when DATA_TYPE='decimal' then 'System.Decimal' else DATA_TYPE end ,NULL,(select MasterGridID from MasterGridNewTable where MasterGridName='InternalAssetTransfer')
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME='AssetNewView' 
 and COLUMN_NAME not like 'Attribute%' and COLUMN_NAME not like 'Create%'  and COLUMN_NAME not like 'Last%'
 and COLUMN_NAME not like 'Status%' and COLUMN_NAME not in (replace(table_name,'NewView','ID'))
 and COLUMN_NAME not like 'DOF%' and DATA_TYPE not in ('int','bit') and COLUMN_NAME not like 'CategoryL%'
 and COLUMN_NAME not like 'LocationL%' and COLUMN_NAME not like '%Attribute%'and COLUMN_NAME not like '%path%'
 and COLUMN_NAME not in ('ERPUpdateType','QFAssetCode','SyncDateTime','DistributionID')


 go 
 Create Function [dbo].[GetSecondLevelLocationValue]
(
@LocationID int 
) returns int 
as 
begin
 declare @L2ID int 

 select @L2ID=Level2ID from LocationNewHierarchicalView where ChildID=@LocationID
 return @L2ID
end 
go 
Create table CategoryTypeTable 
(
	 CategoryTypeID int not null primary key identity(1,1),
	 CategoryTypeCode nvarchar(100) not null,
	 CategoryTypeName nvarchar(100) not null,
	 StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references User_LoginUserTable(UserID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references User_LoginUserTable(UserID),
	LastModifiedDateTime SmallDateTime NULL,
	 Attribute1 nvarchar(200) NULL,
	 Attribute2 nvarchar(200) NULL,
	 Attribute3 nvarchar(200) NULL,
	 Attribute4 nvarchar(200) NULL,
	 Attribute5 nvarchar(200) NULL,
	 Attribute6 nvarchar(200) NULL,
	 Attribute7 nvarchar(200) NULL,
	 Attribute8 nvarchar(200) NULL,
	 Attribute9 nvarchar(200) NULL,
	 Attribute10 nvarchar(200) NULL,
	 Attribute11 nvarchar(200) NULL,
	 Attribute12 nvarchar(200) NULL,
	 Attribute13 nvarchar(200) NULL,
	 Attribute14 nvarchar(200) NULL,
	 Attribute15 nvarchar(200) NULL,
	 Attribute16 nvarchar(200) NULL
)
go 
Create table CategoryTypeDescriptionTable
(
  CategoryTypeDescriptionID int not null primary key identity(1,1),
  CategoryTypeID int not null foreign key references CategoryTypeTable(CategoryTypeID),
  CategoryTypeDescription nvarchar(max) not null,
   LanguageID int not null foreign key references LanguageTable(LanguageID),
	 CreatedBy int not null foreign key references User_LoginUserTable(UserID),
	 CreatedDateTime SmallDatetime not null, 
	 LastModifiedBy int foreign key references User_LoginUserTable(UserID),
	 LastModifiedDateTime SmallDateTime NULL
)
go 
Alter table CategoryTable add CategoryTypeID int foreign key references CategoryTypeTable(CategoryTypeID)
go 
Alter table UserApprovalRoleMappingTable add CategoryTypeID int foreign key references CategoryTypeTable(CategoryTypeID)
go
Alter table UserApprovalRoleMappingTable add CategoryTypeID int foreign key references CategoryTypeTable(CategoryTypeID)
go
Alter table ApproveWorkflowTable add CategoryTypeID int foreign key references CategoryTypeTable(CategoryTypeID)
go
Alter table ApprovalHistoryTable add CategoryTypeID int foreign key references CategoryTypeTable(CategoryTypeID)
go 

create Procedure prc_GetSecondLevelLocationValue
(
@LocationID int 
)
as 
begin 
declare @L2ID int 

 select @L2ID=Level2ID from LocationNewHierarchicalView where ChildID=@LocationID
 select @L2ID as SecondLevelID
end 
go 
If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='InternalAssetTransferApproval')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('InternalAssetTransferApproval','InternalAssetTransferApproval',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Approval'),1,0);
End
Go

If not exists(SELECT MenuName FROM USER_MENUTABLE where MenuName='InternalAssetTransferApproval')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO,ParentTransactionID) Values(

'InternalAssetTransferApproval',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='InternalAssetTransferApproval'),'/InternalAssetTransferApproval/Index',
(Select MenuID from USER_MENUTABLE where MenuName='Approval' ),8,1);
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
	Left join UserApprovalRoleMappingTable D on  case when c.ApprovalLocationTypeID=5 then 
	a.FromLocationID else a.ToLocationID end =D.LocationID and D.ApprovalRoleID=c.ApprovalRoleID


	join PersonTable p on b.CreatedBy=p.PersonID
	join StatusTable s on a.StatusID=s.StatusID
	Left join (select * from DelegateRoleTable where  getdate() between EffectiveStartDate and EffectiveEndDate) DD on DD.EmployeeID=D.UserID
	where a.ApproveModuleID in (5,11) and a.StatusID=150  --and a.OrderNo<c.MaxOrderNo 
GO


if not exists(select EntityName from EntityTable where EntityName='UserRole')
Begin
 insert into EntityTable(EntityID,EntityName,QueryString) select max(entityID)+1,'UserRole','IPRC_ExcelImportUserRole' from EntityTable
end 
go 
create procedure IPRC_ExcelImportUserRole
(
 @UserName          nvarchar(100)=NULL,
 @UserRoleName      nvarchar(100)=NULL,
 @ImportTypeID	    int,
 @UserID		   int,
 @LanguageID       int=1,
 @CompanyID       int=1003
)
as 
begin 
  Declare @UserIDs int ,@RoleID int 
  If exists(Select username from User_LoginUserTable where UserName=@UserName)
  Begin
     select @UserIDs=userID from User_LoginUserTable where UserName=@UserName
  end 
  Else 
  Begin 
    SElect @UserName+'- given Username not available in db . ' as ReturnMessage
				Return 
  end 
  If exists(Select RoleName from User_RoleTable where RoleName=@UserRoleName)
  Begin
     select @RoleID=RoleID from  User_RoleTable where RoleName=@UserRoleName
  end 
  Else 
  Begin 
    SElect @UserRoleName+'- given Role Name not available in db . ' as ReturnMessage
				Return 
  end 
  If @UserIDs is not null and @RoleID is not null 
  Begin
     insert into User_UserRoleTable(UserID,RoleID) values(@UserIDs,@RoleID)
  end 
end 
go 

if not exists(select ImportField from ImportTemplateNewTable where ImportField='UserName' and entityid =(select EntityID from EntityTable where EntityName='UserRole') and ImportTemplateTypeID=1 )
Begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'UserName',1,1,1,100,NULL,0,100,1,0,NULL,EntityID
From EntityTable where EntityName='UserRole'
End 
go 
if not exists(select ImportField from ImportTemplateNewTable where ImportField='UserRoleName' and entityid =(select EntityID from EntityTable where EntityName='UserRole') and ImportTemplateTypeID=1 )
Begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'UserRoleName',2,1,1,100,NULL,0,100,1,0,NULL,EntityID
From EntityTable where EntityName='UserRole'
End 
go 

If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='CategoryType')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('CategoryType','CategoryType',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Master'),1,0);
End
Go
If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='CategoryType')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO,ParentTransactionID) Values(
'CategoryType',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='CategoryType'),'/MasterPage/Index?pageName=CategoryType',
(Select MenuID from USER_MENUTABLE where MenuName='Master' ),1,1);
End
Go

ALTER Procedure [dbo].[iprc_AMSExcelImportCustodian]
(
 @PersonCode nvarchar(100) =NULL,
 @FirstName nvarchar(100)=NULL,
 @LastName nvarchar(100)=NULL,
 @DOJ nvarchar(100)=NULL,
 @MobileNo nvarchar(100)=NULL,
 @Gender nvarchar(10)=NULL,
 @DepartmentCode nvarchar(100)=NULL,
 @EmailID nvarchar(100)=NULL,
 @DesignationName nvarchar(100)=NULL,
 @SignatureImage nvarchar(max)=NULL,
 @ImportTypeID int,
	@UserID int=1
) 
As 
Begin 
	DECLARE @DepartmentID INT,@DesignationID int ,@statusID int 
	DECLARE @ErrorToBeReturned nvarchar(max);
	DECLARE @ConfigValue nvarchar(max);
	Declare @ImportExcelNotAllowCreateReferenceFieldNewEntry nvarchar(10)
	Select @ImportExcelNotAllowCreateReferenceFieldNewEntry=ConfiguarationValue from configurationTable where ConfiguarationName='ImportExcelNotAllowCreateReferenceFieldNewEntry'
	set @personCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@personCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
	set @FirstName=REPLACE(@FirstName,'''','''')
	set @LastName=replace(@LastName,'''','''')
	DECLARE @MESSAGETABLE TABLE(TEXT NVARCHAR(MAX))
	Select @statusID=dbo.[fnGetActiveStatusID]()

	IF(LEN(@Gender) > 1) SET @Gender = SUBSTRING(@Gender, 1, 1)

	if(@DepartmentCode is not null and @DepartmentCode!='')
	BEGIN
		select @DepartmentID=DepartmentID from DepartmentTable where DepartmentCode=@DepartmentCode and StatusID=@statusID

		--Insert Department
		if(@ImportExcelNotAllowCreateReferenceFieldNewEntry='false')
		Begin
			IF (@DepartmentID IS NULL)
			BEGIN
				INSERT INTO DepartmentTable(DepartmentCode,StatusID, CreatedBy,DepartmentName,CreatedDateTime)
					select @DepartmentCode, @statusID, @UserID,@DepartmentCode,getdate()
				set @DepartmentID=@@IDENTITY;

				Insert into DepartmentDescriptionTable(DepartmentID,DepartmentDescription,LanguageID,CreatedBy,CreatedDateTime)
				Select C.DepartmentID,c.DepartmentCode,L.LanguageID,C.CreatedBy,C.CreatedDateTime From DepartmentTable C join 
						LanguageTable L on 1=1 left join DepartmentDescriptionTable MD on MD.DepartmentID=C.DepartmentID and MD.languageid=L.languageID 
						where MD.LanguageID is null
							and C.DepartmentCode=@DepartmentCode
			END
			ELSE
			BEGIN
				if(@DepartmentCode is not null and @DepartmentID is null)

				return @DepartmentCode+'- Department Is not available;'
			END
		END
		Else
		IF (@DepartmentID IS NULL)
		BEGIN
			return @DepartmentCode+'- Department Is not available;'
		END				 
	END

	if(@DesignationName is not null and @DesignationName!='')
	BEGIN
		select @DesignationID=DesignationID from DesignationTable where DesignationName=@DesignationName and statusID=@statusID
		--Insert Department
		if(@ImportExcelNotAllowCreateReferenceFieldNewEntry='false')
		Begin
			IF (@DesignationID IS NULL)
			BEGIN
				INSERT INTO DesignationTable(DEsignationCode,StatusID, CreatedBy,DesignationName,CreatedDateTime)
					select @designationName,@statusID, @UserID,@designationName,GETDATE()

				set @DepartmentID=@@IDENTITY;

				Insert into DesignationDescriptionTable(DesignationID,DesignationDescription,LanguageID,CreatedBy,CreatedDateTime)
				Select C.DEsignationID,c.DesignationCode,L.LanguageID,C.CreatedBy,C.CreatedDateTime From DesignationTable C join 
						LanguageTable L on 1=1 left join DesignationDescriptionTable MD on MD.DesignationID=C.DesignationID and MD.languageid=L.languageID 
						where MD.LanguageID is null
							and C.DesignationName=@designationName
			END
			ELSE
			BEGIN
				if(@DesignationName is not null and @DesignationID is null)
					return @DesignationName+'- Designation Is not available;'
			END
		END
		Else
		IF (@DesignationID IS NULL)
		BEGIN
			return @DesignationName+'- Designation Is not available;'
		END				 
	END

	if(@ImportTypeID=1)
		   BEGIN
		    if NOT EXISTS (SELECT * FROM PersonTable WHERE PersonCode = @PersonCode)
			BEGIN
			Declare @userIDs int ,@companyID int 
	        select top 1 @companyID=CompanyID from CompanyTable where StatusID=1
				INSERT INTO User_LoginUserTable(UserName,Password,PasswordSalt,LastActivityDate,LastLoginDate,LastLoggedInDate,ChangePasswordAtNextLogin,IsLockedOut,IsDisabled,IsApproved)
					VALUES(@PersonCode ,'Mod6/JMHjHeKXDkUK/zd7PfLlJg=','BZxI8E2lroNt28VMhZsyyaNZha8=',GETDATE(),GETDATE(),GETDATE(),1,0,0,1 )
                
				set @userIDs=@@IDENTITY
				INSERT INTO PersonTable(PersonID, PersonFirstName, PersonLastName, PersonCode, AllowLogin, DepartmentID, UserTypeID, StatusID, Culture,EmailID,MobileNo,DesignationID,DOJ,Gender,SignaturePath,CreatedBy,CreatedDateTime) 
					VALUES(@userIDs, @FirstName, @LastName, @PersonCode, 0, @DepartmentID, 3, 1, 'en-GB',@EmailID,@MobileNo,@DesignationID,case when @DOJ is not null then CONVERT(DATETIME,@DOJ,103) else null end,@Gender,@SignatureImage,@UserID,getdate())

					Insert into UserCompanyMappingTable(userid,CompanyID,StatusID) select @userIDs,@companyID,dbo.fnGetActiveStatusID()
			END
			else 
			Begin
			    INSERT INTO @MESSAGETABLE(TEXT)VALUES('Person code "' + @PersonCode + '" already exists' )
			End 
			End
			ELSE if(@ImportTypeID=2)
			
		BEGIN	
		if exists(select PersonCode from PersonTable where personcode=@PersonCode) 
		Begin 
		UPDATE PersonTable 
			SET PersonFirstName = case when @FirstName is not null then @FirstName else PersonFirstName end ,  
			PersonLastName=case when @LastName is not null then @LastName else PersonLastName end  ,
			EmailID=case when @EmailID is not null then @EmailID else EmailID end ,
			MobileNo=case when @MobileNo is not null then @MobileNo else mobileNo end ,
			DepartmentID=isnull(@DepartmentID,DepartmentID),
			Designationid=isnull(@DesignationID,Designationid),
			doj=case when @DOJ is not null then CONVERT(DATETIME,@DOJ,103) else doj end,
			gender=case when @Gender is not null then @Gender else Gender end ,
			SignaturePath=case when @SignatureImage is not null then @SignatureImage else SignaturePath end,
			LastModifiedBy=@UserID,
			LastModifiedDateTime=GETDATE()
			 WHERE PersonCode = @PersonCode
			 End 
			 Else 
			 Begin
			   INSERT INTO @MESSAGETABLE(TEXT)VALUES(@PersonCode + '- This PersonCode not available in PErsonTable')
			 End 

	End 
	DEclare @ReturnMessage nvarchar(max)

	Select @ReturnMessage = COALESCE(@ReturnMessage + ', ' + Text, Text)  from @MESSAGETABLE
			select @ReturnMessage as ReturnMessage

end 
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
  left join LocationnewHierarchicalView Old on b.FromLocationID=Old.ChildID
  left join LocationnewHierarchicalView new on b.ToLocationID=new.ChildID
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



ALTER Procedure [dbo].[prc_AssetTransferForApproval]
(
  @UserID int ,
  @ApprovalID int = null,
  @CompanyID int =  NULL,
  @LanguageID int = 1
)
as 
Begin 
    
	Select a.*,b.orderno as lvl , inidoc.FileName as InitorDoc,approvaldoc.FileName as ApprovalDoc 
	From TransactionTable a 
	join ApprovalHistoryTable b on a.TransactionID=b.TransactionID 
	left join (SELECT a.ObjectKeyID,
   STUFF((SELECT '; ' + US.Filename 
          FROM DocumentTable US
          WHERE US.ObjectKeyID = a.ObjectKeyID 
          ORDER BY Filename
          FOR XML PATH('')), 1, 1, '') [FileName]
FROM DocumentTable a join TransactionTable b on a.ObjectKeyID=b.TransactionID and a.TransactionType in ('AssetTransfer','InternalAssetTransfer')
where a.statusID = dbo.fnGetActiveStatusID()
GROUP BY a.ObjectKeyID)inidoc on inidoc.ObjectKeyID=a.TransactionID

left join (SELECT a.ObjectKeyID,
   STUFF((SELECT '; ' + US.Filename 
          FROM DocumentTable US
          WHERE US.ObjectKeyID = a.ObjectKeyID 
          ORDER BY Filename
          FOR XML PATH('')), 1, 1, '') [FileName]
FROM DocumentTable a join ApprovalHistoryTable b on a.ObjectKeyID=b.ApprovalHistoryID and a.TransactionType in ('AssetTransfer','InternalAssetTransfer') and b.ApproveModuleID in (5,11)
where a.statusID = dbo.fnGetActiveStatusID()
GROUP BY a.ObjectKeyID)approvaldoc on approvaldoc.ObjectKeyID=b.ApprovalHistoryID

Where b.ApproveModuleID in (5,11)  and a.transactionTypeID in (5,11) and b.statusID = 150 and b.ApprovalHistoryID=@ApprovalID
End 
go 




Alter table CategoryTypeTable add IsAllCategoryType bit default(0)
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
			a.Attribute14,a.Attribute15,a.Attribute16,CT.CategoryTypeName as CategoryType
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
ALTER VIEW [dbo].[AssetNewView] 
AS 
	
	SELECT A.AssetID,A.AssetCode,A.Barcode,A.RFIDTagCode,A.LocationID,A.DepartmentID,A.SectionID,A.CustodianID,A.SupplierID,A.AssetConditionID,A.PONumber,A.PurchaseDate,A.PurchasePrice,A.ComissionDate,A.WarrantyExpiryDate,
	A.DisposalReferenceNo,A.DisposedDateTime,A.DisposedRemarks,A.AssetRemarks,A.DepreciationClassID,A.DepreciationFlag,A.SalvageValue,A.VoucherNo,A.StatusID,A.CreatedBy,A.CreatedDateTime,A.LastModifiedBy,
	A.LastModifiedDateTime,A.AssetDescription,A.ReferenceCode,A.SerialNo,A.NetworkID,A.InvoiceNo,A.DeliveryNote,A.Make,A.Capacity,A.MappedAssetID,A.CreateFromHHT,A.DisposalValue,
	A.MailAlert,A.partialDisposalTotalValue,A.IsTransfered,A.CategoryID,A.TransferTypeID,A.UploadedDocumentPath,A.UploadedImagePath,A.AssetApproval,A.ReceiptNumber,A.InsertedToOracle,
	A.InvoiceDate,A.DistributionID,A.ProductID,A.CompanyID,A.SyncDateTime,A.Attribute1,A.Attribute2,A.Attribute3,A.Attribute4,A.Attribute5,A.Attribute6,A.Attribute7,A.Attribute8,A.Attribute9,A.Attribute10,A.Attribute11,A.Attribute12,
	A.Attribute13,A.Attribute14,A.Attribute15,A.Attribute16,A.ManufacturerID,A.ModelID,A.QFAssetCode,A.DOFPO_LINE_NUM,A.DOFMASS_ADDITION_ID,A.ERPUpdateType,A.DOFPARENT_MASS_ADDITION_ID,A.DOFFIXED_ASSETS_UNITS,A.zDOF_Asset_Updated,A.Latitude,A.Longitude,
	A.DisposalTypeID,c.CategoryName,A.CurrentCost,A.ProceedofSales,A.SoldTo,A.AllowTransfer,A.CostOfRemoval,
	c.CategoryNameHierarchy AS CategoryHierarchy, C.childCode AS CategoryCode,LV.ChildCode AS LocationCode,LV.LocationName,
	CASE WHEN LV.PID4 IS NULL OR LV.PID4 ='' THEN '' ELSE LV.PID4 END AS LocationHierarchy,D.DepartmentCode,d.DepartmentName AS DepartmentName,
	S.SectionCode,s.SectionName as SectionDescription,cu.PersonCode AS CustodianCode,Cu.PersonFirstName +''+Cu.PersonLastName AS CustodianName ,MDT.ModelName Model,
	AC.AssetConditionCode,AC.AssetConditionName AS AssetCondition, SU.PartyCode as SupplierCode,SU.PartyName as suppliername ,
	P.ProductCode,p.ProductName AS ProductName ,
	--MFDT.ManufacturerName Manufacturer,
	PE.PersonFirstName+'-'+PE.PersonLastName AS CreateBy, M.PersonFirstName+'-'+M.PersonLastName AS ModifedBy,
	CO.CompanyCode,CO.CompanyName AS CompanyName,
	--DED.DepreciationEndDay DepreciationEndDate,
	--depd.EndDay as DepreciationEndDate,
	--Dep.DepreciationEndDate  AS DepreciationEndDate,
	--'' as DepreciationEndDate,
	LV.L1Desc FirstLevelLocationName,LV.L2Desc SecondLevelLocationName,LV.L3Desc ThirdLevelLocationName, 
	LV.L4Desc FourthLevelLocationName, 	LV.L5Desc FifthLevelLocationName, 	LV.L6Desc SixthLevelLocationName, 
	C.L1Desc FirstLevelCategoryName, 	C.L2Desc SecondLevelCategoryName,	C.L3Desc ThirdLevelCategoryName,
	C.L4Desc FourthLevelCategoryName,C.L5Desc FifthLevelCategoryName,C.L6Desc SixthLevelCategoryName,
	--C.LanguageID,--isnull(A.PurchasePrice,0)-isnull(dep.AssetValueAfterDepreciation,0.00) as Accumulatedvalue,isnull(dep.AssetValueAfterDepreciation,0.00) as NetValue,
--	ISNULL(dep.AccumulatedValue,0.00) AS Accumulatedvalue,ISNULL(dep.NetValue,0.00) AS NetValue,
	--CASE WHEN A.NetbookValue is null and  ISNULL(dep.AccumulatedValue,0.00)=0 THEN ISNULL(A.PurchasePrice,0) ELSE ISNULL(A.NetbookValue,0.00) END AS NetValue,
	ST.Status,IsScanned,
	CAST(LV.Level1ID AS VARCHAR(50)) LocationL1 ,CAST(LV.Level2ID AS VARCHAR(50)) LocationL2,CAST(LV.Level3ID AS VARCHAR(50)) LocationL3,
	CAST(LV.Level4ID AS VARCHAR(50)) LocationL4,CAST(LV.Level5ID AS VARCHAR(50)) LocationL5,CAST(LV.Level6ID AS VARCHAR(50))  LocationL6,
	CAST(C.Level1ID AS VARCHAR(50)) CategoryL1,CAST(C.Level2ID AS VARCHAR(50)) CategoryL2,CAST(C.Level3ID AS VARCHAR(50)) CategoryL3,
	CAST(C.Level4ID AS VARCHAR(50)) CategoryL4,CAST(C.Level5ID AS VARCHAR(50)) CategoryL5,CAST(C.Level6ID AS VARCHAR(50)) CategoryL6,
--	CTT.CustodianType,
	--Category Attributes
	c.Attribute1 AS CategoryAttribute1,c.Attribute2 AS CategoryAttribute2,c.Attribute3 AS CategoryAttribute3,c.Attribute4 AS CategoryAttribute4,
	c.Attribute5 AS CategoryAttribute5 ,c.Attribute6 AS CategoryAttribute6,
	c.Attribute7 AS CategoryAttribute7,c.Attribute8 AS CategoryAttribute8,c.Attribute9 AS CategoryAttribute9,c.Attribute10 AS CategoryAttribute10,
	c.Attribute11 as CategoryAttribute11,c.Attribute12 as CategoryAttribute12,c.Attribute13 as CategoryAttribute13,c.Attribute14 as CategoryAttribute14,
	c.Attribute15 as CategoryAttribute15,c.Attribute16 as CategoryAttribute16,
	--Location Attributes
	LV.Attribute1 AS LocationAttribute1,LV.Attribute2 AS LocationAttribute2,LV.Attribute3 AS LocationAttribute3,LV.Attribute4 AS LocationAttribute4,
	LV.Attribute5 AS LocationAttribute5 ,LV.Attribute6 AS LocationAttribute6,
	LV.Attribute7 AS LocationAttribute7,LV.Attribute8 AS LocationAttribute8,LV.Attribute9 AS LocationAttribute9,LV.Attribute10 AS LocationAttribute10,
	LV.Attribute11 AS LocationAttribute11,LV.Attribute12 AS LocationAttribute12,LV.Attribute13 AS LocationAttribute13,LV.Attribute14 AS LocationAttribute14,
	LV.Attribute15 AS LocationAttribute15,LV.Attribute16 AS LocationAttribute16,
	--a.DisposalTransactionID AS DisposalTransactionID,
	A.Attribute17,A.Attribute18,Attribute19,Attribute20,Attribute21,Attribute22,Attribute23,Attribute24,Attribute25,Attribute26,
	Attribute27,Attribute28,Attribute29,Attribute30,Attribute31,Attribute32,Attribute33,Attribute34,Attribute35,Attribute36,Attribute37,Attribute38,Attribute39,Attribute40,lv.LocationType,c.CategoryType
	--UsefulAssetLife, AssetAge
	
	 FROM AssetTable A 
	LEFT JOIN CategoryNewHierarchicalView AS c ON A.CategoryID = c.ChildId --AND C.LanguageID IN (1)
	LEFT JOIN ProductTable P ON P.ProductID=A.ProductID AND c.childid=p.categoryID
	LEFT JOIN CompanyTable CO ON CO.CompanyID=A.CompanyID 
	LEFT JOIN StatusTable ST ON A.StatusID=ST.StatusID	
	LEFT JOIN LocationNewHierarchicalView LV ON A.LocationID = LV.ChildId --AND LV.LanguageID IN (1)
	LEFT JOIN DepartmentTable D ON D.DepartmentID=A.DepartmentID 
	LEFT JOIN SectionTable S ON S.SectionID=A.SectionID  
	LEFT JOIN PersonTable Cu ON A.CustodianID=Cu.PersonID 
	LEFT JOIN PersonTable PE ON A.CreatedBy=PE.PersonID 
	LEFT JOIN PersonTable M ON A.LastModifiedBy=M.PersonID 
	LEFT JOIN AssetConditionTable AC ON a.AssetConditionID=AC.AssetConditionID
	LEFT JOIN PartyTable SU ON SU.PartyID = A.SupplierID  and partyTypeID=2
	left join ModelTable MDT on A.ModelID=mdt.ModelID
	

	
GO
Create Procedure prc_ValidateAssetCategoryMapping
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

  Declare @approvalCnt int ,@AssetCnt int ,@workflowCnt int 
  Select @AssetCnt=Count(*) from @TransactionLineItemTable 
  Select @workflowCnt=Count(*) from ApproveWorkflowLineItemTable where ApproveWorkFlowID=@ApprovalWorkFlowID

  select @approvalCnt=count(*) from ApproveWorkflowLineItemTable a 
  Left join @TransactionLineItemTable T on a.ApproveWorkFlowID=T.ApprovalWorkFlowID
  Left join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
  Left join CategoryTypeTable CT on T.CategoryType=Ct.categoryTypeName
  Left join UserApprovalRoleMappingTable D on  case when b.ApprovalLocationTypeID=5 then 
	T.FromLocationL2 else T.ToLocationL2 end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID and D.CategoryTypeID=CT.CategoryTypeID
  where ApproveWorkFlowID=@ApprovalWorkFlowID

  if(isnull(@AssetCnt,0)*isnull(@workflowCnt,0)=isnull(@approvalCnt,0))
  begin 
    Select 1 as result
  end 
  else 
  begin 
    select 0 as result 
  end 


end 
go 

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
NULL ApprovalURL,
--Notification supporting fields
		A.TransactionID SYSDataID1, AH.ApprovalHistoryID SYSDataID2, ah.CreatedBy SYSDataID3,
		 
		  UGRP.EMailIDs SYSToAddresses,
		  '' SYSCCAddresses, '' SYSBCCAddresses,
		NULL SYSToMobileNos, NULL SYSWhatsAppMobileNos,(SELECT EMAILID FROM PERSONTABLE WHERE PERSONID=AH.CREATEDBY) as ApprovedBy,convert(nvarchar(100),ah.CreatedDateTime,103) as ApprovedDate
		
  From TransactionTable a 
  join TransactionLineItemTable b on a.TransactionID=b.TransactionID 
  join AssetNewView a1 on b.AssetID=a1.AssetID 
  left join ApprovalHistoryTable AH on a.TransactionID=AH.TransactionID 
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
  
  LEFT JOIN (SELECT ApprovalRoleID, --STRING_AGG(EmailID, ';') EMailIDs,LocationID
stuff((select ';'+Emailid from UserRightValueView PL where PL.ApprovalRoleID=p2.ApprovalRoleID group by EMailID  FOR XML PATH('')), 1, 1, '') as EMailIDs--,
  --LocationID

			FROM UserRightValueView P2 where RightName = 'AssetTransferApproval'
				AND ApprovalRoleID IS NOT NULL
				AND EmailID IS NOT NULL
			GROUP BY ApprovalRoleID) UGRP ON UGRP.ApprovalRoleID = AH.ApprovalRoleID

WHERE  a.TransactionTypeID=5

GO

ALTER view [dbo].[rvw_AssetRetirement] 
as 
Select * from AssetNewView where StatusID  in (250)
GO

ALTER view [dbo].[rvw_AssetSummary] 
as 
Select * from AssetNewView  where StatusID not in (250,500)
GO



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
  left join LocationnewHierarchicalView Old on b.FromLocationID=Old.ChildID
  left join LocationnewHierarchicalView new on b.ToLocationID=new.ChildID
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


ALTER view [dbo].[rvw_AssetTransfer]
		as 
			select * from TransactionLineItemViewForTransfer where TransactionTypeID=5
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
		Left join UserApprovalRoleMappingTable D on  a.FromLocationID=D.LocationID and D.ApprovalRoleID=c.ApprovalRoleID
		and case when CT.IsAllCategoryType=1 then D.CategoryTypeID else a.CategorytypeID end =D.CategoryTypeID 
		--case when c.ApprovalLocationTypeID=5 then a.FromLocationID else a.ToLocationID end =D.LocationID and D.ApprovalRoleID=c.ApprovalRoleID
	--join (select TransactionID,ApproveModuleID,max(Orderno) as MaxOrderNo from ApprovalHistoryTable  where ApproveModuleID=10 and StatusID=150 
	--group by TransactionID,ApproveModuleID )c on a.TransactionID=c.TransactionID
	join PersonTable p on b.CreatedBy=p.PersonID
	join StatusTable s on a.StatusID=s.StatusID
	Left join (select * from DelegateRoleTable where  getdate() between EffectiveStartDate and EffectiveEndDate) DD on DD.EmployeeID=D.UserID
	where a.ApproveModuleID=10 and a.StatusID=150 --and a.OrderNo<c.MaxOrderNo
	
GO


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
	where a.StatusID = dbo.fnGetActiveStatusID() and a.TransactionTypeID in (5,11)
GO


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
	left join CategoryTypeTable CT on a.CategoryTypeID=CT.CategoryTypeID
	Left join UserApprovalRoleMappingTable D on  case when c.ApprovalLocationTypeID=5 then 
	a.FromLocationID else a.ToLocationID end =D.LocationID and D.ApprovalRoleID=c.ApprovalRoleID  
	and case when CT.IsAllCategoryType=1 then D.CategoryTypeID else a.CategorytypeID end =D.CategoryTypeID 


	join PersonTable p on b.CreatedBy=p.PersonID
	join StatusTable s on a.StatusID=s.StatusID
	Left join (select * from DelegateRoleTable where  getdate() between EffectiveStartDate and EffectiveEndDate) DD on DD.EmployeeID=D.UserID
	where a.ApproveModuleID in (5,11) and a.StatusID=150  --and a.OrderNo<c.MaxOrderNo 
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

   Declare @validationTable Table(OrderNo int,ApprovalRoleID int) 

  Declare @approvalCnt int ,@AssetCnt int ,@workflowCnt int 
  Select @AssetCnt=Count(*) from @TransactionLineItemTable 

  Select @workflowCnt=Count(*) from ApproveWorkflowLineItemTable where ApproveWorkFlowID=@ApprovalWorkFlowID
   insert into @validationTable(OrderNo,ApprovalRoleID)
 select a.OrderNo,a.ApprovalRoleID from ApproveWorkflowLineItemTable a 
  Left join @TransactionLineItemTable T on a.ApproveWorkFlowID=T.ApprovalWorkFlowID
  Left join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
  Left join CategoryTypeTable CT on T.CategoryType=Ct.categoryTypeName
  Left join UserApprovalRoleMappingTable D on  case when b.ApprovalLocationTypeID=5 then 
	T.FromLocationL2 else T.ToLocationL2 end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID and D.CategoryTypeID=CT.CategoryTypeID
  where ApproveWorkFlowID=@ApprovalWorkFlowID and CT.statusID=[dbo].fnGetActiveStatusID()
  group by a.OrderNo,a.ApprovalRoleID

    select @approvalCnt=count(a.OrderNo) from  @validationTable a
  join ApproveWorkflowLineItemTable b on a.approvalroleID=b.ApprovalRoleID and a.OrderNo=b.OrderNo
  where b.ApproveWorkFlowID=@ApprovalWorkFlowID

  if(isnull(@workflowCnt,0)=isnull(@approvalCnt,0))
  begin 
    Select 1 as result
  end 
  else 
  begin 
    select 0 as result 
  end 


end 
go 



if not exists(select EntityName from EntityTable where EntityName='UserCategoryMapping')
Begin
 insert into EntityTable(EntityID,EntityName,QueryString) select max(entityID)+1,'UserCategoryMapping','Iprc_UserCategoryMapping' from EntityTable
end 
go 

if not exists(select ImportField from ImportTemplateNewTable where ImportField='UserCode' and entityid =(select EntityID from EntityTable where EntityName='UserCategoryMapping') and ImportTemplateTypeID=1 )
Begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'UserCode',1,1,1,100,NULL,0,100,1,0,NULL,EntityID
From EntityTable where EntityName='UserCategoryMapping'
End 
go 
if not exists(select ImportField from ImportTemplateNewTable where ImportField='CategoryCode' and entityid =(select EntityID from EntityTable where EntityName='UserCategoryMapping') and ImportTemplateTypeID=1 )
Begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'CategoryCode',2,1,1,100,NULL,0,100,1,0,NULL,EntityID
From EntityTable where EntityName='UserCategoryMapping'
End 
go 
Create Procedure [dbo].[Iprc_UserCategoryMapping]
(
 @UserCode     nvarchar(100)  = NULL, 
 @CategoryCode nvarchar(100)  = null,
 @ImportTypeID int,
 @UserID int

)
as 
Begin 
set @CategoryCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@CategoryCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
set @UserCode= REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@UserCode, '!', ''), '#', ''), '$', ''),'@', ''),'%', ''),' ', '');
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
IF (@UserCode is not NULL and @UserCode!='')
  BEGIN
  IF EXISTS(SELECT PersonCode FROM PersonTable WHERE PersonCode=@UserCode and StatusID=@statusID)
	BEGIN  
		SELECT @PersonID=PersonID FROM  PersonTable WHERE PersonCode=@UserCode and StatusID=@statusID
	END
Else 
Begin 
     SElect @UserCode+'- User Code not avaliable in table please add it ' as ReturnMessage
	Return 
End 

End 
If not exists (select childid from CategoryNewHierarchicalView  where childid=@categoryID and level=2) 
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
   SElect @UserCode+'-'+@CategoryCode+'-Already Exists' as ReturnMessage
	Return 
 
end 
End 
End 
end 
go 
if not exists(select ImportField from ImportTemplateNewTable where ImportField='CategoryTypeCode' and entityid =(select EntityID from EntityTable where EntityName='UserApprovalRoleMapping') and ImportTemplateTypeID=1 )
Begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'CategoryTypeCode',4,1,1,100,NULL,0,100,1,0,NULL,EntityID
From EntityTable where EntityName='UserApprovalRoleMapping'
End 
go 

if not exists(select ImportField from ImportTemplateNewTable where ImportField='LocationCode' and entityid =(select EntityID from EntityTable where EntityName='UserApprovalRoleMapping') and ImportTemplateTypeID=2 )
Begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'LocationCode',1,1,1,100,NULL,0,100,2,0,NULL,EntityID
From EntityTable where EntityName='UserApprovalRoleMapping'
End 
go
if not exists(select ImportField from ImportTemplateNewTable where ImportField='UserCode' and entityid =(select EntityID from EntityTable where EntityName='UserApprovalRoleMapping') and ImportTemplateTypeID=2 )
Begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'UserCode',2,1,1,100,NULL,0,100,2,0,NULL,EntityID
From EntityTable where EntityName='UserApprovalRoleMapping'
End 
go
if not exists(select ImportField from ImportTemplateNewTable where ImportField='ApprovalRoleCode' and entityid =(select EntityID from EntityTable where EntityName='UserApprovalRoleMapping') and ImportTemplateTypeID=2 )
Begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'ApprovalRoleCode',3,1,1,100,NULL,0,100,2,0,NULL,EntityID
From EntityTable where EntityName='UserApprovalRoleMapping'
End 
go
if not exists(select ImportField from ImportTemplateNewTable where ImportField='CategoryTypeCode' and entityid =(select EntityID from EntityTable where EntityName='UserApprovalRoleMapping') and ImportTemplateTypeID=2 )
Begin 
insert into ImportTemplateNewTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,
DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
Select 'CategoryTypeCode',4,1,1,100,NULL,0,100,2,0,NULL,EntityID
From EntityTable where EntityName='UserApprovalRoleMapping'
End 
go

update ImportTemplateNewTable set ImportField='PersonCode' where ImportField='UserCode' and EntityID in (select EntityID from EntityTable where EntityName in ('UserApprovalRoleMapping','UserLocationMapping','UserCategoryMapping'))
go 

ALTER Procedure [dbo].[Iprc_UserLocationMapping]
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
If not exists (select childid from LocationNewHierarchicalView  where childid=@LocationID and level=2) 
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
go 
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
If not exists (select childid from CategoryNewHierarchicalView  where childid=@categoryID and level=2) 
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
go 


ALTER Procedure [dbo].[Iprc_UserApprovalRoleMapping]
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

If not exists (select childid from LocationNewHierarchicalView  where childid=@LocationID and level=2) 
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
go 

Create Procedure [dbo].[prc_ValidateForTransaction]
(
   @FromAssetID				nvarchar(max)	 = NULL,
   @ToLocationID			int				 = NULL,
   @FromLocationTypeID		int				 = NULL,
   @ToLocationTypeID		Int				 = NULL,
   @ApprovalWorkFlowID		int              = NULL
)
as 
Begin
  Declare @ErrorMsg nvarchar(max),@UpdateCount int,@AprpovalCnt int ,@StausID int 
  select @StausID=[dbo].fnGetActiveStatusID()
  
  Declare @updateTable table(updateRole bit,ApprovalRoleID int)

  declare @TransactionLineItemTable table (AssetID int,FromLocationL2 int ,ToLocationL2 int,
  LocationType nvarchar(100),CategoryType nvarchar(100),ApprovalWorkFlowID int )
  
  Select @AprpovalCnt=count(OrderNo) from 
  ApproveWorkflowTable a join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
  where a.StatusID=1 and b.StatusID=1 and a.ApproveWorkflowID=@ApprovalWorkFlowID

  --Declare @ApprovalUserTable Table ()
  insert into @TransactionLineItemTable(AssetID,FromLocationL2,ToLocationL2,LocationType,CategoryType,ApprovalWorkFlowID)
  select AssetID,LocationL2,@ToLocationID,LocationType,categorytype,@ApprovalWorkFlowID
  From AssetNewView where assetid in (select value from Split(@fromAssetID,',')) 

  insert into @updateTable(updateRole,ApprovalRoleID)
  Select case when a.ApproveModuleID=5 or a.ApproveModuleID=11 then UpdateDestinationLocationsForTransfer else UpdateRetirementDetailsForEachAssets end as updateRole, 
  b.ApprovalRoleID FRom ApproveWorkflowTable a 
	  join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
	  join ApprovalRoleTable C on b.ApprovalRoleID=C.ApprovalRoleID
	  where a.ApproveWorkflowID=@ApprovalWorkFlowID and a.StatusID=@StausID and b.StatusID=@StausID and c.StatusID=@StausID
  
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

    if(select isnull(count(a.OrderNo),0) from  @validationTable a
  join ApproveWorkflowLineItemTable b on a.approvalroleID=b.ApprovalRoleID and a.OrderNo=b.OrderNo
  where b.ApproveWorkFlowID=@ApprovalWorkFlowID and b.StatusID=@StausID )=@AprpovalCnt
  Begin 
     set @ErrorMsg ='Approval role not assigned to the user'
  end 

  
 
 --select a.OrderNo,a.ApprovalRoleID from ApproveWorkflowLineItemTable a 
 --  join @TransactionLineItemTable T on a.ApproveWorkFlowID=T.ApprovalWorkFlowID
 --  join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
 --  join CategoryTypeTable CT on T.CategoryType=Ct.categoryTypeName
 --  join UserApprovalRoleMappingTable D on  case when b.ApprovalLocationTypeID=5 then 
	--T.FromLocationL2 else T.ToLocationL2 end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID and D.CategoryTypeID=CT.CategoryTypeID
 -- where ApproveWorkFlowID=@ApprovalWorkFlowID and CT.statusID=[dbo].fnGetActiveStatusID()
 -- group by a.OrderNo,a.ApprovalRoleID
 
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

ALTER View [dbo].[UserApprovalRoleMappingView] 
	as 
	select x.*,p1.PersonFirstName+'-'+p1.PersonLastName  as personName from (
	select isnull(DD.DelegatedEmployeeID,a.UserID) as UserID,a.ApprovalRoleID,LocationID,CategoryTypeID
	from UserApprovalRoleMappingTable a 
	--join PersonTable p1 on a.UserID=p1.PersonID 
	Left join (select * from DelegateRoleTable 
			where  getdate() between EffectiveStartDate and EffectiveEndDate) DD on DD.EmployeeID=a.UserID
		where a.StatusID = dbo.fnGetActiveStatusID() ) x 
	join PersonTable p1 on x.UserID=p1.PersonID 
	group by x.UserID,x.ApprovalRoleID,p1.PersonFirstName,p1.PersonLastName,LocationID,CategoryTypeID
GO

ALTER View [dbo].[ApprovalHistoryView] 
  as 
  Select b.ApprovalRoleName,case when c.StatusID = dbo.fnGetActiveStatusID() then 'Approved' else c.Status end as ApprovalStatus,p.PersonFirstName+'-'+p.PersonLastName as ApprovedBy ,a.LastModifiedDateTime as ApprovedDatetime ,a.*
  ,[FileName] as DocumentName,
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
Left join (
select t2.approvalRoleID,t2.LocationID,t2.CategoryTypeID, STUFF((select ','+PersonName 
from UserApprovalRoleMappingView t1
where t1.approvalRoleID=t2.approvalRoleID and t1.LocationID=t2.LocationID and t1.CategorytypeID=t2.CategorytypeID
order by personName 
 FOR XML PATH('')), 1, 1, '') personName
 from UserApprovalRoleMappingView t2  group by t2.approvalRoleID,t2.LocationID,t2.CategorytypeID
) t3 on b.ApprovalRoleID=t3.ApprovalRoleID and case when b.ApprovalLocationTypeID=5 then a.FromLocationID else a.ToLocationID end =t3.LocationID  --and ( a.FromLocationID=t3.LocationID or  a.ToLocationID=t3.LocationID)
and case when CT.IsAllCategoryType=1 then t3.CategoryTypeID else a.CategorytypeID end =t3.CategoryTypeID 
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
  Declare @ErrorMsg nvarchar(max),@UpdateCount int,@AprpovalCnt int ,@StausID int 
  select @StausID=[dbo].fnGetActiveStatusID()
  
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

  
 
 --select a.OrderNo,a.ApprovalRoleID from ApproveWorkflowLineItemTable a 
 --  join @TransactionLineItemTable T on a.ApproveWorkFlowID=T.ApprovalWorkFlowID
 --  join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
 --  join CategoryTypeTable CT on T.CategoryType=Ct.categoryTypeName
 --  join UserApprovalRoleMappingTable D on  case when b.ApprovalLocationTypeID=5 then 
	--T.FromLocationL2 else T.ToLocationL2 end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID and D.CategoryTypeID=CT.CategoryTypeID
 -- where ApproveWorkFlowID=@ApprovalWorkFlowID and CT.statusID=[dbo].fnGetActiveStatusID()
 -- group by a.OrderNo,a.ApprovalRoleID
 
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

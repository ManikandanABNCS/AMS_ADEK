
IF OBJECT_ID('PeriodTable') IS NULL
BEGIN
  Create table PeriodTable
  (
    PeriodID int not null primary key identity(1,1),
	PeriodCode nvarchar(100) Not NULL,
	PeriodName nvarchar(200) Not NULL,
	Year int not null,
	StartDate smallDatetime not null,
	EndDate SmallDatetime not null,
	StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references User_LoginUserTable(UserID),
	CreatedDateTime SmallDatetime not null,
	LastModifiedBy int  null foreign key references User_LoginUserTable(UserID),
	LastModifiedDateTime SmallDatetime
  )
End 
Go 

IF OBJECT_ID('DepreciationMethodTable') IS NULL
BEGIN
  Create table DepreciationMethodTable
  (
	DepreciationMethodID int not null primary key identity(1,1),
	DepreciationMethodCode nvarchar(100) Not null,
	DepreciationMethodName nvarchar(100) Not NULL
  )
End 
Go 
IF OBJECT_ID('DepreciationClassTable') IS NULL
BEGIN
  Create table DepreciationClassTable
  (
     DepreciationClassID  int not null primary key identity(1,1),
	 DepreciationClassCode nvarchar(100) Not NULL,
	 DepreicationClassName nvarchar(100) not null, 
	 DepreciationMethodID int not null foreign key references DepreciationMethodTable(DepreciationMethodID),
	 StatusID int not null foreign key references StatusTable(StatusID),
	 CreatedBy int not null foreign key references User_LoginUserTable(UserID),
	 CreatedDateTime SmallDatetime not null,
	 LastModifiedBy int  null foreign key references User_LoginUserTable(UserID),
	 LastModifiedDateTime SmallDatetime
  )
End 
Go 
IF OBJECT_ID('DepreciationClassLineItemTable') IS NULL
BEGIN
  Create table DepreciationClassLineItemTable
  (
     DepreciationClassLineItemID  int not null primary key identity(1,1),
	 DepreciationClassID int not null foreign key references DepreciationClassTable(DepreciationClassID),
	 AssetStartValue nvarchar(100) not null, 
	 AssetEndValue nvarchar(100) not null, 
	 Duration nvarchar(100)  null,
	 DepreciationPercentage decimal(18,5) NULL
  )
End 
Go 
IF OBJECT_ID('DepreciationTable') IS NULL
BEGIN
  Create table DepreciationTable
  (
    DepreciationID int not null primary key identity(1,1),
	PeriodID int not null foreign key references PeriodTable(PeriodID),
	CompanyID int not null foreign key references CompanyTable(CompanyID),
	DepreciationDoneBy int not null foreign key references User_LoginUserTable(UserID),
	DepreciationDoneDate SmallDateTime not null,
    IsDepreciationApproval bit default(0) not null,
	DepreciationApprovedDate SmallDateTime null,
	DepreciationApprovedBy int null foreign key references User_LoginUserTable(UserID),
	StatusID int not null foreign key references StatusTable(StatusID),
    IsDeletedApproval bit default(0) null,
	DeletedApprovedDate SmallDatetime NULL,
	DeletedApprovedBy int null foreign key references User_LoginUserTable(UserID),
	DeletedDoneBy int null foreign key references User_LoginUserTable(UserID),
	DeletedDoneDate SmallDateTime NULL,
	IsReclassification bit default(0) NULL
  )
End 
Go 
IF OBJECT_ID('DepreciationLineItemTable') IS NULL
BEGIN
  Create table DepreciationLineItemTable
  (
    DepreciationLineItemID int not null Primary key identity(1,1),
	DepreciationID int not null foreign key references DepreciationTable(DepreciationID),
	DepreciationClassID int not null foreign key references DepreciationClassTable(DepreciationClassID),
	DepreciationClassLineItemID int not null foreign key references DepreciationClassLineItemTable(DepreciationClassLineItemID),
	AssetID int not null foreign key references AssetTable(AssetID),
	DepreciationValue Decimal(18,5)  NULL,
	AssetValueAfterDepreciation Decimal(18,5)  NULL,
	AssetValueBeforeDepreciation Decimal(18,5)  NULL,
	LastCategoryID int not null foreign key references CategoryTable(CategoryID),
	CategoryL2 int not null foreign key references CategoryTable(CategoryID),
	IsReclassification bit default(0) NULL, 
	ReclassificationValue  bit default(0) NULL
  )
End 
Go 

create view rvw_Depreciation
as
select A.*,0 as PeriodID, 
ISNULL(PURCHASEPRICE-DepreciationValue,0) as NetValue,DepreciationValue as DepreciationValue
From AssetView A 

Join (select DL.AssetID,sum(DepreciationValue) as DepreciationValue from  DepreciationLineItemTable DL 
	Join DepreciationTable DT on DL.DepreciationID=DT.DepreciationID and DT.StatusID<>3
Join PeriodTable P on DT.PeriodID=p.PeriodID 
where isnull(DT.IsDepreciationApproval,0)=1 and isnull(DT.IsDeletedApproval,0)= 0
group by DL.AssetID) b  on a.AssetID=B.AssetID
where A.StatusID=50
go 
ALTER View [dbo].[ApprovalHistoryView] 
  as 
  Select b.ApprovalRoleName,case when c.StatusID=50 then 'Approved' else c.Status end as ApprovalStatus,p.PersonFirstName+'-'+p.PersonLastName as ApprovedBy ,a.LastModifiedDateTime as ApprovedDatetime ,a.*
  ,[FileName] as DocumentName,
  Case when p.PersonID is not null then p.PersonFirstName+'-'+p.PersonLastName else t3.personName end as UserName
 From ApprovalHistoryTable a 

  join ApprovalRoleTable b on a.ApprovalRoleID=b.ApprovalRoleID
  left join StatusTable c on a.StatusID=c.StatusID
  left join PersonTable p on a.LastModifiedBy=p.PersonID

  left join (SELECT a.ObjectKeyID,
   STUFF((SELECT '; ' + US.DocumentName 
          FROM DocumentTable US
          WHERE US.ObjectKeyID = a.ObjectKeyID and us.TransactionType  like '%Approval%'
          ORDER BY Filename
          FOR XML PATH('')), 1, 1, '') [FileName]
FROM DocumentTable a join ApprovalHistoryTable b on a.ObjectKeyID=b.ApprovalHistoryID and a.TransactionType like '%Approval%' 
GROUP BY a.ObjectKeyID) approvaldoc on a.ApprovalHistoryID=approvaldoc.ObjectkeyID 
Left join (
select t2.approvalRoleID, STUFF((select ','+PersonName 
from UserApprovalRoleMappingView t1
where t1.approvalRoleID=t2.approvalRoleID
order by personName 
 FOR XML PATH('')), 1, 1, '') personName
 from UserApprovalRoleMappingView t2  group by t2.approvalRoleID
) t3 on b.ApprovalRoleID=t3.ApprovalRoleID

 
GO
select * from LocationTypeTable

IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'LocationTypeCode' AND Object_ID = Object_ID(N'LocationTypeTable'))
Begin 
alter table LocationTypeTable add  LocationTypeCode nvarchar(100)  null 
END
GO
IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'StatusID' AND Object_ID = Object_ID(N'LocationTypeTable'))
Begin 
alter table LocationTypeTable add  StatusID int 
END
GO
IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'CreatedBy' AND Object_ID = Object_ID(N'LocationTypeTable'))
Begin 
alter table LocationTypeTable add  CreatedBy int 
END
GO
IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'CreatedDateTime' AND Object_ID = Object_ID(N'LocationTypeTable'))
Begin 
alter table LocationTypeTable add  CreatedDateTime smalldatetime 
END
GO
IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'LastModifiedBy' AND Object_ID = Object_ID(N'LocationTypeTable'))
Begin 
alter table LocationTypeTable add  LastModifiedBy int 
END
GO
IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'LastModifiedDateTime' AND Object_ID = Object_ID(N'LocationTypeTable'))
Begin 
alter table LocationTypeTable add  LastModifiedDateTime smalldatetime 
END
GO
EXEC sp_RENAME 'LocationTypeTable.LocationType' , 'LocationTypeName', 'COLUMN'

--select * from LocationTypeTable
update LocationTypeTable set LocationtypeCode=LocationTypeName,StatusID=50,CreatedBy=2,CreatedDateTime=GETDATE()
where LocationtypeCode is null 
go 

IF  EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'LocationTypeCode' AND Object_ID = Object_ID(N'LocationTypeTable'))
Begin 
alter table LocationTypeTable alter column  LocationTypeCode nvarchar(100) not null 
END
GO
IF  EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'LocationTypeName' AND Object_ID = Object_ID(N'LocationTypeTable'))
Begin 
alter table LocationTypeTable alter column  LocationTypeName nvarchar(100) not null 
END
GO

 IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_LocationTypeTable_StatusID')
    BEGIN
    ALTER TABLE dbo.LocationTypeTable ADD CONSTRAINT FK_LocationTypeTable_StatusID foreign key (StatusID) references StatusTable(StatusID)
    END
	go 
	IF  EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'StatusID' AND Object_ID = Object_ID(N'LocationTypeTable'))
Begin 
alter table LocationTypeTable alter column  StatusID int not null
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_LocationTypeTable_CreatedBy')
    BEGIN
    ALTER TABLE dbo.LocationTypeTable ADD CONSTRAINT FK_LocationTypeTable_CreatedBy foreign key (CreatedBy) references user_LoginUserTable(UserID)
    END
	go 

	IF  EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'CreatedBy' AND Object_ID = Object_ID(N'LocationTypeTable'))
Begin 
alter table LocationTypeTable alter column  CreatedBy int not null
END
GO
If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='LocationType')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('LocationType','LocationType',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Master'),1,0);
End
Go

If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='LocationType')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'LocationType',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='LocationType'),'/MasterPage/Index?pageName=LocationType',
(Select MenuID from USER_MENUTABLE where MenuName='Master' ),10);
End
Go

select * from ApprovalRoleTable


IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'UpdateDestinationlocationsForTransfer' AND Object_ID = Object_ID(N'ApprovalRoleTable'))
Begin 
alter table ApprovalRoleTable add  UpdateDestinationlocationsForTransfer  BIT DEFAULT(0)
END
GO

IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'UpdateRetirementDetailsForEachAssets' AND Object_ID = Object_ID(N'ApprovalRoleTable'))
Begin 
alter table ApprovalRoleTable add  UpdateRetirementDetailsForEachAssets BIT DEFAULT(0)
END
GO


create view UserApprovalRoleMappingView 
	as 
	select a.UserID,a.ApprovalRoleID,p1.PersonFirstName+'-'+p1.PersonLastName  as personName
	from UserApprovalRoleMappingTable a 
	join PersonTable p1 on a.UserID=p1.PersonID 
	where a.StatusID=50 
	group by a.UserID,a.ApprovalRoleID,p1.PersonFirstName,p1.PersonLastName 
	go

  ALTER View [dbo].[ApprovalHistoryView] 
  as 
  Select b.ApprovalRoleName,c.Status as ApprovalStatus,p.PersonFirstName+'-'+p.PersonLastName as ApprovedBy ,a.LastModifiedDateTime as ApprovedDatetime ,a.*
  ,[FileName] as DocumentName,t3.personName as UserName From ApprovalHistoryTable a 

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



ALTER VIEW [dbo].[LocationHierarchicalView]
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
			a.Attribute13, a.Attribute14, a.Attribute15, a.Attribute16,LT.LocationTypeName as LocationType
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
IF OBJECT_ID('RetirementTypeTable') IS NULL
BEGIN
  Create table RetirementTypeTable
  (
    RetirementTypeID int not null primary key identity(1,1),
	RetirementTypeCode nvarchar(100) Not NULL,
	RetirementTypeName nvarchar(100) Not NULL,
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
End 
Go 

IF OBJECT_ID('RetirementTypeDescriptionTable') IS NULL
BEGIN
  Create table RetirementTypeDescriptionTable
  (
     RetirementTypeDescriptionID int not null primary key identity(1,1),
	 RetirementTypeID int not null foreign key references RetirementTypeTable(RetirementTypeID),
	 RetirementTypeDescription nvarchar(max) not null, 
	 LanguageID int not null foreign key references LanguageTable(LanguageID),
	 CreatedBy int not null foreign key references User_LoginUserTable(UserID),
	 CreatedDateTime SmallDatetime not null, 
	 LastModifiedBy int foreign key references User_LoginUserTable(UserID),
	 LastModifiedDateTime SmallDateTime NULL
  )
End 
Go 

If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='RetirementType')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('RetirementType','RetirementType',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Master'),1,0);
End
Go

If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='RetirementType')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'RetirementType',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='RetirementType'),'/MasterPage/Index?pageName=RetirementType',
(Select MenuID from USER_MENUTABLE where MenuName='Master' ),10);
End
Go

IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'RetirementTypeID' AND Object_ID = Object_ID(N'AssetTable'))
Begin 
alter table AssetTable add  RetirementTypeID  int NULL Foreign key references RetirementTypeTable(RetirementTypeID)
END
GO

select * from assettable where statusid=50

select * from mastergridtable

select * from mastergridlineitemtable where mastergridid=4018

Update mastergridlineitemtable set FieldName='FromLocationType.LocationTypeName' where FieldName='FromLocationType.LocationType'
go 
Update mastergridlineitemtable set FieldName='ToLocationType.LocationTypeName' where FieldName='ToLocationType.LocationType'
go 

 If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='AssetTransferFinalApproval')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('AssetTransferFinalApproval','AssetTransferFinalApproval',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Approval'),1,0);
End
Go

If not exists(SELECT MenuName FROM USER_MENUTABLE where MenuName='AssetTransferFinalApproval')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(

'AssetTransferFinalApprovall',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='AssetTransferFinalApproval'),'/AssetTransferFinalApproval/Index',
(Select MenuID from USER_MENUTABLE where MenuName='Approval' ),8);
end
go
IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'StampPath' AND Object_ID = Object_ID(N'LocationTable'))
Begin 
Alter table LocationTable add StampPath nvarchar(max) NULL END
GO

IF  EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'StampPath' AND Object_ID = Object_ID(N'LocationTable'))
Begin 
Alter table PersonTable drop column  StampPath 
End 
GO

ALTER View [dbo].[SecondLevelLocationTable] 
as  
Select a.LocationCode,a.LocationName,b.LocationName AS ParentLocation,a.LocationID,b.LocationName+'/'+a.LocationName as SecondLevelLocationName,l.LocationTypeName as LocationType
   from LocationTable a 
   join LocationTable b on a.ParentLocationID=b.LocationID
   left join LocationTypeTable l ON A.LocationTypeID=L.LocationTypeID
   where b.ParentLocationID is null and a.StatusID!=500 and b.StatusID!=500
GO


ALTER Procedure [dbo].[rprc_AssetRetirementReceiptFooter]
(
  @TransactionID int 
)
as 
Begin 
select  Ap.AreaApproveDate,ap.AreaApproverName,ap.AreaDepartment,ap.AreaPostion,ap.Comments,ap.areaFromStamp as AreaFromStamp
,ap.areaToStamp as AreaToStamp ,ap.SignaturePath as AreaSignature,
 Ap1.AreaApproveDate as AmsApproveDate,ap1.AreaApproverName as AmsApproerName,ap1.AreaDepartment as AmsDepartment,ap1.AreaPostion as AmsPostion,
 ap1.Comments as AmsComments,ap1.AMSFromStamp as AMSFromStamp,ap1.AMSToStamp as AMSToStamp,ap1.SignaturePath as AMSSignature,
  Ap2.AreaApproveDate as ReceiverApproveDate,Ap2.AreaApproverName as ReceiverApproerName,Ap2.AreaDepartment as ReceiverDepartment,Ap2.AreaPostion as ReceiverPostion,
 Ap2.Comments as ReceiverComments,Ap2.SignaturePath as ReceiverSignature ,ap2.HAMSFromStamp as ReceiverFromStamp,ap2.HAMSToStamp as ReceiverToStamp
From TransactionTable a 
  Left join (select A2.PersonFirstName+'-'+A2.PersonLastName as AreaApproverName,A1.LastModifiedDateTime as AreaApproveDate,
   A3.DesignationName as AreaPostion,A4.DepartmentName as AreaDepartment,A1.Remarks as Comments,A1.TransactionID,A2.SignaturePath,
   FL.StampPath as areaFromStamp,TL.StampPath as areaToStamp
   from ApprovalHistoryTable A1
   join PersonTable A2 on A1.LastModifiedBy=A2.PersonID
   Left join DesignationTable A3 on A2.DesignationID=A3.DesignationID
   Left join DepartmentTable A4 on A2.DepartmentID=A4.DepartmentID
   Left join LocationTable FL on A1.FromLocationID=FL.LocationID
   Left join LocationTable TL on A1.ToLocationID=TL.LocationID
   where ApprovalRoleID in (select ApprovalRoleID from ApprovalRoleTable where ApprovalRoleName in ('Area authority in ADEK'))
   and a1.ApproveModuleID=10
   )AP on a.TransactionID=AP.TransactionID
   Left join (select A2.PersonFirstName+'-'+A2.PersonLastName as AreaApproverName,A1.LastModifiedDateTime as AreaApproveDate,
   A3.DesignationName as AreaPostion,A4.DepartmentName as AreaDepartment,A1.Remarks as Comments,A1.TransactionID,A2.SignaturePath,
   FL.StampPath as AMSFromStamp,TL.StampPath as AMSToStamp
   from ApprovalHistoryTable A1
   join PersonTable A2 on A1.LastModifiedBy=A2.PersonID
   Left join DesignationTable A3 on A2.DesignationID=A3.DesignationID
   Left join DepartmentTable A4 on A2.DepartmentID=A4.DepartmentID
   Left join LocationTable FL on A1.FromLocationID=FL.LocationID
   Left join LocationTable TL on A1.ToLocationID=TL.LocationID
   where ApprovalRoleID in (select ApprovalRoleID from ApprovalRoleTable where ApprovalRoleName in ('AMS – Team Leader'))
   and a1.ApproveModuleID=10
   )AP1 on a.TransactionID=AP1.TransactionID
    Left join (select A2.PersonFirstName+'-'+A2.PersonLastName as AreaApproverName,A1.LastModifiedDateTime as AreaApproveDate,
   A3.DesignationName as AreaPostion,A4.DepartmentName as AreaDepartment,A1.Remarks as Comments,A1.TransactionID,A2.SignaturePath,
  FL.StampPath as HAMSFromStamp,TL.StampPath as HAMSToStamp
   from ApprovalHistoryTable A1
   join PersonTable A2 on A1.LastModifiedBy=A2.PersonID
   Left join DesignationTable A3 on A2.DesignationID=A3.DesignationID
   Left join DepartmentTable A4 on A2.DepartmentID=A4.DepartmentID
    Left join LocationTable FL on A1.FromLocationID=FL.LocationID
   Left join LocationTable TL on A1.ToLocationID=TL.LocationID
   where ApprovalRoleID in (select ApprovalRoleID from ApprovalRoleTable where ApprovalRoleName in ('AMS – HAMS'))
   and a1.ApproveModuleID=10
   )AP2 on a.TransactionID=AP2.TransactionID
   where a.TransactionID=@TransactionID
End 


go 


ALTER Procedure [dbo].[rprc_AssetRetirementReceiptHeader]
(
  @TransactionID int 
)
as 
Begin 
  select  P1.PersonFirstName+'-'+p1.PersonLastName as SenderName,L1.LocationName as FromLocationName,
      p1.EMailID as SenderEmailID,P1.MobileNo as SenderPhoneno,a.Remarks as Comments,D.DesignationName as SenderJobTitle,AP.ApproveDate,Ap.SignaturePath
	  ,Ap.FromStamp,ap.ToStamp
  from TransactionTable a 
	join (select * from (select  ROW_NUMBER() over( 
         PARTITION BY transactionid  Order by AssetID asc) as serialNo,* from TransactionLineItemTable)y where y.SerialNo=1 ) TL on a.TransactionID=TL.TransactionID
	  join PersonTable P1 on a.CreatedBy=p1.PersonID
	    Left join DesignationTable D on P1.DesignationID=d.DesignationID
		  Left join LocationTable L1 on TL.FromLocationID=L1.LocationID
		   Left join (select A2.PersonFirstName+'-'+A2.PersonLastName as AreaApproverName,A1.LastModifiedDateTime as ApproveDate,
		   A1.Remarks as Comments,A1.TransactionID,A2.SignaturePath,
		    FL.StampPath as FromStamp,TL.StampPath as ToStamp
		   from ApprovalHistoryTable A1
		   join PersonTable A2 on A1.LastModifiedBy=A2.PersonID
		    Left join LocationTable FL on A1.FromLocationID=FL.LocationID
   Left join LocationTable TL on A1.ToLocationID=TL.LocationID
		 where A1.OrderNo=1 
		   and a1.ApproveModuleID=10
		   )AP on a.TransactionID=AP.TransactionID
		  WHERE A.TRANSACTIONID=@TransactionID
End 
GO 

ALTER Procedure [dbo].[rprc_AssetTransferReceiptHeader]
(
  @TransactionID int 
)
as 
Begin 
   Select P1.PersonFirstName+'-'+p1.PersonLastName as SenderName,L1.LocationName as FromLocationName,L2.LocationName as ToLocationName,P2.PersonFirstName+'-'+p2.PersonLastName as ReceiverName,
    p1.EMailID as SenderEmailID,P1.MobileNo as SenderPhoneno,a.Remarks as Comments,D.DesignationName as SenderJobTitle,D1.DesignationName as ReceiverJobTitle,
	P2.EmailID as ReceiverEmailID,P2.MobileNo as ReceiverPhoneNo,T.TransferTypeName,ap.ApproveDate,ap.SignaturePath,
	ap.FromStamp,aP.ToStamp

	From TransactionTable a 
	join (select * from (select  ROW_NUMBER() over( 
         PARTITION BY transactionid  Order by AssetID asc) as serialNo,* from TransactionLineItemTable)y where y.SerialNo=1 ) TL on a.TransactionID=TL.TransactionID
	  join PersonTable P1 on a.CreatedBy=p1.PersonID
	  Left join DesignationTable D on P1.DesignationID=d.DesignationID
	 Left join TransferTypeTable T on TL.TransferTypeID=T.TransferTypeID
	left join (SELECT * FROM (select  ROW_NUMBER() over( 
         PARTITION BY a.transactionid , a.approvemoduleID
         ORDER BY Orderno DESC 
     ) AS SerialNo,* from approvalHistorytable a ) X where X.SerialNo=1 and statusid=50 ) b on a.transactionid=b.transactionid 	
   Left join personTable p2 on  b.LastModifiedBy=p2.personID
   Left join DesignationTable D1 on p2.DesignationID=D1.DesignationID
   Left join LocationTable L1 on TL.FromLocationID=L1.LocationID
   Left join LocationTable L2 on TL.ToLocationID=L2.LocationID
    Left join (select A2.PersonFirstName+'-'+A2.PersonLastName as AreaApproverName,A1.LastModifiedDateTime as ApproveDate,
		   A1.Remarks as Comments,A1.TransactionID,A2.SignaturePath,
		 FL.StampPath as FromStamp,TL.StampPath as ToStamp
		   from ApprovalHistoryTable A1
		   join PersonTable A2 on A1.LastModifiedBy=A2.PersonID
		   Left join LocationTable FL on A1.FromLocationID=FL.LocationID
          Left join LocationTable TL on A1.ToLocationID=TL.LocationID
		 where A1.OrderNo=1 
		   and a1.ApproveModuleID=10
		   )AP on a.TransactionID=AP.TransactionID
	  where a.TransactionID=@TransactionID
End 
go 
create View [dbo].[FinalLevelTransferView]
	 as 
	 Select a.*,b.TransactionNo 
	 From ApprovalhistoryTable a 
	 join TransactionTable b on a.transactionid=b.transactionid
	 where a.approvemoduleID=10  and  b.transactiontypeID=10 and b.postingStatusID=11 and a.statusID=150
GO

ALTER view [dbo].[AssetTransferApprovalView] 
  as 
   
   Select ApprovalHistoryID,ApproveWorkFlowID,ApproveWorkFlowLineItemID,
		a.ApproveModuleID,ApprovalRoleID,a.TransactionID as ApprovalTransactionID,
		OrderNo,a.Remarks as ApprovalRemarks,FromLocationID,ToLocationID,
		FromLocationTypeID,ToLocationTypeID,a.StatusID as ApprovalStatusID,
        a.CreatedBy as ApprovalCreatedBy,a.CreatedDateTime as ApprovalCreatedDateTime,
		LastModifiedBy,LastModifiedDateTime,ObjectKeyID1,EmailsecrectKey ,b.*, ROW_NUMBER() over( 
         PARTITION BY a.transactionid , a.approvemoduleID
         ORDER BY Orderno asc 
     ) AS SerialNo ,p.PersonFirstName+'-'+p.PersonLastName as CreatedUSer,s.Status as ApprovalStatus
	From ApprovalHistoryTable a join TransactionTable b on a.TransactionID=b.TransactionID 
	join (select TransactionID,ApproveModuleID,max(Orderno) as MaxOrderNo from ApprovalHistoryTable  where ApproveModuleID=10 and StatusID=150 
	group by TransactionID,ApproveModuleID )c on a.TransactionID=c.TransactionID
	join PersonTable p on b.CreatedBy=p.PersonID
	join StatusTable s on a.StatusID=s.StatusID
	where a.ApproveModuleID=5 and a.StatusID=150
GO



IF  EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'RetirementTypeCode' AND Object_ID = Object_ID(N'TransactionLineItemTable'))
Begin 
Alter table TransactionLineItemTable drop column RetirementTypeCode 
END
GO
IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'RetirementTypeID' AND Object_ID = Object_ID(N'TransactionLineItemTable'))
Begin 
Alter table TransactionLineItemTable add RetirementTypeID int NULL foreign key references retirementTypetable(RetirementTypeID)
END
GO
IF  EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'RoomCode' AND Object_ID = Object_ID(N'TransactionLineItemTable'))
Begin 
Alter table TransactionLineItemTable drop column RoomCode 
END
GO
IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'RoomID' AND Object_ID = Object_ID(N'TransactionLineItemTable'))
Begin 
Alter table TransactionLineItemTable add RoomID int NULL foreign key references LocationTable(LocationID)
END
GO

IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'DisposalValue' AND Object_ID = Object_ID(N'TransactionLineItemTable'))
Begin 
Alter table TransactionLineItemTable add DisposalValue decimal(18,2) null
END
GO
IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'DisposalReferencesNo' AND Object_ID = Object_ID(N'TransactionLineItemTable'))
Begin 
Alter table TransactionLineItemTable add DisposalReferencesNo nvarchar(100) null
END
GO
IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'DisposalRemarks' AND Object_ID = Object_ID(N'TransactionLineItemTable'))
Begin 
Alter table TransactionLineItemTable add DisposalRemarks nvarchar(100) null
END
GO
IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'ProceedOfSales' AND Object_ID = Object_ID(N'TransactionLineItemTable'))
Begin 
Alter table TransactionLineItemTable add ProceedOfSales  decimal(18,2) null
END
GO
IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'CostOfRemoval' AND Object_ID = Object_ID(N'TransactionLineItemTable'))
Begin 
Alter table TransactionLineItemTable add CostOfRemoval  decimal(18,2) null
END
GO
IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'DisposalDate' AND Object_ID = Object_ID(N'TransactionLineItemTable'))
Begin 
Alter table TransactionLineItemTable add DisposalDate SmallDatetime
END
GO


ALTER view [dbo].[AssetTransferApprovalView]
  as 
   
  Select ApprovalHistoryID,ApproveWorkFlowID,ApproveWorkFlowLineItemID,
		a.ApproveModuleID,a.ApprovalRoleID,a.TransactionID as ApprovalTransactionID,
		OrderNo,a.Remarks as ApprovalRemarks,FromLocationID,ToLocationID,
		FromLocationTypeID,ToLocationTypeID,a.StatusID as ApprovalStatusID,
        a.CreatedBy as ApprovalCreatedBy,a.CreatedDateTime as ApprovalCreatedDateTime,
		a.LastModifiedBy,a.LastModifiedDateTime,ObjectKeyID1,EmailsecrectKey ,b.*, ROW_NUMBER() over( 
         PARTITION BY a.transactionid , a.approvemoduleID
         ORDER BY Orderno asc 
     ) AS SerialNo ,p.PersonFirstName+'-'+p.PersonLastName as CreatedUSer,s.Status as ApprovalStatus,isnull(c.UpdateDestinationlocationsForTransfer,0) as enableUpdate
	From ApprovalHistoryTable a join TransactionTable b on a.TransactionID=b.TransactionID 
	join ApprovalRoleTable c on a.ApprovalRoleID=c.ApprovalRoleID
	--join (select TransactionID,ApproveModuleID,max(Orderno) as MaxOrderNo from ApprovalHistoryTable  where ApproveModuleID=5 and StatusID=150 
	--group by TransactionID,ApproveModuleID )c on a.TransactionID=c.TransactionID
	join PersonTable p on b.CreatedBy=p.PersonID
	join StatusTable s on a.StatusID=s.StatusID
	where a.ApproveModuleID=5 and a.StatusID=150 --and a.OrderNo<c.MaxOrderNo
GO
ALTER view [dbo].[AssetRetirementApprovalView] 
  as 

   Select a.ApprovalHistoryID,ApproveWorkFlowID,ApproveWorkFlowLineItemID,
		a.ApproveModuleID,a.ApprovalRoleID,a.TransactionID as ApprovalTransactionID,
		a.OrderNo,a.Remarks as ApprovalRemarks,FromLocationID,ToLocationID,
		FromLocationTypeID,ToLocationTypeID,a.StatusID as ApprovalStatusID,
        a.CreatedBy as ApprovalCreatedBy,a.CreatedDateTime as ApprovalCreatedDateTime,
		a.LastModifiedBy,a.LastModifiedDateTime,ObjectKeyID1,EmailsecrectKey ,b.*, ROW_NUMBER() over( 
         PARTITION BY a.transactionid , a.approvemoduleID
         ORDER BY a.Orderno asc 
     ) AS SerialNo ,p.PersonFirstName+'-'+p.PersonLastName as CreatedUSer,s.Status as ApprovalStatus
	 ,isnull(c.UpdateRetirementDetailsForEachAssets,0) as enableUpdate
	From ApprovalHistoryTable a join TransactionTable b on a.TransactionID=b.TransactionID 
		join ApprovalRoleTable c on a.ApprovalRoleID=c.ApprovalRoleID
	--join (select TransactionID,ApproveModuleID,max(Orderno) as MaxOrderNo from ApprovalHistoryTable  where ApproveModuleID=10 and StatusID=150 
	--group by TransactionID,ApproveModuleID )c on a.TransactionID=c.TransactionID
	join PersonTable p on b.CreatedBy=p.PersonID
	join StatusTable s on a.StatusID=s.StatusID
	where a.ApproveModuleID=10 and a.StatusID=150 --and a.OrderNo<c.MaxOrderNo
	
GO



update EntityCodeTable set CodePrefix='GS/AM/#DateTime.Now.Year#/AT/' where EntityCodeName='AssetTransfer'
go 
update EntityCodeTable set CodePrefix='GS/AM/#DateTime.Now.Year#/AR/' where EntityCodeName='AssetRetirement'
go 
ALTER Procedure [dbo].[rprc_AssetRetirementReceipt]
(
  @TransactionID int 
)
as 
Begin 
  Select  ROW_NUMBER() over( 
           Order by b.AssetID asc) as slNo,c.Barcode,p.ProductName as AssetDescription,c.SerialNo,c.AssetRemarks
		   ,TransactionNo as Receiptyear --'GS/AM/'+convert(nvarchar(100),YEAR(a.CreatedDateTime))+'/AR/' + TransactionNo  as Receiptyear
  From TransactionTable a 
   join TransactionLineItemTable b on a.TransactionID=b.TransactionID
   join AssetTable c on b.AssetID=c.AssetID
   join ProductTable p on c.ProductID=p.ProductID
   where a.TransactionID=@TransactionID
End 
go 
ALTER Procedure [dbo].[rprc_AssetRetirementReceiptHeader]
(
  @TransactionID int 
)
as 
Begin 
  select  P1.PersonFirstName+'-'+p1.PersonLastName as SenderName,L1.LocationName as FromLocationName,
      p1.EMailID as SenderEmailID,P1.MobileNo as SenderPhoneno,a.Remarks as Comments,D.DesignationName as SenderJobTitle,AP.ApproveDate,Ap.SignaturePath
	  ,Ap.FromStamp,ap.ToStamp,a.TransactionNo,a.TransactionNo as Receiptyear--'GS/AM/'+convert(nvarchar(100),YEAR(a.CreatedDateTime))+'/AR/' + TransactionNo  as Receiptyear
  from TransactionTable a 
	join (select * from (select  ROW_NUMBER() over( 
         PARTITION BY transactionid  Order by AssetID asc) as serialNo,* from TransactionLineItemTable)y where y.SerialNo=1 ) TL on a.TransactionID=TL.TransactionID
	  join PersonTable P1 on a.CreatedBy=p1.PersonID
	    Left join DesignationTable D on P1.DesignationID=d.DesignationID
		  Left join LocationTable L1 on TL.FromLocationID=L1.LocationID
		   Left join (select A2.PersonFirstName+'-'+A2.PersonLastName as AreaApproverName,A1.LastModifiedDateTime as ApproveDate,
		   A1.Remarks as Comments,A1.TransactionID,A2.SignaturePath,
		    FL.StampPath as FromStamp,TL.StampPath as ToStamp
		   from ApprovalHistoryTable A1
		   join PersonTable A2 on A1.LastModifiedBy=A2.PersonID
		    Left join LocationTable FL on A1.FromLocationID=FL.LocationID
   Left join LocationTable TL on A1.ToLocationID=TL.LocationID
		 where A1.OrderNo=1 
		   and a1.ApproveModuleID=10
		   )AP on a.TransactionID=AP.TransactionID
		  WHERE A.TRANSACTIONID=@TransactionID
End 
go 


ALTER Procedure [dbo].[rprc_AssetTransferReceipt]
(
  @TransactionID int 
)
as 
Begin 
   Select c.Barcode,p.ProductName as AssetDescription,c.SerialNo,c.AssetRemarks,
   L1.LocationName as FromLocationName,L2.LocationName as ToLocationName,P1.PersonFirstName+'-'+p1.PersonLastName as SenderName, 
   p1.EMailID as SenderEmailID,P1.MobileNo as SenderPhoneno,a.Remarks as Comments,	T.TransferTypeName,
   d.DesignationName as SenderJobTitle,Ap.AreaApproveDate,ap.AreaApproverName,ap.AreaDepartment,ap.AreaPostion,ap.Comments, ROW_NUMBER() over( 
           Order by b.AssetID asc) as slNo,a.TransactionNo,a.TransactionNo as Receiptyear--'GS/AM/'+convert(nvarchar(100),YEAR(a.CreatedDateTime))+'/AT/'+TransactionNo as Receiptyear
   From TransactionTable a 
   join TransactionLineItemTable b on a.TransactionID=b.TransactionID
   join AssetTable c on b.AssetID=c.AssetID
   join ProductTable p on c.ProductID=p.ProductID
   join LocationTable L1 on b.FromLocationID=L1.LocationID
   join LocationTable L2 on B.ToLocationID=L2.LocationID
   join PersonTable P1 on a.CreatedBy=p1.PersonID
   Left join DesignationTable D on P1.DesignationID=d.DesignationID
   Left join TransferTypeTable T on b.TransferTypeID=T.TransferTypeID
   Left join (select A2.PersonFirstName+'-'+A2.PersonLastName as AreaApproverName,A1.LastModifiedDateTime as AreaApproveDate,
   A3.DesignationName as AreaPostion,A4.DepartmentName as AreaDepartment,A1.Remarks as Comments,A1.TransactionID
   from ApprovalHistoryTable A1
   join PersonTable A2 on A1.LastModifiedBy=A2.PersonID
   Left join DesignationTable A3 on A2.DesignationID=A3.DesignationID
   Left join DepartmentTable A4 on A2.DepartmentID=A4.DepartmentID
   where ApprovalRoleID in (select ApprovalRoleID from ApprovalRoleTable where ApprovalRoleName in ('Area authority in ADEK'))
   and a1.ApproveModuleID=5
   )AP on a.TransactionID=AP.TransactionID

   where a.TransactionID=@TransactionID

End 
go 

ALTER Procedure [dbo].[rprc_AssetTransferReceiptHeader]
(
  @TransactionID int 
)
as 
Begin 
   Select P1.PersonFirstName+'-'+p1.PersonLastName as SenderName,L1.LocationName as FromLocationName,L2.LocationName as ToLocationName,P2.PersonFirstName+'-'+p2.PersonLastName as ReceiverName,
    p1.EMailID as SenderEmailID,P1.MobileNo as SenderPhoneno,a.Remarks as Comments,D.DesignationName as SenderJobTitle,D1.DesignationName as ReceiverJobTitle,
	P2.EmailID as ReceiverEmailID,P2.MobileNo as ReceiverPhoneNo,T.TransferTypeName,ap.ApproveDate,ap.SignaturePath,
	ap.FromStamp,aP.ToStamp, TransactionNo as Receiptyear--'GS/AM/'+convert(nvarchar(100),YEAR(a.CreatedDateTime))+'/AT/'+TransactionNo as Receiptyear

	From TransactionTable a 
	join (select * from (select  ROW_NUMBER() over( 
         PARTITION BY transactionid  Order by AssetID asc) as serialNo,* from TransactionLineItemTable)y where y.SerialNo=1 ) TL on a.TransactionID=TL.TransactionID
	  join PersonTable P1 on a.CreatedBy=p1.PersonID
	  Left join DesignationTable D on P1.DesignationID=d.DesignationID
	 Left join TransferTypeTable T on TL.TransferTypeID=T.TransferTypeID
	left join (SELECT * FROM (select  ROW_NUMBER() over( 
         PARTITION BY a.transactionid , a.approvemoduleID
         ORDER BY Orderno DESC 
     ) AS SerialNo,* from approvalHistorytable a ) X where X.SerialNo=1 and statusid=50 ) b on a.transactionid=b.transactionid 	
   Left join personTable p2 on  b.LastModifiedBy=p2.personID
   Left join DesignationTable D1 on p2.DesignationID=D1.DesignationID
   Left join LocationTable L1 on TL.FromLocationID=L1.LocationID
   Left join LocationTable L2 on TL.ToLocationID=L2.LocationID
    Left join (select A2.PersonFirstName+'-'+A2.PersonLastName as AreaApproverName,A1.LastModifiedDateTime as ApproveDate,
		   A1.Remarks as Comments,A1.TransactionID,A2.SignaturePath,
		 FL.StampPath as FromStamp,TL.StampPath as ToStamp
		   from ApprovalHistoryTable A1
		   join PersonTable A2 on A1.LastModifiedBy=A2.PersonID
		   Left join LocationTable FL on A1.FromLocationID=FL.LocationID
          Left join LocationTable TL on A1.ToLocationID=TL.LocationID
		 where A1.OrderNo=1 
		   and a1.ApproveModuleID=10
		   )AP on a.TransactionID=AP.TransactionID
	  where a.TransactionID=@TransactionID
End 
go 
ALTER View [dbo].[ApprovalHistoryView] 
  as 
  Select b.ApprovalRoleName,case when c.StatusID=50 then 'Approved' else c.Status end as ApprovalStatus,p.PersonFirstName+'-'+p.PersonLastName as ApprovedBy ,a.LastModifiedDateTime as ApprovedDatetime ,a.*
  ,[FileName] as DocumentName,t3.personName as UserName From ApprovalHistoryTable a 

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


ALTER View [dbo].[TransactionLineItemViewForTransfer]
  as 
  Select a1.*,a1.LocationHierarchy as OldLocationName,New.LocationName as NewLocationName,
  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubType,
ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,Remarks,
TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
OldDept.DepartmentName as OldDepartmentName,NewDept.DepartmentName as NewDepartmentName,OldCate.CategoryName as OldCategoryName,
Oldprod.ProductName as OldProductName,newprod.ProductName as NewProductName,OldSec.SectionName as oldSectionName,newsec.SectionName as NewSectionName,

newCate.CategoryName as NewCategoryName
  From TransactionTable a 
  join TransactionLineItemTable b on a.TransactionID=b.TransactionID 
  join AssetView a1 on b.AssetID=a1.AssetID 
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

GO
ALTER View [dbo].[ApprovalHistoryView] 
  as 
  Select b.ApprovalRoleName,case when c.StatusID=50 then 'Approved' else c.Status end as ApprovalStatus,p.PersonFirstName+'-'+p.PersonLastName as ApprovedBy ,a.LastModifiedDateTime as ApprovedDatetime ,a.*
  ,[FileName] as DocumentName,t3.personName as UserName From ApprovalHistoryTable a 

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
 ALTER View [dbo].[TransactionLineItemViewForTransfer]
  as 
  Select a1.*,a1.LocationHierarchy as OldLocationName,New.LocationName as NewLocationName,
  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubType,
ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,Remarks,
TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
OldDept.DepartmentName as OldDepartmentName,NewDept.DepartmentName as NewDepartmentName,OldCate.CategoryName as OldCategoryName,
Oldprod.ProductName as OldProductName,newprod.ProductName as NewProductName,OldSec.SectionName as oldSectionName,newsec.SectionName as NewSectionName,

newCate.CategoryName as NewCategoryName
  From TransactionTable a 
  join TransactionLineItemTable b on a.TransactionID=b.TransactionID 
  join AssetView a1 on b.AssetID=a1.AssetID 
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

GO
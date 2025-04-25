Create Procedure prc_AssetTransferForApproval
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
FROM DocumentTable a join TransactionTable b on a.ObjectKeyID=b.TransactionID and a.TransactionType='AssetTransfer'
where a.statusID=5
GROUP BY a.ObjectKeyID)inidoc on inidoc.ObjectKeyID=a.TransactionID

left join (SELECT a.ObjectKeyID,
   STUFF((SELECT '; ' + US.Filename 
          FROM DocumentTable US
          WHERE US.ObjectKeyID = a.ObjectKeyID 
          ORDER BY Filename
          FOR XML PATH('')), 1, 1, '') [FileName]
FROM DocumentTable a join ApprovalHistoryTable b on a.ObjectKeyID=b.ApprovalHistoryID and a.TransactionType='AssetTransferApproval' and b.ApproveModuleID=5
where a.statusID=5
GROUP BY a.ObjectKeyID)approvaldoc on approvaldoc.ObjectKeyID=b.ApprovalHistoryID

Where b.ApproveModuleID=5  and a.transactionTypeID=5 and b.statusID=101 and b.ApprovalHistoryID=@ApprovalID
End 


go 

select * from ApprovalHistoryTable 



ALTER Procedure [dbo].[prc_AssetTransferIndexList]
  (
    @UserID int =NULL,
	@CompanyID int = NULL,
	@LanguageID int = NULL
  )
  as 
  BEgin
    Select * , ROW_NUMBER() over( 
         PARTITION BY a.transactionid , a.approvemoduleID
         ORDER BY Orderno DESC 
     ) AS SerialNo ,p.PersonFirstName+'-'+p.PersonLastName as CreatedBy
	From ApprovalHistoryTable a join TransactionTable b on a.TransactionID=b.TransactionID 
	join PersonTable p on b.CreatedBy=p.PersonID
	where a.ApproveModuleID=5 and a.StatusID=150 and b.CreatedBy!=@UserID
  End 
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
FROM DocumentTable a join TransactionTable b on a.ObjectKeyID=b.TransactionID and a.TransactionType='AssetTransfer'
where a.statusID=50
GROUP BY a.ObjectKeyID)inidoc on inidoc.ObjectKeyID=a.TransactionID

left join (SELECT a.ObjectKeyID,
   STUFF((SELECT '; ' + US.Filename 
          FROM DocumentTable US
          WHERE US.ObjectKeyID = a.ObjectKeyID 
          ORDER BY Filename
          FOR XML PATH('')), 1, 1, '') [FileName]
FROM DocumentTable a join ApprovalHistoryTable b on a.ObjectKeyID=b.ApprovalHistoryID and a.TransactionType='AssetTransferApproval' and b.ApproveModuleID=5
where a.statusID=50
GROUP BY a.ObjectKeyID)approvaldoc on approvaldoc.ObjectKeyID=b.ApprovalHistoryID

Where b.ApproveModuleID=5  and a.transactionTypeID=5 and b.statusID=150 and b.ApprovalHistoryID=@ApprovalID
End 


go 

-- ==============================================================================================================================================
-- Author:		Balakrishnan.P
-- Create date: 03-OCT-2019 10:00:00 AM
-- Description:	WMS System Procedure for creating menus and rights
-- **********************************************************************************************************************************************
-- Date Time			Author			Ver		Description
-- **********************************************************************************************************************************************
-- 11-Dec-2019 13:10	Balakrishnan	0.55	Duplicate menu name gives confussion over location, fixed this issue using parent menu name
-- ==============================================================================================================================================
Create PROC [dbo].[wsprc_CreateMenu]
	@RightGroupName varchar(50),
	@RightName varchar(50),	
	@ParentMenuNameL1 varchar(50),
	@ParentMenuNameL2 varchar(50),
	@MenuName varchar(50),
	@MenuURL varchar(50),
	@ValueType int,
	@MenuPosition	int = NULL,
	@ParentTransactionID	INT = NULL,
	@ParentTransactionType	VARCHAR(500) = NULL
AS
BEGIN
	IF (@RightName IS NOT NULL)
	BEGIN
		EXEC wsprc_CreateRight @RightGroupName, @RightName, @ValueType
	END

	/* Trim the unwanted space in the menu string */
	SET @ParentMenuNameL1 = REPLACE(@ParentMenuNameL1, ' ', '')
	SET @ParentMenuNameL2 = REPLACE(@ParentMenuNameL2, ' ', '')
	SET @MenuName = REPLACE(@MenuName, ' ', '')

	IF(@ParentMenuNameL1 = '') SET @ParentMenuNameL1 = NULL
	IF(@ParentMenuNameL2 = '') SET @ParentMenuNameL2 = NULL

	IF((@ParentMenuNameL2 IS NOT NULL) AND (@ParentMenuNameL1 IS NULL))
	BEGIN
		SET @ParentMenuNameL1 = @ParentMenuNameL2 
		SET @ParentMenuNameL2 = NULL
	END

	--Make sure L1 menu is there
	IF(@ParentMenuNameL1 IS NOT NULL)
	BEGIN
		IF NOT EXISTS(SELECT A.* FROM [User_MenuTable] A WHERE A.[MenuName] = @ParentMenuNameL1)
		BEGIN
			INSERT INTO [dbo].[User_MenuTable]
					   ([MenuName]
					   ,[RightID]
					   ,[TargetObject]
					   ,[ParentMenuID]
					   ,[OrderNo])
				VALUES( @ParentMenuNameL1, NULL, NULL, (SELECT MenuID FROM [User_MenuTable] WHERE MenuName = @ParentMenuNameL1), 9)
		END
	END

	IF(@ParentMenuNameL2 IS NOT NULL)
	BEGIN
		--Make sure L2 menu is there
		IF NOT EXISTS(SELECT CHILD.* FROM [User_MenuTable] CHILD 
				LEFT JOIN [User_MenuTable] PARENT ON PARENT.MenuID = CHILD.ParentMenuID 
				WHERE CHILD.[MenuName] = @ParentMenuNameL2 AND (@ParentMenuNameL1 IS NULL OR PARENT.MenuName = @ParentMenuNameL1))
		BEGIN
			INSERT INTO [dbo].[User_MenuTable]
					   ([MenuName]
					   ,[RightID]
					   ,[TargetObject]
					   ,[ParentMenuID]
					   ,[OrderNo])
				VALUES( @ParentMenuNameL2, NULL, NULL, (SELECT MenuID FROM [User_MenuTable] WHERE MenuName = @ParentMenuNameL1), 9)
		END
	END
		
	--Get the exact parent menu
	DECLARE @ParentMenuID	INT
	SELECT @ParentMenuID = MenuID FROM [User_MenuTable]  WHERE MenuName = @ParentMenuNameL1

	IF(@ParentMenuNameL2 IS NOT NULL)
	BEGIN
		SELECT @ParentMenuID = MenuID FROM [User_MenuTable]  WHERE MenuName = @ParentMenuNameL2 AND ParentMenuID = @ParentMenuID
	END

	IF NOT EXISTS(SELECT * FROM [User_MenuTable] WHERE [MenuName] = @MenuName AND ParentMenuID = @ParentMenuID)
	BEGIN
		--Get the menu position
		IF(@MenuPosition IS NULL)
			SELECT @MenuPosition = ISNULL(MAX([OrderNo]), 1) + 1 FROM [User_MenuTable] 
				WHERE [ParentMenuID] = @ParentMenuID

		DECLARE @RightID INT
		SELECT @RightID = RightID FROM [User_RightTable] WHERE [RightName] = @RightName

		INSERT INTO [dbo].[User_MenuTable]
				   ([MenuName]
				   ,[RightID]
				   ,[TargetObject]
				   ,[ParentMenuID]
				   ,[OrderNo]
				   ,ParentTransactionID
				   ,ParentTransactionType)
		VALUES(@MenuName, @RightID, @MenuURL, @ParentMenuID, @MenuPosition, @ParentTransactionID, @ParentTransactionType)
	END
END
go 

-- ==============================================================================================================================================
-- Author:		Balakrishnan.P
-- Create date: 03-OCT-2019 10:00:00 AM
-- Description:	WMS System Procedure for creating rights
-- **********************************************************************************************************************************************
-- Date Time			Author			Ver		Description
-- **********************************************************************************************************************************************
-- 09-May-2020 13:10	Balakrishnan	Right Name null value condition added
-- ==============================================================================================================================================
create PROC [dbo].[wsprc_CreateRight]
	@RightGroupName varchar(50),
	@RightName varchar(50),
	@ValueType int
AS
BEGIN
	IF(@RightName IS NULL) RETURN;

	SET @RightName = REPLACE(@RightName, ' ', '')

	IF (@RightGroupName IS NOT NULL)
	BEGIN
		EXEC wsprc_CreateRightGroup @RightGroupName, @RightGroupName
	END

	IF NOT EXISTS(SELECT * FROM [User_RightTable] WHERE [RightName] = @RightName)
	BEGIN
		INSERT INTO [dbo].[User_RightTable]([RightName],[RightDescription],[ValueType]
				   ,[DisplayRight]
				   ,[RightGroupID]
				   ,[IsActive]
				   ,[IsDeleted])
			 VALUES
				   (@RightName, @RightName, @ValueType, 1, (SELECT [RightGroupID] FROM [User_RightGroupTable] WHERE [RightGroupName] = @RightGroupName), 1, 0)
	END
END
go 


	  -- ==============================================================================================================================================
-- Author:		Balakrishnan.P
-- Create date: 03-OCT-2019 10:00:00 AM
-- Description:	WMS System Procedure for creating rights
-- **********************************************************************************************************************************************
-- 20-May-2019 13:10	Balakrishnan	
-- ==============================================================================================================================================
create PROC [dbo].[wsprc_CreateRightGroup]
	@RightGroupName varchar(50),
	@RightGroupDesc varchar(50)
AS
BEGIN
	IF NOT EXISTS(SELECT * FROM [User_RightGroupTable] WHERE RightGroupName = @RightGroupName)
	BEGIN
		INSERT INTO [dbo].[User_RightGroupTable](RightGroupName, RightGroupDesc)
			 VALUES(@RightGroupName, @RightGroupDesc)
	END
END
go 


Create Procedure [dbo].[rprc_AssetRetirementReceipt]
(
  @TransactionID int 
)
as 
Begin 
  Select  ROW_NUMBER() over( 
           Order by b.AssetID asc) as slNo,c.Barcode,p.ProductName as AssetDescription,c.SerialNo,c.AssetRemarks
  From TransactionTable a 
   join TransactionLineItemTable b on a.TransactionID=b.TransactionID
   join AssetTable c on b.AssetID=c.AssetID
   join ProductTable p on c.ProductID=p.ProductID
   where a.TransactionID=@TransactionID
End 
go 

create Procedure [dbo].[rprc_AssetRetirementReceiptHeader]
(
  @TransactionID int 
)
as 
Begin 
  select  P1.PersonFirstName+'-'+p1.PersonLastName as SenderName,L1.LocationName as FromLocationName,
      p1.EMailID as SenderEmailID,P1.MobileNo as SenderPhoneno,a.Remarks as Comments,D.DesignationName as SenderJobTitle
  from TransactionTable a 
	join (select * from (select  ROW_NUMBER() over( 
         PARTITION BY transactionid  Order by AssetID asc) as serialNo,* from TransactionLineItemTable)y where y.SerialNo=1 ) TL on a.TransactionID=TL.TransactionID
	  join PersonTable P1 on a.CreatedBy=p1.PersonID
	    Left join DesignationTable D on P1.DesignationID=d.DesignationID
		  Left join LocationTable L1 on TL.FromLocationID=L1.LocationID
		  WHERE A.TRANSACTIONID=@TransactionID
End 
go 

create Procedure [dbo].[rprc_AssetRetirementReceiptFooter]
(
  @TransactionID int 
)
as 
Begin 
select  Ap.AreaApproveDate,ap.AreaApproverName,ap.AreaDepartment,ap.AreaPostion,ap.Comments,
 Ap1.AreaApproveDate as AmsApproveDate,ap1.AreaApproverName as AmsApproerName,ap1.AreaDepartment as AmsDepartment,ap1.AreaPostion as AmsPostion,
 ap1.Comments as AmsComments,
  Ap2.AreaApproveDate as ReceiverApproveDate,Ap2.AreaApproverName as ReceiverApproerName,Ap2.AreaDepartment as ReceiverDepartment,Ap2.AreaPostion as ReceiverPostion,
 Ap2.Comments as ReceiverComments
From TransactionTable a 
  Left join (select A2.PersonFirstName+'-'+A2.PersonLastName as AreaApproverName,A1.LastModifiedDateTime as AreaApproveDate,
   A3.DesignationName as AreaPostion,A4.DepartmentName as AreaDepartment,A1.Remarks as Comments,A1.TransactionID
   from ApprovalHistoryTable A1
   join PersonTable A2 on A1.LastModifiedBy=A2.PersonID
   Left join DesignationTable A3 on A2.DesignationID=A3.DesignationID
   Left join DepartmentTable A4 on A2.DepartmentID=A4.DepartmentID
   where ApprovalRoleID in (select ApprovalRoleID from ApprovalRoleTable where ApprovalRoleName in ('Area authority in ADEK'))
   and a1.ApproveModuleID=10
   )AP on a.TransactionID=AP.TransactionID
   Left join (select A2.PersonFirstName+'-'+A2.PersonLastName as AreaApproverName,A1.LastModifiedDateTime as AreaApproveDate,
   A3.DesignationName as AreaPostion,A4.DepartmentName as AreaDepartment,A1.Remarks as Comments,A1.TransactionID
   from ApprovalHistoryTable A1
   join PersonTable A2 on A1.LastModifiedBy=A2.PersonID
   Left join DesignationTable A3 on A2.DesignationID=A3.DesignationID
   Left join DepartmentTable A4 on A2.DepartmentID=A4.DepartmentID
   where ApprovalRoleID in (select ApprovalRoleID from ApprovalRoleTable where ApprovalRoleName in ('AMS – Team Leader'))
   and a1.ApproveModuleID=10
   )AP1 on a.TransactionID=AP1.TransactionID
    Left join (select A2.PersonFirstName+'-'+A2.PersonLastName as AreaApproverName,A1.LastModifiedDateTime as AreaApproveDate,
   A3.DesignationName as AreaPostion,A4.DepartmentName as AreaDepartment,A1.Remarks as Comments,A1.TransactionID
   from ApprovalHistoryTable A1
   join PersonTable A2 on A1.LastModifiedBy=A2.PersonID
   Left join DesignationTable A3 on A2.DesignationID=A3.DesignationID
   Left join DepartmentTable A4 on A2.DepartmentID=A4.DepartmentID
   where ApprovalRoleID in (select ApprovalRoleID from ApprovalRoleTable where ApprovalRoleName in ('AMS – HAMS'))
   and a1.ApproveModuleID=10
   )AP2 on a.TransactionID=AP2.TransactionID
   where a.TransactionID=@TransactionID
End 
Go 

create Procedure [dbo].[rprc_AssetTransferReceipt]
(
  @TransactionID int 
)
as 
Begin 
   Select c.Barcode,p.ProductName as AssetDescription,c.SerialNo,c.AssetRemarks,
   L1.LocationName as FromLocationName,L2.LocationName as ToLocationName,P1.PersonFirstName+'-'+p1.PersonLastName as SenderName, 
   p1.EMailID as SenderEmailID,P1.MobileNo as SenderPhoneno,a.Remarks as Comments,	T.TransferTypeName,
   d.DesignationName as SenderJobTitle,Ap.AreaApproveDate,ap.AreaApproverName,ap.AreaDepartment,ap.AreaPostion,ap.Comments, ROW_NUMBER() over( 
           Order by b.AssetID asc) as slNo
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

create Procedure [dbo].[rprc_AssetTransferReceiptFooter]
(
  @TransactionID int 
)
as 
Begin 
Select Ap.AreaApproveDate,ap.AreaApproverName,ap.AreaDepartment,ap.AreaPostion,ap.Comments,
 Ap1.AreaApproveDate as AmsApproveDate,ap1.AreaApproverName as AmsApproerName,ap1.AreaDepartment as AmsDepartment,ap1.AreaPostion as AmsPostion,
 ap1.Comments as AmsComments,
  Ap2.AreaApproveDate as ReceiverApproveDate,Ap2.AreaApproverName as ReceiverApproerName,Ap2.AreaDepartment as ReceiverDepartment,Ap2.AreaPostion as ReceiverPostion,
 Ap2.Comments as ReceiverComments
 From TransactionTable a 
  Left join (select A2.PersonFirstName+'-'+A2.PersonLastName as AreaApproverName,A1.LastModifiedDateTime as AreaApproveDate,
   A3.DesignationName as AreaPostion,A4.DepartmentName as AreaDepartment,A1.Remarks as Comments,A1.TransactionID
   from ApprovalHistoryTable A1
   join PersonTable A2 on A1.LastModifiedBy=A2.PersonID
   Left join DesignationTable A3 on A2.DesignationID=A3.DesignationID
   Left join DepartmentTable A4 on A2.DepartmentID=A4.DepartmentID
   where ApprovalRoleID in (select ApprovalRoleID from ApprovalRoleTable where ApprovalRoleName in ('Area authority in ADEK'))
   and a1.ApproveModuleID=5
   )AP on a.TransactionID=AP.TransactionID
   Left join (select A2.PersonFirstName+'-'+A2.PersonLastName as AreaApproverName,A1.LastModifiedDateTime as AreaApproveDate,
   A3.DesignationName as AreaPostion,A4.DepartmentName as AreaDepartment,A1.Remarks as Comments,A1.TransactionID
   from ApprovalHistoryTable A1
   join PersonTable A2 on A1.LastModifiedBy=A2.PersonID
   Left join DesignationTable A3 on A2.DesignationID=A3.DesignationID
   Left join DepartmentTable A4 on A2.DepartmentID=A4.DepartmentID
   where ApprovalRoleID in (select ApprovalRoleID from ApprovalRoleTable where ApprovalRoleName in ('AMS – Team Leader'))
   and a1.ApproveModuleID=5
   )AP1 on a.TransactionID=AP1.TransactionID
    Left join (select A2.PersonFirstName+'-'+A2.PersonLastName as AreaApproverName,A1.LastModifiedDateTime as AreaApproveDate,
   A3.DesignationName as AreaPostion,A4.DepartmentName as AreaDepartment,A1.Remarks as Comments,A1.TransactionID
   from ApprovalHistoryTable A1
   join PersonTable A2 on A1.LastModifiedBy=A2.PersonID
   Left join DesignationTable A3 on A2.DesignationID=A3.DesignationID
   Left join DepartmentTable A4 on A2.DepartmentID=A4.DepartmentID
   where ApprovalRoleID in (select ApprovalRoleID from ApprovalRoleTable where ApprovalRoleName in ('AMS – HAMS'))
   and a1.ApproveModuleID=5
   )AP2 on a.TransactionID=AP2.TransactionID
   where a.TransactionID=@TransactionID
End 

go 

create Procedure [dbo].[rprc_AssetTransferReceiptHeader]
(
  @TransactionID int 
)
as 
Begin 
   Select P1.PersonFirstName+'-'+p1.PersonLastName as SenderName,L1.LocationName as FromLocationName,L2.LocationName as ToLocationName,P2.PersonFirstName+'-'+p2.PersonLastName as ReceiverName,
    p1.EMailID as SenderEmailID,P1.MobileNo as SenderPhoneno,a.Remarks as Comments,D.DesignationName as SenderJobTitle,D1.DesignationName as ReceiverJobTitle,
	P2.EmailID as ReceiverEmailID,P2.MobileNo as ReceiverPhoneNo,T.TransferTypeName

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
	P2.EmailID as ReceiverEmailID,P2.MobileNo as ReceiverPhoneNo,T.TransferTypeName,ap.ApproveDate,ap.SignaturePath,ap.StampPath

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
		   A1.Remarks as Comments,A1.TransactionID,A2.SignaturePath,A2.StampPath
		   from ApprovalHistoryTable A1
		   join PersonTable A2 on A1.LastModifiedBy=A2.PersonID
		 where A1.OrderNo=1 
		   and a1.ApproveModuleID=10
		   )AP on a.TransactionID=AP.TransactionID
	  where a.TransactionID=@TransactionID
End 
go 

ALTER Procedure [dbo].[rprc_AssetTransferReceiptFooter]
(
  @TransactionID int 
)
as 
Begin 
Select Ap.AreaApproveDate,ap.AreaApproverName,ap.AreaDepartment,ap.AreaPostion,ap.Comments,Ap.SignaturePath as AreaSignature,Ap.StampPath as AreaStamp,
 Ap1.AreaApproveDate as AmsApproveDate,ap1.AreaApproverName as AmsApproerName,ap1.AreaDepartment as AmsDepartment,ap1.AreaPostion as AmsPostion,Ap1.SignaturePath as AMSSignature,
 AP1.StampPath as AmsStamp,ap1.Comments as AmsComments,
  Ap2.AreaApproveDate as ReceiverApproveDate,Ap2.AreaApproverName as ReceiverApproerName,Ap2.AreaDepartment as ReceiverDepartment,Ap2.AreaPostion as ReceiverPostion,
 Ap2.Comments as ReceiverComments,Ap2.SignaturePath as ReceiverSignature,Ap2.StampPath as ReceiverStamp
 From TransactionTable a 
  Left join (select A2.PersonFirstName+'-'+A2.PersonLastName as AreaApproverName,A1.LastModifiedDateTime as AreaApproveDate,
   A3.DesignationName as AreaPostion,A4.DepartmentName as AreaDepartment,A1.Remarks as Comments,A1.TransactionID,A2.SignaturePath,A2.StampPath
   from ApprovalHistoryTable A1
   join PersonTable A2 on A1.LastModifiedBy=A2.PersonID
   Left join DesignationTable A3 on A2.DesignationID=A3.DesignationID
   Left join DepartmentTable A4 on A2.DepartmentID=A4.DepartmentID
   where ApprovalRoleID in (select ApprovalRoleID from ApprovalRoleTable where ApprovalRoleName in ('Area authority in ADEK'))
   and a1.ApproveModuleID=5
   )AP on a.TransactionID=AP.TransactionID
   Left join (select A2.PersonFirstName+'-'+A2.PersonLastName as AreaApproverName,A1.LastModifiedDateTime as AreaApproveDate,
   A3.DesignationName as AreaPostion,A4.DepartmentName as AreaDepartment,A1.Remarks as Comments,A1.TransactionID,A2.SignaturePath,A2.StampPath
   from ApprovalHistoryTable A1
   join PersonTable A2 on A1.LastModifiedBy=A2.PersonID
   Left join DesignationTable A3 on A2.DesignationID=A3.DesignationID
   Left join DepartmentTable A4 on A2.DepartmentID=A4.DepartmentID
   where ApprovalRoleID in (select ApprovalRoleID from ApprovalRoleTable where ApprovalRoleName in ('AMS – Team Leader'))
   and a1.ApproveModuleID=5
   )AP1 on a.TransactionID=AP1.TransactionID
    Left join (select A2.PersonFirstName+'-'+A2.PersonLastName as AreaApproverName,A1.LastModifiedDateTime as AreaApproveDate,
   A3.DesignationName as AreaPostion,A4.DepartmentName as AreaDepartment,A1.Remarks as Comments,A1.TransactionID,A2.SignaturePath,A2.StampPath
   from ApprovalHistoryTable A1
   join PersonTable A2 on A1.LastModifiedBy=A2.PersonID
   Left join DesignationTable A3 on A2.DesignationID=A3.DesignationID
   Left join DepartmentTable A4 on A2.DepartmentID=A4.DepartmentID
   where ApprovalRoleID in (select ApprovalRoleID from ApprovalRoleTable where ApprovalRoleName in ('AMS – HAMS'))
   and a1.ApproveModuleID=5
   )AP2 on a.TransactionID=AP2.TransactionID
   where a.TransactionID=@TransactionID
End 
Go 

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
           Order by b.AssetID asc) as slNo
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
Go 

ALTER Procedure [dbo].[rprc_AssetRetirementReceiptHeader]
(
  @TransactionID int 
)
as 
Begin 
  select  P1.PersonFirstName+'-'+p1.PersonLastName as SenderName,L1.LocationName as FromLocationName,
      p1.EMailID as SenderEmailID,P1.MobileNo as SenderPhoneno,a.Remarks as Comments,D.DesignationName as SenderJobTitle,AP.ApproveDate,Ap.SignaturePath,Ap.StampPath
  from TransactionTable a 
	join (select * from (select  ROW_NUMBER() over( 
         PARTITION BY transactionid  Order by AssetID asc) as serialNo,* from TransactionLineItemTable)y where y.SerialNo=1 ) TL on a.TransactionID=TL.TransactionID
	  join PersonTable P1 on a.CreatedBy=p1.PersonID
	    Left join DesignationTable D on P1.DesignationID=d.DesignationID
		  Left join LocationTable L1 on TL.FromLocationID=L1.LocationID
		   Left join (select A2.PersonFirstName+'-'+A2.PersonLastName as AreaApproverName,A1.LastModifiedDateTime as ApproveDate,
		   A1.Remarks as Comments,A1.TransactionID,A2.SignaturePath,A2.StampPath
		   from ApprovalHistoryTable A1
		   join PersonTable A2 on A1.LastModifiedBy=A2.PersonID
		 where A1.OrderNo=1 
		   and a1.ApproveModuleID=10
		   )AP on a.TransactionID=AP.TransactionID
		  WHERE A.TRANSACTIONID=@TransactionID
End 
go 

ALTER Procedure [dbo].[rprc_AssetRetirementReceiptFooter]
(
  @TransactionID int 
)
as 
Begin 
select  Ap.AreaApproveDate,ap.AreaApproverName,ap.AreaDepartment,ap.AreaPostion,ap.Comments,ap.StampPath as AreaStamp,ap.SignaturePath as AreaSignature,
 Ap1.AreaApproveDate as AmsApproveDate,ap1.AreaApproverName as AmsApproerName,ap1.AreaDepartment as AmsDepartment,ap1.AreaPostion as AmsPostion,
 ap1.Comments as AmsComments,ap1.StampPath as AMSStamp,ap1.SignaturePath as AMSSignature,
  Ap2.AreaApproveDate as ReceiverApproveDate,Ap2.AreaApproverName as ReceiverApproerName,Ap2.AreaDepartment as ReceiverDepartment,Ap2.AreaPostion as ReceiverPostion,
 Ap2.Comments as ReceiverComments,Ap2.SignaturePath as ReceiverSignature ,ap2.StampPath as ReceiverStamp
From TransactionTable a 
  Left join (select A2.PersonFirstName+'-'+A2.PersonLastName as AreaApproverName,A1.LastModifiedDateTime as AreaApproveDate,
   A3.DesignationName as AreaPostion,A4.DepartmentName as AreaDepartment,A1.Remarks as Comments,A1.TransactionID,A2.SignaturePath,A2.StampPath
   from ApprovalHistoryTable A1
   join PersonTable A2 on A1.LastModifiedBy=A2.PersonID
   Left join DesignationTable A3 on A2.DesignationID=A3.DesignationID
   Left join DepartmentTable A4 on A2.DepartmentID=A4.DepartmentID
   where ApprovalRoleID in (select ApprovalRoleID from ApprovalRoleTable where ApprovalRoleName in ('Area authority in ADEK'))
   and a1.ApproveModuleID=10
   )AP on a.TransactionID=AP.TransactionID
   Left join (select A2.PersonFirstName+'-'+A2.PersonLastName as AreaApproverName,A1.LastModifiedDateTime as AreaApproveDate,
   A3.DesignationName as AreaPostion,A4.DepartmentName as AreaDepartment,A1.Remarks as Comments,A1.TransactionID,A2.SignaturePath,A2.StampPath
   from ApprovalHistoryTable A1
   join PersonTable A2 on A1.LastModifiedBy=A2.PersonID
   Left join DesignationTable A3 on A2.DesignationID=A3.DesignationID
   Left join DepartmentTable A4 on A2.DepartmentID=A4.DepartmentID
   where ApprovalRoleID in (select ApprovalRoleID from ApprovalRoleTable where ApprovalRoleName in ('AMS – Team Leader'))
   and a1.ApproveModuleID=10
   )AP1 on a.TransactionID=AP1.TransactionID
    Left join (select A2.PersonFirstName+'-'+A2.PersonLastName as AreaApproverName,A1.LastModifiedDateTime as AreaApproveDate,
   A3.DesignationName as AreaPostion,A4.DepartmentName as AreaDepartment,A1.Remarks as Comments,A1.TransactionID,A2.SignaturePath,A2.StampPath
   from ApprovalHistoryTable A1
   join PersonTable A2 on A1.LastModifiedBy=A2.PersonID
   Left join DesignationTable A3 on A2.DesignationID=A3.DesignationID
   Left join DepartmentTable A4 on A2.DepartmentID=A4.DepartmentID
   where ApprovalRoleID in (select ApprovalRoleID from ApprovalRoleTable where ApprovalRoleName in ('AMS – HAMS'))
   and a1.ApproveModuleID=10
   )AP2 on a.TransactionID=AP2.TransactionID
   where a.TransactionID=@TransactionID
End 
go 
alter Procedure [dbo].[prc_AssetRetirementForApproval]
(
  @UserID int=1012 ,
  @ApprovalID int = 6,
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
          WHERE US.ObjectKeyID = a.ObjectKeyID  and us.TransactionType like '%AssetRetirement'
          ORDER BY Filename
          FOR XML PATH('')), 1, 1, '') [FileName]
FROM DocumentTable a join TransactionTable b on a.ObjectKeyID=b.TransactionID and a.TransactionType='AssetRetirement'
where a.statusID=50
GROUP BY a.ObjectKeyID)inidoc on inidoc.ObjectKeyID=a.TransactionID

left join (SELECT a.ObjectKeyID,
   STUFF((SELECT '; ' + US.Filename 
          FROM DocumentTable US
          WHERE US.ObjectKeyID = a.ObjectKeyID and us.TransactionType like '%AssetRetirementApproval' 
          ORDER BY Filename
          FOR XML PATH('')), 1, 1, '') [FileName]
FROM DocumentTable a join ApprovalHistoryTable b on a.ObjectKeyID=b.ApprovalHistoryID and a.TransactionType='AssetRetirementApproval' and b.ApproveModuleID=10
where a.statusID=50
GROUP BY a.ObjectKeyID)approvaldoc on approvaldoc.ObjectKeyID=b.ApprovalHistoryID

Where b.ApproveModuleID=10  and a.transactionTypeID=10 and b.statusID=150 and b.ApprovalHistoryID=@ApprovalID
End 
go 
create Procedure [dbo].[prc_AssetRetirementForApproval]
(
  @UserID int=1012 ,
  @ApprovalID int = 6,
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
          WHERE US.ObjectKeyID = a.ObjectKeyID  and us.TransactionType like '%AssetRetirement'
          ORDER BY Filename
          FOR XML PATH('')), 1, 1, '') [FileName]
FROM DocumentTable a join TransactionTable b on a.ObjectKeyID=b.TransactionID and a.TransactionType='AssetRetirement'
where a.statusID=50
GROUP BY a.ObjectKeyID)inidoc on inidoc.ObjectKeyID=a.TransactionID

left join (SELECT a.ObjectKeyID,
   STUFF((SELECT '; ' + US.Filename 
          FROM DocumentTable US
          WHERE US.ObjectKeyID = a.ObjectKeyID and us.TransactionType like '%AssetRetirementApproval' 
          ORDER BY Filename
          FOR XML PATH('')), 1, 1, '') [FileName]
FROM DocumentTable a join ApprovalHistoryTable b on a.ObjectKeyID=b.ApprovalHistoryID and a.TransactionType='AssetRetirementApproval' and b.ApproveModuleID=10
where a.statusID=50
GROUP BY a.ObjectKeyID)approvaldoc on approvaldoc.ObjectKeyID=b.ApprovalHistoryID

Where b.ApproveModuleID=10  and a.transactionTypeID=10 and b.statusID=150 and b.ApprovalHistoryID=@ApprovalID
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
           Order by b.AssetID asc) as slNo,a.TransactionNo,'GS/AM/'+convert(nvarchar(100),YEAR(a.CreatedDateTime))+'/AT/'+TransactionNo as Receiptyear
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
ALTER Procedure [dbo].[rprc_AssetRetirementReceipt]
(
  @TransactionID int 
)
as 
Begin 
  Select  ROW_NUMBER() over( 
           Order by b.AssetID asc) as slNo,c.Barcode,p.ProductName as AssetDescription,c.SerialNo,c.AssetRemarks
		   ,'GS/AM/'+convert(nvarchar(100),YEAR(a.CreatedDateTime))+'/AR/' + TransactionNo  as Receiptyear
  From TransactionTable a 
   join TransactionLineItemTable b on a.TransactionID=b.TransactionID
   join AssetTable c on b.AssetID=c.AssetID
   join ProductTable p on c.ProductID=p.ProductID
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
	ap.FromStamp,aP.ToStamp,'GS/AM/'+convert(nvarchar(100),YEAR(a.CreatedDateTime))+'/AT/'+TransactionNo as Receiptyear

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


ALTER Procedure [dbo].[rprc_AssetRetirementReceiptHeader]
(
  @TransactionID int 
)
as 
Begin 
  select  P1.PersonFirstName+'-'+p1.PersonLastName as SenderName,L1.LocationName as FromLocationName,
      p1.EMailID as SenderEmailID,P1.MobileNo as SenderPhoneno,a.Remarks as Comments,D.DesignationName as SenderJobTitle,AP.ApproveDate,Ap.SignaturePath
	  ,Ap.FromStamp,ap.ToStamp,a.TransactionNo,'GS/AM/'+convert(nvarchar(100),YEAR(a.CreatedDateTime))+'/AR/' + TransactionNo  as Receiptyear
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

ALTER view [dbo].[TransactionView] 
as 

SElect a.*,b.TransactionTypeName, case when c.StatusID=50 then 'Approved' else c.Status end as Status,p.PersonFirstName+'-'+p.PersonLastName as CreatedUSer
From TransactionTable a 
join TransactionTypeTable b on a.TransactionTypeID=b.TransactionTypeID 
join  StatusTable c on a.StatusID=c.StatusID
join PersonTable p on a.CreatedBy=p.PersonID

GO

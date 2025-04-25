IF OBJECT_ID('DelegateRoleTable') IS NULL
BEGIN
  Create table DelegateRoleTable
  (
    DelegateRoleID int not null primary key identity(1,1),
	EmployeeID int not null foreign key references User_LoginUserTable(UserID),
	DelegatedEmployeeID int not null foreign key references User_LoginUserTable(UserID),
	EffectiveStartDate DateTime not null, 
	EffectiveEndDate DateTime not null, 
	CreatedBy int not null foreign key references User_LoginUserTable(UserID),
	CreatedDateTime SmallDatetime not null,
	LastModifiedBy int  null foreign key references User_LoginUserTable(UserID),
	LastModifiedDateTime SmallDatetime
  )
End 
Go 



If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='DelegateRole')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('DelegateRole','DelegateRole',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Master'),1,0);
End
Go


If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='DelegateRole')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'DelegateRole',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='DelegateRole'),'/MasterPage/Index?pageName=DelegateRole',
(Select MenuID from USER_MENUTABLE where MenuName='Master' ),9);
End
Go


IF OBJECT_ID('ReasonTypeTable') IS NULL
BEGIN
  Create table ReasonTypeTable
  (
    ReasonTypeID int not null primary key identity(1,1),
	ReasonCode nvarchar(50) NOT NULL UNIQUE,
	ReasonName nvarchar(100) NOT NULL,
	StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references User_LoginUserTable(UserID),
	CreatedDateTime SmallDatetime not null,
	LastModifiedBy int  null foreign key references User_LoginUserTable(UserID),
	LastModifiedDateTime SmallDatetime
  )
End 
Go 


If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='ReasonTypeTable')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('ReasonType','ReasonType',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Master'),1,0);
End
Go


If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='ReasonType')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'ReasonType',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='ReasonType'),'/MasterPage/Index?pageName=ReasonType',
(Select MenuID from USER_MENUTABLE where MenuName='Master' ),9);
End
Go


ALTER Table DelegateRoleTable 
Add StatusID int foreign key references StatusTable(StatusID)


ALTER Table DelegateRoleTable 
Add ReasonTypeID int foreign key references ReasonTypeTable(ReasonTypeID)


If not exists(SELECT  RightGroupName FROM USER_RIGHTGROUPTABLE where RightGroupName='Admin')
Begin
INSERT into USER_RIGHTGROUPTABLE(RightGroupName,RightGroupDesc,ParentRightGroupID)
Values('Admin','Admin',8)
END


If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='MasterGridLineItem')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('MasterGridLineItem','ColumnConfigurations',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Admin'),1,0);
End
Go

If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='Admin')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'Admin',null,'HandlerNull()',null,7);
End
Go

If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='ColumnConfigurations')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'ColumnConfigurations',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='MasterGridLineItem'),'/MasterPage/Index?pageName=MasterGridLineItem',
(Select MenuID from USER_MENUTABLE where MenuName='Admin' ),9);
End
Go


ALTER PROC [dbo].[aprc_GetNotificationInputs]
AS
BEGIN
-- to update the delegate role which effective enddate is completed
	UPDATE DelegateRoleTable set StatusID=50 Where EffectiveEndDate<DATEADD(day, -1, GETDATE())

	SELECT DISTINCT A.NotificationInputID, A.SYSDataID1, A.SYSDataID2, A.SYSDataID3, 
			M.QueryType, M.QueryString, T.*,TT.IsAttachmentRequired,TT.AllowHtmlContent,X.TemplateTableBodyContentHeader,X.TemplateTableBodyContent,X.TemplateTableBodyContentFooter
		FROM NotificationInputTable A
		LEFT JOIN NotificationTemplateTable T ON T.NotificationTemplateID = A.NotificationTemplateID
		LEFT JOIN NotificationModuleTable M ON M.NotificationModuleID = T.NotificationModuleID
		LEFT JOIN NotificationTypeTable TT ON TT.NotificationTypeID = T.NotificationTypeID
		LEFT JOIN (Select A.NotificationTemplateID,'<table style="border: 1px solid black;border-collapse: collapse;"><tbody>'
		+'<tr>'+STRING_AGG('<td style="width:100px;border: 1px solid black;border-collapse: collapse;">'+(B.FieldName)+'</td>','')+'</tr>' as TemplateTableBodyContentHeader,
		'<tr>'+STRING_AGG('<td style="width:100px;border: 1px solid black;border-collapse: collapse;">##'+UPPER(B.FieldName)+'##</td>','')+'</tr>' as TemplateTableBodyContent,
		'</tbody></table>' as TemplateTableBodyContentFooter
		from NotificationTemplateFieldTable A
		Left Join NotificationModuleFieldTable B on A.NotificationFieldID=B.NotificationFieldID
		WHERE A.StatusID=5 AND B.StatusID=5
		Group by  A.NotificationTemplateID
		)X ON X.NotificationTemplateID=T.NotificationTemplateID

		WHERE A.IsCompleted = 0
END
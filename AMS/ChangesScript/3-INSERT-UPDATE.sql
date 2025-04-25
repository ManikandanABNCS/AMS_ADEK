if not exists(select ApplicationName from APP_APPLICATIONS where ApplicationName='AMS')
Begin
Insert into APP_APPLICATIONS(ApplicationName,Description)Values('AMS','AMS')
END
go

if not exists(select Status from StatusTable where Status='Active')
Begin
Insert into StatusTable(StatusID,Status)Values(5,'Active')
END
go
If not exists(select Status from StatusTable where Status='Inactive')
Begin
Insert into StatusTable(StatusID,Status)Values(20,'Inactive')
END
go
If not exists(select Status from StatusTable where Status='Deleted')
Begin
Insert into StatusTable(StatusID,Status)Values(30,'Deleted')
END
go

If not exists(select UserType from UserTypeTable where UserType='Web')
Begin
Insert into UserTypeTable(UserTypeID,UserType,StatusID)Values(1,'Web',5)
END
go

If not exists(select UserName from USER_LOGINUSERTABLE where UserName='Admin')
Begin
INSERT INTO USER_LOGINUSERTABLE(  USERNAME, PASSWORD, PASSWORDSALT, LASTLOGGEDINDATE, CHANGEPASSWORDATNEXTLOGIN, EMAIL, PASSWORDQUESTION, PASSWORDANSWER,
ISLOCKEDOUT, ISDISABLED, ISAPPROVED, CREATEDDATE, LASTACTIVITYDATE, LASTLOGINDATE, LASTPASSWORDCHANGEDDATE, LASTLOCKOUTDATE, FAILEDPASSWORDATTEMPTCOUNT,
PasswordAttemptWindowStart, PasswordAnswerAttemptCount, PasswordAnswerAttemptWindowStart, LASTLOGINIPADDRESS, APPLICATIONID, USERCOMMENT)
Values('Admin','TbCdFiAFDlwngDn2cFO0oR9BTVc=','fIdUH9Pz71AW4S1BGQDIemBGqOg=',null,1,null,null,null,0,0,1,GETDATE(),GETDATE(),GETDATE(),null,null,3,null,null,
null,null,1,null);
End
go

If not exists(SELECT PERSONCODE V_count FROM PERSONTABLE WHERE PERSONCODE='Admin')
Begin
INSERT INTO PERSONTABLE( PersonID,PERSONCODE,PersonFirstName,PersonLastName, STATUSID, USERTYPEID,Culture)
Values((select UserID from USER_LOGINUSERTABLE where UserName='Admin'),'Admin','Admin','Admin',5,(select UserTypeId from UserTypeTable where UserType='Web'),'en-GB');
End
go

If not exists(SELECT ROLENAME V_count FROM USER_ROLETABLE WHERE ROLENAME='Admin')
Begin
 Insert into USER_ROLETABLE( ROLENAME, ISACTIVE, ISDELETED, APPLICATIONID, DESCRIPTION, DISPLAYROLE)
Values('Admin',1,0,1,'Administrative Role',1);
End
Go

If not exists(SELECT UserID FROM USER_USERROLETABLE WHERE UserID=(select UserID from User_LoginUserTable where UserName='admin'))
Begin
 Insert into USER_USERROLETABLE( UserID, RoleID)
Values((select UserID from User_LoginUserTable where UserName='admin'),
(select RoleID from USER_ROLETABLE where ROLENAME='Admin')
);

End
Go


If not exists(SELECT RIGHTGROUPNAME FROM USER_RIGHTGROUPTABLE WHERE RIGHTGROUPNAME='Master')
Begin
Insert into USER_RIGHTGROUPTABLE( PARENTRIGHTGROUPID, RIGHTGROUPNAME, RIGHTGROUPDESC)
Values(1,'Master','Master')
End
go

If not exists(SELECT RIGHTGROUPNAME FROM USER_RIGHTGROUPTABLE WHERE RIGHTGROUPNAME='Transaction')
Begin
Insert into USER_RIGHTGROUPTABLE( PARENTRIGHTGROUPID, RIGHTGROUPNAME, RIGHTGROUPDESC)
Values(2,'Transaction','Transaction')
End
go

If not exists(SELECT RIGHTGROUPNAME FROM USER_RIGHTGROUPTABLE WHERE RIGHTGROUPNAME='Reports')
Begin
Insert into USER_RIGHTGROUPTABLE( PARENTRIGHTGROUPID, RIGHTGROUPNAME, RIGHTGROUPDESC)
Values(3,'Reports','Reports')
End
go

If not exists(SELECT RIGHTGROUPNAME FROM USER_RIGHTGROUPTABLE WHERE RIGHTGROUPNAME='Tools')
Begin
Insert into USER_RIGHTGROUPTABLE( PARENTRIGHTGROUPID, RIGHTGROUPNAME, RIGHTGROUPDESC)
Values(4,'Tools','Tools')
End
go


If not exists(SELECT MenuName FROM USER_MENUTABLE where MenuName='Master')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(

'Master',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='Master'),'HandlerNull()',
(Select MenuID from USER_MENUTABLE where MenuName='Master' ),1);
End
go

If not exists(SELECT MenuName  FROM USER_MENUTABLE where MenuName='Transaction')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(

'Transaction',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='Transaction'),'HandlerNull()',
(Select MenuID from USER_MENUTABLE where MenuName='Transaction' ),2);
End
go

If not exists(SELECT MenuName  FROM USER_MENUTABLE where MenuName='Reports')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(

'Reports',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='Reports'),'HandlerNull()',
(Select MenuID from USER_MENUTABLE where MenuName='Reports' ),6);
End
go

If not exists(SELECT MenuName  FROM USER_MENUTABLE where MenuName='Tools')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(

'Tools',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='Tools'),'HandlerNull()',
(Select MenuID from USER_MENUTABLE where MenuName='Tools' ),7);
End
go



If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='UserMaster')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('UserMaster','UserMaster',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Tools'),1,0);
End
Go


If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='UserMaster')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'UserMaster',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='UserMaster'),'/Person/Index',
(Select MenuID from USER_MENUTABLE where MenuName='Tools' ),1);
End
Go

If not exists(SELECT RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='RoleMaster')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('RoleMaster','RoleMaster',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Tools'),1,0);
End
go



If not exists(SELECT MenuName FROM USER_MENUTABLE where MenuName='RoleMaster')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'RoleMaster',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='RoleMaster'),'/Role/Index',
(Select MenuID from USER_MENUTABLE where MenuName='Tools' ),2);
End
go


If not exists(SELECT RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='UserRole')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values( 'UserRole','UserRole',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Tools'),1,0);
end
go



If not exists(SELECT MenuName FROM USER_MENUTABLE where MenuName='UserRole')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(

'UserRole',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='UserRole'),'/Account/UserRole',
(Select MenuID from USER_MENUTABLE where MenuName='Tools' ),3);
end
go


If not exists(SELECT RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='UserRights')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values( 'UserRights','UserRights',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Tools'),1,0);
end
go



If not exists(SELECT MenuName FROM USER_MENUTABLE where MenuName='UserRights')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(

'UserRights',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='UserRights'),'/Account/UserRights',
(Select MenuID from USER_MENUTABLE where MenuName='Tools' ),4);
end
go

If not exists(SELECT RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='RoleRights')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values( 'RoleRights','RoleRights',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Tools'),1,0);
end
go



If not exists(SELECT MenuName FROM USER_MENUTABLE where MenuName='RoleRights')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(

'RoleRights',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='RoleRights'),'/Account/RoleRights',
(Select MenuID from USER_MENUTABLE where MenuName='Tools' ),5);
end
go


If not exists(SELECT RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='ChangePassword')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values( 'ChangePassword','ChangePassword',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Tools'),1,0);
end
go



If not exists(SELECT MenuName FROM USER_MENUTABLE where MenuName='ChangePassword')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(

'ChangePassword',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='ChangePassword'),'/Account/ChangePassword',
(Select MenuID from USER_MENUTABLE where MenuName='Tools' ),6);
end
go


If not exists(SELECT RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='Configuration')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values( 'Configuration','Configuration',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Tools'),1,0);
end
go



If not exists(SELECT MenuName FROM USER_MENUTABLE where MenuName='Configuration')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(

'Configuration',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='Configuration'),'/Configuration/Edit',
(Select MenuID from USER_MENUTABLE where MenuName='Tools' ),7);
end
go

If not exists(SELECT RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='ErrorLog')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values( 'ErrorLog','ErrorLog',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Tools'),1,0);
end
go



If not exists(SELECT MenuName FROM USER_MENUTABLE where MenuName='ErrorLog')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(

'ErrorLog',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='ErrorLog'),'/AppErrorLog/Index',
(Select MenuID from USER_MENUTABLE where MenuName='Tools' ),8);
end
go


If not exists(SELECT LanguageName FROM [LanguageTable] where LanguageName='English')
Begin
Insert into LanguageTable(LanguageName,CultureSymbol,CreatedBy,CreatedDateTime,IsDefault) Values(
'English','en-GB',(select userID from User_LoginUserTable where Username='Admin'),getdate(),1)
end
go
If not exists(SELECT LanguageName FROM [LanguageTable] where LanguageName='Arabic')
Begin
Insert into LanguageTable(LanguageName,CultureSymbol,CreatedBy,CreatedDateTime,IsDefault) Values(
'Arabic','ar-QA',(select userID from User_LoginUserTable where Username='Admin'),getdate(),0)
end
go


If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='LanguageContent')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('LanguageContent','LanguageContent',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Master'),0,1);
End
Go


If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='LanguageContent')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'LanguageContent',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='LanguageContent'),'/LanguageContent/Index',
(Select MenuID from USER_MENUTABLE where MenuName='Master' ),9);
End
Go

If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='AssetCondition')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('AssetCondition','AssetCondition',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Master'),1,0);
End
Go


If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='AssetCondition')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'AssetCondition',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='AssetCondition'),'/MasterPage/Index?pageName=AssetCondition',
(Select MenuID from USER_MENUTABLE where MenuName='Master' ),9);
End
Go


If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='Department')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('Department','Department',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Master'),1,0);
End
Go

If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='Department')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'Department',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='Department'),'/MasterPage/Index?pageName=Department',
(Select MenuID from USER_MENUTABLE where MenuName='Master' ),10);
End
Go
If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='Company')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('Company','Company',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Master'),1,0);
End
Go

If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='Company')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'Company',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='Company'),'/MasterPage/Index?pageName=Company',
(Select MenuID from USER_MENUTABLE where MenuName='Master' ),11);
End
Go
If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='Warehouse')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('Warehouse','Warehouse',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Master'),1,0);
End
Go

If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='Warehouse')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'Warehouse',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='Warehouse'),'/MasterPage/Index?pageName=Warehouse',
(Select MenuID from USER_MENUTABLE where MenuName='Master' ),12);
End
Go
If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='TransferType')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('TransferType','TransferType',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Master'),1,0);
End
Go

If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='TransferType')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'TransferType',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='TransferType'),'/MasterPage/Index?pageName=TransferType',
(Select MenuID from USER_MENUTABLE where MenuName='Master' ),13);
End
Go
If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='Designation')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('Designation','Designation',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Master'),1,0);
End
Go

If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='Designation')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'Designation',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='Designation'),'/MasterPage/Index?pageName=Designation',
(Select MenuID from USER_MENUTABLE where MenuName='Master' ),14);
End
Go

If not exists(SELECT  masterGridName FROM MasterGridTable where masterGridName='Role')
Begin
Insert into MasterGridTable(masterGridName,EntityName)
Values('Role','RolesModel');
End
Go

If not exists(SELECT  FieldName FROM MasterGridLineItemTable where masterGridID in (select masterGridID from MasterGridTable where masterGridName='Role') and FieldName='RoleName')
Begin
Insert into MasterGridLineItemTable(MasterGridID,FieldName,DisplayName,Width,Format,IsDefault,OrderID)
Values((select masterGridID from MasterGridTable where masterGridName='Role'),'RoleName','RoleName',100,null,1,1);
End
Go

If not exists(SELECT  FieldName FROM MasterGridLineItemTable where masterGridID in (select masterGridID from MasterGridTable where masterGridName='Role') and FieldName='Description')
Begin
Insert into MasterGridLineItemTable(MasterGridID,FieldName,DisplayName,Width,Format,IsDefault,OrderID)
Values((select masterGridID from MasterGridTable where masterGridName='Role'),'Description','Description',100,null,1,2);
End
Go

If not exists(SELECT  masterGridName FROM MasterGridTable where masterGridName='AppErrorLog')
Begin
Insert into MasterGridTable(masterGridName,EntityName)
Values('AppErrorLog','ApplicationErrorLogTable');
End
Go

If not exists(SELECT  FieldName FROM MasterGridLineItemTable where masterGridID in (select masterGridID from MasterGridTable where masterGridName='AppErrorLog') and FieldName='ErrorMessage')
Begin
Insert into MasterGridLineItemTable(MasterGridID,FieldName,DisplayName,Width,Format,IsDefault,OrderID)
Values((select masterGridID from MasterGridTable where masterGridName='AppErrorLog'),'ErrorMessage','ErrorMessage',100,null,1,1);
End
Go

If not exists(SELECT  FieldName FROM MasterGridLineItemTable where masterGridID in (select masterGridID from MasterGridTable where masterGridName='AppErrorLog') and FieldName='StackTrace')
Begin
Insert into MasterGridLineItemTable(MasterGridID,FieldName,DisplayName,Width,Format,IsDefault,OrderID)
Values((select masterGridID from MasterGridTable where masterGridName='AppErrorLog'),'StackTrace','StackTrace',100,null,1,2);
End
Go

If not exists(SELECT  masterGridName FROM MasterGridTable where masterGridName='Person')
Begin
Insert into MasterGridTable(masterGridName,EntityName)
Values('Person','PersonView');
End
Go

If not exists(SELECT  FieldName FROM MasterGridLineItemTable where masterGridID in (select masterGridID from MasterGridTable where masterGridName='Person') and FieldName='Username')
Begin
Insert into MasterGridLineItemTable(MasterGridID,FieldName,DisplayName,Width,Format,IsDefault,OrderID)
Values((select masterGridID from MasterGridTable where masterGridName='Person'),'Username','Username',100,null,1,1);
End
Go

If not exists(SELECT  FieldName FROM MasterGridLineItemTable where masterGridID in (select masterGridID from MasterGridTable where masterGridName='Person') and FieldName='PersonFirstName')
Begin
Insert into MasterGridLineItemTable(MasterGridID,FieldName,DisplayName,Width,Format,IsDefault,OrderID)
Values((select masterGridID from MasterGridTable where masterGridName='Person'),'PersonFirstName','PersonFirstName',100,null,1,2);
End
Go

If not exists(SELECT  FieldName FROM MasterGridLineItemTable where masterGridID in (select masterGridID from MasterGridTable where masterGridName='Person') and FieldName='PersonLastName')
Begin
Insert into MasterGridLineItemTable(MasterGridID,FieldName,DisplayName,Width,Format,IsDefault,OrderID)
Values((select masterGridID from MasterGridTable where masterGridName='Person'),'PersonLastName','PersonLastName',100,null,1,3);
End
Go


If not exists(SELECT  FieldName FROM MasterGridLineItemTable where masterGridID in (select masterGridID from MasterGridTable where masterGridName='Person') and FieldName='MobileNo')
Begin
Insert into MasterGridLineItemTable(MasterGridID,FieldName,DisplayName,Width,Format,IsDefault,OrderID)
Values((select masterGridID from MasterGridTable where masterGridName='Person'),'MobileNo','MobileNo',100,null,1,4);
End
Go

If not exists(SELECT  FieldName FROM MasterGridLineItemTable where masterGridID in (select masterGridID from MasterGridTable where masterGridName='Person') and FieldName='EMailID')
Begin
Insert into MasterGridLineItemTable(MasterGridID,FieldName,DisplayName,Width,Format,IsDefault,OrderID)
Values((select masterGridID from MasterGridTable where masterGridName='Person'),'EMailID','EMailID',100,null,1,5);
End
Go


If not exists(SELECT  FieldName FROM MasterGridLineItemTable where masterGridID in (select masterGridID from MasterGridTable where masterGridName='Person') and FieldName='UserType')
Begin
Insert into MasterGridLineItemTable(MasterGridID,FieldName,DisplayName,Width,Format,IsDefault,OrderID)
Values((select masterGridID from MasterGridTable where masterGridName='Person'),'UserType','UserType',100,null,1,6	);
End
Go

If not exists(SELECT  FieldName FROM MasterGridLineItemTable where masterGridID in (select masterGridID from MasterGridTable where masterGridName='Person') and FieldName='Status')
Begin
Insert into MasterGridLineItemTable(MasterGridID,FieldName,DisplayName,Width,Format,IsDefault,OrderID)
Values((select masterGridID from MasterGridTable where masterGridName='Person'),'Status','Status',100,null,1,9);
End
Go

If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='EntityCode')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('EntityCode','EntityCode',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Master'),1,0);
End
Go

If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='EntityCode')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'EntityCode',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='Company'),'/MasterPage/Index?pageName=EntityCode',
(Select MenuID from USER_MENUTABLE where MenuName='Master' ),11);
End
Go

If  exists(SELECT  Status FROM StatusTable where Status='Inactive')
Begin
Update StatusTable set StatusId=50 where Status='Inactive'
End
Go
If  exists(SELECT  Status FROM StatusTable where Status='Deleted')
Begin
Update StatusTable set StatusId=100 where Status='Deleted'
End
Go

If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='Section')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('Section','Section',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Master'),1,0);
End
Go

If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='Section')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'Section',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='Section'),'/MasterPage/Index?pageName=Section',
(Select MenuID from USER_MENUTABLE where MenuName='Master' ),15);
End
Go

If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='Party')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('Party','Party',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Master'),1,0);
End
Go

If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='Party')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'Party',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='Party'),'/MasterPage/Index?pageName=Party',
(Select MenuID from USER_MENUTABLE where MenuName='Master' ),15);
End
Go
If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='Project')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('Project','Project',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Master'),1,0);
End
Go

If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='Project')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'Project',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='Project'),'/MasterPage/Index?pageName=Project',
(Select MenuID from USER_MENUTABLE where MenuName='Master' ),15);
End
Go
If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='UOM')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('UOM','UOM',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Master'),1,0);
End
Go

If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='UOM')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'UOM',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='UOM'),'/MasterPage/Index?pageName=UOM',
(Select MenuID from USER_MENUTABLE where MenuName='Master' ),15);
End
Go
If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='VAT')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('VAT','VAT',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Master'),1,0);
End
Go

If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='VAT')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'VAT',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='VAT'),'/MasterPage/Index?pageName=VAT',
(Select MenuID from USER_MENUTABLE where MenuName='Master' ),15);
End
Go

If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='Country')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('Country','Country',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Master'),1,0);
End
Go

If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='Country')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'Country',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='Country'),'/MasterPage/Index?pageName=Country',
(Select MenuID from USER_MENUTABLE where MenuName='Master' ),15);
End
Go
If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='Currency')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('Currency','Currency',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Master'),1,0);
End
Go

If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='Currency')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'Currency',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='Currency'),'/MasterPage/Index?pageName=Currency',
(Select MenuID from USER_MENUTABLE where MenuName='Master' ),15);
End
Go

If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='Location')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('Location','Location',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Master'),1,0);
End
Go

If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='Location')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'Location',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='Location'),'/MasterPage/Index?pageName=Location',
(Select MenuID from USER_MENUTABLE where MenuName='Master' ),15);
End
Go

If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='Category')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('Category','Category',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Master'),1,0);
End
Go

If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='Category')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'Category',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='Category'),'/MasterPage/Index?pageName=Category',
(Select MenuID from USER_MENUTABLE where MenuName='Master' ),15);
End
Go

If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='Category')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('Category','Category',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Master'),1,0);
End
Go


If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='Category')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'Category',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='Category'),'/Category/Index',
(Select MenuID from USER_MENUTABLE where MenuName='Master' ),1);
End
Go

If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='Product')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('Product','Product',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Master'),1,0);
End
Go

If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='Product')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'Product',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='Product'),'/MasterPage/Index?pageName=Product',
(Select MenuID from USER_MENUTABLE where MenuName='Master' ),15);
End
Go


If not exists(select RightGroupName from USER_RIGHTGROUPTABLE where RightGroupName='Template')
Begin
   Insert into USER_RIGHTGROUPTABLE(RightGroupName,RightGroupDesc)
   values('Template','Template')
End 
Go
update USER_RIGHTGROUPTABLE set ParentRightGroupID=(select RightGroupID from User_RightGroupTable where RightGroupDesc='template') where RightGroupDesc='Template'
go 

If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='NotificationModule')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('NotificationModule','NotificationModule',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Template'),1,0);
End
Go
update USER_RIGHTTABLE set RightGroupID=(select RightGroupID from User_RightGroupTable where RightGroupDesc='template') where rightname='NotificationModule'

If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='Template')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'Template',
NULL,'HandlerNull()',
NULL,4);
End
Go
If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='NotificationModule')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'NotificationModule',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='NotificationModule'),'/MasterPage/Index?pageName=NotificationModule',
(Select MenuID from USER_MENUTABLE where MenuName='Template' ),1);
End
Go
If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='NotificationTemplate')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('NotificationTemplate','NotificationTemplate',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Template'),1,0);
End
Go
If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='NotificationTemplate')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'NotificationTemplate',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='NotificationTemplate'),'/MasterPage/Index?pageName=NotificationTemplate',
(Select MenuID from USER_MENUTABLE where MenuName='Template' ),1);
End
Go


If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='AssetTransfer')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('AssetTransfer','AssetTransfer',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Transaction'),1,0);
End
Go
If not exists(SELECT MenuName FROM USER_MENUTABLE where MenuName='AssetTransfer')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(

'AssetTransfer',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='TransferAsset'),'/AssetTransfer/Index',
(Select MenuID from USER_MENUTABLE where MenuName='Transaction' ),8);
end
go


insert into MasterGridTable (MasterGridName,EntityName) values('AssetTransfer','AssetTable') 
Go 

Insert into MasterGridLineItemTable(FieldName,DisplayName,Width,Format,
IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
select FieldName,DisplayName,Width,Format,
IsDefault,OrderID,ColumnType,ActionScript,(select MasterGridID from MasterGridTable where MasterGridName='AssetTransfer')
from MasterGridLineItemTable where MasterGridID=(select MasterGridID from MasterGridTable where MasterGridName='Asset')
go
insert into MasterGridTable (MasterGridName,EntityName) values('PopupItem','AssetTable') 
Go 
Insert into MasterGridLineItemTable(FieldName,DisplayName,Width,Format,
IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
select FieldName,DisplayName,Width,Format,
IsDefault,OrderID,ColumnType,ActionScript,(select MasterGridID from MasterGridTable where MasterGridName='PopupItem')
from MasterGridLineItemTable where MasterGridID=(select MasterGridID from MasterGridTable where MasterGridName='Asset')
gO 


if not exists(select LocationType from LocationTypeTable where locationtype='School')
Begin 
insert into LocationTypeTable(LocationType) values('School') 
End 
go 
if not exists(select LocationType from LocationTypeTable where locationtype='Warehouse')
Begin 
insert into LocationTypeTable(LocationType) values('Warehouse') 
End 
go 
if not exists(select LocationType from LocationTypeTable where locationtype='Head Quarters')
Begin 
insert into LocationTypeTable(LocationType) values('Head Quarters') 
End 
go 



if not exists(select MasterGridName from MasterGridTable where MasterGridName='ApprovalWorkflow')
Begin 
insert into MasterGridTable (MasterGridName,EntityName) values('ApprovalWorkflow','ApproveWorkflowTable') 
End 
Go 
if not exists(select FieldName from MasterGridLineItemTable where FieldName='ApproveModule.ModuleName' and MasterGridID =(select mastergridid from MasterGridTable where MasterGridName='ApprovalWorkflow'))
Begin 
Insert into MasterGridLineItemTable(FieldName,DisplayName,Width,Format,
IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
select 'ApproveModule.ModuleName','ModuleName',100,NULL,0,1,'System.String',NULL,MasterGridID
from MasterGridTable where MasterGridName='ApprovalWorkflow'

End 
go 
if not exists(select FieldName from MasterGridLineItemTable where FieldName='FromLocationType.LocationType' and MasterGridID =(select mastergridid from MasterGridTable where MasterGridName='ApprovalWorkflow'))
Begin 
Insert into MasterGridLineItemTable(FieldName,DisplayName,Width,Format,
IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
select 'FromLocationType.LocationType','FromLocationType',100,NULL,1,1,'System.String',NULL,MasterGridID
from MasterGridTable where MasterGridName='ApprovalWorkflow'

End 
go 
if not exists(select FieldName from MasterGridLineItemTable where FieldName='ToLocationType.LocationType' and MasterGridID =(select mastergridid from MasterGridTable where MasterGridName='ApprovalWorkflow'))
Begin 
Insert into MasterGridLineItemTable(FieldName,DisplayName,Width,Format,
IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
select 'ToLocationType.LocationType','ToLocationType',100,NULL,1,1,'System.String',NULL,MasterGridID
from MasterGridTable where MasterGridName='ApprovalWorkflow'

End 
go 
if not exists(select ModuleName from ApproveModuleTable where ModuleName='AssetTransfer')
Begin 
insert into ApproveModuleTable(ApproveModuleID,ModuleName,StatusID) values(5,'AssetTransfer',5) 
End 
go 
if not exists(select ModuleName from ApproveModuleTable where ModuleName='AssetRetirement')
Begin 
insert into ApproveModuleTable(ApproveModuleID,ModuleName,StatusID) values(10,'AssetRetirement',5) 
End 
go 

update MasterGridTable set MasterGridName='ApproveWorkflow' where EntityName='ApproveWorkflowTable'
go 

If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='ApproveWorkflow')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('ApproveWorkflow','ApproveWorkflow',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Master'),1,0);
End
Go
If not exists(SELECT MenuName FROM USER_MENUTABLE where MenuName='ApprovalWorkflow')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(

'ApprovalWorkflow',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='ApproveWorkflow'),'/ApproveWorkflow/Index',
(Select MenuID from USER_MENUTABLE where MenuName='Transaction' ),8);
end
go
if not exists(select EntityCodeName from EntityCodeTable where EntityCodeName='AssetTransfer')
Begin 
Insert into EntityCodeTable (EntityCodeName,
CodePrefix,
CodeSuffix,
CodeFormatString,
LastUsedNo,
UseDateTime) 
Values('AssetTransfer','REF',NULL,'{0:00000}',0,0)
End 
Go 

If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='ApprovalRole')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('ApprovalRole','ApprovalRole',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Master'),1,0);
End
Go
If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='ApprovalRole')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'ApprovalRole',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='ApprovalRole'),'/MasterPage/Index?pageName=ApprovalRole',
(Select MenuID from USER_MENUTABLE where MenuName='Master' ),1);
End
Go


update MasterGridTable set EntityName='AssetView' where MasterGridName='PopupItem'
	Go 
	delete from MasterGridLineItemTable where MasterGridID in (select MasterGridID from MasterGridTable where mastergridname='PopupItem') 
	go 
	Insert into MasterGridLineItemTable(FieldName,DisplayName,Width,Format,
IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
SELECT 
    c.name, c.name,100,NULL,1,ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS Orderno,
	Case when t.name='nvarchar' or t.name='varchar' then 'System.String' when t.name='smalldatetime' or t.name='date' then 'System.DateTime'
	when t.name='decimal' then 'System.Decimal' when t.name='bit' then 'System.Boolean'  when t.name='int' or t.name='tinyint' then 'System.Int32'  else 'System.String' end 
	,NULL,(select MasterGridID from MasterGridTable where mastergridname='PopupItem')
  
FROM    
    sys.columns c
INNER JOIN 
    sys.types t ON c.user_type_id = t.user_type_id
LEFT OUTER JOIN 
    sys.index_columns ic ON ic.object_id = c.object_id AND ic.column_id = c.column_id
LEFT OUTER JOIN 
    sys.indexes i ON ic.object_id = i.object_id AND ic.index_id = i.index_id
WHERE
    c.object_id = OBJECT_ID('assetview') and c.name not like '%id%'
	Go 
	 if not exists(select Status from StatusTable where Status='WaitingForApproval')
  Begin
     insert into StatusTable(StatusID,Status)values(101,'WaitingForApproval')
  End
  go 
  select * from AssetTransferTransactionLineItemTable

  if not exists(select TransactionTypeName from TransactionTypeTable where TransactionTypeName='AssetTransfer')
  Begin
   insert into TransactionTypeTable(TransactionTypeID,TransactionTypeName,IsSourceTransactionType,TransactionTypeDesc)
   values(5,'AssetTransfer',0,'AsetTransfer')
  end 
  go 
 if not exists(select PostingStatusID from PostingStatusTable where PostingStatus='WorkInProgress')
  Begin
     insert into PostingStatusTable(PostingStatusID,PostingStatus)values(1,'WorkInProgress')
  End
  go 

  if not exists(select RightGroupName from User_RightGroupTable where RightGroupDesc='Approval')
Begin
  Insert into User_RightGroupTable(RightGroupDesc,RightGroupName) 
  values('Approval','Approval')

  update User_RightGroupTable set ParentRightGroupID=RightGroupID where RightGroupDesc='Approval'
End 

  If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='AssetTransferApproval')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('AssetTransferApproval','AssetTransferApproval',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Approval'),1,0);
End
Go
If not exists(select menuname from User_MenuTable where MenuName='Approval' and TargetObject='HandlerNull()') 
Begin
   Insert into User_MenuTable(MenuName,TargetObject,OrderNo)
   values('Approval','HandlerNull()', 5)
End 
Go 
If not exists(select menuname from User_MenuTable where MenuName='AssetTransferApproval' and TargetObject='AssetTransferApproval/Index' and ParentMenuID=(select MenuID from User_MenuTable where MenuName='Approval')) 
Begin
 Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(

'AssetTransferApproval',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='AssetTransferApproval'),'/AssetTransferApproval/Index',
(Select MenuID from USER_MENUTABLE where MenuName='Approval' ),8);
end
go 

 If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='TransactionList')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('TransactionList','TransactionList',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Transaction'),1,0);
End
Go

If not exists(SELECT MenuName FROM USER_MENUTABLE where MenuName='TransactionList')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(

'TransactionList',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='TransactionList'),'/TransactionList/Index',
(Select MenuID from USER_MENUTABLE where MenuName='Transaction' ),8);
end
go


	 if not exists(select Status from StatusTable where Status='Rejected')
  Begin
     insert into StatusTable(StatusID,Status)values(102,'Rejected')
  End
  go 
  
  If not Exists(select MasterGridName from MasterGridTable where MasterGridName='AssetRetirement')
  Begin 

insert into MasterGridTable (MasterGridName,EntityName) values('AssetRetirement','AssetTable') 


Insert into MasterGridLineItemTable(FieldName,DisplayName,Width,Format,
IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
select FieldName,DisplayName,Width,Format,
IsDefault,OrderID,ColumnType,ActionScript,(select MasterGridID from MasterGridTable where MasterGridName='AssetRetirement')
from MasterGridLineItemTable where MasterGridID=(select MasterGridID from MasterGridTable where MasterGridName='Asset')
End 
go


If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='AssetRetirement')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('AssetRetirement','AssetRetirement',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Transaction'),1,0);
End
Go
If not exists(SELECT MenuName FROM USER_MENUTABLE where MenuName='AssetRetirement')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(

'AssetRetirement',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='AssetRetirement'),'/AssetRetirement/Index',
(Select MenuID from USER_MENUTABLE where MenuName='Transaction' ),8);
end
go
if not exists(select EntityCodeName from EntityCodeTable where EntityCodeName='AssetRetirement')
Begin 
Insert into EntityCodeTable (EntityCodeName,
CodePrefix,
CodeSuffix,
CodeFormatString,
LastUsedNo,
UseDateTime) 
Values('AssetRetirement','REF',NULL,'{0:00000}',0,0)
End 
Go 
 if not exists(select TransactionTypeName from TransactionTypeTable where TransactionTypeName='AssetRetirement')
  Begin
   insert into TransactionTypeTable(TransactionTypeID,TransactionTypeName,IsSourceTransactionType,TransactionTypeDesc)
   values(10,'AssetRetirement',0,'AssetRetirement')
  end 
  go 
  If not exists(select Status from StatusTable where Status='Disposed')
Begin
Insert into StatusTable(StatusID,Status)Values(103,'Disposed')
END
go

If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='AssetDisposalFinalApproval')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('AssetDisposalFinalApproval','AssetDisposalFinalApproval',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Approval'),1,0);
End
Go
If not exists(SELECT MenuName FROM USER_MENUTABLE where MenuName='AssetDisposalFinalApproval')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(

'AssetDisposal-FinalApproval',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='AssetDisposalFinalApproval'),'/AssetDisposalFinalApproval/Index',
(Select MenuID from USER_MENUTABLE where MenuName='Approval' ),8);
end
go


if not exists(select PostingStatus from PostingStatusTable where PostingStatus='CompletedByEndUser')
BEgin
  Insert into PostingStatusTable(PostingStatusID,PostingStatus) values(10,'CompletedByEndUser')
End 
Go 
if not exists(select PostingStatus from PostingStatusTable where PostingStatus='WaitingForFinalApproval')
BEgin
  Insert into PostingStatusTable(PostingStatusID,PostingStatus) values(11,'WaitingForFinalApproval')
End 
Go 
select * from MasterGridTable Order  by 1 desc 


If not exists (select Status from StatusTable where status='Draft')
Begin
   Insert into StatusTable (StatusID,Status) values('10','Draft')
end 
Go 
If exists(select Status from StatusTable where Status='Active') 
Begin
   update StatusTable set StatusID=50 where Status='Active'
End
go 

If exists(select Status from StatusTable where Status='InActive') 
Begin
   update StatusTable set StatusID=100 where Status='InActive'
End
go 

If exists(select Status from StatusTable where Status='WaitingForApproval') 
Begin
   update StatusTable set StatusID=150 where Status='WaitingForApproval'
End
go 

If exists(select Status from StatusTable where Status='Rejected') 
Begin
   update StatusTable set StatusID=200 where Status='Rejected'
End
go 
If exists(select Status from StatusTable where Status='Disposed') 
Begin
   update StatusTable set StatusID=250 where Status='Disposed'
End
go 
If exists(select Status from StatusTable where Status='Deleted') 
Begin
   update StatusTable set StatusID=500 where Status='Deleted'
End
go 

update user_menutable set MenuName='AssetRetirement-FinalApproval' where MenuName='AssetDisposal-FinalApproval'

Go 

If not exists(SELECT  masterGridName FROM MasterGridTable where masterGridName='ImportFormat')
Begin
Insert into MasterGridTable(masterGridName,EntityName)
Values('ImportFormat','ImportFormatTable');
End
Go
If not exists(SELECT  FieldName FROM MasterGridLineItemTable where masterGridID in (select masterGridID from MasterGridTable where masterGridName='ImportFormat') and FieldName='TamplateName')
Begin
Insert into MasterGridLineItemTable(MasterGridID,FieldName,DisplayName,Width,Format,IsDefault,OrderID)
Values((select masterGridID from MasterGridTable where masterGridName='ImportFormat'),'TamplateName','TamplateName',100,null,1,1);
End
Go

If not exists(SELECT  FieldName FROM MasterGridLineItemTable where masterGridID in (select masterGridID from MasterGridTable where masterGridName='ImportFormat') and FieldName='EntityName')
Begin
Insert into MasterGridLineItemTable(MasterGridID,FieldName,DisplayName,Width,Format,IsDefault,OrderID)
Values((select masterGridID from MasterGridTable where masterGridName='ImportFormat'),'EntityName','EntityName',100,null,1,2);
End
Go

If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='ImportFormat')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('ImportFormat','ImportFormat',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Template'),1,0);
End
Go
If not exists(SELECT MenuName FROM USER_MENUTABLE where MenuName='Template')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(

'Template',
NULL,'HandlerNull()',
NULL,5);
end
go
If not exists(SELECT MenuName FROM USER_MENUTABLE where MenuName='ImportFormat')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(

'ImportFormat',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='ImportFormat'),'/ImportFormat/Index',
(Select MenuID from USER_MENUTABLE where MenuName='Template' ),8);
end
go

If not exists(SELECT RIGHTGROUPNAME FROM USER_RIGHTGROUPTABLE WHERE RIGHTGROUPNAME='Others')
Begin
Insert into USER_RIGHTGROUPTABLE( PARENTRIGHTGROUPID, RIGHTGROUPNAME, RIGHTGROUPDESC)
Values(7,'Others','Others')
End
go

If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='ImportMaser')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('ImportMaser','ImportMaser',2,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Others'),1,0);
End
Go
If not exists(SELECT MenuName FROM USER_MENUTABLE where MenuName='Others')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(

'Others',
NULL,'HandlerNull()',
NULL,6);
end
go
If not exists(SELECT MenuName FROM USER_MENUTABLE where MenuName='ImportMaser')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(

'ImportMaser',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='ImportMaser'),'/ImportMaser/Import',
(Select MenuID from USER_MENUTABLE where MenuName='Others' ),8);
end
go

If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='EmailSignature')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('EmailSignature','EmailSignature',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Master'),1,0);
End
Go


If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='EmailSignature')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'EmailSignature',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='EmailSignature'),'/MasterPage/Index?pageName=EmailSignature',
(Select MenuID from USER_MENUTABLE where MenuName='Master' ),9);
End
Go


If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='ReportTemplateCategory')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('ReportTemplateCategory','ReportTemplateCategory',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Template'),1,0);
End
Go

If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='ReportTemplateCategory')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'ReportTemplateCategory',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='ReportTemplateCategory'),'/MasterPage/Index?pageName=ReportTemplateCategory',
(Select MenuID from USER_MENUTABLE where MenuName='Template' ),10);
End
Go
If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='ReportTemplate')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('ReportTemplate','ReportTemplate',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Template'),1,0);
End
Go
If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='Report')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('Report','Report',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Template'),1,0);
End
Go

If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='ReportTemplate')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'ReportTemplate',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='ReportTemplate'),'/ReportTemplate/Index',
(Select MenuID from USER_MENUTABLE where MenuName='Template' ),10);
End
Go
If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='User Report')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'User Report',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='User Report'),'/Report/Index',
(Select MenuID from USER_MENUTABLE where MenuName='Template' ),10);
End
Go
Insert into FieldTypeTable(
FieldType,
FieldTypeDesc,
ViewFileLocation,
ValueFieldName,
AllowedDataTypes,
ScreenControlQueryID)
values(1,'Integer',NULL,NULL,NULL,NULL)
go 
Insert into FieldTypeTable(
FieldType,
FieldTypeDesc,
ViewFileLocation,
ValueFieldName,
AllowedDataTypes,
ScreenControlQueryID)
values(2,'Float',NULL,NULL,NULL,NULL)
go 
Insert into FieldTypeTable(
FieldType,
FieldTypeDesc,
ViewFileLocation,
ValueFieldName,
AllowedDataTypes,
ScreenControlQueryID)
values(2,'String',NULL,NULL,NULL,NULL)

go 
If not exists(SELECT  masterGridName FROM MasterGridTable where masterGridName='ReportTemplate')
Begin
Insert into MasterGridTable(masterGridName,EntityName)
Values('ReportTemplate','ReportTemplateTable');
End
Go

If not exists(SELECT  FieldName FROM MasterGridLineItemTable where masterGridID 
in (select masterGridID from MasterGridTable where masterGridName='ReportTemplate') and FieldName='ReportTemplateName')
Begin
Insert into MasterGridLineItemTable(MasterGridID,FieldName,DisplayName,Width,Format,IsDefault,OrderID)
Values((select masterGridID from MasterGridTable where masterGridName='ReportTemplate'),'ReportTemplateName','ReportTemplateName',100,null,1,1);
End
Go

If not exists(SELECT  FieldName FROM MasterGridLineItemTable where masterGridID in 
(select masterGridID from MasterGridTable where masterGridName='ReportTemplate') and FieldName='QueryString')
Begin
Insert into MasterGridLineItemTable(MasterGridID,FieldName,DisplayName,Width,Format,IsDefault,OrderID)
Values((select masterGridID from MasterGridTable where masterGridName='ReportTemplate'),'QueryString','QueryString',100,null,1,2);
End
Go
If not exists(SELECT  FieldName FROM MasterGridLineItemTable where masterGridID in 
(select masterGridID from MasterGridTable where masterGridName='ReportTemplate') and FieldName='QueryType')
Begin
Insert into MasterGridLineItemTable(MasterGridID,FieldName,DisplayName,Width,Format,IsDefault,OrderID)
Values((select masterGridID from MasterGridTable where masterGridName='ReportTemplate'),'QueryType','QueryType',100,null,1,3);
End
Go
If not exists(SELECT  FieldName FROM MasterGridLineItemTable where masterGridID in 
(select masterGridID from MasterGridTable where masterGridName='ReportTemplate') and FieldName='ReportTemplateCategory.ReportTemplateCategoryName')
Begin
Insert into MasterGridLineItemTable(MasterGridID,FieldName,DisplayName,Width,Format,IsDefault,OrderID)
Values((select masterGridID from MasterGridTable where masterGridName='ReportTemplate'),'ReportTemplateCategory.ReportTemplateCategoryName','TemplateCategory',100,null,1,4);
End
Go
If not exists(SELECT  FieldName FROM MasterGridLineItemTable where masterGridID in 
(select masterGridID from MasterGridTable where masterGridName='ReportTemplate') and FieldName='ReportTemplateFile')
Begin
Insert into MasterGridLineItemTable(MasterGridID,FieldName,DisplayName,Width,Format,IsDefault,OrderID)
Values((select masterGridID from MasterGridTable where masterGridName='ReportTemplate'),'ReportTemplateFile','ReportTemplateFile',100,null,1,5);
End
Go
If not exists(SELECT  FieldName FROM MasterGridLineItemTable where masterGridID in 
(select masterGridID from MasterGridTable where masterGridName='ReportTemplate') and FieldName='Status.Status')
Begin
Insert into MasterGridLineItemTable(MasterGridID,FieldName,DisplayName,Width,Format,IsDefault,OrderID)
Values((select masterGridID from MasterGridTable where masterGridName='ReportTemplate'),'Status.Status','Status',100,null,1,6);
End
Go
If not exists(SELECT  FieldName FROM MasterGridLineItemTable where masterGridID in 
(select masterGridID from MasterGridTable where masterGridName='ReportTemplate') and FieldName='CreatedDateTime')
Begin
Insert into MasterGridLineItemTable(MasterGridID,FieldName,DisplayName,Width,Format,IsDefault,OrderID)
Values((select masterGridID from MasterGridTable where masterGridName='ReportTemplate'),'CreatedDateTime','CreatedDateTime',100,'dd/MM/yyyy',1,7);
End
Go
If not exists(SELECT  masterGridName FROM MasterGridTable where masterGridName='Report')
Begin
Insert into MasterGridTable(masterGridName,EntityName)
Values('Report','Report');
End
Go
select * from MasterGridLineItemTable Order by 1 desc 
--update MasterGridLineItemTable set FieldName='ReportName' where MasterGridLineItemID=4974
If not exists(SELECT  FieldName FROM MasterGridLineItemTable where masterGridID in 
(select masterGridID from MasterGridTable where masterGridName='Report') and FieldName='ReportName')
Begin
Insert into MasterGridLineItemTable(MasterGridID,FieldName,DisplayName,Width,Format,IsDefault,OrderID)
Values((select masterGridID from MasterGridTable where masterGridName='Report'),'ReportName','ReportName',100,null,1,1);
End
Go

If not exists(SELECT  FieldName FROM MasterGridLineItemTable where masterGridID in 
(select masterGridID from MasterGridTable where masterGridName='Report') and FieldName='ReportTitle')
Begin
Insert into MasterGridLineItemTable(MasterGridID,FieldName,DisplayName,Width,Format,IsDefault,OrderID)
Values((select masterGridID from MasterGridTable where masterGridName='Report'),'ReportTitle','ReportTitle',100,null,1,2);
End
Go
If not exists(SELECT  FieldName FROM MasterGridLineItemTable where masterGridID in 
(select masterGridID from MasterGridTable where masterGridName='Report') and FieldName='ReportTemplate.ReportTemplateName')
Begin
Insert into MasterGridLineItemTable(MasterGridID,FieldName,DisplayName,Width,Format,IsDefault,OrderID)
Values((select masterGridID from MasterGridTable where masterGridName='Report'),'ReportTemplate.ReportTemplateName','TemplateName',100,null,1,3);
End
Go
If not exists(SELECT  FieldName FROM MasterGridLineItemTable where masterGridID in 
(select masterGridID from MasterGridTable where masterGridName='Report') and FieldName='CreatedDateTime')
Begin
Insert into MasterGridLineItemTable(MasterGridID,FieldName,DisplayName,Width,Format,IsDefault,OrderID)
Values((select masterGridID from MasterGridTable where masterGridName='Report'),'CreatedDateTime','CreatedDateTime',100,'dd/MM/yyyy',1,4);
End
Go


IF OBJECT_ID('TransferTypeTable') IS NULL
BEGIN
  Create table TransferTypeTable
  (
    TransferTypeID int not null primary key identity(1,1),
	TransferTypeCode nvarchar(max) Not NULL,
	TransferTypeName nvarchar(max) Not null,
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

IF OBJECT_ID('TransferTypeDescriptionTable') IS NULL
BEGIN
  Create table TransferTypeDescriptionTable
  (
     TransferTypeDescriptionID int not null primary key identity(1,1),
	 TransferTypeID int not null foreign key references TransferTypeTable(TransferTypeID),
	 TransferTypeDescription nvarchar(max) not null, 
	 LanguageID int not null foreign key references LanguageTable(LanguageID),
	 CreatedBy int not null foreign key references User_LoginUserTable(UserID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references User_LoginUserTable(UserID),
	LastModifiedDateTime SmallDateTime NULL
  )
End 
Go 

If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='TransferType')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('TransferType','TransferType',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Master'),1,0);
End
Go


If not exists(SELECT  MenuName FROM USER_MENUTABLE where MenuName='TransferType')
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO) Values(
'TransferType',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='TransferType'),'/MasterPage/Index?pageName=TransferType',
(Select MenuID from USER_MENUTABLE where MenuName='Master' ),9);
End
Go

Alter table TransactionLineItemTable Add TransferTypeID int NULL Foreign key references TransferTypeTable(TransferTypeID)
Go 
Alter table PersonTable Add DesignationID int NULL Foreign key references DesignationTable(DesignationID)
Go 
Alter table PersonTable Add SignaturePath nvarchar(max) NULL 
Go 
Alter table PersonTable Add StampPath nvarchar(max) NULL 
Go 

ALTER TABLE PersonTable
ADD FOREIGN KEY (DepartmentID) REFERENCES DepartmentTable (DepartmentID)
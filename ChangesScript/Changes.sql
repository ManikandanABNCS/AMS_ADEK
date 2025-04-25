If not exists(select FieldTypeDesc from AFieldTypeTable where FieldTypeDesc='Integer')
Begin 
Insert into AFieldTypeTable(FieldTypeCode,FieldTypeDesc,ViewFileLocation,ValueFieldName,SelectionControlQueryID)
values('1','Integer',NULL,NULL,NULL)

End 
Go
If not exists(select FieldTypeDesc from AFieldTypeTable where FieldTypeDesc='Float')
Begin 
Insert into AFieldTypeTable(FieldTypeCode,FieldTypeDesc,ViewFileLocation,ValueFieldName,SelectionControlQueryID)
values('2','Float',NULL,NULL,NULL)

End 
Go

If not exists(select FieldTypeDesc from AFieldTypeTable where FieldTypeDesc='String')
Begin 
Insert into AFieldTypeTable(FieldTypeCode,FieldTypeDesc,ViewFileLocation,ValueFieldName,SelectionControlQueryID)
values('3','String',NULL,NULL,NULL)

End 
Go
If not Exists(select ControlName from ASelectionControlQueryTable where ControlName='Barcode') 
Begin 
  Insert into ASelectionControlQueryTable(ControlName,ControlType,Query,DisplayFields,
SearchFields,OrderByQuery,SelectedItemDisplayField,ValueFieldName)
Values('Barcode','MultiColumnComboBox','select AssetID,AssetCode,Barcode,Barcode as DisplayValue From AssetTable where StatusID not in (500)','Barcode','Barcode',NULL,'DisplayValue','Barcode')
End
go

If not exists(select FieldTypeDesc from AFieldTypeTable where FieldTypeDesc='Barcode')
Begin
Insert into AFieldTypeTable(FieldTypeCode,FieldTypeDesc,ViewFileLocation,ValueFieldName,SelectionControlQueryID)
Select '1','Barcode','/Views/Shared/MasterViews/DynamicFilterControl','AssetID',SelectionControlQueryID
From ASelectionControlQueryTable where ControlName='Barcode'
End 
Go
If not Exists(select ControlName from ASelectionControlQueryTable where ControlName='Department') 
Begin 
  Insert into ASelectionControlQueryTable(ControlName,ControlType,Query,DisplayFields,
SearchFields,OrderByQuery,SelectedItemDisplayField,ValueFieldName)
Values('Department','MultiColumnComboBox','select DepartmentID,DepartmentCode,DepartmentName,DepartmentName as DisplayValue From departmenttable where StatusID not in (500)','DepartmentCode,DepartmentName','DepartmentCode,DepartmentName',NULL,'DisplayValue','DepartmentID')
End
go
If not exists(select FieldTypeDesc from AFieldTypeTable where FieldTypeDesc='Department')
Begin
Insert into AFieldTypeTable(FieldTypeCode,FieldTypeDesc,ViewFileLocation,ValueFieldName,SelectionControlQueryID)
Select '1','Department','/Views/Shared/MasterViews/DynamicFilterControl','DepartmentID',SelectionControlQueryID
From ASelectionControlQueryTable where ControlName='Department'
End 
Go
If not Exists(select ControlName from ASelectionControlQueryTable where ControlName='Location') 
Begin 
  Insert into ASelectionControlQueryTable(ControlName,ControlType,Query,DisplayFields,
SearchFields,OrderByQuery,SelectedItemDisplayField,ValueFieldName)
Values('Location','MultiColumnComboBox','select ChildID as LocationID,LocatioName,PID4 as LocationHierarchical,PID4 as DisplayValue From LocationHierarchicalView where StatusID not in (500)','LocatioName,PID4','LocatioName,PID4',NULL,'DisplayValue','LocationID')
End
go
If not exists(select FieldTypeDesc from AFieldTypeTable where FieldTypeDesc='Location')
Begin
Insert into AFieldTypeTable(FieldTypeCode,FieldTypeDesc,ViewFileLocation,ValueFieldName,SelectionControlQueryID)
Select '1','Location','/Views/Shared/MasterViews/DynamicFilterControl','LocationID',SelectionControlQueryID
From ASelectionControlQueryTable where ControlName='Location'
End 
Go
update AFieldTypeTable set FieldTypeCode=FieldTypeID
go 
alter table ScreenFilterLineItemTable drop constraint FK_ScreenFilterLineItemTable_FieldTypeID
Go 
Alter table ScreenFilterLineitemTable add Constraint FK_ScreenFilterLineItemTable_FieldTypeID foreign key (FieldTypeID) references AFieldTypeTable(FieldTypeID)
go 
drop table FieldTypeTable
go

--select * from ScreenControlQueryTable
--REFERENCES AMS_Client.dbo.AFieldTypeTable (FieldTypeID)

--select * from ReportTemplateFieldTable where reporttemplateid=1
--ReportTemplateTable
--exec tmp_CleanTable 'ReportFieldTable'
--exec tmp_CleanTable 'ReportGroupFieldTable'
--exec tmp_CleanTable 'ReportTemplateFieldTable'
--exec tmp_CleanTable 'ScreenFilterLineItemTable'
--exec tmp_CleanTable 'ScreenFilterTable'
--exec tmp_CleanTable 'ReportTable'
--exec tmp_CleanTable 'ReportTemplateTable'

--select * from ASelectionControlQueryTable
--select * from AFieldTypeTable

update AFieldTypeTable set ValueFieldName='Barcode' where FieldTypeDesc='Barcode'
go

ALTER VIEW [dbo].[UserRightValueView]
AS
SELECT  P.PersonID UserID, P.PersonCode, P.PersonFirstName, P.PersonLastName, P.EMailID, P.MobileNo, 
			P.WhatsAppMobileNo ,x.RightValue, x.RightName,x.ApprovalRoleID,x.LocationID
			from (
			Select 
		isnull(DD.DelegatedEmployeeID,RU.UserID) as UserID,RR.RightValue, A.RightName,UA.ApprovalRoleID,UA.LocationID
	FROM User_RightTable A
	LEFT JOIN User_RoleRightTable RR ON RR.RightID = A.RightID
	LEFT JOIN User_UserRoleTable RU ON RU.RoleID = RR.RoleID
	--LEFT JOIN  PersonTable P ON P.PersonID = RU.UserID
	
	LEFT JOIN UserApprovalRoleMappingTable UA ON RU.UserID=UA.UserID AND UA.StatusID=50
	--LEFT JOIN DepartmentTable DP ON DP.DepartmentID = P.DepartmentID
	Left join (select * from DelegateRoleTable where  getdate() between EffectiveStartDate and EffectiveEndDate) DD on DD.EmployeeID=RU.UserID
	WHERE RR.RightValue > 0
	) x join PersonTable p on x.userID=p.PersonID
		AND x.userID IS NOT NULL
		--AND A.RightName = 'ApproveUniformRequest'
UNION
SELECT  P.PersonID UserID, P.PersonCode, P.PersonFirstName, P.PersonLastName, P.EMailID, P.MobileNo, 
			P.WhatsAppMobileNo,x.RightValue, x.RightName,x.ApprovalRoleID,x.LocationID from(
			
			Select 
		isnull(DD.DelegatedEmployeeID,UR.UserID) as UserID,UR.RightValue, A.RightName,UA.ApprovalRoleID,UA.LocationID
	FROM User_RightTable A
	LEFT JOIN User_UserRightTable UR ON UR.RightID = A.RightID
	--LEFT JOIN PersonTable P ON P.PersonID = UR.UserID
	LEFT JOIN UserApprovalRoleMappingTable UA ON UR.UserID=UA.UserID AND UA.StatusID=50
	Left join (select * from DelegateRoleTable where  getdate() between EffectiveStartDate and EffectiveEndDate) DD on DD.EmployeeID=UR.UserID
	--LEFT JOIN DepartmentTable DP ON DP.DepartmentID = P.DepartmentID
	WHERE UR.RightValue > 0
	)x join PersonTable p on x.userID=p.PersonID
		AND x.userID IS NOT NULL
		--AND A.RightName = 'ApproveUniformRequest'

GO



ALTER view [dbo].[UserApprovalRoleMappingView] 
	as 
	select x.*,p1.PersonFirstName+'-'+p1.PersonLastName  as personName from (
	select isnull(DD.DelegatedEmployeeID,a.UserID) as UserID,a.ApprovalRoleID--,p1.PersonFirstName+'-'+p1.PersonLastName  as personName
	from UserApprovalRoleMappingTable a 
	--join PersonTable p1 on a.UserID=p1.PersonID 
	Left join (select * from DelegateRoleTable where  getdate() between EffectiveStartDate and EffectiveEndDate) DD on DD.EmployeeID=a.UserID
	where a.StatusID=50 ) x join PersonTable p1 on x.UserID=p1.PersonID 
	group by x.UserID,x.ApprovalRoleID,p1.PersonFirstName,p1.PersonLastName 
GO


update ASelectionControlQueryTable set Query='select ChildID as LocationID,LocationName,PID4 as LocationHierarchical,PID4 as DisplayValue From LocationHierarchicalView where StatusID not in (500)'
,DisplayFields='LocationName,LocationHierarchical',SearchFields='LocationName,LocationHierarchical' where ControlName='Location'
go 
If not Exists(select ControlName from ASelectionControlQueryTable where ControlName='Category') 
Begin 
  Insert into ASelectionControlQueryTable(ControlName,ControlType,Query,DisplayFields,
SearchFields,OrderByQuery,SelectedItemDisplayField,ValueFieldName)
Values('Category','MultiColumnComboBox','select ChildID as CategoryID,CategoryName,CategoryNameHierarchy as CategoryHierarchical,CategoryNameHierarchy as DisplayValue From CategoryHierarchicalView where StatusID not in (500)',
'CategoryName,CategoryHierarchical','CategoryName,CategoryHierarchical',NULL,'DisplayValue','CategoryID')
End
go
If not exists(select FieldTypeDesc from AFieldTypeTable where FieldTypeDesc='Category')
Begin
Insert into AFieldTypeTable(FieldTypeCode,FieldTypeDesc,ViewFileLocation,ValueFieldName,SelectionControlQueryID)
Select (select max(SelectionControlQueryID)+1 from ASelectionControlQueryTable),'Category','/Views/Shared/MasterViews/DynamicFilterControl','CategoryID',SelectionControlQueryID
From ASelectionControlQueryTable where ControlName='Category'
End 
Go
update ASelectionControlQueryTable set Query='select ChildID as CategoryID,CategoryName,CategoryNameHierarchy as CategoryHierarchical,CategoryNameHierarchy as DisplayValue From CategoryHierarchicalView where StatusID not in (500)',
DisplayFields='CategoryName,CategoryHierarchical',SearchFields='CategoryName,CategoryHierarchical',ValueFieldName='CategoryID' where ControlName='Category'
go 


If not Exists(select ControlName from ASelectionControlQueryTable where ControlName='AssetCondition') 
Begin 
  Insert into ASelectionControlQueryTable(ControlName,ControlType,Query,DisplayFields,
SearchFields,OrderByQuery,SelectedItemDisplayField,ValueFieldName)
Values('AssetCondition','MultiColumnComboBox','select AssetConditionID,AssetConditionCode,AssetConditionName,AssetConditionName as DisplayValue From AssetConditionTable where StatusID not in (500)',
'AssetConditionCode,AssetConditionName','AssetConditionCode,AssetConditionName',NULL,'DisplayValue','AssetConditionID')
End
go
If not exists(select FieldTypeDesc from AFieldTypeTable where FieldTypeDesc='AssetCondition')
Begin
Insert into AFieldTypeTable(FieldTypeCode,FieldTypeDesc,ViewFileLocation,ValueFieldName,SelectionControlQueryID)
Select (select max(SelectionControlQueryID)+1 from ASelectionControlQueryTable),'AssetCondition','/Views/Shared/MasterViews/DynamicFilterControl','AssetConditionID',SelectionControlQueryID
From ASelectionControlQueryTable where ControlName='AssetCondition'
End 
Go

If not exists(select Partytype from PartyTypeTable where PartyType='Supplier')
Begin 
  insert into PartyTypeTable(PartyTypeCode,PartyType,PartyTypeID)
  Values('Supplier','Supplier',10)
End
go 

If not exists(select Partytype from PartyTypeTable where PartyType='Manufacturer')
Begin 
  insert into PartyTypeTable(PartyTypeCode,PartyType,PartyTypeID)
  Values('Manufacturer','Manufacturer',20)
End
go 
If not Exists(select ControlName from ASelectionControlQueryTable where ControlName='Supplier') 
Begin 
  Insert into ASelectionControlQueryTable(ControlName,ControlType,Query,DisplayFields,
SearchFields,OrderByQuery,SelectedItemDisplayField,ValueFieldName)
Values('Supplier','MultiColumnComboBox','select PartyID as SupplierID,PartyCode as SupplierCode,PartyName as SupplierName,PartyName as DisplayValue From PartyTable  where StatusID not in (500) and PartyTypeID=10',
'SupplierCode,SupplierName','SupplierCode,SupplierName',NULL,'DisplayValue','SupplierID')
End
go
If not exists(select FieldTypeDesc from AFieldTypeTable where FieldTypeDesc='Supplier')
Begin
Insert into AFieldTypeTable(FieldTypeCode,FieldTypeDesc,ViewFileLocation,ValueFieldName,SelectionControlQueryID)
Select (select max(SelectionControlQueryID)+1 from ASelectionControlQueryTable),'Supplier','/Views/Shared/MasterViews/DynamicFilterControl','SupplierID',SelectionControlQueryID
From ASelectionControlQueryTable where ControlName='Supplier'
End 
Go

If not Exists(select ControlName from ASelectionControlQueryTable where ControlName='Manufacturer') 
Begin 
  Insert into ASelectionControlQueryTable(ControlName,ControlType,Query,DisplayFields,
SearchFields,OrderByQuery,SelectedItemDisplayField,ValueFieldName)
Values('Manufacturer','MultiColumnComboBox','select PartyID as ManufacturerID,PartyCode as ManufacturerCode,PartyName as ManufacturerName,PartyName as DisplayValue From PartyTable  where StatusID not in (500) and PartyTypeID=20',
'ManufacturerCode,ManufacturerName','ManufacturerCode,ManufacturerName',NULL,'DisplayValue','ManufacturerID')
End
go
If not exists(select FieldTypeDesc from AFieldTypeTable where FieldTypeDesc='Manufacturer')
Begin
Insert into AFieldTypeTable(FieldTypeCode,FieldTypeDesc,ViewFileLocation,ValueFieldName,SelectionControlQueryID)
Select (select max(SelectionControlQueryID)+1 from ASelectionControlQueryTable),'Manufacturer','/Views/Shared/MasterViews/DynamicFilterControl','ManufacturerID',SelectionControlQueryID
From ASelectionControlQueryTable where ControlName='Manufacturer'
End 
Go
If not Exists(select ControlName from ASelectionControlQueryTable where ControlName='Model') 
Begin 
  Insert into ASelectionControlQueryTable(ControlName,ControlType,Query,DisplayFields,
SearchFields,OrderByQuery,SelectedItemDisplayField,ValueFieldName)
Values('Model','MultiColumnComboBox','select ModelID,ModelCode,ModelName,ModelName as DisplayValue From ModelTable where StatusID not in (500)',
'ModelCode,ModelName','ModelCode,ModelName',NULL,'DisplayValue','ModelID')
End
go
If not exists(select FieldTypeDesc from AFieldTypeTable where FieldTypeDesc='Model')
Begin
Insert into AFieldTypeTable(FieldTypeCode,FieldTypeDesc,ViewFileLocation,ValueFieldName,SelectionControlQueryID)
Select (select max(SelectionControlQueryID)+1 from ASelectionControlQueryTable),'Model','/Views/Shared/MasterViews/DynamicFilterControl','ModelID',SelectionControlQueryID
From ASelectionControlQueryTable where ControlName='Model'
End 
Go
If not Exists(select ControlName from ASelectionControlQueryTable where ControlName='Custodian') 
Begin 
  Insert into ASelectionControlQueryTable(ControlName,ControlType,Query,DisplayFields,
SearchFields,OrderByQuery,SelectedItemDisplayField,ValueFieldName)
Values('Custodian','MultiColumnComboBox','select PersonID as CustodianID,PersonCode as CustodianCode,PersonFirstName as CustodianName,PersonFirstName as DisplayValue From PersonTable where StatusID not in (500)',
'PersonCode,CustodianName','PersonCode,CustodianName',NULL,'DisplayValue','CustodianID')
End
go
If not exists(select FieldTypeDesc from AFieldTypeTable where FieldTypeDesc='Custodian')
Begin
Insert into AFieldTypeTable(FieldTypeCode,FieldTypeDesc,ViewFileLocation,ValueFieldName,SelectionControlQueryID)
Select (select max(SelectionControlQueryID)+1 from ASelectionControlQueryTable),'Custodian','/Views/Shared/MasterViews/DynamicFilterControl','CustodianID',SelectionControlQueryID
From ASelectionControlQueryTable where ControlName='Custodian'
End 
Go


If not Exists(select ControlName from ASelectionControlQueryTable where ControlName='Section') 
Begin 
  Insert into ASelectionControlQueryTable(ControlName,ControlType,Query,DisplayFields,
SearchFields,OrderByQuery,SelectedItemDisplayField,ValueFieldName)
Values('Section','MultiColumnComboBox','select SectionID,SectionCode,SectionName,SectionName as DisplayValue From AssetConditionTable where StatusID not in (500)',
'SectionCode,SectionName','SectionCode,SectionName',NULL,'DisplayValue','SectionID')
End
go
If not exists(select FieldTypeDesc from AFieldTypeTable where FieldTypeDesc='Section')
Begin
Insert into AFieldTypeTable(FieldTypeCode,FieldTypeDesc,ViewFileLocation,ValueFieldName,SelectionControlQueryID)
Select (select max(SelectionControlQueryID)+1 from ASelectionControlQueryTable),'Section','/Views/Shared/MasterViews/DynamicFilterControl','SectionID',SelectionControlQueryID
From ASelectionControlQueryTable where ControlName='Section'
End 
Go

select* from ASelectionControlQueryTable

Select * from PartyTable
select * from AssetTable
select * from PartyTypeTable
select * from ASelectionControlQueryTable

select AssetConditionID,AssetConditionCode,AssetConditionName,AssetConditionName as DisplayValue From AssetConditionTable where StatusID not in (500)
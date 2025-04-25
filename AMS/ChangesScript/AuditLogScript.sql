Update User_MenuTable set Image='css/images/MenuIcon/AssetMaintainanceRequestApproval.png' where MenuName='AssetMaintenanceRequestApproval' and ParentTransactionID=1
go
Update User_MenuTable set Image='css/images/MenuIcon/AmcScheduleApproval.png' where MenuName='AMCScheduleApproval' and ParentTransactionID=1
go
Update User_MenuTable set Image='css/images/MenuIcon/DepreciationPeriod.png' where MenuName='Period' and ParentTransactionID=1
go
update EntityActionTable set ActionQuery='select AT.* from (Select D.DepartmentCode as Code,D.DepartmentName as Name ,''Department Code'' as CodeTitle ,''Department Name'' as NameTitle,   
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType        
when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''         
end as ActionType,P.PersonFirstName+'' ''+P.PersonLastName as UserName,''Department Modification'' AS ActionName,   
Case When ATL.FieldName = ''Status ID'' Then ''Status''    Else ATL.FieldName End as FieldName,     
Case  When ATL.FieldName = ''Status ID'' Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.OldValue),''-'')    
Else ISNULL(ATL.OldValue,''-'') End as OldValue,   Case When ATL.FieldName = ''Status ID''      Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.NewValue),''-'')     
Else ISNULL(ATL.NewValue,''-'') End as NewValue,   P.PersonID as UserID ,''All Company'' as CompanyName,1 as LanguageID, 0 as CompanyID   
From DepartmentTable D       
  join AuditLogTable AL on D.DepartmentID=convert(int,AL.AuditedObjectKeyValue1)       
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID      
INNER JOIN AuditLogLineItemTable ATL ON AL.AuditLogID=ATL.AuditLogID       
join PersonTable P on P.PersonID=AT.UserID    
where AL.EntityName=''DepartmentTable'' and AL.ActionType=2    
) At where 1=1' where ActionName='DepartmentModification'

go
update EntityActionTable set ActionQuery='select AT.* from (Select D.DepartmentCode as Code,D.DepartmentName as Name ,''Department Code'' as CodeTitle ,''Department Name'' as NameTitle,   
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType     when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''     end as ActionType,
P.PersonFirstName+'' ''+P.PersonLastName as UserName,''Department Deletion'' AS ActionName,''-'' AS FieldName,''-'' AS OldValue,''-'' AS NewValue,P.PersonID as UserID,
''All Company'' as CompanyName,1 as LanguageID, 0 as CompanyID  From DepartmentTable D    
join AuditLogTable AL on D.DepartmentID=convert(int,AL.AuditedObjectKeyValue1)  
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID   
join PersonTable P on P.PersonID=AT.UserID   where AL.EntityName=''DepartmentTable'' and AL.ActionType=3)AT where 1=1' 
where ActionName='DepartmentDeletion'
go 
update EntityActionTable set ActionQuery='select AT.* from (Select D.DepartmentCode as Code,D.DepartmentName as Name ,''Department Code'' as CodeTitle ,''Department Name'' as NameTitle,    
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType     when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''     end as ActionType,
P.PersonFirstName+'' ''+P.PersonLastName as UserName,''Department Creation'' AS ActionName,''-'' AS FieldName,''-'' AS OldValue,''-'' AS NewValue,P.PersonID as UserID   , 
''All Company'' as CompanyName,1 as LanguageID,0 as CompanyID   From DepartmentTable D   
join AuditLogTable AL on D.DepartmentID=convert(int,AL.AuditedObjectKeyValue1)   
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID    
join PersonTable P on P.PersonID=AT.UserID   where AL.EntityName=''DepartmentTable'' and AL.ActionType=1)AT where 1=1' 
where ActionName='DepartmentCreation'
go 
update EntityActionTable set ActionQuery='Select AT.* from (Select D.SectionCode as Code,D.SectionName as Name ,''Section Code'' as CodeTitle ,''Section Name'' as NameTitle, 
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType     when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''  end as ActionType, 
P.PersonFirstName+'' ''+P.PersonLastName as UserName,''Section Creation'' AS ActionName,''-'' AS FieldName,''-'' AS OldValue,''-'' AS NewValue,P.PersonID as UserID   
,  ''All Company'' as CompanyName,1 as LanguageID,0 as CompanyID  From SectionTable D     
join AuditLogTable AL on D.sectionID=convert(int,AL.AuditedObjectKeyValue1)    
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID    
join PersonTable P on P.PersonID=AT.UserID   where AL.EntityName=''SectionTable'' and AL.ActionType=1)AT where 1=1 ' 
where ActionName='SectionCreation'
go 
update EntityActionTable set ActionQuery='Select AT.* from (Select D.SectionCode as Code,D.SectionName as Name ,''Section Code'' as CodeTitle ,''Section Name'' as NameTitle,
AL.EntityName,AT.AuditLogTransactionDateTime,  AT.AuditLogTransactionID,Case AL.ActionType  when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''  end as ActionType,  
P.PersonFirstName+'' ''+P.PersonLastName as UserName,''Section Creation'' AS ActionName,''-'' AS FieldName,''-'' AS OldValue,''-'' AS NewValue,P.PersonID as UserID ,
''All Company'' as CompanyName,1 as LanguageID ,0 as CompanyID  From SectionTable D     
join AuditLogTable AL on D.sectionID=convert(int,AL.AuditedObjectKeyValue1)    
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID    
join PersonTable P on P.PersonID=AT.UserID   where AL.EntityName=''SectionTable'' and AL.ActionType=3)AT where 1=1' 
where ActionName='SectionDeletion'
go 
update EntityActionTable set ActionQuery='Select AT.* From (Select D.SectionCode as Code,D.SectionName as Name ,''Section Code'' as CodeTitle, ''Section Name'' as NameTitle,    
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''  end as ActionType, 
P.PersonFirstName+'' ''+P.PersonLastName as UserName,''Section Modification'' AS ActionName,   Case When ATL.FieldName = ''Status ID'' Then ''Status'' when ATL.FieldName = ''Department ID'' Then ''Department Name''     Else ATL.FieldName End as FieldName,  
Case  When ATL.FieldName = ''Status ID'' Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.OldValue),''-'')    
when  ATL.FieldName = ''Department ID'' Then 
ISNULL((select MT.DepartmentName from DepartmentTable MT  where MT.DepartmentID=ATL.OldValue),''-'')     Else ISNULL(ATL.OldValue,''-'') End as OldValue,   
Case When ATL.FieldName = ''Status ID'' Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.NewValue),''-'')  
when  ATL.FieldName = ''Department ID'' Then ISNULL((select MT.DepartmentName from DepartmentTable MT  where MT.DepartmentID=ATL.NewValue),''-'')     Else ISNULL(ATL.NewValue,''-'') End as NewValue,  
P.PersonID as UserID,''All Company'' as CompanyName,1 as LanguageID,0 as CompanyID   
From SectionTable D   
join AuditLogTable AL on D.sectionID=convert(int,AL.AuditedObjectKeyValue1)    
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID     
INNER JOIN AuditLogLineItemTable ATL ON AL.AuditLogID=ATL.AuditLogID     join PersonTable P on P.PersonID=AT.UserID 
where AL.EntityName=''SectionTable'' and AL.ActionType=2 
) AT where 1=1' 
where ActionName='SectionModification'
go 
update EntityActionTable set ActionQuery='Select AT.* from (Select D.AssetConditionCode as Code,D.AssetConditionName as Name ,''Asset Condition Code'' as CodeTitle ,''Asset Condition'' as NameTitle, 
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType     when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE '''' end as ActionType,
P.PersonFirstName+'' ''+P.PersonLastName as UserName,''Asset Condition Creation'' AS ActionName,''-'' AS FieldName,''-'' AS OldValue,''-'' AS NewValue,P.PersonID as UserID ,  
''All Company'' as CompanyName,1 as LanguageID,0 as CompanyID  From AssetConditionTable D   
 join AuditLogTable AL on D.AssetConditionID=convert(int,AL.AuditedObjectKeyValue1)    
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID  
join PersonTable P on P.PersonID=AT.UserID     where AL.EntityName=''AssetConditionTable'' and AL.ActionType=1)AT where 1=1' 
where ActionName='AssetConditionCreation'
go 
update EntityActionTable set ActionQuery='select AT.* from (Select D.AssetConditionCode as Code,d.AssetConditionName as Name ,''Asset Condition Code'' as CodeTitle ,
''Asset Condition'' as NameTitle,AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,
Case AL.ActionType     when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE '''' end as ActionType,
P.PersonFirstName+'' ''+P.PersonLastName as UserName,''Asset Condition Deletion'' AS ActionName,''-'' AS FieldName,''-'' AS OldValue,''-'' AS NewValue,
P.PersonID as UserID   ,  ''All Company'' as CompanyName,1 as LanguageID,0 as CompanyID  
From AssetConditionTable D      
join AuditLogTable AL on D.AssetConditionID=convert(int,AL.AuditedObjectKeyValue1)    
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID    
join PersonTable P on P.PersonID=AT.UserID    
where AL.EntityName=''AssetConditionTable'' and AL.ActionType=3)At where 1=1' 
where ActionName='AssetConditionDeletion'
go 
update EntityActionTable set ActionQuery='Select AT.* from (Select D.AssetConditionCode as Code,d.AssetConditionName as Name ,''Asset Condition Code'' as CodeTitle ,''Asset Condition'' as NameTitle,    
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''  end as ActionType,
P.PersonFirstName+'' ''+P.PersonLastName as UserName,''Asset Condition Modification'' AS ActionName,     Case When ATL.FieldName = ''Status ID'' Then ''Status''    Else ATL.FieldName End as FieldName,     
Case  When ATL.FieldName = ''Status ID'' Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.OldValue),''-'')     Else ISNULL(ATL.OldValue,''-'') End as OldValue,  
Case When ATL.FieldName = ''Status ID''   Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.NewValue),''-'')      Else ISNULL(ATL.NewValue,''-'') End as NewValue,  
P.PersonID as UserID ,''All Company'' as CompanyName,1 as LanguageId,0 as CompanyID  From AssetConditionTable D    
join AuditLogTable AL on D.AssetConditionID=convert(int,AL.AuditedObjectKeyValue1)   
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID  
INNER JOIN AuditLogLineItemTable ATL ON AL.AuditLogID=ATL.AuditLogID    
join PersonTable P on P.PersonID=AT.UserID    where AL.EntityName=''AssetConditionTable'' and AL.ActionType=2  
)At where 1=1' 
where ActionName='AssetConditionModification'
go 
update EntityActionTable set ActionQuery='Select AT.* from (Select D.TransferTypeCode as Code,d.TransferTypeName as Name ,''Transfer Type Code'' as CodeTitle ,''Transfer Type'' as NameTitle,
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType     when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''     end as ActionType,
P.PersonFirstName+'' ''+P.PersonLastName as UserName,''Transfer Type Creation'' AS ActionName,''-'' AS FieldName,''-'' AS OldValue,''-'' AS NewValue,
P.PersonID as UserID,  ''All Company'' as CompanyName,1 as LanguageID,0 as CompanyID  From TransferTypeTable D   
   join AuditLogTable AL on D.TransferTypeID=convert(int,AL.AuditedObjectKeyValue1)  
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID   
join PersonTable P on P.PersonID=AT.UserID     
where AL.EntityName=''TransferTypeTable'' and AL.ActionType=1)AT where 1=1' 
where ActionName='TransferTypeCreation'
go 
update EntityActionTable set ActionQuery='Select AT.* from (Select D.TransferTypeCode as Code,D.TransferTypeName as Name ,''Transfer Type Code'' as CodeTitle ,''Transfer Type'' as NameTitle,    
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType   when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE '''' end as ActionType,
P.PersonFirstName+'' ''+P.PersonLastName as UserName,''Transfer Type Deletion'' AS ActionName,''-'' AS FieldName,''-'' AS OldValue,''-'' AS NewValue,P.PersonID as UserID  , 
''All Company'' as CompanyName,1 as LanguageID,0 as CompanyID 
From TransferTypeTable D   
join AuditLogTable AL on D.TransferTypeID=convert(int,AL.AuditedObjectKeyValue1)   
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID    
join PersonTable P on P.PersonID=AT.UserID     where AL.EntityName=''TransferTypeTable'' and AL.ActionType=3) AT where 1=1' 
where ActionName='TransferTypeDeletion'
go 
update EntityActionTable set ActionQuery='Select AT.* from (Select D.LocationCode as Code,D.LocationName as Name ,''Location Code'' as CodeTitle ,''Location Name'' as NameTitle,    
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''  end as ActionType,
P.PersonFirstName+'' ''+P.PersonLastName as UserName,''Location Creation'' AS ActionName,''-'' AS FieldName,''-'' AS OldValue,''-'' AS NewValue,P.PersonID as UserID  , 
''All Company'' as CompanyName,1 as LanguageID, 0 as CompanyID  From LocationTable D    
join AuditLogTable AL on D.LocationID=convert(int,AL.AuditedObjectKeyValue1)   
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID    
join PersonTable P on P.PersonID=AT.UserID     
where AL.EntityName=''LocationTable'' and AL.ActionType=1)AT where 1=1' 
where ActionName='DepartmentModification'
go 
update EntityActionTable set ActionQuery='Select AT.* from (Select D.LocationCode as Code,D.LocationName as Name ,''Location Code'' as CodeTitle ,''Location Name'' as NameTitle,     
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType     
when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE '''' end as ActionType,
P.PersonFirstName+'' ''+P.PersonLastName as UserName,''Location Creation'' AS ActionName,''-'' AS FieldName,''-'' AS OldValue,''-'' AS NewValue,
P.PersonID as UserID   ,  ''All Company'' as CompanyName,1 as LanguageId,0 as CompanyID From LocationTable D    
join AuditLogTable AL on D.LocationID=convert(int,AL.AuditedObjectKeyValue1)   
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID    
join PersonTable P on P.PersonID=AT.UserID    
where AL.EntityName=''LocationTable'' and AL.ActionType=3) AT where 1=1' 
where ActionName='LocationDeletion'
go 
update EntityActionTable set ActionQuery='Select AT.* from (Select D.LocationCode as Code,D.LocationName as Name ,''Location Code'' as CodeTitle ,''Location Name'' as NameTitle,   
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''  end as ActionType,
P.PersonFirstName+'' ''+P.PersonLastName as UserName,''Location Modification'' AS ActionName,    
Case When ATL.FieldName = ''Status ID'' Then ''Status'' When ATL.FieldName = ''Parent Location ID'' Then ''Parent Location''  Else ATL.FieldName End as FieldName,  
Case  When ATL.FieldName = ''Status ID'' Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.OldValue),''-'')    
When ATL.FieldName = ''Parent Location ID'' Then ISNULL((Select SST.LocationDescription From LocationView SST Where SST.LocationID = ATL.OldValue),''-'')  
Else ISNULL(ATL.OldValue,''-'') End as OldValue,  
Case When ATL.FieldName = ''Status ID''   Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.NewValue),''-'')     
When ATL.FieldName = ''Parent Location ID'' Then ISNULL((Select SST.LocationName From LocationTable SST  Where SST.LocationID = ATL.NewValue),''-'')    
Else ISNULL(ATL.NewValue,''-'') End as NewValue,   P.PersonID as UserID ,
''All Company'' as CompanyName,1 as LanguageID,0 as CompanyID   From LocationTable D     
 join AuditLogTable AL on D.LocationID=convert(int,AL.AuditedObjectKeyValue1)     
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID     
INNER JOIN AuditLogLineItemTable ATL ON AL.AuditLogID=ATL.AuditLogID    
join PersonTable P on P.PersonID=AT.UserID     
where AL.EntityName=''LocationTable'' and AL.ActionType=2  
) AT where 1=1' 
where ActionName='LocationModification'
go 
update EntityActionTable set ActionQuery='SElect AT.* from (Select D.CategoryCode as Code,D.categoryName as Name ,''Category Code'' as CodeTitle ,''Category Name'' as NameTitle,    
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType  when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE '''' end as ActionType,
P.PersonFirstName+'' ''+P.PersonLastName as UserName,''Category Creation'' AS ActionName,''-'' AS FieldName,''-'' AS OldValue,''-'' AS NewValue,P.PersonID as UserID,  
''All Company'' as CompanyName ,1 as LanguageID,0 as CompanyID  From CategoryTable D   
 join AuditLogTable AL on D.CategoryID=convert(int,AL.AuditedObjectKeyValue1)    
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID    
join PersonTable P on P.PersonID=AT.UserID     where AL.EntityName=''CategoryTable'' and AL.ActionType=1)AT where 1=1 ' 
where ActionName='CategoryCreation'
go 
update EntityActionTable set ActionQuery='Select At.* from (Select D.CategoryCode as Code,D.CategoryName as Name ,''Category Code'' as CodeTitle ,''Category Name'' as NameTitle,     
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType  when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE '''' end as ActionType,
P.PersonFirstName+'' ''+P.PersonLastName as UserName,''Category Deletion'' AS ActionName,''-'' AS FieldName,''-'' AS OldValue,
''-'' AS NewValue,P.PersonID as UserID  ,  ''All Company'' as CompanyName,1 as LanguageID,0 as CompanyID  From CategoryTable D   
join AuditLogTable AL on D.CategoryID=convert(int,AL.AuditedObjectKeyValue1)    
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID    
join PersonTable P on P.PersonID=AT.UserID     where AL.EntityName=''CategoryTable'' and AL.ActionType=3) AT where 1=1' 
where ActionName='CategoryDeletion'
go 
update EntityActionTable set ActionQuery='Select AT.* from (Select D.CategoryCode as Code,D.CategoryName as Name ,''Category Code'' as CodeTitle ,''Category Name'' as NameTitle,      
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType     
when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''  end as ActionType,P.PersonFirstName+'' ''+P.PersonLastName as UserName,
''Category Modification'' AS ActionName,     Case When ATL.FieldName = ''Status ID'' Then ''Status'' When ATL.FieldName = ''Parent Category ID'' Then ''Parent Category''   
When ATL.FieldName = ''Depreciation Class ID'' Then ''Depreciation Class''    Else ATL.FieldName End as FieldName,     
Case  When ATL.FieldName = ''Status ID'' Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.OldValue),''-'')       
When ATL.FieldName = ''Parent Category ID'' Then ISNULL((Select SST.CategoryName From CategoryTable SST Where SST.CategoryID = ATL.OldValue ),''-'')  
When ATL.FieldName = ''Depreciation Class ID'' Then ISNULL((Select dpt.ClassName From DepreciationClassTable dpt Where dpt.DepreciationClassID = ATL.OldValue),''-'')  
Else ISNULL(ATL.OldValue,''-'') End as OldValue,    
Case When ATL.FieldName = ''Status ID'' Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.NewValue),''-'')     
When ATL.FieldName = ''Parent Category ID'' Then ISNULL((Select SST.CategoryName From CategoryTable SST Where SST.CategoryID = ATL.NewValue ),''-'')    
When ATL.FieldName = ''Depreciation Class ID'' Then ISNULL((Select SST.ClassName From DepreciationClassTable SST Where SST.DepreciationClassID = ATL.NewValue),''-'')    
Else ISNULL(ATL.NewValue,''-'') End as NewValue,   P.PersonID as UserID ,''All Company'' as CompanyName,1 as LanguageID, 0 as CompanyID   
From CategoryTable D   
join AuditLogTable AL on D.CategoryID=convert(int,AL.AuditedObjectKeyValue1)    
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID    
INNER JOIN AuditLogLineItemTable ATL ON AL.AuditLogID=ATL.AuditLogID     
join PersonTable P on P.PersonID=AT.UserID      
where AL.EntityName=''CategoryTable'' and AL.ActionType=2  
)AT where 1=1' 
where ActionName='CategoryModification'
go 


If not exists(SELECT  RIGHTNAME FROM USER_RIGHTTABLE where RIGHTNAME='InternalAssetTransferApproval')
Begin
Insert into USER_RIGHTTABLE(RIGHTNAME,RIGHTDESCRIPTION,VALUETYPE,DISPLAYRIGHT,RIGHTGROUPID,ISACTIVE,ISDELETED)
Values('InternalAssetTransferApproval','InternalAssetTransferApproval',95,1,
(select RightGroupID from USER_RIGHTGROUPTABLE where RightGroupName='Approval'),1,0);
End
Go
If not exists(SELECT MenuName FROM USER_MENUTABLE where MenuName='AuditLogReport' and ParentTransactionID=1)
Begin
Insert into user_menutable(MENUNAME,RightID,TARGETOBJECT,PARENTMENUID,ORDERNO,ParentTransactionID) Values(
'AuditLogReport',
(Select RightID FROM USER_RIGHTTABLE where RIGHTNAME='AuditLogReport'),'/ShowReport/AuditLogReport',
(Select MenuID from USER_MENUTABLE where MenuName='Report' ),8,1);
end
go

ALTER Procedure [dbo].[rprc_AuditLogRecords]
(
@FromDate nvarchar(100),
@ToDate nvarchar(100),
@UserID int,
@ActionName Nvarchar(50),
@CompanyID nvarchar(max),
@LanguageID int
)
As

 Set NoCount ON
      SET @CompanyID = 1003
   SET @UserID = NULL

    Declare @ActionQuery As NVarchar(Max)
    Declare @SQLQuery AS NVarchar(Max)
    Declare @ParamDefinition AS NVarchar(Max) 
    Declare @AuditTable Table(Code Varchar(max),Name varchar(Max),CodeTitle varchar(max),NameTitle varchar(max),EntityName Varchar(50),AuditLogTransactionDateTime NVARCHAR(100),AuditLogTransactionID int,
							ActionType Varchar(50),UserName Varchar(250),ActionName Varchar(50),FieldName Varchar(Max),OldValue Varchar(Max),
							NewValue Varchar(Max),UserID int,companyName nvarchar(max),LanguageID int,CompanyID int )
										    
		----insert into dynamicQuery(qry) select ActionQuery from EntityActionTable where ActionName=@ActionName
	set @ActionQuery = (select ActionQuery from EntityActionTable where ActionName=@ActionName)
    Set @SQLQuery = @ActionQuery  
     If @FromDate Is Not Null and @ToDate Is Not Null
	 begin
	 set @FromDate=convert(datetime,@FromDate,103)
	 set @ToDate=convert(datetime,@ToDate,103)
		set @ToDate=dateadd(day,1,cast(@ToDate as datetime))
      --Set @SQLQuery = @SQLQuery + ' And ( AT.AuditLogTransactionDateTime between @FromTransactionDate and @ToTransactionDate)'    
	   	 Set @SQLQuery = @SQLQuery + ' And (CONVERT(DATETIME,AT.AuditLogTransactionDateTime, 103) between cast(@FromDate as Datetime) and cast(@ToDate as Datetime))'    
		 end
	If @UserID Is Not Null
         Set @SQLQuery = @SQLQuery + ' And (AT.UserID = @UserID) and LanguageID=@LanguageID'           
    if (@ActionName='PartialDisposedAssetCreation' or @ActionName='PartialDisposedAssetModification'
			or @ActionName='AssetCreation' or @ActionName='AssetModification' or @ActionName='TransferCategory'
			or @ActionName='TransferAsset')
			begin 
			  --Set @SQLQuery = @SQLQuery + ' And (CompanyName in(Select CompanyName from CompanyTable where companyid in (select * from dbo.CompanyIDToTable(@companyID))))' 
			  	Set @SQLQuery = @SQLQuery + ' And (CompanyName in(Select CompanyDescription from CompanyDEscriptionTable where LanguageID=@LanguageID and companyid in (select Value from dbo.split(@companyID,'',''))))' 
			End 
		  
    Set @ParamDefinition =' 
			   @FromDate nvarchar(100)=null,
               @ToDate nvarchar(100)=null,
               @UserID int=null,
               @ActionName Nvarchar(50)=null,
               @companyID nvarchar(max)=null,
               @LanguageID int=null'

           -- print @SQLQuery   

    Insert Into @AuditTable

    Execute sp_Executesql  
                @SQLQuery, 
                @ParamDefinition,                                 
                @FromDate, 
                @ToDate, 
                @UserID,
                @ActionName,    
                @CompanyID,
                @LanguageID

   Select * from  @AuditTable   
		group by code ,Name ,CodeTitle ,NameTitle,EntityName ,AuditLogTransactionDateTime ,
			AuditLogTransactionID ,ActionType ,UserName ,ActionName ,UserID ,companyName,LanguageID ,CompanyID,FieldName,OldValue,NewValue    
								
    If @@ERROR <> 0 GoTo ErrorHandler 
    Return(0) 
ErrorHandler:
    Return(@@ERROR)


go 

update EntityActionTable set ActionQuery='Select AT.* from (Select D.ProductCode as Code,D.ProductName as Name ,''Product Code'' as CodeTitle ,''Product Name'' as NameTitle,    
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType  when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE '''' end as ActionType,
P.PersonFirstName+'' ''+P.PersonLastName as UserName,''Product Creation'' AS ActionName,''-'' AS FieldName,''-'' AS OldValue,''-'' AS NewValue,P.PersonID as UserID   , 
''All Company'' as CompanyName,1 as LanguageID,0 as CompanyID  From ProductTable D    
join AuditLogTable AL on D.ProductID=convert(int,AL.AuditedObjectKeyValue1)    
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID    
join PersonTable P on P.PersonID=AT.UserID     where AL.EntityName=''ProductTable'' and AL.ActionType=1)At where 1=1' 
where ActionName='ProductCreation'
go 
update EntityActionTable set ActionQuery='Select AT.* from (Select D.ProductCode as Code,D.ProductName as Name ,''Product Code'' as CodeTitle ,''Product Name'' as NameTitle,
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType 
when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''     end as ActionType,P.PersonFirstName+'' ''+P.PersonLastName as UserName,
''Product Deletion'' AS ActionName,''-'' AS FieldName,''-'' AS OldValue,''-'' AS NewValue,P.PersonID as UserID  ,  
''All Company'' as CompanyName,1 as LanguageId,0 as CompanyID  From ProductTable D      
join AuditLogTable AL on D.ProductID=convert(int,AL.AuditedObjectKeyValue1)   
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID    
join PersonTable P on P.PersonID=AT.UserID    
where AL.EntityName=''ProductTable'' and AL.ActionType=3)AT where 1=1' 
where ActionName='ProductDeletion'
go 
update EntityActionTable set ActionQuery='Select AT.* from (Select D.ProductCode as Code,D.ProductName as Name ,''Product Code'' as CodeTitle ,''Product Name'' as NameTitle,    
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType
when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''     end as ActionType,P.PersonFirstName+'' ''+P.PersonLastName as UserName,
''Product Modification'' AS ActionName,   
Case When ATL.FieldName = ''Status ID'' Then ''Status''  when ATL.FieldName = ''Category ID'' Then ''Category Name''    
Else ATL.FieldName End as FieldName,  Case  When ATL.FieldName = ''Status ID'' Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.OldValue),''-'')      
When ATL.FieldName = ''Category ID'' Then ISNULL((Select SST.CategoryName From CategoryTable SST where SST.categoryID = ATL.OldValue),''-'')    Else ISNULL(ATL.OldValue,''-'') End as OldValue,  
Case When ATL.FieldName = ''Status ID''   Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.NewValue),''-'')  
When ATL.FieldName = ''Category ID'' Then ISNULL((Select SST.CategoryName From CategoryTable SST where SST.categoryID= ATL.NewValue),''-'')  Else ISNULL(ATL.NewValue,''-'')   End as NewValue, 
P.PersonID  as UserID,''All Company'' as CompanyName,1 as LanguageID,0 as CompanyID  
From ProductTable D     join AuditLogTable AL on D.ProductID=convert(int,AL.AuditedObjectKeyValue1)   
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID    
INNER JOIN AuditLogLineItemTable ATL ON AL.AuditLogID=ATL.AuditLogID  join PersonTable P on P.PersonID=AT.UserID    
where AL.EntityName=''ProductTable'' and AL.ActionType=2 
)At where 1=1' 
where ActionName='ProductModification'
go 

update EntityActionTable set ActionQuery='select AT.* from (Select D.ModelCode as Code,D.ModelName as Name ,''Model Code'' as CodeTitle ,''Model Name'' as NameTitle,     
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,
Case AL.ActionType when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE '''' end as ActionType,P.PersonFirstName+'' ''+P.PersonLastName as UserName,
''Model Creation'' AS ActionName,''-'' AS FieldName,''-'' AS OldValue,''-'' AS NewValue,P.PersonID as UserID   ,  ''All Company'' as CompanyName,1 as LanguageID,0 as CompanyID   
From ModelTable D     
join AuditLogTable AL on D.ModelID=convert(int,AL.AuditedObjectKeyValue1)   
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID  
join PersonTable P on P.PersonID=AT.UserID   where AL.EntityName=''ModelTable'' and AL.ActionType=1)AT where 1=1' 
where ActionName='ModelCreation'
go 

update EntityActionTable set ActionQuery='select AT.* from (Select D.ModelCode as Code,D.ModelName as Name ,''Model Code'' as CodeTitle ,''Model Name'' as NameTitle,
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType
when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''     end as ActionType,P.PersonFirstName+'' ''+P.PersonLastName as UserName,
''Model Deletion'' AS ActionName,''-'' AS FieldName,''-'' AS OldValue,''-'' AS NewValue,P.PersonID as UserID,  ''All Company'' as CompanyName, 1 as LanguageID, 0 as CompanyID  
From ModelTable D     
join AuditLogTable AL on D.ModelID=convert(int,AL.AuditedObjectKeyValue1)   
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID    
join PersonTable P on P.PersonID=AT.UserID   where AL.EntityName=''ModelTable'' and AL.ActionType=3)AT where 1=1' 
where ActionName='ModelDeletion'
go 
update EntityActionTable set ActionQuery='select AT.* from (Select D.ModelCode as Code,D.ModelName as Name ,''Model Code'' as CodeTitle ,''Model Name'' as NameTitle,
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType
when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE '''' 
end as ActionType,P.PersonFirstName+'' ''+P.PersonLastName as UserName,''Model Modification'' AS ActionName,     
Case When ATL.FieldName = ''Status ID'' Then ''Status''    Else ATL.FieldName End as FieldName,        
Case  When ATL.FieldName = ''Status ID'' Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.OldValue),''-'')  
Else ISNULL(ATL.OldValue,''-'') End as OldValue,   Case When ATL.FieldName = ''Status ID'' 
Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.NewValue),''-'')
Else ISNULL(ATL.NewValue,''-'') End as NewValue,   P.PersonID as UserID ,''All Company'' as CompanyName,1 as LanguageID, 0 as CompanyID   From ModelTable D     
  join AuditLogTable AL on D.ModelID=convert(int,AL.AuditedObjectKeyValue1)       
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID      
INNER JOIN AuditLogLineItemTable ATL ON AL.AuditLogID=ATL.AuditLogID        
join PersonTable P on P.PersonID=AT.UserID    where AL.EntityName=''ModelTable'' and AL.ActionType=2    
) At where 1=1' 
where ActionName='ModelModification'
go 
update EntityActionTable set ActionQuery='select AT.* from (Select D.ManufacturerCode as Code,D.ManufacturerName as Name ,''Manufacturer Code'' as CodeTitle ,''Manufacturer Name'' as NameTitle,
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType 
when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''
end as ActionType,P.PersonFirstName+'' ''+P.PersonLastName as UserName,''Manufacturer Creation'' AS ActionName,''-'' AS FieldName,''-'' AS OldValue,''-'' AS NewValue,P.PersonID as UserID   , 
''All Company'' as CompanyName,1 as LanguageID,0 as CompanyID   From ManufacturerTable D  
join AuditLogTable AL on D.ManufacturerID=convert(int,AL.AuditedObjectKeyValue1)   
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID   
join PersonTable P on P.PersonID=AT.UserID   where AL.EntityName=''ManufacturerTable'' and AL.ActionType=1
)AT where 1=1' 
where ActionName='ManufacturerCreation'
go 
update EntityActionTable set ActionQuery='select AT.* from (Select D.ManufacturerCode as Code,D.ManufacturerName as Name ,''Manufacturer Code'' as CodeTitle ,''Manufacturer Name'' as NameTitle,   
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType     
when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''   end as ActionType,P.PersonFirstName+'' ''+P.PersonLastName as UserName,
''Manufacturer Deletion'' AS ActionName,''-'' AS FieldName,''-'' AS OldValue,''-'' AS NewValue,P.PersonID as UserID, 
''All Company'' as CompanyName,1 as LanguageID, 0 as CompanyID  From ManufacturerTable D      
join AuditLogTable AL on D.ManufacturerID=convert(int,AL.AuditedObjectKeyValue1)    
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID    
join PersonTable P on P.PersonID=AT.UserID   where AL.EntityName=''ManufacturerTable'' and AL.ActionType=3
)AT where 1=1' 
where ActionName='ManufacturerDeletion'
go 
update EntityActionTable set ActionQuery='select AT.* from (Select D.ManufacturerCode as Code,D.ManufacturerName as Name ,''Manufacturer Code'' as CodeTitle ,''Manufacturer Name'' as NameTitle,
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType 
when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''
end as ActionType,P.PersonFirstName+'' ''+P.PersonLastName as UserName,''Manufacturer Modification'' AS ActionName,
Case When ATL.FieldName = ''Status ID'' Then ''Status''    Else ATL.FieldName End as FieldName,
Case  When ATL.FieldName = ''Status ID'' Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.OldValue),''-'') 
Else ISNULL(ATL.OldValue,''-'') End as OldValue,   Case When ATL.FieldName = ''Status ID'' 
Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.NewValue),''-'') 
Else ISNULL(ATL.NewValue,''-'') End as NewValue,   P.PersonID as UserID ,''All Company'' as CompanyName,1 as LanguageID, 0 as CompanyID  
From ManufacturerTable D    
join AuditLogTable AL on D.ManufacturerID=convert(int,AL.AuditedObjectKeyValue1)       
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID        
INNER JOIN AuditLogLineItemTable ATL ON AL.AuditLogID=ATL.AuditLogID        
join PersonTable P on P.PersonID=AT.UserID    where AL.EntityName=''ManufacturerTable'' and AL.ActionType=2) At where 1=1' 
where ActionName='ManufacturerModification'
go 
update EntityActionTable set ActionQuery='Select AT.* from (Select D.PartyCode as Code,D.PartyName as Name ,''Supplier Code'' as CodeTitle ,''Supplier Name'' as NameTitle,    
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType   when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''
end as ActionType,P.PersonFirstName+'' ''+P.PersonLastName as UserName,''Supplier Creation'' AS ActionName,''-'' AS FieldName,''-'' AS OldValue,''-'' AS NewValue,
P.PersonID as UserID  ,  ''All Company'' as CompanyName, 1 as LanguageID,0 as CompanyID From PartyTable D   
 join AuditLogTable AL on D.PartyID=convert(int,AL.AuditedObjectKeyValue1)    join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID    
join PersonTable P on P.PersonID=AT.UserID   where AL.EntityName=''SupplierTable'' and AL.ActionType=1)AT where 1=1' 
where ActionName='SupplierCreation'
go 
update EntityActionTable set ActionQuery='Select AT.* from (Select D.PartyCode as Code,D.PartyName as Name ,''Supplier Code'' as CodeTitle ,''Supplier Name'' as NameTitle,    
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType   when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''
end as ActionType,P.PersonFirstName+'' ''+P.PersonLastName as UserName,''Supplier Creation'' AS ActionName,''-'' AS FieldName,''-'' AS OldValue,''-'' AS NewValue,
P.PersonID as UserID  ,  ''All Company'' as CompanyName, 1 as LanguageID,0 as CompanyID From PartyTable D   
 
join AuditLogTable AL on D.PartyID=convert(int,AL.AuditedObjectKeyValue1)    join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID    
join PersonTable P on P.PersonID=AT.UserID   where AL.EntityName=''SupplierTable'' and AL.ActionType=3)AT where 1=1' 
where ActionName='SupplierDeletion'
go 
update EntityActionTable set ActionQuery='Select AT.* from (Select D.PartyCode as Code,D.PartyName as Name ,''Supplier Code'' as CodeTitle ,''Supplier Name'' as NameTitle,AL.EntityName,
AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType 
when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE '''' end as ActionType,P.PersonFirstName+'' ''+P.PersonLastName as UserName,
''Supplier Modification'' AS ActionName,     Case When ATL.FieldName = ''Status ID'' Then ''Status''    Else ATL.FieldName End as FieldName,   
Case  When ATL.FieldName = ''Status ID'' Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.OldValue),''-'')     
Else ISNULL(ATL.OldValue,''-'') End as OldValue,     
Case When ATL.FieldName = ''Status ID'' Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.NewValue),''-'')     
Else ISNULL(ATL.NewValue,''-'') End as NewValue,   P.PersonID as UserID ,''All Company'' as CompanyName,1 as LanguageId,0 as CompanyID   From PartyTable D  
join AuditLogTable AL on D.PartyID=convert(int,AL.AuditedObjectKeyValue1)     
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID     
INNER JOIN AuditLogLineItemTable ATL ON AL.AuditLogID=ATL.AuditLogID 
join PersonTable P on P.PersonID=AT.UserID    where AL.EntityName=''SupplierTable'' and AL.ActionType=2) AT where 1=1  ' 
where ActionName='SupplierModification'
go 

update EntityActionTable set ActionQuery='select AT.Code,AT.Name,AT.CodeTitle,AT.NameTitle,AT.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,AT.ActionType, 
AT.UserName,AT.ActionName,AT.FieldName,AT.OldValue,AT.NewValue,AT.UserID,AT.CompanyName,AT.LanguageID,AT.CompanyID  from
(  
Select D.Barcode as Code,D.AssetDescription as Name ,''Barcode'' as CodeTitle ,''Asset Description'' as NameTitle, AL.EntityName,AT.AuditLogTransactionDateTime,
AT.AuditLogTransactionID,Case AL.ActionType   when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''   end as ActionType,
P.PersonFirstName+'' ''+P.PersonLastName as UserName,''Asset Modification'' AS ActionName,  
Case APPROVE.StatusId when 6 then   Case When APPROVELINEITEM.FieldName = ''Status ID'' Then ''Status'' 
When APPROVELINEITEM.FieldName = ''Asset Condition ID'' Then ''Asset Condition''   
When APPROVELINEITEM.FieldName = ''Department ID'' Then ''Department Name'' 
When APPROVELINEITEM.FieldName = ''Section ID'' Then ''Section Name'' 
When APPROVELINEITEM.FieldName = ''Location ID'' Then ''Location name''   When APPROVELINEITEM.FieldName = ''Custodian ID'' Then ''Custodian Name'' 
When APPROVELINEITEM.FieldName = ''Category ID'' Then ''Category Name''  When APPROVELINEITEM.FieldName = ''Depreciation Class ID'' Then ''Depreciation Class''  
When APPROVELINEITEM.FieldName = ''Supplier ID'' Then ''Supplier Name'' When APPROVELINEITEM.FieldName = ''Product ID'' Then ''Asset Description''   
When APPROVELINEITEM.FieldName = ''Uploaded Document Path'' Then ''Uploaded Document'' when  APPROVELINEITEM.FieldName = ''Uploaded Image Path''  then ''Uploaded Image''   
when  APPROVELINEITEM.FieldName = ''Network ID''  then ''Owned/ Leased''   Else APPROVELINEITEM.FieldName End  else  
Case When ATL.FieldName = ''Status ID'' Then ''Status'' When ATL.FieldName = ''Asset Condition ID'' Then ''Asset Condition''   
When ATL.FieldName = ''Department ID'' Then ''Department Name'' When ATL.FieldName = ''Section ID'' Then ''Section Name'' When ATL.FieldName = ''Location ID'' 
Then ''Location name''   When ATL.FieldName = ''Custodian ID'' Then ''Custodian Name'' When ATL.FieldName = ''Category ID'' Then ''Category Name''  
When ATL.FieldName = ''Depreciation Class ID'' Then ''Depreciation Class''   When ATL.FieldName = ''Supplier ID'' Then ''Supplier Name'' When ATL.FieldName = ''Product ID''
Then ''Asset Description''  When ATL.FieldName = ''Uploaded Document Path'' Then ''Uploaded Document'' when  ATL.FieldName = ''Uploaded Image Path'' 
then ''Uploaded Image''   when  APPROVELINEITEM.FieldName = ''Network ID''  then ''Owned/ Leased''   Else ATL.FieldName End  End as FieldName,  
Case  When (ATL.FieldName = ''Status ID'' or APPROVELINEITEM.FieldName=''Status ID'') Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.OldValue else  ATL.OldValue end )),''-'')   
When (ATL.FieldName = ''Asset Condition ID'' or APPROVELINEITEM.FieldName=''Asset Condition ID'') Then ISNULL((Select sst.AssetConditionName From AssetConditionTable SST Where SST.AssetConditionID = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.OldValue else  ATL.OldValue end )),''-'')    
When (ATL.FieldName = ''Department ID'' or APPROVELINEITEM.FieldName=''Department ID'') Then ISNULL((Select sst.DepartmentName From DepartmentTable SST Where SST.DepartmentID = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.OldValue else  ATL.OldValue end )),''-'')    
When (ATL.FieldName = ''Section ID'' or APPROVELINEITEM.FieldName=''Section ID'') Then ISNULL((Select sst.SectionName From SectionTable SST  Where SST.SectionID = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.OldValue else  ATL.OldValue end )),''-'')    
When (ATL.FieldName = ''Location ID'' or APPROVELINEITEM.FieldName=''Location ID'') Then ISNULL((Select SST.PID4 From LocationHierarchicalView SST Where SST.LanguageID=L.LanguageID and SST.childID = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.OldValue else  ATL.OldValue end )),''-'')   
When (ATL.FieldName = ''Custodian ID'' or APPROVELINEITEM.FieldName=''Custodian ID'') Then ISNULL((Select SST.PersonfirstName From PersonTable SST Where SST.PersonID = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.OldValue else  ATL.OldValue end )),''-'')     
When (ATL.FieldName = ''Category ID'' or APPROVELINEITEM.FieldName=''Category ID'') Then ISNULL((Select SST.PID3 From CategoryHierarchicalView SST Where SST.LanguageID=L.LanguageID and SST.ChildId = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.OldValue else  ATL.OldValue end )),''-'')    
When (ATL.FieldName = ''Depreciation Class ID'' or APPROVELINEITEM.FieldName=''Depreciation Class ID'')  Then ISNULL((Select SST.ClassName From DepreciationClassTable SST Where SST.DepreciationClassID = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.OldValue else  ATL.OldValue end )),''-'')   
When (ATL.FieldName = ''Supplier ID'' or APPROVELINEITEM.FieldName=''Supplier ID'') Then ISNULL((Select SST.partyname From partytable SST  Where SST.PartyID =(Case APPROVE.StatusId when 6 then APPROVELINEITEM.OldValue else  ATL.OldValue end )),''-'')   
When (ATL.FieldName = ''Product ID'' or APPROVELINEITEM.FieldName=''Product ID'') Then ISNULL((Select SST.ProductName From productTable SST Where SST.productID =(Case APPROVE.StatusId when 6 then APPROVELINEITEM.OldValue else  ATL.OldValue end )),''-'')   
When (ATL.FieldName = ''Uploaded Document Path'' or APPROVELINEITEM.FieldName=''Uploaded Document Path'')  then   isnull(Case APPROVE.StatusId when 6 then RIGHT( APPROVELINEITEM.OldValue, CHARINDEX( ''\'', REVERSE( APPROVELINEITEM.OldValue ) + ''\'' ) - 1 )  else  RIGHT( ATL.OldValue, CHARINDEX( ''\'', REVERSE( ATL.OldValue ) + ''\'' ) - 1 )    end,''-'' )  
when (ATL.FieldName = ''Uploaded Image Path'' or APPROVELINEITEM.FieldName=''Uploaded Image Path'') then isnull(Case APPROVE.StatusId when 6 then   RIGHT( APPROVELINEITEM.OldValue, CHARINDEX( ''\'', REVERSE( APPROVELINEITEM.OldValue ) + ''\'' ) - 1 )   else  RIGHT( ATL.OldValue, CHARINDEX( ''\'', REVERSE( ATL.OldValue ) + ''\'' ) - 1 )    end ,''-'')  
 when ATL.OldValue=''null'' then NULL else ATL.OldValue
End as OldValue, 
 
 Case When (ATL.FieldName = ''Status ID'' or APPROVELINEITEM.FieldName=''Status ID'')  Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.NewValue else  ATL.NewValue end )),''-'') 
when (ATL.FieldName = ''Asset Condition ID'' or APPROVELINEITEM.FieldName=''Asset Condition ID'') Then ISNULL((Select SST.AssetConditionName From AssetConditionTable SST  Where SST.AssetConditionID = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.NewValue else  ATL.NewValue end )),''-'')    
When (ATL.FieldName = ''Department ID'' or APPROVELINEITEM.FieldName=''Department ID'') Then ISNULL((Select SST.DepartmentName From DepartmentTable SST  Where SST.DepartmentID = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.NewValue else  ATL.NewValue end )),''-'')   
When (ATL.FieldName = ''Section ID'' or APPROVELINEITEM.FieldName=''Section ID'') Then ISNULL((Select SST.SectionName From SectionTable SST  Where SST.SectionID = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.NewValue else  ATL.NewValue end )),''-'')    
When (ATL.FieldName = ''Location ID'' or APPROVELINEITEM.FieldName=''Location ID'') Then ISNULL((Select SST.PID4 From LocationHierarchicalView SST Where SST.LanguageID=L.LanguageID and SST.ChildId =(Case APPROVE.StatusId when 6 then APPROVELINEITEM.NewValue else  ATL.NewValue end )),''-'')     
When (ATL.FieldName = ''Custodian ID'' or APPROVELINEITEM.FieldName=''Custodian ID'')  Then ISNULL((Select SST.PersonfirstName From PersonTable SST Where SST.PersonID = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.NewValue else  ATL.NewValue end )),''-'')    
When (ATL.FieldName = ''Category ID'' or APPROVELINEITEM.FieldName=''Category ID'') Then ISNULL((Select SST.PID3 From CategoryHierarchicalView SST Where SST.LanguageID=L.LanguageID and SST.childID = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.NewValue else  ATL.NewValue end )),''-'')      
When (ATL.FieldName = ''Depreciation Class ID'' or APPROVELINEITEM.FieldName=''Depreciation Class ID'')Then ISNULL((Select SST.ClassName From DepreciationClassTable SST Where SST.DepreciationClassID = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.NewValue else  ATL.NewValue end )),''-'')  
When (ATL.FieldName = ''Supplier ID'' or APPROVELINEITEM.FieldName=''Supplier ID'') Then ISNULL((Select SST.partyname From PartyTable SST  Where SST.PartyID = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.NewValue else  ATL.NewValue end )),''-'')   
When (ATL.FieldName = ''Product ID'' or APPROVELINEITEM.FieldName=''Product ID'') Then ISNULL((Select sst.ProductName From productTable SST  Where SST.productID =(Case APPROVE.StatusId when 6 then APPROVELINEITEM.NewValue else  ATL.NewValue end )),''-'')    
When (ATL.FieldName = ''Uploaded Document Path'' or APPROVELINEITEM.FieldName=''Uploaded Document Path'')  then   isnull(Case APPROVE.StatusId when 6 then RIGHT( APPROVELINEITEM.NewValue, CHARINDEX( ''\'', REVERSE( APPROVELINEITEM.NewValue ) + ''\'' ) - 1 )  else  RIGHT( ATL.NewValue, CHARINDEX( ''\'', REVERSE( ATL.NewValue ) + ''\'' ) - 1 )    end,''-'' ) 
when (ATL.FieldName = ''Uploaded Image Path'' or APPROVELINEITEM.FieldName=''Uploaded Image Path'') then isnull(Case APPROVE.StatusId when 6 then   RIGHT( APPROVELINEITEM.NewValue, CHARINDEX( ''\'', REVERSE( APPROVELINEITEM.NewValue ) + ''\'' ) - 1 )   else  RIGHT( ATL.NewValue, CHARINDEX( ''\'', REVERSE( ATL.NewValue ) + ''\'' ) - 1 )    end ,''-'')  
when ATL.newvalue=''null'' then NULL else ATL.newvalue
End as NewValue,  P.PersonID as UserID,CD.CompanyDescription as CompanyName, L.LanguageID,Com.CompanyID  
From AssetTable D join LanguageTable L on 1=1 and L.LanguageID=1  
join AuditLogTable AL on D.AssetID=convert(int,AL.AuditedObjectKeyValue1) 
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID 
INNER JOIN AuditLogLineItemTable ATL ON AL.AuditLogID=ATL.AuditLogID 
LEFT OUTER JOIN ApprovalTable APPROVE on AL.AuditedObjectKeyValue1 = APPROVE.ObjectKeyValue and APPROVE.StatusID = 6 and APPROVE.ActionType=2 and   
cast((convert(CHAR(11),AT.AuditLogTransactionDateTime,101)) as datetime)=cast(convert(char(11),APPROVE.CreatedDatetime,101) as datetime)  
LEFT OUTER JOIN ApprovalLineItemTable APPROVELINEITEM on APPROVE.ApprovalID = APPROVELINEITEM.ApprovalID  join PersonTable P on P.PersonID=AT.UserID  
join CompanyTable com on D.CompanyID=Com.companyID  join CompanyDescriptionTable CD on Com.CompanyID=CD.CompanyID and L.LanguageID=CD.LanguageID 
where AL.EntityName in (''AssetTable'') and AL.ActionType=2 and AT.url not like ''%AssetMapping%'' and AT.url not like''%AttachDetachApproval%'' 
and AT.url not like''%TransferAsset%'' and AT.url not like''%AssetApproval%''  and AT.url not like ''%PartialDisposedAsset%'' 
and AT.url not like ''%TransferCategory%'' and ATL.Fieldname!=''Asset Description'') AT   where AT.FieldName not in (''Asset Approval'',''Mail Alert'')' 
where ActionName='AssetModification'
go 
if not exists(select ActionName from EntityActionTable where ActionName='ApprovalRoleCreate')
Begin
 Insert into EntityActionTable(TemplateName,ActionName,ActionQuery)
 values('ApprovalRoleCreate','ApprovalRoleCreation','select AT.* from (Select D.ApprovalRoleCode as Code,D.ApprovalRoleName as Name ,''Approval Role Code'' as CodeTitle ,''Approval Role Name'' as NameTitle,    
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType     when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''     end as ActionType,
P.PersonFirstName+'' ''+P.PersonLastName as UserName,''Department Creation'' AS ActionName,''-'' AS FieldName,''-'' AS OldValue,''-'' AS NewValue,P.PersonID as UserID   , 
''All Company'' as CompanyName,1 as LanguageID,0 as CompanyID   From ApprovalRoleTable D   
join AuditLogTable AL on D.ApprovalRoleID=convert(int,AL.AuditedObjectKeyValue1)   
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID    
join PersonTable P on P.PersonID=AT.UserID   where AL.EntityName=''ApprovalRoleTable'' and AL.ActionType=1)AT where 1=1')
End
go
if not exists(select ActionName from EntityActionTable where ActionName='ApprovalRoleModification')
Begin
Insert into EntityActionTable(TemplateName,ActionName,ActionQuery)
 values('ApprovalRoleModify','ApprovalRoleModification','select AT.* from (Select D.ApprovalRoleCode as Code,D.ApprovalRoleName as Name ,''Approval Role Code'' as CodeTitle ,''Approval Role Name'' as NameTitle,   
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType        
when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''         
end as ActionType,P.PersonFirstName+'' ''+P.PersonLastName as UserName,''ApprovalRole Modification'' AS ActionName,   
Case When ATL.FieldName = ''Status ID'' Then ''Status''    Else ATL.FieldName End as FieldName,     
Case  When ATL.FieldName = ''Status ID'' Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.OldValue),''-'')    
Else ISNULL(ATL.OldValue,''-'') End as OldValue,   Case When ATL.FieldName = ''Status ID''      Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.NewValue),''-'')     
Else ISNULL(ATL.NewValue,''-'') End as NewValue,   P.PersonID as UserID ,''All Company'' as CompanyName,1 as LanguageID, 0 as CompanyID   
From ApprovalRoleTable D       
  join AuditLogTable AL on D.ApprovalRoleID=convert(int,AL.AuditedObjectKeyValue1)       
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID      
INNER JOIN AuditLogLineItemTable ATL ON AL.AuditLogID=ATL.AuditLogID       
join PersonTable P on P.PersonID=AT.UserID    
where AL.EntityName=''ApprovalRoleTable'' and AL.ActionType=2) At where 1=1')
End
Go 
if not exists(select ActionName from EntityActionTable where ActionName='ApprovalRoleDeletion')
Begin
Insert into EntityActionTable(TemplateName,ActionName,ActionQuery)
values('ApprovalRoleDelete','ApprovalRoleDeletion','select AT.* from (Select D.ApprovalRoleCode as Code,D.ApprovalRoleName as Name ,''ApprovalRole Code'' as CodeTitle ,''ApprovalRole Name'' as NameTitle,   
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType     when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''     end as ActionType,
P.PersonFirstName+'' ''+P.PersonLastName as UserName,''ApprovalRole Deletion'' AS ActionName,''-'' AS FieldName,''-'' AS OldValue,''-'' AS NewValue,P.PersonID as UserID,
''All Company'' as CompanyName,1 as LanguageID, 0 as CompanyID  From ApprovalRoleTable D    
join AuditLogTable AL on D.ApprovalRoleID=convert(int,AL.AuditedObjectKeyValue1)  
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID   
join PersonTable P on P.PersonID=AT.UserID   where AL.EntityName=''ApprovalRoleTable'' and AL.ActionType=3)AT where 1=1')
End
Go 

if not exists(select ActionName from EntityActionTable where ActionName='DesignationCreation')
Begin
 Insert into EntityActionTable(TemplateName,ActionName,ActionQuery)
 values('DesignationCreate','DesignationCreation','select AT.* from (Select D.DesignationCode as Code,D.DesignationName as Name ,''Approval Role Code'' as CodeTitle ,''Approval Role Name'' as NameTitle,    
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType     when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''     end as ActionType,
P.PersonFirstName+'' ''+P.PersonLastName as UserName,''Department Creation'' AS ActionName,''-'' AS FieldName,''-'' AS OldValue,''-'' AS NewValue,P.PersonID as UserID   , 
''All Company'' as CompanyName,1 as LanguageID,0 as CompanyID   From DesignationTable D   
join AuditLogTable AL on D.DesignationID=convert(int,AL.AuditedObjectKeyValue1)   
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID    
join PersonTable P on P.PersonID=AT.UserID   where AL.EntityName=''DesignationTable'' and AL.ActionType=1)AT where 1=1')
End
go
if not exists(select ActionName from EntityActionTable where ActionName='DesignationModification')
Begin
Insert into EntityActionTable(TemplateName,ActionName,ActionQuery)
 values('DesignationModify','DesignationModification','select AT.* from (Select D.DesignationCode as Code,D.DesignationName as Name ,''Approval Role Code'' as CodeTitle ,''Approval Role Name'' as NameTitle,   
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType        
when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''         
end as ActionType,P.PersonFirstName+'' ''+P.PersonLastName as UserName,''Designation Modification'' AS ActionName,   
Case When ATL.FieldName = ''Status ID'' Then ''Status''    Else ATL.FieldName End as FieldName,     
Case  When ATL.FieldName = ''Status ID'' Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.OldValue),''-'')    
Else ISNULL(ATL.OldValue,''-'') End as OldValue,   Case When ATL.FieldName = ''Status ID''      Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.NewValue),''-'')     
Else ISNULL(ATL.NewValue,''-'') End as NewValue,   P.PersonID as UserID ,''All Company'' as CompanyName,1 as LanguageID, 0 as CompanyID   
From DesignationTable D       
  join AuditLogTable AL on D.DesignationID=convert(int,AL.AuditedObjectKeyValue1)       
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID      
INNER JOIN AuditLogLineItemTable ATL ON AL.AuditLogID=ATL.AuditLogID       
join PersonTable P on P.PersonID=AT.UserID    
where AL.EntityName=''DesignationTable'' and AL.ActionType=2) At where 1=1')
End
Go 
if not exists(select ActionName from EntityActionTable where ActionName='DesignationDeletion')
Begin
Insert into EntityActionTable(TemplateName,ActionName,ActionQuery)
values('DesignationDelete','DesignationDeletion','select AT.* from (Select D.DesignationCode as Code,D.DesignationName as Name ,''Designation Code'' as CodeTitle ,''Designation Name'' as NameTitle,   
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType     when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''     end as ActionType,
P.PersonFirstName+'' ''+P.PersonLastName as UserName,''Designation Deletion'' AS ActionName,''-'' AS FieldName,''-'' AS OldValue,''-'' AS NewValue,P.PersonID as UserID,
''All Company'' as CompanyName,1 as LanguageID, 0 as CompanyID  From DesignationTable D    
join AuditLogTable AL on D.DesignationID=convert(int,AL.AuditedObjectKeyValue1)  
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID   
join PersonTable P on P.PersonID=AT.UserID   where AL.EntityName=''DesignationTable'' and AL.ActionType=3)AT where 1=1')
End
Go 
if not exists(select ActionName from EntityActionTable where ActionName='CategoryTypeCreation')
Begin
 Insert into EntityActionTable(TemplateName,ActionName,ActionQuery)
 values('CategoryTypeCreate','CategoryTypeCreation','select AT.* from (Select D.CategoryTypeCode as Code,D.CategoryTypeName as Name ,''Approval Role Code'' as CodeTitle ,''Approval Role Name'' as NameTitle,    
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType     when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''     end as ActionType,
P.PersonFirstName+'' ''+P.PersonLastName as UserName,''Department Creation'' AS ActionName,''-'' AS FieldName,''-'' AS OldValue,''-'' AS NewValue,P.PersonID as UserID   , 
''All Company'' as CompanyName,1 as LanguageID,0 as CompanyID   From CategoryTypeTable D   
join AuditLogTable AL on D.CategoryTypeID=convert(int,AL.AuditedObjectKeyValue1)   
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID    
join PersonTable P on P.PersonID=AT.UserID   where AL.EntityName=''CategoryTypeTable'' and AL.ActionType=1)AT where 1=1')
End
go
if not exists(select ActionName from EntityActionTable where ActionName='CategoryTypeModification')
Begin
Insert into EntityActionTable(TemplateName,ActionName,ActionQuery)
 values('CategoryTypeModify','CategoryTypeModification','select AT.* from (Select D.CategoryTypeCode as Code,D.CategoryTypeName as Name ,''Approval Role Code'' as CodeTitle ,''Approval Role Name'' as NameTitle,   
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType        
when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''         
end as ActionType,P.PersonFirstName+'' ''+P.PersonLastName as UserName,''CategoryType Modification'' AS ActionName,   
Case When ATL.FieldName = ''Status ID'' Then ''Status''    Else ATL.FieldName End as FieldName,     
Case  When ATL.FieldName = ''Status ID'' Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.OldValue),''-'')    
Else ISNULL(ATL.OldValue,''-'') End as OldValue,   Case When ATL.FieldName = ''Status ID''      Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.NewValue),''-'')     
Else ISNULL(ATL.NewValue,''-'') End as NewValue,   P.PersonID as UserID ,''All Company'' as CompanyName,1 as LanguageID, 0 as CompanyID   
From CategoryTypeTable D       
  join AuditLogTable AL on D.CategoryTypeID=convert(int,AL.AuditedObjectKeyValue1)       
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID      
INNER JOIN AuditLogLineItemTable ATL ON AL.AuditLogID=ATL.AuditLogID       
join PersonTable P on P.PersonID=AT.UserID    
where AL.EntityName=''CategoryTypeTable'' and AL.ActionType=2) At where 1=1')
End
Go 
if not exists(select ActionName from EntityActionTable where ActionName='CategoryTypeDeletion')
Begin
Insert into EntityActionTable(TemplateName,ActionName,ActionQuery)
values('CategoryTypeDelete','CategoryTypeDeletion','select AT.* from (Select D.CategoryTypeCode as Code,D.CategoryTypeName as Name ,''CategoryType Code'' as CodeTitle ,''CategoryType Name'' as NameTitle,   
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType     when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''     end as ActionType,
P.PersonFirstName+'' ''+P.PersonLastName as UserName,''CategoryType Deletion'' AS ActionName,''-'' AS FieldName,''-'' AS OldValue,''-'' AS NewValue,P.PersonID as UserID,
''All Company'' as CompanyName,1 as LanguageID, 0 as CompanyID  From CategoryTypeTable D    
join AuditLogTable AL on D.CategoryTypeID=convert(int,AL.AuditedObjectKeyValue1)  
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID   
join PersonTable P on P.PersonID=AT.UserID   where AL.EntityName=''CategoryTypeTable'' and AL.ActionType=3)AT where 1=1')
End
Go 

if not exists(select ActionName from EntityActionTable where ActionName='LocationTypeCreation')
Begin
 Insert into EntityActionTable(TemplateName,ActionName,ActionQuery)
 values('LocationTypeCreate','LocationTypeCreation','select AT.* from (Select D.LocationTypeCode as Code,D.LocationTypeName as Name ,''Approval Role Code'' as CodeTitle ,''Approval Role Name'' as NameTitle,    
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType     when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''     end as ActionType,
P.PersonFirstName+'' ''+P.PersonLastName as UserName,''Department Creation'' AS ActionName,''-'' AS FieldName,''-'' AS OldValue,''-'' AS NewValue,P.PersonID as UserID   , 
''All Company'' as CompanyName,1 as LanguageID,0 as CompanyID   From LocationTypeTable D   
join AuditLogTable AL on D.LocationTypeID=convert(int,AL.AuditedObjectKeyValue1)   
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID    
join PersonTable P on P.PersonID=AT.UserID   where AL.EntityName=''LocationTypeTable'' and AL.ActionType=1)AT where 1=1')
End
go
if not exists(select ActionName from EntityActionTable where ActionName='LocationTypeModification')
Begin
Insert into EntityActionTable(TemplateName,ActionName,ActionQuery)
 values('LocationTypeModify','LocationTypeModification','select AT.* from (Select D.LocationTypeCode as Code,D.LocationTypeName as Name ,''Approval Role Code'' as CodeTitle ,''Approval Role Name'' as NameTitle,   
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType        
when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''         
end as ActionType,P.PersonFirstName+'' ''+P.PersonLastName as UserName,''LocationType Modification'' AS ActionName,   
Case When ATL.FieldName = ''Status ID'' Then ''Status''    Else ATL.FieldName End as FieldName,     
Case  When ATL.FieldName = ''Status ID'' Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.OldValue),''-'')    
Else ISNULL(ATL.OldValue,''-'') End as OldValue,   Case When ATL.FieldName = ''Status ID''      Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.NewValue),''-'')     
Else ISNULL(ATL.NewValue,''-'') End as NewValue,   P.PersonID as UserID ,''All Company'' as CompanyName,1 as LanguageID, 0 as CompanyID   
From LocationTypeTable D       
  join AuditLogTable AL on D.LocationTypeID=convert(int,AL.AuditedObjectKeyValue1)       
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID      
INNER JOIN AuditLogLineItemTable ATL ON AL.AuditLogID=ATL.AuditLogID       
join PersonTable P on P.PersonID=AT.UserID    
where AL.EntityName=''LocationTypeTable'' and AL.ActionType=2) At where 1=1')
End
Go 
if not exists(select ActionName from EntityActionTable where ActionName='LocationTypeDeletion')
Begin
Insert into EntityActionTable(TemplateName,ActionName,ActionQuery)
values('LocationTypeDelete','LocationTypeDeletion','select AT.* from (Select D.LocationTypeCode as Code,D.LocationTypeName as Name ,''LocationType Code'' as CodeTitle ,''LocationType Name'' as NameTitle,   
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType     when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''     end as ActionType,
P.PersonFirstName+'' ''+P.PersonLastName as UserName,''LocationType Deletion'' AS ActionName,''-'' AS FieldName,''-'' AS OldValue,''-'' AS NewValue,P.PersonID as UserID,
''All Company'' as CompanyName,1 as LanguageID, 0 as CompanyID  From LocationTypeTable D    
join AuditLogTable AL on D.LocationTypeID=convert(int,AL.AuditedObjectKeyValue1)  
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID   
join PersonTable P on P.PersonID=AT.UserID   where AL.EntityName=''LocationTypeTable'' and AL.ActionType=3)AT where 1=1')
End
Go 

update EntitySingleActionTable set EntityQuery='Select AT.* from(   Select D.DepartmentCode as Code,D.DepartmentName as Name ,''Department Code'' as CodeTitle ,''Department Name'' as NameTitle,   
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE '''' end as ActionType, 
P.PersonFirstName+'' ''+P.PersonLastName as UserName,   Case When ATL.FieldName = ''Status ID'' Then ''Status''   Else ATL.FieldName End as FieldName, 
Case  When ATL.FieldName = ''Status ID''  Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.OldValue),''-'')  
Else ISNULL(ATL.OldValue,''-'') End as OldValue,     
Case When ATL.FieldName = ''Status ID'' Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.NewValue),''-'')    
Else ISNULL(ATL.NewValue,''-'') End as NewValue,   P.PersonID ,AL.AuditedObjectKeyValue1,Al.AuditedObjectKeyValue2,1 as LanguageID  
From DepartmentTable D  
 join AuditLogTable AL  on D.DepartmentID  =convert(int,AL.AuditedObjectKeyValue1) 
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID      
INNER JOIN AuditLogLineItemTable ATL ON AL.AuditLogID=ATL.AuditLogID 
join PersonTable P on P.PersonID=AT.UserID  where Al.EntityName=''DepartmentTable''  
) AT where 1=1'
where EntityName='DepartmentTable'
go 
update EntitySingleActionTable set EntityQuery='select AT.* from (  Select D.SectionCode as Code,D.SectionName as Name ,''Section Code'' as CodeTitle ,''Section Name'' as NameTitle,   
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''end as ActionType, 
P.PersonFirstName+'' ''+P.PersonLastName as UserName,  
Case When ATL.FieldName = ''Status ID'' Then ''Status'' when ATL.FieldName = ''Department ID'' Then ''Department Description''  Else ATL.FieldName End as FieldName, 
Case  When ATL.FieldName = ''Status ID''  Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.OldValue),''-'')   
when  ATL.FieldName = ''Department ID'' Then ISNULL((select MT.DepartmentName from DepartmentTable MT where MT.DepartmentID=ATL.OldValue),''-'')   Else ISNULL(ATL.OldValue,''-'') End as OldValue,     
Case When ATL.FieldName = ''Status ID'' Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.NewValue),''-'')     
when  ATL.FieldName = ''Department ID'' Then ISNULL((select MT.DepartmentName from DepartmentTable MT   where MT.DepartmentID=ATL.NewValue),''-'')   Else ISNULL(ATL.NewValue,''-'') End as NewValue,  
P.PersonID ,AL.AuditedObjectKeyValue1,Al.AuditedObjectKeyValue2,1 as LanguageID  From SectionTable D  
 join AuditLogTable AL  on D.SectionID  =convert(int,AL.AuditedObjectKeyValue1)  
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID      
INNER JOIN AuditLogLineItemTable ATL ON AL.AuditLogID=ATL.AuditLogID  join PersonTable P on P.PersonID=AT.UserID  
where Al.EntityName=''SectionTable''  )AT where 1=1'
where EntityName='SectionTable'
go 

update EntitySingleActionTable set EntityQuery='select AT.* from (  Select D.Partycode as Code,D.PartyName as Name ,''Party Code'' as CodeTitle ,''Party Name'' as NameTitle, 
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE '''' end as ActionType, 
P.PersonFirstName+'' ''+P.PersonLastName as UserName,   Case When ATL.FieldName = ''Status ID'' Then ''Status''  Else ATL.FieldName End as FieldName, 
Case  When ATL.FieldName = ''Status ID''  Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.OldValue),''-'')    Else ISNULL(ATL.OldValue,''-'') End as OldValue,    
Case When ATL.FieldName = ''Status ID'' Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.NewValue),''-'')      Else ISNULL(ATL.NewValue,''-'') End as NewValue,  
P.PersonID ,AL.AuditedObjectKeyValue1,Al.AuditedObjectKeyValue2,1 as LanguageID  From PartyTable D 
join AuditLogTable AL  on D.PartyID  =convert(int,AL.AuditedObjectKeyValue1)  join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID 
INNER JOIN AuditLogLineItemTable ATL ON AL.AuditLogID=ATL.AuditLogID 
join PersonTable P on P.PersonID=AT.UserID  where Al.EntityName=''PartyTable'' 
)AT where 1=1'
where EntityName='SupplierTable'
go 

select * from EntitySingleActionTable

update EntitySingleActionTable set EntityQuery='select AT.* from (  Select D.AssetConditionCode as Code,D.AssetConditionName as Name ,''Asset Condition Code'' as CodeTitle ,''Asset Condition'' as NameTitle,   
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''  end as ActionType,
P.PersonFirstName+'' ''+P.PersonLastName as UserName,   Case When ATL.FieldName = ''Status ID'' Then ''Status''  Else ATL.FieldName End as FieldName,  
Case  When ATL.FieldName = ''Status ID''  Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.OldValue),''-'')    Else ISNULL(ATL.OldValue,''-'') End as OldValue,    
Case When ATL.FieldName = ''Status ID'' Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.NewValue),''-'')    
Else ISNULL(ATL.NewValue,''-'') End as NewValue,   P.PersonID ,AL.AuditedObjectKeyValue1,Al.AuditedObjectKeyValue2,1 as LanguageID  
From AssetconditionTable D  
join AuditLogTable AL  on D.AssetConditionID  =convert(int,AL.AuditedObjectKeyValue1)  
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID      
JOIN AuditLogLineItemTable ATL ON AL.AuditLogID=ATL.AuditLogID  
join PersonTable P on P.PersonID=AT.UserID  where Al.EntityName=''AssetConditionTable''  
)AT where 1=1'
where EntityName='AssetConditionTable'
go 


update EntitySingleActionTable set EntityQuery='select AT.* from (  Select D.TransferTypeCode as Code,D.TransferTypeName as Name ,''Transfer Type Code'' as CodeTitle ,''Transfer Type'' as NameTitle, 
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE '''' end as ActionType,  
P.PersonFirstName+'' ''+P.PersonLastName as UserName,   Case When ATL.FieldName = ''Status ID'' Then ''Status''  Else ATL.FieldName End as FieldName,  
Case  When ATL.FieldName = ''Status ID''  Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.OldValue),''-'')    Else ISNULL(ATL.OldValue,''-'') End as OldValue, 
Case When ATL.FieldName = ''Status ID'' Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.NewValue),''-'')      
Else ISNULL(ATL.NewValue,''-'') End as NewValue,   P.PersonID ,AL.AuditedObjectKeyValue1,Al.AuditedObjectKeyValue2,1 as LanguageID  
From TransferTypeTable D  
join AuditLogTable AL  on D.TransferTypeID  =convert(int,AL.AuditedObjectKeyValue1)  
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID      
JOIN AuditLogLineItemTable ATL ON AL.AuditLogID=ATL.AuditLogID  
join PersonTable P on P.PersonID=AT.UserID  where Al.EntityName=''TransferTypeTable'' 
)AT where 1=1'
where EntityName='TransferTypeTable'
go 
select * from EntitySingleActionTable

update EntitySingleActionTable set EntityQuery='select AT.* from (  Select D.LocationCode as Code,D.LocationName as Name ,''Location Code'' as CodeTitle ,''Location Name'' as NameTitle,   
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''  end as ActionType,  
P.PersonFirstName+'' ''+P.PersonLastName as UserName,   Case When ATL.FieldName = ''Status ID'' Then ''Status'' when ATL.FieldName = ''Parent Location ID'' Then ''Parent Location Name''  Else ATL.FieldName End as FieldName, 
Case  When ATL.FieldName = ''Status ID''  Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.OldValue),''-'')   
when  ATL.FieldName = ''Parent Location ID'' Then ISNULL((select MT.LocationName 
from LocationTable MT where MT.LocationID=ATL.OldValue),''-'')   Else ISNULL(ATL.OldValue,''-'') End as OldValue,     
Case When ATL.FieldName = ''Status ID'' Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.NewValue),''-'')     
when  ATL.FieldName = ''Parent Location ID'' Then ISNULL((select MT.LocationName from LocationTable MT 
 where MT.LocationID=ATL.NewValue),''-'')   Else ISNULL(ATL.NewValue,''-'') End as NewValue,  
P.PersonID ,AL.AuditedObjectKeyValue1,Al.AuditedObjectKeyValue2,1 as LanguageID  
From LocationTable D  
 join AuditLogTable AL  on D.LocationID  =convert(int,AL.AuditedObjectKeyValue1)  
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID      
INNER JOIN AuditLogLineItemTable ATL ON AL.AuditLogID=ATL.AuditLogID  join PersonTable P on P.PersonID=AT.UserID  
where Al.EntityName=''LocationTable'' )AT where 1=1'
where EntityName='LocationTable'
go 


update EntitySingleActionTable set EntityQuery='select AT.* from (  Select D.CategoryCode as Code,D.CategoryName as Name ,''Category Code'' as CodeTitle ,''Category Name'' as NameTitle, 
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''  end as ActionType, 
P.PersonFirstName+'' ''+P.PersonLastName as UserName,   Case When ATL.FieldName = ''Status ID'' Then ''Status'' when ATL.FieldName = ''Parent Category ID'' Then ''Parent Category Name''  Else ATL.FieldName End as FieldName,  
Case  When ATL.FieldName = ''Status ID''  Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.OldValue),''-'')   
when  ATL.FieldName = ''Parent Category ID'' Then ISNULL((select MT.CategoryName from CategoryTable MT where MT.CategoryID=ATL.OldValue),''-'')   Else ISNULL(ATL.OldValue,''-'') End as OldValue, 
Case When ATL.FieldName = ''Status ID'' Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.NewValue),''-'')    
when  ATL.FieldName = ''Parent Category ID'' Then ISNULL((select MT.CategoryName from CategoryTable MT where MT.CategoryID=ATL.NewValue),''-'')   Else ISNULL(ATL.NewValue,''-'') End as NewValue,  
P.PersonID ,AL.AuditedObjectKeyValue1,Al.AuditedObjectKeyValue2,1 as LanguageID  
From CategoryTable D  
 join AuditLogTable AL  on D.CategoryID  =convert(int,AL.AuditedObjectKeyValue1)  
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID     
INNER JOIN AuditLogLineItemTable ATL ON AL.AuditLogID=ATL.AuditLogID  
join PersonTable P on P.PersonID=AT.UserID  where Al.EntityName=''CategoryTable'' )AT where 1=1'
where EntityName='CategoryTable'
go 
--select * from EntitySingleActionTable

update EntitySingleActionTable set EntityQuery='select AT.* from ( Select D.Productcode as Code,D.ProductName as Name ,  ''Product Code'' as CodeTitle ,''Product Name'' as NameTitle, 
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE '''' end as ActionType,  
P.PersonFirstName+'' ''+P.PersonLastName as UserName, Case When ATL.FieldName = ''Status ID'' Then ''Status'' when ATL.FieldName = ''Category ID'' Then ''Category Name'' Else ATL.FieldName End as FieldName,  
Case When ATL.FieldName = ''Status ID'' Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.OldValue),''-'') 
when ATL.FieldName = ''Category ID''   Then ISNULL((select MT.CategoryName from CategoryTable MT  where MT.CategoryID=ATL.OldValue),''-'') Else ISNULL(ATL.OldValue,''-'') End as OldValue,   
Case When ATL.FieldName = ''Status ID'' Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.NewValue),''-'')   
when ATL.FieldName = ''Category ID'' Then ISNULL((select MT.CategoryName from CategoryTable MT where MT.CategoryID=ATL.NewValue),''-'')   Else ISNULL(ATL.NewValue,''-'') End as NewValue,
P.PersonID ,AL.AuditedObjectKeyValue1,Al.AuditedObjectKeyValue2,1 as LanguageID  
From ProductTable D   
join AuditLogTable AL on D.ProductID =convert(int,AL.AuditedObjectKeyValue1)  
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID   
INNER JOIN AuditLogLineItemTable ATL ON AL.AuditLogID=ATL.AuditLogID   
join PersonTable P on P.PersonID=AT.UserID where Al.EntityName=''ProductTable''   
)AT where 1=1'
where EntityName='ProductTable'
go 
update EntitySingleActionTable set EntityQuery='Select AT.Code,AT.Name,AT.CodeTitle,AT.NameTitle,AT.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,AT.ActionType,AT.UserName,  
AT.FieldName,AT.OldValue,AT.NewValue,  AT. personID,AT.AuditedObjectKeyValue1,AT.AuditedObjectKeyValue2,AT.LanguageID  From     
(  Select D.Barcode as Code,D.AssetDescription as Name ,''Barcode'' as CodeTitle ,''Asset Description'' as NameTitle,  AL.EntityName,
Convert(nvarchar,AT.AuditLogTransactionDateTime,103) as AuditLogTransactionDateTime,AT.AuditLogTransactionID,  
Case AL.ActionType   when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''end as ActionType,P.PersonFirstName+'' ''+P.PersonLastName as UserName,
Case APPROVE.StatusId when 6 then   Case When APPROVELINEITEM.FieldName = ''Status ID'' Then ''Status'' 
When APPROVELINEITEM.FieldName = ''Asset Condition ID'' Then ''Asset Condition''     
When APPROVELINEITEM.FieldName = ''Department ID'' Then ''Department Name'' When APPROVELINEITEM.FieldName = ''Section ID'' Then ''Section Name''  
When APPROVELINEITEM.FieldName = ''Location ID'' Then ''Location name''   When APPROVELINEITEM.FieldName = ''Custodian ID'' Then ''Custodian Name''  
When APPROVELINEITEM.FieldName = ''Category ID'' Then ''Category Name''  When APPROVELINEITEM.FieldName = ''Depreciation Class ID'' Then ''Depreciation Class''    
When APPROVELINEITEM.FieldName = ''Supplier ID'' Then ''Supplier Name'' When APPROVELINEITEM.FieldName = ''Product ID'' Then ''Asset Description''      
When APPROVELINEITEM.FieldName = ''Uploaded Document Path'' Then ''Uploaded Document''   when  APPROVELINEITEM.FieldName = ''Uploaded Image Path''  
then ''Uploaded Image''     when  APPROVELINEITEM.FieldName = ''Network ID''  then ''Owned/ Leased''   Else APPROVELINEITEM.FieldName End    
else  Case When ATL.FieldName = ''Status ID'' Then ''Status'' When ATL.FieldName = ''Asset Condition ID'' Then ''Asset Condition''     
When ATL.FieldName = ''Department ID'' Then ''Department Name'' When ATL.FieldName = ''Section ID'' Then ''Section Name''  
When ATL.FieldName = ''Location ID'' Then ''Location name''   When ATL.FieldName = ''Custodian ID'' Then ''Custodian Name''   
When ATL.FieldName = ''Category ID'' Then ''Category Name''  When ATL.FieldName = ''Depreciation Class ID'' Then ''Depreciation Class''    
When ATL.FieldName = ''Supplier ID'' Then ''Supplier Name'' When APPROVELINEITEM.FieldName = ''Product ID'' Then ''Asset Description''     
When ATL.FieldName = ''Uploaded Document Path'' Then ''Uploaded Document'' when  ATL.FieldName = ''Uploaded Image Path''  then ''Uploaded Image''     
when  APPROVELINEITEM.FieldName = ''Network ID''  then ''Owned/ Leased''   Else ATL.FieldName End End as FieldName,       
Case  When (ATL.FieldName = ''Status ID'' or APPROVELINEITEM.FieldName=''Status ID'') Then ISNULL((Select SST.Status From StatusTable SST Where convert(varchar(100),SST.StatusID) = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.OldValue else  ATL.OldValue end )),''-'')  
When (ATL.FieldName = ''Asset Condition ID'' or APPROVELINEITEM.FieldName=''Asset Condition ID'') Then ISNULL((Select SST.AssetConditionName From AssetConditionTable SST  Where convert(varchar(100),SST.AssetConditionID) = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.OldValue else  ATL.OldValue end )),''-'')  
When (ATL.FieldName = ''Department ID'' or APPROVELINEITEM.FieldName=''Department ID'') Then ISNULL((Select SST.DepartmentName From DepartmentTable SST  Where convert(varchar(100),SST.DepartmentID) = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.OldValue else  ATL.OldValue end )),''-'')    
When (ATL.FieldName = ''Section ID'' or APPROVELINEITEM.FieldName=''Section ID'') Then ISNULL((Select SST.SectionName From SectionTable SST  Where convert(varchar(100),SST.SectionID) = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.OldValue else  ATL.OldValue end )),''-'')    
When (ATL.FieldName = ''Location ID'' or APPROVELINEITEM.FieldName=''Location ID'') Then ISNULL((Select SST.PID4 From LocationHierarchicalView SST Where languageID=L.LanguageID and convert(varchar(100),SST.childID) = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.OldValue else  ATL.OldValue end )),''-'')      When (ATL.FieldName = ''Custodian ID'' or APPROVELINEITEM.FieldName=''Custodian ID'') 
Then ISNULL((Select SST.PersonfirstName From PersonTable SST Where convert(varchar(100),SST.PersonID) = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.OldValue else  ATL.OldValue end )),''-'')        When (ATL.FieldName = ''Category ID'' or APPROVELINEITEM.FieldName=''Category ID'') 
Then ISNULL((Select SST.PID3 From CategoryHierarchicalView SST Where languageID=L.LanguageID and convert(varchar(100),SST.ChildId) = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.OldValue else  ATL.OldValue end )),''-'')        
When (ATL.FieldName = ''Depreciation Class ID'' or APPROVELINEITEM.FieldName=''Depreciation Class ID'')  Then ISNULL((Select SST.ClassName From DepreciationClassTable SST Where convert(varchar(100),SST.DepreciationClassID) = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.OldValue else  ATL.OldValue end )),''-'')    
When (ATL.FieldName = ''Supplier ID'' or APPROVELINEITEM.FieldName=''Supplier ID'') Then ISNULL((Select SST.PartyName From PartyTable SST  Where convert(varchar(100),SST.PartyID) =(Case APPROVE.StatusId when 6 then APPROVELINEITEM.OldValue else  ATL.OldValue end )),''-'')      
When (ATL.FieldName = ''Product ID'' or APPROVELINEITEM.FieldName=''Product ID'') Then ISNULL((Select SST.ProductName From ProductTable SST Where convert(varchar(100),SST.productID) =(Case APPROVE.StatusId when 6 then APPROVELINEITEM.OldValue else  ATL.OldValue end )),''-'')      
When (ATL.FieldName = ''Uploaded Document Path'' or APPROVELINEITEM.FieldName=''Uploaded Document Path'')  then   isnull(Case APPROVE.StatusId when 6 then RIGHT( APPROVELINEITEM.OldValue, CHARINDEX( ''\'', REVERSE( APPROVELINEITEM.OldValue ) + ''\'' ) - 1 )  else  RIGHT( ATL.OldValue, CHARINDEX( ''\'', REVERSE( ATL.OldValue ) + ''\'' ) - 1 )    end,''-'' )  
when (ATL.FieldName = ''Uploaded Image Path'' or APPROVELINEITEM.FieldName=''Uploaded Image Path'') then isnull(Case APPROVE.StatusId when 6 then   RIGHT( APPROVELINEITEM.OldValue, CHARINDEX( ''\'', REVERSE( APPROVELINEITEM.OldValue ) + ''\'' ) - 1 )   else  RIGHT( ATL.OldValue, CHARINDEX( ''\'', REVERSE( ATL.OldValue ) + ''\'' ) - 1 )    end ,''-'')     
Else ISNULL(Case APPROVE.StatusId when 6 then   case when (APPROVELINEITEM.OldValue is null or APPROVELINEITEM.OldValue='''') then ''-'' else APPROVELINEITEM.OldValue end    else  ATL.OldValue end ,''-'')  End as OldValue,      
Case When (ATL.FieldName = ''Status ID'' or APPROVELINEITEM.FieldName=''Status ID'')  Then ISNULL((Select SST.Status From StatusTable SST Where convert(varchar(100),SST.StatusID) = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.NewValue else  ATL.NewValue end )),''-'')     
when (ATL.FieldName = ''Asset Condition ID'' or APPROVELINEITEM.FieldName=''Asset Condition ID'') Then ISNULL((Select SST.AssetConditionName From AssetConditionTable SST Where convert(varchar(100),SST.AssetConditionID) = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.NewValue else  ATL.NewValue end )),''-'')   
When (ATL.FieldName = ''Department ID'' or APPROVELINEITEM.FieldName=''Department ID'') Then ISNULL((Select SST.DepartmentName From DepartmentTable SST  Where convert(varchar(100),SST.DepartmentID) = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.NewValue else  ATL.NewValue end )),''-'')      
When (ATL.FieldName = ''Section ID'' or APPROVELINEITEM.FieldName=''Section ID'') Then ISNULL((Select SST.SectionName From SectionTable SST  Where convert(varchar(100),SST.SectionID) = (Case APPROVE.StatusId when 6 then  APPROVELINEITEM.NewValue else  ATL.NewValue end )),''-'')     
When (ATL.FieldName = ''Location ID'' or APPROVELINEITEM.FieldName=''Location ID'') Then ISNULL((Select SST.PID4 From LocationHierarchicalView SST Where languageID=L.LanguageID and convert(varchar(100),SST.childID) = (Case APPROVE.StatusId  when 6 then APPROVELINEITEM.NewValue else  ATL.NewValue end )),''-'')      
When (ATL.FieldName = ''Custodian ID'' or APPROVELINEITEM.FieldName=''Custodian ID'')  Then ISNULL((Select SST.PersonFirstName From PersonTable SST Where convert(varchar(100),SST.PersonID) = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.NewValue else  ATL.NewValue end )),''-'')  
When (ATL.FieldName = ''Category ID'' or APPROVELINEITEM.FieldName=''Category ID'') Then ISNULL((Select SST.PID3 From CategoryHierarchicalView SST Where languageID=L.LanguageID and convert(varchar(100),SST.ChildId) = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.NewValue else  ATL.NewValue end )),''-'')     
When (ATL.FieldName = ''Depreciation Class ID'' or APPROVELINEITEM.FieldName=''Depreciation Class ID'')Then ISNULL((Select SST.ClassName From DepreciationClassTable SST Where convert(varchar(100),SST.DepreciationClassID) = (Case APPROVE.StatusId when 6 then APPROVELINEITEM.NewValue else  ATL.NewValue end )),''-'')      
When (ATL.FieldName = ''Supplier ID'' or APPROVELINEITEM.FieldName=''Supplier ID'') Then ISNULL((Select SST.PartyName From PartyTable SST Where convert(varchar(100),SST.PartyID) =(Case APPROVE.StatusId when 6 then APPROVELINEITEM.NewValue  else  ATL.NewValue end )),''-'')      
When (ATL.FieldName = ''Product ID'' or APPROVELINEITEM.FieldName=''Product ID'') Then ISNULL((Select SST.ProductName From ProductTable SST Where  convert(varchar(100),SST.productID) =(Case APPROVE.StatusId when 6 then APPROVELINEITEM.NewValue  else  ATL.NewValue end )),''-'')     
When (ATL.FieldName = ''Uploaded Document Path'' or APPROVELINEITEM.FieldName=''Uploaded Document Path'')  then   isnull(Case APPROVE.StatusId when 6 then RIGHT( APPROVELINEITEM.NewValue, CHARINDEX( ''\'', REVERSE( APPROVELINEITEM.NewValue ) + ''\'' ) - 1 )  else  RIGHT( ATL.NewValue, CHARINDEX( ''\'', REVERSE( ATL.NewValue ) + ''\'' ) - 1 )    end,''-'' )    
when (ATL.FieldName = ''Uploaded Image Path'' or APPROVELINEITEM.FieldName=''Uploaded Image Path'') then isnull(Case APPROVE.StatusId when 6 then   RIGHT( APPROVELINEITEM.NewValue, CHARINDEX( ''\'', REVERSE( APPROVELINEITEM.NewValue ) + ''\'' ) - 1 )   else  RIGHT( ATL.NewValue, CHARINDEX( ''\'', REVERSE( ATL.NewValue ) + ''\'' ) - 1 )    end ,''-'')    Else ISNULL(Case APPROVE.StatusId when 6 then APPROVELINEITEM.NewValue else  ATL.NewValue end ,''-'')   End as NewValue,   
P.PersonID ,AL.AuditedObjectKeyValue1,Al.AuditedObjectKeyValue2,L.LanguageID  From AssetTable D    
join LanguageTable L on 1=1   join AuditLogTable AL on D.AssetID=convert(int,AL.AuditedObjectKeyValue1)    
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID    
INNER JOIN AuditLogLineItemTable ATL ON AL.AuditLogID=ATL.AuditLogID    
LEFT OUTER JOIN ApprovalTable APPROVE on AL.AuditedObjectKeyValue1 = APPROVE.ObjectKeyValue   
and APPROVE.StatusID = 6 and APPROVE.ActionType in (2) and   cast((convert(CHAR(11),AT.AuditLogTransactionDateTime,101)) as datetime)=cast(convert(char(11),APPROVE.CreatedDatetime,101) as datetime)  
LEFT OUTER JOIN ApprovalLineItemTable APPROVELINEITEM on APPROVE.ApprovalID = APPROVELINEITEM.ApprovalID  
join PersonTable P on P.PersonID=AT.UserID   where AL.EntityName=''AssetTable''  and AT.url not like ''%AssetMapping%''    
and AT.url not like''%AttachDetachApproval%'' and AT.url not like''%TransferAsset%''  and AT.url not like ''%PartialDisposedAsset%''  
and AT.url not like ''%TransferCategory%'' and ATL.Fieldname!=''Asset Description'') AT   
where AT.FieldName not in (''Asset Approval'',''Mail Alert'') and at.oldValue!=at.NewValue'
where EntityName='AssetTable'
go 

update EntitySingleActionTable set EntityQuery='Select AT.* from(   Select D.ManufacturerCode as Code,D.ManufacturerName as Name ,''Manufacturer Code'' as CodeTitle ,''Manufacturer Name'' as NameTitle,   
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE '''' end as ActionType,  P.PersonFirstName+'' ''+P.PersonLastName as UserName,  
Case When ATL.FieldName = ''Status ID'' Then ''Status''   Else ATL.FieldName End as FieldName,   Case  When ATL.FieldName = ''Status ID''  Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.OldValue),''-'')  Else ISNULL(ATL.OldValue,''-'') End as OldValue,     
Case When ATL.FieldName = ''Status ID'' Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.NewValue),''-'')    Else ISNULL(ATL.NewValue,''-'') End as NewValue,   
P.PersonID ,AL.AuditedObjectKeyValue1,Al.AuditedObjectKeyValue2, 1 as LanguageID  From ManufacturerTable D  
 join AuditLogTable AL  on D.ManufacturerID  =convert(int,AL.AuditedObjectKeyValue1)  
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID     
INNER JOIN AuditLogLineItemTable ATL ON AL.AuditLogID=ATL.AuditLogID  join PersonTable P on P.PersonID=AT.UserID 
where Al.EntityName=''ManufacturerTable''  ) AT where 1=1'
where EntityName='ManufacturerTable'
go 
update EntitySingleActionTable set EntityQuery='Select AT.* from(   Select D.ModelCode as Code,D.ModelName as Name ,''Model Code'' as CodeTitle ,''Model Name'' as NameTitle,  
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE '''' end as ActionType,  P.PersonFirstName+'' ''+P.PersonLastName as UserName, 
Case When ATL.FieldName = ''Status ID'' Then ''Status''   Else ATL.FieldName End as FieldName, 
Case  When ATL.FieldName = ''Status ID''  Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.OldValue),''-'')  Else ISNULL(ATL.OldValue,''-'') End as OldValue,      
Case When ATL.FieldName = ''Status ID'' Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.NewValue),''-'')    
Else ISNULL(ATL.NewValue,''-'') End as NewValue,   P.PersonID ,AL.AuditedObjectKeyValue1,Al.AuditedObjectKeyValue2,1 as LanguageID  From ModelTable D 
join AuditLogTable AL  on D.ModelID  =convert(int,AL.AuditedObjectKeyValue1) 
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID     
INNER JOIN AuditLogLineItemTable ATL ON AL.AuditLogID=ATL.AuditLogID  
join PersonTable P on P.PersonID=AT.UserID  where Al.EntityName=''ModelTable''  ) AT where 1=1'
where EntityName='ModelTable'
go 

update EntitySingleActionTable set EntityQuery='Select AT.* from(    Select D.DesignationCode as Code,D.DesignationName as Name ,''Designation Code'' as CodeTitle ,''Designation Name'' as NameTitle,    
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE ''''  
end as ActionType,   P.PersonFirstName+'' ''+P.PersonLastName as UserName,   Case When ATL.FieldName = ''Status ID'' Then ''Status''   Else ATL.FieldName End as FieldName, 
Case  When ATL.FieldName = ''Status ID''  Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.OldValue),''-'') 
Else ISNULL(ATL.OldValue,''-'') End as OldValue,       
Case When ATL.FieldName = ''Status ID'' Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.NewValue),''-'')  
Else ISNULL(ATL.NewValue,''-'') End as NewValue,    
P.PersonID ,AL.AuditedObjectKeyValue1,Al.AuditedObjectKeyValue2,1 as LanguageID 
From DesignationTable D 
join AuditLogTable AL  on D.DesignationID  =convert(int,AL.AuditedObjectKeyValue1)   join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID      
INNER JOIN AuditLogLineItemTable ATL ON AL.AuditLogID=ATL.AuditLogID  join PersonTable P on P.PersonID=AT.UserID  where Al.EntityName=''DesignationTable'' ) AT where 1=1'
where EntityName='DesignationTable'
go 
if not exists(select EntityName from EntitySingleActionTable where EntityName='ApprovalRoleTable')
Begin
Insert into EntitySingleActionTable(EntityName,EntityQuery,IsFormFound)
values('ApprovalRoleTable','Select AT.* from(   Select D.ApprovalRoleCode as Code,D.ApprovalRoleName as Name ,''ApprovalRole Code'' as CodeTitle ,''ApprovalRole Name'' as NameTitle,   
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE '''' end as ActionType, 
P.PersonFirstName+'' ''+P.PersonLastName as UserName,   Case When ATL.FieldName = ''Status ID'' Then ''Status''   Else ATL.FieldName End as FieldName, 
Case  When ATL.FieldName = ''Status ID''  Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.OldValue),''-'')  
Else ISNULL(ATL.OldValue,''-'') End as OldValue,     
Case When ATL.FieldName = ''Status ID'' Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.NewValue),''-'')    
Else ISNULL(ATL.NewValue,''-'') End as NewValue,   P.PersonID ,AL.AuditedObjectKeyValue1,Al.AuditedObjectKeyValue2,1 as LanguageID  
From ApprovalRoleTable D  
 join AuditLogTable AL  on D.ApprovalRoleID  =convert(int,AL.AuditedObjectKeyValue1) 
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID      
INNER JOIN AuditLogLineItemTable ATL ON AL.AuditLogID=ATL.AuditLogID 
join PersonTable P on P.PersonID=AT.UserID  where Al.EntityName=''ApprovalRoleTable''  
) AT where 1=1',1)
End  
go 

if not exists(select EntityName from EntitySingleActionTable where EntityName='CategoryTypeTable')
Begin
Insert into EntitySingleActionTable(EntityName,EntityQuery,IsFormFound)
values('CategoryTypeTable','Select AT.* from(   Select D.CategoryTypeCode as Code,D.CategoryTypeName as Name ,''CategoryType Code'' as CodeTitle ,''CategoryType Name'' as NameTitle,   
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE '''' end as ActionType, 
P.PersonFirstName+'' ''+P.PersonLastName as UserName,   Case When ATL.FieldName = ''Status ID'' Then ''Status''   Else ATL.FieldName End as FieldName, 
Case  When ATL.FieldName = ''Status ID''  Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.OldValue),''-'')  
Else ISNULL(ATL.OldValue,''-'') End as OldValue,     
Case When ATL.FieldName = ''Status ID'' Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.NewValue),''-'')    
Else ISNULL(ATL.NewValue,''-'') End as NewValue,   P.PersonID ,AL.AuditedObjectKeyValue1,Al.AuditedObjectKeyValue2,1 as LanguageID  
From CategoryTypeTable D  
 join AuditLogTable AL  on D.CategoryTypeID  =convert(int,AL.AuditedObjectKeyValue1) 
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID      
INNER JOIN AuditLogLineItemTable ATL ON AL.AuditLogID=ATL.AuditLogID 
join PersonTable P on P.PersonID=AT.UserID  where Al.EntityName=''CategoryTypeTable''  
) AT where 1=1',1)
End  
go 
if not exists(select EntityName from EntitySingleActionTable where EntityName='LocationTypeTable')
Begin
Insert into EntitySingleActionTable(EntityName,EntityQuery,IsFormFound)
values('LocationTypeTable','Select AT.* from(   Select D.LocationTypeCode as Code,D.LocationTypeName as Name ,''LocationType Code'' as CodeTitle ,''LocationType Name'' as NameTitle,   
AL.EntityName,AT.AuditLogTransactionDateTime,AT.AuditLogTransactionID,Case AL.ActionType when 1 then ''Created'' when 2 then ''Modified'' when 3 then ''Deleted'' ELSE '''' end as ActionType, 
P.PersonFirstName+'' ''+P.PersonLastName as UserName,   Case When ATL.FieldName = ''Status ID'' Then ''Status''   Else ATL.FieldName End as FieldName, 
Case  When ATL.FieldName = ''Status ID''  Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.OldValue),''-'')  
Else ISNULL(ATL.OldValue,''-'') End as OldValue,     
Case When ATL.FieldName = ''Status ID'' Then ISNULL((Select SST.Status From StatusTable SST Where SST.StatusID = ATL.NewValue),''-'')    
Else ISNULL(ATL.NewValue,''-'') End as NewValue,   P.PersonID ,AL.AuditedObjectKeyValue1,Al.AuditedObjectKeyValue2,1 as LanguageID  
From LocationTypeTable D  
 join AuditLogTable AL  on D.LocationTypeID  =convert(int,AL.AuditedObjectKeyValue1) 
join AuditLogTransactionTable AT on AT.AuditLogTransactionID=AL.AuditLogTransactionID      
INNER JOIN AuditLogLineItemTable ATL ON AL.AuditLogID=ATL.AuditLogID 
join PersonTable P on P.PersonID=AT.UserID  where Al.EntityName=''LocationTypeTable''  
) AT where 1=1',1)
End  
go 

if not exists(select ConfiguarationName from ConfigurationTable where ConfiguarationName='IsMandatoryLocationType')
begin
  Insert into ConfigurationTable(ConfiguarationName,ConfiguarationValue,ToolTipName,DataType,DropDownValue,MinValue,MaxValue,
	ConfiguarationType,DisplayOrderID,DefaultValue,DisplayConfiguration,SuffixText,
	CatagoryName,CategoryName)
	values('IsMandatoryLocationType','true','IsMandatoryLocationType','Boolean',NULL,
	NULL,NULL,'C',136,1,1,null,'AssetSettings','AssetSettings')
End 
go 
if not exists(select ConfiguarationName from ConfigurationTable where ConfiguarationName='IsMandatoryCategoryType')
begin
  Insert into ConfigurationTable(ConfiguarationName,ConfiguarationValue,ToolTipName,DataType,DropDownValue,MinValue,MaxValue,
	ConfiguarationType,DisplayOrderID,DefaultValue,DisplayConfiguration,SuffixText,
	CatagoryName,CategoryName)
	values('IsMandatoryCategoryType','true','IsMandatoryCategoryType','Boolean',NULL,
	NULL,NULL,'C',137,1,1,null,'AssetSettings','AssetSettings')
End 
go 


ALTER Procedure [dbo].[IPRC_ExceImportBulkAssets]
(
	@AssetCode nvarchar(100)=Null,
	@Barcode nvarchar(100)=Null,
	@DepartmentCode nvarchar(100)=Null,
	@SectionCode nvarchar(100)=NULL,
	@CustodianCode nvarchar(100)=NULL,
	@ModelCode nvarchar(100)=NULL,
	@PONumber nvarchar(100)=NULL,
	@ReferenceCode nvarchar(100)=null,
	@SerialNo nvarchar(100)=NULL,
	@PurchaseDate nvarchar(100)=NULL,
	@WarrantyExpiryDate nvarchar(100)=NULL,
	@CategoryCode nvarchar(100)=NULL,
	@LocationCode nvarchar(100)=NULL,
	@AssetDescription nvarchar(max)=NULL,
	@AssetConditionCode nvarchar(100)=NULL,
	@PurchasePrice nvarchar(100)=NULL,
	@DeliveryNote nvarchar(100)=null,
	@RFIDTagCode nvarchar(100)=null,
	@SupplierCode nvarchar(100)=null,
	@AssetRemarks nvarchar(max)=null,
	@InvoiceNo nvarchar(100)=null,
	@InvoiceDate nvarchar(100)=null,
	@ManufacturerCode nvarchar(100)=NULL,
	@ComissionDate nvarchar(100)=null,
	@VoucherNo nvarchar(100)=null,
	@Make nvarchar(100)=null,
	@Capacity nvarchar(100)=null,
	@MappedAssetID nvarchar(100)=null,
	@ReceiptNumber nvarchar(100)=null,
	@ImportTypeID int,
	@UserID int,
	@LanguageID int,
	@CompanyID int,	
	@Attribute1 nvarchar(100)=null,
	@Attribute2 nvarchar(100)=null,
	@Attribute3 nvarchar(100)=null,
	@Attribute4 nvarchar(100)=null,
	@Attribute5 nvarchar(100)=null,
	@Attribute6 nvarchar(100)=null,
	@Attribute7 nvarchar(100)=null,
	@Attribute8 nvarchar(100)=null,
	@Attribute9 nvarchar(100)=null,
	@Attribute10 nvarchar(100)=null,
	@Attribute11 nvarchar(100)=null,
	@Attribute12 nvarchar(100)=null,
	@Attribute13 nvarchar(100)=null,
	@Attribute14 nvarchar(100)=null,
	@Attribute15 nvarchar(100)=null,
	@Attribute16 nvarchar(100)=null,
    @QFAssetCode nvarchar(100)=NULL,
	@Attribute17 nvarchar(100)=NULL,
	@Attribute18 nvarchar(100)=NULL,
	@Attribute19 nvarchar(100)=NULL,
	@Attribute20 nvarchar(100)=NULL,
	@Attribute21 nvarchar(100)=NULL,
	@Attribute22 nvarchar(100)=NULL,
	@Attribute23 nvarchar(100)=NULL,
	@Attribute24 nvarchar(100)=NULL,
	@Attribute25 nvarchar(100)=NULL,
	@Attribute26 nvarchar(100)=NULL,
	@Attribute27 nvarchar(100)=NULL,
	@Attribute28 nvarchar(100)=NULL,
	@Attribute29 nvarchar(100)=NULL,
	@Attribute30 nvarchar(100)=NULL,
	@Attribute31 nvarchar(100)=NULL,
	@Attribute32 nvarchar(100)=NULL,
	@Attribute33 nvarchar(100)=NULL,
	@Attribute34 nvarchar(100)=NULL,
	@Attribute35 nvarchar(100)=NULL,
	@Attribute36 nvarchar(100)=NULL,
	@Attribute37 nvarchar(100)=NULL,
	@Attribute38 nvarchar(100)=NULL,
	@Attribute39 nvarchar(100)=NULL,
	@Attribute40 nvarchar(100)=NULL
	
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
if(@PurchasePrice='-')
Begin 
set @PurchasePrice=NULL
End 
 	--Declartion part 
	Declare @DepartmentID int,@SectionID int,@CustodianID int,@SupplierID int,@AssetConditionID int, @CategoryID int, @ManufacturerID int,@ModelID int ,@ProductID int ,@LocationID int  ,@DateFormat bit ,@numberFormat bit 
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
	   SElect @Barcode+'- Manufacturer Code is must be add if update/Create Model Code ' as ReturnMessage
				Return 
	END 
	IF @CustodianCode IS NOT NULL AND (@DepartmentCode IS NULL OR @DepartmentCode='')
	BEGIN 
	   SElect @Barcode+'- Department Code is must be add if Update/Create Custodian Code ' as ReturnMessage
				Return 
	END 
	IF @SectionCode IS NOT NULL AND (@DepartmentCode IS NULL OR @DepartmentCode='')
	BEGIN 
	   SElect @Barcode+'- Department Code is must be add if Update/Create Section Code ' as ReturnMessage
				Return 
	END 
	If ((@ComissionDate is not null and @ComissionDate!=''))
		Begin
		  SET DATEFORMAT dmy;
		  SELECT @DateFormat= ISDATE(@ComissionDate)
		  If @DateFormat=0
		   BEGIN 
		     SElect @Barcode+'- Comission Date is invalid' as ReturnMessage
				Return 
		   END 
		End 
			If ((@WarrantyExpiryDate is not null and @WarrantyExpiryDate!=''))
		Begin
		  SET DATEFORMAT dmy;
		  SELECT @DateFormat= ISDATE(@WarrantyExpiryDate)
		  If @DateFormat=0
		   BEGIN 
		     SElect @Barcode+'- Warranty Expiry Date is invalid' as ReturnMessage
				Return 
		   END 
		End
		--print @PurchasePrice
		If ((@PurchasePrice is not null and @PurchasePrice!=''))
		Begin
		  SET DATEFORMAT dmy;
		  SELECT @numberFormat= isnumeric(@PurchasePrice)
		  If @numberFormat=0
		   BEGIN 
		     SElect @Barcode+'- Purchase Price is invalid' as ReturnMessage
				Return 
		   END
		   Else
			BEGIN
				If convert(decimal(18,5),@PurchasePrice)<0
					BEGIN
						SElect @Barcode+'- Purchase Price should not be negative' as ReturnMessage
						Return	
					END
			END
		End 

		If ((@PurchaseDate is not null and @PurchaseDate!=''))
		Begin
		  SET DATEFORMAT dmy;
		  SELECT @DateFormat= ISDATE(@PurchaseDate)
		  If @DateFormat=0
		   BEGIN 
		     SElect @Barcode+'-  Purchase Date is invalid' as ReturnMessage
				Return 
		   END 
		End 
		If ((@InvoiceDate is not null and @InvoiceDate!=''))
		Begin
		  SET DATEFORMAT dmy;
		  SELECT @DateFormat= ISDATE(@InvoiceDate)
		  If @DateFormat=0
		   BEGIN 
		     SElect @Barcode+'- Invoice Date is invalid' as ReturnMessage
				Return 
		   END 
		End 

		if ((@PurchaseDate is null or @PurchaseDate=' ') and (@WarrantyExpiryDate is not null and  @WarrantyExpiryDate!=''))
					 Begin 
					      SElect 'Without Purchase date warranty expiry month should not be added For'+@Barcode as ReturnMessage
					      Return 
					 End 
					 else if ((@PurchaseDate is not null and @PurchaseDate!='') and (@WarrantyExpiryDate is not null and  @WarrantyExpiryDate!=''))
					 Begin 
					      Declare @res bit 
						  Select @res=ISDATE(@WarrantyExpiryDate)
						  IF @res=1 
						  begin
						   IF CONVERT(DATETIME,@PURCHASEDATE,103)>CONVERT(DATETIME,@WarrantyExpiryDate,103)
						   Begin 
					      SElect 'warranty expiry month should be in greater then Purchase Date  '+@Barcode as ReturnMessage
					      Return
						  End
						  End
						 
					 End 
	Declare @ErrorID int 
	Declare @ErrorMsg nvarchar(max)
	If @ImportTypeID=1
	begin
	    exec Prc_AssetCreationValidation @userID,@CategoryCode,@LocationCode,null,null,null,@DepartmentCode,@SerialNo,@ManufacturerCode,null,'ExcelAssetInsert',@ErrorID,@ErrorMsg
		if @ErrorID>0
		Begin
			 Select @ErrorMsg
			 Return
		End 
		if upper(@BarcodeAutoGenerateEnable)='TRUE' and upper(@IsBarcodeSettingApplyImportAsset)='TRUE'
		begin 
		  set @Barcode='-';
		End 
		if UPPER(@BarcodeEqualAssetCode)='TRUE' and upper(@IsBarcodeSettingApplyImportAsset)='TRUE'
		Begin 
		set @AssetCode=@Barcode
		End 
	end 
	Else 
	Begin 
	  Declare @AssetID int 
	  Select @AssetID=AssetID from AssetTable where barcode=@Barcode
	  exec Prc_AssetModificationValidation @userID,@AssetID,@CategoryCode,@LocationCode,null,null,null,@DepartmentCode,@SerialNo,@ManufacturerCode,null,'ExcelAssetInsert',@ErrorID,@ErrorMsg
		if @ErrorID>0
		Begin
			 Select @ErrorMsg
			 Return
		End 
	End 
	
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
				SELECT @SECTIONID=SECTIONID FROM SECTIONTABLE WHERE SECTIONCODE=@SECTIONCODE AND DEPARTMENTID in (SELECT DEPARTMENTID FROM DEPARTMENTTABLE WHERE DEPARTMENTCODE=@DEPARTMENTCODE and StatusID=1) and StatusID=1
			    
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
			  IF EXISTS(SELECT PERSONCODE FROM PERSONTABLE WHERE PERSONCODE=@CUSTODIANCODE AND (USERTYPEID=2 OR USERTYPEID=3) and StatusID=1)
			  BEGIN
				SELECT @CUSTODIANID=PERSONID FROM PERSONTABLE WHERE PERSONCODE=@CUSTODIANCODE AND  (USERTYPEID=2 OR USERTYPEID=3) and StatusID=1
			  END 
			  ELSE 
			  BEGIN 
			    IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
		   BEGIN 	

			    INSERT INTO User_LoginUserTable(UserName,Password,PasswordSalt,LastActivityDate,LastLoginDate,LastLoggedInDate,ChangePasswordAtNextLogin,IsLockedOut,IsDisabled,IsApproved)
			VALUES(@CUSTODIANCODE ,'Mod6/JMHjHeKXDkUK/zd7PfLlJg=','BZxI8E2lroNt28VMhZsyyaNZha8=',GETDATE(),GETDATE(),GETDATE(),1,0,0,1 )

			INSERT INTO PersonTable(PersonID, PersonFirstName, PersonLastName, PersonCode, AllowLogin, DepartmentID, UserTypeID, StatusID, Culture) 
				VALUES(@@IDENTITY, @CUSTODIANCODE, @CUSTODIANCODE, @CUSTODIANCODE, 0, @DepartmentID, 2, 1, 'en-GB')
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
				SELECT @PRODUCTID=b.PRODUCTID FROM ProductDescriptionTable a join ProductTable b on a.ProductId=B.ProductID WHERE replace(ProductDescription,' ','')=@ProductCode AND LANGUAGEID=1 
				AND CATEGORYID in (SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1) and StatusID=1
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
				   
				 INSERT INTO PRODUCTTABLE(PRODUCTCODE,STATUSID,CATEGORYID,CREATEDBY,CREATEDDATETIME,ProductName)
				VALUES(@ProductCode,1,@CATEGORYID,@USERID,getdate(),@ProductCode)
				 
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
						CATEGORYID=(SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1) and StatusID=1)
			   BEGIN 
					SELECT @MANUFACTURERID =MANUFACTURERID FROM MANUFACTURERTABLE WHERE MANUFACTURERCODE=@MANUFACTURERCODE AND 
						CATEGORYID=(SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1) and StatusID=1
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
			
			 IF EXISTS(SELECT MODELCODE FROM  MODELTABLE WHERE MODELCODE=@MODELCODE and StatusID=1 AND CATEGORYID=(SELECT CATEGORYID FROM CATEGORYTABLE C WHERE C.CATEGORYCODE=@CATEGORYCODE and StatusID=1) 
			    AND MANUFACTURERID=(SELECT MANUFACTURERID FROM MANUFACTURERTABLE WHERE  MANUFACTURERCODE=@MANUFACTURERCODE and CATEGORYID=(SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1)  and StatusID=1))
				BEGIN 
				
				  SELECT @MODELID=MODELID FROM MODELTABLE WHERE CATEGORYID=(SELECT CATEGORYID FROM CATEGORYTABLE C WHERE C.CATEGORYCODE=@CATEGORYCODE and StatusID=1) 
			    AND MANUFACTURERID=(SELECT MANUFACTURERID FROM MANUFACTURERTABLE WHERE  MANUFACTURERCODE=@MANUFACTURERCODE and CATEGORYID=(SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1)  and StatusID=1) and StatusID=1
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
			    AND MANUFACTURERID=(SELECT MANUFACTURERID FROM MANUFACTURERTABLE WHERE  MANUFACTURERCODE=@MANUFACTURERCODE and StatusID=1))
				BEGIN 
				  SELECT @MODELID=MODELID FROM MODELTABLE WHERE  MODELCODE=@MODELCODE  AND 
			     MANUFACTURERID=(SELECT MANUFACTURERID FROM MANUFACTURERTABLE WHERE  MANUFACTURERCODE=@MANUFACTURERCODE and StatusID=1) and StatusID=1
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
			IF @ImportTypeID=1 
			BEGIN 
			 IF UPPER(@LocationMandatory)='TRUE' AND (@LOCATIONID IS NULL OR  @LOCATIONID ='') 
			 BEGIN 
				INSERT INTO @MESSAGETABLE(TEXT)VALUES('lOCATIONCODE MANDATORY DATA FOR'+@Barcode)
			 END 
			ELSE 
			BEGIN
		       If exists(select barcode from assettable where barcode=@barcode )
			   Begin 
			     INSERT INTO @MESSAGETABLE(TEXT)VALUES('Already Exists this barcode :'+@Barcode)
			   End 
			   Else 
			   Begin 
			   print 'Insert'
			      If((@Barcode is not null and @Barcode!='') and (@AssetCode is not null and @AssetCode!='') and (@ProductID is not null and @ProductID!='') and (@CategoryID is not null and @CategoryID!='')
				  and (@ASSETDESCRIPTION is not null and @ASSETDESCRIPTION!=''))
				  Begin 
					INSERT INTO ASSETTABLE (ASSETCODE,BARCODE,DEPARTMENTID,SECTIONID,CUSTODIANID,CATEGORYID,LOCATIONID,PRODUCTID,MANUFACTURERID,MODELID,SUPPLIERID,ASSETCONDITIONID,PurchasePrice,
					PONUMBER,REFERENCECODE,SERIALNO,ASSETDESCRIPTION,DELIVERYNOTE,RFIDTAGCODE,INVOICENO,VOUCHERNO,MAKE,CAPACITY,MAPPEDASSETID,RECEIPTNUMBER,PURCHASEDATE,ComissionDate,WarrantyExpiryDate,
					STATUSID,COMPANYID,CreatedBy,CreatedDateTime,AssetApproval,CreateFromHHT,ERPUpdateType,DepreciationFlag,Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6,Attribute7,Attribute8,Attribute9,Attribute10,Attribute11,Attribute12,
					Attribute13,Attribute14,Attribute15,Attribute16,AssetRemarks,QFAssetCode,A.Attribute17,A.Attribute18,Attribute19,Attribute20,Attribute21,Attribute22,Attribute23,Attribute24,Attribute25,Attribute26,
Attribute27,Attribute28,Attribute29,Attribute30,Attribute31,Attribute32,Attribute33,Attribute34,Attribute35,Attribute36,Attribute37,Attribute38,Attribute39,Attribute40)
					
					VALUES( CASE WHEN UPPER(@BarcodeAutoGenerateEnable)='TRUE' and  UPPER(@IsBarcodeSettingApplyImportAsset)='TRUE' THEN '-' ELSE 
					CASE WHEN  UPPER(@BarcodeEqualAssetCode)='TRUE' and  UPPER(@IsBarcodeSettingApplyImportAsset)='TRUE' THEN @Barcode ELSE @AssetCode END END ,
					CASE WHEN UPPER(@BarcodeAutoGenerateEnable)='TRUE' and  UPPER(@IsBarcodeSettingApplyImportAsset)='TRUE' THEN '-' ELSE @Barcode END
					,@DepartmentID,@SECTIONID,@CUSTODIANID,@CATEGORYID,@LOCATIONID,@PRODUCTID,@MANUFACTURERID,@MODELID,@SUPPLIERID,@ASSETCONDITIONID,case when @PurchasePrice is not null and  @PurchasePrice!=' '  then convert(decimal(18,5),@PurchasePrice) else 0 end ,@PONUMBER,@REFERENCECODE,@SERIALNO,
					@ASSETDESCRIPTION,@DELIVERYNOTE,@RFIDTAGCODE,@INVOICENO,@VOUCHERNO,@MAKE,@CAPACITY,@MAPPEDASSETID,@RECEIPTNUMBER,case when @PURCHASEDATE is not null then CONVERT(DATETIME,@PURCHASEDATE,103) else NULL end ,
					case when @ComissionDate is not null then CONVERT(DATETIME,@ComissionDate,103) else null end , case when @WarrantyExpiryDate is not null then CONVERT(DATETIME,@WarrantyExpiryDate,103) else NULL end ,
					1,@companyID,@userID,getdate(),1,0,0,0,@Attribute1,@Attribute2,@Attribute3,@Attribute4,@Attribute5,
			@Attribute6,@Attribute7,@Attribute8,@Attribute9,@Attribute10,@Attribute11,@Attribute12,
			@Attribute13,@Attribute14,@Attribute15,@Attribute16,@AssetRemarks,@QFAssetCode,@Attribute17,@Attribute18,@Attribute19,@Attribute20,@Attribute21,@Attribute22,@Attribute23,@Attribute24,@Attribute25,@Attribute26,
@Attribute27,@Attribute28,@Attribute29,@Attribute30,@Attribute31,@Attribute32,@Attribute33,@Attribute34,@Attribute35,@Attribute36,@Attribute37,@Attribute38,@Attribute39,@Attribute40)
			End 
			Else 
			Begin 
			  INSERT INTO @MESSAGETABLE(TEXT)VALUES('Mandatory field are passed with empty '+@Barcode)
			End 
			 End 
			END 
			END 
			ELSE 
			BEGIN 
			IF EXISTS (SELECT barcode FROM dbo.AssetTable WHERE Barcode=@Barcode)
			BEGIN
				UPDATE ASSETTABLE SET
				--ASSETCODE=CASE WHEN UPPER(@BarcodeEqualAssetCode)='TRUE' THEN ISNULL(@Barcode,ASSETCODE) ELSE ISNULL(@ASSETCODE,ASSETCODE) END,
				ASSETCODE=CASE WHEN @ASSETCODE is null or @ASSETCODE='' then AssetCode else @ASSETCODE end,
				BARCODE=ISNULL(@Barcode,BARCODE),
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
				PURCHASEDATE=CASE WHEN @PURCHASEDATE IS NULL OR RTRIM(LTRIM(@PURCHASEDATE))='' THEN PURCHASEDATE ELSE CONVERT(DATETIME,@PURCHASEDATE,103)  END,
				ComissionDate=CASE WHEN @ComissionDate IS NULL OR RTRIM(LTRIM(@ComissionDate))='' THEN ComissionDate ELSE CONVERT(DATETIME,@ComissionDate,103) END,
				WarrantyExpiryDate=CASE WHEN @WarrantyExpiryDate IS NULL OR RTRIM(LTRIM(@WarrantyExpiryDate))='' THEN WarrantyExpiryDate ELSE CONVERT(DATETIME,@WarrantyExpiryDate,103) END,			
				Attribute1=CASE WHEN @Attribute1 IS NULL OR RTRIM(LTRIM(@Attribute1))='' THEN Attribute1 ELSE @Attribute1 END,
				Attribute2=CASE WHEN @Attribute2 IS NULL OR RTRIM(LTRIM(@Attribute2))='' THEN Attribute2 ELSE @Attribute2 END,
				Attribute3=CASE WHEN @Attribute3 IS NULL OR RTRIM(LTRIM(@Attribute3))='' THEN Attribute3 ELSE @Attribute3 END,
				Attribute4=CASE WHEN @Attribute4 IS NULL OR RTRIM(LTRIM(@Attribute4))='' THEN Attribute4 ELSE @Attribute4 END,
				Attribute5=CASE WHEN @Attribute5 IS NULL OR RTRIM(LTRIM(@Attribute5))='' THEN Attribute5 ELSE @Attribute5 END,
				Attribute6=CASE WHEN @Attribute6 IS NULL OR RTRIM(LTRIM(@Attribute6))='' THEN Attribute6 ELSE @Attribute6 END,
				Attribute7=CASE WHEN @Attribute7 IS NULL OR RTRIM(LTRIM(@Attribute7))='' THEN Attribute7 ELSE @Attribute7 END,
				Attribute8=CASE WHEN @Attribute8 IS NULL OR RTRIM(LTRIM(@Attribute8))='' THEN Attribute8 ELSE @Attribute8 END,
				Attribute9=CASE WHEN @Attribute9 IS NULL OR RTRIM(LTRIM(@Attribute9))='' THEN Attribute9 ELSE @Attribute9 END,
				Attribute10=CASE WHEN @Attribute10 IS NULL OR RTRIM(LTRIM(@Attribute10))='' THEN Attribute10 ELSE @Attribute10 END,
				Attribute11=CASE WHEN @Attribute11 IS NULL OR RTRIM(LTRIM(@Attribute11))='' THEN Attribute11 ELSE @Attribute11 END,
				Attribute12=CASE WHEN @Attribute12 IS NULL OR RTRIM(LTRIM(@Attribute12))='' THEN Attribute12 ELSE @Attribute12 END,
				Attribute13=CASE WHEN @Attribute13 IS NULL OR RTRIM(LTRIM(@Attribute13))='' THEN Attribute13 ELSE @Attribute13 END,
				Attribute14=CASE WHEN @Attribute14 IS NULL OR RTRIM(LTRIM(@Attribute14))='' THEN Attribute14 ELSE @Attribute14 END,
				Attribute15=CASE WHEN @Attribute15 IS NULL OR RTRIM(LTRIM(@Attribute15))='' THEN Attribute15 ELSE @Attribute15 END,
				Attribute16=CASE WHEN @Attribute16 IS NULL OR RTRIM(LTRIM(@Attribute16))='' THEN Attribute16 ELSE @Attribute16 END,
				AssetRemarks=CASE WHEN @AssetRemarks IS NULL OR RTRIM(LTRIM(@AssetRemarks))='' THEN AssetRemarks ELSE @AssetRemarks END,
				QFAssetCode=CASE WHEN @QFAssetCode IS NULL OR RTRIM(LTRIM(@QFAssetCode))='' THEN QFAssetCode ELSE @QFAssetCode END,
				LastModifiedDateTime=getdate(),
				LastModifiedBy=@UserID ,
				Attribute17=CASE WHEN @Attribute17 IS NULL OR RTRIM(LTRIM(@Attribute17))='' THEN Attribute17 ELSE @Attribute17 END,
				Attribute18=CASE WHEN @Attribute18 IS NULL OR RTRIM(LTRIM(@Attribute18))='' THEN Attribute18 ELSE Attribute18 END,
				Attribute19=CASE WHEN @Attribute19 IS NULL OR RTRIM(LTRIM(@Attribute19))='' THEN Attribute19 ELSE @Attribute19 END,
				Attribute20=CASE WHEN @Attribute20 IS NULL OR RTRIM(LTRIM(@Attribute20))='' THEN Attribute20 ELSE @Attribute20 END,
			    Attribute21=CASE WHEN @Attribute21 IS NULL OR RTRIM(LTRIM(@Attribute21))='' THEN Attribute21 ELSE @Attribute21 END,
				Attribute22=CASE WHEN @Attribute22 IS NULL OR RTRIM(LTRIM(@Attribute22))='' THEN Attribute22 ELSE @Attribute22 END,
				Attribute23=CASE WHEN @Attribute23 IS NULL OR RTRIM(LTRIM(@Attribute23))='' THEN Attribute23 ELSE @Attribute23 END,
				Attribute24=CASE WHEN @Attribute24 IS NULL OR RTRIM(LTRIM(@Attribute24))='' THEN Attribute24 ELSE @Attribute24 END,
				Attribute25=CASE WHEN @Attribute25 IS NULL OR RTRIM(LTRIM(@Attribute25))='' THEN Attribute25 ELSE @Attribute25 END,
				Attribute26=CASE WHEN @Attribute26 IS NULL OR RTRIM(LTRIM(@Attribute26))='' THEN Attribute26 ELSE @Attribute26 END,
				Attribute27=CASE WHEN @Attribute27 IS NULL OR RTRIM(LTRIM(@Attribute27))='' THEN Attribute27 ELSE @Attribute27 END,
				Attribute28=CASE WHEN @Attribute28 IS NULL OR RTRIM(LTRIM(@Attribute28))='' THEN Attribute28 ELSE @Attribute28 END,
				Attribute29=CASE WHEN @Attribute29 IS NULL OR RTRIM(LTRIM(@Attribute29))='' THEN Attribute29 ELSE @Attribute29 END,
				Attribute30=CASE WHEN @Attribute30 IS NULL OR RTRIM(LTRIM(@Attribute30))='' THEN Attribute30 ELSE @Attribute30 END,
				Attribute31=CASE WHEN @Attribute31 IS NULL OR RTRIM(LTRIM(@Attribute31))='' THEN Attribute31 ELSE @Attribute31 END,
				Attribute32=CASE WHEN @Attribute32 IS NULL OR RTRIM(LTRIM(@Attribute32))='' THEN Attribute32 ELSE @Attribute32 END,
				Attribute33=CASE WHEN @Attribute33 IS NULL OR RTRIM(LTRIM(@Attribute33))='' THEN Attribute33 ELSE @Attribute33 END,
				Attribute34=CASE WHEN @Attribute34 IS NULL OR RTRIM(LTRIM(@Attribute34))='' THEN Attribute34 ELSE @Attribute34 END,
				Attribute35=CASE WHEN @Attribute35 IS NULL OR RTRIM(LTRIM(@Attribute35))='' THEN Attribute35 ELSE @Attribute35 END,
				Attribute36=CASE WHEN @Attribute36 IS NULL OR RTRIM(LTRIM(@Attribute36))='' THEN Attribute36 ELSE @Attribute36 END,
				Attribute37=CASE WHEN @Attribute37 IS NULL OR RTRIM(LTRIM(@Attribute37))='' THEN Attribute37 ELSE @Attribute37 END,
				Attribute38=CASE WHEN @Attribute38 IS NULL OR RTRIM(LTRIM(@Attribute38))='' THEN Attribute38 ELSE @Attribute38 END,
				Attribute39=CASE WHEN @Attribute39 IS NULL OR RTRIM(LTRIM(@Attribute39))='' THEN Attribute39 ELSE @Attribute39 END,
				Attribute40=CASE WHEN @Attribute40 IS NULL OR RTRIM(LTRIM(@Attribute40))='' THEN Attribute40 ELSE @Attribute40 END
				 WHERE BARCODE=@BARCODE 
			 END
			 ELSE
			 BEGIN 
				 INSERT INTO @MESSAGETABLE(TEXT)VALUES(@Barcode + '- This barcode not available in Assettable')
			 END 
			END 
			--select * from @MESSAGETABLE

			Select @ReturnMessage = COALESCE(@ReturnMessage + ', ' + Text, Text)  from @MESSAGETABLE
			select @ReturnMessage as ReturnMessage
        
			 
End
go 

ALTER Procedure [dbo].[IPRC_AMSExceImportBulkAssets]
(
	@AssetCode nvarchar(100)=NULL,
	@Barcode nvarchar(100)=NULL,
	@DepartmentCode nvarchar(100)=NULL,
	@SectionCode nvarchar(100)=NULL,
	@CustodianCode nvarchar(100)=NULL,
	@ModelCode nvarchar(100)=NULL,
	@PONumber nvarchar(100)=NULL,
	@ReferenceCode nvarchar(100)=null,
	@SerialNo nvarchar(100)=NULL,
	@PurchaseDate nvarchar(100)=NULL,
	@WarrantyExpiryDate nvarchar(100)=NULL,
	@CategoryCode nvarchar(100)=NULL,
	@LocationCode nvarchar(100)=NULL,
	@AssetDescription nvarchar(max)=NULL,
	@AssetConditionCode nvarchar(100)=NULL,
	@PurchasePrice nvarchar(100)=NULL,
	@DeliveryNote nvarchar(100)=null,
	@RFIDTagCode nvarchar(100)=null,
	@SupplierCode nvarchar(100)=NULL,
	@AssetRemarks nvarchar(max)=null,
	@InvoiceNo nvarchar(100)=null,
	@InvoiceDate nvarchar(100)=null,
	@ManufacturerCode nvarchar(100)=NULL,
	@ComissionDate nvarchar(100)=null,
	@VoucherNo nvarchar(100)=null,
	@Make nvarchar(100)=null,
	@Capacity nvarchar(100)=null,
	@MappedAssetID nvarchar(100)=null,
	@ReceiptNumber nvarchar(100)=null,
	@ImportTypeID int,
	@UserID int,
	@LanguageID int,
	@CompanyID int,	
	@Attribute1 nvarchar(100)=null,
	@Attribute2 nvarchar(100)=null,
	@Attribute3 nvarchar(100)=null,
	@Attribute4 nvarchar(100)=null,
	@Attribute5 nvarchar(100)=null,
	@Attribute6 nvarchar(100)=null,
	@Attribute7 nvarchar(100)=null,
	@Attribute8 nvarchar(100)=null,
	@Attribute9 nvarchar(100)=null,
	@Attribute10 nvarchar(100)=null,
	@Attribute11 nvarchar(100)=null,
	@Attribute12 nvarchar(100)=null,
	@Attribute13 nvarchar(100)=null,
	@Attribute14 nvarchar(100)=null,
	@Attribute15 nvarchar(100)=null,
	@Attribute16 nvarchar(100)=null,
    @QFAssetCode nvarchar(100)=NULL,
	@Attribute17 nvarchar(100)=NULL,
	@Attribute18 nvarchar(100)=NULL,
	@Attribute19 nvarchar(100)=NULL,
	@Attribute20 nvarchar(100)=NULL,
	@Attribute21 nvarchar(100)=NULL,
	@Attribute22 nvarchar(100)=NULL,
	@Attribute23 nvarchar(100)=NULL,
	@Attribute24 nvarchar(100)=NULL,
	@Attribute25 nvarchar(100)=NULL,
	@Attribute26 nvarchar(100)=NULL,
	@Attribute27 nvarchar(100)=NULL,
	@Attribute28 nvarchar(100)=NULL,
	@Attribute29 nvarchar(100)=NULL,
	@Attribute30 nvarchar(100)=NULL,
	@Attribute31 nvarchar(100)=NULL,
	@Attribute32 nvarchar(100)=NULL,
	@Attribute33 nvarchar(100)=NULL,
	@Attribute34 nvarchar(100)=NULL,
	@Attribute35 nvarchar(100)=NULL,
	@Attribute36 nvarchar(100)=NULL,
	@Attribute37 nvarchar(100)=NULL,
	@Attribute38 nvarchar(100)=NULL,
	@Attribute39 nvarchar(100)=NULL,
	@Attribute40 nvarchar(100)=NULL
	
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
if(@PurchasePrice='-')
Begin 
set @PurchasePrice=NULL
End 
 	--Declartion part 
	Declare @DepartmentID int,@SectionID int,@CustodianID int,@SupplierID int,@AssetConditionID int, @CategoryID int, @ManufacturerID int,@ModelID int ,@ProductID int ,@LocationID int  ,@DateFormat bit ,@numberFormat bit 
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
	   SElect @Barcode+'- Manufacturer Code is must be add if update/Create Model Code ' as ReturnMessage
				Return 
	END 
	IF @CustodianCode IS NOT NULL AND (@DepartmentCode IS NULL OR @DepartmentCode='')
	BEGIN 
	   SElect @Barcode+'- Department Code is must be add if Update/Create Custodian Code ' as ReturnMessage
				Return 
	END 
	IF @SectionCode IS NOT NULL AND (@DepartmentCode IS NULL OR @DepartmentCode='')
	BEGIN 
	   SElect @Barcode+'- Department Code is must be add if Update/Create Section Code ' as ReturnMessage
				Return 
	END 
	If ((@ComissionDate is not null and @ComissionDate!=''))
		Begin
		  SET DATEFORMAT dmy;
		  SELECT @DateFormat= ISDATE(@ComissionDate)
		  If @DateFormat=0
		   BEGIN 
		     SElect @Barcode+'- Comission Date is invalid' as ReturnMessage
				Return 
		   END 
		End 
			If ((@WarrantyExpiryDate is not null and @WarrantyExpiryDate!=''))
		Begin
		  SET DATEFORMAT dmy;
		  SELECT @DateFormat= ISDATE(@WarrantyExpiryDate)
		  If @DateFormat=0
		   BEGIN 
		     SElect @Barcode+'- Warranty Expiry Date is invalid' as ReturnMessage
				Return 
		   END 
		End
		--print @PurchasePrice
		If ((@PurchasePrice is not null and @PurchasePrice!=''))
		Begin
		  SET DATEFORMAT dmy;
		  SELECT @numberFormat= isnumeric(@PurchasePrice)
		  If @numberFormat=0
		   BEGIN 
		     SElect @Barcode+'- Purchase Price is invalid' as ReturnMessage
				Return 
		   END
		   Else
			BEGIN
				If convert(decimal(18,5),@PurchasePrice)<0
					BEGIN
						SElect @Barcode+'- Purchase Price should not be negative' as ReturnMessage
						Return	
					END
			END
		End 

		If ((@PurchaseDate is not null and @PurchaseDate!=''))
		Begin
		  SET DATEFORMAT dmy;
		  SELECT @DateFormat= ISDATE(@PurchaseDate)
		  If @DateFormat=0
		   BEGIN 
		     SElect @Barcode+'-  Purchase Date is invalid' as ReturnMessage
				Return 
		   END 
		End 
		If ((@InvoiceDate is not null and @InvoiceDate!=''))
		Begin
		  SET DATEFORMAT dmy;
		  SELECT @DateFormat= ISDATE(@InvoiceDate)
		  If @DateFormat=0
		   BEGIN 
		     SElect @Barcode+'- Invoice Date is invalid' as ReturnMessage
				Return 
		   END 
		End 

		if ((@PurchaseDate is null or @PurchaseDate=' ') and (@WarrantyExpiryDate is not null and  @WarrantyExpiryDate!=''))
					 Begin 
					      SElect 'Without Purchase date warranty expiry month should not be added For'+@Barcode as ReturnMessage
					      Return 
					 End 
					 else if ((@PurchaseDate is not null and @PurchaseDate!='') and (@WarrantyExpiryDate is not null and  @WarrantyExpiryDate!=''))
					 Begin 
					      Declare @res bit 
						  Select @res=ISDATE(@WarrantyExpiryDate)
						  IF @res=1 
						  begin
						   IF CONVERT(DATETIME,@PURCHASEDATE,103)>CONVERT(DATETIME,@WarrantyExpiryDate,103)
						   Begin 
					      SElect 'warranty expiry month should be in greater then Purchase Date  '+@Barcode as ReturnMessage
					      Return
						  End
						  End
						 
					 End 
	Declare @ErrorID int 
	Declare @ErrorMsg nvarchar(max)
	If @ImportTypeID=1
	begin
		 exec Prc_AssetCreationValidation @userID,@CategoryCode,@LocationCode,null,null,null,@DepartmentCode,@SerialNo,@ManufacturerCode,null,'ExcelAssetInsert',@ErrorID,@ErrorMsg
		if @ErrorID>0
		Begin
			 Select @ErrorMsg
			 Return
		End 
		if upper(@BarcodeAutoGenerateEnable)='TRUE' and upper(@IsBarcodeSettingApplyImportAsset)='TRUE'
		begin 
		  set @Barcode='-';
		End 
		if UPPER(@BarcodeEqualAssetCode)='TRUE' and upper(@IsBarcodeSettingApplyImportAsset)='TRUE'
		Begin 
		set @AssetCode=@Barcode
		End 
	end 
	Else 
	Begin 
	  Declare @AssetID int 
	  Select @AssetID=AssetID from AssetTable where barcode=@Barcode
	  exec Prc_AssetModificationValidation @userID,@AssetID,@CategoryCode,@LocationCode,null,null,null,@DepartmentCode,@SerialNo,@ManufacturerCode,null,'ExcelAssetInsert',@ErrorID,@ErrorMsg
		if @ErrorID>0
		Begin
			 Select @ErrorMsg
			 Return
		End 
	End 
	
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
				SELECT @SECTIONID=SECTIONID FROM SECTIONTABLE WHERE SECTIONCODE=@SECTIONCODE AND DEPARTMENTID in (SELECT DEPARTMENTID FROM DEPARTMENTTABLE WHERE DEPARTMENTCODE=@DEPARTMENTCODE and StatusID=1) and StatusID=1
			    
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
			  IF EXISTS(SELECT PERSONCODE FROM PERSONTABLE WHERE PERSONCODE=@CUSTODIANCODE AND (USERTYPEID=2 OR USERTYPEID=3) and StatusID=1)
			  BEGIN
				SELECT @CUSTODIANID=PERSONID FROM PERSONTABLE WHERE PERSONCODE=@CUSTODIANCODE AND  (USERTYPEID=2 OR USERTYPEID=3) and StatusID=1
			  END 
			  ELSE 
			  BEGIN 
			    IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
		   BEGIN 	

			    INSERT INTO User_LoginUserTable(UserName,Password,PasswordSalt,LastActivityDate,LastLoginDate,LastLoggedInDate,ChangePasswordAtNextLogin,IsLockedOut,IsDisabled,IsApproved)
			VALUES(@CUSTODIANCODE ,'Mod6/JMHjHeKXDkUK/zd7PfLlJg=','BZxI8E2lroNt28VMhZsyyaNZha8=',GETDATE(),GETDATE(),GETDATE(),1,0,0,1 )

			INSERT INTO PersonTable(PersonID, PersonFirstName, PersonLastName, PersonCode, AllowLogin, DepartmentID, UserTypeID, StatusID, Culture) 
				VALUES(@@IDENTITY, @CUSTODIANCODE, @CUSTODIANCODE, @CUSTODIANCODE, 0, @DepartmentID, 2, 1, 'en-GB')
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
			 IF EXISTS(SELECT PartyCode FROM PartyTable WHERE PartyCode=@SUPPLIERCODE and StatusID=1)
			 BEGIN 
				SELECT @SUPPLIERID =partyID FROM PartyTable WHERE PartyCode=@SUPPLIERCODE and StatusID=1 and PartyTypeID=1
			 END 
			 ELSE 
			 BEGIN 
			   IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
		   BEGIN 	
				INSERT INTO PartyTable(PartyCode,STATUSID,CREATEDBY,CREATEDDATETIME,PartyName,PartyTypeID)
				VALUES(@SUPPLIERCODE,1,@USERID,GETDATE(),@SupplierCode,(select PartyTypeID from PartyTypeTable where PartyType='Supplier'))
				
				SET @SUPPLIERID = SCOPE_IDENTITY()
				--INSERT INTO part(SUPPLIERID,SUPPLIERDESCRIPTION,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
				--Select @SUPPLIERID,@SUPPLIERCODE,LanguageID,@USERID,GETDATE() from LanguageTable
				END 
			ELSE 
			BEGIN
			   INSERT INTO @MESSAGETABLE(TEXT)VALUES('SupplierCode Cant allow Create Data')
			END 
				END
			 END 
	 IF(@ASSETDESCRIPTION is not NULL and @ASSETDESCRIPTION!='')                                                                                                                                                                                                                                                            
			BEGIN 
			 IF EXISTS(SELECT Productdescription FROM ProductDescriptionTable a join ProductTable b on a.ProductId=B.ProductID WHERE replace(ProductDescription,' ','')=@ProductCode AND LANGUAGEID=1 
				AND CATEGORYID in (SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1) and StatusID=1)
			 BEGIN
			 --print 'ProductID'
				SELECT @PRODUCTID=b.PRODUCTID FROM ProductDescriptionTable a join ProductTable b on a.ProductId=B.ProductID WHERE replace(ProductDescription,' ','')=@ProductCode AND LANGUAGEID=1 
				AND CATEGORYID in (SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1) and StatusID=1
				--FROM PRODUCTVIEW WHERE replace(productName,' ','')=@ProductCode AND LANGUAGEID=1 and StatusID=1
				--AND CATEGORYID in (SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1)
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
				   
				 INSERT INTO PRODUCTTABLE(PRODUCTCODE,STATUSID,CATEGORYID,CREATEDBY,CREATEDDATETIME,ProductName)
				VALUES(@ProductCode,1,@CATEGORYID,@USERID,getdate(),@ProductCode)
				 
				 SET @ProductID = SCOPE_IDENTITY()
				 
				 INSERT INTO PRODUCTDESCRIPTIONTABLE (PRODUCTDESCRIPTION,PRODUCTID,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
				 Select @ProductCode,@ProductID,LanguageID,@USERID,getdate() from LanguageTable
				

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
						CATEGORYID=(SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1) and StatusID=1)
			   BEGIN 
					SELECT @MANUFACTURERID =MANUFACTURERID FROM MANUFACTURERTABLE WHERE MANUFACTURERCODE=@MANUFACTURERCODE AND 
						CATEGORYID=(SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1) and StatusID=1
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
			
			 IF EXISTS(SELECT MODELCODE FROM  MODELTABLE WHERE MODELCODE=@MODELCODE and StatusID=1 AND CATEGORYID=(SELECT CATEGORYID FROM CATEGORYTABLE C WHERE C.CATEGORYCODE=@CATEGORYCODE and StatusID=1) 
			    AND MANUFACTURERID=(SELECT MANUFACTURERID FROM MANUFACTURERTABLE WHERE  MANUFACTURERCODE=@MANUFACTURERCODE and CATEGORYID=(SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1)  and StatusID=1))
				BEGIN 
				
				  SELECT @MODELID=MODELID FROM MODELTABLE WHERE CATEGORYID=(SELECT CATEGORYID FROM CATEGORYTABLE C WHERE C.CATEGORYCODE=@CATEGORYCODE and StatusID=1) 
			    AND MANUFACTURERID=(SELECT MANUFACTURERID FROM MANUFACTURERTABLE WHERE  MANUFACTURERCODE=@MANUFACTURERCODE and CATEGORYID=(SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1)  and StatusID=1) and StatusID=1
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
			    AND MANUFACTURERID=(SELECT MANUFACTURERID FROM MANUFACTURERTABLE WHERE  MANUFACTURERCODE=@MANUFACTURERCODE and StatusID=1))
				BEGIN 
				  SELECT @MODELID=MODELID FROM MODELTABLE WHERE  MODELCODE=@MODELCODE  AND 
			     MANUFACTURERID=(SELECT MANUFACTURERID FROM MANUFACTURERTABLE WHERE  MANUFACTURERCODE=@MANUFACTURERCODE and StatusID=1) and StatusID=1
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
			IF @ImportTypeID=1 
			BEGIN 
			 IF UPPER(@LocationMandatory)='TRUE' AND (@LOCATIONID IS NULL OR  @LOCATIONID ='') 
			 BEGIN 
				INSERT INTO @MESSAGETABLE(TEXT)VALUES('lOCATIONCODE MANDATORY DATA FOR'+@Barcode)
			 END 
			ELSE 
			BEGIN
		       If exists(select barcode from assettable where barcode=@barcode )
			   Begin 
			     INSERT INTO @MESSAGETABLE(TEXT)VALUES('Already Exists this barcode :'+@Barcode)
			   End 
			   Else 
			   Begin 
			   print 'Insert'
			      If((@Barcode is not null and @Barcode!='') and (@AssetCode is not null and @AssetCode!='') and (@ProductID is not null and @ProductID!='') and (@CategoryID is not null and @CategoryID!='')
				  and (@ASSETDESCRIPTION is not null and @ASSETDESCRIPTION!=''))

				  Begin 
					INSERT INTO ASSETTABLE (ASSETCODE,BARCODE,DEPARTMENTID,SECTIONID,CUSTODIANID,CATEGORYID,LOCATIONID,PRODUCTID,MANUFACTURERID,MODELID,SUPPLIERID,ASSETCONDITIONID,PurchasePrice,
					PONUMBER,REFERENCECODE,SERIALNO,ASSETDESCRIPTION,DELIVERYNOTE,RFIDTAGCODE,INVOICENO,VOUCHERNO,MAKE,CAPACITY,MAPPEDASSETID,RECEIPTNUMBER,PURCHASEDATE,ComissionDate,WarrantyExpiryDate,
					STATUSID,COMPANYID,CreatedBy,CreatedDateTime,AssetApproval,CreateFromHHT,ERPUpdateType,DepreciationFlag,Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6,Attribute7,Attribute8,Attribute9,Attribute10,Attribute11,Attribute12,
					Attribute13,Attribute14,Attribute15,Attribute16,AssetRemarks,QFAssetCode,A.Attribute17,A.Attribute18,Attribute19,Attribute20,Attribute21,Attribute22,Attribute23,Attribute24,Attribute25,Attribute26,
Attribute27,Attribute28,Attribute29,Attribute30,Attribute31,Attribute32,Attribute33,Attribute34,Attribute35,Attribute36,Attribute37,Attribute38,Attribute39,Attribute40)
					
					VALUES( CASE WHEN UPPER(@BarcodeAutoGenerateEnable)='TRUE' and  UPPER(@IsBarcodeSettingApplyImportAsset)='TRUE' THEN '-' ELSE 
					CASE WHEN  UPPER(@BarcodeEqualAssetCode)='TRUE' and  UPPER(@IsBarcodeSettingApplyImportAsset)='TRUE' THEN @Barcode ELSE @AssetCode END END ,
					CASE WHEN UPPER(@BarcodeAutoGenerateEnable)='TRUE' and  UPPER(@IsBarcodeSettingApplyImportAsset)='TRUE' THEN '-' ELSE @Barcode END
					,@DepartmentID,@SECTIONID,@CUSTODIANID,@CATEGORYID,@LOCATIONID,@PRODUCTID,@MANUFACTURERID,@MODELID,@SUPPLIERID,@ASSETCONDITIONID,case when @PurchasePrice is not null and  @PurchasePrice!=' '  then convert(decimal(18,5),@PurchasePrice) else 0 end ,@PONUMBER,@REFERENCECODE,@SERIALNO,
					@ASSETDESCRIPTION,@DELIVERYNOTE,@RFIDTAGCODE,@INVOICENO,@VOUCHERNO,@MAKE,@CAPACITY,@MAPPEDASSETID,@RECEIPTNUMBER,case when @PURCHASEDATE is not null then CONVERT(DATETIME,@PURCHASEDATE,103) else NULL end ,
					case when @ComissionDate is not null then CONVERT(DATETIME,@ComissionDate,103) else null end , case when @WarrantyExpiryDate is not null then CONVERT(DATETIME,@WarrantyExpiryDate,103) else NULL end ,
					1,@companyID,@userID,getdate(),1,0,0,0,@Attribute1,@Attribute2,@Attribute3,@Attribute4,@Attribute5,
			@Attribute6,@Attribute7,@Attribute8,@Attribute9,@Attribute10,@Attribute11,@Attribute12,
			@Attribute13,@Attribute14,@Attribute15,@Attribute16,@AssetRemarks,@QFAssetCode,@Attribute17,@Attribute18,@Attribute19,@Attribute20,@Attribute21,@Attribute22,@Attribute23,@Attribute24,@Attribute25,@Attribute26,
@Attribute27,@Attribute28,@Attribute29,@Attribute30,@Attribute31,@Attribute32,@Attribute33,@Attribute34,@Attribute35,@Attribute36,@Attribute37,@Attribute38,@Attribute39,@Attribute40)
			End 
			Else 
			Begin 
			  INSERT INTO @MESSAGETABLE(TEXT)VALUES('Mandatory field are passed with empty '+@Barcode)
			End 
			 End 
			END 
			END 
			ELSE 
			BEGIN 
			IF EXISTS (SELECT barcode FROM dbo.AssetTable WHERE Barcode=@Barcode)
			BEGIN
				UPDATE ASSETTABLE SET
				--ASSETCODE=CASE WHEN UPPER(@BarcodeEqualAssetCode)='TRUE' THEN ISNULL(@Barcode,ASSETCODE) ELSE ISNULL(@ASSETCODE,ASSETCODE) END,
				ASSETCODE=CASE WHEN @ASSETCODE is null or @ASSETCODE='' then AssetCode else @ASSETCODE end,
				BARCODE=ISNULL(@Barcode,BARCODE),
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
				PURCHASEDATE=CASE WHEN @PURCHASEDATE IS NULL OR RTRIM(LTRIM(@PURCHASEDATE))='' THEN PURCHASEDATE ELSE CONVERT(DATETIME,@PURCHASEDATE,103)  END,
				ComissionDate=CASE WHEN @ComissionDate IS NULL OR RTRIM(LTRIM(@ComissionDate))='' THEN ComissionDate ELSE CONVERT(DATETIME,@ComissionDate,103) END,
				WarrantyExpiryDate=CASE WHEN @WarrantyExpiryDate IS NULL OR RTRIM(LTRIM(@WarrantyExpiryDate))='' THEN WarrantyExpiryDate ELSE CONVERT(DATETIME,@WarrantyExpiryDate,103) END,			
				Attribute1=CASE WHEN @Attribute1 IS NULL OR RTRIM(LTRIM(@Attribute1))='' THEN Attribute1 ELSE @Attribute1 END,
				Attribute2=CASE WHEN @Attribute2 IS NULL OR RTRIM(LTRIM(@Attribute2))='' THEN Attribute2 ELSE @Attribute2 END,
				Attribute3=CASE WHEN @Attribute3 IS NULL OR RTRIM(LTRIM(@Attribute3))='' THEN Attribute3 ELSE @Attribute3 END,
				Attribute4=CASE WHEN @Attribute4 IS NULL OR RTRIM(LTRIM(@Attribute4))='' THEN Attribute4 ELSE @Attribute4 END,
				Attribute5=CASE WHEN @Attribute5 IS NULL OR RTRIM(LTRIM(@Attribute5))='' THEN Attribute5 ELSE @Attribute5 END,
				Attribute6=CASE WHEN @Attribute6 IS NULL OR RTRIM(LTRIM(@Attribute6))='' THEN Attribute6 ELSE @Attribute6 END,
				Attribute7=CASE WHEN @Attribute7 IS NULL OR RTRIM(LTRIM(@Attribute7))='' THEN Attribute7 ELSE @Attribute7 END,
				Attribute8=CASE WHEN @Attribute8 IS NULL OR RTRIM(LTRIM(@Attribute8))='' THEN Attribute8 ELSE @Attribute8 END,
				Attribute9=CASE WHEN @Attribute9 IS NULL OR RTRIM(LTRIM(@Attribute9))='' THEN Attribute9 ELSE @Attribute9 END,
				Attribute10=CASE WHEN @Attribute10 IS NULL OR RTRIM(LTRIM(@Attribute10))='' THEN Attribute10 ELSE @Attribute10 END,
				Attribute11=CASE WHEN @Attribute11 IS NULL OR RTRIM(LTRIM(@Attribute11))='' THEN Attribute11 ELSE @Attribute11 END,
				Attribute12=CASE WHEN @Attribute12 IS NULL OR RTRIM(LTRIM(@Attribute12))='' THEN Attribute12 ELSE @Attribute12 END,
				Attribute13=CASE WHEN @Attribute13 IS NULL OR RTRIM(LTRIM(@Attribute13))='' THEN Attribute13 ELSE @Attribute13 END,
				Attribute14=CASE WHEN @Attribute14 IS NULL OR RTRIM(LTRIM(@Attribute14))='' THEN Attribute14 ELSE @Attribute14 END,
				Attribute15=CASE WHEN @Attribute15 IS NULL OR RTRIM(LTRIM(@Attribute15))='' THEN Attribute15 ELSE @Attribute15 END,
				Attribute16=CASE WHEN @Attribute16 IS NULL OR RTRIM(LTRIM(@Attribute16))='' THEN Attribute16 ELSE @Attribute16 END,
				AssetRemarks=CASE WHEN @AssetRemarks IS NULL OR RTRIM(LTRIM(@AssetRemarks))='' THEN AssetRemarks ELSE @AssetRemarks END,
				QFAssetCode=CASE WHEN @QFAssetCode IS NULL OR RTRIM(LTRIM(@QFAssetCode))='' THEN QFAssetCode ELSE @QFAssetCode END,
				LastModifiedDateTime=getdate(),
				LastModifiedBy=@UserID ,
				Attribute17=CASE WHEN @Attribute17 IS NULL OR RTRIM(LTRIM(@Attribute17))='' THEN Attribute17 ELSE @Attribute17 END,
				Attribute18=CASE WHEN @Attribute18 IS NULL OR RTRIM(LTRIM(@Attribute18))='' THEN Attribute18 ELSE Attribute18 END,
				Attribute19=CASE WHEN @Attribute19 IS NULL OR RTRIM(LTRIM(@Attribute19))='' THEN Attribute19 ELSE @Attribute19 END,
				Attribute20=CASE WHEN @Attribute20 IS NULL OR RTRIM(LTRIM(@Attribute20))='' THEN Attribute20 ELSE @Attribute20 END,
			    Attribute21=CASE WHEN @Attribute21 IS NULL OR RTRIM(LTRIM(@Attribute21))='' THEN Attribute21 ELSE @Attribute21 END,
				Attribute22=CASE WHEN @Attribute22 IS NULL OR RTRIM(LTRIM(@Attribute22))='' THEN Attribute22 ELSE @Attribute22 END,
				Attribute23=CASE WHEN @Attribute23 IS NULL OR RTRIM(LTRIM(@Attribute23))='' THEN Attribute23 ELSE @Attribute23 END,
				Attribute24=CASE WHEN @Attribute24 IS NULL OR RTRIM(LTRIM(@Attribute24))='' THEN Attribute24 ELSE @Attribute24 END,
				Attribute25=CASE WHEN @Attribute25 IS NULL OR RTRIM(LTRIM(@Attribute25))='' THEN Attribute25 ELSE @Attribute25 END,
				Attribute26=CASE WHEN @Attribute26 IS NULL OR RTRIM(LTRIM(@Attribute26))='' THEN Attribute26 ELSE @Attribute26 END,
				Attribute27=CASE WHEN @Attribute27 IS NULL OR RTRIM(LTRIM(@Attribute27))='' THEN Attribute27 ELSE @Attribute27 END,
				Attribute28=CASE WHEN @Attribute28 IS NULL OR RTRIM(LTRIM(@Attribute28))='' THEN Attribute28 ELSE @Attribute28 END,
				Attribute29=CASE WHEN @Attribute29 IS NULL OR RTRIM(LTRIM(@Attribute29))='' THEN Attribute29 ELSE @Attribute29 END,
				Attribute30=CASE WHEN @Attribute30 IS NULL OR RTRIM(LTRIM(@Attribute30))='' THEN Attribute30 ELSE @Attribute30 END,
				Attribute31=CASE WHEN @Attribute31 IS NULL OR RTRIM(LTRIM(@Attribute31))='' THEN Attribute31 ELSE @Attribute31 END,
				Attribute32=CASE WHEN @Attribute32 IS NULL OR RTRIM(LTRIM(@Attribute32))='' THEN Attribute32 ELSE @Attribute32 END,
				Attribute33=CASE WHEN @Attribute33 IS NULL OR RTRIM(LTRIM(@Attribute33))='' THEN Attribute33 ELSE @Attribute33 END,
				Attribute34=CASE WHEN @Attribute34 IS NULL OR RTRIM(LTRIM(@Attribute34))='' THEN Attribute34 ELSE @Attribute34 END,
				Attribute35=CASE WHEN @Attribute35 IS NULL OR RTRIM(LTRIM(@Attribute35))='' THEN Attribute35 ELSE @Attribute35 END,
				Attribute36=CASE WHEN @Attribute36 IS NULL OR RTRIM(LTRIM(@Attribute36))='' THEN Attribute36 ELSE @Attribute36 END,
				Attribute37=CASE WHEN @Attribute37 IS NULL OR RTRIM(LTRIM(@Attribute37))='' THEN Attribute37 ELSE @Attribute37 END,
				Attribute38=CASE WHEN @Attribute38 IS NULL OR RTRIM(LTRIM(@Attribute38))='' THEN Attribute38 ELSE @Attribute38 END,
				Attribute39=CASE WHEN @Attribute39 IS NULL OR RTRIM(LTRIM(@Attribute39))='' THEN Attribute39 ELSE @Attribute39 END,
				Attribute40=CASE WHEN @Attribute40 IS NULL OR RTRIM(LTRIM(@Attribute40))='' THEN Attribute40 ELSE @Attribute40 END
				 WHERE BARCODE=@BARCODE 
			 END
			 ELSE
			 BEGIN 
				 INSERT INTO @MESSAGETABLE(TEXT)VALUES(@Barcode + '- This barcode not available in Assettable')
			 END 
			END 
			--select * from @MESSAGETABLE

			Select @ReturnMessage = COALESCE(@ReturnMessage + ', ' + Text, Text)  from @MESSAGETABLE
			select @ReturnMessage as ReturnMessage
        
			 
End
go 



ALTER Procedure [dbo].[Prc_AssetCreationValidation]
(
	@UserID				int					=	NULL,
	@CategoryCode		NVARCHAR(MAX)		=	NULL,
	@LocationCode		nvarchar(max)		=	NULL,
	@CategoryID			int					=	NULL,
	@LocationID			int					=	NULL,
	@DepartmentID		int					=	NULL,
	@DepartmentCode		nvarchar(max)		=	NULL,
	@SerialNo			NVARCHAR(MAX)		=	NULL,
	@ManufacturerCode	nvarchar(max)		=	NULL,
	@ManufacturerID		INT					=	NULL, --dont allow duplicate serial no  --dont allow manufacturer based duplicate serial no 
	@DataProcessedBy	nvarchar(50)		=   null, 
	@ErrorID			int					OutPut,
	@ErrorMsg			nvarchar(max)		Output

)
as 
Begin 
	Declare @LocationLevel int,@CategoryLevel int,@CategoryBasedSerialNo nvarchar(max),@ManufacturerBasedSerialNo nvarchar(max),@enableCategorySerial int,
	@enableManufacturerSerialno int,@defaultSerialNoMandatory int,@UserLocationMapping int,@UserCategoryMapping int ,@UserDepartmentMapping int 

	Declare @ParentCategoryTable Table(CategoryID int)
	Declare @ManufacturerTable Table(ManufacturerID int)
	Declare @UserCategoryTable Table(CategoryID int)
	Declare @UserLocationTable Table(LocationID int)
	Declare @UserDepartmentTable Table(DepartmentID int)

	Select @LocationLevel=ConfiguarationValue from ConfigurationTable where ConfiguarationName='PreferredLevelLocationMapping'
	Select @CategoryLevel=ConfiguarationValue from ConfigurationTable where ConfiguarationName='PreferredLevelCategoryMapping'
	
	Select @CategoryBasedSerialNo=ConfiguarationValue from ConfigurationTable 
			where ConfiguarationName='SelectedCategoriesareMandatorySerialNumberInAssetScreen'
   	Select @ManufacturerBasedSerialNo=ConfiguarationValue from ConfigurationTable 
			where ConfiguarationName='SelectedManufacturerareMandatorySerialNumberInAssetScreen'
	Select @enableCategorySerial=case when ConfiguarationValue='true' then 1 else 0 end from ConfigurationTable
		where ConfiguarationName='DontAllowCategoryBasedDuplicateSerialNo'
	Select @enableManufacturerSerialno=case when ConfiguarationValue='true' then 1 else 0 end from ConfigurationTable
		where ConfiguarationName='DontAllowManufacturerBasedDuplicateSerialNo'
	Select @defaultSerialNoMandatory=case when ConfiguarationValue='true' then 1 else 0 end from ConfigurationTable
		where ConfiguarationName='IsMandatorySerialNumberinAssetScreen'
	
	Select @UserCategoryMapping=case when ConfiguarationValue='true' then 1 else 0 end from ConfigurationTable
		where ConfiguarationName='UserCategoryMapping'	
	Select @UserLocationMapping=case when ConfiguarationValue='true' then 1 else 0 end from ConfigurationTable
		where ConfiguarationName='UserLocationMapping'
	Select @UserDepartmentMapping=case when ConfiguarationValue='true' then 1 else 0 end from ConfigurationTable
		where ConfiguarationName='UserDepartmentMapping'		
	
	Insert into @UserCategoryTable(CategoryID)
	Select CategoryID from UserCategoryMappingTable where PersonID=@UserID

	Insert into @UserLocationTable(LocationID)
	Select LocationID from UserLocationMappingTable where PersonID=@UserID

	Insert into @UserDepartmentTable(DepartmentID)
	Select DepartmentID from UserDepartmentMappingTable where PersonID=@UserID

	Insert into @ParentCategoryTable(CategoryID)
	select Value from  Split(@CategoryBasedSerialNo,',')

	Insert into @ManufacturerTable(ManufacturerID)
	select Value from  Split(@ManufacturerBasedSerialNo,',')

	Set @ErrorID	= 0
	set @ErrorMsg= null

	IF @CategoryID is null or @CategoryID=''
	Begin 
		IF @CategoryCode is null or @CategoryCode=''
		Begin
			set @errorID=1
			set @ErrorMsg ='Category Code is Missed.'
		return
		End
		Else 
		Begin 
		Select @CategoryID=CategoryID from CategoryTable where CategoryCode=@CategoryCode
			IF @CategoryID is null or @CategoryID=''
			Begin
				set @errorID=2
				set @ErrorMsg =@CategoryCode+'- Category Code not valid.'
			End 
			Else 
			Begin 
				If exists(select categoryID from CategoryNewView where CategoryID=@CategoryID and Level<@CategoryLevel)
				begin
				set @errorID=3
				set @ErrorMsg =@CategoryCode+'- Category Code level must be give '+ convert(nvarchar(100),@CategoryLevel  ) + 'Level.'
				end 
		  End 
		End 
	End 
	IF @LocationID is null or @LocationID=''
	Begin 
		IF @LocationCode is null or @LocationCode=''
		Begin
			set @errorID=4
			set @ErrorMsg ='Location Code is Missed.'
		return
		End
		Else 
		Begin 
		Select @LocationID=LocationID from LocationTable where LocationCode=@LocationCode
			IF @LocationID is null or @LocationID=''
			Begin
				set @errorID=5
				set @ErrorMsg =@LocationCode+'- Location Code not valid.'
			End 
			Else 
			Begin 
				If exists(select LocationID from LocationNewView where LocationID=@LocationID and Level<@LocationLevel)
				begin
				set @errorID=6
				set @ErrorMsg =@LocationCode+'- Location Code level must be give '+ convert(nvarchar(100),@LocationLevel  ) + 'Level.'
				end 
			End 
		End 
	End 
	IF @CategoryCode is null or @CategoryCode=''
	Begin
	  Select @CategoryCode=CategoryCode from CategoryTable where categoryID=@CategoryID
	End 
	IF @LocationCode is null or @LocationCode=''
	Begin
	  Select @LocationCode=LocationCode from LocationTable where LocationID=@LocationID
	End 
	IF ((@DepartmentID is null or @DepartmentID='') and (@DepartmentCode is not null or @DepartmentCode!=''))
	BEgin 
		 IF not exists(select DEpartmentID from DepartmentTable where DepartmentCode=@DepartmentCode)
		 Begin
			  Set @ErrorID=11
			  Set @ErrorMsg=@DepartmentCode+ '- Department Code is valid.'
			  return
		 End 
		 Else
		 Begin 
			SElect @DepartmentID=DepartmentID from DepartmentTable where DepartmentCode=@DepartmentCode
			 
		 End 
	End 
	IF ((@DepartmentID is not null or @DepartmentID!='') and (@DepartmentCode is  null or @DepartmentCode=''))
	BEgin  
		SElect @DepartmentCode=DepartmentCode from DepartmentTable where DepartmentID=@DepartmentID
	End 
	If @DepartmentID is not null
	BEgin 
		If not Exists(Select DepartmentID from DepartmentTable where DepartmentID=@DepartmentID
			and DepartmentID in (select DepartmentID from @UserDepartmentTable))
		BEgin 
		set @ErrorID=12
		Set @ErrorMsg= @DepartmentCode+'- given DepartmentCode not mapped with User.'
		return	
		End 
	End 

	If not Exists(select CategoryID from CategoryNewView 
		where CategoryID=@CategoryID and MappedCategoryID in (select CategoryID from @UserCategoryTable))
	Begin 
		set @ErrorID=9
		Set @ErrorMsg=@categoryCode+'- given CategoryCode not mapped with User.'
		return		
	End 
	If not Exists(select LocationID from LocationNewView 
		where LocationID=@LocationID and MappedLocationID in (select LocationID from @UserLocationTable))
	Begin 
		set @ErrorID=10
		Set @ErrorMsg=@LocationCode+'- given LocationCode not mapped with User.'
		return		
	End
	
	IF @ManufacturerCode is null or @ManufacturerCode=''
	Begin 
		IF @ManufacturerID is not null or @ManufacturerID !=''
		Begin
			Select @ManufacturerCode=ManufacturerCode from ManufacturerTable where ManufacturerID=@ManufacturerID
		End 
	End 
	IF @ManufacturerID is null or @ManufacturerID=''
	Begin 
		IF @ManufacturerCode is not null or @ManufacturerCode !=''
		Begin
			Select @ManufacturerID=ManufacturerID from ManufacturerTable where ManufacturerCode=@ManufacturerCode
		End 
	End 
	IF @enableCategorySerial=1 and @Serialno is not null
	Begin 
		IF not exists(select CategoryID from @ParentCategoryTable where CategoryID is not null)
		Begin 
		  IF not exists(select categoryID from @ParentCategoryTable 
			where categoryID=(select Level1ID from categoryNewView where categoryID=@categoryID))
			Begin 
				IF exists(select serialno from AssetNewView where CategoryL1=(select Level1ID from categoryNewView where categoryID=@categoryID)
					and SerialNo=@Serialno)
					Begin
						set @ErrorID=7
						set @ErrorMsg=@Serialno+'- already exists given category-' +  @categoryCode +'.'
						return
					End 
			End
		End 
	End
	Else 
	Begin 
		IF @enableCategorySerial=1 and (@Serialno is null or @Serialno='')
		Begin 
		 set @ErrorID=9
		 Set @ErrorMsg= +'- Serial no is missed -' + @CategoryCode +'.'
		 return
		End 
	End 
	IF @enableManufacturerSerialno=1 and @Serialno is not null and @ManufacturerID is not null
	Begin 
		IF not exists(select ManufacturerID from @ManufacturerTable where ManufacturerID is not null)
		Begin
			IF exists(select serialno from AssetTable where ManufacturerID=@manufacturerID
					and SerialNo=@Serialno)
					Begin
					
						set @ErrorID=8
						Set @ErrorMsg=@Serialno+'- already exists given manufacturerCode-' +  @ManufacturerCode+'.'
						return
					End 
		End 
	End 
	Else 
	Begin
	 IF @enableManufacturerSerialno=1 and (@Serialno is null or @Serialno='') and @ManufacturerID is not null
	 Begin 
		 set @ErrorID=13
		 Set @ErrorMsg= +'- Serial no is missed -' + @ManufacturerCode +'.'
		 return
	 end 
	End 
	IF @defaultSerialNoMandatory=1 and @Serialno is not null
	Begin 
		IF exists(select serialno from AssetTable where SerialNo=@Serialno)
		Begin
		set @ErrorID=8
		Set @ErrorMsg=@Serialno+'- already exists.'
		return
		End 
	End 
	Else 
	Begin 
		IF @defaultSerialNoMandatory=1 and (@Serialno is null or @Serialno='')
		Begin 
			set @ErrorID=14
			Set @ErrorMsg=+'Serial no is missed!.'
		return
		End 
	end 
	
End 
go 
ALTER Procedure [dbo].[Prc_AssetModificationValidation]
(
	@UserID				int					=	NULL,
	@AssetID			int					=	NULL,
	@CategoryCode		NVARCHAR(MAX)		=	NULL,
	@LocationCode		nvarchar(max)		=	NULL,
	@CategoryID			int					=	NULL,
	@LocationID			int					=	NULL,
	@DepartmentID		int					=	NULL,
	@DepartmentCode		nvarchar(max)		=	NULL,
	@SerialNo			NVARCHAR(MAX)		=	NULL,
	@ManufacturerCode	nvarchar(max)		=	NULL,
	@ManufacturerID		INT					=	NULL, --dont allow duplicate serial no  --dont allow manufacturer based duplicate serial no 
	@DataProcessedBy	nvarchar(50)		=   null, 
	@ErrorID			int					OutPut,
	@ErrorMsg			nvarchar(max)		Output

)
as 
Begin 
	Declare @LocationLevel int,@CategoryLevel int,@CategoryBasedSerialNo nvarchar(max),@ManufacturerBasedSerialNo nvarchar(max),@enableCategorySerial int,
	@enableManufacturerSerialno int,@defaultSerialNoMandatory int,@UserLocationMapping int,@UserCategoryMapping int ,@UserDepartmentMapping int 
	,@AssetCategoryID int ,@AssetLocationID int,@AssetDepartmentID int,@AssetmanufacturerID int ,@Barcode nvarchar(max),@ActiveStatusID int

	Select  @ActiveStatusID=[dbo].fnGetActiveStatusID()
	Set @ErrorID	= 0
	set @ErrorMsg= null

	IF not exists(select AssetID from AssetTable where AssetID=@AssetID and StatusID=@ActiveStatusID) 
	Begin
	Select @Barcode=Barcode from  AssetTable where AssetID=@AssetID 
	set @ErrorID=15
		Set @ErrorMsg= @Barcode+'- given Barcode is not an active status.'
		return	
	End 

	Select @AssetCategoryID=CategoryID,@AssetLocationID=LocationID,@AssetDepartmentID=DepartmentID,@AssetmanufacturerID=ManufacturerID 
		From AssetTable where AssetID=@AssetID
	
	IF ((@CategoryID is null or @CategoryID ='') and (@CategoryCode is null or @CategoryCode=''))
	Begin
	 Set @CategoryID=@AssetCategoryID
	End 
	IF ((@LocationID is null or @LocationID ='') and (@LocationCode is null or @LocationCode=''))
	Begin
	 Set @LocationID=@AssetLocationID
	End 
	IF ((@DepartmentID is null or @DepartmentID ='') and (@DepartmentCode is null or @DepartmentCode=''))
	Begin
	 Set @DepartmentID=@AssetDepartmentID
	End 
	IF ((@ManufacturerID is null or @ManufacturerID ='') and (@ManufacturerCode is null or @ManufacturerCode=''))
	Begin
	 Set @ManufacturerID=@AssetManufacturerID
	End

	Declare @ParentCategoryTable Table(CategoryID int)
	Declare @ManufacturerTable Table(ManufacturerID int)
	Declare @UserCategoryTable Table(CategoryID int)
	Declare @UserLocationTable Table(LocationID int)
	Declare @UserDepartmentTable Table(DepartmentID int)

	Select @LocationLevel=ConfiguarationValue from ConfigurationTable where ConfiguarationName='PreferredLevelLocationMapping'
	Select @CategoryLevel=ConfiguarationValue from ConfigurationTable where ConfiguarationName='PreferredLevelCategoryMapping'
	
	Select @CategoryBasedSerialNo=ConfiguarationValue from ConfigurationTable 
			where ConfiguarationName='SelectedCategoriesareMandatorySerialNumberInAssetScreen'
   	Select @ManufacturerBasedSerialNo=ConfiguarationValue from ConfigurationTable 
			where ConfiguarationName='SelectedManufacturerareMandatorySerialNumberInAssetScreen'
	Select @enableCategorySerial=case when ConfiguarationValue='true' then 1 else 0 end from ConfigurationTable
		where ConfiguarationName='DontAllowCategoryBasedDuplicateSerialNo'
	Select @enableManufacturerSerialno=case when ConfiguarationValue='true' then 1 else 0 end from ConfigurationTable
		where ConfiguarationName='DontAllowManufacturerBasedDuplicateSerialNo'
	Select @defaultSerialNoMandatory=case when ConfiguarationValue='true' then 1 else 0 end from ConfigurationTable
		where ConfiguarationName='IsMandatorySerialNumberinAssetScreen'
	
	Select @UserCategoryMapping=case when ConfiguarationValue='true' then 1 else 0 end from ConfigurationTable
		where ConfiguarationName='UserCategoryMapping'	
	Select @UserLocationMapping=case when ConfiguarationValue='true' then 1 else 0 end from ConfigurationTable
		where ConfiguarationName='UserLocationMapping'
	Select @UserDepartmentMapping=case when ConfiguarationValue='true' then 1 else 0 end from ConfigurationTable
		where ConfiguarationName='UserDepartmentMapping'		
	
	Insert into @UserCategoryTable(CategoryID)
	Select CategoryID from UserCategoryMappingTable where PersonID=@UserID

	Insert into @UserLocationTable(LocationID)
	Select LocationID from UserLocationMappingTable where PersonID=@UserID

	Insert into @UserDepartmentTable(DepartmentID)
	Select DepartmentID from UserDepartmentMappingTable where PersonID=@UserID

	Insert into @ParentCategoryTable(CategoryID)
	select Value from  Split(@CategoryBasedSerialNo,',')

	Insert into @ManufacturerTable(ManufacturerID)
	select Value from  Split(@ManufacturerBasedSerialNo,',')

	IF @CategoryID is null or @CategoryID=''
	Begin 
		IF @CategoryCode is null or @CategoryCode=''
		Begin
			set @errorID=1
			set @ErrorMsg ='Category Code is Missed.'
		return
		End
		Else 
		Begin 
		Select @CategoryID=CategoryID from CategoryTable where CategoryCode=@CategoryCode
			IF @CategoryID is null or @CategoryID=''
			Begin
				set @errorID=2
				set @ErrorMsg =@CategoryCode+'- Category Code not valid.'
				return
			End 
			Else 
			Begin 
				If exists(select categoryID from CategoryNewView where CategoryID=@CategoryID and Level<@CategoryLevel)
				begin
				set @errorID=3
				set @ErrorMsg =@CategoryCode+'- Category Code level must be give '+ convert(nvarchar(100),@LocationLevel  ) + 'Level.'
				return
				end 
		  End 
		End 
	End 
	IF @LocationID is null or @LocationID=''
	Begin 
		IF @LocationCode is null or @LocationCode=''
		Begin
			set @errorID=4
			set @ErrorMsg ='Location Code is Missed.'
		return
		End
		Else 
		Begin 
		Select @LocationID=LocationID from LocationTable where LocationCode=@LocationCode
			IF @LocationID is null or @LocationID=''
			Begin
				set @errorID=5
				set @ErrorMsg =@LocationCode+'- Location Code not valid.'
				return
			End 
			Else 
			Begin 
				If exists(select LocationID from LocationNewView where LocationID=@LocationID and Level<@LocationLevel)
				begin
				set @errorID=6
				set @ErrorMsg =@LocationCode+'- Location Code level must be give '+ convert(nvarchar(100),@LocationLevel  ) + ' Level.'
				return
				end 
			End 
		End 
	End 
	IF @CategoryCode is null or @CategoryCode=''
	Begin
	  Select @CategoryCode=CategoryCode from CategoryTable where categoryID=@CategoryID
	End 
	IF @LocationCode is null or @LocationCode=''
	Begin
	  Select @LocationCode=LocationCode from LocationTable where LocationID=@LocationID
	End 
	IF ((@DepartmentID is null or @DepartmentID='') and (@DepartmentCode is not null or @DepartmentCode!=''))
	BEgin 
		 IF not exists(select DEpartmentID from DepartmentTable where DepartmentCode=@DepartmentCode)
		 Begin
			  Set @ErrorID=11
			  Set @ErrorMsg=@DepartmentCode+ '- Department Code is valid.'
			  return
		 End 
		 Else
		 Begin 
			SElect @DepartmentID=DepartmentID from DepartmentTable where DepartmentCode=@DepartmentCode
			 
		 End 
	End 
	IF ((@DepartmentID is not null or @DepartmentID!='') and (@DepartmentCode is  null or @DepartmentCode=''))
	BEgin  
		SElect @DepartmentCode=DepartmentCode from DepartmentTable where DepartmentID=@DepartmentID
	End 
	If @DepartmentID is not null
	BEgin 
		If not Exists(Select DepartmentID from DepartmentTable where DepartmentID=@DepartmentID
			and DepartmentID in (select DepartmentID from @UserDepartmentTable))
		BEgin 
		set @ErrorID=12
		Set @ErrorMsg= @DepartmentCode+'- given DepartmentCode not mapped with User.'
		return	
		End 
	End 

	If not Exists(select CategoryID from CategoryNewView 
		where CategoryID=@CategoryID and MappedCategoryID in (select CategoryID from @UserCategoryTable))
	Begin 
		set @ErrorID=9
		Set @ErrorMsg=@categoryCode+'- given CategoryCode not mapped with User.'
		return		
	End 
	If not Exists(select LocationID from LocationNewView 
		where LocationID=@LocationID and MappedLocationID in (select LocationID from @UserLocationTable))
	Begin 
		set @ErrorID=10
		Set @ErrorMsg=@LocationCode+'- given LocationCode not mapped with User.'
		return		
	End
	
	IF @ManufacturerCode is null or @ManufacturerCode=''
	Begin 
		IF @ManufacturerID is not null or @ManufacturerID !=''
		Begin
			Select @ManufacturerCode=ManufacturerCode from ManufacturerTable where ManufacturerID=@ManufacturerID
		End 
	End 
	IF @ManufacturerID is null or @ManufacturerID=''
	Begin 
		IF @ManufacturerCode is not null or @ManufacturerCode !=''
		Begin
			Select @ManufacturerID=ManufacturerID from ManufacturerTable where ManufacturerCode=@ManufacturerCode
		End 
	End 
	IF @enableCategorySerial=1 and @Serialno is not null
	Begin 
		IF not exists(select CategoryID from @ParentCategoryTable where CategoryID is not null)
		Begin 
		  IF not exists(select categoryID from @ParentCategoryTable 
			where categoryID=(select Level1ID from categoryNewView where categoryID=@categoryID))
			Begin 
				IF exists(select serialno from AssetNewView where CategoryL1=(select Level1ID from categoryNewView where categoryID=@categoryID)
					and SerialNo=@Serialno and assetid!=@AssetID)
					Begin
						set @ErrorID=7
						set @ErrorMsg=@Serialno+'- already exists given category-' +  @categoryCode +'.'
						return
					End 
			End
		End 
	End
	Else 
	Begin 
		IF @enableCategorySerial=1 and (@Serialno is null or @Serialno='')
		Begin 
		 set @ErrorID=9
		 Set @ErrorMsg= +'- Serial no is missed -' + @CategoryCode +'.'
		 return
		End 
	End 
	IF @enableManufacturerSerialno=1 and @Serialno is not null and @ManufacturerID is not null
	Begin 
		IF not exists(select ManufacturerID from @ManufacturerTable where ManufacturerID is not null)
		Begin
			IF exists(select serialno from AssetTable where ManufacturerID=@manufacturerID
					and SerialNo=@Serialno and AssetID=@AssetID)
					Begin
					
						set @ErrorID=8
						Set @ErrorMsg=@Serialno+'- already exists given manufacturerCode-' +  @ManufacturerCode+'.'
						return
					End 
		End 
	End 
	Else 
	Begin
	 IF @enableManufacturerSerialno=1 and (@Serialno is null or @Serialno='') and @ManufacturerID is not null
	 Begin 
		 set @ErrorID=13
		 Set @ErrorMsg= +'- Serial no is missed -' + @ManufacturerCode +'.'
		 return
	 end 
	End 
	IF @defaultSerialNoMandatory=1 and @Serialno is not null
	Begin 
		IF exists(select serialno from AssetTable where SerialNo=@Serialno and AssetID=@AssetID)
		Begin
		set @ErrorID=8
		Set @ErrorMsg=@Serialno+'- already exists.'
		return
		End 
	End 
	Else 
	Begin 
		IF @defaultSerialNoMandatory=1 and (@Serialno is null or @Serialno='')
		Begin 
			set @ErrorID=14
			Set @ErrorMsg=+'Serial no is missed!.'
		return
		End 
	end 
	
End 
go 
ALTER Procedure [dbo].[IPRC_AMSExceImportBulkAssets]
(
	@AssetCode nvarchar(100)=NULL,
	@Barcode nvarchar(100)='AT24105231',
	@DepartmentCode nvarchar(100)=NULL,
	@SectionCode nvarchar(100)=NULL,
	@CustodianCode nvarchar(100)=NULL,
	@ModelCode nvarchar(100)=NULL,
	@PONumber nvarchar(100)=NULL,
	@ReferenceCode nvarchar(100)=null,
	@SerialNo nvarchar(100)=NULL,
	@PurchaseDate nvarchar(100)=NULL,
	@WarrantyExpiryDate nvarchar(100)=NULL,
	@CategoryCode nvarchar(100)=NULL,
	@LocationCode nvarchar(100)='ABDB',
	@AssetDescription nvarchar(max)=NULL,
	@AssetConditionCode nvarchar(100)=NULL,
	@PurchasePrice nvarchar(100)=NULL,
	@DeliveryNote nvarchar(100)=null,
	@RFIDTagCode nvarchar(100)=null,
	@SupplierCode nvarchar(100)=NULL,
	@AssetRemarks nvarchar(max)=null,
	@InvoiceNo nvarchar(100)=null,
	@InvoiceDate nvarchar(100)=null,
	@ManufacturerCode nvarchar(100)=NULL,
	@ComissionDate nvarchar(100)=null,
	@VoucherNo nvarchar(100)=null,
	@Make nvarchar(100)=null,
	@Capacity nvarchar(100)=null,
	@MappedAssetID nvarchar(100)=null,
	@ReceiptNumber nvarchar(100)=null,
	@ImportTypeID int=2,
	@UserID int=1,
	@LanguageID int=1,
	@CompanyID int=1003,	
	@Attribute1 nvarchar(100)=null,
	@Attribute2 nvarchar(100)=null,
	@Attribute3 nvarchar(100)=null,
	@Attribute4 nvarchar(100)=null,
	@Attribute5 nvarchar(100)=null,
	@Attribute6 nvarchar(100)=null,
	@Attribute7 nvarchar(100)=null,
	@Attribute8 nvarchar(100)=null,
	@Attribute9 nvarchar(100)=null,
	@Attribute10 nvarchar(100)=null,
	@Attribute11 nvarchar(100)=null,
	@Attribute12 nvarchar(100)=null,
	@Attribute13 nvarchar(100)=null,
	@Attribute14 nvarchar(100)=null,
	@Attribute15 nvarchar(100)=null,
	@Attribute16 nvarchar(100)=null,
    @QFAssetCode nvarchar(100)=NULL,
	@Attribute17 nvarchar(100)=NULL,
	@Attribute18 nvarchar(100)=NULL,
	@Attribute19 nvarchar(100)=NULL,
	@Attribute20 nvarchar(100)=NULL,
	@Attribute21 nvarchar(100)=NULL,
	@Attribute22 nvarchar(100)=NULL,
	@Attribute23 nvarchar(100)=NULL,
	@Attribute24 nvarchar(100)=NULL,
	@Attribute25 nvarchar(100)=NULL,
	@Attribute26 nvarchar(100)=NULL,
	@Attribute27 nvarchar(100)=NULL,
	@Attribute28 nvarchar(100)=NULL,
	@Attribute29 nvarchar(100)=NULL,
	@Attribute30 nvarchar(100)=NULL,
	@Attribute31 nvarchar(100)=NULL,
	@Attribute32 nvarchar(100)=NULL,
	@Attribute33 nvarchar(100)=NULL,
	@Attribute34 nvarchar(100)=NULL,
	@Attribute35 nvarchar(100)=NULL,
	@Attribute36 nvarchar(100)=NULL,
	@Attribute37 nvarchar(100)=NULL,
	@Attribute38 nvarchar(100)=NULL,
	@Attribute39 nvarchar(100)=NULL,
	@Attribute40 nvarchar(100)=NULL
	
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
if(@PurchasePrice='-')
Begin 
set @PurchasePrice=NULL
End 
 	--Declartion part 
	Declare @DepartmentID int,@SectionID int,@CustodianID int,@SupplierID int,@AssetConditionID int, @CategoryID int, @ManufacturerID int,@ModelID int ,@ProductID int ,@LocationID int  ,@DateFormat bit ,@numberFormat bit 
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
	   SElect @Barcode+'- Manufacturer Code is must be add if update/Create Model Code ' as ReturnMessage
				Return 
	END 
	IF @CustodianCode IS NOT NULL AND (@DepartmentCode IS NULL OR @DepartmentCode='')
	BEGIN 
	   SElect @Barcode+'- Department Code is must be add if Update/Create Custodian Code ' as ReturnMessage
				Return 
	END 
	IF @SectionCode IS NOT NULL AND (@DepartmentCode IS NULL OR @DepartmentCode='')
	BEGIN 
	   SElect @Barcode+'- Department Code is must be add if Update/Create Section Code ' as ReturnMessage
				Return 
	END 
	If ((@ComissionDate is not null and @ComissionDate!=''))
		Begin
		  SET DATEFORMAT dmy;
		  SELECT @DateFormat= ISDATE(@ComissionDate)
		  If @DateFormat=0
		   BEGIN 
		     SElect @Barcode+'- Comission Date is invalid' as ReturnMessage
				Return 
		   END 
		End 
			If ((@WarrantyExpiryDate is not null and @WarrantyExpiryDate!=''))
		Begin
		  SET DATEFORMAT dmy;
		  SELECT @DateFormat= ISDATE(@WarrantyExpiryDate)
		  If @DateFormat=0
		   BEGIN 
		     SElect @Barcode+'- Warranty Expiry Date is invalid' as ReturnMessage
				Return 
		   END 
		End
		--print @PurchasePrice
		If ((@PurchasePrice is not null and @PurchasePrice!=''))
		Begin
		  SET DATEFORMAT dmy;
		  SELECT @numberFormat= isnumeric(@PurchasePrice)
		  If @numberFormat=0
		   BEGIN 
		     SElect @Barcode+'- Purchase Price is invalid' as ReturnMessage
				Return 
		   END
		   Else
			BEGIN
				If convert(decimal(18,5),@PurchasePrice)<0
					BEGIN
						SElect @Barcode+'- Purchase Price should not be negative' as ReturnMessage
						Return	
					END
			END
		End 

		If ((@PurchaseDate is not null and @PurchaseDate!=''))
		Begin
		  SET DATEFORMAT dmy;
		  SELECT @DateFormat= ISDATE(@PurchaseDate)
		  If @DateFormat=0
		   BEGIN 
		     SElect @Barcode+'-  Purchase Date is invalid' as ReturnMessage
				Return 
		   END 
		End 
		If ((@InvoiceDate is not null and @InvoiceDate!=''))
		Begin
		  SET DATEFORMAT dmy;
		  SELECT @DateFormat= ISDATE(@InvoiceDate)
		  If @DateFormat=0
		   BEGIN 
		     SElect @Barcode+'- Invoice Date is invalid' as ReturnMessage
				Return 
		   END 
		End 

		if ((@PurchaseDate is null or @PurchaseDate=' ') and (@WarrantyExpiryDate is not null and  @WarrantyExpiryDate!=''))
					 Begin 
					      SElect 'Without Purchase date warranty expiry month should not be added For'+@Barcode as ReturnMessage
					      Return 
					 End 
					 else if ((@PurchaseDate is not null and @PurchaseDate!='') and (@WarrantyExpiryDate is not null and  @WarrantyExpiryDate!=''))
					 Begin 
					      Declare @res bit 
						  Select @res=ISDATE(@WarrantyExpiryDate)
						  IF @res=1 
						  begin
						   IF CONVERT(DATETIME,@PURCHASEDATE,103)>CONVERT(DATETIME,@WarrantyExpiryDate,103)
						   Begin 
					      SElect 'warranty expiry month should be in greater then Purchase Date  '+@Barcode as ReturnMessage
					      Return
						  End
						  End
						 
					 End 
	Declare @ErrorID int 
	Declare @ErrorMsg nvarchar(max)
	If @ImportTypeID=1
	begin
		 exec Prc_AssetCreationValidation @userID,@CategoryCode,@LocationCode,null,null,null,@DepartmentCode,@SerialNo,@ManufacturerCode,null,'ExcelAssetInsert',@ErrorID output,@ErrorMsg output
		if @ErrorID>0
		Begin
			 Select @ErrorMsg
			 Return
		End 
		if upper(@BarcodeAutoGenerateEnable)='TRUE' and upper(@IsBarcodeSettingApplyImportAsset)='TRUE'
		begin 
		  set @Barcode='-';
		End 
		if UPPER(@BarcodeEqualAssetCode)='TRUE' and upper(@IsBarcodeSettingApplyImportAsset)='TRUE'
		Begin 
		set @AssetCode=@Barcode
		End 
	end 
	Else 
	Begin 
	  Declare @AssetID int 
	  Select @AssetID=AssetID from AssetTable where barcode=@Barcode


	  exec Prc_AssetModificationValidation @userID,@AssetID,@CategoryCode,@LocationCode,null,null,null,@DepartmentCode,@SerialNo,@ManufacturerCode,null,'ExcelAssetInsert',@ErrorID output,@ErrorMsg output
		
		if @ErrorID>0
		Begin
			 Select @ErrorMsg
			 Return
		End 
	End 
	
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
				SELECT @SECTIONID=SECTIONID FROM SECTIONTABLE WHERE SECTIONCODE=@SECTIONCODE AND DEPARTMENTID in (SELECT DEPARTMENTID FROM DEPARTMENTTABLE WHERE DEPARTMENTCODE=@DEPARTMENTCODE and StatusID=1) and StatusID=1
			    
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
			  IF EXISTS(SELECT PERSONCODE FROM PERSONTABLE WHERE PERSONCODE=@CUSTODIANCODE AND (USERTYPEID=2 OR USERTYPEID=3) and StatusID=1)
			  BEGIN
				SELECT @CUSTODIANID=PERSONID FROM PERSONTABLE WHERE PERSONCODE=@CUSTODIANCODE AND  (USERTYPEID=2 OR USERTYPEID=3) and StatusID=1
			  END 
			  ELSE 
			  BEGIN 
			    IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
		   BEGIN 	

			    INSERT INTO User_LoginUserTable(UserName,Password,PasswordSalt,LastActivityDate,LastLoginDate,LastLoggedInDate,ChangePasswordAtNextLogin,IsLockedOut,IsDisabled,IsApproved)
			VALUES(@CUSTODIANCODE ,'Mod6/JMHjHeKXDkUK/zd7PfLlJg=','BZxI8E2lroNt28VMhZsyyaNZha8=',GETDATE(),GETDATE(),GETDATE(),1,0,0,1 )

			INSERT INTO PersonTable(PersonID, PersonFirstName, PersonLastName, PersonCode, AllowLogin, DepartmentID, UserTypeID, StatusID, Culture) 
				VALUES(@@IDENTITY, @CUSTODIANCODE, @CUSTODIANCODE, @CUSTODIANCODE, 0, @DepartmentID, 2, 1, 'en-GB')
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
			 IF EXISTS(SELECT PartyCode FROM PartyTable WHERE PartyCode=@SUPPLIERCODE and StatusID=1)
			 BEGIN 
				SELECT @SUPPLIERID =partyID FROM PartyTable WHERE PartyCode=@SUPPLIERCODE and StatusID=1 and PartyTypeID=1
			 END 
			 ELSE 
			 BEGIN 
			   IF UPPER(@ImportExcelNotAllowCreateReferenceFieldNewEntry)='FALSE'
		   BEGIN 	
				INSERT INTO PartyTable(PartyCode,STATUSID,CREATEDBY,CREATEDDATETIME,PartyName,PartyTypeID)
				VALUES(@SUPPLIERCODE,1,@USERID,GETDATE(),@SupplierCode,(select PartyTypeID from PartyTypeTable where PartyType='Supplier'))
				
				SET @SUPPLIERID = SCOPE_IDENTITY()
				--INSERT INTO part(SUPPLIERID,SUPPLIERDESCRIPTION,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
				--Select @SUPPLIERID,@SUPPLIERCODE,LanguageID,@USERID,GETDATE() from LanguageTable
				END 
			ELSE 
			BEGIN
			   INSERT INTO @MESSAGETABLE(TEXT)VALUES('SupplierCode Cant allow Create Data')
			END 
				END
			 END 
	 IF(@ASSETDESCRIPTION is not NULL and @ASSETDESCRIPTION!='')                                                                                                                                                                                                                                                            
			BEGIN 
			 IF EXISTS(SELECT Productdescription FROM ProductDescriptionTable a join ProductTable b on a.ProductId=B.ProductID WHERE replace(ProductDescription,' ','')=@ProductCode AND LANGUAGEID=1 
				AND CATEGORYID in (SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1) and StatusID=1)
			 BEGIN
			 --print 'ProductID'
				SELECT @PRODUCTID=b.PRODUCTID FROM ProductDescriptionTable a join ProductTable b on a.ProductId=B.ProductID WHERE replace(ProductDescription,' ','')=@ProductCode AND LANGUAGEID=1 
				AND CATEGORYID in (SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1) and StatusID=1
				--FROM PRODUCTVIEW WHERE replace(productName,' ','')=@ProductCode AND LANGUAGEID=1 and StatusID=1
				--AND CATEGORYID in (SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1)
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
				   
				 INSERT INTO PRODUCTTABLE(PRODUCTCODE,STATUSID,CATEGORYID,CREATEDBY,CREATEDDATETIME,ProductName)
				VALUES(@ProductCode,1,@CATEGORYID,@USERID,getdate(),@ProductCode)
				 
				 SET @ProductID = SCOPE_IDENTITY()
				 
				 INSERT INTO PRODUCTDESCRIPTIONTABLE (PRODUCTDESCRIPTION,PRODUCTID,LANGUAGEID,CREATEDBY,CREATEDDATETIME)
				 Select @ProductCode,@ProductID,LanguageID,@USERID,getdate() from LanguageTable
				

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
						CATEGORYID=(SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1) and StatusID=1)
			   BEGIN 
					SELECT @MANUFACTURERID =MANUFACTURERID FROM MANUFACTURERTABLE WHERE MANUFACTURERCODE=@MANUFACTURERCODE AND 
						CATEGORYID=(SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1) and StatusID=1
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
			
			 IF EXISTS(SELECT MODELCODE FROM  MODELTABLE WHERE MODELCODE=@MODELCODE and StatusID=1 AND CATEGORYID=(SELECT CATEGORYID FROM CATEGORYTABLE C WHERE C.CATEGORYCODE=@CATEGORYCODE and StatusID=1) 
			    AND MANUFACTURERID=(SELECT MANUFACTURERID FROM MANUFACTURERTABLE WHERE  MANUFACTURERCODE=@MANUFACTURERCODE and CATEGORYID=(SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1)  and StatusID=1))
				BEGIN 
				
				  SELECT @MODELID=MODELID FROM MODELTABLE WHERE CATEGORYID=(SELECT CATEGORYID FROM CATEGORYTABLE C WHERE C.CATEGORYCODE=@CATEGORYCODE and StatusID=1) 
			    AND MANUFACTURERID=(SELECT MANUFACTURERID FROM MANUFACTURERTABLE WHERE  MANUFACTURERCODE=@MANUFACTURERCODE and CATEGORYID=(SELECT CATEGORYID FROM CATEGORYTABLE WHERE CATEGORYCODE=@CATEGORYCODE and StatusID=1)  and StatusID=1) and StatusID=1
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
			    AND MANUFACTURERID=(SELECT MANUFACTURERID FROM MANUFACTURERTABLE WHERE  MANUFACTURERCODE=@MANUFACTURERCODE and StatusID=1))
				BEGIN 
				  SELECT @MODELID=MODELID FROM MODELTABLE WHERE  MODELCODE=@MODELCODE  AND 
			     MANUFACTURERID=(SELECT MANUFACTURERID FROM MANUFACTURERTABLE WHERE  MANUFACTURERCODE=@MANUFACTURERCODE and StatusID=1) and StatusID=1
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
			IF @ImportTypeID=1 
			BEGIN 
			 IF UPPER(@LocationMandatory)='TRUE' AND (@LOCATIONID IS NULL OR  @LOCATIONID ='') 
			 BEGIN 
				INSERT INTO @MESSAGETABLE(TEXT)VALUES('lOCATIONCODE MANDATORY DATA FOR'+@Barcode)
			 END 
			ELSE 
			BEGIN
		       If exists(select barcode from assettable where barcode=@barcode )
			   Begin 
			     INSERT INTO @MESSAGETABLE(TEXT)VALUES('Already Exists this barcode :'+@Barcode)
			   End 
			   Else 
			   Begin 
			   print 'Insert'
			      If((@Barcode is not null and @Barcode!='') and (@AssetCode is not null and @AssetCode!='') and (@ProductID is not null and @ProductID!='') and (@CategoryID is not null and @CategoryID!='')
				  and (@ASSETDESCRIPTION is not null and @ASSETDESCRIPTION!=''))

				  Begin 
					INSERT INTO ASSETTABLE (ASSETCODE,BARCODE,DEPARTMENTID,SECTIONID,CUSTODIANID,CATEGORYID,LOCATIONID,PRODUCTID,MANUFACTURERID,MODELID,SUPPLIERID,ASSETCONDITIONID,PurchasePrice,
					PONUMBER,REFERENCECODE,SERIALNO,ASSETDESCRIPTION,DELIVERYNOTE,RFIDTAGCODE,INVOICENO,VOUCHERNO,MAKE,CAPACITY,MAPPEDASSETID,RECEIPTNUMBER,PURCHASEDATE,ComissionDate,WarrantyExpiryDate,
					STATUSID,COMPANYID,CreatedBy,CreatedDateTime,AssetApproval,CreateFromHHT,ERPUpdateType,DepreciationFlag,Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,Attribute6,Attribute7,Attribute8,Attribute9,Attribute10,Attribute11,Attribute12,
					Attribute13,Attribute14,Attribute15,Attribute16,AssetRemarks,QFAssetCode,A.Attribute17,A.Attribute18,Attribute19,Attribute20,Attribute21,Attribute22,Attribute23,Attribute24,Attribute25,Attribute26,
Attribute27,Attribute28,Attribute29,Attribute30,Attribute31,Attribute32,Attribute33,Attribute34,Attribute35,Attribute36,Attribute37,Attribute38,Attribute39,Attribute40)
					
					VALUES( CASE WHEN UPPER(@BarcodeAutoGenerateEnable)='TRUE' and  UPPER(@IsBarcodeSettingApplyImportAsset)='TRUE' THEN '-' ELSE 
					CASE WHEN  UPPER(@BarcodeEqualAssetCode)='TRUE' and  UPPER(@IsBarcodeSettingApplyImportAsset)='TRUE' THEN @Barcode ELSE @AssetCode END END ,
					CASE WHEN UPPER(@BarcodeAutoGenerateEnable)='TRUE' and  UPPER(@IsBarcodeSettingApplyImportAsset)='TRUE' THEN '-' ELSE @Barcode END
					,@DepartmentID,@SECTIONID,@CUSTODIANID,@CATEGORYID,@LOCATIONID,@PRODUCTID,@MANUFACTURERID,@MODELID,@SUPPLIERID,@ASSETCONDITIONID,case when @PurchasePrice is not null and  @PurchasePrice!=' '  then convert(decimal(18,5),@PurchasePrice) else 0 end ,@PONUMBER,@REFERENCECODE,@SERIALNO,
					@ASSETDESCRIPTION,@DELIVERYNOTE,@RFIDTAGCODE,@INVOICENO,@VOUCHERNO,@MAKE,@CAPACITY,@MAPPEDASSETID,@RECEIPTNUMBER,case when @PURCHASEDATE is not null then CONVERT(DATETIME,@PURCHASEDATE,103) else NULL end ,
					case when @ComissionDate is not null then CONVERT(DATETIME,@ComissionDate,103) else null end , case when @WarrantyExpiryDate is not null then CONVERT(DATETIME,@WarrantyExpiryDate,103) else NULL end ,
					1,@companyID,@userID,getdate(),1,0,0,0,@Attribute1,@Attribute2,@Attribute3,@Attribute4,@Attribute5,
			@Attribute6,@Attribute7,@Attribute8,@Attribute9,@Attribute10,@Attribute11,@Attribute12,
			@Attribute13,@Attribute14,@Attribute15,@Attribute16,@AssetRemarks,@QFAssetCode,@Attribute17,@Attribute18,@Attribute19,@Attribute20,@Attribute21,@Attribute22,@Attribute23,@Attribute24,@Attribute25,@Attribute26,
@Attribute27,@Attribute28,@Attribute29,@Attribute30,@Attribute31,@Attribute32,@Attribute33,@Attribute34,@Attribute35,@Attribute36,@Attribute37,@Attribute38,@Attribute39,@Attribute40)
			End 
			Else 
			Begin 
			  INSERT INTO @MESSAGETABLE(TEXT)VALUES('Mandatory field are passed with empty '+@Barcode)
			End 
			 End 
			END 
			END 
			ELSE 
			BEGIN 
			IF EXISTS (SELECT barcode FROM dbo.AssetTable WHERE Barcode=@Barcode)
			BEGIN
				UPDATE ASSETTABLE SET
				--ASSETCODE=CASE WHEN UPPER(@BarcodeEqualAssetCode)='TRUE' THEN ISNULL(@Barcode,ASSETCODE) ELSE ISNULL(@ASSETCODE,ASSETCODE) END,
				ASSETCODE=CASE WHEN @ASSETCODE is null or @ASSETCODE='' then AssetCode else @ASSETCODE end,
				BARCODE=ISNULL(@Barcode,BARCODE),
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
				PURCHASEDATE=CASE WHEN @PURCHASEDATE IS NULL OR RTRIM(LTRIM(@PURCHASEDATE))='' THEN PURCHASEDATE ELSE CONVERT(DATETIME,@PURCHASEDATE,103)  END,
				ComissionDate=CASE WHEN @ComissionDate IS NULL OR RTRIM(LTRIM(@ComissionDate))='' THEN ComissionDate ELSE CONVERT(DATETIME,@ComissionDate,103) END,
				WarrantyExpiryDate=CASE WHEN @WarrantyExpiryDate IS NULL OR RTRIM(LTRIM(@WarrantyExpiryDate))='' THEN WarrantyExpiryDate ELSE CONVERT(DATETIME,@WarrantyExpiryDate,103) END,			
				Attribute1=CASE WHEN @Attribute1 IS NULL OR RTRIM(LTRIM(@Attribute1))='' THEN Attribute1 ELSE @Attribute1 END,
				Attribute2=CASE WHEN @Attribute2 IS NULL OR RTRIM(LTRIM(@Attribute2))='' THEN Attribute2 ELSE @Attribute2 END,
				Attribute3=CASE WHEN @Attribute3 IS NULL OR RTRIM(LTRIM(@Attribute3))='' THEN Attribute3 ELSE @Attribute3 END,
				Attribute4=CASE WHEN @Attribute4 IS NULL OR RTRIM(LTRIM(@Attribute4))='' THEN Attribute4 ELSE @Attribute4 END,
				Attribute5=CASE WHEN @Attribute5 IS NULL OR RTRIM(LTRIM(@Attribute5))='' THEN Attribute5 ELSE @Attribute5 END,
				Attribute6=CASE WHEN @Attribute6 IS NULL OR RTRIM(LTRIM(@Attribute6))='' THEN Attribute6 ELSE @Attribute6 END,
				Attribute7=CASE WHEN @Attribute7 IS NULL OR RTRIM(LTRIM(@Attribute7))='' THEN Attribute7 ELSE @Attribute7 END,
				Attribute8=CASE WHEN @Attribute8 IS NULL OR RTRIM(LTRIM(@Attribute8))='' THEN Attribute8 ELSE @Attribute8 END,
				Attribute9=CASE WHEN @Attribute9 IS NULL OR RTRIM(LTRIM(@Attribute9))='' THEN Attribute9 ELSE @Attribute9 END,
				Attribute10=CASE WHEN @Attribute10 IS NULL OR RTRIM(LTRIM(@Attribute10))='' THEN Attribute10 ELSE @Attribute10 END,
				Attribute11=CASE WHEN @Attribute11 IS NULL OR RTRIM(LTRIM(@Attribute11))='' THEN Attribute11 ELSE @Attribute11 END,
				Attribute12=CASE WHEN @Attribute12 IS NULL OR RTRIM(LTRIM(@Attribute12))='' THEN Attribute12 ELSE @Attribute12 END,
				Attribute13=CASE WHEN @Attribute13 IS NULL OR RTRIM(LTRIM(@Attribute13))='' THEN Attribute13 ELSE @Attribute13 END,
				Attribute14=CASE WHEN @Attribute14 IS NULL OR RTRIM(LTRIM(@Attribute14))='' THEN Attribute14 ELSE @Attribute14 END,
				Attribute15=CASE WHEN @Attribute15 IS NULL OR RTRIM(LTRIM(@Attribute15))='' THEN Attribute15 ELSE @Attribute15 END,
				Attribute16=CASE WHEN @Attribute16 IS NULL OR RTRIM(LTRIM(@Attribute16))='' THEN Attribute16 ELSE @Attribute16 END,
				AssetRemarks=CASE WHEN @AssetRemarks IS NULL OR RTRIM(LTRIM(@AssetRemarks))='' THEN AssetRemarks ELSE @AssetRemarks END,
				QFAssetCode=CASE WHEN @QFAssetCode IS NULL OR RTRIM(LTRIM(@QFAssetCode))='' THEN QFAssetCode ELSE @QFAssetCode END,
				LastModifiedDateTime=getdate(),
				LastModifiedBy=@UserID ,
				Attribute17=CASE WHEN @Attribute17 IS NULL OR RTRIM(LTRIM(@Attribute17))='' THEN Attribute17 ELSE @Attribute17 END,
				Attribute18=CASE WHEN @Attribute18 IS NULL OR RTRIM(LTRIM(@Attribute18))='' THEN Attribute18 ELSE Attribute18 END,
				Attribute19=CASE WHEN @Attribute19 IS NULL OR RTRIM(LTRIM(@Attribute19))='' THEN Attribute19 ELSE @Attribute19 END,
				Attribute20=CASE WHEN @Attribute20 IS NULL OR RTRIM(LTRIM(@Attribute20))='' THEN Attribute20 ELSE @Attribute20 END,
			    Attribute21=CASE WHEN @Attribute21 IS NULL OR RTRIM(LTRIM(@Attribute21))='' THEN Attribute21 ELSE @Attribute21 END,
				Attribute22=CASE WHEN @Attribute22 IS NULL OR RTRIM(LTRIM(@Attribute22))='' THEN Attribute22 ELSE @Attribute22 END,
				Attribute23=CASE WHEN @Attribute23 IS NULL OR RTRIM(LTRIM(@Attribute23))='' THEN Attribute23 ELSE @Attribute23 END,
				Attribute24=CASE WHEN @Attribute24 IS NULL OR RTRIM(LTRIM(@Attribute24))='' THEN Attribute24 ELSE @Attribute24 END,
				Attribute25=CASE WHEN @Attribute25 IS NULL OR RTRIM(LTRIM(@Attribute25))='' THEN Attribute25 ELSE @Attribute25 END,
				Attribute26=CASE WHEN @Attribute26 IS NULL OR RTRIM(LTRIM(@Attribute26))='' THEN Attribute26 ELSE @Attribute26 END,
				Attribute27=CASE WHEN @Attribute27 IS NULL OR RTRIM(LTRIM(@Attribute27))='' THEN Attribute27 ELSE @Attribute27 END,
				Attribute28=CASE WHEN @Attribute28 IS NULL OR RTRIM(LTRIM(@Attribute28))='' THEN Attribute28 ELSE @Attribute28 END,
				Attribute29=CASE WHEN @Attribute29 IS NULL OR RTRIM(LTRIM(@Attribute29))='' THEN Attribute29 ELSE @Attribute29 END,
				Attribute30=CASE WHEN @Attribute30 IS NULL OR RTRIM(LTRIM(@Attribute30))='' THEN Attribute30 ELSE @Attribute30 END,
				Attribute31=CASE WHEN @Attribute31 IS NULL OR RTRIM(LTRIM(@Attribute31))='' THEN Attribute31 ELSE @Attribute31 END,
				Attribute32=CASE WHEN @Attribute32 IS NULL OR RTRIM(LTRIM(@Attribute32))='' THEN Attribute32 ELSE @Attribute32 END,
				Attribute33=CASE WHEN @Attribute33 IS NULL OR RTRIM(LTRIM(@Attribute33))='' THEN Attribute33 ELSE @Attribute33 END,
				Attribute34=CASE WHEN @Attribute34 IS NULL OR RTRIM(LTRIM(@Attribute34))='' THEN Attribute34 ELSE @Attribute34 END,
				Attribute35=CASE WHEN @Attribute35 IS NULL OR RTRIM(LTRIM(@Attribute35))='' THEN Attribute35 ELSE @Attribute35 END,
				Attribute36=CASE WHEN @Attribute36 IS NULL OR RTRIM(LTRIM(@Attribute36))='' THEN Attribute36 ELSE @Attribute36 END,
				Attribute37=CASE WHEN @Attribute37 IS NULL OR RTRIM(LTRIM(@Attribute37))='' THEN Attribute37 ELSE @Attribute37 END,
				Attribute38=CASE WHEN @Attribute38 IS NULL OR RTRIM(LTRIM(@Attribute38))='' THEN Attribute38 ELSE @Attribute38 END,
				Attribute39=CASE WHEN @Attribute39 IS NULL OR RTRIM(LTRIM(@Attribute39))='' THEN Attribute39 ELSE @Attribute39 END,
				Attribute40=CASE WHEN @Attribute40 IS NULL OR RTRIM(LTRIM(@Attribute40))='' THEN Attribute40 ELSE @Attribute40 END
				 WHERE BARCODE=@BARCODE 
			 END
			 ELSE
			 BEGIN 
				 INSERT INTO @MESSAGETABLE(TEXT)VALUES(@Barcode + '- This barcode not available in Assettable')
			 END 
			END 
			--select * from @MESSAGETABLE

			Select @ReturnMessage = COALESCE(@ReturnMessage + ', ' + Text, Text)  from @MESSAGETABLE
			select @ReturnMessage as ReturnMessage
        
			 
End
go 
update ImportTemplateNewTable set IsMandatory=0,IsUnique=0 where EntityID=(select entityId from EntityTable where EntityName='AssetTransfer') and ImportField not in ('Barcode','RoomCode')
go 
update ImportTemplateNewTable set IsUnique=0 where EntityID=(select entityId from EntityTable where EntityName='AssetTransfer') and ImportField  in ('RoomCode')
go 

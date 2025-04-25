
IF OBJECT_ID('EntityTable') IS NULL
BEGIN
  Create table EntityTable
  (
    EntityID int not null primary key identity(1,1),
	EntityName nvarchar(100) not null
  )
end 
Go 


IF  EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'EntityName' AND Object_ID = Object_ID(N'ImportTemplateTable'))
Begin 
alter table ImportTemplateTable drop column EntityName
END
GO

IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'EntityID' AND Object_ID = Object_ID(N'ImportTemplateTable'))
Begin 
alter table ImportTemplateTable add EntityID int not null foreign key references EntityTable(EntityID)
END
GO

IF  EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'EntityName' AND Object_ID = Object_ID(N'ImportFormatTable'))
Begin 
alter table ImportFormatTable drop column EntityName
END
GO

IF Not EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'EntityID' AND Object_ID = Object_ID(N'ImportFormatTable'))
Begin 
alter table ImportFormatTable add EntityID int not null foreign key references EntityTable(EntityID)
END
GO
Select * from ImportFormatTable
select * from importtemplatetable
select * from MasterGridTable

select * from MasterGridLineItemTable where MasterGridID=4023

update MasterGridLineItemTable set FieldName='Entity.EntityName' where FieldName='EntityName'

If not exists (select * from importTemplateTable where Entityid in (select  EntityID from EntityTable where EntityName='AssetCondition'))
Begin 
insert into ImportTemplateTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
SELECT COLUMN_NAME,ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS displayOrderID, 1,case when IS_NULLABLE='No' then 1 else 0 end ,100,NULL,0,100,1,
case when COLUMN_NAME=replace(table_name,'table','Code') then 1 else 0 end,NULL,(select EntityID from EntityTable where EntityName='AssetCondition')
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME='AssetConditionTable' 
 and COLUMN_NAME not like 'Attribute%' and COLUMN_NAME not like 'Create%'  and COLUMN_NAME not like 'Last%'
 and COLUMN_NAME not like 'Status%' and COLUMN_NAME not in (replace(table_name,'table','ID'))

 insert into ImportTemplateTable(ImportField,DispalyOrderID,IsDisplay,IsMandatory,DataSize,DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID,EntityID)
 SELECT COLUMN_NAME,ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS displayOrderID, case when COLUMN_NAME=replace(table_name,'table','Code') then 1 else 0 end ,case when IS_NULLABLE='No' then 1 else 0 end ,100,NULL,0,100,2,
case when COLUMN_NAME=replace(table_name,'table','Code') then 1 else 0 end ,NULL,(select EntityID from EntityTable where EntityName='AssetCondition')
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME='AssetConditionTable' 
 and COLUMN_NAME not like 'Attribute%' and COLUMN_NAME not like 'Create%'  and COLUMN_NAME not like 'Last%'
 and COLUMN_NAME not like 'Status%' and COLUMN_NAME not in (replace(table_name,'table','ID'))
 End 


 SELECT COLUMN_NAME,ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS displayOrderID, 1,case when IS_NULLABLE='No' then 1 else 0 end ,DATA_TYPE,*
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME='AssetConditionTable' 
 and COLUMN_NAME not like 'Attribute%' and COLUMN_NAME not like 'Create%'  and COLUMN_NAME not like 'Last%'
 and COLUMN_NAME not like 'Status%' and COLUMN_NAME not in (replace(table_name,'table','ID'))

SELECT 
    c.name AS column_name,
    i.name AS index_name,
    c.is_identity
FROM sys.indexes i
    inner join sys.index_columns ic  ON i.object_id = ic.object_id AND i.index_id = ic.index_id
    inner join sys.columns c ON ic.object_id = c.object_id AND c.column_id = ic.column_id
WHERE --i.is_primary_key = 1and
     
	i.object_ID = OBJECT_ID('AssetConditionTable');
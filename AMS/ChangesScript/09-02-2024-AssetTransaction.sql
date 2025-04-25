IF OBJECT_ID('AssetTransactionTable') IS NULL
BEGIN
CREATE TABLE [dbo].[AssetTransactionTable](
	[AssetTransactionID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY ,
	[TransactionNo] [varchar](250) NOT NULL,
	[TransactionDate] [datetime] NOT NULL DEFAULT  getdate(),
	[ReferenceNo] [nvarchar](100),
	[Remarks] [varchar](1000) NULL,
	[StatusID] [int] NOT NULL REFERENCES [dbo].[StatusTable] ([StatusID]),
	[CreatedBy] [int] NOT NULL REFERENCES [dbo].[User_LoginUserTable] ([UserID]),
	[CreatedDateTime] [smalldatetime] NOT NULL,
	[LastModifiedBy] [int] NULL REFERENCES [dbo].[User_LoginUserTable] ([UserID]),
	[LastModifiedDateTime] [smalldatetime] NULL
	)
END
GO

IF OBJECT_ID('AssetTransactionLineItemTable') IS NULL
BEGIN
CREATE TABLE [dbo].[AssetTransactionLineItemTable](
	[AssetTransactionLineItemID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[AssetTransactionID] [int] NOT NULL REFERENCES [dbo].[AssetTransactionTable] ([AssetTransactionID]),
	[AssetID] [int] NOT NULL REFERENCES AssetTable (AssetID),
	[Remarks] [varchar](1000) NOT NULL,
	AdjustmentValue int NOT NULL,
	[StatusID] [int] NOT NULL REFERENCES [dbo].[StatusTable] ([StatusID]),
	[CreatedBy] [int] NOT NULL REFERENCES [dbo].[User_LoginUserTable] ([UserID]),
	[CreatedDateTime] [smalldatetime] NOT NULL,
	[LastModifiedBy] [int] NULL REFERENCES [dbo].[User_LoginUserTable] ([UserID]),
	[LastModifiedDateTime] [smalldatetime] NULL,
) 
END
GO



If not exists(select RightName from User_RightTable where RightName='AssetTransaction')
Begin 
insert into User_RightTable(RightName,RightDescription,ValueType,DisplayRight,RightGroupID,ISActive,IsDeleted)
Values('AssetTransaction','AssetTransaction',255,1,(select RightGroupID from User_RightGroupTable where RightGroupName='Transaction'),1,0)
End
go 


If not exists(select MenuName from User_MenuTable where MenuName='AssetTransaction' and TargetObject='/AssetTransaction/Index?pageName=AssetTransaction')
Begin 
insert into User_MenuTable(MenuName,RightID,TargetObject,ParentMenuID,OrderNo)
Values('AssetTransaction',(select RightID from User_RightTable where RightName='AssetTransaction'),'/AssetTransaction/Index?pageName=AssetTransaction',
(Select MenuID from User_MenuTable where MenuName='Transaction'),6)
End
go 


IF NOT EXists(select * from EntityCodeTable where EntityCodeName='AssetMaintenance')
BEGIN
INSERT into EntityCodeTable(EntityCodeName,CodePrefix,CodeFormatString,LastUsedNo)Values('AssetMaintenance','AM','{0:00000}',0)
END

go
Alter table AssetTransactionTableAdd VendorID int References PartyTable(PartyID)Alter table AssetTransactionTableAdd ServiceDoneBy nvarchar(200) Alter table AssetTransactionLineItemTableAdd IsAdjustmentNetBook bit Default 0 NOT NULLIF OBJECT_ID('CurrencyConversionTable') IS NULL
BEGIN
CREATE TABLE CurrencyConversionTable(
	[CurrencyConversionID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY ,
	FromCurrencyID int NOT NULL References CurrencyTable(CurrencyID),
	ToCurrencyID int NOT NULL References CurrencyTable(CurrencyID),
	CurrencyStartDate DateTime NOT NULL Default GetDate(),
	CurrencyEndDate DateTime,
	ConversionValue Decimal(18,5) NOT NULL Default 0,
	[StatusID] [int] NOT NULL REFERENCES [dbo].[StatusTable] ([StatusID]) Default 50,
	[CreatedBy] [int] NOT NULL REFERENCES [dbo].[User_LoginUserTable] ([UserID]),
	[CreatedDateTime] [smalldatetime] NOT NULL DEFAULT GetDate(),
	[LastModifiedBy] [int] NULL REFERENCES [dbo].[User_LoginUserTable] ([UserID]),
	[LastModifiedDateTime] [smalldatetime] NULL
	)
END
GO


If not exists(select RightName from User_RightTable where RightName='CurrencyConversion')
Begin 
insert into User_RightTable(RightName,RightDescription,ValueType,DisplayRight,RightGroupID,ISActive,IsDeleted)
Values('CurrencyConversion','CurrencyConversion',255,1,(select RightGroupID from User_RightGroupTable where RightGroupName='Master'),1,0)
End
go 


If not exists(select MenuName from User_MenuTable where MenuName='CurrencyConversion' and TargetObject='/MasterPage/Index?pageName=CurrencyConversion')
Begin 
insert into User_MenuTable(MenuName,RightID,TargetObject,ParentMenuID,OrderNo)
Values('CurrencyConversion',(select RightID from User_RightTable where RightName='CurrencyConversion'),'/MasterPage/Index?pageName=CurrencyConversion',
(Select MenuID from User_MenuTable where MenuName='Master'),6)
End
go 

update User_MenuTable set ParentTransactionId=1 where ParentMenuID=(Select MenuID from User_MenuTable where MenuName='Master') and MenuName='CurrencyConversion'



CREATE TRIGGER [dbo].[trg_CurrencyConversionEndDateUpdate]
     ON  CurrencyConversionTable
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		DECLARE @FromCurrencyID int
	DECLARE @ToCurrencyID int

	DECLARE cursorObj CURSOR FOR
	Select FromCurrencyID,ToCurrencyID from Inserted 
	OPEN cursorObj
		FETCH NEXT FROM cursorObj INTO @FromCurrencyID, @ToCurrencyID
		WHILE @@FETCH_STATUS = 0
			BEGIN
				
					BEGIN
						
						 UPDATE CurrencyConversionTable SET CurrencyEndDate=GETDATE(), StatusID=100
							FROM CurrencyConversionTable AL
							INNER JOIN inserted i on i.FromCurrencyID = AL.FromCurrencyID AND i.ToCurrencyID=AL.ToCurrencyID
							AND i.CurrencyConversionID!=AL.CurrencyConversionID
							where AL.StatusID=50 AND AL.CurrencyEndDate is null 
					END
				
		 FETCH NEXT FROM cursorObj INTO @FromCurrencyID, @ToCurrencyID
			END
	CLOSE cursorObj
	DEALLOCATE cursorObj
END
GO

IF not exists(Select ControlName from ASelectionControlQueryTable where ControlName='CurrencySelection')BEGININSERT INTO [dbo].[ASelectionControlQueryTable]
           ([ControlName]
           ,[ControlType]
           ,[Query]
           ,[DisplayFields]
           ,[SearchFields]
           ,[OrderByQuery]
           ,[SelectedItemDisplayField]
           ,[ValueFieldName])
     VALUES
         ('CurrencySelection','MultiColumnComboBox','Select CurrencyID,CurrencyCode from CurrencyTable','CurrencyCode','CurrencyCode','ORDER by CurrencyCode','CurrencyCode','CurrencyID')
ENDGOALTER TRIGGER [dbo].[trg_CurrencyConversionEndDateUpdate]
     ON  CurrencyConversionTable
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		DECLARE @FromCurrencyID int
	DECLARE @ToCurrencyID int
		DECLARE @CurrencyConversionID int
	DECLARE cursorObj CURSOR FOR
	Select FromCurrencyID,ToCurrencyID,CurrencyConversionID from Inserted 
	OPEN cursorObj
		FETCH NEXT FROM cursorObj INTO @FromCurrencyID, @ToCurrencyID,@CurrencyConversionID
		WHILE @@FETCH_STATUS = 0
			BEGIN
				
					BEGIN
						
						 UPDATE CurrencyConversionTable SET CurrencyEndDate=GETDATE(), StatusID=100
							FROM CurrencyConversionTable AL
							WHERE FromCurrencyID=@FromCurrencyID AND ToCurrencyID=@ToCurrencyID
							AND AL.CurrencyConversionID!=@CurrencyConversionID
							AND  AL.StatusID=1 AND AL.CurrencyEndDate is null 

					END
				
		 FETCH NEXT FROM cursorObj INTO @FromCurrencyID, @ToCurrencyID,@CurrencyConversionID
			END
	CLOSE cursorObj
	DEALLOCATE cursorObj
END
GOIF NOt exists(select * from [dbo].[MasterGridNewLineItemTable] where MasterGridID in (select MasterGridID from MasterGridNewTable where MasterGridName='CurrencyConversion')
AND FieldName='FromCurrency.CurrencyCode')
BEGIN
	INSERT into MasterGridNewLineItemTable (MasterGridID,FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType)
	Values((select MasterGridID from MasterGridNewTable where MasterGridName='CurrencyConversion'),
	'FromCurrency.CurrencyCode','FromCurrencyCode',10,null,1,1,'System.String')
END

IF NOt exists(select * from [dbo].[MasterGridNewLineItemTable] where MasterGridID in (select MasterGridID from MasterGridNewTable where MasterGridName='CurrencyConversion')
AND FieldName='ToCurrency.CurrencyCode')
BEGIN
	INSERT into MasterGridNewLineItemTable (MasterGridID,FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType)
	Values((select MasterGridID from MasterGridNewTable where MasterGridName='CurrencyConversion'),
	'ToCurrency.CurrencyCode','ToCurrencyCode',10,null,1,2,'System.String')
END
IF  exists(select * from [dbo].[MasterGridNewLineItemTable] where MasterGridID in (select MasterGridID from MasterGridNewTable where MasterGridName='CurrencyConversion')
And FieldName='CurrencyEndDate')
BEGIN
Update MasterGridNewLineItemTable set Format='dd/MM/yyyy hh:mm:ss' where MasterGridID in (select MasterGridID from MasterGridNewTable where MasterGridName='CurrencyConversion')
And FieldName='CurrencyEndDate'
END
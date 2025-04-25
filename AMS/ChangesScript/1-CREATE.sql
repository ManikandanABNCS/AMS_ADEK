
IF OBJECT_ID('CompanyDescriptionTable') IS NULL
BEGIN
  Create table CompanyDescriptionTable
  (
    CompanyDescriptionID int not null primary key identity(1,1),
	CompanyID int not null foreign key references CompanyTable(CompanyID),
	CompanyDescription nvarchar(max) not null, 
	LanguageID int not null foreign key references LanguageTable(LanguageID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null,
	LastModifiedBy int  null foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDatetime
  )
End 
Go 

IF OBJECT_ID('DepartmentTable') IS NULL
BEGIN
  Create table DepartmentTable
  (
    DepartmentID int not null primary key identity(1,1),
	DepartmentCode nvarchar(max) Not NULL,
	StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
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

IF OBJECT_ID('DepartmentDescriptionTable') IS NULL
BEGIN
  Create table DepartmentDescriptionTable
  (
     DepartmentDescriptionID int not null primary key identity(1,1),
	 DepartmentID int not null foreign key references DepartmentTable(DepartmentID),
	 DepartmentDescription nvarchar(max) not null, 
	 LanguageID int not null foreign key references LanguageTable(LanguageID),
	 CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL
  )
End 
Go 
IF OBJECT_ID('WarehouseTable') IS NULL
BEGIN
  Create table WarehouseTable
  (
     WarehouseID int not null primary key identity(1,1),
	 WarehouseCode nvarchar(100) not null, 
	 StatusID int not null foreign key references StatusTable(StatusID),
	 CreatedBy int not null foreign key references PersonTable(PersonID),
	 CreatedDateTime SmallDatetime not null, 
	 LastModifiedBy int foreign key references PersonTable(PersonID),
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
IF OBJECT_ID('ModelTable') IS NULL
BEGIN
  Create table ModelTable
  (
    ModelID int not null primary key identity(1,1),
	ModelCode nvarchar(max) Not NULL,
	PartyID int not null foreign key references PartyTable(PartyID),
	CategoryID int null foreign key references CategoryTable(CategoryID),
	StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
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
IF OBJECT_ID('ModelDescriptionTable') IS NULL
BEGIN
  Create table ModelDescriptionTable
  (
    ModelDescriptionID int not null primary key identity(1,1),
	ModelID int not null foreign key references ModelTable(ModelID),
	ModelDescription nvarchar(max) not null,
	LanguageID int not null foreign key references LanguageTable(LanguageID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL
  )
End 
Go 

IF OBJECT_ID('WarehouseDescriptionTable') IS NULL
BEGIN
  Create table WarehouseDescriptionTable
  (
     WarehouseDescriptionID int not null primary key identity(1,1),
	 WarehouseDescription nvarchar(max) not null, 
	 WarehouseID int not null Foreign Key references WarehouseTable(WarehouseID),
	 CreatedBy int not null foreign key references PersonTable(PersonID),
	 CreatedDateTime SmallDatetime not null, 
	 LastModifiedBy int foreign key references PersonTable(PersonID),
	 LastModifiedDateTime SmallDateTime NULL
  )
End 
Go 
IF OBJECT_ID('CountryTable') IS NULL
BEGIN
  Create table CountryTable
  (
     CountryID int not null primary key identity(1,1),
	 CountryCode nvarchar(100) not null, 
	 StatusID int not null foreign key references StatusTable(StatusID),
	 CreatedBy int not null foreign key references PersonTable(PersonID),
	 CreatedDateTime SmallDatetime not null, 
	 LastModifiedBy int foreign key references PersonTable(PersonID),
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
  IF OBJECT_ID('CountryDescriptionTable') IS NULL
BEGIN
  Create table CountryDescriptionTable
  (
     CountryDescriptionID int not null primary key identity(1,1),
	 CountryDescription nvarchar(max) not null, 
	 CountryID int not null Foreign Key references CountryTable(CountryID),
	 CreatedBy int not null foreign key references PersonTable(PersonID),
	 CreatedDateTime SmallDatetime not null, 
	 LastModifiedBy int foreign key references PersonTable(PersonID),
	 LastModifiedDateTime SmallDateTime NULL
  )
End 
Go 
IF OBJECT_ID('CurrencyTable') IS NULL
BEGIN
  Create table CurrencyTable
  (
    CurrencyID int not null Primary key identity(1,1),
	IsDefaultCurrency bit not null, 
	CurrencyCode nvarchar(100) not null,
	CountryID int not null foreign key references CountryTable(CountryID),
	NoOfDecimalDigits int not null, 
	StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL

  )
End 
Go 
IF OBJECT_ID('CurrencyDescriptionTable') IS NULL
BEGIN
  Create table CurrencyDescriptionTable
  (
     CurrencyDescriptionID int not null primary key identity(1,1),
	 CurrencyDescription nvarchar(max) not null, 
	 CurrencyID int not null Foreign Key references CurrencyTable(CurrencyID),
	 CreatedBy int not null foreign key references PersonTable(PersonID),
	 CreatedDateTime SmallDatetime not null, 
	 LastModifiedBy int foreign key references PersonTable(PersonID),
	 LastModifiedDateTime SmallDateTime NULL
  )
 End 
 Go 

 IF OBJECT_ID('CostTypeTable') IS NULL
BEGIN
  Create table CostTypeTable
  (
    CostTypeID int not null Primary key identity(1,1),
	 CostTypeCode nvarchar(100) not null, 
	 StatusID int not null foreign key references StatusTable(StatusID),
	 CreatedBy int not null foreign key references PersonTable(PersonID),
	 CreatedDateTime SmallDatetime not null, 
	 LastModifiedBy int foreign key references PersonTable(PersonID),
	 LastModifiedDateTime SmallDateTime NULL

  )
End 
Go 
IF OBJECT_ID('CostTypeDescriptionTable') IS NULL
BEGIN
  Create table CostTypeDescriptionTable
  (
     CostTypeDescriptionID int not null primary key identity(1,1),
	 CostTypeDescription nvarchar(max) not null, 
	 CostTypeID int not null Foreign Key references CostTypeTable(CostTypeID),
	 CreatedBy int not null foreign key references PersonTable(PersonID),
	 CreatedDateTime SmallDatetime not null, 
	 LastModifiedBy int foreign key references PersonTable(PersonID),
	 LastModifiedDateTime SmallDateTime NULL
  )
 End 
 Go 
 IF OBJECT_ID('ExpenseTypeTable') IS NULL
BEGIN
  Create table ExpenseTypeTable
  (
    ExpenseTypeID int not null primary key identity(1,1),
	ExpenseTypeCode nvarchar(100) not null, 
	ExpenseDescription nvarchar(max) not null
  ) 
  End 
  Go 
   IF OBJECT_ID('ExchangeRateTable') IS NULL
BEGIN
  Create table ExchangeRateTable
  (
     ExchangeRateID int not null primary key identity(1,1),
	 EffectiveDate SmallDatetime not null,
	 CurrencyID int not null Foreign Key references CurrencyTable(CurrencyID),
	 ExchangeRate decimal(18,5) not null,
	 ToCurrencyID  int not null Foreign Key references CurrencyTable(CurrencyID),
	  StatusID int not null foreign key references StatusTable(StatusID),
	 CreatedBy int not null foreign key references PersonTable(PersonID),
	 CreatedDateTime SmallDatetime not null, 
	 LastModifiedBy int foreign key references PersonTable(PersonID),
	 LastModifiedDateTime SmallDateTime NULL
  ) 
 End 
  Go 
IF OBJECT_ID('ProjectTable') IS NULL
BEGIN
  Create table ProjectTable
  (
    ProjectID int not null primary key identity(1,1),
	ProjectCode nvarchar(max) Not NULL,
	ProjectManager nvarchar(max) Not NULL,
	ProjectStartDate SmallDatetime NULL,
	ProjectValue decimal(18,5) NULL, 
	ExchangeRate Decimal(18,5) NULL,
	StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
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

IF OBJECT_ID('ProjectDescriptionTable') IS NULL
BEGIN
  Create table ProjectDescriptionTable
  (
     ProjectDescriptionID int not null primary key identity(1,1),
	 ProjectID int not null foreign key references DepartmentTable(DepartmentID),
	 ProjectDescription nvarchar(max) not null, 
	 LanguageID int not null foreign key references LanguageTable(LanguageID),
	 CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL
  )
End 
Go 

IF OBJECT_ID('SectionTable') IS NULL
BEGIN
  Create table SectionTable
  (
    SectionID int not null primary key identity(1,1),
	SectionCode nvarchar(max) Not NULL,
	DepartmentID int not null foreign key references DepartmentTable(DepartmentID),
	StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
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
IF OBJECT_ID('SectionDescriptionTable') IS NULL
BEGIN
  Create table SectionDescriptionTable
  (
     SectionDescriptionID int not null primary key identity(1,1),
	 SectionID int not null foreign key references SectionTable(SectionID),
	 SectionDescription nvarchar(max) not null, 
	 LanguageID int not null foreign key references LanguageTable(LanguageID),
	 CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL
  )
End 
Go 

IF OBJECT_ID('DepreciationMethodTable') IS NULL
BEGIN
  Create table DepreciationMethodTable
  (
  DepreciationMethodID int not null primary key identity(1,1),
  MethodName nvarchar(200) Not null,
  Description nvarchar(max) Not null
  ) 

End 
Go 
IF OBJECT_ID('DepreciationClassTable') IS NULL
BEGIN
  Create table DepreciationClassTable
  (
    DepreciationClassID int not null primary key identity(1,1),
	ClassName nvarchar(max) not null, 
	DepreciationMethodID int not null foreign key references DepreciationMethodTable(DepreciationMethodID),
	StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL
  )
  End 
  Go 
  IF OBJECT_ID('DepreciationClassLineItemTable') IS NULL
BEGIN
	Create table DepreciationClassLineItemTable
	(
	  DepreciationClassLineItemID int not null primary key identity(1,1),
	  DepreciationClassID int not null foreign key references DepreciationClassTable(DepreciationClassID),
	  AssetStartValue nvarchar(100) Not null,
	  AssetEndValue nvarchar(100) Not null,
	  Duration nvarchar(100)  null,
	  DepreciationPercentage Decimal(18,5) NULL
	) 
	End 
Go 
IF OBJECT_ID('CategoryTable') IS NULL
BEGIN
  Create table CategoryTable
  (
    CategoryID int not null primary key identity(1,1),
	CategoryCode nvarchar(max) Not NULL,
	ParentCategoryID int  null foreign key references CategoryTable(CategoryID),
	StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL,
	BudgetYearID int null foreign key references BudgetYearTable(BudgetYearID),
	IsInventory bit default(0),
	ERPCategoryID int NULL,
	DepreciationClassID int not null foreign key references DepreciationClassTable(DepreciationClassID),
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
IF OBJECT_ID('CategoryDescriptionTable') IS NULL
BEGIN
  Create table CategoryDescriptionTable
  (
    CategoryDescriptionID int not null primary key identity(1,1),
	CategoryID int not null foreign key references CategoryTable(CategoryID),
	CategoryDescription nvarchar(max) not null,
	LanguageID int not null foreign key references LanguageTable(LanguageID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL
  )
End 
Go
IF OBJECT_ID('LocationTable') IS NULL
BEGIN
  Create table LocationTable
  (
    LocationID int not null primary key identity(1,1),
	LocationCode nvarchar(max) Not NULL,
	ParentLocationID int  null foreign key references LocationTable(LocationID),
	StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL,
	BudgetYearID int null foreign key references BudgetYearTable(BudgetYearID),
	IsWIPLocation bit default(0),
	OracleLocationID int NULL,
	CompanyID int not null foreign key references CompanyTable(CompanyID),
	IsStoreLocation bit NULL,
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
IF OBJECT_ID('LocationDescriptionTable') IS NULL
BEGIN
  Create table LocationDescriptionTable
  (
    LocationDescriptionID int not null primary key identity(1,1),
	LocationID int not null foreign key references LocationTable(LocationID),
	LocationDescription nvarchar(max) not null,
	LanguageID int not null foreign key references LanguageTable(LanguageID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL
  )
End 
Go 
IF OBJECT_ID('AssetConditionTable') IS NULL
BEGIN
  Create table AssetConditionTable
  (
    AssetConditionID int not null primary key identity(1,1),
	AssetConditionCode nvarchar(max) Not NULL,
	StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
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
IF OBJECT_ID('AssetConditionDescriptionTable') IS NULL
BEGIN
  Create table AssetConditionDescriptionTable
  (
    AssetConditionDescriptionID int not null primary key identity(1,1),
	AssetConditionID int not null foreign key references AssetConditionTable(AssetConditionID),
	AssetConditionDescription nvarchar(max) not null,
	LanguageID int not null foreign key references LanguageTable(LanguageID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL
  )
End 
Go 

 IF OBJECT_ID('PartyTypeTable') IS NULL
BEGIN
   Create table PartyTypeTable
	(
	  PartyTypeID int not null primary key identity(1,1),
	  PartyType nvarchar(max) Not null
	)

End 
Go 

IF OBJECT_ID('PartyTable') IS NULL
BEGIN
  Create table PartyTable
  (
    PartyID int not null primary key identity(1,1),
	PartyCode nvarchar(max) Not NULL,
	PartyTypeID int not null foreign key references PartyTypeTable(PartyTypeID),
	TradeName nvarchar(Max) NOt NULL,
	CountryID int not null foreign key references CountryTable(CountryID),
	ContactPerson nvarchar(max) not null, 
	ContactPersonMobile nvarchar(max) NULL,
	Email nvarchar(max) NULL,
	ContactPersonEmail nvarchar(MAx) NULL, 
	Telephone1 nvarchar(max) NOT NULL,
	Telephone2 nvarchar(max) NULL,
	NotificationEmail nvarchar(max) NULL,
	Mobile nvarchar(max) NULL,
	Fax nvarchar(max) NULL,
	TRNNo nvarchar(max) NULL,
	Remark nvarchar(max) NULL,
	TaxFileNo nvarchar(max) NULL,
	CorporateRegistartionNo nvarchar(max) NULL,
	StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
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
IF OBJECT_ID('PartyDescriptionTable') IS NULL
BEGIN
  Create table PartyDescriptionTable
  (
    PartyDescriptionID int not null primary key identity(1,1),
	PartyID int not null foreign key references PartyTable(PartyID),
	PartyDescription nvarchar(max) not null,
	LanguageID int not null foreign key references LanguageTable(LanguageID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL
  )
End 
Go 
IF OBJECT_ID('UOMTable') IS NULL
BEGIN
  Create table UOMTable
  (
    UOMID int not null primary key identity(1,1),
	UOMCode nvarchar(max) Not NULL,
	StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
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
IF OBJECT_ID('UOMDescriptionTable') IS NULL
BEGIN
  Create table UOMDescriptionTable
  (
     UOMDescriptionID int not null primary key identity(1,1),
	 UOMID int not null foreign key references UOMTable(UOMID),
	 UOMDescription nvarchar(max) not null, 
	 LanguageID int not null foreign key references LanguageTable(LanguageID),
	 CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL
  )
End 
Go 
Alter table WarehouseTable Add LocationID int not null Foreign key references LocationTable(LocationID)
Go 
IF OBJECT_ID('WarehouseTypeTable') IS NULL
BEGIN
  Create table WarehouseTypeTable
  (
    WarehouseTypeID int not null primary key identity(1,1),
	WarehouseType nvarchar(max) Not NULL
	
  )
End 
Go 
Alter table WarehouseTable add WArehouseTypeID int not null Foreign Key references WarehouseTypeTable(WarehouseTypeID)
Go 
IF OBJECT_ID('PaymentTypeTable') IS NULL
BEGIN
  Create table PaymentTypeTable
  (
    PaymentTypeID int not null primary key identity(1,1),
	PaymentType nvarchar(max) Not NULL
	
  )
End 
Go 
IF OBJECT_ID('SupplierAccountDetailTable') IS NULL
BEGIN
  Create table SupplierAccountDetailTable
  (
    SupplierAccountDetailID int not null primary key identity(1,1),
	PartyID int not null foreign key references PartyTable(PartyID),
	PaymentTypeID int not null foreign key references PaymentTypeTable(PaymentTypeID),
	AccountCurrencyID int not null foreign key references CurrencyTable(CurrencyID),
	IsDefaultAccount bit default(0),
	BeneficiaryName nvarchar(200) not null,
	AccountNumber nvarchar(200) not null constraint uc_AccountNo Unique,
	SwiftCode nvarchar(200) not null constraint uc_SwiftCode Unique,
	IBAN nvarchar(200) not null constraint uc_IBAN Unique,
	WireNumber nvarchar(200) not null constraint uc_WireNumber Unique,
	SortCode nvarchar(200) not null,
	BankName nvarchar(500) not null, 
	CountryID int not null foreign key references CountryTable(CountryID),
	Address1 nvarchar(max) not null,
	Address2 nvarchar(max) not null,
	City nvarchar(200) not null,
	PostCode nvarchar(200) not null
  )
End 
Go 
IF OBJECT_ID('ProductTable') IS NULL
BEGIN
  Create table ProductTable
  (
    ProductID int not null primary key identity(1,1),
	ProductCode nvarchar(max) Not NULL,
	CategoryID int not null foreign key references CategoryTable(CategoryID),
	IsInventoryItem bit default(0),
	IsSerialNumberRequired bit default(0),
	UOMID int NULL Foreign Key References UOMTable(UOMID),
	ReorderLevelQuantity int NULL,
	Price Decimal(18,5) NULL ,
	StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
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
IF OBJECT_ID('ProductDescriptionTable') IS NULL
BEGIN
  Create table ProductDescriptionTable
  (
    ProductDescriptionID int not null primary key identity(1,1),
	ProductID int not null foreign key references ProductTable(ProductID),
	ProductDescription nvarchar(max) not null,
	LanguageID int not null foreign key references LanguageTable(LanguageID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL
  )
End 
Go 
IF OBJECT_ID('TransferTypeTable') IS NULL
BEGIN
  Create table TransferTypeTable
  (
    TransferTypeID int not null primary key identity(1,1),
	TransferTypeCode nvarchar(max) Not NULL,
	StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
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
	 CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL
  )
End 
Go 
IF OBJECT_ID('ItemTypeTable') IS NULL
BEGIN
  Create table ItemTypeTable
  (
    ItemTypeID int not null primary key identity(1,1),
	ItemType nvarchar(max) Not NULL
	
  )
End 
Go 

IF OBJECT_ID('ItemSupplierMappingTable') IS NULL
BEGIN
  Create table ItemSupplierMappingTable
  (
  ItemSupplierMappingID int not null Primary key identity(1,1),
  ProductID int not null foreign key references ProductTable(ProductID),
  ItemTypeID int not null foreign key references ItemTypeTable(ItemTypeID),
  PartyID int not null foreign key references PartyTable(PartyID)
 )
End 
Go
IF OBJECT_ID('DesignationTable') IS NULL
BEGIN
  Create table DesignationTable
  (
    DesignationID int not null primary key identity(1,1),
	DesignationCode nvarchar(max) Not NULL,
	StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
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
IF OBJECT_ID('DesignationDescriptionTable') IS NULL
BEGIN
  Create table DesignationDescriptionTable
  (
     DesignationDescriptionID int not null primary key identity(1,1),
	 DesignationID int not null foreign key references DesignationTable(DesignationID),
	 DesignationDescription nvarchar(max) not null, 
	 LanguageID int not null foreign key references LanguageTable(LanguageID),
	 CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL
  )
End 
Go 
IF OBJECT_ID('DisposalTypeTable') IS NULL
BEGIN
  Create table DisposalTypeTable
  (
    DisposalTypeID int not null primary key identity(1,1),
	DisposalTypeCode nvarchar(100) Not NULL,
	StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
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
IF OBJECT_ID('VATTypeTable') IS NULL
BEGIN
  Create table VATTypeTable
  (
    VATTypeID int not null primary key identity(1,1),
	VATType nvarchar(max) Not NULL
  )
End 
Go 
IF OBJECT_ID('VATTable') IS NULL
BEGIN
   Create Table VATTable
   (
      VATID int not null Primary key Identity(1,1),
	  VATTypeID int not null foreign key references VATTypeTable(VATTypeID),
	  IsDefault bit not null default(0),
	  VATCode nvarchar(100) not null, 
	  Percentage decimal(18,5) not null,
	  StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
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
IF OBJECT_ID('VATDescriptionTable') IS NULL
BEGIN
  Create Table VATDescriptionTable
  (
    VATDescriptionID int not nUll Primary key identity(1,1),
	VATID int not null foreign key references VATTAble(VATID),
	VATDescription nvarchar(max) not null,
	 LanguageID int not null foreign key references LanguageTable(LanguageID),
	 CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL
  )
End 
Go 
Alter table CategoryTable add ISBudgetCategory bit default(0) not null
go 
Alter table CategoryTable add ExpenseTypeID int NULL Foreign key references ExpenseTypeTable(ExpenseTypeID)
Go 

IF OBJECT_ID('DisposalTypeDescriptionTable') IS NULL
BEGIN
  Create table DisposalTypeDescriptionTable
  (
     DisposalTypeDescriptionID int not null primary key identity(1,1),
	 DisposalTypeID int not null foreign key references DisposalTypeTable(DisposalTypeID),
	 DisposalTypeDescription nvarchar(max) not null, 
	 LanguageID int not null foreign key references LanguageTable(LanguageID),
	 CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL
  )
End 
Go 

CREATE TABLE [dbo].[AssetTable](
	[AssetID] [int] IDENTITY(1,1) NOT NULL,
	[AssetCode] [nvarchar](50) NOT NULL,
	[Barcode] [varchar](50) NOT NULL,
	[RFIDTagCode] [varchar](50) NULL,
	[LocationID] [int] NULL,
	[DepartmentID] [int] NULL,
	[SectionID] [int] NULL,
	[CustodianID] [int] NULL,
	[SupplierID] [int] NULL,
	[AssetConditionID] [int] NULL,
	[PONumber] [varchar](50) NULL,
	[PurchaseDate] [smalldatetime] NULL,
	[PurchasePrice] [decimal](18, 4) NULL,
	[ComissionDate] [smalldatetime] NULL,
	[WarrantyExpiryDate] [smalldatetime] NULL,
	[DisposalReferenceNo] [varchar](50) NULL,
	[DisposedDateTime] [smalldatetime] NULL,
	[DisposedRemarks] [nvarchar](max) NULL,
	[AssetRemarks] [nvarchar](max) NULL,
	[DepreciationClassID] [int] NULL,
	[DepreciationFlag] [bit] NOT NULL,
	[SalvageValue] [decimal](18, 4) NULL,
	[VoucherNo] [varchar](50) NULL,
	[StatusID] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDateTime] [smalldatetime] NOT NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDateTime] [smalldatetime] NULL,
	[AssetDescription] [nvarchar](max) NOT NULL,
	[ReferenceCode] [nvarchar](50) NULL,
	[SerialNo] [nvarchar](100) NULL,
	[NetworkID] [nvarchar](100) NULL,
	[InvoiceNo] [nvarchar](150) NULL,
	[DeliveryNote] [nvarchar](150) NULL,
	[Make] [nvarchar](150) NULL,
	[Capacity] [nvarchar](150) NULL,
	[MappedAssetID] [nvarchar](200) NULL,
	[CreateFromHHT] [bit] NOT NULL,
	[DisposalValue] [decimal](18, 2) NULL,
	[MailAlert] [bit] NULL,
	[partialDisposalTotalValue] [decimal](18, 2) NULL,
	[IsTransfered] [int] NULL,
	[CategoryID] [int] NOT NULL,
	[TransferTypeID] [int] NULL,
	[UploadedDocumentPath] [varchar](max) NULL,
	[UploadedImagePath] [varchar](max) NULL,
	[AssetApproval] [int] NULL,
	[ReceiptNumber] [nvarchar](100) NULL,
	[InsertedToOracle] [bit] NOT NULL,
	[InvoiceDate] [date] NULL,
	[DistributionID] [nvarchar](50) NULL,
	[ProductID] [int] NOT NULL,
	[CompanyID] [int] NULL,
	[SyncDateTime] [smalldatetime] NULL,
	[Attribute1] [nvarchar](400) NULL,
	[Attribute2] [nvarchar](400) NULL,
	[Attribute3] [nvarchar](400) NULL,
	[Attribute4] [nvarchar](400) NULL,
	[Attribute5] [nvarchar](400) NULL,
	[Attribute6] [nvarchar](400) NULL,
	[Attribute7] [nvarchar](400) NULL,
	[Attribute8] [nvarchar](400) NULL,
	[Attribute9] [nvarchar](400) NULL,
	[Attribute10] [nvarchar](400) NULL,
	[Attribute11] [nvarchar](400) NULL,
	[Attribute12] [nvarchar](400) NULL,
	[Attribute13] [nvarchar](400) NULL,
	[Attribute14] [nvarchar](400) NULL,
	[Attribute15] [nvarchar](400) NULL,
	[Attribute16] [nvarchar](400) NULL,
	[ManufacturerID] [int] NULL,
	[ModelID] [int] NULL,
	[QFAssetCode] [varchar](100) NULL,
	[DOFPO_LINE_NUM] [varchar](250) NULL,
	[DOFMASS_ADDITION_ID] [varchar](250) NULL,
	[ERPUpdateType] [tinyint] NOT NULL,
	[DOFPARENT_MASS_ADDITION_ID] [varchar](250) NULL,
	[DOFFIXED_ASSETS_UNITS] [decimal](18, 5) NULL,
	[DOF_MASS_SPLIT_EXECUTED] [bit] NULL,
	[zDOF_Asset_Updated] [bit] NOT NULL,
	[Latitude] [decimal](15, 10) NULL,
	[Longitude] [decimal](15, 10) NULL,
	[DisposalTypeID] [int] NULL,
	[CurrentCost] [decimal](18, 5) NULL,
	[ProceedofSales] [decimal](18, 5) NULL,
	[SoldTo] [varchar](400) NULL,
	[CostOfRemoval] [decimal](18, 5) NULL,
	[AllowTransfer] [bit] NULL,
	[IsScanned] [bit] NOT NULL,
	[Attribute17] [nvarchar](800) NULL,
	[Attribute18] [nvarchar](800) NULL,
	[Attribute19] [nvarchar](800) NULL,
	[Attribute20] [nvarchar](800) NULL,
	[Attribute21] [nvarchar](800) NULL,
	[Attribute22] [nvarchar](800) NULL,
	[Attribute23] [nvarchar](800) NULL,
	[Attribute24] [nvarchar](800) NULL,
	[Attribute25] [nvarchar](800) NULL,
	[Attribute26] [nvarchar](800) NULL,
	[Attribute27] [nvarchar](800) NULL,
	[Attribute28] [nvarchar](800) NULL,
	[Attribute29] [nvarchar](800) NULL,
	[Attribute30] [nvarchar](800) NULL,
	[Attribute31] [nvarchar](800) NULL,
	[Attribute32] [nvarchar](800) NULL,
	[Attribute33] [nvarchar](800) NULL,
	[Attribute34] [nvarchar](800) NULL,
	[Attribute35] [nvarchar](800) NULL,
	[Attribute36] [nvarchar](800) NULL,
	[Attribute37] [nvarchar](800) NULL,
	[Attribute38] [nvarchar](800) NULL,
	[Attribute39] [nvarchar](800) NULL,
	[Attribute40] [nvarchar](800) NULL,
 CONSTRAINT [PK_AssetTable] PRIMARY KEY CLUSTERED 
(
	[AssetID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [uc_CompanyAssetcode] UNIQUE NONCLUSTERED 
(
	[CompanyID] ASC,
	[AssetCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [uc_CompanyBarcode] UNIQUE NONCLUSTERED 
(
	[CompanyID] ASC,
	[Barcode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [FK_CategoryID]    Script Date: 10/5/2023 6:34:52 AM ******/
CREATE NONCLUSTERED INDEX [FK_CategoryID] ON [dbo].[AssetTable]
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

/****** Object:  Index [FK_DepartmentID]    Script Date: 10/5/2023 6:34:52 AM ******/
CREATE NONCLUSTERED INDEX [FK_DepartmentID] ON [dbo].[AssetTable]
(
	[DepartmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [FK_DepreciationClassID]    Script Date: 10/5/2023 6:34:52 AM ******/
CREATE NONCLUSTERED INDEX [FK_DepreciationClassID] ON [dbo].[AssetTable]
(
	[DepreciationClassID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [FK_LocationID]    Script Date: 10/5/2023 6:34:52 AM ******/
CREATE NONCLUSTERED INDEX [FK_LocationID] ON [dbo].[AssetTable]
(
	[LocationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [FK_SectionID]    Script Date: 10/5/2023 6:34:52 AM ******/
CREATE NONCLUSTERED INDEX [FK_SectionID] ON [dbo].[AssetTable]
(
	[SectionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [FK_StatusID]    Script Date: 10/5/2023 6:34:52 AM ******/
CREATE NONCLUSTERED INDEX [FK_StatusID] ON [dbo].[AssetTable]
(
	[StatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [FK_SupplierID]    Script Date: 10/5/2023 6:34:52 AM ******/
CREATE NONCLUSTERED INDEX [FK_SupplierID] ON [dbo].[AssetTable]
(
	[SupplierID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [FK_TransferTypeID]    Script Date: 10/5/2023 6:34:52 AM ******/
CREATE NONCLUSTERED INDEX [FK_TransferTypeID] ON [dbo].[AssetTable]
(
	[TransferTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [inx_AssetTable_001]    Script Date: 10/5/2023 6:34:52 AM ******/
CREATE NONCLUSTERED INDEX [inx_AssetTable_001] ON [dbo].[AssetTable]
(
	[CompanyID] ASC,
	[WarrantyExpiryDate] ASC,
	[StatusID] ASC
)
INCLUDE([LocationID],[DepartmentID],[CategoryID]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [inx_AssetTable_002]    Script Date: 10/5/2023 6:34:52 AM ******/
CREATE NONCLUSTERED INDEX [inx_AssetTable_002] ON [dbo].[AssetTable]
(
	[CompanyID] ASC,
	[StatusID] ASC
)
INCLUDE([LocationID],[DepartmentID],[CategoryID]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AssetTable_CompanyID]    Script Date: 10/5/2023 6:34:52 AM ******/
CREATE NONCLUSTERED INDEX [IX_AssetTable_CompanyID] ON [dbo].[AssetTable]
(
	[CompanyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AssetTable_CustodianID]    Script Date: 10/5/2023 6:34:52 AM ******/
CREATE NONCLUSTERED INDEX [IX_AssetTable_CustodianID] ON [dbo].[AssetTable]
(
	[CustodianID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

/****** Object:  Index [IX_AssetTable_ManufacturerID]    Script Date: 10/5/2023 6:34:52 AM ******/
CREATE NONCLUSTERED INDEX [IX_AssetTable_ManufacturerID] ON [dbo].[AssetTable]
(
	[ManufacturerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AssetTable_ModelID]    Script Date: 10/5/2023 6:34:52 AM ******/
CREATE NONCLUSTERED INDEX [IX_AssetTable_ModelID] ON [dbo].[AssetTable]
(
	[ModelID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AssetTable_MutipleColumnsIndex]    Script Date: 10/5/2023 6:34:52 AM ******/
CREATE NONCLUSTERED INDEX [IX_AssetTable_MutipleColumnsIndex] ON [dbo].[AssetTable]
(
	[AssetID] ASC
)
INCLUDE([LocationID],[DepartmentID],[SectionID],[CustodianID],[SupplierID],[AssetConditionID],[DepreciationClassID],[StatusID],[CategoryID],[ProductID],[CompanyID],[ManufacturerID],[ModelID]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Barcode]    Script Date: 10/5/2023 6:34:52 AM ******/
CREATE NONCLUSTERED INDEX [IX_Barcode] ON [dbo].[AssetTable]
(
	[Barcode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_MultiColum_Epired]    Script Date: 10/5/2023 6:34:52 AM ******/
CREATE NONCLUSTERED INDEX [IX_MultiColum_Epired] ON [dbo].[AssetTable]
(
	[StatusID] ASC
)
INCLUDE([Barcode],[LocationID],[DepartmentID],[WarrantyExpiryDate],[CategoryID],[CompanyID]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProductID]    Script Date: 10/5/2023 6:34:52 AM ******/
CREATE NONCLUSTERED INDEX [IX_ProductID] ON [dbo].[AssetTable]
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AssetTable] ADD  CONSTRAINT [DEF_Asset_POValue]  DEFAULT ((0)) FOR [PurchasePrice]
GO
ALTER TABLE [dbo].[AssetTable] ADD  CONSTRAINT [DF_AssetTable_CreatedDateTime]  DEFAULT (getdate()) FOR [CreatedDateTime]
GO
ALTER TABLE [dbo].[AssetTable] ADD  CONSTRAINT [DF__AssetTabl__Creat__34B3CB38]  DEFAULT ((0)) FOR [CreateFromHHT]
GO
ALTER TABLE [dbo].[AssetTable] ADD  CONSTRAINT [DEF_Asset_DisposedValue]  DEFAULT ((0)) FOR [DisposalValue]
GO
ALTER TABLE [dbo].[AssetTable] ADD  CONSTRAINT [DF__AssetTabl__Inser__2F650636]  DEFAULT ((1)) FOR [InsertedToOracle]
GO
ALTER TABLE [dbo].[AssetTable] ADD  CONSTRAINT [DF_AssetTable_ERPUpdateType]  DEFAULT ((0)) FOR [ERPUpdateType]
GO
ALTER TABLE [dbo].[AssetTable] ADD  DEFAULT ((0)) FOR [DOF_MASS_SPLIT_EXECUTED]
GO
ALTER TABLE [dbo].[AssetTable] ADD  DEFAULT ((0)) FOR [zDOF_Asset_Updated]
GO
ALTER TABLE [dbo].[AssetTable] ADD  DEFAULT ((0)) FOR [AllowTransfer]
GO
ALTER TABLE [dbo].[AssetTable] ADD  DEFAULT ((0)) FOR [IsScanned]
GO


ALTER TABLE [dbo].[AssetTable]  WITH CHECK ADD  CONSTRAINT [FK__AssetTabl__Compa__10AB74EC] FOREIGN KEY([CompanyID])
REFERENCES [dbo].[CompanyTable] ([CompanyID])
GO
ALTER TABLE [dbo].[AssetTable] CHECK CONSTRAINT [FK__AssetTabl__Compa__10AB74EC]
GO


ALTER TABLE [dbo].[AssetTable]  WITH CHECK ADD  CONSTRAINT [FK_AssetTable_CustodianID] FOREIGN KEY([CustodianID])
REFERENCES [dbo].[User_LoginUserTable] ([UserID])
GO
ALTER TABLE [dbo].[AssetTable] CHECK CONSTRAINT [FK_AssetTable_CustodianID]
GO
ALTER TABLE [dbo].[AssetTable]  WITH CHECK ADD  CONSTRAINT [FK_AssetTable_DepartmentTable] FOREIGN KEY([DepartmentID])
REFERENCES [dbo].[DepartmentTable] ([DepartmentID])
GO
ALTER TABLE [dbo].[AssetTable] CHECK CONSTRAINT [FK_AssetTable_DepartmentTable]
GO
ALTER TABLE [dbo].[AssetTable]  WITH CHECK ADD  CONSTRAINT [FK_AssetTable_DepreciationClassTable] FOREIGN KEY([DepreciationClassID])
REFERENCES [dbo].[DepreciationClassTable] ([DepreciationClassID])
GO
ALTER TABLE [dbo].[AssetTable] CHECK CONSTRAINT [FK_AssetTable_DepreciationClassTable]
GO
ALTER TABLE [dbo].[AssetTable]  WITH CHECK ADD  CONSTRAINT [FK_AssetTable_DisposalTypeID] FOREIGN KEY([DisposalTypeID])
REFERENCES [dbo].[DisposalTypeTable] ([DisposalTypeID])
GO
ALTER TABLE [dbo].[AssetTable] CHECK CONSTRAINT [FK_AssetTable_DisposalTypeID]
GO
ALTER TABLE [dbo].[AssetTable]  WITH CHECK ADD  CONSTRAINT [FK_AssetTable_ManufacturerTable] FOREIGN KEY([ManufacturerID])
REFERENCES [dbo].[PartyTable] ([PartyID])
GO
ALTER TABLE [dbo].[AssetTable] CHECK CONSTRAINT [FK_AssetTable_ManufacturerTable]
GO
ALTER TABLE [dbo].[AssetTable]  WITH CHECK ADD  CONSTRAINT [FK_AssetTable_AssetConditionTable] FOREIGN KEY([AssetConditionID])
REFERENCES [dbo].[AssetConditionTable] ([AssetConditionID])
GO
ALTER TABLE [dbo].[AssetTable] CHECK CONSTRAINT [FK_AssetTable_AssetConditionTable]
GO
ALTER TABLE [dbo].[AssetTable]  WITH CHECK ADD  CONSTRAINT [FK_AssetTable_CategoryTable] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[CategoryTable] ([CategoryID])
GO
ALTER TABLE [dbo].[AssetTable] CHECK CONSTRAINT [FK_AssetTable_CategoryTable]
GO
ALTER TABLE [dbo].[AssetTable]  WITH CHECK ADD  CONSTRAINT [FK_AssetTable_LocationTable] FOREIGN KEY([LocationID])
REFERENCES [dbo].[LocationTable] ([LocationID])
GO
ALTER TABLE [dbo].[AssetTable] CHECK CONSTRAINT [FK_AssetTable_LocationTable]
GO
ALTER TABLE [dbo].[AssetTable]  WITH CHECK ADD  CONSTRAINT [FK_AssetTable_ModelTable] FOREIGN KEY([ModelID])
REFERENCES [dbo].[ModelTable] ([ModelID])
GO
ALTER TABLE [dbo].[AssetTable] CHECK CONSTRAINT [FK_AssetTable_ModelTable]
GO
ALTER TABLE [dbo].[AssetTable]  WITH CHECK ADD  CONSTRAINT [FK_AssetTable_PersonTable] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[PersonTable] ([PersonID])
GO
ALTER TABLE [dbo].[AssetTable] CHECK CONSTRAINT [FK_AssetTable_PersonTable]
GO
ALTER TABLE [dbo].[AssetTable]  WITH CHECK ADD  CONSTRAINT [FK_AssetTable_PersonTable1] FOREIGN KEY([LastModifiedBy])
REFERENCES [dbo].[PersonTable] ([PersonID])
GO
ALTER TABLE [dbo].[AssetTable] CHECK CONSTRAINT [FK_AssetTable_PersonTable1]
GO
ALTER TABLE [dbo].[AssetTable]  WITH CHECK ADD  CONSTRAINT [FK_AssetTable_PersonTable2] FOREIGN KEY([CustodianID])
REFERENCES [dbo].[PersonTable] ([PersonID])
GO
ALTER TABLE [dbo].[AssetTable] CHECK CONSTRAINT [FK_AssetTable_PersonTable2]
GO
ALTER TABLE [dbo].[AssetTable]  WITH CHECK ADD  CONSTRAINT [FK_AssetTable_ProductTable] FOREIGN KEY([ProductID])
REFERENCES [dbo].[ProductTable] ([ProductID])
GO
ALTER TABLE [dbo].[AssetTable] CHECK CONSTRAINT [FK_AssetTable_ProductTable]
GO


ALTER TABLE [dbo].[AssetTable]  WITH CHECK ADD  CONSTRAINT [FK_AssetTable_SectionTable] FOREIGN KEY([SectionID])
REFERENCES [dbo].[SectionTable] ([SectionID])
GO
ALTER TABLE [dbo].[AssetTable] CHECK CONSTRAINT [FK_AssetTable_SectionTable]
GO
ALTER TABLE [dbo].[AssetTable]  WITH CHECK ADD  CONSTRAINT [FK_AssetTable_StatusTable] FOREIGN KEY([StatusID])
REFERENCES [dbo].[StatusTable] ([StatusID])
GO
ALTER TABLE [dbo].[AssetTable] CHECK CONSTRAINT [FK_AssetTable_StatusTable]
GO
ALTER TABLE [dbo].[AssetTable]  WITH CHECK ADD  CONSTRAINT [FK_AssetTable_SupplierTable] FOREIGN KEY([SupplierID])
REFERENCES [dbo].[PartyTable] ([PartyID])
GO
ALTER TABLE [dbo].[AssetTable] CHECK CONSTRAINT [FK_AssetTable_SupplierTable]
GO


CREATE TABLE [dbo].[AssetTransferHistoryTable](
	[AssetHistoryID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NOT NULL,
	[OldLocationID] [int] NULL,
	[NewLocationID] [int] NULL,
	[OldDepartmentID] [int] NULL,
	[NewDepartmentID] [int] NULL,
	[OldSectionID] [int] NULL,
	[NewSectionID] [int] NULL,
	[OldCustodianID] [int] NULL,
	[NewCustodianID] [int] NULL,
	[TransferFrom] [nvarchar](50) NULL,
	[TransferDate] [smalldatetime] NOT NULL,
	[TransferBy] [int] NOT NULL,
	[TransferRemarks] [nvarchar](max) NULL,
	[TransferTypeID] [int] NULL,
	[OldReferencesNo] [nvarchar](100) NULL,
	[NewReferencesNo] [nvarchar](100) NULL,
	[AssetConditionID] [int] NULL,
	[DueDate] [smalldatetime] NULL,
	[StatusID] [int] NULL,
	[IsReturnAsset] [bit] NULL,
	[OldCustodianType] [nvarchar](100) NULL,
	[NewCustodianType] [nvarchar](100) NULL,
	[Attribute1] [nvarchar](400) NULL,
	[Attribute2] [nvarchar](400) NULL,
	[Attribute3] [nvarchar](400) NULL,
	[Attribute4] [nvarchar](400) NULL,
	[Attribute5] [nvarchar](400) NULL,
	[Attribute6] [nvarchar](400) NULL,
	[Attribute7] [nvarchar](400) NULL,
	[Attribute8] [nvarchar](400) NULL,
	[Attribute9] [nvarchar](400) NULL,
	[Attribute10] [nvarchar](400) NULL,
	[Attribute11] [nvarchar](400) NULL,
	[Attribute12] [nvarchar](400) NULL,
	[Attribute13] [nvarchar](400) NULL,
	[Attribute14] [nvarchar](400) NULL,
	[Attribute15] [nvarchar](400) NULL,
	[Attribute16] [nvarchar](400) NULL,
	[OldAttribute1] [nvarchar](400) NULL,
	[OldAttribute2] [nvarchar](400) NULL,
	[OldAttribute3] [nvarchar](400) NULL,
	[OldAttribute4] [nvarchar](400) NULL,
	[OldAttribute5] [nvarchar](400) NULL,
	[OldAttribute6] [nvarchar](400) NULL,
	[OldAttribute7] [nvarchar](400) NULL,
	[OldAttribute8] [nvarchar](400) NULL,
	[OldAttribute9] [nvarchar](400) NULL,
	[OldAttribute10] [nvarchar](400) NULL,
	[OldAttribute11] [nvarchar](400) NULL,
	[OldAttribute12] [nvarchar](400) NULL,
	[OldAttribute13] [nvarchar](400) NULL,
	[OldAttribute14] [nvarchar](400) NULL,
	[OldAttribute15] [nvarchar](400) NULL,
	[OldAttribute16] [nvarchar](400) NULL,
PRIMARY KEY CLUSTERED 
(
	[AssetHistoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [FK_AssetID]    Script Date: 10/5/2023 7:45:47 AM ******/
CREATE NONCLUSTERED INDEX [FK_AssetID] ON [dbo].[AssetTransferHistoryTable]
(
	[AssetID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [FK_NewDepartmentID]    Script Date: 10/5/2023 7:45:47 AM ******/
CREATE NONCLUSTERED INDEX [FK_NewDepartmentID] ON [dbo].[AssetTransferHistoryTable]
(
	[NewDepartmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [FK_NewLocationID]    Script Date: 10/5/2023 7:45:47 AM ******/
CREATE NONCLUSTERED INDEX [FK_NewLocationID] ON [dbo].[AssetTransferHistoryTable]
(
	[NewLocationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [FK_NewSectionID]    Script Date: 10/5/2023 7:45:47 AM ******/
CREATE NONCLUSTERED INDEX [FK_NewSectionID] ON [dbo].[AssetTransferHistoryTable]
(
	[NewSectionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [FK_OldDepartmentID]    Script Date: 10/5/2023 7:45:47 AM ******/
CREATE NONCLUSTERED INDEX [FK_OldDepartmentID] ON [dbo].[AssetTransferHistoryTable]
(
	[OldDepartmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [FK_OldLocationID]    Script Date: 10/5/2023 7:45:47 AM ******/
CREATE NONCLUSTERED INDEX [FK_OldLocationID] ON [dbo].[AssetTransferHistoryTable]
(
	[OldLocationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [FK_OldSectionID]    Script Date: 10/5/2023 7:45:47 AM ******/
CREATE NONCLUSTERED INDEX [FK_OldSectionID] ON [dbo].[AssetTransferHistoryTable]
(
	[OldSectionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [FK_TransferBy]    Script Date: 10/5/2023 7:45:47 AM ******/
CREATE NONCLUSTERED INDEX [FK_TransferBy] ON [dbo].[AssetTransferHistoryTable]
(
	[TransferBy] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AssetTransferHistoryTable_NewCustodianID]    Script Date: 10/5/2023 7:45:47 AM ******/
CREATE NONCLUSTERED INDEX [IX_AssetTransferHistoryTable_NewCustodianID] ON [dbo].[AssetTransferHistoryTable]
(
	[NewCustodianID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AssetTransferHistoryTable_OldCustodianID]    Script Date: 10/5/2023 7:45:47 AM ******/
CREATE NONCLUSTERED INDEX [IX_AssetTransferHistoryTable_OldCustodianID] ON [dbo].[AssetTransferHistoryTable]
(
	[OldCustodianID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_TransferTypeID]    Script Date: 10/5/2023 7:45:47 AM ******/
CREATE NONCLUSTERED INDEX [IX_TransferTypeID] ON [dbo].[AssetTransferHistoryTable]
(
	[TransferTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AssetTransferHistoryTable] ADD  CONSTRAINT [DF_AssetTransferHistoryTable_TransferDate]  DEFAULT (getdate()) FOR [TransferDate]
GO
ALTER TABLE [dbo].[AssetTransferHistoryTable]  WITH CHECK ADD  CONSTRAINT [FK__AssetTran__Asset__20E1DCB5] FOREIGN KEY([AssetID])
REFERENCES [dbo].[AssetTable] ([AssetID])
GO
ALTER TABLE [dbo].[AssetTransferHistoryTable] CHECK CONSTRAINT [FK__AssetTran__Asset__20E1DCB5]
GO
ALTER TABLE [dbo].[AssetTransferHistoryTable]  WITH CHECK ADD FOREIGN KEY([NewDepartmentID])
REFERENCES [dbo].[DepartmentTable] ([DepartmentID])
GO
ALTER TABLE [dbo].[AssetTransferHistoryTable]  WITH CHECK ADD FOREIGN KEY([NewLocationID])
REFERENCES [dbo].[LocationTable] ([LocationID])
GO
ALTER TABLE [dbo].[AssetTransferHistoryTable]  WITH CHECK ADD FOREIGN KEY([NewSectionID])
REFERENCES [dbo].[SectionTable] ([SectionID])
GO
ALTER TABLE [dbo].[AssetTransferHistoryTable]  WITH CHECK ADD FOREIGN KEY([OldDepartmentID])
REFERENCES [dbo].[DepartmentTable] ([DepartmentID])
GO
ALTER TABLE [dbo].[AssetTransferHistoryTable]  WITH CHECK ADD FOREIGN KEY([OldLocationID])
REFERENCES [dbo].[LocationTable] ([LocationID])
GO
ALTER TABLE [dbo].[AssetTransferHistoryTable]  WITH CHECK ADD FOREIGN KEY([OldSectionID])
REFERENCES [dbo].[SectionTable] ([SectionID])
GO
ALTER TABLE [dbo].[AssetTransferHistoryTable]  WITH CHECK ADD FOREIGN KEY([StatusID])
REFERENCES [dbo].[StatusTable] ([StatusID])
GO
ALTER TABLE [dbo].[AssetTransferHistoryTable]  WITH CHECK ADD FOREIGN KEY([TransferBy])
REFERENCES [dbo].[User_LoginUserTable] ([UserID])
GO
ALTER TABLE [dbo].[AssetTransferHistoryTable]  WITH CHECK ADD FOREIGN KEY([TransferTypeID])
REFERENCES [dbo].[TransferTypeTable] ([TransferTypeID])
GO
ALTER TABLE [dbo].[AssetTransferHistoryTable]  WITH CHECK ADD  CONSTRAINT [FK_AssetTransfer_AssetConditionTable] FOREIGN KEY([AssetConditionID])
REFERENCES [dbo].[AssetConditionTable] ([AssetConditionID])
GO
ALTER TABLE [dbo].[AssetTransferHistoryTable] CHECK CONSTRAINT [FK_AssetTransfer_AssetConditionTable]
GO
ALTER TABLE [dbo].[AssetTransferHistoryTable]  WITH CHECK ADD  CONSTRAINT [FK_AssetTransferHistoryTable_NewCustodianID] FOREIGN KEY([NewCustodianID])
REFERENCES [dbo].[User_LoginUserTable] ([UserID])
GO
ALTER TABLE [dbo].[AssetTransferHistoryTable] CHECK CONSTRAINT [FK_AssetTransferHistoryTable_NewCustodianID]
GO
ALTER TABLE [dbo].[AssetTransferHistoryTable]  WITH CHECK ADD  CONSTRAINT [FK_AssetTransferHistoryTable_OldCustodianID] FOREIGN KEY([OldCustodianID])
REFERENCES [dbo].[User_LoginUserTable] ([UserID])
GO
ALTER TABLE [dbo].[AssetTransferHistoryTable] CHECK CONSTRAINT [FK_AssetTransferHistoryTable_OldCustodianID]
GO

CREATE TABLE [dbo].[AssetTransferCategoryHistoryTable](
	[AssetTransferCategoryHistoryID] [int] IDENTITY(1,1) NOT NULL,
	[AssetID] [int] NOT NULL,
	[OldCategoryID] [int] NOT NULL,
	[OldProductID] [int] NOT NULL,
	[OldAssetDescription] [nvarchar](max) NOT NULL,
	[NewCategoryID] [int] NOT NULL,
	[NewProductID] [int] NOT NULL,
	[NewAssetDescription] [nvarchar](max) NOT NULL,
	[StatusID] [int] NOT NULL,
	[Createdby] [int] NOT NULL,
	[CreatedDateTime] [smalldatetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AssetTransferCategoryHistoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[AssetTransferCategoryHistoryTable]  WITH CHECK ADD FOREIGN KEY([AssetID])
REFERENCES [dbo].[AssetTable] ([AssetID])
GO
ALTER TABLE [dbo].[AssetTransferCategoryHistoryTable]  WITH CHECK ADD FOREIGN KEY([Createdby])
REFERENCES [dbo].[PersonTable] ([PersonID])
GO
ALTER TABLE [dbo].[AssetTransferCategoryHistoryTable]  WITH CHECK ADD FOREIGN KEY([NewCategoryID])
REFERENCES [dbo].[CategoryTable] ([CategoryID])
GO
ALTER TABLE [dbo].[AssetTransferCategoryHistoryTable]  WITH CHECK ADD FOREIGN KEY([NewProductID])
REFERENCES [dbo].[ProductTable] ([ProductID])
GO
ALTER TABLE [dbo].[AssetTransferCategoryHistoryTable]  WITH CHECK ADD FOREIGN KEY([OldCategoryID])
REFERENCES [dbo].[CategoryTable] ([CategoryID])
GO
ALTER TABLE [dbo].[AssetTransferCategoryHistoryTable]  WITH CHECK ADD FOREIGN KEY([OldProductID])
REFERENCES [dbo].[ProductTable] ([ProductID])
GO
ALTER TABLE [dbo].[AssetTransferCategoryHistoryTable]  WITH CHECK ADD FOREIGN KEY([StatusID])
REFERENCES [dbo].[StatusTable] ([StatusID])
GO

CREATE TABLE [dbo].[ProductUOMMappingTable](
	[ProductUOMMappingID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NOT NULL,
	[UOMID] [int] NOT NULL,
	[ConversionQuantity] [decimal](18, 5) NULL,
	[StatusID] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDateTime] [smalldatetime] NOT NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDateTime] [smalldatetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductUOMMappingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ProductUOMMappingTable]  WITH CHECK ADD  CONSTRAINT [FK_ProductUOM_CreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[PersonTable] ([PersonID])
GO
ALTER TABLE [dbo].[ProductUOMMappingTable] CHECK CONSTRAINT [FK_ProductUOM_CreatedBy]
GO
ALTER TABLE [dbo].[ProductUOMMappingTable]  WITH CHECK ADD  CONSTRAINT [FK_ProductUOM_LastModifiedBy] FOREIGN KEY([LastModifiedBy])
REFERENCES [dbo].[PersonTable] ([PersonID])
GO
ALTER TABLE [dbo].[ProductUOMMappingTable] CHECK CONSTRAINT [FK_ProductUOM_LastModifiedBy]
GO
ALTER TABLE [dbo].[ProductUOMMappingTable]  WITH CHECK ADD  CONSTRAINT [FK_ProductUOM_ProductID] FOREIGN KEY([ProductID])
REFERENCES [dbo].[ProductTable] ([ProductID])
GO
ALTER TABLE [dbo].[ProductUOMMappingTable] CHECK CONSTRAINT [FK_ProductUOM_ProductID]
GO
ALTER TABLE [dbo].[ProductUOMMappingTable]  WITH CHECK ADD  CONSTRAINT [FK_ProductUOM_StatusID] FOREIGN KEY([StatusID])
REFERENCES [dbo].[StatusTable] ([StatusID])
GO
ALTER TABLE [dbo].[ProductUOMMappingTable] CHECK CONSTRAINT [FK_ProductUOM_StatusID]
GO
ALTER TABLE [dbo].[ProductUOMMappingTable]  WITH CHECK ADD  CONSTRAINT [FK_ProductUOM_UOMID] FOREIGN KEY([UOMID])
REFERENCES [dbo].[UOMTable] ([UOMID])
GO
ALTER TABLE [dbo].[ProductUOMMappingTable] CHECK CONSTRAINT [FK_ProductUOM_UOMID]
GO

CREATE TABLE [dbo].[ApprovalHistoryTable](
	[ApproveHistoryID] [int] IDENTITY(1,1) NOT NULL,
	[ApproveWorkflowID] [int] NOT NULL,
	[ApproveWorkflowLineItemID] [int] NOT NULL,
	[DesignationID] [int] NULL,
	[ApproveModuleID] [int] NOT NULL,
	[ObjectKeyID] [int] NOT NULL,
	[OrderNo] [int] NOT NULL,
	[Remarks] [nvarchar](max) NULL,
	[StatusID] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDateTime] [smalldatetime] NOT NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDateTime] [smalldatetime] NULL,
	[ObjectKeyID1] [nvarchar](100) NULL,
	[EmailsecrectKey] [nvarchar](max) NULL,
	[UserID] [int] NULL,
	[CompanyID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ApproveHistoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApprovalLineItemTable]    Script Date: 10/5/2023 8:35:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApprovalLineItemTable](
	[ApprovalLineItemID] [int] IDENTITY(1,1) NOT NULL,
	[ApprovalID] [int] NOT NULL,
	[FieldName] [nvarchar](200) NOT NULL,
	[OldValue] [nvarchar](max) NULL,
	[NewValue] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ApprovalLineItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApprovalTable]    Script Date: 10/5/2023 8:35:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApprovalTable](
	[ApprovalID] [int] IDENTITY(1,1) NOT NULL,
	[ActionType] [nvarchar](100) NOT NULL,
	[CreatedFrom] [nvarchar](100) NOT NULL,
	[EntityName] [nvarchar](100) NOT NULL,
	[ObjectKeyValue] [int] NOT NULL,
	[StatusID] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDatetime] [smalldatetime] NOT NULL,
	[ApproveBy] [int] NULL,
	[ApproveDate] [smalldatetime] NULL,
	[RejectBy] [int] NULL,
	[RejectedDate] [smalldatetime] NULL,
	[Remarks] [nvarchar](max) NULL,
 CONSTRAINT [PK__Approval__328477D4B8CB0B5C] PRIMARY KEY CLUSTERED 
(
	[ApprovalID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApprovalTypeTable]    Script Date: 10/5/2023 8:35:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApprovalTypeTable](
	[ApprovalTypeID] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [nvarchar](100) NOT NULL,
	[ApprovalType] [nvarchar](50) NOT NULL,
	[StatusID] [int] NOT NULL,
	[IsActive] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ApprovalTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApproveModuleTable]    Script Date: 10/5/2023 8:35:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApproveModuleTable](
	[ApproveModuleID] [int] IDENTITY(1,1) NOT NULL,
	[ModuleName] [nvarchar](100) NOT NULL,
	[StatusID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ApproveModuleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApproveWorkflowLineItemTable]    Script Date: 10/5/2023 8:35:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApproveWorkflowLineItemTable](
	[ApproveWorkflowLineItemID] [int] IDENTITY(1,1) NOT NULL,
	[ApproveWorkflowID] [int] NOT NULL,
	[DesignationID] [int] NULL,
	[OrderNo] [int] NOT NULL,
	[AllowUpdate] [bit] NULL,
	[StatusID] [int] NULL,
	[UserID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ApproveWorkflowLineItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApproveWorkflowTable]    Script Date: 10/5/2023 8:35:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApproveWorkflowTable](
	[ApproveWorkflowID] [int] IDENTITY(1,1) NOT NULL,
	[ApproveModuleID] [int] NOT NULL,
	[CompanyID] [int] NOT NULL,
	[StatusID] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDateTime] [smalldatetime] NOT NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDateTime] [smalldatetime] NULL,
	
PRIMARY KEY CLUSTERED 
(
	[ApproveWorkflowID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ApprovalTable] ADD  CONSTRAINT [DF__ApprovalT__Appro__0D64F3ED]  DEFAULT ((0)) FOR [StatusID]
GO
ALTER TABLE [dbo].[ApprovalTable] ADD  CONSTRAINT [DF_ApprovalTable_CreatedDatetime]  DEFAULT (getdate()) FOR [CreatedDatetime]
GO
ALTER TABLE [dbo].[ApprovalHistoryTable]  WITH CHECK ADD FOREIGN KEY([ApproveWorkflowID])
REFERENCES [dbo].[ApproveWorkflowTable] ([ApproveWorkflowID])
GO
ALTER TABLE [dbo].[ApprovalHistoryTable]  WITH CHECK ADD FOREIGN KEY([ApproveWorkflowLineItemID])
REFERENCES [dbo].[ApproveWorkflowLineItemTable] ([ApproveWorkflowLineItemID])
GO
ALTER TABLE [dbo].[ApprovalHistoryTable]  WITH CHECK ADD FOREIGN KEY([ApproveModuleID])
REFERENCES [dbo].[ApproveModuleTable] ([ApproveModuleID])
GO
ALTER TABLE [dbo].[ApprovalHistoryTable]  WITH CHECK ADD FOREIGN KEY([CompanyID])
REFERENCES [dbo].[CompanyTable] ([CompanyID])
GO
ALTER TABLE [dbo].[ApprovalHistoryTable]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[PersonTable] ([PersonID])
GO
ALTER TABLE [dbo].[ApprovalHistoryTable]  WITH CHECK ADD FOREIGN KEY([DesignationID])
REFERENCES [dbo].[DesignationTable] ([DesignationID])
GO
ALTER TABLE [dbo].[ApprovalHistoryTable]  WITH CHECK ADD FOREIGN KEY([LastModifiedBy])
REFERENCES [dbo].[PersonTable] ([PersonID])
GO

ALTER TABLE [dbo].[ApprovalHistoryTable]  WITH CHECK ADD FOREIGN KEY([StatusID])
REFERENCES [dbo].[StatusTable] ([StatusID])
GO
ALTER TABLE [dbo].[ApprovalHistoryTable]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[PersonTable] ([PersonID])
GO
ALTER TABLE [dbo].[ApprovalLineItemTable]  WITH CHECK ADD  CONSTRAINT [FK__ApprovalL__Appro__113584D1] FOREIGN KEY([ApprovalID])
REFERENCES [dbo].[ApprovalTable] ([ApprovalID])
GO
ALTER TABLE [dbo].[ApprovalLineItemTable] CHECK CONSTRAINT [FK__ApprovalL__Appro__113584D1]
GO


ALTER TABLE [dbo].[ApproveWorkflowLineItemTable]  WITH CHECK ADD FOREIGN KEY([ApproveWorkflowID])
REFERENCES [dbo].[ApproveWorkflowTable] ([ApproveWorkflowID])
GO
ALTER TABLE [dbo].[ApproveWorkflowLineItemTable]  WITH CHECK ADD FOREIGN KEY([DesignationID])
REFERENCES [dbo].[DesignationTable] ([DesignationID])
GO
ALTER TABLE [dbo].[ApproveWorkflowLineItemTable]  WITH CHECK ADD FOREIGN KEY([StatusID])
REFERENCES [dbo].[StatusTable] ([StatusID])
GO
ALTER TABLE [dbo].[ApproveWorkflowLineItemTable]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[PersonTable] ([PersonID])
GO
ALTER TABLE [dbo].[ApproveWorkflowTable]  WITH CHECK ADD FOREIGN KEY([ApproveModuleID])
REFERENCES [dbo].[ApproveModuleTable] ([ApproveModuleID])
GO
ALTER TABLE [dbo].[ApproveWorkflowTable]  WITH CHECK ADD FOREIGN KEY([CompanyID])
REFERENCES [dbo].[CompanyTable] ([CompanyID])
GO
ALTER TABLE [dbo].[ApproveWorkflowTable]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[PersonTable] ([PersonID])
GO
ALTER TABLE [dbo].[ApproveWorkflowTable]  WITH CHECK ADD FOREIGN KEY([LastModifiedBy])
REFERENCES [dbo].[PersonTable] ([PersonID])
GO
ALTER TABLE [dbo].[ApproveWorkflowTable]  WITH CHECK ADD FOREIGN KEY([StatusID])
REFERENCES [dbo].[StatusTable] ([StatusID])
GO


/****** Object:  Table [dbo].[DepreciationLineItemTable]    Script Date: 10/5/2023 11:11:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DepreciationLineItemTable](
	[DepreciationLineItemID] [int] IDENTITY(1,1) NOT NULL,
	[DepreciationID] [int] NOT NULL,
	[AssetID] [int] NOT NULL,
	[DepreciationClassLineItemID] [int] NOT NULL,
	[DepreciationValue] [decimal](22, 10) NULL,
	[AssetValueAfterDepreciation] [decimal](22, 10) NULL,
	[AssetValueBeforeDepreciation] [decimal](22, 10) NULL,
	[CategoryID] [int] NOT NULL,
	[DepreciationClassID] [int] NOT NULL,
	[IsReclassification] [bit] NULL,
	[ReclassificationValue] [decimal](18, 2) NULL,
	[CategoryL2] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[DepreciationLineItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DepreciationTable]    Script Date: 10/5/2023 11:11:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DepreciationTable](
	[DepreciationID] [int] IDENTITY(1,1) NOT NULL,
	[PeriodID] [int] NOT NULL,
	[DepreciationDoneBy] [int] NOT NULL,
	[DepreciationDoneDate] [smalldatetime] NOT NULL,
	[IsDepreciationApproval] [bit] NULL,
	[DepreciationApprovedDate] [smalldatetime] NULL,
	[DepreciationApprovedBy] [int] NULL,
	[StatusID] [int] NOT NULL,
	[IsDeletedApproval] [bit] NULL,
	[DeletedApprovedDate] [smalldatetime] NULL,
	[DeletedApprovedBy] [int] NULL,
	[DeletedDoneBy] [int] NULL,
	[DeletedDoneDate] [smalldatetime] NULL,
	[IsReclassification] [bit] NULL,
	[CompanyID] [int] NULL,
	[IsUpdateIMALL] [bit] NULL,
	[IsUpdateEquation] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[DepreciationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PeriodTable]    Script Date: 10/5/2023 11:11:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PeriodTable](
	[PeriodID] [int] IDENTITY(1,1) NOT NULL,
	[PeriodName] [nvarchar](200) NOT NULL,
	[Year] [int] NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[StatusID] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDateTime] [smalldatetime] NOT NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDateTime] [smalldatetime] NULL,
 CONSTRAINT [PK_PeriodTable] PRIMARY KEY CLUSTERED 
(
	[PeriodID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [FK_AssetID]    Script Date: 10/5/2023 11:11:03 AM ******/
CREATE NONCLUSTERED INDEX [FK_AssetID] ON [dbo].[DepreciationLineItemTable]
(
	[AssetID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [FK_CategoryID]    Script Date: 10/5/2023 11:11:03 AM ******/
CREATE NONCLUSTERED INDEX [FK_CategoryID] ON [dbo].[DepreciationLineItemTable]
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [FK_DepreciationClassID]    Script Date: 10/5/2023 11:11:03 AM ******/
CREATE NONCLUSTERED INDEX [FK_DepreciationClassID] ON [dbo].[DepreciationLineItemTable]
(
	[DepreciationClassID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [FK_DepreciationID]    Script Date: 10/5/2023 11:11:03 AM ******/
CREATE NONCLUSTERED INDEX [FK_DepreciationID] ON [dbo].[DepreciationLineItemTable]
(
	[DepreciationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DepreciationLineItemTable_AssetID]    Script Date: 10/5/2023 11:11:03 AM ******/
CREATE NONCLUSTERED INDEX [IX_DepreciationLineItemTable_AssetID] ON [dbo].[DepreciationLineItemTable]
(
	[AssetID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DepreciationLineItemTable_DepreciationID]    Script Date: 10/5/2023 11:11:03 AM ******/
CREATE NONCLUSTERED INDEX [IX_DepreciationLineItemTable_DepreciationID] ON [dbo].[DepreciationLineItemTable]
(
	[DepreciationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [FK_PeriodID]    Script Date: 10/5/2023 11:11:03 AM ******/
CREATE NONCLUSTERED INDEX [FK_PeriodID] ON [dbo].[DepreciationTable]
(
	[PeriodID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [FK_StatusID]    Script Date: 10/5/2023 11:11:03 AM ******/
CREATE NONCLUSTERED INDEX [FK_StatusID] ON [dbo].[DepreciationTable]
(
	[StatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DepreciationTable_PeriodID]    Script Date: 10/5/2023 11:11:03 AM ******/
CREATE NONCLUSTERED INDEX [IX_DepreciationTable_PeriodID] ON [dbo].[DepreciationTable]
(
	[PeriodID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [FK_StatusID]    Script Date: 10/5/2023 11:11:03 AM ******/
CREATE NONCLUSTERED INDEX [FK_StatusID] ON [dbo].[PeriodTable]
(
	[StatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DepreciationTable] ADD  DEFAULT ((0)) FOR [IsReclassification]
GO
ALTER TABLE [dbo].[DepreciationTable] ADD  DEFAULT ((0)) FOR [IsUpdateIMALL]
GO
ALTER TABLE [dbo].[DepreciationTable] ADD  DEFAULT ((0)) FOR [IsUpdateEquation]
GO
ALTER TABLE [dbo].[PeriodTable] ADD  CONSTRAINT [DF_PeriodTable_StatusID]  DEFAULT ((1)) FOR [StatusID]
GO
ALTER TABLE [dbo].[PeriodTable] ADD  CONSTRAINT [DF_PeriodTable_CreatedDateTime]  DEFAULT (getdate()) FOR [CreatedDateTime]
GO
ALTER TABLE [dbo].[DepreciationLineItemTable]  WITH CHECK ADD  CONSTRAINT [FK__Depreciat__Asset__5F492382] FOREIGN KEY([AssetID])
REFERENCES [dbo].[AssetTable] ([AssetID])
GO
ALTER TABLE [dbo].[DepreciationLineItemTable] CHECK CONSTRAINT [FK__Depreciat__Asset__5F492382]
GO
ALTER TABLE [dbo].[DepreciationLineItemTable]  WITH CHECK ADD FOREIGN KEY([CategoryID])
REFERENCES [dbo].[CategoryTable] ([CategoryID])
GO
ALTER TABLE [dbo].[DepreciationLineItemTable]  WITH CHECK ADD FOREIGN KEY([DepreciationClassID])
REFERENCES [dbo].[DepreciationClassTable] ([DepreciationClassID])
GO
ALTER TABLE [dbo].[DepreciationLineItemTable]  WITH CHECK ADD FOREIGN KEY([DepreciationID])
REFERENCES [dbo].[DepreciationTable] ([DepreciationID])
GO
ALTER TABLE [dbo].[DepreciationLineItemTable]  WITH CHECK ADD FOREIGN KEY([DepreciationClassLineItemID])
REFERENCES [dbo].[DepreciationClassLineItemTable] ([DepreciationClassLineItemID])
GO
ALTER TABLE [dbo].[DepreciationTable]  WITH CHECK ADD FOREIGN KEY([CompanyID])
REFERENCES [dbo].[CompanyTable] ([CompanyID])
GO
ALTER TABLE [dbo].[DepreciationTable]  WITH CHECK ADD FOREIGN KEY([DeletedDoneBy])
REFERENCES [dbo].[User_LoginUserTable] ([UserID])
GO
ALTER TABLE [dbo].[DepreciationTable]  WITH CHECK ADD FOREIGN KEY([DeletedApprovedBy])
REFERENCES [dbo].[User_LoginUserTable] ([UserID])
GO
ALTER TABLE [dbo].[DepreciationTable]  WITH CHECK ADD FOREIGN KEY([DepreciationDoneBy])
REFERENCES [dbo].[User_LoginUserTable] ([UserID])
GO
ALTER TABLE [dbo].[DepreciationTable]  WITH CHECK ADD FOREIGN KEY([DepreciationApprovedBy])
REFERENCES [dbo].[User_LoginUserTable] ([UserID])
GO
ALTER TABLE [dbo].[DepreciationTable]  WITH CHECK ADD FOREIGN KEY([PeriodID])
REFERENCES [dbo].[PeriodTable] ([PeriodID])
GO
ALTER TABLE [dbo].[DepreciationTable]  WITH CHECK ADD FOREIGN KEY([StatusID])
REFERENCES [dbo].[StatusTable] ([StatusID])
GO


     IF OBJECT_ID('ResourceTemplateTable') IS NULL
BEGIN
  Create table ResourceTemplateTable
  (
    ResourceTemplateID int not null primary key identity(1,1), 
	ResourceTemplateName nvarchar(200) not null, 
	DepartmentID int not null foreign key references DepartmentTable(DepartmentID), 
	CompanyID int not null foreign key references CompanyTable(CompanyID),
	StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL
  ) 
  End 
  Go 
     IF OBJECT_ID('ResourceTemplateLineItemTable') IS NULL
BEGIN
  Create table ResourceTemplateLineItemTable
  (
    ResourceTemplateLineItemID int not null primary key identity(1,1),
	ResourceTemplateID int not null foreign key references ResourceTemplateTable(ResourceTemplateID),
	ProductID int not null foreign key references ProductTable(ProductID),
	StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL
  ) 
  End 
  Go 
     IF OBJECT_ID('BOMTable') IS NULL
BEGIN
  Create table BOMTable
  (
    BomID int not null primary key identity(1,1),
	ResourceTemplateID   int not null foreign key references ResourceTemplateTable(ResourceTemplateID),
	CompanyID int not null foreign key references CompanyTable(CompanyID),
	StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL
  ) 
  End 
  Go 
     IF OBJECT_ID('BOMLineItemTable') IS NULL
BEGIN
  Create table BOMLineItemTable
  (
    BOMLineItemID int not null primary key identity(1,1),
	BomID int not null foreign key references BOMTable(BomID),
	ProductID int not null foreign key references ProductTable(ProductID),
	Quantity int not null,
	StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL
  ) 
  End 
  Go 
   IF OBJECT_ID('BudgetYearTable') IS NULL
BEGIN
  Create table BudgetYearTable
  (
     BudgetYearID int not null primary key identity(1,1),
	 BudgetYearCode nvarchar(100) not null,
	 BudgetYear nvarchar(100) not null,
	 BudgetStartDate smallDatetime not null, 
	 BudgetEndDate smallDAtetime not null,
	 StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL
  )
End 
Go 
 IF OBJECT_ID('BudgetAllocationTable') IS NULL
BEGIN
  Create table BudgetAllocationTable
  (
    BudgetAllocationID int not null primary key identity(1,1),
	BudgetYearID int not null foreign key references BudgetYearTable(BudgetYearID),
	CompanyID int not null foreign key references CompanyTable(CompanyID),
	TotalPrice decimal(18,5) not null,
	DepartmentID int not null foreign key references DepartmentTable(DepartmentID),
	CurrencyID int not null foreign key references CurrencyTable(CurrencyID),
	ExchangeRate decimal(18,5) not null,
	StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL
  )
End 
Go 
IF OBJECT_ID('BudgetAllocationLineItemTable') IS NULL
BEGIN
  Create table BudgetAllocationLineItemTable
  (
  BudgetAllocationLineItemID  int not null primary key identity(1,1),
  BudgetAllocationID int not null foreign key references BudgetAllocationTable(BudgetAllocationID),
  ProductID int not null foreign key references ProductTable(ProductID),
  CategoryID int not null foreign key references CategoryTable(CategoryID),
  Quantity int not null, 
  Price decimal(18,5) not null,
  ExpenseTypeID int not null foreign key references ExpenseTypeTable(ExpenseTypeID),
  StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL
  )
  End 
Go
IF OBJECT_ID('BudgetAmountTransferTable') IS NULL
BEGIN
   Create table BudgetAmountTransferTable
   (
      BudgetAmountTransferID int not null primary key identity(1,1),
	  BudgetAllocationID int not null foreign key references BudgetAllocationTable(BudgetAllocationID),
	   StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL
   )

End 
Go 

IF OBJECT_ID('BudgetAmountTransferLineitemTable') IS NULL
BEGIN
   Create table BudgetAmountTransferLineitemTable
   (
     BudgetAmountTransferLineItemID  int not null primary key identity(1,1),
	 BudgetAmountTransferID int not null foreign key references BudgetAmountTransferTable(BudgetAmountTransferID),
	  ProductID int  null foreign key references ProductTable(ProductID),
    CategoryID int  null foreign key references CategoryTable(CategoryID),
	BudgetAllocatedPrice  decimal(18,5) not null,
	BudgetTransferAmount decimal(18,5) not null,
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null
   )
End 
Go 
IF OBJECT_ID('BudgetTransferTable') IS NULL
BEGIN
   Create table BudgetTransferTable
   (
     BudgetTransferID  int not null primary key identity(1,1),
	 BudgetAmountTransferID int not null foreign key references BudgetAmountTransferTable(BudgetAmountTransferID),	
	 CompanyID int not null foreign key references CompanyTable(CompanyID),
	 TotalPrice decimal(18,5) not null,
	 DepartmentID int not null foreign key references DepartmentTable(DepartmentID),
	 CurrencyID int not null foreign key references CurrencyTable(CurrencyID),
	 ExchangeRate decimal(18,5) not null,
	StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL
   )
End 
Go 

IF OBJECT_ID('BudgetTransferLineItemTable') IS NULL
BEGIN
   Create table BudgetTransferLineItemTable
   (
      BudgetTransferLineItemID int not null primary key identity(1,1),
	  BudgetTransferID int not null foreign key references BudgetTransferTable(BudgetTransferID),
	  ProductID int  null foreign key references ProductTable(ProductID),
    CategoryID int  null foreign key references CategoryTable(CategoryID),
	 ExpenseTypeID int not null foreign key references ExpenseTypeTable(ExpenseTypeID),
	 Price decimal(18,5) not null,
     StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL
   )
End 
Go 
IF OBJECT_ID('ForecastRequestTable') IS NULL
BEGIN
   Create table ForecastRequestTable
   (
     ForecastRequestID int not null primary key identity(1,1),
	 DepartmentID int not null foreign key references DepartmentTable(DepartmentID),
	 CurrencyID int not null foreign key references CurrencyTable(CurrencyID),
	 BudgetYearID int not null foreign key references BudgetYearTable(BudgetYearID),
	  CompanyID int not null foreign key references CompanyTable(CompanyID),
	 ExchangeRate decimal(18,5) not null,
	 TotalPrice Decimal(18,5) not null, 
	 BudgetAllocationID int not null foreign key references BudgetAllocationTable(BudgetAllocationID),
	 StatusID int not null foreign key references StatusTable(StatusID),
	 CreatedBy int not null foreign key references PersonTable(PersonID),
	 CreatedDateTime SmallDatetime not null, 
	 LastModifiedBy int foreign key references PersonTable(PersonID),
	 LastModifiedDateTime SmallDateTime NULL

   )
   End 
   Go 
IF OBJECT_ID('ForecastLineItemTable') IS NULL
BEGIN
   Create table ForecastLineItemTable
   (
    ForecastLineItemID   int not null primary key identity(1,1),
	ForecastRequestID int not null foreign key references ForecastRequestTable(ForecastRequestID),
	ProductID int  null foreign key references ProductTable(ProductID),
    CategoryID int  null foreign key references CategoryTable(CategoryID),
	  Quantity int not null, 
	Price decimal(18,5) not null,
	 StatusID int not null foreign key references StatusTable(StatusID),
	 CreatedBy int not null foreign key references PersonTable(PersonID),
	 CreatedDateTime SmallDatetime not null, 
	 LastModifiedBy int foreign key references PersonTable(PersonID),
	 LastModifiedDateTime SmallDateTime NULL

   )
   End 
   Go 
   IF OBJECT_ID('ItemRequestTable') IS NULL
BEGIN
   Create Table ItemRequestTable
   (
     ItemRequestID int not null primary key identity(1,1),
	 ItemRequestCode nvarchar(100) not null,
	 LocationID int not null foreign key references LocationTable(LocationID),
	 DepartmentID  int not null foreign key references DepartmentTable(DepartmentID),
	 RequestDate SmallDatetime not null,
	 RequestPerson int not null foreign key references PersonTable(PersonID),
	 CompanyID int not null foreign key references CompanyTable(CompanyID),
	--ItemRequestID 
	CurrencyID int not null foreign key references CurrencyTable(CurrencyID),
	ExchangeRate SmallDateTime NULL,
	TotalPrice decimal(18,5) not null,
	 StatusID int not null foreign key references StatusTable(StatusID),
	 CreatedBy int not null foreign key references PersonTable(PersonID),
	 CreatedDateTime SmallDatetime not null, 
	 LastModifiedBy int foreign key references PersonTable(PersonID),
	 LastModifiedDateTime SmallDateTime NULL

   )
End 
Go 
 IF OBJECT_ID('ItemRequestLineItemTable') IS NULL
BEGIN
   Create Table ItemRequestLineItemTable
   (
    ItemRequestLineItemID int not null primary key identity(1,1),
	ItemRequestID int not null foreign key references ItemRequestTable(ItemRequestID),
	ProductID int  not null foreign key references ProductTable(ProductID),
	Quantity int not null,
	Remarks nvarchar(max) NULL,
	CategoryID int not null foreign key references CategoryTable(CategoryID),
	UomID int not null foreign key references UomTable(UOMID),
	Price decimal(18,5) not null,
	TotalPrice decimal(18,5) not null,
	BudgetExceedAmount decimal(18,5)  null
   )
   End 
   Go 
   

   IF OBJECT_ID('PurchaseOrderTable') IS NULL
BEGIN
  Create table  PurchaseOrderTable
  (
    PurchaseOrderID int not null primary key identity(1,1),
	PurchaseOrderNo nvarchar(100) not null,
	PurchaseOrderDate smallDatetime not null,
	ReferencesNumber nvarchar(200)  null,
	ExpectedDeliveryDate smallDatetime null,
	SupplierID int not null foreign key references SupplierTable(SupplierID),
	Remarks nvarchar(MAX) NULL,
	ShippingDate SmallDateTime NULL,
	ShippingTerms nvarchar(max) NULL,
	PaymentTerms nvarchar(max) NULL,
	PaymentMethod nvarchar(max) NULL,
	CompanyID int not null foreign key references CompanyTable(CompanyID),
	--ItemRequestID 
	CurrencyID int not null foreign key references CurrencyTable(CurrencyID),
	ExchangeRate SmallDateTime NULL,
	DepartmentID  int not null foreign key references DepartmentTable(DepartmentID),
	TotalPrice decimal(18,5) not null,
	LocationID int not null foreign key references LocationTable(LocationID),
	ItemRequestID int not null foreign key references ItemRequestTable(ItemRequestID),
	StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL
  )

End 
Go 
 IF OBJECT_ID('PurchaseOrderLineItemTable') IS NULL
BEGIN
   Create Table PurchaseOrderLineItemTable
   (
    PurchaseOrderLineItemID int not null primary key identity(1,1),
	PurchaseOrderID int not null foreign key references PurchaseOrderTable(PurchaseOrderID),
	ProductID int  not null foreign key references ProductTable(ProductID),
	PurchaseQuantity int not null,
	PricePerQuantity int not null,
	CategoryID  int not null foreign key references CategoryTable(CategoryID),
	UomID int not null foreign key references UomTable(UOMID),
	TotalPrice decimal(18,5) not null
   )
   End 
   Go 
    IF OBJECT_ID('GRNTable') IS NULL
BEGIN
   Create Table GRNTable
   (
    GRNID int not null primary key identity(1,1),
	PurchaseOrderID int not null foreign key references PurchaseOrderTable(PurchaseOrderID),
	ReceiptNumber nvarchar(200) not null,
	ReceiptDate SmallDatetime not null,
	SupplierID  int  null foreign key references SupplierTable(SupplierID),
	LocationID int  null foreign key references LocationTable(LocationID),
	Remarks nvarchar(max) NULL,
	CompanyID int not null foreign key references CompanyTable(CompanyID),
	CurrencyID int  null foreign key references CurrencyTable(CurrencyID),
	ExchangeRate SmallDateTime NULL,
	DepartmentID  int not null foreign key references DepartmentTable(DepartmentID),
	TotalPrice decimal(18,5) not null,
	WarehouseID int not null foreign key references WarehouseTable(WarehouseID),
	LaborCost decimal(18,5) NULL,
	TransportedCost decimal(18,5) NULL,
	ShippingCost decimal(18,5) NULL,
	OtherCost decimal(18,5) NULL,
	StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL
   )
   End 
   Go 

IF OBJECT_ID('GRNLineItemTable') IS NULL
BEGIN
   Create Table GRNLineItemTable
   (
     GRNLineItemID  int not null primary key identity(1,1),
	GRNID  int not null foreign key references GRNTable(GRNID),
	ProductID int  not null foreign key references ProductTable(ProductID),
	SerialNumber nvarchar(100) NULL,
	ReceivedQuantity int not null,
	PricePerQuantity decimal(18,5) not null,
	CategoryID int not null foreign key references CategoryTable(CategoryID),
	UomID int not null foreign key references UomTable(UOMID),
	TotalPrice decimal(18,5) not null
   )
   End 
   Go 
   IF OBJECT_ID('ItemIssueTable') IS NULL
BEGIN
   Create Table ItemIssueTable
   (
    ItemIssueID int not null primary key identity(1,1),
    ItemIssueNo nvarchar(100) not null,
	ItemIssueDate SmallDatetime not null,
	LocationID  int  null foreign key references LocationTable(LocationID),
	DepartmentID int  null foreign key references DepartmentTable(DepartmentID),
	CompanyID int not null foreign key references CompanyTable(CompanyID),
	CurrencyID int not null foreign key references CurrencyTable(CurrencyID),
	ExchangeRate decimal(18,5) null
	RequestPerson int not null foreign key references PersonTable(PersonID),
	WarehouseID int not null foreign key references WarehouseTable(WarehouseID),
	ItemRequestID int not null foreign key references ItemRequestTable(ItemRequestID),
	StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL
   )
   End 
   Go 
    IF OBJECT_ID('ItemIssueLineitemTable') IS NULL
BEGIN
   Create Table ItemIssueLineitemTable
   (
    ItemIssueLineItemID int not null primary key identity(1,1),
	ItemIssueID  int NOT null foreign key references ItemIssueTable(ItemIssueID),
	CategoryID  int not null foreign key references CategoryTable(CategoryID),
	ProductID int  not null foreign key references ProductTable(ProductID),
	UomID int not null foreign key references UomTable(UOMID),
	Quantity INT NOT NULL,
	StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL
   )
   End 
   Go 


   IF OBJECT_ID('ItemReqestTable') IS NULL
BEGIN
   Create Table ItemReqestTable
   (
     ItemRequestID int not null Primary key identity(1,1), 
	 ItemTypeID int not null foreign key references ItemTypeTable(ItemTypeID), 
	 DepartmentID int not null foreign key references DepartmentTable(DepartmentID), 
	 CurrencyID int not null foreign key references CurrencyTable(CurrencyID),
	 SupplierID int not null foreign key references PartyTable(PartyID),
	 IsProject bit default(0),
	 ProjectID int NULL Foreign key references ProjectTable(ProjectID),
	  StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL
   )
End  
Go 

IF OBJECT_ID('ItemReqestLineItemTable') IS NULL
BEGIN
   Create Table ItemReqestLineItemTable
   (
     ItemReqestLineItemID int not null Primary key identity(1,1), 
	 ItemRequestID int not null foreign key references ItemReqestTable(ItemRequestID),
	 ProductID int not null foreign key references ProductTable(ProductID),
	 UOMID int not null foreign key references UOMTable(UOMID),
	 Quantity int Not null,
	 UnitPrice Decimal(18,5) Not null, 
	 VATID int foreign key references VATTABLE(VATID)

	
   )
End  
Go 

IF OBJECT_ID('RequestQuotationTable') IS NULL
BEGIN
   Create Table RequestQuotationTable
   (
     RequestQuotationID int not null Primary key identity(1,1), 
	 MeterialRequisitionNo nvarchar(200) not null,
	 PreOrderNo nvarchar(200) not null,
	 RequestForQuotationNo nvarchar(200) not null,
	 CurrencyID int not null foreign key references CurrencyTable(CurrencyID),
	 SupplierID int not null foreign key references PartyTable(PartyID),
	 RequestForQuotationDate SmallDatetime not null,
	 RequestForQuotationClosingDate SmallDatetime not null,
	 ExpectingDeliveryDate SmallDatetime NULL,
	  StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL
   )
End  
Go 

IF OBJECT_ID('RequestQuotationLineItemTable') IS NULL
BEGIN
   Create Table RequestQuotationLineItemTable
   (
     RequestQuotationLineItemID int not null Primary key identity(1,1), 
	 RequestQuotationID int not null foreign key references RequestQuotationTable(RequestQuotationID),
	 ProductID int not null foreign key references ProductTable(ProductID),
	 Quantity int Not null
   )
End  
Go 

IF OBJECT_ID('SupplierQuotationTable') IS NULL
BEGIN
   Create Table SupplierQuotationTable
   (
     SupplierQuotationID int not null Primary key identity(1,1), 
	 RequestQuotationID int not null foreign key references RequestQuotationTable(RequestQuotationID),
	QuotationReferenceNo nvarchar(200) not null,
	 QuotationSubmittedDate SmallDatetime not null,
	 QuotationValidity SmallDatetime not null,
	  StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null, 
	LastModifiedBy int foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDateTime NULL
   )
End  
Go 

IF OBJECT_ID('SupplierQuotationLineItemTable') IS NULL
BEGIN
   Create Table SupplierQuotationLineItemTable
   (
     SupplierQuotationLineItemID int not null Primary key identity(1,1), 
	 SupplierQuotationID int not null foreign key references SupplierQuotationTable(SupplierQuotationID),
	 ProductID int not null foreign key references ProductTable(ProductID),
	 QuotedQuantity int Not null,
	 Quantity int not null , 
	 Price decimal(18,5) not null, 
	 DiscountAmount Decimal(18,5) NULL, 
	 Remarks nvarchar(max) NULL,
	 VATID int NULL foreign key references VATTable(VATID)
   )
End  
Go 
IF OBJECT_ID('PriceAnalysisTable') IS NULL
BEGIN
   Create Table PriceAnalysisTable
   (
      PriceAnalysisID int not null Primary key identity(1,1), 
	  PreOrderNo nvarchar(200) not null,
	  PreOrderDate SmallDatetime not null, 
	  isPurchaseContact bit default(0),
	  DeliveredWarehouseID int not null foreign key references WarehouseTable(WarehouseID),
	  DeliveryDate SmallDatetime,
	  Remarks nvarchar(max) NULL, 
	  SpecialInstructions nvarchar(max) , 
	  PaymentTerms nvarchar(max),
	  StatusID int not null foreign key references StatusTable(StatusID),
	  CreatedBy int not null foreign key references PersonTable(PersonID),
	  CreatedDateTime SmallDatetime not null, 
	  LastModifiedBy int foreign key references PersonTable(PersonID),
	  LastModifiedDateTime SmallDateTime NULL
   )
End  
Go 

IF OBJECT_ID('PriceAnalysisLineItemTable') IS NULL
BEGIN
   Create Table PriceAnalysisLineItemTable
   (
     PriceAnalysisLineItemID int not null Primary key identity(1,1), 
	 PriceAnalysisID int not null foreign key references PriceAnalysisTable(PriceAnalysisID),
	 ProductID int not null foreign key references ProductTable(ProductID),
	 SupplierID int not null ,
	 Quantity int not null , 
	 Price decimal(18,5) not null
	
   )
End  
Go 
IF OBJECT_ID('PurchaseTypeTable') IS NULL
BEGIN
  Create table PurchaseTypeTable
  (
    PurchaseTypeID int not null primary key identity(1,1),
	PurchaseType nvarchar(max) Not NULL
  )
End 
Go 
IF OBJECT_ID('PurchaseOrderTable') IS NULL
BEGIN
   Create Table PurchaseOrderTable
   (
      PurchaseOrderID int not null Primary key identity(1,1), 
	  PurchaseOrderReferenceNo nvarchar(100) ,
	  PurchaseOrderDate SmallDatetime not null,
	  PurchaseTypeID int not null foreign key references PurchaseTypeTable(PurchaseTypeID),
	  ExpectedDeliveryDate SmallDatetime  NULL,
	  ShippingDate SmallDatetime  NULL,
	  ShippingTerms nvarchar(max) NULL,
	  PaymentTerms nvarchar(max) NULL,
	  PaymentMethods nvarchar(100) NULL,
	  CompanyID int not null foreign key references CompanyTable(CompanyID),
	    CurrencyID int not null foreign key references CurrencyTable(CurrencyID),
	  StatusID int not null foreign key references StatusTable(StatusID),
	  CreatedBy int not null foreign key references PersonTable(PersonID),
	  CreatedDateTime SmallDatetime not null, 
	  LastModifiedBy int foreign key references PersonTable(PersonID),
	  LastModifiedDateTime SmallDateTime NULL
   )
End  
Go 

IF OBJECT_ID('PurchaseOrderLineItemTable') IS NULL
BEGIN
   Create Table PurchaseOrderLineItemTable
   (
     PurchaseOrderLineItemID int not null Primary key identity(1,1), 
	 PurchaseOrderID int not null foreign key references PurchaseOrderTable(PurchaseOrderID),
	 ProductID int not null foreign key references ProductTable(ProductID),
	 SupplierID int not null,
	 Quantity int not null , 
	 Price decimal(18,5) not null,
	 VATID int foreign key references VATTABLE(VATID)
	
   )
End  
Go 

Alter table PurchaseOrderTable Add CurrencyID int not null foreign key references CurrencyTable(CurrencyID)
Go 
Alter table PurchaseOrderTable Add DepartmentID int not null foreign key references DepartmentTable(DepartmentID)
Go 

IF OBJECT_ID('GRNTable') IS NULL
BEGIN
   Create Table GRNTable
   (
      GRNID int not null Primary key identity(1,1), 
	  GRNNO nvarchar(100) NOT NULL ,
	  ReceivedOn SmallDateTime not null,
	  PurchaseOrderID int not null foreign key references PurchaseOrderTable(PurchaseOrderID),
	  WarehouseID int not null foreign key references WarehouseTable(WarehouseID),
	 
	  CompanyID int not null foreign key references CompanyTable(CompanyID),
	  CurrencyID int not null foreign key references CurrencyTable(CurrencyID),
	  LaborCost Decimal(18,5) NULL,
	  TransportedCost Decimal(18,5) NULL,
	  ShippingCost Decimal(18,5) NULL,
	  OtherCost Decimal(18,5) NULL,
	  StatusID int not null foreign key references StatusTable(StatusID),
	  CreatedBy int not null foreign key references PersonTable(PersonID),
	  CreatedDateTime SmallDatetime not null, 
	  LastModifiedBy int foreign key references PersonTable(PersonID),
	  LastModifiedDateTime SmallDateTime NULL
   )
End  
Go 

IF OBJECT_ID('GRNLineItemTable') IS NULL
BEGIN
   Create Table GRNLineItemTable
   (
     GRNLineItemID int not null Primary key identity(1,1), 
	 GRNID int not null foreign key references GRNTable(GRNID),
	 ProductID int not null foreign key references ProductTable(ProductID),
	 SupplierID int not null,
	 Quantity int not null , 
	 Price decimal(18,5) not null,
	 VATID int foreign key references VATTABLE(VATID),
	 Remarks nvarchar(max) NULL
	
   )
End  
Go
IF OBJECT_ID('ItemDispatchTable') IS NULL
BEGIN
   Create Table ItemDispatchTable
   (
      ItemDispatchID int not null Primary key identity(1,1), 
	  PurchaseOrderID nvarchar(100) NOT NULL ,
	  ReceivedOn SmallDateTime not null,
	  WarehouseID int not null foreign key references WarehouseTable(WarehouseID),
	  CompanyID int not null foreign key references CompanyTable(CompanyID),
	  CurrencyID int not null foreign key references CurrencyTable(CurrencyID),
	  StatusID int not null foreign key references StatusTable(StatusID),
	  CreatedBy int not null foreign key references PersonTable(PersonID),
	  CreatedDateTime SmallDatetime not null, 
	  LastModifiedBy int foreign key references PersonTable(PersonID),
	  LastModifiedDateTime SmallDateTime NULL
   )
End  
Go 

IF OBJECT_ID('ItemDispatchLineItemTable') IS NULL
BEGIN
   Create Table ItemDispatchLineItemTable
   (
     ItemDispatchLineItemID int not null Primary key identity(1,1), 
	 ItemDispatchID int not null foreign key references ItemDispatchTable(ItemDispatchID),
	 ProductID int not null foreign key references ProductTable(ProductID),
	 Quantity int not null , 
	 Price decimal(18,5) not null,
	 VATID int foreign key references VATTABLE(VATID),
	 Remarks nvarchar(max) NULL
	
   )
End  
Go 

IF OBJECT_ID('InvoiceTable') IS NULL
BEGIN
   Create Table InvoiceTable
   (
      InvoiceID int not null Primary key identity(1,1), 
	  InvoiceNo nvarchar(100) not null,
	  InvoiceDate smallDatetime not null,
	  ImvoiceDueDate SmallDatetime not null,
	  PurchaseOrderID int not null foreign key references PurchaseOrderTable(PurchaseOrderID),
	  SupplierInvoiceNo nvarchar(100) not null,
	  PurposeofPayment nvarchar(max) NULL,
	  PaymentInstructions nvarchar(max) NULL,
	  PaymentTypeID int not null foreign key references PaymentTypeTable(PaymentTypeID),
	  SupplierAccountDetailID int not null foreign key references SupplierAccountDetailTable(SupplierAccountDetailID),
	  CompanyID int not null foreign key references CompanyTable(CompanyID),
	  CurrencyID int not null foreign key references CurrencyTable(CurrencyID),
	  StatusID int not null foreign key references StatusTable(StatusID),
	  CreatedBy int not null foreign key references PersonTable(PersonID),
	  CreatedDateTime SmallDatetime not null, 
	  LastModifiedBy int foreign key references PersonTable(PersonID),
	  LastModifiedDateTime SmallDateTime NULL
   )
End  
Go 

IF OBJECT_ID('InvoiceLineItemTable') IS NULL
BEGIN
   Create Table InvoiceLineItemTable
   (
     InvoiceLineItemID int not null Primary key identity(1,1), 
	 InvoiceID int not null foreign key references InvoiceTable(InvoiceID),
	 ProductID int not null foreign key references ProductTable(ProductID),
	 Quantity int not null , 
	 Price decimal(18,5) not null,
	 VATID int foreign key references VATTABLE(VATID),
	 Remarks nvarchar(max) NULL
	
   )
End  
Go 

IF OBJECT_ID('EntityCodeTable') IS NULL
BEGIN
CREATE TABLE [dbo].[EntityCodeTable](
	[EntityCodeID] [int] IDENTITY(1,1) NOT NULL primary key,
	[EntityCodeName] [varchar](50) NOT NULL,
	[CodePrefix] [varchar](50) NULL,
	[CodeSuffix] [varchar](50) NULL,
	[CodeFormatString] [varchar](50) NULL,
	[LastUsedNo] [int] NOT NULL default(0),
	[UseDateTime] [bit] NOT NULL default(0)
	)

	End  
Go

Alter table User_RoleTable
Alter column IsActive bit null

Go 


IF OBJECT_ID('AssetTransferTransactionTable') IS NULL
BEGIN
CREATE TABLE AssetTransferTransactionTable
( 
   AssetTransferTransactionID int not null primary key identity(1,1),
   TransferReferenceNo nvarchar(100) not null,
   LocationID int not null foreign key references LocationTable(LocationID),
   Remarks nvarchar(max) NULL, 
  StatusID int not null foreign key references StatusTable(StatusID),
  CreatedBy int not null foreign key references User_LoginUserTable(UserID),
  CreatedDateTime SmallDatetime not null, 
  LastModifiedBy int foreign key references User_LoginUserTable(UserID),
  LastModifiedDateTime SmallDateTime NULL
) 
End 
Go
IF OBJECT_ID('AssetTransferTransactionLineItemTable') IS NULL
BEGIN
CREATE TABLE AssetTransferTransactionLineItemTable
(
  AssetTransferTransactionLineItemID int not null primary key identity(1,1),
  AssetTransferTransactionID int not null foreign key references AssetTransferTransactionTable(AssetTransferTransactionID),
  AssetID int not null foreign key references AssetTable(AssetID)
) 
End 
Go 
IF OBJECT_ID('DocumentTable') IS NULL
BEGIN
CREATE TABLE DocumentTable
(
  DocumentID int not null Primary key identity(1,1),
  DocumentName nvarchar(100) not null,
  TransactionType nvarchar(100) not null,-- transfer or Retirment 
  FileName  nvarchar(100) not null,
  ObjectKeyID int Not NULL, 
  FilePath nvarchar(max) not null,
  FileExtension  nvarchar(100) not null,
  StatusID int not null foreign key references StatusTable(StatusID)

) 
End 
Go
Go 
IF OBJECT_ID('UserApprovalRoleMappingTable') IS NULL
BEGIN
CREATE TABLE UserApprovalRoleMappingTable
(
  UserApprovalRoleMappingID  int not null primary key identity(1,1),
  UserID int not null foreign key references User_LoginUserTable(UserID),
  LocationID int not null foreign key references LocationTable(LocationID),
  ApprovalRoleID int not null foreign key references ApprovalRoleTable(ApprovalRoleID),
  StatusID int not null foreign key references StatusTable(StatusID)
)
End 
Go 
IF OBJECT_ID('LocationTypeTable') IS NULL
BEGIN
CREATE TABLE LocationTypeTable
(
  LocationTypeID int not null primary key identity(1,1),
  LocationType nvarchar(100)
)
End 
go 
ALTER TABLE LOCATIONTABLE ADD LocationTypeID int null foreign key references LocationTypeTable(LocationTypeID)
go 

IF OBJECT_ID('ApprovalRoleTable') IS NULL
BEGIN
create table ApprovalRoleTable
(
  ApprovalRoleID int not null primary key identity(1,1),
  ApprovalRoleCode nvarchar(100) not null,
  ApprovalRoleName nvarchar(500) not null,
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
	 Attribute16 nvarchar(200) NULL,
	 constraint UC_ApprovalRoleCode unique (ApprovalRoleCode)
)
End 
Go 
IF OBJECT_ID('ApprovalRoleDescriptionTable') IS NULL
BEGIN
  Create table ApprovalRoleDescriptionTable
  (
     ApprovalRoleDescriptionID int not null primary key identity(1,1),
	 ApprovalRoleDescription nvarchar(max) not null, 
	 ApprovalRoleID int not null Foreign Key references ApprovalRoleTable(ApprovalRoleID),
	 CreatedBy int not null foreign key references User_LoginUserTable(UserID),
	 CreatedDateTime SmallDatetime not null, 
	 LastModifiedBy int foreign key references User_LoginUserTable(UserID),
	 LastModifiedDateTime SmallDateTime NULL
  )
End 
Go 
IF OBJECT_ID('UserApprovalRoleMappingTable') IS NULL
BEGIN
CREATE TABLE UserApprovalRoleMappingTable
(
  UserApprovalRoleMappingID  int not null primary key identity(1,1),
  UserID int not null foreign key references User_LoginUserTable(UserID),
  StatusID int not null foreign key references StatusTable(StatusID)
)
End 
Go 

IF OBJECT_ID('ApproveModuleTable') IS NULL
BEGIN
CREATE TABLE ApproveModuleTable
( 
   ApproveModuleID int not null primary key ,
   ModuleName nvarchar(100) Not NULL, 
   StatusID int not null foreign key references StatusTable(StatusID)
) 
End 
Go 
IF OBJECT_ID('ApproveWorkflowTable') IS NULL
BEGIN
CREATE TABLE ApproveWorkflowTable
( 
	   ApproveWorkflowID int not null primary key identity(1,1) ,
	   ApproveModuleID  int not null foreign key references ApproveModuleTable(ApproveModuleID),
	   FromLocationTypeID int not null foreign key references LocationTypeTable(LocationTypeID),
	   ToLocationTypeID int NULL foreign key references LocationTypeTable(LocationTypeID),
	   StatusID int not null foreign key references StatusTable(StatusID),
	   CreatedBy int not null foreign key references User_LoginUserTable(UserID),
	   CreatedDateTime SmallDatetime not null, 
	   LastModifiedBy int foreign key references User_LoginUserTable(UserID),
	  LastModifiedDateTime SmallDateTime NULL,
) 
End 
Go 
IF OBJECT_ID('ApproveWorkflowLineItemTable') IS NULL
BEGIN
CREATE TABLE ApproveWorkflowLineItemTable
( 
	ApproveWorkFlowLineItemID int not null Primary key identity(1,1),
	ApproveWorkFlowID int Not NULL foreign key references ApproveWorkFlowTable(ApproveWorkFlowID),
	ApprovalRoleID int not null foreign key references ApprovalRoleTable(ApprovalRoleID),
	OrderNo int Not NULL,
	StatusID int not null foreign key references StatusTable(StatusID)
) 
End 
Go 

IF OBJECT_ID('UserLocationMappingTable') IS NULL
BEGIN
  Create table UserLocationMappingTable
  (
    UserLocationMappingID int not null primary key identity(1,1),
	UserID int not null foreign key references User_LoginUserTable(UserID),
	LocationID int not null Foreign key references LocationTable(LocationID),
	StatusID int not null foreign key references StatusTable(StatusID)
  )
End 
Go
IF OBJECT_ID('UserCategoryMappingTable') IS NULL
BEGIN
  Create table UserCategoryMappingTable
  (
    UserCategoryMappingID int not null primary key identity(1,1),
	UserID int not null foreign key references User_LoginUserTable(UserID),
	CategoryID int not null Foreign key references CategoryTable(CategoryID),
	StatusID int not null foreign key references StatusTable(StatusID)
  )
End 
Go
IF OBJECT_ID('UserDepartmentMappingTable') IS NULL
BEGIN
  Create table UserDepartmentMappingTable
  (
    UserDepartmentMappingID int not null primary key identity(1,1),
	UserID int not null foreign key references User_LoginUserTable(UserID),
	DepartmentID int not null Foreign key references DepartmentTable(DepartmentID),
	StatusID int not null foreign key references StatusTable(StatusID)
  )
End 
Go
IF OBJECT_ID('ApprovalHistoryTable') IS NULL
BEGIN
CREATE TABLE ApprovalHistoryTable
( 
  ApprovalHistoryID int not null primary key identity(1,1),
  ApproveWorkFlowID int Not NULL foreign key references ApproveWorkFlowTable(ApproveWorkFlowID),
  ApproveWorkFlowLineItemID int Not NULL foreign key references ApproveWorkflowLineItemTable(ApproveWorkFlowLineItemID),
  ApproveModuleID  int not null foreign key references ApproveModuleTable(ApproveModuleID),
  ApprovalRoleID int not null foreign key references ApprovalRoleTable(ApprovalRoleID),
  TransactionID int Not NULL, 
  OrderNo int Not NULL,
  Remarks nvarchar(max) NULL, 
  FromLocationID int not null foreign key references LocationTable(LocationID),
  ToLocationID int NULL foreign key references LocationTable(LocationID),
  FromLocationTypeID int not null foreign key references LocationTypeTable(LocationTypeID),
  ToLocationTypeID int null foreign key references LocationTypeTable(LocationTypeID),
  StatusID int not null foreign key references StatusTable(StatusID),
  CreatedBy int not null foreign key references User_LoginUserTable(UserID),
  CreatedDateTime SmallDatetime not null, 
  LastModifiedBy int foreign key references User_LoginUserTable(UserID),
  LastModifiedDateTime SmallDateTime NULL,
  ObjectKeyID1 int NULL,
  EmailsecrectKey nvarchar(max) NULL
) 
End 
Go 

IF OBJECT_ID('TransactionTypeTable') IS NULL
BEGIN
  Create table TransactionTypeTable
  (
    TransactionTypeID int not null primary key ,
	TransactionTypeName nvarchar(500) not NULL,
	IsSourceTransactionType bit not null default(1),
	SourceTransactionTypeID int NULL foreign key references TransactionTypeTable(TransactionTypeID),
	SourceTransactionTypeID2 int NULL foreign key references TransactionTypeTable(TransactionTypeID),
	SourceTransactionTypeID3 int NULL foreign key references TransactionTypeTable(TransactionTypeID),
	SourceTransactionTypeID4 int NULL foreign key references TransactionTypeTable(TransactionTypeID),
	SourceTransactionTypeID5 int NULL foreign key references TransactionTypeTable(TransactionTypeID),
	TransactionTypeDesc nvarchar(max) not null 
  )
End 
Go
IF OBJECT_ID('PostingStatusTable') IS NULL
BEGIN
  Create table PostingStatusTable
  (
    PostingStatusID int not null primary key,
	PostingStatus nvarchar(100) Not NULL
  )
End 
Go 

IF OBJECT_ID('TransactionTable') IS NULL
BEGIN
  Create table TransactionTable
  (
     TransactionID int not null primary key identity(1,1),
	 TransactionNo nvarchar(100) Not null,
	 TransactionTypeID int not null foreign key references TransactionTypeTable(TransactionTypeID),
	 TransactionSubType  nvarchar(600)  null,
	 ReferenceNo nvarchar(4000) null,
	 CreatedFrom nvarchar(100)  null,
	 SourceTransactionID int NULL foreign key references TransactionTable(TransactionID),
	 SourceDocumentNo nvarchar(200) NULL,
	 Remarks nvarchar(max) NULL,
	 TransactionDate SmallDatetime null, 
	 TransactionValue decimal(18,5) null,

	 StatusID int not null foreign key references StatusTable(StatusID),
	 PostingStatusID int not null foreign key references PostingStatusTable(PostingStatusID),
	 VerifiedBy int NULL Foreign key references User_LoginUserTable(UserID),
	 VerifiedDateTime SmallDateTime NULL,
     PostedBy int NULL Foreign key references User_LoginUserTable(UserID),
     PostedDateTime  SmallDateTime NULL,
     CreatedBy int Not NULL Foreign key references User_LoginUserTable(UserID),
     CreatedDateTime SmallDatetime Not NULL 
  ) 
  End 
  Go 
  IF OBJECT_ID('TransactionLineItemTable') IS NULL
BEGIN
  Create table TransactionLineItemTable
  (
    TransactionLineItemID int not null primary key identity(1,1),
	TransactionID int not null foreign key references TransactionTable(TransactionID),
	TransactionLineNo nvarchar(200) NULL,
	AssetID int not null foreign key references AssetTable(AssetID),
	 FromLocationID int NULL Foreign key references LocationTable(LocationID),
	 ToLocationID int NULL Foreign key references LocationTable(LocationID),
	 FromDepartmentID int  NULL Foreign key references DepartmentTable(DepartmentID),
	 ToDepartmentID int  NULL Foreign key references DepartmentTable(DepartmentID),
	  FromCustodianID int  NULL Foreign key references PersonTable(PersonID),
	 ToCustodianID int  NULL Foreign key references PersonTable(PersonID),
	 FromCategoryID int null Foreign key references CategoryTable(CategoryID),
	 ToCategoryID int null Foreign key references CategoryTable(CategoryID),
	 FromProductID int null Foreign key references ProductTable(ProductID),
	 ToProductID int null Foreign key references ProductTable(ProductID),
	 FromSectionID int null Foreign key references SectionTable(SectionID),
	 ToSectionID int Null Foreign key references SectionTable(SectionID),
	 StatusID int not null foreign key references StatusTable(StatusID),
	 CreatedBy int Not NULL Foreign key references User_LoginUserTable(UserID),
     CreatedDateTime SmallDatetime Not NULL 
  ) 
  End 
  Go 
   IF OBJECT_ID('ApprovalLocationTypeTable') IS NULL
BEGIN
  Create table ApprovalLocationTypeTable
  (
    ApprovalLocationTypeID int Primary key not null,
	ApprovalLocationTypeCode nvarchar(100) not null, 
	ApprovalLocationTypeName nvarchar(100) not null
  )
  End 
  Go 

  If not exists(select ApprovalLocationTypeName from ApprovalLocationTypeTable where ApprovalLocationTypeName='Source Location')
  Begin
    insert into ApprovalLocationTypeTable(ApprovalLocationTypeID,ApprovalLocationTypeCode,ApprovalLocationTypeName)
	values(5,'Source Location','Source Location')
  End 
  Go 
   If not exists(select ApprovalLocationTypeName from ApprovalLocationTypeTable where ApprovalLocationTypeName='Destination Location')
  Begin
    insert into ApprovalLocationTypeTable(ApprovalLocationTypeID,ApprovalLocationTypeCode,ApprovalLocationTypeName)
	values(10,'Destination Location','Destination Location')
  End 
  Go 
  Alter table ApprovalRoleTable add ApprovalLocationTypeID int Foreign key references ApprovalLocationTypeTable(ApprovalLocationTypeID)
  Go 
  
IF OBJECT_ID('ImportTemplateTypeTable') IS NULL
BEGIN
  create table ImportTemplateTypeTable
  (
     ImportTemplateTypeID int not null primary key identity(1,1),
	 ImportTemplateType nvarchar(50) NOT NULL
  )
End 
gO 
IF NOT EXISTS(SELECT IMPORTTEMPLATETYPE FROM ImportTemplateTypeTable WHERE ImportTemplateType='CREATE')
BEGIN 
  INSERT INTO ImportTemplateTypeTable(IMPORTTEMPLATETYPE) VALUES ('Create')
END 
gO 
IF NOT EXISTS(SELECT IMPORTTEMPLATETYPE FROM ImportTemplateTypeTable WHERE ImportTemplateType='Edit')
BEGIN 
  INSERT INTO ImportTemplateTypeTable(IMPORTTEMPLATETYPE) VALUES ('Edit')
END 
gO 


IF OBJECT_ID('ImportTemplateTable') IS NULL
BEGIN
  create table ImportTemplateTable
  (
  ImportTemplateID int not null primary key identity(1,1),
  EntityName nvarchar(500) not null,
  ImportField nvarchar(500) NOT NULL,
  DispalyOrderID INT NOT NULL,
  IsDisplay  BIT DEFAULT(1) NOT NULL,
  IsMandatory BIT DEFAULT(0) NOT NULL,
  DataSize INT NOT NULL,
  DataFormat NVARCHAR(50) NULL,
  IsForeignKey BIT DEFAULT(0) NOT NULL,
  Width INT NOT NULL,
  ImportTemplateTypeID INT NOT NULL FOREIGN KEY REFERENCES ImportTemplateTypeTable(ImportTemplateTypeID),
  IsUnique BIT DEFAULT(0) NOT NULL,
  ReferenceEntityID int null

  )
  end 
  go 
  IF OBJECT_ID('ImportFormatTable') IS NULL
BEGIN
  create table ImportFormatTable
  (
    ImportFormatID int not null primary key identity(1,1),
	EntityName nvarchar(500) not null, 
	TamplateName nvarchar(500) not null, 
	FormatPath nvarchar(max)  null, 
	FormatExtension nvarchar(50) Not null,
	ImportTemplateTypeID INT NOT NULL FOREIGN KEY REFERENCES ImportTemplateTypeTable(ImportTemplateTypeID),
	StatusID int not null foreign key references StatusTable(StatusID),
	CreatedBy int not null foreign key references PersonTable(PersonID),
	CreatedDateTime SmallDatetime not null,
	LastModifiedBy int  null foreign key references PersonTable(PersonID),
	LastModifiedDateTime SmallDatetime
  )
  End 
  Go 
   IF OBJECT_ID('ImportFormatLineItemTable') IS NULL
BEGIN
  create table ImportFormatLineItemTable
  (
  ImportFormatLineItemID int not null Primary key identity(1,1),
  ImportFormatID int not null foreign key references ImportFormatTable(ImportFormatID),
  ImportTemplateID  int not null foreign key references ImportTemplateTable(ImportTemplateID),
  DisplayOrderID int not null
  )
  End 
  Go 
  if not exists(select ImportTemplateID from ImportTemplateTable where EntityName='AssetCondition' and ImportField='AssetConditionCode' 
	and ImportTemplateTypeID=(select ImportTemplateTypeID from ImportTemplateTypeTable where ImportTemplateType='Create'))
	Begin 
	  Insert into ImportTemplateTable(EntityName,ImportField,DispalyOrderID,IsDisplay,IsMandatory,
			DataSize,DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID)
	 select 'AssetCondition','AssetConditionCode',1,1,1,50,NULL,0,150, ImportTemplateTypeID,1,NULL
	 from ImportTemplateTypeTable where ImportTemplateType='Create'
	end 
	Go 

	 if not exists(select ImportTemplateID from ImportTemplateTable where EntityName='AssetCondition' and ImportField='AssetConditionName' 
	and ImportTemplateTypeID=(select ImportTemplateTypeID from ImportTemplateTypeTable where ImportTemplateType='Create'))
	Begin 
	  Insert into ImportTemplateTable(EntityName,ImportField,DispalyOrderID,IsDisplay,IsMandatory,
			DataSize,DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID)
	 select 'AssetCondition','AssetConditionName',2,1,1,50,NULL,0,150, ImportTemplateTypeID,1,NULL
	 from ImportTemplateTypeTable where ImportTemplateType='Create'
	end 
	go 
	  if not exists(select ImportTemplateID from ImportTemplateTable where EntityName='AssetCondition' and ImportField='AssetConditionCode' 
	and ImportTemplateTypeID=(select ImportTemplateTypeID from ImportTemplateTypeTable where ImportTemplateType='Edit'))
	Begin 
	  Insert into ImportTemplateTable(EntityName,ImportField,DispalyOrderID,IsDisplay,IsMandatory,
			DataSize,DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID)
	 select 'AssetCondition','AssetConditionCode',1,1,1,50,NULL,0,150, ImportTemplateTypeID,1,NULL
	 from ImportTemplateTypeTable where ImportTemplateType='Edit'
	end 
	Go 

	 if not exists(select ImportTemplateID from ImportTemplateTable where EntityName='AssetCondition' and ImportField='AssetConditionName' 
	and ImportTemplateTypeID=(select ImportTemplateTypeID from ImportTemplateTypeTable where ImportTemplateType='Edit'))
	Begin 
	  Insert into ImportTemplateTable(EntityName,ImportField,DispalyOrderID,IsDisplay,IsMandatory,
			DataSize,DataFormat,IsForeignKey,Width,ImportTemplateTypeID,IsUnique,ReferenceEntityID)
	 select 'AssetCondition','AssetConditionName',2,1,1,50,NULL,0,150, ImportTemplateTypeID,1,NULL
	 from ImportTemplateTypeTable where ImportTemplateType='Edit'
	end 
	go 
	Alter table NotificationTemplateTable add constraint FK_NotificationTemplateTable_NotificationModuleID FOREIGN KEY(NotificationModuleID) references NotificationModuleTable(NotificationModuleID)
	go 

	ALTER TABLE dbo.AssetConditionTable DROP CONSTRAINT UC_AssetConditionCode
go
Alter table AssetConditionTable Alter column AssetConditionCode nvarchar(200) not null 
go 
Alter table AssetConditionTable Alter column AssetConditionName nvarchar(max) not null  
go 

ALTER TABLE dbo.AssetConditionTable add CONSTRAINT UC_AssetConditionCode UNIQUE (AssetConditionCode)
go
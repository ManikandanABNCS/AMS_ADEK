if not exists(select ConfiguarationName from ConfigurationTable where ConfiguarationName='SelectedCategoriesareMandatorySerialNumberInAssetScreen')
begin
  Insert into ConfigurationTable(ConfiguarationName,ConfiguarationValue,ToolTipName,DataType,DropDownValue,MinValue,MaxValue,
 ConfiguarationType,DisplayOrderID,DefaultValue,DisplayConfiguration,SuffixText,
 CatagoryName,CategoryName)
	values('SelectedCategoriesareMandatorySerialNumberInAssetScreen','','SelectedCategoriesareMandatory','MultiSelect',NULL,
	NULL,100,'C',131,1,1,null,'AssetSettings','AssetSettings')
End 
GO 
if not exists(select ConfiguarationName from ConfigurationTable where ConfiguarationName='SelectedManufacturerareMandatorySerialNumberInAssetScreen')
begin
  Insert into ConfigurationTable(ConfiguarationName,ConfiguarationValue,ToolTipName,DataType,DropDownValue,MinValue,MaxValue,
	ConfiguarationType,DisplayOrderID,DefaultValue,DisplayConfiguration,SuffixText,
	CatagoryName,CategoryName)
	values('SelectedManufacturerareMandatorySerialNumberInAssetScreen','','SelectedManufacturerareMandatory','MultiSelect',NULL,
	NULL,100,'C',132,1,1,null,'AssetSettings','AssetSettings')
End 
GO 
if not exists(select ConfiguarationName from ConfigurationTable where ConfiguarationName='PreferredLevelLocationMapping')
begin
  Insert into ConfigurationTable(ConfiguarationName,ConfiguarationValue,ToolTipName,DataType,DropDownValue,MinValue,MaxValue,
	ConfiguarationType,DisplayOrderID,DefaultValue,DisplayConfiguration,SuffixText,
	CatagoryName,CategoryName)
	values('PreferredLevelLocationMapping','2','PreferredLevelLocationMapping','Numeric',NULL,
	1,6,'C',133,1,1,null,'AssetSettings','AssetSettings')
End 
GO 
if not exists(select ConfiguarationName from ConfigurationTable where ConfiguarationName='PreferredLevelCategoryMapping')
begin
  Insert into ConfigurationTable(ConfiguarationName,ConfiguarationValue,ToolTipName,DataType,DropDownValue,MinValue,MaxValue,
	ConfiguarationType,DisplayOrderID,DefaultValue,DisplayConfiguration,SuffixText,
	CatagoryName,CategoryName)
	values('PreferredLevelCategoryMapping','2','PreferredLevelCategoryMapping','Numeric',NULL,
	1,6,'C',134,1,1,null,'AssetSettings','AssetSettings')
End 
GO 


update ConfigurationTable set DefaultValue=2 where ConfiguarationName='SelectedCategoriesareMandatorySerialNumberInAssetScreen'
go 
update ConfigurationTable set DefaultValue=23 where ConfiguarationName='SelectedManufacturerareMandatorySerialNumberInAssetScreen'
go 
if not exists(select ConfiguarationName from ConfigurationTable where ConfiguarationName='DontAllowCategoryBasedDuplicateSerialNo')
begin
  Insert into ConfigurationTable(ConfiguarationName,ConfiguarationValue,ToolTipName,DataType,DropDownValue,MinValue,MaxValue,
	ConfiguarationType,DisplayOrderID,DefaultValue,DisplayConfiguration,SuffixText,
	CatagoryName,CategoryName)
	values('DontAllowCategoryBasedDuplicateSerialNo','true','CategoryBasedDuplicateSerialNo','Boolean',NULL,
	NULL,NULL,'C',135,1,1,null,'AssetSettings','AssetSettings')
End 
GO 
if not exists(select ConfiguarationName from ConfigurationTable where ConfiguarationName='DontAllowManufacturerBasedDuplicateSerialNo')
begin
  Insert into ConfigurationTable(ConfiguarationName,ConfiguarationValue,ToolTipName,DataType,DropDownValue,MinValue,MaxValue,
	ConfiguarationType,DisplayOrderID,DefaultValue,DisplayConfiguration,SuffixText,
	CatagoryName,CategoryName)
	values('DontAllowManufacturerBasedDuplicateSerialNo','false','ManufacturerBasedDuplicateSerialNo','Boolean',NULL,
	NULL,NULL,'C',136,1,1,null,'AssetSettings','AssetSettings')
End 
GO 

Create Procedure Prc_AssetCreationValidation
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
				set @ErrorMsg =@CategoryCode+'- Category Code level must be give '+ @CategoryLevel + 'Level.'
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
				set @ErrorMsg =@LocationCode+'- Location Code level must be give '+ @LocationLevel + 'Level.'
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
Create Procedure Prc_AssetModificationValidation
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
	,@AssetCategoryID int ,@AssetLocationID int,@AssetDepartmentID int,@AssetmanufacturerID int 

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
				set @ErrorMsg =@CategoryCode+'- Category Code level must be give '+ @CategoryLevel + 'Level.'
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
				set @ErrorMsg =@LocationCode+'- Location Code level must be give '+ @LocationLevel + 'Level.'
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

create Procedure Prc_AssetTransferValidation
(
	@UserID				int					=	NULL,
	@AssetID			int					=	NULL,
	@LocationCode		nvarchar(max)		=	NULL,
	@LocationID			int					=	NULL,
	@DepartmentID		int					=	NULL,
	@DepartmentCode		nvarchar(max)		=	NULL,
	@DataProcessedBy	nvarchar(50)		=   null, 
	@ErrorID			int					OutPut,
	@ErrorMsg			nvarchar(max)		Output
)
as 
Begin 
	Declare @LocationLevel int,@UserLocationMapping int,@UserDepartmentMapping int ,@AssetLocationID int,@AssetDepartmentID int

	Select @AssetLocationID=LocationID,@AssetDepartmentID=DepartmentID
		From AssetTable where AssetID=@AssetID

	IF ((@LocationID is null or @LocationID ='') and (@LocationCode is null or @LocationCode=''))
	Begin
	 Set @LocationID=@AssetLocationID
	End 
	IF ((@DepartmentID is null or @DepartmentID ='') and (@DepartmentCode is null or @DepartmentCode=''))
	Begin
	 Set @DepartmentID=@AssetDepartmentID
	End 

	Declare @UserLocationTable Table(LocationID int)
	Declare @UserDepartmentTable Table(DepartmentID int)

	Select @LocationLevel=ConfiguarationValue from ConfigurationTable where ConfiguarationName='PreferredLevelLocationMapping'

	Select @UserLocationMapping=case when ConfiguarationValue='true' then 1 else 0 end from ConfigurationTable
		where ConfiguarationName='UserLocationMapping'
	Select @UserDepartmentMapping=case when ConfiguarationValue='true' then 1 else 0 end from ConfigurationTable
		where ConfiguarationName='UserDepartmentMapping'		
	
	Insert into @UserLocationTable(LocationID)
	Select LocationID from UserLocationMappingTable where PersonID=@UserID

	Insert into @UserDepartmentTable(DepartmentID)
	Select DepartmentID from UserDepartmentMappingTable where PersonID=@UserID

	Set @ErrorID	= 0
	set @ErrorMsg= null

	IF @LocationID is null or @LocationID=''
	Begin 
		IF @LocationCode is null or @LocationCode=''
		Begin
			set @errorID=1
			set @ErrorMsg ='Location Code is Missed.'
		return
		End
		Else 
		Begin 
		Select @LocationID=LocationID from LocationTable where LocationCode=@LocationCode
			IF @LocationID is null or @LocationID=''
			Begin
				set @errorID=2
				set @ErrorMsg =@LocationCode+'- Location Code not valid.'
			End 
			Else 
			Begin 
				If exists(select LocationID from LocationNewView where LocationID=@LocationID and Level<@LocationLevel)
				begin
				set @errorID=3
				set @ErrorMsg =@LocationCode+'- Location Code level must be give '+ @LocationLevel + 'Level.'
				end 
			End 
		End 
	End 
	IF @LocationCode is null or @LocationCode=''
	Begin
	  Select @LocationCode=LocationCode from LocationTable where LocationID=@LocationID
	End 
	IF ((@DepartmentID is null or @DepartmentID='') and (@DepartmentCode is not null or @DepartmentCode!=''))
	BEgin 
		 IF not exists(select DEpartmentID from DepartmentTable where DepartmentCode=@DepartmentCode)
		 Begin
			  Set @ErrorID=4
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
		set @ErrorID=6
		Set @ErrorMsg= @DepartmentCode+'- given DepartmentCode not mapped with User.'
		return	
		End 
	End 

	If not Exists(select LocationID from LocationNewView 
		where LocationID=@LocationID and MappedLocationID in (select LocationID from @UserLocationTable))
	Begin 
		set @ErrorID=7
		Set @ErrorMsg=@LocationCode+'- given LocationCode not mapped with User.'
		return		
	End
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

 	--Declartion part 
	Declare @DepartmentID int,@SectionID int,@CustodianID int,@SupplierID int,@AssetConditionID int, @CategoryID int, @ManufacturerID int,@ModelID int ,@ProductID int ,@LocationID int 
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
alter Procedure [dbo].[IPRC_AMSExceImportBulkAssets]
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

 	--Declartion part 
	Declare @DepartmentID int,@SectionID int,@CustodianID int,@SupplierID int,@AssetConditionID int, @CategoryID int, @ManufacturerID int,@ModelID int ,@ProductID int ,@LocationID int 
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
ALTER Procedure [dbo].[Prc_AssetTransferValidation]
(
	@UserID				int					=	NULL,
	@AssetID			int					=	NULL,
	@LocationCode		nvarchar(max)		=	NULL,
	@LocationID			int					=	NULL,
	@DepartmentID		int					=	NULL,
	@DepartmentCode		nvarchar(max)		=	NULL,
	@DataProcessedBy	nvarchar(50)		=   null, 
	@ErrorID			int					OutPut,
	@ErrorMsg			nvarchar(max)		Output
)
as 
Begin 
	Declare @LocationLevel int,@UserLocationMapping int,@UserDepartmentMapping int ,@AssetLocationID int,@AssetDepartmentID int

	Select @AssetLocationID=LocationID,@AssetDepartmentID=DepartmentID
		From AssetTable where AssetID=@AssetID
   
	
	IF ((@DepartmentID is null or @DepartmentID ='') and (@DepartmentCode is null or @DepartmentCode=''))
	Begin
	 Set @DepartmentID=@AssetDepartmentID
	End 

	Declare @UserLocationTable Table(LocationID int)
	Declare @UserDepartmentTable Table(DepartmentID int)

	Select @LocationLevel=ConfiguarationValue from ConfigurationTable where ConfiguarationName='PreferredLevelLocationMapping'

	Select @UserLocationMapping=case when ConfiguarationValue='true' then 1 else 0 end from ConfigurationTable
		where ConfiguarationName='UserLocationMapping'
	Select @UserDepartmentMapping=case when ConfiguarationValue='true' then 1 else 0 end from ConfigurationTable
		where ConfiguarationName='UserDepartmentMapping'		
	
	Insert into @UserLocationTable(LocationID)
	Select LocationID from UserLocationMappingTable where PersonID=@UserID

	Insert into @UserDepartmentTable(DepartmentID)
	Select DepartmentID from UserDepartmentMappingTable where PersonID=@UserID

	Set @ErrorID	= 0
	set @ErrorMsg= null

	IF @LocationID is null or @LocationID=''
	Begin 
		IF @LocationCode is null or @LocationCode=''
		Begin
			set @errorID=1
			set @ErrorMsg ='Location Code is Missed.'
		return
		End
		Else 
		Begin 
		Select @LocationID=LocationID from LocationTable where LocationCode=@LocationCode
			IF @LocationID is null or @LocationID=''
			Begin
				set @errorID=2
				set @ErrorMsg =@LocationCode+'- Location Code not valid.'
			End 
			Else 
			Begin 
				If exists(select LocationID from LocationNewView where LocationID=@LocationID and Level<@LocationLevel)
				begin
				set @errorID=3
				set @ErrorMsg =@LocationCode+'- Location Code level must be give '+ @LocationLevel + 'Level.'
				end 
			End 
		End 
	End 
	IF @LocationCode is null or @LocationCode=''
	Begin
	  Select @LocationCode=LocationCode from LocationTable where LocationID=@LocationID
	End 
	IF ((@DepartmentID is null or @DepartmentID='') and (@DepartmentCode is not null or @DepartmentCode!=''))
	BEgin 
		 IF not exists(select DEpartmentID from DepartmentTable where DepartmentCode=@DepartmentCode)
		 Begin
			  Set @ErrorID=4
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
		set @ErrorID=6
		Set @ErrorMsg= @DepartmentCode+'- given DepartmentCode not mapped with User.'
		return	
		End 
	End 

	If not Exists(select LocationID from LocationNewView 
		where LocationID=@LocationID and MappedLocationID in (select LocationID from @UserLocationTable))
	Begin 
		set @ErrorID=7
		Set @ErrorMsg=@LocationCode+'- given LocationCode not mapped with User.'
		return		
	End
	IF @AssetLocationID=@LocationID
	Begin
	 set @ErrorID=8
		Set @ErrorMsg=@LocationCode+'- Asset Location and To Location are the same.'
		return	
	End 
	
	
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
				set @ErrorMsg =@CategoryCode+'- Category Code level must be give '+ @CategoryLevel + 'Level.'
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
				set @ErrorMsg =@LocationCode+'- Location Code level must be give '+ @LocationLevel + 'Level.'
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
Create Procedure Prc_AssetRetirementValidation
(
	@UserID				int					=	NULL,
	@AssetID			int					=	NULL,
	@DataProcessedBy	nvarchar(50)		=   null, 
	@ErrorID			int					OutPut,
	@ErrorMsg			nvarchar(max)		Output
)
as 
Begin
Declare @LocationLevel int,@CategoryLevel int,@UserLocationMapping int,@UserCategoryMapping int ,@UserDepartmentMapping int 
	,@AssetCategoryID int ,@AssetLocationID int,@AssetDepartmentID int,@categoryCode nvarchar(max),@LocationCode nvarchar(max),
	@DepartmentCode nvarchar(max),@CategoryID int,@LocationID int,@DepartmentID int ,@ActiveStatusID int,@Barcode nvarchar(max)

	Select  @ActiveStatusID=[dbo].fnGetActiveStatusID()

	Set @ErrorID	= 0
	set @ErrorMsg= null

	IF not exists(select AssetID from AssetTable where AssetID=@AssetID and StatusID=@ActiveStatusID) 
	Begin
	Select @Barcode=Barcode from  AssetTable where AssetID=@AssetID 
	set @ErrorID=7
		Set @ErrorMsg= @Barcode+'- given Barcode is not an active status.'
		return	
	End 
	Select @CategoryID=CategoryID,@LocationID=LocationID,@DepartmentID=DepartmentID 
		From AssetTable where AssetID=@AssetID and StatusID=@ActiveStatusID

	
	Declare @UserCategoryTable Table(CategoryID int)
	Declare @UserLocationTable Table(LocationID int)
	Declare @UserDepartmentTable Table(DepartmentID int)

	Select @LocationLevel=ConfiguarationValue from ConfigurationTable where ConfiguarationName='PreferredLevelLocationMapping'
	Select @CategoryLevel=ConfiguarationValue from ConfigurationTable where ConfiguarationName='PreferredLevelCategoryMapping'

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
	
	IF @CategoryCode is null or @CategoryCode=''
	Begin
	  Select @CategoryCode=CategoryCode from CategoryTable where categoryID=@CategoryID
	End 
	IF @LocationCode is null or @LocationCode=''
	Begin
	  Select @LocationCode=LocationCode from LocationTable where LocationID=@LocationID
	End  
	IF ((@DepartmentID is not null or @DepartmentID!='') and (@DepartmentCode is  null or @DepartmentCode=''))
	BEgin  
		SElect @DepartmentCode=DepartmentCode from DepartmentTable where DepartmentID=@DepartmentID
	End
	If @DepartmentID is not null
	Begin 
		If not Exists(Select DepartmentID from DepartmentTable where DepartmentID=@DepartmentID
			and DepartmentID in (select DepartmentID from @UserDepartmentTable))
		BEgin 
		set @ErrorID=1
		Set @ErrorMsg= @DepartmentCode+'- given DepartmentCode not mapped with User.'
		return	
		End 
	End 

	If exists(select categoryID from CategoryNewView where CategoryID=@CategoryID and Level<@CategoryLevel)
	begin
	set @errorID=2
	set @ErrorMsg =@CategoryCode+'- Category Code level must be give '+ @CategoryLevel + 'Level.'
	end 
	If exists(select LocationID from LocationNewView where LocationID=@LocationID and Level<@LocationLevel)
	begin
	set @errorID=3
	set @ErrorMsg =@LocationCode+'- Location Code level must be give '+ @LocationLevel + 'Level.'
	return
	end 
	If not Exists(select CategoryID from CategoryNewView 
		where CategoryID=@CategoryID and MappedCategoryID in (select CategoryID from @UserCategoryTable))
	Begin 
		set @ErrorID=4
		Set @ErrorMsg=@categoryCode+'- given CategoryCode not mapped with User.'
		return		
	End 
	If not Exists(select LocationID from LocationNewView 
		where LocationID=@LocationID and MappedLocationID in (select LocationID from @UserLocationTable))
	Begin 
		set @ErrorID=5
		Set @ErrorMsg=@LocationCode+'- given LocationCode not mapped with User.'
		return		
	End
End 
go 
ALTER Procedure [dbo].[Prc_AssetTransferValidation]
(
	@UserID				int					=	NULL,
	@AssetID			int					=	NULL,
	@LocationCode		nvarchar(max)		=	NULL,
	@LocationID			int					=	NULL,
	@DepartmentID		int					=	NULL,
	@DepartmentCode		nvarchar(max)		=	NULL,
	@DataProcessedBy	nvarchar(50)		=   null, 
	@ErrorID			int					OutPut,
	@ErrorMsg			nvarchar(max)		Output
)
as 
Begin 
	Declare @LocationLevel int,@UserLocationMapping int,@UserDepartmentMapping int ,@AssetLocationID int,@AssetDepartmentID int
	,@Barcode nvarchar(max),@ActiveStatusID int

	Select  @ActiveStatusID=[dbo].fnGetActiveStatusID()
	Set @ErrorID	= 0
	set @ErrorMsg= null

	IF not exists(select AssetID from AssetTable where AssetID=@AssetID and StatusID=@ActiveStatusID) 
	Begin
	Select @Barcode=Barcode from  AssetTable where AssetID=@AssetID 
	set @ErrorID=7
		Set @ErrorMsg= @Barcode+'- given Barcode is not an active status.'
		return	
	End 

	Select @AssetLocationID=LocationID,@AssetDepartmentID=DepartmentID
		From AssetTable where AssetID=@AssetID
   
	
	IF ((@DepartmentID is null or @DepartmentID ='') and (@DepartmentCode is null or @DepartmentCode=''))
	Begin
	 Set @DepartmentID=@AssetDepartmentID
	End 

	Declare @UserLocationTable Table(LocationID int)
	Declare @UserDepartmentTable Table(DepartmentID int)

	Select @LocationLevel=ConfiguarationValue from ConfigurationTable where ConfiguarationName='PreferredLevelLocationMapping'

	Select @UserLocationMapping=case when ConfiguarationValue='true' then 1 else 0 end from ConfigurationTable
		where ConfiguarationName='UserLocationMapping'
	Select @UserDepartmentMapping=case when ConfiguarationValue='true' then 1 else 0 end from ConfigurationTable
		where ConfiguarationName='UserDepartmentMapping'		
	
	Insert into @UserLocationTable(LocationID)
	Select LocationID from UserLocationMappingTable where PersonID=@UserID

	Insert into @UserDepartmentTable(DepartmentID)
	Select DepartmentID from UserDepartmentMappingTable where PersonID=@UserID

	

	IF @LocationID is null or @LocationID=''
	Begin 
		IF @LocationCode is null or @LocationCode=''
		Begin
			set @errorID=1
			set @ErrorMsg ='Location Code is Missed.'
		return
		End
		Else 
		Begin 
		Select @LocationID=LocationID from LocationTable where LocationCode=@LocationCode
			IF @LocationID is null or @LocationID=''
			Begin
				set @errorID=2
				set @ErrorMsg =@LocationCode+'- Location Code not valid.'
			End 
			Else 
			Begin 
				If exists(select LocationID from LocationNewView where LocationID=@LocationID and Level<@LocationLevel)
				begin
				set @errorID=3
				set @ErrorMsg =@LocationCode+'- Location Code level must be give '+ @LocationLevel + 'Level.'
				end 
			End 
		End 
	End 
	IF @LocationCode is null or @LocationCode=''
	Begin
	  Select @LocationCode=LocationCode from LocationTable where LocationID=@LocationID
	End 
	IF ((@DepartmentID is null or @DepartmentID='') and (@DepartmentCode is not null or @DepartmentCode!=''))
	BEgin 
		 IF not exists(select DEpartmentID from DepartmentTable where DepartmentCode=@DepartmentCode)
		 Begin
			  Set @ErrorID=4
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
		set @ErrorID=6
		Set @ErrorMsg= @DepartmentCode+'- given DepartmentCode not mapped with User.'
		return	
		End 
	End 

	If not Exists(select LocationID from LocationNewView 
		where LocationID=@LocationID and MappedLocationID in (select LocationID from @UserLocationTable))
	Begin 
		set @ErrorID=7
		Set @ErrorMsg=@LocationCode+'- given LocationCode not mapped with User.'
		return		
	End
	IF @AssetLocationID=@LocationID
	Begin
	 set @ErrorID=8
		Set @ErrorMsg=@LocationCode+'- Asset Location and To Location are the same.'
		return	
	End 
	
	
End 
go 
ALTER Procedure [dbo].[prc_ValidateForTransaction]
(
   @FromAssetID				nvarchar(max)	 = NULL,
   @ToLocationID			int				 = NULL,
   @moduleID				int              = NULL,
   @RightName				nvarchar(100)	 = NULL,
   @ErrorID					int OutPut,
   @ErrorMessage			nvarchar(max) OutPut
)
as 
Begin
	Declare @UpdateCount int,@ApprovalCnt int ,@statusID int,@CategoryTypeCnt int --@modelID int ,
	,@CategoryType nvarchar(max),@ID int,@FromLocationtypeID int, @ToLocationTypeID int,@ApprovalWorkflowID int 

	select @statusID=[dbo].fnGetActiveStatusID()
	
	Set @ErrorID=0
	Set @ErrorMessage=null

	declare @TransactionLineItemTable table (AssetID int,FromLocationL2 int ,ToLocationL2 int,
	LocationType nvarchar(100),CategoryType nvarchar(100),ApprovalWorkFlowID int,Barcode nvarchar(max),MappedCategryName nvarchar(max) )
	
	if not exists(select mappedLocationID from LocationNewView where MappedLocationID=@ToLocationID and @moduleID!=11)
	 Begin 
		 Declare @LocationCode nvarchar(max)
		 Select @LocationCode=LocationCode from LocationTable where LocationID=@ToLocationID
		 Set @ErrorID=1
		 Set @ErrorMessage=@LocationCode+'- Selected Loction Code is not Perferred Location Level.'
		 return
	 end 

	insert into @TransactionLineItemTable(AssetID,FromLocationL2,ToLocationL2,LocationType,CategoryType,ApprovalWorkFlowID,Barcode,MappedCategryName)
	select AssetID,LocationL2,@ToLocationID,LocationType,categorytype,null,Barcode,B.CategoryName 
	From AssetNewView a join CategoryTable b on a.MappedCategoryID=b.CategoryID where assetid in (select value from Split(@fromAssetID,',')) 

	Select @CategoryTypeCnt=count(CategoryType) from @TransactionLineItemTable group by LocationType
	if(@CategoryTypeCnt>1)
	begin
		if not exists(select CategoryTypeName from CategoryTypeTable where IsAllCategoryType=1 and statusID=@statusID)
		Begin
		Set @ErrorID=2
		Set @ErrorMessage='Selected Assets have both IT and NON-IT so CategoryType-All Should be created.'
		return
		End 
		Select @CategoryType=CategoryTypeName from CategoryTypeTable where IsAllCategoryType=1 and statusID=@statusID
		update @TransactionLineItemTable set CategoryType=@CategoryType
	End 
	Else 
	Begin 
		Select @CategoryType=CategoryType from @TransactionLineItemTable group by CategoryType
	end 
	if(select count(FromLocationL2) from @TransactionLineItemTable group by FromLocationL2)>1
	begin 
		Set @ErrorID=3
		Set @ErrorMessage='Selected Asset have different Mapped Location please select same second level location .'
		return
	End
	if(select count(LocationType) from @TransactionLineItemTable group by LocationType)>1
	begin 
		Set @ErrorID=4
		Set @ErrorMessage='Selected Asset have different location type please select same location type.'
		return
	End
	Select @FromLocationtypeID=LocationTypeID from LocationTypeTable 
		where LocationTypeName=(select top 1 LocationType from @TransactionLineItemTable)
	Select @ToLocationTypeID=LocationTypeID from LocationTable where LocationID=@ToLocationID

	if not exists(select b.ApprovalRoleID from ApproveWorkflowTable a 
				  join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
				  where a.StatusID=@statusID and b.StatusID=@statusID and 
				  a.ApproveModuleID=(select ApproveModuleID from ApproveModuleTable where ApproveModuleID=@moduleID and StatusID=@statusID)
				  and a.FromLocationTypeID=@FromLocationtypeID and 
				  case when @moduleID=5 or @moduleID=11 then a.ToLocationTypeID else NULL end=@ToLocationTypeID)
			Begin 
				Declare @FromLocationType nvarchar(100),@ToLocationType nvarchar(100)
				Select top 1 @FromLocationType=Locationtype from @TransactionLineItemTable 
				Select @ToLocationType=LocationType From LocationNewView where LocationID=@ToLocationID
				if @moduleID=5 or @moduleID=11 
				Begin 
				Set @ErrorMessage ='Workflow not defined for From LocationType-'+@FromLocationType+' and To Location Type - '+ @ToLocationType+'.'
				End 
				Else 
				Begin 
				Set @ErrorMessage ='Workflow not defined for From LocationType-'+@FromLocationType+'.'
				End 
				Set @ErrorID=5
				return
			End 
	select @ApprovalWorkflowID=a.ApproveWorkflowID from ApproveWorkflowTable a 
				  join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
				  where a.StatusID=@statusID and b.StatusID=@statusID and 
				  a.ApproveModuleID=(select ApproveModuleID from ApproveModuleTable where ApproveModuleID=@moduleID and StatusID=@statusID)
				  and a.FromLocationTypeID=@FromLocationtypeID and 
				  case when @moduleID=5 or @moduleID=11 then a.ToLocationTypeID else NULL end=@ToLocationTypeID
    
	update @TransactionLineItemTable set ApprovalWorkFlowID=@ApprovalWorkflowID
	 
	SElect @moduleID=ApproveModuleID from ApproveWorkflowTable where ApproveWorkflowID=@ApprovalWorkFlowID
	
	Declare @updateTable table(updateRole bit,ApprovalRoleID int,moduleName nvarchar(100),FromLocationType nvarchar(100),ToLocationType nvarchar(100),ModuleID int)
	  
	if @moduleID=11 
	begin
		select @ToLocationID =MappedLocationID from LocationNewView where LocationID=@ToLocationID
		update @TransactionLineItemTable set ToLocationL2=@ToLocationID
	end
	if @moduleID=5 
	Begin
	   if exists (select top 1 assetID from @TransactionLineItemTable where FromLocationL2=ToLocationL2)
	   begin 
	     Set @ErrorID=12
		Set @ErrorMessage='From and To Location are the Same , Please select different Location.'
		return
	   End 
	end 

	Select @ApprovalCnt=count(OrderNo) from 
	ApproveWorkflowTable a join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
	where a.StatusID=@statusID and b.StatusID=@statusID and a.ApproveWorkflowID=@ApprovalWorkFlowID

	if not exists(select * from @TransactionLineItemTable where CategoryType is not null and CategoryType!='') 
	begin 
		Declare @MissingCategoryName nvarchar(max)
		Select @MissingCategoryName=stuff((Select ',' + T.MappedCategryName from @TransactionLineItemTable T 
							 where T.CategoryType is null and T.AssetID=a.AssetID FOR XML PATH('')), 1, 1, '') from @TransactionLineItemTable a 
		Set @ErrorID=6
		set @ErrorMessage=@MissingCategoryName+' Category Type(s) are Missed.'
		return
	End 

	insert into @updateTable(updateRole,ApprovalRoleID,moduleName,FromLocationType,ToLocationType,ModuleID)
		Select case when a.ApproveModuleID=5 or a.ApproveModuleID=11 then UpdateDestinationLocationsForTransfer 
				when  a.ApproveModuleID=10 then UpdateRetirementDetailsForEachAssets else cast(0 as bit) end as updateRole, 
		b.ApprovalRoleID,d.ModuleName,FL.LocationTypeName,TL.LocationTypeName,a.ApproveWorkflowID FRom ApproveWorkflowTable a 
		join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
		join ApproveModuleTable D on a.ApproveModuleID=d.ApproveModuleID
		join ApprovalRoleTable C on b.ApprovalRoleID=C.ApprovalRoleID
		Join LocationTypeTable FL on a.FromLocationTypeID=FL.LocationTypeID
		Left join LocationTypeTable TL on a.ToLocationTypeID=TL.LocationTypeID
		where a.ApproveWorkflowID=@ApprovalWorkFlowID and a.StatusID=@statusID and b.StatusID=@statusID and c.StatusID=@statusID

	Declare @MissedModuleName nvarchar(100),@MissedFromLocationtype nvarchar(100),@MissedToLocationtype nvarchar(100),@MissedModuleID int,@errorMsg nvarchar(max)
	Select top 1 @MissedModuleName=moduleName,@MissedFromLocationtype=FromLocationType,@MissedToLocationtype=ToLocationType,@MissedModuleID=ModuleID from @updateTable
	If(@moduleID=5 or @moduleID=11 or @moduleID=10)
	Begin
		if not exists(select * from @updateTable where updateRole=1)
		Begin 
			Set @ErrorID=7
				If @MissedModuleID=5 or @MissedModuleID=11
				Begin
				Set @errorMsg=@MissingCategoryName+' Update option not provided for modulename : '+@MissedModuleName+',From Location Type :'+@MissedFromLocationtype+',To Location type : '+@MissedToLocationtype+'.'
				End 
				Else 
				Begin
				Set @errorMsg=@MissingCategoryName+' Update option not provided for modulename : '+@MissedModuleName+',From Location Type :'+@MissedFromLocationtype+'.'
				End
			set @ErrorMessage=@errorMsg
			return
		End 
		if (select updateRole from @updateTable group by updateRole having count(updateRole)>1)>1
		Begin 
			Set @ErrorID=8
				If @MissedModuleID=5 or @MissedModuleID=11
				Begin
				Set @errorMsg=@MissingCategoryName+' Update option shouldnot be enabled for more than one, modulename : '+@MissedModuleName+',From Location Type :'+@MissedFromLocationtype+',To Location type : '+@MissedToLocationtype+'.'
				End 
				Else 
				Begin
				Set @errorMsg=@MissingCategoryName+' Update option shouldnot be enabled for more than one, modulename : '+@MissedModuleName+',From Location Type :'+@MissedFromLocationtype+'.'
				End
			set @ErrorMessage=@errorMsg
			return
		End 
	End 
	 Declare @validationTable Table(UserID int,LocationID int,ApprovalRoleID int,CategoryTypeCnt int ) 
	 Declare @validationTable1 Table(UserID int,LocationID int,ApprovalRoleID int,CategoryTypeCnt int ) 
	 Declare @validationTable2 Table(UserID int,LocationID int,ApprovalRoleID int,CategoryTypeCnt int ) 

	insert into @validationTable(UserID,LocationID,ApprovalRoleID,CategoryTypeCnt)
		select userID,LocationID,a.ApprovalRoleID,count(d.categoryTypeID) as categorytypeCnt 
		from ApproveWorkflowLineItemTable a 
		join @TransactionLineItemTable T on a.ApproveWorkFlowID=T.ApprovalWorkFlowID
		join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
		join CategoryTypeTable CT on T.CategoryType=Ct.categoryTypeName
		join 
		(
			select a1.*,b1.IsAllCategoryType from UserApprovalRoleMappingTable a1  
			join CategoryTypeTable b1 on a1.CategoryTypeID=b1.CategoryTypeID
			where a1.StatusID=@statusID and b1.StatusID=@statusID
		) D on  case when b.ApprovalLocationTypeID=5 then 
		T.FromLocationL2 when b.ApprovalLocationTypeID=10 then T.ToLocationL2 else T.FromLocationL2 end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID 
		and ct.CategoryTypeID = (case when CT.IsAllCategoryType=0 and D.IsAllCategoryType=1 then CT.CategoryTypeID else D.CategoryTypeID end)
			where ApproveWorkFlowID=@ApprovalWorkFlowID and CT.statusID=@statusID and a.StatusID=@statusID
			and b.StatusID=@statusID and D.StatusID=@statusID
			group by userID,LocationID,a.ApprovalRoleID

	if(select count(*) from (
			select  a.ApprovalRoleID  from @validationTable a group by a.ApprovalRoleID)x) !=@ApprovalCnt
	Begin 
		Declare @MissingApprovalRoleName nvarchar(max),@ModuleName nvarchar(max)
		
		Select @MissingApprovalRoleName=stuff((Select ',' + b.ApprovalRoleName From ApproveWorkflowLineItemTable a Join ApprovalRoleTable b on a.ApprovalRoleID=b.ApprovalRoleID
		where ApproveWorkFlowID=@ApprovalWorkFlowID and a.StatusID=@statusID and b.StatusID=@statusID and a.ApprovalRoleID not in(select ApprovalRoleID from @validationTable)
		and b.ApprovalRoleID=p.ApprovalRoleID FOR XML PATH('')), 1, 1, '') from ApprovalRoleTable P
		
		Select @ModuleName=b.ModuleName from ApproveWorkflowTable a 
			join ApproveModuleTable b on a.ApproveModuleID=b.ApproveModuleID 
				where ApproveWorkFlowID=@ApprovalWorkFlowID and a.StatusID=@statusID
		 
		Set @ErrorMessage ='Missing user(s) for the role : '+@MissingApprovalRoleName+' , ModuleName :'+@ModuleName+'.'
		Set @ErrorID=9
		return
	end 
    Else 
	Begin 
	insert into @validationTable1(UserID,LocationID,ApprovalRoleID,CategoryTypeCnt)
		select a.userID,a.LocationID,a.ApprovalRoleID,categorytypeCnt
		From @validationTable a  
		join [ValidateAccessRightsView]  b  on a.UserID=b.UserID --and a.LocationID=b.LocationID and a.ApprovalRoleID=b.ApprovalRoleID
		and b.RightName=@RightName
	if(select count(*) from (
		select  a.ApprovalRoleID  from @validationTable1 a group by a.ApprovalRoleID)x )!=@ApprovalCnt
	Begin 
		Set @ErrorID=10
		Declare @MissingUserName nvarchar(max)

		Select @MissingUserName=stuff((Select ',' + PersonCode from @validationTable a join PersonTable b on a.UserID=b.PersonID 
		where userID not in (select userID from @validationTable1) and b.personID=p.PersonID FOR XML PATH('')), 1, 1, '') from PersonTable P
		
		Set @ErrorMessage ='Approval Rights not provided for the user(s)- '+@MissingUserName +'.'
		return
	END
	if (select count(*) from (
		select  a.ApprovalRoleID  from @validationTable1 a group by a.ApprovalRoleID)x  )=@ApprovalCnt
	begin 
		insert into @validationTable2(UserID,LocationID,ApprovalRoleID,CategoryTypeCnt)
		select a.userID,a.LocationID,a.ApprovalRoleID,categorytypeCnt
		From @validationTable a  
		join persontable p on a.userID=p.personID and p.EMailID is not null and p.StatusID=@statusID
 if(select count(*) from (
	select  a.ApprovalRoleID  from @validationTable2 a group by a.ApprovalRoleID)x  )!=@ApprovalCnt
	 Begin 
		Declare @MissingUserEmail nvarchar(max)

		Select @MissingUserEmail=stuff((Select ',' + PersonCode from @validationTable a join PersonTable b on a.UserID=b.PersonID 
			where userID not in (select userID from @validationTable2) and b.personID=p.PersonID FOR XML PATH('')), 1, 1, '') from PersonTable P
		
		Set @ErrorMessage ='EmailID not assigned to the mapped workflow user(s)- '+@MissingUserEmail +'.'
		Set @ErrorID=11
		RETURN
	 END 
	END 
	END
		
		
  end  
  go 
  Alter table ZDOF_UserPODataTable add LocationID int foreign key references LocationTable(LocationID)

  go
  
ALTER trigger [dbo].[trg_Ins_AssetDataTable] on [dbo].[ZDOF_UserPODataTable] 
after Insert
As
Begin
   Declare @ProductID int,@categoryID int,@qty int ,@zDOFPurchaseOrderID int ,@PDHeaderID nvarchar(300) ,@PoLineItemID nvarchar(300),@ProductName nvarchar(max),@UnitCost decimal(18,5),@SupplierID int,@CreatedBy int 
   Declare @DefaultLanguageID INT,@CompanyID	INT,@Cnt int ,@LastBarcodeVal INT,@CompanyCode nvarchar(10),@LocationID int 
   Declare @BarcodeAuto bit ,@BarcodeAssetCodeequal bit ,@DepartmentID int 
     Select @BarcodeAuto=case when upper(ConfiguarationValue)='TRUE' then 1 else 0 end  From ConfigurationTable where ConfiguarationName='BarcodeAutoGenerateEnable'
	 Select @BarcodeAssetCodeequal=case when upper(ConfiguarationValue)='TRUE' then 1 else 0 end  From ConfigurationTable where ConfiguarationName='BarcodeEqualAssetCode'
	
	SELECT @DefaultLanguageID = MIN(LanguageID) FROM LanguageTable
	SELECT @CompanyID = MIN(CompanyID) FROM CompanyTable WHERE StatusID = 1
	Select @CompanyCode=left(companyCode,4) From CompanyTable where companyID=@CompanyID
  
    Select @qty=QUANTITY, @zDOFPurchaseOrderID=ZDOFPurchaseOrderID, @PDHeaderID=PO_HEADER_ID, @PoLineItemID=PO_LINE_ID, @ProductName=PO_LINE_DESC,
	@UnitCost=UnitCost,@categoryID=CategoryID,@CreatedBy=CreatedBy,@DepartmentID=DepartmentID,@LocationID=LocationID From Inserted
	
	IF NOT EXISTS(SELECT * FROM SupplierTable WHERE SupplierCode 
		IN(SELECT VENDOR_NUMBER FROM ZDOF_PurchaseOrderTable WHERE VENDOR_NUMBER IS NOT NULL AND ZDOFPurchaseOrderID = @zDOFPurchaseOrderID) )
	BEGIN
		--Generate Supplier data
		INSERT INTO PartyTable(partycode, StatusID, CreatedBy,PartyName,CreatedDateTime,PartyTypeID)
		SELECT VENDOR_NUMBER, 1, @CreatedBy,VENDOR_NAME,getdate(),(select partyTypeID from PartyTypeTable where PartyType='Supplier')
			FROM  ZDOF_PurchaseOrderTable A 
			LEFT JOIN PartyTable B ON A.VENDOR_NUMBER = B.PartyCode
			WHERE A.VENDOR_NUMBER IS NOT NULL AND A.VENDOR_NAME IS NOT NULL AND  A.ZDOFPurchaseOrderID = @zDOFPurchaseOrderID AND AssetsCreated = 0 AND B.PartyID IS NULL

		--INSERT INTO p(SupplierID, SupplierDescription, LanguageID, CreatedBy)
		--SELECT B.SupplierID, VENDOR_NAME, @DefaultLanguageID, @CreatedBy
		--	FROM  ZDOF_PurchaseOrderTable A 
		--	LEFT JOIN SupplierTable B ON A.VENDOR_NUMBER = B.SupplierCode
		--	WHERE A.VENDOR_NUMBER IS NOT NULL AND A.VENDOR_NAME IS NOT NULL AND  A.ZDOFPurchaseOrderID = @zDOFPurchaseOrderID AND AssetsCreated = 0 AND B.SupplierID IS NOT NULL
	END

    IF Not Exists (select ProductName from  ProductTable  where 
						 ProductName=@ProductName and CategoryID=@CategoryID) 
	Begin 
	   Insert into ProductTable(ProductCode,CategoryID,StatusID,Createdby,CreatedDateTime,ProductName) 
	   values(@PoLineItemID,@categoryID,1,@CreatedBy,getdate(),@ProductName)
	   SET @ProductID = SCOPE_IDENTITY()
	   Insert into ProductDescriptionTable(ProductID,ProductDescription,LanguageID,Createdby,CreatedDatetime)
	   Select @ProductID,@ProductName,LanguageID,@CreatedBy,GETDATE()
	   From LanguageTable
	End 
	Else
	Begin 
	   Select @ProductID=ProductID from ProductTable  where categoryID=@categoryID and ProductName=@ProductName
	End 
	set @cnt=0

		Select @LastBarcodeVal = BarcodeLastValue from BarcodeAutoSequenceTable where companyID=@CompanyID

			WHILE(@CNT < @qty)
			BEGIN
				SET @CNT = @CNT + 1
			----6065 Barcode sequence is wrong
			--	SELECT @LastBarcodeVal = MAX(CAST(SUBSTRING(Barcode, 5, 7) as int)) FROM AssetTable 
			--		WHERE AssetID <> 6065 AND ISNUMERIC(SUBSTRING(Barcode, 5, 7)) = 1 AND StatusID <> 3

			--	IF((@LastBarcodeVal IS NULL) OR (@LastBarcodeVal < 13247))
			--		SET @LastBarcodeVal  = 13247

			--	SET @LastBarcodeVal  = @LastBarcodeVal  + 1

			INSERT INTO ASSETTABLE (BARCODE,ASSETCODE,CATEGORYID,PRODUCTID,SUPPLIERID,PurchasePrice,
				PONUMBER,ASSETDESCRIPTION,PURCHASEDATE,ComissionDate,
				STATUSID,COMPANYID,CreatedBy,CreatedDateTime,AssetApproval,CreateFromHHT,ERPUpdateType,
				DepreciationFlag,Attribute1,DepartmentID, DOFPO_LINE_NUM,LocationID)
					
				SELECT 
					'-', '-',
					--'AT' + FORMAT(@LastBarcodeVal, '00000000'),
					--'AT' + FORMAT(@LastBarcodeVal, '00000000'),
				--Select case when @BarcodeAuto=1 then '-' else @CompanyCode + FORMAT(@LastBarcodeVal, '000000') end , 
				--case when @BarcodeAuto=1 and @BarcodeAssetCodeequal=1 then '-' else @CompanyCode+ FORMAT(@LastBarcodeVal, '000000') end ,
				@categoryID,@ProductID,@SupplierID,
				@UnitCost,PO_NUMBER,@ProductName,PO_DATE,PO_DATE,1,@CompanyID,@CreatedBy,getdate(),1,0, 5,
				0,VENDOR_SITE_CODE,@DepartmentID, ISNULL(LINE_NUM, 1),@LocationID
				From ZDOF_PurchaseOrderTable where ZDOFPurchaseOrderID = @zDOFPurchaseOrderID

			SET @LastBarcodeVal = @LastBarcodeVal + 1					
		End 

		--Update BarcodeAutoSequenceTable set BarcodeLastValue = @LastBarcodeVal where CompanyID=@CompanyID

		UPDATE ZDOF_PurchaseOrderTable 
					SET GeneratedAssetQty = GeneratedAssetQty + @qty
					WHERE ZDOFPurchaseOrderID = @zDOFPurchaseOrderID

		UPDATE ZDOF_PurchaseOrderTable 
		SET AssetsCreated = 1
		WHERE ZDOFPurchaseOrderID = @zDOFPurchaseOrderID AND CONVERT(INT,ROUND(QUANTITY,0,0),0)<= GeneratedAssetQty			
End 
go 
create View [dbo].[LocationForUserMappingView] 
as  
	Select a.LocationCode,a.LocationName,b.LocationName AS ParentLocation,a.LocationID,b.LocationName+'/'+a.LocationName as SecondLevelLocationName,l.LocationTypeName as LocationType
	   from LocationTable a 
	   join LocationTable b on a.ParentLocationID=b.LocationID
	   left join LocationTypeTable l ON A.LocationTypeID=L.LocationTypeID
	   where b.ParentLocationID is null and a.StatusID not in (500,3) and b.StatusID not in (500,3)
GO


ALTER Procedure [dbo].[prc_ValidateForTransaction]
(
   @FromAssetID				nvarchar(max)	 = NULL,
   @ToLocationID			int				 = NULL,
   @ModuleID				int              = NULL,
   @RightName				nvarchar(100)	 = NULL,
   @ErrorID					int OutPut,
   @ErrorMessage			nvarchar(max) OutPut
)
as 
Begin
	DECLARE @ASSETTRANSFER INT = 5,
			@ASSETRETIREMENT INT = 10,
			@INTERNALASSETTRANSFER INT = 11, 
			@ASSETADDITION INT = 4,
			@ASSETMAINTENANCE INT = 15,
			@ASSETMAINTENANCEREQUEST INT = 16

	Declare @UpdateCount int,@ApprovalCnt int ,@statusID int,@CategoryTypeCnt int --@modelID int ,
		,@CategoryType nvarchar(max),@ID int,@FromLocationtypeID int, @ToLocationTypeID int,@ApprovalWorkflowID int ,@HQID int,@MappedToLocationID int

	select @statusID=[dbo].fnGetActiveStatusID()
	--select * from LocationTypeTable

	select @HQID=locationtypeid from LocationTypeTable where LocationTypeName='Head Quarters' and StatusID=@statusID

	Set @ErrorID=0
	Set @ErrorMessage=null

	declare @TransactionLineItemTable table (AssetID int,FromLocationL2 int ,ToLocationL2 int,
			LocationType nvarchar(100),CategoryType nvarchar(100),ApprovalWorkFlowID int,Barcode nvarchar(max),
			MappedCategryName nvarchar(max) )
	
	insert into @TransactionLineItemTable(AssetID,FromLocationL2,ToLocationL2,LocationType,CategoryType,ApprovalWorkFlowID,Barcode,MappedCategryName)
	select AssetID,LocationL2,@ToLocationID,LocationType,categorytype,null,Barcode,B.CategoryName 
	From AssetNewView a join CategoryTable b on a.CategoryID=b.CategoryID where assetid in (select value from Split(@fromAssetID,',')) 

	IF(@ModuleID = @INTERNALASSETTRANSFER)
	BEGIN
		select @MappedToLocationID =MappedLocationID from LocationNewView where LocationID=@ToLocationID
		update @TransactionLineItemTable set ToLocationL2=@MappedToLocationID
		
		If @ToLocationID is null or @ToLocationID=''
		Begin 
		Set @ErrorID=15
		Set @ErrorMessage='Selected Loction Code is not Preferred Location Level.'
		return
		end 

		If not exists(select AssetID From @TransactionLineItemTable where FromLocationL2=ToLocationL2)
		Begin
		Set @ErrorID=14
		Set @ErrorMessage='Selected From Location and To Location should be Same.'
		return
		End
		Else 
		Begin 
		Select @ToLocationTypeID=LocationTypeID from LocationTable where LocationID=@MappedToLocationID
			if @HQID!=@ToLocationTypeID
			Begin 
			  If exists(select LocationID from LocationTable where ParentLocationID=@ToLocationID)
			  Begin 
				Set @ErrorID=16
				Set @ErrorMessage='Non HQ Location should be selected end level.'
				return
			  End 
			End
		
		end 
		--Validate HQ, only requires 2nd level locations
		--NON HQ validate end level should be selected
		--Validate from and to parent location should be same
		--Set @ErrorID = 0
		--Set @ErrorMessage=null
		--return

	END
	ELSE
	BEGIN
		if not exists(select mappedLocationID from LocationNewView where MappedLocationID=@ToLocationID )
		Begin 
			Declare @LocationCode nvarchar(max)
			Select @LocationCode=LocationCode from LocationTable where LocationID=@ToLocationID

			Set @ErrorID=1
			Set @ErrorMessage=@LocationCode+'- Selected Loction Code is not Preferred Location Level.'
			return
		end 
	END

	
	--select * from @TransactionLineItemTable
	Select @CategoryTypeCnt=count(CategoryType) from @TransactionLineItemTable group by LocationType
	if(@CategoryTypeCnt>1)
	begin
		if not exists(select CategoryTypeName from CategoryTypeTable where IsAllCategoryType=1 and statusID=@statusID)
		Begin
		Set @ErrorID=2
		Set @ErrorMessage='Selected Assets have both IT and NON-IT so CategoryType-All Should be created.'
		return
		End 
		Select @CategoryType=CategoryTypeName from CategoryTypeTable where IsAllCategoryType=1 and statusID=@statusID
		update @TransactionLineItemTable set CategoryType=@CategoryType
	End 
	Else 
	Begin 
		Select @CategoryType=CategoryType from @TransactionLineItemTable group by CategoryType
	end 
	if(select count(FromLocationL2) from @TransactionLineItemTable group by FromLocationL2)>1
	begin 
		Set @ErrorID=3
		Set @ErrorMessage='Selected Asset have different Mapped Location please select same second level location .'
		return
	End
	if(select count(LocationType) from @TransactionLineItemTable group by LocationType)>1
	begin 
		Set @ErrorID=4
		Set @ErrorMessage='Selected Asset have different location type please select same location type.'
		return
	End
	IF exists(select LocationType from @TransactionLineItemTable where LocationType is null )
	Begin 
	Set @ErrorID=13
		Set @ErrorMessage='Selected Asset must be map with location type.'
		return
	End 
	--if @moduleID=11 
	--begin
	--	select @ToLocationID =MappedLocationID from LocationNewView where LocationID=@ToLocationID
	--	update @TransactionLineItemTable set ToLocationL2=@ToLocationID
	--end

	Select @FromLocationtypeID=LocationTypeID from LocationTypeTable 
		where LocationTypeName=(select top 1 LocationType from @TransactionLineItemTable)

	Select @ToLocationTypeID=LocationTypeID from LocationTable where LocationID=@ToLocationID
	
	IF @ToLocationTypeID is null or @ToLocationTypeID=''
	Begin 
	Set @ErrorID=14
		Set @ErrorMessage='Selected To Location must be map with location type.'
		return
	End 
	Declare @CheckVal bit 
	set @CheckVal=0

	IF @HQID=@FromLocationtypeID and @HQID=@ToLocationTypeID
	begin
	set @CheckVal=1
	end 

	if @moduleID=11 and(@CheckVal=0)
	begin
		Set @ErrorID=0
		set @ErrorMessage=null
	    return
	End 
	print @CheckVal
	if not exists(select b.ApprovalRoleID from ApproveWorkflowTable a 
				  join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
				  where a.StatusID=@statusID and b.StatusID=@statusID and 
				  a.ApproveModuleID=(select ApproveModuleID from ApproveModuleTable where ApproveModuleID=@moduleID and StatusID=@statusID)
				  and a.FromLocationTypeID=@FromLocationtypeID and 
				  case when @moduleID=5 or @moduleID=11 then a.ToLocationTypeID else NULL end=@ToLocationTypeID)
			Begin 
				Declare @FromLocationType nvarchar(100),@ToLocationType nvarchar(100)
				Select top 1 @FromLocationType=Locationtype from @TransactionLineItemTable 
				Select @ToLocationType=LocationType From LocationNewView where LocationID=@ToLocationID
				if @moduleID=5 or @moduleID=11 
				Begin 
				Set @ErrorMessage ='Workflow not defined for From LocationType-'+@FromLocationType+' and To Location Type - '+ @ToLocationType+'.'
				End 
				Else 
				Begin 
				Set @ErrorMessage ='Workflow not defined for From LocationType-'+@FromLocationType+'.'
				End 
				Set @ErrorID=5
				return
			End 
	select @ApprovalWorkflowID=a.ApproveWorkflowID from ApproveWorkflowTable a 
				  join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
				  where a.StatusID=@statusID and b.StatusID=@statusID and 
				  a.ApproveModuleID=(select ApproveModuleID from ApproveModuleTable where ApproveModuleID=@moduleID and StatusID=@statusID)
				  and a.FromLocationTypeID=@FromLocationtypeID and 
				  case when @moduleID=5 or @moduleID=11 then a.ToLocationTypeID else NULL end=@ToLocationTypeID
    
	update @TransactionLineItemTable set ApprovalWorkFlowID=@ApprovalWorkflowID
	 
	SElect @moduleID=ApproveModuleID from ApproveWorkflowTable where ApproveWorkflowID=@ApprovalWorkFlowID
	
	Declare @updateTable table(updateRole bit,ApprovalRoleID int,moduleName nvarchar(100),FromLocationType nvarchar(100),ToLocationType nvarchar(100),ModuleID int)
	  
	
	if @moduleID=5 
	Begin
	   if exists (select top 1 assetID from @TransactionLineItemTable where FromLocationL2=ToLocationL2)
	   begin 
	     Set @ErrorID=12
		Set @ErrorMessage='From and To Location are the Same , Please select different Location.'
		return
	   End 
	end 

	Select @ApprovalCnt=count(OrderNo) from 
	ApproveWorkflowTable a join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
	where a.StatusID=@statusID and b.StatusID=@statusID and a.ApproveWorkflowID=@ApprovalWorkFlowID

	if not exists(select * from @TransactionLineItemTable where CategoryType is not null and CategoryType!='') 
	begin 
		Declare @MissingCategoryName nvarchar(max)
		Select @MissingCategoryName=stuff((Select ',' + T.MappedCategryName from @TransactionLineItemTable T 
							 where T.CategoryType is null and T.AssetID=a.AssetID FOR XML PATH('')), 1, 1, '') from @TransactionLineItemTable a 
		Set @ErrorID=6
		set @ErrorMessage=@MissingCategoryName+' Category Type(s) are Missed.'
		return
	End 

	insert into @updateTable(updateRole,ApprovalRoleID,moduleName,FromLocationType,ToLocationType,ModuleID)
		Select case when a.ApproveModuleID=5 or a.ApproveModuleID=11 then UpdateDestinationLocationsForTransfer 
				when  a.ApproveModuleID=10 then UpdateRetirementDetailsForEachAssets else cast(0 as bit) end as updateRole, 
		b.ApprovalRoleID,d.ModuleName,FL.LocationTypeName,TL.LocationTypeName,a.ApproveWorkflowID FRom ApproveWorkflowTable a 
		join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
		join ApproveModuleTable D on a.ApproveModuleID=d.ApproveModuleID
		join ApprovalRoleTable C on b.ApprovalRoleID=C.ApprovalRoleID
		Join LocationTypeTable FL on a.FromLocationTypeID=FL.LocationTypeID
		Left join LocationTypeTable TL on a.ToLocationTypeID=TL.LocationTypeID
		where a.ApproveWorkflowID=@ApprovalWorkFlowID and a.StatusID=@statusID and b.StatusID=@statusID and c.StatusID=@statusID

	Declare @MissedModuleName nvarchar(100),@MissedFromLocationtype nvarchar(100),@MissedToLocationtype nvarchar(100),@MissedModuleID int,@errorMsg nvarchar(max)
	Select top 1 @MissedModuleName=moduleName,@MissedFromLocationtype=FromLocationType,@MissedToLocationtype=ToLocationType,@MissedModuleID=ModuleID from @updateTable
	If(@moduleID=5 or @moduleID=11 or @moduleID=10)
	Begin
		if not exists(select * from @updateTable where updateRole=1)
		Begin 
			Set @ErrorID=7
				If @MissedModuleID=5 or @MissedModuleID=11
				Begin
				Set @errorMsg=@MissingCategoryName+' Update option not provided for modulename : '+@MissedModuleName+',From Location Type :'+@MissedFromLocationtype+',To Location type : '+@MissedToLocationtype+'.'
				End 
				Else 
				Begin
				Set @errorMsg=@MissingCategoryName+' Update option not provided for modulename : '+@MissedModuleName+',From Location Type :'+@MissedFromLocationtype+'.'
				End
			set @ErrorMessage=@errorMsg
			return
		End 
		if (select updateRole from @updateTable group by updateRole having count(updateRole)>1)>1
		Begin 
			Set @ErrorID=8
				If @MissedModuleID=5 or @MissedModuleID=11
				Begin
				Set @errorMsg=@MissingCategoryName+' Update option shouldnot be enabled for more than one, modulename : '+@MissedModuleName+',From Location Type :'+@MissedFromLocationtype+',To Location type : '+@MissedToLocationtype+'.'
				End 
				Else 
				Begin
				Set @errorMsg=@MissingCategoryName+' Update option shouldnot be enabled for more than one, modulename : '+@MissedModuleName+',From Location Type :'+@MissedFromLocationtype+'.'
				End
			set @ErrorMessage=@errorMsg
			return
		End 
	End 
	 Declare @validationTable Table(UserID int,LocationID int,ApprovalRoleID int,CategoryTypeCnt int ) 
	 Declare @validationTable1 Table(UserID int,LocationID int,ApprovalRoleID int,CategoryTypeCnt int ) 
	 Declare @validationTable2 Table(UserID int,LocationID int,ApprovalRoleID int,CategoryTypeCnt int ) 

	insert into @validationTable(UserID,LocationID,ApprovalRoleID,CategoryTypeCnt)
		select userID,LocationID,a.ApprovalRoleID,count(d.categoryTypeID) as categorytypeCnt 
		from ApproveWorkflowLineItemTable a 
		join @TransactionLineItemTable T on a.ApproveWorkFlowID=T.ApprovalWorkFlowID
		join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
		join CategoryTypeTable CT on T.CategoryType=Ct.categoryTypeName
		join 
		(
			select a1.*,b1.IsAllCategoryType from UserApprovalRoleMappingTable a1  
			join CategoryTypeTable b1 on a1.CategoryTypeID=b1.CategoryTypeID
			where a1.StatusID=@statusID and b1.StatusID=@statusID
		) D on  case when b.ApprovalLocationTypeID=5 then 
		T.FromLocationL2 when b.ApprovalLocationTypeID=10 then T.ToLocationL2 else T.FromLocationL2 end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID 
		and ct.CategoryTypeID = (case when CT.IsAllCategoryType=0 and D.IsAllCategoryType=1 then CT.CategoryTypeID else D.CategoryTypeID end)
			where ApproveWorkFlowID=@ApprovalWorkFlowID and CT.statusID=@statusID and a.StatusID=@statusID
			and b.StatusID=@statusID and D.StatusID=@statusID
			group by userID,LocationID,a.ApprovalRoleID

	if(select count(*) from (
			select  a.ApprovalRoleID  from @validationTable a group by a.ApprovalRoleID)x) !=@ApprovalCnt
	Begin 
		Declare @MissingApprovalRoleName nvarchar(max),@ModuleName nvarchar(max)
		
		Select @MissingApprovalRoleName=stuff((Select ',' + b.ApprovalRoleName From ApproveWorkflowLineItemTable a Join ApprovalRoleTable b on a.ApprovalRoleID=b.ApprovalRoleID
		where ApproveWorkFlowID=@ApprovalWorkFlowID and a.StatusID=@statusID and b.StatusID=@statusID and a.ApprovalRoleID not in(select ApprovalRoleID from @validationTable)
		and b.ApprovalRoleID=p.ApprovalRoleID FOR XML PATH('')), 1, 1, '') from ApprovalRoleTable P
		
		Select @ModuleName=b.ModuleName from ApproveWorkflowTable a 
			join ApproveModuleTable b on a.ApproveModuleID=b.ApproveModuleID 
				where ApproveWorkFlowID=@ApprovalWorkFlowID and a.StatusID=@statusID
		 
		Set @ErrorMessage ='Missing user(s) for the role : '+@MissingApprovalRoleName+' , ModuleName :'+@ModuleName+'.'
		Set @ErrorID=9
		return
	end 
    Else 
	Begin 
	insert into @validationTable1(UserID,LocationID,ApprovalRoleID,CategoryTypeCnt)
		select a.userID,a.LocationID,a.ApprovalRoleID,categorytypeCnt
		From @validationTable a  
		join [ValidateAccessRightsView]  b  on a.UserID=b.UserID --and a.LocationID=b.LocationID and a.ApprovalRoleID=b.ApprovalRoleID
		and b.RightName=@RightName
	if(select count(*) from (
		select  a.ApprovalRoleID  from @validationTable1 a group by a.ApprovalRoleID)x )!=@ApprovalCnt
	Begin 
		Set @ErrorID=10
		Declare @MissingUserName nvarchar(max)

		Select @MissingUserName=stuff((Select ',' + PersonCode from @validationTable a join PersonTable b on a.UserID=b.PersonID 
		where userID not in (select userID from @validationTable1) and b.personID=p.PersonID FOR XML PATH('')), 1, 1, '') from PersonTable P
		
		Set @ErrorMessage ='Approval Rights not provided for the user(s)- '+@MissingUserName +'.'
		return
	END
	if (select count(*) from (
		select  a.ApprovalRoleID  from @validationTable1 a group by a.ApprovalRoleID)x  )=@ApprovalCnt
	begin 
		insert into @validationTable2(UserID,LocationID,ApprovalRoleID,CategoryTypeCnt)
		select a.userID,a.LocationID,a.ApprovalRoleID,categorytypeCnt
		From @validationTable a  
		join persontable p on a.userID=p.personID and p.EMailID is not null and p.StatusID=@statusID
 if(select count(*) from (
	select  a.ApprovalRoleID  from @validationTable2 a group by a.ApprovalRoleID)x  )!=@ApprovalCnt
	 Begin 
		Declare @MissingUserEmail nvarchar(max)

		Select @MissingUserEmail=stuff((Select ',' + PersonCode from @validationTable a join PersonTable b on a.UserID=b.PersonID 
			where userID not in (select userID from @validationTable2) and b.personID=p.PersonID FOR XML PATH('')), 1, 1, '') from PersonTable P
		
		Set @ErrorMessage ='EmailID not assigned to the mapped workflow user(s)- '+@MissingUserEmail +'.'
		Set @ErrorID=11
		RETURN
	 END 
	END 
	END
		
		
  end  
  go 



  ALTER View [dbo].[nvwAssetRetirement_ForAfterApproval]
as 
select a1.AssetCode,a1.Barcode,a1.CategoryName,a1.LocationName,a1.DepartmentName,a1.SectionDescription,a1.CustodianName,a1.AssetDescription,
  a1.AssetCondition,a1.suppliername,
  
  old.LocationName as OldLocationName,
  a.TransactionID,TransactionNo,TransactionTypeID,TransactionSubTypeName ,
ReferenceNo,CreatedFrom,SourceTransactionID,SourceDocumentNo,a.Remarks,
TransactionDate,TransactionValue,a.StatusID AS TransactionStatusID,PostingStatusID,
VerifiedBy,VerifiedDateTime,PostedBy,PostedDateTime,a.CreatedBy as TransactionCreatedBy,a.CreatedDateTime as TransactionCreatedDatetime,
FORMATMESSAGE( dbo.fn_GetServerURL()+'TransactionApproval/EmailView?id=%d' , a.TransactionID)  ApprovalURL,
--Notification supporting fields
		A.TransactionID SYSDataID1, NULL SYSDataID2, NULL SYSDataID3,
		p.EMailID SYSToAddresses, '' SYSCCAddresses, '' SYSBCCAddresses,
		NULL SYSToMobileNos, NULL SYSWhatsAppMobileNos,b.DisposalValue, b.DisposalReferencesNo,b.DisposalRemarks,b.ProceedOfSales,b.CostOfRemoval,
		b.DisposalDate,RE.RetirementTypeName,NULL as SYSUserID
 From TransactionTable a 
  join TransactionLineItemTable b on a.TransactionID=b.TransactionID 
  join AssetNewView a1 on b.AssetID=a1.AssetID 
  --left join ApprovalHistoryTable AH on a.TransactionID=AH.TransactionID and ah.StatusID=150
  left join LocationTable Old on b.FromLocationID=Old.LocationID
  Left join RetirementTypeTable RE on b.RetirementTypeID=RE.RetirementTypeID
  Left join PersonTable p on a.CreatedBy=p.PersonID and p.EMailID is not null
  Left join TransactionSubTypeTable TST on a.TransactionSubTypeID=TST.TransactionSubTypeID
  where  a.TransactionTypeID=10 --a.StatusID = dbo.fnGetActiveStatusID() and
GO

update MasterGridNewLineItemTable set DisplayName='CommissionDate' where FieldName='ComissionDate'
go
Alter table CountryTable add CountryName nvarchar(max) Not null
go
if not exists(select MasterGridName from MasterGridNewTable where MasterGridName='TransactionList')
Begin
Insert into MasterGridNewTable(MasterGridName,EntityName)
values('TransactionList','TransactionView')
End 
update User_RightTable set ValueType=95 where RightName='TransactionList'
go 
if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='TransactionNo' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='TransactionList')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'TransactionNo','TransactionNo',100,NULL,1,1,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='TransactionList'
	End 
	go  
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='TransactionTypeName' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='TransactionList')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'TransactionTypeName','TransactionTypeName',100,NULL,1,2,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='TransactionList'
	End 
	go   
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='CreatedUSer' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='TransactionList')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'CreatedUSer','CreatedBy',100,NULL,1,3,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='TransactionList'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='CreatedDateTime' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='TransactionList')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'CreatedDateTime','CreatedDateTime',100,'dd/MM/yyyy hh:mm:ss',1,4,'System.DateTime',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='TransactionList'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='Status' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='TransactionList')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'Status','Status',100,NULL,1,5,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='TransactionList'
	End 
	go 

	if not exists(select MasterGridName from MasterGridNewTable where MasterGridName='AssetRetirementIndex')
Begin
Insert into MasterGridNewTable(MasterGridName,EntityName)
values('AssetRetirementIndex','TransactionView')
End 
go 
if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='TransactionNo' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='AssetRetirementIndex')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'TransactionNo','TransactionNo',100,NULL,1,1,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='AssetRetirementIndex'
	End 
	go  
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='CreatedUSer' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='AssetRetirementIndex')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'CreatedUSer','CreatedBy',100,NULL,1,2,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='AssetRetirementIndex'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='CreatedDateTime' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='AssetRetirementIndex')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'CreatedDateTime','CreatedDateTime',100,'dd/MM/yyyy hh:mm:ss',1,3,'System.DateTime',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='AssetRetirementIndex'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='Status' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='AssetRetirementIndex')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'Status','Status',100,NULL,1,4,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='AssetRetirementIndex'
	End 
	go 
if not exists(select MasterGridName from MasterGridNewTable where MasterGridName='AssetTransferIndex')
Begin
Insert into MasterGridNewTable(MasterGridName,EntityName)
values('AssetTransferIndex','TransactionView')
End 
go 
if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='TransactionNo' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='AssetTransferIndex')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'TransactionNo','TransactionNo',100,NULL,1,1,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='AssetTransferIndex'
	End 
	go  
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='CreatedUSer' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='AssetTransferIndex')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'CreatedUSer','CreatedBy',100,NULL,1,2,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='AssetTransferIndex'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='CreatedDateTime' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='AssetTransferIndex')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'CreatedDateTime','CreatedDateTime',100,'dd/MM/yyyy hh:mm:ss',1,3,'System.DateTime',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='AssetTransferIndex'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='Status' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='AssetTransferIndex')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'Status','Status',100,NULL,1,4,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='AssetTransferIndex'
	End 
	go 
	if not exists(select MasterGridName from MasterGridNewTable where MasterGridName='AssetTransferIndex')
Begin
Insert into MasterGridNewTable(MasterGridName,EntityName)
values('AssetTransferIndex','TransactionView')
End 
go 
if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='TransactionNo' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='AssetTransferIndex')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'TransactionNo','TransactionNo',100,NULL,1,1,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='AssetTransferIndex'
	End 
	go  
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='CreatedUSer' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='AssetTransferIndex')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'CreatedUSer','CreatedBy',100,NULL,1,2,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='AssetTransferIndex'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='CreatedDateTime' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='AssetTransferIndex')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'CreatedDateTime','CreatedDateTime',100,'dd/MM/yyyy hh:mm:ss',1,3,'System.DateTime',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='AssetTransferIndex'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='Status' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='AssetTransferIndex')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'Status','Status',100,NULL,1,4,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='AssetTransferIndex'
	End 
	go 
	if not exists(select MasterGridName from MasterGridNewTable where MasterGridName='InternalAssetTransferIndex')
Begin
Insert into MasterGridNewTable(MasterGridName,EntityName)
values('InternalAssetTransferIndex','TransactionView')
End 
go 
if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='TransactionNo' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='InternalAssetTransferIndex')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'TransactionNo','TransactionNo',100,NULL,1,1,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='InternalAssetTransferIndex'
	End 
	go  
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='CreatedUSer' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='InternalAssetTransferIndex')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'CreatedUSer','CreatedBy',100,NULL,1,2,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='InternalAssetTransferIndex'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='CreatedDateTime' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='InternalAssetTransferIndex')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'CreatedDateTime','CreatedDateTime',100,'dd/MM/yyyy hh:mm:ss',1,3,'System.DateTime',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='InternalAssetTransferIndex'
	End 
	go 
	if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='Status' 
		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='InternalAssetTransferIndex')) 
	Begin 
	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
	select 'Status','Status',100,NULL,1,4,'System.String',NULL,MasterGridID
	FRom MasterGridNewTable where MasterGridName='InternalAssetTransferIndex'
	End 
	go 
	 update MasterGridNewLineItemTable set DisplayName='CreatedUser' where DisplayName='CreatedUSer'
	 go 

	 ALTER Procedure  [dbo].[dprc_DashboardCount](
	@LangaugeID int = 1, 
	@UserID int = 1, 
	@CompanyID int = 1003
) 
as 
Begin  
	Declare @UserDepartmentMapping bit,@UserLocationMapping bit, @UserCategoryMapping bit,@AssetApproval bit,@AssetMutipleApproval bit ,@TransferAssetApproval bit
	,@MultipleAssetTransferApproval bit,@LocationEnable bit,@WarrantyNotificationDay int, @TotalAssetCount int ,@ActiveAssetCount int ,@DisposedAssetCount int,
	@warrantyCount int,@statusID int 

	select @statusID=dbo.fnGetActiveStatusID()
	select @UserDepartmentMapping= case when lower(ConfiguarationValue)='false' then 0 else 1 end  from ConfigurationTable where ConfiguarationName='UserDepartmentMapping'
	select @UserLocationMapping= case when lower(ConfiguarationValue)='false' then 0 else 1 end  from ConfigurationTable where ConfiguarationName='UserLocationMapping'
	select @UserCategoryMapping= case when lower(ConfiguarationValue)='false' then 0 else 1 end  from ConfigurationTable where ConfiguarationName='UserCategoryMapping'
	Declare @LocMap Table(LocationID int)
	Declare @CatMap Table(CategoryID int)

	insert into @LocMap(LocationID)
	select LocationID  from ChildLocationMappingTable(@UserID)
	insert into @CatMap(CategoryID)
	select CategoryID  from ChildCategoryMappingTable(@UserID)

	if((select COUNT(*) FROM @LocMap) = 0) SET @UserLocationMapping = 0
	if((select COUNT(*) FROM @CatMap) = 0) SET @UserCategoryMapping = 0

	select @LocationEnable = case when lower(ConfiguarationValue)='false' then 0 else 1 end  from ConfigurationTable where ConfiguarationName='LocationEnable'
	select @AssetApproval = case when lower(ConfiguarationValue)='false' then 0 else 1 end  from ConfigurationTable where ConfiguarationName='AssetApproval'
	select @AssetMutipleApproval= case when lower(ConfiguarationValue)='false' then 0 else 1 end  from ConfigurationTable where ConfiguarationName='AssetApprovalBasedOnWorkFlow'
	select @TransferAssetApproval= case when lower(ConfiguarationValue)='false' then 0 else 1 end  from ConfigurationTable where ConfiguarationName='TransferAssetApproval'
	select @MultipleAssetTransferApproval= case when lower(ConfiguarationValue)='false' then 0 else 1 end  from ConfigurationTable where ConfiguarationName='TransferAssetApprovalBasedOnWorkFlow'
	select @WarrantyNotificationDay= 
		case 
			when ConfiguarationValue='AssetWarrantyNotificationBefore30days' then 30 
			when ConfiguarationValue='AssetWarrantyNotificationBefore60days' then 60 
			when ConfiguarationValue='AssetWarrantyNotificationBefore90days' then 90 
			else 0 
			end    
		from ConfigurationTable where ConfiguarationName='WarrantyAlertNotificationDays'
		--select @UserCategoryMapping,@UserLocationMapping,@UserDepartmentMapping
		--select * from AssetTable where categoryId not in (select * from @CatMap)
		--select * from AssetTable where locationid not in (select * from @LocMap)
	Select  @TotalAssetCount=SUM(CASE WHEN statusID not in (3,500) THEN 1 ELSE 0 END) ,
			@ActiveAssetCount=SUM(CASE WHEN statusID not in (3,4,500,250) THEN 1 ELSE 0 END) 
		from AssetTable A
		LEFT JOIN @CatMap CM ON CM.CategoryID = A.CategoryID
		LEFT JOIN @LocMap LM ON LM.LocationID = A.LocationID
		where 
		--(@UserCategoryMapping=0 or CM.CategoryID IS NOT NULL /*CategoryID in (select CategoryID from #CatMap )*/) 
		--and
		(@LocationEnable = 1 OR  @UserLocationMapping=0  or LM.LocationID IS NOT NULL /*@UserLocationMapping = 0 or LocationID in (select LocationID from #LocMap)*/ )
		and (@UserDepartmentMapping=0 or DepartmentID in (Select DepartmentID from UserDepartmentMappingTable where PersonID=@UserID and StatusID=@statusID))
		----and (@AssetApproval=0 or AssetID  in (select ObjectkeyValue from ApprovalTable where StatusID=5 and ActionType in (2,3,4)))
		 and CompanyID=@CompanyID --and statusID!=3 

	--Select @ActiveAssetCount=COUNT(AssetID) 
	--	from AssetTable 
	--	where (@UserCategoryMapping=0 or CategoryID in (select CategoryID from #CatMap )) 
	--	and (@LocationEnable=1 and @UserLocationMapping=0 or LocationID in (select locationid from #LocMap))
	--	and (@UserDepartmentMapping=0 or DepartmentID in (Select DepartmentID from UserDepartmentMappingTable where PersonID=@UserID))
	--	and (@AssetApproval=0 or AssetID  not in (select ObjectkeyValue from ApprovalTable where StatusID=5 and ActionType in (1)))
	--	and CompanyID=@CompanyID and statusID not in (3,4)

	Select @DisposedAssetCount=COUNT(AssetID) 
		from AssetTable 
		where (@UserCategoryMapping=0 or CategoryID in (select CategoryID from @CatMap )) 
		and (@LocationEnable=1 and @UserLocationMapping=0 or LocationID in (select locationid from @LocMap))
		and (@UserDepartmentMapping=0 or DepartmentID in (Select DepartmentID from UserDepartmentMappingTable where PersonID=@UserID and StatusID=@statusID))
		and CompanyID=@CompanyID and statusID in (4,250)

	Select @warrantyCount=COUNT(AssetID) 
		from AssetTable 
		where (@UserCategoryMapping=0 or CategoryID in (select CategoryID from @CatMap )) 
		and (@LocationEnable=1 and @UserLocationMapping=0 or LocationID in (select locationid from @LocMap))
		and (@UserDepartmentMapping=0 or DepartmentID in (Select DepartmentID from UserDepartmentMappingTable where PersonID=@UserID and StatusID=@statusID))
		and (@AssetApproval=0 or AssetID  not in (select ObjectkeyValue from ApprovalTable where StatusID=5 and ActionType in (1)))
		and CompanyID=@CompanyID and statusID not in (3,4,500,250) 
		and (WarrantyExpiryDate>=DATEADD(day,-1,getdate()) and WarrantyExpiryDate<=DATEADD(day,@WarrantyNotificationDay+1,getdate()))
	
	Select isnull(@TotalAssetCount,0) as TotalAssetCount,isnull(@ActiveAssetCount,0) as ActiveAssetCount,@DisposedAssetCount as DisposedAssetCount,@warrantyCount as AssetWarrantyExpiryCount

End 
go 




ALTER Procedure [dbo].[prc_ValidateForTransaction]
(
   @FromAssetID				nvarchar(max)	 = NULL,
   @ToLocationID			int				 = NULL,
   @ModuleID				int              = NULL,
   @RightName				nvarchar(100)	 = NULL,
   @ErrorID					int OutPut,
   @ErrorMessage			nvarchar(max) OutPut
)
as 
Begin
	DECLARE @ASSETTRANSFER INT = 5,
			@ASSETRETIREMENT INT = 10,
			@INTERNALASSETTRANSFER INT = 11, 
			@ASSETADDITION INT = 4,
			@ASSETMAINTENANCE INT = 15,
			@ASSETMAINTENANCEREQUEST INT = 16

	Declare @UpdateCount int,@ApprovalCnt int ,@statusID int,@CategoryTypeCnt int --@modelID int ,
		,@CategoryType nvarchar(max),@ID int,@FromLocationtypeID int, @ToLocationTypeID int,@ApprovalWorkflowID int ,@HQID int,@MappedToLocationID int

	select @statusID=[dbo].fnGetActiveStatusID()
	--select * from LocationTypeTable

	select @HQID=locationtypeid from LocationTypeTable where LocationTypeName='Head Quarters' and StatusID=@statusID

	Set @ErrorID=0
	Set @ErrorMessage=null

	declare @TransactionLineItemTable table (AssetID int,FromLocationL2 int ,ToLocationL2 int,
			LocationType nvarchar(100),CategoryType nvarchar(100),ApprovalWorkFlowID int,Barcode nvarchar(max),
			MappedCategryName nvarchar(max) )
	
	insert into @TransactionLineItemTable(AssetID,FromLocationL2,ToLocationL2,LocationType,CategoryType,ApprovalWorkFlowID,Barcode,MappedCategryName)
	select AssetID,LocationL2,@ToLocationID,LocationType,categorytype,null,Barcode,B.CategoryName 
	From AssetNewView a join CategoryTable b on a.CategoryID=b.CategoryID where assetid in (select value from Split(@fromAssetID,',')) 

	IF(@ModuleID = @INTERNALASSETTRANSFER)
	BEGIN
		select @MappedToLocationID =MappedLocationID from LocationNewView where LocationID=@ToLocationID
		update @TransactionLineItemTable set ToLocationL2=@MappedToLocationID
		
		If @ToLocationID is null or @ToLocationID=''
		Begin 
		Set @ErrorID=15
		Set @ErrorMessage='Selected Loction Code is not Preferred Location Level.'
		return
		end 

		If not exists(select AssetID From @TransactionLineItemTable where FromLocationL2=ToLocationL2)
		Begin
		Set @ErrorID=14
		Set @ErrorMessage='Selected From Location and To Location should be Same.'
		return
		End
		Else 
		Begin 
		Select @ToLocationTypeID=LocationTypeID from LocationTable where LocationID=@MappedToLocationID
			if @HQID!=@ToLocationTypeID
			Begin 
			  If exists(select LocationID from LocationTable where ParentLocationID=@ToLocationID)
			  Begin 
				Set @ErrorID=16
				Set @ErrorMessage='Non HQ Location should be selected end level.'
				return
			  End 
			End
		
		end 
		--Validate HQ, only requires 2nd level locations
		--NON HQ validate end level should be selected
		--Validate from and to parent location should be same
		--Set @ErrorID = 0
		--Set @ErrorMessage=null
		--return

	END
	ELSE
	BEGIN
		if not exists(select mappedLocationID from LocationNewView where MappedLocationID=@ToLocationID )
		Begin 
			Declare @LocationCode nvarchar(max)
			Select @LocationCode=LocationCode from LocationTable where LocationID=@ToLocationID

			Set @ErrorID=1
			Set @ErrorMessage=@LocationCode+'- Selected Loction Code is not Preferred Location Level.'
			return
		end 
	END

	
	--select * from @TransactionLineItemTable
	Select @CategoryTypeCnt=count(CategoryType) from @TransactionLineItemTable group by LocationType
	if(@CategoryTypeCnt>1)
	begin
		if not exists(select CategoryTypeName from CategoryTypeTable where IsAllCategoryType=1 and statusID=@statusID)
		Begin
		Set @ErrorID=2
		Set @ErrorMessage='Selected Assets have both IT and NON-IT so CategoryType-All Should be created.'
		return
		End 
		Select @CategoryType=CategoryTypeName from CategoryTypeTable where IsAllCategoryType=1 and statusID=@statusID
		update @TransactionLineItemTable set CategoryType=@CategoryType
	End 
	Else 
	Begin 
		Select @CategoryType=CategoryType from @TransactionLineItemTable group by CategoryType
	end 
	select * from @TransactionLineItemTable
	--select count(FromLocationL2) from @TransactionLineItemTable group by FromLocationL2
	if(select count(FromLocationL2) from (select FromLocationL2 from @TransactionLineItemTable group by FromLocationL2)x)>1
	begin 
		Set @ErrorID=3
		Set @ErrorMessage='Selected Asset have different Mapped Location please select same second level location .'
		return
	End
	if(select count(LocationType) from (select LocationType from @TransactionLineItemTable group by LocationType)x)>1
	begin 
		Set @ErrorID=4
		Set @ErrorMessage='Selected Asset have different location type please select same location type.'
		return
	End
	IF exists(select LocationType from @TransactionLineItemTable where LocationType is null )
	Begin 
	Set @ErrorID=13
		Set @ErrorMessage='Selected Asset must be map with location type.'
		return
	End 
	--if @moduleID=11 
	--begin
	--	select @ToLocationID =MappedLocationID from LocationNewView where LocationID=@ToLocationID
	--	update @TransactionLineItemTable set ToLocationL2=@ToLocationID
	--end

	Select @FromLocationtypeID=LocationTypeID from LocationTypeTable 
		where LocationTypeName=(select top 1 LocationType from @TransactionLineItemTable)

	Select @ToLocationTypeID=LocationTypeID from LocationTable where LocationID = (select top 1 ToLocationL2 from @TransactionLineItemTable)
	print @ToLocationID
	IF @ToLocationTypeID is null or @ToLocationTypeID=''
	Begin 
	Set @ErrorID=14
		Set @ErrorMessage='Selected To Location must be map with location type.'
		return
	End 
	Declare @CheckVal bit 
	set @CheckVal=0

	IF @HQID=@FromLocationtypeID and @HQID=@ToLocationTypeID
	begin
	set @CheckVal=1
	end 

	if @moduleID=11 and(@CheckVal=0)
	begin
		Set @ErrorID=0
		set @ErrorMessage=null
	    return
	End 
	print @CheckVal
	if not exists(select b.ApprovalRoleID from ApproveWorkflowTable a 
				  join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
				  where a.StatusID=@statusID and b.StatusID=@statusID and 
				  a.ApproveModuleID=(select ApproveModuleID from ApproveModuleTable where ApproveModuleID=@moduleID and StatusID=@statusID)
				  and a.FromLocationTypeID=@FromLocationtypeID and 
				  case when @moduleID=5 or @moduleID=11 then a.ToLocationTypeID else NULL end=@ToLocationTypeID)
			Begin 
				Declare @FromLocationType nvarchar(100),@ToLocationType nvarchar(100)
				Select top 1 @FromLocationType=Locationtype from @TransactionLineItemTable 
				Select @ToLocationType=LocationType From LocationNewView where LocationID=@ToLocationID
				if @moduleID=5 or @moduleID=11 
				Begin 
				Set @ErrorMessage ='Workflow not defined for From LocationType-'+@FromLocationType+' and To Location Type - '+ @ToLocationType+'.'
				End 
				Else 
				Begin 
				Set @ErrorMessage ='Workflow not defined for From LocationType-'+@FromLocationType+'.'
				End 
				Set @ErrorID=5
				return
			End 
	select @ApprovalWorkflowID=a.ApproveWorkflowID from ApproveWorkflowTable a 
				  join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
				  where a.StatusID=@statusID and b.StatusID=@statusID and 
				  a.ApproveModuleID=(select ApproveModuleID from ApproveModuleTable where ApproveModuleID=@moduleID and StatusID=@statusID)
				  and a.FromLocationTypeID=@FromLocationtypeID and 
				  case when @moduleID=5 or @moduleID=11 then a.ToLocationTypeID else NULL end=@ToLocationTypeID
    
	update @TransactionLineItemTable set ApprovalWorkFlowID=@ApprovalWorkflowID
	 
	SElect @moduleID=ApproveModuleID from ApproveWorkflowTable where ApproveWorkflowID=@ApprovalWorkFlowID
	
	Declare @updateTable table(updateRole bit,ApprovalRoleID int,moduleName nvarchar(100),FromLocationType nvarchar(100),ToLocationType nvarchar(100),ModuleID int)
	  
	
	if @moduleID=5 
	Begin
	   if exists (select top 1 assetID from @TransactionLineItemTable where FromLocationL2=ToLocationL2)
	   begin 
	     Set @ErrorID=12
		Set @ErrorMessage='From and To Location are the Same , Please select different Location.'
		return
	   End 
	end 

	Select @ApprovalCnt=count(OrderNo) from 
	ApproveWorkflowTable a join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
	where a.StatusID=@statusID and b.StatusID=@statusID and a.ApproveWorkflowID=@ApprovalWorkFlowID

	if not exists(select * from @TransactionLineItemTable where CategoryType is not null and CategoryType!='') 
	begin 
		Declare @MissingCategoryName nvarchar(max)
		Select @MissingCategoryName=stuff((Select ',' + T.MappedCategryName from @TransactionLineItemTable T 
							 where T.CategoryType is null and T.AssetID=a.AssetID FOR XML PATH('')), 1, 1, '') from @TransactionLineItemTable a 
		Set @ErrorID=6
		set @ErrorMessage=@MissingCategoryName+' Category Type(s) are Missed.'
		return
	End 

	insert into @updateTable(updateRole,ApprovalRoleID,moduleName,FromLocationType,ToLocationType,ModuleID)
		Select case when a.ApproveModuleID=5 or a.ApproveModuleID=11 then UpdateDestinationLocationsForTransfer 
				when  a.ApproveModuleID=10 then UpdateRetirementDetailsForEachAssets else cast(0 as bit) end as updateRole, 
		b.ApprovalRoleID,d.ModuleName,FL.LocationTypeName,TL.LocationTypeName,a.ApproveWorkflowID FRom ApproveWorkflowTable a 
		join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
		join ApproveModuleTable D on a.ApproveModuleID=d.ApproveModuleID
		join ApprovalRoleTable C on b.ApprovalRoleID=C.ApprovalRoleID
		Join LocationTypeTable FL on a.FromLocationTypeID=FL.LocationTypeID
		Left join LocationTypeTable TL on a.ToLocationTypeID=TL.LocationTypeID
		where a.ApproveWorkflowID=@ApprovalWorkFlowID and a.StatusID=@statusID and b.StatusID=@statusID and c.StatusID=@statusID

	Declare @MissedModuleName nvarchar(100),@MissedFromLocationtype nvarchar(100),@MissedToLocationtype nvarchar(100),@MissedModuleID int,@errorMsg nvarchar(max)
	Select top 1 @MissedModuleName=moduleName,@MissedFromLocationtype=FromLocationType,@MissedToLocationtype=ToLocationType,@MissedModuleID=ModuleID from @updateTable
	If(@moduleID=5 or @moduleID=11 or @moduleID=10)
	Begin
		if not exists(select * from @updateTable where updateRole=1)
		Begin 
			Set @ErrorID=7
				If @MissedModuleID=5 or @MissedModuleID=11
				Begin
				Set @errorMsg=@MissingCategoryName+' Update option not provided for modulename : '+@MissedModuleName+',From Location Type :'+@MissedFromLocationtype+',To Location type : '+@MissedToLocationtype+'.'
				End 
				Else 
				Begin
				Set @errorMsg=@MissingCategoryName+' Update option not provided for modulename : '+@MissedModuleName+',From Location Type :'+@MissedFromLocationtype+'.'
				End
			set @ErrorMessage=@errorMsg
			return
		End 
		if (select updateRole from @updateTable group by updateRole having count(updateRole)>1)>1
		Begin 
			Set @ErrorID=8
				If @MissedModuleID=5 or @MissedModuleID=11
				Begin
				Set @errorMsg=@MissingCategoryName+' Update option shouldnot be enabled for more than one, modulename : '+@MissedModuleName+',From Location Type :'+@MissedFromLocationtype+',To Location type : '+@MissedToLocationtype+'.'
				End 
				Else 
				Begin
				Set @errorMsg=@MissingCategoryName+' Update option shouldnot be enabled for more than one, modulename : '+@MissedModuleName+',From Location Type :'+@MissedFromLocationtype+'.'
				End
			set @ErrorMessage=@errorMsg
			return
		End 
	End 
	 Declare @validationTable Table(UserID int,LocationID int,ApprovalRoleID int,CategoryTypeCnt int ) 
	 Declare @validationTable1 Table(UserID int,LocationID int,ApprovalRoleID int,CategoryTypeCnt int ) 
	 Declare @validationTable2 Table(UserID int,LocationID int,ApprovalRoleID int,CategoryTypeCnt int ) 

	insert into @validationTable(UserID,LocationID,ApprovalRoleID,CategoryTypeCnt)
		select userID,LocationID,a.ApprovalRoleID,count(d.categoryTypeID) as categorytypeCnt 
		from ApproveWorkflowLineItemTable a 
		join @TransactionLineItemTable T on a.ApproveWorkFlowID=T.ApprovalWorkFlowID
		join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
		join CategoryTypeTable CT on T.CategoryType=Ct.categoryTypeName
		join 
		(
			select a1.*,b1.IsAllCategoryType from UserApprovalRoleMappingTable a1  
			join CategoryTypeTable b1 on a1.CategoryTypeID=b1.CategoryTypeID
			where a1.StatusID=@statusID and b1.StatusID=@statusID
		) D on  case when b.ApprovalLocationTypeID=5 then 
		T.FromLocationL2 when b.ApprovalLocationTypeID=10 then T.ToLocationL2 else T.FromLocationL2 end =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID 
		and ct.CategoryTypeID = (case when CT.IsAllCategoryType=0 and D.IsAllCategoryType=1 then CT.CategoryTypeID else D.CategoryTypeID end)
			where ApproveWorkFlowID=@ApprovalWorkFlowID and CT.statusID=@statusID and a.StatusID=@statusID
			and b.StatusID=@statusID and D.StatusID=@statusID
			group by userID,LocationID,a.ApprovalRoleID

	if(select count(*) from (
			select  a.ApprovalRoleID  from @validationTable a group by a.ApprovalRoleID)x) !=@ApprovalCnt
	Begin 
		Declare @MissingApprovalRoleName nvarchar(max),@ModuleName nvarchar(max)
		
		Select @MissingApprovalRoleName=stuff((Select ',' + b.ApprovalRoleName From ApproveWorkflowLineItemTable a Join ApprovalRoleTable b on a.ApprovalRoleID=b.ApprovalRoleID
		where ApproveWorkFlowID=@ApprovalWorkFlowID and a.StatusID=@statusID and b.StatusID=@statusID and a.ApprovalRoleID not in(select ApprovalRoleID from @validationTable)
		and b.ApprovalRoleID=p.ApprovalRoleID FOR XML PATH('')), 1, 1, '') from ApprovalRoleTable P
		
		Select @ModuleName=b.ModuleName from ApproveWorkflowTable a 
			join ApproveModuleTable b on a.ApproveModuleID=b.ApproveModuleID 
				where ApproveWorkFlowID=@ApprovalWorkFlowID and a.StatusID=@statusID
		 
		Set @ErrorMessage ='Missing user(s) for the role : '+@MissingApprovalRoleName+' , ModuleName :'+@ModuleName+'.'
		Set @ErrorID=9
		return
	end 
    Else 
	Begin 
	insert into @validationTable1(UserID,LocationID,ApprovalRoleID,CategoryTypeCnt)
		select a.userID,a.LocationID,a.ApprovalRoleID,categorytypeCnt
		From @validationTable a  
		join [ValidateAccessRightsView]  b  on a.UserID=b.UserID --and a.LocationID=b.LocationID and a.ApprovalRoleID=b.ApprovalRoleID
		and b.RightName=@RightName
	if(select count(*) from (
		select  a.ApprovalRoleID  from @validationTable1 a group by a.ApprovalRoleID)x )!=@ApprovalCnt
	Begin 
		Set @ErrorID=10
		Declare @MissingUserName nvarchar(max)

		Select @MissingUserName=stuff((Select ',' + PersonCode from @validationTable a join PersonTable b on a.UserID=b.PersonID 
		where userID not in (select userID from @validationTable1) and b.personID=p.PersonID FOR XML PATH('')), 1, 1, '') from PersonTable P
		
		Set @ErrorMessage ='Approval Rights not provided for the user(s)- '+@MissingUserName +'.'
		return
	END
	if (select count(*) from (
		select  a.ApprovalRoleID  from @validationTable1 a group by a.ApprovalRoleID)x  )=@ApprovalCnt
	begin 
		insert into @validationTable2(UserID,LocationID,ApprovalRoleID,CategoryTypeCnt)
		select a.userID,a.LocationID,a.ApprovalRoleID,categorytypeCnt
		From @validationTable a  
		join persontable p on a.userID=p.personID and p.EMailID is not null and p.StatusID=@statusID
 if(select count(*) from (
	select  a.ApprovalRoleID  from @validationTable2 a group by a.ApprovalRoleID)x  )!=@ApprovalCnt
	 Begin 
		Declare @MissingUserEmail nvarchar(max)

		Select @MissingUserEmail=stuff((Select ',' + PersonCode from @validationTable a join PersonTable b on a.UserID=b.PersonID 
			where userID not in (select userID from @validationTable2) and b.personID=p.PersonID FOR XML PATH('')), 1, 1, '') from PersonTable P
		
		Set @ErrorMessage ='EmailID not assigned to the mapped workflow user(s)- '+@MissingUserEmail +'.'
		Set @ErrorID=11
		RETURN
	 END 
	END 
	END
		
		
  end  
  go 
  ALTER Procedure [dbo].[Prc_ValidateBulkAssetAddition] 
(
  @LocationCode   nvarchar(max) =NULL,
  @CategoryCode   nvarchar(max) = null,
  @ErrorID        int OutPut,
  @ErrorMessage   nvarchar(max) OutPut
)
as 
Begin 
    Declare @L2LocID int, @LocTypeID int,@statusID int,@ApproveworkflowID int,@categoryTypeID int,@L2catID int 
   ,@workflowCnt int ,@approvalCnt int,@CategoryCount int,@LocationCount int ,@CategoryTypeCnt int ,@LocationTypeName nvarchar(100)
   Select @statusID=[dbo].fnGetActiveStatusID()
   
   Declare @LocationIDTable Table(LocationCode nvarchar(300),LocationID int,L2LocationID int,LocationTypeID int )
   Declare @CategoryIDTable Table(CategoryCode nvarchar(300),CategoryID int ,CategoryTypeID int)
   Declare @CategoryTypeTable Table (CategoryTypeID int)

   
   Insert into @CategoryIDTable(CategoryCode)
   select value from Split(@CategoryCode,',')
   group by value
  
   insert into @LocationIDTable(LocationCode)
   select value from Split(@LocationCode,',')
   group by value

   Update a 
	   set a.CategoryID=b.CategoryID
	   From @CategoryIDTable a 
	   join CategoryTable b on a.CategoryCode=b.CategoryCode
	   Where b.StatusID=@statusID

	Update a 
	   set a.LocationID=b.LocationID
	   From @LocationIDTable a 
	   join LocationTable b on a.LocationCode=b.LocationCode
	   Where b.StatusID=@statusID

	Set @ErrorMessage=null
	Set @ErrorID=0

   if exists (select CategoryID from @CategoryIDTable where CategoryID is null)
	Begin
	   Declare @MissingCategoryCode nvarchar(max)
	   Select @MissingCategoryCode=stuff((Select ',' + T.Categorycode from @CategoryIDTable T 
	                     where T.CategoryID is null FOR XML PATH('')), 1, 1, '')

	    set @ErrorMessage =@MissingCategoryCode +' Category code(s) are Invalid.'
		Set @ErrorID=1
		return 
	End 

	if exists (select LocationID from @LocationIDTable where LocationID is null)
	Begin
		Declare @MissingLocationCode nvarchar(max)
		Select @MissingLocationCode=stuff((Select ',' + T.LocationCode from @LocationIDTable T 
	                     where T.LocationID is null  FOR XML PATH('')), 1, 1, '') 
		Set @ErrorMessage =@MissingLocationCode + ' Location code(s) are Invalid.'
		Set @ErrorID=2
		return
	End 

	Update a 
	   set a.CategoryTypeID=b.CategoryTypeID
	   From @CategoryIDTable a 
	   join CategoryNewHierarchicalView b on a.CategoryID=b.CategoryID
	   Where b.StatusID=@statusID
	
	Update a 
	   set a.LocationTypeID=b.LocationTypeID,
	   a.L2LocationID=b.MappedLocationID
	   From @LocationIDTable a 
	   join LocationNewHierarchicalView b on a.LocationID=b.locationid
	   Where b.StatusID=@statusID

		-- select * from @LocationIDTable
	if exists (Select CategoryTypeID from @CategoryIDTable where CategoryTypeID is null) 
	Begin
		Declare @MissingCategoryType nvarchar(max)
		Select @MissingCategoryType=stuff((Select ',' + T.CategoryCode from @CategoryIDTable T 
	                     where T.CategoryTypeID is null and T.CategoryID=a.CategoryID FOR XML PATH('')), 1, 1, '') from @CategoryIDTable a 
		Set @ErrorMessage =@MissingCategoryType + +' Category Type(s) are Missed.'
		Set @ErrorID=3
		return
	End 
   if exists (select LocationTypeID from @LocationIDTable where LocationTypeID is null )
	Begin 
		Declare @MissingLocationType nvarchar(max)
		Select @MissingLocationType=stuff((Select ',' +T.LocationCode from @LocationIDTable T 
	                     where T.LocationTypeID is null and T.LocationID=a.LocationID FOR XML PATH('')), 1, 1, '') from @LocationIDTable a 
		Set @ErrorMessage =@MissingLocationType + ' - Location Type(s) are Missed.'
		Set @ErrorID=8
		return
	End 
	if(select count(L2LocationID) from @LocationIDTable group by L2LocationID )>1
	Begin 
		--Declare @DifferentLocCode nvarchar(max)
		--Select @DifferentLocCode=stuff((Select T.LocationCode from @c T 
	 --                    where T.CategoryTypeID is null and T.CategoryID=a.CategoryID FOR XML PATH('')), 1, 1, '') from @CategoryIDTable a
		Set @ErrorMessage ='Location Mismatch: Same Second level Location only allowed here.'
		Set @ErrorID=3
		return
	End 
	Else 
	Begin 

	   select @L2LocID=L2LocationID from @LocationIDTable group by L2LocationID
	   select @LocTypeID=LocationTypeID from @LocationIDTable where L2LocationID=@L2LocID
	   Select @LocationTypeName=LocationTypeName from LocationTypeTable where LocationTypeID=@LocTypeID
	  
	   --print @L2LocID
	   --print @LocTypeID
	   --print @LocationTypeName
	   if(Select Count(CategoryTypeID) from @CategoryIDTable group by CategoryTypeID)>1
			  begin
				Select @categoryTypeID=CategoryTypeID from CategoryTypeTable where IsAllCategoryType=1 and statusID=@statusID
			  End 
			  Else 
			  Begin 
			  Select @categoryTypeID=CategoryTypeID from @CategoryIDTable group by CategoryTypeID
			  end 
	   end 

	if not exists( select b.ApprovalRoleID from ApproveWorkflowTable a 
					join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
						where a.StatusID=@statusID and b.StatusID=@statusID and 
						a.ApproveModuleID=(select ApproveModuleID from ApproveModuleTable where ModuleName='AssetAddition' and StatusID=@statusID)
						and a.FromLocationTypeID=@LocTypeID)
	begin 
	print 'welcome'
		Set @ErrorMessage ='Workflow not defined for LocationType-'+@LocationTypeName+'.'
		Set @ErrorID=4
		return
	end 
	Else 
	Begin 
	select @ApproveworkflowID=b.ApproveWorkFlowID from ApproveWorkflowTable a 
		join ApproveWorkflowLineItemTable b on a.ApproveWorkflowID=b.ApproveWorkFlowID
		where a.StatusID=@statusID and b.StatusID=@statusID and 
		a.ApproveModuleID=(select ApproveModuleID from ApproveModuleTable where ModuleName='AssetAddition' and StatusID=@statusID)
		and a.FromLocationTypeID=@LocTypeID

	declare @TransactionLineItemTable table (FromLocationL2 int ,LocationTypeID int,CategoryTypeID int,ApprovalWorkFlowID int )
	insert into @TransactionLineItemTable(FromLocationL2,LocationTypeID,CategoryTypeID,ApprovalWorkFlowID)
		values(@L2LocID,@LocTypeID,@categoryTypeID,@ApproveworkflowID)

	Declare @validationTable Table(UserID int,LocationID int,ApprovalRoleID int,CategoryTypeCnt int ) 

	Select @workflowCnt=Count(*) from ApproveWorkflowLineItemTable where ApproveWorkFlowID=@ApproveworkflowID and StatusID=@statusID

	insert into @validationTable(UserID,LocationID,ApprovalRoleID,CategoryTypeCnt)
	select userID,LocationID,a.ApprovalRoleID,count(d.categoryTypeID) as categorytypeCnt 
		from ApproveWorkflowLineItemTable a 
		join @TransactionLineItemTable T on a.ApproveWorkFlowID=T.ApprovalWorkFlowID
		join ApprovalRoleTable  b on a.ApprovalRoleID=b.ApprovalRoleID
		join CategoryTypeTable CT on T.CategoryTypeID=Ct.CategoryTypeID
		join 
		(
		select a1.*,b1.IsAllCategoryType from UserApprovalRoleMappingTable a1  
		join CategoryTypeTable b1 on a1.CategoryTypeID=b1.CategoryTypeID
		where a1.StatusID=@statusID and b1.StatusID=@statusID
		) D on  T.FromLocationL2 =D.LocationID and D.ApprovalRoleID=b.ApprovalRoleID 
		and ct.CategoryTypeID = (case when CT.IsAllCategoryType=0 and D.IsAllCategoryType=1 then CT.CategoryTypeID else D.CategoryTypeID end)
		where ApproveWorkFlowID=@ApproveworkflowID and CT.statusID=@statusID AND d.StatusID=@statusID and a.statusID=@statusID
		group by userID,LocationID,a.ApprovalRoleID

	select @approvalCnt= count(*) from (
	select  a.ApprovalRoleID  from @validationTable a group by a.ApprovalRoleID) x
	if(isnull((@workflowCnt),0)=isnull(@approvalCnt,0))
	begin 
		Declare @validationTable1 Table(UserID int,LocationID int,ApprovalRoleID int,CategoryTypeCnt int ) 
		Declare @validationTable2 Table(UserID int,LocationID int,ApprovalRoleID int,CategoryTypeCnt int ) 
					
		insert into @validationTable1(UserID,LocationID,ApprovalRoleID,CategoryTypeCnt)
		select a.userID,a.LocationID,a.ApprovalRoleID,categorytypeCnt
			From @validationTable a  
			join [ValidateAccessRightsView] b  on a.UserID=b.UserID --and a.LocationID=b.LocationID and a.ApprovalRoleID=b.ApprovalRoleID
			and b.RightName='AssetApproval'

	if(select count(*) from (
	select  a.ApprovalRoleID  from @validationTable1 a group by a.ApprovalRoleID)x )!=@approvalCnt
	Begin 
		Declare @MissingUserName nvarchar(max)
		Select @MissingUserName=stuff((Select ',' + PersonCode from @validationTable a join PersonTable b on a.UserID=b.PersonID 
		where userID not in (select userID from @validationTable1) and b.personID=p.PersonID FOR XML PATH('')), 1, 1, '') from PersonTable P
		
		Set @ErrorMessage ='Approval Rights not provided for the user(s)- '+@MissingUserName +'.'
		Set @ErrorID=5
		return
	end 
	if (select count(*) from (
	select  a.ApprovalRoleID  from @validationTable1 a group by a.ApprovalRoleID)x  )=@approvalCnt
	begin 
		insert into @validationTable2(UserID,LocationID,ApprovalRoleID,CategoryTypeCnt)
		select a.userID,a.LocationID,a.ApprovalRoleID,categorytypeCnt
		From @validationTable a  
		join persontable p on a.userID=p.personID and p.EMailID is not null and p.StatusID=@statusID

			if(select count(*) from (
			select  a.ApprovalRoleID  from @validationTable2 a group by a.ApprovalRoleID)x  )!=@approvalCnt
			 Begin 
				Declare @MissingUserEmail nvarchar(max)
				Select @MissingUserEmail=stuff((Select ',' + PersonCode from @validationTable a join PersonTable b on a.UserID=b.PersonID 
					where userID not in (select userID from @validationTable2) and b.personID=p.PersonID FOR XML PATH('')), 1, 1, '') from PersonTable P

				Set @ErrorMessage ='EmailID not assigned to the mapped workflow user(s)- '+@MissingUserEmail +'.'
				Set @ErrorID=6

				Return
			end 
		End 

	End 
	Else 
		Begin 
		Declare @MissingApprovalRoleName nvarchar(max),@ModuleName nvarchar(max)
		Select @MissingApprovalRoleName=stuff((Select ',' + b.ApprovalRoleName From ApproveWorkflowLineItemTable a Join ApprovalRoleTable b on a.ApprovalRoleID=b.ApprovalRoleID
			where ApproveWorkFlowID=@ApproveworkflowID and a.StatusID=@statusID and b.StatusID=@statusID and a.ApprovalRoleID not in(select ApprovalRoleID from @validationTable)
				and b.ApprovalRoleID=p.ApprovalRoleID FOR XML PATH('')), 1, 1, '') from ApprovalRoleTable P
		Select @ModuleName=b.ModuleName from ApproveWorkflowTable a join ApproveModuleTable b on a.ApproveModuleID=b.ApproveModuleID where ApproveWorkFlowID=@ApproveworkflowID and a.StatusID=@statusID
		 
		Set @ErrorMessage ='Missing user(s) for the role : '+@MissingApprovalRoleName+' , ModuleName :'+@ModuleName+', LocationType : '+@LocationTypeName+'.'
		Set @ErrorID=7
		End 
	End 

End 

go 

ALTER View [dbo].[ApprovalHistoryView] 
  as 
  Select b.ApprovalRoleName,case when c.StatusID = dbo.fnGetActiveStatusID() then 'Approved' else c.Status end as ApprovalStatus,p.PersonFirstName+'-'+p.PersonLastName as ApprovedBy ,a.LastModifiedDateTime as ApprovedDatetime ,a.*
  ,Ltrim([FileName]) as DocumentName,
  Case when p.PersonID is not null then  t3.personName else t3.personName end as UserName
 From ApprovalHistoryTable a 

  join ApprovalRoleTable b on a.ApprovalRoleID=b.ApprovalRoleID
  left join StatusTable c on a.StatusID=c.StatusID
  left join PersonTable p on a.LastModifiedBy=p.PersonID
  left join CategoryTypeTable CT on a.CategoryTypeID=ct.categorytypeID
  left join (SELECT a.ObjectKeyID,
   STUFF((SELECT '; ' + US.DocumentName 
          FROM DocumentTable US
          WHERE US.ObjectKeyID = a.ObjectKeyID and us.TransactionType  like '%Approval%'
          ORDER BY Filename
          FOR XML PATH('')), 1, 1, '') [FileName]
		FROM DocumentTable a join ApprovalHistoryTable b on a.ObjectKeyID=b.ApprovalHistoryID and a.TransactionType like '%Approval%' 
		GROUP BY a.ObjectKeyID) approvaldoc on a.ApprovalHistoryID=approvaldoc.ObjectkeyID 
  Left join 
      (
	  select t2.approvalRoleID,t2.LocationID,t2.TransactionID,t2.ApprovemoduleID, STUFF((select ','+PersonName 
			from ApprovalHistoryMappedUser t1 
			where  t1.transactionid=t2.transactionid and t1.approvemoduleID=t2.approvemoduleID and t1.approvalRoleID=t2.approvalRoleID and t1.LocationID=t2.LocationID
			 order by personName   FOR XML PATH('')), 1, 1, '') personName
		 from ApprovalHistoryMappedUser t2  group by t2.approvalRoleID,t2.LocationID,t2.transactionid,t2.approvemoduleID
	   )t3 on a.TransactionID=t3.TransactionID and a.ApproveModuleID=t3.ApproveModuleID and a.approvalRoleID=t3.approvalRoleID 
	   and  case when b.ApprovalLocationTypeID=5 then a.FromLocationID else a.ToLocationID end =t3.LocationID 

--Left join (
--select t2.approvalRoleID,t2.LocationID,t2.CategoryTypeID, STUFF((select ','+PersonName 
--from UserApprovalRoleMappingView t1
--where t1.approvalRoleID=t2.approvalRoleID and t1.LocationID=t2.LocationID and t1.CategorytypeID=t2.CategorytypeID
--order by personName 
-- FOR XML PATH('')), 1, 1, '') personName
-- from UserApprovalRoleMappingView t2  group by t2.approvalRoleID,t2.LocationID,t2.CategorytypeID
--) t3 on b.ApprovalRoleID=t3.ApprovalRoleID and case when b.ApprovalLocationTypeID=5 then a.FromLocationID else a.ToLocationID end =t3.LocationID  --and ( a.FromLocationID=t3.LocationID or  a.ToLocationID=t3.LocationID)
--and case when CT.IsAllCategoryType=1 then t3.CategoryTypeID else a.CategorytypeID end =t3.CategoryTypeID 
GO

Alter table ProductTable alter column ProductCode nvarchar(max) not null
go
--if not exists(select FieldName from MasterGridNewLineItemTable where FieldName='MasterGridID' 
--		and MasterGridID=(select mastergridid from MasterGridNewTable where MasterGridName='MasterGridLineItem')) 
--	Begin 
--	insert into MasterGridNewLineItemTable(FieldName,DisplayName,Width,Format,IsDefault,OrderID,ColumnType,ActionScript,MasterGridID)
--	select 'MasterGridID','MasterGridID',100,NULL,0,3,'System.String',NULL,MasterGridID
--	FRom MasterGridNewTable where MasterGridName='MasterGridLineItem'
--	End 
--	go 
--	update MasterGridNewLineItemTable set IsDefault=1 where MasterGridID=1058
ALTER View [dbo].[vwReportTemplateObjects]
AS
	SELECT * 
		FROM  [vwSystemTemplateObjects]
		WHERE
			(ObjectName LIKE 'rvw%' OR ObjectName LIKE 'rprc%') and 
			ObjectName not in (select QueryString from ReportTemplateTable where StatusID = dbo.fnGetActiveStatusID() and QueryString is not null)
GO
alter table reportTable add CONSTRAINT  DF_ReportPageHeight DEFAULT 0.00000 FOR ReportPageHeight;
go 
alter table reportTable add CONSTRAINT  DF_ReportPageWidth DEFAULT 0.00000 FOR ReportPageWidth;
go 

-- ==============================================================================================================================================
-- Author:		Balakrishnan.P
-- Create date: 03-OCT-2019 10:00:00 AM
-- Description:	Trigger for maintaining menus for each report templates
-- **********************************************************************************************************************************************
-- Date Time			Author			Ver		Description
-- **********************************************************************************************************************************************
-- 11-Dec-2019 13:10	Balakrishnan	0.55	
-- ==============================================================================================================================================
Create TRIGGER [dbo].[trg_Ins_ReportTemplateTable]
   ON  [dbo].[ReportTemplateTable]
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	/* Insert the Required Menus */
	DECLARE trg_Ins_ReportTemplateTable_C CURSOR
	FOR SELECT ReportTemplateID, ReportTemplateName, C.ReportTemplateCategoryName
		FROM inserted A
		LEFT JOIN ReportTemplateCategoryTable C ON C.ReportTemplateCategoryID = A.ReportTemplateCategoryID

	DECLARE @ReportTemplateID INT, 
			@ReportTemplateName VARCHAR(500),
			@ReportTemplateCategoryName VARCHAR(500),
			@RightID	INT,
			@RightName VARCHAR(250),
			@TargetObject VARCHAR(250)

	OPEN trg_Ins_ReportTemplateTable_C
	FETCH NEXT FROM trg_Ins_ReportTemplateTable_C 
		INTO @ReportTemplateID, @ReportTemplateName, @ReportTemplateCategoryName

	WHILE (@@FETCH_STATUS = 0)
	BEGIN
		SET @RightName = 'Report' + @ReportTemplateName;
		SET @TargetObject = '/ShowReport/DynamicReport/' + CAST(@ReportTemplateID as varchar(50))

		exec wsprc_CreateMenu 'Reports', @RightName, 'Reports', @ReportTemplateCategoryName, @ReportTemplateName, @TargetObject, 1, 
			NULL, @ReportTemplateID, 'ReportTemplate'

		SELECT @RightID = RightID FROM User_RightTable WHERE RightName = @RightName

		UPDATE ReportTemplateTable
			SET RightID = @RightID
			WHERE ReportTemplateID = @ReportTemplateID

		FETCH NEXT FROM trg_Ins_ReportTemplateTable_C 
			INTO @ReportTemplateID, @ReportTemplateName, @ReportTemplateCategoryName
	END

	CLOSE trg_Ins_ReportTemplateTable_C
	DEALLOCATE trg_Ins_ReportTemplateTable_C
END
go 

-- ==============================================================================================================================================
-- Author:		Balakrishnan.P
-- Create date: 03-OCT-2019 10:00:00 AM
-- Description:	Trigger for maintaining menus for each report templates
-- **********************************************************************************************************************************************
-- Date Time			Author			Ver		Description
-- **********************************************************************************************************************************************
-- 11-Dec-2019 13:10	Balakrishnan	0.55	
-- ==============================================================================================================================================
create TRIGGER [dbo].[trg_Upd_ReportTemplateTable]
   ON  [dbo].[ReportTemplateTable]
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	/* Insert the Required Menus */
	DECLARE trg_Upd_ReportTemplateTable_C CURSOR
	FOR SELECT ReportTemplateID, ReportTemplateName FROM inserted

	DECLARE @ReportTemplateID INT, 
			@ReportTemplateName VARCHAR(500)

	OPEN trg_Upd_ReportTemplateTable_C
	FETCH NEXT FROM trg_Upd_ReportTemplateTable_C 
		INTO @ReportTemplateID, @ReportTemplateName

	WHILE (@@FETCH_STATUS = 0)
	BEGIN
		UPDATE User_MenuTable 
			SET MenuName = @ReportTemplateName
			WHERE ParentTransactionID = @ReportTemplateID
				AND ParentTransactionType = 'ReportTemplate'

		FETCH NEXT FROM trg_Upd_ReportTemplateTable_C 
			INTO @ReportTemplateID, @ReportTemplateName
	END

	CLOSE trg_Upd_ReportTemplateTable_C
	DEALLOCATE trg_Upd_ReportTemplateTable_C
END


ALTER trigger [dbo].[trg_Ins_AssetDataTable] on [dbo].[ZDOF_UserPODataTable] 
after Insert
As
Begin
   Declare @ProductID int,@categoryID int,@qty int ,@zDOFPurchaseOrderID int ,@PDHeaderID nvarchar(300) ,@PoLineItemID nvarchar(300),@ProductName nvarchar(max),@UnitCost decimal(18,5),@SupplierID int,@CreatedBy int 
   Declare @DefaultLanguageID INT,@CompanyID	INT,@Cnt int ,@LastBarcodeVal INT,@CompanyCode nvarchar(10)
   Declare @BarcodeAuto bit ,@BarcodeAssetCodeequal bit ,@DepartmentID int 
     Select @BarcodeAuto=case when upper(ConfiguarationValue)='TRUE' then 1 else 0 end  From ConfigurationTable where ConfiguarationName='BarcodeAutoGenerateEnable'
	 Select @BarcodeAssetCodeequal=case when upper(ConfiguarationValue)='TRUE' then 1 else 0 end  From ConfigurationTable where ConfiguarationName='BarcodeEqualAssetCode'
	
	SELECT @DefaultLanguageID = MIN(LanguageID) FROM LanguageTable
	SELECT @CompanyID = MIN(CompanyID) FROM CompanyTable WHERE StatusID = 1
	Select @CompanyCode=left(companyCode,4) From CompanyTable where companyID=@CompanyID
  
    Select @qty=QUANTITY, @zDOFPurchaseOrderID=ZDOFPurchaseOrderID, @PDHeaderID=PO_HEADER_ID, @PoLineItemID=PO_LINE_ID, @ProductName=PO_LINE_DESC,
	@UnitCost=UnitCost,@categoryID=CategoryID,@CreatedBy=CreatedBy,@DepartmentID=DepartmentID From Inserted
	
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
				DepreciationFlag,Attribute1,DepartmentID, DOFPO_LINE_NUM)
					
				SELECT 
					'-', '-',
					--'AT' + FORMAT(@LastBarcodeVal, '00000000'),
					--'AT' + FORMAT(@LastBarcodeVal, '00000000'),
				--Select case when @BarcodeAuto=1 then '-' else @CompanyCode + FORMAT(@LastBarcodeVal, '000000') end , 
				--case when @BarcodeAuto=1 and @BarcodeAssetCodeequal=1 then '-' else @CompanyCode+ FORMAT(@LastBarcodeVal, '000000') end ,
				@categoryID,@ProductID,@SupplierID,
				@UnitCost,PO_NUMBER,@ProductName,PO_DATE,PO_DATE,1,@CompanyID,@CreatedBy,getdate(),1,0, 5,
				0,VENDOR_SITE_CODE,@DepartmentID, ISNULL(LINE_NUM, 1)
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


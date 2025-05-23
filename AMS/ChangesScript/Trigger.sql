

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

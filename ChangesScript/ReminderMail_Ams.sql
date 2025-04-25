Alter table notificationinputtable add ReminderMailCount int NULL 
go
update notificationinputtable set ReminderMailCount=0 
go
ALTER TABLE notificationinputtable ADD CONSTRAINT DF_notificationinputtable_MailCount DEFAULT 0 FOR ReminderMailCount;
go
Alter table notificationinputtable add ReminderDatetime SmallDateTime 
go


ALTER PROC [dbo].[aprc_GetNotificationInputs]
AS
BEGIN

select a.*,'<td style="width:100px;border: 1px solid black;border-collapse: collapse;">'+UPPER(FieldName)+'</td>'  as FieldTitle,
'<td style="width:100px;border: 1px solid black;border-collapse: collapse;">##'+UPPER(FieldName)+'##</td>' as FieldValue,b.NotificationTemplateID into #tmpNotificationModuleFieldTable
		from NotificationModuleFieldTable a join NotificationTemplateFieldTable b  on A.NotificationFieldID=B.NotificationFieldID
		where A.StatusID = dbo.fnGetActiveStatusID() AND B.StatusID = dbo.fnGetActiveStatusID()

	SELECT DISTINCT A.NotificationInputID, A.SYSDataID1, A.SYSDataID2, A.SYSDataID3, 
			M.QueryType, M.QueryString, T.*,TT.IsAttachmentRequired,TT.AllowHtmlContent,X.TemplateTableBodyContentHeader,X.TemplateTableBodyContent,X.TemplateTableBodyContentFooter,A.ReminderMailCount
		FROM NotificationInputTable A
		LEFT JOIN NotificationTemplateTable T ON T.NotificationTemplateID = A.NotificationTemplateID
		LEFT JOIN NotificationModuleTable M ON M.NotificationModuleID = T.NotificationModuleID
		LEFT JOIN NotificationTypeTable TT ON TT.NotificationTypeID = T.NotificationTypeID
		Left join (Select A.NotificationTemplateID,'<table style="border: 1px solid black;border-collapse: collapse;"><tbody>'
		+'<tr>'+STUFF((  SELECT '' + FieldTitle AS "text()"
          FROM #tmpNotificationModuleFieldTable US
          WHERE US.NotificationTemplateID = a.NotificationTemplateID 
          ORDER BY NotificationTemplateID
            FOR XML PATH(''),TYPE).value('./text()[1]','NVARCHAR(MAX)'),1,0,'')+'</tr>' as TemplateTableBodyContentHeader,
		'<tr>'+STUFF((  SELECT '' + FieldValue AS "text()"
          FROM #tmpNotificationModuleFieldTable US
          WHERE US.NotificationTemplateID = a.NotificationTemplateID 
          ORDER BY NotificationTemplateID
            FOR XML PATH(''),TYPE).value('./text()[1]','NVARCHAR(MAX)'),1,0,'')+'</tr>' as TemplateTableBodyContent,
		'</tbody></table>' as TemplateTableBodyContentFooter
		
		from #tmpNotificationModuleFieldTable A
		
		Group by  A.NotificationTemplateID) X ON X.NotificationTemplateID=T.NotificationTemplateID
		--LEFT JOIN (Select A.NotificationTemplateID,'<table style="border: 1px solid black;border-collapse: collapse;"><tbody>'
		--+'<tr>'+STRING_AGG('<td style="width:100px;border: 1px solid black;border-collapse: collapse;">'+(B.FieldName)+'</td>','')+'</tr>' as TemplateTableBodyContentHeader,
		--'<tr>'+STRING_AGG('<td style="width:100px;border: 1px solid black;border-collapse: collapse;">##'+UPPER(B.FieldName)+'##</td>','')+'</tr>' as TemplateTableBodyContent,
		--'</tbody></table>' as TemplateTableBodyContentFooter
		--from NotificationTemplateFieldTable A
		--Left Join NotificationModuleFieldTable B on A.NotificationFieldID=B.NotificationFieldID
		--WHERE A.StatusID = dbo.fnGetActiveStatusID() AND B.StatusID = dbo.fnGetActiveStatusID()
		--Group by  A.NotificationTemplateID
		--)X ON X.NotificationTemplateID=T.NotificationTemplateID

		WHERE A.IsCompleted = 0
		drop table #tmpNotificationModuleFieldTable
END
go 


If not exists(select ConfiguarationName from ConfigurationTable where ConfiguarationName='ReminderMainInDays')
Begin 
	insert into ConfigurationTable(ConfiguarationName,ConfiguarationValue,ToolTipName,DataType,DropDownValue,MinValue,MaxValue,ConfiguarationType,DisplayOrderID,
	DefaultValue,DisplayConfiguration,SuffixText,CatagoryName)
	values('ReminderMainInDays','2','ReminderMainInDays','Numeric',NULL,1,10,'C',130,1,1,NULL,'EMail')
End 
go 

Create Procedure prc_RemainderMainInsertData
as 
Begin 
	declare @ReminderMaininday int,@TransferNotifiID int, @RetirementNotifiID int 

	select @ReminderMaininday=cast(ConfiguarationValue as int) from ConfigurationTable where ConfiguarationName='ReminderMainInDays'

	select * into tmp_WaitingForApprovalTable from 
	(
	select *,ROW_NUMBER() over( 
         PARTITION BY transactionid , approvemoduleID ORDER BY Orderno asc ) AS SerialNo
	from ApprovalHistoryTable where statusid=150 --and DATEADD(day,@ReminderMaininday,CAST(CreatedDateTime AS Date))=CAST( GETDATE() AS Date) 
	) x where x.SerialNo=1 

	 select @TransferNotifiID= NotificationTemplateID from NotificationTemplateTable where 
			TemplateCode='AssetTransferBeforeApproval'

	 select @RetirementNotifiID= NotificationTemplateID from NotificationTemplateTable where 
				   TemplateCode='AssetRetirementBeforeApproval'
				 
       update b set 
	   IsCompleted=0,
	   ReminderDatetime=GETDATE(),
	   ReminderMailCount=DATEDIFF(day,a.CreatedDateTime,GETDATE())/@ReminderMaininday

	   from NotificationInputTable  B 
	   join tmp_WaitingForApprovalTable  A on a.TransactionID=b.SYSDataID1 and a.ApprovalHistoryID=b.SYSDataID2
	   and case when a.ApproveModuleID=5 then @TransferNotifiID else @RetirementNotifiID end =b.NotificationTemplateID
	   where a.StatusID=150 and 
	   DATEADD(day,@ReminderMaininday,CAST( CASE WHEN B.ReminderMailCount=0 THEN A.CreatedDateTime ELSE B.ReminderDatetime END AS Date))=CAST(GETDATE() AS Date) 
	   AND CAST(B.ReminderDatetime AS Date)!=CAST(GETDATE() AS Date) 
		
		drop table tmp_WaitingForApprovalTable
end 

go 



ALTER PROC [dbo].[aprc_GetNotificationInputs]
AS
BEGIN
 

 exec prc_RemainderMainInsertData

select a.*,'<td style="width:100px;border: 1px solid black;border-collapse: collapse;">'+UPPER(FieldName)+'</td>'  as FieldTitle,
'<td style="width:100px;border: 1px solid black;border-collapse: collapse;">##'+UPPER(FieldName)+'##</td>' as FieldValue,b.NotificationTemplateID into #tmpNotificationModuleFieldTable
		from NotificationModuleFieldTable a join NotificationTemplateFieldTable b  on A.NotificationFieldID=B.NotificationFieldID
		where A.StatusID = dbo.fnGetActiveStatusID() AND B.StatusID = dbo.fnGetActiveStatusID()

	SELECT DISTINCT A.NotificationInputID, A.SYSDataID1, A.SYSDataID2, A.SYSDataID3, 
			M.QueryType, M.QueryString, T.*,TT.IsAttachmentRequired,TT.AllowHtmlContent,X.TemplateTableBodyContentHeader,X.TemplateTableBodyContent,X.TemplateTableBodyContentFooter,A.ReminderMailCount
		FROM NotificationInputTable A
		LEFT JOIN NotificationTemplateTable T ON T.NotificationTemplateID = A.NotificationTemplateID
		LEFT JOIN NotificationModuleTable M ON M.NotificationModuleID = T.NotificationModuleID
		LEFT JOIN NotificationTypeTable TT ON TT.NotificationTypeID = T.NotificationTypeID
		Left join (Select A.NotificationTemplateID,'<table style="border: 1px solid black;border-collapse: collapse;"><tbody>'
		+'<tr>'+STUFF((  SELECT '' + FieldTitle AS "text()"
          FROM #tmpNotificationModuleFieldTable US
          WHERE US.NotificationTemplateID = a.NotificationTemplateID 
          ORDER BY NotificationTemplateID
            FOR XML PATH(''),TYPE).value('./text()[1]','NVARCHAR(MAX)'),1,0,'')+'</tr>' as TemplateTableBodyContentHeader,
		'<tr>'+STUFF((  SELECT '' + FieldValue AS "text()"
          FROM #tmpNotificationModuleFieldTable US
          WHERE US.NotificationTemplateID = a.NotificationTemplateID 
          ORDER BY NotificationTemplateID
            FOR XML PATH(''),TYPE).value('./text()[1]','NVARCHAR(MAX)'),1,0,'')+'</tr>' as TemplateTableBodyContent,
		'</tbody></table>' as TemplateTableBodyContentFooter
		
		from #tmpNotificationModuleFieldTable A
		
		Group by  A.NotificationTemplateID) X ON X.NotificationTemplateID=T.NotificationTemplateID
		--LEFT JOIN (Select A.NotificationTemplateID,'<table style="border: 1px solid black;border-collapse: collapse;"><tbody>'
		--+'<tr>'+STRING_AGG('<td style="width:100px;border: 1px solid black;border-collapse: collapse;">'+(B.FieldName)+'</td>','')+'</tr>' as TemplateTableBodyContentHeader,
		--'<tr>'+STRING_AGG('<td style="width:100px;border: 1px solid black;border-collapse: collapse;">##'+UPPER(B.FieldName)+'##</td>','')+'</tr>' as TemplateTableBodyContent,
		--'</tbody></table>' as TemplateTableBodyContentFooter
		--from NotificationTemplateFieldTable A
		--Left Join NotificationModuleFieldTable B on A.NotificationFieldID=B.NotificationFieldID
		--WHERE A.StatusID = dbo.fnGetActiveStatusID() AND B.StatusID = dbo.fnGetActiveStatusID()
		--Group by  A.NotificationTemplateID
		--)X ON X.NotificationTemplateID=T.NotificationTemplateID

		WHERE A.IsCompleted = 0
		drop table #tmpNotificationModuleFieldTable
END
go 

update MasterGridNewLineItemTable set FieldName='Entity.EntityName' where FieldName='EntityName' and MasterGridID in (select MasterGridID from MasterGridNewTable where MasterGridName='ImportFormat')
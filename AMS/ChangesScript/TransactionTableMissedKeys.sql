IF OBJECT_ID('PostingStatusTable') IS NULL
BEGIN
CREATE TABLE PostingStatusTable(
	PostingStatusID int NOT NULL PRIMARY KEY,
	PostingStatus nvarchar(100) NOT NULL
)
END

IF NOT EXISTS(select PostingStatus from PostingStatusTable where PostingStatus='WorkInProgress')
BEGIN
	INSERT into PostingStatusTable(PostingStatusID,PostingStatus)values(1,'WorkInProgress')
END

IF NOT EXISTS(select PostingStatus from PostingStatusTable where PostingStatus='CompletedByEndUser')
BEGIN
	INSERT into PostingStatusTable(PostingStatusID,PostingStatus)values(10,'CompletedByEndUser')
END

IF NOT EXISTS(select PostingStatus from PostingStatusTable where PostingStatus='WaitingForFinalApproval')
BEGIN
	INSERT into PostingStatusTable(PostingStatusID,PostingStatus)values(11,'WaitingForFinalApproval')
END


ALTER TABLE TransactionTable
ADD  FOREIGN KEY (PostingStatusID) REFERENCES PostingStatusTable (PostingStatusID);
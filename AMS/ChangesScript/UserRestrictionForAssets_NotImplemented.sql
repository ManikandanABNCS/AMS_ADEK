ALTER TABLE LocationTable
	ADD UserMappingLocationID INT REFERENCES LocationTable
	
ALTER TABLE CategoryTable
	ADD UserMappingCategoryID INT REFERENCES CategoryTable

ALTER TABLE AssetTable
	ADD UserMappingLocationID INT REFERENCES LocationTable

UPDATE A
	SET A.UserMappingLocationID = B.SecondLevel
	FROM LocationTable A
	LEFT JOIN LocationHierarchicalView B ON A.LocationID = B.ChildID
	WHERE B.SecondLevel IS NOT NULL AND A.UserMappingLocationID IS NULL

UPDATE A
	SET A.UserMappingLocationID = B.UserMappingLocationID
	FROM AssetTable A
	LEFT JOIN LocationTable B ON A.LocationID = B.LocationID
	WHERE B.UserMappingLocationID IS NOT NULL AND A.UserMappingLocationID IS NULL
	
UPDATE CategoryTable SET UserMappingCategoryID = CategoryID	WHERE UserMappingCategoryID IS NULL

DROP PROC IF EXISTS prc_GetUserAssets
GO

--For improved performance Lets store the Mapping level Category ID & Location ID in Asset table
--Mapping level Category ID in Category table
--Mapping level Location ID in Location table

CREATE PROC prc_GetUserAssets (
	@UserID int ,
	@NoOfRecords INT
)
AS
Begin 
	DECLARE @STATUS_DELETED INT = 3

	DECLARE @LoctionMappingFound INT, @CategoryMappingFound INT, @DepartmentMappingFound INT
	SELECT @LoctionMappingFound = COUNT(*) FROM UserLocationMappingTable WHERE StatusID <>  @STATUS_DELETED AND PersonID = @UserID
	SELECT @CategoryMappingFound = COUNT(*) FROM UserCategoryMappingTable WHERE StatusID <>  @STATUS_DELETED AND PersonID = @UserID
	SELECT @DepartmentMappingFound = COUNT(*) FROM UserDepartmentMappingTable WHERE StatusID <>  @STATUS_DELETED AND PersonID = @UserID

	SELECT COUNT(*) from [AssetTable] A
		--LEFT JOIN LocationTable L1 ON A.LocationID = L1.LocationID
		WHERE (@LoctionMappingFound = 0 OR A.UserMappingLocationID IN(SELECT LocationID FROM UserLocationMappingTable WHERE StatusID <>  @STATUS_DELETED AND PersonID = @UserID))
			AND (@CategoryMappingFound = 0 OR A.CategoryID IN(SELECT CategoryID FROM UserCategoryMappingTable WHERE StatusID <>  @STATUS_DELETED AND PersonID = @UserID))
			AND (@DepartmentMappingFound = 0 OR A.DepartmentID IN(SELECT DepartmentID FROM UserDepartmentMappingTable WHERE StatusID <>  @STATUS_DELETED AND PersonID = @UserID))
	
	select TOP (@NoOfRecords) * from [AssetNewView] A
		--LEFT JOIN LocationTable L1 ON A.LocationID = L1.LocationID
		WHERE (@LoctionMappingFound = 0 OR A.UserMappingLocationID IN(SELECT LocationID FROM UserLocationMappingTable WHERE StatusID <>  @STATUS_DELETED AND PersonID = @UserID))
			AND (@CategoryMappingFound = 0 OR A.CategoryID IN(SELECT CategoryID FROM UserCategoryMappingTable WHERE StatusID <>  @STATUS_DELETED AND PersonID = @UserID))
			AND (@DepartmentMappingFound = 0 OR A.DepartmentID IN(SELECT DepartmentID FROM UserDepartmentMappingTable WHERE StatusID <>  @STATUS_DELETED AND PersonID = @UserID))
	RETURN 
End 
GO

EXEC prc_GetUserAssets 34223, 10
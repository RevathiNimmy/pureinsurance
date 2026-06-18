EXECUTE DDLDropProcedure 'spu_Report_AllUsers'
GO

CREATE PROCEDURE spu_Report_AllUsers
AS


BEGIN

CREATE Table #UserList
(
	RowNumber	INT IDENTITY,
	UserID		INT,
	Username	VARCHAR(255)
)

INSERT INTO #UserList(UserID,Username) VALUES(0,'All')
INSERT INTO #UserList(UserID,Username) SELECT User_id , username FROM PMUser ORDER BY username

SELECT UserID,Username FROM #UserList

END

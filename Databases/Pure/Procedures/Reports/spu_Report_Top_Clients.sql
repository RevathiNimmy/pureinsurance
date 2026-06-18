SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Report_Top_Clients'
GO
CREATE PROCEDURE spu_Report_Top_Clients
    @branch_id int
AS

DECLARE @iBranchID int
SELECT @iBranchID = ISNULL(@branch_id, 0)

DECLARE @party_cnt INT,
        @address_cnt INT,
        @phone VARCHAR(255),
        @area_code VARCHAR(10),
        @number VARCHAR(255),
        @extension VARCHAR(6)

CREATE TABLE #ReportPartyList
(
	party_cnt int NOT NULL,
	shortname char(20) NOT NULL,
	is_prospect tinyint NULL,
	prospect char(1) NOT NULL,
	resolved_name varchar(100) NOT NULL,
	code char(10) NOT NULL,
	address1 varchar(60) NOT NULL,
	address2 varchar(60) NULL,
	address3 varchar(60) NULL,
	address4 varchar(60) NULL,
	postal_code varchar(20) NOT NULL,
	address_cnt int NOT NULL,
	department varchar(255) NOT NULL,
	phone varchar(255) NOT NULL
) 

INSERT INTO #ReportPartyList
SELECT P.party_cnt,
    P.shortname,
    P.resolved_name
FROM Party P,
    Party_Type PT,
    Account A,
    Transdetail T,
    Document D
WHERE P.party_type_id = PT.party_type_id
AND PT.code IN ('PC', 'GC', 'CC')
AND P.is_deleted = 0
AND A.account_key = P.party_id
AND T.account_id = A.account_id
AND D.document_id = T.document_id
AND D.document_date > DATEADD( year, -2, GetDate())
AND (
        @iBranchID = 0
        OR
        (
            @iBranchID <> 0
            AND
            P.source_id = @iBranchID
        )
    )

DECLARE List_Cursor CURSOR FAST_FORWARD FOR
    SELECT party_cnt
    FROM #ReportPartyList

OPEN List_Cursor

FETCH NEXT FROM List_Cursor INTO @party_cnt

WHILE @@FETCH_STATUS = 0 BEGIN
    FETCH NEXT FROM List_Cursor INTO @party_cnt
END

DROP TABLE #ReportPartyList

GO


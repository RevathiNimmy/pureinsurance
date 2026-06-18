SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Report_Account_List'
GO
CREATE PROCEDURE spu_Report_Account_List
    @branch_id int
AS

DECLARE @iBranchID int

SELECT @iBranchID = ISNULL(@branch_id, 0)

IF @iBranchID = 0
BEGIN
    SELECT A.short_code Account_Code,
        A.account_name Account_Name,
        L.ledger_name Ledger,
        S.description Status,
        SRC.description Branch
    FROM Account A,
        Ledger L,
        AccountStatus S,
        Source SRC
    WHERE L.ledger_id = A.ledger_id
    AND S.accountstatus_id = A.accountstatus_id
    And l.ledger_short_name <> 'SA'
    AND SRC.source_id = A.company_id
    union
    SELECT A.short_code Account_Code,
        A.account_name Account_Name,
        L.ledger_name Ledger,
        S.description Status,
        SRC.description Branch
    FROM Account A,
        Ledger L,
        AccountStatus S,
        Party P,
	Source SRC
    WHERE L.ledger_id = A.ledger_id
    AND S.accountstatus_id = A.accountstatus_id
    And l.ledger_short_name = 'SA'
    and p.party_cnt=a.account_key
    and p.is_deleted=0
    AND SRC.source_id = A.company_id
END
ELSE
BEGIN
    SELECT A.short_code Account_Code,
        A.account_name Account_Name,
        L.ledger_name Ledger,
        S.description Status,
        SRC.description Branch
    FROM Account A,
        Ledger L,
        AccountStatus S,
	Source SRC
    WHERE L.ledger_id = A.ledger_id
    AND S.accountstatus_id = A.accountstatus_id
    AND A.company_id = @iBranchID
    And l.ledger_short_name <> 'SA'
    AND SRC.source_id = A.company_id
    UNION
    SELECT A.short_code Account_Code,
        A.account_name Account_Name,
        L.ledger_name Ledger,
        S.description Status,
        SRC.description Branch
    FROM Account A,
        Ledger L,
        AccountStatus S,
        Party P,
	Source SRC
    WHERE L.ledger_id = A.ledger_id
    AND S.accountstatus_id = A.accountstatus_id
    AND A.company_id = @iBranchID
    And l.ledger_short_name = 'SA'
    and p.party_cnt=a.account_key
    and p.is_deleted=0
    AND SRC.source_id = A.company_id
END

GO


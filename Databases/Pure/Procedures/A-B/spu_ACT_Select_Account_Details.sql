SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_Select_Account_Details'
GO

CREATE PROCEDURE spu_ACT_Select_Account_Details
(
    @AccountID as INT
)
 AS

SELECT
    a.account_name,
    a.contact_name,
    a.phone_area_code,
    a.phone_number,
    a.phone_extension,
    acs.code,
    a.ledger_id,
    l.ledger_short_name
FROM Account a
INNER JOIN Ledger l ON l.ledger_id=a.ledger_id
INNER JOIN AccountStatus acs ON acs.accountstatus_id=a.accountstatus_id
WHERE account_id = @AccountID
GO

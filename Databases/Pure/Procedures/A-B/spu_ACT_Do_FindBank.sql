SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Do_FindBank'
GO


CREATE PROCEDURE spu_ACT_Do_FindBank
    @ShortCode varchar(10) = NULL,
    @BankName varchar(30) = NULL,
    @company_id INTEGER = NULL,
    @MaxRowsToFetch INT = -1
AS

DECLARE @source_id INT

SELECT @source_id = NULL
/* Test to see if we are multi company */
IF EXISTS (SELECT * FROM hidden_options WHERE branch_id = 1 AND option_number = 16)
BEGIN
    IF EXISTS (SELECT * FROM hidden_options WHERE branch_id = 1 AND option_number = 37)
    BEGIN
        /* We are multi company */
        SELECT @source_id = @company_id
    END
END
IF @MaxRowsToFetch<>-1
BEGIN
SET NOCOUNT ON    
SET ROWCOUNT @MaxRowsToFetch
END
/* eck 31/08/01 modified to return the correct head office name*/
SELECT DISTINCT
    bk.bank_id,
    bk.code,
    bk.branch_code,
    bk.bank_name,
    bk2.bank_name,
    bk.bank_address1
    FROM
    bank bk
    INNER JOIN bank bk2 ON bk2.bank_id = bk.head_office
    WHERE (bk.code LIKE @ShortCode OR @ShortCode = NULL)
    AND (bk.bank_name LIKE @BankName OR @BankName = NULL)
    ORDER BY bk.code
IF @MaxRowsToFetch<>-1
BEGIN
SET ROWCOUNT 0  
SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Select_Small_Amount_Write_Off'
GO

-- Get the positve and negative small write off amounts for a branch

CREATE PROCEDURE spu_ACT_Select_Small_Amount_Write_Off

    @lSourceId INT

AS

SELECT
    A.account_id,
    SAWO.negamount,
    SAWO.posamount
FROM
    Small_Amount_Write_Off SAWO
JOIN
    Account A ON (A.short_code = SAWO.short_code)

WHERE
    source_id = @lSourceId
AND
    is_deleted = 0
AND
    effective_date <=GetDate()

GO



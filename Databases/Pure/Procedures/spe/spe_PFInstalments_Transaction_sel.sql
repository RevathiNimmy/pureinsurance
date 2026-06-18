SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PFInstalments_Transaction_sel'
GO
/*
    Return Premium Finance Instalment Transaction Code List

    Return Array:
         0 - pfinstalments_transaction_id
         1 - code
         2 - description

    History
        PF060901 - Created
*/
CREATE PROCEDURE spe_PFInstalments_Transaction_sel
AS

    SELECT
        pfinstalments_transaction_id,
        code,
        description
    FROM
        pfinstalments_transaction
    WHERE
        is_deleted = 0
    AND
        effective_date < = GetDate()
    ORDER BY
        description

GO


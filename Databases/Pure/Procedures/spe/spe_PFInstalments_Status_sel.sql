SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PFInstalments_Status_sel'
GO
/*
    Return Premium Finance Instalment Status List

    Return Array:
         0 - pfinstalments_status_id
         1 - code
         2 - description

    History
        PF060901 - Created
*/
CREATE PROCEDURE spe_PFInstalments_Status_sel
AS

    SELECT
        pfinstalments_status_id,
        code,
        description
    FROM
        PFInstalments_status
    WHERE
        is_deleted = 0
    AND
        effective_date < = getdate()
    ORDER BY
        description

GO


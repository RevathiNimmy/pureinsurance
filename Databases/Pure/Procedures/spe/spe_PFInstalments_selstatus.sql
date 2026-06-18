SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PFInstalments_selstatus'
GO
CREATE PROCEDURE spe_PFInstalments_selstatus
AS

SELECT
    PFInstalments_status_id,
    code,
    Description
FROM
    PFInstalments_status
ORDER BY
    PFInstalments_status_id

GO


EXECUTE DDLDropProcedure 'spu_PF_GetFirstInstalmentStatus'
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
CREATE PROCEDURE spu_PF_GetFirstInstalmentStatus
    @pfprem_finance_cnt INT,
    @pfprem_finance_version INT,
    @statusId TINYINT OUTPUT	
AS

SELECT
    @statusId=PFInstalments.Status	
FROM
    PFInstalments 
WHERE
    PFInstalments.pfprem_finance_cnt = @pfprem_finance_cnt
AND PFInstalments.pfprem_finance_version = @pfprem_finance_version
AND InstalmentNumber = 1

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
-------------------------------------------------------------------------------
--  Author:  AMB
--  Date:    02 Oct 2003
--  Desc:    SFB 1.8.6 Accident Management development
-------------------------------------------------------------------------------

SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Extra_Scheme_selall'
GO

CREATE PROCEDURE spu_Extra_Scheme_selall
AS 

SELECT 
    Extra_scheme_id,
    [Description]
FROM 
    Extra_Scheme
WHERE
    [Description] <> ''
ORDER BY
    [Description] ASC

GO



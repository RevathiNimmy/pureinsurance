EXECUTE DDLDropProcedure 'spu_PFFrequency_Sel_All'
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE spu_PFFrequency_Sel_All
AS
SELECT
    pffrequency_id, code, description, caption_id, effective_date,
    is_deleted, period, amount
FROM PFFrequency
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

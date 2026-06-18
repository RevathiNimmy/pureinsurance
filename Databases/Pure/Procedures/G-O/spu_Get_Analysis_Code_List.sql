EXECUTE DDLDropProcedure 'spu_Get_Analysis_Code_List'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_Get_Analysis_Code_List
    @EffectiveDate  datetime


--
-- ED03072002 Retrieves list of Analysis_Code
--

AS

BEGIN

    SELECT analysis_code_id,
           description,
           code
      FROM analysis_code
     WHERE is_deleted = 0
       AND Effective_date <= @EffectiveDate
  ORDER BY description

END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO


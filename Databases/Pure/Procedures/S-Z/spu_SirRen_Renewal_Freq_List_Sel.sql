SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SirRen_Renewal_Freq_List_Sel'
GO

CREATE PROCEDURE spu_SirRen_Renewal_Freq_List_Sel
    @EffectiveDate  datetime

AS

BEGIN

    SELECT [RNF].renewal_frequency_id,
           [RNF].description,
           [RNF].number_of_months
      FROM renewal_frequency [RNF]
     WHERE [RNF].is_deleted = 0
       AND [RNF].effective_date <= @EffectiveDate

END
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO


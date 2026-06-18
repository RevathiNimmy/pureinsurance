EXECUTE DDLDropProcedure 'spu_Get_Risk_Code_List'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_Get_Risk_Code_List
    @RiskGroupId    int,
    @EffectiveDate  datetime


--
-- ED03072002 Retrieves list of Risk_Code for Group
--

AS

SELECT risk_code_id,
       description
  FROM risk_code
 WHERE risk_group_id = @RiskGroupId
   AND is_deleted = 0
   AND Effective_date <= @EffectiveDate

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO


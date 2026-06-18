SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_CLM_Get_PrimCause_RiskTypeGrp'
GO

CREATE  PROCEDURE spu_CLM_Get_PrimCause_RiskTypeGrp
    @primary_cause_id int
AS
SELECT
    pr.primary_cause_id,
    pr.risk_type_group_id
FROM    
    primary_cause_risk_type_group as pr
WHERE   
    pr.primary_cause_id = @primary_cause_id

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

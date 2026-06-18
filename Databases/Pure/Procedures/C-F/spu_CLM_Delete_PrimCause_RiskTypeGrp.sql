SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_CLM_Delete_PrimCause_RiskTypeGrp'
GO

CREATE  PROCEDURE spu_CLM_Delete_PrimCause_RiskTypeGrp
    @primary_cause_id int,
	@userid INT = NULL,
	@uniqueid VARCHAR(50) = NULL,
	@screenhierarchy VARCHAR(100) = NULL

AS

UPDATE pcr  SET 
        userid=@UserId,
        uniqueid=@uniqueid,
		screenhierarchy=@screenhierarchy
    FROM Primary_Cause_Risk_Type_Group pcr 

    WHERE
	    primary_cause_id = @primary_cause_id

DELETE FROM Primary_Cause_Risk_Type_Group
WHERE primary_cause_id = @primary_cause_id

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_CLM_Add_PrimCause_RiskTypeGrp'
GO

CREATE  PROCEDURE spu_CLM_Add_PrimCause_RiskTypeGrp
    @primary_cause_id int,
    @risk_type_group_id int,
	@userid INT = NULL,
	@uniqueid VARCHAR(50) = NULL,
	@screenhierarchy VARCHAR(100) = NULL
AS
BEGIN

SELECT @screenhierarchy=@screenhierarchy 

INSERT INTO primary_cause_risk_type_group
    (
    primary_cause_id ,
    risk_type_group_id,
	userid,
	uniqueid ,
	screenhierarchy
    )
VALUES 
    (
    @primary_cause_id ,
    @risk_type_group_id,
	@userid,
	@uniqueid ,
	@screenhierarchy
    )
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


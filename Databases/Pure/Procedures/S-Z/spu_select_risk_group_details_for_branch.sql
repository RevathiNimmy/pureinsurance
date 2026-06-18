SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_select_risk_group_details_for_branch'
GO


CREATE PROCEDURE spu_select_risk_group_details_for_branch
    @source_id int
AS

-- SET 30/04/2003 PS235
DECLARE @CommissionByRisk int
SELECT @CommissionByRisk = Value from hidden_options where branch_id = 1 and option_number = 40

IF @CommissionByRisk = 1
BEGIN
    SELECT DISTINCT rg.risk_group_id, rg.code, rg.description FROM risk_group rg where rg.is_deleted = 0
END
ELSE 
BEGIN
    SELECT DISTINCT 
	rg.risk_group_id, 
	rg.code, 
	rg.description 
    FROM
        risk_group  RG,
        risk_by_source RBS
    WHERE
	RG.is_deleted=0
	and RBS.risk_group_id = RG.risk_group_id
        AND RBS.source_id in(0,@source_id)
END
GO




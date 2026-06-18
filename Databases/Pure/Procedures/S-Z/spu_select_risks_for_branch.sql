SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_select_risks_for_branch'
GO


CREATE PROCEDURE spu_select_risks_for_branch
    @source_id int,
    @country_id int = 0
AS

-- SET 30/04/2003 PS235
DECLARE @CommissionByRisk int
SELECT @CommissionByRisk = Value from hidden_options where branch_id = 1 and option_number = 40

IF @CommissionByRisk = 1
BEGIN
    SELECT R.risk_code_id FROM risk_code R,
			       risk_group G
    WHERE
        G.risk_group_id = R.risk_group_id
	AND (G.country_id = @Country_id OR @country_id = 0)
END
ELSE 
BEGIN
    SELECT
        R.risk_code_id
    FROM
        risk_code   R,
        risk_group  G,
        risk_by_source  S
    WHERE
        G.risk_group_id = R.risk_group_id
        AND S.risk_group_id = G.risk_group_id
        AND (S.source_id = @source_id OR S.source_id=0)
	AND (G.country_id = @Country_id OR @country_id = 0)
END
GO




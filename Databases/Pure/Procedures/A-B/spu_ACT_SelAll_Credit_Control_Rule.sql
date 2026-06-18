SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_SelAll_Credit_Control_Rule'
GO

CREATE PROCEDURE spu_ACT_SelAll_Credit_Control_Rule
    @source_id INT = 0
AS

BEGIN

    IF @source_id = 0 BEGIN

        SELECT ccr.credit_control_rule_id,
            ccr.description,
            ccr.source_id,
            ccr.business_type,
            ccr.pffrequency_id,
            pff.description,
            ccr.is_active,
            ISNULL(ccr.use_effective_date,0)
        FROM Credit_Control_Rule ccr
        LEFT JOIN PFFrequency pff
        ON ccr.pffrequency_id = pff.pffrequency_id
        ORDER BY ccr.credit_control_rule_id
    END
    ELSE BEGIN
        SELECT ccr.credit_control_rule_id,
            ccr.description,
            ccr.source_id,
            ccr.business_type,
            ccr.pffrequency_id,
            pff.description,
            ccr.is_active,
            ISNULL(ccr.use_effective_date,0)
        FROM Credit_Control_Rule ccr
        LEFT JOIN PFFrequency pff
        ON ccr.pffrequency_id = pff.pffrequency_id
        WHERE ccr.source_id = @source_id
        ORDER BY ccr.credit_control_rule_id
    END
    

END
GO
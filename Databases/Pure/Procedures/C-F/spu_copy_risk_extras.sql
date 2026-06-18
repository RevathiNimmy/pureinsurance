SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_copy_risk_extras'
GO


CREATE PROCEDURE spu_copy_risk_extras
    @old_risk_cnt int,
    @new_risk_cnt int
AS

/*
    1.0 Created to copy extras for risk when the risk is copied in bSIRRiskScreen.
        Tom 31/07/2001

*/

/* 1.1 RKS 13/10/2005 Premium Override work */

INSERT INTO rating_section (
        risk_cnt,
        rating_section_id,
        rating_section_type_id,
        policy_section_type_id,
        sequence_number,
        description,
        rate_type_id,
        annual_rate,
        sum_insured,
        annual_premium,
        this_premium,
        original_flag,
        is_amended,
        calculated_premium,
        override_reason)
    SELECT  @new_risk_cnt,
        rating_section_id,
        rating_section_type_id,
        policy_section_type_id,
        sequence_number,
        description,
        rate_type_id,
        annual_rate,
        sum_insured,
        annual_premium,
        this_premium,
        1,
        is_amended,
        calculated_premium,
        override_reason
    FROM    rating_section
    WHERE   risk_cnt = @old_risk_cnt

    EXEC spu_copy_risk_reinsurance
        @old_risk_cnt = @old_risk_cnt,
        @new_risk_cnt = @new_risk_cnt
GO



SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_AddRIToPolicy'
GO


-- **********************************************************************************************
-- add reinsurance model to supplied policy or policies with no reinsurance (at risk level)
-- Note: this will only work if RI model supplied is 100% retained
-- **********************************************************************************************
CREATE PROCEDURE spu_AddRIToPolicy
			@InsuranceRef VARCHAR(30),
			@RIModelCode VARCHAR(10),
			@Message VARCHAR(1000) OUTPUT
AS

    -- Code removed as part of ri upgrades

    -- Future implementation should ensure appropriate configuration of
    -- ri_model and call spu_RI_Arrangement_refresh for each risk.


GO

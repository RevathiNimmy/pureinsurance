SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_RiskType_RI_Model_Usag_add'
GO
CREATE PROCEDURE spe_RiskType_RI_Model_Usag_add
    @risk_type_id int,
    @ri_band int,
    @ri_model_id int,
    @description varchar(255),
    @is_deleted int,
    @effective_date datetime
AS
BEGIN
INSERT INTO Risk_Type_RI_Model_Usage (
    risk_type_id,
    ri_band,
    ri_model_id,
    description,
    is_deleted,
    effective_date )
VALUES (
    @risk_type_id,
    @ri_band,
    @ri_model_id,
    @description,
    @is_deleted,
    @effective_date)
END

GO


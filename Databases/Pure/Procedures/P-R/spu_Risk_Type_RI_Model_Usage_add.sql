SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_RiskType_RI_Model_Usag_add'
EXECUTE DDLDropProcedure 'spu_RiskType_RI_Model_Usage_add'
EXECUTE DDLDropProcedure 'spu_Risk_Type_RI_Model_Usage_add'
GO


CREATE PROCEDURE spu_Risk_Type_RI_Model_Usage_add
    @risk_type_id int,
    @ri_band int,
    @ri_model_id int,
    @description varchar(255),
    @is_deleted int,
    @effective_date datetime,
    @expiry_date datetime,
	@portfolio_transfer_from_cnt int,
	@UserId int = NULL,
	@UniqueId varchar(50) = NUll,
	@ScreenHierarchy varchar(500) = NUll
AS

    IF NOT EXISTS (SELECT * FROM Risk_Type_RI_Model_Usage
                      WHERE risk_type_id = @risk_type_id
                        AND ri_band = @ri_band
                        AND ri_model_id = @ri_model_id
                        AND is_deleted = @is_deleted
                        AND effective_date = @effective_date
                        AND expiry_date = @expiry_date)

        INSERT INTO Risk_Type_RI_Model_Usage (
                risk_type_id,
                ri_band,
                ri_model_id,
                description,
                is_deleted,
                effective_date,
                expiry_date,
            	portfolio_transfer_from_cnt,
				UserId,
				UniqueId,
				ScreenHierarchy)
        VALUES (@risk_type_id,
                @ri_band,
                @ri_model_id,
                @description,
                @is_deleted,
                @effective_date,
                @expiry_date,
            	@portfolio_transfer_from_cnt,
				@UserId,
				@UniqueId,
				@ScreenHierarchy)


GO



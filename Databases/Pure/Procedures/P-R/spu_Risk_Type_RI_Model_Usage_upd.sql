SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


-- rename
EXECUTE DDLDropProcedure 'spu_risktype_ri_model_usage_update'
EXECUTE DDLDropProcedure 'spu_risktype_ri_model_usage_upd'
EXECUTE DDLDropProcedure 'spu_risk_type_ri_model_usage_upd'
GO


CREATE PROCEDURE spu_Risk_Type_RI_Model_Usage_upd
    @risk_type_id int,
    @ri_band int,
    @ri_model_id int,
    @description varchar(255),
    @is_deleted int,
    @effective_date datetime,
    @expiry_date datetime,
	@portfolio_transfer_from_cnt int,
	-- Primary key:
    @risk_type_ri_model_usage_cnt int,
	@UserId int = NULL,
	@UniqueId varchar(50) = NUll,
	@ScreenHierarchy varchar(500) = NUll
AS

    UPDATE	Risk_Type_RI_Model_Usage
    SET		risk_type_id = @risk_type_id,
    		ri_band = @ri_band,
    		ri_model_id = @ri_model_id,
        	description = @description,
        	is_deleted = @is_deleted,
        	effective_date = @effective_date,
        	expiry_date = @expiry_date,
    		portfolio_transfer_from_cnt = @portfolio_transfer_from_cnt,
			UserId = @UserId,
			UniqueId = @UniqueId,
			ScreenHierarchy = @ScreenHierarchy
    WHERE
    		risk_type_ri_model_usage_cnt = @risk_type_ri_model_usage_cnt

GO



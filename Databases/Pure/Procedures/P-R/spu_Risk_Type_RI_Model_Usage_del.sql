SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_RiskType_RI_Model_Usage_del'
EXECUTE DDLDropProcedure 'spu_Risk_Type_RI_Model_Usage_del'
GO


CREATE PROCEDURE spu_Risk_Type_RI_Model_Usage_del
    @risk_type_ri_model_usage_cnt int,
	@UserId int = NULL,
	@UniqueId varchar(50) = NUll,
	@ScreenHierarchy varchar(500) = NUll
AS

IF @UniqueId IS NOT NULL
BEGIN
	UPDATE rtrma SET UserId = @UserId, UniqueId = @UniqueId, 
			 ScreenHierarchy = @ScreenHierarchy + '(' + (SELECT description FROM RI_Model WHERE ri_model_id = rtrma.ri_model_id) + ')'
			 FROM Risk_Type_RI_Model_Usage rtrma
			 WHERE risk_type_ri_model_usage_cnt = @risk_type_ri_model_usage_cnt
END

    DELETE	Risk_Type_RI_Model_Usage
    WHERE	risk_type_ri_model_usage_cnt = @risk_type_ri_model_usage_cnt

GO



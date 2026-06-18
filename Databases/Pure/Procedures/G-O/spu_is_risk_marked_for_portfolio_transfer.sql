SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_is_risk_marked_for_portfolio_transfer'
GO


CREATE PROCEDURE spu_is_risk_marked_for_portfolio_transfer
	@risk_cnt int,
	@transfer_date datetime
AS

    -- Check if the given risk is against a replaced RI model
    If Exists (Select *
                From    risk r
                Join    ri_arrangement ra 
                        On ra.risk_cnt = r.risk_cnt
			    Join    risk_type_ri_model_usage u 
                        On u.risk_type_id = r.risk_type_id 
                        And u.ri_band = ra.ri_band_id 
                        And u.ri_model_id = ra.ri_model_id
			    Left Join
                        risk_type_ri_model_usage u2 
                        On u2.portfolio_transfer_from_cnt = u.risk_type_ri_model_usage_cnt
            	Where   r.risk_cnt = @risk_cnt
        		And     u2.portfolio_transfer_from_cnt is not null -- Risk is against a replaced RI model
			    And     IsNull(u.expiry_date,'29-Dec-1899') <= @transfer_date
    			And     ra.original_flag = 0 -- "live" RI model
    			And     u.is_deleted = 0
    			And     u2.is_deleted = 0)
        Select '1'
    Else
        Select '0'


Go


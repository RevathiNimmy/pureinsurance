SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Risk_sel'
GO
/*************************************************************************/
/* ERWIN generated select all fields for a given key                 */
/*************************************************************************/
/*************************************************************************/
/* 1.0  07/07/97  RFC  Original (Based on Original by SP)        */
/*************************************************************************/
CREATE PROCEDURE spe_Risk_sel
    @risk_cnt int,
    @isDeferredStatusRequired int =0
AS

DECLARE @RiskStatusId int

  If Exists ( Select  1
                From    ri_arrangement ra
                Join    ri_model rm ON rm.ri_model_id = ra.ri_model_id
                Where   ra.risk_cnt = @risk_cnt
                And     ra.original_flag = 0 -- only new records!
                And     rm.ri_model_type = 2) -- deferred
				And @isDeFerredStatusRequired =1
        -- Set risk status
        Update  Risk
        Set     risk_status_id = (Select risk_status_id
                                    From risk_status
                                    Where code = 'RIDEFERRED')
        Where   risk_cnt = @risk_cnt
	Begin
		SELECT @RiskStatusId =9
	END

SELECT
    risk_cnt,
    risk_status_id,
    risk_folder_cnt,
    accumulation_id,
    risk_type_id,
    description,
    sequence_number,
    sum_insured_requested,
    inception_date,
    expiry_date,
    is_not_index_linked,
    is_accumulated,
    lapsed_reason_id,
    lapsed_date,
    lapsed_description,
    var_data_ref,
    total_sum_insured,
    total_annual_premium,
    total_this_premium,
    is_ri_at_risk_level,
    is_auto_reinsured,
    gis_screen_id,
    eml_percentage,
    risk_number,
    variation_number,
    is_risk_selected,
    coverage,
    insured_item,
    extensions
 FROM Risk

WHERE risk_cnt = @risk_cnt

GO


SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Risk_upd'
GO

CREATE PROCEDURE spe_Risk_upd
    @risk_cnt int,
    @risk_status_id int,
    @risk_folder_cnt int,
    @accumulation_id int,
    @risk_type_id int,
    @description varchar(255),
    @sequence_number int,
    @sum_insured_requested numeric(19,4),
    @inception_date datetime,
    @expiry_date datetime,
    @is_not_index_linked tinyint,
    @is_accumulated tinyint,
    @lapsed_reason_id int,
    @lapsed_date datetime,
    @lapsed_description varchar(255),
    @var_data_ref int,
    @total_sum_insured numeric(19,4),
    @total_annual_premium numeric(19,4),
    @total_this_premium numeric(19,4),
    @is_ri_at_risk_level tinyint,
    @is_auto_reinsured tinyint,
    @gis_screen_id int,
    @eml_percentage float,
    @risk_number integer = NULL,
    @variation_number integer = NULL,
    @is_risk_selected tinyint = NULL,
    @coverage varchar(255) = NULL,
    @insured_item varchar(255) = NULL,
    @extensions varchar(255) = NULL

AS
BEGIN
IF @accumulation_id = 0
	SELECT @accumulation_id = NULL

IF ISNULL(@inception_date, 0) = 0
	SELECT @inception_date = getdate()

UPDATE Risk
    SET
    risk_status_id=@risk_status_id,
    risk_folder_cnt=@risk_folder_cnt,
    accumulation_id=@accumulation_id,
    risk_type_id=@risk_type_id,
    description=@description,
    sequence_number=@sequence_number,
    sum_insured_requested=@sum_insured_requested,
    inception_date=@inception_date,
    expiry_date=@expiry_date,
    is_not_index_linked=@is_not_index_linked,
    is_accumulated=@is_accumulated,
    lapsed_reason_id=@lapsed_reason_id,
    lapsed_date=@lapsed_date,
    lapsed_description=@lapsed_description,
    var_data_ref=@var_data_ref,
    total_sum_insured=@total_sum_insured,
    total_annual_premium=@total_annual_premium,
    total_this_premium=@total_this_premium,
    is_ri_at_risk_level=@is_ri_at_risk_level,
    is_auto_reinsured=@is_auto_reinsured,
    gis_screen_id=@gis_screen_id,
    eml_percentage=@eml_percentage,
    risk_number=@risk_number,
    variation_number=@variation_number,
    is_risk_selected=@is_risk_selected,
    coverage = @coverage,
    insured_item = @insured_item,
    extensions = @extensions

WHERE  risk_cnt = @risk_cnt

END

GO


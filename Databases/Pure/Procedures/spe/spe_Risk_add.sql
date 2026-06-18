SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Risk_add'
GO
/*************************************************************************/
/* ERWIN generated add record and return Identity column     */
/*************************************************************************/
/*************************************************************************/
/* 1.0  06/08/1997 RFC Original (Based on SP Original)           */
-- PW011002 - add coverage, insured item, extensions and risk number
/*************************************************************************/
CREATE PROCEDURE spe_Risk_add
    @risk_cnt int OUTPUT ,
    @risk_status_id int ,
    @risk_folder_cnt int ,
    @accumulation_id int ,
    @risk_type_id int ,
    @description varchar(255) ,
    @sequence_number int ,
    @sum_insured_requested numeric(19,4) ,
    @inception_date datetime,
    @expiry_date datetime ,
    @is_not_index_linked tinyint ,
    @is_accumulated tinyint ,
    @lapsed_reason_id int ,
    @lapsed_date datetime ,
    @lapsed_description varchar(255) ,
    @var_data_ref int ,
    @total_sum_insured numeric(19,4) ,
    @total_annual_premium numeric(19,4) ,
    @total_this_premium numeric(19,4) ,
    @is_ri_at_risk_level tinyint ,
    @is_auto_reinsured tinyint ,
    @gis_screen_id int ,
    @eml_percentage float,
    @risk_number integer = NULL,
    @variation_number integer = NULL,
    @is_risk_selected tinyint = NULL,
    @coverage varchar(255) = NULL,
    @insured_item varchar(255) = NULL,
    @extensions varchar(255) = NULL, 
    @premium_this_year numeric(19,4) = NULL,
    @is_mandatory_risk TINYINT = NULL	

AS

BEGIN

IF ISNULL(@inception_date, 0) = 0
	SELECT @inception_date = getdate()

INSERT INTO Risk (
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
    extensions,
    premium_this_year,
	is_mandatory_risk)
VALUES (
    @risk_status_id,
    @risk_folder_cnt,
    @accumulation_id,
    @risk_type_id,
    @description,
    @sequence_number,
    @sum_insured_requested,
    @inception_date,
    @expiry_date,
    @is_not_index_linked,
    @is_accumulated,
    @lapsed_reason_id,
    @lapsed_date,
    @lapsed_description,
    @var_data_ref,
    @total_sum_insured,
    @total_annual_premium,
    @total_this_premium,
    @is_ri_at_risk_level,
    @is_auto_reinsured,
    @gis_screen_id,
    @eml_percentage,
    @risk_number,
    @variation_number,
    @is_risk_selected,
    @coverage,
    @insured_item,
    @extensions,
    @premium_this_year,
	@is_mandatory_risk)
END

BEGIN
SELECT @risk_cnt = @@IDENTITY
END

GO



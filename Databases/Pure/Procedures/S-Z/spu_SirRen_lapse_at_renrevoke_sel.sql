SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_SirRen_lapse_at_renrevoke_sel'
GO

/* If a new policy has been create at confirmation this will need to be lapsed
   so return lapse_required = 1 if this is the case
   Also return the rest of the data required to do a lapse */
CREATE PROCEDURE spu_SirRen_lapse_at_renrevoke_sel
    @insurance_folder_cnt int
AS
BEGIN
	DECLARE @renewal_insurance_folder_cnt INT
	DECLARE @lapse_required TINYINT
	DECLARE @gis_data_model_id INT
	
	SELECT @lapse_required = 0

	--Get the insurance_folder_cnt of the renewal record
	SELECT @renewal_insurance_folder_cnt = i.insurance_folder_cnt
	FROM renewal_control rc
	INNER JOIN insurance_file i ON i.insurance_file_cnt = rc.renewal_insurance_file_cnt
	WHERE rc.insurance_folder_cnt =  @insurance_folder_cnt

	SELECT @gis_data_model_id = gis_data_model_id
	FROM renewal_control 
	WHERE insurance_folder_cnt =  @insurance_folder_cnt
	
	--If the renewal insurance_folder_cnt is different from the original then we will
	--need to lapse the record we have create
	SELECT @lapse_required = 0
	if (@renewal_insurance_folder_cnt <> @insurance_folder_cnt AND @gis_data_model_id IN (1,2,3))
	    SELECT @lapse_required = 1

	--Return the lapse_required, gis data model code, gis business type code and
	--renewal insurance file cnt
	SELECT @lapse_required as lapse_required, gdm.code as gis_data_model_code , gbt.code as gis_business_type_code,
	       rc.renewal_insurance_file_cnt
	FROM gis_qem_usage gqu
	INNER JOIN renewal_control rc ON rc.renewal_gis_scheme_id = gqu.gis_scheme_id AND rc.gis_data_model_id = gqu.gis_data_model_id
	INNER JOIN gis_business_type gbt ON gbt.gis_business_type_id = gqu.gis_business_type_id
	INNER JOIN gis_data_model gdm ON gdm.gis_data_model_id = rc.gis_data_model_id
	WHERE insurance_folder_cnt = @insurance_folder_cnt		

END

GO
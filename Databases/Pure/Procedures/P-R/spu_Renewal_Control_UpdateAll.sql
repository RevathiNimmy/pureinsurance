SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Renewal_Control_UpdateAll'
GO


CREATE PROCEDURE spu_Renewal_Control_UpdateAll
    @insurance_folder_cnt int,
    @product_id int,
    @renewal_insurance_file_cnt int,
    @renewal_status_type_code char(10),
    @suspension_level int,
    @renewal_gis_scheme_id int,
    @renewal_date datetime,
    @gis_scheme_id int,
    @party_cnt int,
    @risk_code_id int,
    @gis_data_model_id int,
    @renewal_edi_audit_id int,
    @old_insurance_file_cnt int = NULL,
    @offer_alt smallint = NULL,
    @Reset_Flag tinyint = NULL
AS

/* Amended: 23/07/01 IJM - New param to update the insurance folder cnt */
DECLARE @renewal_status_type_id int

/*
    CTAF 25/05/2001

    I've done it this way because DirectUpdate is sometimes called without all the parameters.
    On these occasions, the missing columns were set to NULL.
*/

/* Find the renewal_status_type_id from the code */

/* AK 161101 - getting rid of these million update statements */
IF (@renewal_status_type_code IS NOT NULL)
BEGIN

    SELECT @renewal_status_type_id = renewal_status_type_id
    FROM renewal_status_type
    WHERE code = @renewal_status_type_code
END

    UPDATE Renewal_Control
    SET renewal_status_type_id = isnull(@renewal_status_type_id, renewal_status_type_id),
    product_id = isnull(@product_id, product_id),
    renewal_insurance_file_cnt = isnull(@renewal_insurance_file_cnt, renewal_insurance_file_cnt),
    suspension_level = isnull(@suspension_level, suspension_level),
    renewal_gis_scheme_id = isnull(@renewal_gis_scheme_id, renewal_gis_scheme_id),
    renewal_date = isnull(@renewal_date, renewal_date),
    gis_scheme_id = isnull(@gis_scheme_id, gis_scheme_id),
    party_cnt = isnull(@party_cnt, party_cnt),
    risk_code_id = isnull(@risk_code_id, risk_code_id),
    gis_data_model_id = isnull(@gis_data_model_id, gis_data_model_id),
    renewal_edi_audit_id = isnull(@renewal_edi_audit_id, renewal_edi_audit_id),
    old_insurance_file_cnt = isnull(@old_insurance_file_cnt, old_insurance_file_cnt),
    offer_alt = isnull(@offer_alt, offer_alt),
    Reset_Flag = isnull(@Reset_Flag, Reset_Flag)
    WHERE insurance_folder_cnt = @insurance_folder_cnt
GO



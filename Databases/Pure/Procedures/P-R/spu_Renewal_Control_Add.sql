SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Renewal_Control_Add'
GO


CREATE PROCEDURE spu_Renewal_Control_Add
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
    @old_insurance_file_cnt int,
    @Offer_Alt smallint,
    @Reset_Flag tinyint = NULL

AS


DECLARE @renewal_status_type_id int

/* Find the renewal_status_type_id from the code */
SELECT @renewal_status_type_id = renewal_status_type_id
FROM renewal_status_type
WHERE code = @renewal_status_type_code

/*Insert the row*/
INSERT INTO Renewal_Control (insurance_folder_cnt,
                product_id,
                renewal_insurance_file_cnt,
                renewal_status_type_id,
                suspension_level,
                renewal_gis_scheme_id,
                renewal_date,
                gis_scheme_id,
                party_cnt,
                risk_code_id,
                gis_data_model_id,
                renewal_edi_audit_id,
                old_insurance_file_cnt,
                Offer_Alt,
                Reset_Flag
)
VALUES (@insurance_folder_cnt,
    @product_id,
    @renewal_insurance_file_cnt,
    @renewal_status_type_id,
    @suspension_level,
    @renewal_gis_scheme_id,
    @renewal_date,
    @gis_scheme_id,
    @party_cnt,
    @risk_code_id,
    @gis_data_model_id,
    @renewal_edi_audit_id,    
    @old_insurance_file_cnt,
    @Offer_Alt,
    @Reset_Flag)
GO



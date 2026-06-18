SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_Scheme_add_v2'
GO


CREATE PROCEDURE spu_GIS_Scheme_add_v2
    @gis_scheme_id int OUTPUT,
    @gis_business_type_id int,
    @gis_insurer_id int,
    @scheme_no int,
    @scheme_ver smallint,
    @scheme_status tinyint,
    @start_date datetime,
    @scheme_desc varchar(70),
    @priority smallint,
    @agency_code varchar(30),
    @product_code varchar(30),
    @activation_level smallint,
    @printing_privileges smallint,
    @broker_group varchar(30),
    @commision_perc decimal(5,2),
    @quote_day_num int,
    @selection_day_num int,
    @invite_day_num int,
    @confirm_day_num int,
    @lapse_day_num int,
    @max_change_num int,
    @min_change_num int,
    @expiry_date datetime,
    @qm_insurer_ref varchar(10),
    @scheme_type_flags int,
    @filename varchar(255),
    @edi_mail_box varchar(13),
    @refer_email_address varchar(255),
    @refer_fax_number varchar(20),
    @scheme_type int,
    @scheme_variant int,
    @dict_ver varchar(12),
    @class_of_business varchar(10),
    @country_id int
AS


BEGIN
INSERT INTO GIS_Scheme (
    gis_business_type_id,
    gis_insurer_id,
    filename,
    scheme_no,
    scheme_ver,
    scheme_status,
    start_date,
    scheme_desc,
    priority,
    agency_code,
    product_code,
    activation_level,
    printing_privileges,
    broker_group,
    commision_perc,
    quote_day_num,
    selection_day_num,
    invite_day_num,
    confirm_day_num,
    lapse_day_num,
    max_change_num,
    min_change_num,
    expiry_date,
    qm_insurer_ref,
    scheme_type_flags,
    edi_mail_box,
    refer_email_address,
    refer_fax_number,
    scheme_type,
    scheme_variant,
    dict_ver,
    class_of_business,
    country_id)
VALUES (
    @gis_business_type_id,
    @gis_insurer_id,
    @filename,
    @scheme_no,
    @scheme_ver,
    @scheme_status,
    @start_date,
    @scheme_desc,
    @priority,
    @agency_code,
    @product_code,
    @activation_level,
    @printing_privileges,
    @broker_group,
    @commision_perc,
    @quote_day_num,
    @selection_day_num,
    @invite_day_num,
    @confirm_day_num,
    @lapse_day_num,
    @max_change_num,
    @min_change_num,
    @expiry_date,
    @qm_insurer_ref,
    @scheme_type_flags,
    @edi_mail_box,
    @refer_email_address,
    @refer_fax_number,
    @scheme_type,
    @scheme_variant,
    @dict_ver,
    @class_of_business,
    @country_id)
END

BEGIN
SELECT @gis_scheme_id = @@IDENTITY
END
GO



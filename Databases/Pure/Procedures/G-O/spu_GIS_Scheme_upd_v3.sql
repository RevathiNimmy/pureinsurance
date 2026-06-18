SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GIS_Scheme_upd_v3'
GO


CREATE PROCEDURE spu_GIS_Scheme_upd_v3
    @gis_scheme_id int,
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
    @commision_perc decimal(5, 2),
    @selection_day_num int,
    @quote_day_num int,
    @invite_day_num int,
    @confirm_day_num int,
    @lapse_day_num int,
    @min_change_num int,
    @max_change_num int,
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
    @country_id int,
    @pre_selection_day_num int,
    @reminder_day_num int
AS


BEGIN

UPDATE GIS_Scheme
    SET
    gis_business_type_id=@gis_business_type_id,
    gis_insurer_id=@gis_insurer_id,
    filename=@filename,
    scheme_no=@scheme_no,
    scheme_ver=@scheme_ver,
    scheme_status=@scheme_status,
    start_date=@start_date,
    scheme_desc=@scheme_desc,
    priority=@priority,
    agency_code=@agency_code,
    product_code=@product_code,
    activation_level=@activation_level,
    printing_privileges=@printing_privileges,
    broker_group=@broker_group,
    commision_perc=@commision_perc,
    pre_selection_day_num=@pre_selection_day_num,
    selection_day_num=@selection_day_num,
    quote_day_num=@quote_day_num,
    invite_day_num=@invite_day_num,
    reminder_day_num = @reminder_day_num,
    confirm_day_num=@confirm_day_num,
    lapse_day_num=@lapse_day_num,
    min_change_num=@min_change_num,
    max_change_num=@max_change_num,
    expiry_date=@expiry_date,
    qm_insurer_ref=@qm_insurer_ref,
    scheme_type_flags=@scheme_type_flags,
    edi_mail_box=@edi_mail_box,
    refer_email_address=@refer_email_address,
    refer_fax_number=@refer_fax_number,
    scheme_type=@scheme_type,
    scheme_variant=@scheme_variant,
    dict_ver=@dict_ver,
    class_of_business=@class_of_business,
    country_id=@country_id

WHERE gis_scheme_id = @gis_scheme_id

END
GO



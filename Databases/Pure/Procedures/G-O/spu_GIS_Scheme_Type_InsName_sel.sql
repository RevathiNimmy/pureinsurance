SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GIS_Scheme_Type_InsName_sel'
GO


CREATE PROCEDURE spu_GIS_Scheme_Type_InsName_sel
    @gis_business_type_id int
AS


SELECT
    gs.gis_scheme_id,
    gs.gis_quote_engine_id,
    gs.gis_business_type_id,
    gi.description,
    gs.scheme_no,
    gs.scheme_ver,
    gs.scheme_status,
    gs.start_date,
    gs.scheme_desc,
    gs.priority,
    gs.agency_code,
    gs.product_code,
    gs.activation_level,
    gs.printing_privileges,
    gs.broker_group,
    gs.commision_perc,
    gs.quote_day_num,
    gs.selection_day_num,
    gs.invite_day_num,
    gs.confirm_day_num,
    gs.lapse_day_num,
    gs.max_change_num,
    gs.min_change_num,
    gs.expiry_date,
    gs.qm_insurer_ref,
    gs.scheme_type_flags,
    gs.filename,
    gs.edi_mail_box,
    gs.refer_email_address,
    gs.refer_fax_number,
    gs.scheme_type,
    gs.scheme_variant,
    gi.gis_insurer_id,
    gi.code,
    gi.polaris_insurer_no
 FROM GIS_Scheme gs,
      GIS_Insurer gi
WHERE gs.gis_insurer_id = gi.gis_insurer_id and
      gs.scheme_status = 1 and
      gs.start_date <= GETDATE() and
      gs.gis_business_type_id = @gis_business_type_id
GO



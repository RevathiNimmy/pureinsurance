SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_Insurers_Type_sel'
GO


CREATE PROCEDURE spu_GIS_Insurers_Type_sel
    @gis_business_type_id int
AS


SELECT DISTINCT
        i.gis_insurer_id,
        i.code,
        i.caption_id,
        i.description,
        i.effective_date,
        i.method,
        i.icr,
        i.polaris_insurer_no,
--BSJ 23/05/00 - removed mail_box as column no longer exists
--        i.mail_box,
--
        i.abi_1_edi_directory,
        i.abi_81_insurer
      FROM GIS_Scheme s, GIS_Insurer i
     WHERE i.gis_insurer_id = s.gis_insurer_id and
           i.is_deleted = 0 and
           s.gis_business_type_id = @gis_business_type_id
GO



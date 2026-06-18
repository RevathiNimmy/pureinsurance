SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_user_def_detail_rat_saa'
GO


CREATE PROCEDURE spu_GIS_user_def_detail_rat_saa
    @GIS_user_def_detail_id INT
AS


select  hr.GIS_user_def_header_rates_id,
    hr.code,
    hr.description,
    dr.value
from    GIS_user_def_header_rates hr,
    GIS_user_def_detail ld,
    GIS_user_def_detail_rates dr
where   dr.GIS_user_def_detail_id = @GIS_user_def_detail_id
and ld.GIS_user_def_header_id = hr.GIS_user_def_header_id
and dr.GIS_user_def_detail_id = ld.GIS_user_def_detail_id
and dr.GIS_user_def_detail_rates_id = hr.GIS_user_def_header_rates_id

union

select  hr.GIS_user_def_header_rates_id,
    hr.code,
    hr.description,
    0
from    GIS_user_def_header_rates hr,
    GIS_user_def_detail ld
where   ld.GIS_user_def_detail_id = @GIS_user_def_detail_id
and hr.GIS_user_def_header_id = ld.GIS_user_def_header_id
and hr.GIS_user_def_header_rates_id not in (
    select  GIS_user_def_detail_rates_id
    from    GIS_user_def_detail_rates dr
    where   ld.GIS_user_def_detail_id = @GIS_user_def_detail_id)
GO



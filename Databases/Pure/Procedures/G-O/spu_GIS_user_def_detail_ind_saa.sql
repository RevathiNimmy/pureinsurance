SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_user_def_detail_ind_saa'
GO


CREATE PROCEDURE spu_GIS_user_def_detail_ind_saa
    @GIS_user_def_detail_id INT
AS


select  hi.GIS_user_def_header_inds_id,
    hi.code,
    hi.description,
    di.value
from    GIS_user_def_header_inds hi,
    GIS_user_def_detail ld,
    GIS_user_def_detail_inds di
where   di.GIS_user_def_detail_id = @GIS_user_def_detail_id
and ld.GIS_user_def_header_id = hi.GIS_user_def_header_id
and di.GIS_user_def_detail_id = ld.GIS_user_def_detail_id
and di.GIS_user_def_detail_inds_id = hi.GIS_user_def_header_inds_id

union

select  hi.GIS_user_def_header_inds_id,
    hi.code,
    hi.description,
    ""
from    GIS_user_def_header_inds hi,
    GIS_user_def_detail ld
where   ld.GIS_user_def_detail_id = @GIS_user_def_detail_id
and hi.GIS_user_def_header_id = ld.GIS_user_def_header_id
and hi.GIS_user_def_header_inds_id not in (
    select  GIS_user_def_detail_inds_id
    from    GIS_user_def_detail_inds di
    where   di.GIS_user_def_detail_id = @GIS_user_def_detail_id)
GO



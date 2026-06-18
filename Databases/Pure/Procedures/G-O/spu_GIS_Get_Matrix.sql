EXECUTE DDLDropProcedure 'spu_GIS_Get_Matrix'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO


CREATE PROCEDURE spu_GIS_Get_Matrix

@SchemeID int,
@RateType varchar(70),
@ZGroup varchar(30)

 AS

declare @zgroupid int

--2d or 1d
IF @zgroup=''
    BEGIN

        select i.lookup1,i.lookup2,i.lookup3,i.rate  from gis_rate_items i
        inner join gis_rate_type t
        on i.gis_rate_type_id=t.gis_rate_type_id
        where t.gis_scheme_id=@schemeid
        and t.description=@rateType
        order by lookup1,lookup2

    END

ELSE
--3d limited by  Z axis
    BEGIN
        --get zgroupid

        Select @zgroupid=gis_list_grouping_id
        from gis_list_grouping g
        inner join gis_rate_type t
        on g.gis_list_type_id=t.gis_list_type_lookup3
        where t.gis_scheme_id=@schemeid
        and t.description=@ratetype
        and g.description=@zgroup
        and g.gis_scheme_id = @schemeid
        and g.is_deleted = 0

        Select i.lookup1,i.lookup2,i.lookup3,i.rate  from gis_rate_items i
        inner join gis_rate_type t
        on i.gis_rate_type_id=t.gis_rate_type_id
        where t.gis_scheme_id=@schemeid
        and t.description=@rateType
        and lookup3=@zgroupid
        order by lookup1,lookup2
    END
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO

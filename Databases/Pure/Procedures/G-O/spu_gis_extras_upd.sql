SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_gis_extras_upd'
GO

CREATE PROCEDURE spu_gis_extras_upd
    @gis_scheme_id int,
    @Party_cnt int,
    @gis_scheme_extra_id int
AS

BEGIN
    select @gis_scheme_extra_id = isnull(@gis_scheme_extra_id,0)

    if @gis_scheme_extra_id = 0
    Begin
        insert into gis_scheme_extras(party_cnt, gis_scheme_id)
        values(@party_cnt, @gis_scheme_id)
    END
    ELSE
    BEGIN
        UPDATE GIS_Scheme_extras
        SET
           party_cnt=@party_cnt,
           gis_scheme_id=@gis_scheme_id
        WHERE gis_scheme_extra_id = @gis_scheme_extra_id
    END
END
GO

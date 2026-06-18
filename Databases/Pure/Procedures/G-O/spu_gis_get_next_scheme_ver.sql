SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_gis_get_next_scheme_ver'
GO

CREATE PROCEDURE spu_gis_get_next_scheme_ver
    @gis_insurer_id   integer,
    @scheme_no        integer,
    @scheme_ver       integer    OUTPUT
    			
AS

SELECT  @scheme_ver = ISNULL(MAX(scheme_ver), 0)
FROM    Gis_Scheme 
WHERE   gis_insurer_id = @gis_insurer_id
AND     scheme_no = @scheme_no


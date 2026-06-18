-- Retrieves list of gis scheme extras, for selected gis_scheme_id.

SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_gis_extras_sel'
GO

CREATE PROCEDURE spu_gis_extras_sel
    @gis_scheme_id integer
AS

SELECT 
   gis_scheme_extras.gis_scheme_extra_id,
   type = case party_type.code
            when 'FE' THEN 'Fee'
            when 'EX' THEN 'Extra'
          END,
   party.Shortname,
   gis_scheme_extras.Party_cnt
FROM 
	GIS_Scheme_extras
	join party on party.party_cnt = gis_scheme_extras.party_cnt
	join party_type on party_type.party_type_id = party.party_type_id
WHERE gis_scheme_id = @gis_scheme_id

GO

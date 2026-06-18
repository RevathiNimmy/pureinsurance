SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_SAM_CheckNexusPolicy'
GO

CREATE PROCEDURE spu_SAM_CheckNexusPolicy
	@Insurance_File_Cnt INT
AS
	select 
		isdate(gs.onlinestartdate) is_nexus 
	from 
		gis_policy_link gpl
		left join gis_scheme gs on gs.gis_scheme_id = gpl.gis_scheme_id
	where 
		gpl.insurance_file_cnt=@Insurance_File_Cnt



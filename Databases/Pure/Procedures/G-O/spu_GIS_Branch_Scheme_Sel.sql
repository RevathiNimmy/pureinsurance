-- AK22072004: Create spu_GIS_Branch_Scheme_Sel.sql
-- Retrieves branch usage data for GII schemes

SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_GIS_Branch_Scheme_Sel'
GO

CREATE PROCEDURE spu_GIS_Branch_Scheme_Sel
	@SchemeID integer
AS

    SELECT b.gis_scheme_id,
           b.source_id,
           s.code,
           s.[description] AS branch_name,
           b.pm_company_number, 
           b.agency_code,
           b.relationship_status,
	   b.edi_mail_box
      FROM GIS_Branch_Scheme b 
INNER JOIN Source s ON s.source_id = b.source_id 
     WHERE b.gis_scheme_id = @SchemeID
       AND s.is_deleted = 0
  ORDER BY b.source_id

GO

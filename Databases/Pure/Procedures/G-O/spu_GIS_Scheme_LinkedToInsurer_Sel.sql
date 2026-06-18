-- JRD01102004: Create spu_GIS_Scheme_LinkedToInsurer_Sel.sql
-- Retrieves GII Schemes linked to a particular GII Scheme by the InsurerID
-- JOINing on the GIS_Branch_Scheme table

-- Idea is to supply a SchemeID for an existing Branch_Scheme record and retrive all linked
-- SchemeID's

SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_GIS_Scheme_LinkedToInsurer_Sel'
GO

CREATE PROCEDURE spu_GIS_Scheme_LinkedToInsurer_Sel
	@SchemeID integer,
	@SourceID integer,
	@ClassOfBusiness varchar(10),
	@PMCompanyNumber integer,
	@CountryID integer
AS
BEGIN
	SELECT gs2.gis_scheme_id FROM gis_scheme gs1
--	INNER JOIN gis_scheme gs2 ON gs1.edi_mail_box = gs2.edi_mail_box
	INNER JOIN gis_scheme gs2 ON gs1.gis_insurer_id = gs2.gis_insurer_id
	INNER JOIN gis_branch_scheme gbs ON gbs.gis_scheme_id = gs1.gis_scheme_id
	WHERE gs1.gis_scheme_id = @SchemeID
	AND gs2.gis_scheme_id <> gs1.gis_scheme_id
	AND gbs.source_id = @SourceID
	AND gs2.class_of_business = @ClassOfBusiness
	AND gbs.pm_company_number = @PMCompanyNumber
	AND gs2.country_id = @CountryID
END
GO
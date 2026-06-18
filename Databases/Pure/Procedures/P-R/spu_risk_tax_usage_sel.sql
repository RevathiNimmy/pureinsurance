SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_risk_tax_usage_sel'
GO

CREATE PROCEDURE spu_risk_tax_usage_sel
    	@Risk_Code_id int
AS

IF (@Risk_Code_id=-1)
BEGIN
	SELECT

	DISTINCT RTU.COB_Rating_Section_id,
	CRS.description,
	RTU.Tax_Group_Id,
	TG.description
	FROM Risk_Tax_Usage RTU
	LEFT OUTER JOIN COB_Rating_Section CRS ON CRS.COB_Rating_Section_Id = RTU.COB_Rating_Section_Id
	LEFT OUTER JOIN Tax_Group TG ON TG.tax_group_id = RTU.Tax_Group_id
END
ELSE
BEGIN
	SELECT
	
	RTU.COB_Rating_Section_id,
	CRS.description,
	RTU.Tax_Group_Id,
	TG.description
	FROM Risk_Tax_Usage RTU
	LEFT OUTER JOIN COB_Rating_Section CRS ON CRS.COB_Rating_Section_Id = RTU.COB_Rating_Section_Id
	LEFT OUTER JOIN Tax_Group TG ON TG.tax_group_id = RTU.Tax_Group_id
	WHERE RTU.risk_code_id = @Risk_Code_id
END

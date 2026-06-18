SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Risk_Sections_sel'
GO

CREATE PROCEDURE spu_Risk_sections_sel
    @risk_code_id int,
    @language_id integer = 1

AS
    CREATE TABLE #TempRiskSection
    (
     COB_Rating_Section_ID integer NULL,
     COB_Code CHAR(10),
     COB_Description varchar(255),
     Is_Selected tinyint,
     Tax_group_id integer,
     Tax_group_Description varchar(255)
    )
INSERT INTO #TempRiskSection
	 SELECT NULL,
	'N/A',
	'Not Applicable',
	0,
	NULL,
	'Not Applicable'

INSERT INTO #TempRiskSection
	SELECT
	COB_Rating_Section_id, 
	Code,
	ISNULL(pmc.caption,description) as description,
	0,
	NULL,
	'Not Applicable'
	FROM COB_Rating_Section COB
    left outer join pmcaption pmc on (pmc.caption_id=cob.caption_id AND pmc.language_id=@language_id)
	WHERE effective_date <= getdate()


UPDATE TRS
	Set TRS.Is_Selected = 1,
	    TRS.Tax_Group_id = RTU.Tax_Group_id
FROM #TempRiskSection TRS
JOIN Risk_Tax_Usage RTU ON RTU.COB_Rating_Section_Id = TRS.COB_Rating_Section_Id

WHERE RTU.risk_code_id = @risk_code_id

IF EXISTS (SELECT * FROM Risk_Tax_Usage 
		WHERE risk_code_id = @risk_code_id
		AND COB_Rating_Section_id IS NULL)
BEGIN 	
	UPDATE #TempRiskSection
	SET is_selected = 1,
	tax_group_id = (SELECT Tax_group_id FROM Risk_Tax_Usage 
			WHERE risk_code_id = @risk_code_id
			AND COB_Rating_Section_id IS NULL)
	WHERE COB_Rating_Section_Id IS NULL
END	
UPDATE TRS
	Set TRS.Tax_Group_Description = TG.Description
FROM #TempRiskSection TRS
JOIN Tax_Group TG ON TG.tax_group_id = TRS.tax_group_id


select * from #TempRiskSection
DROP TABLE #TempRiskSection
GO
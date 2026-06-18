SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_CLM_Get_Valid_Primary_Causes'
GO

CREATE  PROCEDURE spu_CLM_Get_Valid_Primary_Causes
    @ins_file_cnt int,
	@mode int=1,
	@bIncludeDeleted bit=0
AS
BEGIN

--In View mode all Causes should be populated
IF (@mode<>0)
BEGIN
	SELECT 
		pc.primary_cause_id,
		pmc.caption,
		pc.code

	FROM Insurance_File AS ins

	INNER JOIN 
		Product AS pro ON ins.product_id = pro.product_id 

	INNER JOIN 
		Product_Risk_Type_Group AS prtg ON ins.product_id = prtg.product_id 

	INNER JOIN 
		Risk_Type_Group AS rtg ON prtg.risk_type_group_id = rtg.risk_type_group_id 

	INNER JOIN 
		Primary_Cause_Risk_Type_Group AS pcrtg ON rtg.risk_type_group_id = pcrtg.risk_type_group_id 

	INNER JOIN 
		Primary_Cause AS pc ON pcrtg.primary_cause_id = pc.primary_cause_id 

	INNER JOIN
		PMCaption AS pmc ON pc.caption_id = pmc.caption_id

	WHERE ins.insurance_file_cnt = @ins_file_cnt 
	-- Tracy Richards 07/08/03 - only used by openClaim, so make sure deleted Primary 
	--Causes are filtered out
		AND (pc.Is_deleted = 0 Or pc.Is_deleted = @bIncludeDeleted)
	ORDER BY pmc.caption ASC
END
ELSE
BEGIN
	SELECT 
		pc.primary_cause_id,
		pmc.caption,
		pc.code

	FROM Insurance_File AS ins

	INNER JOIN 
		Product AS pro ON ins.product_id = pro.product_id 

	INNER JOIN 
		Product_Risk_Type_Group AS prtg ON ins.product_id = prtg.product_id 

	INNER JOIN 
		Risk_Type_Group AS rtg ON prtg.risk_type_group_id = rtg.risk_type_group_id 

	INNER JOIN 
		Primary_Cause_Risk_Type_Group AS pcrtg ON rtg.risk_type_group_id = pcrtg.risk_type_group_id 

	INNER JOIN 
		Primary_Cause AS pc ON pcrtg.primary_cause_id = pc.primary_cause_id 

	INNER JOIN
		PMCaption AS pmc ON pc.caption_id = pmc.caption_id

	WHERE ins.insurance_file_cnt = @ins_file_cnt 

	ORDER BY pmc.caption ASC

END
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


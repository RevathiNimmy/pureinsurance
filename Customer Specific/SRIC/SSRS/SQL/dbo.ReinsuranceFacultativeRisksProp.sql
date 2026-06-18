Create PROCEDURE [dbo].[ReinsuranceFacultativeRisksProp]
	@insurance_file_cnt	INT
AS

	SELECT		dbo.RI_Arrangement.risk_cnt,
				dbo.RI_Arrangement.ri_band_id
	FROM 		dbo.RI_Arrangement	
	INNER JOIN	dbo.Risk ON dbo.Risk.risk_cnt = dbo.RI_Arrangement.risk_cnt
	INNER JOIN	dbo.Insurance_File_Risk_Link	ON dbo.Insurance_File_Risk_Link.risk_cnt = dbo.Risk.risk_cnt
	WHERE		EXISTS(SELECT ri_arrangement_id FROM dbo.RI_Arrangement_Line  WHERE dbo.RI_Arrangement_Line.ri_arrangement_id = dbo.RI_Arrangement.ri_arrangement_id AND dbo.RI_Arrangement_Line.type IN ('F')) 
	 			AND dbo.Insurance_File_Risk_Link.insurance_file_cnt = @insurance_file_cnt
	 			AND dbo.RI_Arrangement.original_flag = 0

GO



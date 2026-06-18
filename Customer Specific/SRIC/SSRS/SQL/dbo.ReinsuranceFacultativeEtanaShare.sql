CREATE PROCEDURE [dbo].[ReinsuranceFacultativeEtanaShare]
	@risk_id			INT,
	@ri_band			INT,
	@insurance_file_cnt INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT  SUM([RIAL].premium_percent) as [this_share_percent],
			SUM([RIAL].sum_insured) AS [sum_insured],
			SUM([RIAL].[premium_value]) AS [premium_value]
	FROM   insurance_file AS [IF]  
	 INNER JOIN insurance_file_risk_link AS [IFRL]  
	  ON [IFRL].[insurance_file_cnt] = [IF].[insurance_file_cnt]  
	  INNER JOIN RI_Arrangement AS [RIA]   
	  ON [RIA].[risk_cnt] = [IFRL].[risk_cnt]  
	   INNER JOIN RI_Arrangement_Line AS [RIAL]  
		ON [RIAL].[ri_arrangement_id] = [RIA].[ri_arrangement_id]  

	WHERE [RIA].[original_flag] = 0  
	AND [RIAL].[type] <>'F'  
	AND [RIA].[ri_band_id] = @ri_band  
	AND [RIA].[risk_cnt] = @risk_id  
	and [IFRL].insurance_file_cnt = @insurance_file_cnt;


END



GO



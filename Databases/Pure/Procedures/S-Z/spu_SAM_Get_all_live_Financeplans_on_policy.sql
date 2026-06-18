EXECUTE DDLDropProcedure 'spu_SAM_Get_all_live_Financeplans_on_policy'
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE spu_SAM_Get_all_live_Financeplans_on_policy  
	@Insurance_folder_cnt  Int,
	@Insurance_file_cnt	int
AS   
BEGIN

	SELECT  pfPrem_Finance_cnt,pfPrem_Finance_version     
	 FROM pfPremiumFinance p   
	 INNER Join Insurance_file I  ON I.insurance_file_cnt = p.insurance_file_cnt   
	 WHERE i.insurance_folder_cnt = @Insurance_folder_cnt and statusInd = '040' 
	 AND i.insurance_file_cnt >= @Insurance_file_cnt
	 
END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
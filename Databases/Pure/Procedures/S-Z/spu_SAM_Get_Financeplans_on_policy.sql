SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_Get_Financeplans_on_policy'
GO

CREATE PROCEDURE spu_SAM_Get_Financeplans_on_policy  
 @Insurance_file_cnt  Int  
AS  
BEGIN  
  
 SELECT  pfPrem_Finance_cnt,pfPrem_Finance_version  
  FROM pfPremiumFinance 
    WHERE insurance_file_cnt = @Insurance_file_cnt
	ORDER BY pfPrem_Finance_cnt, pfPrem_Finance_version ASC
END    
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
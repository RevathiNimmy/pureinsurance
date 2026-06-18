SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_SAM_Del_Standard_Policy_Wordings
GO
--Start (girija) - UIIC WR22 – Capture Quote Details – Policy Standard Wordings-(6.2)
CREATE PROCEDURE spu_SAM_Del_Standard_Policy_Wordings  
    @insurance_file_cnt int 
    
AS  
  
BEGIN  
DELETE FROM policy_standard_wording 
 WHERE insurance_file_cnt = @insurance_file_cnt
END  
--End (girija) - UIIC WR22 – Capture Quote Details – Policy Standard Wordings-(6.2)

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO


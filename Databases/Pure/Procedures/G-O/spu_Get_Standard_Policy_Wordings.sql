SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_Get_Standard_Policy_Wordings
GO
--Start (girija) - UIIC WR22 – Capture Quote Details – Policy Standard Wordings-(6.1)
CREATE PROCEDURE spu_Get_Standard_Policy_Wordings  
    @insurance_file_cnt int 
    
AS  
  
BEGIN  
SELECT dt.code, dt.description, dt.document_template_id
  FROM policy_standard_wording psw, document_template dt
 WHERE psw.insurance_file_cnt = @insurance_file_cnt
   AND psw.document_template_id = dt.document_template_id
 ORDER BY psw.policy_standard_wording_id
END  
--End (girija) - UIIC WR22 – Capture Quote Details – Policy Standard Wordings-(6.1)

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
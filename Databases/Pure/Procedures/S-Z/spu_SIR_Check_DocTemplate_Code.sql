SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_SIR_Check_DocTemplate_Code'
GO

CREATE PROCEDURE spu_SIR_Check_DocTemplate_Code  
    @code AS VARCHAR(10),  
    @effective_date AS DATETIME  
AS  
  
    SELECT code, effective_date  
    FROM Document_Template  
    WHERE code = @code  
    AND effective_date = @effective_date  

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


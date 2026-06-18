SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Insurance_File_Tax_Delete'
GO

CREATE PROCEDURE spu_Insurance_File_Tax_Delete  
    @tax_calculation_cnt int  
AS  
  
begin  
 DELETE FROM Tax_Calculation  
 WHERE tax_calculation_cnt = @tax_calculation_cnt  
 AND risk_CNT IS NULL
end  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

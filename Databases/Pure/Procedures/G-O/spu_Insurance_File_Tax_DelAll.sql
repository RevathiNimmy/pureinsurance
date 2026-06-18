SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Insurance_File_Tax_DelAll'
GO

CREATE PROCEDURE spu_Insurance_File_Tax_DelAll  
    @InsuranceFileCnt int  
AS  
BEGIN  
DELETE  Tax_Calculation  
FROM Tax_Calculation  
WHERE   insurance_file_cnt = @InsuranceFileCnt  AND TransType IN ('TTR','TTIF')
END



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

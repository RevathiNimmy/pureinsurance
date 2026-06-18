SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Risk_Tax_DelAll'
GO

CREATE PROCEDURE spu_Risk_Tax_DelAll  
    @RiskCnt int  
AS  
BEGIN  
DELETE  Tax_Calculation  
FROM Tax_Calculation  
WHERE   Risk_cnt=@RiskCnt AND TransType LIKE 'TTR%'
END



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

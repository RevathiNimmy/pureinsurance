SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

Execute DDLDropProcedure 'spu_SAM_Get_Insured'
GO

CREATE PROCEDURE spu_SAM_Get_Insured
     @InsuranceFileCnt int
AS

SELECT 
	insured_cnt 
FROM 
	insurance_file
WHERE 
	insurance_file_cnt = @InsuranceFileCnt

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
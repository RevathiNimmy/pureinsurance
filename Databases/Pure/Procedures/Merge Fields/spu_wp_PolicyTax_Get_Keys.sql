SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_PolicyTax_Get_Keys'
GO

CREATE PROCEDURE spu_wp_PolicyTax_Get_Keys
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

SELECT Tax_Calculation_cnt FROM Tax_Calculation WHERE Insurance_File_Cnt = @InsuranceFileCnt
AND risk_cnt IS NULL AND transtype='TTIF'

GO

SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS ON 
GO


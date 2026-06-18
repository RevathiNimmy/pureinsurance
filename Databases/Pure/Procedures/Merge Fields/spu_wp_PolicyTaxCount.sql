SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_PolicyTaxCount'
GO

CREATE PROCEDURE spu_wp_PolicyTaxCount
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

SELECT Count(1) as how_many FROM Tax_Calculation WHERE insurance_file_cnt = @InsuranceFileCnt
AND risk_cnt IS NULL AND transtype='TTIF'

GO

SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS ON 
GO


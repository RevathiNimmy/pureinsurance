SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_wp_CreditControlItem_Get_Keys'
GO

CREATE PROCEDURE spu_wp_CreditControlItem_Get_Keys
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT = NULL,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

SELECT       	0



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

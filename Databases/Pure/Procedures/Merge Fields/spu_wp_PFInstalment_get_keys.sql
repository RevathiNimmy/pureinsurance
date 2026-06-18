SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_wp_PFInstalment_get_keys'
GO


CREATE PROCEDURE spu_wp_PFInstalment_get_keys
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


DECLARE @nPFPremiumFinanceVersion INT
DECLARE @nPFPremiumFinanceCnt INT

SELECT	@nPFPremiumFinanceCnt = pfp.pfprem_finance_cnt,
		@nPFPremiumFinanceVersion = MAX(pfp.pfprem_finance_version) 
		FROM PFPremiumFinance pfp		
		WHERE pfp.Insurance_File_Cnt = @InsuranceFileCnt GROUP BY pfp.pfprem_finance_cnt

SELECT	
    InstalmentNumber
FROM	
    PFInstalments 
   
WHERE	
    PFPrem_Finance_Cnt = @nPFPremiumFinanceCnt
	AND PFPrem_Finance_Version = @nPFPremiumFinanceVersion
GO


SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

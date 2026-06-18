SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_GetSchemeDetails'
GO
CREATE PROCEDURE spe_GetSchemeDetails
		@pfprem_finance_cnt int,
		@pfprem_finance_version int
AS

SELECT	
    s.CompanyNo,
	s.SchemeNo,
	s.SchemeVersion,
	s.Party_Cnt,
	s.DataModelCode,
	s.StartDate,
	s.EndDate,
	s.PaymentMethod_cnt,
	s.SystemTag,
	s.SchemeName,
	s.SchemeDescription,
	s.QuoteableInd,
	s.BasisofCalcNew,
	s.BasisofCalcMTA,
	s.BasisofCalcRenewal,
	s.BasisofCalcCancel,
	s.BasisofPP,
	s.QuoteDocID,
	s.BankDocID,
	s.CreditDocID,
	s.InsrMailBoxNo,
	s.EdiMessageCount,
	s.NoOfInstallments,
	s.IsInHouse,
	s.ImmediateBankDetails
FROM	
    PFPremiumFinance pf,
	PFScheme s
WHERE	
    pf.pfprem_finance_cnt = @pfprem_finance_cnt
AND		
    pf.pfprem_finance_version = pfprem_finance_version
AND		
    pf.CompanyNo = s.CompanyNo
AND		
    pf.SchemeNo = s.SchemeNo
AND		
    pf.SchemeVersion = s.SchemeVersion

GO
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


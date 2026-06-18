DDLDROPPROCEDURE 'spu_pf_getsgdocurl'
GO
CREATE PROCEDURE spu_pf_getsgdocurl(
		@pf_prem_finance_cnt int)
AS
SELECT sgdocurl FROM pfpremiumfinance WHERE pfprem_finance_cnt = @pf_prem_finance_cnt

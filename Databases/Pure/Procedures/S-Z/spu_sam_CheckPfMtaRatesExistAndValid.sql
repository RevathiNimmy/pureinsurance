ddldropprocedure spu_sam_CheckPfMtaRatesExistAndValid
go

CREATE PROCEDURE spu_sam_CheckPfMtaRatesExistAndValid
    @pfprem_finance_cnt integer,
    @pfprem_finance_version integer
AS
Begin
	select 
		1
	from 
		pfpremiumfinance 
		join pfrf on pfrf.schemeno=pfpremiumfinance.schemeno
			and pfrf.schemeversion = pfpremiumfinance.schemeversion 
		join pfscheme on pfscheme.schemeno=pfpremiumfinance.schemeno
			and pfscheme.schemeversion = pfpremiumfinance.schemeversion 
	where 	
		pfpremiumfinance.pfprem_finance_cnt=@pfprem_finance_cnt and 
		pfpremiumfinance.pfprem_finance_version=@pfprem_finance_version and 
		pfrf.productfamily='MTA' and
		pfrf.startdate <= getdate() and 
		pfrf.enddate >= getdate() and
		pfscheme.QuoteableInd = 'Y' and
		pfscheme.startdate <= getdate() and
		pfscheme.enddate >= getdate()
End






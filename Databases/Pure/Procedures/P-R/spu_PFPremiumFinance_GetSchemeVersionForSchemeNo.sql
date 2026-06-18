ddldropprocedure spu_PFPremiumFinance_GetSchemeVersionForSchemeNo
go

CREATE PROCEDURE spu_PFPremiumFinance_GetSchemeVersionForSchemeNo
    @SchemeNo INT
AS
	select 
		max(schemeversion) 
	from 
		pfscheme 
	where 
		quoteableInd='Y' and 
		schemeno=@SchemeNo
go
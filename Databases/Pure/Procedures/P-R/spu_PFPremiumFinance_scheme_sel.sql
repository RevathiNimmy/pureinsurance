SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_PFPremiumFinance_scheme_sel'
GO

Create PROCEDURE spu_PFPremiumFinance_scheme_sel  
    @financeplancnt int,  
    @financeplanversion int,
	@mediatype int output
    
AS BEGIN  
declare @schemeno as int
declare @schemeversion as int
select @schemeno=schemeno,@schemeversion=schemeversion from PFPremiumFinance where pfprem_finance_cnt=@financeplancnt and pfprem_finance_version=@financeplanversion
select @mediatype=mediatype_id from pfscheme where schemeno=@schemeno and schemeversion=@schemeversion
End


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
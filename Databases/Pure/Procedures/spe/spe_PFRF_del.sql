SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PFRF_del'
GO
CREATE PROCEDURE spe_PFRF_del
    @CompanyNo int,
    @SchemeNo int,
    @SchemeVersion int,
    @StartDate datetime,
    @ProductFamily char(1)
AS

DELETE FROM
    PFRF
WHERE
    CompanyNo = @CompanyNo
AND SchemeNo = @SchemeNo
AND SchemeVersion = @SchemeVersion
AND StartDate = @StartDate
AND ProductFamily = @ProductFamily

--eck310102 don't delete if premiumfinance

AND not exists (select pfprem_finance_cnt from PFPremiumFinance
	          where companyno = @companyNo
		  and schemeno = @schemeno
                             and schemeversion = @schemeversion)

GO


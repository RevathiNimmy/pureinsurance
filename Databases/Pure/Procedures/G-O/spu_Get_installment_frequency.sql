SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Get_installment_frequency'
GO
CREATE PROCEDURE spu_Get_installment_frequency
@ifilecnt int
AS
Select code from pffrequency
Inner join pfrf on pfrf.pffrequency_id = pffrequency.pffrequency_id
Inner join pfpremiumfinance PF on (pfrf.companyno=PF.companyno and pfrf.schemeno=PF.schemeno and pfrf.schemeversion =PF.schemeversion)
Where productfamily = 'NB'
AND insurance_file_cnt = @ifilecnt
GO 



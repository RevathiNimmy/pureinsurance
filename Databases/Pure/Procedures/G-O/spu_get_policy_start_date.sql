SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_policy_start_date'
GO


CREATE PROCEDURE spu_get_policy_start_date
		@pfprem_finance_cnt int
AS


SELECT cover_start_date, inception_date_tpi
FROM PFPremiumFinance 
INNER join insurance_file ON PFPremiumFinance.insurance_file_cnt = insurance_file.insurance_file_cnt
WHERE PFPremiumFinance.pfprem_finance_cnt = @pfprem_finance_cnt


GO

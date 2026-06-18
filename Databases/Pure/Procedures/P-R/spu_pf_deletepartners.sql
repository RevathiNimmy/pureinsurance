DDLDROPPROCEDURE 'spu_pf_deletepartners'
GO
CREATE PROCEDURE spu_pf_deletepartners(
		@pf_prem_finance_cnt int)
AS
DELETE FROM pfpartners WHERE pfprem_finance_cnt = @pf_prem_finance_cnt

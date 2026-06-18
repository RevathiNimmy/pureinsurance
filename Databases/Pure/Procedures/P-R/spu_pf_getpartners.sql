DDLDROPPROCEDURE 'spu_pf_getpartners'
GO
CREATE PROCEDURE spu_pf_getpartners(
		@pf_prem_finance_cnt int)
AS
SELECT fullname, address1, address2, address3, address4, postcode FROM pfpartners WHERE pfprem_finance_cnt = @pf_prem_finance_cnt

EXECUTE DDLDropProcedure 'spu_sir_Is_Pending_portfolio_transfer'
GO

CREATE PROCEDURE spu_sir_Is_Pending_portfolio_transfer 
@sPolicy_ref Varchar(30)
AS
BEGIN
SELECT * FROM insurance_file_pt_ri_usage IFU INNER JOIN insurance_file IFI
ON IFU.insurance_file_cnt =IFI.insurance_file_cnt 
WHERE IFI.insurance_ref =LTRIM(RTRIM(@sPolicy_ref))
END
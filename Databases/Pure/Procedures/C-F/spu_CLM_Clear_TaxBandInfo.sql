
EXECUTE DDLDropProcedure 'spu_CLM_Clear_TaxBandInfo'
GO
CREATE PROCEDURE spu_CLM_Clear_TaxBandInfo
@nReserveID INT
AS
BEGIN
Delete From  tblTaxBandInfo Where ReserveID=@nReserveID
END  

Go

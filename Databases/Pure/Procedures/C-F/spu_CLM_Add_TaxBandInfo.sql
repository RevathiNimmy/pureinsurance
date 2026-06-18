
EXECUTE DDLDropProcedure 'spu_CLM_Add_TaxBandInfo'
GO
CREATE PROCEDURE spu_CLM_Add_TaxBandInfo
@nReserveID INT,
@nTaxBandID INT,
@dRate FLOAT,
@bIsValue TINYINT,
@nClassOfBusinessID INT,
@crTaxAmount CURRENCY
AS
BEGIN
INSERT INTO tblTaxBandInfo(ReserveID ,TaxBandID ,Rate ,IsValue ,ClassOfBusinessID ,TaxAmount)
VALUES(@nReserveID,@nTaxBandID,@dRate,@bIsValue,@nClassOfBusinessID,@crTaxAmount)

END  
Go

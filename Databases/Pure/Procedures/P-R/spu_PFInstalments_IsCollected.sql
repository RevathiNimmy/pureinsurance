SET QUOTED_IDENTIFIER ON
GO
EXECUTE DDLDropProcedure 'spu_PFInstalments_IsCollected'
GO

CREATE PROCEDURE spu_PFInstalments_IsCollected  
    @nPFprem_finance_cnt INT,  
    @nPFprem_finance_version INT  
AS  
IF EXISTS(SELECT 1 FROM pfinstalments WHERE pfprem_finance_cnt=@nPFprem_finance_cnt 
AND pfprem_finance_version < @nPFprem_finance_version AND STATUS=3 AND InstalmentNumber<>0)  
BEGIN  
 	SELECT 1  
END  
ELSE  
BEGIN  
 	SELECT 0  
END  
GO


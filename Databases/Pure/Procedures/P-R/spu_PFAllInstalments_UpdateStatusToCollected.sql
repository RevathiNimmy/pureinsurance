SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_PFAllInstalments_UpdateStatusToCollected'
GO
CREATE PROCEDURE spu_PFAllInstalments_UpdateStatusToCollected
@PremiumFinanceCnt INT,
@PremiumFinanceVersion INT
AS 
BEGIN
DECLARE @kPFInstalmentCollected INT = 3,
@kPFInstalmenStatusNew INT = 1

    UPDATE PFInstalments
    SET Status = @kPFInstalmentCollected
    WHERE PFPrem_Finance_cnt = @PremiumFinanceCnt
    AND PFPrem_Finance_version = @PremiumFinanceVersion	AND status = @kPFInstalmenStatusNew AND Amount = 0

END
SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIR_CheckInstalmentDepositRequired'
GO


CREATE PROCEDURE spu_SIR_CheckInstalmentDepositRequired

    @insurance_file_cnt INT 
AS

    IF EXISTS(SELECT * FROM pfPremiumFinance WHERE Insurance_File_Cnt = @insurance_file_cnt AND Deposit>0)
    BEGIN
        SELECT 1
    END
    ELSE
    BEGIN
        SELECT 0
    END
  
GO
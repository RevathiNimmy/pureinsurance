EXECUTE DDLDropProcedure 'spu_PFInstalments_Settle'
GO

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE spu_PFInstalments_Settle 
    @FinanceCount int,
    @Financeversion int,
    @DueDate smalldatetime,
    @CollectedStatus int,
    @ManualStatus int

AS BEGIN

    UPDATE PFInstalments
    SET Status= @CollectedStatus
    WHERE pfprem_finance_cnt= @FinanceCount
    AND  pfprem_finance_version=@Financeversion
    AND Status<> @ManualStatus
    AND DueDate>=@DueDate

END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

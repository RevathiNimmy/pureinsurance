SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PFPremiumFinance_delete'
GO
CREATE PROCEDURE spe_PFPremiumFinance_delete
    @pfprem_finance_cnt int,
    @pfprem_finance_version int,
    @returnstate int OUTPUT
AS

DECLARE @kPfPremFinanceUpdateStatus VARCHAR(10)='011'
    /* Process in a transaction for integrity */
    Begin Transaction

    /* Clear out the transaction table */
    DELETE
        PFTransaction_id
    WHERE
        pfprem_finance_cnt = @pfprem_finance_cnt
    AND
        pfprem_finance_version = @pfprem_finance_version

    /* Check for error deleting */
    If @@Error <> 0
    Begin
        Rollback Transaction

        Select @returnstate = 0
        Return
    End
	/* Delete the PFInstalments record */

	IF EXISTS(
	SELECT NULL FROM PFPremiumFinance 		
		WHERE 
			pfprem_finance_cnt=@pfprem_finance_cnt 
		AND 
			pfprem_finance_version=@pfprem_finance_version
		AND 
			statusind=@kPfPremFinanceUpdateStatus)
   BEGIN
		EXEC spu_PFInstalments_Delete @pfprem_finance_cnt,@pfprem_finance_version
   END
    /* Check for error deleting */
    If @@Error <> 0
    Begin
        Rollback Transaction

        Select @returnstate = 0
        Return
    End

	/* Clear out the tax entries for instalments from tax_calculation table */
    DELETE
        Tax_Calculation
    WHERE
        pfprem_finance_cnt = @pfprem_finance_cnt
    AND
        pfprem_finance_version = @pfprem_finance_version

    /* Check for error deleting */
    If @@Error <> 0
    Begin
        Rollback Transaction

        Select @returnstate = 0
        Return
    End

    /* Delete the premium finance record */
    DELETE
        PFPremiumFinance
    WHERE
        pfprem_finance_cnt = @pfprem_finance_cnt
    AND
        pfprem_finance_version = @pfprem_finance_version

    /* Check for error deleting */
    If @@Error <> 0
    Begin
        Rollback Transaction

        Select @returnstate = 0
        Return
    End

    /* We made it this far commit transaction and return success */
    Commit Transaction
    Select @returnstate = 1

GO


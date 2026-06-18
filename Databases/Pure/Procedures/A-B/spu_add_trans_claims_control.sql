SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_add_trans_claims_control'
GO


CREATE PROCEDURE spu_add_trans_claims_control
    @stats_folder_cnt int,
    @nIsCloned INT =0,
    @transaction_export_folder_cnt int OUTPUT
AS


BEGIN

/**************************************************************************************************************/
/* spu_add_trans_claims_control is the controlling procedure for creating the                    */
/* Transaction_Export Folder and Detail records for claims.                 */
/*                                                                                                          */
/* 1 parameters is passed in -  @stats_folder_id        */
/*                                                                                                          */
/* The stored procedures for all trans details methods are called in sequence.                  */
/*                                                                                                          */
/* A failure on any of the methods will result in the deletion of all stats details for         */
/* the given stats folder;  the stats folder records for the given policy will then be          */
/* deleted.                                                                                             */
/**************************************************************************************************************/
/* Revision Description of Modification         Date        Who     */
/* --------         ---------------------------             ----        --- */
/* 1.0      Original                        16/07/2001  RWH */
/*                                          */
/**************************************************************************************************************/

DECLARE @return_status int
DECLARE @RI2007Enabled int

Select @RI2007Enabled = ISNULL(value,0) From hidden_options Where option_number = 88 --RI2007Enabled

-- Create transaction folder
    EXECUTE
    @return_status = spu_add_trans_export_folder
                    @transaction_export_folder_cnt OUTPUT,
                    @stats_folder_cnt
    IF @return_status <> 0
        GOTO Err_Add_Trans_Details

-- Create transaction records for sales transaction export
    EXECUTE
    @return_status = spu_add_trans_details_clm_gross  
                    @transaction_export_folder_cnt,  
                    @stats_folder_cnt,
                    @nIsCloned  
    IF @return_status <> 0
        GOTO Err_Add_Trans_Details

-- Create transaction records for reinsurance transaction export
    If @RI2007Enabled = 1 Begin
	EXECUTE
	    @return_status = spu_add_trans_details_clm_reins_ri2007
	                    @transaction_export_folder_cnt,
	                    @stats_folder_cnt
    End
    Else Begin
	EXECUTE
	    @return_status = spu_add_trans_details_clm_reins
	                    @transaction_export_folder_cnt,
	                    @stats_folder_cnt
    End
    IF @return_status <> 0
        GOTO Err_Add_Trans_Details

-- Create transaction records for coinsurance transaction export
    EXECUTE
    @return_status = spu_add_trans_details_clm_coins
                    @transaction_export_folder_cnt,
                    @stats_folder_cnt
    IF @return_status <> 0
        GOTO Err_Add_Trans_Details

RETURN

Err_Add_Trans_Details:
    BEGIN
        /* Delete all transactions for this folder */
        DELETE FROM Transaction_Export_Detail
            WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt

        /* Delete the transactions folder record */
        DELETE FROM Transaction_Export_Folder
            WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt

        RETURN
    END

END
GO



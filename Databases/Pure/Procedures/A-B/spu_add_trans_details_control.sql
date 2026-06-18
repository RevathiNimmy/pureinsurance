EXECUTE DDLDropProcedure 'spu_add_trans_details_control'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_add_trans_details_control
    @transaction_export_folder_cnt int,
    @stats_folder_cnt int
AS
BEGIN
DECLARE @return_status int
DECLARE @RI2007Enabled int

Select @RI2007Enabled = ISNULL(value,0) From hidden_options Where option_number = 88 --RI2007Enabled

    EXECUTE
    @return_status = spu_add_trans_details_sales
                    @transaction_export_folder_cnt,
                    @stats_folder_cnt
    IF @return_status <> 0
        GOTO Err_Add_Trans_Details
    EXECUTE
    @return_status = spu_add_trans_details_purchase
                    @transaction_export_folder_cnt,
                    @stats_folder_cnt
    IF @return_status <> 0
        GOTO Err_Add_Trans_Details
    
    If @RI2007Enabled = 1 Begin
	EXECUTE
	    @return_status = spu_add_trans_details_reins_ri2007
	                    @transaction_export_folder_cnt,
	                    @stats_folder_cnt
    End
    Else Begin
	EXECUTE
	    @return_status = spu_add_trans_details_reins
	                    @transaction_export_folder_cnt,
	                    @stats_folder_cnt
    End
    IF @return_status <> 0
        GOTO Err_Add_Trans_Details

    EXECUTE
    @return_status = spu_add_trans_details_coins
                    @transaction_export_folder_cnt,
                    @stats_folder_cnt
    IF @return_status <> 0
        GOTO Err_Add_Trans_Details

    EXECUTE
    @return_status = spu_sir_upd_acc_export_status
                    @transaction_export_folder_cnt,
                    'p'
    IF @return_status <> 0
        GOTO Err_Add_Trans_Details

    -- DD 16/02/2004 Added for Sub Agent Commission
    EXECUTE
    @return_status = spu_add_trans_details_commission
                    @transaction_export_folder_cnt,
                    @stats_folder_cnt
    IF @return_status <> 0
        GOTO Err_Add_Trans_Details

RETURN
Err_Add_Trans_Details:
    BEGIN
        DELETE FROM Transaction_Export_Detail
            WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt
        DELETE FROM Transaction_Export_Folder
            WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt
        RETURN
    END
END
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO


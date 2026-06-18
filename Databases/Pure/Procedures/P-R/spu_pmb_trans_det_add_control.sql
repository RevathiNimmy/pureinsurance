SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmb_trans_det_add_control'
GO


CREATE PROCEDURE spu_pmb_trans_det_add_control
    @transaction_export_folder_cnt int
AS


BEGIN
/******************************************************************************************/
/* spu_PMB_trans_det_add_control is the controlling procedure for creating the             */
/* Transaction_Export_Detail table.                           */
/*                                                                                        */
/* 1 parameter is passed in                                   */
/*                                                                                        */
/******************************************************************************************/
/* Revision Description of Modification         Date        Who       */
/* --------     ---------------------------         ----        ---       */
/* 1.0      Original                    24/11/1998  TF    */
/******************************************************************************************/
DECLARE @return_status      int,
    @transaction_type_code  char(10)

/* Get transaction_type */
    SELECT  @transaction_type_code = transaction_type_code
    FROM    Transaction_Export_Folder   F
    WHERE   F.transaction_export_folder_cnt = @transaction_export_folder_cnt
/* Create transaction records for NB_DIRECT */
    IF @transaction_type_code = 'NB D'
    BEGIN
        EXECUTE
        @return_status = spu_pmb_trans_det_add_NBD
                    @transaction_export_folder_cnt
        IF @return_status <> 0
            GOTO Err_Add_Trans_Details
    END
/* Create transaction records for NB_DIRECT with INSTALLMENTS */
    IF @transaction_type_code = 'NB DI'
    BEGIN
        EXECUTE
        @return_status = spu_MBP_trans_det_add_NBDI
                    @transaction_export_folder_cnt
        IF @return_status <> 0
            GOTO Err_Add_Trans_Details
    END
/* Create transaction records for NB_INDIRECT */
    IF @transaction_type_code = 'NB I'
    BEGIN
        EXECUTE
        @return_status = spu_pmb_trans_det_add_NBI
                    @transaction_export_folder_cnt
        IF @return_status <> 0
            GOTO Err_Add_Trans_Details
    END
/* Create transaction records for NB_INDIRECT with INSTALLMENTS */
    IF @transaction_type_code = 'NB II'
    BEGIN
        EXECUTE
        @return_status = spu_MBP_trans_det_add_NBII
                    @transaction_export_folder_cnt
        IF @return_status <> 0
            GOTO Err_Add_Trans_Details
    END
/* Create transaction records for MTA_AP_DIRECT */
    IF @transaction_type_code = 'AP D'
    BEGIN
        EXECUTE
        @return_status = spu_MBP_trans_det_add_APD
                    @transaction_export_folder_cnt
        IF @return_status <> 0
            GOTO Err_Add_Trans_Details
    END
/* Create transaction records for MTA_AP_DIRECT with INSTALLMENTS */
    IF @transaction_type_code = 'AP DI'
    BEGIN
        EXECUTE
        @return_status = spu_MBP_trans_det_add_APDI
                    @transaction_export_folder_cnt
        IF @return_status <> 0
            GOTO Err_Add_Trans_Details
    END
/* Create transaction records for MTA_AP_INDIRECT */
    IF @transaction_type_code = 'AP I'
    BEGIN
        EXECUTE
        @return_status = spu_MBP_trans_det_add_API
                    @transaction_export_folder_cnt
        IF @return_status <> 0
            GOTO Err_Add_Trans_Details
    END
/* Create transaction records for MTA_AP_INDIRECT with INSTALLMENTS */
    IF @transaction_type_code = 'AP II'
    BEGIN
        EXECUTE
        @return_status = spu_MBP_trans_det_add_APII
                    @transaction_export_folder_cnt
        IF @return_status <> 0
            GOTO Err_Add_Trans_Details
    END
/* Create transaction records for MTA_RP_DIRECT */
    IF @transaction_type_code = 'RP D'
    BEGIN
        EXECUTE
        @return_status = spu_MBP_trans_det_add_RPD
                    @transaction_export_folder_cnt
        IF @return_status <> 0
            GOTO Err_Add_Trans_Details
    END
/* Create transaction records for MTA_RP_DIRECT with INSTALLMENTS */
    IF @transaction_type_code = 'RP DI'
    BEGIN
        EXECUTE
        @return_status = spu_MBP_trans_det_add_RPDI
                    @transaction_export_folder_cnt
        IF @return_status <> 0
            GOTO Err_Add_Trans_Details
    END
/* Create transaction records for MTA_RP_INDIRECT */
    IF @transaction_type_code = 'RP I'
    BEGIN
        EXECUTE
        @return_status = spu_MBP_trans_det_add_RPI
                    @transaction_export_folder_cnt
        IF @return_status <> 0
            GOTO Err_Add_Trans_Details
    END
/* Create transaction records for MTA_RP_INDIRECT with INSTALLMENTS */
    IF @transaction_type_code = 'RP II'
    BEGIN
        EXECUTE
        @return_status = spu_MBP_trans_det_add_RPII
                    @transaction_export_folder_cnt
        IF @return_status <> 0
            GOTO Err_Add_Trans_Details
    END
/* Create transaction records for MTA_DIRECT to INSTALLMENTS */
    IF @transaction_type_code = 'MTA DI'
    BEGIN
        EXECUTE
        @return_status = spu_MBP_trans_det_add_RPD
                    @transaction_export_folder_cnt
        IF @return_status <> 0
            GOTO Err_Add_Trans_Details
        EXECUTE
        @return_status = spu_MBP_trans_det_add_APDI
                    @transaction_export_folder_cnt
        IF @return_status <> 0
            GOTO Err_Add_Trans_Details
    END
/* Create transaction records for MTA_INDIRECT to INSTALLMENTS */
    IF @transaction_type_code = 'MTA II'
    BEGIN
        EXECUTE
        @return_status = spu_MBP_trans_det_add_RPI
                    @transaction_export_folder_cnt
        IF @return_status <> 0
            GOTO Err_Add_Trans_Details
        EXECUTE
        @return_status = spu_MBP_trans_det_add_APII
                    @transaction_export_folder_cnt
        IF @return_status <> 0
            GOTO Err_Add_Trans_Details
    END
/* Set accounts_export_status  to Pending */
    EXECUTE
    @return_status = spu_sir_upd_acc_export_status
                    @transaction_export_folder_cnt,
                    'p'
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



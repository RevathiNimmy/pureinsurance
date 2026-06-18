SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_sir_upd_acc_export_status'
GO


CREATE PROCEDURE spu_sir_upd_acc_export_status
    @transaction_export_folder_cnt int,
    @accounts_export_status char(1)
AS


BEGIN
/******************************************************************************************/
/* spu_sir_upd_acc_export_status     sets the accounts export status code          */
/*                                            */
/* two parameters are passed in @transaction_export_folder_cnt                */
/*              @accounts_export_status                   */
/******************************************************************************************/
/* Revision Description of Modification         Date        Who       */
/* --------     ---------------------------         ----        ---       */
/* 1.0      Original                    30/09/1997  TF    */
/******************************************************************************************/
    UPDATE  Transaction_Export_Folder
    SET accounts_export_status = @accounts_export_status
    WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt
END
GO



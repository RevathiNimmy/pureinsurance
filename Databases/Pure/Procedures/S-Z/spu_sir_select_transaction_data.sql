SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_sir_select_transaction_data'
GO


CREATE PROCEDURE spu_sir_select_transaction_data
    @transaction_export_folder_cnt int
AS


BEGIN
/******************************************************************************************/
/* spu_sir_select_transaction_data   returns details of transaction set        */
/*                                            */
/* one parameters is  passed in                               */
/******************************************************************************************/
/* Revision Description of Modification         Date        Who       */
/* --------     ---------------------------         ----        ---       */
/* 1.0      Original                    09/10/1997  TF    */
/* 2.0      Extras for account key etc.         11/11/1998  TF    */
/******************************************************************************************/

    /* Get the Header details */
    SELECT  document_ref,
        debit_credit,
        transaction_type_code,
        document_date,
        accounting_date,
        document_comment,
        currency_code,
        business_type_code,
        insurance_ref,
        product_code,
        branch_code,
        agent_shortname,
        insurance_holder_shortname,
        effective_date,
        created_by_user_id,
        source_id,
        is_payable_by_instalments,
        insurance_holder_id,
        agent_id
    FROM    Transaction_Export_Folder
    WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt
    /* Get the Transaction details */
    SELECT  transaction_ledger_code,
        account_type_code,
        transaction_amount,
        mapping_code
    FROM    Transaction_Export_Detail
    WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt
    ORDER BY transaction_export_detail_id

END
GO



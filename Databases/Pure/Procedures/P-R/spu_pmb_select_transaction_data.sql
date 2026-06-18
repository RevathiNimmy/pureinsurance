EXECUTE DDLDropProcedure 'spu_pmb_select_transaction_data'
GO

CREATE PROCEDURE spu_pmb_select_transaction_data
    @transaction_export_folder_cnt INT
AS

Declare
      @InsuranceFilecnt Integer,
      @TransType integer,
      @InsFileType Integer
      select @InsuranceFilecnt  = Insurance_File_cnt from Transaction_Export_Folder where transaction_export_folder_cnt = @transaction_export_folder_cnt
      select @TransType = last_trans_type_id from Insurance_File_System where insurance_file_cnt = @InsuranceFilecnt
      select @InsFileType = insurance_file_type_id from insurance_file where insurance_file_cnt = @InsuranceFilecnt
   

    -- Only group together if underwriting
    IF (SELECT value FROM hidden_options WHERE option_number = 1) = 'A' BEGIN
        -- Get the Header details 
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
                agent_id,
                posting_period_number,               -- RWH (21/06/01) UW need to use this when posting to Orion.
                insurance_file_cnt,
                reason,
                transaction_ledger_code,
                account_type_code,
                ROUND(transaction_amount, 4) transaction_amount,
                ROUND(charges_total, 2)      AS charges_total,
                ROUND(taxes_total, 2)        AS taxes_total,
                mapping_code,
                transaction_account_key,
                spare,
                real_insurance_file_cnt,
                purchase_order_no,
                purchase_invoice_no,
        		-- Blank to be in sync with U/W
        	    0 'underwriting_year_id',
                suspended,
                release_to_income,
                release_account_code,
                transdetail_type_code,
                tax_group_id,
                tax_band_id,
                terms_of_payment_id,
				payment_due_date,
				d.manually_released,
				d.released_on_full_settlement,
				d.released_for_whole_posting,
				d.released_on_policy_effective 
        FROM    Transaction_Export_Folder F 
        JOIN    Transaction_Export_Detail D ON D.transaction_export_folder_cnt = F.transaction_export_folder_cnt
        WHERE   F.transaction_export_folder_cnt = @transaction_export_folder_cnt
        ORDER BY transaction_export_detail_id
    
    END ELSE BEGIN
    
        -- Get the Header details 
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
                agent_id,
                posting_period_number,               -- RWH (21/06/01) UW need to use this when posting to Orion.
                insurance_file_cnt,
                reason,
                transaction_ledger_code,
                account_type_code,
                ROUND(SUM(transaction_amount), 2) transaction_amount,  
                ROUND(SUM(charges_total), 4)      AS charges_total,
                ROUND(SUM(taxes_total), 4)        AS taxes_total,
                mapping_code,
                transaction_account_key,
                spare,
                real_insurance_file_cnt,
                purchase_order_no,
                purchase_invoice_no,
        		F.underwriting_year_id,
                suspended,
                release_to_income,
                release_account_code,
                transdetail_type_code,
                tax_group_id,
                tax_band_id,
                terms_of_payment_id,
				payment_due_date,
				d.manually_released,
				d.released_on_full_settlement,
				d.released_for_whole_posting,
				d.released_on_policy_effective,
				d.fee_type 
        FROM    Transaction_Export_Folder F 
        JOIN    Transaction_Export_Detail D ON D.transaction_export_folder_cnt = F.transaction_export_folder_cnt
        WHERE   F.transaction_export_folder_cnt = @transaction_export_folder_cnt
        GROUP BY 
                document_ref, debit_credit, transaction_type_code, document_date, accounting_date,
                document_comment, currency_code, business_type_code, insurance_ref, product_code,
                branch_code, agent_shortname, insurance_holder_shortname, effective_date, 
                created_by_user_id, source_id, is_payable_by_instalments, insurance_holder_id, 
                agent_id, posting_period_number, insurance_file_cnt, reason, transaction_ledger_code, 
                account_type_code, mapping_code, transaction_account_key, spare, real_insurance_file_cnt,
                purchase_order_no, purchase_invoice_no, F.underwriting_year_id,suspended,release_to_income,
                release_account_code, transdetail_type_code, tax_group_id, tax_band_id,terms_of_payment_id,
				payment_due_date,manually_released,released_on_full_settlement,released_for_whole_posting,released_on_policy_effective , d.fee_type 
        ORDER BY
                MAX(transaction_export_detail_id)
    END

GO



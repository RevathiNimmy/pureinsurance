
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Move_Suspended_Agent_Commission'
GO

CREATE PROCEDURE spu_ACT_Move_Suspended_Agent_Commission  
 @old_insurance_file_cnt INT,  
 @new_insurance_file_cnt INT,  
 @user_id INT  
  
AS  
  
DECLARE @old_account_id INT  
DECLARE @new_account_id INT  
DECLARE @suspended_transdetail_id INT  
DECLARE @old_comm_rate NUMERIC(8,2)  
DECLARE @new_comm_rate NUMERIC(8,2)  
  
DECLARE @original_document_id INT  
DECLARE @document_id INT  
DECLARE @document_id_rev INT  
DECLARE @documenttype_id INT  
DECLARE @document_type_code VARCHAR(10)  
DECLARE @document_reference VARCHAR(25)  
  
DECLARE @base_transdetail_id INT  
DECLARE @new_suspended_transdetail_id INT  
  
DECLARE @eff_date_Prev_version DATETIME  
DECLARE @eff_date_MTA_version DATETIME  
  
DECLARE @old_premium NUMERIC(8,2)  
  
BEGIN  
  
SELECT @eff_date_Prev_version=cover_start_date  
FROM insurance_file  
WHERE insurance_file_cnt=@old_insurance_file_cnt  
  
SELECT @eff_date_MTA_version=cover_start_date  
FROM insurance_file  
WHERE insurance_file_cnt=@new_insurance_file_cnt  
  
SELECT @old_account_id = account_id FROM account  
WHERE account_key = (SELECT lead_agent_cnt FROM insurance_file  
WHERE insurance_file_cnt=@old_insurance_file_cnt)  
  
SELECT @new_account_id = account_id FROM account  
WHERE account_key = (SELECT lead_agent_cnt FROM insurance_file  
WHERE insurance_file_cnt=@new_insurance_file_cnt)  
  
if (ISNULL(@old_account_id, 0) = ISNULL(@new_account_id, 0))  
 OR (ISNULL(@new_account_id, 0) = 0)  
Begin  
    -- if comm account isn't changing into another valid comm account  
    RETURN  
End  
  
DECLARE susp_acc_trans_cur CURSOR FAST_FORWARD FOR  
SELECT suspended_transdetail_id FROM Suspended_Accounts_Transactions  
WHERE insurance_file_cnt=@old_insurance_file_cnt  
AND destination_account_id = @old_account_id  
AND transdetail_type_id = (SELECT transdetail_type_id FROM transdetail_type  
  WHERE code='COMM')  
AND NOT EXISTS (SELECT suspended_transdetail_id FROM Released_Accounts_Transactions WHERE  
suspended_transdetail_id = Suspended_Accounts_Transactions.suspended_transdetail_id  
AND recall_date IS NULL)  
AND @eff_date_MTA_version<=@eff_date_Prev_version  
AND is_deleted=0  
  
OPEN susp_acc_trans_cur  
FETCH NEXT FROM susp_acc_trans_cur INTO @suspended_transdetail_id  
WHILE @@FETCH_STATUS = 0  
BEGIN  
  
 --For each record  
  
 SELECT @old_comm_rate = ac.commission_percentage  
 FROM   Agent_Commission ac  
 WHERE  (ac.is_lead_agent = 1) AND ac.insurance_file_cnt=@old_insurance_file_cnt  
  
 SELECT @old_comm_rate=ISNULL(@old_comm_rate,0)  
  
 SELECT @new_comm_rate = ac.commission_percentage  
 FROM   Agent_Commission ac  
 WHERE  (ac.is_lead_agent = 1) AND ac.insurance_file_cnt=@new_insurance_file_cnt  
  
 SELECT @new_comm_rate=ISNULL(@new_comm_rate,0)  
  
 --COMM rate for old insurance_file is different from new insurance_file  
 IF @old_comm_rate<>@new_comm_rate  
    BEGIN  
  
  --We need to rollout the old one and reapply with a new rate  
  
  --Get new document_id to update the  duplicate document with new document reference  
  SELECT @document_type_code= code, @documenttype_id=documenttype_id FROM documenttype  
  WHERE documenttype_id=(SELECT documenttype_id FROM document d  
     JOIN transdetail t ON t.document_id=d.document_id  
      WHERE t.transdetail_id= @suspended_transdetail_id)  
  
  EXEC spu_ACT_Generate_DocumentReference @document_type_code, @user_id , 1, @document_reference OUTPUT  
  
  INSERT INTO document (batch_id, company_id, sub_branch_id, postingstatus_id, documenttype_id, auditset_id, document_ref, document_date, created_date, authorised_date,  
   comment, write_off_reason_id, insurance_file_cnt, reason, claim_id, terms_of_payment_id, payment_due_date)  
  SELECT  batch_id, company_id, sub_branch_id, postingstatus_id, @documenttype_id, auditset_id, @document_reference, GETDATE(), GETDATE(),  GETDATE(),  
   comment, write_off_reason_id, insurance_file_cnt, reason, claim_id, terms_of_payment_id, payment_due_date  
  FROM    Document  
  WHERE document_id=(SELECT document_id FROM transdetail  
  WHERE transdetail_id= @suspended_transdetail_id)  
  
  SELECT  @document_id=document_id FROM document  
  WHERE  document_ref =@document_reference  
  
  --Duplicate the document for suspended_transdetail_id reversing all the amount signs  
  INSERT INTO transdetail(account_id, postingstatus_id, company_id, sub_branch_id, currency_id, period_id, document_id, document_sequence, accounting_date,  
   amount, base_amount_unrounded, fully_matched, currency_amount, currency_amount_unrounded, currency_base_xrate, euro_currency_id, euro_amount,  
   euro_base_xrate, euro_ccy_xrate, comment, insurance_ref, operator_id, purchase_order_no, purchase_invoice_no, department, spare, ref_date,  
   ref_amount, ref_quantity, ref_units, insurance_ref_index, department_id, not_reported, underwriting_year_id, amount_currency_id,  
   account_currency_id, account_amount, account_amount_unrounded, account_base_xrate, system_currency_id, system_amount,  
   system_amount_unrounded, system_base_xrate, outstanding_amount, outstanding_currency_amount, outstanding_account_amount,  
   outstanding_system_amount, amount_updated, reference, type_code, transdetail_type_id, batch_id, tax_group_id, tax_band_id, claim_ref,  
   balance_type, risk_transfer, risk_transfer_reconciliation_date)  
  SELECT  account_id, postingstatus_id, company_id, sub_branch_id, currency_id, period_id, @document_id, document_sequence, GETDATE(),  
   amount * (-1), base_amount_unrounded* (-1), fully_matched, currency_amount* (-1), currency_amount_unrounded* (-1) , currency_base_xrate, euro_currency_id, euro_amount* (-1),  
   euro_base_xrate, euro_ccy_xrate, comment, insurance_ref, operator_id, purchase_order_no, purchase_invoice_no, department, spare, ref_date,  
   ref_amount* (-1), ref_quantity* (-1), ref_units, insurance_ref_index, department_id, not_reported, underwriting_year_id, amount_currency_id,  
   account_currency_id, account_amount* (-1), account_amount_unrounded* (-1), account_base_xrate, system_currency_id, system_amount* (-1),  
   system_amount_unrounded*(-1), system_base_xrate, 0, 0, 0,  
   0, amount_updated, reference, type_code, transdetail_type_id, batch_id, tax_group_id, tax_band_id, claim_ref,  
   balance_type, risk_transfer, risk_transfer_reconciliation_date  
  FROM transdetail  
  WHERE document_id=(SELECT document_id FROM transdetail  
  WHERE transdetail_id= @suspended_transdetail_id)  
  
  --Update the suspended_transdetail_id is_deleted=1  
  UPDATE Suspended_Accounts_Transactions  
  SET is_deleted=1  
  WHERE suspended_transdetail_id= @suspended_transdetail_id  
  
  --Duplicate the document for suspended_transdetail_id again  
  
  EXEC spu_ACT_Generate_DocumentReference @document_type_code, @user_id , 1, @document_reference OUTPUT  
  
  INSERT INTO document (batch_id, company_id, sub_branch_id, postingstatus_id, documenttype_id, auditset_id, document_ref, document_date, created_date, authorised_date,  
   comment, write_off_reason_id, insurance_file_cnt, reason, claim_id, terms_of_payment_id, payment_due_date)  
  SELECT  batch_id, company_id, sub_branch_id, postingstatus_id, @documenttype_id, auditset_id, @document_reference, GETDATE(), GETDATE(),  GETDATE(),  
   comment, write_off_reason_id, insurance_file_cnt, reason, claim_id, terms_of_payment_id, payment_due_date  
  FROM    Document  
  WHERE document_id=(SELECT document_id FROM transdetail  
  WHERE transdetail_id= @suspended_transdetail_id)  
  
  SELECT  @document_id_rev=document_id FROM document  
  WHERE  document_ref =@document_reference  
  
  INSERT INTO transdetail(account_id, postingstatus_id, company_id, sub_branch_id, currency_id, period_id, document_id, document_sequence, accounting_date,  
   amount, base_amount_unrounded, fully_matched, currency_amount, currency_amount_unrounded, currency_base_xrate, euro_currency_id, euro_amount,  
   euro_base_xrate, euro_ccy_xrate, comment, insurance_ref, operator_id, purchase_order_no, purchase_invoice_no, department, spare, ref_date,  
   ref_amount, ref_quantity, ref_units, insurance_ref_index, department_id, not_reported, underwriting_year_id, amount_currency_id,  
   account_currency_id, account_amount, account_amount_unrounded, account_base_xrate, system_currency_id, system_amount,  
   system_amount_unrounded, system_base_xrate, outstanding_amount, outstanding_currency_amount, outstanding_account_amount,  
   outstanding_system_amount, amount_updated, reference, type_code, transdetail_type_id, batch_id, tax_group_id, tax_band_id, claim_ref,  
   balance_type, risk_transfer, risk_transfer_reconciliation_date)  
  SELECT  account_id, postingstatus_id, company_id, sub_branch_id, currency_id, period_id, @document_id_rev, document_sequence, GETDATE(),  
   amount, base_amount_unrounded, fully_matched, currency_amount, currency_amount_unrounded, currency_base_xrate, euro_currency_id, euro_amount,  
   euro_base_xrate, euro_ccy_xrate, comment, insurance_ref, operator_id, purchase_order_no, purchase_invoice_no, department, spare, ref_date,  
   ref_amount, ref_quantity, ref_units, insurance_ref_index, department_id, not_reported, underwriting_year_id, amount_currency_id,  
   account_currency_id, account_amount, account_amount_unrounded, account_base_xrate, system_currency_id, system_amount,  
   system_amount_unrounded, system_base_xrate, outstanding_amount, outstanding_currency_amount, outstanding_account_amount,  
   outstanding_system_amount, amount_updated, reference, type_code, transdetail_type_id, batch_id, tax_group_id, tax_band_id, claim_ref,  
   balance_type, risk_transfer, risk_transfer_reconciliation_date  
  FROM transdetail  
  WHERE document_id=(SELECT document_id FROM transdetail  
  WHERE transdetail_id= @suspended_transdetail_id)  
  
  --Recalculating the amounts on the transdetail records with the revised commission calculation  
  UPDATE transdetail  
  SET  amount = (((amount / @old_comm_rate) * 100) * @new_comm_rate) / 100,
   base_amount_unrounded = (((base_amount_unrounded / @old_comm_rate) * 100) * @new_comm_rate) / 100,
   currency_amount = (((currency_amount / @old_comm_rate) * 100) * @new_comm_rate) / 100,
   currency_amount_unrounded = (((currency_amount_unrounded / @old_comm_rate) * 100) * @new_comm_rate) / 100,
   euro_amount = (((euro_amount / @old_comm_rate) * 100) * @new_comm_rate) / 100,
   account_amount = (((account_amount / @old_comm_rate) * 100) * @new_comm_rate) / 100,
   account_amount_unrounded = (((account_amount_unrounded / @old_comm_rate) * 100) * @new_comm_rate) / 100,
   system_amount = (((system_amount / @old_comm_rate) * 100) * @new_comm_rate) / 100,
   system_amount_unrounded = (((system_amount_unrounded / @old_comm_rate) * 100) * @new_comm_rate) / 100,
   outstanding_amount = (((outstanding_amount / @old_comm_rate) * 100) * @new_comm_rate) / 100,
   outstanding_currency_amount = (((outstanding_currency_amount / @old_comm_rate) * 100) * @new_comm_rate) / 100,
   outstanding_account_amount = (((outstanding_account_amount / @old_comm_rate) * 100) * @new_comm_rate) / 100,
   outstanding_system_amount = (((outstanding_system_amount / @old_comm_rate) * 100) * @new_comm_rate) / 100
  FROM transdetail td JOIN document d ON td.document_id=d.document_id  
  WHERE d.insurance_file_cnt=@old_insurance_file_cnt  
  AND td.document_id=@document_id_rev  
  AND spare IN ('COMM', 'COMSUSP')
  
  --Duplicate the Suspended_Accounts_Transaction records  
  SELECT @base_transdetail_id=td.transdetail_id  
  FROM transdetail td JOIN document d ON td.document_id=d.document_id  
  WHERE d.insurance_file_cnt=@old_insurance_file_cnt  
   AND td.document_id=@document_id_rev  
   AND spare='GROSS'  
  
  SELECT @new_suspended_transdetail_id=td.transdetail_id  
  FROM transdetail td JOIN document d ON td.document_id=d.document_id  
  WHERE d.insurance_file_cnt=@old_insurance_file_cnt  
   AND td.document_id=@document_id_rev  
   AND spare='COMM'  
  
  --Just duplicate the original record as per the cursor suspended_transdetail_id with a new destination account  
  --(the new Agent).  
  
  INSERT INTO Suspended_Accounts_Transactions(  suspended_transdetail_id,linked_transdetail_id, linked_percentage,  
    pfprem_finance_cnt, pfprem_finance_version, insurance_file_cnt, destination_account_id, documenttype_id,  
    transdetail_type_id, spare, is_deleted, manually_released, released_on_full_settlement, released_for_whole_posting,  
    released_on_policy_effective)  
  SELECT @new_suspended_transdetail_id, @base_transdetail_id, linked_percentage,  
      pfprem_finance_cnt, pfprem_finance_version, @new_insurance_file_cnt, @new_account_id, @documenttype_id,  
      transdetail_type_id, spare, 0, manually_released, released_on_full_settlement, released_for_whole_posting,  
      released_on_policy_effective  
  FROM suspended_Accounts_Transactions  
  WHERE suspended_transdetail_id=@suspended_transdetail_id  
  
  UPDATE Transdetail  
  SET outstanding_amount=0,  
   outstanding_currency_amount=0,  
   outstanding_account_amount=0,  
   outstanding_system_amount=0  
  WHERE document_id= (SELECT d.document_id FROM document d JOIN transdetail t ON  
   t.document_id=d.document_id WHERE t.transdetail_id= @suspended_transdetail_id)  
  
   END  
  
 ELSE  
  
   BEGIN  
  --UPDATE destination_account_id = @new_account_id IN Suspended_Accounts_Transactions  
  UPDATE Suspended_Accounts_Transactions  
  SET destination_account_id = @new_account_id  
  WHERE suspended_transdetail_id= @suspended_transdetail_id  
   END  
  
 FETCH NEXT FROM susp_acc_trans_cur INTO @suspended_transdetail_id  
  
END  
  
CLOSE susp_acc_trans_cur  
  
DEALLOCATE susp_acc_trans_cur  
  
END
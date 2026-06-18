SET QUOTED_IDENTIFIER OFF 
GO
EXECUTE DDLDropProcedure 'spu_ACT_Get_TransDetails_For_CreditControl'
GO


/* Created by : Vidya Rangdale
Creation Date : 26/02/2014
Description   : This is used to find out all the transactions for single instalment plan
Test Code     : Exec spu_ACT_Get_TransDetails_For_CreditControl 
 */



CREATE PROCEDURE spu_ACT_Get_TransDetails_For_CreditControl 
	@nAccount_id INT,  
	@nPFprem_finance_cnt INT  
AS  
  
CREATE TABLE #temptransaction  
(  
    insurance_file_cnt   INT             NULL,  
    pfprem_finance_cnt       INT         NULL  
)  
  
INSERT INTO #temptransaction  
  SELECT DISTINCT insurance_file_cnt,pfprem_finance_cnt  
        FROM PFTransaction_id WHERE pfprem_finance_cnt = @nPFprem_finance_cnt  
  
SELECT t.account_id,t.transdetail_id,t.outstanding_amount  
	FROM Account a 
	INNER JOIN TransDetail t ON t.account_id = a.account_id  
	LEFT JOIN transdetail_type tt ON tt.transdetail_type_id = t.transdetail_type_id  
	LEFT  JOIN CashListItem cli ON cli.transdetail_id = t.transdetail_id  
	LEFT  JOIN mediatype mt ON mt.mediatype_id = cli.mediatype_id  
	INNER JOIN Document d ON d.document_id = t.document_id INNER JOIN Period p ON p.period_id = t.period_id  
	INNER JOIN DocumentType dt ON dt.documenttype_id = d.documenttype_id  
	INNER JOIN PMUser pmu ON pmu.user_id = t.operator_id  
	LEFT  JOIN Insurance_file i ON i.insurance_file_cnt = d.insurance_file_cnt  
	INNER  JOIN Insurance_file ifl ON ifl.insurance_ref = t.insurance_ref  
	--INNER JOIN pfpremiumfinance pfm ON ifl.insurance_file_cnt = pfm.insurance_file_cnt  
	INNER JOIN #temptransaction tmptrans ON ifl.insurance_file_cnt = tmptrans.insurance_file_cnt  
	LEFT  JOIN Underwriting_Year uwy ON uwy.underwriting_year_id = t.underwriting_year_id  
	INNER JOIN Currency c ON c.currency_id = t.currency_id INNER JOIN Currency c2 ON c2.currency_id = t.amount_currency_id  
	LEFT  JOIN tax_band tb ON tb.tax_band_id = t.tax_band_id  
	LEFT  JOIN tax_group tg ON tg.tax_group_id = t.tax_group_id  
	LEFT  JOIN tax_group_tax_band tgtb ON tgtb.tax_band_id = t.tax_band_id AND tgtb.tax_group_id = t.tax_group_id  
	LEFT  JOIN transdetail_type tdt ON tdt.transdetail_type_id = t.transdetail_type_id  
	LEFT JOIN party ON Party.Party_cnt=a.Account_key  
	LEFT JOIN party_type ON party.party_type_id=party_type.party_type_id  
	LEFT JOIN Party_agent ON Party.Party_cnt=Party_agent.Party_cnt  
	LEFT JOIN Party ag ON ag.Party_cnt=i.lead_agent_cnt  
	LEFT JOIN account AS acc2 ON acc2.account_key = i.insured_cnt  
	LEFT JOIN AuditSet AS auds ON auds.document_id = d.document_id AND ISNULL(auds.rejected,0)=0  
	LEFT JOIN AuditSet_Type AS audst ON audst.auditset_type_id = auds.auditset_type_id  
 WHERE --d.company_id IN (2,1) AND 
 (t.account_id=@nAccount_id)  
  AND dt.documenttype_id in (SELECT documenttype_id FROM documenttype WHERE CODE IN ('SEC','SED'))  
  AND tmptrans.pfprem_finance_cnt= @nPFprem_finance_cnt  
  AND t.outstanding_amount <> 0  
 ORDER BY t.company_id, d.document_date DESC  
  
DROP TABLE #temptransaction  

GO



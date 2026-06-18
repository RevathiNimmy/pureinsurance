SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Spoke_ExportCreditControl'
GO


CREATE  PROCEDURE spu_ACT_Spoke_ExportCreditControl  
    @branch_code CHAR(10)  
AS  

BEGIN  

    SELECT cci.credit_control_item_id,  
	cci.credit_control_reason,  
	cci.account_id,  
	cci.document_id,  
	CONVERT(VARCHAR(10),cci.document_date,126),  
	cci.insurance_file_cnt,  
	cci.pfprem_finance_cnt,  
	cci.pfprem_finance_version,  
	cci.amount,  
	cci.can_auto_cancel,  
	cci.will_auto_cancel,  
	cci.credit_control_step_id,  
	CONVERT(VARCHAR(10),cci.created_date,126),  
	CONVERT(VARCHAR(10),cci.due_date,126),  
	cci.letter_sent,  
	cci.recurrence_count,  
	ccs.recurring_days,  
	ccs.recurring_letters,  
	ccs.auto_cancel_policy,  
	ccs.check_auto_cancel,  
	ccs.jump_to_next_step,  
	ccs.pmuser_group_id,  
	ccs.pmwrk_task_id,  
	ccs.number_of_days,  
	ccs.broker_days,  
	ccs.policy_tolerance_amount,  
	ccs.account_tolerance_amount,  
	ccr.business_type,  
	acs.code,  
	CASE WHEN bt.code='DIRECT' OR ISNULL(pagt.code,'')='Comm Acc' THEN  
		 'DIRECT'  
	ELSE  
		bt.code  
	END IFBusiness_Type,  
	p.shortname,  
	ccs2.credit_control_step_id,  
	risk_count = (SELECT COUNT(*)  
		FROM Insurance_File_Risk_Link ifrl  
			  WHERE ifrl.insurance_file_cnt = cci.insurance_file_cnt  
				  AND ifrl.status_flag <> 'D'),  
	cci.pmuser_group_id,  
	cci.pmuser_id,  
	cci.claim_id,  
	cci.claim_debt_id,  
	cci.claim_debt_version,  
	cci.partial_amount,  
	cci.is_deleted,  
	cci.pfinstalments_id,  
	ccs.action_type_id,  
	ccs.second_pmwrk_task_id,  
	ccs.second_pmuser_group_id,  
	ccs.second_action_type_id,  
	ccs.second_letter_id,  
	ccs.second_oip_letter_id,  
	ccs.percentage_step_one,  
	ccs.percentage_step_two,  
	ccs.oip_document_template_id,  
	'' data_changed,  
	iff.Insurance_folder_cnt,  
	dt.code,  
	dt1.code,  
	ccs.client_document_template_id,  
	p.party_cnt, 
	ccs.pmwrk_task_group_id, 
	ccs.step_description,
	ccs.stop_account,
	ccs.auto_lapse_renewal,  
	ISNULL(cci.is_balance_amount, 0),
	ccs.jump_to_next_step_broker,
	ccs.single_instalment_jump_to_next_step_broker,
	ccs.single_instalment_account_number_of_days,
	ccs.single_instalment_account_tollerance_amount,
	ccs.single_instalment_broker_letter_id
	
FROM Credit_Control_Item cci  

INNER JOIN Credit_Control_Step ccs ON cci.credit_control_step_id = ccs.credit_control_step_id  
INNER JOIN Credit_Control_Rule ccr ON ccs.credit_control_rule_id = ccr.credit_control_rule_id  
INNER JOIN Source s ON ccr.source_id = s.source_id  


LEFT JOIN   (  
            SELECT MIN(cci_2.created_date) as bob,cci_2.claim_debt_id FROM Credit_Control_Item cci_2 GROUP BY cci_2.claim_debt_Id  
            ) MinCCI  
            ON MinCCI.claim_debt_id = cci.claim_debt_id AND ccr.business_type in ('CLF','CLD')  


LEFT JOIN   (  
            SELECT SUM (cci_3.Amount-isnull(cci_3.partial_amount,0)) as TotalOwing,cci_3.claim_debt_id FROM Credit_Control_Item cci_3 WHERE claim_debt_id is not null GROUP BY cci_3.claim_debt_Id  
            ) OwingTot  
            ON OwingTot.claim_debt_id = cci.claim_debt_id AND ccr.business_type in ('CIN','CIA')  

 LEFT JOIN Account acc ON cci.account_id = acc.account_id  
	 LEFT JOIN AccountStatus acs ON acc.accountstatus_id = acs.accountstatus_id  

	 LEFT JOIN Party p ON acc.account_key = p.party_cnt
		LEFT JOIN Party_Other po ON  
			p.party_cnt = po.party_cnt

 LEFT JOIN Insurance_File iff ON cci.insurance_file_cnt = iff.insurance_file_cnt  
 LEFT JOIN Party ag ON ag.party_cnt=iff.lead_agent_cnt  
 LEFT JOIN Party_Agent pag ON pag.party_cnt=ag.party_cnt  
 LEFT JOIN Party_Agent_Type pagt ON pagt.party_agent_type_id=pag.party_agent_type_id  
 LEFT JOIN Business_Type bt ON iff.business_type_id = bt.business_type_id  
 LEFT JOIN Credit_Control_Step ccs2 ON ccs.next_step = ccs2.step_number  
    AND ccs.credit_control_rule_id = ccs2.credit_control_rule_id  
 LEFT JOIN Document_Template dt ON  
     ccs.client_document_template_id = dt.document_template_id  
 LEFT JOIN Document_Template dt1 ON  
     ccs.second_letter_id = dt1.document_template_id  	 
 LEFT JOIN PFPremiumFinance pf ON cci.pfprem_finance_cnt = pf.pfprem_finance_cnt 
 AND cci.pfprem_finance_version = pf.pfprem_finance_version 

 

 WHERE s.code = @branch_code AND ISNULL(cci.is_deleted,0) <> 1  AND ISNULL(PF.StatusInd,0) <> '999'
  AND (ISNULL(cci.is_deleted,0) <> 1 
      OR cci.pfinstalments_id IN (
                          SELECT MIN(pfinstalments_id) FROM PFInstalments
                          GROUP BY pfprem_finance_cnt, pfprem_finance_version, InstalmentNumber
                          HAVING COUNT(InstalmentNumber) > 1 AND MIN(status) = 1)
       )   

 ORDER BY  
    iff.Insurance_folder_cnt, 
    iff.Insurance_file_cnt, 
	cci.created_date,
	cci.will_auto_cancel desc, 
    cci.can_auto_cancel desc, 
    cci.pfprem_finance_cnt, 
    cci.pfprem_finance_version, 
    cci.due_date Desc   
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

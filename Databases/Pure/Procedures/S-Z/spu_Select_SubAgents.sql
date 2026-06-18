SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Select_SubAgents'
GO

CREATE PROCEDURE spu_Select_SubAgents  
    @insurance_file_cnt int  
AS  
  
-- 1 : No Payment  
-- 2 : Fully Paid  
-- 3 : Partially Paid  
SELECT  
    P.party_cnt,  
    P.shortname,  
    P.resolved_name,  
    isnull(SUM(AC.commission_percentage),0) CommissionPC,  
    isnull(SUM(AC.commission_value),0) CommissionValue,  
    (CASE SUM(TD.currency_amount - TD.outstanding_currency_amount) WHEN 0 Then 1 --Amount and Outstanding_Amount are equal and no payment have been made  
    ELSE  
  Case SUM(TD.currency_amount - TD.outstanding_currency_amount) when SUM(TD.currency_amount) Then 2 -- Outsanding_Amount = 0 , Fully Paid  
   ELSE 3 --Partially Paid  
  end  
    END   )   'AllocationStatus'  
FROM Insurance_File_Agent IFA  
LEFT JOIN Document D ON D.insurance_file_cnt = IFA.insurance_file_cnt  
LEFT JOIN Agent_Commission AC ON IFA.insurance_file_cnt=AC.insurance_file_cnt 
AND d.document_ref like 'S%' 
LEFT JOIN Account A ON A.account_key = IFA.party_cnt   
AND ((IFA.party_cnt=AC.party_cnt 
AND AC.is_lead_agent=0)  or AC.party_cnt is null)
INNER JOIN Party P ON P.party_cnt=IFA.party_cnt  
LEFT JOIN TransDetail TD ON TD.document_id = D.document_id  
AND TD.account_id = A.account_id  
WHERE IFA.insurance_file_cnt = @insurance_file_cnt  
AND (d.document_id =(SELECT  
   ISNULL(MIN( d.document_id),0)
  FROM Insurance_File_Agent IFA  
		LEFT JOIN Document D ON D.insurance_file_cnt = IFA.insurance_file_cnt  
		LEFT JOIN Agent_Commission AC ON IFA.insurance_file_cnt=AC.insurance_file_cnt  
		AND d.document_ref like 'S%'  
		JOIN Account A ON A.account_key = IFA.party_cnt  
		AND ((IFA.party_cnt=AC.party_cnt  
		AND AC.is_lead_agent=0)  or AC.party_cnt is null)  
		INNER JOIN Party P ON P.party_cnt=IFA.party_cnt  
		LEFT JOIN TransDetail TD ON TD.document_id = D.document_id  
		AND TD.account_id = A.account_id  
WHERE IFA.insurance_file_cnt = @insurance_file_cnt  )
OR 
0=  (SELECT   ISNULL(MIN( d.document_id),0)
  FROM Insurance_File_Agent IFA  
		LEFT JOIN Document D ON D.insurance_file_cnt = IFA.insurance_file_cnt  
		LEFT JOIN Agent_Commission AC ON IFA.insurance_file_cnt=AC.insurance_file_cnt  
		AND d.document_ref like 'S%'  
		JOIN Account A ON A.account_key = IFA.party_cnt  
		AND ((IFA.party_cnt=AC.party_cnt  
		AND AC.is_lead_agent=0)  or AC.party_cnt is null)  
		INNER JOIN Party P ON P.party_cnt=IFA.party_cnt  
		LEFT JOIN TransDetail TD ON TD.document_id = D.document_id  
		AND TD.account_id = A.account_id  
WHERE IFA.insurance_file_cnt = @insurance_file_cnt  ))
GROUP BY P.party_cnt,  
    P.shortname,  
    P.resolved_name  
ORDER BY P.shortname  
GO


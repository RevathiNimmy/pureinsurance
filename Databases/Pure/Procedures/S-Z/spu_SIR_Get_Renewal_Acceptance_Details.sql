--**********************************
-- Author : Pankaj Kaushik
--   
-- History: 18/06/2008    
--
-- Task : WR9 Batch Renewals
--***********************************

EXECUTE DDLDropProcedure 'spu_SIR_Get_Renewal_Acceptance_Details'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_SIR_Get_Renewal_Acceptance_Details  
     @insurance_file_cnt int
   
AS  
BEGIN

   	SELECT rs.renewal_status_Cnt,
	p.description, 
	rs.insurance_holder_cnt, 
	pa.shortname,
	pt.code,
	i.insurance_ref, 
	rs.renewal_insurance_file_cnt,
	(SELECT insurance_ref FROM insurance_file WHERE insurance_file_cnt = rs.renewal_insurance_file_cnt) ,
	i.insurance_folder_cnt,
	i.insurance_file_structure_id,
	rs.renewal_status_type_id,
	rst.description, 
	rs.critical_date,
	rs.insurance_file_cnt,
	i.cover_start_date,
	i.expiry_date,
	i.lead_agent_cnt,
	p.product_id,
	i.renewal_date,
	CASE IsNull(la.ShortName,'') WHEN '' THEN 'Direct'
	ELSE la.ShortName END LeadAgent,
	CASE IsNull(ah.ShortName,'') WHEN '' THEN 'None'
	ELSE ah.ShortName END AccHandler,
	s.code BranchCode
	,CASE WHEN EXISTS(SELECT * FROM Claim c WHERE i.insurance_ref = c.policy_number) THEN 'YES' Else 'NO' END,
	s.is_deleted Closed,
	pta.is_in_transfer_mode , pta.transfer_to_party_cnt, xferpa.shortname, p.is_true_monthly_policy, i.anniversary_copy,
	rs.renewal_exception_reason_id,rs.renewal_exception_notes,rer.description,
	CASE WHEN EXISTS(SELECT TOP 1 inf.insurance_file_cnt FROM Insurance_File inf INNER JOIN Renewal_Status rs ON inf.insurance_file_cnt = rs.renewal_insurance_file_cnt
	INNER JOIN 	PFPremiumFinance pfp ON rs.insurance_file_cnt = pfp.Insurance_File_Cnt 
	WHERE rs.renewal_insurance_file_cnt = @insurance_file_cnt AND pfp.STATUSIND  IN ('999')) THEN 'INVOICE' ELSE i.payment_method END
	FROM insurance_file i 
	JOIN renewal_status rs       ON rs.renewal_insurance_file_cnt = i.insurance_file_cnt
	JOIN product p               ON p.product_id = rs.product_id
	JOIN renewal_status_type rst ON rst.renewal_status_type_id = rs.renewal_status_type_id
	JOIN party pa                ON pa.party_cnt = rs.insurance_holder_cnt
	JOIN party_type pt           ON pa.party_type_id = pt.party_type_id
	LEFT JOIN party la           ON la.party_cnt = rs.lead_agent_cnt
	LEFT JOIN party ah           ON ah.party_cnt = i.account_handler_cnt
	LEFT JOIN source s           ON s.source_id = i.source_id
	LEFT JOIN Party_Agent pta     ON pta.party_cnt = rs.lead_agent_cnt
	LEFT JOIN Party xferpa       ON xferpa.party_cnt = pta.transfer_to_party_cnt
	LEFT JOIN renewal_exception_reason rer  ON rs.renewal_exception_reason_id = rer.renewal_exception_reason_id
	WHERE (rs.renewal_status_type_id IN
	        (SELECT renewal_status_type_id FROM renewal_status_type WHERE UPPER(code) IN ('UPDATE'))
	      )
	      AND  rs.renewal_insurance_file_cnt = @insurance_file_cnt
         
END  

GO

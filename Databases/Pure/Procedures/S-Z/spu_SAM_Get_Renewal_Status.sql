SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_Renewal_Status'
GO

CREATE PROCEDURE spu_SAM_Get_Renewal_Status      
 @insurance_file_cnt INT  
AS    
  
SELECT  

 renewal_status_Type.code renewal_status_type_code,  
 renewal_status_Type.description  renewal_status_type_description,  
 renewal_status.renewal_status_cnt,  
 renewal_status.insurance_holder_cnt,  
 renewal_status.lead_agent_cnt,  
 renewal_status.date_created,  
 renewal_status.critical_date,  
 renewal_status.is_invite_printed,  
 renewal_status.insurance_file_cnt,  
 renewal_status.date_invite_printed,  
 renewal_status.renewal_exception_notes,       
 renewal_status.email_sent,  
 renewal_status.email_sent_date,  
 product.code product_code,  
 renewal_exception_reason.code renewal_exception_reason_code,  
 renewal_exception_reason.description renewal_exception_reason_description, 
 party_agent.is_in_transfer_mode, 
 agent.shortname agent_shortname, 
 transfer_agent.shortname transfer_to_party_shortname, 
 transfer_to_business_type.code transfer_business_type_code,
 transfer_to_business_type.description transfer_business_type_description,
 Product.is_renewable is_renewable,
 CASE  
 WHEN EXISTS (SELECT 1 FROM insurance_file WHERE renewal_status.insurance_file_cnt = Insurance_File.insurance_file_cnt AND renewal_status.renewal_insurance_file_cnt =Insurance_File.insurance_file_cnt )  
 THEN 1  
 ELSE 0  
 END IsMigratedPolicy

FROM  
      renewal_status WITH(NOLOCK)
INNER JOIN Insurance_File WITH(NOLOCK)ON 
       renewal_status.Insurance_File_cnt = insurance_file.Insurance_File_cnt  
INNER JOIN renewal_status_Type WITH(NOLOCK) ON  
       renewal_status.renewal_status_type_id = renewal_status_Type.renewal_status_type_id 
INNER JOIN product WITH(NOLOCK)ON  
       renewal_status.product_id = Product.product_id  
LEFT OUTER JOIN party_agent WITH(NOLOCK)ON 
	party_agent.party_cnt = renewal_status.lead_agent_cnt
LEFT OUTER JOIN business_type transfer_to_business_type WITH(NOLOCK)ON
	party_agent.transfer_to_business_type_id =transfer_to_business_type.business_type_id
LEFT OUTER JOIN party transfer_agent WITH(NOLOCK)ON 
	party_agent.transfer_to_party_cnt = transfer_agent.party_cnt
LEFT OUTER JOIN party agent WITH(NOLOCK) ON 
	party_agent.party_cnt = agent.party_cnt
LEFT OUTER JOIN renewal_exception_reason WITH(NOLOCK) ON  
       renewal_exception_reason.renewal_exception_reason_id = renewal_status.renewal_exception_reason_id    

WHERE renewal_status.renewal_insurance_file_cnt = @insurance_file_cnt      
  
GO

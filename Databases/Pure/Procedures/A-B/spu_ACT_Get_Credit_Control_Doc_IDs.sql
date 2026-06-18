SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Get_Credit_Control_Doc_IDs'						  
GO


CREATE PROCEDURE spu_ACT_Get_Credit_Control_Doc_IDs  
    @session_id INT  
AS  
  
BEGIN  
  SELECT DISTINCT  
  ccs.client_document_template_id,  
  ccs.oip_document_template_id,  
  acc.account_key,  
  cci.insurance_file_cnt,  
  business_type.code,  
  CASE WHEN ISNULL(pa.Receives_Client_Correspondence,0)=0 THEN ccs.client_document_template_id ELSE ccs.broker_letter_id END,  
  cci.credit_control_item_id,  
  IFL.Insurance_Folder_Cnt,  
  DT.document_type_id,  
  DT1.description AS Client_Document_template_Description,  
  CASE WHEN ISNULL(pa.Receives_Client_Correspondence,0)=0 THEN DT1.description ELSE DT2.description End ,  
  CASE WHEN ISNULL(pa.Receives_Client_Correspondence,0)=0 THEN dt1.email_sub_template_code else dt2.email_sub_template_code end ,  
  CASE WHEN ISNULL(pa.Receives_Client_Correspondence,0)=0 THEN dt1.email_attachment_template_code else dt2.email_attachment_template_code end,  
  c2.code Correspondence_Type,  
  c1.code Default_Preferred_Correspondence,  
  Insurance_File.lead_agent_cnt,  
  Is_Agent_Correspondence  
 FROM TempIDList til  
  
  INNER JOIN Credit_Control_Item cci ON  
    til.link_id = cci.credit_control_item_id  
  
  INNER JOIN Insurance_File ON  
    cci.Insurance_File_Cnt = Insurance_File.Insurance_file_cnt  
  
  INNER JOIN Business_Type ON  
    Insurance_File.Business_type_id = Business_Type.Business_Type_Id  
  
  INNER JOIN Account acc ON  
    cci.account_id = acc.account_id  
  
  INNER JOIN Credit_Control_Step ccs ON  
    cci.credit_control_step_id = ccs.credit_control_step_id  
  
  INNER JOIN Insurance_Folder IFL ON  
 Insurance_File.Insurance_Folder_Cnt=IFL.Insurance_Folder_Cnt  
  
  Inner Join Document_Template DT ON  
 DT.document_template_id=ccs.client_document_template_id or DT.document_template_id=ccs.Broker_Letter_id  
  
  LEFT JOIN Document_Template DT1 ON  
    ccs.client_document_template_id = DT1.document_template_id  
  
  LEFT JOIN Document_Template DT2 ON  
    ccs.Broker_Letter_id = DT2.document_template_id  
  
  left join contact_type c1 on Insurance_File.Default_Preferred_Correspondence = c1.contact_type_id  
  left join Correspondence_Type c2 on Insurance_File.Correspondence_Type = c2.Correspondence_Type_ID  
  left join party_agent as PA on PA.party_cnt = Insurance_File.lead_agent_cnt  
 
 WHERE til.session_id = @session_id  
END
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

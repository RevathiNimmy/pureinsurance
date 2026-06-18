--**********************************************************************************************  
-- Author : Pankaj Kaushik
--   
-- History: 06/02/2008    
--
-- Task : Renewal Back Office Changes 
--**********************************************************************************************  


SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

EXEC DDLDropProcedure 'spu_renewal_customer_email_document_template_sel'
GO


CREATE PROCEDURE spu_renewal_customer_email_document_template_sel  
  @insurance_file_cnt INT,  
  @renewal_type varchar(40)  
AS  
BEGIN  
    IF @renewal_type = 'selection'  
      BEGIN  
  SELECT  
  p.product_id,  
  p.is_renewable,  
  p.is_renewal_selection_enabled,  
  rs.renewal_status_type_id,  
  rs.insurance_file_cnt,  
  p.renewal_selection_man_review_template_id,  
  p.renewal_selection_man_review_attachment_template_id,  
  p.renewal_selection_invite_template_id,  
  p.renewal_selection_invite_attachment_template_id,  
  p.renewal_selection_update_template_id,  
  p.renewal_selection_update_attachment_template_id,  
  DTSel.Document_Type_Id,  
  DTInv.Document_Type_Id,  
  DTUpd.Document_Type_Id,  
  DTSel.Description,  
  DTInv.Description,  
  DTUpd.Description  
  
  FROM  
  renewal_status rs  
  LEFT JOIN product p ON rs.product_id = p.product_id  
  LEFT JOIN document_Template DTSel ON DTSel.Document_Template_Id = p.renewal_selection_man_review_attachment_template_id  
  LEFT JOIN document_Template DTInv ON DTInv.Document_Template_Id = p.renewal_selection_invite_attachment_template_id  
  LEFT JOIN document_Template DTUpd ON DTUpd.Document_Template_Id = p.renewal_selection_update_attachment_template_id  
  WHERE rs.renewal_insurance_file_cnt = @insurance_file_cnt  
  AND p.is_renewable = 1  
         AND p.TradeRNLOnline = 1  
         AND p.is_renewal_selection_enabled = 1  
      END  
    IF @renewal_type = 'invitation'  
    BEGIN  
  SELECT  
  p.product_id,  
  p.is_renewable,  
  p.is_renewal_invite_enabled,  
  rs.renewal_status_type_id,  
  rs.insurance_file_cnt,  
  p.renewal_invite_man_review_template_id,  
  p.renewal_invite_man_review_attachment_template_id,  
  p.renewal_invite_invite_template_id,  
  p.renewal_invite_invite_attachment_template_id,  
  p.renewal_invite_update_template_id,  
  p.renewal_invite_update_attachment_template_id,  
  DTSel.Document_Type_Id,  
  DTInv.Document_Type_Id,  
  DTUpd.Document_Type_Id,  
  DTSel.Description,  
  DTInv.Description,  
  DTUpd.Description    
  FROM  
  renewal_status rs  
  LEFT JOIN product p ON rs.product_id = p.product_id  
  LEFT JOIN document_Template DTSel ON DTSel.Document_Template_Id = p.renewal_invite_man_review_attachment_template_id  
  LEFT JOIN document_Template DTInv ON DTInv.Document_Template_Id = p.renewal_invite_invite_attachment_template_id  
  LEFT JOIN document_Template DTUpd ON DTUpd.Document_Template_Id = p.renewal_invite_update_attachment_template_id  
  WHERE rs.renewal_insurance_file_cnt = @insurance_file_cnt  
  AND p.is_renewable = 1  
  AND p.TradeRNLOnline = 1  
  AND p.is_renewal_invite_enabled = 1  
    END  
  
    IF @renewal_type = 'acceptance'  
    BEGIN  
  IF EXISTS(SELECT renewal_insurance_file_cnt  FROM renewal_status  
          WHERE renewal_status.renewal_insurance_file_cnt = @insurance_file_cnt)  
  BEGIN  
  
   SELECT  
   p.product_id,  
   p.is_renewable,  
   p.is_renewal_update_enabled,  
   rs.renewal_status_type_id,  
   rs.insurance_file_cnt,  
   p.renewal_update_man_review_template_id,  
   p.renewal_update_man_review_attachment_template_id,  
   p.renewal_update_invite_template_id,  
   p.renewal_update_invite_attachment_template_id,  
   p.renewal_update_update_template_id,  
   p.renewal_update_update_attachment_template_id,  
   DTSel.Document_Type_Id,  
   DTInv.Document_Type_Id,  
   DTUpd.Document_Type_Id,  
   DTSel.Description,  
   DTInv.Description,  
   DTUpd.Description    
   FROM  
   renewal_status rs  
   LEFT JOIN product p ON rs.product_id = p.product_id  
   LEFT JOIN document_Template DTSel ON DTSel.Document_Template_Id = p.renewal_update_man_review_attachment_template_id  
   LEFT JOIN document_Template DTInv ON DTInv.Document_Template_Id = p.renewal_update_invite_attachment_template_id  
   LEFT JOIN document_Template DTUpd ON DTUpd.Document_Template_Id = p.renewal_update_update_attachment_template_id  
  
   WHERE rs.renewal_insurance_file_cnt = @insurance_file_cnt  
   AND p.is_renewable = 1  
          AND p.TradeRNLOnline = 1  
          AND p.is_renewal_update_enabled = 1  
 END  
 ELSE  
 BEGIN  
   SELECT  
   p.product_id,  
   p.is_renewable,  
   p.is_renewal_update_enabled,  
   5,    --THIS IS FOR RENEWAL UPDATE  
   ifi.insurance_file_cnt,  
   p.renewal_update_man_review_template_id,  
   p.renewal_update_man_review_attachment_template_id,  
   p.renewal_update_invite_template_id,  
   p.renewal_update_invite_attachment_template_id,  
   p.renewal_update_update_template_id,  
   p.renewal_update_update_attachment_template_id,  
   DTSel.Document_Type_Id,  
   DTInv.Document_Type_Id,  
   DTUpd.Document_Type_Id,  
   DTSel.Description,  
   DTInv.Description,  
   DTUpd.Description      
   FROM  
   Insurance_File ifi  
   LEFT JOIN product p ON ifi.product_id = p.product_id  
   LEFT JOIN document_Template DTSel ON DTSel.Document_Template_Id = p.renewal_update_man_review_attachment_template_id  
   LEFT JOIN document_Template DTInv ON DTInv.Document_Template_Id = p.renewal_update_invite_attachment_template_id  
   LEFT JOIN document_Template DTUpd ON DTUpd.Document_Template_Id = p.renewal_update_update_attachment_template_id  
   WHERE ifi.insurance_file_cnt = @insurance_file_cnt  
   AND p.is_renewable = 1  
          AND p.TradeRNLOnline = 1  
          AND p.is_renewal_update_enabled = 1  
  
 END  
    END  
END  

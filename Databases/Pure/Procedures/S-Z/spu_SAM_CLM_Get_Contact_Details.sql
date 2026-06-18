DDLDROPPROCEDURE 'spu_SAM_CLM_Get_Contact_Details'
GO
    
CREATE PROCEDURE spu_SAM_CLM_Get_Contact_Details    
    
 @claim_id integer    
   
AS    
  
BEGIN    
 SELECT    
  Client_tel_no,    
  Client_fax_no,    
  Client_mobile_no,    
  Client_email,    
  Client_claim_number,    
  Insurer_name,    
  insurer_tel_no,    
  insurer_fax_no,    
  insurer_email,    
  insurer_claim_number,    
  Insurer_Contact,    
  Client_tel_no_off    
 FROM Claim    
 WHERE claim_id = @claim_id    
END    
GO
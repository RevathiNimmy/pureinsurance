SET QUOTED_IDENTIFIER OFF    
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_get_contacts_for_party'
GO

CREATE PROCEDURE spu_get_contacts_for_party
    @party_cnt INT,
    @insurance_file_cnt INT=0,    
    @TransactionMode INT=0     
AS

DECLARE @IsInTransferMode int,      
   @TransferToPartyCnt int     
    
 IF @insurance_file_cnt<>0 and @TransactionMode=1    
  BEGIN    
   SELECT @party_cnt=pa.party_cnt,      
     @IsInTransferMode=pa.is_in_transfer_mode,      
     @TransferToPartyCnt=pa.transfer_to_party_cnt      
   FROM Party_Agent pa JOIN Insurance_File ifi ON pa.party_cnt = ifi.lead_agent_cnt      
   WHERE  ifi.insurance_file_cnt = @insurance_file_cnt      
        
   SELECT  @party_cnt = IsNull(@party_cnt,0),      
     @IsInTransferMode = IsNull(@IsInTransferMode,0),      
     @TransferToPartyCnt = IsNull(@TransferToPartyCnt,0)      
        
   IF @TransactionMode = 1 -- Open claim      
   BEGIN      
    IF @party_cnt <> 0 AND @IsInTransferMode <> 0 AND @TransferToPartyCnt<> 0      
     SELECT @party_cnt = @TransferToPartyCnt      
   END      
 END     

SELECT contact.*, 
       contact_type.code AS 'contacttypecode',contact_type.description As 'ContactTypeDescription'
  FROM Party_Contact_Usage 
  JOIN contact ON party_contact_usage.contact_cnt = contact.contact_cnt 
  JOIN contact_type ON contact.contact_type_id = contact_type.contact_type_id
 WHERE (party_cnt = @party_cnt)
GO



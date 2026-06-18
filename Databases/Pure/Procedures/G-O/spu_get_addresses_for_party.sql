SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_get_addresses_for_party
GO

CREATE PROCEDURE spu_get_addresses_for_party
    @party_cnt INT,
    @insurance_file_cnt INT=0,    
    @TransactionMode INT=0     
AS  
BEGIN
 DECLARE @nIsInTransferMode INT,      
         @nTransferToPartyCnt INT     
    
 IF @insurance_file_cnt<>0 AND @TransactionMode=1    
  BEGIN    
   SELECT @party_cnt=pa.party_cnt,      
     @nIsInTransferMode=pa.is_in_transfer_mode,      
     @nTransferToPartyCnt=pa.transfer_to_party_cnt      
   FROM Party_Agent pa JOIN Insurance_File ifi ON pa.party_cnt = ifi.lead_agent_cnt      
   WHERE  ifi.insurance_file_cnt = @insurance_file_cnt      
        
   SELECT  @party_cnt = ISNULL(@party_cnt,0),      
     @nIsInTransferMode = ISNULL(@nIsInTransferMode,0),      
     @nTransferToPartyCnt = ISNULL(@nTransferToPartyCnt,0)      
        
   IF @TransactionMode = 1 -- Open claim      
   BEGIN      
    IF @party_cnt <> 0 AND @nIsInTransferMode <> 0 AND @nTransferToPartyCnt<> 0      
     SELECT @party_cnt = @nTransferToPartyCnt      
   END      
 END 	
  
    SELECT  a.postal_code,
            pau.address_usage_type_id,  
            a.address1,  
            a.address2,  
            a.address3,  
            a.address4,  
            pau.address_cnt,  
            c.description,
			a.address5,
			a.address6,
			a.address7,
			a.address8,
			a.address9,
			a.address10,  
            c.country_id 
    FROM    address a
        INNER JOIN party_address_usage pau
            ON pau.address_cnt = a.address_cnt
        LEFT OUTER JOIN country c  
            ON a.country_id = c.country_id  
    WHERE   pau.party_cnt = @party_cnt  
        AND CONVERT(VARCHAR(10), a.address_id) <> a.postal_code           
    UNION  
    SELECT  postal_code = '',  
            pau.address_usage_type_id,  
            a.address1,  
            a.address2,  
            a.address3,  
            a.address4,  
            pau.address_cnt,  
            c.description ,
			a.address5,
			a.address6,
			a.address7,
			a.address8,
			a.address9,
			a.address10,  
            c.country_id  
    FROM    
        party_address_usage pau
        INNER JOIN address a
            ON pau.address_cnt = a.address_cnt
        LEFT OUTER JOIN country c  
            ON a.country_id = c.country_id  
    WHERE   pau.party_cnt = @party_cnt  
        AND CONVERT(VARCHAR(10), a.address_id) = a.postal_code  
 
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

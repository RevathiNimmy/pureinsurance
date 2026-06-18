SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_claim_party_details'
GO

CREATE PROCEDURE spu_get_claim_party_details  
    @perilid int,  
    @claimid int  
AS  

BEGIN
    SELECT 
	a.Party_Claim_id,
	a.Claim_Party_type_id,  
     	a.Name, 
	a.Address1, 
	a.Address2, 
	a.Address3, 
	a.Address4, 
	a.PostCode,  
	a.License_type ,
	License_type.description,  
	a.License_Number, 
	a.Date_of_Birth, 
	a.Sex,  
	a.party_status, 
	driver_status.description,  
	a.Phone_Number, 
	a.Fax_Number,  
	a.Reference_Number, 
	a.Reg_Number  
    FROM party_claim a 
		LEFT OUTER JOIN License_type  ON
	        	a.License_type = License_type.License_type_id,  

        party_claim b 
		LEFT OUTER JOIN Driver_Status  ON
	        	b.Party_Status = Driver_Status.driver_status_id,Peril_Party c  
    WHERE a.party_claim_id = b.party_claim_id 
    AND c.party_claim_id = a.party_claim_id 
    AND c.claim_peril_id = @perilid 
    AND c.claim_id = @claimid  
END



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

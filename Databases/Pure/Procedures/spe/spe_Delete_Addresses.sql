SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Delete_Addresses'
GO
CREATE PROCEDURE spe_Delete_Addresses
    @is_deleted INT,
	@party_cnt INT,
	@address_cnt INT
	  
AS  
BEGIN  
 
UPDATE Party_Address_Usage  
    SET is_Deleted = 1
	WHERE address_cnt = @address_cnt AND party_cnt = @party_cnt
 
END  
GO

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Address_del'
GO

CREATE PROCEDURE spe_Address_del
    @address_cnt int
AS

DELETE FROM Party_Address_Usage    
WHERE address_cnt = @address_cnt  
  
DELETE FROM Contact_Address_Usage    
WHERE address_cnt = @address_cnt

DELETE FROM Address
WHERE address_cnt = @address_cnt

GO


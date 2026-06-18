SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_BankGuarantee_PLDProduct'
GO
  
CREATE   PROCEDURE spu_BankGuarantee_PLDProduct   
	@BG_Id  INT  
AS    
  
		DELETE FROM   
		    Bg_Product_Link WHERE Bg_Id =@BG_Id   
  
  GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO 